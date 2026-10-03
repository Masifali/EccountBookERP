package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.copy;
import static com.mst.services.PurchaseInvoiceFinancialRules.i;

/**
 * Read side of Architecture.WinApp.Purchase.PurchaseInvoiceAgainstGrnOrder (ScreenDefinition 132
 * "PurchaseInvoiceAgainstGrnOrder", DocumentTypeId 172; its GRN loader is frmLoadGRN with DocumentTypeId 169 =
 * GRNDirectAgainstOrder). Every query is the one the desktop form reaches through its BLL; tenancy always comes from
 * the session. No catch-and-return-empty: a failing procedure surfaces as an error.
 */
@Repository
public class PurchaseInvoiceAgainstGrnOrderRepository {

    public static final int TYPE = 172;
    /** toolStripButton3_Click_1 :3114 LoadGRN.DocumentTypeId = 169; GetGrnIdAndInvoiceTypeId :3440 DocumentTypeId = 169. */
    public static final int GRN_TYPE = 169;
    /** base.Name (Designer :719) - rights (:350), header ScreenName (:2709) and DMS attachments (:2552, :2916). */
    public static final String SCREEN_NAME = "PurchaseInvoiceAgainstGrnOrder";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public PurchaseInvoiceAgainstGrnOrderRepository(JdbcTemplate jdbc, CurrentUserContext context) {
        this.jdbc = jdbc;
        this.context = context;
    }

    private int org() { return context.currentOrganizationId(); }
    private int company() { return context.currentCompanyId(); }

    /** CommonServices.GetERPFeatureById(4) (Load :349) - SubsidiaryAccountAllownOnVouchers. */
    public boolean subsidiaryFeature() {
        return jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?", org(), company())
                .stream().anyMatch(r -> i(copy(r), "Id") == 4);
    }

    /** SupplierNameFilll :1516 - feature 4 ON: CommonServices.GetVendorsAndCustomers(1) -> USP_GetVendorsAndCustomers
     *  @PartyTypeId=1 (Id, CompanyName, PartyCode, GlAccountId; col 3 hidden). Feature OFF: SupplierCustomerGetAllServiceBind ->
     *  SupplierCustomer.Getall (BLL 0600:110) -> Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId' (the mapping the
     *  round-1 port of screen 131 uses; WinApp CommonServices is not in the workspace). The same table fills Supplier, Comm
     *  Agent and Broker Ac (:1525-1527 / :1541-1543). */
    public List<Map<String, Object>> suppliers(boolean subsidiary) {
        if (subsidiary)
            return jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomers @OrganizationId=?,@CompanyId=?,@PartyTypeId=?", org(), company(), 1);
        return jdbc.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationCompanyId'", org(), company());
    }

    /** AccountsFill :1556 - CoaAllocationGetAllServiceBind -> COAAllocation.GetAll (BLL 0648:154) -> Sp_COAAllocation_GetAllMethod
     *  'COAAllocationSearch' (@UserId only when non-zero); the form keeps rows whose AccountTypeId is not 2, 11, 12 or 15. */
    public List<Map<String, Object>> accounts() {
        int user = context.currentUserId();
        List<Map<String, Object>> rows = user != 0
                ? jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@UserId=?,@Activity='COAAllocationSearch'", org(), company(), user)
                : jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='COAAllocationSearch'", org(), company());
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : rows) {
            var r = copy(raw);
            int t = i(r, "AccountTypeId");
            if (t == 2 || t == 11 || t == 12 || t == 15) continue;
            var m = new LinkedHashMap<String, Object>();
            m.put("Id", i(r, "Id"));
            m.put("AccountTitle", r.get("AccountTitle"));
            out.add(m);
        }
        return out;
    }

    /** OtherItemsBind :1579 - InventoryItemsOther.GetAll -> Sp_InventoryItemsOther_GetAllMethod 'ReadAll' (Id / OtherItemName). */
    public List<Map<String, Object>> otherItems() {
        return jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll',@organizationId=?,@CompanyId=?", org(), company());
    }

    /** BranchFill :1482 - BrancheServiceBind -> Branches.GetAll (BLL 0058) -> Sp_Branches_GetAllMethod 'GetAll' (Id / BranchName). */
    public List<Map<String, Object>> branches() {
        return jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", org(), company());
    }

    /** ProjectFill :1499 - ProjectServiceBind -> Projects.GetAlldt (BLL 0078) -> Sp_Projects_GetAllMethod @MethodType='GetAll'. */
    public List<Map<String, Object>> projects() {
        return jdbc.queryForList("EXEC dbo.Sp_Projects_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'", org(), company());
    }

    /** DocumentNo :1641 - CommonServices.PurchaseInvoiceGenerateCode(172) -> Sp_InvPurchaseInvoice_GetAllMethod 'GenerateCode'. */
    public int nextDocNo() {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity='GenerateCode'",
                org(), company(), TYPE, context.currentFinancialYearId());
        return rows.isEmpty() ? 0 : i(copy(rows.get(0)), "DocNo");
    }

    /** VoucherHeadIdGet :1659 - CommonServices.VoucherHeadIdGet(Id, 172). */
    public int voucherHeadId(int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?", org(), company(), TYPE, id);
        return rows.isEmpty() ? 0 : i(copy(rows.get(0)), "Id");
    }

    /** ReadById :2460 - GetByID -> 'ReadById'; DAL 0434 GetDate reads type 172 details with 'PurchaseDetailReadByInvPurchaseInvoiceId'. */
    public Map<String, Object> header(int id) {
        return jdbc.queryForMap("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById' WITH RECOMPILE", id);
    }
    public List<Map<String, Object>> read(int id, String activity) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=? WITH RECOMPILE", id, activity);
    }

    /** LoadDataDetailGridAgainstGP :1901 - InvPurchaseInvoice.GrnLoadForPurchaseInvoice (BLL 0581:653) -> usp_GrnLoadForPurchaseInvoice. */
    public List<Map<String, Object>> grnRows(String grnIds) {
        return jdbc.queryForList("EXEC dbo.usp_GrnLoadForPurchaseInvoice @GrnIds=?", grnIds);
    }

    /** LoadFreightData :1944 - GetTransporterAndFreightFromGrn (BLL 0581:773). */
    public List<Map<String, Object>> grnFreight(String grnIds) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@Activity='GetTransporterAndFreightFromGrn'", grnIds);
    }

    /** LoadPurchaseOrderExpensesChargToProductData :1992 - GetPurchaseOrderExpensesChargToProdut(POID, null) (BLL 0581:870):
     *  @Id only when non-zero, @POIds never. Without either the procedure's branch (IF @Id &gt; 0 OR @POIds IS NOT NULL) selects
     *  nothing and returns no result set at all, so the call is not made for POID 0 (same outcome: no rows). */
    public List<Map<String, Object>> orderChargesToProduct(int purchaseOrderId) {
        if (purchaseOrderId == 0) return List.of();
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='GetPurchaseOrderExpensesChargToProdut'", purchaseOrderId);
    }

    /** LoadExpData :2047 - GetItemQtyAndItemRateFromPO (BLL 0581:904). */
    public List<Map<String, Object>> grnBagPrices(String grnIds) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@Activity='GetItemQtyAndItemRateFromPO'", grnIds);
    }

    /** LoadEmptyBagsData :2024 - GetEmptyBagsFromGrn(ids, 0) (BLL 0581:842). */
    public List<Map<String, Object>> grnEmptyBags(String grnIds) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@OrderId=?,@Activity='GetEmptyBagsFromGrn'", grnIds, 0);
    }

    /** GetGrnIdAndInvoiceTypeId :3424 - InvGrn.GetGRNIdByDocNo (BLL 0576:288): Org, Company, DocumentTypeId 169, DocNo,
     *  FinancialYearId when non-zero. The procedure leaves out GRNs already on a purchase invoice. */
    public List<Map<String, Object>> grnIdByDocNo(int docNo) {
        int year = context.currentFinancialYearId();
        if (year != 0)
            return jdbc.queryForList("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocNo=?,@FinancialYearId=?,@Activity='GetGRNIdByDocNo'", org(), company(), GRN_TYPE, docNo, year);
        return jdbc.queryForList("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocNo=?,@Activity='GetGRNIdByDocNo'", org(), company(), GRN_TYPE, docNo);
    }

    /** Tenancy guard before a browser-supplied GRN id reaches a loader procedure: the GRN's number, when it is a type-169 GRN
     *  of the session's organization and company (0 otherwise). */
    public int grnDocNo(int grnId) {
        var rows = jdbc.queryForList("SELECT DocNo FROM dbo.InvGrn WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=?", grnId, org(), company(), GRN_TYPE);
        return rows.isEmpty() ? 0 : i(copy(rows.get(0)), "DocNo");
    }

    /**
     * GetAll :2292 - CommonServices.PurchaseInvoiceHistory(172, CanViewAllRecord, NoOfRecords). The WinApp CommonServices source
     * is not in the workspace; this follows InvPurchaseInvoice.FormHistory (BLL 0581:369): @CanViewAllRecord always,
     * @FinancialYearId, @NoOfRecords when non-zero (tab select sends 50, LoadAll 0), @EntryUser only when the user may NOT view
     * all records. The form has no date / supplier / document-number filters.
     */
    public List<Map<String, Object>> history(boolean canViewAll, int noOfRecords) {
        var sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?,@FinancialYearId=?");
        var args = new ArrayList<Object>(List.of(org(), company(), TYPE, canViewAll, context.currentFinancialYearId()));
        if (noOfRecords != 0) { sql.append(",@NoOfRecords=?"); args.add(noOfRecords); }
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(context.currentUserId()); }
        sql.append(",@Activity='FormHistory' WITH RECOMPILE");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }
}
