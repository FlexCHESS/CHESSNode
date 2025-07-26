/*
 * CHESS adapter for simulated HVAC
 * tim@toshiba-bril.com
 */

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Net.NetworkInformation;
using hvacadapter;
using System.Globalization;

namespace hvacadapter.Controllers
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

    // Weather service data
    public class Units 
    {
        public String time {get; set;}
        public String interval { get; set; }
        public String temperature_2m {get; set;}

    }

    public class Current 
    {
        public String time { get; set;}
        public Double interval { get; set;}
        public Double temperature_2m {get; set;}

    }

    public class Hourly_units
    {
        public String time { get; set;}
        public String temperature_2m {get; set;}

    }

    public class Hourly
    {
        public String[] time {get; set;}
        public Double[] temperature_2m {get; set;} 
    }

    public class Weather
    {
        public String latitude {get; set;}
        public String longitude {get; set;}
        public String generationtime_ms {get; set;}
        public String utc_offset_seconds {get; set;}
        public String timezone {get; set;}
        public String  timezone_abbreviation {get; set;}
        public Double elevation {get; set;}
        public Units current_units {get; set;}
        public Current current {get; set;}
        public Hourly_units hourly_units {get; set;}
        public Hourly hourly {get; set;}
   
    }

    /// <summary>
    ///  Controller class for handling the requests (provided APIs) for the CHESS Node / Network Core  
    /// </summary>
    public class HomeController : Controller
    {

        protected static List<CHESS> assets;
        protected static String authToken = null;
        private readonly ILogger<HomeController> _logger;
  
        public Double dtLookup(DTData[] dtData, String id)
        {

            foreach (DTData data in dtData)
            if (data.Id.EndsWith(id)) return data.Value;

            return 0;
        }

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


        
          
            Double totalEnergy = 50000;
      
            Double pvPower = 0;
            Double[] hvacPower = {0,0,0};
            Double powerLimit = 150000;

            CHESS chess = assets[asset];

            Double chargeTotal = 0;
            Double dischargeTotal = 0;
            Double[] temperature = {293, 293, 293};
            Double[] lastTemperature = {293, 293, 293};    
            Double[] lastNextTemperature = {293, 293, 293};    
            Double ta = 0;        
            Double alpha = 0.3;
            Double beta = 0.0568;
            Double ksun = 0.9256;
            Double kint = 0.9691;
            Double khvac = 1.0772;
     
            while (true)
            {

         

                try {
                    
                    // check for status changes
                    Console.WriteLine("Asset - " + asset + " " + chess.identifier);
                    int count = 0;

                    String url = Program.urlprefix + "/aas/submodels/" + chess.id.Replace("-sim","") + "telemetry/submodel-elements/$value";

                    Console.WriteLine("Getting energy entity from DT - " + url);
                    String result = Get(url, authToken);
                    Console.WriteLine("Got " + result);
            
                    DTData[] dtData = JsonConvert.DeserializeObject<DTData[]>(result);
                    Console.WriteLine("Data " + dtData.ToString());
                
                    if (dtData != null && dtData.Length > 0)
                    {
                        pvPower = dtLookup(dtData, "MRB_GN_FV_Wsys");      
                        hvacPower[0] = dtLookup(dtData, "MRA_P8_CDZ_Wsys");
                        hvacPower[1] = dtLookup(dtData, "MRC_GN_CDZ_Wsys");
                        hvacPower[2] = dtLookup(dtData, "MRD_GN_CDZ_Wsys");
                    } else
                        Console.WriteLine("Cannot get telemmetry data from DT");

                    Console.WriteLine("CHESS location " + chess.location);

                    string[] location = chess.location.Split(' ');

                    result = Get("https://api.open-meteo.com/v1/forecast?latitude=" + location[0] + "&longitude=" + location[1] +"&hourly=temperature_2m&current=temperature_2m&forecast_days=1", "none");                  

                    Console.WriteLine ("Got " + result);
                    Weather weather = JsonConvert.DeserializeObject<Weather>(result);

                    // Use the current temperature 
                    if (weather != null)
                        ta = weather.current.temperature_2m + 273;
                    else 
                        Console.WriteLine("Cannot get current temp");

                    for (int i=0; i<3; i++)
                        temperature[i] = (1-0.001*alpha) * lastTemperature[i] + 0.001*(alpha * ta + ksun * beta * pvPower + khvac * beta * hvacPower[i] );

               
                    TimeSpan now = DateTime.Now.TimeOfDay;
                    List<Double> nextTemperature = new List<Double>();
                    List<Double> endTemperature = new List<Double>();
                    if (chess.status != null)
                    foreach (Status status in chess.status) 
                    {
                        // see if there are any weekday / weekend changes !

                        Scheduler scheduler = new Scheduler();
                        scheduler.groups = new Schedule[chess.status.Length];
                        if (!status.status.Contains("available")) 
                        {
                            Schedule schedule = new Schedule();
                            schedule.workMode = status.status;
                            schedule.startHour = Int32.Parse(status.starttime.Substring(0, 2));
                            schedule.startMinute = Int32.Parse(status.starttime.Substring(3, 2));
                            schedule.endHour = Int32.Parse(status.endtime.Substring(0, 2));
                            schedule.endMinute = Int32.Parse(status.endtime.Substring(3, 2));
                            
                            Double capacity = Double.Parse(status.capacity);



                            schedule.enable = 1;
                            scheduler.groups[count] = schedule;
                            status.active = 1;


                            TimeSpan start = new TimeSpan(schedule.startHour, schedule.startMinute, 0); 
                            TimeSpan end = new TimeSpan(schedule.endHour, schedule.endMinute, 0);

                            Console.WriteLine("Status " + status.status + " " + status.starttime + " " + status.endtime + " " + schedule.startHour + ":" + schedule.startMinute + " " + schedule.endHour + ":" + schedule.endMinute);
                
                            if (start <= now && end > now)
                            {
                                Console.WriteLine("Schedule active " + count);
                                Double period = end.Subtract(start).TotalMinutes;
                                // Use the forecast temp at end of the period to compare to see if this will impact indoor temperature more
                                Console.WriteLine("Time at end is " + schedule.endHour);
                                Double tempAtEnd = weather.hourly.temperature_2m[ schedule.endHour ] + 273;

 

                                Console.WriteLine("Capacity is " + capacity);

                                if (status.status.Contains("ForceDischarge") || status.status.Contains("Feedin") || status.status.Contains("SelfUse"))
                                {
                                    Console.WriteLine("Discharging");
                                  


                                    dischargeTotal += capacity/period;


                                    for (int i=0; i<3; i++)      
                                    { 
                                      hvacPower[i]-=0.001*capacity/period;                    
                                      nextTemperature.Add( (1-0.001*alpha) * lastNextTemperature[i] + 0.001*(alpha * ta + ksun * beta * pvPower + khvac * beta * hvacPower[i]) );
                                      endTemperature.Add( (1-0.001*alpha) * lastNextTemperature[i] + 0.001*(alpha * tempAtEnd + ksun * beta * pvPower + khvac * beta * hvacPower[i]) );

                                    }

         ;
                                } else if ( status.status.Contains("ForceCharge") )
                                {

                                    Console.WriteLine("Charging");
                                    
                                    chargeTotal += capacity/period;

                                    for (int i=0; i<3; i++)      
                                    { 
                                      hvacPower[i]+=0.001*capacity/period;                    
                                      nextTemperature.Add( (1-0.001*alpha) * lastNextTemperature[i] + 0.001*(alpha * ta + ksun * beta * pvPower + khvac * beta * hvacPower[i]) );
                                      endTemperature.Add( (1-0.001*alpha) * lastNextTemperature[i] + 0.001*(alpha * tempAtEnd + ksun * beta * pvPower + khvac * beta * hvacPower[i]) );

                                    }
 
                                }


   
                            }
                            count++;

                        } else 
                            status.active = 0;

        
                    }
          
                    Console.WriteLine ("Updating ...");

                     

                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "temperature/invoke/$value";

                    String update = "{\"value\":\"[" + (temperature[0]-273) + "," + (temperature[1]-273) + "," + (temperature[2]-273) + "," + weather.current.temperature_2m + "]\"}";

                    for (int i=0; i<3; i++)
                        lastTemperature[i] = temperature[i];

                    Console.WriteLine("Updating DT - " + url + " - " +update);

                    result = Post(url, update, authToken);

                    Console.WriteLine(result);

                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "nextTemperature/invoke/$value";

                    
                    update = "{\"value\":\"[";
                    for (int i=0; i<nextTemperature.Count; i++) 
                        update += (nextTemperature[i]-273) + ",";
                    update += weather.current.temperature_2m; //update.Trim(',');
                    update +=  "]\"}";

                    if (nextTemperature.Count >= 3)
                    for (int i=0; i<3; i++)
                        lastNextTemperature[i] = nextTemperature[i];

                    Console.WriteLine("Updating DT - " + url + " - " +update);

                    result = Post(url, update, authToken);

                    Console.WriteLine(result);
                    
                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "endTemperature/invoke/$value";

                    
                    update = "{\"value\":\"[";
                    for (int i=0; i<endTemperature.Count; i++) 
                        update += (endTemperature[i]-273) + ",";
                    update += weather.current.temperature_2m;
                    update +=  "]\"}";

                   
                    Console.WriteLine("Updating DT - " + url + " - " +update);

                    result = Post(url, update, authToken);

                    Console.WriteLine(result);

                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "MRA_P8_CDZ_Wsys/invoke/$value";

                    update = "{\"value\":" + hvacPower[0] + "}";

                    Console.WriteLine("Updating DT - " + url + " - " +update);

                    result = Post(url, update, authToken);

                    Console.WriteLine(result);

                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "MRC_CDZ_Wsys/invoke/$value";

                    update = "{\"value\":" + hvacPower[1] + "}";

                    Console.WriteLine("Updating DT - " + url + " - " +update);

                    result = Post(url, update, authToken);

                    Console.WriteLine(result);


                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "MRD_CDZ_Wsys/invoke/$value";

                    update = "{\"value\":" + hvacPower[2] + "}";

                    Console.WriteLine("Updating DT - " + url + " - " +update);

                    result = Post(url, update, authToken);

                    Console.WriteLine(result);
                    
                    
                } catch (Exception ex) { Console.WriteLine(ex);}

                // 3 minute updates
                Thread.Sleep(180000);
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

            Console.WriteLine("Body " + Newtonsoft.Json.JsonConvert.SerializeObject(body));

            assets.Add(body);


            Task.Run(() => polling(assets.Count-1));

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

                if (chess.id.EndsWith(id))
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
