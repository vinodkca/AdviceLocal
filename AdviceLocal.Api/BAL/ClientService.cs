using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdviceLocal.Api.DAL;
using AdviceLocal.Model;

namespace AdviceLocal.Api.BAL
{
    public class RespClient{
        public string total { get; set; }
    }

     public class RespDataClient{
        public Client[] data { get; set; }
    }
    public class ClientService
    {
        // connection string to my SQL Server
        ClientRepository clientRepo = null;
        
        public ClientService()
        {
           clientRepo = new ClientRepository(AppSetting.SQL_DIAD);
        }
        
        public bool InsertClient(List<Client> lstClient)
        {
            return clientRepo.InsertCalls(lstClient);       
        }

        public bool TruncateTable(){
            return clientRepo.TruncateTable();
        }

        public void RenameTable()
        {
            clientRepo.RenameTable();       
        }
    }
}
