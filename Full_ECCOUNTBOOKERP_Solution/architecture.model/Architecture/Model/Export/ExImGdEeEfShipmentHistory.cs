using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImGdEeEfShipmentHistory
{
	public DateTime BLDate { get; set; }

	public DateTime ContractLcDate { get; set; }

	public DateTime DateOfNegotiation { get; set; }

	public DateTime DateOfRealization { get; set; }

	public DateTime DateOfShipment { get; set; }

	public DateTime DocCourierDate { get; set; }

	public DateTime DocToBankDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime FeReturnMonth { get; set; }

	public DateTime InvoiceDate { get; set; }

	public DateTime LotCompletedDate { get; set; }

	public DateTime SalesOrderDate { get; set; }

	public DateTime SchInspectionDate { get; set; }

	public DateTime SchProductionDate { get; set; }

	public DateTime SchShipmentDate { get; set; }

	public double AdvancePaymentFc { get; set; }

	public double CadDaFc { get; set; }

	public double ExchangeRate { get; set; }

	public double FobValue { get; set; }

	public double GrossKgs { get; set; }

	public double NetKgs { get; set; }

	public double RealizedAmountRs { get; set; }

	public double RealizedAmouontFC { get; set; }

	public double TotalAmountFc { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int ConsigneeId { get; set; }

	public int ContractCurrencyId { get; set; }

	public int DeliveryTermsId { get; set; }

	public int DestinationPortId { get; set; }

	public int DueDays { get; set; }

	public int EformNoId { get; set; }

	public int ExImInvoiceNoId { get; set; }

	public int ExporterBankId { get; set; }

	public int FeReturnPageNo { get; set; }

	public int Id { get; set; }

	public int ImporterBankId { get; set; }

	public int Item_ItrsNo { get; set; }

	public int LcOrderId { get; set; }

	public int LoadingPortId { get; set; }

	public int NoOfContainers { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RealizedCurrencyId { get; set; }

	public int SalesOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int ThisNoOfShipment { get; set; }

	public int VoucherId { get; set; }

	public string BankGLAccount { get; set; }

	public int BillOfLadingNo { get; set; }

	public string ContractCurrency { get; set; }

	public string ContractNoLcNo { get; set; }

	public string CourierType { get; set; }

	public string CustomerName { get; set; }

	public string DeliveryTerms { get; set; }

	public string DestinationPortName { get; set; }

	public string EFCommodityDesc { get; set; }

	public string EFormNo { get; set; }

	public string ExImInvoiceNo { get; set; }

	public string ExporterBankName { get; set; }

	public string FbpNo { get; set; }

	public string FePaymentTermRem { get; set; }

	public string ForwarderName { get; set; }

	public string HsCode { get; set; }

	public string ImporterBankName { get; set; }

	public string LoadingPortName { get; set; }

	public string RealizedCurrencyName { get; set; }

	public string RemarksDesc { get; set; }

	public string SalesOrderNo { get; set; }

	public string SalesTerm { get; set; }

	public string ScheduleDesc { get; set; }

	public string ShippingAgentName { get; set; }

	public string ShippingCompanyName { get; set; }

	public string VoucherNo { get; set; }

	public virtual List<ExImGdEeEfShipmentHistoryGpVehicleContainerDetail> eximGdEeEfShipmentHistoryDetail { get; set; }
}
