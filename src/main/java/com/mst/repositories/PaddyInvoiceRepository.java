package com.mst.repositories;

import java.util.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Reads of the Paddy Purchase invoice screens 466 frmPurchaseInvoiceForGate / 467 frmPurchaseInvoiceForGhallaMandi.
 * Every method is the same stored procedure + parameters as the desktop BLL/CommonServices method named in its comment.
 */
@Repository
public class PaddyInvoiceRepository {
    @Autowired private JdbcTemplate jdbc;

    /** CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll: Sp_COAAllocation_GetAllMethod 'COAAllocationSearch' (@UserId only when non-zero). */
    public List<Map<String, Object>> coaAllocation(int org, int company, int userId) {
        if (userId != 0) return jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@UserId=?,@Activity='COAAllocationSearch'", org, company, userId);
        return jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='COAAllocationSearch'", org, company);
    }

    /** InventoryItemsOther.GetAll: Sp_InventoryItemsOther_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> otherItems(int org, int company) {
        return jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll',@organizationId=?,@CompanyId=?", org, company);
    }

    /** CommonServices.PurchaseInvoiceGenerateCode -> InvPurchaseInvoice.GenerateInvPurchaseInvoiceCode: 'GenerateCode', DocNo of the first row. */
    public int generateCode(int org, int company, int documentTypeId, int year) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@Activity='GenerateCode'", org, company, documentTypeId, year);
        if (rows.isEmpty()) return 0;
        Object v = rows.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /** CommonServices.VoucherHeadIdGet -> VoucherHead.GetVoucherHeadIdByDocumentTypeIdandRefDocNoId. */
    public int voucherHeadId(int org, int company, int documentTypeId, int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_Vouchers_GetMethods @Activity='GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocumentTypeSrNo=?", org, company, documentTypeId, id);
        if (rows.isEmpty()) return 0;
        Object v = rows.get(0).get("Id");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /** InvPurchaseInvoice.PendingGrnforLoadingInvoice: usp_getGrnLoaderDataForPurchaseInvoice (optional parameters only when non-zero / non-empty). */
    public List<Map<String, Object>> pendingGrns(int org, int company, int documentTypeId, int supplierId, int orderId, int year, java.sql.Date from, java.sql.Date to) {
        StringBuilder sql = new StringBuilder("EXEC dbo.usp_getGrnLoaderDataForPurchaseInvoice @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?");
        List<Object> args = new ArrayList<>(List.of(org, company, documentTypeId));
        if (supplierId != 0) { sql.append(",@SupplierCustomerId=?"); args.add(supplierId); }
        if (orderId != 0) { sql.append(",@OrderId=?"); args.add(orderId); }
        if (year != 0) { sql.append(",@FinancialYearId=?"); args.add(year); }
        if (from != null) { sql.append(",@FromDate=?"); args.add(from); }
        if (to != null) { sql.append(",@ToDate=?"); args.add(to); }
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** PurchaseOrder.GetOrdersBySupplierId: Sp_PurchaseOrder_GetAllMethod 'GetOrdersBySupplierId' (DocumentTypeId 41, Status Open, IsApproved true). */
    public List<Map<String, Object>> openApprovedOrders(int org, int company, int supplierId, int year) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=41,@OrderSupCustId=?,@OrderStatus='Open',@IsApproved=1");
        List<Object> args = new ArrayList<>(List.of(org, company, supplierId));
        sql.append(",@Activity='GetOrdersBySupplierId'");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** InvPurchaseInvoice.GrnLoadForPurchaseInvoice(ids): usp_GrnLoadForPurchaseInvoice @GrnIds. */
    public List<Map<String, Object>> grnLoad(String grnIds) {
        return jdbc.queryForList("EXEC dbo.usp_GrnLoadForPurchaseInvoice @GrnIds=?", grnIds);
    }

    /** InvPurchaseInvoice.GrnLoadForPaddyPurchaseByOrder_GrnId(grnId, orderId). */
    public List<Map<String, Object>> grnLoadByOrder(int grnId, int orderId) {
        return jdbc.queryForList("EXEC dbo.USP_GrnLoadForPaddyPurchaseByOrder_GrnId @GrnId=?,@OrderId=?", grnId, orderId);
    }

    /** CommonServices.GetGhallaMandi -> Ghalla_Mandi.GetAll: USp_Ghalla_Mandi_GetAllMethod 'GetAll'. */
    public List<Map<String, Object>> ghallaMandi(int org, int company) {
        return jdbc.queryForList("EXEC dbo.USp_Ghalla_Mandi_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='GetAll'", org, company);
    }

    /** InvPurchaseInvoice.GetPendingGhallaMandiGrnForInvoice: Sp_InvPurchaseInvoice_GetAllMethod (optional parameters only when non-zero / non-null). */
    public List<Map<String, Object>> ghallaPending(int org, int company, int documentTypeId, int year, int supplierId, int ghallaMandiId, java.sql.Date from, java.sql.Date to) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?");
        List<Object> args = new ArrayList<>(List.of(org, company, documentTypeId, year));
        if (supplierId != 0) { sql.append(",@SupplierCustomerId=?"); args.add(supplierId); }
        if (ghallaMandiId != 0) { sql.append(",@GhallaMandiId=?"); args.add(ghallaMandiId); }
        if (from != null) { sql.append(",@FromDate=?"); args.add(from); }
        if (to != null) { sql.append(",@ToDate=?"); args.add(to); }
        sql.append(",@Activity='GetPendingGhallaMandiGrnForInvoice'");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** InvPurchaseInvoice.GrnGhallaMandiLoadForInvoice(ids): Sp_InvPurchaseInvoice_GetAllMethod @GdnIds, 'GrnGhallaMandiLoadForInvoice'. */
    public List<Map<String, Object>> grnGhallaLoad(String ids) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@Activity='GrnGhallaMandiLoadForInvoice'", ids);
    }

    /** InvPurchaseInvoice.GetTransporterAndFreightFromGrn(ids). */
    public List<Map<String, Object>> transporterAndFreight(String ids) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@Activity='GetTransporterAndFreightFromGrn'", ids);
    }

    /** InvPurchaseInvoice.GetEmptyBagsFromGrn(ids, orderId). */
    public List<Map<String, Object>> emptyBagsFromGrn(String ids, int orderId) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@OrderId=?,@Activity='GetEmptyBagsFromGrn'", ids, orderId);
    }

    /** InvPurchaseInvoice.GetItemQtyAndItemRateFromPO(ids). */
    public List<Map<String, Object>> itemQtyAndRateFromPo(String ids) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @GdnIds=?,@Activity='GetItemQtyAndItemRateFromPO'", ids);
    }

    /** CommonServices.PurchaseInvoiceHistory -> InvPurchaseInvoice.FormHistory: 'FormHistory' with @NoOfRecords only when non-zero, @EntryUser only when the user cannot view all. */
    public List<Map<String, Object>> history(int org, int company, int documentTypeId, boolean canViewAll, int year, int noOfRecords, int userId) {
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@CanViewAllRecord=?");
        List<Object> args = new ArrayList<>(List.of(org, company, documentTypeId, canViewAll));
        if (year != 0) { sql.append(",@FinancialYearId=?"); args.add(year); }
        if (noOfRecords != 0) { sql.append(",@NoOfRecords=?"); args.add(noOfRecords); }
        if (!canViewAll) { sql.append(",@EntryUser=?"); args.add(userId); }
        sql.append(",@Activity='FormHistory' WITH RECOMPILE");
        return jdbc.queryForList(sql.toString(), args.toArray());
    }

    /** DAL 0434 GetDate for InvPurchaseInvoice.GetByID: header 'ReadById' and the detail/child collections by @Id. */
    public Map<String, Object> header(int id) {
        var rows = jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity='ReadById' WITH RECOMPILE", id);
        return rows.isEmpty() ? null : rows.get(0);
    }
    public List<Map<String, Object>> collection(int id, String activity) {
        return jdbc.queryForList("EXEC dbo.Sp_InvPurchaseInvoice_GetAllMethod @Id=?,@Activity=? WITH RECOMPILE", id, activity);
    }

    /** DMSAttachments.GetByID(Id, ScreenName). */
    public List<Map<String, Object>> attachmentsByScreen(int id, String screenName) {
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?,@Id=?,@Activity='ReadById'", screenName, id);
    }
}
