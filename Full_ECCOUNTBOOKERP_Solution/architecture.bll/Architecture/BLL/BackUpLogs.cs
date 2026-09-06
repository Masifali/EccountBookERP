using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using Architecture.Common;
using Architecture.DAL;
using Architecture.DAL.Common;
using Architecture.Model;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Architecture.BLL;

public class BackUpLogs
{
	private static string BackUpFolderPath = "";

	public static int Save(Architecture.Model.BackUpLogs obj)
	{
		try
		{
			obj.EntryDate = DateTime.Now;
			return new Architecture.DAL.BackUpLogs().SetData(obj, "USP_BackUpLogs_Insert");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool BackUpLogsExists(int BackupTypeId, DateTime TodayDate)
	{
		try
		{
			List<SqlParameter> p = new List<SqlParameter>();
			p.Add(new SqlParameter
			{
				ParameterName = "@BackupTypeId",
				Value = BackupTypeId
			});
			return GenericProvider.GetDataTableProc<Architecture.Model.BackUpLogs>("USP_BackUpLogsExistsOrNot", p).Rows.Count > 0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void RunDatabaseBackup(string connectionString, string backupDirectory, BackgroundWorker worker = null, string Terminal01Path = null, string Terminal02Path = null)
	{
		try
		{
			if (string.IsNullOrEmpty(backupDirectory) || !Directory.Exists(backupDirectory))
			{
				throw new Exception("Database BackUp Folder path does not exists on the current network.");
			}
			using SqlConnection con = new SqlConnection(connectionString);
			con.Open();
			string databaseName = con.Database.ToString();
			string password = "scs@_2024";
			Architecture.Model.BackUpLogs obj = new Architecture.Model.BackUpLogs();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EntryUserId = clsGlobalVariables.UserAccount.ID;
			obj.BackupTypeId = 1;
			obj.BackupType = "Database";
			obj.BackupStatus = true;
			obj.BackupStartTime = DateTime.Now;
			BackUpFolderPath = backupDirectory;
			obj.BackupLocation = backupDirectory;
			BackupDatabase(connectionString, databaseName, BackUpFolderPath);
			obj.BackupEndTime = DateTime.Now;
			worker?.ReportProgress(25);
			obj.ZipAndEncryptStartTime = DateTime.Now;
			string zipFilePath = Path.Combine($"{backupDirectory}\\{databaseName}_BackUp_{DateTime.Now:yyyyMMdd_HHmm}.zip");
			ZipAndEncrypt(BackUpFolderPath, zipFilePath, password);
			obj.ZipAndEncryptEndTime = DateTime.Now;
			worker?.ReportProgress(50);
			obj.OldDeletedFileCount = DeleteOldBackups(backupDirectory);
			worker?.ReportProgress(75);
			worker?.ReportProgress(100);
			Save(obj);
			CopyBackupToNetwork(zipFilePath, Terminal01Path);
			CopyBackupToNetwork(zipFilePath, Terminal02Path);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static void SendBackupByEmail(string filePath)
	{
		try
		{
			string fromAddress = "your-email@gmail.com";
			string toAddress = "recipient-email@gmail.com";
			string subject = "Database Backup";
			string body = "Please find the attached database backup file.";
			SmtpClient obj = new SmtpClient("smtp.gmail.com", 587)
			{
				Credentials = new NetworkCredential(fromAddress, "your-email-password"),
				EnableSsl = true
			};
			MailMessage mailMessage = new MailMessage
			{
				From = new MailAddress(fromAddress),
				Subject = subject,
				Body = body
			};
			mailMessage.To.Add(toAddress);
			mailMessage.Attachments.Add(new Attachment(filePath));
			obj.Send(mailMessage);
		}
		catch (Exception ex)
		{
			throw new Exception("Error sending email: " + ex.Message);
		}
	}

	private static void CopyBackupToNetwork(string filePath, string TerminalPath)
	{
		try
		{
			if (string.IsNullOrEmpty(TerminalPath) || !Directory.Exists(Path.GetDirectoryName(TerminalPath)))
			{
				Console.WriteLine(TerminalPath + " path does not exist or is inaccessible.");
			}
			string fileName = Path.GetFileName(filePath);
			string TerminaldestinationPath = Path.Combine(TerminalPath, fileName);
			File.Copy(filePath, TerminaldestinationPath, overwrite: true);
			Console.WriteLine("Backup file copied to network path: " + TerminaldestinationPath);
		}
		catch (Exception ex)
		{
			throw new Exception("Error copying backup to network path: " + ex.Message);
		}
	}

	public static void RunLayoutBackup(string connectionString, string backupDirectory)
	{
		try
		{
			string password = "scs@2023";
			Architecture.Model.BackUpLogs obj = new Architecture.Model.BackUpLogs();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EntryUserId = clsGlobalVariables.UserAccount.ID;
			obj.BackupTypeId = 2;
			obj.BackupType = "Layout";
			obj.BackupStatus = true;
			obj.BackupStartTime = DateTime.Now;
			BackUpFolderPath = backupDirectory;
			obj.BackupLocation = backupDirectory;
			BackupDatabase(connectionString, "Layout", BackUpFolderPath);
			obj.BackupEndTime = DateTime.Now;
			obj.ZipAndEncryptStartTime = DateTime.Now;
			string zipFilePath = Path.Combine($"{backupDirectory}\\Layout_BackUp_{DateTime.Now:yyyyMMdd_HHmm}.zip");
			ZipAndEncrypt(BackUpFolderPath, zipFilePath, password);
			obj.ZipAndEncryptEndTime = DateTime.Now;
			obj.OldDeletedFileCount = DeleteOldBackups(backupDirectory);
			Save(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static void BackupDatabase(string connectionString, string databaseName, string backupDirectory)
	{
		try
		{
			_ = new SqlConnectionStringBuilder(connectionString).DataSource;
			using SqlConnection connection = new SqlConnection(connectionString);
			connection.Open();
			BackUpFolderPath = Path.Combine($"{backupDirectory}\\{databaseName}_BackUp_{DateTime.Now:yyyyMMdd_HHmm}.bak");
			new SqlCommand("BACKUP DATABASE [" + databaseName + "] TO DISK='" + BackUpFolderPath + "' WITH FORMAT", connection).ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static void ZipAndEncrypt(string inputFilePath, string outputFilePath, string password)
	{
		try
		{
			using (FileStream backupFileStream = File.OpenRead(inputFilePath))
			{
				using FileStream zipFileStream = new FileStream(outputFilePath, FileMode.Create);
				using ZipOutputStream zipOutputStream = new ZipOutputStream(zipFileStream);
				zipOutputStream.Password = password;
				ZipEntry zipEntry = new ZipEntry(Path.GetFileName(inputFilePath));
				zipOutputStream.PutNextEntry(zipEntry);
				byte[] buffer = new byte[4096];
				StreamUtils.Copy(backupFileStream, zipOutputStream, buffer);
				zipOutputStream.CloseEntry();
			}
			File.Delete(inputFilePath);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static int DeleteOldBackups(string backupDirectory)
	{
		int CountDeletedFile = 0;
		try
		{
			FileInfo[] files = new DirectoryInfo(backupDirectory).GetFiles();
			foreach (FileInfo file in files)
			{
				if (file.Name.Contains("BackUp") && file.Extension.ToLower() == ".zip" && file.CreationTime < DateTime.Now.AddDays(-7.0))
				{
					CountDeletedFile++;
					file.Delete();
				}
			}
			return CountDeletedFile;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
