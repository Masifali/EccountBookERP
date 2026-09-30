package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Desktop save models of the Party Processing transaction screens owned by this port
 * (GRN/GDN PP 687/688, GRN/GDN Store PP 679/680, Gate Pass PP 681/682, GRN For Purchase 613,
 * GDN For Sale 615, Advance Delivery Order PP 875).
 *
 * Each nested class declares EXACTLY the non-virtual properties of the C# model (decompiled
 * architecture.model), spelled as the C# property, so {@link DesktopModel#toParams()} binds the same
 * "@Name" list GenericProvider.SetProc binds. C# value types start at their CLR default (0 / false);
 * a DateTime left null is omitted (the procedure default applies) - the forms always set them.
 */
public final class PpBModels {

    private PpBModels() { }

    // ================================================================= Model 0417 InvGrnPartyProcessing
    public static class GrnPartyProcessing extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime PostDate;
        public double CarriageAmount;
        public double FactoryWeight;
        public double PartyWeight;
        public int ActionId;
        public int FinancialYearId;
        public int BranchesId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int GatePassId;
        public int GpNo;
        public int Id;
        public int ModifyUser;
        public int OrganizationId;
        public int PostUser;
        public int ProjectsId;
        public int StockPartyId;
        public int SupplierCustomerId;
        public int TransporterId;
        public int TradeToCompanyId;
        public String BiltyNo;
        public String RemarksHeader;
        public String VehicleNo;
        public String VehicleType;
        public String ScreenName;
    }

    // ================================================================= Model 0418 InvGrnPartyProcessingDetail
    public static class GrnPartyProcessingDetail extends DesktopModel {
        public double AdLsWeight;
        public double EBWPerUnit;
        public double EBWTotal;
        public double GrossWeight;
        public double ItemQty;
        public double NetBillWeight;
        public double StockWeight;
        public int CityId;
        public int CropYearId;
        public int RefDocumentTypeId;
        public int RefDocNoId;
        public int RefDocSubIdNo;
        public int Id;
        public int InvGrnPartyProcessingId;
        public int ItemId;
        public int ItemUomId;
        public int JobLotId;
        public int PackingTypeId;
        public int WarehouseId;
        public int ActionTypeId;
        public String CommentsDetail;
        public String ContainerNo;
        public String SealNo;
        public int LineId;
        public int DeliveryOrderId;
        public int DeliveryOrderDetailId;
        public int DoDocumentTypeId;
        public int InvoiceDocumentTypeId;
        public int InvoiceId;
        public int InvoiceDetailId;
    }

    // ================================================================= Model 0414 InvGrnEmptyBagsPartyProcessing
    public static class GrnEmptyBagsPartyProcessing extends DesktopModel {
        public double ItemQty;
        public int DocumentTypeId;
        public int Id;
        public int InvGrnPartyProcessingId;
        public int ItemId;
        public int WarehouseId;
        public String Remarks;
    }

    // ================================================================= Model 0410 InventoryTransactionsPartyProcessing
    public static class InventoryTransactionsPartyProcessing extends DesktopModel {
        public int OrganizationId;
        public int CompanyId;
        public int RefDocumentTypeId;
        public int RefDocIdNo;
    }

    // ================================================================= Model 0401 InventoryTransactionsPartyProcessingForFifo
    public static class TransPpFifo extends DesktopModel {
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

    // ================================================================= Model 0416 InvGrnGdnStorePartyProcessing
    public static class GrnGdnStorePartyProcessing extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public int ActionId;
        public int ApprovedUserId;
        public int BranchesId;
        public int FinancialYearId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int Id;
        public int ModifyUser;
        public int OrganizationId;
        public int ProjectsId;
        public int RefDocumentTypeId;
        public int RefDoNo;
        public int RefEntryIdNo;
        public int ReferencePartyId;
        public int StockPartyId;
        public String BiltyNo;
        public String RemarksHeader;
        public String VehicleNo;
        public String ScreenName;
    }

    // ================================================================= Model 0415 InvGrnGdnStoreDetailPartyProcessing
    public static class GrnGdnStoreDetailPartyProcessing extends DesktopModel {
        public BigDecimal ItemQty = BigDecimal.ZERO;
        public int Id;
        public int InvGrnStorePartyProcessingId;
        public int ItemId;
        public int ItemUomId;
        public int WarehouseId;
        public int ActionTypeId;
        public String RemarksSub;
    }

    // ================================================================= Model 0409 GatePassPartyProcessing
    public static class GatePassPartyProcessing extends DesktopModel {
        public boolean IsApproved;
        public boolean PostState;
        public boolean IsWeighable;
        public LocalDateTime EntryDate;
        public LocalDateTime GpDate;
        public LocalDateTime InDateTimeStamp;
        public LocalDateTime ModifyDate;
        public LocalDateTime OutDateTimeStamp;
        public LocalDateTime PostDate;
        public double DifferenceWeight;
        public double FactoryWeight;
        public double Freight;
        public double ItemQty;
        public double SupplierWeight;
        public int CityId;
        public int CompanyId;
        public int BranchesId;
        public int DocumentTypeId;
        public int EntryUser;
        public int GpSrNo;
        public int GpTypeSrNo;
        public int Id;
        public int ActionId;
        public int ModifyUser;
        public int FinancialYearId;
        public int OrganizationId;
        public int PostUser;
        public int StockPartyId;
        public int SupplierCustomerId;
        public int WeighBridgeId;
        public int ItemId;
        public int AdvanceDeliveryOrderId;
        public String BiltyNo;
        public String GatepassType;
        public String OtherRemarks;
        public String Status;
        public String WeighBridgeStatus;
        public String VarietyName;
        public String VehicleNo;
        public String VehicleType;
        public String ScreenName;
    }

    // ================================================================= Model 1026 Inventory.InvGrn
    public static class InvGrn extends DesktopModel {
        public boolean IsApproved;
        public boolean AddWages;
        public boolean ScaleShortWeightApply;
        public boolean SupplierShortWeightApply;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime PostDate;
        public double CarriageAmount;
        public double FreightDeduction;
        public double BiltyFreight;
        public double AdvanceByFactoryFreight;
        public double AdvanceByPartyFreight;
        public double AccessWeight;
        public double FactoryWeight;
        public double OtherCharges;
        public double PartyWeight;
        public double ScaleKart;
        public int BranchesId;
        public int CompanyId;
        public int FinancialYearId;
        public int DocNo;
        public int SupplierDispatchId;
        public int DocumentTypeId;
        public int BaseDocumentTypeId;
        public int EntryUser;
        public int GpNo;
        public int Id;
        public int InwardGatePassId;
        public int ModifyUser;
        public int OrganizationId;
        public int PostUser;
        public int ProjectsId;
        public int ReferencePartyId;
        public int StockPartyId;
        public int SupplierCustomerId;
        public int TransporterId;
        public int TransporterSupCustId;
        public int GhallaMandiId;
        public int BillCalculateTypeId;
        public int GrnTypeId;
        public int ReqestedById;
        public LocalDateTime ReturnableDate;
        public String BiltyNo;
        public String ReferenceDocNo;
        public String RemarksHeader;
        public String SupplierReference;
        public String TransporterDocRef;
        public String VehicleNo;
        public int ActionId;
        public String VehicleType;
        public String DeliveryTerm;
        public String Status;
        public String TransType;
        public String ScreenName;
        public String AttachmentsValues;
        public String CustomAttachmentsValues;
    }

    // ================================================================= Model 1027 Inventory.InvGrnDetail
    public static class InvGrnDetail extends DesktopModel {
        public double AdLsWeight;
        public double EBWPerUnit;
        public double EBWTotal;
        public double EbPurAgainstWeight;
        public double GrossWeight;
        public double SupplierQty;
        public double ItemQty;
        public double NetBillWeight;
        public double StockWeight;
        public double WtCut;
        public double WtCutTotal;
        public double ScaleKart;
        public double ScaleShortWeight;
        public double SupplierShortWeight;
        public double FreightAmount;
        public double StockEbUnit;
        public double StockEbTotal;
        public int Id;
        public int InvGrnId;
        public int ItemId;
        public int ItemUomId;
        public int JobLotId;
        public int PackingTypeId;
        public int RefDocumentTypeId;
        public int RefDocNoId;
        public int RefDocSubIdNo;
        public int PurchaseOrderId;
        public int PurchaseOrderDetailId;
        public int PurchaserOrderNo;
        public int SupplySchedulId;
        public int GatePassInwarDetailId;
        public int WarehouseId;
        public int WareHouseFromId;
        public int InvPurchasedemondId;
        public int PurchaseDemondDetailId;
        public String AreaCity;
        public String CommentsDetail;
        public LocalDateTime PackingDate;
        public LocalDateTime ExpiryDate;
        public int CropYearId;
        public String CropYear;
        public String LabReportRef;
        public int ContractorId;
        public int CityId;
        public int WbTicketId;
        public int LineId;
        public int WeightCutOnId;
        public int LabId;
        public double QtyForWtCut;
        public int GdnId;
        public int GdnDetailId;
        public int GdnDocumentTypeId;
        public int AssetId;
        public int ConditionId;
        public int DeliveryChallanId;
        public int DeliveryChallanDetailId;
        public int ItemConditionId;
        public int RackId;
    }

    // ================================================================= Model 1002 InvgrnDetailEmptyBags
    public static class InvGrnDetailEmptyBags extends DesktopModel {
        public double PurchaseQty;
        public double ReceivedQty;
        public int Id;
        public int InvGrnId;
        public int ItemId;
        public int TypeId;
        public int PurchaseOrderId;
        public int BagsCondition;
        public String Remarks;
    }

    // ================================================================= Model 1048 Inventory.InventoryTransactions
    public static class InventoryTransactions extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime DocDate;
        public double AmountIn;
        public double AmountOut;
        public double BillWeightIn;
        public double BillWeightOut;
        public double ExpenseAmountIn;
        public double ItemRate;
        public double QtyIn;
        public double QtyOut;
        public double StockWeightIn;
        public double StockWeightOut;
        public int BranchesId;
        public int CompanyId;
        public int DocCodeNo;
        public int InvPackingTypeId;
        public int ItemId;
        public int ItemUom;
        public int JobLotId;
        public int OrganizationId;
        public int ProjectsId;
        public int RateUom;
        public int RefDocIdNo;
        public int RefDocSubIdNo;
        public int RefDocumentTypeId;
        public int SupplierCustomerId;
        public int WarehouseId;
        public long Id;
        public String CalcType;
        public String CropBatch;
        public String TranRemarks;
    }

    // ================================================================= Model 1024 Inventory.InvGdn
    public static class InvGdn extends DesktopModel {
        public boolean IsApproved;
        public boolean AddWages;
        public boolean ShortWeightDeductionApply;
        public boolean IsStockReserved;
        public LocalDateTime DocDate;
        public LocalDateTime GPDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public LocalDateTime PostDate;
        public LocalDateTime ReturnableDate;
        public double CarriageAmount;
        public double FactoryWeight;
        public double OtherCharges;
        public double PartyWeight;
        public int BranchesId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int GpNo;
        public int Id;
        public int AutoUpdateId;
        public int ModifyUser;
        public int OrganizationId;
        public int OutwardGatePassId;
        public int PostUser;
        public int ProjectsId;
        public int ReferencePartyId;
        public int StockPartyId;
        public int RequestedByLookUpId;
        public int SupplierCustomerId;
        public int TransporterId;
        public int TransporterSupCustId;
        public int BillCalculateTypeId;
        public String BiltyNo;
        public String ContainerNo;
        public String ContainerNo1;
        public String ReferenceDocNo;
        public String RemarksHeader;
        public String SupplierReference;
        public String TransporterDocRef;
        public String ScreenName;
        public String VehicleNo;
        public String DeliveryTerm;
        public String VehicleType;
        public String AttachmentsValues;
        public String CustomAttachmentsValues;
        public int ActionId;
        public int FinancialYearId;
        public String DriverName;
        public String DriverCNIC;
        public String DriverCellNo;
        public int VehicleTypeId;
        public int OrderId;
        public int OrderTypeId;
        public int AdvanceDeliveryOrderId;
    }

    // ================================================================= Model 1025 Inventory.InvGdnDetail
    public static class InvGdnDetail extends DesktopModel {
        public double AdLsWeight;
        public double EBWPerUnit;
        public double EBWTotal;
        public double EbUnitStock;
        public double EbTotalStock;
        public double GrossWeight;
        public double ItemQty;
        public double NetBillWeight;
        public double StockWeight;
        public double WtCut;
        public double WtCutTotal;
        public double ShortWeight;
        public double ItemRate;
        public double ItemAmount;
        public boolean IsAssetItem;
        public int DeliveryScheduleId;
        public int Id;
        public int InvGdnId;
        public int ItemId;
        public int ItemUomId;
        public int ItemVariantId;
        public int CastingTypeId;
        public int ProductionStageId;
        public int JobLotId;
        public int PackingTypeId;
        public int SaleOrderId;
        public int SaleOrderDetailId;
        public int RefPartyId;
        public int RefDocumentTypeId;
        public int RefDocIdNo;
        public int RefDocSubIdNo;
        public int WarehouseId;
        public int ItemConditionId;
        public int GrnId;
        public int GrnDetailId;
        public int GrnDocumentTypeId;
        public int WareHouseToId;
        public int CityId;
        public int LineId;
        public int DeliveryTypeId;
        public int DepartmentId;
        public int AssetId;
        public int WbTicketId;
        public String AreaCity;
        public String CommentsDetail;
        public String ContainerNo;
        public String CropYear;
        public String LabReportRef;
        public String VehicleNo;
        public String ItemDescription;
        public int GpNo;
        public int RateUomId;
        public int InvDeliveryOrderId;
        public int InvDeliveryOrderDetailId;
        public int ActionTypeId;
        public int CropYearId;
        public int OrderTypeId;
        public int BrandItemId;
        public int RackId;
        public LocalDateTime GpDate;
        public int SecondaryUomId;
        public BigDecimal SecondaryUomQty = BigDecimal.ZERO;
    }

    // ================================================================= Model 1023 InventoryStockEvalautionDetail
    public static class StockEvalautionDetail extends DesktopModel {
        public boolean IsApproved;
        public LocalDateTime DocDate;
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
        public String CalcType;
        public String CropBatch;
        public String TranRemarks;
        public String VehicleNo;
        public String BiltyNo;
    }

    // ================================================================= Model 1006 Inventory.InvDeliveryOrder
    public static class InvDeliveryOrder extends DesktopModel {
        public boolean IsApproved;
        public boolean IsStockReserved;
        public LocalDateTime ApprovedDate;
        public LocalDateTime ExpiryDate;
        public LocalDateTime ReturnableDate;
        public LocalDateTime DocDate;
        public LocalDateTime EntryDate;
        public LocalDateTime ModifyDate;
        public BigDecimal DoTotalQty = BigDecimal.ZERO;
        public int ApprovedUser;
        public int TransporterId;
        public int BranchesId;
        public int ToBranchId;
        public int FromBranchId;
        public int CompanyId;
        public int DocNo;
        public int DocumentTypeId;
        public int EntryUser;
        public int Id;
        public int ModifyUser;
        public int OrganizationId;
        public int ProjectsId;
        public int EximInvoiceId;
        public int FinancialYearId;
        public int ActionId;
        public int LoadingPortId;
        public int SaleTypeId;
        public int DepartmentFromId;
        public int DepartmentToId;
        public int RequestedByLookUpId;
        public int ApprovedByLookUpId;
        public String LoadingInstructions;
        public String DeliveryOrderType;
        public String OtherWeightRemarks;
        public String AccountRemarks;
        public String VehicleNo;
        public String ScreenName;
        public String VehicleType;
        public double GrossWeight;
        public double NetWeight;
        public double PackingWeight;
        public double OtherWeight;
        public String AttachmentsValues;
        public String CustomAttachmentsValues;
    }

    // ================================================================= Model 1000 Inventory.InvDeliveryOrderdetail
    public static class InvDeliveryOrderDetail extends DesktopModel {
        public double DoQty;
        public double DoWeight;
        public double LoadingQty;
        public double LoadingWeight;
        public double PackingWeight;
        public double TotalPackingWeight;
        public double GrossWeight;
        public double StockWeight;
        public double OtherWeight;
        public double InnerQty;
        public double InnerUomId;
        public double InnerEbUnit;
        public double InnerEbTotal;
        public double AccessWtSet;
        public double OuterEbTotal;
        public int Id;
        public int InvDeliveryOrderId;
        public int InvPackingTypeId;
        public int ItemId;
        public int PackUomId;
        public int CastingTypeId;
        public int ItemVariantId;
        public int SaleOrderId;
        public int SaleOrderDetailId;
        public int InvoiceDetailId;
        public int SupplierCustomerId;
        public int WarehouseId;
        public int WareHouseToId;
        public int JobLotId;
        public int ToJobLotId;
        public int RefPartyId;
        public int RefDocumentTypeId;
        public int RefDocIdNo;
        public int RefDocSubIdNo;
        public int CropYearId;
        public int ExImInvoiceId;
        public int ActionTypeId;
        public int BagTypeId;
        public int ContainerId;
        public int DeliveryTypeId;
        public int AssetId;
        public String LoadingRemarks;
        public String ContainerRemarks;
        public String InspectionRemarks;
        public String ItemDiscription;
        public int DeliveryScheduleId;
        public int DeliveryScheduleDetailId;
        public int ThirdPartyAnalysisSubId;
        public int ThirdPartyAnalysisId;
        public boolean IsAssetItem;
    }

    // ================================================================= Model 0046 DocumentApprovalDetail
    public static class DocumentApprovalDetail extends DesktopModel {
        public int OrganizationId;
        public int CompanyId;
        public int DocumentTypeId;
        public int Id;
        public BigDecimal LimitAmount = BigDecimal.ZERO;
    }
}
