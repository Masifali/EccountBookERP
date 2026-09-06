using System;

namespace Architecture.Model.ModernHistoryModels;

public class OrderHeaderModel
{
	public int OrderId { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public string OrderCategoryName { get; set; }

	public int CatagorySrNo { get; set; }

	public string SupplierRefNo { get; set; }

	public int RefrenenceParty { get; set; }

	public string PaymentTerm { get; set; }

	public int OrderDueDays { get; set; }

	public DateTime OrderDueDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public string DeliveryTerm { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public int DeliveryDays { get; set; }

	public string DeliveryRemarks { get; set; }

	public string RemarksHeader { get; set; }

	public bool IsAproved { get; set; }

	public string OrderStatus { get; set; }

	public string ExpiryStatus { get; set; }

	public int AttachmentsCount { get; set; }

	public string Supplier_Customer_Name { get; set; }

	public string Supplier_Customer_Mobile { get; set; }

	public string Supplier_Customer_Email { get; set; }

	public string Supplier_Customer_Address { get; set; }

	public int CommissionAgentId { get; set; }

	public string CommissionAgentName { get; set; }

	public string CommissionType { get; set; }

	public double CommRate { get; set; }

	public int UomScheduleIdCmRate { get; set; }

	public double CommAmount { get; set; }

	public int BrokerAgentId { get; set; }

	public string BrokerAgentName { get; set; }

	public string BrokeryType { get; set; }

	public double BrokeryRate { get; set; }

	public double BrokeryUom { get; set; }

	public double BrokeryAmount { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string ApprovalUserName { get; set; }

	public int PostUser { get; set; }

	public DateTime PostDate { get; set; }

	public string EntryUserProfileImage { get; set; }

	public string ModifyUserProfileImage { get; set; }

	public string ApprovalUserProfileImage { get; set; }
}
