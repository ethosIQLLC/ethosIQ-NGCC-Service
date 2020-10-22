using ethosIQ_Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ethosIQ_NGCC_Shared.DAO
{
    public class TranslationDAO
    {
        private Database CollectionDatabase;

        public TranslationDAO(Database CollectionDatabase)
        {
            this.CollectionDatabase = CollectionDatabase;
        }

        public List<Translation> GetTranslation()
        {
            List<Translation> AllAgentIDs = new List<Translation>();

            if (CollectionDatabase != null)
            {
                string getAllAgentIDstatement = @"SELECT * FROM TBL_NGCC_TRANSLATE";

                using (IDbConnection connection = CollectionDatabase.CreateConnection())
                {
                    using (IDbCommand getAllAgentIDsCommand = CollectionDatabase.CreateCommand(getAllAgentIDstatement, connection))
                    {
                        connection.Open();

                        using (IDataReader reader = getAllAgentIDsCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    AllAgentIDs.Add(new Translation(reader["NGCC"] == null ? "null" : reader["NGCC"].ToString(), reader["TVID"] == null ? "null" : reader["TVID"].ToString()));
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                }
            }

            return AllAgentIDs;
        }
    
    }
}
