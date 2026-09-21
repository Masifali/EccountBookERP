package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 22 "Contra Voucher" — the payload of
 * {@code Architecture.WinApp.Account_Definition.ContraVoucher}, DocumentTypeId 10.
 *
 * ---------------------------------------------------------------------------------------------
 * FIELD ORDER IS THE CONTRACT
 * ---------------------------------------------------------------------------------------------
 * {@code GenericProvider.SetProc} reflects over a model's NON-VIRTUAL properties and sends one
 * parameter per property, so the procedure's parameter list IS the property list. The three
 * classes below mirror, field for field and in declaration order:
 *
 *     Architecture.Model.Accounts.VoucherHead              (model 1195)
 *     Architecture.Model.Accounts.VoucherDetail            (model 1191)
 *     Architecture.Model.Accounts.voucherCostCenterDetail  (model 1190)
 *
 * Every field below was checked against the procedure's own declared parameters, read out of
 * GoldenAceDb(0509)t.sql: Sp_VoucherHead_Insert / _Update / _H_Insert accept all 55 header
 * fields, Sp_VoucherDetail_Insert / _H_Insert accept all 84 detail fields, and
 * USP_voucherCostCenterDetail_Insert accepts all 8 cost-centre fields. Nothing here is a guess.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT IS **NOT** HERE: Type
 * ---------------------------------------------------------------------------------------------
 * {@code VoucherHead.Type} is declared {@code virtual} on the model (line 8), so SetProc SKIPS
 * it and the database never receives it. BLL 0654 still computes it
 * ({@code DocumentTypeId == 10 → VoucherType.Contra}) for its own branching, but adding a
 * {@code @Type} parameter here would send one the procedures do not declare.
 *
 * ---------------------------------------------------------------------------------------------
 * ONE GRID ROW BECOMES TWO DETAIL LINES
 * ---------------------------------------------------------------------------------------------
 * The client posts {@link Row}s — what the operator actually typed. The service turns each into
 * the debit/credit pair the desktop builds at Insert():598-635. The client never posts
 * {@link Detail} objects directly: letting a caller choose which side of a ledger entry is
 * debited is not something this API will accept.
 */
public class ContraVoucherDto {

    // ======================================================== what the client actually sends

    /** 0 for a new voucher; the existing id to update. */
    public Integer Id = 0;

    public Integer VoucherCode = 0;
    public String  VoucherDate;                 // "yyyy-MM-dd"
    public Integer ProjectId = 0;               // CmbProjectId — the header "Cost Center"
    public Integer RefAccountId = 0;            // CmbCreditAccount
    public String  ChequeDate;
    public String  ChequeNo;
    public Integer CheqId = 0;                  // CmbCheqNo
    public String  PayTitle;
    public String  Remarks;                     // txtremarksmain
    public Integer MultiCurrencyId = 0;
    public Double  ExchangeCurrencyRate = 0d;
    public Double  FcAmount = 0d;

    /** The grid, one entry per row the operator added. */
    public List<Row> rows = new ArrayList<>();

    /**
     * Set only when the operator has already been shown the duplicate-voucher warning and chose
     * to continue. It is not a way to skip the check: the server still runs it, and only stops
     * asking once this says the operator answered Yes.
     */
    public Boolean duplicateAcknowledged = Boolean.FALSE;

    /**
     * Likewise for the negative-balance WARNING (DisplayWarningforNegativeBalance). It has no
     * effect when the company's configuration REFUSES the entry outright
     * (PreventNegativeBalanceEntry), which is not a question the operator gets to answer.
     */
    public Boolean negativeBalanceAcknowledged = Boolean.FALSE;

    /** One row of {@code grd} — Add_Click_1():441 builds exactly these nine cells. */
    public static class Row {
        public Integer AccountId = 0;           // CmbDebitAccount
        public Integer JobLotId = 0;            // CmbJobLot
        public String  Remarks;                 // txtremarks
        public Double  Amount = 0d;             // txtamount
        public Double  FcyAmount = 0d;          // Amount / ExchangeRate, computed by the page
        public Integer BranchId = 0;            // cmbBranchName, 0 when the Branch feature is off
        public Integer CostCenterId = 0;        // CmbCostCenter
        /** Display only — the account's caption, used when composing auto-remarks. */
        public String  AccountTitle;
    }

    // ============================================== the shapes the writer sends to the procedures

    /**
     * {@code Architecture.Model.Accounts.VoucherHead}, non-virtual properties in declaration
     * order. Built by the service; never accepted from a request.
     */
    public static class Head {
        public Boolean IncludeWHT              = Boolean.FALSE;
        public Boolean IsApproved              = Boolean.FALSE;
        public Boolean PostState               = Boolean.FALSE;
        public Boolean InclusiveTax;
        public String  ChequeDate;
        public String  DueDate;
        public String  EntryDate;
        public String  ModifyDate;
        public String  PostDate;
        public String  VoucherDate;
        public Double  BillAmount              = 0d;
        public Double  ExchangeCurrencyRate    = 0d;
        public Double  FcAmount                = 0d;
        public Double  VoucherAmount           = 0d;
        public Double  CostCenterAmount        = 0d;
        public Integer AgainstAccountId        = 0;
        public Integer BranchId                = 0;
        public Integer CheqId                  = 0;
        public Integer ChequePrintId           = 0;
        public Integer CompanyId               = 0;
        public Integer DocumentTypeId          = 0;
        public Integer DocumentTypeSrNo        = 0;
        public Integer DueDays                 = 0;
        public Integer EntryUser               = 0;
        public Integer FinancialYearId         = 0;
        public Integer Id                      = 0;
        public Integer ModifyUser              = 0;
        public Integer MultiCurrencyId         = 0;
        public Integer OrganizationId          = 0;
        public Integer PostUser                = 0;
        public Integer ProjectId               = 0;
        public Integer RefAccountId            = 0;
        public Integer RefDocNoId              = 0;
        public Integer VoucherCode             = 0;
        public Integer ActionId                = 0;     // 1 insert, 2 update — BLL 0654
        public String  BankBranch;
        public String  ChequeNo;
        public String  ConversionFormula;
        public String  DrCrNoteType;
        public String  ManualBillNo;
        public String  PayTitle;
        public String  Remarks;
        public String  RemarksOtherLingo;
        public String  Source;
        public String  AttachmentsValues;
        public Integer RefDocumentTypeId       = 0;
        public Integer FixedAssetEntryTypeId   = 0;
        public String  CustomAttachmentsValues;
        public Boolean CustomAccounts          = Boolean.FALSE;
        public Integer BaseDocumentTypeId      = 0;
        public Integer AdvanceTaxAccountId     = 0;
        public Double  AdvanceTaxAmount        = 0d;
        public Integer OtherChargesAccountId   = 0;
        public Double  OtherChargesAmount      = 0d;
        public Boolean IsUploaded              = Boolean.FALSE;
    }

    /**
     * {@code Architecture.Model.Accounts.VoucherDetail}, non-virtual properties in declaration
     * order. Contra fills a small subset; the rest stay at the model's own defaults, exactly as
     * a freshly constructed VoucherDetail does on the desktop.
     */
    public static class Detail {
        public String  DCheqDate;
        public String  GpDate;
        public Double  Adjustment                        = 0d;
        public Double  AdvanceAmount                     = 0d;
        public Double  Commission                        = 0d;
        public Double  CreditAmount                      = 0d;
        public Double  DCurrencyAmount                   = 0d;
        public Double  DebitAmount                       = 0d;
        public Double  TaxAmount                         = 0d;
        public Double  DExchangeCurrencyRate             = 0d;
        public Double  Expenses                          = 0d;
        public Double  ExTax                             = 0d;
        public Double  Freight                           = 0d;
        public Double  ItemAmount                        = 0d;
        public Double  ItemRate                          = 0d;
        public Double  Journal                           = 0d;
        public Double  QtyIn                             = 0d;
        public Double  QtyOut                            = 0d;
        public Double  RateCut                           = 0d;
        public Double  RateCutAmount                     = 0d;
        public Double  SaleTax                           = 0d;
        public Double  TaxesTotalAmount                  = 0d;
        public Double  TaxPrcnt                          = 0d;
        public Double  WeightIn                          = 0d;
        public Double  WeightOut                         = 0d;
        public Double  WhtHolding                        = 0d;
        public Double  ItemCgsRate                       = 0d;
        public Double  TotalDebitAmount                  = 0d;
        public Double  TotalCreditAmount                 = 0d;
        public Double  ThirdCurrencyFcyExchangeRate      = 0d;
        public Double  ThirdCurrencyHcyExchangeRate      = 0d;
        public Double  ThirdCurrencyAmount               = 0d;
        public Double  ThirdCurrencyReceiverExchangeRate = 0d;
        public Double  ThirdCurrencyReceiverFcyAmount    = 0d;
        public Integer ThirdCurrencyId                   = 0;
        public Integer AccountId                         = 0;
        public Integer AgainstAccountId                  = 0;
        public Integer DMultiCurrencyId                  = 0;
        public Integer DocumentTypeIdRef                 = 0;
        public Integer GpNo                              = 0;
        public Integer Id                                = 0;
        public Integer InvoiceNoRefId                    = 0;
        public Integer ItemId                            = 0;
        public Integer JobLotId                          = 0;
        public Integer OrderNo                           = 0;
        public Integer SupplierCustomerId                = 0;
        public Integer EmployeeId                        = 0;
        public Integer SubsidiaryTypeId                  = 0;
        public Integer SubsidiaryAccountId               = 0;
        public Integer SubsidiaryAgainstTypeId           = 0;
        public Integer SubsidiaryAgainstAccountId        = 0;
        public Integer TaxTypeId                         = 0;
        public Integer ActionId                          = 0;
        public Integer VoucherHeadId                     = 0;
        public Integer RefDocumentTypeId                 = 0;
        public Integer RefDocNoId                        = 0;
        public Integer RefDocNoDetailId                  = 0;
        public Integer RefDocSubIdNo                     = 0;
        public Integer LineId                            = 0;
        public Integer InstrumentTypeId                  = 0;
        public Integer SubNo                             = 0;
        public Integer SortNo                            = 0;
        public Integer IsCGS                             = 0;
        public String  CheqNoDetail;
        public String  Comments;
        public String  CommentsOtherLingo;
        public String  DConversionFormula;
        public String  IsTaxable;
        public String  PaymentType;
        public String  RefInvoiceNo;
        public String  TaxesRemarks;
        public String  VehicleNo;
        public String  PayeeTitle;
        public Integer PaymentTypeId                     = 0;
        public Integer ChequeTypeId                      = 0;
        public Integer BranchesId                        = 0;
        public Integer CostCenterId                      = 0;
        public Double  SBRTaxAmount                      = 0d;
        public Double  DiscountPercent                   = 0d;
        public Double  DiscountAmount                    = 0d;
        public Integer ReferenceAccountId                = 0;
        public Integer LocationTypeId                    = 0;
        public Integer BaseFcyId                         = 0;
        public Double  BaseFcyExchangeRate               = 0d;
        public Double  BaseFcyAmount                     = 0d;
    }

    /**
     * {@code Architecture.Model.Accounts.voucherCostCenterDetail}, non-virtual properties in
     * declaration order. {@code ActionTypeId} is left 0, as the desktop leaves it —
     * USP_voucherCostCenterDetail_Insert declares it nullable and simply inserts it.
     */
    public static class CostCentre {
        public java.math.BigDecimal costPrcent = java.math.BigDecimal.ZERO;
        public Double  costAmount      = 0d;
        public Integer ActionTypeId    = 0;
        public Integer CostCenterId    = 0;
        public Integer Id              = 0;
        public Integer SortNo          = 0;
        public Integer VoucherDetailId = 0;
        public Integer VoucherHeaderId = 0;
    }

    /**
     * {@code Architecture.Model.DocumentApprovalDetail} (model 0046) — the five fields DAL 0586
     * sets before calling {@code [DAW].[USp_DocumentApprovalDetail_Insert]}.
     */
    public static class ApprovalDetail {
        public Integer OrganizationId = 0;
        public Integer CompanyId      = 0;
        public Integer DocumentTypeId = 0;
        public Integer Id             = 0;
        public java.math.BigDecimal LimitAmount = java.math.BigDecimal.ZERO;
    }
}
