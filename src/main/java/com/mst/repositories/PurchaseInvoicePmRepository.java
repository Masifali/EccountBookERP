package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.ProcExec;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 501 "Purchase Invoice PM" - {@code PurchaseInvoicePackingMaterial.cs}, DocumentTypeId 702.
 * Reads only; each is the desktop's own BLL call with its parameters sent when the BLL sends them.
 * Writes go through {@link PurchaseInvoiceWriteRepository} (the DAL 0434 contracts).
 */
@Repository
public class PurchaseInvoicePmRepository {

    public static final int DOCUMENT_TYPE_ID = 702;
    public static final int GRN_DOCUMENT_TYPE_ID = 701;
    public static final String SCREEN = "PurchaseInvoicePackingMaterial";

    private final JdbcTemplate jdbc;

    public PurchaseInvoicePmRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** CommonServices.PurchaseInvoiceGenerateCode(702) -> GenerateInvPurchaseInvoiceCode. */
    public int nextDocNo(UserAccount u, int fy) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy, "GenerateCode");
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "DocNo"));
    }

    /** BranchSrNoFill :449 -> GenerateBranchCode. */
    public int nextBranchSrNo(UserAccount u, int fy) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy, u.getBranchesId(), "GenerateBranchSrNo");
        if (rows.isEmpty()) return 0;
        Iterator<Object> it = rows.get(0).values().iterator();
        return it.hasNext() ? intOf(it.next()) : 0;
    }

    /** SupplierNameFilll :479 -> InvGrn.GetoutstandingGrnPartiesForInvoice (DocumentTypeId 701). Id, CompanyName, GlAccountId. */
    public List<Map<String, Object>> suppliers(UserAccount u, int fy) {
        return jdbc.queryForList("EXEC dbo.USP_GetoutstandingGrnPartiesForInvoice @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), fy, GRN_DOCUMENT_TYPE_ID);
    }

    /** AccountsFill :396 - the same list GRN PM's transporter combo uses: Id (account), SupplierCustomerId, AccountTitle. */
    public List<Map<String, Object>> accounts(UserAccount u, boolean subsidiary, GrnPmRepository grn) {
        return grn.transporters(u, subsidiary);
    }

    /** DatatableHelper.GetTaxAccountsFromGlobal - account types 3,6,8,16,17,18,19, one row per account. */
    public List<Map<String, Object>> taxAccounts(UserAccount u) {
        Set<Integer> types = Set.of(3, 6, 8, 16, 17, 18, 19);
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
            if (!types.contains(intOf(col(r, "AccountTypeId")))) continue;
            int id = intOf(col(r, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", id);
            m.put("AccountTitle", Objects.toString(col(r, "AccountTitle"), ""));
            m.put("AccountCode", Objects.toString(col(r, "AccountCode"), ""));
            m.put("ParentAccountTitle", Objects.toString(col(r, "ParentAccountTitle"), ""));
            out.add(m);
        }
        return out;
    }

    /** HistoryBranchComboFill :1196 - branches the user has in PI 702 (unless PurchaseInvoiceBranchWise). */
    public List<Map<String, Object>> historyBranches(UserAccount u) {
        return jdbc.queryForList("EXEC [dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice] @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), DOCUMENT_TYPE_ID);
    }

    /** HistoryComboFill :1138 - Usp_AllComboAgainstPurchaseInvoice, rows with Activity "Supplier". */
    public List<Map<String, Object>> historySuppliers(UserAccount u, String branchIds) {
        StringBuilder sql = new StringBuilder("EXEC [dbo].[Usp_AllComboAgainstPurchaseInvoice] @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID)));
        if (branchIds != null && !branchIds.isEmpty()) { sql.append(", @BranchesIds=?"); a.add(branchIds); }
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList(sql.toString(), a.toArray())) {
            if (!"Supplier".equals(Objects.toString(col(r, "Activity"), ""))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("Description", col(r, "ReferenceName"));
            out.add(m);
        }
        return out;
    }

    /** LoadGrnForStore (Filterstatus 1, DocumentTypeId 701) -> InvGrn.GrnLoadForStoreInvoicePurchaseOrderBase. */
    public List<Map<String, Object>> pendingGrns(UserAccount u, java.sql.Date from, java.sql.Date to) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @OrderTaxable=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), GRN_DOCUMENT_TYPE_ID, false));
        if (from != null) { sql.append(", @fromDate=?"); a.add(from); }
        if (to != null) { sql.append(", @toDate=?"); a.add(to); }
        sql.append(", @Activity=?"); a.add("GrnLoadForStoreInvoicePurchaseOrderBase");
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** InvPurchaseInvoice.LoadGrnForPackingMaterialInvoiceByGrnIds. */
    public List<Map<String, Object>> grnLines(String grnIds) {
        if (grnIds == null || grnIds.isBlank()) return List.of();
        return jdbc.queryForList("EXEC dbo.USP_LoadGrnForPackingMaterialInvoiceByGrnIds @GdnIds=?", grnIds);
    }

    /** GetTransporterAndFreightFromGrn. */
    public List<Map<String, Object>> grnFreight(String grnIds) {
        if (grnIds == null || grnIds.isBlank()) return List.of();
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?, @Activity=?", grnIds, "GetTransporterAndFreightFromGrn");
    }

    /** GetWagesAmountByRefDocumentTypeIdandRefIds(org, comp, 701, ids) - WagesAmount of row 0, else null. */
    public Double wages(UserAccount u, String grnIds) {
        var rows = jdbc.queryForList("EXEC dbo.USP_GetWagesAmountByRefDocumentTypeIdandRefIds @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefIds=?",
                u.getOrganizationId(), u.getCompanyId(), GRN_DOCUMENT_TYPE_ID, grnIds == null ? "" : grnIds);
        return rows.isEmpty() ? null : PurchaseOrderPmRepository.dbl(col(rows.get(0), "WagesAmount"));
    }

    /** GrnIds of a set of GRN rows must all be this company's 701 GRNs. */
    public Set<Integer> ownGrns(UserAccount u, Collection<Integer> ids) {
        Set<Integer> out = new HashSet<>();
        for (Integer id : ids) {
            if (id == null || id <= 0) continue;
            var rows = jdbc.queryForList("SELECT Id FROM dbo.InvGrn WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=?",
                    id, u.getOrganizationId(), u.getCompanyId(), GRN_DOCUMENT_TYPE_ID);
            if (!rows.isEmpty()) out.add(id);
        }
        return out;
    }

    /** CommonServices.GetTaxTypeIdAndPercentByItemId. */
    public List<Map<String, Object>> taxSchedule(UserAccount u, int itemId, java.sql.Date effected) {
        return jdbc.queryForList("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @EffectedDate=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemId, effected, "GetItemTaxScheduleForItemId");
    }

    // ================================================================== history / record

    public static final class HistoryFilter {
        public boolean canViewAll;
        public int financialYearId;
        public java.sql.Date from, to;
        public int fromDocNo, toDocNo, supplierId;
        public String branchIds;
    }

    /** GetAll :1569 -> PurchaseInvoice_PM_FormHistory. */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        StringBuilder sql = new StringBuilder("EXEC dbo.USP_PurchaseInvoice_PM_FormHistory @OrganizationId=?, @CompanyId=?, @CanViewAllRecord=?, @DocumentTypeId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), f.canViewAll, DOCUMENT_TYPE_ID));
        if (f.financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(f.financialYearId); }
        if (!f.canViewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        if (f.from != null) { sql.append(", @FromDate=?"); a.add(f.from); }
        if (f.to != null) { sql.append(", @ToDate=?"); a.add(f.to); }
        if (f.fromDocNo != 0) { sql.append(", @FromDocNo=?"); a.add(f.fromDocNo); }
        if (f.toDocNo != 0) { sql.append(", @ToDocNo=?"); a.add(f.toDocNo); }
        if (f.supplierId != 0) { sql.append(", @SupplierCustomerId=?"); a.add(f.supplierId); }
        if (f.branchIds != null && !f.branchIds.isEmpty()) { sql.append(", @BranchesIds=?"); a.add(f.branchIds); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** InvPurchaseInvoice.GetByID header, refused unless it is this company's 702 invoice. */
    public Map<String, Object> header(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?, @Activity=?", id, "ReadById");
        if (rows.isEmpty()) throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Record Not Found");
        Map<String, Object> h = rows.get(0);
        if (intOf(col(h, "OrganizationId")) != u.getOrganizationId() || intOf(col(h, "CompanyId")) != u.getCompanyId()
                || intOf(col(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Record Not Found");
        }
        return h;
    }

    /** DAL GetDate :620 - 702 reads PurchaseDetailPackingMaterialReadByHeaderId. */
    public List<Map<String, Object>> details(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?, @Activity=?", id, "PurchaseDetailPackingMaterialReadByHeaderId");
    }

    public List<Map<String, Object>> freight(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID");
    }

    public List<Map<String, Object>> journal(int id) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?, @Activity=?", id, "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID");
    }

    public List<Map<String, Object>> attachments(UserAccount u, int id) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?", SCREEN, id, "ReadById")
                .stream()
                .filter(r -> intOf(col(r, "OrganizationId")) == u.getOrganizationId() && intOf(col(r, "CompanyId")) == u.getCompanyId())
                .toList();
    }

    /** CommonServices.VoucherHeadIdGet(id, 702). */
    public int voucherHeadId(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId", u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id);
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "Id"));
    }

    /** btnDelete_Click :2579 -> InvPurchaseInvoice.RemoveByID. */
    public void delete(UserAccount u, int id) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?, @CompanyId=?, @Id=?, @DocumentTypeId=?, @UserId=?",
                u.getOrganizationId(), u.getCompanyId(), id, DOCUMENT_TYPE_ID, u.getId());
    }
}
