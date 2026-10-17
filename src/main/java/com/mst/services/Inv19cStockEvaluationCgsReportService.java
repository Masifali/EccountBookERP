package com.mst.services;

import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;

import java.io.File;
import java.sql.Types;
import java.util.*;

/**
 * frmStockEvaluationCgsReport (Architecture.WinApp.Inventory_Stocks_Report/frmStockEvaluationCgsReport.cs) - ScreenDefinition 874
 * "Stock Evaluation Cgs Report" (Inventory hub, module 19 Stock Reports). Line refs ":NNN" are that file.
 * Data: BLL InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales -> USP_GetDataFromInventoryStocksEvaluationsForSales
 * (ComboData :292, ReportType "Sale" = @DocType, DocumentTypeIds "95,96,99,100,103,128,211" = @CustomDocumentTypeIds);
 * InventoryStockEvalautionDetail.SaleReportWithGrossProfitsAndLossNew -> USP_SaleReportWithGrossProfitsAndLossNew (GridFill :483);
 * CommonServices.DynamicReportsLoad("EvaluationCgsReport") -> files of reports.crystal.template-root/EvaluationCgsReport (ReportsLoad :259);
 * CommonServices.DateType() -> fixed list (Datetypefill :278, filled client side).
 * The BLL/DAL sources are not in the recovered tree: the parameter guards below follow the BLL convention of this code base
 * (identity + dates + ActivityName always, every other value only when it is not 0 / not empty).
 */
@Service
public class Inv19cStockEvaluationCgsReportService {

    public static final int SCREEN_ID = 874;
    private static final String REPORT_FOLDER = "EvaluationCgsReport";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    public Inv19cStockEvaluationCgsReportService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
        this.jdbc = jdbc; this.context = context; this.rights = rights;
    }

    private static int num(Object o) {
        if (o instanceof Number) return ((Number) o).intValue();
        if (o == null || String.valueOf(o).isBlank()) return 0;
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static boolean truthy(Object o) { String s = str(o).toLowerCase(Locale.ROOT); return s.equals("true") || s.equals("1"); }
    private static SqlParameterValue day(String iso) {
        return new SqlParameterValue(Types.TIMESTAMP, java.sql.Timestamp.valueOf(iso.substring(0, 10) + " 00:00:00"));
    }

    /** InitializeComponentMethod :203 - ComboData + ReportsLoad; clsGlobalVariables.ActiveYr.Start_Period for Date Type 5 (:456). */
    public Map<String, Object> init() {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        var fy = jdbc.queryForList("SELECT Start_Period FROM dbo.FinancialYear WHERE Id=?", context.currentFinancialYearId());
        out.put("fyStart", fy.isEmpty() || fy.get(0).get("Start_Period") == null ? java.time.LocalDate.now().toString() : String.valueOf(fy.get(0).get("Start_Period")).substring(0, 10));
        out.put("combos", combos());
        out.put("dynamicReports", dynamicReports());
        return out;
    }

    /** ComboData :292 - InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales (Id, RefName, Activity);
     *  Activity values used by AllComboBind :346: GetItemClass, GetParentCategory, GetItemCategory, GetItemType, GetItems, GetWarehouse,
     *  GetJobLot, GetCropYear, GetPackingType, GetSupplierCustomer, GetDocumentType, GetCity. Also btnRefresh_Click :907. */
    public List<Map<String, Object>> combos() {
        return jdbc.queryForList("EXEC dbo.USP_GetDataFromInventoryStocksEvaluationsForSales @OrganizationId=?,@CompanyId=?,@DocType=?,@CustomDocumentTypeIds=?,@AppId=?,@UserId=?",
                context.currentOrganizationId(), context.currentCompanyId(), "Sale", "95,96,99,100,103,128,211", context.currentAppId(), context.currentUserId());
    }

    /** ReportsLoad :259 - CommonServices.DynamicReportsLoad("EvaluationCgsReport"): file names without extension (tsDrowdown items). */
    public List<String> dynamicReports() {
        List<String> out = new ArrayList<>();
        if (reportRoot == null || reportRoot.isBlank()) return out;
        File[] files = new File(reportRoot, REPORT_FOLDER).listFiles();
        if (files == null) return out;
        Arrays.sort(files, Comparator.comparing(f -> f.getName().toLowerCase(Locale.ROOT)));
        for (File f : files) {
            String n = f.getName();
            if (!f.isFile() || !n.toLowerCase(Locale.ROOT).endsWith(".rpt")) continue;
            int dot = n.lastIndexOf('.');
            out.add(dot > 0 ? n.substring(0, dot) : n);
        }
        return out;
    }

    /** GridFill :483 -> InventoryStockEvalautionDetail.SaleReportWithGrossProfitsAndLossNew (USP_SaleReportWithGrossProfitsAndLossNew).
     *  "Please Add Atleast One Check!" :559 when ReportType is Summary and no Add-* box is ticked. */
    public List<Map<String, Object>> show(Map<String, Object> p) {
        rights.require(context.requireAccountingUser(), SCREEN_ID, "View");
        String reportType = str(p.get("reportType"));
        boolean summary = "Summary".equals(reportType);
        String[] flags = { "documentTypeWise", "documentWise", "customerWise", "warehouseWise", "itemWise", "cropYearWise", "jobLotWise", "packingTypeWise", "uomWise", "cityWise" };
        String[] flagParams = { "@AddDocumentType", "@AddDoc", "@AddCustomerId", "@AddWarehouse", "@AddItem", "@AddCropYear", "@AddJobLotId", "@AddPackingTypeId", "@AddUom", "@AddCity" };
        if (summary) {
            boolean any = false;
            for (String f : flags) any |= truthy(p.get(f));
            if (!any) throw new IllegalArgumentException("Please Add Atleast One Check!");
        }
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_SaleReportWithGrossProfitsAndLossNew @OrganizationId=?,@CompanyId=?,@FinancialYearId=?");
        List<Object> a = new ArrayList<>(List.of(context.currentOrganizationId(), context.currentCompanyId(), context.currentFinancialYearId()));
        if (!str(p.get("fromDate")).isEmpty()) { sql.append(",@FromDate=?"); a.add(day(str(p.get("fromDate")))); }
        if (!str(p.get("toDate")).isEmpty()) { sql.append(",@ToDate=?"); a.add(day(str(p.get("toDate")))); }
        if (num(p.get("fromDocNo")) != 0) { sql.append(",@DocNoFrom=?"); a.add(num(p.get("fromDocNo"))); }
        if (num(p.get("toDocNo")) != 0) { sql.append(",@DocNoTo=?"); a.add(num(p.get("toDocNo"))); }
        if (num(p.get("itemClassId")) != 0) { sql.append(",@ItemClassGroupId=?"); a.add(num(p.get("itemClassId"))); }
        if (num(p.get("parentCategoryId")) != 0) { sql.append(",@InventoryParentCategories=?"); a.add(num(p.get("parentCategoryId"))); }
        if (num(p.get("itemCategoryId")) != 0) { sql.append(",@ItemCategoryId=?"); a.add(num(p.get("itemCategoryId"))); }
        if (num(p.get("itemTypeId")) != 0) { sql.append(",@ItemTypeId=?"); a.add(num(p.get("itemTypeId"))); }
        if (num(p.get("itemId")) != 0) { sql.append(",@ItemId=?"); a.add(num(p.get("itemId"))); }
        if (num(p.get("warehouseId")) != 0) { sql.append(",@WarehouseId=?"); a.add(num(p.get("warehouseId"))); }
        if (!str(p.get("cropYear")).isEmpty()) { sql.append(",@CropYear=?"); a.add(str(p.get("cropYear"))); }
        if (num(p.get("jobLotId")) != 0) { sql.append(",@JobLotId=?"); a.add(num(p.get("jobLotId"))); }
        if (num(p.get("packingTypeId")) != 0) { sql.append(",@PackingTypeId=?"); a.add(num(p.get("packingTypeId"))); }
        if (num(p.get("documentTypeId")) != 0) { sql.append(",@DocumentTypeId=?"); a.add(num(p.get("documentTypeId"))); }
        if (num(p.get("supplierCustomerId")) != 0) { sql.append(",@SupplierCustomerId=?"); a.add(num(p.get("supplierCustomerId"))); }
        if (num(p.get("cityId")) != 0) { sql.append(",@CityId=?"); a.add(num(p.get("cityId"))); }
        sql.append(",@ActivityName=?"); a.add(reportType);
        if (summary) {
            for (int i = 0; i < flags.length; i++) {
                if (truthy(p.get(flags[i]))) { sql.append(",").append(flagParams[i]).append("=?"); a.add(1); }
            }
        }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }
}
