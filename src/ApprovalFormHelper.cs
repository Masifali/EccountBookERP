using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.ModernHistories;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Helper
{

public static class ApprovalFormHelper
{
	private static readonly Padding DefaultPadding = new Padding(2);

	public static ApprovalList ProcessApproval(List<ApprovalSideBarMainCard> selectedCards, UserAccount userAccount, bool flagUnApprove, int DocumentTypeId, TextBox txtRemarks, string ScreenName, int ReasonId = 0)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Expected O, but got Unknown
		string action = (flagUnApprove ? "UnApprove" : "Approve");
		string reqType = (flagUnApprove ? "UP" : "AP");
		ApprovalList approvalWrapper = new ApprovalList
		{
			AllApprovalLists = new List<ApprovalList>()
		};
		try
		{
			if (!selectedCards.Any())
			{
				MessageBox.Show("Please select at least one record to " + action + ".", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return null;
			}
			string remarks = txtRemarks?.Text?.Trim();
			if (string.IsNullOrWhiteSpace(remarks))
			{
				txtRemarks.Focus();
				MessageBox.Show("Approval remarks are required...", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return null;
			}
			if (MessageBox.Show("Are you sure to " + action + " selected '" + ScreenName + "'(s)?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return null;
			}
			foreach (ApprovalSideBarMainCard card in selectedCards)
			{
				approvalWrapper.AllApprovalLists.Add(new ApprovalList
				{
					OrganizationId = userAccount.OrganizationId,
					CompanyId = userAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = DocumentTypeId,
					Id = card.DocumentId,
					PostDate = DateTime.Now,
					EntryUser = userAccount.ID,
					ReqType = reqType,
					Comments = remarks,
					ReasonId = ReasonId
				});
			}
			return approvalWrapper;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SetupCardTableLayout(TableLayoutPanel table, int caption1MinWidth = 70, int value1MinWidth = 120, int caption2MinWidth = 70, int value2MinWidth = 120)
	{
		table.SuspendLayout();
		table.ColumnCount = 4;
		table.RowCount = 0;
		table.ColumnStyles.Clear();
		table.RowStyles.Clear();
		table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, caption1MinWidth));
		table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, value1MinWidth));
		table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, caption2MinWidth));
		table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, value2MinWidth));
		table.AutoSize = true;
		table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
		table.Dock = DockStyle.Top;
		table.ResumeLayout();
	}

	public static void DynamicaCardRowRender(TableLayoutPanel table, CardRowModel rowModel)
	{
		int row = table.RowCount++;
		table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
		AddCellNew(table, rowModel.Col1, 0, row);
		AddCellNew(table, rowModel.Col2, 1, row);
		AddCellNew(table, rowModel.Col3, 2, row);
		AddCellNew(table, rowModel.Col4, 3, row);
	}

	private static void AddCellNew(TableLayoutPanel table, CardCellModel model, int col, int row)
	{
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0126: Expected O, but got Unknown
		if (model == null || !model.Visible)
		{
			return;
		}
		Control ctrl;
		switch (model.CellType)
		{
		case CardCellType.Link:
			ctrl = new LinkLabel
			{
				Text = model.Text,
				LinkColor = model.ForeColor,
				ActiveLinkColor = Color.DarkBlue
			};
			if (model.OnClick != null)
			{
				ctrl.Click += model.OnClick;
			}
			break;
		case CardCellType.Button:
			ctrl = new Button
			{
				Text = model.Text,
				FlatStyle = FlatStyle.Flat,
				Height = model.Height
			};
			if (model.OnClick != null)
			{
				ctrl.Click += model.OnClick;
			}
			break;
		case CardCellType.UltraCombo:
		{
			UltraCombo val = new UltraCombo();
			((Control)val).Dock = DockStyle.Fill;
			((Control)val).Height = ((model.Height > 0) ? model.Height : 24);
			((Control)val).Font = model.Font ?? new Font("Verdana", 10f);
			InfragisticsHelper.BindAndRetainSelection(val, model.DataSource, model.ColumnNameId, model.ColumnNameText, model.Text, AllColumns: true, null, insertDefaultRow: false, null, null, ActivateRow: false);
			ctrl = (Control)val;
			break;
		}
		default:
			ctrl = new Label
			{
				Text = model.Text
			};
			break;
		}
		ctrl.Dock = DockStyle.Fill;
		ctrl.AutoSize = false;
		ctrl.Margin = model.Padding;
		ctrl.Font = (model.Bold ? new Font(model.Font, FontStyle.Bold) : model.Font);
		ctrl.ForeColor = model.ForeColor;
		_ = model.BackColor;
		if (model.BackColor.Name != "White")
		{
			ctrl.BackColor = model.BackColor;
		}
		ctrl.Height = model.Height;
		table.Controls.Add(ctrl, col, row);
		if (model.ColumnSpan > 1)
		{
			table.SetColumnSpan(ctrl, model.ColumnSpan);
		}
	}

	public static CardCellModel DefaultLabelModel(string text)
	{
		return new CardCellModel
		{
			Text = text,
			ForeColor = Color.DarkCyan,
			Font = new Font("Segoe UI", 9f),
			Padding = new Padding(0)
		};
	}

	public static CardCellModel DefaultValueModel(string text)
	{
		return new CardCellModel
		{
			Text = text,
			Font = new Font("Segoe UI", 9f),
			ForeColor = Color.Black,
			Padding = new Padding(0)
		};
	}

	public static CardCellModel DefaultLinkModel(string text, Action action)
	{
		return new CardCellModel
		{
			Text = text,
			CellType = CardCellType.Link,
			Font = new Font("Segoe UI", 10f),
			ForeColor = Color.RoyalBlue,
			OnClick = delegate
			{
				action();
			},
			Padding = new Padding(0)
		};
	}

	public static CardCellModel DefaultButtonModel(string text, Color color, Action action, int colSpan)
	{
		return new CardCellModel
		{
			Text = text,
			CellType = CardCellType.Button,
			Bold = true,
			ForeColor = color,
			Height = 28,
			ColumnSpan = colSpan,
			TextAlign = ContentAlignment.MiddleCenter,
			OnClick = delegate
			{
				action();
			}
		};
	}

	public static void AddPreviewField(FlowLayoutPanel container, PreviewToolTipField field)
	{
		Panel rowPanel = new Panel
		{
			Width = field.CaptionWidth + field.ValueWidth + 20,
			Height = field.RowHeight,
			Margin = new Padding(3),
			BackColor = Color.Transparent
		};
		Label lblCaption = new Label
		{
			Text = field.Caption,
			Width = field.CaptionWidth,
			Height = field.RowHeight,
			Dock = DockStyle.Left,
			TextAlign = ContentAlignment.MiddleLeft,
			Font = new Font("Segoe UI", 9f, FontStyle.Bold),
			ForeColor = Color.FromArgb(90, 90, 90),
			Padding = field.CaptionPadding
		};
		if (field.ValueControl is TextBox txt)
		{
			txt.ReadOnly = true;
			txt.BorderStyle = BorderStyle.None;
			txt.BackColor = Color.White;
			txt.ForeColor = (field.IsHighlight ? Color.FromArgb(0, 120, 215) : Color.FromArgb(40, 40, 40));
			txt.Font = (field.IsHighlight ? new Font("Segoe UI", 9.5f, FontStyle.Bold) : new Font("Segoe UI", 9f, FontStyle.Regular));
			txt.Dock = DockStyle.Fill;
			txt.Margin = field.ValuePadding;
		}
		rowPanel.Controls.Add(field.ValueControl);
		rowPanel.Controls.Add(lblCaption);
		container.Controls.Add(rowPanel);
	}

	public static async void FadeInCard(Form card)
	{
		try
		{
			card.Opacity = 0.0;
			card.Show();
			for (double i = 0.0; i <= 1.0; i += 0.1)
			{
				if (card.IsDisposed)
				{
					return;
				}
				card.Opacity = i;
				await Task.Delay(15);
			}
			if (!card.IsDisposed)
			{
				card.Opacity = 1.0;
			}
		}
		catch
		{
		}
	}
}

}
