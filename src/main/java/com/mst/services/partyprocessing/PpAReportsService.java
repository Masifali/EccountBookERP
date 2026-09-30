package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.repositories.partyprocessing.PpARepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpASupport.*;

/**
 * BLL of the Party Processing report forms of group PpA (Architecture.WinApp.PartyProcessingReports):
 *
 *   690 GRN Info            frmPartyProcessingGrnInfo.cs        GrnGdnReports.PartyProcessingGrnInfo
 *   691 Gate Pass           frmGatePassPartyProcessing.cs       PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister
 *   692 / 693 GRN / GDN     frmGRNGDNPartyProcessing.cs         GrnGdnReports.GrnAndGdnSlipAndRegister (Tag
 *                           "frmGRNPartyProcessingRegister" -> DocumentTypeId 49, "frmGDNPartyProcessingRegister" -> 89)
 *   694 Stock Report        frmStockReportPartyProcessing.cs    StocksReport.PartyProcessingstockGeneralSummaryByWeight
 *
 * Each search builds the parameters exactly as the BLL does (guarded ones only when the BLL sends them);
 * the prints (PartyprocessingPpAReports) receive the same filter values from the page.
 */
@Service
public class PpAReportsService {

    public static final int SCREEN_GRN_INFO = 690;
    public static final int SCREEN_GATE_PASS = 691;
    public static final int SCREEN_GRN_REGISTER = 692;
    public static final int SCREEN_GDN_REGISTER = 693;
    public static final int SCREEN_STOCK = 694;

    /** CommonServices.DateType(): the fixed list the forms bind with ZeroIndex true. */
    private static final String[][] DATE_TYPES = { { "1", "This Day" }, { "2", "This Week" }, { "3", "This Month" },
            { "4", "This Year" }, { "5", "Financial Year" } };

    @Autowired private PpARepository repo;
    @Autowired private PpASupport pp;

    private static List<Map<String, Object>> list(String[][] rows, String idKey, String textKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (String[] r : rows) out.add(map(idKey, r[0], textKey, r[1]));
        return out;
    }

    /** Rows of a combined drop-down result whose activity column equals the value: Id / name. */
    private static List<Map<String, Object>> byActivity(List<Map<String, Object>> rows, String col, String activity, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) if (activity.equals(str(r.get(col)))) out.add(map("Id", r.get("Id"), "name", r.get(nameCol)));
        return out;
    }

    private Map<String, Object> common(UserAccount u) {
        return map("dateTypes", list(DATE_TYPES, "Id", "Parameters"), "financialYearStart", pp.financialYearStart(u));
    }

    private static void put(Map<String, Object> p, String k, int v) { if (v != 0) p.put(k, v); }

    private static void putDate(Map<String, Object> p, String k, Object v) {
        Timestamp t = when(v);
        if (t != null) p.put(k, t);
    }

    // ================================================================== 690 GRN Info

    /** frmGRNHistory_Load: ComboFill (GetDataForDropDownFromGrn, no doc types), Datetypefill, GrnTypeBind. */
    public Map<String, Object> grnInfoSetup() {
        UserAccount u = pp.user(SCREEN_GRN_INFO);
        Map<String, Object> out = common(u);
        out.putAll(grnInfoCombos(u));
        out.put("grnTypes", list(new String[][] { { "1", "Pending" }, { "2", "Processed" } }, "Id", "Name"));
        return out;
    }

    /** btnRefresh_Click: ComboFill(). */
    public Map<String, Object> grnInfoCombos() { return grnInfoCombos(pp.user(SCREEN_GRN_INFO)); }

    private Map<String, Object> grnInfoCombos(UserAccount u) {
        List<Map<String, Object>> rows = repo.dropDownFromGrn(u, null);
        return map("stockParties", byActivity(rows, "Activity", "StockParty", "ReferenceName"),
                "refParties", byActivity(rows, "Activity", "ReferenceParty", "ReferenceName"),
                "items", byActivity(rows, "Activity", "Item", "ReferenceName"),
                "warehouses", byActivity(rows, "Activity", "Warehouse", "ReferenceName"),
                "jobLots", byActivity(rows, "Activity", "JobLot", "ReferenceName"));
    }

    /** gridHisory(): Sp_InvPartyProcessingGrn_Info with the BLL's guards; the grid columns the form builds. */
    public List<Map<String, Object>> grnInfo(Map<String, Object> f) {
        UserAccount u = pp.user(SCREEN_GRN_INFO);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 49);
        putDate(p, "FromDate", f.get("fromDate"));
        putDate(p, "ToDate", f.get("toDate"));
        put(p, "SupplierCustomerId", i(f, "stockPartyId"));
        put(p, "ReferencePartyId", i(f, "refPartyId"));
        put(p, "WarHouseId", i(f, "warehouseId"));
        put(p, "JobLotId", i(f, "jobLotId"));
        put(p, "DocNoFrom", cint(f.get("docNoFrom")));
        put(p, "DocNoTo", cint(f.get("docNoTo")));
        put(p, "GPNoFrom", cint(f.get("gpNoFrom")));
        put(p, "GPNoTo", cint(f.get("gpNoTo")));
        put(p, "GrnTypeId", i(f, "grnTypeId"));
        put(p, "ItemId", i(f, "itemId"));
        return pick(repo.rows("Sp_InvPartyProcessingGrn_Info", p),
                "Id", "Id", "DocumentTypeId", "DocumentTypeId", "GrnNo", "GrnNo", "GrnDate", "GrnDate", "StockPartyName", "StockPartyName",
                "ReferencePartyName", "ReferencePartyName", "GpDate", "GpDate", "GpNo", "GpNo", "VehicleNo", "VehicleNo", "BiltyNo", "BiltyNo",
                "TransporterName", "TransporterName", "CarriageAmount", "CarriageAmount", "IsApproved", "IsApproved",
                "GrnEntryDate", "GrnEntryDate", "GrnEntryUser", "GrnEntryUser", "GrnModifyDate", "GrnModifyDate", "GrnModifyUser", "GrnModifyUser",
                "GrnApprovedDate", "GrnApprovedDate", "GrnApprovedUser", "GrnApprovedUser", "JobOrderNo", "JobOrderNo",
                "WareHouseName", "WareHouseName", "ItemName", "ItemName", "CropYear", "CropYear", "JobLot", "JobLotDescription",
                "PackType", "PackTypeDesc", "UOM", "UOMCode", "ItemQty", "ItemQty", "GrossWeight", "GrossWeight", "EBWPerUnit", "EBWPerUnit",
                "EBWTotal", "EBWTotal", "AdLsWeight", "AdLsWeight", "NetBillWeight", "NetBillWeight", "StockWeight", "StockWeight", "GrnType", "GrnType");
    }

    /** GenerateReport(PrintId): GrnAndGdnSlipAndRegister(49, active year, Id) - "No Record Found For Display" when empty. */
    public Map<String, Object> grnSlipCheck(int id) {
        UserAccount u = pp.user(SCREEN_GRN_INFO);
        if (id <= 0) throw invalid("No Record Found For Display");
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.financialYearId(), "DocumentTypeId", 49, "Id", id, "IsApproved", false);
        if (repo.rows("[dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]", p).isEmpty()) throw invalid("No Record Found For Display");
        return map("id", id);
    }

    // ================================================================== 691 Gate Pass

    /** frmGatePassReport_Load: Datetypefill, ComboFill (GetDataForDropDownFromGPPrtyProcessing), IsAcceptFill. */
    public Map<String, Object> gatePassSetup() {
        UserAccount u = pp.user(SCREEN_GATE_PASS);
        Map<String, Object> out = common(u);
        out.putAll(gatePassCombos(u));
        out.put("statuses", list(new String[][] { { "1", "Open" }, { "2", "Accepted" }, { "3", "Rejected" } }, "Id", "Status"));
        return out;
    }

    public Map<String, Object> gatePassCombos() { return gatePassCombos(pp.user(SCREEN_GATE_PASS)); }

    private Map<String, Object> gatePassCombos(UserAccount u) {
        List<Map<String, Object>> rows = repo.dropDownFromGatePass(u);
        return map("stockParties", byActivity(rows, "Activity", "StockParty", "ReferenceName"),
                "refParties", byActivity(rows, "Activity", "ReferenceParty", "ReferenceName"),
                "documentTypes", byActivity(rows, "Activity", "DocumentType", "ReferenceName"),
                "gatePassTypes", byActivity(rows, "Activity", "GatePassType", "ReferenceName"));
    }

    /** GridFill(): PartyProcessingGatePassSlipandRegister (Status / GatePassType are the combos' texts, sent when not empty). */
    public List<Map<String, Object>> gatePass(Map<String, Object> f) {
        UserAccount u = pp.user(SCREEN_GATE_PASS);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "FinancialYearId", pp.financialYearId());
        put(p, "DocumentTypeId", i(f, "documentTypeId"));
        put(p, "StockPartyId", i(f, "stockPartyId"));
        put(p, "ReferencePartyId", i(f, "refPartyId"));
        put(p, "DocNoFrom", cint(f.get("docNoFrom")));
        put(p, "DocNoTo", cint(f.get("docNoTo")));
        putDate(p, "FromDate", f.get("fromDate"));
        putDate(p, "ToDate", f.get("toDate"));
        if (!s(f, "status").isEmpty()) p.put("Status", s(f, "status"));
        p.put("IsApproved", false);
        if (!s(f, "gatePassType").isEmpty()) p.put("GatePassType", s(f, "gatePassType"));
        return pick(repo.rows("[dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]", p),
                "Id", "Id", "DocumentTypeId", "DocumentTypeId", "GpDate", "GpDate", "GpSrNo", "GpSrNo", "GpType", "GpType",
                "GatePassType", "GatePassType", "GpTypeSrNo", "GpTypeSrNo", "StockParty", "StockPartyName", "ReferenceParty", "ReferenceParty",
                "VarietyName", "VarietyName", "VehicleType", "VehicleType", "VehicleNo", "VehicleNo", "BiltyNo", "BiltyNo", "ItemQty", "ItemQty",
                "OtherRemarks", "OtherRemarks", "InDateTimeStamp", "InDateTimeStamp", "OutDateTimeStamp", "OutDateTimeStamp", "Status", "Status",
                "SupplierWeight", "SupplierWeight", "FactoryWeight", "FactoryWeight", "DifferenceWeight", "DifferenceWeight",
                "WBStatus", "WeighBridgeStatus", "IsWeighable", "IsWeighable");
    }

    // ================================================================== 692 / 693 GRN / GDN register

    public static int docType(String mode) { return "gdn".equalsIgnoreCase(mode) ? 89 : 49; }

    public static int screen(String mode) { return "gdn".equalsIgnoreCase(mode) ? SCREEN_GDN_REGISTER : SCREEN_GRN_REGISTER; }

    /** frmGatePassReport_Load: Tag -> DocumentTypeId / buttons / caption, Datetypefill, ComboFill. */
    public Map<String, Object> grnGdnSetup(String mode) {
        UserAccount u = pp.user(screen(mode));
        Map<String, Object> out = common(u);
        out.putAll(grnGdnCombos(u, mode));
        out.put("documentTypeId", docType(mode));
        return out;
    }

    public Map<String, Object> grnGdnCombos(String mode) { return grnGdnCombos(pp.user(screen(mode)), mode); }

    /** ComboFill(): GetDataForDropDownFromGrn(org, comp, DocumentTypeId.ToString()) - StockParty / ReferenceParty / Item. */
    private Map<String, Object> grnGdnCombos(UserAccount u, String mode) {
        List<Map<String, Object>> rows = repo.dropDownFromGrn(u, String.valueOf(docType(mode)));
        return map("stockParties", byActivity(rows, "Activity", "StockParty", "ReferenceName"),
                "refParties", byActivity(rows, "Activity", "ReferenceParty", "ReferenceName"),
                "items", byActivity(rows, "Activity", "Item", "ReferenceName"));
    }

    /** GridFill(): GrnAndGdnSlipAndRegister (BranchesId = session branch; FromDate / ToDate always; @IsApproved false). */
    public List<Map<String, Object>> grnGdn(String mode, Map<String, Object> f) {
        UserAccount u = pp.user(screen(mode));
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", pp.financialYearId(), "DocumentTypeId", docType(mode));
        put(p, "BranchesId", toInt(u.getBranchesId()));
        putDate(p, "FromDate", f.get("fromDate"));
        putDate(p, "ToDate", f.get("toDate"));
        put(p, "SupplierCustomerId", i(f, "supplierCustomerId"));
        put(p, "StockPartyId", i(f, "stockPartyId"));
        put(p, "DocNoFrom", cint(f.get("docNoFrom")));
        put(p, "DocNoTo", cint(f.get("docNoTo")));
        put(p, "ItemId", i(f, "itemId"));
        p.put("IsApproved", false);
        return pick(repo.rows("[dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]", p),
                "Id", "Id", "DocNo", "DocNo", "DocDate", "DocDate", "StockParty", "StockPartyName", "ReferenceParty", "ReferencePartyName",
                "GpSrNo", "GpNo", "WareHouse", "WareHouseName", "ItemName", "ItemName", "CropYear", "CropYear", "JobLot", "JobLotDescription",
                "PackingType", "PackTypeDesc", "PackUom", "UOMCode", "ItemQty", "ItemQty", "StockWeight", "StockWeight", "EBWPerUnit", "EBWPerUnit",
                "EBWTotal", "EBWTotal", "AdLsWeight", "AdLsWeight", "NetBillWeight", "NetBillWeight", "GrossWeight", "GrossWeight",
                "Transporter", "TransporterName", "VehicleNo", "VehicleNo", "BiltyNo", "BiltyNo", "CityName", "CityName", "Freight", "CarriageAmount",
                "RemarksHeader", "RemarksHeader", "Comments", "CommentsDetail");
    }

    // ================================================================== 694 Stock Report

    /** ReportTypeFill(): value / text as the form adds them. */
    private static final String[][] REPORT_TYPES = {
            { "ItemStockSummary", "Item Stock Summary" },
            { "ItemandWarehouseStockSummary", "Item and WareHouse Stock Summary" },
            { "ItemandCropYearStockSummary", "Crop Year and Item Stock Summary" },
            { "ItemandCropYearandWarehouseStockSummary", "Crop Year, Warehouse and Item Stock Summary" },
            { "JobLotandItemStockSummary", "JobLot and Item" },
            { "WarehouseandJoblotandItemStockSummary", "Warehouse, Joblot and Item Stock Summary" } };

    public Map<String, Object> stockSetup() {
        UserAccount u = pp.user(SCREEN_STOCK);
        Map<String, Object> out = common(u);
        out.putAll(stockCombos(u));
        out.put("reportTypes", list(REPORT_TYPES, "Id", "name"));
        return out;
    }

    public Map<String, Object> stockCombos() { return stockCombos(pp.user(SCREEN_STOCK)); }

    /** ComboFill(): InventoryTransactionsPartyProcessing_DropDownAndList split by ActivityType. */
    private Map<String, Object> stockCombos(UserAccount u) {
        List<Map<String, Object>> rows = repo.stockDropDowns(u);
        return map("itemTypes", byActivity(rows, "ActivityType", "ItemTypes", "name"),
                "categories", byActivity(rows, "ActivityType", "ItemCategories", "name"),
                "items", byActivity(rows, "ActivityType", "Items", "name"),
                "warehouses", byActivity(rows, "ActivityType", "Warehouse", "name"),
                "jobLots", byActivity(rows, "ActivityType", "JobLot", "name"),
                "cropYears", byActivity(rows, "ActivityType", "CropYear", "name"),
                "stockParties", byActivity(rows, "ActivityType", "StockParty", "name"));
    }

    /**
     * GridBind(): PartyProcessingstockGeneralSummaryByWeight. Item Wise Stock ticked -> IsPackSizeOn / IsPackTypeOn
     * not sent (0), else 1; Skip Zero ticked -> @SkipZero 1; @Activity = the report type value. The page shapes the
     * grid per report type from these rows (the form's DataTables).
     */
    public List<Map<String, Object>> stock(Map<String, Object> f) {
        UserAccount u = pp.user(SCREEN_STOCK);
        String activity = s(f, "activity");
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        putDate(p, "DateFrom", f.get("fromDate"));
        putDate(p, "DateTo", f.get("toDate"));
        put(p, "ItemTypeId", i(f, "itemTypeId"));
        put(p, "ItemCategoryId", i(f, "itemCategoryId"));
        if (!s(f, "cropYear").isEmpty()) p.put("CropYear", s(f, "cropYear"));
        put(p, "JobLotId", i(f, "jobLotId"));
        put(p, "WarehouseId", i(f, "warehouseId"));
        put(p, "ItemId", i(f, "itemId"));
        int on = flag(f, "itemWise") ? 0 : 1;
        put(p, "IsPackSizeOn", on);
        put(p, "IsPackTypeOn", on);
        put(p, "StockPartyId", i(f, "stockPartyId"));
        if (flag(f, "skipZero")) p.put("SkipZero", 1);
        p.put("Activity", activity);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.rows("Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt", p)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", r.get("ItemId"));
            m.put("StockParty", r.get("StockParty"));
            m.put("ItemName", r.get("ItemName"));
            m.put("WareHouseName", r.get("WareHouseName"));
            m.put("CropYear", r.get("CropYear"));
            m.put("JobLot", r.get("JobLot"));
            m.put("PackUom", r.get("PackUom"));
            m.put("PackingType", r.get("PackingType"));
            for (String k : new String[] { "OpQty", "QtyIn", "QtyOut", "BalQty", "OpWeight", "WeightIn", "WeightOut", "BalWeight",
                    "OpBillWeight", "BillWeightIn", "BillWeightOut" }) m.put(k, toDouble(r.get(k)));
            m.put("BalBillWeight", toDouble(r.get("BillBalWeight")));
            out.add(m);
        }
        return out;
    }
}
