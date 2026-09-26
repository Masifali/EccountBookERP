package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ProductionJobOrderDto;
import com.mst.repositories.ProductionJobOrderRepository;
import com.mst.repositories.ProductionJobOrderLookupsRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Production Job Order - frmProductionJobOrder.cs, DocumentTypeId 401.
 *
 * ---------------------------------------------------------------------------------------------
 * VALIDATION IS THE DESKTOP'S, MESSAGE FOR MESSAGE
 * ---------------------------------------------------------------------------------------------
 * btnsave_Click (:1611-1790) refuses to save on each of the conditions below and shows exactly
 * these strings. They are reproduced verbatim so an operator who knows the desktop sees the same
 * text here - not a reworded or "improved" version, and not a different set of required fields.
 *
 * ---------------------------------------------------------------------------------------------
 * DOCUMENT TYPE, TENANCY AND USER COME FROM THE SERVER
 * ---------------------------------------------------------------------------------------------
 * DocumentTypeId is fixed at 401 by the form itself (:1624). Organization, Company, Branch,
 * FinancialYear, EntryUser and ModifyUser are read from the signed-in user. None of them is taken
 * from the request body: a client-supplied DocumentTypeId would let this screen write another
 * screen's documents, and a client-supplied user id would forge authorship.
 */
@Service
public class ProductionJobOrderService {

    private static final Logger LOG = LoggerFactory.getLogger(ProductionJobOrderService.class);

    /** frmProductionJobOrder.cs:1624 - invProductionJobOrder.DocumentTypeId = 401. */
    public static final int DOCUMENT_TYPE_ID = 401;

    /** frmProductionJobOrder.cs:2435 / :2581 - the production documents a job order can already
     *  be consumed by. They are NOT this screen's document type, and are used only by the
     *  row-edit guard. */
    public static final int PRODUCTION_OUTPUT_DOCUMENT_TYPE_ID = 112;
    public static final int PRODUCTION_INPUT_DOCUMENT_TYPE_ID  = 80;

    /** The desktop screen name, for the per-screen grant lookup. */
    private static final String SCREEN_NAME = "frmProductionJobOrder";

    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";
    private static final String SQL_USER_RIGHTS =
            "EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, @RightName=?, "
          + "@CompanyId=?, @Activity=?";

    @Autowired private ProductionJobOrderRepository repo;
    @Autowired private ProductionJobOrderLookupsRepository lookups;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private JdbcTemplate jdbcTemplate;

    // =============================================================================== save

    @Transactional
    public Map<String, Object> save(ProductionJobOrderDto dto) {
        UserAccount u = currentUserContext.requireAccountingUser();
        validate(dto);

        /* Server-owned, every time - whatever the request body said is discarded. */
        dto.DocumentTypeId  = DOCUMENT_TYPE_ID;
        dto.OrganizationId  = u.getOrganizationId();
        dto.CompanyId       = u.getCompanyId();
        dto.BranchesId      = u.getBranchesId() == null ? 0 : u.getBranchesId();
        dto.FinancialYearId = currentUserContext.currentFinancialYearId();
        dto.EntryUser       = u.getId();
        dto.ModifyUser      = u.getId();
        dto.SupplierCustomerId = 0;                    // the form always sends 0 (:1646)
        dto.IsApproved      = Boolean.FALSE;           // the form saves false (:1645)

        String now = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date());
        dto.EntryDate  = now;
        dto.ModifyDate = now;

        /* No referential check here. The desktop runs CheckJobOrderIdRefToInvFoodProduction only
           when a GRID ROW is opened for editing (see rowEditable below), never on save, and it
           asks about document types 112/80 rather than this screen's 401. An extra guard on save
           would refuse job orders the desktop accepts. */

        /* ---------------------------------------------------------------------------------
         * ActionTypeId is the row-action discriminator the insert procedures demand.
         * ProductionPreCostingAndJobOrder.cs sets it on every detail row:
         *
         *     jobOrderInputdetail.ActionTypeId  = (Id <= 0) ? 1 : 2;     (:1858)
         *     jobOrderOutputdetail.ActionTypeId = (Id <= 0) ? 1 : 2;     (:1767)
         *     ...ActionTypeId = 3;   for a row removed from the grid     (:2752, :3573)
         *
         * so 1 = insert, 2 = update, 3 = delete. Nothing in this port ever set it, so every row
         * reached Sp_InvProductionJobOrderInput_Insert with 0 and the procedure raised
         * "ActionTypeId Not Found In Input Detail Of Item ...".
         *
         * It is derived here from the row's own Id rather than taken from the request: it decides
         * whether a row is inserted, updated or DELETED, and a client-supplied 3 would let a
         * caller remove rows it should not touch.
         * --------------------------------------------------------------------------------- */
        if (dto.invProductionJobOrderInput != null) {
            for (ProductionJobOrderDto.Input r : dto.invProductionJobOrderInput) {
                r.ActionTypeId = (r.Id == null || r.Id <= 0) ? 1 : 2;
            }
        }
        if (dto.invProductionJobOrderOutput != null) {
            for (ProductionJobOrderDto.Output r : dto.invProductionJobOrderOutput) {
                r.ActionTypeId = (r.Id == null || r.Id <= 0) ? 1 : 2;
            }
        }

        int id = repo.save(dto);

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("planCode", dto.PlanCode);
        /* The desktop's own confirmation text (:1788). */
        res.put("message", "Save Successfully" + (dto.PlanCode == null ? "" : dto.PlanCode));
        return res;
    }

    /**
     * Exactly the desktop's checks, in the desktop's order, with the desktop's wording.
     *
     * Insert() runs FormValidation() first (the seven header fields), then the two grid-empty
     * checks, then the per-row checks as it walks each grid. Same order here, so the first
     * message an operator sees is the same message on both apps.
     *
     * "Mannual ReportNo" and "Cropt Year" are the desktop's spellings. They are not typos to fix:
     * an operator who has learnt this screen should read the same words.
     */
    private void validate(ProductionJobOrderDto dto) {
        /* ---- FormValidation(), :1434-1478 ---- */
        if (dto.PlanCode == null || dto.PlanCode == 0)
            throw new IllegalArgumentException("Plan Code Field Required");
        if (zero(dto.InvProductionPlantId))
            throw new IllegalArgumentException("Plan/Feader Field Required");
        if (blank(dto.RefInvoiceNo) || "0".equals(dto.RefInvoiceNo.trim()))
            throw new IllegalArgumentException("Mannual ReportNo Field Required");
        if (blank(dto.PlanType))
            throw new IllegalArgumentException("Plan Type Field Required");
        if (blank(dto.ProductionType))
            throw new IllegalArgumentException("Production Type Field Required");
        if (blank(dto.PlanStatus))
            throw new IllegalArgumentException("Status Field Required");
        if (zero(dto.WipItemId))
            throw new IllegalArgumentException("WIPItem Field Required");

        /* ---- Insert(), :1655-1663 ---- */
        if (dto.invProductionJobOrderOutput == null || dto.invProductionJobOrderOutput.isEmpty())
            throw new IllegalArgumentException("OutPut Grid Record Not Found");
        if (dto.invProductionJobOrderInput == null || dto.invProductionJobOrderInput.isEmpty())
            throw new IllegalArgumentException("Input Grid Record Not Found");

        for (ProductionJobOrderDto.Output r : dto.invProductionJobOrderOutput) {
            if (zero(r.ItemId))          throw new IllegalArgumentException("Item Field Require in Planned Output Detail");
            if (zero(r.InvPackingTypeId))throw new IllegalArgumentException("Packing Type in Planned Output Detail");
            if (zero(r.CropYearId))      throw new IllegalArgumentException("CropYear Field Require in Planned Output Detail");
            if (zero(r.WarehouseId))     throw new IllegalArgumentException("WareHouse Field Require in Planned Output Detail");
            if (zero(r.JobLotId))        throw new IllegalArgumentException("JobLot Field Require in Planned Output Detail");
        }
        for (ProductionJobOrderDto.Input r : dto.invProductionJobOrderInput) {
            if (zero(r.ItemId))       throw new IllegalArgumentException("Item Field Require in Planned Input Detail");
            if (zero(r.CropYearId))   throw new IllegalArgumentException("CropYear Field Require in Planned Input Detail");
            if (zero(r.JobLotId))     throw new IllegalArgumentException("JobLot Field Require in Planned Input Detail");
            if (zeroD(r.Qty))         throw new IllegalArgumentException("Quantity Field Require in Planned Input Detail");
            if (zero(r.UomSchIdQty))  throw new IllegalArgumentException("PackSize Field Require in Planned Input Detail");
            if (zeroD(r.NetWeight))   throw new IllegalArgumentException("Weight Field Require in Planned Input Detail");
            if (zero(r.WarehouseId))  throw new IllegalArgumentException("WareHouse Field Require in Planned Input Detail");
            if (zero(r.PackingTypeId))throw new IllegalArgumentException("Packing Type Field Require in Planned Input Detail");
        }
    }

    // =============================================================================== reads

    public Map<String, Object> load(int id) {
        currentUserContext.requireAccountingUser();
        Map<String, Object> head = repo.header(id);
        if (head == null) return null;
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("header", head);
        /* One procedure per grid, each taking the header Id alone - DAL 0273 GetDate. */
        res.put("input",  repo.inputRows(id));
        res.put("output", repo.outputRows(id));
        return res;
    }

    /**
     * The History tab. BindHistoryGrid sends only NoOfRecords - opening the tab asks for 50, the
     * Load All button asks for 0 (everything) - and the rows are then projected onto the desktop's
     * own fifteen columns, in the desktop's order, with its dd-MMM-yyyy dates. No extra column is
     * added and none is dropped, so the two History screens list the same thing.
     */
    public Map<String, Object> history(Integer noOfRecords) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean canViewAll = canViewAllRecords(u);
        List<Map<String, Object>> raw = repo.history(u, DOCUMENT_TYPE_ID, canViewAll, u.getId(), noOfRecords);

        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id",          r.get("Id"));
            o.put("Date",        day(r.get("PlanDate")));
            o.put("Code",        r.get("PlanCode"));
            o.put("Type",        r.get("PlanType"));
            o.put("InvRef",      r.get("RefInvoiceNo"));
            o.put("PType",       r.get("ProductionType"));
            o.put("SO",          r.get("RefSalesOrderId"));
            o.put("StartDate",   day(r.get("StartDate")));
            o.put("EndDate",     day(r.get("EndDate")));
            o.put("PlanStatus",  r.get("PlanStatus"));
            o.put("PlantInst",   r.get("PackingInstructions"));
            o.put("DelInst",     r.get("DeliveryInstructions"));
            o.put("ProdInst",    r.get("ProductionInsturction"));
            o.put("OtherInst",   r.get("OtherInstructions"));
            o.put("Attachments", r.get("NoOfAttachments"));
            rows.add(o);
        }

        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("canViewAllRecords", canViewAll);
        res.put("rows", rows);
        return res;
    }

    /**
     * Conversion.ToDateTime(...).ToString("dd-MMM-yyy") - the desktop's history date format.
     *
     * A new SimpleDateFormat per call, deliberately: a shared static one is not thread-safe and
     * two operators opening History at the same moment can get each other's half-formatted date.
     */
    private static String day(Object v) {
        if (v == null) return "";
        SimpleDateFormat out = new SimpleDateFormat("dd-MMM-yyyy", Locale.ENGLISH);
        if (v instanceof java.util.Date) return out.format((java.util.Date) v);
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return "";
        for (String pattern : new String[] { "yyyy-MM-dd HH:mm:ss.S", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd" }) {
            try { return out.format(new SimpleDateFormat(pattern).parse(s)); }
            catch (java.text.ParseException ignored) { /* try the next shape */ }
        }
        return s;
    }


    /**
     * grdPlannedOutput / grdInputDetail double-click - may this saved row be opened for editing?
     *
     * The desktop asks CheckJobOrderIdRefToInvFoodProduction about the PRODUCTION document type
     * (112 for the output grid, 80 for the input grid), and refuses with these exact words when
     * the job order is already consumed there.
     */
    public Map<String, Object> rowEditable(String grid, int jobOrderId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean output = !"input".equalsIgnoreCase(grid == null ? "" : grid.trim());
        int docType = output ? PRODUCTION_OUTPUT_DOCUMENT_TYPE_ID : PRODUCTION_INPUT_DOCUMENT_TYPE_ID;

        Map<String, Object> res = new LinkedHashMap<>();
        if (jobOrderId <= 0) {                    // an unsaved row references nothing yet
            res.put("editable", true);
            return res;
        }
        boolean used = repo.referencedByProduction(u, jobOrderId, docType) > 0;
        res.put("editable", !used);
        if (used) {
            res.put("message", output
                    ? "Record Not Update Because Record has exist in Production OutPut"
                    : "Record Not Update Because Record has exist in Production InPut");
        }
        return res;
    }

    public int nextCode() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextCode(u, DOCUMENT_TYPE_ID, currentUserContext.currentFinancialYearId());
    }

    public int nextPlanTypeCode(String planType) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.nextPlanTypeCode(u, planType, currentUserContext.currentFinancialYearId());
    }

    // =============================================================== authorization

    /**
     * CommonServices.SetRightsValueInRightsObject, reproduced the way the rest of this port
     * already does it (SaleOrderCmagtService, InvoiceHistoryService).
     *
     * CORRECTION - the first version of this method got the call wrong twice:
     *
     *   - it passed "CanView AllRecord" as @RightName. @RightName is the signed-in user's ROLE
     *     (tblUser.RightName = clsGlobalVariables.UserAccount.RoleName, CommonServices :17571),
     *     which the procedure uses to pick its Admin branch. The right being asked about is NOT a
     *     parameter: the procedure returns every right row for the screen and the caller picks
     *     the one whose RightName is "CanView AllRecord" (:17634).
     *   - it then accepted ANY returned row with Value = 1, so a user holding only "Print" on
     *     this screen would have been treated as holding "CanView AllRecord" - that is, allowed
     *     to read every other user's job orders.
     *
     * Computed per call and never cached in a field: this is a singleton bean, and a cached
     * answer would leak one user's permission to the next request. A failed read returns the
     * restrictive answer.
     */
    private boolean canViewAllRecords(UserAccount u) {
        String role = currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            List<Map<String, Object>> rights = jdbcTemplate.queryForList(
                    SQL_USER_RIGHTS, u.getId(), SCREEN_NAME, role, u.getCompanyId(), "GetByUserId");
            for (Map<String, Object> r : rights) {
                Object name = ci(r, "RightName");
                if (name != null
                        && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    return toBool(ci(r, "Value"));
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read CanView AllRecord for {}; restricting to own records",
                     SCREEN_NAME, e);
        }
        return false;
    }

    /**
     * RoleName, now taken from the login itself.
     *
     * The desktop reads clsGlobalVariables.UserAccount.RoleName - a column Sp_UserAccount_Login
     * returns, which is NOT a column of dbo.UserAccount and so cannot be recovered by reloading
     * the user. DesktopLoginAuthenticationProvider carries it on the principal;
     * CurrentUserContext.currentRoleName() reads it there and falls back to
     * UserGroup.UserGroupRole, which is what this method used to do on its own.
     */
    private String currentRoleName() {
        return currentUserContext.currentRoleName();
    }

    /** The procedure's column casing is not guaranteed, so rows are read case-insensitively. */
    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean toBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }


    // =============================================================================== lookups

    /**
     * Every list the screen binds at load, each from the procedure the desktop calls for it -
     * see ProductionJobOrderLookupsRepository for the form -> BLL -> procedure trace of each one.
     *
     * entryTypes and filterTypes are literals here because they are literals in the desktop too:
     * frmProductionJobOrder builds both as in-memory DataTables (:560-583), so there is no table
     * to read them from. Every other list on this screen is a database read.
     */
    public Map<String, Object> lookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("cropYears",     lookups.cropYears(u));
        m.put("warehouses",    lookups.warehouses(u));
        m.put("jobLots",       lookups.jobLots(u));
        m.put("plants",        lookups.plants(u));
        m.put("packingTypes",  lookups.packingTypes());
        m.put("coaAccounts",   lookups.coaAccounts(u));
        m.put("wipItems",      lookups.wipItems(u));
        m.put("items",         lookups.allItems(u));
        m.put("entryTypes",    ENTRY_TYPES);
        m.put("filterTypes",   FILTER_TYPES);
        return m;
    }

    /** cmbItem_Leave / cmbItemInput_Leave -> bindRateUomAndItemPackUom / bindPackSizeInput. */
    public List<Map<String, Object>> uomSchedule(int itemId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (itemId <= 0) return new ArrayList<>();
        return lookups.uomSchedule(u, itemId);
    }

    /**
     * cmbFiltersType_Leave - the cmbDocNo list depends on the chosen filter, and on nothing else.
     * An unrecognised filter returns an empty list rather than falling back to some other list:
     * the desktop binds nothing in that case either.
     */
    public Map<String, Object> docNoList(String filterType) {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> m = new LinkedHashMap<>();
        String f = filterType == null ? "" : filterType.trim();
        if ("SaleOrder".equalsIgnoreCase(f)) {
            m.put("valueField", "Id"); m.put("displayField", "DocNo");
            m.put("caption", "SaleOrder");            m.put("rows", lookups.saleOrderNumbers(u));
        } else if ("Export Contract".equalsIgnoreCase(f)) {
            m.put("valueField", "Id"); m.put("displayField", "LcOrderNo");
            m.put("caption", "Export ContractNo");    m.put("rows", lookups.lcOrderNumbers(u));
        } else if ("Item Type".equalsIgnoreCase(f)) {
            m.put("valueField", "Id"); m.put("displayField", "TypeDescription");
            m.put("caption", "ItemType");             m.put("rows", lookups.itemTypes(u));
        } else if ("Item Category".equalsIgnoreCase(f)) {
            m.put("valueField", "Id"); m.put("displayField", "CategoryDescription");
            m.put("caption", "ItemCategory");         m.put("rows", lookups.itemCategories(u));
        } else {
            m.put("valueField", "Id"); m.put("displayField", "DocNo");
            m.put("caption", "");                     m.put("rows", new ArrayList<>());
        }
        return m;
    }

    /**
     * cmbDocNo_Leave - which items the Input or Output picker may offer once a filter and a
     * document are chosen.
     *
     * The four branches are the desktop's, kept apart on purpose:
     *
     *   Item Category / Item Type  read the FULL item list and keep the rows whose ItemCategoryId
     *                              (or ItemTypeId) equals the chosen value. The Output grid reads
     *                              Item.GetAll and the Input grid reads Item.ReadAllItems - two
     *                              different procedures' activities, so they stay two calls.
     *   SaleOrder                  SaleOrder.ReadByPurchaseOrderIDNOrderItemId, key OrderItemId.
     *   Export Contract            ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId, key ExImItemId.
     *
     * With no filter chosen the desktop calls ItemDetailFill, i.e. the unfiltered list, and so
     * does this.
     */
    public Map<String, Object> filteredItems(String entryType, String filterType, Integer docNoId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean input = "Input".equalsIgnoreCase(entryType == null ? "" : entryType.trim());
        String f = filterType == null ? "" : filterType.trim();
        int id = docNoId == null ? 0 : docNoId;

        Map<String, Object> m = new LinkedHashMap<>();
        m.put("valueField", "Id");
        m.put("displayField", "ItemName");

        if (f.isEmpty() || id <= 0) {
            m.put("rows", lookups.allItems(u));
            return m;
        }
        if ("SaleOrder".equalsIgnoreCase(f)) {
            m.put("valueField", "OrderItemId");
            m.put("rows", lookups.itemsBySaleOrder(id));
            return m;
        }
        if ("Export Contract".equalsIgnoreCase(f)) {
            m.put("valueField", "ExImItemId");
            m.put("rows", lookups.itemsByExportContract(u, id));
            return m;
        }
        String column = "Item Category".equalsIgnoreCase(f) ? "ItemCategoryId"
                      : "Item Type".equalsIgnoreCase(f)     ? "ItemTypeId"
                      : null;
        if (column == null) { m.put("rows", new ArrayList<>()); return m; }

        List<Map<String, Object>> source = input ? lookups.allItems(u) : lookups.itemsForOutputFilter(u);
        List<Map<String, Object>> kept = new ArrayList<>();
        for (Map<String, Object> r : source) {
            if (asInt(r.get(column)) == id) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", r.get("Id"));
                o.put("ItemName", r.get("ItemName"));
                kept.add(o);
            }
        }
        m.put("rows", kept);
        return m;
    }

    /** frmProductionJobOrder :560-570 - cmbentrytype's two literal rows. */
    private static final List<Map<String, Object>> ENTRY_TYPES = staticList(
            new Object[][] { { 1, "Input" }, { 2, "Output" } });

    /** frmProductionJobOrder :572-583 - cmbFiltersType's four literal rows, in the desktop's order. */
    private static final List<Map<String, Object>> FILTER_TYPES = staticList(
            new Object[][] { { 1, "SaleOrder" }, { 2, "Export Contract" },
                             { 3, "Item Type" }, { 4, "Item Category" } });

    private static List<Map<String, Object>> staticList(Object[][] rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Object[] r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r[0]);
            m.put("name", r[1]);
            out.add(m);
        }
        return Collections.unmodifiableList(out);
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static boolean blank(String s)  { return s == null || s.trim().isEmpty(); }
    private static boolean zero(Integer v)  { return v == null || v == 0; }
    private static boolean zeroD(Double v)  { return v == null || v == 0.0; }
}
