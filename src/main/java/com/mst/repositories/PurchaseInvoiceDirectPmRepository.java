package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.ProcExec;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 495 "Purchase Invoice Direct PM" - {@code frmPurchaseInvoiceDirectPM.cs}, DocumentTypeId 245.
 * Reads only; each is the desktop's own BLL call with its parameters sent when the BLL sends them.
 * Writes go through {@link PurchaseInvoiceWriteRepository} (the DAL 0434 contracts).
 */
@Repository
public class PurchaseInvoiceDirectPmRepository {

    public static final int DOCUMENT_TYPE_ID = 245;
    public static final String SCREEN = "frmPurchaseInvoiceDirectPM";

    private final JdbcTemplate jdbc;

    public PurchaseInvoiceDirectPmRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** DocumentNo :695 -> CommonServices.PurchaseInvoiceGenerateCode(245). */
    public int nextDocNo(UserAccount u, int fy) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy, "GenerateCode");
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "DocNo"));
    }

    /** BranchSrNoFill :754 -> InvPurchaseInvoice.GenerateBranchCode. */
    public int nextBranchSrNo(UserAccount u, int fy) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @BranchesId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, fy, u.getBranchesId(), "GenerateBranchSrNo");
        if (rows.isEmpty()) return 0;
        Iterator<Object> it = rows.get(0).values().iterator();
        return it.hasNext() ? intOf(it.next()) : 0;
    }

    /** SupplierDtFillFromGlobal :711 - globalSupplierCustomer, unfiltered. */
    public List<Map<String, Object>> suppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("CompanyName", col(r, "CompanyName"));
            m.put("PartyCode", col(r, "PartyCode"));
            m.put("GlAccountId", intOf(col(r, "GlAccountId")));
            m.put("CityId", intOf(col(r, "CityId")));
            m.put("CityName", col(r, "CityName"));
            m.put("MobileNo", col(r, "MobilePersonal"));
            out.add(m);
        }
        return out;
    }

    /**
     * AccountsFill :927 - dtAccountlst (Id = GL account, SupplierCustomerId = the combo value, AccountTitle).
     * Feature 4 on: GetVendorsAndCustomersForTransporter; off: CoaAllocationAccountTitleByAccountTypeIds
     * ("3,4,6,11,12,13,14,19,20,21") with the account id in both columns.
     */
    public List<Map<String, Object>> accounts(UserAccount u, boolean subsidiary, GrnPmRepository grn) {
        if (subsidiary) return grn.transporters(u, true);
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, @AccountTypeIds=?, @UserId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), u.getAppId() == null ? 0 : u.getAppId(), "3,4,6,11,12,13,14,19,20,21", u.getId(),
                "GetAccountTitleByAccountTypeIds")) {
            Map<String, Object> m = new LinkedHashMap<>();
            int id = intOf(col(r, "Id"));
            m.put("Id", id);
            m.put("AccountTitle", col(r, "AccountTitle"));
            m.put("AccountCode", col(r, "AccountCode"));
            m.put("SupplierCustomerId", id);
            out.add(m);
        }
        return out;
    }

    /** Branchlot :809 - BranchImplemented is never set, so BranchesId is 0 and @BranchId is not sent. */
    public List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC [dbo].[USP_GetJobLotsAllocatedToBranch] @OrganizationId=?, @CompanyId=?",
                u.getOrganizationId(), u.getCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("JobLotDescription", col(r, "JobLotDescription"));
            m.put("BranchId", intOf(col(r, "BranchId")));
            m.put("BranchName", col(r, "BranchName"));
            out.add(m);
        }
        return out;
    }

    /** racksWithWarehouseAndItems - the global list for the user's branch, with the warehouse's branch. */
    public List<Map<String, Object>> racks(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.usp_getRackswithWarehouseByItemId @OrganizationId=?, @CompanyId=?, @BranchId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getBranchesId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("RackName", col(r, "RackName"));
            m.put("WarehouseId", intOf(col(r, "invWarehouseId")));
            m.put("WareHouseName", col(r, "WareHouseName"));
            m.put("ItemId", intOf(col(r, "ItemId")));
            m.put("BranchId", intOf(col(r, "BranchId")));
            m.put("BranchName", col(r, "BranchName"));
            out.add(m);
        }
        return out;
    }

    /** CurrencyFill :1651 - MultiCurrency.GetAll, CurrencyCode shown. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : jdbc.queryForList("EXEC dbo.Sp_MultiCurrency_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadAll")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(r, "Id")));
            m.put("CurrencyCode", col(r, "CurrencyCode"));
            out.add(m);
        }
        return out;
    }

    /** cmbCurrency_Leave :1505 - VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher for 245. */
    public Double lastExchangeRate(UserAccount u, int currencyId) {
        var rows = jdbc.queryForList(
                "EXEC dbo.Sp_Vouchers_GetMethods @OrganizationId=?, @CompanyId=?, @DocumentTypeIds=?, @DMultiCurrencyIds=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), String.valueOf(DOCUMENT_TYPE_ID), String.valueOf(currencyId),
                "GetMultiCurrencyAndLastRate");
        return rows.isEmpty() ? null : dbl(col(rows.get(0), "LastExchRate"));
    }

    /** CmbRefDocumentType_Leave :1280 - PurchaseOrder.GetContractScheduleAndInvoiceDataForPM (DocumentTypeId 245). */
    public List<Map<String, Object>> refDocs(UserAccount u, int refDocumentTypeId, int recId) {
        if (refDocumentTypeId <= 0) return List.of();
        StringBuilder sql = new StringBuilder("EXEC [dbo].[USP_GetContractScheduleAndInvoiceDataForPM] "
                + "@OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @RefDocumentTypeId=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, refDocumentTypeId));
        if (recId != 0) { sql.append(", @RecId=?"); a.add(recId); }
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** DocDate_Leave :3770 - CommonServices.GetTaxScheduleDetailbyItemIds. */
    public List<Map<String, Object>> taxByItems(UserAccount u, String itemIds, java.sql.Date effected) {
        return jdbc.queryForList("EXEC dbo.Sp_ItemTaxSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemIds=?, @EffectedDate=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), itemIds, effected, "GetTaxScheduleDetailbyItemIds");
    }

    /** BalanceStock :1202 - GetAvgRateQtyAndStockInHand; zero-valued ids are not sent. */
    public double stockInHand(UserAccount u, int itemId, java.sql.Date docDate, int conditionId, int recId, int warehouseId, int rackId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_GetAvgRatesAndStockInHand_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @DocDate=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), itemId, docDate));
        if (recId > 0) { sql.append(", @DocumentTypeId=?"); a.add(DOCUMENT_TYPE_ID); }
        if (recId != 0) { sql.append(", @RecId=?"); a.add(recId); }
        if (conditionId != 0) { sql.append(", @ItemConditionId=?"); a.add(conditionId); }
        if (warehouseId != 0) { sql.append(", @WarehouseId=?"); a.add(warehouseId); }
        if (rackId != 0) { sql.append(", @RackId=?"); a.add(rackId); }
        sql.append(", @Activity=?"); a.add("GetAvgRateQtyAndStockInHand");
        var rows = jdbc.queryForList(sql.toString(), a.toArray());
        return rows.isEmpty() ? 0d : dbl(col(rows.get(0), "QtyInHand"));
    }

    // ================================================================== history / record

    public List<Map<String, Object>> historyBranches(UserAccount u) {
        return jdbc.queryForList("EXEC [dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice] @OrganizationId=?, @CompanyId=?, @UserId=?, @DocumentTypeId=?",
                u.getOrganizationId(), u.getCompanyId(), u.getId(), DOCUMENT_TYPE_ID);
    }

    /** HistoryComboFill :3182 - rows with Activity "Supplier". */
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

    public static final class HistoryFilter {
        public boolean canViewAll;
        public int financialYearId;
        public String dateType = "doc";
        public java.sql.Date from, to;
        public int fromDocNo, toDocNo, supplierId;
        public String branchIds;
    }

    /** GetAll :3240 -> InvPurchaseInvoice.FormHistory (Sp_InvPurchaseInvoice_GetAllMethod 'FormHistory'). */
    public List<Map<String, Object>> history(UserAccount u, HistoryFilter f) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @CanViewAllRecord=?");
        List<Object> a = new ArrayList<>(List.of(u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, f.canViewAll));
        if (f.financialYearId != 0) { sql.append(", @FinancialYearId=?"); a.add(f.financialYearId); }
        if (!f.canViewAll) { sql.append(", @EntryUser=?"); a.add(u.getId()); }
        String fromName, toName;
        switch (f.dateType) {
            case "entry" -> { fromName = "@EntryFromDate"; toName = "@EntryToDate"; }
            case "modify" -> { fromName = "@ModifyFromDate"; toName = "@ModifyToDate"; }
            case "approved" -> { fromName = "@ApprovedFromDate"; toName = "@ApprovedToDate"; }
            default -> { fromName = "@FromDate"; toName = "@ToDate"; }
        }
        if (f.from != null) { sql.append(", ").append(fromName).append("=?"); a.add(f.from); }
        if (f.to != null) { sql.append(", ").append(toName).append("=?"); a.add(f.to); }
        if (f.fromDocNo != 0) { sql.append(", @FromDocNo=?"); a.add(f.fromDocNo); }
        if (f.toDocNo != 0) { sql.append(", @ToDocNo=?"); a.add(f.toDocNo); }
        if (f.supplierId != 0) { sql.append(", @SupplierCustomerId=?"); a.add(f.supplierId); }
        if (f.branchIds != null && !f.branchIds.isEmpty()) { sql.append(", @BranchesIds=?"); a.add(f.branchIds); }
        sql.append(", @Activity=?"); a.add("FormHistory");
        return jdbc.queryForList(sql.toString(), a.toArray());
    }

    /** InvPurchaseInvoice.GetByID header, refused unless it is this company's 245 invoice. */
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

    /** DAL GetDate :632 - 61 / 64 / 245 read USP_InvPurchaseInvoiceDetail_ReadById. */
    public List<Map<String, Object>> details(int id) {
        return jdbc.queryForList("EXEC [dbo].[USP_InvPurchaseInvoiceDetail_ReadById] @Id=?", id);
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

    /** CommonServices.VoucherHeadIdGet(id, 245). */
    public int voucherHeadId(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity=?, @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @DocumentTypeSrNo=?",
                "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId", u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id);
        return rows.isEmpty() ? 0 : intOf(col(rows.get(0), "Id"));
    }

    /** DeleteDetailrow :2325 -> InvPurchaseInvoice.StockInReferenceValidationReferredOrNot. */
    public void stockInReferenceValidation(UserAccount u, int id, int detailId) {
        ProcExec.run(jdbc, "EXEC [dbo].[usp_StockInReferenceValidationReferredOrNot] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @DetailId=?",
                u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id, detailId);
    }

    /** btnDelete_Click :2714 -> InvPurchaseInvoice.RemoveByID. */
    public void delete(UserAccount u, int id) {
        ProcExec.run(jdbc, "EXEC dbo.Sp_InvoicesVouchersandStocksDelete @OrganizationId=?, @CompanyId=?, @Id=?, @DocumentTypeId=?, @UserId=?",
                u.getOrganizationId(), u.getCompanyId(), id, DOCUMENT_TYPE_ID, u.getId());
    }
}
