using ethosIQ_Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ethosIQ_NGCC_Shared.DAO
{
    public class NGCCSourceDAO
    {
        Database ConfigurationDatabase;

        public NGCCSourceDAO(Database ConfigurationDatabase)
        {
            this.ConfigurationDatabase = ConfigurationDatabase;
        }

        public List<NGCCSource> GetAllSources()
        {
            List<NGCCSource> NGCCSources = new List<NGCCSource>();

            if (ConfigurationDatabase != null)
            {
                string selectStatement = "SELECT * FROM NGCC_SOURCE";

                using (IDbConnection connection = ConfigurationDatabase.CreateConnection())
                {
                    using (IDbCommand getAllEnabledGenesysSitesCommand = ConfigurationDatabase.CreateCommand(selectStatement, connection))
                    {
                        connection.Open();

                        using (IDataReader reader = getAllEnabledGenesysSitesCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                /*
                                NGCCSources.Add(new NGCCSource(Convert.ToInt32(reader["NGCCSOURCEID"]), 
                                                               reader["NAME"].ToString(), 
                                                               reader["IPADDRESS"].ToString(),
                                                               Convert.ToInt32(reader["PORT"].ToString()),
                                                               reader["TENANTID"].ToString(),
                                                               reader["USERNAME"].ToString(),
                                                               reader["PASSWORD"].ToString(),
                                                               Convert.ToBoolean(reader["REALTIMEENABLED"]),
                                                               reader["REALTIMEURL"].ToString()));
                                                               */
                            }
                        }
                    }
                }
            }

            return NGCCSources;
        }

        public int Insert(NGCCSource NGCCSource)
        {
            int result = 0;

            if (ConfigurationDatabase != null)
            {
                List<NGCCSource> CurrentNGCCSources = new List<NGCCSource>();

                string selectORStatement = "SELECT * FROM NGCC_SOURCE WHERE NAME = '" + NGCCSource.Name + "'";

                using (IDbConnection connection = ConfigurationDatabase.CreateConnection())
                {
                    using (IDbCommand statement = ConfigurationDatabase.CreateCommand(selectORStatement, connection))
                    {
                        connection.Open();

                        IDataReader reader = statement.ExecuteReader();

                        while (reader.Read())
                        {
                            //CurrentFileTypes.Add(new NGCCSource(Convert.ToInt32(reader.GetValue(0)), reader.GetValue(1).ToString(), reader.GetValue(2).ToString()));
                        }
                    }
                }

                if (CurrentNGCCSources.Count == 0)
                {
                    string insertStatement = "INSERT INTO NGCC_SOURCE (NAME, IPADDRESS, PORT, TENANTID, USERNAME, PASSWORD) VALUES ('" + NGCCSource.Name + "','" + NGCCSource.IPAddress + "'," + NGCCSource.Port + ",'" + NGCCSource.TenantID + "','" + NGCCSource.Username + "','" + NGCCSource.Password + "')";

                    using (IDbConnection connection = ConfigurationDatabase.CreateConnection())
                    {
                        using (IDbCommand statement = ConfigurationDatabase.CreateCommand(insertStatement, connection))
                        {
                            connection.Open();

                            statement.ExecuteNonQuery();
                        }
                    }

                    /*
                    string selectStatement = "SELECT FILETYPEID FROM FILE_SOURCE WHERE NAME = '" + FileSource.Name + "'";

                    using (IDbConnection connection = ConfigurationDatabase.CreateConnection())
                    {
                        using (IDbCommand statement = ConfigurationDatabase.CreateCommand(selectStatement, connection))
                        {
                            connection.Open();

                            IDataReader reader = statement.ExecuteReader();

                            while (reader.Read())
                            {
                                if (reader.GetValue(0) == DBNull.Value)
                                {
                                    result = 0;
                                }
                                else
                                {
                                    result = Convert.ToInt32(reader.GetValue(0));
                                }
                            }
                        }
                    }
                    */
                }
            }

            return result;
        }

        public void Delete(string Name)
        {
            if (ConfigurationDatabase != null)
            {
                string deleteStatement = "DELETE FROM NGCC_SOURCE WHERE NAME = '" + Name + "'";

                using (IDbConnection connection = ConfigurationDatabase.CreateConnection())
                {
                    using (IDbCommand statement = ConfigurationDatabase.CreateCommand(deleteStatement, connection))
                    {
                        connection.Open();

                        statement.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
