using System;
using System.Collections.Generic;
using System.Data;
using Architecture.PSDSMS.DataLayer;

namespace Architecture.PSDSMS;

public class MessageLogService
{
	private static MessageLogService messageLogService;

	public static MessageLogService Instance
	{
		get
		{
			if (messageLogService == null)
			{
				messageLogService = new MessageLogService();
			}
			return messageLogService;
		}
	}

	private MessageLogService()
	{
	}

	public List<Message> GetMessages()
	{
		DataTable dataTable = new DataTable();
		try
		{
			List<Message> messages = new List<Message>();
			dataTable = SQLHelper.Instance.GetDataTable("SELECT ISNULL(MessageId,0) AS MessageId, ISNULL(MessageLogDate,GetDate()) AS MessageLogDate, ISNULL(MessageText,'') AS MessageText, ISNULL(Mobile,'00966000000000') AS Mobile, ISNULL(PassCode,'') AS PassCode, ISNULL(Status,'Pending') AS Status FROM Messages WHERE Status IS NULL OR Status='Pending' ORDER BY MessageID ASC");
			foreach (DataRow row in dataTable.Rows)
			{
				messages.Add(new Message
				{
					MessageId = Convert.ToInt32(row["MessageId"]),
					MessageLogDate = Convert.ToDateTime(row["MessageLogDate"]),
					MessageText = row["MessageText"].ToString(),
					Mobile = ("00966" + row["Mobile"].ToString().Substring(row["Mobile"].ToString().Length - 9).ToString()).Trim(),
					PassCode = row["PassCode"].ToString(),
					Status = row["Status"].ToString()
				});
			}
			return messages;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			dataTable.Dispose();
		}
	}

	public List<Message> GetAllMessages()
	{
		DataTable dataTable = new DataTable();
		try
		{
			List<Message> messages = new List<Message>();
			dataTable = SQLCompact40.Instance.GetDataTable("SELECT ISNULL(MessageId,0) AS MessageId, ISNULL(MessageLogDate,GetDate()) AS MessageLogDate, IsNULL(MessageText,'') AS MessageText,ISNULL(Mobile,'00966') AS Mobile, ISNULL(PassCode,'') AS Password,ISNULL(Status,'Pending') AS Status FROM Messages WHERE Status IS NULL OR Status='Pending' ORDER BY MessageID ASC");
			foreach (DataRow row in dataTable.Rows)
			{
				messages.Add(new Message
				{
					MessageId = Convert.ToInt32(row["MessageId"]),
					MessageLogDate = Convert.ToDateTime(row["MessageLogDate"]),
					MessageText = row["MessageText"].ToString(),
					Mobile = ("00966" + row["Mobile"].ToString().Substring(row["Mobile"].ToString().Length - 9).ToString()).Trim(),
					PassCode = row["PassCode"].ToString(),
					Status = row["Status"].ToString()
				});
			}
			return messages;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			dataTable.Dispose();
		}
	}

	public void AddMessages()
	{
		try
		{
			List<Message> messages = GetMessages();
			SQLCompact40.Instance.ExecuteNonQuery("DELETE FROM Messages WHERE Status='Sent'");
			if (messages == null && messages.Count <= 0)
			{
				return;
			}
			foreach (Message item in messages)
			{
				if (SQLCompact40.Instance.ExecuteScalar("SELECT ID From Messages WHERE MessageId=" + item.MessageId) == 0)
				{
					SQLCompact40.Instance.ExecuteNonQuery("INSERT INTO Messages(MessageId, MessageLogDate, MessageText, Mobile, PassCode,Status) VALUES(" + item.MessageId + ",'" + item.MessageLogDate.Value.ToString("yyyy-M-d HH:mm:ss") + "', N'" + item.MessageText.ToString().Replace("'", "''") + "', '" + item.Mobile.ToString().Replace("'", "''") + "','" + item.PassCode.ToString().Replace("'", "''") + "','" + item.Status.ToString().Replace("'", "''") + "')");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
