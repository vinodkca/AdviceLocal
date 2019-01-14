using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using AdviceLocal.Api.Interface;
using AdviceLocal.Model;
using System.Threading.Tasks;

namespace AdviceLocal.Api.DAL
{
    public class ClientRepository : Repository, IClientRepository
    {
         private static IDbConnection db = null;

        public ClientRepository(string strConn)
        {
            db = new SqlConnection(strConn);
        } 

        public bool InsertCalls(List<Client> lstClient)
        {
            return BulkCopy<Client>(db as SqlConnection, "STG_Client", lstClient);
        }

        public bool TruncateTable()
        {
           return TruncateTable(db as SqlConnection,"STG_Client");
        }

        public bool RenameTable()
        {   
            return RenameTable(db as SqlConnection);
        }
     }
}
