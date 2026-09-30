package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpBModels;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpBSupport.*;

/**
 * BLL of Architecture.WinApp.PartyProcessing.PartyProGrnAndGdn (one form, two ScreenDefinition rows):
 *
 *   687  Tag "GoodsReceivingNotesPartyProcessing"  DocumentTypeId 49, gate pass type 54  (?mode=grn)
 *   688  Tag "GoodsDispatchNotesPartyProcessing"   DocumentTypeId 89, gate pass type 55  (?mode=gdn)
 *
 * Save: InvGrnPartyProcessing.Save (BLL 0303: insert ActionId 1 / ModifyUser 0, update ActionId 2 / EntryUser 0) -> DAL 0306 SetData,
 * one transaction: Sp_InvGrnPartyProcessing_Insert / _Update -> Sp_InvGrnPartyProcessingDetail_Insert per row (LineId 1..n, removed rows
 * first with ActionTypeId 3) -> Sp_InvGrnEmptyBagsPartyProcessing_Insert per row -> for 89 without loader rows the party processing FIFO
 * (USP_GetStockByFifoMethodPartyProcessing, USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId on update,
 * USP_InventoryTransactionsPartyProcessing_Insert) / for 89 with loader rows and for 49 Sp_InventoryTransactionsPartyProcessing_Insert ->
 * for 89 USP_InventoryValidationPartyProcessing per detail row.
 */
@Service
public class PpBGrnGdnService {

    public static final int SCREEN_GRN = 687;
    public static final int SCREEN_GDN = 688;
    private static final String PROC = "Sp_InvGrnPartyProcessing_GetAllMethod";
    private static final String GP_PROC = "Sp_GatePassPartyProcessing_GetAllMethod";

    @Autowired private PpBSupport pp;

    static final class Mode {
        final int screen, doc, gpDoc;
        final String name;
        Mode(String m) {
            boolean gdn = "gdn".equalsIgnoreCase(str(m));
            screen = gdn ? SCREEN_GDN : SCREEN_GRN;
            doc = gdn ? 89 : 49;
            gpDoc = gdn ? 55 : 54;
            name = gdn ? "GoodsDispatchNotesPartyProcessing" : "GoodsReceivingNotesPartyProcessing";
        }
    }

    // ------------------------------------------------------------------ Load

    /**
     * InvFrmGRN_Load: configs (DefaultDaysToLessFromHistoryFromDate, AllowMultipleOrPartialInvoicingForADO,
     * PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance - 10 when 0), rights (btnSave Save, btnUpdate Update,
     * printToolStripButton / btnGdn339 Print, btnDelete Delete), GenerateCode, the combos, Gatepass(), HistoryCombosFill().
     */
    public Map<String, Object> setup(String mode) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        Map<String, Object> out = lists(u);
        out.put("rights", pp.rights(u, m.screen));
        out.put("documentTypeId", m.doc);
        out.put("defaultDays", toInt(pp.lookups().config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("allowMultipleAdo", allowMultiple(u));
        out.put("tolerance", tolerance(u));
        out.put("docNo", docNo(u, m));
        out.put("pending", pending(u, m));
        out.put("history", historyCombos(u, m));
        return out;
    }

    private boolean allowMultiple(UserAccount u) { return toBool(pp.lookups().config(u, "AllowMultipleOrPartialInvoicingForADO")); }

    private BigDecimal tolerance(UserAccount u) {
        BigDecimal t = toDec(pp.lookups().config(u, "PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance"));
        return t.signum() == 0 ? BigDecimal.TEN : t;
    }

    /** toolStripButton1_Click (Refresh): StockPartyBind, ReferencePartyBind, VehicleTypesFill, TradeToCompanyFill, WarehouseFill,
     *  ItemNameFill, CropYear, JobLotFillWithoutOrder, PackingTypeFill, CityFill. */
    public Map<String, Object> refresh(String mode) {
        Mode m = new Mode(mode);
        return lists(pp.user(m.screen));
    }

    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("stockParties", pick(pp.lookups().stockParties(u), "Id", "CompanyName"));
        out.put("refParties", pick(pp.lookups().referenceParties(u), "Id", "ReferencePartyName"));      // CmbRefParty and CmbTransport
        out.put("tradeToCompany", pick(pp.lookups().lookupsByType(u, 17), "Id", "LookupName"));
        out.put("vehicleTypes", pick(pp.lookups().vehicleTypes(), "Id", "VehicleDescription"));
        out.put("cropYears", pick(pp.lookups().cropYears(u), "Id", "CropYear"));
        out.put("jobLots", pick(pp.lookups().jobLotsPartyProcessing(u), "Id", "JobLotDescription"));
        out.put("packingTypes", pick(pp.lookups().packingTypes(), "Id", "PackTypeDesc"));
        out.put("warehouses", pick(pp.lookups().warehouses(u), "Id", "WareHouseName"));
        out.put("cities", pick(pp.lookups().cities(u), "Id", "CityName"));
        out.put("items", pick(pp.lookups().itemsForPartyProcessing(u, "1,2,4"), "Id", "ItemName"));
        out.put("emptyBagItems", pick(pp.lookups().itemPartyProcessing(u, "7"), "Id", "ItemName"));     // grdEmptyBagsSettings
        return out;
    }

    /** GenerateCode(): BLL 0303 GenerateCode (@Org @Company @FinancialYearId @DocumentTypeId 'GenerateCode') -> DocNo. */
    private int docNo(UserAccount u, Mode m) {
        List<Map<String, Object>> r = pp.db().rows(PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.hrm().financialYearId(), "DocumentTypeId", m.doc, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    public Map<String, Object> code(String mode) {
        Mode m = new Mode(mode);
        return map("docNo", docNo(pp.user(m.screen), m));
    }

    /** PackUOMFillWithoutOrder: UOMSchedule.SearchByObject(ItemId) -> Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uoms(String mode, int itemId) {
        Mode m = new Mode(mode);
        return pick(pp.lookups().uoms(pp.user(m.screen), itemId), "Id", "UOMCode", "Equivalent");
    }

    // ------------------------------------------------------------------ gate pass

    /** Gatepass(): BLL 0298 GpNoPandingforInvGrnPartyProcessing (@Org @Company @DocumentTypeId 54 / 55 @FinancialYearId). */
    private List<Map<String, Object>> pending(UserAccount u, Mode m) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows(GP_PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", m.gpDoc, "FinancialYearId", pp.hrm().financialYearId(), "Activity", "GpNoPandingforInvGrnPartyProcessing")) {
            out.add(map("Id", col(r, "Id"), "GatepassType", col(r, "GatepassType"), "GpNo", col(r, "GpSrNo"), "GpDate", col(r, "GpDate"),
                    "Status", col(r, "Status"), "StockPartyId", col(r, "StockPartyId"), "StockParty", col(r, "StockPartyName"),
                    "RefPartyId", col(r, "RefPartyId"), "ReferenceParty", col(r, "ReferencePartyName"), "VehicleNo", col(r, "VehicleNo"),
                    "BiltyNo", col(r, "BiltyNo"), "ItemName", col(r, "VarietyName"), "ItemQty", col(r, "ItemQty"),
                    "SupplierWeight", col(r, "SupplierWeight"), "FactoryWeight", col(r, "FactoryWeight")));
        }
        return out;
    }

    public List<Map<String, Object>> pending(String mode) {
        Mode m = new Mode(mode);
        return pending(pp.user(m.screen), m);
    }

    /** BLL 0298 ReadByGpNoForInvGrnPartyProcessing (@Org @Company @DocumentTypeId, @FinancialYearId / @GpSrNo / @Id when not 0). */
    private List<Map<String, Object>> gatePassRows(UserAccount u, Mode m, int gpSrNo, int id) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", m.gpDoc);
        p.put("FinancialYearId", nz0(pp.hrm().financialYearId()));
        p.put("GpSrNo", nz0(gpSrNo));
        p.put("Id", nz0(id));
        p.put("Activity", "ReadByGpNoForInvGrnPartyProcessing");
        return pp.db().rows(GP_PROC, p);
    }

    /**
     * GatePassRecordFill(): the gate pass by its number (combgatepass text); for a GDN whose gate pass type is "AdvanceDO" also
     * GridDetailFillFromAdvanceDeliveryOrderData(CmbStockParty.Value, GPID): [dbo].[USP_InvDeliveryOrder_GetDataForGdnPP]
     * (@Org @Company @FinancialYearId @BranchesId @StockPartyId @GpId).
     */
    public Map<String, Object> gatePass(String mode, int gpNo, int gpId, int stockPartyId) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        List<Map<String, Object>> r = gatePassRows(u, m, gpNo, 0);
        Map<String, Object> out = new LinkedHashMap<>();
        if (r.isEmpty()) { out.put("found", false); return out; }
        out.put("found", true);
        out.put("gp", pick(r, "Id", "GpSrNo", "GatepassType", "VehicleType", "VehicleNo", "BiltyNo", "OtherRemarks", "Freight", "ItemQty",
                "SupplierWeight", "FactoryWeight", "DifferenceWeight", "CityId", "VarietyName", "StockPartyId").get(0));
        if (m.doc == 89 && "AdvanceDO".equals(str(col(r.get(0), "GatepassType")))) out.put("ado", adoRows(u, stockPartyId, gpId));
        return out;
    }

    private List<Map<String, Object>> adoRows(UserAccount u, int stockPartyId, int gpId) {
        return pp.db().rows("[dbo].[USP_InvDeliveryOrder_GetDataForGdnPP]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.hrm().financialYearId(), "BranchesId", branch(u), "StockPartyId", stockPartyId, "GpId", gpId);
    }

    // ------------------------------------------------------------------ loader popup (LoadavailableTransactionsForIssuancePartyProcessing)

    /** ComboFill(): the loader's combos; FromDate = ActiveYr.Start_Period. */
    public Map<String, Object> loaderSetup(String mode) {
        Mode m = new Mode(mode);
        return pp.loaderSetup(pp.user(m.screen));
    }

    /** PendingInventoryTransactionsForIssuanceLoad(): BLL 0574 GetAvailableTransactionsForIssuanceForPartyProcessing. */
    public List<Map<String, Object>> loader(String mode, Map<String, Object> b) {
        Mode m = new Mode(mode);
        return pp.loaderRows(pp.user(m.screen), b);
    }

    // ------------------------------------------------------------------ history

    /** HistoryCombosFill(): BLL 0303 GetDataForDropDownFromGrn(@Org @Company @DocumentTypeIds "49" / "89") -> StockParty / ReferenceParty. */
    private Map<String, Object> historyCombos(UserAccount u, Mode m) {
        List<Map<String, Object>> stock = new ArrayList<>(), ref = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows("[dbo].[USP_GetDataForDropDownFromGrnPartyProcessing]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocumentTypeIds", String.valueOf(m.doc))) {
            String a = str(col(r, "Activity"));
            if ("StockParty".equals(a)) stock.add(map("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
            else if ("ReferenceParty".equals(a)) ref.add(map("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
        }
        return map("stockParties", stock, "refParties", ref);
    }

    public Map<String, Object> historyCombos(String mode) {
        Mode m = new Mode(mode);
        return historyCombos(pp.user(m.screen), m);
    }

    /**
     * HistoryGridFill(): BLL 0303 FormHistory -> [dbo].[USP_InvGrnPartyProcessing_FormHistory]: @BranchesId, @CanViewAllRecord and
     * @EntryUser when the user cannot view all, the date pair by the radio (doc / entry / modify), doc no range, stock / reference party,
     * @ActionId 1 (Reffered) / 2 (Not Reffered) - the radios are only visible on the GRN.
     */
    public List<Map<String, Object>> history(String mode, Map<String, Object> b) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        boolean all = pp.canViewAll(u, m.screen);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", m.doc);
        p.put("FinancialYearId", pp.hrm().financialYearId());
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", u.getId());
        p.put("BranchesId", nz0(branch(u)));
        dateFilter(p, b, "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", null, null);
        p.put("DocNoFrom", nz0(toInt(b.get("fromDocNo"))));
        p.put("DocNoTo", nz0(toInt(b.get("toDocNo"))));
        p.put("StockPartyId", nz0(toInt(b.get("stockPartyId"))));
        p.put("ReferencePartyId", nz0(toInt(b.get("refPartyId"))));
        if (m.doc == 49) p.put("ActionId", nz0(toInt(b.get("actionId"))));
        return pick(pp.db().rows("[dbo].[USP_InvGrnPartyProcessing_FormHistory]", p), "Id", "DocumentTypeId", "DocDate", "DocNo", "GatePassId",
                "GpNo", "StockPartyName", "ReferencePartyName", "TradeToCompany", "TransporterName", "Freight=CarriageAmount", "VehicleType",
                "VehicleNo", "BiltyNo", "PartyWeight", "FactoryWeight", "EntryDate", "EntryUser=EntryUserName", "ModifyDate",
                "ModifyUser=ModifyUserName", "NoOfAttachments", "RemarksHeader");
    }

    // ------------------------------------------------------------------ read

    /** ReadById / DetailGridBind: BLL 0303 GetByID -> 'ReadByID' + DAL GetDate 'ReadDetailByHeaderId' + 'ReadEmptyBagsByHeaderId'. */
    public Map<String, Object> byId(String mode, int id) {
        Mode m = new Mode(mode);
        return read(pp.user(m.screen), m, id);
    }

    private Map<String, Object> read(UserAccount u, Mode m, int id) {
        List<Map<String, Object>> h = pp.db().rows(PROC, "Id", id, "Activity", "ReadByID");
        if (h.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = h.get(0);
        if (toInt(col(r, "CompanyId")) != u.getCompanyId() || toInt(col(r, "OrganizationId")) != u.getOrganizationId()
                || toInt(col(r, "DocumentTypeId")) != m.doc) throw invalid("Record not found");
        Map<String, Object> out = pick(h, "Id", "DocNo", "DocDate", "StockPartyId", "StockPartyName", "SupplierCustomerId", "TransporterId",
                "CarriageAmount", "PartyWeight", "FactoryWeight", "GpNo", "VehicleType", "VehicleNo", "BiltyNo", "RemarksHeader", "GatePassId",
                "TradeToCompanyId").get(0);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : pp.db().rows(PROC, "Id", id, "Activity", "ReadDetailByHeaderId")) {    // FilldtDetailFromListCommonForReadById
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", col(d, "Id"));
            x.put("RefDocumentTypeId", col(d, "RefDocumentTypeId"));
            x.put("RefDocIdNo", col(d, "RefDocNoId"));
            x.put("RefDocSubIdNo", col(d, "RefDocSubIdNo"));
            x.put("DoDocumentTypeId", col(d, "DoDocumentTypeId"));
            x.put("DeliveryOrderId", col(d, "DeliveryOrderId"));
            x.put("DeliveryOrderDetailId", col(d, "DeliveryOrderDetailId"));
            x.put("DeliveryOrderNo", col(d, "DeliveryOrderNo"));
            x.put("InvoiceDocumentTypeId", col(d, "InvoiceDocumentTypeId"));
            x.put("InvoiceId", col(d, "InvoiceId"));
            x.put("InvoiceDetailId", col(d, "InvoiceDetailId"));
            x.put("InvoiceDocType", col(d, "InvoiceDocumentType"));
            x.put("InvoiceNo", col(d, "InvoiceDocNo"));
            x.put("InvoiceManualBillNo", col(d, "InvoiceManualBillNo"));
            x.put("WareHouseId", col(d, "WarehouseId"));
            x.put("ItemId", col(d, "ItemId"));
            x.put("Item", col(d, "ItemName"));
            x.put("CropYearId", col(d, "CropYearId"));
            x.put("JobId", col(d, "JobLotId"));
            x.put("PackingTypeId", col(d, "PackingTypeId"));
            x.put("PackUomId", col(d, "ItemUomId"));
            x.put("PackUom", col(d, "UOMCode"));
            x.put("Equivalent", col(d, "Equivalent"));
            x.put("Qty", col(d, "ItemQty"));
            x.put("GrossWight", col(d, "GrossWeight"));
            x.put("EbUnit", col(d, "EBWPerUnit"));
            x.put("EbTotal", col(d, "EBWTotal"));
            x.put("AddLesswt", col(d, "AdLsWeight"));
            x.put("NetBillWeight", col(d, "NetBillWeight"));
            x.put("StockWeight", col(d, "StockWeight"));
            x.put("BalWeight", col(d, "NetBillWeight"));
            x.put("CityId", col(d, "CityId"));
            x.put("Comments", col(d, "CommentsDetail"));
            x.put("ContainerNo", col(d, "ContainerNo"));
            x.put("SealNo", col(d, "SealNo"));
            x.put("InvoiceQty", col(d, "InvoiceQty"));
            x.put("InvoiceWeight", col(d, "InvoiceWeight"));
            x.put("InvoiceEbUnit", col(d, "InvoiceEbUnit"));
            x.put("InvoiceEbTotal", col(d, "InvoiceEbTotal"));
            x.put("WareHouseName", col(d, "WareHouseName"));
            x.put("CropYear", col(d, "CropYear"));
            x.put("JobLot", col(d, "JobLotDescription"));
            x.put("PackingType", col(d, "PackTypeDesc"));
            x.put("CityName", col(d, "CityName"));
            det.add(x);
        }
        out.put("details", det);
        out.put("emptyBags", pick(pp.db().rows(PROC, "Id", id, "Activity", "ReadEmptyBagsByHeaderId"), "ItemId", "Qty=ItemQty", "Remarks"));
        return out;
    }

    // ------------------------------------------------------------------ save

    /**
     * Insert(): FormValidation(), "Grid Record Not Found", the DO GpQty check, the per-row "... in Detail Grid (Row No : n )" checks,
     * the gross / factory weight check (tolerance for a GDN against an Advance DO), the empty bags check, then Save and the DAL
     * transaction. The confirm boxes run on the page.
     */
    public Map<String, Object> save(String mode, Map<String, Object> b) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.hrm().require(u, m.screen, recId > 0 ? "Update" : "Save");
        Map<String, Object> existing = recId > 0 ? read(u, m, recId) : null;

        // FormValidation()
        String docNoText = trim(b.get("docNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw invalid("DocNo Field is Required");
        int stockPartyId = toInt(b.get("stockPartyId"));
        if (stockPartyId <= 0) throw invalid("Stock Party Field is Required");
        int refPartyId = toInt(b.get("refPartyId"));
        if (refPartyId <= 0) throw invalid("Reference Party Field is Required");
        int gpNo = toInt(trim(b.get("gpNo")));
        if (gpNo == 0) throw invalid("Gatepass Field is Required");
        double carriage = d(trim(b.get("carriageAmount")));
        int transporterId = toInt(b.get("transporterId"));
        if (carriage > 0.0 && transporterId == 0) throw invalid("Transporter Field is Required");
        if (transporterId > 0 && carriage == 0.0) throw invalid("Freight Amount Field is Required");
        double factory = d(trim(b.get("factoryWeight")));
        if (factory == 0.0) throw invalid("Factory Weight Field is Required");

        List<Map<String, Object>> rows = PpBGrnGdnStoreService.list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");

        // the gate pass: a new document takes one of this company's accepted, unused gate passes (grdGp Load); an update keeps
        // the saved one unless another pending gate pass was loaded.
        int gatePassId = Math.max(0, toInt(b.get("gatePassId")));
        double gpQty = 0;
        if (existing != null && gatePassId == toInt(existing.get("GatePassId"))) {
            gpQty = 0;
            for (Map<String, Object> d : PpBGrnGdnStoreService.list(existing.get("details"))) gpQty += d(d.get("Qty"));   // ReadById: GpQty = Sum(Qty)
        } else if (gatePassId > 0) {
            List<Map<String, Object>> g = gatePassRows(u, m, 0, gatePassId);
            if (g.isEmpty()) throw invalid("Status Not Accepted Please check status");
            gpQty = d(col(g.get(0), "ItemQty"));
        }

        boolean allowMultiple = allowMultiple(u);
        List<Integer> ownIds = new ArrayList<>();
        List<String> ownRefs = new ArrayList<>(), ownDos = new ArrayList<>();
        if (existing != null) for (Map<String, Object> d : PpBGrnGdnStoreService.list(existing.get("details"))) {
            ownIds.add(toInt(d.get("Id")));
            ownRefs.add(toInt(d.get("RefDocumentTypeId")) + ":" + toInt(d.get("RefDocIdNo")) + ":" + toInt(d.get("RefDocSubIdNo")));
            ownDos.add(toInt(d.get("DeliveryOrderId")) + ":" + toInt(d.get("DeliveryOrderDetailId")));
        }

        double gridTotalQty = 0;
        boolean anyDo = false;
        for (Map<String, Object> r : rows) { gridTotalQty += d(r.get("Qty")); if (toInt(r.get("DeliveryOrderId")) > 0) anyDo = true; }
        if (anyDo && gridTotalQty != gpQty) {
            throw invalid("Sum of Detail Qty '" + net(gridTotalQty) + "' not equal to GpQty '" + net(gpQty) + "' Please Check");
        }

        LocalDateTime now = LocalDateTime.now();
        PpBModels.GrnPartyProcessing h = new PpBModels.GrnPartyProcessing();
        h.Id = recId;
        LocalDateTime docDate = toDate(b.get("docDate"));
        h.DocDate = docDate == null ? now : docDate;
        h.DocNo = toInt(docNoText);
        h.StockPartyId = stockPartyId;
        h.GpNo = gpNo;
        h.SupplierCustomerId = refPartyId;
        h.PartyWeight = d(trim(b.get("partyWeight")));
        h.FactoryWeight = factory;
        h.VehicleType = str(b.get("vehicleType"));
        h.VehicleNo = trim(b.get("vehicleNo"));
        h.BiltyNo = trim(b.get("biltyNo"));
        h.TransporterId = transporterId;
        h.CarriageAmount = carriage;
        h.RemarksHeader = trim(b.get("remarks"));
        h.GatePassId = gatePassId;
        h.TradeToCompanyId = toInt(b.get("tradeToCompanyId"));
        h.ScreenName = m.name;
        h.DocumentTypeId = m.doc;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.BranchesId = branch(u);
        h.FinancialYearId = pp.hrm().financialYearId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.PostDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();

        List<PpBModels.GrnPartyProcessingDetail> details = new ArrayList<>();
        if (recId > 0) {                                                              // lstRemoveRecord (DeleteDetailRecord)
            for (Map<String, Object> r : PpBGrnGdnStoreService.list(b.get("removed"))) {
                int did = toInt(r.get("Id"));
                if (did <= 0 || !ownIds.contains(did)) continue;
                PpBModels.GrnPartyProcessingDetail d = detail(r);
                d.NetBillWeight = d.StockWeight;
                d.ActionTypeId = 3;
                details.add(d);
            }
        }

        List<Map<String, Object>> crops = pp.lookups().cropYears(u), jobs = pp.lookups().jobLotsPartyProcessing(u), packs = pp.lookups().packingTypes();
        List<Map<String, Object>> avail = null, ado = null;
        double grossWeight = 0, riceQty = 0;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            String rn = " in Detail Grid (Row No : " + (i + 1) + " )";
            if (toInt(r.get("WareHouseId")) == 0) throw invalid("Warehouse Name Field is Required" + rn);
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Name Field is Required" + rn);
            if (toInt(r.get("CropYearId")) == 0 || !has(crops, "Id", toInt(r.get("CropYearId")))) throw invalid("Crop Year Field is Required" + rn);
            if (toInt(r.get("JobId")) == 0 || !has(jobs, "Id", toInt(r.get("JobId")))) throw invalid("Job/Lot Field is Required" + rn);
            if (toInt(r.get("PackingTypeId")) == 0 || !has(packs, "Id", toInt(r.get("PackingTypeId")))) throw invalid("Packing Type Field is Required" + rn);
            if (toInt(r.get("PackUomId")) == 0) throw invalid("Pack Unit Field is Required" + rn);
            if (d(r.get("Qty")) == 0.0) throw invalid("Qty Field is Required" + rn);
            if (d(r.get("GrossWight")) == 0.0) throw invalid("Gross Weight Field is Required" + rn);
            if (d(r.get("NetBillWeight")) == 0.0) throw invalid("NetBillWeight Field is Required" + rn);
            if (d(r.get("StockWeight")) == 0.0) throw invalid("Stock Weight Field is Required" + rn);
            if (toInt(r.get("CityId")) == 0) throw invalid("City Field is Required" + rn);
            PpBModels.GrnPartyProcessingDetail d = detail(r);
            if (d.Id > 0 && !ownIds.contains(d.Id)) d.Id = 0;
            grossWeight += d.GrossWeight;
            riceQty += d.ItemQty;
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            if (d.StockWeight > d.GrossWeight && !allowMultiple) {
                throw invalid("StockWeight cannot greater than Gross Weight '" + net(d.GrossWeight) + "' Please check!" + rn);
            }
            // the loader / advance DO references must be ones this company's lists offered (or the saved ones)
            if (d.RefDocumentTypeId > 0 || d.RefDocNoId > 0 || d.RefDocSubIdNo > 0) {
                String key = d.RefDocumentTypeId + ":" + d.RefDocNoId + ":" + d.RefDocSubIdNo;
                if (!ownRefs.contains(key)) {
                    if (avail == null) avail = pp.available(u, null, null, stockPartyId, 0, 0, 0, 0, 0, 0, 0, 0);
                    boolean ok = false;
                    for (Map<String, Object> a : avail) {
                        if (toInt(a.get("RefDocumentTypeId")) == d.RefDocumentTypeId && toInt(a.get("RefDocIdNo")) == d.RefDocNoId
                                && toInt(a.get("RefDocSubIdNo")) == d.RefDocSubIdNo) ok = true;
                    }
                    if (!ok) throw invalid("Record not found...");
                }
            }
            if (d.DeliveryOrderId > 0 || d.DeliveryOrderDetailId > 0) {
                if (!ownDos.contains(d.DeliveryOrderId + ":" + d.DeliveryOrderDetailId)) {
                    if (ado == null) ado = adoRows(u, stockPartyId, gatePassId);
                    boolean ok = false;
                    for (Map<String, Object> a : ado) {
                        if (toInt(col(a, "InvDeliveryOrderId")) == d.DeliveryOrderId && toInt(col(a, "InvDeliveryOrderDetailId")) == d.DeliveryOrderDetailId) ok = true;
                    }
                    if (!ok) throw invalid("Record not found...");
                }
            }
            details.add(d);
        }

        boolean ifDoExist = false;
        for (PpBModels.GrnPartyProcessingDetail d : details) if (d.DeliveryOrderId > 0) ifDoExist = true;
        if (m.doc == 89 && ifDoExist) {
            BigDecimal diff = new BigDecimal(net(grossWeight - factory)).abs();
            BigDecimal tol = tolerance(u);
            if (diff.compareTo(tol) > 0) {
                throw invalid("The difference of gross weight and factory weight:'" + diff.toPlainString() + "' is greater than the tolerance: '"
                        + tol.toPlainString() + "' Please Check");
            }
        } else if (grossWeight != factory) {
            throw invalid("Sum of GrossWeight '" + net(grossWeight) + "' not equal to FactoryWeight '" + net(factory) + "' Please Check");
        }

        List<PpBModels.GrnEmptyBagsPartyProcessing> bags = new ArrayList<>();
        double ebQty = 0;
        for (Map<String, Object> r : PpBGrnGdnStoreService.list(b.get("emptyBags"))) {
            PpBModels.GrnEmptyBagsPartyProcessing e = new PpBModels.GrnEmptyBagsPartyProcessing();
            e.DocumentTypeId = m.doc;
            e.ItemId = toInt(r.get("ItemId"));
            e.ItemQty = d(r.get("Qty"));
            e.Remarks = str(r.get("Remarks"));
            bags.add(e);
            ebQty += e.ItemQty;
        }
        if (ebQty > riceQty) throw invalid("EmptyBags Qty '" + net(ebQty) + "' cannot be greater than RiceQty '" + net(riceQty) + "'");

        // InvGrnPartyProcessing.Save
        if (h.Id == 0) { h.ActionId = 1; h.ModifyUser = 0; } else { h.ActionId = 2; h.EntryUser = 0; }
        final List<Map<String, Object>> glItems = m.doc == 89 ? pp.lookups().itemGlIdsAndNames(u) : null;
        pp.db().tx(() -> {
            int num = pp.db().set(h.Id == 0 ? "Sp_InvGrnPartyProcessing_Insert" : "Sp_InvGrnPartyProcessing_Update", h);
            if (num > 0) h.Id = num; else num = h.Id;
            int line = 1;
            boolean loaderRows = false;
            for (PpBModels.GrnPartyProcessingDetail d : details) {
                if (h.DocumentTypeId == 89 && d.ActionTypeId != 3 && d.RefDocumentTypeId > 0 && d.RefDocNoId > 0 && d.RefDocSubIdNo > 0) loaderRows = true;
                d.LineId = line++;
                d.InvGrnPartyProcessingId = h.Id;
                d.Id = pp.db().set("Sp_InvGrnPartyProcessingDetail_Insert", d);
            }
            for (PpBModels.GrnEmptyBagsPartyProcessing e : bags) {
                e.InvGrnPartyProcessingId = h.Id;
                pp.db().set("Sp_InvGrnEmptyBagsPartyProcessing_Insert", e);
            }
            if (h.DocumentTypeId == 89 && !loaderRows) fifo(u, h, details, glItems);
            if ((h.DocumentTypeId == 89 && loaderRows) || h.DocumentTypeId == 49) {
                PpBModels.InventoryTransactionsPartyProcessing t = new PpBModels.InventoryTransactionsPartyProcessing();
                t.OrganizationId = h.OrganizationId; t.CompanyId = h.CompanyId; t.RefDocumentTypeId = h.DocumentTypeId; t.RefDocIdNo = num;
                pp.db().set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
            }
            if (h.DocumentTypeId == 89) {
                for (PpBModels.GrnPartyProcessingDetail d : details) {
                    pp.db().rows("USP_InventoryValidationPartyProcessing", "OrganizationId", h.OrganizationId, "CompanyId", h.CompanyId,
                            "DocumentTypeId", h.DocumentTypeId, "DocDate", Timestamp.valueOf(h.DocDate), "StockPartyId", h.StockPartyId,
                            "ItemId", d.ItemId, "WarehouseId", d.WarehouseId, "JobLotId", d.JobLotId, "CropYearId", d.CropYearId,
                            "InvPackingTypeId", d.PackingTypeId, "PackUomId", d.ItemUomId, "NetWeight", d.NetBillWeight,
                            "RefDocumentTypeId", d.RefDocumentTypeId, "RefDocNoId", d.RefDocNoId, "RefDocSubIdNo", d.RefDocSubIdNo);
                }
            }
            return num;
        });
        Map<String, Object> res = saved(h.Id, (recId > 0 ? "Record Update Successfully [" : "Record Save Successfully [") + h.DocNo + "]");
        res.put("documentTypeId", m.doc);
        return res;
    }

    private static PpBModels.GrnPartyProcessingDetail detail(Map<String, Object> r) {
        PpBModels.GrnPartyProcessingDetail d = new PpBModels.GrnPartyProcessingDetail();
        d.Id = toInt(r.get("Id"));
        d.RefDocNoId = toInt(r.get("RefDocIdNo"));
        d.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
        d.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
        d.DeliveryOrderId = toInt(r.get("DeliveryOrderId"));
        d.DeliveryOrderDetailId = toInt(r.get("DeliveryOrderDetailId"));
        d.DoDocumentTypeId = toInt(r.get("DoDocumentTypeId"));
        d.WarehouseId = toInt(r.get("WareHouseId"));
        d.ItemId = toInt(r.get("ItemId"));
        d.CropYearId = toInt(r.get("CropYearId"));
        d.JobLotId = toInt(r.get("JobId"));
        d.PackingTypeId = toInt(r.get("PackingTypeId"));
        d.ItemQty = d(r.get("Qty"));
        d.ItemUomId = toInt(r.get("PackUomId"));
        d.GrossWeight = d(r.get("GrossWight"));
        d.EBWPerUnit = d(r.get("EbUnit"));
        d.EBWTotal = d(r.get("EbTotal"));
        d.AdLsWeight = d(r.get("AddLesswt"));
        d.NetBillWeight = d(r.get("NetBillWeight"));
        d.StockWeight = d(r.get("StockWeight"));
        d.CityId = toInt(r.get("CityId"));
        d.CommentsDetail = trim(r.get("Comments"));
        d.ContainerNo = trim(r.get("ContainerNo"));
        d.SealNo = trim(r.get("SealNo"));
        return d;
    }

    /** DAL 0306 SetData, DocumentTypeId 89 without loader rows: FIFOImplementionForPartyProcessing per detail row and the stock rows. */
    private void fifo(UserAccount u, PpBModels.GrnPartyProcessing h, List<PpBModels.GrnPartyProcessingDetail> details, List<Map<String, Object>> glItems) {
        double num5 = 0, num6 = 0;
        for (PpBModels.GrnPartyProcessingDetail d : details) if (d.ActionTypeId != 3) { num5 += d.NetBillWeight; num6 += d.StockWeight; }
        List<PpOut> reserve = new ArrayList<>();
        List<PpBModels.TransPpFifo> out = new ArrayList<>();
        Timestamp docDate = Timestamp.valueOf(h.DocDate);
        for (PpBModels.GrnPartyProcessingDetail d : details) {
            if (d.ActionTypeId == 3) continue;
            String name = itemName(glItems, d.ItemId);
            if (name == null) continue;
            boolean upd = h.ModifyUser > 0;
            List<PpOut> got = pp.fifoPartyProcessing(u, d.ItemId, h.StockPartyId, docDate, d.ItemUomId, d.WarehouseId, d.JobLotId, d.PackingTypeId,
                    d.CropYearId, upd ? h.DocumentTypeId : 0, upd ? h.Id : 0, d.ItemQty, d.StockWeight, d.LineId, name, reserve);
            for (PpOut o : got) {
                PpBModels.TransPpFifo t = toFifoModel(o);
                if (num5 > 0.0 && num6 > 0.0) { t.BillWeightOut = num5 / num6 * t.StockWeightOut; o.billWeightOut = t.BillWeightOut; }
                t.CityId = d.CityId;
                t.GpNo = h.GpNo;
                t.VehicleNo = h.VehicleNo;
                t.TranRemarks = "Goods Dispatch Notes Party Processing";
                t.GrossWeight = d.GrossWeight;
                t.EbUnit = d.EBWPerUnit;
                t.EbTotal = d.EBWTotal;
                t.AddLess = d.AdLsWeight;
                t.TransporterId = h.TransporterId;
                t.FreightAmount = h.CarriageAmount;
                t.DetailRemarks = d.CommentsDetail;
                out.add(t);
                reserve.add(o);
            }
        }
        if (out.isEmpty()) return;
        if (h.ModifyUser > 0) {
            pp.db().rows("[dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId]", "OrganizationId", h.OrganizationId,
                    "CompanyId", h.CompanyId, "RefDocumentTypeId", h.DocumentTypeId, "RefDocIdNo", h.Id);
        }
        for (PpBModels.TransPpFifo t : out) {
            t.OrganizationId = h.OrganizationId;
            t.CompanyId = h.CompanyId;
            t.DocDate = h.DocDate;
            t.DocCodeNo = h.DocNo;
            t.SupplierCustomerId = h.SupplierCustomerId;
            t.StockPartyId = h.StockPartyId;
            t.BranchesId = h.BranchesId;
            t.RefDocumentTypeId = h.DocumentTypeId;
            t.CalcType = "Weight";
            t.EntryUserId = h.EntryUser;
            t.ModifyUserId = h.ModifyUser;
            t.EntryDate = LocalDateTime.now();
            t.ModifyDate = LocalDateTime.now();
            for (PpBModels.GrnPartyProcessingDetail d : details) {
                if (d.LineId == t.LineId && d.LineId > 0 && t.LineId > 0) { t.RefDocIdNo = d.InvGrnPartyProcessingId; t.RefDocSubIdNo = d.Id; break; }
            }
            pp.db().set("USP_InventoryTransactionsPartyProcessing_Insert", t);
        }
    }

    // ------------------------------------------------------------------ delete / print

    /** btnDelete_Click: "No record found to Delete" / DAL 0306 DeleteById (@Id, @EntryUser, 'DeleteById') -> "Delete Record Successfully". */
    public Map<String, Object> delete(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        pp.hrm().require(u, m.screen, "Delete");
        if (id <= 0) throw invalid("No record found to Delete");
        read(u, m, id);
        pp.db().tx(() -> pp.db().scalar(PROC, map("Id", id, "EntryUser", u.getId(), "Activity", "DeleteById")));
        return saved(id, "Delete Record Successfully");
    }

    /**
     * GrnSlipPartyProcessing333 (49) / GdnSlipPartyProcessing339 (89): the slip rows of GrnGdnReports.GrnAndGdnSlipAndRegister ->
     * [dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt] (@Org @Company @FinancialYearId @DocumentTypeId @Id). 49 then opens the 333 template
     * (key ppb-333); the 339 template name is not in the available sources, so 89 prints these rows through the grid-to-PDF printer.
     */
    public List<Map<String, Object>> slip(String mode, int id) {
        Mode m = new Mode(mode);
        UserAccount u = pp.user(m.screen);
        pp.hrm().require(u, m.screen, "Print");
        read(u, m, id);
        List<Map<String, Object>> rows = pp.db().rows("[dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", pp.hrm().financialYearId(), "DocumentTypeId", m.doc, "Id", id);
        if (rows.isEmpty()) throw invalid("No Record Found For Display");
        return rows;
    }
}
