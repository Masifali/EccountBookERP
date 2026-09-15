package com.mst.models.dto;

import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import lombok.Data;

@Data
public class VoucherRequestDto {
	private Integer id;
	private Integer documentTypeId;
	private Integer voucherCode;
	private LocalDate voucherDate;
	private String remarks;
	private Double voucherAmount;
	private Integer refAccountId;
	private Integer againstAccountId;
	private Integer supplierCustomerId;
	private String chequeNo;
	private LocalDate chequeDate;
	private String payTitle;
	private String manualBillNo;
	private Double billAmount;
	private LocalDate dueDate;
	private Integer dueDays;

	// Added for Expense Voucher (DocumentTypeId=26) - ditto ExpenseVoucherNew.cs's Insert():
	// vh.ProjectId (CmbProjectId - captioned "Cost Center" but really the desktop Projects table,
	// see CommonServices.ProjectServiceBind() -> EXEC Sp_Projects_GetAllMethod), vh.MultiCurrencyId/
	// ExchangeCurrencyRate/FcAmount (cmbCurrency/txtExchangeRate/txtFcyAmount - all 3 are
	// FormValidation()-required fields on this form). All nullable/optional so every other voucher
	// type sharing this same DTO/saveVoucher() pipeline is completely unaffected when they are absent.
	private Integer projectId;
	private Integer multiCurrencyId;
	private Double exchangeCurrencyRate;
	private Double fcAmount;

	// Ditto ExpenseVoucherNew.cs's Insert(): every detail-grid row's LocationTypeId
	// (cmbLocationType - a single header-level control in the simplified port, stored per detail
	// row exactly as desktop does: "vd.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);").
	private Integer locationTypeId;

	// When true, saveVoucher() also inserts the exact desktop-ditto SECOND (mirrored) VoucherDetail
	// row against refAccountId for every detail line - the real double-entry posting
	// ExpenseVoucherNew.cs's Insert() always performs (vd = debit grid-account line, vd2 = credit
	// RefAccountId line, see lines 2417-2483 of that file). Left unset (null/false) for every
	// voucher already using this DTO before Expense Voucher (CPV/BPV/CRV/BRV/JV/Contra) so their
	// already-verified, already-committed Save behavior is not changed by this addition - each of
	// those needs its own dedicated verification pass (debit/credit direction differs by voucher
	// type) before being opted in, tracked as a separate, explicitly flagged gap.
	//
	// UPDATE: CPV/BPV (PaymentVoucherNew.cs, shared VouchersWithTax form) verified this session -
	// its own Insert() "vd"/"vd2" pair debits the grid account and credits CmbCreditAccount
	// (= refAccountId), the exact same direction as Expense Voucher, so CPV/BPV now opt in too.
	// CRV/BRV/JV/Contra remain un-opted-in pending their own verification pass.
	private Boolean mirrorCreditToRefAccount;

	// Ditto PostDatedCheqPaymentVouchers.cs's Insert(): vh.RemarksOtherLingo (txtReceivedBy -
	// "Received By", a real VoucherHead column reused for this unrelated purpose on this one form).
	private String remarksOtherLingo;

	// CPV/BPV (PaymentVoucherNew.cs) header WHT (Withholding Tax) fields - ditto Insert():
	// vh.RefDocNoId = CmbAgainstAc.Value ("WHT Debit Ac" on screen), vh.IncludeWHT =
	// ChkBoxWthHolding.Checked. The WHT "Credit Account" (CmbWithHoldingAc) reuses the already-
	// existing head-level againstAccountId field above (vh.AgainstAccountId = CmbWithHoldingAc.Value
	// in the real Insert()), so no separate field was needed for it. taxTypeId/taxTypeName/
	// taxPercent/taxAmount carry the exact numbers the client already computed with the real
	// Total()/TaxAmountProportion() formula (Excluded Tax: gross-up; Included Tax: gross-down) so the
	// server only has to persist the two extra WHT GL rows ("vd3"/"vd4" in Insert()), never
	// recompute the tax math itself. All nullable; every other voucher that never sends them is
	// unaffected.
	private Integer refDocNoId;
	private Boolean includeWht;
	private Integer taxTypeId;
	private String taxTypeName;
	private Double taxPercent;
	private Double taxAmount;

	// Payment By Invoice (PaymentByInvoiceVoucherNew.cs - an alternate CPV/BPV entry screen tied to
	// specific invoices, NOT a separate DocumentTypeId; it saves into the SAME DocumentTypeId=1/2
	// VoucherHead/VoucherDetail rows as the regular CPV/BPV forms) header field - ditto Insert():
	// vh.BranchId (combbranch - overridable per voucher on this one form, unlike every other voucher
	// which only ever uses the session's own current branch).
	private Integer branchId;

	// When true, saveVoucher()'s WHT GL posting below creates ONLY the single debit row (ditto
	// PaymentByInvoiceVoucherNew.cs's Insert(): its own "vd2" WHT row sets DebitAmount only, with NO
	// matching credit-side row created anywhere in Insert() - a real, asymmetric one-row posting,
	// different from CPV/BPV's own real two-row vd3/vd4 pair). Left unset/false for CPV/BPV so their
	// already-verified two-row behavior is unchanged.
	private Boolean postSingleWhtDebitRow;

	private List<VoucherDetailRowDto> details = new ArrayList<>();

	@Data
	public static class VoucherDetailRowDto {
		private Integer id;
		private Integer accountId;
		private String comments;
		private Double debitAmount;
		private Double creditAmount;
		private Integer jobLotId;
		private String refInvoiceNo;
		private Integer supplierCustomerId;
		private String chequeNoDetail;
		private LocalDate chequeDateDetail;

		// Ditto ExpenseVoucherNew.cs's Insert(): vd.CostCenterId (r2.Cells["CostCenterId"] - the
		// Cost Center Breakup Detail grid column). Optional; VoucherDetail.CostCenterId column
		// already exists in the schema/entity, just never wired from any voucher's Save until now.
		private Integer costCenterId;

		// Ditto PostDatedCheqPaymentVouchers.cs's Insert() grid loop: vd.PaymentType (r.Cells
		// ["PayTitle"].Text - PDC Payment's per-row "Pay Title" grid column) and
		// vd.CommentsOtherLingo (r.Cells["BankName"].Value - the per-row Bank Name column, a real
		// VoucherDetail column reused for this unrelated purpose on this one form, same pattern as
		// VoucherHead.RemarksOtherLingo above). Optional/nullable for every other voucher.
		private String paymentType;
		private String commentsOtherLingo;

		// CPV/BPV (PaymentVoucherNew.cs) per-detail-line fields - ditto Insert()'s "vd"/"vd2" pair,
		// both of which set every one of these on every grid row: vd.PaymentTypeId (CmbPaymentType,
		// real EXEC [Account].[USP_lookUp_GetAllMethod] @Activity='GetDataPaymentTypeForPayments'
		// dropdown - paymentType above already carries its text), vd.InstrumentTypeId
		// (CmbFinancialInstrument, BPV-only, EXEC usp_getInstrumentType), vd.ChequeTypeId
		// (cmbChequeType, BPV-only, EXEC [dbo].[sp_ChequeType]), vd.PayeeTitle (txtPayTitle,
		// per-line), vd.ReferenceAccountId (CmbReferenceAccount, optional per-line reference
		// account). All nullable/optional for every other voucher that never sends them.
		private Integer paymentTypeId;
		private Integer instrumentTypeId;
		private Integer chequeTypeId;
		private String payeeTitle;
		private Integer referenceAccountId;

		// Payment By Invoice (PaymentByInvoiceVoucherNew.cs) per-detail-line fields - ditto
		// Insert(): vd.OrderNo (InvoiceId - the specific invoice this line pays down),
		// vd.DocumentTypeIdRef (RefDocumentTypeId - the invoice's own DocumentTypeId),
		// vd.QtyIn/QtyOut/ItemAmount (real VoucherDetail quantity/amount columns reused on this one
		// form to carry InvoiceAmount/TotalPaidAmount/BalanceAmount respectively, exactly as
		// Insert() does - the same kind of column reuse already seen on PDC Payment's
		// PaymentType/CommentsOtherLingo above), vd.WhtHolding (per-line WHT preview amount). All
		// nullable/optional for every other voucher.
		private Integer orderNo;
		private Integer documentTypeIdRef;
		private Double qtyIn;
		private Double qtyOut;
		private Double itemAmount;
		private Double whtHolding;

		// Payment By Invoice's own per-line AgainstAccountId (ditto Insert(): vd.AgainstAccountId =
		// combcreditac.Value, the CPV/BPV cash/bank account being paid FROM, stamped on every grid
		// line with NO mirror row created - single-entry style like PDC Payment). Kept as its own
		// field, separate from the header-level againstAccountId above, because this voucher ALSO
		// uses that header field for an unrelated purpose (its own WHT Credit Account) - reusing it
		// here would silently corrupt whichever value was set second. When present, takes priority
		// over the header-level dto.againstAccountId fallback in saveVoucher()'s detail loop.
		private Integer lineAgainstAccountId;

		public Integer getId() { return id; }
		public void setId(Integer id) { this.id = id; }
		public Integer getAccountId() { return accountId; }
		public void setAccountId(Integer accountId) { this.accountId = accountId; }
		public String getComments() { return comments; }
		public void setComments(String comments) { this.comments = comments; }
		public Double getDebitAmount() { return debitAmount; }
		public void setDebitAmount(Double debitAmount) { this.debitAmount = debitAmount; }
		public Double getCreditAmount() { return creditAmount; }
		public void setCreditAmount(Double creditAmount) { this.creditAmount = creditAmount; }
		public Integer getJobLotId() { return jobLotId; }
		public void setJobLotId(Integer jobLotId) { this.jobLotId = jobLotId; }
		public String getRefInvoiceNo() { return refInvoiceNo; }
		public void setRefInvoiceNo(String refInvoiceNo) { this.refInvoiceNo = refInvoiceNo; }
		public Integer getSupplierCustomerId() { return supplierCustomerId; }
		public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }
		public Integer getCostCenterId() { return costCenterId; }
		public void setCostCenterId(Integer costCenterId) { this.costCenterId = costCenterId; }
		public String getChequeNoDetail() { return chequeNoDetail; }
		public void setChequeNoDetail(String chequeNoDetail) { this.chequeNoDetail = chequeNoDetail; }
		public LocalDate getChequeDateDetail() { return chequeDateDetail; }
		public void setChequeDateDetail(LocalDate chequeDateDetail) { this.chequeDateDetail = chequeDateDetail; }
		public String getPaymentType() { return paymentType; }
		public void setPaymentType(String paymentType) { this.paymentType = paymentType; }
		public String getCommentsOtherLingo() { return commentsOtherLingo; }
		public void setCommentsOtherLingo(String commentsOtherLingo) { this.commentsOtherLingo = commentsOtherLingo; }
		public Integer getPaymentTypeId() { return paymentTypeId; }
		public void setPaymentTypeId(Integer paymentTypeId) { this.paymentTypeId = paymentTypeId; }
		public Integer getInstrumentTypeId() { return instrumentTypeId; }
		public void setInstrumentTypeId(Integer instrumentTypeId) { this.instrumentTypeId = instrumentTypeId; }
		public Integer getChequeTypeId() { return chequeTypeId; }
		public void setChequeTypeId(Integer chequeTypeId) { this.chequeTypeId = chequeTypeId; }
		public String getPayeeTitle() { return payeeTitle; }
		public void setPayeeTitle(String payeeTitle) { this.payeeTitle = payeeTitle; }
		public Integer getReferenceAccountId() { return referenceAccountId; }
		public void setReferenceAccountId(Integer referenceAccountId) { this.referenceAccountId = referenceAccountId; }
		public Integer getOrderNo() { return orderNo; }
		public void setOrderNo(Integer orderNo) { this.orderNo = orderNo; }
		public Integer getDocumentTypeIdRef() { return documentTypeIdRef; }
		public void setDocumentTypeIdRef(Integer documentTypeIdRef) { this.documentTypeIdRef = documentTypeIdRef; }
		public Double getQtyIn() { return qtyIn; }
		public void setQtyIn(Double qtyIn) { this.qtyIn = qtyIn; }
		public Double getQtyOut() { return qtyOut; }
		public void setQtyOut(Double qtyOut) { this.qtyOut = qtyOut; }
		public Double getItemAmount() { return itemAmount; }
		public void setItemAmount(Double itemAmount) { this.itemAmount = itemAmount; }
		public Double getWhtHolding() { return whtHolding; }
		public void setWhtHolding(Double whtHolding) { this.whtHolding = whtHolding; }
		public Integer getLineAgainstAccountId() { return lineAgainstAccountId; }
		public void setLineAgainstAccountId(Integer lineAgainstAccountId) { this.lineAgainstAccountId = lineAgainstAccountId; }
	}

	public Integer getId() { return id; }
	public void setId(Integer id) { this.id = id; }
	public Integer getDocumentTypeId() { return documentTypeId; }
	public void setDocumentTypeId(Integer documentTypeId) { this.documentTypeId = documentTypeId; }
	public Integer getVoucherCode() { return voucherCode; }
	public void setVoucherCode(Integer voucherCode) { this.voucherCode = voucherCode; }
	public LocalDate getVoucherDate() { return voucherDate; }
	public void setVoucherDate(LocalDate voucherDate) { this.voucherDate = voucherDate; }
	public String getRemarks() { return remarks; }
	public void setRemarks(String remarks) { this.remarks = remarks; }
	public Double getVoucherAmount() { return voucherAmount; }
	public void setVoucherAmount(Double voucherAmount) { this.voucherAmount = voucherAmount; }
	public List<VoucherDetailRowDto> getDetails() { return details; }
	public void setDetails(List<VoucherDetailRowDto> details) { this.details = details; }
	public Integer getRefAccountId() { return refAccountId; }
	public void setRefAccountId(Integer refAccountId) { this.refAccountId = refAccountId; }
	public Integer getAgainstAccountId() { return againstAccountId; }
	public void setAgainstAccountId(Integer againstAccountId) { this.againstAccountId = againstAccountId; }
	public String getPayTitle() { return payTitle; }
	public void setPayTitle(String payTitle) { this.payTitle = payTitle; }
	public String getManualBillNo() { return manualBillNo; }
	public void setManualBillNo(String manualBillNo) { this.manualBillNo = manualBillNo; }
	public Double getBillAmount() { return billAmount; }
	public void setBillAmount(Double billAmount) { this.billAmount = billAmount; }
	public Integer getProjectId() { return projectId; }
	public void setProjectId(Integer projectId) { this.projectId = projectId; }
	public Integer getMultiCurrencyId() { return multiCurrencyId; }
	public void setMultiCurrencyId(Integer multiCurrencyId) { this.multiCurrencyId = multiCurrencyId; }
	public Double getExchangeCurrencyRate() { return exchangeCurrencyRate; }
	public void setExchangeCurrencyRate(Double exchangeCurrencyRate) { this.exchangeCurrencyRate = exchangeCurrencyRate; }
	public Double getFcAmount() { return fcAmount; }
	public void setFcAmount(Double fcAmount) { this.fcAmount = fcAmount; }
	public String getRemarksOtherLingo() { return remarksOtherLingo; }
	public void setRemarksOtherLingo(String remarksOtherLingo) { this.remarksOtherLingo = remarksOtherLingo; }
	public Integer getRefDocNoId() { return refDocNoId; }
	public void setRefDocNoId(Integer refDocNoId) { this.refDocNoId = refDocNoId; }
	public Boolean getIncludeWht() { return includeWht; }
	public void setIncludeWht(Boolean includeWht) { this.includeWht = includeWht; }
	public Integer getBranchId() { return branchId; }
	public void setBranchId(Integer branchId) { this.branchId = branchId; }
	public LocalDate getChequeDate() { return chequeDate; }
	public void setChequeDate(LocalDate chequeDate) { this.chequeDate = chequeDate; }
	public Boolean getMirrorCreditToRefAccount() { return mirrorCreditToRefAccount; }
	public void setMirrorCreditToRefAccount(Boolean mirrorCreditToRefAccount) { this.mirrorCreditToRefAccount = mirrorCreditToRefAccount; }
	public Integer getLocationTypeId() { return locationTypeId; }
	public void setLocationTypeId(Integer locationTypeId) { this.locationTypeId = locationTypeId; }
	public Double getTaxAmount() { return taxAmount; }
	public void setTaxAmount(Double taxAmount) { this.taxAmount = taxAmount; }
	public Double getTaxPercent() { return taxPercent; }
	public void setTaxPercent(Double taxPercent) { this.taxPercent = taxPercent; }
	public String getTaxTypeName() { return taxTypeName; }
	public void setTaxTypeName(String taxTypeName) { this.taxTypeName = taxTypeName; }
	public Integer getTaxTypeId() { return taxTypeId; }
	public void setTaxTypeId(Integer taxTypeId) { this.taxTypeId = taxTypeId; }
	public Boolean getPostSingleWhtDebitRow() { return postSingleWhtDebitRow; }
	public void setPostSingleWhtDebitRow(Boolean postSingleWhtDebitRow) { this.postSingleWhtDebitRow = postSingleWhtDebitRow; }
	public String getChequeNo() { return chequeNo; }
	public void setChequeNo(String chequeNo) { this.chequeNo = chequeNo; }
	public Integer getSupplierCustomerId() { return supplierCustomerId; }
	public void setSupplierCustomerId(Integer supplierCustomerId) { this.supplierCustomerId = supplierCustomerId; }
	public LocalDate getDueDate() { return dueDate; }
	public void setDueDate(LocalDate dueDate) { this.dueDate = dueDate; }
	public Integer getDueDays() { return dueDays; }
	public void setDueDays(Integer dueDays) { this.dueDays = dueDays; }
}
