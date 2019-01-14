using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace AdviceLocal.Api.BAL
{
    public class AppSetting{
        // Adding JSON file into IConfiguration.
        public static readonly IConfiguration config; 
        public static readonly string SQL_DIAD;
        public static readonly int CLIENT_LIMIT; 
        public static readonly int CLIENT_SKIP; 
        
        //Initialize all static variable  after const have been called. Static constructor is called only once
        static AppSetting(){
            config = new ConfigurationBuilder()
                        .AddJsonFile("AppConfig.json",  optional: true, reloadOnChange: true)
                        .Build();  
            SQL_DIAD = config["DBConnection:SQLDIAD"]; 
            CLIENT_LIMIT =  Convert.ToInt32(config["Client:Limit"]); 
            CLIENT_SKIP =  Convert.ToInt32(config["Client:Skip"]);             

        }

        //CONST are called before static constructor
        public const string CLIENT_URL = "http://p.lssdev.com/legacyclients";
        
        //Client DETAILS
        public static readonly string clientLimit = $"{CLIENT_URL}?limit={{0}}";
        public static readonly string clientLimitSkip =  $"{CLIENT_URL}?limit={{0}}&skip={{1}}";        
        
    }

}

