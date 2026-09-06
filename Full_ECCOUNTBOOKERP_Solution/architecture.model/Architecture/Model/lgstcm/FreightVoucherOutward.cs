using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.lgstcm;

public class FreightVoucherOutward
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal biltyFreight { get; set; }

	public decimal freightRate { get; set; }

	public decimal netWeight { get; set; }

	public decimal NetWeightDo { get; set; }

	public decimal NetWeightWB { get; set; }

	public decimal otherCharges { get; set; }

	public decimal QtyforRate { get; set; }

	public decimal totalbiltyfreight { get; set; }

	public decimal AdvanceOrCashFreight { get; set; }

	public decimal totalFreight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int documentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int FreightVoucherOutwardId { get; set; }

	public int GatePassOutwardId { get; set; }

	public int InvDeliveryOrderId { get; set; }

	public int chargeToDrAccountId { get; set; }

	public int loadingFromCityId { get; set; }

	public int ModifyUserId { get; set; }

	public decimal NoOfBags { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int PurchaseOrderHeaderId { get; set; }

	public int rateUomId { get; set; }

	public int BillWeightBaseId { get; set; }

	public int TransporterSupCustId { get; set; }

	public int transporterId { get; set; }

	public int unloadingToCityId { get; set; }

	public string ApprovalRemarks { get; set; }

	public string AttachmentsValues { get; set; }

	public DateTime biltyDate { get; set; }

	public string biltyNo { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string RemarksHeader { get; set; }

	public string vehicleNo { get; set; }

	public int MasterDocId { get; set; }

	public int MasterDocNo { get; set; }

	public DateTime? MasterDocDate { get; set; }

	public string MasterDocRemarks { get; set; }

	public virtual string ChargedToDrAccountTitle { get; set; }

	public virtual string ChargedToDrAccountCode { get; set; }

	public virtual string TransporterAccountTitle { get; set; }

	public virtual string TransporterAccountCode { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string BillWeightBase { get; set; }

	public virtual int GpSrNo { get; set; }

	public virtual DateTime? GpDate { get; set; }

	public virtual int DeliveryOrderNo { get; set; }

	public virtual DateTime? DeliveryOrderDate { get; set; }

	public virtual string UnloadingToCityName { get; set; }

	public virtual string LoadingFromCityName { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }

	public virtual List<FreightVoucherOutward> FreightVoucherOutwardList { get; set; }

	public virtual List<FreightVoucherOutwardPaymentDetail> FreightVoucherOutwardPaymentDetailList { get; set; }

	public virtual List<FreightVoucherOutwardExpenseDetail> FreightVoucherOutwardExpenseDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadList { get; set; }
}
