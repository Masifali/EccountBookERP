using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Lab;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Helper;

public static class FormHelper
{
	public enum ValidType
	{
		String,
		Double,
		INT,
		DateTime
	}

	public class WarehouseRackColumnMap
	{
		public string WarehouseId { get; set; }

		public string WarehouseName { get; set; }

		public string RackId { get; set; }

		public string RackName { get; set; }
	}

	private static HashSet<int> LoadIds = new HashSet<int> { 1, 3, 4 };

	private static HashSet<string> LoadNames = new HashSet<string> { "Load", "Load & PartyWeight", "Load & FactoryWeight" };

	private static HashSet<int> Ponchds = new HashSet<int> { 2, 5, 6 };

	private static HashSet<string> PonchNames = new HashSet<string> { "Ponch", "Ponch & PartyWeight", "Ponch & FactoryWeight" };

	public static bool DeliveryTermIsPonch(int deliveryTermId, string deliveryTerm)
	{
		if (!Ponchds.Contains(deliveryTermId))
		{
			return PonchNames.Contains(deliveryTerm);
		}
		return true;
	}

	public static bool DeliveryTermIsLoad(int deliveryTermId, string deliveryTerm)
	{
		if (!LoadIds.Contains(deliveryTermId))
		{
			return LoadNames.Contains(deliveryTerm);
		}
		return true;
	}

	public static bool AskForRemarks(string message, TextBox txtWarningRemarks)
	{
		MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		frmRemarks frm = new frmRemarks("Remarks Required");
		if (frm.ShowDialog() != DialogResult.OK)
		{
			return false;
		}
		string remarks = frm.RemarksText;
		if (string.IsNullOrWhiteSpace(remarks))
		{
			MessageBox.Show("Remarks are required to continue.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
		txtWarningRemarks.Text = remarks;
		return true;
	}

	public static void AddTabPageToTabControl(TabControl tabControl, string tabPageName, string tabPageText = null, int? index = null)
	{
		if (tabControl != null && !string.IsNullOrWhiteSpace(tabPageName) && tabControl.TabPages.Cast<TabPage>().FirstOrDefault((TabPage tp) => tp.Name.Equals(tabPageName, StringComparison.OrdinalIgnoreCase)) == null)
		{
			TabPage newTab = new TabPage
			{
				Name = tabPageName,
				Text = (string.IsNullOrWhiteSpace(tabPageText) ? tabPageName.Substring(3) : tabPageText)
			};
			if (index.HasValue && index.Value >= 0 && index.Value <= tabControl.TabPages.Count)
			{
				tabControl.TabPages.Insert(index.Value, newTab);
			}
			else
			{
				tabControl.TabPages.Add(newTab);
			}
		}
	}

	public static void SetDifference(TextBox txt1, TextBox txt2, TextBox result)
	{
		decimal value1 = Conversion.ToDecimal((object)txt1.Text);
		decimal value2 = Conversion.ToDecimal((object)txt2.Text);
		result.Text = (value1 - value2).ToString("#,##0.###");
	}

	public static void SetDifferenceForLable(Label txt1, Label txt2, Label result)
	{
		decimal value1 = Conversion.ToDecimal((object)txt1.Text);
		decimal value2 = Conversion.ToDecimal((object)txt2.Text);
		result.Text = (value1 - value2).ToString("#,##0.###");
	}

	public static void OpenFormInPanel(string screenName, Form formInstance, Panel PanelForViewForm)
	{
		try
		{
			Form existingForm = PanelForViewForm.Controls.OfType<Form>().FirstOrDefault((Form f) => f.Tag != null && f.Tag.ToString() == screenName);
			if (existingForm != null)
			{
				existingForm.BringToFront();
				return;
			}
			formInstance.Tag = screenName;
			formInstance.TopLevel = false;
			formInstance.AutoScroll = true;
			formInstance.WindowState = FormWindowState.Maximized;
			PanelForViewForm.Controls.Add(formInstance);
			PanelForViewForm.Dock = DockStyle.Fill;
			formInstance.MinimizeBox = false;
			formInstance.MaximizeBox = false;
			formInstance.ControlBox = false;
			formInstance.FormBorderStyle = FormBorderStyle.FixedSingle;
			formInstance.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static T OpenOrGetFormInPanel<T>(string screenName, Func<T> formFactory, Panel panel) where T : Form
	{
		try
		{
			Form existingForm = panel.Controls.OfType<Form>().FirstOrDefault((Form f) => f.Tag != null && f.Tag.ToString() == screenName);
			if (existingForm != null)
			{
				existingForm.BringToFront();
				return existingForm as T;
			}
			T formInstance = formFactory();
			formInstance.Tag = screenName;
			formInstance.TopLevel = false;
			formInstance.AutoScroll = true;
			formInstance.WindowState = FormWindowState.Maximized;
			formInstance.MinimizeBox = false;
			formInstance.MaximizeBox = false;
			formInstance.ControlBox = false;
			formInstance.FormBorderStyle = FormBorderStyle.FixedSingle;
			panel.Controls.Add(formInstance);
			panel.Dock = DockStyle.Fill;
			formInstance.Show();
			return formInstance;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return null;
		}
	}

	public static bool ValidateControls(List<(Control control, string fieldName, ValidType validationType)> controlsToValidate)
	{
		try
		{
			foreach (var item in controlsToValidate)
			{
				Control control = item.control;
				string fieldName = item.fieldName;
				ValidType validationType = item.validationType;
				UltraCombo combo = (UltraCombo)(object)((control is UltraCombo) ? control : null);
				if (combo != null)
				{
					if (validationType == ValidType.String)
					{
						if (string.IsNullOrWhiteSpace(((Control)(object)combo).Text))
						{
							MessageBox.Show(fieldName + " field is required");
							combo.Focus();
							return false;
						}
					}
					else if (((UltraGridBase)combo).ActiveRow == null || Conversion.ToInt(combo.Value) == 0)
					{
						MessageBox.Show(fieldName + " field is required");
						combo.Focus();
						return false;
					}
				}
				else if (control is TextBox textBox)
				{
					switch (validationType)
					{
					case ValidType.String:
						if (string.IsNullOrWhiteSpace(textBox.Text))
						{
							MessageBox.Show(fieldName + " field is required");
							textBox.Focus();
							return false;
						}
						break;
					case ValidType.Double:
					{
						if (!double.TryParse(textBox.Text, out var value) || value == 0.0)
						{
							MessageBox.Show(fieldName + " must be a non-zero number");
							textBox.Focus();
							return false;
						}
						break;
					}
					case ValidType.INT:
					{
						if (!int.TryParse(textBox.Text, out var value2) || value2 == 0)
						{
							MessageBox.Show(fieldName + " must be a non-zero number");
							textBox.Focus();
							return false;
						}
						break;
					}
					}
				}
				else if (control is DateTimePicker dateTime && Conversion.CheckDateTimeNull(dateTime.Value))
				{
					MessageBox.Show(fieldName + " is not valid");
					dateTime.Focus();
					return false;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			return false;
		}
	}

	public static void EditMethodFromLinkedForPartyProcessing(int documentTypeId, int recId, int baseDocumentTypeId = 0)
	{
		try
		{
			UserAccount userAccount = clsGlobalVariables.UserAccount;
			if (new Dictionary<int, (string, string, string, string)>
			{
				{
					49,
					("Architecture.WinApp.PartyProcessing.PartyProGrnAndGdn", "PartyProGrnAndGdn", "GoodsReceivingNotesPartyProcessing", "ReadById")
				},
				{
					89,
					("Architecture.WinApp.PartyProcessing.PartyProGrnAndGdn", "PartyProGrnAndGdn", "GoodsDispatchNotesPartyProcessing", "ReadById")
				},
				{
					44,
					("Architecture.WinApp.PartyProcessing.frmGrnGdnStorePartyProcessing", "frmGrnGdnStorePartyProcessing", "GRNStorePartyProcessing", "ReadById")
				},
				{
					45,
					("Architecture.WinApp.PartyProcessing.frmGrnGdnStorePartyProcessing", "frmGrnGdnStorePartyProcessing", "GDNStorePartyProcessing", "ReadById")
				},
				{
					121,
					("Architecture.WinApp.PartyProcessing.StockOpeningBalancePartyProcessing", "StockOpeningBalancePartyProcessing", "StockOpeningBalancePartyProcessing", "ReadById")
				},
				{
					158,
					("Architecture.WinApp.PartyProcessing.StockAdjustment", "StockAdjustment", "StockAdjustment", "ReadById")
				},
				{
					220,
					("Architecture.WinApp.PartyProcessing.StockTransfer", "StockTransfer", "StockTransfer", "ReadById")
				},
				{
					176,
					("Architecture.WinApp.PartyProcessing.invfrmStockConversionPartyProcessing", "invfrmStockConversionPartyProcessing", "invfrmStockConversionPartyProcessing", "ReadById")
				},
				{
					123,
					("Architecture.WinApp.PartyProcessing.ProductionPartyProcessingBill", "ProductionPartyProcessingBill", "ProductionPartyProcessingBill", "ReadById")
				},
				{
					117,
					("Architecture.WinApp.PartyProcessing.frmProductionPartyProcessing", "frmProductionPartyProcessing", "frmProductionPartyProcessing", "ReadByIdInput")
				},
				{
					118,
					("Architecture.WinApp.PartyProcessing.frmProductionPartyProcessing", "frmProductionPartyProcessing", "frmProductionPartyProcessing", "ReadByIdOutPut")
				}
			}.TryGetValue(documentTypeId, out var config) && JustCheckCanOpenForm(config.Item2, userAccount))
			{
				Type formType = Type.GetType(config.Item1);
				if (formType == null)
				{
					throw new Exception("Target URL not configured properly: " + config.Item1);
				}
				if (!(Activator.CreateInstance(formType, userAccount) is Form formInstance))
				{
					throw new Exception("Failed to create form instance: " + config.Item1);
				}
				formInstance.WindowState = FormWindowState.Maximized;
				formInstance.Tag = config.Item3;
				switch (documentTypeId)
				{
				case 117:
					SetTabIndex(formInstance, "tabControlMain", 0);
					SetTabIndex(formInstance, "tabControlInput", 0);
					break;
				case 118:
					SetTabIndex(formInstance, "tabControlMain", 1);
					SetTabIndex(formInstance, "tabControlOutPut", 0);
					break;
				}
				formInstance.Show();
				MethodInfo method = formType.GetMethod(config.Item4);
				if (method == null)
				{
					throw new Exception("Method " + config.Item4 + " not found on form " + formType.FullName);
				}
				method.Invoke(formInstance, new object[1] { recId });
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public static void PrintMethodFromLink(int id, int documentTypeId, string targetUrl, string screenName)
	{
		try
		{
			HashSet<string> deliveryOrderScreens = new HashSet<string> { "DeliveryOrder", "ExportDeliveryOrderB", "ExportDeliveryOrderNew", "DeliveryOrderTransfer" };
			HashSet<string> purchaseInvoiceScreens = new HashSet<string> { "frmPurchaseInvoiceDirectStore", "frmPurchaseInvoiceDirectStoreWithVariant" };
			string method = "";
			DataTable dt = ReportsMethod.FormHistory();
			if (dt.Rows.Count > 0)
			{
				method = (from row in dt.AsEnumerable()
					where row.Field<string>("ScreenName") == screenName || row.Field<int>("RefDocumentTypeId") == documentTypeId
					select row.Field<string>("ReportMethod")).FirstOrDefault();
			}
			if (!string.IsNullOrEmpty(method))
			{
				Type type = typeof(CommonServices);
				MethodInfo methodInfo = type.GetMethod(method);
				type = typeof(CrystalReportPrint_Helper);
				type.GetMethod(method);
				if (methodInfo != null)
				{
					CommonServices instance = new CommonServices();
					object[] parameters = ((methodInfo.GetParameters().Length != 1) ? new object[2] { id, documentTypeId } : new object[1] { id });
					methodInfo.Invoke(instance, parameters);
				}
				else if (methodInfo != null)
				{
					CommonServices instance2 = new CommonServices();
					object[] parameters2 = ((methodInfo.GetParameters().Length != 1) ? new object[2] { id, documentTypeId } : new object[1] { id });
					methodInfo.Invoke(instance2, parameters2);
				}
				else
				{
					MessageBox.Show("Method " + method + " not found on " + type.FullName);
				}
			}
			else if (!string.IsNullOrWhiteSpace(screenName))
			{
				if (deliveryOrderScreens.Contains(screenName))
				{
					CommonServices.InvDeliveryOrderSlip(id, documentTypeId);
				}
				else if (purchaseInvoiceScreens.Contains(screenName))
				{
					CommonServices.PurchaseInvoiceStoreDirectSlip(id, documentTypeId);
				}
				else if (screenName == "ExportPartyDefines")
				{
					CommonServices.InvRptSupplierSlip_293(id);
				}
				else if (screenName == "FoodProductionWithValues")
				{
					switch (documentTypeId)
					{
					case 80:
					case 112:
						CommonServices.ProductionInPutAndOutPut601(id);
						break;
					case 181:
						CommonServices.ProductionConsumptionReport_623(id);
						break;
					case 110:
						CommonServices.ProductionPMAndOverHeads_606(id, "OverHead");
						break;
					case 111:
						CommonServices.ProductionPMAndOverHeads_606(id, "PackingMaterial");
						break;
					}
				}
			}
			else if (documentTypeId == 89)
			{
				CrystalReportPrint_Helper.GdnSlipPartyProcessing339(id);
			}
			else if (documentTypeId == 49)
			{
				CrystalReportPrint_Helper.GrnSlipPartyProcessing333(id);
			}
			else if (documentTypeId == 121)
			{
				CommonServices.StockOpeningBalancePartyProcSlipandRegister416_01(id);
			}
			else if (documentTypeId == 158)
			{
				CommonServices.StockAdjustmentPartyProcessingSlip(id);
			}
			else if (documentTypeId == 220)
			{
				CommonServices.StockTransferPartyProcessingSlip(id);
			}
			else if (documentTypeId == 44)
			{
				CommonServices.PartyProcessingGRNSlip332(id);
			}
			else if (documentTypeId == 45)
			{
				CommonServices.PartyProcessingGDNSlip332_01(id);
			}
			else if (documentTypeId == 117 || documentTypeId == 118)
			{
				CrystalReportPrint_Helper.ProductionPartyProcessingSlip601_01(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	private static void SetTabIndex(Form form, string tabControlName, int index)
	{
		FieldInfo tabControlField = form.GetType().GetField(tabControlName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		if (tabControlField != null && tabControlField.GetValue(form) is TabControl tabControl && index >= 0 && index < tabControl.TabPages.Count)
		{
			tabControl.SelectedIndex = index;
		}
	}

	public static bool JustCheckCanOpenForm(string screenName, UserAccount user)
	{
		int num;
		if (!(user.RoleName == "Admin"))
		{
			num = (clsGlobalVariables.ScreenViewReights.Any((tblUserRights sc) => sc.ScreenName == screenName) ? 1 : 0);
			if (num == 0)
			{
				MessageBox.Show("You don't have rights to view this form.");
			}
		}
		else
		{
			num = 1;
		}
		return (byte)num != 0;
	}

	public static bool CanOpenForm(string screenName, Action formAction, UserAccount user)
	{
		bool haveRight = clsGlobalVariables.ScreenViewReights.Any((tblUserRights sc) => sc.ScreenName == screenName);
		if (user.RoleName == "Admin" || haveRight)
		{
			formAction();
			return true;
		}
		MessageBox.Show("You Don't Have rights To View This Form..");
		return false;
	}

	public static bool ConfirmAction(string Message)
	{
		return MessageBox.Show(Message + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
	}

	public static void ValidateField(object value, string fieldName, int rowIndex, string GridName = "Detail Grid")
	{
		if (value == null || (value is int && (int)value == 0) || (value is double d && d <= 0.0) || (value is decimal i && i <= 0m) || (value is float f && f <= 0f) || (value is string s && string.IsNullOrWhiteSpace(s)) || (value is DateTime dt && Conversion.CheckDateTimeNull(dt)))
		{
			throw new Exception($"{fieldName} is required in {GridName} at row No: {rowIndex + 1}");
		}
	}

	public static void UpdateAttachmentsForObject<T>(T obj, string screenName, Attachment AT, string atSaveListName, string atDeleteListName)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Expected O, but got Unknown
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		PropertyInfo attachmentsListProp = typeof(T).GetProperty(atSaveListName);
		if (attachmentsListProp == null)
		{
			throw new ArgumentException("Property '" + atSaveListName + "' not found on type " + typeof(T).Name);
		}
		PropertyInfo deleteAttachmentsListProp = typeof(T).GetProperty(atDeleteListName);
		if (deleteAttachmentsListProp == null)
		{
			throw new ArgumentException("Property '" + atDeleteListName + "' not found on type " + typeof(T).Name);
		}
		IList<DMSAttachments> attachmentsList = attachmentsListProp.GetValue(obj) as IList<DMSAttachments>;
		IList<DMSAttachments> deleteAttachmentsList = deleteAttachmentsListProp.GetValue(obj) as IList<DMSAttachments>;
		if (attachmentsList == null)
		{
			attachmentsList = new List<DMSAttachments>();
			attachmentsListProp.SetValue(obj, attachmentsList);
		}
		if (deleteAttachmentsList == null)
		{
			deleteAttachmentsList = new List<DMSAttachments>();
			deleteAttachmentsListProp.SetValue(obj, deleteAttachmentsList);
		}
		string attachmentsValues = string.Empty;
		if (AT?.lst != null && AT.lst.Count > 0)
		{
			for (int i = 0; i < AT.lst.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments
				{
					Attachment = Conversion.ToString((object)AT.lst[i].Path),
					UploadedFileCustomName = Conversion.ToString((object)AT.lst[i].CustomFileName),
					Id = Conversion.ToInt((object)AT.lst[i].ActionId),
					ScreenName = screenName
				};
				attachmentsValues = ((!string.IsNullOrEmpty(attachmentsValues)) ? (attachmentsValues + "," + attach.Attachment) : attach.Attachment);
				attachmentsList.Add(attach);
			}
		}
		if (AT?.RemovedAttachmentListInUpdateCase != null && AT.RemovedAttachmentListInUpdateCase.Count > 0)
		{
			foreach (TempAttachments removed in AT.RemovedAttachmentListInUpdateCase)
			{
				DMSAttachments delete = new DMSAttachments
				{
					Attachment = Path.GetFileName(removed.Path),
					ScreenName = screenName
				};
				deleteAttachmentsList.Add(delete);
			}
		}
		PropertyInfo attachValuesProp = typeof(T).GetProperty("AttachmentsValues");
		if (attachValuesProp != null && attachValuesProp.CanWrite)
		{
			attachValuesProp.SetValue(obj, attachmentsValues ?? string.Empty);
		}
	}

	public static void ShowWagesBillIfNeeded(bool Status, UserAccount User, int documentTypeId, int RecId, double grossWeight)
	{
		if (Status)
		{
			frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(User);
			frmwagesBillHeader.RefDocTypeId = documentTypeId;
			frmwagesBillHeader.RefDocId = RecId;
			frmwagesBillHeader.GrossWeightTotal = grossWeight;
			frmwagesBillHeader.ShowDialog();
			frmwagesBillHeader.BringToFront();
		}
	}

	public static void ShowLabImagesPreview(string PicPath)
	{
		if (File.Exists(PicPath))
		{
			Image image = Image.FromFile(PicPath);
			LabImagesPreview labImagesPreview = new LabImagesPreview();
			labImagesPreview.ImageToShow = image;
			labImagesPreview.Show();
		}
	}

	public static void LoadAttachmentsForObject(Attachment AT, int recId, string screenName = null, int documentTypeId = 0)
	{
		List<DMSAttachments> dmsList = ((documentTypeId > 0) ? DMSAttachments.ReadAttachmentsbyRefDocumentTypeId(recId, documentTypeId) : DMSAttachments.GetByID(recId, screenName));
		if (dmsList == null)
		{
			return;
		}
		AT.lst.Clear();
		foreach (DMSAttachments dmsAttachment in dmsList)
		{
			TempAttachments tempAttachment = new TempAttachments
			{
				Id = 0,
				ActionId = 2,
				Path = dmsAttachment.Attachment,
				CustomFileName = dmsAttachment.UploadedFileCustomName
			};
			AT.lst.Add(tempAttachment);
		}
	}

	public static void DeleteAllAttachmentsFromDb(Attachment AT, int RecId, int DocumentTypeId, string screenName)
	{
		if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
		{
			DMSAttachments.RemoveByIdAndNames(RecId, screenName, DocumentTypeId, 0);
		}
	}

	private static void DecimalTextBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private static void NumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	public static void FormEventHandler(Control parent)
	{
		foreach (Control c in parent.Controls)
		{
			if (c is TextBox textBox)
			{
				string tagString = Convert.ToString(textBox.Tag);
				string ColumnName = Convert.ToString(textBox.Name);
				if (!(tagString == "double"))
				{
					if (tagString == "int")
					{
						textBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
						{
							CommonServices.OnlytextNumberFunction(sender, e);
						};
					}
				}
				else if (ColumnName.Contains("Less"))
				{
					textBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
					{
						CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
					};
				}
				else
				{
					textBox.KeyPress += delegate(object sender, KeyPressEventArgs e)
					{
						CommonServices.OnlytextdecimelFunction(sender, e);
					};
				}
			}
			if (c.HasChildren)
			{
				FormEventHandler(c);
			}
		}
	}

	public static void SetCtrlGrdBarRight(CtrlGrdBar ctrlGrd, Rightsobjects formright)
	{
		ctrlGrd.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
		ctrlGrd.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
		ctrlGrd.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
		ctrlGrd.mGridExport.Enabled = formright.DoHaveGridExportRights;
		ctrlGrd.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
		ctrlGrd.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
	}

	public static (int WarehouseId, string WarehouseName, int RackId, string RackName) ResolveWarehouseAndRack(List<RacksWithWarehouseAndItem> itemData, int configWarehouseId)
	{
		if (itemData == null || itemData.Count == 0)
		{
			return default((int, string, int, string));
		}
		RacksWithWarehouseAndItem baseRack = itemData.FirstOrDefault((RacksWithWarehouseAndItem x) => x.BaseRackId > 0);
		if (baseRack != null)
		{
			return (WarehouseId: baseRack.WarehouseId, WarehouseName: baseRack.WareHouseName, RackId: baseRack.Id, RackName: baseRack.RackName);
		}
		List<RacksWithWarehouseAndItem> warehouses = (from x in itemData
			group x by x.WarehouseId into g
			select g.First()).ToList();
		if (warehouses.Count == 1)
		{
			RacksWithWarehouseAndItem wh2 = warehouses[0];
			List<RacksWithWarehouseAndItem> racks = (from x in itemData
				where x.WarehouseId == wh2.WarehouseId
				group x by x.Id into g
				select g.First()).ToList();
			if (racks.Count == 1)
			{
				return (WarehouseId: wh2.WarehouseId, WarehouseName: wh2.WareHouseName, RackId: racks[0].Id, RackName: racks[0].RackName);
			}
			return (WarehouseId: wh2.WarehouseId, WarehouseName: wh2.WareHouseName, RackId: 0, RackName: null);
		}
		if (configWarehouseId > 0)
		{
			RacksWithWarehouseAndItem wh = warehouses.FirstOrDefault((RacksWithWarehouseAndItem x) => x.WarehouseId == configWarehouseId);
			if (wh != null)
			{
				List<RacksWithWarehouseAndItem> racks2 = (from x in itemData
					where x.WarehouseId == wh.WarehouseId
					group x by x.Id into g
					select g.First()).ToList();
				if (racks2.Count == 1)
				{
					return (WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: racks2[0].Id, RackName: racks2[0].RackName);
				}
				return (WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: 0, RackName: null);
			}
		}
		return default((int, string, int, string));
	}

	public static void ResolveWarehouseRack(DataRow dr, int itemId, Dictionary<int, List<RacksWithWarehouseAndItem>> rackLookup, WarehouseRackColumnMap map)
	{
		if (map == null)
		{
			throw new ArgumentNullException("map");
		}
		int warehouseId = (dr.Table.Columns.Contains(map.WarehouseId) ? Conversion.ToInt(dr[map.WarehouseId]) : 0);
		int rackId = (dr.Table.Columns.Contains(map.RackId) ? Conversion.ToInt(dr[map.RackId]) : 0);
		if (warehouseId > 0 && rackId > 0)
		{
			return;
		}
		int defaultWarehouseId = (((from x in clsGlobalVariables.getGlobalAllItems
			where x.Id == itemId
			select x.InventoryParentCategoriesId).FirstOrDefault() == 2) ? Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow")) : Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse")));
		if (!rackLookup.TryGetValue(itemId, out var itemData))
		{
			return;
		}
		(int, string, int, string) result = ResolveWarehouseAndRack(itemData, defaultWarehouseId);
		if (warehouseId == 0 && result.Item1 > 0)
		{
			dr[map.WarehouseId] = result.Item1;
			if (!string.IsNullOrEmpty(map.WarehouseName) && dr.Table.Columns.Contains(map.WarehouseName))
			{
				dr[map.WarehouseName] = result.Item2;
			}
		}
		if (rackId == 0 && result.Item3 > 0)
		{
			dr[map.RackId] = result.Item3;
			if (!string.IsNullOrEmpty(map.RackName) && dr.Table.Columns.Contains(map.RackName))
			{
				dr[map.RackName] = result.Item4;
			}
		}
	}

	public static void SetObjectData<T>(Form frm, T obj)
	{
		try
		{
			Type typeFromHandle = typeof(T);
			string getItemName = string.Empty;
			Type getItemType = null;
			PropertyInfo[] array = (from x in typeFromHandle.GetProperties()
				where !x.GetMethod.IsVirtual
				select x).ToArray();
			foreach (PropertyInfo item in array)
			{
				getItemName = item.Name;
				getItemType = item.PropertyType;
				Control ctlTxt = frm.Controls.Find("txt" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlTxt != null && ctlTxt.Text != string.Empty)
				{
					item.SetValue(obj, Convert.ChangeType(ctlTxt.Text, getItemType), null);
					continue;
				}
				Control ctlMsk = frm.Controls.Find("msk" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlMsk != null && ctlMsk.Text != string.Empty)
				{
					item.SetValue(obj, Convert.ChangeType(ctlMsk.Text, getItemType), null);
					continue;
				}
				Control ctlCmb = frm.Controls.Find("cmb" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlCmb != null)
				{
					ComboBox cmb = (ComboBox)ctlCmb;
					item.SetValue(obj, Convert.ChangeType(cmb.SelectedValue, getItemType), null);
					continue;
				}
				Control ctlDat = frm.Controls.Find("dat" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlDat != null && ctlDat.Text != string.Empty)
				{
					item.SetValue(obj, Convert.ChangeType(ctlDat.Text, getItemType), null);
					continue;
				}
				Control ctlChk = frm.Controls.Find("chk" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlChk != null)
				{
					CheckBox chk = (CheckBox)ctlChk;
					item.SetValue(obj, Convert.ChangeType(chk.Checked, getItemType), null);
					continue;
				}
				Control ctlPic = frm.Controls.Find("pic" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlPic != null)
				{
					MemoryStream ms = new MemoryStream();
					PictureBox pic = (PictureBox)ctlPic;
					pic.Image.Save(ms, pic.Image.RawFormat);
					item.SetValue(obj, Convert.ChangeType(ms.GetBuffer(), getItemType), null);
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void SetObjectDataInfragastic<T>(Form frm, T obj)
	{
		try
		{
			Type typeFromHandle = typeof(T);
			string getItemName = string.Empty;
			Type getItemType = null;
			PropertyInfo[] array = (from x in typeFromHandle.GetProperties()
				where !x.GetMethod.IsVirtual
				select x).ToArray();
			foreach (PropertyInfo item in array)
			{
				getItemName = item.Name;
				getItemType = item.PropertyType;
				Control ctlTxt = frm.Controls.Find("txt" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlTxt != null && ctlTxt.Text != string.Empty)
				{
					item.SetValue(obj, Convert.ChangeType(ctlTxt.Text, getItemType), null);
					continue;
				}
				Control ctlMsk = frm.Controls.Find("msk" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlMsk != null && ctlMsk.Text != string.Empty)
				{
					item.SetValue(obj, Convert.ChangeType(ctlMsk.Text, getItemType), null);
					continue;
				}
				Control control = frm.Controls.Find("cmb" + getItemName, searchAllChildren: false).FirstOrDefault();
				UltraCombo ctlCmb = (UltraCombo)(object)((control is UltraCombo) ? control : null);
				if (ctlCmb != null)
				{
					UltraCombo cmb = ctlCmb;
					item.SetValue(obj, Convert.ChangeType(cmb.Value, getItemType), null);
					continue;
				}
				Control ctlDat = frm.Controls.Find("dat" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlDat != null && ctlDat.Text != string.Empty)
				{
					item.SetValue(obj, Convert.ChangeType(ctlDat.Text, getItemType), null);
					continue;
				}
				Control ctlChk = frm.Controls.Find("chk" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlChk != null)
				{
					CheckBox chk = (CheckBox)ctlChk;
					item.SetValue(obj, Convert.ChangeType(chk.Checked, getItemType), null);
					continue;
				}
				Control ctlPic = frm.Controls.Find("pic" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlPic != null)
				{
					MemoryStream ms = new MemoryStream();
					PictureBox pic = (PictureBox)ctlPic;
					pic.Image.Save(ms, pic.Image.RawFormat);
					item.SetValue(obj, Convert.ChangeType(ms.GetBuffer(), getItemType), null);
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void GetObjectData<T>(Form frm, T obj)
	{
		try
		{
			Type typeFromHandle = typeof(T);
			string getItemName = string.Empty;
			PropertyInfo[] array = (from x in typeFromHandle.GetProperties()
				where !x.GetMethod.IsVirtual
				select x).ToArray();
			foreach (PropertyInfo obj2 in array)
			{
				getItemName = obj2.Name;
				_ = obj2.PropertyType;
				object getValue = obj2.GetValue(obj);
				Control ctlTxt = frm.Controls.Find("txt" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlTxt != null && getValue != null)
				{
					ctlTxt.Text = getValue.ToString();
					continue;
				}
				Control ctlMsk = frm.Controls.Find("msk" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlMsk != null && getValue != null)
				{
					ctlMsk.Text = getValue.ToString();
					continue;
				}
				Control ctlCmb = frm.Controls.Find("cmb" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlCmb != null && getValue != null)
				{
					ctlCmb.Text = getValue.ToString();
					continue;
				}
				Control ctlDat = frm.Controls.Find("dat" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlDat != null && getValue != null)
				{
					ctlDat.Text = getValue.ToString();
					continue;
				}
				Control ctlChk = frm.Controls.Find("chk" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlChk != null && getValue != null)
				{
					((CheckBox)ctlChk).Checked = (bool)getValue;
					continue;
				}
				Control ctlPic = frm.Controls.Find("pic" + getItemName, searchAllChildren: false).FirstOrDefault();
				if (ctlPic != null && getValue != null)
				{
					PictureBox obj3 = (PictureBox)ctlPic;
					MemoryStream ms = new MemoryStream((byte[])getValue);
					obj3.Image = Image.FromStream(ms);
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void ControlReset(Form frm)
	{
		try
		{
			foreach (Control item in frm.Controls)
			{
				if (item.GetType() == typeof(TextBox))
				{
					item.Text = string.Empty;
				}
				if (item.GetType() == typeof(MaskedTextBox))
				{
					item.Text = string.Empty;
				}
				if (item.GetType() == typeof(DateTimePicker))
				{
					item.Text = DateTime.Now.ToString();
				}
				if (item.GetType() == typeof(CheckBox))
				{
					((CheckBox)item).Checked = false;
				}
				if (item.GetType() == typeof(PictureBox))
				{
					((PictureBox)item).Image = null;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void ControlStatus(Form frm, bool status)
	{
		try
		{
			foreach (Control item in frm.Controls)
			{
				if (!(item.GetType() == typeof(ToolStrip)) && !(item.GetType() == typeof(ToolStripButton)))
				{
					item.Enabled = status;
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static int GetInt(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		object val = ((obj != null) ? obj.Value : null);
		if (val != null)
		{
			return Conversion.ToInt(val);
		}
		return 0;
	}

	public static long GetInt64(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		object val = ((obj != null) ? obj.Value : null);
		if (val != null)
		{
			return Conversion.ToInt64(val);
		}
		return 0L;
	}

	public static double GetDouble(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		object val = ((obj != null) ? obj.Value : null);
		if (val != null)
		{
			return Conversion.ToDouble(val);
		}
		return 0.0;
	}

	public static decimal GetDecimal(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		object val = ((obj != null) ? obj.Value : null);
		if (val != null)
		{
			return Conversion.ToDecimal(val);
		}
		return 0m;
	}

	public static string GetString(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		object val = ((obj != null) ? obj.Value : null);
		if (val != null)
		{
			return Conversion.ToString(val);
		}
		return string.Empty;
	}

	public static string GetText(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		return ((obj != null) ? obj.Text : null) ?? string.Empty;
	}

	public static DateTime? GetDate(GridEXRow r, string col)
	{
		GridEXCell obj = r.Cells[col];
		object val = ((obj != null) ? obj.Value : null);
		if (val != null)
		{
			return Conversion.ToDateTime(val);
		}
		return null;
	}

	public static bool Require(bool condition, string field, GridEXRow r)
	{
		if (!condition)
		{
			return false;
		}
		MessageBox.Show($"{field} is required (Row {r.RowIndex + 1})");
		r.BeginEdit();
		return true;
	}
}
