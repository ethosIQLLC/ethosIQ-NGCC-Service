using ethosIQ_Database;
using System;
using System.Data;

namespace ethosIQ_NGCC_Shared.Installation
{
    public static class NGCCSourceTableInstallation
    {
        public static bool CreateNGCCSourceTable(Database ConfigurationDatabase)
        {
            bool serverExists = false;
            if (ConfigurationDatabase != null)
            {
                string checkForTableQuery = "SELECT count(*) AS \"SERVEREXISTS\" FROM sqlite_master " +
                                            "WHERE type= 'table' " +
                                            "AND name = 'NGCC_SOURCE'";

                string createTableQuery = "CREATE TABLE NGCC_SOURCE" +
                                                "(" +
                                                "NGCCSOURCEID INTEGER PRIMARY KEY AUTOINCREMENT," +
                                                "NAME TEXT," +
                                                "IPADRESS TEXT," +
                                                "PORT INTEGER," +
                                                "TENANTID TEXT," +
                                                "USERNAME TEXT," +
                                                "PASSWORD TEXT," +
                                                "REALTIMEENABLED INTEGER," +
                                                "REALTIMEURL INTEGER" +
                                                ")";

                using (IDbConnection connection = ConfigurationDatabase.CreateOpenConnection())
                {
                    using (IDbCommand findTableCommand = ConfigurationDatabase.CreateCommand(checkForTableQuery, connection))
                    {
                        using (IDataReader reader = findTableCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                serverExists = Convert.ToBoolean(reader["SERVEREXISTS"]);
                            }
                        }
                    }
                }

                if (!serverExists)
                {
                    using (IDbConnection connection = ConfigurationDatabase.CreateOpenConnection())
                    {
                        using (IDbCommand initialize = ConfigurationDatabase.CreateCommand(createTableQuery, connection))
                        {
                            initialize.ExecuteNonQuery();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool DropFileSourceTable(Database ConfigurationDatabase)
        {
            if (ConfigurationDatabase != null)
            {
                string dropTable = "DROP TABLE NGCC_SOURCE";

                using (IDbConnection connection = ConfigurationDatabase.CreateOpenConnection())
                {
                    using (IDbCommand dropTableCommand = ConfigurationDatabase.CreateCommand(dropTable, connection))
                    {
                        dropTableCommand.ExecuteNonQuery();

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
