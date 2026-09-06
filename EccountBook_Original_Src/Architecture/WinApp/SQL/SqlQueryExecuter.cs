using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Properties;

namespace Architecture.WinApp.SQL;

public class SqlQueryExecuter : Form
{
	private WaitForm waitForm = new WaitForm();

	private string connectionString = "";

	private string script = "";

	public static string _Role = "";

	private IContainer components;

	private ToolStrip toolStrip1;

	private SplitContainer splitContainer1;

	private DataGridView grdResult;

	private ToolStripButton btnNew;

	private ToolStripButton btnExecute;

	private RichTextBox txtQuery;

	private ToolStripButton btnOpenFile;

	private OpenFileDialog openFileDialog1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripLabel lblFileName;

	public SqlQueryExecuter()
	{
		InitializeComponent();
	}

	private void SqlQueryExecuter_Load(object sender, EventArgs e)
	{
		splitContainer1.Panel2Collapsed = true;
		connectionString = Services.GetConnectionString();
		string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
		Text = "SQL Query. Database: " + databaseName;
		if (_Role == "Admin")
		{
			btnOpenFile.Enabled = true;
		}
		else
		{
			btnOpenFile.Enabled = false;
		}
	}

	private void HighlightKeywords()
	{
		try
		{
			int selectionStart = txtQuery.SelectionStart;
			int selectionLength = txtQuery.SelectionLength;
			txtQuery.SelectAll();
			txtQuery.SelectionColor = Color.Black;
			txtQuery.DeselectAll();
			foreach (var keyword in SqlKeywords.GetKeywords())
			{
				int startIndex = 0;
				while ((startIndex = FindWholeWord(txtQuery.Text, keyword.Word, startIndex)) != -1)
				{
					txtQuery.Select(startIndex, keyword.Word.Length);
					txtQuery.SelectionColor = keyword.Color;
					startIndex += keyword.Word.Length;
				}
			}
			txtQuery.SelectionStart = selectionStart;
			txtQuery.SelectionLength = selectionLength;
			txtQuery.SelectionColor = Color.Black;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private int FindWholeWord(string text, string word, int startIndex)
	{
		for (int index = text.IndexOf(word, startIndex, StringComparison.OrdinalIgnoreCase); index != -1; index = text.IndexOf(word, index + 1, StringComparison.OrdinalIgnoreCase))
		{
			if ((index == 0 || !char.IsLetterOrDigit(text[index - 1])) && (index + word.Length == text.Length || !char.IsLetterOrDigit(text[index + word.Length])))
			{
				return index;
			}
		}
		return -1;
	}

	private void txtQuery_TextChanged(object sender, EventArgs e)
	{
		try
		{
			HighlightKeywords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private async void ExecuteQuery(string query)
	{
		if (txtQuery.Text == string.Empty)
		{
			MessageBox.Show("There is nothing to execute");
			return;
		}
		if ((txtQuery.Text.ToLower().Contains("delete") || txtQuery.Text.ToLower().Contains("update")) && MessageBox.Show("Some thing is going to be deleted or updated in this process. " + Environment.NewLine + " Are you sure you want to proceed?", "Confirm Execution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			txtQuery.Text = string.Empty;
			lblFileName.Text = string.Empty;
			splitContainer1.Panel2Collapsed = true;
		}
		waitForm.Show(this);
		using SqlConnection connection = new SqlConnection(connectionString);
		_ = 1;
		try
		{
			connection.Open();
			SqlCommand command = new SqlCommand(query, connection);
			DataTable dataTable = new DataTable();
			bool hasData = false;
			using (SqlDataReader reader = await command.ExecuteReaderAsync())
			{
				if (reader.HasRows)
				{
					dataTable.Load(reader);
					grdResult.DataSource = dataTable;
					hasData = true;
					splitContainer1.Panel2Collapsed = false;
					waitForm.Close();
				}
				else
				{
					grdResult.DataSource = null;
					splitContainer1.Panel2Collapsed = true;
					waitForm.Close();
				}
			}
			if (!hasData)
			{
				splitContainer1.Panel2Collapsed = true;
				command.CommandText = query;
				int affectedRows = await command.ExecuteNonQueryAsync();
				waitForm.Close();
				MessageBox.Show($"{affectedRows} rows affected.");
				grdResult.DataSource = null;
			}
		}
		catch (SqlException sqlEx)
		{
			waitForm.Close();
			MessageBox.Show("SQL Error: " + sqlEx.Message);
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private async void ExecuteQueryScript(string query)
	{
		if (string.IsNullOrWhiteSpace(query))
		{
			MessageBox.Show("There is nothing to execute");
		}
		else
		{
			if ((query.ToLower().Contains("delete") || query.ToLower().Contains("update")) && MessageBox.Show("Some operations may delete or update data. Are you sure you want to proceed?", "Confirm Execution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			waitForm.Show(this);
			using SqlConnection connection = new SqlConnection(connectionString);
			await connection.OpenAsync();
			using SqlTransaction transaction = connection.BeginTransaction();
			ResultForm resultForm = new ResultForm();
			resultForm.Show();
			try
			{
				string[] batches = Regex.Replace(query, "--.*?$|/\\*.*?\\*/", "", RegexOptions.Multiline | RegexOptions.Singleline).Split(new string[1] { "GO" }, StringSplitOptions.RemoveEmptyEntries);
				string[] array = batches;
				foreach (string batch in array)
				{
					string trimmedBatch = batch.Trim();
					if (string.IsNullOrWhiteSpace(trimmedBatch))
					{
						continue;
					}
					using SqlCommand command = new SqlCommand(trimmedBatch, connection, transaction);
					_ = 1;
					try
					{
						resultForm.AddMessage($"{await command.ExecuteNonQueryAsync()} rows affected for command: {trimmedBatch}", Color.Green);
					}
					catch (SqlException sqlEx)
					{
						resultForm.AddMessage("SQL Error: " + sqlEx.Message + "\nCommand: " + trimmedBatch, Color.Red);
						transaction.Rollback();
						MessageBox.Show("Rolling back due to SQL error: " + sqlEx.Message);
						return;
					}
					catch (Exception ex2)
					{
						resultForm.AddMessage("Error: " + ex2.Message + "\nCommand: " + trimmedBatch, Color.Red);
						transaction.Rollback();
						MessageBox.Show("Rolling back due to error: " + ex2.Message);
						return;
					}
				}
				transaction.Commit();
				resultForm.AddMessage("All commands executed successfully.", Color.Blue);
			}
			catch (Exception ex)
			{
				resultForm.AddMessage("Error: " + ex.Message, Color.Red);
				transaction.Rollback();
				MessageBox.Show("Rolling back due to unexpected error: " + ex.Message);
			}
			finally
			{
				waitForm.Close();
			}
		}
	}

	private void btnExecute_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("Have you taken backup? " + Environment.NewLine + " this is recommended that backup must be taken before execution of any query.", "Confirm Execution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				if (lblFileName.Text == string.Empty)
				{
					ExecuteQuery(txtQuery.Text);
				}
				else
				{
					ExecuteQueryScript(script);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void SqlQueryExecuter_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.F5)
			{
				ExecuteQuery(txtQuery.Text);
				e.SuppressKeyPress = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		if (MessageBox.Show("Do you want to clear?", "Confirm Execution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
		{
			txtQuery.Text = string.Empty;
			lblFileName.Text = string.Empty;
			splitContainer1.Panel2Collapsed = true;
		}
	}

	private void btnOpenFile_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
		openFileDialog.Title = "Open SQL Script";
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			string filePath = openFileDialog.FileName;
			lblFileName.Text = filePath;
			try
			{
				script = File.ReadAllText(filePath);
				MessageBox.Show("File selected.");
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error reading file: " + ex.Message);
				return;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SQL.SqlQueryExecuter));
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.txtQuery = new System.Windows.Forms.RichTextBox();
		this.grdResult = new System.Windows.Forms.DataGridView();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnExecute = new System.Windows.Forms.ToolStripButton();
		this.btnOpenFile = new System.Windows.Forms.ToolStripButton();
		this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.lblFileName = new System.Windows.Forms.ToolStripLabel();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdResult).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnOpenFile, this.btnExecute, this.toolStripSeparator1, this.lblFileName });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(956, 25);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 25);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.txtQuery);
		this.splitContainer1.Panel2.Controls.Add(this.grdResult);
		this.splitContainer1.Size = new System.Drawing.Size(956, 541);
		this.splitContainer1.SplitterDistance = 326;
		this.splitContainer1.TabIndex = 2;
		this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
		this.txtQuery.Font = new System.Drawing.Font("Courier New", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQuery.Location = new System.Drawing.Point(0, 0);
		this.txtQuery.Name = "txtQuery";
		this.txtQuery.Size = new System.Drawing.Size(952, 322);
		this.txtQuery.TabIndex = 0;
		this.txtQuery.Text = "";
		this.txtQuery.TextChanged += new System.EventHandler(txtQuery_TextChanged);
		this.grdResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.grdResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdResult.Location = new System.Drawing.Point(0, 0);
		this.grdResult.Name = "grdResult";
		this.grdResult.Size = new System.Drawing.Size(952, 207);
		this.grdResult.TabIndex = 0;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(51, 22);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnExecute.Image = Architecture.WinApp.Properties.Resources.play;
		this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnExecute.Name = "btnExecute";
		this.btnExecute.Size = new System.Drawing.Size(68, 22);
		this.btnExecute.Text = "Execute";
		this.btnExecute.Click += new System.EventHandler(btnExecute_Click);
		this.btnOpenFile.Image = (System.Drawing.Image)resources.GetObject("btnOpenFile.Image");
		this.btnOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOpenFile.Name = "btnOpenFile";
		this.btnOpenFile.Size = new System.Drawing.Size(77, 22);
		this.btnOpenFile.Text = "Open File";
		this.btnOpenFile.Click += new System.EventHandler(btnOpenFile_Click);
		this.openFileDialog1.FileName = "openFileDialog1";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.lblFileName.Name = "lblFileName";
		this.lblFileName.Size = new System.Drawing.Size(0, 22);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(956, 566);
		base.Controls.Add(this.splitContainer1);
		base.Controls.Add(this.toolStrip1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "SqlQueryExecuter";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "SQL Query Executer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(SqlQueryExecuter_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SqlQueryExecuter_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdResult).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
