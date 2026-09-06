// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Themes_ERP;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Dashboard
{
	public class frmMenue : Form
	{
		private Architecture.Model.UserAccount UserAccount = new Architecture.Model.UserAccount();

		private int ModuleId;

		private string ModuleIconPath = "";

		private DataTable dtForSKM = new DataTable();

		private DataTable dtForSKS = new DataTable();

		private DashboardNew newdash;

		private IContainer components;

		private Panel panel1;

		private Label lblModuleName;

		private FlowLayoutPanel accountsFlowLayout;

		private Panel panel2;

		private FlowLayoutPanel ScreensFlowlayout;

		public frmMenue(Architecture.Model.UserAccount pUser, int ModId, string ModuleName, DashboardNew dashboardNew)
		{
			InitializeComponent();
			UserAccount = pUser;
			ModuleId = ModId;
			lblModuleName.Text = ModuleName;
			newdash = dashboardNew;
		}

		private void ApprovalDashboard_Load(object sender, EventArgs e)
		{
			try
			{
				if (dtForSKM.Columns.Count == 0)
				{
					dtForSKM.Columns.Add("SrNo", typeof(int));
					dtForSKM.Columns.Add("AppId", typeof(int));
					dtForSKM.Columns.Add("App", typeof(string));
				}
				if (dtForSKS.Columns.Count == 0)
				{
					dtForSKS.Columns.Add("SrNo", typeof(int));
					dtForSKS.Columns.Add("ScreenId", typeof(int));
					dtForSKS.Columns.Add("ScreenName", typeof(string));
					dtForSKS.Columns.Add("ScreenAlias", typeof(string));
					dtForSKS.Columns.Add("TargetUrl", typeof(string));
					dtForSKS.Columns.Add("IsFavorite", typeof(bool));
				}
				DynamicallyGenerateCardsForModule();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void DynamicallyGenerateCards(int ModId)
		{
			try
			{
				dtForSKS.Clear();
				ScreensFlowlayout.Controls.Clear();
				List<Architecture.Model.tblUserRights> lst = clsGlobalVariables.ScreenViewReights.Where((Architecture.Model.tblUserRights x) => x.ModuleID == ModId && x.Value).ToList();
				if (lst.Count > 0)
				{
					ScreenDynamicallyGenerateCards[] AccountCardsList = new ScreenDynamicallyGenerateCards[lst.Count];
					for (int i = 0; i < lst.Count; i++)
					{
						AccountCardsList[i] = new ScreenDynamicallyGenerateCards(UserAccount);
						AccountCardsList[i].Margin = new Padding(10, 5, 5, 5);
						AccountCardsList[i].IsFavorite = Conversion.ToBool((object)lst[i].IsFavorite);
						AccountCardsList[i].ScreenId = Conversion.ToInt((object)lst[i].ScreenID);
						AccountCardsList[i].ModuleId = Conversion.ToInt((object)lst[i].ModuleID);
						AccountCardsList[i].Id = Conversion.ToString((object)lst[i].TargetUrl);
						AccountCardsList[i].Title = Conversion.ToString((object)lst[i].ScreenAlias);
						AccountCardsList[i].ScreenName = Conversion.ToString((object)lst[i].ScreenName);
						ScreensFlowlayout.Controls.Add(AccountCardsList[i]);
						AccountCardsList[i].UserControlClicked += UserControl_Click1;
						dtForSKS.Rows.Add(i, Conversion.ToInt((object)lst[i].ScreenID), Conversion.ToString((object)lst[i].ScreenName), Conversion.ToString((object)lst[i].ScreenAlias), Conversion.ToString((object)lst[i].TargetUrl), Conversion.ToBool((object)lst[i].IsFavorite));
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private Type ResolveType(string targetUrl)
		{
			if (string.IsNullOrWhiteSpace(targetUrl)) return null;

			targetUrl = targetUrl.Trim();

			// 1. Direct Type.GetType lookup
			Type typ = Type.GetType(targetUrl, false, true);
			if (typ != null) return typ;

			// 2. Try with Assembly qualification ", Architecture.WinApp"
			if (!targetUrl.Contains(","))
			{
				typ = Type.GetType(targetUrl + ", Architecture.WinApp", false, true);
				if (typ != null) return typ;
			}

			// Extract simple class name
			string className = targetUrl;
			if (className.Contains("."))
			{
				className = className.Substring(className.LastIndexOf('.') + 1);
			}
			if (className.Contains(","))
			{
				className = className.Substring(0, className.IndexOf(',')).Trim();
			}

			// 3. Search loaded assemblies
			List<System.Reflection.Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
			System.Reflection.Assembly executingAsm = System.Reflection.Assembly.GetExecutingAssembly();
			if (executingAsm != null && !assemblies.Contains(executingAsm))
			{
				assemblies.Add(executingAsm);
			}

			foreach (System.Reflection.Assembly asm in assemblies)
			{
				if (asm.IsDynamic) continue;
				try
				{
					typ = asm.GetType(targetUrl, false, true);
					if (typ != null) return typ;

					if (!targetUrl.StartsWith("Architecture.WinApp.", StringComparison.OrdinalIgnoreCase))
					{
						typ = asm.GetType("Architecture.WinApp." + targetUrl, false, true);
						if (typ != null) return typ;
					}

					typ = asm.GetType("Architecture.WinApp." + className, false, true);
					if (typ != null) return typ;

					Type[] types = null;
					try
					{
						types = asm.GetTypes();
					}
					catch (System.Reflection.ReflectionTypeLoadException rtle)
					{
						types = rtle.Types.Where(t => t != null).ToArray();
					}
					catch
					{
					}

					if (types != null)
					{
						foreach (Type t in types)
						{
							if (typeof(Form).IsAssignableFrom(t) || typeof(UserControl).IsAssignableFrom(t))
							{
								if (string.Equals(t.Name, className, StringComparison.OrdinalIgnoreCase) ||
									string.Equals(t.FullName, targetUrl, StringComparison.OrdinalIgnoreCase) ||
									string.Equals(t.Name, targetUrl, StringComparison.OrdinalIgnoreCase))
								{
									return t;
								}
							}
						}
					}
				}
				catch
				{
				}
			}

			// 4. Direct Fallback Map for Accounts Submodules
			if (string.Equals(className, "JournalVoucher", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("JournalVoucher", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher_New);
			if (string.Equals(className, "JournalVoucher_New", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("JournalVoucher_New", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.JournalVoucher_New);
			if (string.Equals(className, "PaymentVoucherNew", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PaymentVoucher", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.PaymentVoucherNew);
			if (string.Equals(className, "ReceiptsVoucherNew", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("ReceiptsVoucher", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("ReceiptVoucher", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("Acfrmfcbankreceipt", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.ReceiptsVoucherNew);
			if (string.Equals(className, "ContraVoucher", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("ContraVoucher", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.ContraVoucher);
			if (string.Equals(className, "ExpenseVoucherNew", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("ExpenseVoucher", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.VouchersWithTax.ExpenseVoucherNew);
			if (string.Equals(className, "ExpenseVoucher", StringComparison.OrdinalIgnoreCase)) return typeof(Architecture.WinApp.Account_Definition.ExpenseVoucher);
			if (string.Equals(className, "PaymentByInvoiceVoucherNew", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PaymentByInvoice", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("InvoicesAdjustment", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("Premature", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.PaymentByInvoiceVoucherNew);
			if (string.Equals(className, "frmPartyPaymentVoucher", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PartyPayment", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.frmPartyPaymentVoucher);
			if (string.Equals(className, "frmPartyReceiptVoucher", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PartyReceipt", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.frmPartyReceiptVoucher);
			if (string.Equals(className, "frmPdcTransactionPayment", StringComparison.OrdinalIgnoreCase) || string.Equals(className, "AcfrmDefPdcManagment", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PdcManagment", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("PdcTransaction", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Bank.frmPdcTransactionPayment);
			if (string.Equals(className, "PostDatedCheqPaymentVouchers", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PostDatedCheqPayment", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.PostDatedCheqPaymentVouchers);
			if (string.Equals(className, "PostDatedCheqRegister", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PostDatedCheqRegister", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("PostDatedChequeReports", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.PostDatedCheqRegister);
			if (string.Equals(className, "PostDatedCheqInformation", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PostDatedCheqInformation", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.PostDatedCheqInformation);
			if (string.Equals(className, "GeneralLedgerStatment", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("GeneralLedgerStatment", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.GeneralLedgerStatment);
			if (string.Equals(className, "GeneralLedger", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("GeneralLedger", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.GeneralLedger);
			if (string.Equals(className, "DayBook", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("DayBook", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.DayBook);
			if (string.Equals(className, "BalanceSheet", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("BalanceSheet", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.BalanceSheet);
			if (string.Equals(className, "ProfitLossNew", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("ProfitLoss", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("Profit", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.ProfitLossNew);
			if (string.Equals(className, "ProfitLoss", StringComparison.OrdinalIgnoreCase)) return typeof(Architecture.WinApp.Account_Reports.ProfitLoss);
			if (string.Equals(className, "SelectedTrialBalance", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("SelectedTrialBalance", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.SelectedTrialBalance);
			if (string.Equals(className, "frmTrialBalancesAllLevel", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("TrialBalancesAllLevel", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.frmTrialBalancesAllLevel);
			if (string.Equals(className, "TrialBalance", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("TrialBalance", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.TrialBalance);
			if (string.Equals(className, "PayablesAging", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("PayablesAging", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("PayableAging", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("SubsidiaryPayable", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.PayablesAging);
			if (string.Equals(className, "ReceivableAging", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("ReceivableAging", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("ReceivablesAging", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.ReceivableAging);
			if (string.Equals(className, "Payables", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("Payables", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("Payable", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.Payables);
			if (string.Equals(className, "Receivables", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("Receivables", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("Receivable", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.Receivables);
			if (string.Equals(className, "VoucherReport", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("VoucherReport", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("VoucherValidation", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("FreightVoucher", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.VoucherReport);
			if (string.Equals(className, "BankBalances", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("BankBalances", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("BankCharges", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.BankBalances);
			if (string.Equals(className, "frmBankReconciliationWithVouchers", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("BankReconciliationWithVouchers", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("BankReconciliation", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.AdjustmentVouchers.frmBankReconciliationWithVouchers);
			if (string.Equals(className, "SupfrmBankDetail", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("SupBankDetail", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("BankDetail", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(SupfrmBankDetail);
			if (string.Equals(className, "AcfrmDefCoa", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("AcfrmDefCoa", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(AcfrmDefCoa);
			if (string.Equals(className, "supfrmDefineSupplier", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("supfrmDefineSupplier", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("DefineSupplier", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("PartyLimits", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("SupplierLimits", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Supplier_Purchase.supfrmDefineSupplier);
			if (string.Equals(className, "AcfrmChequebookRegistration", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("Cheque", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("Cheq", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.AcfrmChequebookRegistration);
			if (string.Equals(className, "CoaTitleChange", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("CoaTitleChange", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("ChartOfAccountManagement", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Reports.CoaTitleChange);
			if (string.Equals(className, "AcfrmAcAllocation", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("AcAllocation", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("AccountAllocation", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(AcfrmAcAllocation);
			if (string.Equals(className, "AcfrmOpeningBalance", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("OpeningBalance", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(AcfrmOpeningBalance);
			if (string.Equals(className, "frmAccountCustomGroup", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("CustomGroup", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("PartyCustomGroup", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(Architecture.WinApp.Account_Definition.frmAccountCustomGroup);
			if (string.Equals(className, "AcfrmDefineBank", StringComparison.OrdinalIgnoreCase) || targetUrl.IndexOf("DefineBank", StringComparison.OrdinalIgnoreCase) >= 0 || targetUrl.IndexOf("BankReconciliationUploadExcel", StringComparison.OrdinalIgnoreCase) >= 0) return typeof(AcfrmDefineBank);

			return null;
		}

		private Form CreateFormInstance(Type typ, UserAccount userAccount, DashboardNew dash = null)
		{
			if (typ == null) return null;

			try
			{
				if (dash != null)
				{
					System.Reflection.ConstructorInfo ctorDash = typ.GetConstructor(new[] { typeof(UserAccount), typeof(DashboardNew) });
					if (ctorDash != null)
					{
						return ctorDash.Invoke(new object[] { userAccount, dash }) as Form;
					}
				}

				System.Reflection.ConstructorInfo ctorUser = typ.GetConstructor(new[] { typeof(UserAccount) });
				if (ctorUser != null)
				{
					return ctorUser.Invoke(new object[] { userAccount }) as Form;
				}

				System.Reflection.ConstructorInfo ctorEmpty = typ.GetConstructor(Type.EmptyTypes);
				if (ctorEmpty != null)
				{
					return ctorEmpty.Invoke(null) as Form;
				}

				return Activator.CreateInstance(typ, userAccount) as Form;
			}
			catch (Exception ex)
			{
				Exception inner = ex.InnerException ?? ex;
				throw new Exception("Could not initialize screen '" + typ.Name + "': " + inner.Message, inner);
			}
		}

		private void UserControl_Click1(object sender, EventArgs e)
		{
			try
			{
				ScreenDynamicallyGenerateCards obj = (ScreenDynamicallyGenerateCards)sender;
				string TargetUrl = obj.Id;
				if (string.IsNullOrEmpty(TargetUrl))
				{
					MessageBox.Show("Target URL is not configured for " + obj.Title, "Screen Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}
				Type typ = ResolveType(TargetUrl);
				if (typ != null)
				{
					UserAccount.UserAccountAllocationsList = new List<Architecture.Model.UserAccountAllocation>();
					Architecture.Model.UserAccountAllocation allocation = new Architecture.Model.UserAccountAllocation();
					allocation.ScreenName = obj.ScreenName;
					UserAccount.UserAccountAllocationsList.Add(allocation);
					Form form = CreateFormInstance(typ, UserAccount, newdash);
					if (form != null)
					{
						form.TopLevel = false;
						form.Text = obj.Title;
						form.Tag = obj.ScreenName;
						form.WindowState = FormWindowState.Maximized;
						newdash.panel2.Controls.Add(form);
						HistoryStack.Push(form);
						form.Show();
						return;
					}
				}
				MessageBox.Show("Screen '" + obj.Title + "' (" + TargetUrl + ") is not configured or currently under development in this build.", "Module Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void ApplyThemeToAllControls(Control parent)
		{
			try
			{
				foreach (Control c in parent.Controls)
				{
					Panel panel = c as Panel;
					if (panel != null && (panel.BackColor == Color.Teal || panel.BackColor == Color.FromArgb(10, 110, 110)))
					{
						ThemeSettings themeSettings = clsGlobalVariables.GetThemeSettings.FirstOrDefault((ThemeSettings row) => row.ScreenName == "Screens");
						if (themeSettings != null)
						{
							panel.BackColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_BackColor);
							panel.ForeColor = ColorTranslator.FromHtml(themeSettings.HeaderPanel_FontColor);
						}
					}
					else
					{
						Button button = c as Button;
						if (button != null && (button.BackColor == Color.Teal || button.BackColor == Color.FromArgb(10, 110, 110)))
						{
							ThemeSettings themeSettings2 = clsGlobalVariables.GetThemeSettings.FirstOrDefault((ThemeSettings row) => row.ScreenName == "Screens");
							if (themeSettings2 != null)
							{
								button.BackColor = ColorTranslator.FromHtml(themeSettings2.Button_BackColor);
								button.ForeColor = ColorTranslator.FromHtml(themeSettings2.Button_FontColor);
							}
						}
					}
					if ((c is Form || c is Panel || c is Button) && c.HasChildren)
					{
						ApplyThemeToAllControls(c);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void DynamicallyGenerateCardsForModule()
		{
			try
			{
				accountsFlowLayout.Controls.Clear();
				dtForSKM.Clear();
				List<Architecture.Model.tblUserRights> lst = clsGlobalVariables.ScreenViewReights.Where((Architecture.Model.tblUserRights x) => x.AppId == ModuleId && x.Value).ToList();
				if (lst.Count <= 0)
				{
					return;
				}
				var modulelst = from x in lst
					group x by new { x.ModuleID, x.ModuleDescription, x.IconUrl } into lg
					select new
					{
						GroupColumn = lg.Key,
						TotalCount = lg.Count()
					};
				if (modulelst == null)
				{
					return;
				}
				ModuleDynamicallyGenerateCards[] AccountCardsList = new ModuleDynamicallyGenerateCards[modulelst.Count()];
				int i = 0;
				foreach (var Module in modulelst)
				{
					AccountCardsList[i] = new ModuleDynamicallyGenerateCards();
					AccountCardsList[i].Margin = new Padding(15, 5, 5, 5);
					AccountCardsList[i].Id = Conversion.ToString((object)Module.GroupColumn.ModuleID);
					AccountCardsList[i].Title = Conversion.ToString((object)Module.GroupColumn.ModuleDescription);
					AccountCardsList[i].Count = Conversion.ToString((object)Module.TotalCount);
					accountsFlowLayout.Controls.Add(AccountCardsList[i]);
					AccountCardsList[i].UserControlClicked += UserControl_Click;
					if (modulelst.Count() == 1)
					{
						UserControl_Click(AccountCardsList[i], null);
					}
					dtForSKM.Rows.Add(i, Conversion.ToInt((object)Module.GroupColumn.ModuleID), Conversion.ToString((object)Module.GroupColumn.ModuleDescription));
					i++;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void UserControl_Click(object sender, EventArgs e)
		{
			try
			{
				ModuleDynamicallyGenerateCards obj = (ModuleDynamicallyGenerateCards)sender;
				DynamicallyGenerateCards(Conversion.ToInt((object)obj.Id));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void frmMenue_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyCode == Keys.E)
				{
					Close();
				}
				if (e.Shift && e.KeyCode == Keys.M)
				{
				}
				if (e.Shift && e.KeyCode == Keys.S)
				{
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void frmMenue_FormClosed(object sender, FormClosedEventArgs e)
		{
			try
			{
				HistoryStack.Remove(this);
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.ScreensFlowlayout = new System.Windows.Forms.FlowLayoutPanel();
			this.accountsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblModuleName = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			base.SuspendLayout();
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.ScreensFlowlayout);
			this.panel1.Controls.Add(this.accountsFlowLayout);
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1112, 654);
			this.panel1.TabIndex = 1;
			this.ScreensFlowlayout.AutoScroll = true;
			this.ScreensFlowlayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ScreensFlowlayout.Location = new System.Drawing.Point(0, 224);
			this.ScreensFlowlayout.Name = "ScreensFlowlayout";
			this.ScreensFlowlayout.Size = new System.Drawing.Size(1112, 430);
			this.ScreensFlowlayout.TabIndex = 4;
			this.accountsFlowLayout.AutoScroll = true;
			this.accountsFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.accountsFlowLayout.Location = new System.Drawing.Point(0, 53);
			this.accountsFlowLayout.Name = "accountsFlowLayout";
			this.accountsFlowLayout.Size = new System.Drawing.Size(1112, 171);
			this.accountsFlowLayout.TabIndex = 3;
			this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
			this.panel2.Controls.Add(this.lblModuleName);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1112, 53);
			this.panel2.TabIndex = 2;
			this.lblModuleName.AutoSize = true;
			this.lblModuleName.Font = new System.Drawing.Font("Cambria", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblModuleName.ForeColor = System.Drawing.Color.White;
			this.lblModuleName.Location = new System.Drawing.Point(11, 10);
			this.lblModuleName.Name = "lblModuleName";
			this.lblModuleName.Size = new System.Drawing.Size(109, 32);
			this.lblModuleName.TabIndex = 0;
			this.lblModuleName.Text = "Screens";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1112, 654);
			base.Controls.Add(this.panel1);
			base.KeyPreview = true;
			base.Name = "frmMenue";
			this.Text = "Screens";
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmMenue_FormClosed);
			base.Load += new System.EventHandler(ApprovalDashboard_Load);
			base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmMenue_KeyDown);
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
