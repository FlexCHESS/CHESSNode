/*
 * Aveva adapter - for HVAC / PV data
 *
 *
 * OpenAPI spec version: 1.0.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using IO.Swagger.Attributes;
using IO.Swagger.Security;
using Microsoft.AspNetCore.Authorization;
using IO.Swagger.Models;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure.DigitalTwins.Core;
using Azure;
using IoT.Services;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization.DataContracts;


namespace IO.Swagger.Controllers
{
    /// <summary>
    /// API server for the Aveva CHESS adapter
    /// </summary>
    [ApiController]
    public class DefaultApiController : Controller
    {



        protected static List<CHESS> assets;

        protected static String authToken = "";
        private readonly ILogger<DefaultApiController> _logger;

        // The main CHESS data structure
        public class CHESS
        {

            public String identifier { get; set; }
            public String location { get; set; }
            public String id { get; set; }
            public String currentStatus { get; set; }
            public ChessStatus[] status { get; set; }
            public ChessStatus[] limits { get; set; }


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


        public DefaultApiController(ILogger<DefaultApiController> logger)
        {
            _logger = logger;
            if (assets == null)
                assets = new List<CHESS>();
        }

        // check for the activation of a chess status
        private Boolean getStatus(ChessStatus status)
        {
            DateTime starttime;
            DateTime endtime;

            if (status.recurrence.ToLower().Equals("daily") ||
                (status.recurrence.ToLower().Equals("weekdays") && ((DateTime.Today.DayOfWeek != DayOfWeek.Saturday) && (DateTime.Today.DayOfWeek != DayOfWeek.Sunday))) ||
                (status.recurrence.ToLower().Equals("weekends") && ((DateTime.Today.DayOfWeek == DayOfWeek.Saturday) || (DateTime.Today.DayOfWeek == DayOfWeek.Sunday))))
            {

                return true;


            }

            return false;
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

        /// <summary>
        /// The controller for handling the initialisation of CHESS adapter
        /// </summary>
        /// 
        /// POST - Setup CHESS assets with this adapter

        [HttpPost]
        [Consumes("application/json")]
        [Produces("text/plain")]
        [Route("/init")]
        public IActionResult Init([Required][FromBody] CHESS body, [FromHeader] String Authorization)
        {

            if (body == null)
            {
                return Json(assets);
            }

            // store the token for later use to call APIs
            if (Authorization != null)
                authToken = Authorization;

            Console.WriteLine("Body " + Newtonsoft.Json.JsonConvert.SerializeObject(body));

            assets.Add(body);


            return StatusCode(200);

        }

        /// <summary>
        /// The controller for handling the status setting / retrieval from CHESS
        /// </summary>
        /// 
        /// POST - Setup a schedule for the CHESS assets
        /// GET -  Get the status of assets


        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Route("/status/{id}")]

        public IActionResult Status([FromRoute] String id, [FromBody] CHESS body, [FromHeader] String Authorization)
        {

            // Update the stored token
            if (Authorization != null)
                authToken = Authorization;

            if (id == null)
            {
                return Json(assets);
            }


            foreach (CHESS chess in assets)
            {

                Console.WriteLine("Looking for CHESS " + chess.id + " matching " + id);

                if (chess.id.Equals(id))
                    if (body != null)
                    {

                        // we need to update !

                        chess.limits = body.status;

                        return Json(chess);

                    }

            }
            return StatusCode(404);
        }

        // Get the status for a CHESS

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Route("/status/{id}")]
        public IActionResult Status([FromRoute] String id, [FromHeader] String Authorization)
        {

            // Update the stored token
            if (Authorization != null)
                authToken = Authorization;

            if (id == null)
            {
                return Json(assets);
            }


            foreach (CHESS chess in assets)
            {

                Console.WriteLine("Looking for CHESS " + chess.id + " matching " + id);

                if (chess.id.Equals(id))

                    return Json(chess);

            }


            return StatusCode(404);

        }
        /// <summary>
        /// Trigger update of Aveva information
        /// </summary>
        /// <remarks>Receives an array of timeseries data </remarks>
        /// <param name="body"></param>
        /// <response code="200">Successfully processed the updates</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Route("/update")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ValidateModelState]
        [SwaggerOperation("UpdatePost")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<LoadCurve>), description: "Successfully processed the updates")]
        public virtual IActionResult UpdatePost([FromBody]CHESSData body, [FromHeader] String Authorization)
        {


            if (body == null) return BadRequest();

            if (Authorization == null)
                return StatusCode(401);

            // Need to use token to authorize the access
            Token token = decodeToken(Authorization);

            if (!token.scope.Contains("saFlexibilityProvider"))
                return StatusCode(403);

            // Assume digital twins exist already!

            string prefix = "it-iren-chess1";
            string definition = "";
     
            PropertyInfo[] propertyInfos = typeof(CHESSData).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {


                JsonPatchDocument patch = new JsonPatchDocument();

                CHESSDataElement [] propertyValue = (CHESSDataElement[])propertyInfo.GetValue(body);
                {
                    var element = propertyValue[propertyValue.Length - 1];
                
                    patch.AppendAdd("/value", element.value.Replace(",","."));
                   

                    Console.WriteLine("Updating " + prefix + propertyInfo.Name + " " + element.value);

                    try
                    {
                        Response response = Program.dtClient.UpdateDigitalTwin(prefix + propertyInfo.Name, patch);
                    } catch {

                        Console.WriteLine("Create twin");

                        DataSpecificationIEC61360 dataSpecificationIEC61360 = new DataSpecificationIEC61360();
                        dataSpecificationIEC61360.Id = prefix + propertyInfo.Name;
                        dataSpecificationIEC61360.Value = element.value.Replace(",",".");
                        dataSpecificationIEC61360.SourceOfDefinition = "IREN";
                        if (propertyInfo.Name.Contains("AL"))
                        {
                            dataSpecificationIEC61360.Symbol = "A";
                            dataSpecificationIEC61360.UnitIdValue = "";
                            dataSpecificationIEC61360.Unit = "A";
                            definition = "Current in A";
                        } else
                        if (propertyInfo.Name.Contains("VFF"))
                        {
                            dataSpecificationIEC61360.Symbol = "V";
                            dataSpecificationIEC61360.UnitIdValue = "";
                            dataSpecificationIEC61360.Unit = "V";
                            definition = "Voltage in V";
                        } else
                        if (propertyInfo.Name.Contains("Hz"))
                        {
                            dataSpecificationIEC61360.Symbol = "Hz";
                            dataSpecificationIEC61360.UnitIdValue = "";
                            dataSpecificationIEC61360.Unit = "Hz";
                            definition = "Frequency in Hz";
                        } else
                        if (propertyInfo.Name.Contains("kWh"))
                        {
                            dataSpecificationIEC61360.Symbol = "kWh";
                            dataSpecificationIEC61360.UnitIdValue = "";
                            dataSpecificationIEC61360.Unit = "kWh";
                            definition = "Energy in kWh";
                        } else
                        if (propertyInfo.Name.Contains("PF"))
                        {
                            dataSpecificationIEC61360.Symbol = "";
                            dataSpecificationIEC61360.UnitIdValue = "";
                            dataSpecificationIEC61360.Unit = "";
                            definition = "Power factor";
                        } else
                        {
                            dataSpecificationIEC61360.Symbol = "kW";
                            dataSpecificationIEC61360.UnitIdValue = "";
                            dataSpecificationIEC61360.Unit = "kW";
                            definition = "Power in kW";
                        }
                        dataSpecificationIEC61360.ValueFormat = "NR2S..3.3";
                        dataSpecificationIEC61360.PreferredName = new LangStringPreferredNameTypeIec61360();
                        dataSpecificationIEC61360.PreferredName.langString = new LangString();
                        dataSpecificationIEC61360.PreferredName.langString.Language = "en";
                        dataSpecificationIEC61360.PreferredName.langString.Text = prefix + propertyInfo.Name;
                        dataSpecificationIEC61360.ShortName = new LangStringShortNameTypeIec61360();
                        dataSpecificationIEC61360.ShortName.langString = new LangString();
                        dataSpecificationIEC61360.ShortName.langString.Language = "en";    
                        dataSpecificationIEC61360.ShortName.langString.Text = propertyInfo.Name;
                        dataSpecificationIEC61360.LevelType = DataSpecificationIEC61360LevelType.Nom;
                        dataSpecificationIEC61360.Definition = new LangStringDefinitionTypeIec61360();
                        dataSpecificationIEC61360.Definition.langString = new LangString();
                        dataSpecificationIEC61360.Definition.langString.Language = "en";
                        dataSpecificationIEC61360.Definition.langString.Text = definition;
                        dataSpecificationIEC61360.DataType = DataSpecificationIEC61360DataType.REAL_MEASURE;
                        Program.dtClient.CreateOrReplaceDigitalTwin<DataSpecificationIEC61360>(prefix + propertyInfo.Name, dataSpecificationIEC61360);


                        IoT.Services.SubmodelElement submodelElement = new IoT.Services.SubmodelElement();

                        submodelElement.Id = "sme-" + prefix + propertyInfo.Name;
                        submodelElement.Kind = new Kind { };
                        submodelElement.DataSpecificationTemplateGlobalRefValue = "";
                        submodelElement.DisplayName = new LangStringElementType();
                        submodelElement.Description = new LangStringElementType();
                        submodelElement.Tags = new LangStringElementType();
                        submodelElement.SemanticIdValue = "Measurement";

                        Console.WriteLine(JsonConvert.SerializeObject(submodelElement));

                        Program.dtClient.CreateOrReplaceDigitalTwin("sme-" + prefix + propertyInfo.Name, submodelElement);

                        var smerel = new SubmodelElementDataSpecificationRelationship
                        {
                            TargetId = "sme-" + prefix + propertyInfo.Name,
                            Name = "submodelElement"

                        };

                        string relId = $"{prefix}telemetry-contains->sme-{prefix}{propertyInfo.Name}";
                        Program.dtClient.CreateOrReplaceRelationship(prefix + "telemetry", relId, smerel);
                        Console.WriteLine("Created contains relationship successfully");
                    }

                }

            }
            return StatusCode(200);

        }
    }
}
