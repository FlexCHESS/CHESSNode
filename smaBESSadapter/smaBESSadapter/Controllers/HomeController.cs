/*
 * CHESS adapter for SMA BESS
 * tim@toshiba-bril.com
 */

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Net.NetworkInformation;
using smaBESSadapter;
using System.Globalization;

namespace smaBESSadapter.Controllers
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
        public String plantID { get; set; }
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
        public Int32 Errno { get; set; }
        public DeviceList Result { get; set; }
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

    // Real-time data from devices
    public class RealData
    {


        public String variable { get; set; }
        public String unit { get; set; }
        public String name { get; set; }
        public Double value { get; set; }
        public String time { get; set; }

    }

    // DT Data
    public class DTData
    {

        public String Id {get; set;}
        public Double Value {get; set;}
    }

    // Real-time data response
    public class RealDataResponse
    {

        public Int32 errno { get; set; }
        public RealData[] result { get; set; }

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
    }

    // Set of schedules for a scheduler
    public class Scheduler
    {

        public String deviceSN { get; set; }
        public String enable { get; set; }
        public Schedule[] groups { get; set; }
    }

    public class SchedulerResponse
    {
        public Int32 errno { get; set; }
        public String Msg { get; set; }
        public Scheduler Result { get; set; }
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

    /// <summary>
    ///  Controller class for handling the requests (provided APIs) for the CHESS Node / Network Core  
    /// </summary>
    public class HomeController : Controller
    {

        protected static List<CHESS> assets;
        protected static String authToken = null;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            if (assets == null)
                assets = new List<CHESS>();
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


        /// <summary>
        /// Polling loop to update  CHESS DT  - It is not necessary if this is done in the MQTT telemetry handler (TBC) 
        /// </summary>
        /// 

        protected void polling(int asset)
        {


            CHESS chess = assets[asset];

            Random rnd = new Random();
          
            Double totalEnergy = rnd.Next(1,100);
     
          
            String url = Program.urlprefix + "/aas/submodels/" + chess.id + "EnergyEntity/submodel-elements/sme-" + chess.id + "maximumAllowedBatteryEnergy/$value";

            Console.WriteLine("Getting max energy from DT - " + url);

            String result = Get(url, authToken);

            Console.WriteLine(result);

            DTData[] dtData = (DTData[])JsonConvert.DeserializeObject(result);

            

            while (true)
            {

                Double chargeTotal = 0;
                Double dischargeTotal = 0;
                Double temperature = 0;

                try {

                    // check for status changes
                    foreach (Status status in chess.status) 
                    {
                        // see if there are any weekday / weekend changes !
                        int count = 0;
                        Scheduler scheduler = new Scheduler();
                        scheduler.groups = new Schedule[chess.status.Length];
                        if (!status.service.Contains("available") && getStatus(status)) 
                        {
                            Schedule schedule = new Schedule();
                            schedule.workMode = status.service;
                            schedule.startHour = Int32.Parse(status.starttime.Substring(0, 2));
                            schedule.startMinute = Int32.Parse(status.starttime.Substring(3, 2));
                            schedule.endHour = Int32.Parse(status.endtime.Substring(0, 2));
                            schedule.endMinute = Int32.Parse(status.endtime.Substring(3, 2));


                            schedule.fdSoc = Int32.Parse(status.capacity);
                            schedule.enable = 1;

                            scheduler.groups[count] = schedule;
                            status.active = 1;

                            count++;

                        } else 
                            status.active = 0;

                        scheduler.deviceSN = chess.deviceData.deviceSN;
        
                    }


                
                    chargeTotal += rnd.Next(1,100);    
                    dischargeTotal = 1000-chargeTotal;
                    temperature = rnd.Next(10,30);

                    
                    if (dtData != null && dtData.Length > 0)
                    {
                        url = Program.urlprefix + "/aas/submodels/"+chess.id+"StateOfBatteryEntity/submodel-elements/sme-" + chess.id + "stateOfCharge/invoke/$value";

                        String update = "{\"value\":" + Math.Round((chargeTotal - dischargeTotal) / dtData[0].Value) + "}";

                        Console.WriteLine("Updating DT - " + url + " - " +update);

                        result = Post(url, update, authToken);

                        Console.WriteLine(result);

                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "CDD/submodel-elements/sme-" + chess.id + "temperature/invoke/$value";

                        update = "{\"value\":" + temperature + "}";

                        Console.WriteLine("Updating DT - " + url + " - " +update);

                        result = Post(url, update, authToken);

                        Console.WriteLine(result);

                    }
                    else
                        Console.WriteLine("No charge data available");

                } catch (Exception ex) {Console.WriteLine(ex.ToString());}

                // 15 minute updates
                Thread.Sleep(900000);
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

            Console.WriteLine("Configure MQTT for - " + body.identifier);

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "bash";

            startInfo.Arguments = "deploy.sh " + body.id + " " + body.location;
            process.StartInfo = startInfo;
            process.Start();
            Console.WriteLine(process);

            assets.Add(body);

            Task.Run(() => polling(assets.Count - 1));
      
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

                if (chess.id.Equals(id))
                    if (body != null)
                    {

                         // we need to update !
                        chess.currentStatus = body.currentStatus;
                        chess.status = body.status;

                        return Json(chess);

                    }

            }
            return StatusCode(404);
        }

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

                if (chess.id.Equals(id))

                    return Json(chess);
                
            }


            return StatusCode(404);

        }


    }
}
