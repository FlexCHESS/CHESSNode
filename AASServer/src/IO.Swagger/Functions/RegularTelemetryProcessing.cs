
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CAL_Api;
using System.Transactions;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.IO;
using Azure.DigitalTwins.Core;
using Azure;
using System.Reflection.Metadata;
using Azure.Identity;
using Microsoft.Azure.Devices;
using IoT.Services;

namespace IoT.Functions
{
 

    public  class DTProcessing
    {
       
        private static DigitalTwinsClient dtClient;



        static async Task   getDT(String dtid)
        {


            try
            {
                Response<SubmodelElement> twinResponse = await dtClient.GetDigitalTwinAsync<SubmodelElement>(dtid);
                SubmodelElement existingDigitalTwin = twinResponse.Value;
                Console.WriteLine("Got " + existingDigitalTwin.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        static DTProcessing()
        {

            CAL.registerLu(CAL_Api.LuRole_t.CAL_ROLE_TRUSTED_LU, "CHESS", "Toshiba Europe Ltd", "1.0", "dbname=flexchess user=postgres password=gollum");
            CAL.calNotification += new EventHandler<CalEventArgs>(notificationHandler);
            CAL.requestNotify("SELECT * from public.real_measure order by id; archive=1", 0, 0);


            string _adtServiceUrl = Environment.GetEnvironmentVariable("adtServiceUrl", EnvironmentVariableTarget.Process);
            string _adtClientId =  Environment.GetEnvironmentVariable("adtClientId", EnvironmentVariableTarget.Process);
            string _adtClientSecret = Environment.GetEnvironmentVariable("adtClientSecret", EnvironmentVariableTarget.Process);
            string _adtTenantId = Environment.GetEnvironmentVariable("adtTenantId", EnvironmentVariableTarget.Process);



            ClientSecretCredential credential = new ClientSecretCredential(_adtTenantId, _adtClientId, _adtClientSecret);

//            var credential = new DefaultAzureCredential();
            dtClient = new DigitalTwinsClient(new Uri(_adtServiceUrl), credential);
            Console.WriteLine($"Service client created – ready to go");


            

        }
        public static async void notificationHandler(object e, CalEventArgs args)
        {
            // here we have a callback from the REM
            int nt;
     
            if (args.rnId == 1)
            {


               

                // now all the MCD / device related data

                CAL.getInfo("SELECT * from public.real_measure where lastvalue != value order by id");

                nt = CAL.resultNumTuples();
                
                for (int i = 0; i < nt; i++)
                {
                    var updateTwinData = new JsonPatchDocument();
                    String modelId = CAL.resultStringValue(1);
                    String deviceId = CAL.resultStringValue(2);
                    Console.WriteLine("Notification " + modelId  + deviceId);

                    await getDT(modelId + deviceId);

                    //for (int j = 4; j < CAL.resultNumFields(); j++) {
              

                    string key = "/value";
                    string value = CAL.resultStringValue(5);
                    updateTwinData.AppendReplace(key, value); // Convert.ToDouble(CAL.resultStringValue(5)));
                    

            
                    //}

                    Response response = dtClient.UpdateDigitalTwin(modelId+deviceId, updateTwinData);


                    Console.WriteLine("Response " + response.Status.ToString());

                    CAL.resultNextTuple();
                }
                CAL.resultFree();
            }
        }

    }
}
