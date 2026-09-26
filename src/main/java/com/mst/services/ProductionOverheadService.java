package com.mst.services;

import com.mst.repositories.ProductionOverheadRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ProductionOverheadRepository.col;
import static com.mst.repositories.ProductionOverheadRepository.toDouble;
import static com.mst.repositories.ProductionOverheadRepository.toInt;

/**
 * Screen 280, Overhead tab - frmProductionOverhead.cs.
 *
 * Tenancy, financial year, branch and user come from the session (CurrentUserContext) and never
 * from the request, as the desktop takes them from UserAccount / clsGlobalVariables.ActiveYr.
 *
 * The page holds the grid (tableOverHeads) exactly as the desktop form holds it in memory; the
 * server re-runs every validation OverHeadInsert runs before anything is written, so a crafted
 * request cannot skip one, and then performs DAL InvFoodProductionOverHeads.SetData line for line
 * in one transaction.
 */
@Service
public class ProductionOverheadService {

    /** ChartOfAccountOverHead:593 - the account types the OverHead Account combo keeps. */
    private static final Set<Integer> COA_TYPES = Set.of(6, 8, 11, 12, 13, 14, 20, 21);

    @Autowired private ProductionOverheadRepository repo;
    @Autowired private CurrentUserContext user;

    private int org()    { return user.currentOrganizationId(); }
    private int comp()   { return user.currentCompanyId(); }
    private int branch() { return user.currentBranchId(); }
    private int year()   { return user.currentFinancialYearId(); }

    // ============================================================================== page state

    /**
     * Everything frmFoodProduction_Load:216 decides: the rights object, the generated doc no and
     * the four lists it binds (Charges Type is hard-coded on the desktop, :571-572, so it is here).
     */
    public Map<String, Object> state() {
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> rights = rights();
        out.put("rights", rights);
        out.put("docNo", generateCode());
        out.put("chargesTypes", chargesTypes());
        out.put("chartOfAccounts", chartOfAccounts());
        out.put("rateUoms", rateUoms());
        out.put("jobOrders", jobOrders());
        return out;
    }

    /**
     * frmFoodProduction_Load:223-235 and CommonServices.SetRightsValueInRightsObject (:14738).
     *
     * The screen name is decided first: "FoodProductionWithValues" when the user's view-rights
     * list holds ANY row for that screen, otherwise the form's own base.Name
     * "frmProductionOverhead". Then only the role spelled exactly "Admin" starts with
     * Save/Update/Print; grant rows set them for everyone else, and "Rate"
     * (DoHaveCanRateandAmount) comes only from a grant row, for Admin too.
     *
     * An unreadable grid grants nothing beyond the Admin defaults.
     */
    public Map<String, Object> rights() {
        String screen = ProductionOverheadRepository.OWN_SCREEN_NAME;
        try {
            for (Map<String, Object> r : repo.screenViewRights(user.currentUserId(), comp(), user.currentAppId())) {
                Object n = col(r, "ScreenName");
                if (n != null && ProductionOverheadRepository.SHELL_SCREEN_NAME.equals(String.valueOf(n))) {
                    screen = ProductionOverheadRepository.SHELL_SCREEN_NAME;
                    break;
                }
            }
        } catch (Exception ignored) {
            /* an empty ScreenViewReights list: Any() is false and base.Name is used */
        }
        String role = user.currentRoleName();
        boolean admin = "Admin".equals(role);
        boolean save = admin, update = admin, print = admin, rate = false;
        try {
            for (Map<String, Object> row : repo.userRights(user.currentUserId(), screen, role, comp())) {
                String name = str(col(row, "RightName"));
                boolean v = asBool(col(row, "Value"));
                switch (name) {
                    case "Save":   save   = admin || v; break;
                    case "Update": update = admin || v; break;
                    case "Print":  print  = admin || v; break;
                    case "Rate":   rate   = v;          break;
                    default: break;
                }
            }
        } catch (Exception ignored) { /* keep the defaults */ }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("screenName", screen);
        m.put("save", save);
        m.put("update", update);
        m.put("print", print);
        m.put("rateAndAmount", rate);
        return m;
    }

    // ================================================================================= lookups

    /** GenerateCodeofOverHead:546 - only a code greater than zero is shown. */
    public int generateCode() {
        return repo.generateCode(org(), comp(), year(), branch());
    }

    /** ChargestypeOverHeadsOverHead:564 - hard-coded on the desktop. */
    public List<Map<String, Object>> chargesTypes() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(pair(1, "Processing Charges"));
        l.add(pair(2, "Other Charges"));
        return l;
    }

    /**
     * ChartOfAccountOverHead:584 - COAAllocationSearch filtered by the form itself: AccountTypeId
     * in {6,8,11,12,13,14,20,21}, or AccountTypeId 10 with PLNoteId 3. Id and AccountTitle only.
     */
    public List<Map<String, Object>> chartOfAccounts() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.coaAllocation(org(), comp(), user.currentUserId())) {
            int type = toInt(col(r, "AccountTypeId"));
            int note = toInt(col(r, "PLNoteId"));
            if (COA_TYPES.contains(type) || (type == 10 && note == 3)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", col(r, "Id"));
                m.put("AccountTitle", col(r, "AccountTitle"));
                out.add(m);
            }
        }
        return out;
    }

    /** UOMOverHead:617 - Id, UOM (from UOMCode), Equivalent. */
    public List<Map<String, Object>> rateUoms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomStatic()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("UOM", col(r, "UOMCode"));
            m.put("Equivalent", col(r, "Equivalent"));
            out.add(m);
        }
        return out;
    }

    /** JobOrderNoFill:342 - Id, PlanCode, DocumentTypeId (the third column hidden in the combo). */
    public List<Map<String, Object>> jobOrders() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.jobOrders(org(), comp(), branch())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", col(r, "Id"));
            m.put("PlanCode", col(r, "PlanCode"));
            m.put("DocumentTypeId", col(r, "DocumentTypeId"));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> plants(int jobOrderId) {
        return repo.plants(org(), comp(), year(), branch(), jobOrderId);
    }

    /** RadInputItemOH.Checked ? 80 : 112 - the page sends which radio is on. */
    public List<Map<String, Object>> brandItems(int jobOrderId, int documentTypeId) {
        return repo.brandItems(org(), comp(), jobOrderId, 0, documentTypeId);
    }

    public List<Map<String, Object>> brandUoms(int jobOrderId, int itemId) {
        return repo.brandUoms(org(), comp(), jobOrderId, itemId);
    }

    public List<Map<String, Object>> inputOutputTotals(int jobOrderId) {
        return repo.inputOutputTotals(org(), comp(), jobOrderId);
    }

    public List<Map<String, Object>> outputQty(int jobOrderId, int brandId, int brandUomId, int documentTypeId) {
        return repo.outputQty(org(), comp(), jobOrderId, brandId, brandUomId, documentTypeId);
    }

    /** BtnLoadFohOverHeads_Click:1779 - clsGlobalVariables.UserAccount + ActiveYr, same values. */
    public List<Map<String, Object>> fohExpenses(int jobOrderId, int plantId, LocalDate docDate) {
        return repo.fohExpenses(org(), comp(), year(), branch(), plantId, jobOrderId, docDate);
    }

    // =================================================================================== reads

    /**
     * ReadByIdOverHeadJobOrderWise:944 - the rows, plus GetVoucherHeadId(RecOverHeadId, 110)
     * (:999). RecOverHeadId is the JOB ORDER id there, not a detail row id, while SetData keys
     * each voucher by its detail row id - the lookup is reproduced as written.
     */
    public Map<String, Object> byJobOrder(int jobOrderId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = repo.byJobOrder(jobOrderId);
        out.put("rows", rows);
        int vh = 0;
        if (!rows.isEmpty()) {
            String approved = str(col(rows.get(0), "JobOrderStatus"));
            String plan = str(col(rows.get(0), "PlanStatus"));
            if (!"Approved".equals(approved) && "In Process".equals(plan)) {
                vh = repo.voucherHeadId(org(), comp(), ProductionOverheadRepository.DOC_TYPE_ID, jobOrderId);
            }
        }
        out.put("voucherHeadId", vh);
        return out;
    }

    /** grdOverHeadHistory_SelectionChanged:1478 - the same read, no checks, no voucher. */
    public List<Map<String, Object>> historyDetail(int jobOrderId) {
        return repo.byJobOrder(jobOrderId);
    }

    /** BindGridHistoryOverHead:1326 - the page groups by job order exactly as the LINQ does. */
    public List<Map<String, Object>> history() {
        return repo.history(org(), comp(), year(), branch());
    }

    /** btnPrintOverHead_Click:1583 - the row count decides "Record Not Found For DisPlay". */
    public int report606Rows(int jobOrderId) {
        return repo.report606(org(), comp(), jobOrderId).size();
    }

    // ==================================================================================== save

    /** A validation the desktop answers with a MessageBox and stops; nothing is written. */
    public static class OverheadValidationException extends RuntimeException {
        public OverheadValidationException(String m) { super(m); }
    }

    /**
     * OverHeadInsert:779 (Save and Update both come here) then InvFoodProductionOverHeads.Save
     * (BLL) and DAL SetData, in ONE transaction.
     *
     * The page has already run FormValidationOfOverHead and the Yes/No confirmation; both
     * checks of FormValidationOfOverHead are repeated here.
     *
     * @return "Save Successfully" or "Update Successfully", the desktop's own text (:905/:909)
     */
    @Transactional(rollbackFor = Exception.class)
    public String save(Map<String, Object> body) {
        Map<String, Object> rights = rights();
        int recOverHeadId = toInt(body.get("recOverHeadId"));
        /* toolStrip5.Enabled = Print right (:234) disables every toolbar button; Save is further
           gated by the Save right (:231). The toolbar Update is not gated by Update - the desktop
           applies that right to the detail "Update" button (:232) instead. */
        if (!Boolean.TRUE.equals(rights.get("print"))
                || (recOverHeadId == 0 && !Boolean.TRUE.equals(rights.get("save")))) {
            throw new OverheadValidationException("You do not have the right to perform this action.");
        }

        /* FormValidationOfOverHead:1541 */
        String docNoText = str(body.get("docNo"));
        if (docNoText.isEmpty()) throw new OverheadValidationException("Doc No. Field Required");
        int jobOrderId = toInt(body.get("jobOrderId"));
        Map<String, Object> jobRow = null;
        for (Map<String, Object> j : repo.jobOrders(org(), comp(), branch())) {
            if (toInt(col(j, "Id")) == jobOrderId && jobOrderId != 0) { jobRow = j; break; }
        }
        if (jobRow == null) throw new OverheadValidationException("Job Order No. Field Required");
        /* :832 - BaseDocumentTypeId = 2 only when the chosen job order's DocumentTypeId is 401. */
        int baseDocumentTypeId = toInt(col(jobRow, "DocumentTypeId")) == 401 ? 2 : 0;

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        int uid = user.currentUserId();
        List<Map<String, Object>> list = new ArrayList<>();
        Object rowsObj = body.get("rows");
        List<?> rows = rowsObj instanceof List ? (List<?>) rowsObj : new ArrayList<>();
        for (Object o : rows) {
            if (!(o instanceof Map)) continue;
            @SuppressWarnings("unchecked") Map<String, Object> r = (Map<String, Object>) o;
            /* :815 - Conversion.ToInt(ItemQty) <= 0 && !(ItemRate > 0) && !(Amount > 0) -> skipped. */
            if (netToInt(r.get("ItemQty")) <= 0 && !(toDouble(r.get("ItemRate")) > 0)
                    && !(toDouble(r.get("Amount")) > 0)) continue;

            Map<String, Object> m = new LinkedHashMap<>();
            m.put("OrganizationId", org());
            m.put("CompanyId", comp());
            m.put("BranchesId", branch());
            m.put("FinancialYearId", year());
            m.put("DocumentTypeId", ProductionOverheadRepository.DOC_TYPE_ID);
            m.put("EntryDate", now);
            m.put("EntryUser", uid);
            m.put("ModifyDate", now);
            m.put("ModifyUser", uid);
            m.put("Id", toInt(r.get("Id")));
            m.put("DocNo", toInt(r.get("DocNo")));
            m.put("DocDate", parseDate(r.get("DocDate")));
            m.put("InvProductionJobOrderId", jobOrderId);
            m.put("BaseDocumentTypeId", baseDocumentTypeId);
            m.put("PlantId", toInt(r.get("PlantId")));
            m.put("ChargesTypeId", toInt(r.get("ChargesTypeId")));
            m.put("CharOfAccountId", toInt(r.get("ChartOfAccountId")));
            m.put("BrandId", toInt(r.get("BrandId")));
            m.put("BrandUomId", toInt(r.get("BrandUomId")));
            m.put("ItemQty", toDouble(r.get("ItemQty")));
            m.put("ItemRate", toDouble(r.get("ItemRate")));
            m.put("RateUomId", toInt(r.get("RateUomId")));
            m.put("Amount", toDouble(r.get("Amount")));
            m.put("ohRemarks", r.get("Remarks") == null ? "" : String.valueOf(r.get("Remarks")));
            m.put("DetailDocumentTypeId", toInt(r.get("DetailDocumentTypeId")));
            /* never assigned by the form: the C# default */
            m.put("JobOrderOverHeadId", 0);

            int plant = (Integer) m.get("PlantId"), charges = (Integer) m.get("ChargesTypeId");
            int coa = (Integer) m.get("CharOfAccountId"), brand = (Integer) m.get("BrandId");
            int brandUom = (Integer) m.get("BrandUomId"), rateUom = (Integer) m.get("RateUomId");
            double qty = (Double) m.get("ItemQty"), rate = (Double) m.get("ItemRate");
            double amount = (Double) m.get("Amount");
            if (plant == 0) throw new OverheadValidationException("Plant Field Required in Detail");
            if (charges == 0) throw new OverheadValidationException("ChargesType Field Require in Detail");
            if (coa == 0) throw new OverheadValidationException("OverHead Account Field Require in Detail");
            if (brand == 0 && brandUom != 0) throw new OverheadValidationException("OutPut Item Field Require in Detail");
            if (brandUom == 0 && brand != 0) throw new OverheadValidationException("OutPut UomField Require in Detail");
            if (charges != 3) {
                if (qty == 0.0 && rate != 0.0) throw new OverheadValidationException("ItemQty Required in Detail");
                if (rate == 0.0 && qty != 0.0) throw new OverheadValidationException("ItemRate Required in Detail");
                if (rateUom == 0 && brand == 0 && brandUom == 0 && charges == 1)
                    throw new OverheadValidationException("RateUom Required in Detail");
            }
            if (amount == 0.0) throw new OverheadValidationException("Amount Required in Detail");
            list.add(m);
        }
        if (list.isEmpty()) throw new OverheadValidationException("Grid Record not found please check!");

        setData(list);
        return recOverHeadId > 0 ? "Update Successfully" : "Save Successfully";
    }

    /**
     * DAL Architecture.DAL.Production.InvFoodProductionOverHeads.SetData, per row:
     *
     *  1. Id == 0 -> Sp_InvFoodProductionOverHeads_Insert, else _Update (SetProc, ExecuteScalar).
     *     A positive result is the new Id and ModifyUser is zeroed; otherwise EntryUser is zeroed
     *     and the row keeps its own Id.
     *  2. When config InventoryFinancialsEffectsInActive is true, no voucher is written.
     *  3. ModifyUser > 0 (the update path) looks up the row's existing voucher
     *     (DocumentTypeId 110, DocumentTypeSrNo = row Id).
     *  4. The job order's WIP account (GetGlAccountsByJobOrderId); none -> "JobOrder WIPAccount
     *     Not Found", which rolls everything back.
     *  5. Voucher head (Insert when none was found, else Update): DocumentTypeSrNo = row Id,
     *     VoucherDate = DocDate, VoucherCode = DocNo, DocumentTypeId 110, BaseDocumentTypeId,
     *     RefAccountId = the overhead account, AgainstAccountId = WIP, VoucherAmount = Amount,
     *     EntryDate/ModifyDate now, users, organisation, company, year, BranchId = BranchesId.
     *  6. Two lines: Dr WIP / Cr overhead account and Cr overhead account / Dr WIP, each with
     *     Comments = ohRemarks and OrderNo = the job order id.
     *  7. USP_VoucherBalanceCheck on the voucher head.
     *
     * OHDetailRowsRemoveIds is never set by this form (the grid refuses deletes in update mode),
     * so USP_InvFoodProductionOHRowsDeleteByIds is never reached from this screen.
     * GetERPFeaturesByCompanyId(org, comp, 5) is called by the DAL and its result discarded; it has
     * no effect and is not repeated.
     */
    private void setData(List<Map<String, Object>> list) {
        int o = org(), c = comp();
        boolean financialsInactive = asBool(repo.configKey(o, c, "InventoryFinancialsEffectsInActive"));
        for (Map<String, Object> row : list) {
            boolean insert = toInt(row.get("Id")) == 0;
            int loc0 = repo.saveOverheadRow(insert, at(row));
            if (loc0 > 0) {
                row.put("ModifyUser", 0);
                row.put("Id", loc0);
            } else {
                row.put("EntryUser", 0);
                loc0 = toInt(row.get("Id"));
            }
            if (financialsInactive) continue;

            Map<String, Object> vh = ProductionOverheadRepository.voucherHeadDefaults();
            int loc1 = 0;
            if (toInt(row.get("ModifyUser")) > 0) {
                loc1 = repo.voucherHeadIdInTransaction(o, c, toInt(row.get("DocumentTypeId")), toInt(row.get("Id")));
                vh.put("Id", loc1);
            }
            int jobOrderId = toInt(row.get("InvProductionJobOrderId"));
            List<Map<String, Object>> gl = repo.glAccountsByJobOrderId(o, c, jobOrderId);
            if (gl.isEmpty()) throw new OverheadValidationException("JobOrder WIPAccount Not Found");
            int wip = toInt(col(gl.get(0), "WorkInProccessAcId"));
            int coa = toInt(row.get("CharOfAccountId"));
            double amount = toDouble(row.get("Amount"));
            String remarks = row.get("ohRemarks") == null ? "" : String.valueOf(row.get("ohRemarks"));
            Object docDate = row.get("DocDate");

            vh.put("DocumentTypeSrNo", loc0);
            vh.put("VoucherDate", docDate == null ? null : Timestamp.valueOf(((LocalDate) docDate).atStartOfDay()));
            vh.put("VoucherCode", toInt(row.get("DocNo")));
            vh.put("DocumentTypeId", toInt(row.get("DocumentTypeId")));
            vh.put("BaseDocumentTypeId", toInt(row.get("BaseDocumentTypeId")));
            vh.put("RefAccountId", coa);
            vh.put("AgainstAccountId", wip);
            Timestamp now = Timestamp.valueOf(LocalDateTime.now());
            vh.put("EntryDate", now);
            vh.put("ModifyDate", now);
            vh.put("VoucherAmount", amount);
            vh.put("EntryUser", toInt(row.get("EntryUser")));
            vh.put("ModifyUser", toInt(row.get("ModifyUser")));
            vh.put("OrganizationId", toInt(row.get("OrganizationId")));
            vh.put("CompanyId", toInt(row.get("CompanyId")));
            vh.put("FinancialYearId", toInt(row.get("FinancialYearId")));
            vh.put("BranchId", toInt(row.get("BranchesId")));

            List<Map<String, Object>> lines = new ArrayList<>();
            Map<String, Object> d1 = ProductionOverheadRepository.voucherDetailDefaults();
            d1.put("AccountId", wip);
            d1.put("AgainstAccountId", coa);
            d1.put("Comments", remarks);
            d1.put("DebitAmount", amount);
            d1.put("OrderNo", jobOrderId);
            lines.add(d1);
            Map<String, Object> d2 = ProductionOverheadRepository.voucherDetailDefaults();
            d2.put("AccountId", coa);
            d2.put("AgainstAccountId", wip);
            d2.put("Comments", remarks);
            d2.put("CreditAmount", amount);
            d2.put("OrderNo", jobOrderId);
            lines.add(d2);

            int loc2 = repo.saveVoucherHead(loc1 == 0, at(vh));
            if (loc2 > 0) vh.put("Id", loc2);
            int headId = toInt(vh.get("Id"));
            for (Map<String, Object> d : lines) {
                d.put("VoucherHeadId", headId);
                repo.insertVoucherDetail(at(d));
            }
            if (headId > 0) repo.voucherBalanceCheck(toInt(row.get("OrganizationId")), toInt(row.get("CompanyId")), headId);
        }
    }

    // ================================================================================ helpers

    /** Model property -> @PropertyName, as GenericProvider.SetProc names them. LocalDate -> SQL date. */
    private static Map<String, Object> at(Map<String, Object> model) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : model.entrySet()) {
            Object v = e.getValue();
            if (v instanceof LocalDate) v = java.sql.Date.valueOf((LocalDate) v);
            p.put("@" + e.getKey(), v);
        }
        return p;
    }

    /** Conversion.ToInt of a double: Convert.ToInt32, which rounds half to even. */
    private static int netToInt(Object o) {
        return (int) Math.rint(toDouble(o));
    }

    private static LocalDate parseDate(Object o) {
        String s = str(o);
        if (s.length() >= 10) {
            try { return LocalDate.parse(s.substring(0, 10)); } catch (Exception ignored) { }
        }
        return null;
    }

    private static Map<String, Object> pair(int id, String type) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", id);
        m.put("Type", type);
        return m;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    /** Conversion.ToBool - true/false, 1/0 alike. */
    private static boolean asBool(Object o) {
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = str(o).toLowerCase();
        return "1".equals(s) || "true".equals(s) || "yes".equals(s);
    }
}
