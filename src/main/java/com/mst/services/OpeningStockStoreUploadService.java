package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.OpeningStockStoreRepository;
import com.mst.security.CurrentUserContext;
import org.apache.poi.ss.usermodel.Cell;
import org.apache.poi.ss.usermodel.CellType;
import org.apache.poi.ss.usermodel.DateUtil;
import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.io.InputStream;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Screen 341's "Upload Opening" toolbar button (frmStoreOpeningStockBalancing.cs btnUploadOpening_Click,
 * resolved-source build): it opens {@code frmItemDefineExcelSheetUpload} with
 * {@code ItemDefinitionType = (BaseDocumentTypeId == 1) ? 2 : …} — i.e. 2 for the store screen.
 *
 * Despite its caption the button does NOT upload opening balances: that form defines ITEMS from the
 * "Items" sheet of an .xlsx workbook (Upload → preview grid → Save → BLL 0583 Item.SaveList → DAL 0436
 * SaveList, one transaction). Ported here as that form behaves for ItemDefinitionType 2.
 *
 * Reads (btnSave_Click:143): ItemCategory.Getall, ItemType.Getall, UOM.Getall, GetWarehouseRacks,
 * GetGlobalAllAccountsWithCustomGroup, config AutoCoaDefineByItemNameOnInsert, ERP feature 8,
 * Item.GenerateCode per new category. (getMotherItems, ItemClass.GetAll and ItemGroup.GetAll are read
 * by the desktop too but only used when ItemDefinitionType == 1 — not read here.)
 *
 * Writes (DAL 0436 SaveList:725), per row: Sp_Item_Insert → id; if id > 0: [ApplyGST is never set →
 * no tax schedule]; per allocation (exactly one: the user's company/branch): Sp_ItemAllocation_Insert,
 * Sp_UOMSchedule_Insert from the base unit (ItemGroupId is 0), [prices are 0 → no price schedules],
 * Sp_ItemsReorderSchedule_Insert when ReorderLevel > 0.
 *
 * DESKTOP BEHAVIOUR REPRODUCED (NOT CORRECTED)
 *  1. Rows are skipped when Item_Name is blank; every other error stops the whole upload.
 *  2. After the first item of a category, ItemCodeNew is the generator's TEMPLATE for every later item
 *     of that category when the category uses coded numbering (only the first gets "-n") — duplicates.
 *  3. With config AutoCoaDefineByItemNameOnInsert on, Stock/Sale GL are not set at all (SaveList, unlike
 *     SetData, never creates accounts) → items saved with PurchaseGLAC / SaleGLAC 0.
 *  4. HS code, stock levels, status, flags and the allocation are only filled inside the CGS-account
 *     branch; CGS_GL_Account empty → "CGS GL Account not found.Please check!".
 *  5. With ERP feature 8, Is_Company feeds IsThirdParty and Is_ThirdParty feeds IsCompany (crossed).
 *  6. Is_Active / Is_Local / Is_Company compare only "NO", "No", "nO" (so "no" counts as yes).
 *  7. Reorder schedule BranchesId is the item's BranchesId, never set → 0.
 *  8. "Items Uploaded Successfully" only when the LAST item's procedure returned an id > 0.
 *
 * DEVIATIONS (WEB ONLY)
 *  - The file is parsed on the server with Apache POI (.xlsx only, as the desktop's dialog filter);
 *    a missing column is reported by name (the desktop fails with a null reference).
 *  - Uploading needs the 341 Save right (the desktop form checks no right).
 *  - At most 5,000 data rows per workbook.
 */
@Service
public class OpeningStockStoreUploadService {

    public static final int ITEM_DEFINITION_TYPE = 2;
    private static final int MAX_ROWS = 5000;

    private final OpeningStockStoreRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public OpeningStockStoreUploadService(OpeningStockStoreRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    private UserAccount user() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(OpeningStockStoreService.SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        return u;
    }

    // ------------------------------------------------------------------ btnUpload_Click_1 / GenerateOneDt

    /** ExcelDataReader AsDataSet(UseHeaderRow = true) → the "Items" sheet as a table of cell texts. */
    public Map<String, Object> preview(String fileName, InputStream in) {
        user();
        if (fileName == null || !fileName.toLowerCase().endsWith(".xlsx")) throw new IllegalArgumentException("please choose  .xlsx file only.");
        List<String> columns = new ArrayList<>();
        List<Map<String, String>> rows = new ArrayList<>();
        try (Workbook wb = new XSSFWorkbook(in)) {
            Sheet sheet = wb.getSheet("Items");
            if (sheet == null) throw new IllegalArgumentException("Sheet named 'Items' was not found in the workbook.");
            Row header = sheet.getRow(sheet.getFirstRowNum());
            if (header == null) return table(columns, rows);
            int width = Math.max(0, header.getLastCellNum());
            for (int c = 0; c < width; c++) {
                String name = text(header.getCell(c)).trim();
                if (name.isEmpty()) name = "Column" + (c + 1);
                String unique = name;
                for (int k = 1; columns.contains(unique); k++) unique = name + "_" + k;
                columns.add(unique);
            }
            for (int r = sheet.getFirstRowNum() + 1; r <= sheet.getLastRowNum(); r++) {
                if (rows.size() >= MAX_ROWS) throw new IllegalArgumentException("At most " + MAX_ROWS + " rows can be uploaded at once.");
                Row row = sheet.getRow(r);
                Map<String, String> o = new LinkedHashMap<>();
                for (int c = 0; c < columns.size(); c++) o.put(columns.get(c), row == null ? "" : text(row.getCell(c)));
                rows.add(o);
            }
        } catch (IllegalArgumentException e) {
            throw e;
        } catch (Exception e) {
            throw new IllegalArgumentException(e.getMessage() == null ? "The workbook could not be read." : e.getMessage());
        }
        return table(columns, rows);
    }

    private static Map<String, Object> table(List<String> columns, List<Map<String, String>> rows) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("columns", columns);
        out.put("rows", rows);
        return out;
    }

    /** Conversion.ToString(cell value) — numbers print as the CLR prints a double, booleans "True"/"False". */
    private static String text(Cell cell) {
        if (cell == null) return "";
        CellType t = cell.getCellType() == CellType.FORMULA ? cell.getCachedFormulaResultType() : cell.getCellType();
        switch (t) {
            case STRING: return cell.getStringCellValue();
            case BOOLEAN: return cell.getBooleanCellValue() ? "True" : "False";
            case NUMERIC:
                if (DateUtil.isCellDateFormatted(cell)) {
                    java.time.LocalDateTime d = cell.getLocalDateTimeCellValue();
                    return d.format(java.time.format.DateTimeFormatter.ofPattern("M/d/yyyy h:mm:ss a", java.util.Locale.US));
                }
                return clr(cell.getNumericCellValue());
            default: return "";
        }
    }

    // ------------------------------------------------------------------ btnSave_Click:143

    @Transactional
    public Map<String, Object> save(List<Map<String, String>> gridRows) {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        if (gridRows == null || gridRows.isEmpty()) {                                   // :173
            out.put("saved", false);
            out.put("message", "");
            return out;
        }
        if (gridRows.size() > MAX_ROWS) throw new IllegalArgumentException("At most " + MAX_ROWS + " rows can be uploaded at once.");
        List<Map<String, Object>> categories = repo.itemCategories(u);
        List<Map<String, Object>> types = repo.itemTypes(u);
        List<Map<String, Object>> uoms = repo.uomMaster(u);
        List<Map<String, Object>> racks = repo.warehouseRacks(u);
        List<Map<String, Object>> accounts = repo.accountsWithCustomGroup(u);
        boolean autoCoa = bool(repo.config(u, "AutoCoaDefineByItemNameOnInsert"));
        boolean feature8 = repo.feature(u, 8);

        Map<Integer, Object[]> codeCache = new HashMap<>();        // (LastItemCode, ItemCodeNewTemplate, IsNumericMode)
        Map<Integer, Integer> padLength = new HashMap<>();
        List<Map<String, Object>> items = new ArrayList<>();
        List<double[]> levels = new ArrayList<>();                 // {Equivalent, Min, Max, Reorder}
        Timestamp now = new Timestamp(System.currentTimeMillis());

        for (Map<String, String> row : gridRows) {
            if (cell(row, "Item_Name").trim().isEmpty()) continue;                      // :184
            Map<String, Object> item = OpeningStockStoreRepository.itemModel();
            item.put("OrganizationId", u.getOrganizationId());
            item.put("CompanyId", u.getCompanyId());
            item.put("EntryUser", u.getId());
            item.put("ModifyUser", u.getId());
            item.put("PostUser", u.getId());
            item.put("EntryDate", now);
            item.put("ModifyDate", now);
            item.put("PostDate", now);

            String category = cell(row, "Item_Category").trim();
            if (category.isEmpty()) fail("Item Category is required!");
            Map<String, Object> cat = first(categories, "CategoryDescription", category);
            if (cat == null) fail("Item Category:" + category + " not define please Check!");
            int categoryId = toInt(cat.get("Id"));
            item.put("ItemCategoryId", categoryId);

            String type = cell(row, "Item_Type").trim();
            if (type.isEmpty()) fail("Item Type is required!");
            Map<String, Object> typ = first(types, "TypeDescription", type);
            if (typ == null) fail("Item Type:" + type + " not define please Check!");
            int typeId = toInt(typ.get("Id"));
            item.put("ItemTypeId", typeId);

            if (!codeCache.containsKey(categoryId)) {                                   // :218
                List<Map<String, Object>> gen = repo.generateItemCode(u, categoryId, typeId);
                if (gen.isEmpty()) fail("Could not generate item code for the selected Category.");
                String code = str(gen.get(0).get("ItemCode"));
                String codeNew = str(gen.get(0).get("ItemCodeNew"));
                int last;
                try { last = Integer.parseInt(code); } catch (NumberFormatException e) { throw new IllegalArgumentException("Input string was not in a correct format."); }
                boolean numeric = codeNew.equals(String.valueOf(last)) || codeNew.equals(code);
                codeCache.put(categoryId, new Object[] { last, codeNew, numeric });
                item.put("ItemCode", code);
                item.put("ItemCodeNew", (!numeric && ITEM_DEFINITION_TYPE != 1) ? codeNew + "-" + convInt(code) : codeNew);
                padLength.put(categoryId, code.length());
            } else {
                Object[] t = codeCache.get(categoryId);
                int next = (Integer) t[0] + 1;
                int pad = padLength.getOrDefault(categoryId, 0);
                String code = pad > 0 ? String.format("%" + pad + "s", next).replace(' ', '0') : String.valueOf(next);
                item.put("ItemCode", code);
                item.put("ItemCodeNew", (Boolean) t[2] ? code : (String) t[1]);        // D2
                codeCache.put(categoryId, new Object[] { next, t[1], t[2] });
            }

            String name = cell(row, "Item_Name").trim();
            item.put("ItemName", name);
            item.put("ItemNameOtherLingo", cell(row, "Other_Name").trim());
            String baseUom = cell(row, "Base_Unit").trim();
            if (baseUom.isEmpty()) fail("Base Unit is required");
            Map<String, Object> unit = null;
            for (Map<String, Object> x : uoms) {
                if (str(x.get("UOMCode")).equals(baseUom) || netString(x.get("Equivalent")).equals(baseUom)) { unit = x; break; }
            }
            if (unit == null) fail("Base Unit:" + baseUom + " not define please Check!");
            item.put("BaseUnitId", toInt(unit.get("Id")));
            double equivalent = com.mst.repositories.StoreIssuanceRepository.toDouble(unit.get("Equivalent"));

            /* ItemDefinitionType 2: Rack_Name (:304). */
            String rackName = cell(row, "Rack_Name").trim();
            if (rackName.isEmpty()) fail("Rack Name is required!");
            Map<String, Object> rack = first(racks, "rackName", rackName);
            if (rack == null) fail("Rack Name:" + rackName + " not define please Check!");
            item.put("RackId", toInt(rack.get("Id")));

            String stock = cell(row, "Stock_GL_Account").trim();
            String sale = cell(row, "Sale_GL_Account").trim();
            String cgs = cell(row, "CGS_GL_Account").trim();
            if (!autoCoa) {                                                             // :321 (D3)
                if (!stock.isEmpty()) {
                    Map<String, Object> a = first(accounts, "AccountCode", stock);
                    if (a == null) fail("Stock GL Account not found please check.");
                    item.put("PurchaseGLAC", toInt(a.get("ChartOfAccountId")));
                } else {
                    item.put("PurchaseGLAC", toInt(cat.get("InventoryAccountId")));
                }
                if (!sale.isEmpty()) {
                    Map<String, Object> a = first(accounts, "AccountCode", sale);
                    if (a == null) fail("Sale GL Account not found please check.");
                    item.put("SaleGLAC", toInt(a.get("ChartOfAccountId")));
                } else {
                    item.put("SaleGLAC", toInt(cat.get("RevenueAccountId")));
                }
            }
            if (cgs.isEmpty()) fail("CGS GL Account not found.Please check!");         // :434 (D4)
            Map<String, Object> ca = first(accounts, "AccountCode", cgs);
            if (ca == null) fail("CGS GL Account not found please check.");
            item.put("COGSGLAC", toInt(ca.get("ChartOfAccountId")));
            item.put("HSCode", cell(row, "HS_Code").trim());
            double min = convInt(optional(row, "Min_Stock"));
            double max = convInt(optional(row, "Max_Stock"));
            item.put("MinStockLevel", min);
            item.put("MaxStockLevel", max);
            if (min > max) fail("Min stock level cannot be greater than max stock level for Item:" + name);
            double reorder = convInt(optional(row, "ReOrder_Level"));
            item.put("ReorderLevel", reorder);
            item.put("ReOrderQty", (double) convInt(optional(row, "ReOrder_Qty")));
            item.put("ItemStatus", !isNo(cell(row, "Is_Active").trim()));              // D6
            item.put("IsTaxable", false);
            String isCompany = cell(row, "Is_Company").trim();
            String isThird = cell(row, "Is_ThirdParty").trim();
            item.put("IsImport", isNo(cell(row, "Is_Local").trim()));
            if (feature8) {
                boolean f3 = !isNo(isCompany), f4 = !isNo(isThird);
                if (!f3 && !f4) fail("Please Check Any Of ('Is Company' Or 'Is third Party')");
                item.put("IsThirdParty", f3);                                          // D5
                item.put("IsCompany", f4);
            } else {
                item.put("IsThirdParty", false);
                item.put("IsCompany", true);
            }
            items.add(item);
            levels.add(new double[] { equivalent, min, max, reorder });
        }

        /* DAL 0436 SaveList:725 — one transaction (this method's). */
        int num = 0;
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        for (int i = 0; i < items.size(); i++) {
            Map<String, Object> item = items.get(i);
            double[] lv = levels.get(i);
            num = repo.setProc("Sp_Item_Insert", item);
            if (num > 0) {
                /* ItemAllocationlist — one row: org, company, the user's branch, active (:425). */
                Map<String, Object> al = new LinkedHashMap<>();
                al.put("IsActive", true);
                al.put("BranchId", branch);
                al.put("CompanyId", u.getCompanyId());
                al.put("OrganizationId", u.getOrganizationId());
                al.put("Id", 0);
                al.put("ItemId", num);
                repo.setProc("Sp_ItemAllocation_Insert", al);
                /* ItemGroupId 0 → the base-unit schedule (:795). Model 1075 defaults for the rest. */
                Timestamp t = new Timestamp(System.currentTimeMillis());
                Map<String, Object> us = new LinkedHashMap<>();
                us.put("EntryDate", t);
                us.put("ModifyDate", t);
                us.put("Equivalent", lv[0]);
                us.put("QtyEquivalent", 0d);
                us.put("BaseRateUom", false);
                us.put("BasePackUom", false);
                us.put("BaseSecondaryUom", false);
                us.put("Active", true);
                us.put("CompanyId", u.getCompanyId());
                us.put("EntryUser", u.getId());
                us.put("Id", 0);
                us.put("ItemId", num);
                us.put("ModifyUser", 0);
                us.put("OrganizationId", u.getOrganizationId());
                us.put("ScheduleUnitId", toInt(item.get("BaseUnitId")));
                repo.setProc("Sp_UOMSchedule_Insert", us);
                if (lv[3] > 0d) {                                                       // :867
                    Map<String, Object> r = OpeningStockStoreRepository.reorderModel();
                    r.put("ItemId", num);
                    r.put("EffectiveDate", t);
                    r.put("EntryDate", t);
                    r.put("ModifyDate", t);
                    r.put("EntryUser", u.getId());
                    r.put("OrganizationId", u.getOrganizationId());
                    r.put("CompanyId", u.getCompanyId());
                    r.put("BranchesId", 0);                                             // D7
                    r.put("MinQty", lv[1]);
                    r.put("MaxQty", lv[2]);
                    r.put("OptimalQty", 0d);
                    r.put("ReOrderPoint", lv[3]);
                    repo.setProc("Sp_ItemsReorderSchedule_Insert", r);
                }
            } else {
                num = toInt(item.get("Id"));
            }
        }
        out.put("saved", num > 0);                                                      // D8
        out.put("message", num > 0 ? "Items Uploaded Successfully" : "");
        return out;
    }

    // ------------------------------------------------------------------ helpers

    /** gridEXRow.Cells["x"] — the column must exist. */
    private static String cell(Map<String, String> row, String column) {
        if (!row.containsKey(column)) throw new IllegalArgumentException("Column '" + column + "' was not found in the 'Items' sheet.");
        String v = row.get(column);
        return v == null ? "" : v;
    }

    /** GetCellValue — "" when the column is absent. */
    private static String optional(Map<String, String> row, String column) {
        String v = row.get(column);
        return v == null ? "" : v.trim();
    }

    private static Map<String, Object> first(List<Map<String, Object>> rows, String column, String value) {
        for (Map<String, Object> r : rows) if (str(com.mst.repositories.StoreIssuanceRepository.ci(r, column)).equals(value)) return r;
        return null;
    }

    private static boolean isNo(String s) { return "NO".equals(s) || "No".equals(s) || "nO".equals(s); }

    /** Conversion.ToInt(string) — Convert.ToInt32: an integer text, anything else 0. */
    private static int convInt(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static String netString(Object v) {
        if (v == null) return "";
        if (v instanceof Double || v instanceof Float) return clr(((Number) v).doubleValue());
        if (v instanceof java.math.BigDecimal) return ((java.math.BigDecimal) v).toPlainString();
        return String.valueOf(v);
    }

    private static boolean bool(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    private static void fail(String m) { throw new IllegalArgumentException(m); }
}
