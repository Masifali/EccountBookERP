package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.StoreReportsBRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Date;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management Reports (module 46), group B — three report screens, read-only (no Save).
 *
 * <pre>
 * 454 "Stock Adjustment Report"    ScreenName StockAdjustmentRegister   (base.Name "StockAdjustmentRegister")
 *     Architecture.WinApp.Inventory_Reports.StockAdjustmentRegister (StockAdjustmentRegister.cs)
 *     route /store/reports/stock-adjustment-register, prints 410-InvStockAdjustmentRegister.rpt
 *     (grid rows) and 409-InvStockAdjustmentSlip.rpt (row Print). Document type of the rows: the
 *     Stock Adjustment's own (DocumentTypeId column of the procedure).
 * 455 "Department Request History" ScreenName DepartmentRequestHistory  (base.Name "DepartmentRequestHistory")
 *     Architecture.WinApp.Inventory_Reports.DepartmentRequestHistory (DepartmentRequestHistory.cs)
 *     route /store/reports/department-request-history, prints 450-RptDepartmentRequestRegister.rpt
 *     (grid rows) and 451-RptDepartmentRequestSlip.rpt (row Print, DocumentTypeId 450).
 * 456 "Stock Transfer Report"      ScreenName StockTransferRegister     (base.Name "frmStockTranfserRegister")
 *     Architecture.WinApp.Inventory_Reports.frmStockTranfserRegister (frmStockTranfserRegister.cs)
 *     route /store/reports/stock-transfer-register, prints 408-StockTransferRegister.rpt (grid rows).
 *     Document types 68 (Auto) / 806 (Manual); branch list for DocumentTypeId 68.
 * </pre>
 *
 * BLL / DAL: 0546 InvStockAdjustment.StockAdjustmentSlipAndRegister409; 0068 DepartmentRequest
 * (GetDataForDropDownFromDepartmentRequest, DepartmentRequestHistory); 0559 InvStockTransferHeader
 * (GetBranchesAllocatedToUserFromStockTransfer, AllComboAgainstStockTransfer,
 * StockTransferSlipandRegister); 0571 InvCropYear.Getall; 0582 InvWareHouse.Getall; 0594
 * jobLot.GetAll; 0583 Item.ReadAllItems; 0136 GeneralReprots.StaticColumnNames. All reads go
 * through GenericProvider.GetDataTableProc (DesktopProc.rows). Organization, company, user and
 * financial year always come from the session.
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *  1. 454: the grid is bound to the procedure's own DataTable (DataGridHistory.DataSource =
 *     GrnData + RetrieveStructure, StockAdjustmentRegister.cs:272-273), so every procedure column
 *     is a grid column, including the company columns (CompCountry ... OrgReportingRemarks); only
 *     Id, DocumentTypeId and CompLogoImage are hidden (:291-293).
 *  2. 454: Qty / NetWeight "#,##0.###" (totals "#,##0.##") summed; Amount stringFormatsingle summed;
 *     ItemRate DecimalRateFormate AVERAGED (AggregateFunction 3) (:295-310). Decimal counts are the
 *     configuration values "Default NoofDecimal Points For Amount / Rate" with the desktop's own
 *     fall-backs (CommonServices.GetDecimalConfiguration:5376-5420: amount 1..4 else 0; rate 0 -> 2,
 *     1..4, else 0).
 *  3. 454: Show with no rows clears the grid and says nothing (:276-279); the register print keeps
 *     whatever the last Show fetched (GrnData), even after New.
 *  4. 454: New (Reset :207) clears the five combos' text only - dates and grid are kept.
 *     Refresh (:491) re-reads Item, Entry Type, Job Lot and Warehouse lists, NOT Crop Year.
 *  5. 454: the row Print (409 slip, :397) runs the same procedure with @Id only - no date filter.
 *  6. 455: Item Condition combo is filled (FillAllDropDowns :135) but GridFill never sends it (:220-239).
 *  7. 455: IsApproved defaults to row 1 = "Not Approved" (ComboApprovedfill :173); "Approved" sends
 *     @IsApproved=1, "Not Approved" @IsApproved=0, "All" omits it (BLL 0068 :420, ApprovedFilter).
 *  8. 455: the grid (no @DocumentTypeId) also lists the warehouse variant (DocTypeId 1615), but the
 *     row Print slip (CommonServices.DepartmentSlip451:11607) filters DocumentTypeId 450, so a 1615
 *     row prints "No Record Found For Display".
 *  9. 455: no rows -> grid cleared + "Record Not found For Display" (:269-270). The register print
 *     pushes the procedure's raw rows (dt), not the reshaped grid table (:355).
 * 10. 455: the "New" button (toolStripButton1) has no Click handler (designer :731-770 wires only
 *     Refresh, Print, Show and the grid), and Reset (:182) is reached only from the form's Ctrl+N,
 *     which never fires because KeyPreview is off. The page shows the New button without an action.
 *     (Reset itself would reset the dates, clear Department Name and the grid, but not dt.)
 * 11. 456: the grid columns and their order come from the designer layout's column sets
 *     (frmStockTranfserRegister.resx grdfrm_DesignTimeLayout), captions from its columns; Equivalent
 *     is re-captioned "Pack Size" (:360); EntryDate / IsApproved / ApprovedDate / ApprovedUserName
 *     are hidden (:415-418), Id hidden (:414). Default sort: Qty ascending (layout SortKey ColIndex 22).
 * 12. 456: dtlst (:302-332) names two columns "OtherWeight." and "EbTotal " (trailing dot / space)
 *     while the layout binds "OtherWeight" / "EbTotal", so the Other Weight and Eb Total columns
 *     show blank. The projection below keeps the desktop's column names, so they stay blank here too.
 * 13. 456: row values pass through Conversion.ToInt/ToDouble/ToString (:335): BiltyNo is ToInt
 *     (a non-numeric bilty number shows 0), DoNo / TicketNo / WorkingReportNo / EbUnit null -> 0,
 *     DocDate / ApprovedDate cut to the date (ToShortDateString), a null date becomes 01-Jan-1900.
 * 14. 456: Qty "#,#" (zero prints empty), WbNetWeight / GrossWeight / NetWeight / DiffWeight
 *     "#,##0.##" with totals, AdLsWeight "#,##0.##" summed with no TotalFormatString (:391-413).
 * 15. 456: Show requires a branch ("Select Branch First", :346-347); Branch ids are sent as
 *     ",id1,id2" (leading comma, :295). Document Type: none chosen -> 0 (not sent), Auto -> 68,
 *     anything else -> 806 (:285).
 * 16. 456: AllComboAgainstStockTransfer - the form sets DocumentTypeId = 608, but the BLL reads only
 *     DocumentTypeIds, so no document type is sent (:123 / BLL 0559 :913). The procedure's
 *     'JobLotTo' rows carry the FROM job lot id with the TO job lot's name and ignore @BranchesIds;
 *     the To JobLot filter therefore sends that id as @ToJobLotId.
 * 17. 456: the branch list procedure's WHERE ends "and @DocumentTypeId Is Null Or p.DocumentTypeId =
 *     @DocumentTypeId" without brackets, so with 68 it lists every branch that has an Auto transfer,
 *     allocated to this user or not. The desktop offers that list; so does the page.
 * 18. 456: New (btnRefresh_Click :480) resets the dates, clears Item Name only, then Shows;
 *     Refresh (:495) re-reads the combos and the document types. Load Shows immediately (:261).
 * 19. 456: ComboBind binds Item / From JobLot / To JobLot / From / To Warehouse with BindDDL(ZeroIndex
 *     true) (:182-186, DropDownBind.cs:48-93): a "...Select Any Value..." row (0) is inserted and
 *     Value = 0 on every bind, so leaving the branch list or pressing Refresh drops the selections.
 *     Leaving the branch list with nothing ticked blanks the five combos (Text empty, no active
 *     row, :548-561).
 * 20. 454 / 455: every combo is bound only when its list has rows (454 :131-199; 455 per Activity
 *     group :138-150), otherwise it keeps what it had. 454/455 binders use ZeroIndex false: no
 *     default row, the combo starts empty.
 * 21. 455: IsApproved is bound with BindDDL(ZeroIndex false) and row 1 activated (:172-173).
 *     Department Name value goes to ReportsParameters.RefDocumentTypeId, which BLL 0068 :380-386
 *     sends as @DepartmentId.
 *
 * DEVIATIONS (web only)
 *  1. 454: on the desktop, gridHisory() first copies GrnData into an unused dtHistory whose "Id"
 *     column is int but receives Conversion.ToDateTime(Id).ToShortDateString() ("1/1/1900") -
 *     DataTable.Rows.Add throws, the catch shows the message and the grid is NEVER bound when rows
 *     exist (:268-270). The dead copy is not ported: the grid shows GrnData as :272-274 intends.
 *     GrnData itself is assigned before the throw, so the desktop's 410-Register print is unaffected.
 *  2. 456: posted branch ids are matched against the branch list; if none of the posted ids is in it
 *     the Show is refused ("Select Branch First") instead of running with no branch filter (the
 *     desktop's BranchesIds = "" -> all branches only arises from a typed name that matches nothing).
 *  3. Crystal layouts (409, 410, 450, 451, 408) are out of scope: the print endpoints return the
 *     exact rows the desktop pushes into each template; the page prints them as a table. None of
 *     these templates has a ReportRegistry key yet (see the contracts in the port report).
 *  4. Dates are sent as dates (yyyy-MM-dd). Every procedure here declares its date parameters as
 *     SQL date, so the time part the desktop pickers carry is dropped by SQL Server anyway.
 *  5. Rights: none of the three forms reads a right; page access is left to the sidebar Authority.
 *  6. Layout: each page reproduces its designer geometry 1:1 in px (454 ClientSize 964 x 661, 455
 *     978 x 610, 456 998 x 610; teal caption / History bars, label and control Location/Size), with
 *     the history grid below the filters as on the desktop and a web-only footer History button that
 *     scrolls to it. Native date inputs show the browser's date format, not "dd-MMM-yy", and a
 *     cleared date input is put back to its last value (a DateTimePicker is never empty).
 *  7. The ctrlGrdBar gear (saved layouts / print / export) is shown disabled.
 *  8. Source: the newer desktop build (recovery\resolved-source) named by the porting brief was not
 *     on the device when this was re-checked (2026-09-25); the port follows
 *     recovered_source\ECCOUNTBOOKERP\Architecture.WinApp.Inventory_Reports (same sizes on device).
 *
 * NOT PORTED: saved grid layouts (ctrlGrdBar / GetGridLayout), the attachment viewer behind the
 * NoOfAttachments link, keyboard shortcuts (Ctrl+P / Ctrl+N / Ctrl+E), grid filter row / group-by box.
 */
@Service
public class StoreReportsBService {

    private static final Logger LOG = LoggerFactory.getLogger(StoreReportsBService.class);

    /** frmStockTranfserRegister.BranchesFill:220 — the branch list is read for DocumentTypeId 68. */
    private static final int ST_BRANCH_DOCUMENT_TYPE = 68;
    /** frmStockTranfserRegister.GridFill:285 — Auto / Manual. */
    private static final int ST_AUTO = 68;
    private static final int ST_MANUAL = 806;
    /** CommonServices.DepartmentSlip451:11612. */
    private static final int DEPT_REQ_SLIP_DOCUMENT_TYPE = 450;

    /**
     * Sp_StockAdjustmentSlipAndRegister's result, in SELECT order (r.* from #Data, then the company
     * columns). RetrieveStructure makes each of them a grid column; CompLogoImage is hidden (:293)
     * and binary, so it is not shipped.
     */
    static final List<String> SA_COLUMNS = Arrays.asList(
            "Id", "DocumentTypeId", "DocDate", "DocNo", "RemarksHeader", "EntryType", "ItemName", "PackUom",
            "CropYear", "JobLotDescription", "PackTypeDesc", "Qty", "NetWeight", "ItemRate", "RateUom", "Amount",
            "WareHouseName", "EnteryDate", "EntryUser", "ModifyDate", "ModifyUser", "NoOfAttachments",
            "CompCountry", "CompContactPerson", "CompMobileA", "CompMobileB", "CompMobileC", "CompEmailA",
            "CompEmailB", "CompanyWebsite", "CompanyFaxNo", "OrgReportingRemarks");

    private final StoreReportsBRepository repo;
    private final StoreIssuanceRepository common;
    private final CurrentUserContext ctx;

    public StoreReportsBService(StoreReportsBRepository repo, StoreIssuanceRepository common, CurrentUserContext ctx) {
        this.repo = repo;
        this.common = common;
        this.ctx = ctx;
    }

    // ============================================================================ 454

    /** StockAdjustmentRegister_Load:107 - CropYearBind, WareHouseNameFill, JobLotFill, EntryTypeBind, ItemBind. */
    public Map<String, Object> saLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));                   // :133 BindDDLNew Id/CropYear
        out.putAll(saRefreshLists(u));
        out.put("fromDate", repo.financialYearStart(u, ctx.currentFinancialYearId()));      // :118 ActiveYr.Start_Period
        out.put("toDate", LocalDate.now().toString());                                       // designer: picker Value = Now
        out.put("amountDecimals", amountDecimals(u));
        out.put("rateDecimals", rateDecimals(u));
        return out;
    }

    /** toolStripButton1_Click:491 - ItemBind, EntryTypeBind, JobLotFill, WareHouseNameFill (no crop year). */
    public Map<String, Object> saRefresh() {
        return saRefreshLists(ctx.requireAccountingUser());
    }

    private Map<String, Object> saRefreshLists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("warehouses", project(repo.warehouses(u), "Id", "WareHouseName"));             // :149
        out.put("jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"));               // :165
        out.put("entryTypes", project(repo.staticColumns("StockAdjustmentType"), "Id", "Type"));  // :181
        out.put("items", project(repo.readAllItems(u), "Id", "ItemName"));                     // :197 BindDDL Id/ItemName
        return out;
    }

    /** gridHisory:224 - the Show button. */
    public Map<String, Object> saShow(String fromDate, String toDate, int itemId, int cropYearId, int jobLotId,
                                      int warehouseId, int entryTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> raw = repo.stockAdjustmentRegister(u, day(fromDate), day(toDate), 0,
                itemId, jobLotId, warehouseId, entryTypeId, cropYearId);                    // :230-240
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("columns", SA_COLUMNS);
        out.put("rows", pick(raw, SA_COLUMNS));
        return out;
    }

    /** GenerateReport:384 - the row's Print button -> 409-InvStockAdjustmentSlip.rpt rows (Id only). */
    public List<Map<String, Object>> saSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("Not Record Found For Display");
        List<Map<String, Object>> rows = repo.stockAdjustmentRegister(u, null, null, id, 0, 0, 0, 0, 0);
        if (rows.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");   // :403-406
        return printable(rows);
    }

    // ============================================================================ 455

    /** frmGatePassReport_Load:198 - dates, FillAllDropDowns, ComboApprovedfill. */
    public Map<String, Object> drLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>(drDropDowns(u));
        /* ComboApprovedfill:166-172 - the form's own three rows; row 1 activated (:173). */
        List<Map<String, Object>> approved = new ArrayList<>();
        approved.add(idName(1, "Approved"));
        approved.add(idName(2, "Not Approved"));
        approved.add(idName(3, "All"));
        out.put("approved", approved);
        out.put("approvedDefault", "Not Approved");
        out.put("fromDate", repo.financialYearStart(u, ctx.currentFinancialYearId()));      // :203
        out.put("toDate", LocalDate.now().toString());                                       // :204
        return out;
    }

    /** btnRefresh_Click:407 - FillAllDropDowns only. */
    public Map<String, Object> drRefresh() {
        return drDropDowns(ctx.requireAccountingUser());
    }

    /**
     * FillAllDropDowns:103 - one call, split on the Activity column; each group is bound as
     * (Id, ReferenceName). An empty result leaves the combos as they were (:116-119): "empty" is
     * returned so the page does the same.
     */
    private Map<String, Object> drDropDowns(UserAccount u) {
        List<Map<String, Object>> rows = repo.departmentRequestDropDowns(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("empty", rows.isEmpty());
        out.put("departments", byActivity(rows, "DepartmentFrom"));
        out.put("items", byActivity(rows, "Item"));
        out.put("assets", byActivity(rows, "Asset"));
        out.put("itemConditions", byActivity(rows, "ItemCondition"));
        return out;
    }

    /**
     * GridFill:214. Returns the reshaped grid table (:243-263) and the raw procedure rows (dt),
     * which is what 450-Register prints (:355).
     */
    public Map<String, Object> drShow(String fromDate, String toDate, int departmentId, int itemId, int assetId,
                                      String approved) {
        UserAccount u = ctx.requireAccountingUser();
        Boolean isApproved;
        String a = approved == null ? "" : approved;
        if ("All".equals(a)) isApproved = null;                    // :236-239 ApprovedFilter "All" -> not sent
        else isApproved = "Approved".equals(a);                    // :228-235; any other text leaves the default false
        List<Map<String, Object>> raw = repo.departmentRequestHistory(u, 0, 0, departmentId, itemId, assetId,
                day(fromDate), day(toDate), isApproved, 0);
        List<Map<String, Object>> table = new ArrayList<>();
        for (Map<String, Object> r : raw) {                         // :260-263
            Map<String, Object> t = new LinkedHashMap<>();
            t.put("Id", cToInt(ci(r, "Id")));
            t.put("DocTypeId", cToInt(ci(r, "DocTypeId")));
            t.put("DocDate", dateText(ci(r, "DocDate")));
            t.put("DocNo", cToInt(ci(r, "DocNo")));
            t.put("DepartmentNameFrom", str(ci(r, "DepartmentNameFrom")));
            t.put("ToDepartmentName", str(ci(r, "ToDepartmentName")));
            t.put("RequestedQty", cToDouble(ci(r, "RequestedQty")));
            t.put("ItemName", str(ci(r, "ItemName")));
            t.put("UOMCode", str(ci(r, "UOMCode")));
            t.put("ItemCondition", str(ci(r, "ItemCondition")));
            t.put("AssetName", str(ci(r, "AssetName")));
            t.put("EntryDate", dateText(ci(r, "EntryDate")));
            t.put("EntryUser", str(ci(r, "EntryUser")));
            t.put("ModifyDate", dateText(ci(r, "ModifyDate")));
            t.put("ModifyUser", str(ci(r, "ModifyUser")));
            t.put("NoOfAttachments", cToInt(ci(r, "NoOfAttachments")));
            table.add(t);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", table);
        out.put("raw", printable(raw));
        if (raw.isEmpty()) out.put("message", "Record Not found For Display");               // :270
        return out;
    }

    /** grdfrm_ColumnButtonClick:363 -> CommonServices.DepartmentSlip451 (Id, DocumentTypeId 450, ApprovedFilter "All"). */
    public List<Map<String, Object>> drSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = id == 0 ? new ArrayList<>()
                : repo.departmentRequestHistory(u, DEPT_REQ_SLIP_DOCUMENT_TYPE, id, 0, 0, 0, null, null, null, 0);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");  // :11615-11618
        return printable(rows);
    }

    // ============================================================================ 456

    /** frmGatePassReport_Load:244 - dates, BranchesFill, DocumentTypefill (combos: stCombos). */
    public Map<String, Object> stLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> branches = stBranchList(u);
        out.put("branches", branches);
        /* :235 CmbBranchName.Text = UserAccount.BranchName - the signed-in user's own branch ticked. */
        List<Integer> def = new ArrayList<>();
        Integer own = u.getBranchesId();
        for (Map<String, Object> b : branches) if (own != null && toInt(b.get("Id")) == own) def.add(own);
        out.put("defaultBranchIds", def);
        out.put("documentTypes", stDocumentTypes());
        out.put("fromDate", repo.financialYearStart(u, ctx.currentFinancialYearId()));      // :254 / :260
        out.put("toDate", LocalDate.now().toString());                                       // :255
        return out;
    }

    /** DocumentTypefill:198 - the form's own two rows, BindDDLNew, no row active. */
    public List<Map<String, Object>> stDocumentTypes() {
        List<Map<String, Object>> dt = new ArrayList<>();
        dt.add(idName(1, "Auto"));
        dt.add(idName(2, "Manual"));
        return dt;
    }

    /**
     * ComboBind:114 - one Usp_AllComboAgainstStockTransfer call for the ticked branches, split on
     * Activity (:158-181). "empty" = the procedure returned nothing, in which case the desktop
     * returns early and keeps the old lists (:139-142).
     */
    public Map<String, Object> stCombos(List<Integer> branchIds) {
        UserAccount u = ctx.requireAccountingUser();
        String csv = stBranchCsv(u, branchIds, false);
        List<Map<String, Object>> rows = repo.stockTransferCombos(u, csv);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("empty", rows.isEmpty());
        out.put("items", byActivity(rows, "ItemName"));
        out.put("jobLotsFrom", byActivity(rows, "JobLotFrom"));
        out.put("jobLotsTo", byActivity(rows, "JobLotTo"));
        out.put("warehousesFrom", byActivity(rows, "WareHouseFrom"));
        out.put("warehousesTo", byActivity(rows, "WareHouseTo"));
        return out;
    }

    /** GridFill:269. documentType: "" (no row active) / "Auto" / "Manual". */
    public Map<String, Object> stShow(List<Integer> branchIds, String fromDate, String toDate, int itemId,
                                      int fromWarehouseId, int toWarehouseId, int jobLotId, int toJobLotId,
                                      String documentType) {
        UserAccount u = ctx.requireAccountingUser();
        String csv = stBranchCsv(u, branchIds, true);
        int docType = (documentType == null || documentType.isEmpty()) ? 0
                : ("Auto".equals(documentType) ? ST_AUTO : ST_MANUAL);                          // :285
        List<Map<String, Object>> raw = repo.stockTransferRegister(u, docType, 0, itemId, day(fromDate), day(toDate),
                fromWarehouseId, toWarehouseId, jobLotId, toJobLotId, csv);                    // :299
        List<Map<String, Object>> table = new ArrayList<>();
        for (Map<String, Object> r : raw) {                         // :333-336, dtlst's own column names
            Map<String, Object> t = new LinkedHashMap<>();
            t.put("Id", cToInt(ci(r, "Id")));
            t.put("DocNo", cToInt(ci(r, "DocNo")));
            t.put("DocDate", shortDate(ci(r, "DocDate")));
            t.put("WbNetWeight", cToDouble(ci(r, "WbNetWeight")));
            t.put("OtherWeight.", cToDouble(ci(r, "OtherWeight")));
            t.put("DiffWeight", cToDouble(ci(r, "DiffWeight")));
            t.put("EntryDate", dateText(toDateTime(ci(r, "EntryDate"))));
            t.put("IsApproved", cToString(ci(r, "IsApproved")));
            t.put("ApprovedUserId ", cToInt(ci(r, "ApprovedUserId")));
            t.put("ApprovedDate", shortDate(ci(r, "ApprovedDate")));
            t.put("TransferType", cToString(ci(r, "TransferType")));
            t.put("GpSrNo", cToInt(ci(r, "GpSrNo")));
            t.put("VehicleNo", cToString(ci(r, "VehicleNo")));
            t.put("BiltyNo", cToInt(ci(r, "BiltyNo")));
            t.put("DoNo", cToInt(ci(r, "DoNo")));
            t.put("ItemName", cToString(ci(r, "ItemName")));
            t.put("Equivalent", cToDouble(ci(r, "Equivalent")));
            t.put("ToWareHouseName", cToString(ci(r, "ToWareHouseName")));
            t.put("JobLotTo", cToString(ci(r, "JobLotTo")));
            t.put("TicketNo", cToInt(ci(r, "TicketNo")));
            t.put("WorkingReportNo", cToInt(ci(r, "WorkingReportNo")));
            t.put("CropYear", cToString(ci(r, "CropYear")));
            t.put("Qty", cToDouble(ci(r, "Qty")));
            t.put("GrossWeight", cToDouble(ci(r, "GrossWeight")));
            t.put("EbUnit", cToInt(ci(r, "EbUnit")));
            t.put("EbTotal ", (double) cToInt(ci(r, "EbTotal")));
            t.put("AdLsWeight", cToDouble(ci(r, "AdLsWeight")));
            t.put("NetWeight", cToDouble(ci(r, "NetWeight")));
            t.put("EntryUserName", cToString(ci(r, "EntryUserName")));
            t.put("ApprovedUserName", cToString(ci(r, "ApprovedUserName")));
            table.add(t);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", table);
        out.put("raw", printable(raw));
        return out;
    }

    /** BranchesFill:220 - (BranchId, BranchName). */
    private List<Map<String, Object>> stBranchList(UserAccount u) {
        return project(repo.stockTransferBranches(u, ST_BRANCH_DOCUMENT_TYPE), "BranchId", "BranchName");
    }

    /**
     * :286-298 - "," + id for each ticked branch, in list order. Only ids in the branch list count
     * (the desktop can only tick listed rows). For Show, nothing ticked is "Select Branch First".
     */
    private String stBranchCsv(UserAccount u, List<Integer> posted, boolean requireOne) {
        Set<Integer> want = new LinkedHashSet<>(posted == null ? new ArrayList<>() : posted);
        StringBuilder sb = new StringBuilder();
        if (!want.isEmpty()) {
            for (Map<String, Object> b : stBranchList(u)) {
                int id = toInt(b.get("Id"));
                if (want.contains(id)) sb.append(',').append(id);
            }
        }
        if (requireOne && sb.length() == 0) throw new IllegalArgumentException("Select Branch First");
        return sb.toString();
    }

    // ============================================================================ helpers

    private int amountDecimals(UserAccount u) {
        int n = cToInt(safeConfig(u, "Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 0;
    }

    private int rateDecimals(UserAccount u) {
        int n = cToInt(safeConfig(u, "Default NoofDecimal Points For Rate"));
        if (n == 0) return 2;
        return n >= 1 && n <= 4 ? n : 0;
    }

    private String safeConfig(UserAccount u, String key) {
        try { return common.config(u, key); }
        catch (Exception e) { LOG.warn("Could not read configuration '{}'", key, e); return ""; }
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("Name", name);
        return m;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(idName(toInt(ci(r, id)), str(ci(r, name))));
        return out;
    }

    /** Rows whose Activity column equals the key, as (Id, ReferenceName). */
    private static List<Map<String, Object>> byActivity(List<Map<String, Object>> rows, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (activity.equals(str(ci(r, "Activity")))) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        }
        return out;
    }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, List<String> cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (String c : cols) m.put(c, jsonValue(ci(r, c)));
            out.add(m);
        }
        return out;
    }

    /** The procedure rows as pushed into the .rpt, minus binary columns (CompLogoImage). */
    private static List<Map<String, Object>> printable(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                if (e.getValue() instanceof byte[]) continue;
                m.put(e.getKey(), jsonValue(e.getValue()));
            }
            out.add(m);
        }
        return out;
    }

    private static Object jsonValue(Object v) {
        if (v instanceof java.util.Date || v instanceof LocalDateTime || v instanceof LocalDate) return dateText(v);
        return v;
    }

    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    /** A DateTime cell as "yyyy-MM-ddTHH:mm:ss"; DBNull stays null (blank cell). */
    static String dateText(Object v) {
        LocalDateTime t = asDateTime(v);
        return t == null ? null : t.format(ISO);
    }

    private static LocalDateTime asDateTime(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        if (v instanceof LocalDateTime) return (LocalDateTime) v;
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay();
        String s = String.valueOf(v).trim();
        try { return LocalDateTime.parse(s.length() > 19 ? s.substring(0, 19).replace(' ', 'T') : s.replace(' ', 'T')); }
        catch (Exception e) {
            try { return LocalDate.parse(s.substring(0, 10)).atStartOfDay(); } catch (Exception e2) { return null; }
        }
    }

    /** Conversion.ToDateTime - DBNull / unparseable -> 01-Jan-1900. */
    private static LocalDateTime toDateTime(Object v) {
        LocalDateTime t = asDateTime(v);
        return t == null ? LocalDate.of(1900, 1, 1).atStartOfDay() : t;
    }

    /** Conversion.ToDateTime(x).ToShortDateString() stored back into a DateTime column: midnight. */
    private static String shortDate(Object v) {
        return dateText(toDateTime(v).toLocalDate().atStartOfDay());
    }

    /**
     * Conversion.ToInt = Convert.ToInt32 with every failure 0: floating values round half to even,
     * strings must be a plain integer (Int32.Parse), bool is 1/0.
     */
    static int cToInt(Object v) {
        if (v == null) return 0;
        try {
            if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
            if (v instanceof Integer || v instanceof Short || v instanceof Byte) return ((Number) v).intValue();
            if (v instanceof Long) return Math.toIntExact((Long) v);
            if (v instanceof Number) {
                return new BigDecimal(v.toString()).setScale(0, RoundingMode.HALF_EVEN).intValueExact();
            }
            String s = String.valueOf(v).trim();
            if (s.isEmpty()) return 0;
            return Integer.parseInt(s.startsWith("+") ? s.substring(1) : s);
        } catch (Exception e) {
            return 0;
        }
    }

    /** Conversion.ToDouble - failures and infinities are 0. */
    static double cToDouble(Object v) {
        if (v == null) return 0d;
        try {
            double d = v instanceof Number ? ((Number) v).doubleValue()
                    : v instanceof Boolean ? (((Boolean) v) ? 1d : 0d)
                    : Double.parseDouble(String.valueOf(v).trim());
            return Double.isInfinite(d) || Double.isNaN(d) ? 0d : d;
        } catch (Exception e) {
            return 0d;
        }
    }

    /** Conversion.ToString - DBNull is ""; bool prints True/False as .NET does. */
    static String cToString(Object v) {
        if (v == null) return "";
        if (v instanceof Boolean) return ((Boolean) v) ? "True" : "False";
        return String.valueOf(v);
    }

    /** A picker value "yyyy-MM-dd..." as a SQL date; blank -> null (CheckDateTimeNull -> not sent). */
    static Date day(String s) {
        if (s == null || s.trim().length() < 10) return null;
        try { return Date.valueOf(LocalDate.parse(s.trim().substring(0, 10))); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }
}
