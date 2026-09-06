using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.AttendanceManagement;
using Architecture.WinApp.HRM.PolicyManagment;
using Architecture.WinApp.HRM.ProfileManagement;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.HRM.EmployeeManagement;

public class frmEmployeeRegistration : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtAllLookUp = new DataTable();

	private DataTable dtemployee = new DataTable();

	private DataTable dtEmployeeSalaryGrid = new DataTable();

	private DataTable dtEmployeeAssetsBenefits = new DataTable();

	private DataTable dtEmployeeDirectBenefits = new DataTable();

	private DataTable dtWeekDays = new DataTable();

	private DataTable dtAddressDetail = new DataTable();

	private int AddressDetailupdateDetailIndex;

	private DataTable dtEmployeeExperience = new DataTable();

	private int EmployeeExperienceupdateDetailIndex;

	private DataTable dtEmployeeEducation = new DataTable();

	private int EmployeeEducationupdateDetailIndex;

	private DataTable dtEmployeeFamilyInfo = new DataTable();

	private int EmployeeFamilyInfoupdateDetailIndex;

	private DataTable dtEmployeeReference = new DataTable();

	private int EmployeeReferenceupdateDetailIndex;

	private DataTable dtEmployeeBankAccount = new DataTable();

	private int EmployeeBankAccountupdateDetailIndex;

	public Rightsobjects formright = new Rightsobjects();

	private int RecId;

	private bool UpdateMode;

	public static Image imgpreview;

	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePathcookingpic = "";

	private string ofdcookingpic = "";

	private DataTable dtReportedPersone = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnRefresh;

	private MaskedTextBox txtMobile1;

	private Button btnBloodGroup;

	private Button btnNationality;

	private Button btnGender;

	private Button btnReleaseTitle;

	private Button btnTitle;

	private Button btnEmployeeCategory;

	private Label label36;

	private Label label32;

	internal UltraCombo comproject;

	private Label label41;

	internal UltraCombo combranches;

	private Label label42;

	private Label label34;

	private Label label37;

	private Label label35;

	private Label label33;

	private Label label31;

	private Label label30;

	private Label label29;

	private Label label28;

	private Label label25;

	private PictureBox pictureBoxEmployeeSignature;

	internal UltraCombo cmbGender;

	internal UltraCombo cmbRelationTitle;

	internal UltraCombo cmbTitle;

	internal UltraCombo cmbBloodGroup;

	internal UltraCombo cmbEmployeeCatagory;

	internal UltraCombo cmbNationality;

	private Button btnEmployeeSignature;

	private Label label27;

	private Button btnEmployeePicture;

	private PictureBox pictureBoxEmployeePhoto;

	private Label label24;

	private Label label23;

	private Label label22;

	private DateTimePicker dtpCNICExpiryDate;

	private TextBox txtMobile2;

	private Label label21;

	private Label label20;

	private TextBox txtRelationMobileNo;

	private Label label19;

	private TextBox txtLastName;

	private Label label18;

	private MaskedTextBox txtCNIC;

	private TextBox txtLicenseNo;

	private Label label15;

	private TextBox txtPassportNo;

	private Label label16;

	private Label label13;

	private Label label14;

	private DateTimePicker dtpCNICIssueDate;

	private Label label11;

	private TextBox txtEmail;

	private Label label12;

	private Label label9;

	private Label label10;

	private DateTimePicker dtpDOB;

	private Label label7;

	private Label label8;

	private TextBox txtRelationName;

	private TextBox txtMiddleName;

	private Label label5;

	private TextBox txtFirstName;

	private Label label6;

	private Label label4;

	private Label label3;

	private TextBox txtEmployeeNo;

	private Label label1;

	private Panel panel6;

	private Panel panel8;

	private Panel panel7;

	internal GridEX grdEmployeeRegistration;

	private ToolStrip toolStrip2;

	private ToolStripButton btnPrintRegister;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel9;

	private TabControl tabControl3;

	private TabPage tabPage5;

	private TabPage tabPage6;

	private TabPage tabPage7;

	private TabPage tabPage8;

	private Panel panel10;

	private Button btnReportedPerson;

	private Button btnSection;

	private Button btnShift;

	private Button btnReportedBranch;

	private Button btnLocation;

	private Button btnDesignation;

	private Button btnStore;

	private Button btnDepartment;

	private Label label38;

	private TextBox txtSalary;

	internal UltraCombo cmbShift;

	internal UltraCombo cmbDesignation;

	internal UltraCombo cmbSection;

	internal UltraCombo cmbDepartment;

	internal UltraCombo cmbStore;

	internal UltraCombo cmbReportedPerson;

	internal UltraCombo cmbReportedBranch;

	internal UltraCombo cmbLocation;

	private Label label44;

	private Label label45;

	private Label label46;

	private Label label47;

	private Label label48;

	private Label label49;

	private Label label50;

	private TextBox txtSiteLocation;

	private Label lblReportedBranch;

	private Label label51;

	private Label label52;

	private DateTimePicker txtEndDate;

	private Label label53;

	private DateTimePicker txtStartDate;

	private Button btnEmployeeType;

	internal UltraCombo cmbEmployeeType;

	private Label label2;

	private Panel panel11;

	private Panel panel13;

	private Panel panel12;

	private Button btnGenerateBreakup;

	internal GridEX grdEmployeeSalary;

	private Panel panel14;

	private Panel panel15;

	private Panel panel16;

	private Panel panel17;

	internal GridEX grdEmployeeDirectBenefits;

	internal GridEX grdEmployeeAssetsBenefit;

	private Label label61;

	private Label label62;

	private Panel panel18;

	internal GridEX grdWeekDays;

	private Panel panel19;

	private TabControl tabControl4;

	private TabPage tabPage9;

	private TabPage tabPage10;

	private TabPage tabPage11;

	private TabPage tabPage12;

	private TabPage tabPage13;

	private TabPage tabPage14;

	private Panel panel20;

	private Panel panel21;

	private Panel panel22;

	internal GridEX grdAddressDetail;

	private Label label64;

	private TextBox txtAddressDetail;

	internal UltraCombo cmbProvince;

	private Label label65;

	internal UltraCombo cmbCity;

	private Label label66;

	internal UltraCombo cmbAddressType;

	private Label label67;

	internal UltraCombo cmbCountry;

	private Label label68;

	private Button btnUpdateAddressDetail;

	private Button btnAddAddressDetail;

	private Button btnCancelAddressDetail;

	private Panel panel23;

	private Panel panel25;

	internal GridEX grdEmployeeExperience;

	private Panel panel24;

	private TextBox txtDescription;

	private Label label72;

	private DateTimePicker dtpTo;

	private DateTimePicker dtpFrom;

	private TextBox txtOrganization;

	private Label label73;

	internal UltraCombo cmbCityEmployeeExperience;

	private Label label74;

	private Label label75;

	internal UltraCombo cmbDesignationEmployeeExperience;

	private Label label76;

	private Label label77;

	private Button btnCancelExperience;

	private Button btnUpdateExperience;

	private Button btnAddExperience;

	private Panel panel26;

	private Panel panel28;

	internal GridEX grdEmployeeEducation;

	private Panel panel27;

	private DateTimePicker PassingYearDate;

	private DateTimePicker StartYearDate;

	internal UltraCombo cmbDegree;

	private TextBox txtCGPA;

	private Label label78;

	private Label label79;

	private Label label80;

	private TextBox txtInstitute;

	private Label label81;

	private TextBox txtDegreeTitle;

	private Label label83;

	private Label label84;

	internal UltraCombo cmbCityEducation;

	private Label label85;

	private Button btnCancelEducation;

	private Button btnUpdateEducation;

	private Button btnAddEducation;

	private Panel panel29;

	private Panel panel31;

	private Panel panel30;

	private TextBox txtDescriptionFamilyInfo;

	private Label label90;

	private TextBox txtNameFamilyInfo;

	internal UltraCombo cmbRelationShip;

	private Label label91;

	private Label label87;

	private Button btnCancelFamily;

	private Button btnUpdateFamily;

	private Button btnAddFamily;

	internal GridEX grdFamilyInfo;

	private Label label17;

	private DateTimePicker txtProbationEndingDate;

	private Panel panel32;

	private Panel panel34;

	private Panel panel33;

	private TextBox txtDescriptionEmployeeRefrence;

	private Label label92;

	private TextBox txtEmailEmployeeRefrence;

	private Label label93;

	private TextBox txtMobileEmployeeRefrence;

	private Label label94;

	internal UltraCombo cmbDesignationEmployeeRefrence;

	private TextBox txtOrganizationEmployeeRefrence;

	private Label label95;

	private Label label98;

	private TextBox txtRefrenceName;

	private Label label100;

	internal GridEX grdReference;

	private Button btnCancelReference;

	private Button btnUpdateReference;

	private Button btnAddReference;

	private Panel panel35;

	private Panel panel37;

	private Panel panel36;

	private CheckBox IsForPayrollBankAccount;

	private TextBox txtAccountNo;

	private Label label101;

	private TextBox txtAccountTitle;

	private Label label102;

	internal UltraCombo cmbBankAccount;

	private Label label104;

	internal GridEX grdBankAccount;

	private Button btnCancelBankAccount;

	private Button btnUpdateBankAccount;

	private Button btnAddBankAccount;

	internal UltraCombo cmbEmployeeGroup;

	private Label label26;

	private Button btnEmployeeGroup;

	private CheckBox IsActiveEmployeeInformation;

	private CheckBox ChkAllBenefits;

	private Button btnGenerateAllBenefits;

	private TextBox txtEmployeeNoForUpdate;

	private Label label39;

	private Label label40;

	private Label label43;

	private ToolStripButton btnDutyRoasterForm;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton BtnShifttiming;

	private ToolStripSeparator toolStripSeparator2;

	private Button btnAddressType;

	private Button btnCityName;

	private Button btnDefineDegree;

	private Button btnSalaryBreakUp;

	private Button btnDefineBenefits;

	private Button btncoa2;

	private Button btncoa3;

	private Button btnCoa1;

	internal UltraCombo CmbSalaryLoanAc;

	internal UltraCombo CmbSalaryPayableAc;

	private Label label54;

	internal UltraCombo CmbSalariesExpensesAc;

	private Label label55;

	private Label label56;

	public frmEmployeeRegistration(UserAccount userAccount)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmEmployeeRegistration_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(980, 700);
			tabPage2.AutoScroll = true;
			tabPage2.AutoScrollMargin = new Size(20, 20);
			tabPage2.AutoScrollMinSize = new Size(980, 700);
			formright = CommonServices.SetRightsValueInRightsObject("EmployeeRegistration");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			Project();
			braches();
			AccountBinds();
			AllLookUpFill();
			EmployeeTypeComboFill();
			NationalityComboFill();
			BloodGroupComboFill();
			EmployeeCatagoryComboFill();
			EmployeeTitleComboFill();
			EmployeeRelationComboFill();
			EmployeeGenderComboFill();
			EmlpoyeeNoGenerate();
			EmployeeName();
			DepartmentNameComboFill();
			LocationNameComboFill();
			ReportedBranchComboFill();
			SectionNameComboFill();
			StoreNameComboFill();
			DesignationNameComboFill();
			ShiftNameComboFill();
			ReportedPersonComboFill();
			AddressTypeComboFill();
			CityFill();
			CountryFill();
			ProvinceFill();
			DegreeFill();
			RelationShipFill();
			BankName();
			EmployeeGroupComboFill();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			dtEmployeeSalaryGrid.Columns.Add("SalaryTypeId");
			dtEmployeeSalaryGrid.Columns.Add("SalaryType");
			dtEmployeeSalaryGrid.Columns.Add("TypePercent", typeof(double));
			dtEmployeeSalaryGrid.Columns.Add("Amount", typeof(double));
			grdEmployeeSalary.DataSource = dtEmployeeSalaryGrid;
			grdEmployeeSalary.RetrieveStructure();
			grdEmployeeSalarySetting();
			dtEmployeeDirectBenefits.Columns.Add("BenefitId", typeof(int));
			dtEmployeeDirectBenefits.Columns.Add("BenefitName");
			dtEmployeeDirectBenefits.Columns.Add("BenefitValue");
			dtEmployeeDirectBenefits.Columns.Add("ApprovalPersonId");
			dtEmployeeDirectBenefits.Columns.Add("Select", typeof(bool));
			dtEmployeeAssetsBenefits.Columns.Add("BenefitId", typeof(int));
			dtEmployeeAssetsBenefits.Columns.Add("BenefitName");
			dtEmployeeAssetsBenefits.Columns.Add("BenefitValue");
			dtEmployeeAssetsBenefits.Columns.Add("ApprovalPersonId");
			dtEmployeeAssetsBenefits.Columns.Add("Select", typeof(bool));
			grdEmployeeDirectBenefitFill();
			grdEmployeeAssetsBenefitFill();
			dtWeekDays.Columns.Add("WeekDaysProfileId");
			dtWeekDays.Columns.Add("WeekDays");
			dtWeekDays.Columns.Add("Select", typeof(bool));
			grdWeekDays.DataSource = dtWeekDays;
			grdWeekDays.RetrieveStructure();
			grdWeekDaysSetting();
			grdWeekDaysFill();
			dtAddressDetail.Columns.Add("AddressTypeId");
			dtAddressDetail.Columns.Add("AddressType");
			dtAddressDetail.Columns.Add("CountryId");
			dtAddressDetail.Columns.Add("Country");
			dtAddressDetail.Columns.Add("ProvinceId");
			dtAddressDetail.Columns.Add("Province");
			dtAddressDetail.Columns.Add("CityId");
			dtAddressDetail.Columns.Add("City");
			dtAddressDetail.Columns.Add("AddressDetail");
			grdAddressDetail.DataSource = dtAddressDetail;
			grdAddressDetail.RetrieveStructure();
			grdAddressDetailSetting();
			dtEmployeeExperience.Columns.Add("DesignationId");
			dtEmployeeExperience.Columns.Add("Designation");
			dtEmployeeExperience.Columns.Add("Organization");
			dtEmployeeExperience.Columns.Add("CityId");
			dtEmployeeExperience.Columns.Add("City");
			dtEmployeeExperience.Columns.Add("FromDate");
			dtEmployeeExperience.Columns.Add("ToDate");
			dtEmployeeExperience.Columns.Add("Description");
			grdEmployeeExperience.DataSource = dtEmployeeExperience;
			grdEmployeeExperience.RetrieveStructure();
			grdEmployeeExperienceSetting();
			dtEmployeeEducation.Columns.Add("DegreeId");
			dtEmployeeEducation.Columns.Add("Degree");
			dtEmployeeEducation.Columns.Add("DegreeTitle");
			dtEmployeeEducation.Columns.Add("Institute");
			dtEmployeeEducation.Columns.Add("CityId");
			dtEmployeeEducation.Columns.Add("City");
			dtEmployeeEducation.Columns.Add("StartYear");
			dtEmployeeEducation.Columns.Add("PassingYear");
			dtEmployeeEducation.Columns.Add("CGPA");
			grdEmployeeEducation.DataSource = dtEmployeeEducation;
			grdEmployeeEducation.RetrieveStructure();
			grdEmployeeEducationSetting();
			dtEmployeeFamilyInfo.Columns.Add("RelationShipId");
			dtEmployeeFamilyInfo.Columns.Add("RelationShip");
			dtEmployeeFamilyInfo.Columns.Add("RelationName");
			dtEmployeeFamilyInfo.Columns.Add("Description");
			grdFamilyInfo.DataSource = dtEmployeeFamilyInfo;
			grdFamilyInfo.RetrieveStructure();
			grdEmployeeFamilyInfoSetting();
			dtEmployeeReference.Columns.Add("ReferenceName");
			dtEmployeeReference.Columns.Add("OrganizationName");
			dtEmployeeReference.Columns.Add("DesignationId");
			dtEmployeeReference.Columns.Add("Designation");
			dtEmployeeReference.Columns.Add("Mobile");
			dtEmployeeReference.Columns.Add("Email");
			dtEmployeeReference.Columns.Add("Description");
			grdReference.DataSource = dtEmployeeReference;
			grdReference.RetrieveStructure();
			grdEmployeeReferenceSetting();
			dtEmployeeBankAccount.Columns.Add("BankId");
			dtEmployeeBankAccount.Columns.Add("Bank");
			dtEmployeeBankAccount.Columns.Add("AccountTitle");
			dtEmployeeBankAccount.Columns.Add("AccountNo");
			dtEmployeeBankAccount.Columns.Add("IsPayRoll");
			grdBankAccount.DataSource = dtEmployeeBankAccount;
			grdBankAccount.RetrieveStructure();
			grdBankAccountSetting();
			EmployeeBenefitsandWeekDaysCheckBoxNull();
			txtEndDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.End_Period);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllLookUpFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genProfile coa = new genProfile();
			coa.OrganizationId = UserAccount.OrganizationId;
			coa.CompanyId = UserAccount.CompanyId;
			coa.ProfilePrefix = "All";
			dtAllLookUp = genProfile.GetByProfileTypeId(coa);
			dtReportedPersone = new DataTable();
			dtReportedPersone.Columns.Add("EmployeeId");
			dtReportedPersone.Columns.Add("EmployeeName");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)34))
				{
					dtReportedPersone.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeGroupComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			DataTable dtGroup = genEmployeeGroup.Getall(new genEmployeeGroup
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGroup.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtGroup, cmbEmployeeGroup, "EmployeeGroupId", "EmployeeGroupName", "Employee Group", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeTypeComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeType");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)20))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbEmployeeType, "Id", "EmployeeType", "Employee Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NationalityComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Nationality");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)15))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbNationality, "Id", "Nationality", "Nationality", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BloodGroupComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("BloodGroup");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)22))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbBloodGroup, "Id", "BloodGroup", "Blood Group", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeCatagoryComboFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeCategory");
			DataTable dtEmployeeCategory = genEmployeeCategory.Getall(new genEmployeeCategory
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtEmployeeCategory.Rows.Count > 0)
			{
				for (int i = 0; i < dtEmployeeCategory.Rows.Count; i++)
				{
					dt.Rows.Add(dtEmployeeCategory.Rows[i]["EmployeeCategoryId"], dtEmployeeCategory.Rows[i]["EmployeeCategoryName"]);
				}
				DDL.BindDDL(dt, cmbEmployeeCatagory, "Id", "EmployeeCategory", "Employee Category", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeTitleComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeTitle");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)24))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbTitle, "Id", "EmployeeTitle", "Employee Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeRelationComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeRelation");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)25))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			DDL.BindDDL(dt, cmbRelationTitle, "Id", "EmployeeRelation", "Employee Relation", ZeroIndex: false);
			DDL.BindDDL(dt, cmbRelationShip, "Id", "EmployeeRelation", "Employee Relation", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeGenderComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeGender");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)26))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbGender, "Id", "EmployeeGender", "Employee Gender", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtProject = CommonServices.ProjectServiceBind();
			if (dtProject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).DisplayLayout.Bands[0].Columns[1].Width = 168;
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable dtBranch = CommonServices.BrancheServiceBind();
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBranch, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).DisplayLayout.Bands[0].Columns[1].Width = 180;
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmlpoyeeNoGenerate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int Code = genEmployee.GenerateEmployeeNo(new genEmployee
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Code > 0)
			{
				txtEmployeeNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			genEmployeeHistory obj = new genEmployeeHistory();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtemployee = genEmployeeHistory.GetAllActiveEmployee(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDepartment = genDepartment.Getall(new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDepartment, cmbDepartment, "DepartmentId", "DepartmentName", "Department Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtLocation = genLocation.Getall(new genLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLocation.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLocation, cmbLocation, "LocationId", "LocationName", "Location Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportedBranchComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ReportedBranch");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)28))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbReportedBranch, "Id", "ReportedBranch", "ReportedBranch", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SectionNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtSection = genSection.Getall(new genSection
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtSection.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSection, cmbSection, "SectionId", "SectionName", "Section Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StoreNameComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("StoreName");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)31))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbStore, "Id", "StoreName", "Store Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DesignationNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = genDesignation.Getall(new genDesignation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbDesignation, "DesignationId", "DesignationName", "Designation Name", false);
				DDL.BindDDLNew(dt, cmbDesignationEmployeeExperience, "DesignationId", "DesignationName", "Designation Name", false);
				DDL.BindDDLNew(dt, cmbDesignationEmployeeRefrence, "DesignationId", "DesignationName", "Designation Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShiftNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = genShift.Getall(new genShift
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbShift, "ShiftId", "ShiftName", "ShiftName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportedPersonComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ReportedPerson");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)34))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbReportedPerson, "Id", "ReportedPerson", "Reported Person", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddressTypeComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AddressType");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (dtAllLookUp.Rows.Count > 0 && Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)59))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbAddressType, "Id", "AddressType", "Address Type", ZeroIndex: false);
				((UltraGridBase)cmbAddressType).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CountryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable dtCountry = country.GetAll(new Country());
			if (dtCountry.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCountry, cmbCountry, "Id", "Description", "Country Name", false);
				((UltraGridBase)cmbCountry).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCity = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCity, cmbCity, "Id", "CityName", "City Name", false);
				DDL.BindDDLNew(dtCity, cmbCityEmployeeExperience, "Id", "CityName", "City Name", false);
				DDL.BindDDLNew(dtCity, cmbCityEducation, "Id", "CityName", "City Name", false);
				((UltraGridBase)cmbCity).DisplayLayout.Bands[0].Columns[1].Width = 165;
				((UltraGridBase)cmbCityEmployeeExperience).DisplayLayout.Bands[0].Columns[1].Width = 165;
				((UltraGridBase)cmbCityEducation).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProvinceFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtProvice = StateProvince.GetAll(new StateProvince
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProvice.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProvice, cmbProvince, "Id", "Description", "Province Name", false);
				((UltraGridBase)cmbProvince).DisplayLayout.Bands[0].Columns[1].Width = 165;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DegreeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Degree");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)57))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbDegree, "Id", "Degree", "Degree", ZeroIndex: false);
				((UltraGridBase)cmbDegree).DisplayLayout.Bands[0].Columns[1].Width = 191;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RelationShipFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			for (int i = 0; i < dtAllLookUp.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAllLookUp.Rows[i]["ProfileTypeId"]) == Conversion.ToInt((object)47))
				{
					dt.Rows.Add(dtAllLookUp.Rows[i]["ProfileId"], dtAllLookUp.Rows[i]["ProfileName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbRelationShip, "Id", "Name", "Relationship", ZeroIndex: false);
				((UltraGridBase)cmbRelationShip).DisplayLayout.Bands[0].Columns[1].Width = 191;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Bank.GetAllDt(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbBankAccount, "Id", "BranchName", "Bank Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountBinds()
	{
		try
		{
			DataTable dtCom = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,20,21");
			if (dtCom.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCom, CmbSalariesExpensesAc, "Id", "AccountTitle", "Expense Account", true);
			}
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("5,8");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbSalaryPayableAc, "Id", "AccountTitle", "Payable Account", true);
				DDL.BindDDLNew(dt, CmbSalaryLoanAc, "Id", "AccountTitle", "Loan Account", true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
			ResetEmployeeInformation();
			ResetEmployeeSalary();
			ResetEmployeeBenefits();
			ResetWeekDays();
			ResetAddressDetail();
			ResetEmployeeExperience();
			ResetEmployeeEducation();
			ResetEmployeeFamilyInfo();
			ResetReference();
			ResetBankAccount();
			EmployeeBenefitsandWeekDaysCheckBoxNull();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Expected O, but got Unknown
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Expected O, but got Unknown
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Expected O, but got Unknown
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d63: Expected O, but got Unknown
		//IL_0e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e88: Expected O, but got Unknown
		//IL_101a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1021: Expected O, but got Unknown
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_120c: Expected O, but got Unknown
		//IL_135a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1361: Expected O, but got Unknown
		//IL_14f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fe: Expected O, but got Unknown
		try
		{
			if (!ValidationEmployeeRegistration())
			{
				return;
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			DateTime startDate = dtpCNICIssueDate.Value;
			DateTime endDate = dtpCNICExpiryDate.Value;
			if (startDate.AddYears(10) < endDate)
			{
				MessageBox.Show("CNIC Expired!");
				dtpCNICExpiryDate.Focus();
				return;
			}
			genEmployee genEmployee = new genEmployee();
			genEmployee.EmployeeId = RecId;
			genEmployee.ActionTypeId = 0;
			genEmployee.AddressDetail = "";
			genEmployee.AddressId = 0;
			genEmployee.AlteredById = UserAccount.ID;
			genEmployee.AlteredOn = DateTime.Now;
			genEmployee.BillCategoryProfileId = 45;
			genEmployee.BloodGroupProfileId = Conversion.ToInt(cmbBloodGroup.Value);
			genEmployee.CNIC = Conversion.ToString((object)txtCNIC.Text).Trim();
			genEmployee.CNICExpiryDate = Conversion.ToDateTime((object)dtpCNICExpiryDate.Value);
			genEmployee.CNICIssueDate = Conversion.ToDateTime((object)dtpCNICIssueDate.Value);
			genEmployee.CompanyId = Conversion.ToInt((object)UserAccount.CompanyId);
			genEmployee.BranchesId = Conversion.ToInt(combranches.Value);
			genEmployee.ProjectId = Conversion.ToInt(comproject.Value);
			genEmployee.ConsultantTypeProfileId = 46;
			genEmployee.ControlTypeProfileId = 0;
			genEmployee.CreatedById = UserAccount.ID;
			genEmployee.CreatedOn = DateTime.Now;
			genEmployee.DOB = Conversion.ToDateTime((object)dtpDOB.Value);
			genEmployee.Email = Conversion.ToString((object)txtEmail.Text);
			genEmployee.EmployeeCategoryId = Conversion.ToInt(cmbEmployeeCatagory.Value);
			genEmployee.EmployeeHistoryId = 0;
			genEmployee.EmployeeNo = Conversion.ToString((object)txtEmployeeNo.Text);
			genEmployee.EmployeePictureString = Conversion.ToString((object)"");
			genEmployee.EmployeeTypeProfileId = Conversion.ToInt(cmbEmployeeType.Value);
			genEmployee.FistName = Conversion.ToString((object)txtFirstName.Text).Trim();
			genEmployee.GenderProfileId = Conversion.ToInt(cmbGender.Value);
			genEmployee.IsApproved = Conversion.ToBool((object)false);
			genEmployee.LastName = Conversion.ToString((object)txtLastName.Text).Trim();
			genEmployee.LicenseNo = Conversion.ToString((object)txtLicenseNo.Text).Trim();
			genEmployee.MiddleName = Conversion.ToString((object)txtMiddleName.Text).Trim();
			genEmployee.Mobile1 = Conversion.ToString((object)txtMobile1.Text).Trim();
			genEmployee.Mobile2 = Conversion.ToString((object)txtMobile2.Text).Trim();
			genEmployee.NationalityProfileId = Conversion.ToInt(cmbNationality.Value);
			genEmployee.OrganizationId = UserAccount.OrganizationId;
			genEmployee.PartyLocationId = Conversion.ToInt((object)0);
			genEmployee.PassportNo = Conversion.ToString((object)txtPassportNo.Text);
			genEmployee.PersonId = Conversion.ToInt((object)0);
			genEmployee.PictureFilePath = Conversion.ToString((object)fileSavePath);
			genEmployee.SignPictureFilePath = Conversion.ToString((object)fileSavePathcookingpic);
			genEmployee.RelationMobile = Conversion.ToString((object)txtRelationMobileNo.Text);
			genEmployee.RelationName = Conversion.ToString((object)txtRelationName.Text).Trim();
			genEmployee.Signature = Conversion.ToString((object)"");
			genEmployee.SignPictureString = Conversion.ToString((object)"");
			genEmployee.SubTitleId = Conversion.ToInt(cmbRelationTitle.Value);
			genEmployee.TitleId = Conversion.ToInt(cmbTitle.Value);
			genEmployee.UserLogId = UserAccount.ID;
			genEmployee.Active = Conversion.ToBool((object)IsActiveEmployeeInformation.Checked);
			genEmployee.EmployeeHistorylist = new List<genEmployeeHistory>();
			genEmployee.EmployeeSalarieslist = new List<EmployeeSalary>();
			genEmployee.EmployeeAddressDetailslist = new List<tblEmployeeAddressDetail>();
			genEmployee.EmployeeBankAccountslist = new List<hrmEmployeeBankAccount>();
			genEmployee.EmployeeEducationslist = new List<GenEmployeeEducation>();
			genEmployee.EmployeeExperienceslist = new List<hrmEmployeeExperience>();
			genEmployee.EmployeeFamilyInfoslist = new List<hrmEmployeeFamilyInfo>();
			genEmployee.EmployeeReferenceslist = new List<hrmEmployeeReference>();
			genEmployee.EmployeeWeekDayslist = new List<genEmployeeWeekDay>();
			genEmployee.HrmEmployeeBenefitslist = new List<hrmEmployeeBenefit>();
			if (!EmployeeInformationValidation())
			{
				return;
			}
			genEmployeeHistory employeeHistory = new genEmployeeHistory();
			employeeHistory.FromDate = Conversion.ToDateTime((object)txtStartDate.Value);
			employeeHistory.ToDate = Conversion.ToDateTime((object)txtEndDate.Value);
			employeeHistory.ProbationEnding = Conversion.ToDateTime((object)txtProbationEndingDate.Value);
			employeeHistory.DesignationId = Conversion.ToInt(cmbDesignation.Value);
			employeeHistory.ShiftId = Conversion.ToInt(cmbShift.Value);
			employeeHistory.DepartmentId = Conversion.ToInt(cmbDepartment.Value);
			employeeHistory.PayableAcId = Conversion.ToInt(CmbSalaryPayableAc.Value);
			employeeHistory.ExpenseAccountId = Conversion.ToInt(CmbSalariesExpensesAc.Value);
			employeeHistory.LoanAcId = Conversion.ToInt(CmbSalaryLoanAc.Value);
			employeeHistory.LocationId = Conversion.ToInt(cmbLocation.Value);
			employeeHistory.SectionId = Conversion.ToInt(cmbSection.Value);
			employeeHistory.EmployeeTypeProfileId = Conversion.ToInt(cmbEmployeeType.Value);
			employeeHistory.PartyLocationId = Conversion.ToInt(cmbReportedBranch.Value);
			employeeHistory.ReportToEmployeeId = Conversion.ToInt(cmbReportedPerson.Value);
			employeeHistory.StoreId = Conversion.ToInt(cmbStore.Value);
			employeeHistory.TotalSalary = Conversion.ToDecimal((object)txtSalary.Text);
			employeeHistory.Active = IsActiveEmployeeInformation.Checked;
			employeeHistory.ActionTypeId = 1;
			employeeHistory.AlteredById = UserAccount.ID;
			employeeHistory.AlteredOn = DateTime.Now;
			employeeHistory.BranchId = 0;
			employeeHistory.CompanyId = UserAccount.CompanyId;
			employeeHistory.OrganizationId = UserAccount.OrganizationId;
			employeeHistory.BranchId = UserAccount.BranchesId;
			employeeHistory.CreatedById = UserAccount.ID;
			employeeHistory.CreatedOn = DateTime.Now;
			employeeHistory.EmployeeHistoryLineId = 1;
			employeeHistory.WithEffectFrom = DateTime.Now;
			employeeHistory.EmployeeGroupId = Conversion.ToInt(cmbEmployeeGroup.Value);
			genEmployee.EmployeeHistorylist.Add(employeeHistory);
			if (grdEmployeeSalary.GetRows().Count() == 0)
			{
				MessageBox.Show("Employee Salary Breakup Required");
				return;
			}
			GridEXRow[] rows;
			if (grdEmployeeSalary.GetRows().Count() > 0)
			{
				rows = grdEmployeeSalary.GetRows();
				foreach (GridEXRow r10 in rows)
				{
					EmployeeSalary employeeSalary = new EmployeeSalary();
					employeeSalary.SalaryTypeProfileId = Conversion.ToInt(r10.Cells["SalaryTypeId"].Value);
					employeeSalary.SalaryTypePercent = Conversion.ToDecimal(r10.Cells["TypePercent"].Value);
					employeeSalary.Amount = Conversion.ToDecimal(r10.Cells["Amount"].Value);
					employeeSalary.ActionTypeId = 1;
					employeeSalary.AlteredById = UserAccount.ID;
					employeeSalary.AlteredOn = DateTime.Now;
					employeeSalary.CompanyId = UserAccount.CompanyId;
					employeeSalary.CreatedById = UserAccount.ID;
					employeeSalary.CreatedOn = DateTime.Now;
					employeeSalary.OrganizationId = UserAccount.OrganizationId;
					employeeSalary.UserLogId = UserAccount.ID;
					genEmployee.EmployeeSalarieslist.Add(employeeSalary);
				}
			}
			rows = grdEmployeeDirectBenefits.GetRows();
			foreach (GridEXRow r9 in rows)
			{
				if (Conversion.ToBool(r9.Cells["Select"].Value))
				{
					hrmEmployeeBenefit obj2 = new hrmEmployeeBenefit();
					if (Conversion.ToInt(r9.Cells["BenefitId"].Value) == 0)
					{
						MessageBox.Show("PLease Check Benefit");
						return;
					}
					obj2.BenefitId = Conversion.ToInt(r9.Cells["BenefitId"].Value);
					if (Conversion.ToString((object)r9.Cells["BenefitValue"].Text.Trim()) == string.Empty)
					{
						MessageBox.Show("Please Check Benefit Value");
						return;
					}
					obj2.Description = Conversion.ToString((object)r9.Cells["BenefitValue"].Text.Trim());
					if (Conversion.ToInt(r9.Cells["ApprovalPersonId"].Value) == 0)
					{
						MessageBox.Show("Please Check Approval Person");
						return;
					}
					obj2.ApprovalEmployeeId = Conversion.ToInt(r9.Cells["ApprovalPersonId"].Value);
					obj2.ActionTypeId = 1;
					obj2.AlteredById = UserAccount.ID;
					obj2.AlteredOn = DateTime.Now;
					obj2.CompanyId = UserAccount.CompanyId;
					obj2.OrganizationId = UserAccount.OrganizationId;
					obj2.CreatedById = UserAccount.ID;
					obj2.CreatedOn = DateTime.Now;
					obj2.UserLogId = UserAccount.ID;
					genEmployee.HrmEmployeeBenefitslist.Add(obj2);
				}
			}
			rows = grdEmployeeAssetsBenefit.GetRows();
			foreach (GridEXRow r8 in rows)
			{
				if (Conversion.ToBool(r8.Cells["Select"].Value))
				{
					hrmEmployeeBenefit obj1 = new hrmEmployeeBenefit();
					if (Conversion.ToInt(r8.Cells["BenefitId"].Value) == 0)
					{
						MessageBox.Show("PLease Check Benefit");
						return;
					}
					obj1.BenefitId = Conversion.ToInt(r8.Cells["BenefitId"].Value);
					if (Conversion.ToString((object)r8.Cells["BenefitValue"].Text.Trim()) == string.Empty)
					{
						MessageBox.Show("Please Check Benefit Value");
						return;
					}
					obj1.Description = Conversion.ToString((object)r8.Cells["BenefitValue"].Text.Trim());
					if (Conversion.ToInt(r8.Cells["ApprovalPersonId"].Value) == 0)
					{
						MessageBox.Show("Please Check Approval Person");
						return;
					}
					obj1.ApprovalEmployeeId = Conversion.ToInt(r8.Cells["ApprovalPersonId"].Value);
					obj1.ActionTypeId = 1;
					obj1.AlteredById = UserAccount.ID;
					obj1.AlteredOn = DateTime.Now;
					obj1.CompanyId = UserAccount.CompanyId;
					obj1.OrganizationId = UserAccount.OrganizationId;
					obj1.CreatedById = UserAccount.ID;
					obj1.CreatedOn = DateTime.Now;
					obj1.UserLogId = UserAccount.ID;
					genEmployee.HrmEmployeeBenefitslist.Add(obj1);
				}
			}
			rows = grdWeekDays.GetRows();
			foreach (GridEXRow r7 in rows)
			{
				if (Conversion.ToBool(r7.Cells["Select"].Value))
				{
					genEmployeeWeekDay employeeWeekDay = new genEmployeeWeekDay();
					employeeWeekDay.WeekDayProfileId = Conversion.ToInt(r7.Cells["WeekDaysProfileId"].Value);
					employeeWeekDay.WeekDayName = Conversion.ToString((object)r7.Cells["WeekDays"].Text);
					employeeWeekDay.ActionTypeId = 1;
					employeeWeekDay.AlteredById = UserAccount.ID;
					employeeWeekDay.AlteredOn = DateTime.Now;
					employeeWeekDay.OrganizationId = UserAccount.OrganizationId;
					employeeWeekDay.CompanyId = UserAccount.CompanyId;
					employeeWeekDay.CreatedById = UserAccount.ID;
					employeeWeekDay.CreatedOn = DateTime.Now;
					employeeWeekDay.LineId = r7.RowIndex;
					genEmployee.EmployeeWeekDayslist.Add(employeeWeekDay);
				}
			}
			if (grdAddressDetail.GetRows().Count() > 0)
			{
				rows = grdAddressDetail.GetRows();
				foreach (GridEXRow r6 in rows)
				{
					tblEmployeeAddressDetail addressDetail = new tblEmployeeAddressDetail();
					addressDetail.AddressTypeId = Conversion.ToInt(r6.Cells["AddressTypeId"].Value);
					addressDetail.CountryId = Conversion.ToInt(r6.Cells["CountryId"].Value);
					addressDetail.ProvinceId = Conversion.ToInt(r6.Cells["ProvinceId"].Value);
					addressDetail.CityId = Conversion.ToInt(r6.Cells["CityId"].Value);
					addressDetail.AddressDetail = Conversion.ToString((object)r6.Cells["AddressDetail"].Text);
					addressDetail.CompanyId = UserAccount.CompanyId;
					addressDetail.OrganizationId = UserAccount.OrganizationId;
					genEmployee.EmployeeAddressDetailslist.Add(addressDetail);
				}
			}
			if (grdEmployeeExperience.GetRows().Count() > 0)
			{
				rows = grdEmployeeExperience.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					hrmEmployeeExperience employeeExperience = new hrmEmployeeExperience();
					employeeExperience.DesignationId = Conversion.ToInt(r5.Cells["DesignationId"].Value);
					employeeExperience.OrginizationName = Conversion.ToString((object)r5.Cells["Organization"].Text);
					employeeExperience.CityId = Conversion.ToInt(r5.Cells["CityId"].Value);
					employeeExperience.FromDate = Conversion.ToDateTime(r5.Cells["FromDate"].Value);
					employeeExperience.ToDate = Conversion.ToDateTime(r5.Cells["ToDate"].Value);
					employeeExperience.Description = Conversion.ToString((object)r5.Cells["Description"].Text);
					employeeExperience.ActionTypeId = 1;
					employeeExperience.AlteredById = UserAccount.ID;
					employeeExperience.AlteredOn = DateTime.Now;
					employeeExperience.CompanyId = UserAccount.CompanyId;
					employeeExperience.CreatedById = UserAccount.ID;
					employeeExperience.CreatedOn = DateTime.Now;
					employeeExperience.EmployeeExperienceLineId = r5.RowIndex;
					employeeExperience.OrganizationId = UserAccount.OrganizationId;
					genEmployee.EmployeeExperienceslist.Add(employeeExperience);
				}
			}
			if (grdEmployeeEducation.GetRows().Count() > 0)
			{
				rows = grdEmployeeEducation.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					GenEmployeeEducation employeeEducation = new GenEmployeeEducation();
					employeeEducation.DegreeId = Conversion.ToInt(r4.Cells["DegreeId"].Value);
					employeeEducation.DegreeTitle = Conversion.ToString((object)r4.Cells["DegreeTitle"].Text);
					employeeEducation.Institute = Conversion.ToString((object)r4.Cells["Institute"].Text);
					employeeEducation.CGPA = Conversion.ToDouble((object)r4.Cells["CGPA"].Text);
					employeeEducation.CityId = Conversion.ToInt(r4.Cells["CityId"].Value);
					employeeEducation.StartYear = Conversion.ToDateTime(r4.Cells["StartYear"].Value);
					employeeEducation.PassingYear = Conversion.ToDateTime(r4.Cells["PassingYear"].Value);
					employeeEducation.EntryDate = DateTime.Now;
					employeeEducation.PostDate = DateTime.Now;
					employeeEducation.ModifyDate = DateTime.Now;
					employeeEducation.PostDate = DateTime.Now;
					employeeEducation.PostState = false;
					employeeEducation.PostUser = UserAccount.ID;
					employeeEducation.EntryUser = UserAccount.ID;
					employeeEducation.ModifyUser = UserAccount.ID;
					employeeEducation.CompanyId = UserAccount.CompanyId;
					employeeEducation.OrganizationId = UserAccount.OrganizationId;
					genEmployee.EmployeeEducationslist.Add(employeeEducation);
				}
			}
			if (grdFamilyInfo.GetRows().Count() > 0)
			{
				rows = grdFamilyInfo.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					hrmEmployeeFamilyInfo employeeFamilyInfo = new hrmEmployeeFamilyInfo();
					employeeFamilyInfo.RelationshipProfileId = Conversion.ToInt(r3.Cells["RelationShipId"].Value);
					employeeFamilyInfo.Name = Conversion.ToString((object)r3.Cells["RelationName"].Text);
					employeeFamilyInfo.Description = Conversion.ToString((object)r3.Cells["Description"].Text);
					employeeFamilyInfo.AlteredOn = DateTime.Now;
					employeeFamilyInfo.AlteredById = UserAccount.ID;
					employeeFamilyInfo.ActionTypeId = 1;
					employeeFamilyInfo.CompanyId = UserAccount.CompanyId;
					employeeFamilyInfo.OrganizationId = UserAccount.OrganizationId;
					employeeFamilyInfo.CreatedById = UserAccount.ID;
					employeeFamilyInfo.CreatedOn = DateTime.Now;
					employeeFamilyInfo.EmployeeFamilyInfoLineId = r3.RowIndex;
					employeeFamilyInfo.UserLogId = UserAccount.ID;
					employeeFamilyInfo.BranchId = 0;
					employeeFamilyInfo.ProjectId = 0;
					genEmployee.EmployeeFamilyInfoslist.Add(employeeFamilyInfo);
				}
			}
			if (grdReference.GetRows().Count() > 0)
			{
				rows = grdReference.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					hrmEmployeeReference employeeReference = new hrmEmployeeReference();
					employeeReference.Name = Conversion.ToString((object)r2.Cells["ReferenceName"].Text);
					employeeReference.OrginizationName = Conversion.ToString((object)r2.Cells["OrganizationName"].Text);
					employeeReference.DesignationId = Conversion.ToInt(r2.Cells["DesignationId"].Value);
					employeeReference.Mobile = Conversion.ToString((object)r2.Cells["Mobile"].Text);
					employeeReference.Email = Conversion.ToString((object)r2.Cells["Email"].Text);
					employeeReference.Description = Conversion.ToString((object)r2.Cells["Description"].Text);
					employeeReference.ActionTypeId = 1;
					employeeReference.AlteredById = UserAccount.ID;
					employeeReference.AlteredOn = DateTime.Now;
					employeeReference.CompanyId = UserAccount.CompanyId;
					employeeReference.OrganizationId = UserAccount.OrganizationId;
					employeeReference.CreatedById = UserAccount.ID;
					employeeReference.CreatedOn = DateTime.Now;
					employeeReference.UserLogId = UserAccount.ID;
					genEmployee.EmployeeReferenceslist.Add(employeeReference);
				}
			}
			if (grdBankAccount.GetRows().Count() > 0)
			{
				rows = grdBankAccount.GetRows();
				foreach (GridEXRow r in rows)
				{
					hrmEmployeeBankAccount bankAccount = new hrmEmployeeBankAccount();
					bankAccount.BankAccountId = Conversion.ToInt(r.Cells["BankId"].Value);
					bankAccount.AccountTitle = Conversion.ToString((object)r.Cells["AccountTitle"].Text);
					bankAccount.AccountNo = Conversion.ToString((object)r.Cells["AccountNo"].Text);
					bankAccount.IsPayrollAccount = Conversion.ToBool(r.Cells["IsPayRoll"].Value);
					bankAccount.ActionTypeId = 1;
					bankAccount.AlteredById = UserAccount.ID;
					bankAccount.AlteredOn = DateTime.Now;
					bankAccount.CompanyId = UserAccount.CompanyId;
					bankAccount.OrganizationId = UserAccount.OrganizationId;
					bankAccount.CreatedById = UserAccount.ID;
					bankAccount.CreatedOn = DateTime.Now;
					bankAccount.UserLogId = UserAccount.ID;
					genEmployee.EmployeeBankAccountslist.Add(bankAccount);
				}
			}
			genEmployee.Save(genEmployee);
			if (RecId == 0)
			{
				MessageBox.Show("Information Saved");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Information Update");
			}
			if (ofd != "" && ofd != string.Empty)
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				pictureBoxEmployeePhoto.Image = null;
				pictureBoxEmployeePhoto.Invalidate();
				ofd = "";
			}
			if (ofdcookingpic != "" && ofdcookingpic != string.Empty)
			{
				File.Copy(ofdcookingpic, fileSavePathcookingpic, overwrite: true);
				pictureBoxEmployeeSignature.Image = null;
				pictureBoxEmployeeSignature.Invalidate();
				ofdcookingpic = "";
			}
			btnnew_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = HRM_Reports.EmployeeRegistrationSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeId = Conversion.ToInt((object)RecId)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1112-Employee_Registration_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintRegister_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = HRM_Reports.EmployeeRegistrationSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1111-Employee_Registration_Register.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			Project();
			braches();
			AllLookUpFill();
			AccountBinds();
			EmployeeTypeComboFill();
			NationalityComboFill();
			BloodGroupComboFill();
			EmployeeCatagoryComboFill();
			EmployeeTitleComboFill();
			EmployeeRelationComboFill();
			EmployeeGenderComboFill();
			EmployeeName();
			DepartmentNameComboFill();
			LocationNameComboFill();
			ReportedBranchComboFill();
			SectionNameComboFill();
			StoreNameComboFill();
			DesignationNameComboFill();
			ShiftNameComboFill();
			ReportedPersonComboFill();
			AddressTypeComboFill();
			CityFill();
			CountryFill();
			ProvinceFill();
			DegreeFill();
			RelationShipFill();
			BankName();
			EmployeeGroupComboFill();
			GridBindEmployeeSalary(Conversion.ToInt(cmbLocation.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGender_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 26;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReleaseTitle_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 25;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTitle_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 24;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployeeCategory_Click(object sender, EventArgs e)
	{
		try
		{
			new frmEmployeeCategory(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBloodGroup_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 22;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNationality_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 15;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployeeType_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 20;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReportedPerson_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 34;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReportedBranch_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 28;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLocation_Click(object sender, EventArgs e)
	{
		try
		{
			new frmgenLocation(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStore_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 31;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSection_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineSection(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShift_Click(object sender, EventArgs e)
	{
		try
		{
			new frmGenShift(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDesignation_Click(object sender, EventArgs e)
	{
		try
		{
			new genDesignation(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDepartment_Click(object sender, EventArgs e)
	{
		try
		{
			new genDepartment(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployeeGroup_Click(object sender, EventArgs e)
	{
		try
		{
			new genEmployeeGroup(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			txtEmployeeNoForUpdate.Text = string.Empty;
			txtEmployeeNo.Clear();
			((Control)(object)cmbEmployeeType).Text = "";
			((Control)(object)cmbEmployeeCatagory).Text = "";
			((Control)(object)cmbTitle).Text = "";
			txtFirstName.Clear();
			txtMiddleName.Clear();
			txtLastName.Clear();
			((Control)(object)cmbRelationTitle).Text = "";
			txtRelationName.Clear();
			txtRelationMobileNo.Clear();
			((Control)(object)cmbGender).Text = "";
			dtpDOB.Value = DateTime.Now;
			((Control)(object)cmbNationality).Text = "";
			txtEmail.Clear();
			txtMobile1.Clear();
			txtMobile2.Clear();
			txtCNIC.Clear();
			dtpCNICIssueDate.Value = DateTime.Now;
			dtpCNICExpiryDate.Value = DateTime.Now;
			txtPassportNo.Clear();
			txtLicenseNo.Clear();
			((Control)(object)cmbBloodGroup).Text = "";
			EmlpoyeeNoGenerate();
			pictureBoxEmployeePhoto.Image = null;
			pictureBoxEmployeePhoto.Invalidate();
			pictureBoxEmployeeSignature.Image = null;
			pictureBoxEmployeeSignature.Invalidate();
			txtEmployeeNo.Focus();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = false;
			dtEmployeeBankAccount.Rows.Clear();
			grdBankAccount.ClearStructure();
			dtAddressDetail.Rows.Clear();
			grdAddressDetail.ClearStructure();
			dtEmployeeExperience.Rows.Clear();
			grdEmployeeExperience.ClearStructure();
			dtEmployeeFamilyInfo.Rows.Clear();
			grdFamilyInfo.ClearStructure();
			dtEmployeeReference.Rows.Clear();
			grdReference.ClearStructure();
			dtEmployeeEducation.Rows.Clear();
			grdEmployeeEducation.ClearStructure();
			dtEmployeeSalaryGrid.Rows.Clear();
			grdEmployeeSalary.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool ValidationEmployeeRegistration()
	{
		if (Conversion.ToInt((object)txtEmployeeNo.Text) == 0)
		{
			MessageBox.Show("EmployeeNo Required!");
			txtEmployeeNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEmployeeType).ActiveRow == null || Conversion.ToInt(cmbEmployeeType.Value) == 0)
		{
			MessageBox.Show("Employee Type Required!");
			cmbEmployeeType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEmployeeCatagory).ActiveRow == null || Conversion.ToInt(cmbEmployeeCatagory.Value) == 0)
		{
			MessageBox.Show("Employee Catagory Required!");
			cmbEmployeeCatagory.Focus();
			return false;
		}
		if (((UltraGridBase)cmbTitle).ActiveRow == null || Conversion.ToInt(cmbTitle.Value) == 0)
		{
			MessageBox.Show("Employee Title Required!");
			cmbTitle.Focus();
			return false;
		}
		if (txtFirstName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("FirstName Required!");
			txtFirstName.Focus();
			return false;
		}
		if (txtLastName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("LastName Required!");
			txtLastName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGender).ActiveRow == null || Conversion.ToInt(cmbGender.Value) == 0)
		{
			MessageBox.Show("Gender Required!");
			cmbGender.Focus();
			return false;
		}
		if (((UltraGridBase)cmbNationality).ActiveRow == null || Conversion.ToInt(cmbNationality.Value) == 0)
		{
			MessageBox.Show("Nationality Required!");
			cmbNationality.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtCNIC.Text.Trim()) == string.Empty || Conversion.ToString((object)txtCNIC.Text.Trim()) == null)
		{
			MessageBox.Show("CNIC Required!");
			txtCNIC.Focus();
			return false;
		}
		if (txtMobile1.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Mobile1 Required!");
			txtMobile1.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepartment).ActiveRow == null)
		{
			MessageBox.Show("Department Required!");
			cmbDepartment.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSalaryPayableAc).ActiveRow == null || Conversion.ToInt(CmbSalaryPayableAc.Value) == 0)
		{
			MessageBox.Show("Payables Account Field Required");
			CmbSalaryPayableAc.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSalariesExpensesAc).ActiveRow == null || Conversion.ToInt(CmbSalariesExpensesAc.Value) == 0)
		{
			MessageBox.Show("Expense Account Field Required");
			CmbSalariesExpensesAc.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSalaryLoanAc).ActiveRow == null || Conversion.ToInt(CmbSalaryLoanAc.Value) == 0)
		{
			MessageBox.Show("Loan Account Field Required");
			CmbSalaryLoanAc.Focus();
			return false;
		}
		return true;
	}

	private void grdEmployeeRegistrationSetting()
	{
		try
		{
			grdEmployeeRegistration.RootTable.Columns["EmployeeType"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["EmployeeCatagory"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["Title"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["FirstName"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["MiddleName"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["LastName"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["RelationTitle"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["RelationName"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["RelationMobileNo"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["Gender"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["DOB"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["Nationality"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["Email"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["Mobile1"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["Mobile2"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["CNIC"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["CNIC Issue Date"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["CNIC Expiry Date"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["PassportNo"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["LicenseNo"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["BloodGroup"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["FlatSharePercent"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["IsAllowShare"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["NeedLogin"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["SignPictureFilePath"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["PictureFilePath"].EditType = (EditType)0;
			grdEmployeeRegistration.RootTable.Columns["EmployeeType"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["EmployeeCatagory"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["Title"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["FirstName"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["MiddleName"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["LastName"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["RelationTitle"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["RelationName"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["RelationMobileNo"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["Gender"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["DOB"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["Nationality"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["Email"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["Mobile1"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["Mobile2"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["CNIC"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["CNIC Issue Date"].FilterEditType = (FilterEditType)6;
			grdEmployeeRegistration.RootTable.Columns["CNIC Expiry Date"].FilterEditType = (FilterEditType)6;
			grdEmployeeRegistration.RootTable.Columns["PassportNo"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["LicenseNo"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["BloodGroup"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["FlatSharePercent"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["IsAllowShare"].FilterEditType = (FilterEditType)7;
			grdEmployeeRegistration.RootTable.Columns["NeedLogin"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["SignPictureFilePath"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["PictureFilePath"].FilterEditType = (FilterEditType)1;
			grdEmployeeRegistration.RootTable.Columns["EmployeeId"].Visible = false;
			grdEmployeeRegistration.RootTable.Columns["EmployeeType"].Width = 100;
			grdEmployeeRegistration.RootTable.Columns["EmployeeCatagory"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["Title"].Width = 80;
			grdEmployeeRegistration.RootTable.Columns["FirstName"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["MiddleName"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["LastName"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["RelationTitle"].Width = 80;
			grdEmployeeRegistration.RootTable.Columns["RelationName"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["RelationMobileNo"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["Gender"].Width = 100;
			grdEmployeeRegistration.RootTable.Columns["DOB"].Width = 100;
			grdEmployeeRegistration.RootTable.Columns["Nationality"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["Email"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["Mobile1"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["Mobile2"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["CNIC"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["CNIC Issue Date"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["CNIC Expiry Date"].Width = 150;
			grdEmployeeRegistration.RootTable.Columns["PassportNo"].Width = 100;
			grdEmployeeRegistration.RootTable.Columns["LicenseNo"].Width = 100;
			grdEmployeeRegistration.RootTable.Columns["BloodGroup"].Width = 50;
			grdEmployeeRegistration.RootTable.Columns["FlatSharePercent"].Width = 50;
			grdEmployeeRegistration.RootTable.Columns["IsAllowShare"].Width = 50;
			grdEmployeeRegistration.RootTable.Columns["NeedLogin"].Width = 50;
			grdEmployeeRegistration.RootTable.Columns["SignPictureFilePath"].Width = 200;
			grdEmployeeRegistration.RootTable.Columns["PictureFilePath"].Width = 200;
			grdEmployeeRegistration.RootTable.Columns.Add("Edit");
			grdEmployeeRegistration.RootTable.Columns["Edit"].Caption = "Edit";
			grdEmployeeRegistration.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmployeeRegistration.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdEmployeeRegistration.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdEmployeeRegistration.RootTable.Columns["Edit"].Width = 50;
			grdEmployeeRegistration.RootTable.Columns["Edit"].Position = 0;
			grdEmployeeRegistration.RootTable.Columns.Add("Print");
			grdEmployeeRegistration.RootTable.Columns["Print"].Caption = "Print";
			grdEmployeeRegistration.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmployeeRegistration.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdEmployeeRegistration.RootTable.Columns["Print"].ButtonText = "Print";
			grdEmployeeRegistration.RootTable.Columns["Print"].Width = 50;
			grdEmployeeRegistration.RootTable.Columns["Print"].Position = 1;
			grdEmployeeRegistration.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBindEmployeeRegistration()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genEmployee.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeId", typeof(int));
			dt.Columns.Add("EmployeeNo", typeof(int));
			dt.Columns.Add("EmployeeType");
			dt.Columns.Add("EmployeeCatagory");
			dt.Columns.Add("Title");
			dt.Columns.Add("FirstName");
			dt.Columns.Add("MiddleName");
			dt.Columns.Add("LastName");
			dt.Columns.Add("RelationTitle");
			dt.Columns.Add("RelationName");
			dt.Columns.Add("RelationMobileNo");
			dt.Columns.Add("Gender");
			dt.Columns.Add("DOB", typeof(DateTime));
			dt.Columns.Add("Nationality");
			dt.Columns.Add("Email");
			dt.Columns.Add("Mobile1");
			dt.Columns.Add("Mobile2");
			dt.Columns.Add("CNIC");
			dt.Columns.Add("CNIC Issue Date", typeof(DateTime));
			dt.Columns.Add("CNIC Expiry Date", typeof(DateTime));
			dt.Columns.Add("PassportNo");
			dt.Columns.Add("LicenseNo");
			dt.Columns.Add("BloodGroup");
			dt.Columns.Add("FlatSharePercent");
			dt.Columns.Add("IsAllowShare");
			dt.Columns.Add("NeedLogin");
			dt.Columns.Add("SignPictureFilePath");
			dt.Columns.Add("PictureFilePath");
			dt.Columns.Add("Status");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["EmployeeId"], lst.Rows[i]["EmployeeNo"], lst.Rows[i]["EmployeeType"], lst.Rows[i]["EmployeeCategory"], lst.Rows[i]["TitleName"], lst.Rows[i]["FistName"], lst.Rows[i]["MiddleName"], lst.Rows[i]["LastName"], lst.Rows[i]["RelationTitleName"], lst.Rows[i]["RelationName"], lst.Rows[i]["RelationMobile"], lst.Rows[i]["Gender"], Conversion.ToDateTime(lst.Rows[i]["DOB"]).ToString("dd-MMM-yyyyy"), lst.Rows[i]["Nationality"], lst.Rows[i]["Email"], lst.Rows[i]["Mobile1"], lst.Rows[i]["Mobile2"], lst.Rows[i]["CNIC"], Conversion.ToDateTime(lst.Rows[i]["CNICIssueDate"]).ToString("dd-MMM-yyyyy"), Conversion.ToDateTime(lst.Rows[i]["CNICExpiryDate"]).ToString("dd-MMM-yyyyy"), lst.Rows[i]["PassportNo"], lst.Rows[i]["LicenseNo"], lst.Rows[i]["BloodGroup"], lst.Rows[i]["FlatSharePercent"], lst.Rows[i]["IsAllowShare"], lst.Rows[i]["NeedLogin"], lst.Rows[i]["SignPictureFilePath"], lst.Rows[i]["PictureFilePath"], lst.Rows[i]["ActiveStatus"]);
			}
			grdEmployeeRegistration.DataSource = dt;
			grdEmployeeRegistration.RetrieveStructure();
			grdEmployeeRegistrationSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridBindEmployeeRegistration();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetrivedDataEmployeeRegistration(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			genEmployee genEmployee = new genEmployee();
			RecId = Id;
			genEmployee = genEmployee.GetByID(RecId);
			txtEmployeeNo.Text = Conversion.ToString((object)genEmployee.EmployeeNo);
			cmbEmployeeType.Value = Conversion.ToInt((object)genEmployee.EmployeeTypeProfileId);
			cmbEmployeeCatagory.Value = Conversion.ToInt((object)genEmployee.EmployeeCategoryId);
			cmbTitle.Value = Conversion.ToInt((object)genEmployee.TitleId);
			txtFirstName.Text = Conversion.ToString((object)genEmployee.FistName);
			txtMiddleName.Text = Conversion.ToString((object)genEmployee.MiddleName);
			txtLastName.Text = Conversion.ToString((object)genEmployee.LastName);
			cmbRelationTitle.Value = Conversion.ToInt((object)genEmployee.SubTitleId);
			txtRelationName.Text = Conversion.ToString((object)genEmployee.RelationName);
			txtRelationMobileNo.Text = Conversion.ToString((object)genEmployee.RelationMobile);
			cmbGender.Value = Conversion.ToInt((object)genEmployee.GenderProfileId);
			dtpDOB.Value = Conversion.ToDateTime((object)genEmployee.DOB);
			cmbNationality.Value = Conversion.ToInt((object)genEmployee.NationalityProfileId);
			txtEmail.Text = Conversion.ToString((object)genEmployee.Email);
			txtMobile1.Text = Conversion.ToString((object)genEmployee.Mobile1);
			txtMobile2.Text = Conversion.ToString((object)genEmployee.Mobile2);
			txtCNIC.Text = Conversion.ToString((object)genEmployee.CNIC);
			dtpCNICIssueDate.Value = Conversion.ToDateTime((object)genEmployee.CNICIssueDate);
			dtpCNICExpiryDate.Value = Conversion.ToDateTime((object)genEmployee.CNICExpiryDate);
			txtPassportNo.Text = Conversion.ToString((object)genEmployee.PassportNo);
			txtLicenseNo.Text = Conversion.ToString((object)genEmployee.LicenseNo);
			cmbBloodGroup.Value = Conversion.ToInt((object)genEmployee.BloodGroupProfileId);
			combranches.Value = Conversion.ToInt((object)genEmployee.BranchesId);
			comproject.Value = Conversion.ToInt((object)genEmployee.ProjectId);
			fileSavePath = genEmployee.PictureFilePath;
			fileSavePathcookingpic = genEmployee.SignPictureFilePath;
			if (fileSavePath != "")
			{
				if (File.Exists(fileSavePath))
				{
					pictureBoxEmployeePhoto.Image = Image.FromFile(genEmployee.PictureFilePath);
				}
				else
				{
					fileSavePath = "";
					pictureBoxEmployeePhoto.Image = null;
					pictureBoxEmployeePhoto.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				pictureBoxEmployeePhoto.Image = null;
				pictureBoxEmployeePhoto.Invalidate();
			}
			if (fileSavePathcookingpic != "")
			{
				if (File.Exists(fileSavePathcookingpic))
				{
					pictureBoxEmployeeSignature.Image = Image.FromFile(genEmployee.SignPictureFilePath);
				}
				else
				{
					fileSavePathcookingpic = "";
					pictureBoxEmployeeSignature.Image = null;
					pictureBoxEmployeeSignature.Invalidate();
				}
			}
			else
			{
				fileSavePathcookingpic = "";
				pictureBoxEmployeeSignature.Image = null;
				pictureBoxEmployeeSignature.Invalidate();
			}
			if (genEmployee.EmployeeHistorylist.Count > 0)
			{
				txtStartDate.Value = genEmployee.EmployeeHistorylist[0].FromDate;
				txtEndDate.Value = genEmployee.EmployeeHistorylist[0].ToDate;
				txtProbationEndingDate.Value = genEmployee.EmployeeHistorylist[0].ProbationEnding;
				cmbDesignation.Value = genEmployee.EmployeeHistorylist[0].DesignationId;
				cmbShift.Value = genEmployee.EmployeeHistorylist[0].ShiftId;
				cmbDepartment.Value = genEmployee.EmployeeHistorylist[0].DepartmentId;
				cmbDepartment_Leave(null, null);
				CmbSalaryPayableAc.Value = genEmployee.EmployeeHistorylist[0].PayableAcId;
				CmbSalariesExpensesAc.Value = genEmployee.EmployeeHistorylist[0].ExpenseAccountId;
				CmbSalaryLoanAc.Value = genEmployee.EmployeeHistorylist[0].LoanAcId;
				cmbLocation.Value = genEmployee.EmployeeHistorylist[0].LocationId;
				cmbSection.Value = genEmployee.EmployeeHistorylist[0].SectionId;
				cmbEmployeeType.Value = genEmployee.EmployeeHistorylist[0].EmployeeTypeProfileId;
				cmbReportedBranch.Value = genEmployee.EmployeeHistorylist[0].PartyLocationId;
				cmbReportedPerson.Value = genEmployee.EmployeeHistorylist[0].ReportToEmployeeId;
				cmbStore.Value = genEmployee.EmployeeHistorylist[0].StoreId;
				txtSalary.Text = Conversion.ToInt((object)genEmployee.EmployeeHistorylist[0].TotalSalary).ToString();
				cmbEmployeeGroup.Value = Conversion.ToInt((object)genEmployee.EmployeeHistorylist[0].EmployeeGroupId);
				IsActiveEmployeeInformation.Checked = genEmployee.EmployeeHistorylist[0].Active;
			}
			if (genEmployee.EmployeeSalarieslist.Count > 0)
			{
				dtEmployeeSalaryGrid.Rows.Clear();
				for (int i3 = 0; i3 < genEmployee.EmployeeSalarieslist.Count; i3++)
				{
					dtEmployeeSalaryGrid.Rows.Add(genEmployee.EmployeeSalarieslist[i3].SalaryTypeProfileId, genEmployee.EmployeeSalarieslist[i3].SalaryType, genEmployee.EmployeeSalarieslist[i3].SalaryTypePercent, genEmployee.EmployeeSalarieslist[i3].Amount);
				}
				grdEmployeeSalary.DataSource = dtEmployeeSalaryGrid;
				grdEmployeeSalary.RetrieveStructure();
				grdEmployeeSalarySetting();
			}
			if (genEmployee.HrmEmployeeBenefitslist.Count > 0)
			{
				for (int i4 = 0; i4 < genEmployee.HrmEmployeeBenefitslist.Count; i4++)
				{
					GridEXRow[] rows;
					if (genEmployee.HrmEmployeeBenefitslist[i4].ProfileName == "Payroll Benefits")
					{
						rows = grdEmployeeDirectBenefits.GetRows();
						foreach (GridEXRow r3 in rows)
						{
							if (genEmployee.HrmEmployeeBenefitslist[i4].BenefitId == Conversion.ToInt(r3.Cells["BenefitId"].Value))
							{
								if (r3 != null && (int)r3.RowType == 0)
								{
									r3.Delete();
								}
								grdEmployeeDirectBenefits.UpdateData();
								dtEmployeeDirectBenefits.Rows.Add(genEmployee.HrmEmployeeBenefitslist[i4].BenefitId, genEmployee.HrmEmployeeBenefitslist[i4].ProfileName, genEmployee.HrmEmployeeBenefitslist[i4].Description, genEmployee.HrmEmployeeBenefitslist[i4].ApprovalEmployeeId, 1);
							}
						}
					}
					if (!(genEmployee.HrmEmployeeBenefitslist[i4].ProfileName == "Assets Benefits"))
					{
						continue;
					}
					rows = grdEmployeeAssetsBenefit.GetRows();
					foreach (GridEXRow r2 in rows)
					{
						if (genEmployee.HrmEmployeeBenefitslist[i4].BenefitId == Conversion.ToInt(r2.Cells["BenefitId"].Value))
						{
							if (r2 != null && (int)r2.RowType == 0)
							{
								r2.Delete();
							}
							grdEmployeeAssetsBenefit.UpdateData();
							dtEmployeeAssetsBenefits.Rows.Add(genEmployee.HrmEmployeeBenefitslist[i4].BenefitId, genEmployee.HrmEmployeeBenefitslist[i4].ProfileName, genEmployee.HrmEmployeeBenefitslist[i4].Description, genEmployee.HrmEmployeeBenefitslist[i4].ApprovalEmployeeId, 1);
						}
					}
				}
				grdEmployeeDirectBenefits.DataSource = dtEmployeeDirectBenefits;
				grdEmployeeDirectBenefits.RetrieveStructure();
				grdEmployeeDirectBenefitSetting();
				grdEmployeeAssetsBenefit.DataSource = dtEmployeeAssetsBenefits;
				grdEmployeeAssetsBenefit.RetrieveStructure();
				grdEmployeeAssetsBenefitSetting();
			}
			if (genEmployee.EmployeeWeekDayslist.Count > 0)
			{
				GridEXRow[] rows = grdWeekDays.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Select"].Value = 0;
					obj.BeginEdit();
				}
				for (int i2 = 0; i2 < genEmployee.EmployeeWeekDayslist.Count; i2++)
				{
					rows = grdWeekDays.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (genEmployee.EmployeeWeekDayslist[i2].WeekDayProfileId == Conversion.ToInt(r.Cells["WeekDaysProfileId"].Value))
						{
							if (r != null && (int)r.RowType == 0)
							{
								r.Delete();
							}
							dtWeekDays.Rows.Add(genEmployee.EmployeeWeekDayslist[i2].WeekDayProfileId, genEmployee.EmployeeWeekDayslist[i2].WeekDayProfileName, 1);
						}
					}
				}
				grdWeekDays.DataSource = dtWeekDays;
				grdWeekDays.RetrieveStructure();
				grdWeekDaysSetting();
			}
			if (genEmployee.EmployeeAddressDetailslist.Count > 0)
			{
				dtAddressDetail.Rows.Clear();
				for (int n = 0; n < genEmployee.EmployeeAddressDetailslist.Count; n++)
				{
					dtAddressDetail.Rows.Add(genEmployee.EmployeeAddressDetailslist[n].AddressTypeId, genEmployee.EmployeeAddressDetailslist[n].AddressType, genEmployee.EmployeeAddressDetailslist[n].CountryId, genEmployee.EmployeeAddressDetailslist[n].CountryName, genEmployee.EmployeeAddressDetailslist[n].ProvinceId, genEmployee.EmployeeAddressDetailslist[n].StateProvince, genEmployee.EmployeeAddressDetailslist[n].CityId, genEmployee.EmployeeAddressDetailslist[n].CityName, genEmployee.EmployeeAddressDetailslist[n].AddressDetail);
				}
				grdAddressDetail.DataSource = dtAddressDetail;
				grdAddressDetail.RetrieveStructure();
				grdAddressDetailSetting();
			}
			if (genEmployee.EmployeeExperienceslist.Count > 0)
			{
				dtEmployeeExperience.Rows.Clear();
				for (int m = 0; m < genEmployee.EmployeeExperienceslist.Count; m++)
				{
					dtEmployeeExperience.Rows.Add(genEmployee.EmployeeExperienceslist[m].DesignationId, genEmployee.EmployeeExperienceslist[m].Designation, genEmployee.EmployeeExperienceslist[m].OrginizationName, genEmployee.EmployeeExperienceslist[m].CityId, genEmployee.EmployeeExperienceslist[m].CityName, genEmployee.EmployeeExperienceslist[m].FromDate.ToString("dd-MMM-yyyy"), genEmployee.EmployeeExperienceslist[m].ToDate.ToString("dd-MMM-yyyy"), genEmployee.EmployeeExperienceslist[m].Description);
				}
				grdEmployeeExperience.DataSource = dtEmployeeExperience;
				grdEmployeeExperience.RetrieveStructure();
				grdEmployeeExperienceSetting();
			}
			if (genEmployee.EmployeeEducationslist.Count > 0)
			{
				dtEmployeeEducation.Rows.Clear();
				for (int l = 0; l < genEmployee.EmployeeEducationslist.Count; l++)
				{
					dtEmployeeEducation.Rows.Add(genEmployee.EmployeeEducationslist[l].DegreeId, genEmployee.EmployeeEducationslist[l].DegreeName, genEmployee.EmployeeEducationslist[l].DegreeTitle, genEmployee.EmployeeEducationslist[l].Institute, genEmployee.EmployeeEducationslist[l].CityId, genEmployee.EmployeeEducationslist[l].CityName, genEmployee.EmployeeEducationslist[l].StartYear.ToString("dd-MMM-yyyy"), genEmployee.EmployeeEducationslist[l].PassingYear.ToString("dd-MMM-yyyy"), genEmployee.EmployeeEducationslist[l].CGPA);
				}
				grdEmployeeEducation.DataSource = dtEmployeeEducation;
				grdEmployeeEducation.RetrieveStructure();
				grdEmployeeEducationSetting();
			}
			if (genEmployee.EmployeeFamilyInfoslist.Count > 0)
			{
				dtEmployeeFamilyInfo.Rows.Clear();
				for (int k = 0; k < genEmployee.EmployeeFamilyInfoslist.Count; k++)
				{
					dtEmployeeFamilyInfo.Rows.Add(genEmployee.EmployeeFamilyInfoslist[k].RelationshipProfileId, genEmployee.EmployeeFamilyInfoslist[k].RelationshipName, genEmployee.EmployeeFamilyInfoslist[k].Name, genEmployee.EmployeeFamilyInfoslist[k].Description);
				}
				grdFamilyInfo.DataSource = dtEmployeeFamilyInfo;
				grdFamilyInfo.RetrieveStructure();
				grdEmployeeFamilyInfoSetting();
			}
			if (genEmployee.EmployeeReferenceslist.Count > 0)
			{
				dtEmployeeReference.Rows.Clear();
				for (int j = 0; j < genEmployee.EmployeeReferenceslist.Count; j++)
				{
					dtEmployeeReference.Rows.Add(genEmployee.EmployeeReferenceslist[j].Name, genEmployee.EmployeeReferenceslist[j].OrginizationName, genEmployee.EmployeeReferenceslist[j].DesignationId, genEmployee.EmployeeReferenceslist[j].DesignationName, genEmployee.EmployeeReferenceslist[j].Mobile, genEmployee.EmployeeReferenceslist[j].Email, genEmployee.EmployeeReferenceslist[j].Description);
				}
				grdReference.DataSource = dtEmployeeReference;
				grdReference.RetrieveStructure();
				grdEmployeeReferenceSetting();
			}
			if (genEmployee.EmployeeBankAccountslist.Count > 0)
			{
				dtEmployeeBankAccount.Rows.Clear();
				for (int i = 0; i < genEmployee.EmployeeBankAccountslist.Count; i++)
				{
					dtEmployeeBankAccount.Rows.Add(genEmployee.EmployeeBankAccountslist[i].BankProfileId, genEmployee.EmployeeBankAccountslist[i].BankName, genEmployee.EmployeeBankAccountslist[i].AccountTitle, genEmployee.EmployeeBankAccountslist[i].AccountNo, genEmployee.EmployeeBankAccountslist[i].IsPayrollAccount);
				}
				grdBankAccount.DataSource = dtEmployeeBankAccount;
				grdBankAccount.RetrieveStructure();
				grdBankAccountSetting();
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void definition()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				string value = list[0].ConfigKey.ToString();
				if (value != null)
				{
					saveDirectory = value;
					Confiquration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployeePicture_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (Confiquration)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd);
					string exten = Path.GetExtension(ofd);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath = Path.Combine(saveDirectory, fileName);
					pictureBoxEmployeePhoto.Image = new Bitmap(openFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnEmployeeSignature_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (Confiquration)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofdcookingpic = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofdcookingpic);
					string exten = Path.GetExtension(ofdcookingpic);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePathcookingpic = Path.Combine(saveDirectory, fileName);
					pictureBoxEmployeeSignature.Image = new Bitmap(openFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEmployeeRegistration_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdEmployeeRegistration).Name);
			if (fs != null)
			{
				grdEmployeeRegistration.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetEmployeeInformation()
	{
		try
		{
			txtStartDate.Value = DateTime.Now;
			txtEndDate.Value = DateTime.Now;
			txtProbationEndingDate.Value = DateTime.Now;
			((Control)(object)cmbDesignation).Text = string.Empty;
			((Control)(object)cmbShift).Text = string.Empty;
			((Control)(object)cmbDepartment).Text = string.Empty;
			((Control)(object)cmbLocation).Text = string.Empty;
			((Control)(object)cmbSection).Text = string.Empty;
			((Control)(object)cmbEmployeeType).Text = string.Empty;
			((Control)(object)cmbReportedBranch).Text = string.Empty;
			((Control)(object)cmbReportedPerson).Text = string.Empty;
			((Control)(object)cmbStore).Text = string.Empty;
			txtSalary.Text = string.Empty;
			((Control)(object)cmbEmployeeGroup).Text = string.Empty;
			IsActiveEmployeeInformation.Checked = false;
			((UltraGridBase)CmbSalaryPayableAc).Rows[0].Activate();
			((UltraGridBase)CmbSalariesExpensesAc).Rows[0].Activate();
			((UltraGridBase)CmbSalaryLoanAc).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool EmployeeInformationValidation()
	{
		if (((UltraGridBase)cmbDesignation).ActiveRow == null || Conversion.ToInt(cmbDesignation.Value) == 0)
		{
			MessageBox.Show("Designation Required");
			cmbDesignation.Focus();
			return false;
		}
		if (((UltraGridBase)cmbShift).ActiveRow == null || Conversion.ToInt(cmbShift.Value) == 0)
		{
			MessageBox.Show("Shift Required");
			cmbShift.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepartment).ActiveRow == null || Conversion.ToInt(cmbDepartment.Value) == 0)
		{
			MessageBox.Show("Department Required");
			cmbDepartment.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLocation).ActiveRow == null || Conversion.ToInt(cmbLocation.Value) == 0)
		{
			MessageBox.Show("Location Required");
			cmbLocation.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSection).ActiveRow == null || Conversion.ToInt(cmbSection.Value) == 0)
		{
			MessageBox.Show("Section Required");
			cmbSection.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEmployeeType).ActiveRow == null || Conversion.ToInt(cmbEmployeeType.Value) == 0)
		{
			MessageBox.Show("Employee Type Required");
			cmbEmployeeType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSalary.Text) == 0.0)
		{
			MessageBox.Show("Salary Required");
			txtSalary.Focus();
			return false;
		}
		return true;
	}

	private void ResetEmployeeSalary()
	{
		try
		{
			GridBindEmployeeSalary(Conversion.ToInt(cmbLocation.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBindEmployeeSalary(int LocationId)
	{
		try
		{
			if (LocationId <= 0)
			{
				return;
			}
			dtEmployeeSalaryGrid.Rows.Clear();
			List<hrmSalaryBreakupPolicy> lst = new List<hrmSalaryBreakupPolicy>();
			lst = hrmSalaryBreakupPolicy.GetByLocationId(LocationId);
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtEmployeeSalaryGrid.Rows.Add(lst[i].SalaryTypeProfileId, lst[i].SalaryType, lst[i].SalaryTypePercent, 0);
				}
				grdEmployeeSalary.DataSource = dtEmployeeSalaryGrid;
				grdEmployeeSalary.RetrieveStructure();
				grdEmployeeSalarySetting();
			}
			else
			{
				grdEmployeeSalary.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeSalarySetting()
	{
		try
		{
			grdEmployeeSalary.RootTable.Columns["SalaryTypeId"].EditType = (EditType)0;
			grdEmployeeSalary.RootTable.Columns["TypePercent"].EditType = (EditType)0;
			grdEmployeeSalary.RootTable.Columns["SalaryType"].EditType = (EditType)0;
			grdEmployeeSalary.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdEmployeeSalary.RootTable.Columns["SalaryTypeId"].Visible = false;
			grdEmployeeSalary.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdEmployeeSalary.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdEmployeeSalary.RootTable.Columns["Amount"].FormatString = "0,0";
			grdEmployeeSalary.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdEmployeeSalary.RootTable.Columns["TypePercent"].AggregateFunction = (AggregateFunction)2;
			grdEmployeeSalary.RootTable.Columns["TypePercent"].TextAlignment = (TextAlignment)3;
			grdEmployeeSalary.RootTable.Columns["TypePercent"].FormatString = "0,0";
			grdEmployeeSalary.RootTable.Columns["TypePercent"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSalaryBreakup()
	{
		try
		{
			if (Conversion.ToDouble((object)txtSalary.Text) > 0.0)
			{
				GridEXRow[] rows = grdEmployeeSalary.GetRows();
				foreach (GridEXRow obj in rows)
				{
					grdEmployeeSalary.UpdateData();
					obj.BeginEdit();
					double Amount = Conversion.ToDouble(obj.Cells["TypePercent"].Value) / 100.0 * Conversion.ToDouble((object)txtSalary.Text);
					obj.Cells["Amount"].Value = Amount;
					grdEmployeeSalarySetting();
					grdEmployeeSalary.UpdateData();
				}
			}
			else
			{
				MessageBox.Show("Please Enter Salary First In Employee Information");
				tabControl3.SelectedIndex = 0;
				txtSalary.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetEmployeeBenefits()
	{
		try
		{
			grdEmployeeDirectBenefitFill();
			grdEmployeeAssetsBenefitFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateBreakup_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSalaryBreakup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeDirectBenefitFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			dtEmployeeDirectBenefits.Rows.Clear();
			DataTable dt = hrmBenefit.Getall(new hrmBenefit
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["BenefitTypeProfileId"]) == 1)
				{
					dtEmployeeDirectBenefits.Rows.Add(dt.Rows[i]["BenefitId"], dt.Rows[i]["ProfileName"], dt.Rows[i]["BenefitName"]);
				}
			}
			grdEmployeeDirectBenefits.DataSource = dtEmployeeDirectBenefits;
			grdEmployeeDirectBenefits.RetrieveStructure();
			grdEmployeeDirectBenefitSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeAssetsBenefitFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			dtEmployeeAssetsBenefits.Rows.Clear();
			DataTable dt = hrmBenefit.Getall(new hrmBenefit
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["BenefitTypeProfileId"]) == 2)
				{
					dtEmployeeAssetsBenefits.Rows.Add(dt.Rows[i]["BenefitId"], dt.Rows[i]["ProfileName"], dt.Rows[i]["BenefitName"]);
				}
			}
			grdEmployeeAssetsBenefit.DataSource = dtEmployeeAssetsBenefits;
			grdEmployeeAssetsBenefit.RetrieveStructure();
			grdEmployeeAssetsBenefitSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeDirectBenefitSetting()
	{
		try
		{
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].ColumnType = (ColumnType)0;
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].HasValueList = true;
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].LimitToList = true;
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].EditType = (EditType)4;
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].Caption = "Approval Person";
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].ValueList.PopulateValueList((IEnumerable)dtReportedPersone.DefaultView, "EmployeeId", "EmployeeName");
			grdEmployeeDirectBenefits.RootTable.Columns["ApprovalPersonId"].Width = 250;
			grdEmployeeDirectBenefits.RootTable.Columns["Select"].EditType = (EditType)8;
			grdEmployeeDirectBenefits.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdEmployeeDirectBenefits.RootTable.Columns["BenefitId"].Visible = false;
			grdEmployeeDirectBenefits.RootTable.Columns["BenefitId"].EditType = (EditType)0;
			grdEmployeeDirectBenefits.RootTable.Columns["BenefitValue"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeAssetsBenefitSetting()
	{
		try
		{
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].ColumnType = (ColumnType)0;
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].HasValueList = true;
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].LimitToList = true;
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].EditType = (EditType)4;
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].Caption = "Approval Person";
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].ValueList.PopulateValueList((IEnumerable)dtReportedPersone.DefaultView, "EmployeeId", "EmployeeName");
			grdEmployeeAssetsBenefit.RootTable.Columns["ApprovalPersonId"].Width = 250;
			grdEmployeeAssetsBenefit.RootTable.Columns["Select"].EditType = (EditType)8;
			grdEmployeeAssetsBenefit.RootTable.Columns["BenefitId"].Visible = false;
			grdEmployeeAssetsBenefit.RootTable.Columns["BenefitId"].EditType = (EditType)0;
			grdEmployeeAssetsBenefit.RootTable.Columns["BenefitValue"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetWeekDays()
	{
		try
		{
			grdWeekDaysFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWeekDaysFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			dtWeekDays.Rows.Clear();
			DataTable dt = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfileTypeId = 60
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtWeekDays.Rows.Add(dt.Rows[i]["ProfileId"], dt.Rows[i]["ProfileName"], 1);
				}
				grdWeekDays.DataSource = dtWeekDays;
				grdWeekDays.RetrieveStructure();
				grdWeekDaysSetting();
			}
			else
			{
				grdWeekDays.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWeekDaysSetting()
	{
		try
		{
			grdWeekDays.RootTable.Columns["WeekDaysProfileId"].Visible = false;
			grdWeekDays.RootTable.Columns["WeekDaysProfileId"].EditType = (EditType)0;
			grdWeekDays.RootTable.Columns["WeekDays"].EditType = (EditType)0;
			grdWeekDays.RootTable.Columns["Select"].EditType = (EditType)8;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAddressDetailSetting()
	{
		grdAddressDetail.RootTable.Columns["AddressTypeId"].Visible = false;
		grdAddressDetail.RootTable.Columns["CountryId"].Visible = false;
		grdAddressDetail.RootTable.Columns["ProvinceId"].Visible = false;
		grdAddressDetail.RootTable.Columns["CityId"].Visible = false;
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetAddressDetail();
			cmbAddressType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (AddressDetailValidation())
			{
				dtAddressDetail.Rows.Add(cmbAddressType.Value, ((Control)(object)cmbAddressType).Text, cmbCountry.Value, ((Control)(object)cmbCountry).Text, cmbProvince.Value, ((Control)(object)cmbProvince).Text, cmbCity.Value, ((Control)(object)cmbCity).Text, txtAddressDetail.Text);
				grdAddressDetail.DataSource = dtAddressDetail;
				grdAddressDetail.RetrieveStructure();
				grdAddressDetailSetting();
				ResetAddressDetail();
				cmbAddressType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		if (AddressDetailValidation())
		{
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["AddressTypeId"] = cmbAddressType.Value;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["AddressType"] = ((Control)(object)cmbAddressType).Text;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["CountryId"] = cmbCountry.Value;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["Country"] = ((Control)(object)cmbCountry).Text;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["ProvinceId"] = cmbProvince.Value;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["Province"] = ((Control)(object)cmbProvince).Text;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["CityId"] = cmbCity.Value;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["City"] = ((Control)(object)cmbCity).Text;
			dtAddressDetail.Rows[AddressDetailupdateDetailIndex]["AddressDetail"] = txtAddressDetail.Text;
			grdAddressDetail.DataSource = dtAddressDetail;
			ResetAddressDetail();
			cmbAddressType.Focus();
		}
	}

	private void grdAddressDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdAddressDetail.CurrentRow;
			if (item != null)
			{
				AddressDetailupdateDetailIndex = item.RowIndex;
				cmbAddressType.Value = item.Cells["AddressTypeId"].Value;
				cmbCountry.Value = item.Cells["CountryId"].Value;
				cmbProvince.Value = item.Cells["ProvinceId"].Value;
				cmbCity.Value = item.Cells["CityId"].Value;
				txtAddressDetail.Text = item.Cells["AddressDetail"].Text;
				cmbAddressType.Focus();
				btnAddAddressDetail.Visible = false;
				btnUpdateAddressDetail.Visible = true;
				btnCancelAddressDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool AddressDetailValidation()
	{
		if (((UltraGridBase)cmbAddressType).ActiveRow == null)
		{
			MessageBox.Show("Address Required");
			cmbAddressType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCountry).ActiveRow == null)
		{
			MessageBox.Show("Country Required");
			cmbCountry.Focus();
			return false;
		}
		if (((UltraGridBase)cmbProvince).ActiveRow == null)
		{
			MessageBox.Show("Province Required");
			cmbProvince.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCity).ActiveRow == null)
		{
			MessageBox.Show("City Required");
			cmbCity.Focus();
			return false;
		}
		if (txtAddressDetail.Text == string.Empty)
		{
			MessageBox.Show("Address Detail Required");
			txtAddressDetail.Focus();
			return false;
		}
		return true;
	}

	private void ResetAddressDetail()
	{
		try
		{
			((Control)(object)cmbAddressType).Text = string.Empty;
			((Control)(object)cmbCountry).Text = string.Empty;
			((Control)(object)cmbProvince).Text = string.Empty;
			((Control)(object)cmbCity).Text = string.Empty;
			txtAddressDetail.Text = string.Empty;
			btnAddAddressDetail.Visible = true;
			btnUpdateAddressDetail.Visible = false;
			btnCancelAddressDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeExperienceSetting()
	{
		try
		{
			grdEmployeeExperience.RootTable.Columns["DesignationId"].Visible = false;
			grdEmployeeExperience.RootTable.Columns["CityId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetEmployeeExperience()
	{
		try
		{
			((Control)(object)cmbDesignationEmployeeExperience).Text = string.Empty;
			txtOrganization.Text = string.Empty;
			((Control)(object)cmbCityEmployeeExperience).Text = string.Empty;
			dtpFrom.Value = DateTime.Now;
			dtpTo.Value = DateTime.Now;
			txtDescription.Text = string.Empty;
			btnAddExperience.Visible = true;
			btnUpdateExperience.Visible = false;
			btnCancelExperience.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool EmployeeExperienceValidation()
	{
		if (((UltraGridBase)cmbDesignationEmployeeExperience).ActiveRow == null)
		{
			MessageBox.Show("Designation Required");
			cmbDesignationEmployeeExperience.Focus();
			return false;
		}
		if (txtOrganization.Text == string.Empty)
		{
			MessageBox.Show("Organization Required");
			txtOrganization.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCityEmployeeExperience).ActiveRow == null)
		{
			MessageBox.Show("Designation Required");
			cmbCityEmployeeExperience.Focus();
			return false;
		}
		return true;
	}

	private void btnAddExperience_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeExperienceValidation())
			{
				dtEmployeeExperience.Rows.Add(cmbDesignationEmployeeExperience.Value, ((Control)(object)cmbDesignationEmployeeExperience).Text, txtOrganization.Text, cmbCityEmployeeExperience.Value, ((Control)(object)cmbCityEmployeeExperience).Text, Conversion.ToDateTime((object)dtpFrom.Value).ToString("dd-MMM-yyyy"), Conversion.ToDateTime((object)dtpTo.Value).ToString("dd-MMM-yyyy"), txtDescription.Text);
				grdEmployeeExperience.DataSource = dtEmployeeExperience;
				grdEmployeeExperience.RetrieveStructure();
				grdEmployeeExperienceSetting();
				ResetEmployeeExperience();
				cmbDesignationEmployeeExperience.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateExperience_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeExperienceValidation())
			{
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["DesignationId"] = cmbDesignationEmployeeExperience.Value;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["Designation"] = ((Control)(object)cmbDesignationEmployeeExperience).Text;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["Organization"] = txtOrganization.Text;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["CityId"] = cmbCityEmployeeExperience.Value;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["City"] = ((Control)(object)cmbCityEmployeeExperience).Text;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["FromDate"] = dtpFrom.Value;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["ToDate"] = dtpTo.Value;
				dtEmployeeExperience.Rows[EmployeeExperienceupdateDetailIndex]["Description"] = txtDescription.Text;
				grdEmployeeExperience.DataSource = dtEmployeeExperience;
				ResetEmployeeExperience();
				cmbDesignationEmployeeExperience.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelExperience_Click(object sender, EventArgs e)
	{
		try
		{
			ResetEmployeeExperience();
			cmbDesignationEmployeeExperience.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeExperience_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdEmployeeExperience.CurrentRow;
			if (item != null)
			{
				EmployeeExperienceupdateDetailIndex = item.RowIndex;
				cmbDesignationEmployeeExperience.Value = item.Cells["DesignationId"].Value;
				txtOrganization.Text = item.Cells["Organization"].Text;
				cmbCityEmployeeExperience.Value = item.Cells["CityId"].Value;
				dtpFrom.Value = Conversion.ToDateTime(item.Cells["FromDate"].Value);
				dtpTo.Value = Conversion.ToDateTime(item.Cells["ToDate"].Value);
				txtDescription.Text = item.Cells["Description"].Text;
				cmbDesignationEmployeeExperience.Focus();
				btnAddExperience.Visible = false;
				btnUpdateExperience.Visible = true;
				btnCancelExperience.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool EmployeeEducationValidation()
	{
		if (((UltraGridBase)cmbDegree).ActiveRow == null)
		{
			MessageBox.Show("Degree Required");
			cmbDegree.Focus();
			return false;
		}
		if (txtDegreeTitle.Text == string.Empty)
		{
			MessageBox.Show("Degree Title Required");
			txtDegreeTitle.Focus();
			return false;
		}
		if (txtInstitute.Text == string.Empty)
		{
			MessageBox.Show("Institute Required");
			txtInstitute.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCityEducation).ActiveRow == null)
		{
			MessageBox.Show("City Required");
			cmbCityEducation.Focus();
			return false;
		}
		if (txtCGPA.Text == string.Empty)
		{
			MessageBox.Show("CGPA Required");
			txtCGPA.Focus();
			return false;
		}
		return true;
	}

	private void grdEmployeeEducationSetting()
	{
		try
		{
			grdEmployeeEducation.RootTable.Columns["DegreeId"].Visible = false;
			grdEmployeeEducation.RootTable.Columns["CityId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetEmployeeEducation()
	{
		try
		{
			((Control)(object)cmbDegree).Text = string.Empty;
			txtDegreeTitle.Text = string.Empty;
			txtInstitute.Text = string.Empty;
			((Control)(object)cmbCityEducation).Text = string.Empty;
			StartYearDate.Value = DateTime.Now;
			PassingYearDate.Value = DateTime.Now;
			txtCGPA.Text = string.Empty;
			btnAddEducation.Visible = true;
			btnUpdateEducation.Visible = false;
			btnCancelEducation.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateEducation_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeEducationValidation())
			{
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["DegreeId"] = cmbDegree.Value;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["Degree"] = ((Control)(object)cmbDegree).Text;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["DegreeTitle"] = txtDegreeTitle.Text;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["Institute"] = txtInstitute.Text;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["CityId"] = cmbCityEducation.Value;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["City"] = ((Control)(object)cmbCityEducation).Text;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["StartYear"] = StartYearDate.Value;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["PassingYear"] = PassingYearDate.Value;
				dtEmployeeEducation.Rows[EmployeeEducationupdateDetailIndex]["CGPA"] = txtCGPA.Text;
				grdEmployeeEducation.DataSource = dtEmployeeEducation;
				ResetEmployeeEducation();
				cmbDegree.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddEducation_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeEducationValidation())
			{
				dtEmployeeEducation.Rows.Add(cmbDegree.Value, ((Control)(object)cmbDegree).Text, txtDegreeTitle.Text, txtInstitute.Text, cmbCityEducation.Value, ((Control)(object)cmbCityEducation).Text, Conversion.ToDateTime((object)StartYearDate.Value).ToString("dd-MMM-yyyy"), Conversion.ToDateTime((object)PassingYearDate.Value).ToString("dd-MMM-yyyy"), txtCGPA.Text);
				grdEmployeeEducation.DataSource = dtEmployeeEducation;
				grdEmployeeEducation.RetrieveStructure();
				grdEmployeeEducationSetting();
				ResetEmployeeEducation();
				cmbDegree.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelEducation_Click(object sender, EventArgs e)
	{
		try
		{
			ResetEmployeeEducation();
			cmbDegree.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeEducation_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdEmployeeEducation.CurrentRow;
			if (item != null)
			{
				EmployeeEducationupdateDetailIndex = item.RowIndex;
				cmbDegree.Value = item.Cells["DegreeId"].Value;
				txtDegreeTitle.Text = item.Cells["DegreeTitle"].Text;
				txtInstitute.Text = item.Cells["Institute"].Text;
				cmbCityEducation.Value = item.Cells["CityId"].Value;
				StartYearDate.Value = Conversion.ToDateTime(item.Cells["StartYear"].Value);
				PassingYearDate.Value = Conversion.ToDateTime(item.Cells["PassingYear"].Value);
				txtCGPA.Text = item.Cells["CGPA"].Text;
				btnAddEducation.Visible = false;
				btnUpdateEducation.Visible = true;
				btnCancelEducation.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool EmployeeFamilyInfoValidation()
	{
		if (((UltraGridBase)cmbRelationShip).ActiveRow == null)
		{
			MessageBox.Show("Relationship Required");
			cmbRelationShip.Focus();
			return false;
		}
		if (txtNameFamilyInfo.Text == string.Empty)
		{
			MessageBox.Show("Relationship Name Required");
			txtNameFamilyInfo.Focus();
			return false;
		}
		return true;
	}

	private void grdEmployeeFamilyInfoSetting()
	{
		try
		{
			grdFamilyInfo.RootTable.Columns["RelationShipId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetEmployeeFamilyInfo()
	{
		try
		{
			((Control)(object)cmbRelationShip).Text = string.Empty;
			txtNameFamilyInfo.Text = string.Empty;
			txtDescriptionFamilyInfo.Text = string.Empty;
			btnAddFamily.Visible = true;
			btnUpdateFamily.Visible = false;
			btnCancelFamily.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateFamily_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeFamilyInfoValidation())
			{
				dtEmployeeFamilyInfo.Rows[EmployeeFamilyInfoupdateDetailIndex]["RelationShipId"] = cmbRelationShip.Value;
				dtEmployeeFamilyInfo.Rows[EmployeeFamilyInfoupdateDetailIndex]["RelationShip"] = ((Control)(object)cmbRelationShip).Text;
				dtEmployeeFamilyInfo.Rows[EmployeeFamilyInfoupdateDetailIndex]["RelationName"] = txtNameFamilyInfo.Text;
				dtEmployeeFamilyInfo.Rows[EmployeeFamilyInfoupdateDetailIndex]["Description"] = txtDescriptionFamilyInfo.Text;
				grdFamilyInfo.DataSource = dtEmployeeFamilyInfo;
				ResetEmployeeFamilyInfo();
				cmbRelationShip.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddFamily_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeFamilyInfoValidation())
			{
				dtEmployeeFamilyInfo.Rows.Add(cmbRelationShip.Value, ((Control)(object)cmbRelationShip).Text, txtNameFamilyInfo.Text, txtDescriptionFamilyInfo.Text);
				grdFamilyInfo.DataSource = dtEmployeeFamilyInfo;
				grdFamilyInfo.RetrieveStructure();
				grdEmployeeFamilyInfoSetting();
				ResetEmployeeFamilyInfo();
				cmbRelationShip.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelFamily_Click(object sender, EventArgs e)
	{
		try
		{
			ResetEmployeeFamilyInfo();
			cmbRelationShip.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFamilyInfo_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdFamilyInfo.CurrentRow;
			if (item != null)
			{
				EmployeeFamilyInfoupdateDetailIndex = item.RowIndex;
				cmbRelationShip.Value = item.Cells["RelationShipId"].Value;
				txtNameFamilyInfo.Text = item.Cells["RelationName"].Text;
				txtDescriptionFamilyInfo.Text = item.Cells["Description"].Text;
				btnAddFamily.Visible = false;
				btnUpdateFamily.Visible = true;
				btnCancelFamily.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool EmployeeReferenceValidation()
	{
		if (((UltraGridBase)cmbDesignationEmployeeRefrence).ActiveRow == null)
		{
			MessageBox.Show("Designation Required");
			cmbDesignationEmployeeRefrence.Focus();
			return false;
		}
		if (txtRefrenceName.Text == string.Empty)
		{
			MessageBox.Show("Reference Name Required");
			txtRefrenceName.Focus();
			return false;
		}
		if (txtOrganizationEmployeeRefrence.Text == string.Empty)
		{
			MessageBox.Show("Organization Required");
			txtOrganizationEmployeeRefrence.Focus();
			return false;
		}
		if (txtMobileEmployeeRefrence.Text == string.Empty)
		{
			MessageBox.Show("Mobile Required");
			txtMobileEmployeeRefrence.Focus();
			return false;
		}
		return true;
	}

	private void grdEmployeeReferenceSetting()
	{
		try
		{
			grdReference.RootTable.Columns["DesignationId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetReference()
	{
		try
		{
			txtRefrenceName.Text = string.Empty;
			txtOrganizationEmployeeRefrence.Text = string.Empty;
			((Control)(object)cmbDesignationEmployeeRefrence).Text = string.Empty;
			txtMobileEmployeeRefrence.Text = string.Empty;
			txtEmailEmployeeRefrence.Text = string.Empty;
			txtDescriptionEmployeeRefrence.Text = string.Empty;
			btnAddReference.Visible = true;
			btnUpdateReference.Visible = false;
			btnCancelReference.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelReference_Click(object sender, EventArgs e)
	{
		try
		{
			ResetReference();
			txtRefrenceName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddReference_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeReferenceValidation())
			{
				dtEmployeeReference.Rows.Add(txtRefrenceName.Text, txtOrganizationEmployeeRefrence.Text, cmbDesignationEmployeeRefrence.Value, ((Control)(object)cmbDesignationEmployeeRefrence).Text, txtMobileEmployeeRefrence.Text, txtEmailEmployeeRefrence.Text, txtDescriptionEmployeeRefrence.Text);
				grdReference.DataSource = dtEmployeeReference;
				grdReference.RetrieveStructure();
				grdEmployeeReferenceSetting();
				ResetReference();
				txtRefrenceName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateReference_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeReferenceValidation())
			{
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["ReferenceName"] = txtRefrenceName.Text;
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["OrganizationName"] = txtOrganizationEmployeeRefrence.Text;
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["DesignationId"] = cmbDesignationEmployeeRefrence.Value;
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["Designation"] = ((Control)(object)cmbDesignationEmployeeRefrence).Text;
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["Mobile"] = txtMobileEmployeeRefrence.Text;
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["Email"] = txtEmailEmployeeRefrence.Text;
				dtEmployeeReference.Rows[EmployeeReferenceupdateDetailIndex]["Description"] = txtDescriptionEmployeeRefrence.Text;
				grdReference.DataSource = dtEmployeeReference;
				ResetReference();
				txtRefrenceName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdReference_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdReference.CurrentRow;
			if (item != null)
			{
				EmployeeReferenceupdateDetailIndex = item.RowIndex;
				txtRefrenceName.Text = item.Cells["ReferenceName"].Text;
				txtOrganizationEmployeeRefrence.Text = item.Cells["OrganizationName"].Text;
				cmbDesignationEmployeeRefrence.Value = item.Cells["DesignationId"].Value;
				txtMobileEmployeeRefrence.Text = item.Cells["Mobile"].Text;
				txtEmailEmployeeRefrence.Text = item.Cells["Email"].Text;
				txtDescriptionEmployeeRefrence.Text = item.Cells["Description"].Text;
				btnAddReference.Visible = false;
				btnUpdateReference.Visible = true;
				btnCancelReference.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool EmployeeBankAccountValidation()
	{
		if (((UltraGridBase)cmbBankAccount).ActiveRow == null)
		{
			MessageBox.Show("Bank Required");
			cmbBankAccount.Focus();
			return false;
		}
		if (txtAccountTitle.Text == string.Empty)
		{
			MessageBox.Show("Account Title Required");
			txtAccountTitle.Focus();
			return false;
		}
		if (txtAccountNo.Text == string.Empty)
		{
			MessageBox.Show("Account No Required");
			txtAccountNo.Focus();
			return false;
		}
		return true;
	}

	private void grdBankAccountSetting()
	{
		try
		{
			grdBankAccount.RootTable.Columns["BankId"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetBankAccount()
	{
		try
		{
			((Control)(object)cmbBankAccount).Text = string.Empty;
			txtAccountTitle.Text = string.Empty;
			txtAccountNo.Text = string.Empty;
			IsForPayrollBankAccount.Checked = false;
			btnAddBankAccount.Visible = true;
			btnUpdateBankAccount.Visible = false;
			btnCancelBankAccount.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelBankAccount_Click(object sender, EventArgs e)
	{
		try
		{
			ResetBankAccount();
			cmbBankAccount.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddBankAccount_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeBankAccountValidation())
			{
				dtEmployeeBankAccount.Rows.Add(cmbBankAccount.Value, ((Control)(object)cmbBankAccount).Text, txtAccountTitle.Text, txtAccountNo.Text, IsForPayrollBankAccount.Checked);
				grdBankAccount.DataSource = dtEmployeeBankAccount;
				grdBankAccount.RetrieveStructure();
				grdBankAccountSetting();
				ResetBankAccount();
				cmbBankAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateBankAccount_Click(object sender, EventArgs e)
	{
		try
		{
			if (EmployeeBankAccountValidation())
			{
				dtEmployeeBankAccount.Rows[EmployeeBankAccountupdateDetailIndex]["BankId"] = cmbBankAccount.Value;
				dtEmployeeBankAccount.Rows[EmployeeBankAccountupdateDetailIndex]["Bank"] = ((Control)(object)cmbBankAccount).Text;
				dtEmployeeBankAccount.Rows[EmployeeBankAccountupdateDetailIndex]["AccountTitle"] = txtAccountTitle.Text;
				dtEmployeeBankAccount.Rows[EmployeeBankAccountupdateDetailIndex]["AccountNo"] = txtAccountNo.Text;
				dtEmployeeBankAccount.Rows[EmployeeBankAccountupdateDetailIndex]["IsPayRoll"] = IsForPayrollBankAccount.Checked;
				grdBankAccount.DataSource = dtEmployeeBankAccount;
				ResetBankAccount();
				cmbBankAccount.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdBankAccount_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdBankAccount.CurrentRow;
			if (item != null)
			{
				EmployeeBankAccountupdateDetailIndex = item.RowIndex;
				cmbBankAccount.Value = item.Cells["BankId"].Value;
				txtAccountTitle.Text = item.Cells["AccountTitle"].Text;
				txtAccountNo.Text = item.Cells["AccountNo"].Text;
				IsForPayrollBankAccount.Checked = Conversion.ToBool(item.Cells["IsPayRoll"].Value);
				btnAddBankAccount.Visible = false;
				btnUpdateBankAccount.Visible = true;
				btnCancelBankAccount.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeRegistration_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				RetrivedDataEmployeeRegistration(Conversion.ToInt((object)grdEmployeeRegistration.CurrentRow.Cells[0].Value.ToString()));
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = HRM_Reports.EmployeeRegistrationSlipAndRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EmployeeId = Conversion.ToInt((object)grdEmployeeRegistration.CurrentRow.Cells[0].Value.ToString())
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1112-Employee_Registration_Slip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeBenefitsandWeekDaysCheckBoxNull()
	{
		try
		{
			GridEXRow[] rows = grdEmployeeAssetsBenefit.GetRows();
			foreach (GridEXRow obj in rows)
			{
				grdEmployeeAssetsBenefit.UpdateData();
				obj.BeginEdit();
				obj.Cells["Select"].Value = 0;
				grdEmployeeAssetsBenefit.UpdateData();
			}
			rows = grdEmployeeDirectBenefits.GetRows();
			foreach (GridEXRow obj2 in rows)
			{
				grdEmployeeDirectBenefits.UpdateData();
				obj2.BeginEdit();
				obj2.Cells["Select"].Value = 0;
				grdEmployeeDirectBenefits.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLocation_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbLocation).ActiveRow != null)
			{
				GridBindEmployeeSalary(Conversion.ToInt(cmbLocation.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeRegistration_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RetrivedDataEmployeeRegistration(Conversion.ToInt((object)grdEmployeeRegistration.CurrentRow.Cells[0].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateAllBenefits_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] rows;
			if (ChkAllBenefits.Checked)
			{
				if (grdEmployeeDirectBenefits.GetRows().Count() > 0)
				{
					GridEXRow r = grdEmployeeDirectBenefits.CurrentRow;
					rows = grdEmployeeDirectBenefits.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Select"].Value = 1;
						obj.Cells["ApprovalPersonId"].Value = Conversion.ToInt(r.Cells["ApprovalPersonId"].Value);
						obj.EndEdit();
					}
				}
				return;
			}
			GridEXRow r2 = grdEmployeeDirectBenefits.CurrentRow;
			rows = grdEmployeeDirectBenefits.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToBool(item.Cells["Select"].Value))
				{
					item.BeginEdit();
					item.Cells["ApprovalPersonId"].Value = Conversion.ToInt(r2.Cells["ApprovalPersonId"].Value);
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWeekDays_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		grdWeekDays.UpdateData();
	}

	private void grdEmployeeAssetsBenefit_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdEmployeeAssetsBenefit.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmployeeNoForUpdate_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			int EmployeeId = genEmployee.GetEmployeeIdByEmployeeNo(new genEmployee
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmployeeNo = txtEmployeeNoForUpdate.Text.Trim()
			});
			if (EmployeeId > 0)
			{
				RetrivedDataEmployeeRegistration(EmployeeId);
				return;
			}
			throw new Exception("Record Not Found For this EmployeeNo");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeDirectBenefits_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdEmployeeDirectBenefits.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDutyRoasterForm_Click(object sender, EventArgs e)
	{
		try
		{
			frmDutyRoasterNew obj = new frmDutyRoasterNew(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("DutyRoaster");
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You don't have the right to view this form...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShifttiming_Click(object sender, EventArgs e)
	{
		try
		{
			frmGenShiftTiming obj = new frmGenShiftTiming(UserAccount);
			obj.formright = CommonServices.SetRightsValueInRightsObject("ShiftTiming");
			if (obj.formright.DoHaveViewRight)
			{
				obj.Show();
			}
			else
			{
				MessageBox.Show("You don't have the right to view this form...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEmployeeNoForUpdate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddressType_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 59;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCityName_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineDegree_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 57;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSalaryBreakUp_Click(object sender, EventArgs e)
	{
		try
		{
			new SalaryBreakupPolicy(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineBenefits_Click(object sender, EventArgs e)
	{
		try
		{
			new BenifitDefine(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCoa1_Click(object sender, EventArgs e)
	{
		try
		{
			if (CommonServices.SetRightsValueInRightsObject("AcfrmDefCoa").DoHaveViewRight)
			{
				new AcfrmDefCoa(UserAccount).Show();
			}
			else
			{
				MessageBox.Show("You Dont Have rights View Of This Form..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btncoa2_Click(object sender, EventArgs e)
	{
		try
		{
			if (CommonServices.SetRightsValueInRightsObject("AcfrmDefCoa").DoHaveViewRight)
			{
				new AcfrmDefCoa(UserAccount).Show();
			}
			else
			{
				MessageBox.Show("You Dont Have rights View Of This Form..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btncoa3_Click(object sender, EventArgs e)
	{
		try
		{
			if (CommonServices.SetRightsValueInRightsObject("AcfrmDefCoa").DoHaveViewRight)
			{
				new AcfrmDefCoa(UserAccount).Show();
			}
			else
			{
				MessageBox.Show("You Dont Have rights View Of This Form..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDepartment_Leave(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbDepartment).ActiveRow != null)
			{
				DataTable dtCoa = genEmployee.GetCaoByDepartmentId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.OrganizationId,
					DepartmentId = Conversion.ToInt(cmbDepartment.Value)
				});
				if (dtCoa != null && dtCoa.Rows.Count > 0)
				{
					CmbSalaryPayableAc.Value = Conversion.ToInt(dtCoa.Rows[0]["PayableAcId"]);
					CmbSalariesExpensesAc.Value = Conversion.ToInt(dtCoa.Rows[0]["ExpenseAccountId"]);
					CmbSalaryLoanAc.Value = Conversion.ToInt(dtCoa.Rows[0]["LoanAcId"]);
				}
				else
				{
					((UltraGridBase)CmbSalaryPayableAc).Rows[0].Activate();
					((UltraGridBase)CmbSalariesExpensesAc).Rows[0].Activate();
					((UltraGridBase)CmbSalaryLoanAc).Rows[0].Activate();
				}
			}
			else
			{
				((UltraGridBase)CmbSalaryPayableAc).Rows[0].Activate();
				((UltraGridBase)CmbSalariesExpensesAc).Rows[0].Activate();
				((UltraGridBase)CmbSalaryLoanAc).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEmployeeRegistration_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_099f: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_09cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0b: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Expected O, but got Unknown
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a21: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Expected O, but got Unknown
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a37: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a42: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4d: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a84: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8f: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad1: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1e: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b60: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6b: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b81: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8c: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Expected O, but got Unknown
		//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bef: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c10: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1b: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c52: Expected O, but got Unknown
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Expected O, but got Unknown
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d87: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4d: Expected O, but got Unknown
		//IL_0e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e58: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Expected O, but got Unknown
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e84: Expected O, but got Unknown
		//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8f: Expected O, but got Unknown
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Expected O, but got Unknown
		//IL_0fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fce: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_107e: Expected O, but got Unknown
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Expected O, but got Unknown
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Expected O, but got Unknown
		//IL_1103: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Expected O, but got Unknown
		//IL_1119: Unknown result type (might be due to invalid IL or missing references)
		//IL_1123: Expected O, but got Unknown
		//IL_1150: Unknown result type (might be due to invalid IL or missing references)
		//IL_115a: Expected O, but got Unknown
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d3: Expected O, but got Unknown
		//IL_11ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f4: Expected O, but got Unknown
		//IL_122c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Expected O, but got Unknown
		//IL_1284: Unknown result type (might be due to invalid IL or missing references)
		//IL_128e: Expected O, but got Unknown
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fc: Expected O, but got Unknown
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_1333: Expected O, but got Unknown
		//IL_1381: Unknown result type (might be due to invalid IL or missing references)
		//IL_138b: Expected O, but got Unknown
		//IL_13c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cd: Expected O, but got Unknown
		//IL_143c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1446: Expected O, but got Unknown
		//IL_149f: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a9: Expected O, but got Unknown
		//IL_150d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Expected O, but got Unknown
		//IL_1591: Unknown result type (might be due to invalid IL or missing references)
		//IL_159b: Expected O, but got Unknown
		//IL_15a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b1: Expected O, but got Unknown
		//IL_162b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1635: Expected O, but got Unknown
		//IL_1636: Unknown result type (might be due to invalid IL or missing references)
		//IL_1640: Expected O, but got Unknown
		//IL_1641: Unknown result type (might be due to invalid IL or missing references)
		//IL_164b: Expected O, but got Unknown
		//IL_164c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1656: Expected O, but got Unknown
		//IL_1657: Unknown result type (might be due to invalid IL or missing references)
		//IL_1661: Expected O, but got Unknown
		//IL_1662: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Expected O, but got Unknown
		//IL_18d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18df: Expected O, but got Unknown
		//IL_7781: Unknown result type (might be due to invalid IL or missing references)
		//IL_778b: Expected O, but got Unknown
		//IL_7909: Unknown result type (might be due to invalid IL or missing references)
		//IL_7913: Expected O, but got Unknown
		//IL_7f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f37: Expected O, but got Unknown
		//IL_14471: Unknown result type (might be due to invalid IL or missing references)
		//IL_1447b: Expected O, but got Unknown
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.EmployeeManagement.frmEmployeeRegistration));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.btncoa2 = new System.Windows.Forms.Button();
		this.btncoa3 = new System.Windows.Forms.Button();
		this.btnCoa1 = new System.Windows.Forms.Button();
		this.CmbSalaryLoanAc = new UltraCombo();
		this.CmbSalaryPayableAc = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbSalariesExpensesAc = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.btnEmployeeGroup = new System.Windows.Forms.Button();
		this.cmbEmployeeGroup = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtProbationEndingDate = new System.Windows.Forms.DateTimePicker();
		this.btnEmployeeType = new System.Windows.Forms.Button();
		this.cmbEmployeeType = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.btnReportedPerson = new System.Windows.Forms.Button();
		this.btnSection = new System.Windows.Forms.Button();
		this.btnShift = new System.Windows.Forms.Button();
		this.btnReportedBranch = new System.Windows.Forms.Button();
		this.btnLocation = new System.Windows.Forms.Button();
		this.btnDesignation = new System.Windows.Forms.Button();
		this.btnStore = new System.Windows.Forms.Button();
		this.btnDepartment = new System.Windows.Forms.Button();
		this.IsActiveEmployeeInformation = new System.Windows.Forms.CheckBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtSalary = new System.Windows.Forms.TextBox();
		this.cmbShift = new UltraCombo();
		this.cmbDesignation = new UltraCombo();
		this.cmbSection = new UltraCombo();
		this.cmbDepartment = new UltraCombo();
		this.cmbStore = new UltraCombo();
		this.cmbReportedPerson = new UltraCombo();
		this.cmbReportedBranch = new UltraCombo();
		this.cmbLocation = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtSiteLocation = new System.Windows.Forms.TextBox();
		this.lblReportedBranch = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.label53 = new System.Windows.Forms.Label();
		this.txtStartDate = new System.Windows.Forms.DateTimePicker();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdEmployeeSalary = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.btnSalaryBreakUp = new System.Windows.Forms.Button();
		this.label40 = new System.Windows.Forms.Label();
		this.btnGenerateBreakup = new System.Windows.Forms.Button();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdEmployeeAssetsBenefit = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdEmployeeDirectBenefits = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.btnDefineBenefits = new System.Windows.Forms.Button();
		this.btnGenerateAllBenefits = new System.Windows.Forms.Button();
		this.ChkAllBenefits = new System.Windows.Forms.CheckBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.tabPage8 = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdWeekDays = new GridEX();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.tabControl4 = new System.Windows.Forms.TabControl();
		this.tabPage9 = new System.Windows.Forms.TabPage();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel22 = new System.Windows.Forms.Panel();
		this.grdAddressDetail = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.btnCityName = new System.Windows.Forms.Button();
		this.btnAddressType = new System.Windows.Forms.Button();
		this.btnCancelAddressDetail = new System.Windows.Forms.Button();
		this.btnUpdateAddressDetail = new System.Windows.Forms.Button();
		this.btnAddAddressDetail = new System.Windows.Forms.Button();
		this.label64 = new System.Windows.Forms.Label();
		this.txtAddressDetail = new System.Windows.Forms.TextBox();
		this.cmbProvince = new UltraCombo();
		this.label65 = new System.Windows.Forms.Label();
		this.cmbCity = new UltraCombo();
		this.label66 = new System.Windows.Forms.Label();
		this.cmbAddressType = new UltraCombo();
		this.label67 = new System.Windows.Forms.Label();
		this.cmbCountry = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.tabPage10 = new System.Windows.Forms.TabPage();
		this.panel23 = new System.Windows.Forms.Panel();
		this.panel25 = new System.Windows.Forms.Panel();
		this.grdEmployeeExperience = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.btnCancelExperience = new System.Windows.Forms.Button();
		this.btnUpdateExperience = new System.Windows.Forms.Button();
		this.btnAddExperience = new System.Windows.Forms.Button();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label72 = new System.Windows.Forms.Label();
		this.dtpTo = new System.Windows.Forms.DateTimePicker();
		this.dtpFrom = new System.Windows.Forms.DateTimePicker();
		this.txtOrganization = new System.Windows.Forms.TextBox();
		this.label73 = new System.Windows.Forms.Label();
		this.cmbCityEmployeeExperience = new UltraCombo();
		this.label74 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDesignationEmployeeExperience = new UltraCombo();
		this.label76 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.tabPage11 = new System.Windows.Forms.TabPage();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.grdEmployeeEducation = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.btnDefineDegree = new System.Windows.Forms.Button();
		this.btnCancelEducation = new System.Windows.Forms.Button();
		this.btnUpdateEducation = new System.Windows.Forms.Button();
		this.btnAddEducation = new System.Windows.Forms.Button();
		this.PassingYearDate = new System.Windows.Forms.DateTimePicker();
		this.StartYearDate = new System.Windows.Forms.DateTimePicker();
		this.cmbDegree = new UltraCombo();
		this.txtCGPA = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.txtInstitute = new System.Windows.Forms.TextBox();
		this.label81 = new System.Windows.Forms.Label();
		this.txtDegreeTitle = new System.Windows.Forms.TextBox();
		this.label83 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.cmbCityEducation = new UltraCombo();
		this.label85 = new System.Windows.Forms.Label();
		this.tabPage12 = new System.Windows.Forms.TabPage();
		this.panel29 = new System.Windows.Forms.Panel();
		this.panel31 = new System.Windows.Forms.Panel();
		this.grdFamilyInfo = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.btnCancelFamily = new System.Windows.Forms.Button();
		this.btnUpdateFamily = new System.Windows.Forms.Button();
		this.btnAddFamily = new System.Windows.Forms.Button();
		this.txtDescriptionFamilyInfo = new System.Windows.Forms.TextBox();
		this.label90 = new System.Windows.Forms.Label();
		this.txtNameFamilyInfo = new System.Windows.Forms.TextBox();
		this.cmbRelationShip = new UltraCombo();
		this.label91 = new System.Windows.Forms.Label();
		this.label87 = new System.Windows.Forms.Label();
		this.tabPage13 = new System.Windows.Forms.TabPage();
		this.panel32 = new System.Windows.Forms.Panel();
		this.panel34 = new System.Windows.Forms.Panel();
		this.grdReference = new GridEX();
		this.panel33 = new System.Windows.Forms.Panel();
		this.btnCancelReference = new System.Windows.Forms.Button();
		this.btnUpdateReference = new System.Windows.Forms.Button();
		this.btnAddReference = new System.Windows.Forms.Button();
		this.txtDescriptionEmployeeRefrence = new System.Windows.Forms.TextBox();
		this.label92 = new System.Windows.Forms.Label();
		this.txtEmailEmployeeRefrence = new System.Windows.Forms.TextBox();
		this.label93 = new System.Windows.Forms.Label();
		this.txtMobileEmployeeRefrence = new System.Windows.Forms.TextBox();
		this.label94 = new System.Windows.Forms.Label();
		this.cmbDesignationEmployeeRefrence = new UltraCombo();
		this.txtOrganizationEmployeeRefrence = new System.Windows.Forms.TextBox();
		this.label95 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.txtRefrenceName = new System.Windows.Forms.TextBox();
		this.label100 = new System.Windows.Forms.Label();
		this.tabPage14 = new System.Windows.Forms.TabPage();
		this.panel35 = new System.Windows.Forms.Panel();
		this.panel37 = new System.Windows.Forms.Panel();
		this.grdBankAccount = new GridEX();
		this.panel36 = new System.Windows.Forms.Panel();
		this.btnCancelBankAccount = new System.Windows.Forms.Button();
		this.btnUpdateBankAccount = new System.Windows.Forms.Button();
		this.btnAddBankAccount = new System.Windows.Forms.Button();
		this.IsForPayrollBankAccount = new System.Windows.Forms.CheckBox();
		this.txtAccountNo = new System.Windows.Forms.TextBox();
		this.label101 = new System.Windows.Forms.Label();
		this.txtAccountTitle = new System.Windows.Forms.TextBox();
		this.label102 = new System.Windows.Forms.Label();
		this.cmbBankAccount = new UltraCombo();
		this.label104 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtMobile1 = new System.Windows.Forms.MaskedTextBox();
		this.btnBloodGroup = new System.Windows.Forms.Button();
		this.btnNationality = new System.Windows.Forms.Button();
		this.btnGender = new System.Windows.Forms.Button();
		this.btnReleaseTitle = new System.Windows.Forms.Button();
		this.btnTitle = new System.Windows.Forms.Button();
		this.btnEmployeeCategory = new System.Windows.Forms.Button();
		this.label36 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.pictureBoxEmployeeSignature = new System.Windows.Forms.PictureBox();
		this.cmbGender = new UltraCombo();
		this.cmbRelationTitle = new UltraCombo();
		this.cmbTitle = new UltraCombo();
		this.cmbBloodGroup = new UltraCombo();
		this.cmbEmployeeCatagory = new UltraCombo();
		this.cmbNationality = new UltraCombo();
		this.btnEmployeeSignature = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.btnEmployeePicture = new System.Windows.Forms.Button();
		this.pictureBoxEmployeePhoto = new System.Windows.Forms.PictureBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.dtpCNICExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.txtMobile2 = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtRelationMobileNo = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtLastName = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtCNIC = new System.Windows.Forms.MaskedTextBox();
		this.txtLicenseNo = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtPassportNo = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.dtpCNICIssueDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.dtpDOB = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtRelationName = new System.Windows.Forms.TextBox();
		this.txtMiddleName = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtFirstName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtEmployeeNoForUpdate = new System.Windows.Forms.TextBox();
		this.txtEmployeeNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShifttiming = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDutyRoasterForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdEmployeeRegistration = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnPrintRegister = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl3.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalaryLoanAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalaryPayableAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalariesExpensesAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStore).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportedPerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportedBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).BeginInit();
		this.tabPage6.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeSalary).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage7.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeAssetsBenefit).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeDirectBenefits).BeginInit();
		this.panel15.SuspendLayout();
		this.tabPage8.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWeekDays).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel19.SuspendLayout();
		this.tabControl4.SuspendLayout();
		this.tabPage9.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAddressDetail).BeginInit();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbProvince).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAddressType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCountry).BeginInit();
		this.tabPage10.SuspendLayout();
		this.panel23.SuspendLayout();
		this.panel25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeExperience).BeginInit();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCityEmployeeExperience).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignationEmployeeExperience).BeginInit();
		this.tabPage11.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel28.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeEducation).BeginInit();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDegree).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCityEducation).BeginInit();
		this.tabPage12.SuspendLayout();
		this.panel29.SuspendLayout();
		this.panel31.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFamilyInfo).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRelationShip).BeginInit();
		this.tabPage13.SuspendLayout();
		this.panel32.SuspendLayout();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdReference).BeginInit();
		this.panel33.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignationEmployeeRefrence).BeginInit();
		this.tabPage14.SuspendLayout();
		this.panel35.SuspendLayout();
		this.panel37.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBankAccount).BeginInit();
		this.panel36.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBankAccount).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxEmployeeSignature).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGender).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRelationTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBloodGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCatagory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNationality).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxEmployeePhoto).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeRegistration).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 742);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 742);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 716);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 710);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 380);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 330);
		this.panel5.TabIndex = 2;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(950, 330);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(942, 304);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Employee History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.tabControl3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(936, 298);
		this.panel9.TabIndex = 0;
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Controls.Add(this.tabPage7);
		this.tabControl3.Controls.Add(this.tabPage8);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl3.Location = new System.Drawing.Point(0, 0);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(936, 298);
		this.tabControl3.TabIndex = 0;
		this.tabPage5.Controls.Add(this.panel10);
		this.tabPage5.Location = new System.Drawing.Point(4, 22);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(928, 272);
		this.tabPage5.TabIndex = 0;
		this.tabPage5.Text = "Employee Information";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.btncoa2);
		this.panel10.Controls.Add(this.btncoa3);
		this.panel10.Controls.Add(this.btnCoa1);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalaryLoanAc);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalaryPayableAc);
		this.panel10.Controls.Add(this.label54);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalariesExpensesAc);
		this.panel10.Controls.Add(this.label55);
		this.panel10.Controls.Add(this.label56);
		this.panel10.Controls.Add(this.btnEmployeeGroup);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeGroup);
		this.panel10.Controls.Add(this.label26);
		this.panel10.Controls.Add(this.label17);
		this.panel10.Controls.Add(this.txtProbationEndingDate);
		this.panel10.Controls.Add(this.btnEmployeeType);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeType);
		this.panel10.Controls.Add(this.label2);
		this.panel10.Controls.Add(this.btnReportedPerson);
		this.panel10.Controls.Add(this.btnSection);
		this.panel10.Controls.Add(this.btnShift);
		this.panel10.Controls.Add(this.btnReportedBranch);
		this.panel10.Controls.Add(this.btnLocation);
		this.panel10.Controls.Add(this.btnDesignation);
		this.panel10.Controls.Add(this.btnStore);
		this.panel10.Controls.Add(this.btnDepartment);
		this.panel10.Controls.Add(this.IsActiveEmployeeInformation);
		this.panel10.Controls.Add(this.label38);
		this.panel10.Controls.Add(this.txtSalary);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShift);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignation);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStore);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportedPerson);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportedBranch);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocation);
		this.panel10.Controls.Add(this.label44);
		this.panel10.Controls.Add(this.label45);
		this.panel10.Controls.Add(this.label46);
		this.panel10.Controls.Add(this.label47);
		this.panel10.Controls.Add(this.label48);
		this.panel10.Controls.Add(this.label49);
		this.panel10.Controls.Add(this.label50);
		this.panel10.Controls.Add(this.txtSiteLocation);
		this.panel10.Controls.Add(this.lblReportedBranch);
		this.panel10.Controls.Add(this.label51);
		this.panel10.Controls.Add(this.label52);
		this.panel10.Controls.Add(this.txtEndDate);
		this.panel10.Controls.Add(this.label53);
		this.panel10.Controls.Add(this.txtStartDate);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(922, 266);
		this.panel10.TabIndex = 0;
		this.btncoa2.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btncoa2.Location = new System.Drawing.Point(579, 66);
		this.btncoa2.Name = "btncoa2";
		this.btncoa2.Size = new System.Drawing.Size(31, 25);
		this.btncoa2.TabIndex = 14;
		this.btncoa2.TabStop = false;
		this.btncoa2.UseVisualStyleBackColor = true;
		this.btncoa2.Click += new System.EventHandler(btncoa2_Click);
		this.btncoa3.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btncoa3.Location = new System.Drawing.Point(579, 96);
		this.btncoa3.Name = "btncoa3";
		this.btncoa3.Size = new System.Drawing.Size(31, 25);
		this.btncoa3.TabIndex = 16;
		this.btncoa3.TabStop = false;
		this.btncoa3.UseVisualStyleBackColor = true;
		this.btncoa3.Click += new System.EventHandler(btncoa3_Click);
		this.btnCoa1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnCoa1.Location = new System.Drawing.Point(579, 36);
		this.btnCoa1.Name = "btnCoa1";
		this.btnCoa1.Size = new System.Drawing.Size(31, 25);
		this.btnCoa1.TabIndex = 12;
		this.btnCoa1.TabStop = false;
		this.btnCoa1.UseVisualStyleBackColor = true;
		this.btnCoa1.Click += new System.EventHandler(btnCoa1_Click);
		this.CmbSalaryLoanAc.AllowNull = (DefaultableBoolean)1;
		this.CmbSalaryLoanAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalaryLoanAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalaryLoanAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance369).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance369;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance370).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance370).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance370).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance370;
		((AppearanceBase)appearance371).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance371;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance372).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance372).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance372).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance372;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance373).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance373).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance373;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance375).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance375;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance376).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance376;
		((AppearanceBase)appearance377).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance377).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance378).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance378).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance378).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance378).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance378;
		((AppearanceBase)appearance379).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance380).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance380;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance381).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance381;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalaryLoanAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalaryLoanAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSalaryLoanAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalaryLoanAc).Location = new System.Drawing.Point(407, 95);
		((System.Windows.Forms.Control)(object)this.CmbSalaryLoanAc).Name = "CmbSalaryLoanAc";
		((System.Windows.Forms.Control)(object)this.CmbSalaryLoanAc).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalaryLoanAc).TabIndex = 15;
		this.CmbSalaryPayableAc.AllowNull = (DefaultableBoolean)1;
		this.CmbSalaryPayableAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalaryPayableAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalaryPayableAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalaryPayableAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalaryPayableAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSalaryPayableAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalaryPayableAc).Location = new System.Drawing.Point(407, 35);
		((System.Windows.Forms.Control)(object)this.CmbSalaryPayableAc).Name = "CmbSalaryPayableAc";
		((System.Windows.Forms.Control)(object)this.CmbSalaryPayableAc).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalaryPayableAc).TabIndex = 11;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(304, 71);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(102, 15);
		this.label54.TabIndex = 251;
		this.label54.Text = "Salary Expense Ac";
		this.CmbSalariesExpensesAc.AllowNull = (DefaultableBoolean)1;
		this.CmbSalariesExpensesAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalariesExpensesAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalariesExpensesAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance220).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalariesExpensesAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalariesExpensesAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSalariesExpensesAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalariesExpensesAc).Location = new System.Drawing.Point(407, 65);
		((System.Windows.Forms.Control)(object)this.CmbSalariesExpensesAc).Name = "CmbSalariesExpensesAc";
		((System.Windows.Forms.Control)(object)this.CmbSalariesExpensesAc).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalariesExpensesAc).TabIndex = 13;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(304, 41);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(100, 15);
		this.label55.TabIndex = 252;
		this.label55.Text = "Salary Payable Ac";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(304, 101);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(50, 15);
		this.label56.TabIndex = 253;
		this.label56.Text = "Loan Ac";
		this.btnEmployeeGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeGroup.Location = new System.Drawing.Point(580, 155);
		this.btnEmployeeGroup.Name = "btnEmployeeGroup";
		this.btnEmployeeGroup.Size = new System.Drawing.Size(30, 26);
		this.btnEmployeeGroup.TabIndex = 20;
		this.btnEmployeeGroup.TabStop = false;
		this.btnEmployeeGroup.UseVisualStyleBackColor = true;
		this.btnEmployeeGroup.Click += new System.EventHandler(btnEmployeeGroup_Click);
		this.cmbEmployeeGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance356).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance357).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance357).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance357).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance357;
		((AppearanceBase)appearance358).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance358;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance359).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance359).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance359).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance359).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance359;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance360).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance360;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance361).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance361;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance362;
		((AppearanceBase)appearance364).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance364).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance365).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance365).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance365).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance365).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance366).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance367).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance367;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance368).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance368;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeGroup).Location = new System.Drawing.Point(407, 155);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeGroup).Name = "cmbEmployeeGroup";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeGroup).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeGroup).TabIndex = 19;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(305, 161);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(95, 15);
		this.label26.TabIndex = 245;
		this.label26.Text = "Employee Group";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 71);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(110, 15);
		this.label17.TabIndex = 243;
		this.label17.Text = "Probation End Date";
		this.txtProbationEndingDate.CustomFormat = "dd-MMM-yyyy";
		this.txtProbationEndingDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProbationEndingDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtProbationEndingDate.Location = new System.Drawing.Point(116, 67);
		this.txtProbationEndingDate.Name = "txtProbationEndingDate";
		this.txtProbationEndingDate.Size = new System.Drawing.Size(184, 23);
		this.txtProbationEndingDate.TabIndex = 2;
		this.btnEmployeeType.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeType.Location = new System.Drawing.Point(580, 125);
		this.btnEmployeeType.Name = "btnEmployeeType";
		this.btnEmployeeType.Size = new System.Drawing.Size(30, 26);
		this.btnEmployeeType.TabIndex = 18;
		this.btnEmployeeType.TabStop = false;
		this.btnEmployeeType.UseVisualStyleBackColor = true;
		this.btnEmployeeType.Click += new System.EventHandler(btnEmployeeType_Click);
		this.cmbEmployeeType.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance271).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance271).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance330).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance341).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance352).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance352;
		((AppearanceBase)appearance363).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance363).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance363;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance374).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance374).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance374).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance374).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance374).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance374;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeType).Location = new System.Drawing.Point(407, 125);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeType).Name = "cmbEmployeeType";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeType).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeType).TabIndex = 17;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(305, 131);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(87, 15);
		this.label2.TabIndex = 241;
		this.label2.Text = "Employee Type";
		this.btnReportedPerson.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnReportedPerson.Location = new System.Drawing.Point(884, 65);
		this.btnReportedPerson.Name = "btnReportedPerson";
		this.btnReportedPerson.Size = new System.Drawing.Size(30, 26);
		this.btnReportedPerson.TabIndex = 26;
		this.btnReportedPerson.TabStop = false;
		this.btnReportedPerson.UseVisualStyleBackColor = true;
		this.btnReportedPerson.Click += new System.EventHandler(btnReportedPerson_Click);
		this.btnSection.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnSection.Location = new System.Drawing.Point(270, 155);
		this.btnSection.Name = "btnSection";
		this.btnSection.Size = new System.Drawing.Size(30, 26);
		this.btnSection.TabIndex = 8;
		this.btnSection.TabStop = false;
		this.btnSection.UseVisualStyleBackColor = true;
		this.btnSection.Click += new System.EventHandler(btnSection_Click);
		this.btnShift.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnShift.Location = new System.Drawing.Point(270, 125);
		this.btnShift.Name = "btnShift";
		this.btnShift.Size = new System.Drawing.Size(30, 26);
		this.btnShift.TabIndex = 6;
		this.btnShift.TabStop = false;
		this.btnShift.UseVisualStyleBackColor = true;
		this.btnShift.Click += new System.EventHandler(btnShift_Click);
		this.btnReportedBranch.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnReportedBranch.Location = new System.Drawing.Point(884, 35);
		this.btnReportedBranch.Name = "btnReportedBranch";
		this.btnReportedBranch.Size = new System.Drawing.Size(30, 26);
		this.btnReportedBranch.TabIndex = 24;
		this.btnReportedBranch.TabStop = false;
		this.btnReportedBranch.UseVisualStyleBackColor = true;
		this.btnReportedBranch.Click += new System.EventHandler(btnReportedBranch_Click);
		this.btnLocation.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLocation.Location = new System.Drawing.Point(884, 5);
		this.btnLocation.Name = "btnLocation";
		this.btnLocation.Size = new System.Drawing.Size(30, 26);
		this.btnLocation.TabIndex = 22;
		this.btnLocation.TabStop = false;
		this.btnLocation.UseVisualStyleBackColor = true;
		this.btnLocation.Click += new System.EventHandler(btnLocation_Click);
		this.btnDesignation.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDesignation.Location = new System.Drawing.Point(270, 95);
		this.btnDesignation.Name = "btnDesignation";
		this.btnDesignation.Size = new System.Drawing.Size(30, 26);
		this.btnDesignation.TabIndex = 4;
		this.btnDesignation.TabStop = false;
		this.btnDesignation.UseVisualStyleBackColor = true;
		this.btnDesignation.Click += new System.EventHandler(btnDesignation_Click);
		this.btnStore.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnStore.Location = new System.Drawing.Point(852, 227);
		this.btnStore.Name = "btnStore";
		this.btnStore.Size = new System.Drawing.Size(30, 26);
		this.btnStore.TabIndex = 13;
		this.btnStore.TabStop = false;
		this.btnStore.UseVisualStyleBackColor = true;
		this.btnStore.Visible = false;
		this.btnStore.Click += new System.EventHandler(btnStore_Click);
		this.btnDepartment.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDepartment.Location = new System.Drawing.Point(580, 5);
		this.btnDepartment.Name = "btnDepartment";
		this.btnDepartment.Size = new System.Drawing.Size(30, 26);
		this.btnDepartment.TabIndex = 10;
		this.btnDepartment.TabStop = false;
		this.btnDepartment.UseVisualStyleBackColor = true;
		this.btnDepartment.Click += new System.EventHandler(btnDepartment_Click);
		this.IsActiveEmployeeInformation.AutoSize = true;
		this.IsActiveEmployeeInformation.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsActiveEmployeeInformation.Location = new System.Drawing.Point(709, 130);
		this.IsActiveEmployeeInformation.Name = "IsActiveEmployeeInformation";
		this.IsActiveEmployeeInformation.Size = new System.Drawing.Size(71, 19);
		this.IsActiveEmployeeInformation.TabIndex = 28;
		this.IsActiveEmployeeInformation.Text = "Is Active";
		this.IsActiveEmployeeInformation.UseVisualStyleBackColor = true;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(610, 101);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(68, 15);
		this.label38.TabIndex = 127;
		this.label38.Text = "Total Salary";
		this.txtSalary.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSalary.Location = new System.Drawing.Point(709, 97);
		this.txtSalary.Name = "txtSalary";
		this.txtSalary.Size = new System.Drawing.Size(205, 23);
		this.txtSalary.TabIndex = 27;
		this.txtSalary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSalary_KeyPress);
		this.cmbShift.AllowNull = (DefaultableBoolean)1;
		this.cmbShift.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShift.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShift.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShift).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbShift).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShift).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShift).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance163).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShift).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShift).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShift).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShift).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShift.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShift).Location = new System.Drawing.Point(116, 125);
		((System.Windows.Forms.Control)(object)this.cmbShift).Name = "cmbShift";
		((System.Windows.Forms.Control)(object)this.cmbShift).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbShift).TabIndex = 5;
		this.cmbDesignation.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance177).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Location = new System.Drawing.Point(116, 95);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Name = "cmbDesignation";
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).TabIndex = 3;
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance184;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance190).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(116, 155);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 7;
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance329).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance331).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance331).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance331).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance331;
		((AppearanceBase)appearance332).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance332;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance333).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance333).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance333).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance333).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance334).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance334;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance335).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance336;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance337).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance337;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance338).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance338).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance338).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance338).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance338).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance338;
		((AppearanceBase)appearance339).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance340).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance340;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance342;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(407, 5);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Leave += new System.EventHandler(cmbDepartment_Leave);
		this.cmbStore.AllowNull = (DefaultableBoolean)1;
		this.cmbStore.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStore.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStore.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance343).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStore).DisplayLayout.Appearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.cmbStore).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStore).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance344).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance344).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStore).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance344;
		((AppearanceBase)appearance345).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStore).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance345;
		((SpecialBoxBase)((UltraGridBase)this.cmbStore).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance346).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance346).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance346).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStore).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.cmbStore).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStore).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance347).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance347;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance349;
		((AppearanceBase)appearance350).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance350).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance350;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance351).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance351).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance351).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance351).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance351;
		((AppearanceBase)appearance353).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance353;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance354).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance354).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStore).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.cmbStore).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStore).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStore).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStore).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStore.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStore).Location = new System.Drawing.Point(223, -163);
		((System.Windows.Forms.Control)(object)this.cmbStore).Name = "cmbStore";
		((System.Windows.Forms.Control)(object)this.cmbStore).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbStore).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbStore).Visible = false;
		this.cmbReportedPerson.AllowNull = (DefaultableBoolean)1;
		this.cmbReportedPerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportedPerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportedPerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportedPerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance145;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportedPerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportedPerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportedPerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportedPerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportedPerson).Location = new System.Drawing.Point(709, 65);
		((System.Windows.Forms.Control)(object)this.cmbReportedPerson).Name = "cmbReportedPerson";
		((System.Windows.Forms.Control)(object)this.cmbReportedPerson).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportedPerson).TabIndex = 25;
		this.cmbReportedBranch.AllowNull = (DefaultableBoolean)1;
		this.cmbReportedBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportedBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportedBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportedBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportedBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportedBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportedBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportedBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportedBranch).Location = new System.Drawing.Point(709, 35);
		((System.Windows.Forms.Control)(object)this.cmbReportedBranch).Name = "cmbReportedBranch";
		((System.Windows.Forms.Control)(object)this.cmbReportedBranch).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportedBranch).TabIndex = 23;
		this.cmbLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance382).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance382;
		((UltraGridBase)this.cmbLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance383).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance383).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance383).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance383;
		((AppearanceBase)appearance384).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance384;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocation).Location = new System.Drawing.Point(709, 5);
		((System.Windows.Forms.Control)(object)this.cmbLocation).Name = "cmbLocation";
		((System.Windows.Forms.Control)(object)this.cmbLocation).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocation).TabIndex = 21;
		this.cmbLocation.ValueChanged += new System.EventHandler(cmbLocation_ValueChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(610, 71);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(94, 15);
		this.label44.TabIndex = 123;
		this.label44.Text = "Reported Person";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(6, 101);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(71, 15);
		this.label45.TabIndex = 122;
		this.label45.Text = "Designation";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(6, 131);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(32, 15);
		this.label46.TabIndex = 121;
		this.label46.Text = "Shift";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(6, 161);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(47, 15);
		this.label47.TabIndex = 120;
		this.label47.Text = "Section";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(592, 233);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(35, 15);
		this.label48.TabIndex = 119;
		this.label48.Text = "Store";
		this.label48.Visible = false;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(305, 11);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(71, 15);
		this.label49.TabIndex = 118;
		this.label49.Text = "Department";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(589, 202);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(76, 15);
		this.label50.TabIndex = 117;
		this.label50.Text = "Site Location";
		this.label50.Visible = false;
		this.txtSiteLocation.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSiteLocation.Location = new System.Drawing.Point(672, 198);
		this.txtSiteLocation.Name = "txtSiteLocation";
		this.txtSiteLocation.Size = new System.Drawing.Size(226, 23);
		this.txtSiteLocation.TabIndex = 104;
		this.txtSiteLocation.Visible = false;
		this.lblReportedBranch.AutoSize = true;
		this.lblReportedBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblReportedBranch.Location = new System.Drawing.Point(610, 41);
		this.lblReportedBranch.Name = "lblReportedBranch";
		this.lblReportedBranch.Size = new System.Drawing.Size(95, 15);
		this.lblReportedBranch.TabIndex = 116;
		this.lblReportedBranch.Text = "Reported Branch";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(610, 11);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(53, 15);
		this.label51.TabIndex = 115;
		this.label51.Text = "Location";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(6, 41);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(55, 15);
		this.label52.TabIndex = 114;
		this.label52.Text = "End Date";
		this.txtEndDate.CustomFormat = "dd-MMM-yyyy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(116, 37);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(184, 23);
		this.txtEndDate.TabIndex = 1;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(6, 11);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(60, 15);
		this.label53.TabIndex = 113;
		this.label53.Text = "Start Date";
		this.txtStartDate.CustomFormat = "dd-MMM-yyyy";
		this.txtStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStartDate.Location = new System.Drawing.Point(116, 7);
		this.txtStartDate.Name = "txtStartDate";
		this.txtStartDate.Size = new System.Drawing.Size(184, 23);
		this.txtStartDate.TabIndex = 0;
		this.tabPage6.Controls.Add(this.panel11);
		this.tabPage6.Location = new System.Drawing.Point(4, 22);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(928, 272);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "Employee Salary";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add(this.panel13);
		this.panel11.Controls.Add(this.panel12);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(922, 266);
		this.panel11.TabIndex = 0;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeSalary);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 32);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(922, 234);
		this.panel13.TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeSalary.FilterMode = (FilterMode)2;
		this.grdEmployeeSalary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeSalary.GroupByBoxVisible = false;
		this.grdEmployeeSalary.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).Name = "grdEmployeeSalary";
		((ScrollableBase)this.grdEmployeeSalary).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeSalary).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeSalary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).Size = new System.Drawing.Size(922, 234);
		((System.Windows.Forms.Control)(object)this.grdEmployeeSalary).TabIndex = 2;
		this.grdEmployeeSalary.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeSalary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeSalary).VisualStyle = (VisualStyle)4;
		this.panel12.Controls.Add(this.btnSalaryBreakUp);
		this.panel12.Controls.Add(this.label40);
		this.panel12.Controls.Add(this.btnGenerateBreakup);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(922, 32);
		this.panel12.TabIndex = 0;
		this.btnSalaryBreakUp.BackColor = System.Drawing.Color.Teal;
		this.btnSalaryBreakUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSalaryBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSalaryBreakUp.ForeColor = System.Drawing.Color.White;
		this.btnSalaryBreakUp.Location = new System.Drawing.Point(464, 2);
		this.btnSalaryBreakUp.Name = "btnSalaryBreakUp";
		this.btnSalaryBreakUp.Size = new System.Drawing.Size(141, 28);
		this.btnSalaryBreakUp.TabIndex = 230;
		this.btnSalaryBreakUp.Text = "Salary BreakUp";
		this.btnSalaryBreakUp.UseVisualStyleBackColor = false;
		this.btnSalaryBreakUp.Click += new System.EventHandler(btnSalaryBreakUp_Click);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(3, 8);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(358, 16);
		this.label40.TabIndex = 229;
		this.label40.Text = "Click the Generate Button To Generate Salary BreakUp";
		this.btnGenerateBreakup.BackColor = System.Drawing.Color.Teal;
		this.btnGenerateBreakup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGenerateBreakup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateBreakup.ForeColor = System.Drawing.Color.White;
		this.btnGenerateBreakup.Location = new System.Drawing.Point(368, 2);
		this.btnGenerateBreakup.Name = "btnGenerateBreakup";
		this.btnGenerateBreakup.Size = new System.Drawing.Size(93, 28);
		this.btnGenerateBreakup.TabIndex = 0;
		this.btnGenerateBreakup.Text = "Generate";
		this.btnGenerateBreakup.UseVisualStyleBackColor = false;
		this.btnGenerateBreakup.Click += new System.EventHandler(btnGenerateBreakup_Click);
		this.tabPage7.Controls.Add(this.panel14);
		this.tabPage7.Location = new System.Drawing.Point(4, 22);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage7.Size = new System.Drawing.Size(928, 272);
		this.tabPage7.TabIndex = 2;
		this.tabPage7.Text = "Benefits";
		this.tabPage7.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add(this.panel17);
		this.panel14.Controls.Add(this.panel16);
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(922, 266);
		this.panel14.TabIndex = 0;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(574, 40);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(348, 226);
		this.panel17.TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeAssetsBenefit.FilterMode = (FilterMode)2;
		this.grdEmployeeAssetsBenefit.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeAssetsBenefit.GroupByBoxVisible = false;
		this.grdEmployeeAssetsBenefit.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).Name = "grdEmployeeAssetsBenefit";
		((ScrollableBase)this.grdEmployeeAssetsBenefit).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeAssetsBenefit).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeAssetsBenefit).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).Size = new System.Drawing.Size(348, 226);
		((System.Windows.Forms.Control)(object)this.grdEmployeeAssetsBenefit).TabIndex = 1;
		this.grdEmployeeAssetsBenefit.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeAssetsBenefit.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeAssetsBenefit).VisualStyle = (VisualStyle)4;
		this.grdEmployeeAssetsBenefit.CellValueChanged += new ColumnActionEventHandler(grdEmployeeAssetsBenefit_CellValueChanged);
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(0, 40);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(574, 226);
		this.panel16.TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeDirectBenefits.FilterMode = (FilterMode)2;
		this.grdEmployeeDirectBenefits.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeDirectBenefits.GroupByBoxVisible = false;
		this.grdEmployeeDirectBenefits.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).Name = "grdEmployeeDirectBenefits";
		((ScrollableBase)this.grdEmployeeDirectBenefits).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeDirectBenefits).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeDirectBenefits).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).Size = new System.Drawing.Size(574, 226);
		((System.Windows.Forms.Control)(object)this.grdEmployeeDirectBenefits).TabIndex = 1;
		this.grdEmployeeDirectBenefits.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeDirectBenefits.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeDirectBenefits).VisualStyle = (VisualStyle)4;
		this.grdEmployeeDirectBenefits.CellUpdated += new ColumnActionEventHandler(grdEmployeeDirectBenefits_CellUpdated);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.btnDefineBenefits);
		this.panel15.Controls.Add(this.btnGenerateAllBenefits);
		this.panel15.Controls.Add(this.ChkAllBenefits);
		this.panel15.Controls.Add(this.label62);
		this.panel15.Controls.Add(this.label61);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(922, 40);
		this.panel15.TabIndex = 0;
		this.btnDefineBenefits.BackColor = System.Drawing.Color.RoyalBlue;
		this.btnDefineBenefits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDefineBenefits.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineBenefits.ForeColor = System.Drawing.Color.White;
		this.btnDefineBenefits.Location = new System.Drawing.Point(147, 5);
		this.btnDefineBenefits.Name = "btnDefineBenefits";
		this.btnDefineBenefits.Size = new System.Drawing.Size(129, 28);
		this.btnDefineBenefits.TabIndex = 5;
		this.btnDefineBenefits.Text = "Define Benefits";
		this.btnDefineBenefits.UseVisualStyleBackColor = false;
		this.btnDefineBenefits.Click += new System.EventHandler(btnDefineBenefits_Click);
		this.btnGenerateAllBenefits.BackColor = System.Drawing.Color.RoyalBlue;
		this.btnGenerateAllBenefits.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGenerateAllBenefits.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateAllBenefits.ForeColor = System.Drawing.Color.White;
		this.btnGenerateAllBenefits.Location = new System.Drawing.Point(57, 5);
		this.btnGenerateAllBenefits.Name = "btnGenerateAllBenefits";
		this.btnGenerateAllBenefits.Size = new System.Drawing.Size(88, 28);
		this.btnGenerateAllBenefits.TabIndex = 4;
		this.btnGenerateAllBenefits.Text = "Generate";
		this.btnGenerateAllBenefits.UseVisualStyleBackColor = false;
		this.btnGenerateAllBenefits.Click += new System.EventHandler(btnGenerateAllBenefits_Click);
		this.ChkAllBenefits.AutoSize = true;
		this.ChkAllBenefits.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkAllBenefits.ForeColor = System.Drawing.Color.White;
		this.ChkAllBenefits.Location = new System.Drawing.Point(9, 5);
		this.ChkAllBenefits.Name = "ChkAllBenefits";
		this.ChkAllBenefits.Size = new System.Drawing.Size(44, 20);
		this.ChkAllBenefits.TabIndex = 3;
		this.ChkAllBenefits.Text = "All";
		this.ChkAllBenefits.UseVisualStyleBackColor = true;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.White;
		this.label62.Location = new System.Drawing.Point(780, 12);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(134, 19);
		this.label62.TabIndex = 2;
		this.label62.Text = "Assets Benefits";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.White;
		this.label61.Location = new System.Drawing.Point(286, 14);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(130, 19);
		this.label61.TabIndex = 1;
		this.label61.Text = "Direct Benefits";
		this.tabPage8.Controls.Add(this.panel18);
		this.tabPage8.Location = new System.Drawing.Point(4, 22);
		this.tabPage8.Name = "tabPage8";
		this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage8.Size = new System.Drawing.Size(928, 272);
		this.tabPage8.TabIndex = 3;
		this.tabPage8.Text = "Week Days";
		this.tabPage8.UseVisualStyleBackColor = true;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdWeekDays);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(3, 3);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(922, 266);
		this.panel18.TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdWeekDays).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWeekDays.FilterMode = (FilterMode)2;
		this.grdWeekDays.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWeekDays).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWeekDays.GroupByBoxVisible = false;
		this.grdWeekDays.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdWeekDays).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdWeekDays).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdWeekDays).Name = "grdWeekDays";
		((ScrollableBase)this.grdWeekDays).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdWeekDays).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdWeekDays).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWeekDays).Size = new System.Drawing.Size(922, 266);
		((System.Windows.Forms.Control)(object)this.grdWeekDays).TabIndex = 2;
		this.grdWeekDays.TotalRow = (InheritableBoolean)1;
		this.grdWeekDays.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWeekDays).VisualStyle = (VisualStyle)4;
		this.grdWeekDays.CellValueChanged += new ColumnActionEventHandler(grdWeekDays_CellValueChanged);
		this.tabPage4.Controls.Add(this.panel19);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(942, 304);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Employee Detail";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add(this.tabControl4);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 3);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(936, 298);
		this.panel19.TabIndex = 0;
		this.tabControl4.Controls.Add(this.tabPage9);
		this.tabControl4.Controls.Add(this.tabPage10);
		this.tabControl4.Controls.Add(this.tabPage11);
		this.tabControl4.Controls.Add(this.tabPage12);
		this.tabControl4.Controls.Add(this.tabPage13);
		this.tabControl4.Controls.Add(this.tabPage14);
		this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl4.Location = new System.Drawing.Point(0, 0);
		this.tabControl4.Name = "tabControl4";
		this.tabControl4.SelectedIndex = 0;
		this.tabControl4.Size = new System.Drawing.Size(936, 298);
		this.tabControl4.TabIndex = 0;
		this.tabPage9.Controls.Add(this.panel20);
		this.tabPage9.Location = new System.Drawing.Point(4, 22);
		this.tabPage9.Name = "tabPage9";
		this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage9.Size = new System.Drawing.Size(928, 272);
		this.tabPage9.TabIndex = 0;
		this.tabPage9.Text = "Address Detail";
		this.tabPage9.UseVisualStyleBackColor = true;
		this.panel20.Controls.Add(this.panel22);
		this.panel20.Controls.Add(this.panel21);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(922, 266);
		this.panel20.TabIndex = 0;
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.grdAddressDetail);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(0, 104);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(922, 162);
		this.panel22.TabIndex = 1;
		this.grdAddressDetail.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAddressDetail.FilterMode = (FilterMode)2;
		this.grdAddressDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAddressDetail.GroupByBoxVisible = false;
		this.grdAddressDetail.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).Name = "grdAddressDetail";
		((ScrollableBase)this.grdAddressDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAddressDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdAddressDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).Size = new System.Drawing.Size(922, 162);
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).TabIndex = 3;
		this.grdAddressDetail.TotalRow = (InheritableBoolean)1;
		this.grdAddressDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAddressDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdAddressDetail).DoubleClick += new System.EventHandler(grdAddressDetail_DoubleClick);
		this.panel21.Controls.Add(this.btnCityName);
		this.panel21.Controls.Add(this.btnAddressType);
		this.panel21.Controls.Add(this.btnCancelAddressDetail);
		this.panel21.Controls.Add(this.btnUpdateAddressDetail);
		this.panel21.Controls.Add(this.btnAddAddressDetail);
		this.panel21.Controls.Add(this.label64);
		this.panel21.Controls.Add(this.txtAddressDetail);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProvince);
		this.panel21.Controls.Add(this.label65);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCity);
		this.panel21.Controls.Add(this.label66);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAddressType);
		this.panel21.Controls.Add(this.label67);
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCountry);
		this.panel21.Controls.Add(this.label68);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(922, 104);
		this.panel21.TabIndex = 0;
		this.btnCityName.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnCityName.Location = new System.Drawing.Point(714, 25);
		this.btnCityName.Name = "btnCityName";
		this.btnCityName.Size = new System.Drawing.Size(30, 26);
		this.btnCityName.TabIndex = 48;
		this.btnCityName.TabStop = false;
		this.btnCityName.UseVisualStyleBackColor = true;
		this.btnCityName.Click += new System.EventHandler(btnCityName_Click);
		this.btnAddressType.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddressType.Location = new System.Drawing.Point(178, 25);
		this.btnAddressType.Name = "btnAddressType";
		this.btnAddressType.Size = new System.Drawing.Size(30, 26);
		this.btnAddressType.TabIndex = 47;
		this.btnAddressType.TabStop = false;
		this.btnAddressType.UseVisualStyleBackColor = true;
		this.btnAddressType.Click += new System.EventHandler(btnAddressType_Click);
		this.btnCancelAddressDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelAddressDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelAddressDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelAddressDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelAddressDetail.Location = new System.Drawing.Point(772, 71);
		this.btnCancelAddressDetail.Name = "btnCancelAddressDetail";
		this.btnCancelAddressDetail.Size = new System.Drawing.Size(75, 27);
		this.btnCancelAddressDetail.TabIndex = 6;
		this.btnCancelAddressDetail.Text = "Cancel";
		this.btnCancelAddressDetail.UseVisualStyleBackColor = false;
		this.btnCancelAddressDetail.Visible = false;
		this.btnCancelAddressDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateAddressDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateAddressDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateAddressDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateAddressDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateAddressDetail.Location = new System.Drawing.Point(696, 71);
		this.btnUpdateAddressDetail.Name = "btnUpdateAddressDetail";
		this.btnUpdateAddressDetail.Size = new System.Drawing.Size(75, 27);
		this.btnUpdateAddressDetail.TabIndex = 5;
		this.btnUpdateAddressDetail.Text = "Update";
		this.btnUpdateAddressDetail.UseVisualStyleBackColor = false;
		this.btnUpdateAddressDetail.Visible = false;
		this.btnUpdateAddressDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnAddAddressDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddAddressDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddAddressDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddAddressDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddAddressDetail.Location = new System.Drawing.Point(696, 71);
		this.btnAddAddressDetail.Name = "btnAddAddressDetail";
		this.btnAddAddressDetail.Size = new System.Drawing.Size(75, 27);
		this.btnAddAddressDetail.TabIndex = 46;
		this.btnAddAddressDetail.Text = "Add";
		this.btnAddAddressDetail.UseVisualStyleBackColor = false;
		this.btnAddAddressDetail.Click += new System.EventHandler(btnAdd_Click);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(9, 53);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(83, 15);
		this.label64.TabIndex = 45;
		this.label64.Text = "Address Detail";
		this.txtAddressDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddressDetail.Location = new System.Drawing.Point(12, 73);
		this.txtAddressDetail.Name = "txtAddressDetail";
		this.txtAddressDetail.Size = new System.Drawing.Size(678, 23);
		this.txtAddressDetail.TabIndex = 4;
		this.cmbProvince.AllowNull = (DefaultableBoolean)1;
		this.cmbProvince.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProvince.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProvince.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Appearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbProvince).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProvince).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance206;
		((SpecialBoxBase)((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbProvince).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProvince).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProvince).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbProvince).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProvince).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProvince).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProvince).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProvince.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProvince).Location = new System.Drawing.Point(379, 25);
		((System.Windows.Forms.Control)(object)this.cmbProvince).Name = "cmbProvince";
		((System.Windows.Forms.Control)(object)this.cmbProvince).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbProvince).TabIndex = 2;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(379, 7);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(53, 15);
		this.label65.TabIndex = 44;
		this.label65.Text = "Province";
		this.cmbCity.AllowNull = (DefaultableBoolean)1;
		this.cmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance219;
		((SpecialBoxBase)((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance225).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCity).Location = new System.Drawing.Point(547, 25);
		((System.Windows.Forms.Control)(object)this.cmbCity).Name = "cmbCity";
		((System.Windows.Forms.Control)(object)this.cmbCity).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbCity).TabIndex = 3;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(547, 7);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(62, 15);
		this.label66.TabIndex = 43;
		this.label66.Text = "City Name";
		this.cmbAddressType.AllowNull = (DefaultableBoolean)1;
		this.cmbAddressType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAddressType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAddressType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance233;
		((SpecialBoxBase)((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance238).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance239).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAddressType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAddressType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Location = new System.Drawing.Point(12, 25);
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Name = "cmbAddressType";
		((System.Windows.Forms.Control)(object)this.cmbAddressType).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbAddressType).TabIndex = 0;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(9, 7);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(77, 15);
		this.label67.TabIndex = 42;
		this.label67.Text = "Address Type";
		this.cmbCountry.AllowNull = (DefaultableBoolean)1;
		this.cmbCountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbCountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
		((SpecialBoxBase)((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbCountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance251).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance252).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance254).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCountry).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCountry).Location = new System.Drawing.Point(211, 25);
		((System.Windows.Forms.Control)(object)this.cmbCountry).Name = "cmbCountry";
		((System.Windows.Forms.Control)(object)this.cmbCountry).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbCountry).TabIndex = 1;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(209, 7);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(49, 15);
		this.label68.TabIndex = 41;
		this.label68.Text = "Country";
		this.tabPage10.Controls.Add(this.panel23);
		this.tabPage10.Location = new System.Drawing.Point(4, 22);
		this.tabPage10.Name = "tabPage10";
		this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage10.Size = new System.Drawing.Size(928, 272);
		this.tabPage10.TabIndex = 1;
		this.tabPage10.Text = "Experience";
		this.tabPage10.UseVisualStyleBackColor = true;
		this.panel23.Controls.Add(this.panel25);
		this.panel23.Controls.Add(this.panel24);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(3, 3);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(922, 266);
		this.panel23.TabIndex = 0;
		this.panel25.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeExperience);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel25.Location = new System.Drawing.Point(0, 66);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(922, 200);
		this.panel25.TabIndex = 1;
		this.grdEmployeeExperience.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeExperience.FilterMode = (FilterMode)2;
		this.grdEmployeeExperience.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeExperience.GroupByBoxVisible = false;
		this.grdEmployeeExperience.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).Name = "grdEmployeeExperience";
		((ScrollableBase)this.grdEmployeeExperience).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeExperience).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeExperience).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).Size = new System.Drawing.Size(922, 200);
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).TabIndex = 0;
		this.grdEmployeeExperience.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeExperience.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeExperience).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEmployeeExperience).DoubleClick += new System.EventHandler(grdEmployeeExperience_DoubleClick);
		this.panel24.Controls.Add(this.btnCancelExperience);
		this.panel24.Controls.Add(this.btnUpdateExperience);
		this.panel24.Controls.Add(this.btnAddExperience);
		this.panel24.Controls.Add(this.txtDescription);
		this.panel24.Controls.Add(this.label72);
		this.panel24.Controls.Add(this.dtpTo);
		this.panel24.Controls.Add(this.dtpFrom);
		this.panel24.Controls.Add(this.txtOrganization);
		this.panel24.Controls.Add(this.label73);
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCityEmployeeExperience);
		this.panel24.Controls.Add(this.label74);
		this.panel24.Controls.Add(this.label75);
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeExperience);
		this.panel24.Controls.Add(this.label76);
		this.panel24.Controls.Add(this.label77);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(922, 66);
		this.panel24.TabIndex = 0;
		this.btnCancelExperience.BackColor = System.Drawing.Color.Teal;
		this.btnCancelExperience.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelExperience.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelExperience.ForeColor = System.Drawing.Color.White;
		this.btnCancelExperience.Location = new System.Drawing.Point(845, 32);
		this.btnCancelExperience.Name = "btnCancelExperience";
		this.btnCancelExperience.Size = new System.Drawing.Size(71, 25);
		this.btnCancelExperience.TabIndex = 7;
		this.btnCancelExperience.Text = "Cancel";
		this.btnCancelExperience.UseVisualStyleBackColor = false;
		this.btnCancelExperience.Visible = false;
		this.btnCancelExperience.Click += new System.EventHandler(btnCancelExperience_Click);
		this.btnUpdateExperience.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateExperience.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateExperience.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateExperience.ForeColor = System.Drawing.Color.White;
		this.btnUpdateExperience.Location = new System.Drawing.Point(770, 32);
		this.btnUpdateExperience.Name = "btnUpdateExperience";
		this.btnUpdateExperience.Size = new System.Drawing.Size(75, 25);
		this.btnUpdateExperience.TabIndex = 6;
		this.btnUpdateExperience.Text = "Update";
		this.btnUpdateExperience.UseVisualStyleBackColor = false;
		this.btnUpdateExperience.Visible = false;
		this.btnUpdateExperience.Click += new System.EventHandler(btnUpdateExperience_Click);
		this.btnAddExperience.BackColor = System.Drawing.Color.Teal;
		this.btnAddExperience.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddExperience.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddExperience.ForeColor = System.Drawing.Color.White;
		this.btnAddExperience.Location = new System.Drawing.Point(770, 33);
		this.btnAddExperience.Name = "btnAddExperience";
		this.btnAddExperience.Size = new System.Drawing.Size(75, 25);
		this.btnAddExperience.TabIndex = 118;
		this.btnAddExperience.Text = "Add";
		this.btnAddExperience.UseVisualStyleBackColor = false;
		this.btnAddExperience.Click += new System.EventHandler(btnAddExperience_Click);
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(559, 35);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(207, 23);
		this.txtDescription.TabIndex = 5;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(492, 39);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(68, 15);
		this.label72.TabIndex = 117;
		this.label72.Text = "Description";
		this.dtpTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpTo.Location = new System.Drawing.Point(337, 35);
		this.dtpTo.Name = "dtpTo";
		this.dtpTo.Size = new System.Drawing.Size(154, 23);
		this.dtpTo.TabIndex = 3;
		this.dtpFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpFrom.Location = new System.Drawing.Point(337, 6);
		this.dtpFrom.Name = "dtpFrom";
		this.dtpFrom.Size = new System.Drawing.Size(154, 23);
		this.dtpFrom.TabIndex = 2;
		this.txtOrganization.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrganization.Location = new System.Drawing.Point(88, 6);
		this.txtOrganization.Name = "txtOrganization";
		this.txtOrganization.Size = new System.Drawing.Size(192, 23);
		this.txtOrganization.TabIndex = 0;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(283, 39);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(20, 15);
		this.label73.TabIndex = 116;
		this.label73.Text = "To";
		this.cmbCityEmployeeExperience.AllowNull = (DefaultableBoolean)1;
		this.cmbCityEmployeeExperience.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCityEmployeeExperience.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCityEmployeeExperience.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Appearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance277;
		((SpecialBoxBase)((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance280).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance282).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance283).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance283).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance283).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance285).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCityEmployeeExperience).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCityEmployeeExperience).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCityEmployeeExperience.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCityEmployeeExperience).Location = new System.Drawing.Point(559, 5);
		((System.Windows.Forms.Control)(object)this.cmbCityEmployeeExperience).Name = "cmbCityEmployeeExperience";
		((System.Windows.Forms.Control)(object)this.cmbCityEmployeeExperience).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbCityEmployeeExperience).TabIndex = 4;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(497, 9);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(27, 15);
		this.label74.TabIndex = 115;
		this.label74.Text = "City";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(285, 10);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(35, 15);
		this.label75.TabIndex = 114;
		this.label75.Text = "From";
		this.cmbDesignationEmployeeExperience.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignationEmployeeExperience.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignationEmployeeExperience.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignationEmployeeExperience.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance314).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Appearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance315).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance315).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance315).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance315;
		((AppearanceBase)appearance316).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance316;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance318).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance318).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance318).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance319).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance319;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance320).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance321;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance322).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance323).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance323;
		((AppearanceBase)appearance324).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance325).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance325;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance326).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance326;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignationEmployeeExperience).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeExperience).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignationEmployeeExperience.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeExperience).Location = new System.Drawing.Point(88, 32);
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeExperience).Name = "cmbDesignationEmployeeExperience";
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeExperience).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeExperience).TabIndex = 1;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(9, 35);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(71, 15);
		this.label76.TabIndex = 113;
		this.label76.Text = "Designation";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(10, 12);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(76, 15);
		this.label77.TabIndex = 112;
		this.label77.Text = "Organization";
		this.tabPage11.Controls.Add(this.panel26);
		this.tabPage11.Location = new System.Drawing.Point(4, 22);
		this.tabPage11.Name = "tabPage11";
		this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage11.Size = new System.Drawing.Size(928, 272);
		this.tabPage11.TabIndex = 2;
		this.tabPage11.Text = "Education";
		this.tabPage11.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel28);
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(3, 3);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(922, 266);
		this.panel26.TabIndex = 0;
		this.panel28.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeEducation);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel28.Location = new System.Drawing.Point(0, 94);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(922, 172);
		this.panel28.TabIndex = 1;
		this.grdEmployeeEducation.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeEducation.FilterMode = (FilterMode)2;
		this.grdEmployeeEducation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeEducation.GroupByBoxVisible = false;
		this.grdEmployeeEducation.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).Name = "grdEmployeeEducation";
		((ScrollableBase)this.grdEmployeeEducation).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeEducation).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeEducation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).Size = new System.Drawing.Size(922, 172);
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).TabIndex = 0;
		this.grdEmployeeEducation.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeEducation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeEducation).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEmployeeEducation).DoubleClick += new System.EventHandler(grdEmployeeEducation_DoubleClick);
		this.panel27.Controls.Add(this.btnDefineDegree);
		this.panel27.Controls.Add(this.btnCancelEducation);
		this.panel27.Controls.Add(this.btnUpdateEducation);
		this.panel27.Controls.Add(this.btnAddEducation);
		this.panel27.Controls.Add(this.PassingYearDate);
		this.panel27.Controls.Add(this.StartYearDate);
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDegree);
		this.panel27.Controls.Add(this.txtCGPA);
		this.panel27.Controls.Add(this.label78);
		this.panel27.Controls.Add(this.label79);
		this.panel27.Controls.Add(this.label80);
		this.panel27.Controls.Add(this.txtInstitute);
		this.panel27.Controls.Add(this.label81);
		this.panel27.Controls.Add(this.txtDegreeTitle);
		this.panel27.Controls.Add(this.label83);
		this.panel27.Controls.Add(this.label84);
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCityEducation);
		this.panel27.Controls.Add(this.label85);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(0, 0);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(922, 94);
		this.panel27.TabIndex = 0;
		this.btnDefineDegree.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineDegree.Location = new System.Drawing.Point(307, 5);
		this.btnDefineDegree.Name = "btnDefineDegree";
		this.btnDefineDegree.Size = new System.Drawing.Size(30, 26);
		this.btnDefineDegree.TabIndex = 122;
		this.btnDefineDegree.TabStop = false;
		this.btnDefineDegree.UseVisualStyleBackColor = true;
		this.btnDefineDegree.Click += new System.EventHandler(btnDefineDegree_Click);
		this.btnCancelEducation.BackColor = System.Drawing.Color.Teal;
		this.btnCancelEducation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelEducation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelEducation.ForeColor = System.Drawing.Color.White;
		this.btnCancelEducation.Location = new System.Drawing.Point(636, 63);
		this.btnCancelEducation.Name = "btnCancelEducation";
		this.btnCancelEducation.Size = new System.Drawing.Size(75, 27);
		this.btnCancelEducation.TabIndex = 8;
		this.btnCancelEducation.Text = "Cancel";
		this.btnCancelEducation.UseVisualStyleBackColor = false;
		this.btnCancelEducation.Visible = false;
		this.btnCancelEducation.Click += new System.EventHandler(btnCancelEducation_Click);
		this.btnUpdateEducation.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateEducation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateEducation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateEducation.ForeColor = System.Drawing.Color.White;
		this.btnUpdateEducation.Location = new System.Drawing.Point(561, 63);
		this.btnUpdateEducation.Name = "btnUpdateEducation";
		this.btnUpdateEducation.Size = new System.Drawing.Size(75, 27);
		this.btnUpdateEducation.TabIndex = 7;
		this.btnUpdateEducation.Text = "Update";
		this.btnUpdateEducation.UseVisualStyleBackColor = false;
		this.btnUpdateEducation.Visible = false;
		this.btnUpdateEducation.Click += new System.EventHandler(btnUpdateEducation_Click);
		this.btnAddEducation.BackColor = System.Drawing.Color.Teal;
		this.btnAddEducation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddEducation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddEducation.ForeColor = System.Drawing.Color.White;
		this.btnAddEducation.Location = new System.Drawing.Point(561, 63);
		this.btnAddEducation.Name = "btnAddEducation";
		this.btnAddEducation.Size = new System.Drawing.Size(75, 27);
		this.btnAddEducation.TabIndex = 121;
		this.btnAddEducation.Text = "Add";
		this.btnAddEducation.UseVisualStyleBackColor = false;
		this.btnAddEducation.Click += new System.EventHandler(btnAddEducation_Click);
		this.PassingYearDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PassingYearDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.PassingYearDate.Location = new System.Drawing.Point(424, 35);
		this.PassingYearDate.Name = "PassingYearDate";
		this.PassingYearDate.Size = new System.Drawing.Size(131, 23);
		this.PassingYearDate.TabIndex = 4;
		this.StartYearDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.StartYearDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.StartYearDate.Location = new System.Drawing.Point(424, 7);
		this.StartYearDate.Name = "StartYearDate";
		this.StartYearDate.Size = new System.Drawing.Size(131, 23);
		this.StartYearDate.TabIndex = 3;
		this.cmbDegree.AllowNull = (DefaultableBoolean)1;
		this.cmbDegree.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDegree.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDegree.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Appearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.cmbDegree).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDegree).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance289).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance289).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance289).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDegree).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance290).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDegree).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance290;
		((SpecialBoxBase)((UltraGridBase)this.cmbDegree).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance291).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance291).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDegree).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbDegree).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDegree).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance292).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance293).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance296).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance297).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance297;
		((AppearanceBase)appearance298).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDegree).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.cmbDegree).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDegree).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDegree).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDegree).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDegree.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDegree).Location = new System.Drawing.Point(87, 5);
		((System.Windows.Forms.Control)(object)this.cmbDegree).Name = "cmbDegree";
		((System.Windows.Forms.Control)(object)this.cmbDegree).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbDegree).TabIndex = 0;
		this.txtCGPA.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCGPA.Location = new System.Drawing.Point(631, 35);
		this.txtCGPA.Name = "txtCGPA";
		this.txtCGPA.Size = new System.Drawing.Size(218, 23);
		this.txtCGPA.TabIndex = 6;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(559, 39);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(36, 15);
		this.label78.TabIndex = 6;
		this.label78.Text = "CGPA";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(346, 39);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(72, 15);
		this.label79.TabIndex = 56;
		this.label79.Text = "Passing Year";
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(346, 11);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(57, 15);
		this.label80.TabIndex = 55;
		this.label80.Text = "Start Year";
		this.txtInstitute.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInstitute.Location = new System.Drawing.Point(87, 65);
		this.txtInstitute.Name = "txtInstitute";
		this.txtInstitute.Size = new System.Drawing.Size(468, 23);
		this.txtInstitute.TabIndex = 2;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.Location = new System.Drawing.Point(12, 69);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(51, 15);
		this.label81.TabIndex = 54;
		this.label81.Text = "Institute";
		this.txtDegreeTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDegreeTitle.Location = new System.Drawing.Point(87, 35);
		this.txtDegreeTitle.Name = "txtDegreeTitle";
		this.txtDegreeTitle.Size = new System.Drawing.Size(218, 23);
		this.txtDegreeTitle.TabIndex = 1;
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(12, 39);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(71, 15);
		this.label83.TabIndex = 51;
		this.label83.Text = "Degree Title";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.Location = new System.Drawing.Point(12, 11);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(48, 15);
		this.label84.TabIndex = 52;
		this.label84.Text = "Degree ";
		this.cmbCityEducation.AllowNull = (DefaultableBoolean)1;
		this.cmbCityEducation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCityEducation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCityEducation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCityEducation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance303).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance303;
		((SpecialBoxBase)((UltraGridBase)this.cmbCityEducation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance304).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance308;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance309).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance310).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance311;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance312).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance313;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCityEducation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCityEducation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCityEducation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCityEducation).Location = new System.Drawing.Point(631, 5);
		((System.Windows.Forms.Control)(object)this.cmbCityEducation).Name = "cmbCityEducation";
		((System.Windows.Forms.Control)(object)this.cmbCityEducation).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbCityEducation).TabIndex = 5;
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(559, 11);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(62, 15);
		this.label85.TabIndex = 50;
		this.label85.Text = "City Name";
		this.tabPage12.Controls.Add(this.panel29);
		this.tabPage12.Location = new System.Drawing.Point(4, 22);
		this.tabPage12.Name = "tabPage12";
		this.tabPage12.Size = new System.Drawing.Size(928, 272);
		this.tabPage12.TabIndex = 3;
		this.tabPage12.Text = "Family";
		this.tabPage12.UseVisualStyleBackColor = true;
		this.panel29.Controls.Add(this.panel31);
		this.panel29.Controls.Add(this.panel30);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel29.Location = new System.Drawing.Point(0, 0);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(928, 272);
		this.panel29.TabIndex = 0;
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.grdFamilyInfo);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel31.Location = new System.Drawing.Point(0, 60);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(928, 212);
		this.panel31.TabIndex = 1;
		this.grdFamilyInfo.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFamilyInfo.FilterMode = (FilterMode)2;
		this.grdFamilyInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFamilyInfo.GroupByBoxVisible = false;
		this.grdFamilyInfo.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).Name = "grdFamilyInfo";
		((ScrollableBase)this.grdFamilyInfo).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdFamilyInfo).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdFamilyInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).Size = new System.Drawing.Size(928, 212);
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).TabIndex = 0;
		this.grdFamilyInfo.TotalRow = (InheritableBoolean)1;
		this.grdFamilyInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFamilyInfo).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdFamilyInfo).DoubleClick += new System.EventHandler(grdFamilyInfo_DoubleClick);
		this.panel30.Controls.Add(this.btnCancelFamily);
		this.panel30.Controls.Add(this.btnUpdateFamily);
		this.panel30.Controls.Add(this.btnAddFamily);
		this.panel30.Controls.Add(this.txtDescriptionFamilyInfo);
		this.panel30.Controls.Add(this.label90);
		this.panel30.Controls.Add(this.txtNameFamilyInfo);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRelationShip);
		this.panel30.Controls.Add(this.label91);
		this.panel30.Controls.Add(this.label87);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 0);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(928, 60);
		this.panel30.TabIndex = 0;
		this.btnCancelFamily.BackColor = System.Drawing.Color.Teal;
		this.btnCancelFamily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelFamily.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelFamily.ForeColor = System.Drawing.Color.White;
		this.btnCancelFamily.Location = new System.Drawing.Point(834, 25);
		this.btnCancelFamily.Name = "btnCancelFamily";
		this.btnCancelFamily.Size = new System.Drawing.Size(68, 26);
		this.btnCancelFamily.TabIndex = 4;
		this.btnCancelFamily.Text = "Cancel";
		this.btnCancelFamily.UseVisualStyleBackColor = false;
		this.btnCancelFamily.Visible = false;
		this.btnCancelFamily.Click += new System.EventHandler(btnCancelFamily_Click);
		this.btnUpdateFamily.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateFamily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateFamily.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateFamily.ForeColor = System.Drawing.Color.White;
		this.btnUpdateFamily.Location = new System.Drawing.Point(763, 25);
		this.btnUpdateFamily.Name = "btnUpdateFamily";
		this.btnUpdateFamily.Size = new System.Drawing.Size(71, 26);
		this.btnUpdateFamily.TabIndex = 3;
		this.btnUpdateFamily.Text = "Update";
		this.btnUpdateFamily.UseVisualStyleBackColor = false;
		this.btnUpdateFamily.Visible = false;
		this.btnUpdateFamily.Click += new System.EventHandler(btnUpdateFamily_Click);
		this.btnAddFamily.BackColor = System.Drawing.Color.Teal;
		this.btnAddFamily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddFamily.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddFamily.ForeColor = System.Drawing.Color.White;
		this.btnAddFamily.Location = new System.Drawing.Point(763, 26);
		this.btnAddFamily.Name = "btnAddFamily";
		this.btnAddFamily.Size = new System.Drawing.Size(71, 26);
		this.btnAddFamily.TabIndex = 124;
		this.btnAddFamily.Text = "Add";
		this.btnAddFamily.UseVisualStyleBackColor = false;
		this.btnAddFamily.Click += new System.EventHandler(btnAddFamily_Click);
		this.txtDescriptionFamilyInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescriptionFamilyInfo.Location = new System.Drawing.Point(454, 27);
		this.txtDescriptionFamilyInfo.Name = "txtDescriptionFamilyInfo";
		this.txtDescriptionFamilyInfo.Size = new System.Drawing.Size(303, 23);
		this.txtDescriptionFamilyInfo.TabIndex = 2;
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(451, 7);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(68, 15);
		this.label90.TabIndex = 35;
		this.label90.Text = "Description";
		this.txtNameFamilyInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNameFamilyInfo.Location = new System.Drawing.Point(222, 27);
		this.txtNameFamilyInfo.Name = "txtNameFamilyInfo";
		this.txtNameFamilyInfo.Size = new System.Drawing.Size(228, 23);
		this.txtNameFamilyInfo.TabIndex = 1;
		this.cmbRelationShip.AllowNull = (DefaultableBoolean)1;
		this.cmbRelationShip.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRelationShip.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRelationShip.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRelationShip).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance259;
		((SpecialBoxBase)((UltraGridBase)this.cmbRelationShip).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRelationShip).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRelationShip).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRelationShip.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRelationShip).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.cmbRelationShip).Name = "cmbRelationShip";
		((System.Windows.Forms.Control)(object)this.cmbRelationShip).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbRelationShip).TabIndex = 0;
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(5, 7);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(72, 15);
		this.label91.TabIndex = 34;
		this.label91.Text = "Relationship";
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(239, 7);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(39, 15);
		this.label87.TabIndex = 33;
		this.label87.Text = "Name";
		this.tabPage13.Controls.Add(this.panel32);
		this.tabPage13.Location = new System.Drawing.Point(4, 22);
		this.tabPage13.Name = "tabPage13";
		this.tabPage13.Size = new System.Drawing.Size(928, 272);
		this.tabPage13.TabIndex = 4;
		this.tabPage13.Text = "Reference";
		this.tabPage13.UseVisualStyleBackColor = true;
		this.panel32.Controls.Add(this.panel34);
		this.panel32.Controls.Add(this.panel33);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(928, 272);
		this.panel32.TabIndex = 0;
		this.panel34.Controls.Add((System.Windows.Forms.Control)(object)this.grdReference);
		this.panel34.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel34.Location = new System.Drawing.Point(0, 97);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(928, 175);
		this.panel34.TabIndex = 1;
		this.grdReference.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdReference).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdReference.FilterMode = (FilterMode)2;
		this.grdReference.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdReference).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdReference.GroupByBoxVisible = false;
		this.grdReference.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdReference).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdReference).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdReference).Name = "grdReference";
		((ScrollableBase)this.grdReference).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdReference).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdReference).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdReference).Size = new System.Drawing.Size(928, 175);
		((System.Windows.Forms.Control)(object)this.grdReference).TabIndex = 0;
		this.grdReference.TotalRow = (InheritableBoolean)1;
		this.grdReference.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdReference).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdReference).DoubleClick += new System.EventHandler(grdReference_DoubleClick);
		this.panel33.Controls.Add(this.btnCancelReference);
		this.panel33.Controls.Add(this.btnUpdateReference);
		this.panel33.Controls.Add(this.btnAddReference);
		this.panel33.Controls.Add(this.txtDescriptionEmployeeRefrence);
		this.panel33.Controls.Add(this.label92);
		this.panel33.Controls.Add(this.txtEmailEmployeeRefrence);
		this.panel33.Controls.Add(this.label93);
		this.panel33.Controls.Add(this.txtMobileEmployeeRefrence);
		this.panel33.Controls.Add(this.label94);
		this.panel33.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeRefrence);
		this.panel33.Controls.Add(this.txtOrganizationEmployeeRefrence);
		this.panel33.Controls.Add(this.label95);
		this.panel33.Controls.Add(this.label98);
		this.panel33.Controls.Add(this.txtRefrenceName);
		this.panel33.Controls.Add(this.label100);
		this.panel33.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel33.Location = new System.Drawing.Point(0, 0);
		this.panel33.Name = "panel33";
		this.panel33.Size = new System.Drawing.Size(928, 97);
		this.panel33.TabIndex = 0;
		this.btnCancelReference.BackColor = System.Drawing.Color.Teal;
		this.btnCancelReference.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelReference.ForeColor = System.Drawing.Color.White;
		this.btnCancelReference.Location = new System.Drawing.Point(821, 60);
		this.btnCancelReference.Name = "btnCancelReference";
		this.btnCancelReference.Size = new System.Drawing.Size(75, 26);
		this.btnCancelReference.TabIndex = 7;
		this.btnCancelReference.Text = "Cancel";
		this.btnCancelReference.UseVisualStyleBackColor = false;
		this.btnCancelReference.Visible = false;
		this.btnCancelReference.Click += new System.EventHandler(btnCancelReference_Click);
		this.btnUpdateReference.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateReference.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateReference.ForeColor = System.Drawing.Color.White;
		this.btnUpdateReference.Location = new System.Drawing.Point(746, 60);
		this.btnUpdateReference.Name = "btnUpdateReference";
		this.btnUpdateReference.Size = new System.Drawing.Size(75, 26);
		this.btnUpdateReference.TabIndex = 6;
		this.btnUpdateReference.Text = "Update";
		this.btnUpdateReference.UseVisualStyleBackColor = false;
		this.btnUpdateReference.Visible = false;
		this.btnUpdateReference.Click += new System.EventHandler(btnUpdateReference_Click);
		this.btnAddReference.BackColor = System.Drawing.Color.Teal;
		this.btnAddReference.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddReference.ForeColor = System.Drawing.Color.White;
		this.btnAddReference.Location = new System.Drawing.Point(746, 61);
		this.btnAddReference.Name = "btnAddReference";
		this.btnAddReference.Size = new System.Drawing.Size(75, 26);
		this.btnAddReference.TabIndex = 127;
		this.btnAddReference.Text = "Add";
		this.btnAddReference.UseVisualStyleBackColor = false;
		this.btnAddReference.Click += new System.EventHandler(btnAddReference_Click);
		this.txtDescriptionEmployeeRefrence.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescriptionEmployeeRefrence.Location = new System.Drawing.Point(446, 66);
		this.txtDescriptionEmployeeRefrence.Name = "txtDescriptionEmployeeRefrence";
		this.txtDescriptionEmployeeRefrence.Size = new System.Drawing.Size(291, 23);
		this.txtDescriptionEmployeeRefrence.TabIndex = 5;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.Location = new System.Drawing.Point(368, 71);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(68, 15);
		this.label92.TabIndex = 121;
		this.label92.Text = "Description";
		this.txtEmailEmployeeRefrence.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmailEmployeeRefrence.Location = new System.Drawing.Point(446, 9);
		this.txtEmailEmployeeRefrence.Name = "txtEmailEmployeeRefrence";
		this.txtEmailEmployeeRefrence.Size = new System.Drawing.Size(291, 23);
		this.txtEmailEmployeeRefrence.TabIndex = 3;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label93.Location = new System.Drawing.Point(368, 13);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(36, 15);
		this.label93.TabIndex = 120;
		this.label93.Text = "Email";
		this.txtMobileEmployeeRefrence.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMobileEmployeeRefrence.Location = new System.Drawing.Point(446, 37);
		this.txtMobileEmployeeRefrence.Name = "txtMobileEmployeeRefrence";
		this.txtMobileEmployeeRefrence.Size = new System.Drawing.Size(291, 23);
		this.txtMobileEmployeeRefrence.TabIndex = 4;
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.Location = new System.Drawing.Point(368, 41);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(44, 15);
		this.label94.TabIndex = 119;
		this.label94.Text = "Mobile";
		this.cmbDesignationEmployeeRefrence.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignationEmployeeRefrence.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignationEmployeeRefrence.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignationEmployeeRefrence.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Appearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance264).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance264).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance270;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance284;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance295).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance317).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance327).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance328;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignationEmployeeRefrence).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeRefrence).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignationEmployeeRefrence.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeRefrence).Location = new System.Drawing.Point(120, 64);
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeRefrence).Name = "cmbDesignationEmployeeRefrence";
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeRefrence).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbDesignationEmployeeRefrence).TabIndex = 2;
		this.txtOrganizationEmployeeRefrence.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrganizationEmployeeRefrence.Location = new System.Drawing.Point(120, 37);
		this.txtOrganizationEmployeeRefrence.Name = "txtOrganizationEmployeeRefrence";
		this.txtOrganizationEmployeeRefrence.Size = new System.Drawing.Size(243, 23);
		this.txtOrganizationEmployeeRefrence.TabIndex = 1;
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.Location = new System.Drawing.Point(9, 41);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(111, 15);
		this.label95.TabIndex = 118;
		this.label95.Text = "Organization Name";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.Location = new System.Drawing.Point(9, 68);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(71, 15);
		this.label98.TabIndex = 117;
		this.label98.Text = "Designation";
		this.txtRefrenceName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefrenceName.Location = new System.Drawing.Point(120, 7);
		this.txtRefrenceName.Name = "txtRefrenceName";
		this.txtRefrenceName.Size = new System.Drawing.Size(243, 23);
		this.txtRefrenceName.TabIndex = 0;
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.Location = new System.Drawing.Point(9, 9);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(88, 15);
		this.label100.TabIndex = 116;
		this.label100.Text = "Refrence Name";
		this.tabPage14.Controls.Add(this.panel35);
		this.tabPage14.Location = new System.Drawing.Point(4, 22);
		this.tabPage14.Name = "tabPage14";
		this.tabPage14.Size = new System.Drawing.Size(928, 272);
		this.tabPage14.TabIndex = 5;
		this.tabPage14.Text = "Bank Account";
		this.tabPage14.UseVisualStyleBackColor = true;
		this.panel35.Controls.Add(this.panel37);
		this.panel35.Controls.Add(this.panel36);
		this.panel35.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel35.Location = new System.Drawing.Point(0, 0);
		this.panel35.Name = "panel35";
		this.panel35.Size = new System.Drawing.Size(928, 272);
		this.panel35.TabIndex = 0;
		this.panel37.Controls.Add((System.Windows.Forms.Control)(object)this.grdBankAccount);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel37.Location = new System.Drawing.Point(0, 80);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(928, 192);
		this.panel37.TabIndex = 1;
		this.grdBankAccount.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdBankAccount).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdBankAccount.FilterMode = (FilterMode)2;
		this.grdBankAccount.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdBankAccount).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBankAccount.GroupByBoxVisible = false;
		this.grdBankAccount.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdBankAccount).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdBankAccount).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdBankAccount).Name = "grdBankAccount";
		((ScrollableBase)this.grdBankAccount).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdBankAccount).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdBankAccount).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBankAccount).Size = new System.Drawing.Size(928, 192);
		((System.Windows.Forms.Control)(object)this.grdBankAccount).TabIndex = 0;
		this.grdBankAccount.TotalRow = (InheritableBoolean)1;
		this.grdBankAccount.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBankAccount).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdBankAccount).DoubleClick += new System.EventHandler(grdBankAccount_DoubleClick);
		this.panel36.Controls.Add(this.btnCancelBankAccount);
		this.panel36.Controls.Add(this.btnUpdateBankAccount);
		this.panel36.Controls.Add(this.btnAddBankAccount);
		this.panel36.Controls.Add(this.IsForPayrollBankAccount);
		this.panel36.Controls.Add(this.txtAccountNo);
		this.panel36.Controls.Add(this.label101);
		this.panel36.Controls.Add(this.txtAccountTitle);
		this.panel36.Controls.Add(this.label102);
		this.panel36.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBankAccount);
		this.panel36.Controls.Add(this.label104);
		this.panel36.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel36.Location = new System.Drawing.Point(0, 0);
		this.panel36.Name = "panel36";
		this.panel36.Size = new System.Drawing.Size(928, 80);
		this.panel36.TabIndex = 0;
		this.btnCancelBankAccount.BackColor = System.Drawing.Color.Teal;
		this.btnCancelBankAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelBankAccount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelBankAccount.ForeColor = System.Drawing.Color.White;
		this.btnCancelBankAccount.Location = new System.Drawing.Point(726, 32);
		this.btnCancelBankAccount.Name = "btnCancelBankAccount";
		this.btnCancelBankAccount.Size = new System.Drawing.Size(68, 28);
		this.btnCancelBankAccount.TabIndex = 5;
		this.btnCancelBankAccount.Text = "Cancel";
		this.btnCancelBankAccount.UseVisualStyleBackColor = false;
		this.btnCancelBankAccount.Visible = false;
		this.btnCancelBankAccount.Click += new System.EventHandler(btnCancelBankAccount_Click);
		this.btnUpdateBankAccount.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateBankAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateBankAccount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateBankAccount.ForeColor = System.Drawing.Color.White;
		this.btnUpdateBankAccount.Location = new System.Drawing.Point(654, 32);
		this.btnUpdateBankAccount.Name = "btnUpdateBankAccount";
		this.btnUpdateBankAccount.Size = new System.Drawing.Size(71, 28);
		this.btnUpdateBankAccount.TabIndex = 4;
		this.btnUpdateBankAccount.Text = "Update";
		this.btnUpdateBankAccount.UseVisualStyleBackColor = false;
		this.btnUpdateBankAccount.Visible = false;
		this.btnUpdateBankAccount.Click += new System.EventHandler(btnUpdateBankAccount_Click);
		this.btnAddBankAccount.BackColor = System.Drawing.Color.Teal;
		this.btnAddBankAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddBankAccount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddBankAccount.ForeColor = System.Drawing.Color.White;
		this.btnAddBankAccount.Location = new System.Drawing.Point(654, 32);
		this.btnAddBankAccount.Name = "btnAddBankAccount";
		this.btnAddBankAccount.Size = new System.Drawing.Size(71, 28);
		this.btnAddBankAccount.TabIndex = 130;
		this.btnAddBankAccount.Text = "Add";
		this.btnAddBankAccount.UseVisualStyleBackColor = false;
		this.btnAddBankAccount.Click += new System.EventHandler(btnAddBankAccount_Click);
		this.IsForPayrollBankAccount.AutoSize = true;
		this.IsForPayrollBankAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsForPayrollBankAccount.Location = new System.Drawing.Point(569, 36);
		this.IsForPayrollBankAccount.Name = "IsForPayrollBankAccount";
		this.IsForPayrollBankAccount.Size = new System.Drawing.Size(74, 19);
		this.IsForPayrollBankAccount.TabIndex = 3;
		this.IsForPayrollBankAccount.Text = "Is Payroll";
		this.IsForPayrollBankAccount.UseVisualStyleBackColor = true;
		this.txtAccountNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountNo.Location = new System.Drawing.Point(438, 34);
		this.txtAccountNo.Name = "txtAccountNo";
		this.txtAccountNo.Size = new System.Drawing.Size(125, 23);
		this.txtAccountNo.TabIndex = 2;
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.Location = new System.Drawing.Point(435, 16);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(71, 15);
		this.label101.TabIndex = 111;
		this.label101.Text = "Account No";
		this.txtAccountTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountTitle.Location = new System.Drawing.Point(242, 34);
		this.txtAccountTitle.Name = "txtAccountTitle";
		this.txtAccountTitle.Size = new System.Drawing.Size(192, 23);
		this.txtAccountTitle.TabIndex = 1;
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(243, 16);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(78, 15);
		this.label102.TabIndex = 110;
		this.label102.Text = "Account Title";
		this.cmbBankAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbBankAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBankAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBankAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBankAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.cmbBankAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBankAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBankAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBankAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBankAccount).Location = new System.Drawing.Point(8, 32);
		((System.Windows.Forms.Control)(object)this.cmbBankAccount).Name = "cmbBankAccount";
		((System.Windows.Forms.Control)(object)this.cmbBankAccount).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbBankAccount).TabIndex = 0;
		this.label104.AutoSize = true;
		this.label104.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label104.Location = new System.Drawing.Point(7, 16);
		this.label104.Name = "label104";
		this.label104.Size = new System.Drawing.Size(68, 15);
		this.label104.TabIndex = 109;
		this.label104.Text = "Bank Name";
		this.panel4.Controls.Add(this.txtMobile1);
		this.panel4.Controls.Add(this.btnBloodGroup);
		this.panel4.Controls.Add(this.btnNationality);
		this.panel4.Controls.Add(this.btnGender);
		this.panel4.Controls.Add(this.btnReleaseTitle);
		this.panel4.Controls.Add(this.btnTitle);
		this.panel4.Controls.Add(this.btnEmployeeCategory);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add(this.label32);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel4.Controls.Add(this.label41);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add(this.label34);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.label29);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.pictureBoxEmployeeSignature);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGender);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRelationTitle);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTitle);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBloodGroup);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeCatagory);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNationality);
		this.panel4.Controls.Add(this.btnEmployeeSignature);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.btnEmployeePicture);
		this.panel4.Controls.Add(this.pictureBoxEmployeePhoto);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.dtpCNICExpiryDate);
		this.panel4.Controls.Add(this.txtMobile2);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtRelationMobileNo);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtLastName);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.txtCNIC);
		this.panel4.Controls.Add(this.txtLicenseNo);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtPassportNo);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.dtpCNICIssueDate);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtEmail);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.dtpDOB);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtRelationName);
		this.panel4.Controls.Add(this.txtMiddleName);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtFirstName);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtEmployeeNoForUpdate);
		this.panel4.Controls.Add(this.txtEmployeeNo);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 323);
		this.panel4.TabIndex = 0;
		this.txtMobile1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMobile1.Location = new System.Drawing.Point(131, 264);
		this.txtMobile1.Mask = "0000 0000000";
		this.txtMobile1.Name = "txtMobile1";
		this.txtMobile1.Size = new System.Drawing.Size(243, 23);
		this.txtMobile1.TabIndex = 13;
		this.btnBloodGroup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBloodGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnBloodGroup.Location = new System.Drawing.Point(730, 208);
		this.btnBloodGroup.Name = "btnBloodGroup";
		this.btnBloodGroup.Size = new System.Drawing.Size(30, 26);
		this.btnBloodGroup.TabIndex = 25;
		this.btnBloodGroup.TabStop = false;
		this.btnBloodGroup.UseVisualStyleBackColor = true;
		this.btnBloodGroup.Click += new System.EventHandler(btnBloodGroup_Click);
		this.btnNationality.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNationality.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNationality.Location = new System.Drawing.Point(730, 93);
		this.btnNationality.Name = "btnNationality";
		this.btnNationality.Size = new System.Drawing.Size(30, 26);
		this.btnNationality.TabIndex = 20;
		this.btnNationality.TabStop = false;
		this.btnNationality.UseVisualStyleBackColor = true;
		this.btnNationality.Click += new System.EventHandler(btnNationality_Click);
		this.btnGender.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnGender.Location = new System.Drawing.Point(344, 178);
		this.btnGender.Name = "btnGender";
		this.btnGender.Size = new System.Drawing.Size(30, 26);
		this.btnGender.TabIndex = 10;
		this.btnGender.TabStop = false;
		this.btnGender.UseVisualStyleBackColor = true;
		this.btnGender.Click += new System.EventHandler(btnGender_Click);
		this.btnReleaseTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReleaseTitle.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnReleaseTitle.Location = new System.Drawing.Point(730, 7);
		this.btnReleaseTitle.Name = "btnReleaseTitle";
		this.btnReleaseTitle.Size = new System.Drawing.Size(30, 26);
		this.btnReleaseTitle.TabIndex = 16;
		this.btnReleaseTitle.TabStop = false;
		this.btnReleaseTitle.UseVisualStyleBackColor = true;
		this.btnReleaseTitle.Click += new System.EventHandler(btnReleaseTitle_Click);
		this.btnTitle.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnTitle.Location = new System.Drawing.Point(344, 35);
		this.btnTitle.Name = "btnTitle";
		this.btnTitle.Size = new System.Drawing.Size(30, 26);
		this.btnTitle.TabIndex = 3;
		this.btnTitle.TabStop = false;
		this.btnTitle.UseVisualStyleBackColor = true;
		this.btnTitle.Click += new System.EventHandler(btnTitle_Click);
		this.btnEmployeeCategory.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnEmployeeCategory.Location = new System.Drawing.Point(344, 64);
		this.btnEmployeeCategory.Name = "btnEmployeeCategory";
		this.btnEmployeeCategory.Size = new System.Drawing.Size(30, 26);
		this.btnEmployeeCategory.TabIndex = 5;
		this.btnEmployeeCategory.TabStop = false;
		this.btnEmployeeCategory.UseVisualStyleBackColor = true;
		this.btnEmployeeCategory.Click += new System.EventHandler(btnEmployeeCategory_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Red;
		this.label36.Location = new System.Drawing.Point(978, 392);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(13, 15);
		this.label36.TabIndex = 249;
		this.label36.Text = "*";
		this.label36.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(606, 393);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(13, 15);
		this.label32.TabIndex = 248;
		this.label32.Text = "*";
		this.label32.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1003, 386);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(898, 392);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(79, 15);
		this.label41.TabIndex = 247;
		this.label41.Text = "Project Name";
		this.label41.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(642, 387);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(520, 391);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(79, 15);
		this.label42.TabIndex = 246;
		this.label42.Text = "Branch Name";
		this.label42.Visible = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(490, 99);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(13, 15);
		this.label34.TabIndex = 245;
		this.label34.Text = "*";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(115, 268);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(13, 15);
		this.label37.TabIndex = 197;
		this.label37.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(490, 127);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(13, 15);
		this.label35.TabIndex = 243;
		this.label35.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(115, 184);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(13, 15);
		this.label33.TabIndex = 244;
		this.label33.Text = "*";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(115, 155);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(13, 15);
		this.label31.TabIndex = 242;
		this.label31.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(115, 99);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(13, 15);
		this.label30.TabIndex = 241;
		this.label30.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(115, 41);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(13, 15);
		this.label29.TabIndex = 240;
		this.label29.Text = "*";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(115, 70);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(13, 15);
		this.label28.TabIndex = 239;
		this.label28.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(115, 13);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(13, 15);
		this.label25.TabIndex = 237;
		this.label25.Text = "*";
		this.pictureBoxEmployeeSignature.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBoxEmployeeSignature.Location = new System.Drawing.Point(774, 190);
		this.pictureBoxEmployeeSignature.Name = "pictureBoxEmployeeSignature";
		this.pictureBoxEmployeeSignature.Size = new System.Drawing.Size(169, 81);
		this.pictureBoxEmployeeSignature.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBoxEmployeeSignature.TabIndex = 236;
		this.pictureBoxEmployeeSignature.TabStop = false;
		this.cmbGender.AllowNull = (DefaultableBoolean)1;
		this.cmbGender.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGender.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGender.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGender).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbGender).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGender).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGender).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGender).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbGender).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGender).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbGender).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGender).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGender).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbGender).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGender).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGender).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGender).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGender.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGender).Location = new System.Drawing.Point(131, 178);
		((System.Windows.Forms.Control)(object)this.cmbGender).Name = "cmbGender";
		((System.Windows.Forms.Control)(object)this.cmbGender).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbGender).TabIndex = 9;
		this.cmbRelationTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbRelationTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRelationTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRelationTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRelationTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbRelationTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRelationTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRelationTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRelationTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRelationTitle).Location = new System.Drawing.Point(506, 7);
		((System.Windows.Forms.Control)(object)this.cmbRelationTitle).Name = "cmbRelationTitle";
		((System.Windows.Forms.Control)(object)this.cmbRelationTitle).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.cmbRelationTitle).TabIndex = 15;
		this.cmbTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTitle).Location = new System.Drawing.Point(131, 35);
		((System.Windows.Forms.Control)(object)this.cmbTitle).Name = "cmbTitle";
		((System.Windows.Forms.Control)(object)this.cmbTitle).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbTitle).TabIndex = 2;
		this.cmbBloodGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbBloodGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBloodGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBloodGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBloodGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.cmbBloodGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBloodGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBloodGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBloodGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBloodGroup).Location = new System.Drawing.Point(506, 208);
		((System.Windows.Forms.Control)(object)this.cmbBloodGroup).Name = "cmbBloodGroup";
		((System.Windows.Forms.Control)(object)this.cmbBloodGroup).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.cmbBloodGroup).TabIndex = 24;
		this.cmbEmployeeCatagory.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeCatagory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeCatagory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeCatagory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeCatagory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCatagory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeCatagory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCatagory).Location = new System.Drawing.Point(131, 64);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCatagory).Name = "cmbEmployeeCatagory";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCatagory).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeCatagory).TabIndex = 4;
		this.cmbNationality.AllowNull = (DefaultableBoolean)1;
		this.cmbNationality.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNationality.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNationality.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbNationality).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNationality).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNationality).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNationality).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.cmbNationality).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNationality).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbNationality).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNationality).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNationality).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbNationality).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNationality).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNationality).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNationality).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbNationality.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNationality).Location = new System.Drawing.Point(506, 93);
		((System.Windows.Forms.Control)(object)this.cmbNationality).Name = "cmbNationality";
		((System.Windows.Forms.Control)(object)this.cmbNationality).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.cmbNationality).TabIndex = 19;
		this.btnEmployeeSignature.BackColor = System.Drawing.Color.Teal;
		this.btnEmployeeSignature.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnEmployeeSignature.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnEmployeeSignature.ForeColor = System.Drawing.Color.White;
		this.btnEmployeeSignature.Location = new System.Drawing.Point(808, 273);
		this.btnEmployeeSignature.Name = "btnEmployeeSignature";
		this.btnEmployeeSignature.Size = new System.Drawing.Size(105, 26);
		this.btnEmployeeSignature.TabIndex = 29;
		this.btnEmployeeSignature.Text = "Choose File";
		this.btnEmployeeSignature.UseVisualStyleBackColor = false;
		this.btnEmployeeSignature.Click += new System.EventHandler(btnEmployeeSignature_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(774, 168);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(148, 20);
		this.label27.TabIndex = 235;
		this.label27.Text = "Employee Signature";
		this.btnEmployeePicture.BackColor = System.Drawing.Color.Teal;
		this.btnEmployeePicture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnEmployeePicture.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnEmployeePicture.ForeColor = System.Drawing.Color.White;
		this.btnEmployeePicture.Location = new System.Drawing.Point(808, 113);
		this.btnEmployeePicture.Name = "btnEmployeePicture";
		this.btnEmployeePicture.Size = new System.Drawing.Size(105, 26);
		this.btnEmployeePicture.TabIndex = 28;
		this.btnEmployeePicture.Text = "Choose File";
		this.btnEmployeePicture.UseVisualStyleBackColor = false;
		this.btnEmployeePicture.Click += new System.EventHandler(btnEmployeePicture_Click);
		this.pictureBoxEmployeePhoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBoxEmployeePhoto.Location = new System.Drawing.Point(774, 30);
		this.pictureBoxEmployeePhoto.Name = "pictureBoxEmployeePhoto";
		this.pictureBoxEmployeePhoto.Size = new System.Drawing.Size(169, 81);
		this.pictureBoxEmployeePhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBoxEmployeePhoto.TabIndex = 234;
		this.pictureBoxEmployeePhoto.TabStop = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(774, 7);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(130, 20);
		this.label24.TabIndex = 233;
		this.label24.Text = "Employee Picture";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(382, 214);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(74, 15);
		this.label23.TabIndex = 207;
		this.label23.Text = "Blood Group";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(382, 268);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(97, 15);
		this.label22.TabIndex = 202;
		this.label22.Text = "CNIC Expiry Date";
		this.dtpCNICExpiryDate.CustomFormat = "dd-MMM-yyyy";
		this.dtpCNICExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpCNICExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpCNICExpiryDate.Location = new System.Drawing.Point(506, 264);
		this.dtpCNICExpiryDate.Name = "dtpCNICExpiryDate";
		this.dtpCNICExpiryDate.Size = new System.Drawing.Size(254, 23);
		this.dtpCNICExpiryDate.TabIndex = 27;
		this.txtMobile2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMobile2.Location = new System.Drawing.Point(131, 291);
		this.txtMobile2.Name = "txtMobile2";
		this.txtMobile2.Size = new System.Drawing.Size(243, 23);
		this.txtMobile2.TabIndex = 14;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(10, 295);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(51, 15);
		this.label21.TabIndex = 232;
		this.label21.Text = "Mobile2";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(382, 99);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(65, 15);
		this.label20.TabIndex = 193;
		this.label20.Text = "Nationality";
		this.txtRelationMobileNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRelationMobileNo.Location = new System.Drawing.Point(506, 66);
		this.txtRelationMobileNo.Name = "txtRelationMobileNo";
		this.txtRelationMobileNo.Size = new System.Drawing.Size(254, 23);
		this.txtRelationMobileNo.TabIndex = 18;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(382, 70);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(109, 15);
		this.label19.TabIndex = 188;
		this.label19.Text = "Relation Mobile No";
		this.txtLastName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastName.Location = new System.Drawing.Point(131, 151);
		this.txtLastName.Name = "txtLastName";
		this.txtLastName.Size = new System.Drawing.Size(243, 23);
		this.txtLastName.TabIndex = 8;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(10, 155);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(63, 15);
		this.label18.TabIndex = 183;
		this.label18.Text = "Last Name";
		this.txtCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNIC.Location = new System.Drawing.Point(506, 123);
		this.txtCNIC.Mask = "00000-0000000-0";
		this.txtCNIC.Name = "txtCNIC";
		this.txtCNIC.Size = new System.Drawing.Size(254, 23);
		this.txtCNIC.TabIndex = 21;
		this.txtLicenseNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLicenseNo.Location = new System.Drawing.Point(506, 180);
		this.txtLicenseNo.Name = "txtLicenseNo";
		this.txtLicenseNo.Size = new System.Drawing.Size(254, 23);
		this.txtLicenseNo.TabIndex = 23;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(382, 184);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(65, 15);
		this.label15.TabIndex = 205;
		this.label15.Text = "License No";
		this.txtPassportNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPassportNo.Location = new System.Drawing.Point(506, 151);
		this.txtPassportNo.Name = "txtPassportNo";
		this.txtPassportNo.Size = new System.Drawing.Size(254, 23);
		this.txtPassportNo.TabIndex = 22;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(382, 155);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(71, 15);
		this.label16.TabIndex = 230;
		this.label16.Text = "Passport No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(382, 127);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(34, 15);
		this.label13.TabIndex = 229;
		this.label13.Text = "CNIC";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(382, 241);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(92, 15);
		this.label14.TabIndex = 228;
		this.label14.Text = "CNIC Issue Date";
		this.dtpCNICIssueDate.CustomFormat = "dd-MMM-yyyy";
		this.dtpCNICIssueDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpCNICIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpCNICIssueDate.Location = new System.Drawing.Point(506, 237);
		this.dtpCNICIssueDate.Name = "dtpCNICIssueDate";
		this.dtpCNICIssueDate.Size = new System.Drawing.Size(254, 23);
		this.dtpCNICIssueDate.TabIndex = 26;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(10, 268);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(49, 15);
		this.label11.TabIndex = 227;
		this.label11.Text = "Mobile1";
		this.txtEmail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmail.Location = new System.Drawing.Point(131, 237);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(243, 23);
		this.txtEmail.TabIndex = 12;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(10, 241);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(36, 15);
		this.label12.TabIndex = 226;
		this.label12.Text = "Email";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(10, 214);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 225;
		this.label9.Text = "DOB";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(10, 184);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(45, 15);
		this.label10.TabIndex = 224;
		this.label10.Text = "Gender";
		this.dtpDOB.CustomFormat = "dd-MMM-yyyy";
		this.dtpDOB.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpDOB.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpDOB.Location = new System.Drawing.Point(131, 210);
		this.dtpDOB.Name = "dtpDOB";
		this.dtpDOB.Size = new System.Drawing.Size(243, 23);
		this.dtpDOB.TabIndex = 11;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(382, 41);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(85, 15);
		this.label7.TabIndex = 223;
		this.label7.Text = "Relation Name";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(382, 13);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(76, 15);
		this.label8.TabIndex = 222;
		this.label8.Text = "Relation Title";
		this.txtRelationName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRelationName.Location = new System.Drawing.Point(506, 37);
		this.txtRelationName.Name = "txtRelationName";
		this.txtRelationName.Size = new System.Drawing.Size(254, 23);
		this.txtRelationName.TabIndex = 17;
		this.txtMiddleName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMiddleName.Location = new System.Drawing.Point(131, 123);
		this.txtMiddleName.Name = "txtMiddleName";
		this.txtMiddleName.Size = new System.Drawing.Size(243, 23);
		this.txtMiddleName.TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 127);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 221;
		this.label5.Text = "Middle Name";
		this.txtFirstName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstName.Location = new System.Drawing.Point(131, 95);
		this.txtFirstName.Name = "txtFirstName";
		this.txtFirstName.Size = new System.Drawing.Size(243, 23);
		this.txtFirstName.TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(10, 99);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(64, 15);
		this.label6.TabIndex = 220;
		this.label6.Text = "First Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(30, 15);
		this.label4.TabIndex = 219;
		this.label4.Text = "Title";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 70);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(109, 15);
		this.label3.TabIndex = 218;
		this.label3.Text = "Employee Catagory";
		this.txtEmployeeNoForUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmployeeNoForUpdate.Location = new System.Drawing.Point(247, 9);
		this.txtEmployeeNoForUpdate.Name = "txtEmployeeNoForUpdate";
		this.txtEmployeeNoForUpdate.Size = new System.Drawing.Size(127, 23);
		this.txtEmployeeNoForUpdate.TabIndex = 1;
		this.txtEmployeeNoForUpdate.TabStop = false;
		this.txtEmployeeNoForUpdate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmployeeNoForUpdate_KeyPress);
		this.txtEmployeeNoForUpdate.Leave += new System.EventHandler(txtEmployeeNoForUpdate_Leave);
		this.txtEmployeeNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmployeeNo.Location = new System.Drawing.Point(131, 9);
		this.txtEmployeeNo.Name = "txtEmployeeNo";
		this.txtEmployeeNo.ReadOnly = true;
		this.txtEmployeeNo.Size = new System.Drawing.Size(110, 23);
		this.txtEmployeeNo.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 13);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 216;
		this.label1.Text = "Employee No";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label39);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 57);
		this.panel3.TabIndex = 0;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(3, 31);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(212, 22);
		this.label39.TabIndex = 234;
		this.label39.Text = "Employee Registration";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnupdate, this.btnRefresh, this.btnsave, this.btnPrint, this.toolStripSeparator1, this.BtnShifttiming, this.toolStripSeparator2, this.btnDutyRoasterForm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 6;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(75, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(59, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(116, 24);
		this.btnPrint.Text = "1112-&Print Slip";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnShifttiming.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnShifttiming.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShifttiming.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShifttiming.Name = "BtnShifttiming";
		this.BtnShifttiming.Size = new System.Drawing.Size(89, 24);
		this.BtnShifttiming.Text = "Shift Timing";
		this.BtnShifttiming.Click += new System.EventHandler(BtnShifttiming_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnDutyRoasterForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDutyRoasterForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDutyRoasterForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDutyRoasterForm.Name = "btnDutyRoasterForm";
		this.btnDutyRoasterForm.Size = new System.Drawing.Size(97, 24);
		this.btnDutyRoasterForm.Text = "Duty Roaster";
		this.btnDutyRoasterForm.Click += new System.EventHandler(btnDutyRoasterForm_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 716);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 710);
		this.panel6.TabIndex = 0;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 60);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 650);
		this.panel8.TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeRegistration.FilterMode = (FilterMode)2;
		this.grdEmployeeRegistration.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeRegistration.GroupTotals = (GroupTotals)2;
		this.grdEmployeeRegistration.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Name = "grdEmployeeRegistration";
		((ScrollableBase)this.grdEmployeeRegistration).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeRegistration).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeRegistration).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).Size = new System.Drawing.Size(950, 650);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).TabIndex = 1;
		this.grdEmployeeRegistration.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeRegistration.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeRegistration).VisualStyle = (VisualStyle)4;
		this.grdEmployeeRegistration.ColumnButtonClick += new ColumnActionEventHandler(grdEmployeeRegistration_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRegistration).DoubleClick += new System.EventHandler(grdEmployeeRegistration_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label43);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 60);
		this.panel7.TabIndex = 0;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 13f, System.Drawing.FontStyle.Bold);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(5, 31);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(74, 22);
		this.label43.TabIndex = 235;
		this.label43.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(901, 29);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdEmployeeRegistration;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnPrintRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 6;
		this.toolStrip2.Text = "toolStrip2";
		this.btnPrintRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintRegister.Name = "btnPrintRegister";
		this.btnPrintRegister.Size = new System.Drawing.Size(158, 24);
		this.btnPrintRegister.Text = "1111-&Print Register";
		this.btnPrintRegister.Click += new System.EventHandler(btnPrintRegister_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 742);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmEmployeeRegistration";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEmployeeRegistration_FormClosing);
		base.Load += new System.EventHandler(frmEmployeeRegistration_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmEmployeeRegistration_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.tabControl3.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalaryLoanAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalaryPayableAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalariesExpensesAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShift).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStore).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportedPerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportedBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocation).EndInit();
		this.tabPage6.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeSalary).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage7.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeAssetsBenefit).EndInit();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeDirectBenefits).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.tabPage8.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWeekDays).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.tabControl4.ResumeLayout(false);
		this.tabPage9.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAddressDetail).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbProvince).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAddressType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCountry).EndInit();
		this.tabPage10.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		this.panel25.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeExperience).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCityEmployeeExperience).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignationEmployeeExperience).EndInit();
		this.tabPage11.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		this.panel28.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeEducation).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDegree).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCityEducation).EndInit();
		this.tabPage12.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		this.panel31.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFamilyInfo).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRelationShip).EndInit();
		this.tabPage13.ResumeLayout(false);
		this.panel32.ResumeLayout(false);
		this.panel34.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdReference).EndInit();
		this.panel33.ResumeLayout(false);
		this.panel33.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignationEmployeeRefrence).EndInit();
		this.tabPage14.ResumeLayout(false);
		this.panel35.ResumeLayout(false);
		this.panel37.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBankAccount).EndInit();
		this.panel36.ResumeLayout(false);
		this.panel36.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBankAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxEmployeeSignature).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGender).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRelationTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBloodGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeCatagory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNationality).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBoxEmployeePhoto).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeRegistration).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
