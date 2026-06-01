/*
 * CHESS adapter for Fox BESS - using FoxESS Cloud API
 * tim@toshiba-bril.com
 */

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Net.NetworkInformation;
using foxBESSadapter;
using System.Globalization;
using MQTTnet.Client;
using MQTTnet.Extensions.TopicTemplate;

namespace foxBESSadatper.Controllers
{



    // The main CHESS Status data structure 
    public class Status
    {
        public String status { get; set; }
        public String service { get; set; }
        public String starttime { get; set; }
        public String endtime { get; set; }
        public String capacity { get; set; }
        public String recurrence { get; set; }

        public Int32 active { get; set; }
    }

    // The main CHESS data structure
    public class CHESS
    {

        public String identifier { get; set; }
        public String location { get; set; }
        public String id { get; set; }

        public String currentStatus { get; set; }

        public Status[] status { get; set; }
        public DeviceData deviceData { get; set; }

    }


    // Contact details
    public class Contact
    {
        public String name { get; set; }
        public String email { get; set; }
        public String phone { get; set; }
    }

    // Device descriptor
    public class Device
    {
        public String moduleSN { get; set; }
        public String deviceSN { get; set; }
    }

    // Plant details
    public class PlantDetail
    {
        public String stationName { get; set; }
        public String country { get; set; }
        public String city { get; set; }
        public String address { get; set; }
        public String createDate { get; set; }
        public String postcode { get; set; }
        public String capacity { get; set; }
        public String timezone { get; set; }
        public Contact user { get; set; }

        public Contact installer { get; set; }

        public Device[] modules { get; set; }

    }

    // Plant data

    public class PlantData
    {

        public String stationID { get; set; }
        public String name { get; set; }
        public String ianaTimezone { get; set; }

    }

    // Plant list
    public class PlantList
    {
        public PlantData[] data { get; set; }
        public Int32 currentPage { get; set; }
        public Int32 pageSize { get; set; }
        public Int32 total { get; set; }
    }

    // Device data
    public class DeviceData
    {

        public String deviceSN { get; set; }
        public String moduleSN { get; set; }
        public String stationID { get; set; }
        public Int32 status { get; set; }
        public Boolean hasPV { get; set; }
        public Boolean hasBattery { get; set; }
        public String deviceType { get; set; }
        public String productType { get; set; }
        public String stationName { get; set; }

    }

    // Device list
    public class DeviceList
    {
        public DeviceData[] data { get; set; }
        public Int32 currentPage { get; set; }
        public Int32 pageSize { get; set; }
        public Int32 total { get; set; }
    }

    // Device list response data
    public class DeviceListResponse
    {
        public Int32 errno { get; set; }
        public String msg { get; set; }
        public DeviceList result { get; set; }
    }

    // Functions
    public class Function
    {
        public Boolean scheduler { get; set; }
    }

    // Device details
    public class DeviceDetail
    {

        public String deviceSN { get; set; }
        public String moduleSN { get; set; }
        public String stationID { get; set; }
        public String stationName { get; set; }
        public String afciVersion { get; set; }
        public String managerVersion { get; set; }
        public String masterVersion { get; set; }
        public String slaveVersion { get; set; }
        public String hardwareVersion { get; set; }
        public Int32 status { get; set; }

        public Function function { get; set; }

    }

    public class Datas 
    {
        public String variable { get; set; }
        public String unit { get; set; }
        public String name { get; set; }
        public dynamic value { get; set; }
  
    }

    // Real-time data from devices
    public class RealData
    {

        public String deviceSN { get; set;} 
        public Datas[] datas {get; set;}
        public String time { get; set; }

    }


    // Real-time data response
    public class RealDataResponse
    {

        public Int32 errno { get; set; }
        public RealData[] result { get; set; }

    }


    // DT Data
    public class DTData
    {

        public String Id { get; set; }
        public String DataType { get; set; }
        public String LevelType { get; set; }
        public String ValueFormat { get; set; }
        public String Symbol { get; set; }
        public String Unit { get; set; }
        public Double Value { get; set; }

    }

    // Battery State of Charge limits
    public class BatterySOC
    {
        public Int32 minSoc { get; set; }
        public Int32 minSocOnGrid { get; set; }

    }

    // Time representation
    public class Time
    {

        public Int32 hour { get; set; }
        public Int32 minute { get; set; }
    }

    // Force charge times 
    public class ForceChargeTime
    {
        public String enable1 { get; set; }
        public Time startTime1 { get; set; }
        public Time endTime1 { get; set; }
        public String enable2 { get; set; }
        public Time startTime2 { get; set; }
        public Time endTime2 { get; set; }

    }

    // Schedule
    public class Schedule
    {

        public Int32 enable { get; set; }
        public Int32 startHour { get; set; }
        public Int32 startMinute { get; set; }
        public Int32 endHour { get; set; }
        public Int32 endMinute { get; set; }
        public String workMode { get; set; }
        public Int32 minSocOnGrid { get; set; }
        public Int32 fdSoc { get; set; }
        public Int32 fdPwr { get; set; }
        public Int32 maxSoc { get; set; }
    }

    // Set of schedules for a scheduler
    public class Scheduler
    {
        public String deviceSN { get; set; }
        public Schedule[] groups { get; set; }
    }


    public class SchedulerResponse
    {
        public Int32 errno { get; set; }
        public String msg { get; set; }
        public Scheduler result { get; set; }
    }

    // Real-time data query response
    public class RealQuery
    {
        public String deviceSN { get; set; }
        public RealData[] datas { get; set; }
    }

    // Module data
    public class Module
    {
        public String moduleSN { get; set; }
        public String stationID { get; set; }
        public Int32 status { get; set; }
        public Int32 signal { get; set; }

    }

    // Module list
    public class ModuleList
    {
        public Module[] data { get; set; }
        public Int32 currentPage { get; set; }
        public Int32 pageSize { get; set; }
        public Int32 total { get; set; }
    }

    public static class HomeControllerHelpers
    {
        public static string ToUnixTimeMilliSeconds(DateTime date)
        {
            DateTimeOffset dto = new DateTimeOffset(date);
            return dto.ToUnixTimeMilliseconds().ToString();
        }
        public static string CreateMD5(string path, string key, string timestamp)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(path + "\\r\\n" + key + "\\r\\n" + timestamp);
                var hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
    public class MQTT
    {
        public static IMqttClient mqttClient;
       
        public static async Task<bool> Publish(string channel, string value)
        {
          
            if (mqttClient.IsConnected == false)
            {
            
                return false;
            }

            var message = new MQTTnet.MqttApplicationMessageBuilder()
                    .WithTopic(channel)
                    .WithPayload(value)
                    .WithRetainFlag()
                    .Build();
            await mqttClient.PublishAsync(message);
            return true;
        }


        //connect to mqtt

        public static async Task Connect()
        {
            string clientId = Guid.NewGuid().ToString();
            var factory = new MQTTnet.MqttFactory();
        
            mqttClient = factory.CreateMqttClient();
          
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("uudex.default.svc", 1883)                    // MQTT broker address and port
                .WithCredentials(Program.uudexUser, Program.uudexPass)       // Set username and password
                .WithClientId(clientId)
                .WithCleanSession()
                .Build();

            var connectResult = await mqttClient.ConnectAsync(options);
        }

    }


/// <summary>
///  Controller class for handling the requests (provided APIs) for the CHESS Node / Network Core  
/// </summary>
public class HomeController : Controller
    {

        protected static List<CHESS> assets;
        protected static String authToken = null;
        private readonly ILogger<HomeController> _logger;

     

        protected Double dtLookup(DTData[] dtData, String id)
        {
            Double rv = 0;
            foreach (DTData data in dtData)
            if (data.Id.EndsWith(id)) 
            {
                    if (data.Symbol.Contains("k"))
                        rv += data.Value*1000;
                    else
                        rv += data.Value;
            }
            return rv;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            if (assets == null) {
                assets = new List<CHESS>();
                Task.Run(() => MQTT.Connect());
            }
        }

        // check for the activation of a chess status
        private Boolean getStatus(Status status)
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

        // Get -  remote HTTP request
        public string Get(string uri, string token)
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

        // FoxPost -  remote HTTP request to FoxESS Cloud
        private string FoxPost(string path, string json)
        {
            DateTime now = DateTime.Now;
            string timestamp = HomeControllerHelpers.ToUnixTimeMilliSeconds(now);
            string signature = HomeControllerHelpers.CreateMD5(path, Program.key, timestamp);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.foxesscloud.com" + path);
            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 120000;

            var data = Encoding.ASCII.GetBytes(json);

            Console.WriteLine("Hash " + signature + " payload " + json);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.PreAuthenticate = true;
            request.Headers.Add("timestamp", timestamp);
            request.Headers.Add("lang", "en");
            request.Headers.Add("signature", signature);
            request.Headers.Add("token", Program.key);
            request.Accept = "application/json";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Console.WriteLine("Response " + responseString);

            return responseString;
        }


        /// <summary>
        /// Polling loop to update  CHESS DT  
        /// </summary>
        /// 

        protected void polling(int asset)
        {

            String response = "";
            String json = "";
          
            CHESS chess = assets[asset];

      
            Double totalEnergy = 0;
            Double efficiency = 1;

            String url = Program.urlprefix + "/aas/submodels/" + chess.id + "EnergyEntity/submodel-elements/$value";

            Console.WriteLine("Getting max energy from DT - " + url);

            String result = Get(url, authToken);

            DTData[] dtData = JsonConvert.DeserializeObject<DTData[]>(result);
            Console.WriteLine("Data " + dtData.ToString());

            if (dtData != null && dtData.Length > 0)
            {
                totalEnergy = dtLookup(dtData, "maximumAllowedBatteryEnergy");
                efficiency = dtLookup(dtData, "energyRoundtripEfficiency") / 100;
                Console.WriteLine("TotalEnergy  " + totalEnergy + " Efficiency " + efficiency);
            }
            else
                Console.WriteLine("Cannot get energy data from DT");


            while (true)
            {

                Double chargeTotal = 0;
                Double dischargeTotal = 0;
                Double temperature = 0;
                Double batSoC = 0;
                Double gridPower = 0;
                Double invPower = 0;
                String jsonschedule = "";

                try
                {

                    // check for status changes
                    int count = 0;
                    Scheduler scheduler = new Scheduler();
                    scheduler.groups = new Schedule[chess.status.Length];
                    scheduler.deviceSN = chess.deviceData.deviceSN;
          
                    if (chess.status!=null)
                    foreach (Status status in chess.status)
                    {
                        // see if there are any weekday / weekend changes !


                        if (!status.status.Contains("available") && getStatus(status) && status.active == 0)
                        {
                               
                            status.active = 1;
                        
                            json = "{\r\n  \"deviceSN\": \""+chess.deviceData.deviceSN+"\",\r\n  \"enable\": 1\r\n}";
                            response = FoxPost("/op/v1/device/set/flag", json);
                            Console.WriteLine("Enable = " + response);
                             

                            count++;

                        }
                        else if (status.active == 1)
                        {
                                                  
                            json = "{\r\n  \"deviceSN\": \""+chess.deviceData.deviceSN+"\",\r\n  \"enable\": 0\r\n}";
                            response = FoxPost("/op/v1/device/set/flag", json);

                            Console.WriteLine("Disable = " + response);
                            status.active = 0;
                        }


                    }
                  
                    
                    json = "{\r\n\t\"sns\": [\"" + chess.deviceData.deviceSN + "\"], \r\n\t\"variables\": []\r\n}";
                    response = FoxPost("/op/v1/device/real/query", json);
                    Console.WriteLine("Response " + response);
                    RealDataResponse realData = JsonConvert.DeserializeObject<RealDataResponse>(response);

                    if (realData.result != null)
                    foreach (RealData rd in realData.result)
                    {

                        foreach (Datas data in rd.datas)
                        {
                            MQTT.Publish("N/" + chess.id + data.variable, "{\"unit\": \""+ data.unit + "\", \"name\": \""+data.name+"\", \"value\": "+ data.value.ToString() + "}").Wait();
                            if (data.variable.Equals("chargeEnergyTotal"))
                                chargeTotal = data.value;
                            if (data.variable.Equals("dischargeEnergyTotal"))
                                dischargeTotal = data.value;
                            if (data.variable.Equals("batTemperature_1"))
                                temperature = data.value;
                            if (data.variable.Equals("SoC_1"))
                                batSoC = data.value;
                            if (data.variable.Equals("gridConsumptionPower"))
                                gridPower = data.value;
                            if (data.variable.Equals("invBatPower_1"))
                                invPower = data.value;
                        
                        }
                    }
                    if (totalEnergy > 0)
                    {
                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "StateOfBatteryEntity/submodel-elements/sme-" + chess.id + "stateOfCharge/invoke/$value";

                        String update = "{\"value\":" + batSoC + "}";

                        Console.WriteLine("Updating DT - " + url + " - " + update);

                        result = Post(url, update, authToken);

                        Console.WriteLine(result);
  
                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "gridPower/invoke/$value";

                        update = "{\"value\":" + (-invPower) + "}";

                        Console.WriteLine("Updating DT - " + url + " - " + update);

                        result = Post(url, update, authToken);

                        Console.WriteLine(result);
                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "CDD/submodel-elements/sme-" + chess.id + "temperature/invoke/$value";

                        update = "{\"value\":" + temperature + "}";

                        Console.WriteLine("Updating DT - " + url + " - " + update);

                        result = Post(url, update, authToken);

                        Console.WriteLine(result);

                        json = "{\r\n  \"deviceSN\": \""+chess.deviceData.deviceSN+"\",\r\n  \"enable\": 1\r\n}";
                        response = FoxPost("/op/v1/device/set/flag", json);

                        Console.WriteLine("Response " + response);
                    }
                    else
                    {
                        json = "{\r\n  \"deviceSN\": \""+chess.deviceData.deviceSN+"\",\r\n  \"enable\": 0\r\n}";
                        response = FoxPost("/op/v1/device/set/flag", json);

                        Console.WriteLine("No charge data available");
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                // 2 minute updates
                Thread.Sleep(120000);
            }

        }

        /// <summary>
        /// The controller for handling the initialisation of CHESS adapter
        /// </summary>
        /// 
        /// POST - Setup CHESS assets with this adapter

        [HttpPost(Name = "init")]
        [Consumes("application/json")]
        [Produces("text/plain")]

        public IActionResult Init([Required][FromBody] CHESS body, [FromHeader] String Authorization)
        {

            if (body == null)
            {
                return Json(assets);
            }

            // store the token for later use to call APIs
            if (Authorization != null)
                authToken = Authorization;

            String url = "/op/v0/device/list";
            String json = "{\r\n  \"currentPage\": 1,\r\n  \"pageSize\": 10\r\n}";
            String response = FoxPost(url, json);


            DeviceListResponse? deviceListResponse = JsonConvert.DeserializeObject<DeviceListResponse>(response);
            if (deviceListResponse != null && deviceListResponse.errno == 0)
            {
                foreach (DeviceData device in deviceListResponse.result.data)
                {

                    Console.WriteLine("Got device " + device.deviceSN);
                    if (device.deviceSN.Equals(body.identifier))
                    {

                        Console.WriteLine("Adding device " + body.id);
                        body.deviceData = device;
                        assets.Add(body);

                       
                       Task.Run(() => polling(assets.Count - 1));
                        
                    }
                }

            }
            else return StatusCode(404);
            return StatusCode(200);

        }

        /// <summary>
        /// The controller for handling the status setting / retrieval from CHESS
        /// </summary>
        /// 
        /// POST - Setup a schedule for the CHESS assets
        /// GET -  Get the status of assets

        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Route("status/{id}")]

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

                if (chess.id.EndsWith(id))
                {

                    List<Status> statusList = new List<Status>();

                    String json = "{\r\n\t\"deviceSN\": \"" + chess.deviceData.deviceSN + "\"\r\n}";

                    String response = FoxPost("/op/v0/device/scheduler/get", json);
                    if (response == null)
                        return StatusCode(500);


                    SchedulerResponse currentSchedule = JsonConvert.DeserializeObject<SchedulerResponse>(response);


                    if (currentSchedule != null )
                    {

                        
                        if (currentSchedule.errno == 0 && currentSchedule.result != null)
                        {
                           
                                Console.WriteLine("Result schedule enable " + currentSchedule.msg);
                                foreach (Schedule schedule in currentSchedule.result.groups)
                                {

                                    Status status = new Status();
                                    status.starttime = schedule.startHour.ToString("00") + ":" + schedule.startMinute.ToString("00");
                                    status.endtime = schedule.endHour.ToString("00") + ":" + schedule.endMinute.ToString("00");
                                    status.recurrence = "daily";
                                    status.status = schedule.workMode;
                                   
                                    status.capacity = (schedule.fdPwr * (schedule.endHour - schedule.startHour + 60* (schedule.endMinute - schedule.startMinute))).ToString();
                                    status.service = schedule.enable.ToString();
                                    statusList.Add(status);
                                }
                            
                        
                        } else Console.WriteLine("Error " + currentSchedule.errno +  " " + currentSchedule.msg);

                    }


                    return Json(statusList);
                }

            }
            return StatusCode(404);
        }

        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Route("status/{id}")]

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

                if (chess.id.EndsWith(id))
                {

                    // we need to update !
                    chess.currentStatus = body.currentStatus;
                    chess.status = body.status;




                    if (chess.deviceData.hasBattery) // && chess.deviceData.status == 1)
                    {

                        // now set up the schedule for the assets in the CHESS

                        String json = "{\r\n  \"deviceSN\": \""+chess.deviceData.deviceSN+"\",\r\n  \"enable\": 0\r\n}";
                        String response = FoxPost("/op/v1/device/set/flag", json);

                        Console.WriteLine("Response " + response);

                        Scheduler scheduler = new Scheduler();

                        scheduler.deviceSN = chess.deviceData.deviceSN;

                        scheduler.groups = new Schedule[chess.status.Length];
                        int count = 0;
                        foreach (Status status in chess.status)
                        {
                            if (!status.status.Contains("available") && getStatus(status))
                            {

                                Schedule schedule = new Schedule();
                                schedule.workMode = status.status;
                                schedule.startHour = Int32.Parse(status.starttime.Substring(0, 2));
                                schedule.startMinute = Int32.Parse(status.starttime.Substring(3, 2));
                                schedule.endHour = Int32.Parse(status.endtime.Substring(0, 2));
                                schedule.endMinute = Int32.Parse(status.endtime.Substring(3, 2));
                                //schedule.extraParam = new ExtraParam();
 
                                var remoteTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                         
                                if (remoteTimeZone.IsDaylightSavingTime(DateTime.Now))
                                {
                                    schedule.startHour += 2;
                                    schedule.endHour += 2;
                                } else
                                {
                                    schedule.startHour += 1;
                                    schedule.endHour += 1;
                                }

                                if (schedule.startHour > 23) schedule.startHour -= 24;
                                if (schedule.endHour > 23) schedule.endHour -= 24;

                                TimeSpan start = new TimeSpan(schedule.startHour, schedule.startMinute, 0);
                                TimeSpan end = new TimeSpan(schedule.endHour, schedule.startMinute, 0);

                                Double period = end.Subtract(start).TotalMinutes / 60;
                                Double capacity = Double.Parse(status.capacity);
                                
                                schedule.fdSoc = 90;
        

                                schedule.fdPwr = (int)(capacity / period);
                                schedule.minSocOnGrid = 10;
                             
                                schedule.enable = 1;
                                schedule.minSocOnGrid = 10;
                                schedule.maxSoc = 100;
                                if (schedule.workMode.ToLower().Equals("forcedischarge"))
                                    schedule.fdSoc = 10;
                                scheduler.groups[count] = schedule;
                                status.active = 1;

                                count++;
                            }
                            else status.active = 0;

                        }
                  
                        
                        response = FoxPost("/op/v1/device/scheduler/enable", JsonConvert.SerializeObject(scheduler,Newtonsoft.Json.Formatting.Indented));

                        Console.WriteLine("CHESS response " + response);
                        
                        json = "{\r\n  \"deviceSN\": \""+chess.deviceData.deviceSN+"\",\r\n  \"enable\": 1\r\n}";
                        response = FoxPost("/op/v1/device/set/flag", json);

                        Console.WriteLine("Response " + response);

                    }

                    return Json(chess);
                }
            }

            return StatusCode(404);

        }



    }
}
