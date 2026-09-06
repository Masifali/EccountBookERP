using System;

namespace Architecture.Model.ContractorWages;

public class InvContractorWagesBillDetail
{
	public DateTime RefDocDate { get; set; }

	public double BillWeight { get; set; }

	public double WageRate { get; set; }

	public double RateAddLess { get; set; }

	public double WagesAmount { get; set; }

	public double Weight { get; set; }

	public double WeightCut { get; set; }

	public double BillQty { get; set; }

	public double Qty { get; set; }

	public double RefDocQty { get; set; }

	public double RefDocWeight { get; set; }

	public bool FreeOfCost { get; set; }

	public bool IsCompany { get; set; }

	public int RefLineId { get; set; }

	public int ContractorId { get; set; }

	public int JobOrderId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int Id { get; set; }

	public int InvConractorWagesAccountsId { get; set; }

	public int InvContractorWagesBillHeaderId { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public double PackSize { get; set; }

	public int WagesTypeId { get; set; }

	public int WareHouseFromId { get; set; }

	public int WareHouseToId { get; set; }

	public int WbTransactionsIdDt { get; set; }

	public int InvContractorWagesScheduleId { get; set; }

	public string Crop { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string WagesAccountName { get; set; }

	public virtual int TicketNo { get; set; }

	public virtual int PurchaseGLAC { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string WareHouseFrom { get; set; }

	public virtual string WareHouseTo { get; set; }

	public virtual string WarehouseType { get; set; }
}
