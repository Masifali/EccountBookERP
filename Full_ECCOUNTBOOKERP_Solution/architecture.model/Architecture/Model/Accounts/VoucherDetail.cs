using System;

namespace Architecture.Model.Accounts;

public class VoucherDetail
{
	public DateTime? DCheqDate { get; set; }

	public DateTime? GpDate { get; set; }

	public double Adjustment { get; set; }

	public double AdvanceAmount { get; set; }

	public double Commission { get; set; }

	public double CreditAmount { get; set; }

	public double DCurrencyAmount { get; set; }

	public double DebitAmount { get; set; }

	public double TaxAmount { get; set; }

	public double DExchangeCurrencyRate { get; set; }

	public double Expenses { get; set; }

	public double ExTax { get; set; }

	public double Freight { get; set; }

	public double ItemAmount { get; set; }

	public double ItemRate { get; set; }

	public double Journal { get; set; }

	public double QtyIn { get; set; }

	public double QtyOut { get; set; }

	public double RateCut { get; set; }

	public double RateCutAmount { get; set; }

	public double SaleTax { get; set; }

	public double TaxesTotalAmount { get; set; }

	public double TaxPrcnt { get; set; }

	public double WeightIn { get; set; }

	public double WeightOut { get; set; }

	public double WhtHolding { get; set; }

	public double ItemCgsRate { get; set; }

	public double TotalDebitAmount { get; set; }

	public double TotalCreditAmount { get; set; }

	public double ThirdCurrencyFcyExchangeRate { get; set; }

	public double ThirdCurrencyHcyExchangeRate { get; set; }

	public double ThirdCurrencyAmount { get; set; }

	public double ThirdCurrencyReceiverExchangeRate { get; set; }

	public double ThirdCurrencyReceiverFcyAmount { get; set; }

	public int ThirdCurrencyId { get; set; }

	public int AccountId { get; set; }

	public int AgainstAccountId { get; set; }

	public int DMultiCurrencyId { get; set; }

	public int DocumentTypeIdRef { get; set; }

	public int GpNo { get; set; }

	public int Id { get; set; }

	public int InvoiceNoRefId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int OrderNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public int EmployeeId { get; set; }

	public int SubsidiaryTypeId { get; set; }

	public int SubsidiaryAccountId { get; set; }

	public int SubsidiaryAgainstTypeId { get; set; }

	public int SubsidiaryAgainstAccountId { get; set; }

	public int TaxTypeId { get; set; }

	public int ActionId { get; set; }

	public int VoucherHeadId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocNoDetailId { get; set; }

	public int RefDocSubIdNo { get; set; }

	public int LineId { get; set; }

	public int InstrumentTypeId { get; set; }

	public int SubNo { get; set; }

	public int SortNo { get; set; }

	public int IsCGS { get; set; }

	public string CheqNoDetail { get; set; }

	public string Comments { get; set; }

	public string CommentsOtherLingo { get; set; }

	public string DConversionFormula { get; set; }

	public string IsTaxable { get; set; }

	public string PaymentType { get; set; }

	public string RefInvoiceNo { get; set; }

	public string TaxesRemarks { get; set; }

	public string VehicleNo { get; set; }

	public string PayeeTitle { get; set; }

	public int PaymentTypeId { get; set; }

	public int ChequeTypeId { get; set; }

	public int BranchesId { get; set; }

	public int CostCenterId { get; set; }

	public double SBRTaxAmount { get; set; }

	public double DiscountPercent { get; set; }

	public double DiscountAmount { get; set; }

	public int ReferenceAccountId { get; set; }

	public int LocationTypeId { get; set; }

	public int BaseFcyId { get; set; }

	public double BaseFcyExchangeRate { get; set; }

	public double BaseFcyAmount { get; set; }

	public virtual string AccountCode { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string Location { get; set; }

	public virtual string CostCenterName { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string AgainstAccount { get; set; }

	public virtual string CheqPartyName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string SubsidiaryAccountTitle { get; set; }

	public virtual string InstrumentType { get; set; }

	public virtual string ReferenceAccount { get; set; }

	public virtual int AccountTypeId { get; set; }

	public virtual string BaseFcyCode { get; set; }

	public virtual string GlcyCode { get; set; }

	public virtual string TcyCode { get; set; }
}
