// Main AAS Server Program
// Provides the core APIs for the CHESS nodes / adapters
// tim@toshiba-bril.com

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Azure.DigitalTwins.Core;
using Azure.Identity;
using System.Net.Sockets;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Azure;
using IO.Swagger.Controllers;
using System.Reflection;
using System.Reflection.Metadata;
using IoT.Services;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json.Linq;

namespace IO.Swagger
{


    // Subscription class for UUDEX
    public class Subscription
    {

        public String subscription_uuid { get; set; }
        public String subscription_name { get; set; }
        public String subscription_state { get; set; }
        public String create_datetime { get; set; }
        public String owner_endpoint_uuid { get; set; }


    }

    // Subject class for UUDEX
    public class Subject
    {
        public String subject_uuid { get; set; }
        public String subject_name { get; set; }
        public String dataset_instance_key { get; set; }
        public String description { get; set; }
        public String subscription_type { get; set; }
        public String fulfillment_types_available { get; set; }
        public String full_queue_behavior { get; set; }
        public String max_queue_size_kb { get; set; }
        public String max_message_count { get; set; }
        public String priority { get; set; }
        public String dataset_definition_uuid { get; set; }
        public String owner_participant_uuid { get; set; }
        public String create_datetime { get; set; }

        public String chessId { get; set; }
        public SubscribeSubject subscription { get; set; }

    }


   

    // Subscription dataset definition
    public class Dataset_definition
    {

        public String dataset_definition_uuid { get; set; }
        public String dataset_definition_name { get; set; }
        public String description { get; set; }
        public String schema { get; set; }
        public String create_datetime { get; set; }
    }

    // Subscription definition
    public class CHESSSubjectDefinition
    {
        public String subject_uuid { get; set; }
        public String subject_name { get; set; }
        public String dataset_instance_key { get; set; }
        public String description { get; set; }
        public String subscription_type { get; set; }
        public String fulfillment_types_available { get; set; }
        public String full_queue_behavior { get; set; }
        public String max_queue_size_kb { get; set; }
        public String max_message_count { get; set; }
        public String priority { get; set; }
        public String backing_exchange_name { get; set; }
        public String owner_participant_uuid { get; set; }
        public String owner_participant_short_name { get; set; }
        public String create_datetime { get; set; }
        public Dataset_definition dataset_definition { get; set; }

    }


    //UUDEX SubscribeSubject
    public class SubscribeSubject
    {
        public String subscription_subject_id { get; set; }
        public String subject_name { get; set; }
        public String subject_uuid { get; set; }
        public String subscription_uuid { get; set; }
        public String preferred_fulfillment_type { get; set; }
        public String backing_queue_name { get; set; }

    }



    //UUDEX Message 
    public class CHESSMessage
    {

        public String message { get; set; }

    }

    //UUDEX Consumed Telemetry Messages
    public class ConsumedTelemetryMessage
    {

        public String subject_uuid { get; set; }
        public String subject_name { get; set; }
        public String message_count { get; set; }
        public CHESSMessage[] messages { get; set; }

    }

    //UUDEX Consumed Status Messages
    public class ConsumedStatusMessage
    {

        public String subject_uuid { get; set; }
        public String subject_name { get; set; }
        public String message_count { get; set; }
        public CHESSMessage[] messages { get; set; }

    }

    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {

        public static DigitalTwinsClient dtClient;
        public static UUDEXWebClient uudexClient;

        public static String uudexUser = "";
        public static String uudexPass = "";

        protected static List<Subscription> uudexSubscriptions;
        public static List<Subject> uudexSubjects;

        // Store the status subscriptions here 
        public static CHESSSubjectDefinition[] uudexStatusSubjects;

        // create a subject for the CHESS in the UUDEX server
        public static String createSubject(String chessId)
        {



            String subscription = "{\"subject_name\": \"" + chessId + "telemetry\", \"dataset_instance_key\": \"" + chessId + "\", \"description\": \"Subject For CHESS\", \"subscription_type\": \"MEASUREMENT_VALUES\", \"fulfillment_types_available\": \"DATA_PUSH\", \"dataset_definition_uuid\": \"68ce654c-a94e-4d3b-879a-fae76fa8963f\"}";
            String url = "subjects";
            Console.WriteLine("Creating subject - " + subscription);
            Subject sub = JsonConvert.DeserializeObject<Subject>(uudexClient.post(url, subscription));
            sub.chessId = chessId;

            uudexSubjects.Add(sub);

            return sub.subject_uuid;

        }

        // create a subscription
        protected static Subscription createSubscription(String name)
        {

            String subscription = "{\"subscription_name\": \"" + name + "AASServer\", \"subscription_state\":\"ACTIVE\"}";
            String url = "subscriptions";

            Subscription sub = JsonConvert.DeserializeObject<Subscription>(uudexClient.post(url, subscription));

            uudexSubscriptions.Add(sub);


            return sub;

        }

        // subscribe to a prior subscription
        public static SubscribeSubject subscribe(int index, String subjectUUID)
        {

            if (index < uudexSubscriptions.Count)
            {
                String subscription = "{\"subject_uuid\": \"" + subjectUUID + "\", \"preferred_fulfillment_type\":\"DATA_PUSH\"}";
                String url = "subscriptions/" + uudexSubscriptions[index].subscription_uuid + "/subjects";

                SubscribeSubject sub = JsonConvert.DeserializeObject<SubscribeSubject>(uudexClient.post(url, subscription));


                return sub;
            }
            else return null;
        }


        // publish to a status subject
        public static void publish(String chessId, int index, CHESSStatus status)
        {

            if (index >= uudexSubscriptions.Count) { Console.WriteLine("index out of range " + index); return; }
            foreach (CHESSSubjectDefinition sub in uudexStatusSubjects)
            {
                if (sub.subject_name.Contains(chessId))
                {

                    Console.WriteLine("Publishing to q " + sub.subject_uuid + " for " + chessId);
                    CHESSMessage newmessage = new CHESSMessage();
                    newmessage.message = JsonConvert.SerializeObject(status);


                    String url = "subjects/" + sub.subject_uuid + "/publish";

                    uudexClient.post(url, JsonConvert.SerializeObject(newmessage));
                } else
                    Console.WriteLine("No match for " + chessId + " with " + sub.subject_name);

            }



        }

        // Post data to adapters
        private static string Post(string uri, string json, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.Timeout = 12000;

            var data = Encoding.ASCII.GetBytes(json);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.PreAuthenticate = true;
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

        // consume the messages and send to adapters 
        public static ConsumedTelemetryMessage consume(String chessId, int index)
        {
            if (index < uudexSubscriptions.Count)
            {

                String url = "subscriptions/" + uudexSubscriptions[index].subscription_uuid + "/consume";

                ConsumedTelemetryMessage messages = JsonConvert.DeserializeObject<ConsumedTelemetryMessage>(uudexClient.get(url));

                return messages;

            }
            else return null;
        }


        // Retrieve all the available subjects
        public static CHESSSubjectDefinition[] getSubjectDefinitions()
        {

            String url = "subjects";

            String json = uudexClient.get(url);
            CHESSSubjectDefinition[] messages = JsonConvert.DeserializeObject<CHESSSubjectDefinition[]>(json);
            List<CHESSSubjectDefinition> statusSubjects = new List<CHESSSubjectDefinition>();

            foreach (CHESSSubjectDefinition msg in messages)
            {

                if (msg.subject_name.Contains("telemetry"))
                {
                    Subject sub = new Subject();
                    sub.priority = msg.priority;
                    sub.description = msg.description;
                    sub.subject_uuid = msg.subject_uuid;
                    sub.full_queue_behavior = msg.full_queue_behavior;
                    sub.dataset_definition_uuid = msg.dataset_definition.dataset_definition_uuid;
                    sub.subject_name = msg.subject_name;
                    sub.chessId = msg.subject_name.Replace("telemetry", "");

                    uudexSubjects.Add(sub);

                    sub.subscription = subscribe(0, sub.subject_uuid);
                }
                else
                {
                    statusSubjects.Add(msg);
                }

            }

            return statusSubjects.ToArray();
        }


        /// <summary>
        /// Polling loop to handle the telemetry events
        /// </summary>
        protected static void polling()
        {
            // we poll and consume the telemetry messages from the UUDEX Server
            while (true)
            {

                String url = "subscriptions/" + uudexSubscriptions[0].subscription_uuid + "/consume";

                // Check to see what message there are for this subject
                String msg = uudexClient.get(url);
                ConsumedTelemetryMessage[] cms = JsonConvert.DeserializeObject<ConsumedTelemetryMessage[]>(msg);

               
                String result = "[";

                foreach (ConsumedTelemetryMessage cm in cms)
                {
                    if (cm.messages.Length != 0)
                    try
                    {
                        // we probably need just the most recent message !
                        CHESSMessage message = cm.messages[cm.messages.Length - 1];
                        var telemetry = JObject.Parse(message.message);


                        Subject subject = null;

                        // we need to create the patch document if the value is different
                        foreach (Subject sub in uudexSubjects)
                        {
                            if (sub.subject_uuid.Equals(cm.subject_uuid))
                            {
                                subject = sub;
                                break;
                            }
                        }



                        if (subject != null)
                        {

                            String query = "SELECT DT.$dtId FROM DIGITALTWINS twin JOIN DT RELATED twin.submodelElement WHERE twin.$dtId = '" + subject.subject_name + "'";

                            Pageable<BasicDigitalTwin> smeResponse = Program.dtClient.Query<BasicDigitalTwin>(query);
                            foreach (BasicDigitalTwin smeTwin in smeResponse)
                            {
                                Response<IoT.Services.DataSpecificationIEC61360> dataResponse = Program.dtClient.GetDigitalTwin<IoT.Services.DataSpecificationIEC61360>(smeTwin.Id.Substring(4));



                                if (dataResponse != null)
                                {

                                    String parameter = dataResponse.Value.Id.Substring(subject.chessId.Length).Replace("-","/");

                                    var value = telemetry[parameter];

                                        if (value != null && !value.Equals(dataResponse.Value.Value))
                                        {

                                            JsonPatchDocument patch = new JsonPatchDocument();

  
                                            if (dataResponse.Value.Value != null)
                                                patch.AppendReplace("/value", (string)value);
                                            else
                                                patch.AppendAdd("/value", (string)value);



                                            Response response = Program.dtClient.UpdateDigitalTwin(dataResponse.Value.Id, patch);
                                        }
                                }


                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());


                    }


                }
                Thread.Sleep(30000);
            }
        }

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // create the UUDEX client for asynchronous event messages with the UUDEX server
            // mTLS certificates are used to secure the connection to the server !

            String path = System.Environment.GetEnvironmentVariable("PFX_CERT_PATH");
            String pass = System.Environment.GetEnvironmentVariable("PFX_CERT_PASS");

            uudexUser = System.Environment.GetEnvironmentVariable("UUDEX_USER");
            uudexPass = System.Environment.GetEnvironmentVariable("UUDEX_PASS");


            X509Certificate2 cert = new X509Certificate2(path, pass);
            uudexClient = new UUDEXWebClient(cert);

            uudexSubjects = new List<Subject>();
            uudexSubscriptions = new List<Subscription>();


            string _adtServiceUrl = System.Environment.GetEnvironmentVariable("adtServiceUrl", EnvironmentVariableTarget.Process);
            string _adtClientId = System.Environment.GetEnvironmentVariable("adtClientId", EnvironmentVariableTarget.Process);
            string _adtClientSecret = System.Environment.GetEnvironmentVariable("adtClientSecret", EnvironmentVariableTarget.Process);
            string _adtTenantId = System.Environment.GetEnvironmentVariable("adtTenantId", EnvironmentVariableTarget.Process);



            ClientSecretCredential credential = new ClientSecretCredential(_adtTenantId, _adtClientId, _adtClientSecret);

            //            var credential = new DefaultAzureCredential();
            dtClient = new DigitalTwinsClient(new Uri(_adtServiceUrl), credential);
            Console.WriteLine($"Service client created – ready to go");

            // We need to create the main subscription for this AASService  

            String guid = Guid.NewGuid().ToString().Substring(0, 13).Replace("-", "");
            Console.WriteLine("Create subscription " + createSubscription(guid).subscription_uuid);

            // Now retrieve the CHESS subjects

            uudexStatusSubjects = getSubjectDefinitions();

            Task.Run(() => polling());

            CreateWebHostBuilder(args).Build().Run();
        }





        /// <summary>
        /// Create the web host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>IWebHostBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
