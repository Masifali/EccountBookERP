using System;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeLoadingDeliveryDetail
{
	public DateTime BiltyDate { get; set; }

	public DateTime DocDateDt { get; set; }

	public decimal BiltyExpense { get; set; }

	public decimal BiltyFreight { get; set; }

	public decimal CusAdLsWt { get; set; }

	public decimal CusGrossWeight { get; set; }

	public decimal CusNetWeight { get; set; }

	public decimal CusRate { get; set; }

	public decimal CusPckMtrlWeight { get; set; }

	public decimal SupAdLsWt { get; set; }

	public decimal SupGrossWeight { get; set; }

	public decimal SupNetWeight { get; set; }

	public decimal SupRate { get; set; }

	public decimal SupPckMtrlWeight { get; set; }

	public decimal VehicleGrossWeight { get; set; }

	public decimal VehicleQty { get; set; }

	public decimal ItemSupQty { get; set; }

	public decimal ItemSupWeight { get; set; }

	public int ActionTypeId { get; set; }

	public int BrokerId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CommTradeLoadingDeliveryId { get; set; }

	public int CropYearId { get; set; }

	public int CustomerId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int OrderDetailId { get; set; }

	public int OrderId { get; set; }

	public int PackingTypeId { get; set; }

	public decimal Qty { get; set; }

	public int SortNo { get; set; }

	public int SupplierId { get; set; }

	public int TransporterId { get; set; }

	public int UomId { get; set; }

	public string BiltyNo { get; set; }

	public string CusRemarks { get; set; }

	public string SupRemarks { get; set; }

	public string VehicleNo { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual DateTime OrderDate { get; set; }

	public virtual string CommissionAgent { get; set; }

	public virtual string BrokerName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string TransporterName { get; set; }
}
