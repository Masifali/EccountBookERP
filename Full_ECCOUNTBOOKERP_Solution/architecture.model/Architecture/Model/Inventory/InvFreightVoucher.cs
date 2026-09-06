using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Inventory;

public class InvFreightVoucher
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime GatePassDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal DifferenceWeight { get; set; }

	public decimal FactoryWeight { get; set; }

	public decimal SupplierWeight { get; set; }

	public decimal ToleranceWeight { get; set; }

	public decimal TotalPaidAmount { get; set; }

	public double AllowShortage { get; set; }

	public double ShortageApply { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int CashAccountId { get; set; }

	public int GatePassNo { get; set; }

	public int GatePassId { get; set; }

	public int Id { get; set; }

	public int FreightOn { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int CityId { get; set; }

	public string BiltyNo { get; set; }

	public string RemarksHeader { get; set; }

	public string VehicleNo { get; set; }

	public string ManualNo { get; set; }

	public int driverBiodataId { get; set; }

	public virtual string PartyName { get; set; }

	public virtual string VarietyName { get; set; }

	public virtual double GpQty { get; set; }

	public virtual int RefDocumentTypeId { get; set; }

	public virtual int PolicyTypeId { get; set; }

	public virtual string DriverName { get; set; }

	public virtual string CnicNo { get; set; }

	public virtual string DriverCellNo { get; set; }

	public virtual string whatsappNo { get; set; }

	public virtual string AlternateCellNo { get; set; }

	public virtual string FatherName { get; set; }

	public virtual string fatherCnicNo { get; set; }

	public virtual List<InvFreightVoucherDetail> InvFreightVoucherDetailslist { get; set; }

	public virtual List<InvFreightVoucherPaymentBreakup> InvFreightVoucherPaymentBreakupList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public InvFreightVoucher()
	{
		InvFreightVoucherDetailslist = new List<InvFreightVoucherDetail>();
		InvFreightVoucherPaymentBreakupList = new List<InvFreightVoucherPaymentBreakup>();
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
	}
}
