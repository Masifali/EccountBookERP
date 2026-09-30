package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Desktop save models of the Party Processing screens ported by the "PpC" group:
 * 676 Production Job Order, 677 Production (Input / Output / PM), 675 Processing Bill,
 * 602 Stock Conversion, 603 Stock Transfer, 604 Stock Adjustment, 614 Wages Bill.
 *
 * Each nested class declares EXACTLY the non-virtual properties of the C# model (decompiled
 * architecture.model file named on the class), spelled as the C# property, so
 * {@link DesktopModel#toParams()} binds the same "@Name" list GenericProvider.SetProc binds.
 * Every list was compared with the procedure's parameter list in procdure_index.csv.
 * C# value types start at their CLR default (0 / false); a nullable C# property (DateTime?) is a
 * boxed / nullable Java field and is omitted when null, as AddWithValue(null) omits it.
 * Virtual navigation properties are {@code transient}.
 */
public final class PpCModels {

    private PpCModels() { }

    private static BigDecimal z() { return BigDecimal.ZERO; }

    // ======================================================= model 0421 InvProductionJobOrderPartyProcessing
    /** Sp_InvProductionJobOrderPartyProcessing_Insert / _Update (43 parameters; @RowVersion not in the model). */
    public static class JobOrder extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime EndDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime PlanDate;
        public LocalDateTime StartDate;
        public LocalDateTime ApprovedDate;
        public double FinishGoodRate;
        public int ActionId;
        public int BranchesId;
        public int ByProductacId;
        public int CompanyId;
        public int DocumentTypeId;
        public int EntryUser;
        public int FinishGoodsAcId;
        public int Id;
        public int InvProductionPlantId;
        public int JobLotId;
        public int ModifyUser;
        public int ApprovedUserId;
        public int OrganizationId;
        public int PlanCode;
        public int PlanTypeSrNo;
        public int ProjectsId;
        public int RefSalesOrderId;
        public int ShipmentSeq;
        public int SupplierCustomerId;
        public int StockPartyId;
        public int WipWareHouseId;
        public int WipItemId;
        public int WorkInProccessAcId;
        public String DeliveryInstructions;
        public String LotReference;
        public String OtherInstructions;
        public String PackingInstructions;
        public String PlanStatus;
        public String PlanType;
        public String ProductionInsturction;
        public String ProductionType;
        public String RefInvoiceNo;
        public int FinancialYearId;
        public String ScreenName;
    }

    // ======================================================= model 0403 / 0402 stock adjustment
    /** usp_InvStockAdjustmentPartyProcessing_Insert (21; @EntryDate, @ModifyDate, @ApprovedDate, @UserLogId default). */
    public static class Adjustment extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime DocDate;
        public int ActionId;
        public int AdjustmentTypeId;
        public int ApprovalUserId;
        public int BranchesId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUserId;
        public int FinancialYearId;
        public int Id;
        public int ModifyUserId;
        public int OrganizationId;
        public int ProjectsId;
        public int StockPartyId;
        public String RemarksHeader;
        public transient List<AdjustmentDetail> details = new ArrayList<>();
    }

    /** usp_InvStockAdjustmentDetailPartyProcessing_Insert (16). */
    public static class AdjustmentDetail extends DesktopModel {
        public double NetWeight;
        public double Qty;
        public int ActionTypeId;
        public int CropYearId;
        public int Id;
        public int RefDocumentTypeId;
        public int RefDocNoId;
        public int RefDocSubIdNo;
        public int InvStockAdjustmentId;
        public int ItemId;
        public int JobLotId;
        public int LineId;
        public int PackingTypeId;
        public int PackUomId;
        public int WarehouseId;
        public String RemarksDetail;
    }

    // ======================================================= model 0401 InventoryTransactionsPartyProcessingForFifo
    /** USP_InventoryTransactionsPartyProcessing_Insert (47). */
    public static class InvTransFifo extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public double AddLess;
        public double BillWeightIn;
        public double BillWeightOut;
        public double EbTotal;
        public double EbUnit;
        public double FreightAmount;
        public double GrossWeight;
        public double QtyIn;
        public double QtyOut;
        public double StockWeightIn;
        public double StockWeightOut;
        public int BranchesId;
        public int CityId;
        public int CompanyId;
        public int CropYearId;
        public int DocCodeNo;
        public int EntryUserId;
        public int GpNo;
        public int InvPackingTypeId;
        public int ItemId;
        public int ItemUom;
        public int JobLotId;
        public int JobOrderId;
        public int ModifyUserId;
        public int OrganizationId;
        public int ProjectsId;
        public int RefDocIdNo;
        public int RefDocSubIdNo;
        public int RefDocumentTypeId;
        public int RefRefDocIdNo;
        public int RefRefDocSubIdNo;
        public int RefRefDocumentTypeId;
        public int RefWarehouseId;
        public int StockPartyId;
        public int SupplierCustomerId;
        public int TransporterId;
        public int WarehouseId;
        public long Id;
        public int LineId;
        public String CalcType;
        public String DetailRemarks;
        public String TranRemarks;
        public String VehicleNo;
    }

    // ======================================================= model 0410 InventoryTransactionsPartyProcessing
    /** Sp_InventoryTransactionsPartyProcessing_Insert (4). */
    public static class InvTrans extends DesktopModel {
        public int OrganizationId;
        public int CompanyId;
        public int RefDocumentTypeId;
        public int RefDocIdNo;
    }

    // ======================================================= model 0405 / 0404 stock transfer
    /** Sp_InvStockTransferHeaderPartyProcessing_Insert / _Update (24; @UserLogId default). */
    public static class Transfer extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public double OtherWeight;
        public double WbNetWeight;
        public int ApprovedUserId;
        public int BranchesId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int FinancialYearId;
        public int GatePassId;
        public int Id;
        public int ModifyUser;
        public int NoOfBranches;
        public int OrganizationId;
        public int ProjectsId;
        public int WbTicketId;
        public String RemarksHeader;
        public String TransferType;
        public transient List<TransferDetail> details = new ArrayList<>();
    }

    /** Sp_InvStockTransferDetailPartyProcessing_Insert (27). */
    public static class TransferDetail extends DesktopModel {
        public double AdLsWeight;
        public double EbTotal;
        public double EbUnit;
        public double ExpenseAmount;
        public double GrossWeight;
        public double ItemAmount;
        public double ItemRate;
        public double NetWeight;
        public double Qty;
        public int CropYearId;
        public int FromStockPartyId;
        public int FromWarehouseId;
        public int Id;
        public int InvPackingTypeId;
        public int InvStockTransferHeaderId;
        public int ItemId;
        public int JobLotId;
        public int JobLotIdTo;
        public int LineId;
        public int PackUomId;
        public int RateUomId;
        public int RefDocNoId;
        public int RefDocSubIdNo;
        public int RefDocumentTypeId;
        public int ToStockPartyId;
        public int ToWarehouseId;
        public String RemarksDetail;
    }

    // ======================================================= model 0406 / 0407 / 0408 stock conversion
    /** [USP_InvStockConversionPartyProcessing_Insert] / _Update (23; @UserLogId default). */
    public static class Conversion extends DesktopModel {
        public boolean PostState;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime PostDate;
        public int ActionId;
        public int StockPartyId;
        public int BranchedId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int FinancialYearId;
        public int Id;
        public int ModifyUser;
        public int OrganizationId;
        public int parentCategoryId;
        public int PostUser;
        public int ProjectsId;
        public String DocManualRef;
        public String ProductionNo;
        public String Remarks;
        public transient List<ConversionDetail> details = new ArrayList<>();
        public transient List<ConversionPm> packing = new ArrayList<>();
    }

    /** USP_InvStockConversionPartyProcessingDetail_Insert (23). */
    public static class ConversionDetail extends DesktopModel {
        public double Moisture;
        public double Qty;
        public double Weight;
        public int Id;
        public int InvStockConversionPartyProcessingId;
        public int ItemId;
        public int ItemUomId;
        public int JobLotId;
        public int MoistureSlabId;
        public int PackingtypeId;
        public int PackUnit;
        public int ProjectId;
        public int StockPartyId;
        public int ReferencePartyId;
        public int RefDocumentTypeId;
        public int RefDocNoId;
        public int RefDocSubIdNo;
        public int WarehouseId;
        public int CropYearId;
        public int ActionTypeId;
        public int JobOrderId;
        public String EntryType;
        public String Remarks;
    }

    /** [USP_InvStockConversionPartyProcessingPackingMaterial_Insert] (5). */
    public static class ConversionPm extends DesktopModel {
        public double ItemQty;
        public int Id;
        public int InvStockConversionPartyProcessingId;
        public int ItemId;
        public int WarehouseId;
    }

    // ======================================================= model 0411 / 0412 / 0413 food production
    /** Sp_InvFoodProductionPartyProcessing_Insert / _Update (25; @RowVersion, @UserLogId default). */
    public static class FoodProduction extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public int ApprovedUserId;
        public int BranchId;
        public int FinancialYearId;
        public String ScreenName;
        public int ActionId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int Id;
        public int InvJobOrderId;
        public int ModifyUser;
        public int OrganizationId;
        public int ProjectId;
        public int WIPItemId;
        public String EntryType;
        public String InvJobOrderNo;
        public String MainRemarks;
        public transient List<FoodProductionDetail> details = new ArrayList<>();
    }

    /** Sp_InvFoodProductionPartyProcessingDetail_Insert (24). */
    public static class FoodProductionDetail extends DesktopModel {
        public double Qty;
        public double GrossWeight;
        public double EbUnit;
        public double EbTotal;
        public double Weight;
        public int ActionTypeId;
        public int CropYearId;
        public int Id;
        public int InvFoodProductionPartyProcessingId;
        public int InvJobOrderId;
        public String InvJobOrderNo;
        public int ItemId;
        public int ItemUomId;
        public int JobLotId;
        public int PackingtypeId;
        public int RefDocNoId;
        public int RefDocSubIdNo;
        public int RefDocumentTypeId;
        public int ReferencePartyId;
        public int StockPartyId;
        public int WarehouseId;
        public int ActionId;
        public String EntryType;
        public String Remarks;
    }

    /** Sp_InvFoodProductionPartyProcessingPackingMaterial_Insert / _Update (22; @UserLogId default). */
    public static class FoodProductionPm extends DesktopModel {
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime ApprovedDate;
        public double Qty;
        public boolean IsApproved;
        public int CompanyId;
        public int FinancialYearId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int ApprovedUserId;
        public int Id;
        public int InvJobOrderId;
        public int ItemId;
        public int ModifyUser;
        public int OrganizationId;
        public int WarehouseId;
        public String Remarks;
        public String ScreenName;
        public int ActionId;
    }

    // ======================================================= model 0422..0425 processing bill
    /** USP_InvProductionProcessingBill_Insert / _Update (29; @RowVersion default). */
    public static class ProcessingBill extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime BillDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public BigDecimal Rate = z();
        public BigDecimal Qty = z();
        public BigDecimal Weight = z();
        public BigDecimal Amount = z();
        public BigDecimal TotalBPPurchaseAmt = z();
        public int FinancialYearId;
        public int ApprovedUserId;
        public int BillNo;
        public int BranchesId;
        public int CompanyId;
        public int DocumentTypeId;
        public int EntryUserId;
        public int CreditAccountId;
        public int Id;
        public int InvProductionJobOrderId;
        public int ModifyUserId;
        public int OrganizationId;
        public int ProjectsId;
        public int RateUomId;
        public int StockPartyId;
        public int ActionId;
        public String ScreenName;
        public String RemarksHeader;
        public transient List<ProcessingBillOh> overheads = new ArrayList<>();
        public transient List<ProcessingBillPm> packing = new ArrayList<>();
        public transient List<ProcessingBillOutput> outputs = new ArrayList<>();
    }

    /** USP_InvProductionProcessingBillOH_Insert (8). */
    public static class ProcessingBillOh extends DesktopModel {
        public BigDecimal Amount = z();
        public BigDecimal Qty = z();
        public BigDecimal Rate = z();
        public int AccountId;
        public int Id;
        public int InvProductionProcessingBillId;
        public String OHRemarks;
        public int SortNo;
    }

    /** USP_InvProductionProcessingBillPM_Insert (8). */
    public static class ProcessingBillPm extends DesktopModel {
        public BigDecimal Amount = z();
        public BigDecimal Qty = z();
        public BigDecimal Rate = z();
        public int Id;
        public int InvProductionProcessingBillId;
        public int ItemId;
        public int SortNo;
        public String Remarks;
    }

    /** USP_InvProductionProcessingBillOutPut_Insert (12). */
    public static class ProcessingBillOutput extends DesktopModel {
        public BigDecimal Amount = z();
        public BigDecimal Qty = z();
        public BigDecimal Rate = z();
        public BigDecimal Weight = z();
        public int Id;
        public int InvProductionProcessingBillId;
        public int ItemId;
        public int DebitAccountId;
        public int SortNo;
        public int ItemUomId;
        public int ActionTypeId;
        public String Remarks;
    }

    // ======================================================= model 0883 / 0882 contractor wages
    /** Sp_InvContractorWagesBillHeader_Insert / _Update (27; @ActionId not in the model). */
    public static class WagesHeader extends DesktopModel {
        public boolean IsAproved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public double WeightTotal;
        public double QtyTotal;
        public int ApprovedUserId;
        public int BranchesId;
        public int CompanyId;
        public int FinancialYearId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int Id;
        public int ModifyUser;
        public int OrganizationId;
        public int ProjectsId;
        public int RefDocNo;
        public int RefDocNoId;
        public int RefDocumentTypeId;
        public int StockPartyId;
        public int JobOrderId;
        public int ScaleSlipNo;
        public String OtherRemarks;
        public String RefDocument;
        public transient List<WagesDetail> details = new ArrayList<>();
    }

    /** Sp_InvContractorWagesBillDetail_Insert (31). The virtual display names ride along as transient. */
    public static class WagesDetail extends DesktopModel {
        public LocalDateTime RefDocDate;
        public double BillWeight;
        public double WageRate;
        public double RateAddLess;
        public double WagesAmount;
        public double Weight;
        public double WeightCut;
        public double BillQty;
        public double Qty;
        public double RefDocQty;
        public double RefDocWeight;
        public boolean FreeOfCost;
        public boolean IsCompany;
        public int RefLineId;
        public int ContractorId;
        public int JobOrderId;
        public int RefDocumentTypeId;
        public int Id;
        public int InvConractorWagesAccountsId;
        public int InvContractorWagesBillHeaderId;
        public int InvPackingTypeId;
        public int ItemId;
        public int JobLotId;
        public double PackSize;
        public int WagesTypeId;
        public int WareHouseFromId;
        public int WareHouseToId;
        public int WbTransactionsIdDt;
        public int InvContractorWagesScheduleId;
        public String Crop;
        public String RemarksDetail;
        public transient String WagesAccountName;
        public transient String ItemName;
    }

    // ======================================================= model 1195 / 1191 voucher
    /** Architecture.Model.Accounts.VoucherHead - Sp_VoucherHead_Insert / _Update / _H_Insert. */
    public static class VoucherHead extends DesktopModel {
        public boolean IncludeWHT;
        public boolean IsApproved;
        public boolean PostState;
        public Boolean InclusiveTax;
        public LocalDateTime ChequeDate;
        public LocalDateTime DueDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime PostDate;
        public LocalDateTime VoucherDate;
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
        public String BankBranch;
        public String ChequeNo;
        public String ConversionFormula;
        public String DrCrNoteType;
        public String ManualBillNo;
        public String PayTitle;
        public String Remarks;
        public String RemarksOtherLingo;
        public String Source;
        public String AttachmentsValues;
        public int RefDocumentTypeId;
        public int FixedAssetEntryTypeId;
        public String CustomAttachmentsValues;
        public boolean CustomAccounts;
        public int BaseDocumentTypeId;
        public int AdvanceTaxAccountId;
        public double AdvanceTaxAmount;
        public int OtherChargesAccountId;
        public double OtherChargesAmount;
        public boolean IsUploaded;
        public transient List<VoucherDetail> voucherDetailList = new ArrayList<>();
    }

    /** Architecture.Model.Accounts.VoucherDetail - Sp_VoucherDetail_Insert / _H_Insert. */
    public static class VoucherDetail extends DesktopModel {
        public LocalDateTime DCheqDate;
        public LocalDateTime GpDate;
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
        public String CheqNoDetail;
        public String Comments;
        public String CommentsOtherLingo;
        public String DConversionFormula;
        public String IsTaxable;
        public String PaymentType;
        public String RefInvoiceNo;
        public String TaxesRemarks;
        public String VehicleNo;
        public String PayeeTitle;
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
    }
}
