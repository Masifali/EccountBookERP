using System;
using System.Collections.Generic;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeOrder
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

	public decimal OrderAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal SubBrokeryAmount { get; set; }

	public decimal SubBrokeryRate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CalculationParameterId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SubBrokerId { get; set; }

	public string ManualRefNo { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public virtual string SubBrokerName { get; set; }

	public virtual string CalculationParameter { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual List<CommTradeOrderDetail> CommTradeOrderDetailList { get; set; }

	public virtual List<CommTradeOrderCommBrokery> CommTradeOrderCommBrokeryList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
