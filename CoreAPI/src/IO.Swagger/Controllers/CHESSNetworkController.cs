/*
*   FlexCHESS - CHESS node - CHESS network core API 
*   This is the CHESS Core Network API Controller
*   tim@toshiba-bril.com
*/

using IO.Swagger.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System;
using Newtonsoft.Json;
using Azure;
using System.Collections.Generic;
using System.Linq;
using IoT.Services;
using IO.Swagger.Models;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;

namespace IO.Swagger.Controllers
{

    // CHESS registration object

    public class CHESS
    {
        public IoT.Services.ChessAdapter adapter { get; set; }
        public IoT.Services.Chess[] chess { get; set; }
    }

    // CHESS status object

    public class CHESSStatus
    {
        public String identifier { get; set; }
        public String location { get; set; }
        public String id { get; set; }
        public String currentStatus { get; set; }
        public IoT.Services.ChessStatus[] status { get; set; }
    }

    // CHESS telemetry topic object

    public class Topic
    {
        public String id { get; set; }
        public String topic { get; set; }
        public String error { get; set; }
    }

    [ApiController]
    public class CHESSNetworkController : Controller
    {

        public class Token
        {
            public String sub { get; set; }
            public String aut { get; set; }
            public String aud { get; set; }
            public String nbf { get; set; }
            public String azp { get; set; }
            public String scope { get; set; }
            public String iss { get; set; }
            public String exp { get; set; }
            public String iat { get; set; }
            public String jti { get; set; }
            public String location {get; set; }
            public String uri {get; set;}

            public Token() { }
        }
        //Function to decode JWT token
        private Token decodeToken(String token)
        {
            String str = token.Substring(token.IndexOf(".") + 1, token.LastIndexOf(".") - token.IndexOf(".") - 1);
            if (str.Length % 4 != 0)
                str += new String('=', 4 - str.Length % 4);

            byte[] data = Convert.FromBase64String(str);
            string decodedString = System.Text.Encoding.UTF8.GetString(data);
            Token jwt = JsonConvert.DeserializeObject<Token>(decodedString);


            return jwt;
        }

        
        // Get -  remote HTTP request
        private string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentType = "application/json";
            request.Timeout = 120000;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        // Post -  remote HTTP request
        private string Post(string uri, string json, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 120000;

            var data = Encoding.ASCII.GetBytes(json);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.PreAuthenticate = true;
            if (token != null)
                request.Headers.Add("Authorization", token);
            request.Accept = "application/json";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Remove a registered CHESS from a CHESS node</remarks>
        /// <param name="uri"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpDelete]
        [Route("/register")]
        [ValidateModelState]
        [SwaggerOperation("RegisterCHESSDelete")]
        [SwaggerResponse(statusCode: 200, type: typeof(IoT.Services.ChessAdapter), description: "CHESS removed successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]

        public virtual IActionResult RegisterCHESSDelete([FromQuery][Required()] string uri, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            /// TODO - here we  deregister a registered CHESS !

            return StatusCode(404);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Register CHESS with a CHESS node and update the digital twin</remarks>
        /// <param name="body"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpPost]
        [Route("/register")]
        [Consumes("application/json")]
        [Produces("text/plain")]
        [ValidateModelState]
        [SwaggerOperation("RegisterCHESSPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(IoT.Services.Chess), description: "CHESS created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]


        public virtual IActionResult RegisterCHESSPost([Required][FromBody][SwaggerRequestBody("application/json")] CHESS body, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            // Need to use token to authorize the access
            Token token = decodeToken(Authorization);

            if (!token.scope.Contains("saFlexibilityProvider"))
                return StatusCode(403);


            // We need to register with the platform first!
            
            String response = Post("http://aasserver/register?chessnode="+Program.CHESSNodeKey, Newtonsoft.Json.JsonConvert.SerializeObject(body), Authorization).Trim('"').Replace("\\","");

            Console.WriteLine("Registering CHESS adapter " + body.adapter.identifier + " - Flex platform response - " + response);


            Console.WriteLine("Starting Adapter - " + body.adapter.identifier);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "bash";
            String[] mounts = body.adapter.volumeMount.Split(" ");
            if (mounts.Length == 3)
                startInfo.Arguments = "deploy.sh " + body.adapter.identifier + " " + body.adapter.container.Replace("/", "\\/").Replace(".", "\\.") + " " + body.adapter.credentials + " " + body.adapter.envconf + " " + body.adapter.exposedPort + " " + mounts[0].Replace("/", "\\/") + " " + mounts[1].Replace("/", "\\/") + " " + mounts[2].Replace("/", "\\/") + " " + Program._adtServiceUrl + " " + Program._adtClientId + " " + Program._adtClientSecret + " " + Program._adtTenantId;
            else
                startInfo.Arguments = "deploy.sh " + body.adapter.identifier + " " + body.adapter.container.Replace("/", "\\/").Replace(".", "\\.") + " " + body.adapter.credentials + " " + body.adapter.envconf + " " + body.adapter.exposedPort + " tmp \\/tmp \\/tmp" + " " + Program._adtServiceUrl + " " + Program._adtClientId + " " + Program._adtClientSecret + " " + Program._adtTenantId;
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process);



            String result = "[";
            foreach (IoT.Services.Chess chess in body.chess)
            {

                // Register the CHESS by calling its adapter core API
  
                try
                {
                    String adapterStatus = Get("http://" + body.adapter.identifier + "/status/" + chess.Id);
                    Console.WriteLine("Adapter status - " + adapterStatus);

                } catch (Exception ex) 
                {
                    Console.WriteLine("Adapter status - " + ex.ToString());
                }
                try 
                {
  
                    // Now subscribe to the events for this CHESS by creating a subject and subscribing
                    String subject = Program.createSubject(chess.Id);


                    SubscribeSubject ss = Program.subscribe(0, subject);

                    Topic[] topics = JsonConvert.DeserializeObject<Topic[]>(response);


                    foreach (Topic topic in topics)
                    {
                        if (topic.id.Equals(chess.Id))
                        {
                

                            Console.WriteLine("Created CHESS and subscribe to subject - " + subject + " " + ss.subscription_uuid);

                            String json = "{\"identifier\":\"" + chess.identifier + "\", \"location\":\"" + chess.location + "\",\"standard\":\"" + chess.standard + "\",\"version\":\"" + chess.version + "\",\"id\":\"" + chess.Id + "\",\"topic\":\"q_CHESSNode_2_"+ chess.Id + "telemetry_"+ topic.topic + "\",\"currentStatus\":\"init\"}";

                            Console.WriteLine("Initialising CHESS  - " + json);


                            result += ("{\"request\":"+json+", \"response\": {" + Post("http://" + body.adapter.identifier + "/init", json, Authorization) + "}},");
                        }
                    }
                    Console.WriteLine("Result " + result);
  
                } catch (Exception e)
                {
                   
                    Console.WriteLine(e);
                    return StatusCode(500);
                }

            }


            return Json(JsonConvert.DeserializeObject(result.Trim(',') + "]"));
        }

    }

}