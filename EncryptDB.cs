using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class EncryptDB
{
    private static byte[] keyDb = Encoding.UTF8.GetBytes("4693024482831831");

    public static string Encrypt(string plainText, byte[] key)
    {
        if (string.IsNullOrEmpty(plainText)) return "";
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Mode = CipherMode.CBC;
            rijAlg.Padding = PaddingMode.PKCS7;
            rijAlg.Key = key;
            rijAlg.IV = key;
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    csEncrypt.FlushFinalBlock();
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

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

    static void Main(string[] args)
    {
        string connStr = @"Data Source=MUHAMMADASIF-PC\SQLEXPRESS;Initial Catalog=GoldenAcedb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
        if (args.Length > 0)
        {
            connStr = args[0];
        }
        string encrypted = Encrypt(connStr, keyDb);
        Console.WriteLine("Connection String: " + connStr);
        Console.WriteLine("Encrypted TPSSDb:  " + encrypted);
        Console.WriteLine("Decrypted Verify:  " + Decrypt(encrypted, keyDb));
    }
}
