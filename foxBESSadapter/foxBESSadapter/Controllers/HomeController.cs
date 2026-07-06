/*
 * CHESS adapter for Fox BESS
 * tim@toshiba-bril.com
 */

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Net.NetworkInformation;
using foxBESSadapter;
using System.Globalization;

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
        public Double cycleCost { get; set; }
        public Double capacityFade { get; set; }
        public Double soc { get; set; }
        public Double totalEnergy { get; set; }
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
            String responseString = "Error";

            try {
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

                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            } catch (Exception ex) {Console.WriteLine(ex.ToString());}
            return responseString;
        }

        // Estimate cycle cost using empirical degradation model
        protected Double cycleCost(Double SocMax, Double SocMin)
        {
            Console.WriteLine("Soc max " + SocMax + " Soc min " + SocMin);
            Double[] N  = {10000,9000,8000,7000,6000,5000,4000,3000,2000,1000};
            int i = (int)( (SocMax - SocMin) * 10 );
            Double total = 11460/28.2;
            Double res = total / (2 * N[i] * (SocMax - SocMin));
            return res;
        }

        /// <summary>
        /// Polling loop to update  CHESS DT  - It is not necessary if this is done in the MQTT telemetry handler (TBC) 
        /// </summary>
        /// 

        protected void polling(int asset)
        {


            Random rnd = new Random();
          
            Double totalEnergy = 50000;
            Double efficiency = 1;
            Double maxPower = 0;
            Double maxPower20 = 0;
            Double maxPower80 = 0;
            Double powerLimit = 0;
  
            CHESS chess = assets[asset];

       
            Double minSocOnGrid = 10;
            Double chargeTotal = totalEnergy * minSocOnGrid/100;
            Double dischargeTotal = 0;
            Double lastChargeTotal = chargeTotal;
            Double lastDischargeTotal = dischargeTotal;
            Double temperature = 20;

            Double capacityFade = 0;
                        
            while (true)
            {
                 try {
                    

                    String url = Program.urlprefix + "/aas/submodels/" + chess.id + "EnergyEntity/submodel-elements/$value";

                    Console.WriteLine("Getting energy entity from DT - " + url);
                    String result = Get(url, authToken);
                    Console.WriteLine("Got " + result);
            
                    DTData[] dtData = JsonConvert.DeserializeObject<DTData[]>(result);
                    Console.WriteLine("Data " + dtData.ToString());
                
                    if (dtData != null && dtData.Length > 0)
                    {
                        totalEnergy = dtLookup(dtData, "maximumAllowedBatteryEnergy");      
                        efficiency = dtLookup(dtData, "energyRoundtripEfficiency")/100;
                    } else
                        Console.WriteLine("Cannot get energy data from DT");


                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "PowerEntity/submodel-elements/$value";

                    Console.WriteLine("Getting power entity from DT - " + url);
                    result = Get(url, authToken);
                    Console.WriteLine("Got " + result);
            
                    dtData = JsonConvert.DeserializeObject<DTData[]>(result);

                    if (dtData != null && dtData.Length > 0)
                    {
                        maxPower = dtLookup(dtData, "maximumAllowedBatteryPower");      
                        maxPower20 = dtLookup(dtData, "powerCapabilityAt20Charge");
                        maxPower80 = dtLookup(dtData, "powerCapabilityAt80Charge");
                    
                    } else
                        Console.WriteLine("Cannot get power data from DT");       
/*
                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "StateOfBatteryEntity/submodel-elements/$value";

                    Console.WriteLine("Getting state of battery entity from DT - " + url);
                    result = Get(url, authToken);
                    Console.WriteLine("Got " + result);

                    dtData = JsonConvert.DeserializeObject<DTData[]>(result);

                    if (dtData != null && dtData.Length > 0)
                    {

                        minSocOnGrid =  dtLookup(dtData, "minSocOnGrid");        
                    
                    } else
                        Console.WriteLine("Cannot get SoC data from DT");       
*/                  
                    url = Program.urlprefix + "/aas/submodels/" + chess.id + "VoltageEntity/submodel-elements/$value";

                    Console.WriteLine("Getting voltage entity from DT - " + url);
                    result = Get(url, authToken);
                    Console.WriteLine("Got " + result);

                    dtData = JsonConvert.DeserializeObject<DTData[]>(result);

                    if (dtData != null && dtData.Length > 0)
                    {

                        capacityFade =  dtLookup(dtData, "capacityFade")/100;        
                    
                    } else
                        Console.WriteLine("Cannot get voltage entity data from DT");       
            
               
                    // check for status changes
                    int count = 0;                    
                    TimeSpan now = DateTime.Now.TimeOfDay;

                    if (chess.status != null)
                    foreach (Status status in chess.status) 
                    {
                        // see if there are any weekday / weekend changes !

                        Scheduler scheduler = new Scheduler();
                        scheduler.groups = new Schedule[chess.status.Length];
                        if (!status.status.Contains("available") && getStatus(status)) 
                        {
                            Schedule schedule = new Schedule();
                            schedule.workMode = status.status;
                            schedule.startHour = Int32.Parse(status.starttime.Substring(0, 2));
                            schedule.startMinute = Int32.Parse(status.starttime.Substring(3, 2));
                            schedule.endHour = Int32.Parse(status.endtime.Substring(0, 2));
                            schedule.endMinute = Int32.Parse(status.endtime.Substring(3, 2));

                            Double capacity = Double.Parse(status.capacity);
                            if (schedule.workMode.ToLower().Equals("forcecharge") && maxPower20 > 0 && maxPower80 > 0)
                                powerLimit =  maxPower80 + (maxPower20 - maxPower80) * ((chargeTotal - dischargeTotal)  / totalEnergy);              
                            else
                                powerLimit = maxPower;
                            if (powerLimit > maxPower) powerLimit = maxPower;

                            Console.WriteLine("Power limit is " + powerLimit);

                            schedule.enable = 1;

                            scheduler.groups[count] = schedule;
                            status.active = 1;

                            TimeSpan start = new TimeSpan(schedule.startHour, schedule.startMinute, 0); 
                            TimeSpan end = new TimeSpan(schedule.endHour, schedule.startMinute, 0);

                            Console.WriteLine("Status " + status.status + " " + status.starttime + " " + status.endtime + " " + schedule.startHour + ":" + schedule.startMinute + " " + schedule.endHour + ":" + schedule.endMinute);
                
                            if (start <= now && end > now)
                            {
                                Console.WriteLine("Schedule active " + count);
                                Double period = end.Subtract(start).TotalMinutes;
                                if (60*capacity/period > powerLimit) capacity = period*powerLimit/60;
                                Console.WriteLine("Capacity is " + capacity);

                                if (status.status.Contains("ForceDischarge") || status.status.Contains("Feedin") || status.status.Contains("SelfUse"))
                                {
                                    Console.WriteLine("Discharging");
                                  
                                    dischargeTotal += capacity/period;

                                    if (chargeTotal - dischargeTotal < totalEnergy * minSocOnGrid/100) dischargeTotal -= capacity/period;
                                    
                                  

                                } else if ( status.status.Contains("ForceCharge") )
                                {

                                    Console.WriteLine("Charging");
                                    
                                    chargeTotal += capacity/period;

                                    if (chargeTotal - dischargeTotal > totalEnergy) chargeTotal -= capacity/period;

                                }

                                
                               



                            }
                            count++;

                        } else 
                            status.active = 0;

                    
        
                    }


                
        

                    
                    if (chargeTotal + dischargeTotal > 0)
                    {
                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "StateOfBatteryEntity/submodel-elements/sme-" + chess.id + "stateOfCharge/invoke/$value";

                        String update = "{\"value\":" + Math.Round(100 * (chargeTotal - dischargeTotal)  / totalEnergy) + "}";

                        Console.WriteLine("Updating DT - " + url + " - " +update);

                        result = Post(url, update, authToken);

                        chess.soc =  (chargeTotal - dischargeTotal)  / totalEnergy;
                        chess.totalEnergy = totalEnergy;

                        Console.WriteLine(result);

                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "CDD/submodel-elements/sme-" + chess.id + "temperature/invoke/$value";

                        update = "{\"value\":" + temperature + "}";

                        Console.WriteLine("Updating DT - " + url + " - " +update);

                        result = Post(url, update, authToken);

                        Console.WriteLine(result);

                        url = Program.urlprefix + "/aas/submodels/" + chess.id + "telemetry/submodel-elements/sme-" + chess.id + "gridPower/invoke/$value";


                        Double powerFlow = (1 / efficiency) * 0.06 * ((chargeTotal - lastChargeTotal) - (dischargeTotal-lastDischargeTotal));

                        Console.WriteLine("Updating DT Powerflow - " + url + " - " + powerFlow.ToString() + " " + (chargeTotal-lastChargeTotal).ToString());

                        result = Post(url,  "{\"value\":" + powerFlow.ToString()  + "}", authToken);

                        lastDischargeTotal = dischargeTotal;
                        lastChargeTotal = chargeTotal;

                        Console.WriteLine(result);
                    }
                    else
                        Console.WriteLine("No charge data available");

                } catch (Exception ex) {Console.WriteLine(ex.ToString());}

                // 1 minute updates
                Thread.Sleep(60000);
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

                        Double maxCapacity = 0;
                        
                        foreach (Status status in chess.status)
                        {
                            if ( status.status != null && status.status.ToLower().Equals("forcedischarge") )
                            {
                                Double capacity = Double.Parse(status.capacity);
                                if (capacity > maxCapacity)
                                    maxCapacity = capacity;
                            }

                        }

                        //chess.cycleCost = cycleCost( chess.soc + maxCapacity/chess.totalEnergy, chess.soc );
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

                if (chess.id.EndsWith(id))

                    return Json(chess);
                
            }


            return StatusCode(404);

        }



    }
}