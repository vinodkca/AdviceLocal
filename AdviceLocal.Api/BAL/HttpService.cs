using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AdviceLocal.Api.BAL;
using AdviceLocal.Model;
using Newtonsoft.Json;

namespace AdviceLocal.Api.BAL {
    public static class HttpService {
        private static HttpClient client;
        public static ClientService clientService;        

        static HttpService() {
            //Static constructor
            client = new HttpClient ();      
            clientService = new ClientService();                    
        }

        //public static async Task InitializeService () {
        public static void InitializeService () {
            //Generate URL   
            client.BaseAddress = new Uri (AppSetting.CLIENT_URL);
            client.DefaultRequestHeaders.Clear ();
            client.DefaultRequestHeaders.Add ("x-api-token","pk_936b12f10592c57eb0c4df4517b4d896");
            client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

     

            //Console.WriteLine( $"Completed GetLines :  {ShowLineInfo().Result}");
            //Console.WriteLine(ShowLineInfo().GetAwaiter().GetResult()); //Console.WriteLine( $"{ShowLineInfo().Result}");
        }

        #region API calls
        public static async Task<int> GetClientTotal() {
            int iRows = -1;
            string strClientTotal = String.Format(AppSetting.clientLimit,0);

            HttpResponseMessage resp = await client.GetAsync(strClientTotal);
            if (resp.IsSuccessStatusCode) {

                Stream received = await resp.Content.ReadAsStreamAsync ();
                StreamReader readStream = new StreamReader (received);
                string jsonString = readStream.ReadToEnd ();
                //Console.WriteLine ("{0}", jsonString);
                
                RespClient respClient = JsonConvert.DeserializeObject<RespClient> (jsonString);   
               iRows = Convert.ToInt32( respClient.total);
              
            }
            return iRows;
        }

         public static async Task<int> InsertClient() {
            int iTotalClient = -1;
            string strClientTotal = String.Format(AppSetting.clientLimit,0);

            HttpResponseMessage resp = await client.GetAsync(strClientTotal);
            if (resp.IsSuccessStatusCode) {

                Stream received = await resp.Content.ReadAsStreamAsync ();
                StreamReader readStream = new StreamReader (received);
                string jsonString = readStream.ReadToEnd ();
                //Console.WriteLine ("{0}", jsonString);
                
                RespClient respClient = JsonConvert.DeserializeObject<RespClient> (jsonString);   
                iTotalClient = Convert.ToInt32( respClient.total);
                if(iTotalClient > 0){
                    clientService.TruncateTable();
                    for(int iRow = 0; iRow <= iTotalClient; iRow = iRow + 1000 ) //Max rows returnsed is 1000 per call
                    {
                        await InsertClientRange(1000, iRow);    
                        Console.WriteLine( "Rows inserted " + iRow );
                        //if(iRow == 1000)break;;
                    }
                }
            }

            return iTotalClient;
        }


        private static async Task<List<Client>> InsertClientRange (int iLimit, int iSkip) {
            List<Client> lstClient = null;
            string strClientLimitSkip = String.Format(AppSetting.clientLimitSkip, iLimit, iSkip);
            Console.WriteLine( strClientLimitSkip);
            HttpResponseMessage resp = await client.GetAsync (strClientLimitSkip);
            if (resp.IsSuccessStatusCode) {

                Stream received = await resp.Content.ReadAsStreamAsync ();
                StreamReader readStream = new StreamReader (received);
                string jsonString = readStream.ReadToEnd ();
                RespDataClient respDataClient = JsonConvert.DeserializeObject<RespDataClient> (jsonString);   
                lstClient = new List<Client>(respDataClient.data);

                string strInsertCalls =  clientService.InsertClient(lstClient) ? $"Inserted {lstClient.Count} in STG_Client table" : "Failed to insert calls in STG_Client table";
                Console.WriteLine( strInsertCalls);
            
            }

            return lstClient;
        }
        #endregion
   }
}