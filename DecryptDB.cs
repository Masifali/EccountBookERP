using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class DecryptDB
{
    private static byte[] keyDb = Encoding.UTF8.GetBytes("4693024482831831");
    private static byte[] keyUser = Encoding.UTF8.GetBytes("3024482831469469");

    public static string Decrypt(string cipherText, byte[] key)
    {
        if (string.IsNullOrEmpty(cipherText)) return "";
        try
        {
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
        catch (Exception ex)
        {
            return "[Decryption Error: " + ex.Message + "]";
        }
    }

    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            string input = args[0].Trim();
            Console.WriteLine("=== DECRYPTING INPUT STRING ===");
            Console.WriteLine("Input: " + input);
            Console.WriteLine("DB/Config Key  (4693024482831831): " + Decrypt(input, keyDb));
            Console.WriteLine("User Pass Key  (3024482831469469): " + Decrypt(input, keyUser));
            return;
        }

        string tpss = "31shbmRUY1uONbSEm6Ae9AKjZy3p8Iuzoa17DpMwPE8uXTZkYX36xEraosDrCx+3xGUc0PUXlxroFgdYlbwSQzSO4vKkvpXCtQi16SLkF3qJCTuL+n/v3n+1TMZEp9cdAouaURZy/xpU1Nmo8Ke9a0QLz6OOqhOy6DQmEJrrKeE=";
        string vpsName = "4VctcCutg6yDpje25dkF9A==";
        string vpsUser = "v/DL1vHFEdEiDrmrRGNN9g==";
        string vpsPwd  = "Xe0KYuiObJCdxo82JNav2g==";

        Console.WriteLine("=== DECRYPTED DATABASE CONNECTION DETAILS ===");
        Console.WriteLine("Decrypted TPSSDb Connection String:");
        Console.WriteLine(Decrypt(tpss, keyDb));
        Console.WriteLine();
        Console.WriteLine("VPS Server Name / IP: " + Decrypt(vpsName, keyDb));
        Console.WriteLine("VPS User:            " + Decrypt(vpsUser, keyDb));
        Console.WriteLine("VPS Password:        " + Decrypt(vpsPwd, keyDb));
    }
}
