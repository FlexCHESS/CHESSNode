using Azure.Core;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using static IO.Swagger.Controllers.CHESSNetworkController;
using Newtonsoft.Json;
using System.Text;

namespace IO.Swagger
{
    public class UUDEXWebClient : WebClient
    {


        private readonly X509Certificate2 certificate;

        public UUDEXWebClient(X509Certificate2 cert)
        {
            certificate = cert;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);

            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate X509certificate, X509Chain chain, System.Net.Security.SslPolicyErrors errors)
            {
                return true;
            };

            request.ClientCertificates.Add(certificate);
            return request;
        }

        public String get(String url)
        {
            WebRequest request = GetWebRequest(new Uri("https://preprodapim.umbrellaiot.com:3546/v1/uudex/" + url));

            request.Timeout = 12000;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public String post(String url, String json)
        {
            WebRequest request = GetWebRequest(new Uri("https://preprodapim.umbrellaiot.com:3546/v1/uudex/" + url));

            request.Timeout = 12000;

            var data = Encoding.ASCII.GetBytes(json);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.PreAuthenticate = true;
            //request.Headers.Add("Authorization", token);

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;


        }

    }

}
