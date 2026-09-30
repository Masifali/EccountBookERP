package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.VoucherRequestDto;
import com.mst.repositories.TaxationScreensRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.serviceInterface.IVoucherService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * The BLL side of three Taxation screens:
 *
 *   179 RegularItemsAllocateToTaxItem  "Items Allocation To Tax Item"
 *   175 frmSaleTaxSummaryRpt           "Sale Tax Summary"
 *   174 frmWhtTaxChallanDeposit        "Wht Challan Deposit" (a DocumentTypeId 25 voucher)
 *
 * Rights through DesktopReportRights by each screen's own dbo.ScreenDefinition.Id. Organisation, company
 * and the audit user come from the session, never from the request.
 */
@Service
public class TaxationScreensService {

    public static final int SCREEN_ITEMS_ALLOCATE_TO_TAX_ITEM = 179;
    public static final int SCREEN_SALE_TAX_SUMMARY = 175;
    public static final int SCREEN_WHT_CHALLAN_DEPOSIT = 174;
    /** frmWhtTaxChallanDeposit.cs btnsave_Click: voucher.DocumentTypeId = 25. */
    public static final int WHT_CHALLAN_DOCUMENT_TYPE = 25;

    @Autowired private TaxationScreensRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;
    @Autowired private IVoucherService voucherService;

    private UserAccount user(int screen, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen, action);
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    // ================================================================= 179 Items Allocate To Tax Item

    /** frmItemsCustomizedGroups_Load: TaxableItemFill, ItemCategoryFill, ItemTypeFill. */
    public Map<String, Object> allocationSetup() {
        UserAccount u = user(SCREEN_ITEMS_ALLOCATE_TO_TAX_ITEM, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("taxItems", repo.taxableItems(u)); }
        catch (Exception e) { out.put("taxItemsError", msg(e)); }
        try { out.put("categories", repo.itemCategories(u)); }
        catch (Exception e) { out.put("categoriesError", msg(e)); }
        try { out.put("types", repo.itemTypes(u)); }
        catch (Exception e) { out.put("typesError", msg(e)); }
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, SCREEN_ITEMS_ALLOCATE_TO_TAX_ITEM, "Save"));
        out.put("permissions", perm);
        return out;
    }

    /** ShowData(): UnAllocatedItemsGridFill (ActionId 1) and AllocatedItemsGridFill (ActionId 2); "Select Product Type First" when no tax item. */
    public Map<String, Object> allocationLists(int taxItemId, int categoryId, int typeId) {
        UserAccount u = user(SCREEN_ITEMS_ALLOCATE_TO_TAX_ITEM, "View");
        if (taxItemId <= 0) throw new IllegalArgumentException("Select Product Type First");
        if (!owns(repo.taxableItems(u), taxItemId)) throw new IllegalArgumentException("Select Product Type First");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("unallocated", pairs(repo.allocatedOrUnallocated(u, 1, taxItemId, typeId, categoryId)));
        out.put("allocated", pairs(repo.allocatedOrUnallocated(u, 2, taxItemId, typeId, categoryId)));
        return out;
    }

    private static List<Map<String, Object>> pairs(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", ci(r, "ItemId"));
            m.put("ItemName", ci(r, "ItemName"));
            out.add(m);
        }
        return out;
    }

    /**
     * BtnAllocateItems_Click: "Select Tax Item First"; "Checked Row's first To Allocate Items"; then Save with
     * OrganizationId, CompanyId, EntryUserId, ModifyUserId, EntryDate, ModifyDate, TaxItemId, ItemId,
     * IsActive = true per checked row. Every id must be one of the rows the grid could have shown.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> allocate(Map<String, Object> body) {
        UserAccount u = user(SCREEN_ITEMS_ALLOCATE_TO_TAX_ITEM, "Save");
        int taxItemId = asInt(body.get("taxItemId"));
        int categoryId = asInt(body.get("categoryId"));
        int typeId = asInt(body.get("typeId"));
        List<Integer> ids = ids(body.get("itemIds"));
        if (taxItemId == 0) throw new IllegalArgumentException("Select Tax Item First");
        if (ids.isEmpty()) throw new IllegalArgumentException("Checked Row's first To Allocate Items");
        if (!owns(repo.taxableItems(u), taxItemId)) throw new IllegalArgumentException("Select Tax Item First");
        Set<Integer> shown = idsOf(repo.allocatedOrUnallocated(u, 1, taxItemId, typeId, categoryId), "ItemId");
        for (int id : ids) if (!shown.contains(id)) throw new IllegalArgumentException("Checked Row's first To Allocate Items");
        repo.allocate(u, taxItemId, ids);
        return result(ids.size());
    }

    /**
     * btnDeAllocate_Click: "Select Tax Item First"; "Checked Row's first To Un-Allocate Items"; the page confirms
     * "Are you sure to UnAllocate Item's?"; then DeleteById(TaxItemId, "id,id,").
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> deallocate(Map<String, Object> body) {
        UserAccount u = user(SCREEN_ITEMS_ALLOCATE_TO_TAX_ITEM, "Save");
        int taxItemId = asInt(body.get("taxItemId"));
        int categoryId = asInt(body.get("categoryId"));
        int typeId = asInt(body.get("typeId"));
        List<Integer> ids = ids(body.get("itemIds"));
        if (taxItemId == 0) throw new IllegalArgumentException("Select Tax Item First");
        if (ids.isEmpty()) throw new IllegalArgumentException("Checked Row's first To Un-Allocate Items");
        if (!owns(repo.taxableItems(u), taxItemId)) throw new IllegalArgumentException("Select Tax Item First");
        Set<Integer> shown = idsOf(repo.allocatedOrUnallocated(u, 2, taxItemId, typeId, categoryId), "ItemId");
        for (int id : ids) if (!shown.contains(id)) throw new IllegalArgumentException("Checked Row's first To Un-Allocate Items");
        StringBuilder csv = new StringBuilder();
        for (int id : ids) csv.append(id).append(',');
        repo.deallocate(taxItemId, csv.toString());
        return result(ids.size());
    }

    // ================================================================= 175 Sale Tax Summary

    /**
     * frmSaleTaxSummaryRpt_Load / btnShow_Click: SaleTaxSummary(FromDate, ToDate) once, split into the Output
     * and Input grids on TaxTypeInOut. Columns: TaxTypes, TaxNotesHeading, DebitAmount, CreditAmount, Amount.
     */
    public Map<String, Object> saleTaxSummary(Object fromDate, Object toDate) {
        UserAccount u = user(SCREEN_SALE_TAX_SUMMARY, "View");
        LocalDate from = asDate(fromDate), to = asDate(toDate);
        if (from == null || to == null) throw new IllegalArgumentException("Date From and Date To are required.");
        List<Map<String, Object>> output = new ArrayList<>(), input = new ArrayList<>();
        for (Map<String, Object> r : repo.saleTaxSummary(u, java.sql.Date.valueOf(from), java.sql.Date.valueOf(to))) {
            String kind = String.valueOf(ci(r, "TaxTypeInOut"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("TaxTypes", kind);
            m.put("TaxNotesHeading", ci(r, "TaxNotesHeading"));
            m.put("DebitAmount", asDouble(ci(r, "DebitAmount")));
            m.put("CreditAmount", asDouble(ci(r, "CreditAmount")));
            m.put("Amount", asDouble(ci(r, "Amount")));
            if ("Output".equals(kind)) output.add(m);
            else if ("Input".equals(kind)) input.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("output", output);
        out.put("input", input);
        return out;
    }

    // ================================================================= 174 WHT Challan Deposit

    /**
     * frmWhtTaxChallanDeposit_Load: rights (Save / Print / Update / CanView AllRecord), GenerateCode(25),
     * Company (first row active), braches (first active), Project (first active), BindGLAccount (COAAllocation:
     * Supplier/Customer combo minus AccountTypeId 2/11/15; Credit combo = every row).
     */
    public Map<String, Object> whtChallanSetup() {
        UserAccount u = user(SCREEN_WHT_CHALLAN_DEPOSIT, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("docNo", voucherService.generateNextVoucherCode(WHT_CHALLAN_DOCUMENT_TYPE)); }
        catch (Exception e) { out.put("docNoError", msg(e)); }
        try { out.put("companies", repo.companies(u)); } catch (Exception e) { out.put("companiesError", msg(e)); }
        try { out.put("branches", repo.branches(u)); } catch (Exception e) { out.put("branchesError", msg(e)); }
        try { out.put("projects", repo.projects(u)); } catch (Exception e) { out.put("projectsError", msg(e)); }
        try {
            List<Map<String, Object>> all = repo.coaAllocation(u);
            List<Map<String, Object>> party = new ArrayList<>(), credit = new ArrayList<>();
            for (Map<String, Object> r : all) {
                int t = asInt(ci(r, "AccountTypeId"));
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", ci(r, "Id"));
                m.put("AccountTitle", ci(r, "AccountTitle"));
                credit.add(m);
                if (t != 2 && t != 11 && t != 15) party.add(m);
            }
            out.put("partyAccounts", party);
            out.put("creditAccounts", credit);
        } catch (Exception e) { out.put("accountsError", msg(e)); }
        Map<String, Object> perm = new LinkedHashMap<>();
        for (String a : new String[] { "Save", "Print", "Update", "CanView AllRecord" }) perm.put(a, allowed(u, SCREEN_WHT_CHALLAN_DEPOSIT, a));
        out.put("permissions", perm);
        return out;
    }

    /** GenerateCode() - CommonServices.GenerateVoucherCode(25). */
    public Map<String, Object> whtChallanDocNo() {
        user(SCREEN_WHT_CHALLAN_DEPOSIT, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", voucherService.generateNextVoucherCode(WHT_CHALLAN_DOCUMENT_TYPE));
        return out;
    }

    /**
     * cmbSupplierCustomerGlAcc_Leave: DebitAccountBind (GetDebitAcBySupplierCustomerGLAcIdForChallanDeposit) and
     * the pending grid (GetAllVoucherForChallanDeposit with the Branch and Project combos).
     */
    public Map<String, Object> whtChallanPending(int partyAccountId, int branchId, int projectId) {
        UserAccount u = user(SCREEN_WHT_CHALLAN_DEPOSIT, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("debitAccounts", repo.challanDebitAccounts(u, partyAccountId));
        out.put("pending", repo.pendingWhtChallan(u, branchId, projectId, partyAccountId));
        return out;
    }

    /** HistoryFill - CommonServices.VoucherFormHistory(rights, "25"). */
    public Map<String, Object> whtChallanHistory() {
        user(SCREEN_WHT_CHALLAN_DEPOSIT, "View");
        return voucherService.getWhtChallanDepositHistory();
    }

    /** DataGridHistory "View" button - VoucherHead.GetByID(id): the saved voucher's own detail lines. */
    public Map<String, Object> whtChallanVoucher(int id) {
        user(SCREEN_WHT_CHALLAN_DEPOSIT, "View");
        if (id <= 0) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> v = voucherService.getVoucherById(id);
        if (v == null) throw new IllegalArgumentException("Record not found.");
        return v;
    }

    /**
     * btnsave_Click - FormValidation in the desktop's order ("SupplierCustomer Field required", "Debit Gl Account
     * Field required", "Credit Gl Account Field required", "Company Name Field required", "Branches Name Field
     * required", "Project Name Field required", "Doc NO Field required"), "Checked row first", then
     * VoucherHead.Save with DocumentTypeId 25:
     *
     *   head: VoucherCode = txtDocNo, BranchId, ProjectId, VoucherDate, RefAccountId = Credit GL Ac,
     *         AgainstAccountId = Debit GL Ac, Remarks; VoucherAmount = sum of the checked rows (BLL Save(25)).
     *   one detail per checked row: AccountId = Debit GL Ac, AgainstAccountId = Credit GL Ac,
     *         InvoiceNoRefId = the row's hId, Comments = Remarks, DebitAmount = the row's CreditAmount,
     *         CreditAmount = 0, IsTaxable = "False" - and Sp_VoucherHead_WHTTaxChallanDeposit_Update @Id=hId.
     *   then, per row, the BLL adds the mirror: AccountId = Credit GL Ac (RefAccountId), AgainstAccountId =
     *         Debit GL Ac, CreditAmount = amount, same Comments, IsTaxable "False".
     *
     * VoucherService.saveVoucher with mirrorCreditToRefAccount = true writes exactly that pair (primary line
     * against dto.refAccountId, mirror on refAccountId against the line's account), so it is reused rather
     * than re-implemented; the source-voucher stamp runs here in the same transaction. The Company combo is
     * validated but, as on every other web voucher, the company written is the session's own.
     * "Record Save Successfully" + VoucherCode is the desktop's own message.
     */
    @Transactional(rollbackFor = Exception.class)
    public Map<String, Object> saveWhtChallan(Map<String, Object> body) {
        UserAccount u = user(SCREEN_WHT_CHALLAN_DEPOSIT, "Save");
        int party = asInt(body.get("partyAccountId"));
        int debit = asInt(body.get("debitAccountId"));
        int credit = asInt(body.get("creditAccountId"));
        int company = asInt(body.get("companyId"));
        int branch = asInt(body.get("branchId"));
        int project = asInt(body.get("projectId"));
        String docNoText = text(body.get("docNo"));
        LocalDate docDate = asDate(body.get("docDate"));
        String remarks = body.get("remarks") == null ? "" : String.valueOf(body.get("remarks"));
        List<Integer> checked = ids(body.get("sourceIds"));
        if (party == 0) throw new IllegalArgumentException("SupplierCustomer Field required");
        if (debit == 0) throw new IllegalArgumentException("Debit Gl Account Field required");
        if (credit == 0) throw new IllegalArgumentException("Credit Gl Account Field required");
        if (company == 0) throw new IllegalArgumentException("Company Name Field required");
        if (branch == 0) throw new IllegalArgumentException("Branches Name Field required");
        if (project == 0) throw new IllegalArgumentException("Project Name Field required");
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw new IllegalArgumentException("Doc NO Field required");
        if (docDate == null) throw new IllegalArgumentException("Doc Date Field required");
        if (checked.isEmpty()) throw new IllegalArgumentException("Checked row first");

        // Every id came from a combo this session was shown; every checked row from the pending grid.
        List<Map<String, Object>> coa = repo.coaAllocation(u);
        if (!owns(coa, party) || !owns(coa, credit)) throw new IllegalArgumentException("Credit Gl Account Field required");
        if (!idsOf(repo.challanDebitAccounts(u, party), "RefDocNoId").contains(debit)) throw new IllegalArgumentException("Debit Gl Account Field required");
        if (!owns(repo.companies(u), company)) throw new IllegalArgumentException("Company Name Field required");
        if (!owns(repo.branches(u), branch)) throw new IllegalArgumentException("Branches Name Field required");
        if (!owns(repo.projects(u), project)) throw new IllegalArgumentException("Project Name Field required");
        List<Map<String, Object>> pending = repo.pendingWhtChallan(u, branch, project, party);
        Map<Integer, Double> amounts = new LinkedHashMap<>();
        for (Map<String, Object> r : pending) amounts.put(asInt(ci(r, "hId")), asDouble(ci(r, "CreditAmount")));
        for (int id : checked) if (!amounts.containsKey(id)) throw new IllegalArgumentException("Checked row first");

        VoucherRequestDto dto = new VoucherRequestDto();
        dto.setDocumentTypeId(WHT_CHALLAN_DOCUMENT_TYPE);
        dto.setVoucherCode(asInt(docNoText));
        dto.setVoucherDate(docDate);
        dto.setBranchId(branch);
        dto.setProjectId(project);
        dto.setRefAccountId(credit);
        dto.setAgainstAccountId(debit);
        dto.setRemarks(remarks);
        dto.setMirrorCreditToRefAccount(Boolean.TRUE);
        double total = 0;
        List<VoucherRequestDto.VoucherDetailRowDto> lines = new ArrayList<>();
        for (int hId : new LinkedHashSet<>(checked)) {
            double amount = amounts.get(hId);
            VoucherRequestDto.VoucherDetailRowDto d = new VoucherRequestDto.VoucherDetailRowDto();
            d.setAccountId(debit);
            d.setInvoiceNoRefId(hId);
            d.setComments(remarks);
            d.setDebitAmount(amount);
            d.setCreditAmount(0.0);
            d.setIsTaxable("False");
            lines.add(d);
            total += amount;
        }
        dto.setVoucherAmount(total);
        dto.setDetails(lines);
        Map<String, Object> saved = voucherService.saveVoucher(dto);
        if (saved == null || Boolean.FALSE.equals(saved.get("success"))) {
            throw new IllegalStateException(saved == null ? "Save failed." : String.valueOf(saved.get("message")));
        }
        for (int hId : new LinkedHashSet<>(checked)) repo.markSourceVoucherDeposited(hId);
        Map<String, Object> out = new LinkedHashMap<>(saved);
        out.put("success", true);
        out.put("voucherCode", dto.getVoucherCode());
        out.put("message", "Record Save Successfully" + dto.getVoucherCode());
        return out;
    }

    // ================================================================= helpers

    private static boolean owns(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (asInt(ci(r, "Id")) == id) return true;
        return false;
    }

    private static Set<Integer> idsOf(List<Map<String, Object>> rows, String column) {
        Set<Integer> out = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) out.add(asInt(ci(r, column)));
        return out;
    }

    private static List<Integer> ids(Object v) {
        List<Integer> out = new ArrayList<>();
        if (v instanceof List<?> list) for (Object o : list) { int n = asInt(o); if (n > 0) out.add(n); }
        return out;
    }

    private static Map<String, Object> result(int n) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("count", n);
        return out;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim()); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    private static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }

    private static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) { return null; }
    }
}
