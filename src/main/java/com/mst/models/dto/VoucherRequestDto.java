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
	}
}
