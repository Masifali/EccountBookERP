using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Architecture.Model;

public static class Encryption
{
	private const string EncryptKey = "4693024482831831";

	private const string UserPasswordEncryptKey = "3024482831469469";

	public static string EncryptString(string textForEncrypt)
	{
		try
		{
			return Convert.ToBase64String(EncryptStringToBytes(textForEncrypt, Encoding.UTF8.GetBytes("4693024482831831"), Encoding.UTF8.GetBytes("4693024482831831")));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static string DecryptString(string textForDecrypt)
	{
		try
		{
			return DecryptStringFromBytes(Convert.FromBase64String(textForDecrypt), Encoding.UTF8.GetBytes("4693024482831831"), Encoding.UTF8.GetBytes("4693024482831831"));
		}
		catch (Exception)
		{
			return null;
		}
	}

	private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
	{
		if (plainText == null || plainText.Length <= 0)
		{
			throw new ArgumentNullException("plainText");
		}
		if (key == null || key.Length == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (iv == null || iv.Length == 0)
		{
			throw new ArgumentNullException("key");
		}
		using RijndaelManaged rijAlg = new RijndaelManaged();
		rijAlg.Mode = CipherMode.CBC;
		rijAlg.Padding = PaddingMode.PKCS7;
		rijAlg.FeedbackSize = 128;
		rijAlg.Key = key;
		rijAlg.IV = iv;
		ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
		using MemoryStream msEncrypt = new MemoryStream();
		using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
		using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
		{
			swEncrypt.Write(plainText);
		}
		return msEncrypt.ToArray();
	}

	private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
	{
		if (cipherText == null || cipherText.Length == 0)
		{
			throw new ArgumentNullException("cipherText");
		}
		if (key == null || key.Length == 0)
		{
			throw new ArgumentNullException("key");
		}
		if (iv == null || iv.Length == 0)
		{
			throw new ArgumentNullException("key");
		}
		string plainText = null;
		using RijndaelManaged rijAlg = new RijndaelManaged();
		rijAlg.Mode = CipherMode.CBC;
		rijAlg.Padding = PaddingMode.PKCS7;
		rijAlg.FeedbackSize = 128;
		rijAlg.Key = key;
		rijAlg.IV = iv;
		ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
		using MemoryStream msDecrypt = new MemoryStream(cipherText);
		using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
		using StreamReader srDecrypt = new StreamReader(csDecrypt);
		return srDecrypt.ReadToEnd();
	}

	public static string UserPasswordEncryptString(string textForEncrypt)
	{
		try
		{
			return Convert.ToBase64String(EncryptStringToBytes(textForEncrypt, Encoding.UTF8.GetBytes("3024482831469469"), Encoding.UTF8.GetBytes("3024482831469469")));
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static string UserPasswordDecryptString(string textForDecrypt)
	{
		try
		{
			return DecryptStringFromBytes(Convert.FromBase64String(textForDecrypt), Encoding.UTF8.GetBytes("3024482831469469"), Encoding.UTF8.GetBytes("3024482831469469"));
		}
		catch (Exception)
		{
			return null;
		}
	}
}
