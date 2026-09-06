using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class TestAppConfigDecrypt
{
    private static byte[] keyDb = Encoding.UTF8.GetBytes("4693024482831831");

    public static string Decrypt(string cipherText, byte[] key)
    {
        if (string.IsNullOrEmpty(cipherText)) return "";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.Key = key;
            rijAlg.IV = key;
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    static void Main()
    {
        string rawEncrypted = ConfigurationManager.ConnectionStrings["TPSSDb"].ConnectionString;
        string decryptedConnStr = Decrypt(rawEncrypted, keyDb);

        Console.WriteLine("=== DESKTOP APP CONFIG CHECK ===");
        Console.WriteLine("Raw Config String: " + rawEncrypted);
        Console.WriteLine("Decrypted ConnStr: " + decryptedConnStr);

        try
        {
            using (SqlConnection con = new SqlConnection(decryptedConnStr))
            {
                con.Open();
                Console.WriteLine("\n[OK] Desktop App can successfully connect to Local Database!");
                using (SqlCommand cmd = new SqlCommand("SELECT DB_NAME() AS DB, @@SERVERNAME AS Server", con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("Connected Server:   " + reader["Server"]);
                            Console.WriteLine("Connected Database: " + reader["DB"]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n[FAIL] Connection error: " + ex.Message);
        }
    }
}
