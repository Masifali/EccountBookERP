using System;
using System.Data.SqlClient;

class TestConnection
{
    static void Main()
    {
        string connStr = @"Data Source=MUHAMMADASIF-PC\SQLEXPRESS;Initial Catalog=GoldenAcedb;User ID=sa;Password=sql;Encrypt=True;TrustServerCertificate=True;";
        Console.WriteLine("Testing connection to: " + connStr);
        try
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                Console.WriteLine("STATUS: SUCCESS - Connection opened successfully!");
                using (SqlCommand cmd = new SqlCommand("SELECT DB_NAME() AS DB, @@SERVERNAME AS Server", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Server Name: " + reader["Server"]);
                            Console.WriteLine("Database:    " + reader["DB"]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("STATUS: FAILED - " + ex.Message);
        }
    }
}
