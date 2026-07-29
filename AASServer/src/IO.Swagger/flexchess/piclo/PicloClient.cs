/*
*   FlexCHESS - Piclo Flex marketplace integration - HTTP client
*   tim@toshiba-bril.com
*/
using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace IO.Swagger.Piclo
{
    // Thin REST client for the Piclo Flex marketplace API (https://docs.picloflex.com).
    // Handles Bearer JWT acquisition/renewal (client_id/api_key -> token, per the
    // /authtoken/v1/ operation) and the asset, planned asset and bid submission
    // operations used by PicloMarketController.
    public class PicloClient
    {
        private readonly String baseUrl;
        private readonly String clientId;
        private readonly String apiKey;

        private String token;
        private DateTime tokenExpiry = DateTime.MinValue;

        // Piclo requests all URLs end with a trailing slash - see the docs' getting
        // started warning - so every path below is written with one.
        private const String AuthPath = "/authtoken/v1/";
        private const String AssetsPath = "/assets/v1/";
        private const String PlannedAssetsPath = "/planned-assets/v1/";

        private static readonly JsonSerializerSettings OutSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public PicloClient(String baseUrl, String clientId, String apiKey)
        {
            this.baseUrl = String.IsNullOrEmpty(baseUrl) ? "https://api.picloflex.com" : baseUrl.TrimEnd('/');
            this.clientId = clientId;
            this.apiKey = apiKey;
        }

        // Acquire (and cache) a Bearer JWT, refreshing it a minute ahead of its "exp"
        // claim (falling back to a conservative 55 minute cache if that claim can't be
        // read, e.g. a non-standard token format).
        private String GetToken()
        {
            if (token != null && DateTime.UtcNow < tokenExpiry)
                return token;

            PicloLogin login = new PicloLogin { ClientId = clientId, ApiKey = apiKey };
            String response = Send("POST", AuthPath, JsonConvert.SerializeObject(login), null);
            PicloToken picloToken = JsonConvert.DeserializeObject<PicloToken>(response);

            token = picloToken.Token;
            tokenExpiry = DateTime.UtcNow.AddMinutes(55);
            try
            {
                String payload = token.Split('.')[1];
                payload = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                dynamic claims = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(Convert.FromBase64String(payload)));
                if (claims.exp != null)
                    tokenExpiry = DateTimeOffset.FromUnixTimeSeconds((Int64)claims.exp).UtcDateTime.AddMinutes(-1);
            }
            catch (Exception) { /* keep the default 55 minute cache */ }

            return token;
        }

        private String Send(String method, String path, String body, String bearer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + path);
            request.Method = method;
            request.Accept = "application/json";
            request.Timeout = 30000;
            if (bearer != null)
                request.Headers.Add("Authorization", "Bearer " + bearer);

            if (body != null)
            {
                request.ContentType = "application/json";
                var data = Encoding.UTF8.GetBytes(body);
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                    stream.Write(data, 0, data.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                return reader.ReadToEnd();
        }

        // Authenticated request, retrying once with a freshly-acquired token on a 401
        // (covers a token being revoked/expired early server-side).
        private String Authenticated(String method, String path, String body)
        {
            try
            {
                return Send(method, path, body, GetToken());
            }
            catch (WebException ex) when ((ex.Response as HttpWebResponse)?.StatusCode == HttpStatusCode.Unauthorized)
            {
                token = null;
                return Send(method, path, body, GetToken());
            }
        }

        // Look up existing Flex Assets by their Flex-Provider-defined ref, used to decide
        // whether a sync should create or update an asset.
        public PicloAsset[] GetAssetByRef(String reference)
        {
            String json = Authenticated("GET", AssetsPath + "?ref=" + Uri.EscapeDataString(reference), null);
            return JsonConvert.DeserializeObject<PicloAsset[]>(json) ?? Array.Empty<PicloAsset>();
        }

        public PicloAsset CreateAsset(PicloAsset asset)
        {
            String json = Authenticated("POST", AssetsPath, JsonConvert.SerializeObject(asset, OutSettings));
            return JsonConvert.DeserializeObject<PicloAsset>(json);
        }

        public PicloAsset UpdateAsset(String assetId, PicloAsset asset)
        {
            String json = Authenticated("PATCH", AssetsPath + assetId + "/", JsonConvert.SerializeObject(asset, OutSettings));
            return JsonConvert.DeserializeObject<PicloAsset>(json);
        }

        public PicloPlannedAsset CreatePlannedAsset(PicloPlannedAsset asset)
        {
            String json = Authenticated("POST", PlannedAssetsPath, JsonConvert.SerializeObject(asset, OutSettings));
            return JsonConvert.DeserializeObject<PicloPlannedAsset>(json);
        }

        public PicloBallotResponse SubmitBallot(String competitionId, PicloBallotRequest ballot)
        {
            String path = "/bids/v1/competitions/" + Uri.EscapeDataString(competitionId) + "/ballots/";
            String json = Authenticated("POST", path, JsonConvert.SerializeObject(ballot, OutSettings));
            return JsonConvert.DeserializeObject<PicloBallotResponse>(json);
        }
    }
}
