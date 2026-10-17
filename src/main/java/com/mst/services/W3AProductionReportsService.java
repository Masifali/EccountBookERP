package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.jdbc.core.ColumnMapRowMapper;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.RowMapperResultSetExtractor;
import org.springframework.stereotype.Service;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.time.LocalDate;
import java.util.*;

/**
 * Wave 3 / group W3A - four Production report screens, all read-only:
 *   307 GrnsPendingOrUsedinProduction     (Architecture.WinApp.Production_Reports/GrnsPendingOrUsedinProduction.cs)
 *   311 frmProductionPendingPurchaseInvoice (Architecture.WinApp.Production_Reports/frmProductionPendingPurchaseInvoice.cs)
 *   820 frmConcreteProductionReport       (Architecture.WinApp.Pcc/frmConcreteProductionReport.cs, DocumentTypeId 1850)
 *   591 ProductionRegisterSalt            (Architecture.WinApp.Salt.Reports/ProductionRegisterSalt.cs)
 * Line refs ":NNN" are the respective desktop file. Organization/company/user always come from the session.
 * Rights: the desktop ScreenId "View" right (DesktopReportRights), as the sibling report services do.
 */
@Service
public class W3AProductionReportsService {

    public static final int S_GRNS = 307, S_PENDING_INVOICE = 311, S_CONCRETE = 820, S_SALT = 591;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public W3AProductionReportsService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
        this.jdbc = jdbc; this.context = context; this.rights = rights;
    }

    // ---------------------------------------------------------------- helpers
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        String s = str(o);
        if (s.isEmpty()) return 0;
        try { return (int) Double.parseDouble(s.replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
    private static java.sql.Date day(Object iso) {
        String s = str(iso);
        if (s.length() < 10) throw new IllegalArgumentException("From Date and To Date are required");
        return java.sql.Date.valueOf(LocalDate.parse(s.substring(0, 10)));
    }
    private static Object ci(Map<String, Object> r, String key) {
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static String dateOnly(Object o) { String s = str(o); return s.length() >= 10 ? s.substring(0, 10) : s; }

    private UserAccount user(int screen) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, "View");
        return u;
    }

    /** DataSet-returning procedure: every result set in order (DataSet.Tables[n]). */
    private List<List<Map<String, Object>>> multi(final String sql, final Object... args) {
        return jdbc.execute((ConnectionCallback<List<List<Map<String, Object>>>>) con -> {
            try (PreparedStatement ps = con.prepareStatement(sql)) {
                for (int i = 0; i < args.length; i++) ps.setObject(i + 1, args[i]);
                List<List<Map<String, Object>>> out = new ArrayList<>();
                RowMapperResultSetExtractor<Map<String, Object>> ex = new RowMapperResultSetExtractor<>(new ColumnMapRowMapper());
                boolean hasRs = ps.execute();
                while (true) {
                    if (hasRs) { try (ResultSet rs = ps.getResultSet()) { out.add(ex.extractData(rs)); } }
                    else if (ps.getUpdateCount() == -1) break;
                    hasRs = ps.getMoreResults();
                }
                return out;
            }
        });
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (used by the "Financial Year" date type). */
    private String fyStart() {
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        return fy.isEmpty() || fy.get(0).get("Start_Period") == null ? LocalDate.now().toString() : dateOnly(fy.get(0).get("Start_Period"));
    }

    /** BranchesFill (:154 / 311 :83) - InvFoodProduction.GetBranchsAllocatedToUserFromProduction -> USP_GetBranchsAllocatedToUserFromProduction. */
    private List<Map<String, Object>> productionBranches(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUserFromProduction @OrganizationId=?,@CompanyId=?,@UserId=?",
                context.currentOrganizationId(), context.currentCompanyId(), context.currentUserId());
    }

    /** BranchIds string the desktop builds: "," + id for every ticked branch (null when none ticked => all branches). */
    private static String branchCsv(Object idsObj) {
        StringBuilder sb = new StringBuilder();
        if (idsObj instanceof Collection) for (Object o : (Collection<?>) idsObj) { int id = num(o); if (id > 0) sb.append(',').append(id); }
        return sb.length() == 0 ? null : sb.toString();
    }

    /** ComboFill (307 :182 / 311 :122) - StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists -> USP_Inventory_StockEvalautionDetail_DropDownAndLists. */
    public List<Map<String, Object>> stockCombos(int screen, Object branchIds) {
        user(screen);
        String csv = branchCsv(branchIds);
        if (csv == null) {
            return jdbc.queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?,@CompanyId=?",
                    context.currentOrganizationId(), context.currentCompanyId());
        }
        return jdbc.queryForList("EXEC dbo.USP_Inventory_StockEvalautionDetail_DropDownAndLists @OrganizationId=?,@CompanyId=?,@BranchesIds=?",
                context.currentOrganizationId(), context.currentCompanyId(), csv);
    }

    // ---------------------------------------------------------------- init
    public Map<String, Object> init(String key) {
        Map<String, Object> out = new LinkedHashMap<>();
        switch (key) {
            case "grns": case "pendinginv": {
                int screen = key.equals("grns") ? S_GRNS : S_PENDING_INVOICE;
                UserAccount u = user(screen);
                List<Map<String, Object>> branches = productionBranches(u);
                out.put("branches", branches);
                out.put("userBranchId", context.currentBranchId());
                /* Load -> BranchesFill sets the combo text to the user's branch, then ComboFill reads that text. */
                boolean inList = false;
                for (Map<String, Object> b : branches) if (num(ci(b, "BranchId")) == context.currentBranchId()) inList = true;
                out.put("combos", stockCombos(screen, inList ? List.of(context.currentBranchId()) : null));
                out.put("fyStart", fyStart());
                break;
            }
            case "concrete": {
                UserAccount u = user(S_CONCRETE);
                /* AllCombobind :99 - BLL.pcc ProductionHeader.GetDataForDropDownFromProductionHeader -> pcc.USP_GetDataForDropDownFromProductionHeader */
                List<Map<String, Object>> rows = jdbc.queryForList("EXEC pcc.USP_GetDataForDropDownFromProductionHeader @OrganizationId=?,@CompanyId=?",
                        u.getOrganizationId(), u.getCompanyId());
                out.put("productionTypes", bucket(rows, "ProductionType"));
                out.put("plants", bucket(rows, "Plant"));
                out.put("fyStart", fyStart());
                break;
            }
            case "salt": {
                UserAccount u = user(S_SALT);
                /* AllCombobind :121 - ProductionHeader.GetDataForDropDownFromProductionHeader -> dbo.USP_GetDataForDropDownFromProductionHeader */
                List<Map<String, Object>> rows = jdbc.queryForList("EXEC dbo.USP_GetDataForDropDownFromProductionHeader @OrganizationId=?,@CompanyId=?",
                        u.getOrganizationId(), u.getCompanyId());
                out.put("itemTypes", bucket(rows, "ItemType"));
                out.put("warehouses", bucket(rows, "Warehouse"));
                out.put("items", bucket(rows, "Item"));
                out.put("jobLots", bucket(rows, "JobLot"));
                out.put("productionNos", bucket(rows, "ProductionNo"));
                out.put("itemCategories", bucket(rows, "ItemCategory"));
                out.put("fyStart", fyStart());
                break;
            }
            default: throw new IllegalArgumentException("Unknown report.");
        }
        return out;
    }

    /** rows of one Activity as {Id, Name} (dtcombo.Rows[i]["Activity"] / ["ReferenceName"]). */
    private static List<Map<String, Object>> bucket(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!str(ci(r, "Activity")).equals(activity)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id")); m.put("Name", ci(r, "ReferenceName"));
            out.add(m);
        }
        return out;
    }

    // ---------------------------------------------------------------- 307
    /** GridBind :524 - InvGrn.GetGrnsPendingOrUsedinProduction -> USP_GetGrnsPendingOrUsedinProduction; rows reshaped as dtGrid (:118-141). */
    public List<Map<String, Object>> grnsPendingOrUsed(Map<String, Object> p) {
        user(S_GRNS);
        String branches = branchCsv(p.get("branchIds"));
        if (branches == null) throw new IllegalArgumentException("Select Branch First");
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_GetGrnsPendingOrUsedinProduction @OrganizationId=?,@CompanyId=?,@FromDate=?,@ToDate=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), day(p.get("fromDate")), day(p.get("toDate"))));
        if (num(p.get("partyId")) > 0) { sql.append(",@SupplierCustomerId=?"); a.add(num(p.get("partyId"))); }
        if (num(p.get("itemId")) > 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("jobLotId")) > 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("warehouseId")) > 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("packingTypeId")) > 0) { sql.append(",@InvPackingTypeId=?"); a.add(num(p.get("packingTypeId"))); }
        if (num(p.get("pendingUsed")) > 0) { sql.append(",@PendingUsed=?"); a.add(num(p.get("pendingUsed"))); }
        /* obj.CropYear = CropYear combo text when its Id > 0 :540 */
        if (num(p.get("cropYearId")) > 0 && !str(p.get("cropYear")).isEmpty()) { sql.append(",@CropYear=?"); a.add(str(p.get("cropYear"))); }
        sql.append(",@BranchesIds=?"); a.add(branches);
        List<Map<String, Object>> src = jdbc.queryForList(sql.toString(), a.toArray());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("BranchName", ci(r, "BranchName")); m.put("GrnStatus", ci(r, "GrnStatus")); m.put("Id", ci(r, "Id"));
            m.put("DocDate", dateOnly(ci(r, "DocDate"))); m.put("DocNo", ci(r, "DocNo")); m.put("PartyName", ci(r, "SupplierName"));
            m.put("ItemName", ci(r, "ItemName")); m.put("CropYear", ci(r, "CropYear")); m.put("JobLot", ci(r, "JobLotCode"));
            m.put("PackType", ci(r, "PackTypeCode")); m.put("PackUom", ci(r, "PackUom")); m.put("GatePassNo", ci(r, "GpNo"));
            m.put("BiltyNo", ci(r, "BiltyNo")); m.put("VehicleNo", ci(r, "VehicleNo")); m.put("WareHouse", ci(r, "WareHouseCode"));
            m.put("CityName", ci(r, "CityName")); m.put("FreightAmount", ci(r, "Freight")); m.put("GrnQty", ci(r, "GrnQty"));
            m.put("OutQty", ci(r, "OutQty")); m.put("BalQty", ci(r, "BalQty")); m.put("GrnWeight", ci(r, "GrnWeight"));
            m.put("OutWeight", ci(r, "OutWeight")); m.put("BalWeight", ci(r, "BalWeight"));
            out.add(m);
        }
        return out;
    }

    // ---------------------------------------------------------------- 311
    /** GridBind :258 - InvFoodProductionReports.PendingInvoicesForProduction -> Sp_InvPurchaseInvoice_PendingInvoiceForProduction_rpt (DocumentTypeId 56). */
    public List<Map<String, Object>> pendingInvoices(Map<String, Object> p) {
        user(S_PENDING_INVOICE);
        String branches = branchCsv(p.get("branchIds"));
        if (branches == null) throw new IllegalArgumentException("Select Branch First");
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_PendingInvoiceForProduction_rpt @OrganizationId=?,@CompanyId=?,@FromDate=?,@ToDate=?,@DocumentTypeId=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), day(p.get("fromDate")), day(p.get("toDate")), 56));
        if (num(p.get("supplierId")) > 0) { sql.append(",@SupplierCustomerId=?"); a.add(num(p.get("supplierId"))); }
        if (num(p.get("itemId")) > 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("jobLotId")) > 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("warehouseId")) > 0) { sql.append(",@WareHouseId=?"); a.add(num(p.get("warehouseId"))); }
        sql.append(",@BranchesIds=?"); a.add(branches);
        List<Map<String, Object>> src = jdbc.queryForList(sql.toString(), a.toArray());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id")); m.put("BranchName", ci(r, "BranchName")); m.put("DocNo", ci(r, "DocNo")); m.put("DocDate", dateOnly(ci(r, "DocDate")));
            m.put("SupplierName", ci(r, "SupplierName")); m.put("ManualBillNo", ci(r, "ManualBillNo")); m.put("WareHouse", ci(r, "WareHouseName"));
            m.put("ItemName", ci(r, "ItemName")); m.put("CropYear", ci(r, "CropYear")); m.put("JobLot", ci(r, "JobLotCode"));
            m.put("UOM", ci(r, "UOMCodeItem")); m.put("PackingType", ci(r, "PackTypeCode")); m.put("RateUOM", ci(r, "RateUOM"));
            m.put("ItemQty", ci(r, "ItemQty")); m.put("NetBillWeight", ci(r, "NetBillWeight")); m.put("ItemRate", ci(r, "ItemRate")); m.put("ItemAmount", ci(r, "ItemAmount"));
            out.add(m);
        }
        return out;
    }

    // ---------------------------------------------------------------- 820
    /** HistoryGridFill :219 - BLL.pcc ProductionHeader.Production_Report -> pcc.USP_Production_Report (DocumentTypeId 1850), 3 result sets. */
    public Map<String, Object> concreteProduction(Map<String, Object> p) {
        UserAccount u = user(S_CONCRETE);
        StringBuilder sql = new StringBuilder("EXEC pcc.USP_Production_Report @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@DocumentTypeId=?,@FromDate=?,@ToDate=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), context.currentFinancialYearId(), 1850, day(p.get("fromDate")), day(p.get("toDate"))));
        if (num(p.get("fromDocNo")) > 0) { sql.append(",@FromDocNo=?"); a.add(num(p.get("fromDocNo"))); }
        if (num(p.get("toDocNo")) > 0) { sql.append(",@ToDocNo=?"); a.add(num(p.get("toDocNo"))); }
        if (num(p.get("plantId")) > 0) { sql.append(",@PlantId=?"); a.add(num(p.get("plantId"))); }
        if (num(p.get("productionTypeId")) > 0) { sql.append(",@ProductionTypeId=?"); a.add(num(p.get("productionTypeId"))); }
        List<List<Map<String, Object>>> ds = multi(sql.toString(), a.toArray());
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> main = new ArrayList<>(), in = new ArrayList<>(), outp = new ArrayList<>();
        if (ds.size() > 0) for (Map<String, Object> r : ds.get(0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[]{"ProductionTypeId", "ProductionType", "ItemId", "ItemName", "ItemCode", "ItemUomId"}) m.put(k, ci(r, k));
            m.put("ItemUom", ci(r, "ItemUOM"));
            for (String k : new String[]{"ItemQty", "TotalInputAmount", "TotalAutoRateOutputAmount", "TotalManualRateOutputAmount", "TotalOverHeadAmount", "TotalWagesAmount", "NetFinishAmount"}) m.put(k, ci(r, k));
            main.add(m);
        }
        if (ds.size() > 1) for (Map<String, Object> r : ds.get(1)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[]{"ItemId", "ItemName", "ItemCode", "ItemUomId"}) m.put(k, ci(r, k));
            m.put("ItemUom", ci(r, "UOMCode"));
            for (String k : new String[]{"ItemQty", "NetWeight", "AvgRate", "Amount"}) m.put(k, ci(r, k));
            in.add(m);
        }
        if (ds.size() > 2) for (Map<String, Object> r : ds.get(2)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[]{"ProductionTypeId", "ProductionType", "ItemId", "ItemName", "ItemCode", "ItemAttributeVarientId"}) m.put(k, ci(r, k));
            m.put("Variant", ci(r, "AttributeVarient"));
            m.put("ItemQty", ci(r, "ItemQty")); m.put("VariantQty", ci(r, "VarientQty")); m.put("NetWeight", ci(r, "NetWeight"));
            double vq = ci(r, "VarientQty") instanceof Number ? ((Number) ci(r, "VarientQty")).doubleValue() : 0d;
            double ar = ci(r, "AvgRate") instanceof Number ? ((Number) ci(r, "AvgRate")).doubleValue() : 0d;
            m.put("Rate/VariantQty", vq > 0 ? ar / vq : 0d);   /* :274 */
            m.put("AvgRate", ci(r, "AvgRate")); m.put("Amount", ci(r, "Amount"));
            outp.add(m);
        }
        out.put("main", main); out.put("input", in); out.put("output", outp);
        return out;
    }

    // ---------------------------------------------------------------- 591
    /** Gridfill :233 - ProductionHeader.ProductionSaltSlipAndRegister -> USP_ProductionSaltSlipAndRegister, rows reshaped as the dt columns :246-264. */
    public List<Map<String, Object>> saltRegister(Map<String, Object> p) {
        UserAccount u = user(S_SALT);
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_ProductionSaltSlipAndRegister @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FromDate=?,@ToDate=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), context.currentBranchId(), day(p.get("fromDate")), day(p.get("toDate"))));
        if (num(p.get("warehouseId")) > 0) { sql.append(",@WareHouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (num(p.get("itemId")) > 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("jobLotId")) > 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("itemTypeId")) > 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        /* obj.ItemCategoryId = ToInt(cmbitemcat.Text) :256 - the combo text is the category NAME, so the desktop never filters on it;
           the selected Id is sent here so the filter the user picked takes effect. */
        if (num(p.get("itemCategoryId")) > 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (!str(p.get("productionNo")).isEmpty()) { sql.append(",@ProductionNo=?"); a.add(str(p.get("productionNo"))); }
        List<Map<String, Object>> src = jdbc.queryForList(sql.toString(), a.toArray());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String k : new String[]{"Id", "DocumentTypeId", "DocNo"}) m.put(k, ci(r, k));
            m.put("DocDate", dateOnly(ci(r, "DocDate")));
            for (String k : new String[]{"ProductionNo", "ItemName", "PackingType", "JobLotDescription", "ItemQty", "PackUom", "NetWeight", "ItemRate", "RateUom", "Amount"}) m.put(k, ci(r, k));
            out.add(m);
        }
        return out;
    }
}
