// CHESS Core Network Program
// Provides the core APIs for the CHESS nodes / adapters
// tim@toshiba-bril.com

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Azure.DigitalTwins.Core;
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
using IO.Swagger.Models;
using IoT.Services;
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


    }



    // CHESS data
    public class CHESS
    {
        public String identifier { get; set; }
        public String location { get; set; }
        public String id { get; set; }
        public String currentStatus { get; set; }

        public ChessStatus[] status { get; set; }
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
        public static String CHESSNodeKey = "";
        public static String guid = "";
        public static String _adtServiceUrl = "";
        public static String _adtClientId = "";
        public static String _adtClientSecret = "";
        public static String _adtTenantId = "";


        // We need to persist this data as it is required after reboot.
        // local persistent storage is an option - ToDo

        protected static List<Subscription> uudexSubscriptions;
        protected static List<Subject> uudexSubjects;

        // Store the status subscriptions here 
        protected static CHESSSubjectDefinition[] uudexStatusSubjects;

        // create a subject for the CHESS in the UUDEX server
        public static String createSubject(String chessId)
        {

            String subscription = "{\"subject_name\": \"" + chessId  + "\", \"dataset_instance_key\": \"" + chessId + "\", \"description\": \"Status For CHESS\", \"subscription_type\": \"MEASUREMENT_VALUES\", \"fulfillment_types_available\": \"DATA_PUSH\", \"dataset_definition_uuid\": \"68ce654c-a94e-4d3b-879a-fae76fa8963e\"}";
            String url = "subjects";
            Console.WriteLine("Creating subject - " + subscription);

            // check if subject already exists 
            
            foreach (CHESSSubjectDefinition subject in uudexStatusSubjects)
            {

                Console.WriteLine("Looking for subject " + subject.subject_name + " matching " + chessId);

                if (subject.subject_name.Equals(chessId))
                {
                    Console.WriteLine("Found " + subject.subject_name);
                  
                    return subject.subject_uuid;

                }

            }
            

            Subject sub = JsonConvert.DeserializeObject<Subject>(uudexClient.post(url, subscription));
            sub.chessId = chessId;

            uudexSubjects.Add(sub);

            return sub.subject_uuid;

        }

        // create a subscription
        protected static Subscription createSubscription()
        {

            String subscription = "{\"subscription_name\": \"CHESSNode" + guid + "CHESSNode\", \"subscription_state\":\"ACTIVE\"}";
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

                Console.WriteLine("Subscribing to " + subjectUUID);

                try {
                    String subscribeResponse = uudexClient.post(url, subscription);
                    Console.WriteLine("Subscribe response " + subscribeResponse);
                    SubscribeSubject sub = JsonConvert.DeserializeObject<SubscribeSubject>(subscribeResponse);
                    return sub;
                } catch (Exception ex)
                {
                    Console.WriteLine("Cannot subscribe - " + ex.ToString());
                }

                return null;
            }
            else return null;
        }


        // publish to a status subject
        public static void publish(String chessId, int index, CHESSStatus status)
        {

            if (index >= uudexSubscriptions.Count) { Console.WriteLine("index out of range " + index); return; }
            foreach (CHESSSubjectDefinition sub in uudexStatusSubjects)
            {
                if (sub.subject_name.Equals(chessId))
                {

                    CHESSMessage newmessage = new CHESSMessage();
                    newmessage.message = JsonConvert.SerializeObject(status);


                    String url = "subjects/" + sub.subject_uuid + "/publish";

                    uudexClient.post(url, JsonConvert.SerializeObject(newmessage));
                }

            }



        }

        // Get HTTP request
        public static string Get(string uri, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (token != null)
                request.Headers.Add("Authorization", token);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        // Post data HTTP 
        private static string Post(string uri, string json, string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            request.Timeout = 12000;

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
        public static CHESSSubjectDefinition[] getSubjectDefinitions(CHESS[] myChess, String prefix)
        {

            String url = "subjects";

            String json = uudexClient.get(url);
            CHESSSubjectDefinition[] messages = JsonConvert.DeserializeObject<CHESSSubjectDefinition[]>(json);

            foreach (CHESSSubjectDefinition msg in messages)
            {
                if (!msg.subject_name.Contains("telemetry"))
                {

                    Subject sub = new Subject();
                    sub.priority = msg.priority;
                    sub.description = msg.description;
                    sub.subject_uuid = msg.subject_uuid;
                    sub.full_queue_behavior = msg.full_queue_behavior;
                    sub.dataset_definition_uuid = msg.dataset_definition.dataset_definition_uuid;
                    sub.subject_name = msg.subject_name;
                    sub.chessId = msg.subject_name;
                    foreach (CHESS chess in myChess)
                    {
                        if (chess.id.Contains(prefix) && chess.id.Equals(sub.chessId))
                        {
                            // Found the CHESS !
                            uudexSubjects.Add(sub);
                            subscribe(0, msg.subject_uuid);
                            break;
                        }
                    }
                }


            }

            return messages;
        }


        /// <summary>
        /// Polling loop to handle the telemetry events
        /// </summary>
        protected static void polling(string token)
        {
            // we poll and consume the messages from the Flex Platform
            while (true)
            {
                try
                {
                    
                    String url = "subscriptions/" + uudexSubscriptions[0].subscription_uuid + "/consume";

                    // Check to see what message there are 
                    ConsumedStatusMessage[] cms = JsonConvert.DeserializeObject<ConsumedStatusMessage[]>(uudexClient.get(url));

                    String result = "[";

                    foreach (ConsumedStatusMessage cm in cms)
                    {
                        foreach (CHESSMessage message in cm.messages)
                        {
                            
                            var json = JObject.Parse(message.message);

                            String shortname = cm.subject_name.Substring(cm.subject_name.IndexOf("-",4)+1);
                            Console.WriteLine("POST status message for CHESS " +  cm.subject_name + " : " + shortname);

                            result += ("{" + Post("http://" + json["identifier"] + "/status/" +  shortname, message.message, token) + "},");

                        }
                    }
                    Console.WriteLine(result + "]");
                    
                    Thread.Sleep(30000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in polling loop - " + ex.ToString());
                }
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
            String token = System.Environment.GetEnvironmentVariable("AUTH_TOKEN");
            CHESSNodeKey = System.Environment.GetEnvironmentVariable("CHESS_NODE_KEY");
            String prefix = System.Environment.GetEnvironmentVariable("CHESS_PREFIX");

            _adtServiceUrl = System.Environment.GetEnvironmentVariable("adtServiceUrl", EnvironmentVariableTarget.Process);
            _adtClientId = System.Environment.GetEnvironmentVariable("adtClientId", EnvironmentVariableTarget.Process);
            _adtClientSecret = System.Environment.GetEnvironmentVariable("adtClientSecret", EnvironmentVariableTarget.Process);
            _adtTenantId = System.Environment.GetEnvironmentVariable("adtTenantId", EnvironmentVariableTarget.Process);

            X509Certificate2 cert = new X509Certificate2(path, pass);
            uudexClient = new UUDEXWebClient(cert);

            uudexSubjects = new List<Subject>();
            uudexSubscriptions = new List<Subscription>();

            // We need to create the main subscription for this AASService  

            guid = Guid.NewGuid().ToString().Substring(0, 13).Replace("-", "");
            Console.WriteLine("Create subscription " + createSubscription().subscription_uuid);

            // Now retrieve the CHESS subjects for this CHESS node
            string caps = Get("http://aasserver/status", token);
            if (caps != null)
            {
                Console.WriteLine("CHESS - " + caps);

                CHESS[] chess = JsonConvert.DeserializeObject<CHESS[]>(caps);

                uudexStatusSubjects = getSubjectDefinitions(chess, prefix);

                Task.Run(() => polling(token));

                CreateWebHostBuilder(args).Build().Run();
            }
            else
                Console.WriteLine("Unable to retrieve the adapter capabilities");
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
