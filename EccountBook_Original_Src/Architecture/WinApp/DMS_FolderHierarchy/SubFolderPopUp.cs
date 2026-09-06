using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.Model.DMS;

namespace Architecture.WinApp.DMS_FolderHierarchy;

public class SubFolderPopUp : Form
{
	private FolderHierarchy folderHierarchy = new FolderHierarchy();

	private IContainer components;

	internal Button btnOk;

	internal Label Label1;

	internal TextBox txtNodeName;

	public SubFolderPopUp(FolderHierarchy Obj)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		folderHierarchy = Obj;
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		try
		{
			if (txtNodeName.Text == string.Empty)
			{
				MessageBox.Show("Folder Name Required!");
				txtNodeName.Focus();
				return;
			}
			string path = folderHierarchy.FolderPath + "\\" + txtNodeName.Text;
			if (Directory.Exists(path))
			{
				MessageBox.Show("Folder Name " + txtNodeName.Text + "Already Exist");
				txtNodeName.Focus();
			}
			else
			{
				Directory.CreateDirectory(path);
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		this.btnOk = new System.Windows.Forms.Button();
		this.Label1 = new System.Windows.Forms.Label();
		this.txtNodeName = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.btnOk.Location = new System.Drawing.Point(145, 53);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(75, 23);
		this.btnOk.TabIndex = 5;
		this.btnOk.Text = "OK";
		this.btnOk.UseVisualStyleBackColor = true;
		this.btnOk.Click += new System.EventHandler(btnOk_Click);
		this.Label1.AutoSize = true;
		this.Label1.Location = new System.Drawing.Point(23, 22);
		this.Label1.Name = "Label1";
		this.Label1.Size = new System.Drawing.Size(69, 13);
		this.Label1.TabIndex = 4;
		this.Label1.Text = "Enter Name: ";
		this.txtNodeName.Location = new System.Drawing.Point(98, 19);
		this.txtNodeName.Name = "txtNodeName";
		this.txtNodeName.Size = new System.Drawing.Size(206, 20);
		this.txtNodeName.TabIndex = 3;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(323, 84);
		base.Controls.Add(this.btnOk);
		base.Controls.Add(this.Label1);
		base.Controls.Add(this.txtNodeName);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "SubFolderPopUp";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Sub Folder";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
