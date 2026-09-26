package com.mst.models.saleinvoice;

import java.lang.annotation.*;

/**
 * The desktop's models for Sale Invoice (95), field for field, generated from
 * recovered_source/projects/architecture.model by /home/claude/si/gen_models.py.
 *
 * GenericProvider.SetProc (DAL 0207:283) sends EVERY non-virtual public property as @Name and skips
 * virtual ones. The fields below keep exactly that split: @NotParam marks the C# virtual properties
 * (display-only, never sent). A null reference value is "not supplied" to the procedure, exactly as
 * AddWithValue(name, null) behaves, so the procedure's own default applies.
 * Every non-virtual field was checked to be a declared parameter of each procedure it is sent to.
 */
public final class SaleInvoiceModels {
    private SaleInvoiceModels() {}
    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.FIELD) public @interface NotParam {}
    @Retention(RetentionPolicy.RUNTIME) @Target(ElementType.FIELD) public @interface Nullable {}

    /** Architecture.Model: Architecture.Model.Inventory.InvSaleInvoice -> Sp_InvSaleInvoice_Insert, Sp_InvSaleInvoice_Update */
    public static class Head {
        public boolean IsApproved;
        public boolean CustomAccounts;
        public java.time.LocalDateTime DocDate;
        public java.time.LocalDateTime EntryDate;
        public java.time.LocalDateTime ModifyDate;
        @Nullable public java.time.LocalDateTime PostDate;
        @Nullable public java.time.LocalDateTime DueDate;
        public java.time.LocalDateTime SupplierInvoiceDate;
        public double BillAmount;
        public double CashReceived;
        public double CommAmount;
        public double CommRate;
        public double FreightAmount;
        public int TransporterDebitGLId;
        public int TransporterDebitPartyId;
        public int TransporterCreditPartyId;
        public int DueDays;
        public int BranchesId;
        public int CommissionAgentId;
        public int CompanyId;
        public int DocNo;
        public int BranchSrNo;
        public int SalesTaxNo;
        public int DocumentTypeId;
        public int BaseDocumentTypeId;
        public int DocumentTypeSrNo;
        public int AutoUpdate;
        public int EntryUser;
        public int Id;
        public int ModifyUser;
        public int OrganizationId;
        public int PostUser;
        public int ProjectsId;
        public int ReferencePartyId;
        public int StockPartyId;
        public int SupplierCustomerId;
        public int SupplierInvoiceNo;
        @Nullable public String SupplierReferenceNo;
        public int FinancialYearId;
        public int TransporterId;
        public int CommissionDebitAcGLId;
        public int PaymentTermId;
        public int TaxAccountId;
        public int DiscountAccountId;
        public java.math.BigDecimal InvoiceQty = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal InvoiceWeight = java.math.BigDecimal.ZERO;
        public int LocationTypeId;
        public int CurrencyId;
        public int TransTypeId;
        public int CustomgroupId;
        public int OtherCategoryId;
        public java.math.BigDecimal ExchangeRate = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal DiscountAmount = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal FcyAmount = java.math.BigDecimal.ZERO;
        @Nullable public String CommissionRemarks;
        @Nullable public String CommissionType;
        @Nullable public String IsAttachments;
        public boolean IsTaxable;
        public boolean IsReserve;
        @Nullable public String ManualBillNo;
        @Nullable public String OtherRemarks;
        @Nullable public String RemarksHeader;
        @Nullable public String UomScheduleIdCmRate;
        @Nullable public String DeliveryTerm;
        @Nullable public String ScreenName;
        @Nullable public String FreightRemark;
        @Nullable public String AttachmentsValues;
        @Nullable public String CustomAttachmentsValues;
        public boolean IsUploaded;
    }

    /** Architecture.Model: Architecture.Model.Inventory.InvSaleInvoiceDetail -> Sp_InvSaleInvoiceDetail_Insert */
    public static class Detail {
        public java.time.LocalDateTime GpDate;
        @Nullable public java.time.LocalDateTime DueDate;
        public double AdLsWeight;
        public double AvgCgsRate;
        public double BillAmount;
        public double CommissionAmount;
        public double EBTotalWt;
        public double EBWeight;
        public double ExpenseAmount;
        public double FreightAmount;
        public double GrossWeight;
        public double ItemAmount;
        public double ItemCgsRate;
        public double ItemQty;
        public double ItemRate;
        public double JournalAmount;
        public double NetBillWeight;
        public double NetStockWeight;
        public double RateCut;
        public double RateCutAmount;
        public double TaxAmount;
        public double TaxPercent;
        public double WeightCut;
        public double WeightCutTotal;
        public double ItemDiscount;
        public double ItemDiscountAmount;
        public java.math.BigDecimal ItemRateWOExp = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal PackingAddLess = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal ExchangeRate = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal FcyAmount = java.math.BigDecimal.ZERO;
        public int CurrencyId;
        public int GpNo;
        public int DiscountTypeId;
        public int Id;
        public int InvGdnDetailId;
        public int InvGdnId;
        public int InvSaleInvoiceId;
        public int ItemId;
        public int ItemUOMId;
        public int ItemVariantId;
        public int CastingTypeId;
        public int JobLotId;
        public int PackingTypeId;
        public int SaleOrderId;
        public int SaleOrderDetailId;
        public int TaxNameId;
        public int UomScheduleIdRate;
        public int WarehouseId;
        public int RackId;
        public int InvForwardingId;
        public int InvForwardingDetailId;
        public int RefRefDocumentTypeId;
        public int RefRefDocIdNo;
        public int RefDocSubId;
        public int LineId;
        public int CityId;
        public int BranchId;
        public int ReserveWareHouse;
        public int PaymentTermId;
        public int BillCalculateTypeId;
        public int DueDays;
        public int CgsRateUomId;
        public int GdnPartyProcessingId;
        public int ProductionStageId;
        public int GdnDetailPartyProcessingId;
        public int CostCenterId;
        public int BrandItemId;
        public int ItemConditionId;
        @Nullable public String CropYear;
        @Nullable public String IsTaxable;
        @Nullable public String LabAnalisysNo;
        @Nullable public String RemarksDetail;
        @Nullable public String TaxDescriptions;
        @Nullable public String VehicleNo;
        @Nullable public String ReferenceNo;
        public boolean CommOnSale;
        public int SecondaryUomId;
        public int ReasonId;
        public java.math.BigDecimal SecondaryUomQty = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal SecondaryUomItemRate = java.math.BigDecimal.ZERO;
        @NotParam public double SecondaryUomEquivalent;
        @NotParam public double PerItemSecondaryUomQty;
        @NotParam @Nullable public String SecondaryUomCode;
        @NotParam public int SaleGLAC;
        @NotParam @Nullable public String ItemCode;
        @NotParam @Nullable public String ItemCondition;
        @NotParam @Nullable public String ItemName;
        @NotParam @Nullable public String BrandItemCode;
        @NotParam @Nullable public String BrandItemName;
        @NotParam @Nullable public String DiscountType;
        @NotParam @Nullable public String BranchName;
        @NotParam @Nullable public String UOMCodeItem;
        @NotParam @Nullable public String UOMCodeRate;
        @NotParam @Nullable public String JobLotDescription;
        @NotParam @Nullable public String PackTypeDesc;
        @NotParam @Nullable public String WareHouseName;
        @NotParam @Nullable public String ReserveWareHouseName;
        @NotParam @Nullable public String CityName;
        @NotParam @Nullable public String TaxName;
        @NotParam public int RefDocNo;
        @NotParam public double RefInvoiceQty;
        @NotParam public int GdnNo;
        @NotParam public int SaleOrder;
        @NotParam public int ForwardingDocNo;
        @NotParam public int OtherCategoryId;
        @NotParam @Nullable public String OtherCategory;
        @NotParam public double PackEquivalent;
        @NotParam public double RateUOM;
        @NotParam public double PackUom;
        @NotParam @Nullable public String RefRefDocumentType;
        @NotParam @Nullable public String PaymentTerm;
        @NotParam @Nullable public String CastingType;
        @NotParam @Nullable public String ProductionProcessStage;
        @NotParam @Nullable public String VariantDescription;
        @NotParam @Nullable public String rackName;
        @NotParam public java.time.LocalDateTime OrderDate;
        @NotParam public double WeightFinishGoods;
        @NotParam public double PurchaseInvoiceQty;
        @NotParam public double PurchaseUsedQty;
        @NotParam public double PurchaseBalanceQty;
    }

    /** Architecture.Model: Architecture.Model.Inventory.InvSaleInvoiceFreight -> Sp_InvSaleInvoiceFreight_Insert */
    public static class Freight {
        public double Debit;
        public double FreightAmount;
        public double FrQty;
        public double FrRate;
        public double FrWeight;
        public int Id;
        public int InvGdnId;
        public int InvSaleInvoiceId;
        public int TansporterId;
        public int TransporterSupCustId;
        @Nullable public String Remarks;
    }

    /** Architecture.Model: Architecture.Model.Inventory.InvSaleInvoiceJournal -> Sp_InvSaleInvoiceJournal_Insert */
    public static class Journal {
        public double JvCredit;
        public double JvDebit;
        public double JvPrcnt;
        public double JvQty;
        public double JvRate;
        public int ChartofAccountId;
        public int TransporterSupCustId;
        public int Id;
        public int InvSaleInvoiceId;
        @Nullable public String JvRemarks;
    }

    /** Architecture.Model: Architecture.Model.Inventory.InvSaleInvoiceExpense -> Sp_InvSaleInvoiceExpense_Insert */
    public static class Expense {
        public double Amount;
        public double Qty;
        public double Rate;
        public int Id;
        public int InvRevExpItemId;
        public int InvSaleInvoiceId;
        @Nullable public String Remarks;
        @Nullable public String CustomRemarks;
        @NotParam @Nullable public String OtherItemName;
    }

    /** Architecture.Model: Architecture.Model.Inventory.InvSaleInvoiceCommission -> Sp_InvSaleInvoiceCommission_Insert */
    public static class Commission {
        public double CommAmount;
        public double CommType;
        public double Rate;
        public double RateUom;
        public int CommissionAgentId;
        public int Id;
        public int InvSaleInvoiceId;
        public int CommDebitSupCustId;
        public int DebitAccountId;
        public int SaleOrderId;
        @Nullable public String Remarks;
        @NotParam public int OrderNo;
    }

    /** Architecture.Model: Architecture.Model.Inventory.SaleInvoicePaymentTermsDetail -> USP_SaleInvoicePaymentTermsDetail_Insert */
    public static class PaymentTerm {
        public java.math.BigDecimal Amount = java.math.BigDecimal.ZERO;
        public java.math.BigDecimal PrcntOfTotal = java.math.BigDecimal.ZERO;
        public java.time.LocalDateTime DueDate;
        public int DueDays;
        public int Id;
        public int InvSaleInvoiceId;
        public int PaymentTermId;
        public int SortNo;
        public int SaleOrderId;
        public boolean SystemGeneratedRow;
        @Nullable public String PaymentRemarks;
        @NotParam public int SaleOrderNo;
        @NotParam @Nullable public String TermsDescription;
    }

    /** Architecture.Model: Architecture.Model.Accounts.VoucherHead -> Sp_VoucherHead_Insert, Sp_VoucherHead_Update, Sp_VoucherHead_H_Insert */
    public static class VoucherHead {
        public boolean IncludeWHT;
        public boolean IsApproved;
        public boolean PostState;
        @Nullable public Boolean InclusiveTax;
        @Nullable public java.time.LocalDateTime ChequeDate;
        @Nullable public java.time.LocalDateTime DueDate;
        public java.time.LocalDateTime EntryDate;
        public java.time.LocalDateTime ModifyDate;
        @Nullable public java.time.LocalDateTime PostDate;
        public java.time.LocalDateTime VoucherDate;
        public double BillAmount;
        public double ExchangeCurrencyRate;
        public double FcAmount;
        public double VoucherAmount;
        public double CostCenterAmount;
        public int AgainstAccountId;
        public int BranchId;
        public int CheqId;
        public int ChequePrintId;
        public int CompanyId;
        public int DocumentTypeId;
        public int DocumentTypeSrNo;
        public int DueDays;
        public int EntryUser;
        public int FinancialYearId;
        public int Id;
        public int ModifyUser;
        public int MultiCurrencyId;
        public int OrganizationId;
        public int PostUser;
        public int ProjectId;
        public int RefAccountId;
        public int RefDocNoId;
        public int VoucherCode;
        public int ActionId;
        @Nullable public String BankBranch;
        @Nullable public String ChequeNo;
        @Nullable public String ConversionFormula;
        @Nullable public String DrCrNoteType;
        @Nullable public String ManualBillNo;
        @Nullable public String PayTitle;
        @Nullable public String Remarks;
        @Nullable public String RemarksOtherLingo;
        @Nullable public String Source;
        @Nullable public String AttachmentsValues;
        public int RefDocumentTypeId;
        public int FixedAssetEntryTypeId;
        @Nullable public String CustomAttachmentsValues;
        public boolean CustomAccounts;
        public int BaseDocumentTypeId;
        public int AdvanceTaxAccountId;
        public double AdvanceTaxAmount;
        public int OtherChargesAccountId;
        public double OtherChargesAmount;
        public boolean IsUploaded;
    }

    /** Architecture.Model: Architecture.Model.Accounts.VoucherDetail -> Sp_VoucherDetail_Insert, Sp_VoucherDetail_H_Insert */
    public static class VoucherDetail {
        @Nullable public java.time.LocalDateTime DCheqDate;
        @Nullable public java.time.LocalDateTime GpDate;
        public double Adjustment;
        public double AdvanceAmount;
        public double Commission;
        public double CreditAmount;
        public double DCurrencyAmount;
        public double DebitAmount;
        public double TaxAmount;
        public double DExchangeCurrencyRate;
        public double Expenses;
        public double ExTax;
        public double Freight;
        public double ItemAmount;
        public double ItemRate;
        public double Journal;
        public double QtyIn;
        public double QtyOut;
        public double RateCut;
        public double RateCutAmount;
        public double SaleTax;
        public double TaxesTotalAmount;
        public double TaxPrcnt;
        public double WeightIn;
        public double WeightOut;
        public double WhtHolding;
        public double ItemCgsRate;
        public double TotalDebitAmount;
        public double TotalCreditAmount;
        public double ThirdCurrencyFcyExchangeRate;
        public double ThirdCurrencyHcyExchangeRate;
        public double ThirdCurrencyAmount;
        public double ThirdCurrencyReceiverExchangeRate;
        public double ThirdCurrencyReceiverFcyAmount;
        public int ThirdCurrencyId;
        public int AccountId;
        public int AgainstAccountId;
        public int DMultiCurrencyId;
        public int DocumentTypeIdRef;
        public int GpNo;
        public int Id;
        public int InvoiceNoRefId;
        public int ItemId;
        public int JobLotId;
        public int OrderNo;
        public int SupplierCustomerId;
        public int EmployeeId;
        public int SubsidiaryTypeId;
        public int SubsidiaryAccountId;
        public int SubsidiaryAgainstTypeId;
        public int SubsidiaryAgainstAccountId;
        public int TaxTypeId;
        public int ActionId;
        public int VoucherHeadId;
        public int RefDocumentTypeId;
        public int RefDocNoId;
        public int RefDocNoDetailId;
        public int RefDocSubIdNo;
        public int LineId;
        public int InstrumentTypeId;
        public int SubNo;
        public int SortNo;
        public int IsCGS;
        @Nullable public String CheqNoDetail;
        @Nullable public String Comments;
        @Nullable public String CommentsOtherLingo;
        @Nullable public String DConversionFormula;
        @Nullable public String IsTaxable;
        @Nullable public String PaymentType;
        @Nullable public String RefInvoiceNo;
        @Nullable public String TaxesRemarks;
        @Nullable public String VehicleNo;
        @Nullable public String PayeeTitle;
        public int PaymentTypeId;
        public int ChequeTypeId;
        public int BranchesId;
        public int CostCenterId;
        public double SBRTaxAmount;
        public double DiscountPercent;
        public double DiscountAmount;
        public int ReferenceAccountId;
        public int LocationTypeId;
        public int BaseFcyId;
        public double BaseFcyExchangeRate;
        public double BaseFcyAmount;
        @NotParam @Nullable public String AccountCode;
        @NotParam @Nullable public String BranchName;
        @NotParam @Nullable public String Location;
        @NotParam @Nullable public String CostCenterName;
        @NotParam @Nullable public String AccountTitle;
        @NotParam @Nullable public String AgainstAccount;
        @NotParam @Nullable public String CheqPartyName;
        @NotParam @Nullable public String JobLotDescription;
        @NotParam @Nullable public String SubsidiaryAccountTitle;
        @NotParam @Nullable public String InstrumentType;
        @NotParam @Nullable public String ReferenceAccount;
        @NotParam public int AccountTypeId;
        @NotParam @Nullable public String BaseFcyCode;
        @NotParam @Nullable public String GlcyCode;
        @NotParam @Nullable public String TcyCode;
    }

    /** Architecture.Model: Architecture.Model.Inventory.InventoryStockEvalautionDetail -> USP_InventoryStockEvalautionDetailGdnReferences_Update */
    public static class StockDetail {
        public boolean IsApproved;
        @Nullable public java.time.LocalDateTime DocDate;
        public double AmountIn;
        public double AmountOut;
        public double BillWeightIn;
        public double BillWeightOut;
        public double CgsRate;
        public double ExpenseAmountIn;
        public double ItemRate;
        public double QtyIn;
        public double QtyOut;
        public double StockWeightIn;
        public double StockWeightOut;
        public double CgsAmount;
        public int BranchesId;
        public int CompanyId;
        public int DocCodeNo;
        public int GpNoDcNo;
        public int Id;
        public int InvPackingTypeId;
        public int ItemId;
        public int ItemUom;
        public int JobLotId;
        public int OrderNo;
        public int OrganizationId;
        public int PrdJobOrderNo;
        public int ProjectsId;
        public int RateUom;
        public int RefDocIdNo;
        public int RefDocSubIdNo;
        public int RefDocumentTypeId;
        public int OtherDocumentTypeId;
        public int OtherDocNoId;
        public int OtherSubDocNoId;
        public int SupplierCustomerId;
        public int WarehouseId;
        public int RefWarehouseId;
        public int CityId;
        public int LineId;
        public int RefRefDocumentTypeId;
        public int RefRefDocIdNo;
        public int RefRefDocSubIdNo;
        public int EntryUser;
        public int ModifyUser;
        public int InvoiceId;
        public int InvoiceDetailId;
        public int VarientId;
        public int ItemConditionId;
        @Nullable public String CalcType;
        @Nullable public String CropBatch;
        @Nullable public String TranRemarks;
        @Nullable public String VehicleNo;
        @Nullable public String BiltyNo;
    }

    /** Architecture.Model: Architecture.Model.DocumentApprovalDetail -> DAW.USp_DocumentApprovalDetail_Insert */
    public static class ApprovalDetail {
        public int OrganizationId;
        public int CompanyId;
        public int DocumentTypeId;
        public int Id;
        public java.math.BigDecimal LimitAmount = java.math.BigDecimal.ZERO;
    }
}
