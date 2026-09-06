using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Architecture.Common;

public class LicenseKey
{
	public static string ReturnKey(string Key)
	{
		try
		{
			string EncryptionKey = "AMIR2SPSCS44602";
			byte[] cipherBytes = Convert.FromBase64String(Key);
			using Aes encryptor = Aes.Create();
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[13]
			{
				73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
				100, 101, 118
			});
			encryptor.Key = pdb.GetBytes(32);
			encryptor.IV = pdb.GetBytes(16);
			using MemoryStream ms = new MemoryStream();
			using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cs.Write(cipherBytes, 0, cipherBytes.Length);
				cs.Close();
			}
			return Encoding.Unicode.GetString(ms.ToArray());
		}
		catch (Exception)
		{
			throw new Exception("License Key Not Match...");
		}
	}
}
