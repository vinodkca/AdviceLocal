using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AdviceLocal.Api.BAL;
using AdviceLocal.Model;
using Newtonsoft.Json;

namespace AdviceLocal.Api {
    class Program {
       //Creates one instances of connection and avaids exhaust of ssockets
       public static void Main (string[] args) {
            try
            {                           
                //HttpService.InitializeService().GetAwaiter().GetResult();
                HttpService.InitializeService();
                int iClientTotal =  HttpService.GetClientTotal().GetAwaiter().GetResult();            
                Console.WriteLine( $"Recieved client total :  {iClientTotal}");
            
                //Insert into DB                               
                HttpService.InsertClient().GetAwaiter().GetResult();     
                Console.WriteLine( $"Inserted into DB client total :  {iClientTotal}");

                //Rename Table
                HttpService.clientService.RenameTable();
                Console.WriteLine( "Table Renamed in DB");
                Console.WriteLine( "Completed Successfully");

            }
            catch(Exception e)
            {
                throw e;
            }

        }    

    }
}