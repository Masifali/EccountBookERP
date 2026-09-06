using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Model;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.WholeSale;

public class DummyForm : Form
{
	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	public DummyForm(UserAccount PUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PUser;
	}

	private void DummyForm_Load(object sender, EventArgs e)
	{
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		DataTable parentTable = new DataTable("ParentTable");
		parentTable.Columns.Add("ParentId", typeof(object));
		parentTable.Columns.Add("ParentName", typeof(string));
		parentTable.Rows.Add(1, "Parent 1");
		parentTable.Rows.Add(2, "Parent 2");
		parentTable.Rows.Add(3, "Parent 3");
		DataTable childTable = new DataTable("ChildTable");
		childTable.Columns.Add("ChildId", typeof(object));
		childTable.Columns.Add("ChildName", typeof(string));
		childTable.Columns.Add("ParentId", typeof(object));
		childTable.Rows.Add(1, "Child 1", 1);
		childTable.Rows.Add(2, "Child 2", 1);
		childTable.Rows.Add(3, "Child 3", 2);
		childTable.Rows.Add(4, "Child 4", 2);
		childTable.Rows.Add(5, "Child 5", 3);
		DataSet dataSet = new DataSet();
		dataSet.Tables.Add(parentTable);
		dataSet.Tables.Add(childTable);
		DataRelation relation = new DataRelation("ParentChildRelation", parentTable.Columns["ParentId"], childTable.Columns["ParentId"]);
		dataSet.Relations.Add(relation);
		GridEX gridEX = new GridEX();
		((Control)(object)gridEX).Dock = DockStyle.Fill;
		gridEX.SetDataBinding((object)dataSet, "ParentId");
		gridEX.Hierarchical = true;
		base.Controls.Add((Control)(object)gridEX);
		gridEX.RetrieveStructure();
		((Control)(object)gridEX).Refresh();
		gridEX.HideSelection = (HideSelection)2;
		gridEX.AllowEdit = (InheritableBoolean)2;
		gridEX.RootTable.HierarchicalMode = (HierarchicalMode)1;
		gridEX.RootTable.SelfReferencingSettings.ExpandColumn = gridEX.RootTable.Columns["ParentName"];
		gridEX.RootTable.SelfReferencingSettings.AggregationMode = (SelfReferencingAggregationMode)1;
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(800, 450);
		base.Name = "DummyForm";
		this.Text = "DummyForm";
		base.Load += new System.EventHandler(DummyForm_Load);
		base.ResumeLayout(false);
	}
}
