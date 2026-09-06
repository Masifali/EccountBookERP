using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImInvoiceBankGDBreakUp
{
	public int Id { get; set; }

	public int ExImInvoiceId { get; set; }

	public string BankInvoiceNo { get; set; }

	public DateTime BankInvoiceDate { get; set; }

	public string GDNO { get; set; }

	public DateTime GDDate { get; set; }

	public decimal GDValue { get; set; }

	public decimal GDWeight { get; set; }

	public int GDNoofContrainers { get; set; }

	public int BankId { get; set; }

	public int ActionTypeId { get; set; }

	public int FormDocumentTypeId { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual decimal OtherAmount { get; set; }

	public virtual string BankName { get; set; }

	public int DueDays { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal Freight { get; set; }

	public decimal FobValue { get; set; }

	public decimal CommAmount { get; set; }

	public decimal NetToBeRealized { get; set; }

	public decimal CommPercent { get; set; }

	public string Remarks { get; set; }

	public int DocumentTypeId { get; set; }

	public double CustomDutyPercent { get; set; }

	public double CustomDutyAmount { get; set; }

	public double SalesTaxPercent { get; set; }

	public double SalesTaxAmount { get; set; }

	public double AdditionalCustomDutyPercent { get; set; }

	public double AdditionalCustomDutyAmount { get; set; }

	public double IncomeTaxPercent { get; set; }

	public double IncomeTaxAmount { get; set; }

	public virtual List<ExImInvoiceBankGDBreakUp> ExImInvoiceBankGDBreakUpslist { get; set; }
}
