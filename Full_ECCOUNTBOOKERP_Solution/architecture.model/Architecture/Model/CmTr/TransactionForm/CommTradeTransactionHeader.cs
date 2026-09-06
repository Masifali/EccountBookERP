using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BrokeryAmount { get; set; }

	public decimal BrokeryRate { get; set; }

	public decimal CommAmount { get; set; }

	public decimal CommRate { get; set; }

	public decimal SubBrokeryAmount { get; set; }

	public decimal SubBrokeryRate { get; set; }

	public decimal BillAmount { get; set; }

	public int CurrencyId { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CalculationParameterId { get; set; }

	public int CommissionAgentId { get; set; }

	public int DifferenceAccountId { get; set; }

	public int BrokerAgentId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SubBrokerId { get; set; }

	public string BiltyDates { get; set; }

	public string BiltyNos { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public string VehicleNos { get; set; }

	public virtual string SubBrokerName { get; set; }

	public virtual string CalculationParameter { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual List<CommTradeTransactionDetail> CommTradeTransactionDetailList { get; set; }

	public virtual List<CommTradeTransactionRevExp> CommTradeTransactionRevExpList { get; set; }

	public virtual List<CommTradeTransactionPmOther> CommTradeTransactionPmOtherList { get; set; }

	public virtual List<CommTradeTransactionCommBrokery> CommTradeTransactionCommBrokeryList { get; set; }

	public virtual List<CommTradeTransactionSupplierSummary> CommTradeTransactionSupplierSummaryList { get; set; }

	public virtual List<CommTradeTransactionPaymentTermsDetail> CommTradeTransactionPaymentTermsDetailList { get; set; }

	public virtual List<CommTradeTransactionVehicleFreightDetail> CommTradeTransactionVehicleFreightDetailList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
