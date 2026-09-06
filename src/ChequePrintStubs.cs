using System;
using System.Windows.Forms;

namespace Architecture.WinApp
{
    public class ChequePrintBase : UserControl
    {
        public int Id { get; set; }
        public int ChequeTypeId { get; set; }
        public string CheqType { get; set; }
        public string ChequeNumber { get; set; }
        public string PayeeTitle { get; set; }
        public string Amount { get; set; }
        public string AmountInWords { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string M1 { get; set; }
        public string M2 { get; set; }
        public string Y1 { get; set; }
        public string Y2 { get; set; }
        public string Y3 { get; set; }
        public string Y4 { get; set; }
    }

    public class ChequePrint : ChequePrintBase { }
    public class BankAlFalah : ChequePrintBase { }
    public class ABL : ChequePrintBase { }
    public class AskariBank : ChequePrintBase { }
    public class BankAlHabib : ChequePrintBase { }
    public class FaysalBank : ChequePrintBase { }
    public class HabibMetro : ChequePrintBase { }
    public class HBL : ChequePrintBase { }
    public class MCB : ChequePrintBase { }
    public class MeezanBank : ChequePrintBase { }
    public class NBP : ChequePrintBase { }
    public class NIB : ChequePrintBase { }
    public class SaadiqStandardCharteredBank : ChequePrintBase { }
    public class SilkBank : ChequePrintBase { }
    public class SoneriBank : ChequePrintBase { }
    // public class PdcBank : ChequePrintBase { }

    public class frmpayeeTitle : Form
    {
        public frmpayeeTitle() { }
        public frmpayeeTitle(object userAccount) { }
    }
}

namespace Architecture.WinApp.ChequePrinting
{
    public class ChequePrinting : Form
    {
        public ChequePrinting() { }
        public ChequePrinting(object userAccount) { }
    }
}


namespace Architecture.BLL.Accounts
{
    public class tblUserRights
    {
        public static System.Data.DataTable GetByUserId(int userId) { return new System.Data.DataTable(); }
    }
    public class pdcTransaction
    {
        public static string GenerateCode(params object[] args) { return ""; }
        public static string GenerateCode(object a, object b, object c) { return ""; }
        public static string GenerateCode(object a, object b) { return ""; }
    }
    public class AccountsPrematurePaymentsReceipts
    {
        public static string GenerateCode(object a, object b) { return ""; }
        public static string GenerateCode(object a, object b, object c) { return ""; }
    }
}
namespace Architecture.WinApp.Bank
{
    public class PdcBank : System.Windows.Forms.Form
    {
        public PdcBank() { }
        public PdcBank(object userAccount) { }
    }
}


namespace Architecture.WinApp
{
    public class Attachment : System.Windows.Forms.Form
    {
        public static System.Data.DataTable dtAttachments = new System.Data.DataTable();
        public System.Collections.Generic.List<object> lst = new System.Collections.Generic.List<object>();
        public System.Collections.Generic.List<object> RemovedAttachmentListInUpdateCase = new System.Collections.Generic.List<object>();
        public Attachment() { }
        public Attachment(params object[] args) { }
    }
    public class LoadPendingInvoicesByPayment : System.Windows.Forms.Form
    {
        public System.Data.DataTable dtInvoices = new System.Data.DataTable();
        public LoadPendingInvoicesByPayment() { }
        public LoadPendingInvoicesByPayment(params object[] args) { }
    }
}


namespace Architecture.WinApp
{
    public class RichAttachment : System.Windows.Forms.Form
    {
        public static System.Data.DataTable dtAttachments = new System.Data.DataTable();
        public System.Collections.Generic.List<object> lst = new System.Collections.Generic.List<object>();
        public System.Collections.Generic.List<object> RemovedAttachmentListInUpdateCase = new System.Collections.Generic.List<object>();
        public RichAttachment() { }
        public RichAttachment(params object[] args) { }
    }
    public class AttachmentAddingFromHistory : System.Windows.Forms.Form
    {
        public AttachmentAddingFromHistory(params object[] args) { }
    }
    public class BankBalanceManualEntry : System.Windows.Forms.Form
    {
        public BankBalanceManualEntry(params object[] args) { }
    }
}
namespace Architecture.BLL.Accounts
{
    public class PdcTransaction
    {
        public static int DeleteByID(params object[] args) { return 0; }
        public static System.Data.DataTable GetDataForDropDownFrompdcTransaction(params object[] args) { return new System.Data.DataTable(); }
        public static System.Data.DataTable FormHistory(params object[] args) { return new System.Data.DataTable(); }
        public static System.Data.DataTable Getall(params object[] args) { return new System.Data.DataTable(); }
        public static int Save(params object[] args) { return 0; }
        public static global::Architecture.Model.BankSchema.pdcTransaction GetByID(params object[] args) { return new global::Architecture.Model.BankSchema.pdcTransaction(); }
        public static string GetGenerateCode(params object[] args) { return ""; }
    }
}
namespace Architecture.BLL
{
    public class Country
    {
        public static System.Data.DataTable GetAll(params object[] args) { return new System.Data.DataTable(); }
        public static System.Data.DataTable Getall(params object[] args) { return new System.Data.DataTable(); }
    }
}


namespace Architecture.BLL.Accounts
{
    public class AccountsBudgetHeader
    {
        public static System.Data.DataTable BudgetBalances(params object[] args) { return new System.Data.DataTable(); }
    }
}
namespace Architecture.BLL
{
    public class DMSAttachments
    {
        public static System.Collections.Generic.List<global::Architecture.Model.DMSAttachments> GetByID(params object[] args)
        {
            return new System.Collections.Generic.List<global::Architecture.Model.DMSAttachments>();
        }
        public static int RemoveByIdAndNames(params object[] args) { return 0; }
    }
}


namespace Architecture.BLL.Accounts
{
    public class PdcTransactionBLL
    {
        public static System.Data.DataTable GetDataForDropDownFrompdcTransaction(params object[] args) { return new System.Data.DataTable(); }
        public static System.Data.DataTable FormHistory(params object[] args) { return new System.Data.DataTable(); }
    }
}
namespace Architecture.WinApp
{
    public class frmBankReconciliationUploadExcelSheet : System.Windows.Forms.Form
    {
        public frmBankReconciliationUploadExcelSheet(params object[] args) { }
    }
}


namespace Architecture.BLL.BankSchema
{
    public class PayeeTitle
    {
        public static System.Data.DataTable payeeTitle_ByGlAccountId(params object[] args) { return new System.Data.DataTable(); }
    }
}


namespace Architecture.BLL.Inventory
{
    public class SupplierCustomer
    {
        public static System.Data.DataTable FormHistoryForPartyProcessing(params object[] args) { return new System.Data.DataTable(); }
        public static System.Data.DataTable PartyTypeGetAll(params object[] args) { return new System.Data.DataTable(); }
        public static string GeneratePartyCode(params object[] args) { return ""; }
        public static System.Data.DataTable GetAccountsForSupplierCustomerByPartyTypeId(params object[] args) { return new System.Data.DataTable(); }
        public static int Save(params object[] args) { return 0; }
        public static global::Architecture.Model.Inventory.SupplierCustomer GetByID(params object[] args) { return new global::Architecture.Model.Inventory.SupplierCustomer(); }
        public static System.Data.DataTable GetDataForDropDownFrom_SupplierCustomer(params object[] args) { return new System.Data.DataTable(); }
    }
}
