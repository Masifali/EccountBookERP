using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.PSDSMS
{

public class SQLCompact40
{
	private static SQLCompact40 sQLCompact40;

	private string _dbCon = string.Empty;

	private SqlCeEngine en;

	private SqlCeConnection con;

	private SqlCeCommand cmd;

	public static SQLCompact40 Instance
	{
		get
		{
			if (sQLCompact40 == null)
			{
				sQLCompact40 = new SQLCompact40();
			}
			return sQLCompact40;
		}
	}

	private SQLCompact40()
	{
		_dbCon = "Datasource=\"data.sdf\"; Password=\"Admin123#\"";
		Init();
	}

	public void Init()
	{
		CreateDatabase();
		CreateTable();
	}

	private void CreateDatabase()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			en = new SqlCeEngine(_dbCon);
			if (!en.Verify())
			{
				en.CreateDatabase();
			}
		}
		catch
		{
		}
	}

	private void CreateTable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		con = new SqlCeConnection(_dbCon);
		((DbConnection)(object)con).Close();
		if (((DbConnection)(object)con).State == ConnectionState.Closed)
		{
			((DbConnection)(object)con).Open();
		}
		cmd = new SqlCeCommand("SELECT COUNT(*) AS ID FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='Messages'", con);
		object obj = ((DbCommand)(object)cmd).ExecuteScalar();
		((DbConnection)(object)con).Close();
		if ((int)obj == 0)
		{
			if (((DbConnection)(object)con).State == ConnectionState.Closed)
			{
				((DbConnection)(object)con).Open();
			}
			cmd = new SqlCeCommand("CREATE TABLE Messages(\r\n                                         ID INT PRIMARY KEY IDENTITY(1,1),\r\n                                         MessageId INT DEFAULT((1)),\r\n                                         MessageLogDate NVARCHAR(500) DEFAULT(''),\r\n                                         MessageTExt NVARCHAR(3000) DEFAULT(''),\r\n                                         Mobile NVARCHAR(30) DEFAULT(''),\r\n                                         PassCode NVARCHAR(30) DEFAULT(''),\r\n                                         Status NVARCHAR(50) DEFAULT(''))", con);
			((DbCommand)(object)cmd).ExecuteNonQuery();
			((DbConnection)(object)con).Close();
		}
	}

	public void AlterTable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			con = new SqlCeConnection(_dbCon);
			((DbConnection)(object)con).Close();
			if (((DbConnection)(object)con).State == ConnectionState.Closed)
			{
				((DbConnection)(object)con).Open();
			}
			cmd = new SqlCeCommand("ALTER TABLE USERS \r\n                                             ADD AccountId INT", con);
			((DbCommand)(object)cmd).ExecuteNonQuery();
			((DbConnection)(object)con).Close();
		}
		catch
		{
		}
		finally
		{
			con.Dispose();
			((Component)(object)cmd).Dispose();
		}
	}

	public int ExecuteNonQuery(string strSQL)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		con = new SqlCeConnection(_dbCon);
		((DbConnection)(object)con).Close();
		if (((DbConnection)(object)con).State == ConnectionState.Closed)
		{
			((DbConnection)(object)con).Open();
		}
		cmd = new SqlCeCommand();
		try
		{
			cmd.Connection = con;
			((DbCommand)(object)cmd).CommandText = strSQL;
			int result = ((DbCommand)(object)cmd).ExecuteNonQuery();
			((DbConnection)(object)con).Close();
			return result;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			con.Dispose();
			((Component)(object)cmd).Dispose();
		}
	}

	public int ExecuteScalar(string strSQL)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		int Id = 0;
		con = new SqlCeConnection(_dbCon);
		((DbConnection)(object)con).Close();
		if (((DbConnection)(object)con).State == ConnectionState.Closed)
		{
			((DbConnection)(object)con).Open();
		}
		cmd = new SqlCeCommand();
		try
		{
			cmd.Connection = con;
			((DbCommand)(object)cmd).CommandText = strSQL;
			object ret = ((DbCommand)(object)cmd).ExecuteScalar();
			Id = ((ret != null) ? ((int)ret) : 0);
			((DbConnection)(object)con).Close();
			return Id;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			con.Dispose();
			((Component)(object)cmd).Dispose();
		}
	}

	public DataTable GetDataTable(string strSQL)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		con = new SqlCeConnection(_dbCon);
		((DbConnection)(object)con).Close();
		if (((DbConnection)(object)con).State == ConnectionState.Closed)
		{
			((DbConnection)(object)con).Open();
		}
		cmd = new SqlCeCommand();
		SqlCeDataAdapter da = new SqlCeDataAdapter();
		DataTable dt = new DataTable();
		try
		{
			cmd.Connection = con;
			((DbCommand)(object)cmd).CommandText = strSQL;
			da.SelectCommand = cmd;
			((DbDataAdapter)(object)da).Fill(dt);
			dt.AcceptChanges();
			((DbConnection)(object)con).Close();
			return dt;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			((Component)(object)cmd).Dispose();
			da.Dispose();
			dt.Dispose();
		}
	}
}

}
