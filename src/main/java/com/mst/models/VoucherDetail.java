package com.mst.models;

import java.time.LocalDate;
import java.time.LocalDateTime;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.FetchType;
import javax.persistence.GeneratedValue;
import javax.persistence.GenerationType;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.Table;

import lombok.Data;

/**
 * Ditto of the real GoldenAcedb dbo.VoucherDetail table - one row per debit/credit
 * line under a VoucherHead. Shared the same way as VoucherHead across every voucher
 * type in the whole ERP, so most columns here (ItemId, GpNo/VehicleNo/QtyIn/WeightIn,
 * SupplierCustomerId, ...) belong to Purchase/Sale/weighbridge-style vouchers not
 * built yet - a simple Cash/Bank/Journal/Contra/Expense voucher only ever populates
 * AccountId, DebitAmount/CreditAmount and Comments.
 *
 * Only VoucherHeadId and AccountId are wired as JPA relations (to VoucherHead and
 * the already-real, though still mismapped-table-name, ChartofAccount) - every
 * other *Id-looking column is kept as a plain Integer for the same reason as above.
 */

@Entity
@Table(name = "VoucherDetail")
@Data
public class VoucherDetail {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "Id")
	private Integer id;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "VoucherHeadId", nullable = false)
	private VoucherHead voucherHead;

	@ManyToOne(fetch = FetchType.LAZY)
	@JoinColumn(name = "AccountId", nullable = false)
	private ChartofAccount account;

	@Column(name = "AgainstAccountId")
	private Integer againstAccountId;

	@Column(name = "Comments", length = 1000)
	private String comments;

	@Column(name = "CommentsOtherLingo", length = 1000)
	private String commentsOtherLingo;

	@Column(name = "DebitAmount")
	private Double debitAmount;

	@Column(name = "CreditAmount")
	private Double creditAmount;

	@Column(name = "JobLotId")
	private Integer jobLotId;

	@Column(name = "RefInvoiceNo", length = 50)
	private String refInvoiceNo;

	@Column(name = "TaxesTotalAmount")
	private Double taxesTotalAmount;

	@Column(name = "TaxesRemarks", length = 150)
	private String taxesRemarks;

	@Column(name = "IsTaxable", length = 50)
	private String isTaxable;

	@Column(name = "TaxTypeId")
	private Integer taxTypeId;

	@Column(name = "TaxPrcnt")
	private Double taxPrcnt;

	@Column(name = "DCheqDate")
	private LocalDate dCheqDate;

	@Column(name = "CheqNoDetail", length = 50)
	private String cheqNoDetail;

	@Column(name = "DocumentTypeIdRef")
	private Integer documentTypeIdRef;

	@Column(name = "InvoiceNoRefId")
	private Integer invoiceNoRefId;

	@Column(name = "RefDocSubIdNo")
	private Integer refDocSubIdNo;

	@Column(name = "ItemId")
	private Integer itemId;

	@Column(name = "OrderNo")
	private Integer orderNo;

	@Column(name = "GpNo")
	private Integer gpNo;

	@Column(name = "VehicleNo", length = 50)
	private String vehicleNo;

	@Column(name = "GpDate")
	private LocalDate gpDate;

	@Column(name = "QtyIn")
	private Double qtyIn;

	@Column(name = "QtyOut")
	private Double qtyOut;

	@Column(name = "WeightIn")
	private Double weightIn;

	@Column(name = "WeightOut")
	private Double weightOut;

	@Column(name = "SupplierCustomerId")
	private Integer supplierCustomerId;

	@Column(name = "ItemRate")
	private Double itemRate;

	@Column(name = "RateCut")
	private Double rateCut;

	@Column(name = "RateCutAmount")
	private Double rateCutAmount;

	@Column(name = "ItemAmount")
	private Double itemAmount;

	@Column(name = "Expenses")
	private Double expenses;

	@Column(name = "Freight")
	private Double freight;

	@Column(name = "Journal")
	private Double journal;

	@Column(name = "Commission")
	private Double commission;

	@Column(name = "DMultiCurrencyId")
	private Integer dMultiCurrencyId;

	@Column(name = "DConversionFormula", length = 50)
	private String dConversionFormula;

	@Column(name = "DExchangeCurrencyRate")
	private Double dExchangeCurrencyRate;

	@Column(name = "DCurrencyAmount")
	private Double dCurrencyAmount;

	@Column(name = "PaymentType", length = 50)
	private String paymentType;

	@Column(name = "AdvanceAmount")
	private Double advanceAmount;

	@Column(name = "WhtHolding")
	private Double whtHolding;

	@Column(name = "SaleTax")
	private Double saleTax;

	@Column(name = "ExTax")
	private Double exTax;

	@Column(name = "Adjustment")
	private Double adjustment;

	@Column(name = "ActionId")
	private Integer actionId;

	@Column(name = "LineId")
	private Integer lineId;

	@Column(name = "ItemCgsRate")
	private Double itemCgsRate;

	@Column(name = "PreviousValue")
	private Double previousValue;

	@Column(name = "IsStockRecon")
	private Boolean isStockRecon;

	@Column(name = "TotalCreditAmount")
	private Double totalCreditAmount;

	@Column(name = "TotalDebitAmount")
	private Double totalDebitAmount;

	@Column(name = "SubNo")
	private Integer subNo;

	@Column(name = "EntryDate")
	private LocalDateTime entryDate;

	@Column(name = "ChequeStatus")
	private Boolean chequeStatus;

	@Column(name = "PayeeTitle", length = 100)
	private String payeeTitle;

	@Column(name = "SubsidiaryTypeId")
	private Integer subsidiaryTypeId;

	@Column(name = "EmployeeId")
	private Integer employeeId;

	@Column(name = "SubsidiaryAccountId")
	private Integer subsidiaryAccountId;

	@Column(name = "BookmarkStatus")
	private Boolean bookmarkStatus;

	@Column(name = "BookmarkRemarks")
	private String bookmarkRemarks;

	@Column(name = "InvoiceAmount")
	private Double invoiceAmount;

	@Column(name = "TotalPaidAmount")
	private Double totalPaidAmount;

	@Column(name = "IsCGS")
	private Integer isCGS;

	@Column(name = "SubsidiaryAgainstTypeId")
	private Integer subsidiaryAgainstTypeId;

	@Column(name = "SubsidiaryAgainstAccountId")
	private Integer subsidiaryAgainstAccountId;

	@Column(name = "ThirdCurrencyId")
	private Integer thirdCurrencyId;

	@Column(name = "ThirdCurrencyFcyExchangeRate")
	private Double thirdCurrencyFcyExchangeRate;

	@Column(name = "ThirdCurrencyHcyExchangeRate")
	private Double thirdCurrencyHcyExchangeRate;

	@Column(name = "ThirdCurrencyAmount")
	private Double thirdCurrencyAmount;

	@Column(name = "ThirdCurrencyReceiverExchangeRate")
	private Double thirdCurrencyReceiverExchangeRate;

	@Column(name = "ThirdCurrencyReceiverFcyAmount")
	private Double thirdCurrencyReceiverFcyAmount;

	@Column(name = "SortNo")
	private Integer sortNo;

	@Column(name = "PayeeOnly")
	private Boolean payeeOnly;

	@Column(name = "InstrumentTypeId")
	private Integer instrumentTypeId;

	@Column(name = "ChequeTypeId")
	private Integer chequeTypeId;

	@Column(name = "BranchesId")
	private Integer branchesId;

	@Column(name = "CostCenterId")
	private Integer costCenterId;

	@Column(name = "SBRTaxAmount")
	private Double sBRTaxAmount;

	@Column(name = "DiscountPercent")
	private Double discountPercent;

	@Column(name = "DiscountAmount")
	private Double discountAmount;

	@Column(name = "ReferenceAccountId")
	private Integer referenceAccountId;

	@Column(name = "LocationTypeId")
	private Integer locationTypeId;

	@Column(name = "PaymentTypeId")
	private Integer paymentTypeId;

	@Column(name = "TaxAmount")
	private Double taxAmount;

	@Column(name = "BaseFcyId")
	private Integer baseFcyId;

	@Column(name = "BaseFcyExchangeRate")
	private Double baseFcyExchangeRate;

	@Column(name = "BaseFcyAmount")
	private Double baseFcyAmount;

	@Column(name = "RefDocumentTypeId")
	private Integer refDocumentTypeId;

	@Column(name = "RefDocNoId")
	private Integer refDocNoId;

	@Column(name = "RefDocNoDetailId")
	private Integer refDocNoDetailId;

}