package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseInvoiceHistoryRepository;
import com.mst.repositories.PurchaseReportsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * 476 Architecture.WinApp.Inventory_Reports.frmPurchaseInvoiceHistory "Purchase Report" (Purchase Reports, module 52).
 * Desktop methods are cited as ":NNN" = line in frmPurchaseInvoiceHistory.cs. Organization / company / user /
 * financial year always come from the session. The form itself checks no rights beyond opening the screen; the
 * web requires the screen's View grant.
 */
@Service
public class PurchaseInvoiceHistoryService {
    public static final int SCREEN = 476;
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private final PurchaseInvoiceHistoryRepository repo;
    private final PurchaseReportsRepository shared;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final DesktopAttachmentStore attachmentStore;

    public PurchaseInvoiceHistoryService(PurchaseInvoiceHistoryRepository repo, PurchaseReportsRepository shared,
                                         CurrentUserContext context, DesktopReportRights rights,
                                         DesktopAttachmentStore attachmentStore) {
        this.repo = repo; this.shared = shared; this.context = context; this.rights = rights; this.attachmentStore = attachmentStore;
    }

    /** CommonServices.DateType() - the fixed five rows. */
    private static final List<Map<String, Object>> DATE_TYPES = List.of(
            Map.of("Id", 1, "Parameters", "This Day"), Map.of("Id", 2, "Parameters", "This Week"),
            Map.of("Id", 3, "Parameters", "This Month"), Map.of("Id", 4, "Parameters", "This Year"),
            Map.of("Id", 5, "Parameters", "Financial Year"));
    /** ReportTypeFill :387. */
    private static final List<Map<String, Object>> REPORT_TYPES = List.of(
            Map.of("Id", 1, "ReportName", "Current & Total Purchases"), Map.of("Id", 2, "ReportName", "PurchsaeDetail"),
            Map.of("Id", 3, "ReportName", "AvgRateByItem"), Map.of("Id", 4, "ReportName", "AvgRateBySupplier"));
    /** SupplierGroupFill :407. */
    private static final List<Map<String, Object>> GROUPS = List.of(
            Map.of("Id", 1, "GroupName", "GroupByItem"), Map.of("Id", 2, "GroupName", "GroupBySupplier"),
            Map.of("Id", 3, "GroupName", "GroupItem&CropYear"));

    private UserAccount user() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN, "View");
        return u;
    }

    // ------------------------------------------------------------------ load

    /** frmPurchaseInvoiceHistory_Load :184 - dropdown bind (no branch text yet), report types, custom groups, groups, date types, branches. */
    public Map<String, Object> init() {
        UserAccount u = user();
        var branches = shared.purchaseInvoiceBranches(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", plainRows(branches));
        int current = num(u.getBranchesId());
        out.put("branchId", branches.stream().anyMatch(r -> num(r.get("BranchId")) == current) ? current : 0);
        out.put("dateTypes", DATE_TYPES);
        out.put("reportTypes", REPORT_TYPES);
        out.put("groups", GROUPS);
        out.put("yearStart", shared.yearStart(u, context.currentFinancialYearId()));
        out.put("now", STAMP.format(LocalDateTime.now()));
        int amount = num(shared.configuration(u, "Default NoofDecimal Points For Amount").trim());
        int rate = num(shared.configuration(u, "Default NoofDecimal Points For Rate").trim());
        out.put("amountDecimals", amount >= 1 && amount <= 4 ? amount : 2);
        out.put("rateDecimals", rate >= 1 && rate <= 4 ? rate : rate == 0 ? 2 : 0);
        out.put("customGroups", plainRows(repo.customGroups(u)).stream()
                .map(r -> row("Id", r.get("Id"), "Name", r.get("AcLookUpsDescription"))).toList());
        out.put("lookups", split(repo.dropdown(u, "")));
        return out;
    }

    /** GetDataForPurchaseInvoiceRegisterDropdownBind :265 / btnRefrsh_Click :469 / cmbBranchName_Leave :2158. */
    public Map<String, Object> lookups(List<Integer> branchIds) {
        UserAccount u = user();
        String ids = branchIds == null || branchIds.isEmpty() ? "" : branchIds(u, branchIds);
        return split(repo.dropdown(u, ids));
    }

    private static Map<String, Object> split(List<Map<String, Object>> all) {
        Map<String, String> keys = new LinkedHashMap<>();
        keys.put("ItemParentCategory", "parentCategories"); keys.put("ItemCategory", "itemCategories");
        keys.put("ItemType", "itemTypes"); keys.put("ItemName", "items"); keys.put("WareHouse", "warehouses");
        keys.put("JobLot", "jobLots"); keys.put("CropYear", "cropYears"); keys.put("Supplier", "suppliers");
        keys.put("CommissionAgent", "commissionAgents");
        Map<String, Object> out = new LinkedHashMap<>();
        keys.values().forEach(k -> out.put(k, new ArrayList<Map<String, Object>>()));
        for (var r : all) {
            String k = keys.get(str(r.get("ActivityType")));
            if (k == null) continue;
            @SuppressWarnings("unchecked") List<Map<String, Object>> list = (List<Map<String, Object>>) out.get(k);
            list.add(row("Id", plain(r.get("Id")), "Name", plain(r.get("Name"))));
        }
        return out;
    }

    // ------------------------------------------------------------------ show

    private static final String PURCHASE_DETAIL = "PurchsaeDetail", AVG_ITEM = "AvgRateByItem", AVG_SUPPLIER = "AvgRateBySupplier",
            SUMMARY = "Current & Total Purchases";

    /** btnshow_Click :1576. */
    public Map<String, Object> rows(Map<String, Object> b) {
        UserAccount u = user();
        String type = str(b.get("reportType"));
        if (type.isEmpty()) throw new IllegalArgumentException("Please Select Report Type");
        return switch (type) {
            case PURCHASE_DETAIL -> detail(u, b);
            case AVG_ITEM -> avgItem(u, b);
            case AVG_SUPPLIER -> avgSupplier(u, b);
            case SUMMARY -> summary(u, b);
            default -> result(List.of(), List.of());   // none of the four branches: nothing happens
        };
    }

    /** The desktop's branch text -> ",1,2" (and "Select Branch First" when nothing is ticked). */
    private String branchesFor(UserAccount u, Map<String, Object> b) {
        List<Integer> ids = ints(b.get("branchIds"));
        if (ids.isEmpty()) throw new IllegalArgumentException("Select Branch First");
        return branchIds(u, ids);
    }

    private String branchIds(UserAccount u, List<Integer> ids) {
        Set<Integer> allowed = shared.purchaseInvoiceBranches(u).stream().map(r -> num(r.get("BranchId"))).collect(Collectors.toSet());
        if (ids.stream().anyMatch(id -> id == null || !allowed.contains(id)))
            throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(",", ",", ""));
    }

    private static String idList(Object value) {
        List<Integer> ids = ints(value);
        return ids.isEmpty() ? "" : ids.stream().map(String::valueOf).collect(Collectors.joining(",", ",", ""));
    }

    /** The filter controls shared by all four reports. */
    private Map<String, Object> filter(UserAccount u, Map<String, Object> b) {
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        for (String k : List.of("fromNo", "toNo", "supplierId", "itemId", "commissionAgentId", "jobLotId", "itemCategoryId",
                "itemTypeId", "warehouseId", "parentCategoryId")) f.put(k, num(b.get(k)));
        f.put("cropYear", str(b.get("cropYear")).trim());
        f.put("rateFrom", dbl(b.get("rateFrom")));
        f.put("rateTo", dbl(b.get("rateTo")));
        return f;
    }

    /** gridHisoryDetail :542. */
    private Map<String, Object> detail(UserAccount u, Map<String, Object> b) {
        Map<String, Object> f = filter(u, b);
        LocalDateTime df = date(b.get("dueFrom")), dt = date(b.get("dueTo"));
        if (df != null) f.put("dueFrom", df);
        if (dt != null) f.put("dueTo", dt);
        f.put("branchesIds", branchesFor(u, b));
        f.put("customGroupIds", idList(b.get("customGroupIds")));
        var raw = repo.history(u, f);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (var r : raw) {
            Map<String, Object> row = project(r, DETAIL);
            row.put("IsApproved", bool(r.get("IsApproved")) ? "Approved" : "Not Approved");
            rows.add(row);
        }
        return result(rows, raw);
    }

    /** grdSummeryReport :688. */
    private Map<String, Object> summary(UserAccount u, Map<String, Object> b) {
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        f.put("itemId", num(b.get("itemId")));
        f.put("parentCategoryId", num(b.get("parentCategoryId")));
        f.put("cropYear", str(b.get("cropYear")).trim());
        String group = str(b.get("groupBy"));
        if ("GroupItem&CropYear".equals(group)) f.put("activityId", 1);
        else if ("GroupByItem".equals(group)) f.put("activityId", 2);
        f.put("branchesIds", branchesFor(u, b));
        var raw = repo.ricePaddy(u, f);
        return result(raw.stream().map(r -> project(r, SUMMARY_COLS)).toList(), raw);
    }

    /** gridHisoryAvgRateByItemId :775 / GetAvgRateByItemCommonMethod :919. */
    private Map<String, Object> avgItem(UserAccount u, Map<String, Object> b) {
        var raw = repo.avgRateByItem(u, avgFilter(u, b));
        return result(raw.stream().map(r -> project(r, AVG_ITEM_COLS)).toList(), raw);
    }

    /** gridHisoryAvgRateBySuplier :825 / GetAvgRateBySupplierCommonMethod :877. */
    private Map<String, Object> avgSupplier(UserAccount u, Map<String, Object> b) {
        var raw = repo.avgRateBySupplier(u, avgFilter(u, b));
        return result(raw.stream().map(r -> project(r, AVG_SUPPLIER_COLS)).toList(), raw);
    }

    private Map<String, Object> avgFilter(UserAccount u, Map<String, Object> b) {
        Map<String, Object> f = new LinkedHashMap<>();
        f.put("fromDate", required(b.get("fromDate")));
        f.put("toDate", required(b.get("toDate")));
        for (String k : List.of("supplierId", "itemId", "parentCategoryId", "itemCategoryId", "itemTypeId", "jobLotId")) f.put(k, num(b.get(k)));
        f.put("cropYear", str(b.get("cropYear")).trim());
        f.put("branchesIds", branchesFor(u, b));
        return f;
    }

    // ------------------------------------------------------------------ prints

    /**
     * Print data re-queried the way the desktop does it: print_Click :1662 (221, avg rate by item),
     * btnPrintSummery_Click :1827 (222, by supplier; any failure reads "No Record Found"),
     * PurchaseInvoiceRegisterDetail :1739 (297 and the Print drop-down; no branch filter).
     */
    public Map<String, Object> printData(Map<String, Object> b) {
        UserAccount u = user();
        String kind = str(b.get("kind"));
        List<Map<String, Object>> raw;
        switch (kind) {
            case "avgItem" -> raw = repo.avgRateByItem(u, avgFilter(u, b));
            case "avgSupplier" -> {
                try { raw = repo.avgRateBySupplier(u, avgFilter(u, b)); }
                catch (RuntimeException e) { throw new IllegalArgumentException("No Record Found"); }
            }
            case "register" -> {
                Map<String, Object> f = filter(u, b);
                f.put("customGroupId", firstId(b.get("customGroupIds")));
                raw = repo.detailRegister(u, f);
            }
            default -> throw new IllegalArgumentException("Unknown print");
        }
        if (raw.isEmpty()) throw new IllegalArgumentException("Not Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("raw", plainRows(raw));
        return out;
    }

    private static int firstId(Object v) { List<Integer> l = ints(v); return l.isEmpty() ? 0 : l.get(0); }

    // ------------------------------------------------------------------ links

    /** CommonServices.GetGlAccountIdBySupplierCustomerId (the SupplierName link -> general ledger). */
    public Map<String, Object> glAccount(int supplierCustomerId) {
        UserAccount u = user();
        return Map.of("accountId", supplierCustomerId <= 0 ? 0 : shared.glAccountOfParty(u, supplierCustomerId));
    }

    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) - rows of another tenant are dropped. */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (id <= 0 || documentTypeId <= 0) return out;
        for (var r : shared.attachments(id, documentTypeId)) {
            if (num(r.get("OrganizationId")) != num(u.getOrganizationId()) || num(r.get("CompanyId")) != num(u.getCompanyId())) continue;
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", num(r.get("Id")));
            a.put("AttachmentName", str(r.get("Attachment")));
            a.put("CustomName", str(r.get("UploadedFileCustomName")));
            a.put("EntryDate", plain(r.get("EntryDate")));
            out.add(a);
        }
        return out;
    }

    public record Download(String name, byte[] bytes) {}

    public Download attachment(int id, int documentTypeId, int attachmentId) {
        UserAccount u = user();
        for (var a : attachments(id, documentTypeId)) {
            if (num(a.get("Id")) != attachmentId) continue;
            String stored = basename(str(a.get("CustomName")).isBlank() ? str(a.get("AttachmentName")) : str(a.get("CustomName")));
            String name = str(a.get("AttachmentName")).isBlank() ? stored : basename(str(a.get("AttachmentName")));
            return new Download(name, attachmentStore.read(u, stored));
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    // ------------------------------------------------------------------ desktop datatables (output name, procedure column[, kind])

    /** dt of gridHisoryDetail :576-642. */
    private static final String[][] DETAIL = {
            {"InvoiceId", "Id"}, {"InvoiceDetailId", "InvPurchaseDetailId"}, {"BranchName", "BranchName"}, {"InvoiceType", "OrderType"},
            {"InvoiceDate", "DocDate", "date"}, {"InvoiceNo", "DocNo"}, {"ManualBillNo", "ManualBillNo"}, {"GrnId", "GrnId"},
            {"GrnNo", "DocNoGrn"}, {"GrnDate", "DocDateGrn", "date"}, {"OrderId", "OrderId"}, {"OrderNo", "DocNopo"},
            {"OrderDate", "DocDatepo", "date"}, {"DetailIdsCount", "DetailIdsCount"}, {"GpId", "GpId"}, {"GpNo", "GpSrNo"},
            {"GpDate", "GpDate", "date"}, {"DocumentTypeId", "DocumentTypeId"}, {"SupplierCustomerId", "SupplierCustomerId"},
            {"SupplierName", "SupplierName"}, {"CommissionAgent", "CommAgentName"}, {"CommAmount", "CommAmount"},
            {"CreditAdd", "CreditAdd"}, {"DebitLess", "DebitLess"}, {"VehicleNo", "VehicleNo"}, {"BiltyNo", "BiltyNo"},
            {"ItemName", "ItemName"}, {"WareHouseName", "WareHouseCode"}, {"JobLot", "JobLotCode"}, {"CropYear", "CropYear"},
            {"PackingType", "PackTypeCode"}, {"PackUom", "UOMCode"}, {"ItemQty", "ItemQty"}, {"GrossWeight", "GrossWeight"},
            {"EbPerUint", "EBWPerUnit"}, {"EbTotal", "EBWTotal"}, {"WtCut", "WtCut"}, {"WcutTotal", "WtCutTotal"},
            {"AddLess", "AdLsWeight"}, {"NetBillWeight", "NetBillWeight"}, {"NetStockWeight", "NetStockWeight"},
            {"ItemRate", "ItemRate"}, {"RateUom", "RateUom"}, {"RateCut", "RateCut"}, {"RateCutAmount", "RateCutAmount"},
            {"ItemAmount", "ItemAmount"}, {"FreightAmt", "FreightAmount"}, {"AdvanceFreight", "AdvanceFreight"},
            {"ExpenseAmt", "ExpenseAmount"}, {"CommissionAmt", "CommissionAmount"}, {"ItemNetAmount", "BillAmount"},
            {"BillAmount", "BillAmountH"}, {"EntryDate", "EntryDate"}, {"EntryUser", "EnteryUserName"},
            {"VoucherHeadId", "VoucherHeadId"}, {"IsApproved", "IsApproved"}, {"NoOfAttachments", "NoOfAttachments"},
            {"TicketNos", "TicketNos"}};

    /** dt of grdSummeryReport :707-731. */
    private static final String[][] SUMMARY_COLS = {
            {"BranchName", "BranchName"}, {"ItemCode", "ItemCode"}, {"ItemName", "ItemName"}, {"CropYear", "Crop"},
            {"OpQty", "Op_Qty"}, {"OpActualQty", "Op_ActualQty"}, {"OpWeight", "Op_Weight"}, {"OpAmount", "Op_Amount"},
            {"OpItemExpAmt", "Op_Item_ExpAmt"}, {"OpExp", "Op_Exp"}, {"OpRate", "Op_Rate"}, {"CurrQty", "Cur_Qty"},
            {"CurrAcutalQty", "Curr_AcutalQty"}, {"CurrWeight", "Cur_Weight"}, {"CurrAmount", "Cur_Amount"},
            {"CurrItemExpAmt", "Cur_Item_ExpAmt"}, {"CurrExp", "Cur_Exp"}, {"CurrRate", "Cur_Rate"}, {"TotalQty", "Cl_Qty"},
            {"ClActualQty", "Cl_ActualQty"}, {"TotalWeight", "Cl_Weight"}, {"Amount", "Cl_Amount"},
            {"ClItemExpAmt", "Cl_Item_ExpAmt"}, {"Expense", "Cl_Exp"}, {"Rate", "Cl_Rate"}};

    /** dt of gridHisoryAvgRateByItemId :775-823. */
    private static final String[][] AVG_ITEM_COLS = {
            {"BranchName", "BranchName"}, {"InvoiceDate", "DocDate", "date"}, {"ItemId", "ItemId"}, {"ItemName", "ItemName"},
            {"ItemType", "TypeDescription"}, {"CropYear", "CropYear"}, {"ItemQty", "ItemQty"}, {"NetBillWeight", "NetBillWeight"},
            {"WeightByItem", "TotalWeightByItem"}, {"AvgRate", "AvgRate"}, {"TotPurPrcnt", "TotPurPrcnt"}, {"Amount", "ItemAmount"},
            {"TotalExpense", "TotalExpense"}, {"AvgRateWithExp", "AvgRateExp"}, {"AmountWithExp", "ItemExpAmount"},
            {"MiscExp", "MiscExp"}, {"MiscExp40Kg", "MiscExp40Kg"}, {"FreightExp", "FreigtExp"}, {"FreightExp40Kg", "FrtExp40Kg"},
            {"CommExp", "CommExp"}, {"CommExp40Kg", "CommExp40Kg"}, {"Expense40Kg", "Exp40Kg"}};

    /** dt of gridHisoryAvgRateBySuplier :825-875. */
    private static final String[][] AVG_SUPPLIER_COLS = {
            {"BranchName", "BranchName"}, {"SupplierCustomerId", "SupplierCustomerId"}, {"SupplierName", "CompanyName"},
            {"ItemId", "ItemId"}, {"ItemName", "ItemName"}, {"CropYear", "CropYear"}, {"PackUom", "PackSize"},
            {"ItemQty", "ItemQty"}, {"PackSizeQty", "PackSizeQty"}, {"NetBillWeight", "NetBillWeight"},
            {"WeightByItem", "TotalWeightByItem"}, {"AvgRate", "AvgRate"}, {"TotPurPercent", "TotPurPrcnt"},
            {"Amount", "ItemAmount"}, {"TotalExpense", "TotalExpense"}, {"AvgRateWithExp", "AvgRateExp"},
            {"AmountWithExp", "ItemExpAmount"}, {"MisExp", "MiscExp"}, {"MisExp40Kg", "MiscExp40Kg"},
            {"FreightExp", "FreigtExp"}, {"FreightExp40Kg", "FrtExp40Kg"}, {"CommExp", "CommExp"},
            {"CommExp40Kg", "CommExp40Kg"}, {"Expense40Kg", "Exp40Kg"}};

    // ------------------------------------------------------------------ helpers

    private static Map<String, Object> project(Map<String, Object> source, String[][] spec) {
        Map<String, Object> row = new LinkedHashMap<>();
        for (String[] c : spec) {
            Object v = plain(source.get(c[1]));
            if (c.length > 2 && "date".equals(c[2])) { String s = v == null ? "" : String.valueOf(v); v = s.length() >= 10 ? s.substring(0, 10) : s; }
            row.put(c[0], v);
        }
        return row;
    }

    private static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    private static Map<String, Object> result(List<Map<String, Object>> rows, List<Map<String, Object>> raw) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("raw", plainRows(raw));
        return out;
    }

    private static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            r.forEach((k, v) -> m.put(k, plain(v)));
            out.add(m);
        }
        return out;
    }

    private static Object plain(Object v) {
        if (v instanceof Timestamp t) return STAMP.format(t.toLocalDateTime());
        if (v instanceof java.sql.Date d) return d.toLocalDate().toString();
        if (v instanceof java.util.Date d) return STAMP.format(new Timestamp(d.getTime()).toLocalDateTime());
        if (v instanceof LocalDateTime d) return STAMP.format(d);
        if (v instanceof java.time.OffsetDateTime d) return STAMP.format(d.toLocalDateTime());
        if (v instanceof BigDecimal b) return b.doubleValue();
        return v;
    }

    private static List<Integer> ints(Object value) {
        List<Integer> out = new ArrayList<>();
        if (value instanceof Collection<?> c) for (Object o : c) { int n = num(o); if (n != 0) out.add(n); }
        return out;
    }

    private static LocalDateTime date(Object value) {
        String s = str(value).trim();
        if (s.isEmpty()) return null;
        try {
            if (s.length() == 10) return LocalDate.parse(s).atStartOfDay();
            return LocalDateTime.parse(s.replace(' ', 'T'));
        } catch (RuntimeException e) {
            throw new IllegalArgumentException("Invalid date: " + s);
        }
    }

    private static LocalDateTime required(Object value) {
        LocalDateTime d = date(value);
        if (d == null) throw new IllegalArgumentException("From Date and To Date are required");
        return d;
    }

    private static boolean bool(Object value) {
        if (value instanceof Boolean b) return b;
        if (value instanceof Number n) return n.doubleValue() != 0;
        return "true".equalsIgnoreCase(str(value)) || "1".equals(str(value));
    }

    private static String str(Object value) { return value == null ? "" : value.toString(); }

    private static int num(Object value) { return PurchaseReportsRepository.num(value); }

    private static double dbl(Object value) {
        if (value instanceof Number n) return n.doubleValue();
        if (value == null) return 0;
        try { return Double.parseDouble(value.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }
}
