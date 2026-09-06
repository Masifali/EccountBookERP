using System;

namespace Architecture.Model.ModernHistoryModels;

public class Prucahse_Slae_InvoiceHistoryHeader
{
	public int Sale_Purcahse_OrderNumber { get; set; }

	public int InvoiceId { get; set; }

	public bool IsApproved { get; set; }

	public int InvoiceNumber { get; set; }

	public DateTime InvoiceDate { get; set; }

	public string Reference { get; set; }

	public string ManualBillNumber { get; set; }

	public int DueDays { get; set; }

	public int AttachmentsCount { get; set; }

	public int NoOfAttachments { get; set; }

	public int InvoiceTypeId { get; set; }

	public DateTime DueDate { get; set; }

	public string InvoiceHeaderRemarks { get; set; }

	public int Sale_Purchase_OrderId { get; set; }

	public int BrokerGlId { get; set; }

	public int ComissionAgentGlId { get; set; }

	public int Customer_Supplier_Id { get; set; }

	public string Customer_Supplier_Name { get; set; }

	public string Customer_Supplier_MobileNumber { get; set; }

	public string Customer_Supplier_Email { get; set; }

	public string Customer_Supplier_Address { get; set; }

	public int Customer_Supplier_GlAccountId { get; set; }

	public int ComissionAgentId { get; set; }

	public string ComissionAgentName { get; set; }

	public string ComissionType { get; set; }

	public double ComissioniRate { get; set; }

	public string ComissionRateUom { get; set; }

	public double ComissionAmount { get; set; }

	public string ComissionRemarks { get; set; }

	public bool ComissionPresent { get; set; }

	public int BrokerAgentId { get; set; }

	public string BrokerAgentName { get; set; }

	public string BrokeryType { get; set; }

	public double BrokeryRate { get; set; }

	public double BrokeryRateUom { get; set; }

	public double BrokeryAmount { get; set; }

	public string BrokeryRemarks { get; set; }

	public bool BorkeryPresent { get; set; }

	public int Grn_Gdn_Number { get; set; }

	public string EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string ApprovedUser { get; set; }

	public DateTime ApprovedDate { get; set; }

	public double GrandBillAmount { get; set; }
}
