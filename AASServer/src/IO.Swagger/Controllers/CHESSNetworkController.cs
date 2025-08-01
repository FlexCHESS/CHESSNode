/*
*   FlexCHESS - CHESS node - CHESS network core API 
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
using Azure.DigitalTwins.Core;
using Microsoft.Azure.Devices.Shared;
using System.Collections.Generic;
using System.Linq;
using IoT.Services;
using IO.Swagger.Models;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Globalization;
using MQTTnet.Client;
using MQTTnet.Extensions.TopicTemplate;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace IO.Swagger.Controllers
{

      

    // The main CHESS data structure
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


    [ApiController]
    public class CHESSNetworkController : Controller
    {

        // CHESS registration object
        protected static String authToken = "";
        // DT Data
        public class DTData
        {

            public String Id {get; set;}
            public String DataType {get; set;}
            public String LevelType {get; set;}
            public String ValueFormat {get; set;}
            public String Symbol {get; set;}
            public String Unit {get; set;}
            public Double Value {get; set;}

        }



        protected Double dtLookup(DTData[] dtData, String id)
        {

            foreach (DTData data in dtData)
            if (data.Id.EndsWith(id)) return data.Value;

            return 0;
        }

        public static async void Handle_Received_Message(String server, String subject_name, String chessId)
        {

            /*
             * Subscribes to a topic and processes the received message.
             */

            var mqttFactory = new MQTTnet.MqttFactory();

            Console.WriteLine("MQTT - " + server + " " + subject_name + " " + chessId);

            using (var mqttClient = mqttFactory.CreateMqttClient())
            {
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("uudex.default.svc", 1883).WithCredentials(Program.uudexUser, Program.uudexPass).Build();

                String query = "SELECT DT.$dtId FROM DIGITALTWINS twin JOIN DT RELATED twin.submodelElement WHERE twin.$dtId = '" + subject_name + "'";

                Pageable<BasicDigitalTwin> smeResponse = Program.dtClient.Query<BasicDigitalTwin>(query);

                // Setup message handling before connecting so that queued messages
                // are also handled properly. When there is no event handler attached all
                // received messages get lost.
                mqttClient.ApplicationMessageReceivedAsync += e =>
                {


                    String message = System.Text.Encoding.Default.GetString(e.ApplicationMessage.Payload);

                    String topic = e.ApplicationMessage.Topic.Replace("/", "-");

                    Console.WriteLine("Got message " + topic + " " + message);

                    var telemetry = JObject.Parse(message);


                    if (message != null)
                    {

                        foreach (BasicDigitalTwin smeTwin in smeResponse)
                            if (topic.Contains(smeTwin.Id.Substring(chessId.Length + 4)))
                            {
                                Response<IoT.Services.DataSpecificationIEC61360> dataResponse = Program.dtClient.GetDigitalTwin<IoT.Services.DataSpecificationIEC61360>(smeTwin.Id.Substring(4));

                                if (dataResponse != null)
                                {



                                    var value = telemetry["value"];

                                    Console.WriteLine("Value " + value);

                                    if (!value.Contains(dataResponse.Value.Value))
                                    {

                                        JsonPatchDocument patch = new JsonPatchDocument();

                                        if (dataResponse.Value.Value != null)
                                            patch.AppendReplace("/value", (string)value);
                                        else
                                            patch.AppendAdd("/value", (string)value);

                                        Console.WriteLine("Updating " + dataResponse.Value.Id);

                                        Response response = Program.dtClient.UpdateDigitalTwin(dataResponse.Value.Id, patch);
                                    }

                                }


                            }


                    }

                    return Task.CompletedTask;
                };

                var connectResult = mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);




                if (connectResult.Result.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine("Connected to MQTT broker successfully.");

                    await mqttClient.SubscribeAsync("N/#");

                    Console.WriteLine("MQTT client subscribed to topic.");

                    while (true)
                        Thread.Sleep(1000);
                }


            }

        }
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
            public String location { get; set; }
            public String uri { get; set; }

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

        //Get the subject attributes for a twin from the ADT model
        private String getSubjectAttribute(IoT.Services.Chess twin)
        {
            String query = "SELECT DT.$dtId FROM DIGITALTWINS twin JOIN DT RELATED twin.targetSubjectAttributes WHERE twin.$dtId = '" + twin.Id + "AccessControlPermissions'";

            // get the permissions from subject attriubtes for the submodel
            Pageable<BasicDigitalTwin> attributeResponse = Program.dtClient.Query<BasicDigitalTwin>(query);
            String subjectAttribute = "";
            if (attributeResponse != null)
            {

                foreach (BasicDigitalTwin attribute in attributeResponse)
                {
                    subjectAttribute += " " + attribute.Id;
                }


            }
            else subjectAttribute = "default";
            return subjectAttribute;
        }

        // get the currect status of a chess
        private Boolean getStatus(ChessStatus status)
        {
            DateTime starttime;
            DateTime endtime;

            if (status.recurrence == null) return false;
            
            if (status.recurrence.ToLower().Equals("daily") ||
                (status.recurrence.ToLower().Equals("weekdays") && ((DateTime.Today.DayOfWeek != DayOfWeek.Saturday) && (DateTime.Today.DayOfWeek != DayOfWeek.Sunday))) ||
                (status.recurrence.ToLower().Equals("weekends") && ((DateTime.Today.DayOfWeek == DayOfWeek.Saturday) || (DateTime.Today.DayOfWeek == DayOfWeek.Sunday))))
            {

                starttime = DateTime.ParseExact(status.starttime + ":00", "HH:mm:ss",
                                        CultureInfo.InvariantCulture);
                endtime = DateTime.ParseExact(status.endtime + ":00", "HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                if (starttime <= DateTime.Now && endtime > DateTime.Now)
                    return true;


            }

            return false;
        }

        // Get -  remote HTTP request
        private string Get(string uri, String token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (token != null)
                request.Headers.Add("Authorization", token);

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
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 120000;
            if (token != null)
                request.Headers.Add("Authorization", token);

            var data = Encoding.ASCII.GetBytes(json);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        /// <summary>
        /// Remove a registered CHESS / CHESS adapter
        /// </summary>
        /// <remarks>Remove a registered CHESS from a CHESS node</remarks>
        /// <param name="uri"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpDelete]
        [Route("/register")]
        [Route("/chess/1.0/register")]
        [Route("/coreapi/1.0/register")]
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

        public virtual IActionResult RegisterCHESSDelete([FromQuery][Required] String uri, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            /// TODO - here we  deregister a registered CHESS !

            return StatusCode(404);

        }

        /// <summary>
        /// Register CHESS assets using a CHESS adapter with the digital twin environment
        /// </summary>
        /// <remarks>Register CHESS with a CHESS node and update the digital twin</remarks>
        /// <param name="body"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpPost]
        [Route("/register")]
        [Route("/chess/1.0/register")]
        [Route("/coreapi/1.0/register")]
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


        public virtual IActionResult RegisterCHESSPost([Required][FromBody][SwaggerRequestBody("application/json")] CHESS body, [FromHeader] String Authorization, [FromQuery] String chessnode)
        {

            if (Authorization == null)
                return StatusCode(401);

            // Need to use token to authorize the access
            Token token = decodeToken(Authorization);

            if (!token.scope.Contains("saFlexibilityProvider"))
                return StatusCode(403);

            // check if it already exists !

            String query = "SELECT $dtId FROM  DigitalTwins  where $metadata.$model = 'dtmi:com:flexchess:chessadapter;1' and $dtId = '" + body.adapter.identifier + "'";
            try
            {

                Pageable<IoT.Services.ChessAdapter> twinResponse = Program.dtClient.Query<IoT.Services.ChessAdapter>(query);
                Boolean registered = false;
                if (twinResponse != null)
                {
                    // Get the digital twin !

                    foreach (IoT.Services.ChessAdapter twin in twinResponse)
                    {

                        // Create a patch document and update DT
                        if (twin.Id.Equals(body.adapter.Id))
                        {
                            Console.WriteLine("Already registered - " + twin.Id);
                            registered = true;
                        }
                    }
                }
                if (!registered)
                    Program.dtClient.CreateOrReplaceDigitalTwin(body.adapter.identifier, body.adapter);

            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }



            try
            {
                String result = "[";


                // look for each CHESS to see if is already exists
                Boolean found = false;
                foreach (IoT.Services.Chess chess in body.chess)
                {

                    // Get the existing CHESS adapter DT 
                    query = "SELECT twin.$dtId FROM DIGITALTWINS twin JOIN DT RELATED twin.contains WHERE DT.$dtId = '" + chess.Id + "'";
                    Pageable<IoT.Services.ChessAdapter> twinResponse = Program.dtClient.Query<IoT.Services.ChessAdapter>(query);
                    if (twinResponse == null || twinResponse.Count() == 0)
                    {


                        try
                        {
                            // No DT found for this CHESS 
                           
                            IoT.Services.Chess newtwin = Program.dtClient.CreateOrReplaceDigitalTwin(chess.Id, chess);

                            AccessPermissionRule accessPermissionRule = new AccessPermissionRule();
                            accessPermissionRule.Id = chess.Id + "AccessControlPermissions";

                            Program.dtClient.CreateOrReplaceDigitalTwin(chess.Id+"AccessControlPermissions", accessPermissionRule);

                            var relationship = new BasicRelationship
                            {
                                TargetId = newtwin.Id,
                                Name = "contains"
                            };
                            string relId = $"{body.adapter.identifier}-contains->{newtwin.Id}";
                            Program.dtClient.CreateOrReplaceRelationship(body.adapter.identifier, relId, relationship);
                            Console.WriteLine("Created contains relationship successfully");



                            AccessPermissionRuleTargetSubjectAttributesRelationship rel = new AccessPermissionRuleTargetSubjectAttributesRelationship
                            {
                                TargetId = "saFlexibilityProvider" + chessnode,
                                Name = "targetSubjectAttributes"
                            };
                            relId = $"{newtwin.Id}AccessControlPermissions->saFlexibilityProvider{chessnode}";
                            Program.dtClient.CreateOrReplaceRelationship(newtwin.Id + "AccessControlPermissions", relId, rel);
                            Console.WriteLine("Created permission relationship successfully");

                        }
                        catch (RequestFailedException e)
                        {
                            Console.WriteLine($"Create DT error: {e.Status}: {e.Message}");
                            result += "{\"Error\":\"Not able to create the DT relationship " + chess.Id + "\"},";
                        }

                    }
                    else
                    {
                        Console.WriteLine("CHESS is already registered - " + chess.Id);

                    }

                    try
                    {
                        // Now create the telemetry subject and subscribe to it
                        Boolean subscribed = false;
                        foreach (Subject subject in Program.uudexSubjects)
                        {

                            Console.WriteLine("Looking for subject " + subject.subject_name + " matching " + chess.Id + "telemetry");

                            if (subject.subject_name.Equals(chess.Id + "telemetry"))
                            {
                                Console.WriteLine("Found " + subject.subject_name);

                                result += "{\"id\":\"" + chess.Id + "\", \"topic\":\"" + subject.subscription.subscription_uuid + "\"},";
                                subscribed = true;

                                if (chess.standard.Contains("MQTT"))
                                    Task.Run(() => Handle_Received_Message(chess.identifier, subject.subscription.subject_name, chess.Id));
                                break;
                            }

                        }
                        if (!subscribed)
                        {
                            String sub = Program.createSubject(chess.Id);
                            Console.WriteLine("Subject " + sub);
                            SubscribeSubject ss = Program.subscribe(0, sub);
                            result += "{\"id\":\"" + chess.Id + "\", \"topic\":\"" + ss.subscription_uuid + "\"},";
                            if (chess.standard.Contains("MQTT"))
                                Task.Run(() => Handle_Received_Message(chess.identifier, ss.subject_name, chess.Id));
                        }



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error subscribing " + ex.ToString());
                    }


                }

                return Json(JsonConvert.DeserializeObject(result.Trim(',') + "]"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.ToString());
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Get the capabilities for a CHESS asset
        /// </summary>
        /// <remarks>Get capability of a CHESS from the digital twin</remarks>
        /// <param name="id"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpGet]
        [Route("/capability/{id}")]
        [Route("/chess/1.0/capability/{id}")]
        [ValidateModelState]
        [SwaggerOperation("CapabilityCHESSGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(IoT.Services.Chess), description: "CHESS capability retrieved successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]

        public virtual IActionResult CapabilityCHESSGet([FromRoute] String id, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            // Here we get the capabilities of a registered CHESS !
            Token token = decodeToken(Authorization);


            List<IoT.Services.Chess> res = new List<IoT.Services.Chess>();

            String query = "SELECT * FROM  DigitalTwins  where $metadata.$model = 'dtmi:com:flexchess:chess;1'";
            if (id != null) query += " and $dtId='" + id + "'";
            try
            {

                Pageable<IoT.Services.Chess> twinResponse = Program.dtClient.Query<IoT.Services.Chess>(query);

                if (twinResponse != null)
                {
                    // Get the digital twin !

                    foreach (IoT.Services.Chess twin in twinResponse)
                    {
                        String subjectAttribute = getSubjectAttribute(twin);

                        // check to authorize access
                        if (subjectAttribute.Equals("") || ((token.location == null || subjectAttribute.Contains(token.location)) &&
                            (token.uri == null || subjectAttribute.Contains(token.uri)) && subjectAttribute.Contains(token.scope + token.aud)))
                        {

                            String submodels = "";
                            CHESSSubjectDefinition subject = null;
                            query = "SELECT DT.$dtId FROM DIGITALTWINS twin JOIN DT RELATED twin.contains WHERE twin.$dtId = '" + twin.Id + "'  and  DT.$metadata.$model = 'dtmi:digitaltwins:aas:Submodel;1'";
                            Pageable<BasicDigitalTwin> smResponse = Program.dtClient.Query<BasicDigitalTwin>(query);
                            foreach (BasicDigitalTwin smTwin in smResponse)
                            {

                                if (smTwin != null)
                                    submodels += smTwin.Id.Replace(twin.Id, "") + " ";
                            }

                            foreach (CHESSSubjectDefinition sub in Program.uudexStatusSubjects)
                            {
                                if (sub.subject_name.Contains(twin.Id))
                                {
                                    subject = sub;

                                }
                            }
                            if (subject != null)
                                twin.Contents.Add("status", subject);

                            twin.Contents.Add("submodels", submodels);
                            res.Add(twin);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }

            return Json(res);


        }


        /// <summary>
        /// Get the flexibility status profile for a CHESS asset
        /// </summary>
        /// <remarks>Get status of the registered CHESS frm the digital twin</remarks>
        /// <param name="id"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpGet]
        [Route("/status/{id}")]
        [Route("/chess/1.0/status/{id}")]
        [ValidateModelState]
        [SwaggerOperation("statusCHESSGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(CHESSStatus), description: "CHESS status retrieved successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]

        public virtual IActionResult StatusCHESSGet([FromQuery] String service, [FromRoute][Required] String id, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            // Here we get the capabilities of a registered CHESS !
            Token token = decodeToken(Authorization);


            List<CHESSStatus> res = new List<CHESSStatus>();

            String query = "SELECT * FROM  DigitalTwins  where $metadata.$model = 'dtmi:com:flexchess:chess;1' and $dtId='" + id + "'";
            try
            {

                Pageable<IoT.Services.Chess> twinResponse = Program.dtClient.Query<IoT.Services.Chess>(query);

                if (twinResponse != null)
                {
                    // Get the digital twin !

                    foreach (IoT.Services.Chess twin in twinResponse)
                    {
                        String subjectAttribute = getSubjectAttribute(twin);

                        // check to authorize access
                        if (subjectAttribute.Equals("") || ((token.location == null || subjectAttribute.Contains(token.location)) &&
                            (token.uri == null || subjectAttribute.Contains(token.uri)) && subjectAttribute.Contains(token.scope + token.aud)))
                        {


                            //String status = Get("http://" + twin.identifier + "/status/" + id);
                            //CHESSStatus chessStatus = (CHESSStatus)JsonConvert.DeserializeObject(status);
                            CHESSStatus chessStatus = new CHESSStatus();
                            chessStatus.identifier = twin.identifier;
                            chessStatus.location = twin.location;
                            chessStatus.id = twin.Id;



                            query = "SELECT DT.$dtId FROM  DigitalTwins twin JOIN DT RELATED twin.contains WHERE twin.$dtId = '" + twin.Id + "' and  DT.$metadata.$model = 'dtmi:com:flexchess:chessstatus;1'";
                            try
                            {

                                Pageable<BasicDigitalTwin> statusResponse = Program.dtClient.Query<BasicDigitalTwin>(query);

                                if (statusResponse != null)
                                {
                                    Console.WriteLine("CHESS Status " +  Newtonsoft.Json.JsonConvert.SerializeObject(statusResponse));
                                    chessStatus.status = new IoT.Services.ChessStatus[statusResponse.Count()];
                                    int count = 0;
                                    foreach (BasicDigitalTwin tsId in statusResponse)
                                    {
                                     
                                        IoT.Services.ChessStatus twinStatus = Program.dtClient.GetDigitalTwin<IoT.Services.ChessStatus>(tsId.Id);
                                        if (service == null || twinStatus.service == null || (service.Contains(twinStatus.service) || twinStatus.service.Equals("all")))
                                        {
                                            chessStatus.status[count] = twinStatus;
                                            if (getStatus(twinStatus))
                                                chessStatus.currentStatus += (twinStatus.status + " " + twinStatus.capacity + ";");
                                            count++;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) { Console.WriteLine("Error - " + ex.ToString());  return StatusCode(500); }
                            res.Add(chessStatus);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }

            return Json(res);


        }

        /// <summary>
        /// Get the flexibility status profile for all CHESS assets
        /// </summary>
        /// <remarks>Get status of all registered CHESS from digital twin</remarks>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpGet]
        [Route("/status")]
        [Route("/chess/1.0/status")]
        [ValidateModelState]
        [SwaggerOperation("statusCHESSGetAll")]
        [SwaggerResponse(statusCode: 200, type: typeof(CHESSStatus), description: "CHESS status retrieved successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]

        public virtual IActionResult StatusCHESSGetAll([FromQuery] String service, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            // Here we get the capabilities of a registered CHESS !
            Token token = decodeToken(Authorization);


            List<CHESSStatus> res = new List<CHESSStatus>();

            String query = "SELECT * FROM  DigitalTwins  where $metadata.$model = 'dtmi:com:flexchess:chess;1'";
            try
            {

                Pageable<IoT.Services.Chess> twinResponse = Program.dtClient.Query<IoT.Services.Chess>(query);

                if (twinResponse != null)
                {
                    // Get the digital twin !

                    foreach (IoT.Services.Chess twin in twinResponse)
                    {
                        String subjectAttribute = getSubjectAttribute(twin);

                        // check to authorize access
                        if (subjectAttribute.Equals("") || ((token.location == null || subjectAttribute.Contains(token.location)) &&
                            (token.uri == null || subjectAttribute.Contains(token.uri)) && subjectAttribute.Contains(token.scope + token.aud)))
                        {

                            CHESSStatus chessStatus = new CHESSStatus();
                            chessStatus.identifier = twin.identifier;
                            chessStatus.location = twin.location;
                            chessStatus.id = twin.Id;


                            query = "SELECT DT.$dtId FROM  DigitalTwins twin JOIN DT RELATED twin.contains WHERE twin.$dtId = '" + twin.Id + "' and  DT.$metadata.$model = 'dtmi:com:flexchess:chessstatus;1'";
                            try
                            {

                                Pageable<BasicDigitalTwin> statusResponse = Program.dtClient.Query<BasicDigitalTwin>(query);

               
                                if (statusResponse != null)
                                {
                                    chessStatus.status = new IoT.Services.ChessStatus[statusResponse.Count()];
                                    int count = 0;
                                    foreach (BasicDigitalTwin tsId in statusResponse)
                                    {
                                     
                                       
                                        IoT.Services.ChessStatus twinStatus = Program.dtClient.GetDigitalTwin<IoT.Services.ChessStatus>(tsId.Id);
                                        if (service == null || twinStatus.service == null || (service.Contains(twinStatus.service) || twinStatus.service.Equals("all")))
                                        {
                                            chessStatus.status[count] = twinStatus;
                                            if (getStatus(twinStatus))
                                                chessStatus.currentStatus += (twinStatus.status + " " + twinStatus.capacity + ";");
                                            count++;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex) { return StatusCode(500); }

                            res.Add(chessStatus);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }

            return Json(res);


        }

        /// <summary>
        /// Request the update of the flexibility status profile for a CHESS asset
        /// </summary>
        /// <remarks>Update the status of a registered CHESS in digital twin</remarks>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpPost]
        [Route("/status/{id}")]
        [Route("/chess/1.0/status/{id}")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("statusCHESSSet")]
        [SwaggerResponse(statusCode: 200, type: typeof(IoT.Services.ChessStatus), description: "CHESS status updated successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]

        public virtual IActionResult StatusCHESSSet([FromRoute][Required] String id, [FromBody] CHESSStatus body, [FromHeader] String Authorization)
        {

            if (Authorization == null)
                return StatusCode(401);

            // Here we get the capabilities of a registered CHESS !
            Token token = decodeToken(Authorization);


            List<IoT.Services.ChessStatus> res = new List<IoT.Services.ChessStatus>();

            String query = "SELECT * FROM  DigitalTwins  where $metadata.$model = 'dtmi:com:flexchess:chess;1' and $dtId='" + id + "'";
            try
            {

                Pageable<IoT.Services.Chess> twinResponse = Program.dtClient.Query<IoT.Services.Chess>(query);

                if (twinResponse != null)
                {
                    // Get the digital twin !

                    foreach (IoT.Services.Chess twin in twinResponse)
                    {
                        String subjectAttribute = getSubjectAttribute(twin);
                        Console.WriteLine("Subject attributes " + twin.Id + " " + subjectAttribute);

                        // check to authorize access
                        if (subjectAttribute.Equals("") || ((token.location == null || subjectAttribute.Contains(token.location)) &&
                            (token.uri == null || subjectAttribute.Contains(token.uri)) && subjectAttribute.Contains(token.scope + token.aud)))
                        {


                            query = "SELECT DT.$dtId FROM  DigitalTwins twin JOIN DT RELATED twin.contains WHERE twin.$dtId = '" + id + "' and  DT.$metadata.$model = 'dtmi:com:flexchess:chessstatus;1'";
                            try
                            {

                                Pageable<IoT.Services.ChessStatus> statusResponse = Program.dtClient.Query<IoT.Services.ChessStatus>(query);

                                if (statusResponse != null)
                                {

                                    // we need to see if there is a change in the status or removal  / addition
                                    foreach (IoT.Services.ChessStatus status in statusResponse)
                                    {
                                        uint count = 1;
                                        Boolean found = false;
                                        foreach (IoT.Services.ChessStatus twinstatus in body.status)
                                        {
                                            // Set the status digital twin !

                                            uint index = (UInt32.Parse(status.Id.Substring(status.Id.LastIndexOf("-") + 1)));

                                            Console.WriteLine("Count " + count + " index " + index);
                                            if (index == count)
                                            {
                                                Console.WriteLine("Matched");
                                                twinstatus.Id = status.Id;
                                                Console.WriteLine("Updating status - " + status.Id + " " + body);
                                                JsonPatchDocument patch = new JsonPatchDocument();
                                                patch.AppendReplace("/capacity", twinstatus.capacity);
                                                patch.AppendReplace("/endtime", twinstatus.endtime);
                                                patch.AppendReplace("/starttime", twinstatus.starttime);
                                                patch.AppendReplace("/recurrence", twinstatus.recurrence);
                                                patch.AppendReplace("/status", twinstatus.status);
                                                patch.AppendReplace("/service", twinstatus.service);
                                                Response newtwin = Program.dtClient.UpdateDigitalTwin(status.Id, patch);
                                                found = true;
                                                res.Add(twinstatus);
                                                break;
                                            }

                                            count++;
                                        }


                                        // we need to delete the relationships first before the twin - ToDo
                                        //if (!found) Program.dtClient.DeleteDigitalTwin(status.Id);
                


                                    }

                                } else {
                                        // We need to create the digital twin status 
                                        int count = 1;
                                        foreach (IoT.Services.ChessStatus twinstatus in body.status)
                                        {                   
                                             twinstatus.Id = id + "-status-" + count;
                                             Program.dtClient.CreateOrReplaceDigitalTwin(twinstatus.Id, twinstatus);
                                             var relationship = new BasicRelationship
                                             {
                                                TargetId = twinstatus.Id,
                                                Name = "contains"
                                             };
                                             string relId = $"{id}-contains->{twinstatus.Id}";
                                             Program.dtClient.CreateOrReplaceRelationship(id, relId, relationship);
                                             count++;
                                        }
                                }

                                Program.publish(twin.Id, 0, body);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("error setting the status for DT " + id);
                                return StatusCode(500);
                            }
                        }

                    }
                } 
            }
            catch (Exception ex)
            {
                return this.NotFound(ex.Message);
            }

            return Json(res);


        }



        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Update capability of a CHESS in the digital twin</remarks>
        /// <param name="body"></param>
        /// <response code="200">ok</response>
        /// <response code="401">unauthorized</response>
        /// <response code="404">not found</response>
        [HttpPost]
        [Route("/capability/{id}")]
        [Route("/chess/1.0/capability/{id}")]
        [Consumes("application/json")]
        [Produces("text/plain")]
        [ValidateModelState]
        [SwaggerOperation("CapabilityCHESSPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(Result), description: "CHESS capability updated successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorizaed")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]

        public virtual IActionResult CapabilityCHESSPost([Required][FromBody][SwaggerRequestBody("application/json")] Chess body, [FromRoute][Required] String id, [FromHeader] String Authorization)
        {


            if (Authorization == null)
                return StatusCode(401);

            Token token = decodeToken(Authorization);

            // update the associated data !
            try
            {

                List<IoT.Services.Chess> res = new List<IoT.Services.Chess>();

                String query = "SELECT * FROM  DigitalTwins  where $metadata.$model = 'dtmi:com:flexchess:chess;1' and $dtId='" + id + "'";


                Pageable<IoT.Services.Chess> twinResponse = Program.dtClient.Query<IoT.Services.Chess>(query);

                if (twinResponse != null)
                {
                    // Get the digital twin !

                    foreach (IoT.Services.Chess twin in twinResponse)
                    {
                        String subjectAttribute = getSubjectAttribute(twin);

                        // check to authorize access
                        if (subjectAttribute.Equals("") || ((token.location == null || subjectAttribute.Contains(token.location)) &&
                            (token.uri == null || subjectAttribute.Contains(token.uri)) && subjectAttribute.Contains(token.scope + token.aud)))
                        {
                            JsonPatchDocument patch = new JsonPatchDocument();
                            patch.AppendReplace("/version", body.version);
                            patch.AppendReplace("/standard", body.standard);
                            patch.AppendReplace("/locaion", body.location);
                            patch.AppendReplace("/identifier", body.identifier);
                            patch.AppendReplace("/Contents", body.Contents);

                            Response response = Program.dtClient.UpdateDigitalTwin(id, patch);

                            return StatusCode(200);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return this.NotFound(ex.Message);

            }


            return StatusCode(200);


        }
    

        // Estimate cycle cost using empirical degradation model
        protected Double cycleCost(Double SocMax, Double SocMin)
        {
            Console.WriteLine("Soc max " + SocMax + " Soc min " + SocMin);
            // This is a placeholder - the EMS adapter overrides this function in the controller
            return 0;
        }

        /// <summary>
        /// Get the current flexibility provision for the specified target
        /// </summary>
        /// <remarks>  </remarks>
        /// <param name="body"></param>
        /// <response code="200">Successfully processed the updates</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("/current")]
        [Route("/ems/1.0.0/current")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("currentPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<CHESSStatus>), description: "Successfully got the curent flexibility profiles for given target profile")]
        public virtual IActionResult currentPost([FromBody]CHESSStatus body, [FromHeader] String Authorization)
        {

            // Update the stored token
            if (Authorization != null)
                authToken = Authorization;

            if (body == null) return BadRequest();

            if (Authorization == null)
                return StatusCode(401);


            List<CHESSStatus> res = new List<CHESSStatus>();


            // now get latest status values from AAS server

            string result = Get("http://aasserver.default.svc/status", authToken);

            CHESSStatus[] currentStates = JsonConvert.DeserializeObject<CHESSStatus[]>(result);

            foreach (ChessStatus status in body.status)
            {       


                
                try {
                    Int32 startHour = Int32.Parse(status.starttime.Substring(0, 2));
                    Int32 startMinute = Int32.Parse(status.starttime.Substring(3, 2));
                    Int32 endHour = Int32.Parse(status.endtime.Substring(0, 2));
                    Int32 endMinute = Int32.Parse(status.endtime.Substring(3, 2));  
                    Double targetCapacity = Double.Parse(status.capacity);
                    TimeSpan start = new TimeSpan(startHour, startMinute, 0); 
                    TimeSpan end = new TimeSpan(endHour, endMinute, 0);
                    
                    // See the current flexibility provision matching this requirement
                    foreach (CHESSStatus currentState in currentStates)
                    {
                        Console.WriteLine("CHESS status " + currentState.id);   
                        if (body == null || body.location == null || currentState.location == null || currentState.location.ToLower().Equals(body.location.ToLower()))                    
                        foreach (ChessStatus currentStatus in currentState.status)
                        {
                            if (currentStatus.recurrence.ToLower().Contains("daily") || currentStatus.recurrence.Equals(status.recurrence))
                            {
                                if (status.status == null || status.status.ToLower().Contains(currentStatus.status.ToLower()))
                                if (status.service == null || status.service.Equals("all") || status.service.ToLower().Equals(currentStatus.service.ToLower()))
                                {
                                    startHour = Int32.Parse(currentStatus.starttime.Substring(0, 2));
                                    startMinute = Int32.Parse(currentStatus.starttime.Substring(3, 2));
                                    endHour = Int32.Parse(currentStatus.endtime.Substring(0, 2));
                                    endMinute = Int32.Parse(currentStatus.endtime.Substring(3, 2));  
                                    
                                    Double capacity = Double.Parse(currentStatus.capacity);
                                    TimeSpan thisStart = new TimeSpan(startHour, startMinute, 0); 
                                    TimeSpan thisEnd = new TimeSpan(endHour, endMinute, 0);

                                    TimeSpan period = thisEnd-thisStart;
                                    TimeSpan overlap = new TimeSpan(0,0,0);
                                    Double capacityStart = 0;
                                    Double capacityEnd = 0;
                                    if ((start - thisStart).TotalMinutes >= 0)
                                    {
                                        Console.WriteLine("Overlap is " + (start - thisStart).TotalMinutes);
                                        capacityStart = (Double) ((start-thisStart).TotalMinutes) * capacity / period.TotalMinutes;
                                        capacityEnd = (Double) ((end-thisStart).TotalMinutes) * capacity / period.TotalMinutes;
                                    }
                                    if (capacityStart > capacity) capacityStart = capacity;
                                    if (capacityEnd > capacity) capacityEnd = capacity;
                        

                                    Console.WriteLine("Capacity at start is " + capacityStart+ " of total " + capacity);
                                    Console.WriteLine("Capacity at end is " + capacityEnd+ " of total " + capacity);
                                    currentStatus.capacityStart = capacityStart;
                                    currentStatus.capacityEnd = capacityEnd;
                                }
                            }

                        }
                        
                    }
                
                    // Now see what capacity is needed
                    if (targetCapacity > 0)
                    {


                            Console.WriteLine("Capacity needed for " + status.starttime + "->" + status.endtime + " " + status.status  + " is " + targetCapacity);

                            foreach (CHESSStatus currentState in currentStates)
                            {
                                Console.WriteLine("CHESS status " + currentState.id);   
                                if (body == null || body.location == null || currentState.location == null || currentState.location.ToLower().Equals(body.location.ToLower()))                    
                                foreach (ChessStatus currentStatus in currentState.status)
                                {
                                    if (currentStatus.recurrence.ToLower().Contains("daily") || currentStatus.recurrence.Equals(status.recurrence))
                                    {
                                        Console.WriteLine("Checking " + currentState.id);
                                        if (status.status == null ||status.status.ToLower().Contains(currentStatus.status.ToLower()))
                                        if (status.service == null || status.service.Equals("all") || status.service.ToLower().Equals(currentStatus.service.ToLower()))
                                        {

                                            if (status.status.ToLower().Contains("force"))
                                            {

                                                String url = "http://aasserver.default.svc/api/v3.0/submodels/" + currentState.id + "EnergyEntity/submodel-elements/$value";

                                                Console.WriteLine("Getting energy entity from DT - " + url);
                                                result = Get(url, authToken);
                                                Console.WriteLine("Got " + result);
                                        
                                                DTData[] dtData = JsonConvert.DeserializeObject<DTData[]>(result);
                                                Double maxEnergy = 0;
                                                Double maxPower = 0;
                                                Double efficiency = 1;
                                                Double power80 = 0;
                                                Double minSoC = 10;

                                                if (dtData != null && dtData.Length > 0)
                                                {
                                                    maxEnergy = dtLookup(dtData, "maximumAllowedBatteryEnergy");      
                                                    efficiency = dtLookup(dtData, "energyRoundtripEfficiency") / 100;
                                                } else
                                                    Console.WriteLine("Cannot get energy data from DT");


                                                url = "http://aasserver.default.svc/api/v3.0/submodels/" + currentState.id + "PowerEntity/submodel-elements/$value";

                                                Console.WriteLine("Getting power entity from DT - " + url);
                                                result = Get(url, authToken);
                                                Console.WriteLine("Got " + result);
                                        
                                                dtData = JsonConvert.DeserializeObject<DTData[]>(result);
                                                
                                                if (dtData != null && dtData.Length > 0)
                                                {
                                                    maxPower = dtLookup(dtData, "maximumAllowedBatteryPower");      
                                                    power80 = dtLookup(dtData, "powerCapabilityAt80Charge");
                                                    if (power80 == null || power80 == 0) power80 = maxPower;

                                                } else
                                                    Console.WriteLine("Cannot get power data from DT");

                                               url = "http://aasserver.default.svc/api/v3.0/submodels/" + currentState.id + "StateOfBatteryEntity/submodel-elements/$value";

                                                Console.WriteLine("Getting battery state entity from DT - " + url);
                                                result = Get(url, authToken);
                                                Console.WriteLine("Got " + result);
                                        
                                                dtData = JsonConvert.DeserializeObject<DTData[]>(result);
                                                
                                                if (dtData != null && dtData.Length > 0)
                                                {
                                                    minSoC = dtLookup(dtData, "minSoC");      
                                                

                                                } else
                                                    Console.WriteLine("Cannot get battery state data from DT");



                                                Double thisCapacity = currentStatus.capacityEnd; 
                                                currentStatus.cycleCost =  cycleCost( thisCapacity/maxEnergy, currentStatus.capacityStart/maxEnergy );

                                                // see if we are energy capacity or power limited 
                                                if (maxEnergy * efficiency > thisCapacity)
                                                {
                                                    Console.WriteLine("Can increase this capacity " + currentState.id);
                                                    currentStatus.efficiency = efficiency;
                                                
                                                    Double availableCapacity = maxEnergy * (1 - minSoC/100) * efficiency - thisCapacity;
                                                    Double energyLimit = efficiency * power80 * (end-start).TotalMinutes / 60;    
                                                    if (availableCapacity > energyLimit) 
                                                    {
                                                        Console.WriteLine("Available capacity greater than limit "  + energyLimit);
                                                        currentStatus.capacityMax = energyLimit;
                                                        targetCapacity -= (energyLimit - thisCapacity);
                                                        currentStatus.probability =  thisCapacity/(energyLimit+thisCapacity);
                                                        
                                                    } else
                                                    {
                                                        Console.WriteLine("Available capacity less than limit "  + energyLimit);
                                                        currentStatus.probability =  thisCapacity / (thisCapacity+availableCapacity);
                                                        currentStatus.capacityMax = thisCapacity + availableCapacity;
                                                        targetCapacity -= availableCapacity;
                                                    }
                                                    if (res.IndexOf(currentState) < 0)
                                                        res.Add(currentState);
                                                }

                                            }
                                        }
                                    }
                                }
                                
                            }
                            // now calculate the priority levels based on the probability of being in the state
                            Int32[] ranks = new Int32[res.Count];
                            for (int i=0; i<res.Count; i++)
                            {
                                Double maxProb = 0;
                                Int32 maxIndex = 0;
                                foreach (CHESSStatus resState in res)
                                {
                                    Double totalProb = 0;
                                    Int32 probCount = 1;
                                    foreach (ChessStatus resStatus in resState.status)
                                    {
                                        if (resStatus.probability > 0)
                                        {
                                            totalProb += resStatus.probability;
                                            probCount ++;
                                        }
                                    }

                                    // rank based on probability order
                                    if (totalProb/probCount > maxProb)
                                    {
                                        int j=0;
                                        for (j=0; j<i; j++)
                                        {
                                            if (ranks[j] ==  res.IndexOf(resState))
                                                break;
                                        }
                                        if (j<i)
                                        {
                                            maxIndex = res.IndexOf(resState);
                                            maxProb = totalProb/probCount;
                                        }
                                    }

                                }
                                ranks[i] = maxIndex;
                                Console.WriteLine("Rank " + i + " is "  + maxProb);
                            }
                            Int32 count = 0;
                            Int32 level = 0;
                            // now assign the priority levels
                            for (int i=0; i<res.Count; i++)
                            {
                                foreach (ChessStatus resStatus in res[ranks[i]].status)
                                {
                                    resStatus.priority = level;
                                }
                                count++; 
                                if (count >= res.Count/4)
                                {
                                    count = 0;
                                    level++;
                                }
                            

                            }



                    }
                    

                } catch (Exception ex)
                {
                    Console.WriteLine("Error " + ex.ToString());
                }      
        
                        
            }
            
            res.Add(body);

            return Json(res);


        }

        /// <summary>
        /// Get the current flexibility provision for the specified target
        /// </summary>
        /// <remarks>  </remarks>
        /// <response code="200">Successfully processed the updates</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("/current")]
        [Route("/ems/1.0.0/current")]
        [Produces("application/json")]
        [ValidateModelState]
        [SwaggerOperation("currentGet")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<CHESS>), description: "Successfully got the current available flexibility profiles")]
        public virtual IActionResult currentGet([FromQuery] String location, [FromQuery] String recurrence, [FromQuery] String service, [FromHeader] String Authorization)
        {

            // Update the stored token
            if (Authorization != null)
                authToken = Authorization;


            if (Authorization == null)
                return StatusCode(401);


            List<CHESSStatus> res = new List<CHESSStatus>();


            // now get latest status values from AAS server

            string result = Get("http://aasserver.default.svc/status", authToken);

            CHESSStatus[] currentStates = JsonConvert.DeserializeObject<CHESSStatus[]>(result);

                    
            // See the current flexibility provision 
            foreach (CHESSStatus currentState in currentStates)
            {
                if ((currentState.currentStatus == null || currentState.currentStatus.Equals("available")) && (location == null || currentState.location == null || location.ToLower().Equals(currentState.location.ToLower())))
                {

                    foreach (ChessStatus currentStatus in currentState.status)
                    {
                            if (recurrence == null || recurrence.ToLower().Equals(currentStatus.recurrence.ToLower()))
                            if (service == null || service.ToLower().Equals("all") || service.ToLower().Equals(currentStatus.service.ToLower()))
                                
                                res.Add(currentState);
                            

                    }
                }

            }
            
        
            return Json(res);


        }

        /// <summary>
        /// Invoke the optimiser synchronously with specified limits and objectives
        /// </summary>
        /// <remarks> The available options are requested from the local EMS adapter to pass to the optimiser </remarks>
        /// <param name="body"></param>
        /// <response code="200">Successfully processed the updates</response>
        /// <response code="400">Bad request</response>
        /// <response code="422">Unprocessable entity</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        /// <response code="503">Server busy</response>
        [HttpPost]
        [Route("/run")]
        [Route("/opt/1.0.0/run")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("runPost")]
        [SwaggerResponse(statusCode: 200, type: typeof(OptimiserOut[]), description: "Successfully executed optimisation")]
        public virtual IActionResult runPost([FromBody]OptimiserIn[] body, [FromHeader] String Authorization) 
        {


                String result = "[";
                // Call the EMS endpoint

                foreach(OptimiserIn option in body)
                    foreach(OptionIn optionIn in option.Options)
                    {
                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(optionIn);
                        Console.WriteLine("EMS request for " + json);
                        result += "{\"Objective\":\"" + optionIn.objective + "\", \"Option\":\"" + optionIn.option + "\", \"CHESS\":" + Post("http://emsadapter.default.svc/current", json, Authorization) + "},";
                    }

                // todo - aggregate the input schedules and pass to the optimier

                Console.WriteLine("Result  " + result);

                return Json(JsonConvert.DeserializeObject(result.Trim(',')+"]"));
        }
    }
}

