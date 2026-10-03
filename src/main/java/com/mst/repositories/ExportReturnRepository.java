package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of 190 ExportReturn_Grn "Export Return GRN" (DocumentTypeId 241) and 191 ExportReturnInvoice
 * "Export Return Invoice" (DocumentTypeId 242), with the popups frmLoadExportReurnInvoiceForGrn and
 * frmLoadCommercialInvoiceForReturn. One method per BLL call; every procedure and parameter was read from
 * procdure.utf8.sql / procdure_index.csv. A guarded BLL parameter (`if (x != 0)`) is only added when set;
 * a null is never bound (DesktopProc leaves it out, as ADO.NET AddWithValue(null) does).
 *
 *  Globals (clsGlobalVariables, DatatableHelper.GlobalServicesDbCall)
 *   USP_GetVendorsAndCustomersWithCityName @OrganizationId @CompanyId                 globalAllSupplierCustomer
 *   Sp_MultiCurrency_GetAllMethod @OrganizationId @CompanyId @Activity='ReadAll'        globalMultiCurrency
 *   [dbo].[USP_GETAllAccountsFromCustomGroups] @OrganizationId @CompanyId              AllAccountsWithCustomGroupId
 *   USP_GetWarehousesAllocatedToBranch @OrganizationId @CompanyId @BranchId           globalWarehousesWithBranches
 *   Sp_InvCropYear_GetAllMethod 'ReadAll', [dbo].[SP_JobLot_ReadMethod] 'GetJobLotGlIdsandName',
 *   [dbo].[Sp_InvPackingType_GetAllMethod] @Activity='ReadAll', usp_getAllUomsByCompanyId @OrganizationId @CompanyId
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   configuration values
 *  191
 *   [dbo].[USP_ExportReturnInvoice_GetAllMethod] 'GenerateCode' | 'ReadById' | 'ReadByHeaderId_ExportReturnInvoiceDetail'
 *     | 'ReadByHeaderId_PaymentDetailByHeaderId' | 'FormHistory' | 'DeleteById'
 *   [dbo].[USP_GetDataForDropDownFromExportReturnInvoice] @OrganizationId @CompanyId
 *   [dbo].[USP_PendingGrnDataForExportReturnInvoice] @OrganizationId @CompanyId @BranchesId @FinancialYearId @RecId
 *   Sp_ExImInvoice_GetAllMethod @Id @Activity='ReadExImInvoicePaymentTermsDetailByHeaderId'
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice] @OrganizationId @CompanyId     (invoice loader combos)
 *   [dbo].[usp_ExImInvoice_GetDataForReturn] (invoice loader rows)
 *   Save (DAL ExportReturnInvoice.SetData, one transaction): [dbo].[USP_ExportReturnInvoice_InsertAndUpdate],
 *     [dbo].[USP_ExportReturnInvoiceDetail_Insert], [dbo].[USP_ExportReturnInvoicePaymentDetail_Insert],
 *     Sp_InventoryStockEvalautionDetail_Update (last row has a GRN), Sp_Vouchers_GetMethods
 *     'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId', Sp_VoucherHead_Insert | _Update, Sp_VoucherDetail_Insert,
 *     USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert.
 *   Voucher look-ups (BLL MakeVoucher): Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport',
 *     Sp_Item_GetAllMethod 'GetItemGlIdsandItemName', usp_getCostRateFromExportVoucherByInvoice.
 *  190
 *   [dbo].[USP_ExportReturnGrn_GetAllMethod] 'GenerateCode' | 'ReadById' | 'ReadByHeaderId_ExportReturnGrnDetail'
 *     | 'ReadByHeaderId_ExportReturnGrnOtherItems' | 'FormHistory' | 'DeleteById'
 *   [dbo].[USP_GetDataForDropDownFromExportReturnGrn] @OrganizationId @CompanyId
 *   [dbo].[USP_PendingInwardGatePassForExportReturnGrn] @organizationId @CompanyId @BranchesId @FinancialYearId
 *     @DocumentTypeId [@Id] [@RecId]
 *   [dbo].[USP_GetPendingForwardingForExportReturnGrn] @OrganizationId @CompanyId @BranchesId @FinancialYearId [@RecId]
 *   Sp_ExImForwarding_GetAllMethod @Id 'ReadById' | 'ReadByForwardingHeaderId' | 'ReadOtherItemsByHeaderId'
 *   Sp_ExImInvoice_GetAllMethod @OrganizationId @CompanyId @Id @Activity='GetOtherItemByExImInvoiceId'
 *   [dbo].[USP_ExportForwardingDataByInvoiceId] @OrganizationId @CompanyId @BranchesId @FinancialYearId @ExImInvoiceId
 *   Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId'
 *   usp_ExportRetunrInvoice_GetDataForGrnReturn (return-invoice loader rows)
 *   Save (DAL ExportReturnGrn.SetData, one transaction): [dbo].[USP_ExportReturnGrn_InsertAndUpdate],
 *     [dbo].[USP_ExportReturnGrnDetail_Insert], [dbo].[USP_ExportReturnGrnOtherItems_Insert],
 *     Sp_InventoryTransactions_GetALLMethod, Sp_InventoryStockEvalautionDetail_Update (any row of a return invoice),
 *     the voucher procedures as above (no USP_VoucherBalanceCheck in this DAL).
 *  Attachment procedures are not called (attachments are not part of the web port).
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportReturnRepository {

    public static final String RI = "[dbo].[USP_ExportReturnInvoice_GetAllMethod]";
    public static final String GRN = "[dbo].[USP_ExportReturnGrn_GetAllMethod]";
    private static final String FWD = "Sp_ExImForwarding_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportReturnRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }
    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    public List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    private List<Map<String, Object>> rowsA(UserAccount u, String proc, String activity) { Map<String, Object> p = tenant(u); p.put("Activity", activity); return rows(proc, p); }

    // ================================================================== configuration / rights

    /** GlobalVariables_Helper.GetConfigValueFromGlobal / CommonServies.GetConfigurationFromAllocation -> ConfigKey ("" when none). */
    public String config(UserAccount u, String description) {
        Map<String, Object> p = tenant(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    /**
     * Rightsobjects.DoHaveCanViewAllRecordRights for a non-Admin role: the "CanView AllRecord" row of
     * tblUserRights for this screen (none configured means false, as the desktop leaves the flag at its default).
     */
    public boolean canViewAllRecordRight(UserAccount u, int screenId) {
        Integer n = jdbc.queryForObject(
                "SELECT COUNT(1) FROM dbo.tblUserRights ur INNER JOIN dbo.ScreenRights sr ON sr.Id = ur.RightId "
              + "WHERE sr.ScreenID = ? AND sr.RightName = 'CanView AllRecord' AND ur.UserId = ? AND ur.CompanyId = ? AND ur.Value = 1",
                Integer.class, screenId, u.getId(), u.getCompanyId());
        return n != null && n > 0;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period (the loaders' From Date). */
    public Object financialYearStart(int financialYearId) {
        if (financialYearId == 0) return null;
        List<Map<String, Object>> r = jdbc.queryForList("SELECT Start_Period FROM FinancialYear WHERE Id = ?", financialYearId);
        return r.isEmpty() ? null : ci(r.get(0), "Start_Period");
    }

    // ================================================================== globals

    public List<Map<String, Object>> globalSupplierCustomers(UserAccount u) { return rows("USP_GetVendorsAndCustomersWithCityName", tenant(u)); }
    public List<Map<String, Object>> currencies(UserAccount u) { return rowsA(u, "Sp_MultiCurrency_GetAllMethod", "ReadAll"); }
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) { return rows("[dbo].[USP_GETAllAccountsFromCustomGroups]", tenant(u)); }
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return rows("USP_GetWarehousesAllocatedToBranch", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", branch(u)));
    }
    public List<Map<String, Object>> cropYears(UserAccount u) { return rowsA(u, "[dbo].[Sp_InvCropYear_GetAllMethod]", "ReadAll"); }
    public List<Map<String, Object>> jobLotsGlobal(UserAccount u) { return rowsA(u, "[dbo].[SP_JobLot_ReadMethod]", "GetJobLotGlIdsandName"); }
    public List<Map<String, Object>> packingTypes() { return rows("[dbo].[Sp_InvPackingType_GetAllMethod]", params("Activity", "ReadAll")); }
    public List<Map<String, Object>> allUoms(UserAccount u) { return rows("usp_getAllUomsByCompanyId", tenant(u)); }
    public List<Map<String, Object>> seaPorts(UserAccount u) { return rowsA(u, "Sp_SeaPorts_GetAllMethod", "ReadByCompanyNOrganizationId"); }

    // ================================================================== voucher look-ups

    public List<Map<String, Object>> exportCustomers(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "ReadByOrganizationCompanyIdForExport"); }
    public List<Map<String, Object>> itemGl(UserAccount u) { return rowsA(u, "Sp_Item_GetAllMethod", "GetItemGlIdsandItemName"); }

    /** CommonServies.getCostRateFromExportVoucherByInvoice -> CostRate of the first row (0 when none). */
    public double costRate(UserAccount u, int itemId, int jobLotId, int invoiceId, int invoiceDetailId,
                           int cropYearId, int warehouseId, int itemUomId, int packingTypeId) {
        Map<String, Object> p = tenant(u);
        p.put("ItemId", itemId);
        p.put("JobLotId", jobLotId);
        p.put("InvoiceId", invoiceId);
        p.put("InvoiceDetailId", invoiceDetailId);
        if (cropYearId > 0) p.put("CropYearId", cropYearId);
        if (warehouseId > 0) p.put("WarehouseId", warehouseId);
        if (itemUomId > 0) p.put("ItemUomId", itemUomId);
        if (packingTypeId > 0) p.put("InvPackingTypeId", packingTypeId);
        List<Map<String, Object>> r = rows("usp_getCostRateFromExportVoucherByInvoice", p);
        return r.isEmpty() ? 0 : asDouble(ci(r.get(0), "CostRate"));
    }

    /** CommonServices.VoucherHeadIdGet / CommonServices.GetVoucherHeadId. */
    public int voucherHeadId(int org, int company, int documentTypeId, int id) {
        List<Map<String, Object>> r = rows("Sp_Vouchers_GetMethods", params("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org, "CompanyId", company, "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id));
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "Id"));
    }

    // ================================================================== 191 Export Return Invoice

    public int riGenerateCode(UserAccount u, int financialYearId, int documentTypeId) {
        Map<String, Object> p = tenant(u);
        p.put("BranchesId", branch(u));
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = rows(RI, p);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return asInt(ci(r.get(0), "DocNo"));
    }

    /** ExportReturnInvoice.GetDataForDropDownFromExportReturnInvoice(org, comp, null, null, null). */
    public List<Map<String, Object>> riDropDown(UserAccount u) { return rows("[dbo].[USP_GetDataForDropDownFromExportReturnInvoice]", tenant(u)); }

    /** ExportReturnGrn.PendingGrnDataForExportReturnInvoice - @RecId = ReportsParameters.Id, never set (0). */
    public List<Map<String, Object>> riPendingGrn(UserAccount u, int financialYearId) {
        return rows("[dbo].[USP_PendingGrnDataForExportReturnInvoice]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", financialYearId, "RecId", 0));
    }

    /** ExImInvoice.InvoicePaymentTermsDetailByInvoiceId. */
    public List<Map<String, Object>> invoicePaymentTerms(int exImInvoiceId) {
        return rows("Sp_ExImInvoice_GetAllMethod", params("Id", exImInvoiceId, "Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"));
    }

    public List<Map<String, Object>> riHeader(int id) { return rows(RI, params("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> riDetails(int id) { return rows(RI, params("Id", id, "Activity", "ReadByHeaderId_ExportReturnInvoiceDetail")); }
    public List<Map<String, Object>> riPayments(int id) { return rows(RI, params("Id", id, "Activity", "ReadByHeaderId_PaymentDetailByHeaderId")); }

    /** ExportReturnInvoice.FormHistory - the map already carries exactly the parameters the BLL adds. */
    public List<Map<String, Object>> riHistory(Map<String, Object> p) { return rows(RI, p); }

    /** ExportReturnInvoice.DeleteByID(org, comp, DocumentTypeId, EntryUserId, Id). */
    public void riDelete(UserAccount u, int documentTypeId, int id) {
        rows(RI, params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId,
                "EntryUserId", u.getId(), "Id", id, "Activity", "DeleteById"));
    }

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice (no activity / document types). */
    public List<Map<String, Object>> exportInvoiceDropDown(UserAccount u) { return rows("[dbo].[USP_GetDataForDropDownFromExportInvoice]", tenant(u)); }

    /** ExImInvoice.ExImInvoice_GetDataForReturn. */
    public List<Map<String, Object>> invoiceDataForReturn(Map<String, Object> p) { return rows("[dbo].[usp_ExImInvoice_GetDataForReturn]", p); }

    /** ExportReturnInvoice.ExportRetunrInvoice_GetDataForGrnReturn. */
    public List<Map<String, Object>> returnInvoiceDataForGrn(Map<String, Object> p) { return rows("usp_ExportRetunrInvoice_GetDataForGrnReturn", p); }

    /** DAL ExportReturnInvoice.SetData - one transaction, rolled back on any error. */
    @Transactional(rollbackFor = Exception.class)
    public int riSave(Map<String, Object> header, List<Map<String, Object>> details, List<Map<String, Object>> payments,
                      SaleInvoiceModels.VoucherHead vh, List<SaleInvoiceModels.VoucherDetail> vds) {
        int org = asInt(header.get("OrganizationId")), company = asInt(header.get("CompanyId")), documentTypeId = asInt(header.get("DocumentTypeId"));
        int num3 = ExportMProc.setMap(jdbc, "[dbo].[USP_ExportReturnInvoice_InsertAndUpdate]", header);
        int id;
        if (num3 > 0) id = num3; else { id = asInt(header.get("Id")); num3 = id; }
        header.put("Id", id);

        boolean flag2 = false;
        for (Map<String, Object> d : details) {
            flag2 = asInt(d.get("GrnId")) > 0;                 // reassigned per row: the LAST row decides (DAL quirk)
            d.put("ExportReturnInvoiceId", id);
            d.put("Id", ExportMProc.setMap(jdbc, "[dbo].[USP_ExportReturnInvoiceDetail_Insert]", d));
        }
        for (Map<String, Object> p : payments) {
            p.put("ExportReturnInvoiceId", id);
            p.put("Id", ExportMProc.setMap(jdbc, "[dbo].[USP_ExportReturnInvoicePaymentDetail_Insert]", p));
        }
        if (flag2) stockEvaluationUpdate(org, company, documentTypeId, id);

        int num2 = voucherHeadId(org, company, documentTypeId, id);
        if (num2 > 0) vh.Id = num2;
        vh.DocumentTypeSrNo = id;
        int num = ExportMProc.setProc(jdbc, num2 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.Id = num;
        if (vds.isEmpty()) throw new IllegalStateException("VoucherDetail list Not Found");
        for (SaleInvoiceModels.VoucherDetail item : vds) {
            for (Map<String, Object> d : details) {
                int line = asInt(d.get("LineId"));
                if (line == item.LineId && line > 0 && item.LineId > 0) { item.RefDocSubIdNo = asInt(d.get("Id")); break; }
            }
            item.VoucherHeadId = vh.Id;
            ExportMProc.setProc(jdbc, "Sp_VoucherDetail_Insert", item);
        }
        ProcExec.run(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
        int documentTypeIdRef = ExportMProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", vh);
        for (SaleInvoiceModels.VoucherDetail d : vds) {
            d.VoucherHeadId = vh.Id;
            d.DocumentTypeIdRef = documentTypeIdRef;
            ExportMProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", d);
        }
        return num3;
    }

    /** InventoryStockEvalautionDetail { OrganizationId, CompanyId, RefDocumentTypeId, RefDocIdNo } through SetProc (every property). */
    private void stockEvaluationUpdate(int org, int company, int documentTypeId, int id) {
        Map<String, Object> m = new java.util.LinkedHashMap<>();
        m.put("IsApproved", false);
        for (String k : new String[]{"AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate", "ExpenseAmountIn", "ItemRate", "QtyIn",
                "QtyOut", "StockWeightIn", "StockWeightOut", "CgsAmount"}) m.put(k, 0d);
        for (String k : new String[]{"BranchesId", "CompanyId", "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId", "ItemId", "ItemUom", "JobLotId",
                "OrderNo", "OrganizationId", "PrdJobOrderNo", "ProjectsId", "RateUom", "RefDocIdNo", "RefDocSubIdNo", "RefDocumentTypeId",
                "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId", "SupplierCustomerId", "WarehouseId", "RefWarehouseId", "CityId", "LineId",
                "RefRefDocumentTypeId", "RefRefDocIdNo", "RefRefDocSubIdNo", "EntryUser", "ModifyUser", "InvoiceId", "InvoiceDetailId", "VarientId",
                "ItemConditionId"}) m.put(k, 0);
        m.put("OrganizationId", org);
        m.put("CompanyId", company);
        m.put("RefDocumentTypeId", documentTypeId);
        m.put("RefDocIdNo", id);
        ExportMProc.setMap(jdbc, "Sp_InventoryStockEvalautionDetail_Update", m);
    }

    // ================================================================== 190 Export Return GRN

    public int grnGenerateCode(UserAccount u, int financialYearId, int documentTypeId) {
        Map<String, Object> p = tenant(u);
        p.put("BranchesId", branch(u));
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = rows(GRN, p);
        if (r.isEmpty()) throw new IllegalStateException("There is no row at position 0.");
        return asInt(ci(r.get(0), "DocNo"));
    }

    public List<Map<String, Object>> grnDropDown(UserAccount u) { return rows("[dbo].[USP_GetDataForDropDownFromExportReturnGrn]", tenant(u)); }

    /** GatePassInward.PendingInwardGatePassForExportReturnGrn(.., DocumentTypeId 51, GpId, RecId). */
    public List<Map<String, Object>> pendingInwardGatePasses(UserAccount u, int financialYearId, int gpId, int recId) {
        Map<String, Object> p = params("organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branch(u),
                "FinancialYearId", financialYearId, "DocumentTypeId", 51);
        if (gpId != 0) p.put("Id", gpId);
        if (recId != 0) p.put("RecId", recId);
        return rows("[dbo].[USP_PendingInwardGatePassForExportReturnGrn]", p);
    }

    /** ExImInvoice.GetPendingForwardingForExportReturnGrn - @RecId only when set. */
    public List<Map<String, Object>> pendingForwardings(UserAccount u, int financialYearId, int recId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branch(u),
                "FinancialYearId", financialYearId);
        if (recId != 0) p.put("RecId", recId);
        return rows("[dbo].[USP_GetPendingForwardingForExportReturnGrn]", p);
    }

    public List<Map<String, Object>> forwardingHeader(int id) { return rows(FWD, params("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> forwardingDetail(int id) { return rows(FWD, params("Id", id, "Activity", "ReadByForwardingHeaderId")); }
    public List<Map<String, Object>> forwardingOtherItems(int id) { return rows(FWD, params("Id", id, "Activity", "ReadOtherItemsByHeaderId")); }

    /** ExImInvoice.GetOtherItemByExImInvoiceId. */
    public List<Map<String, Object>> otherItemsByInvoice(UserAccount u, int id) {
        Map<String, Object> p = tenant(u);
        p.put("Id", id);
        p.put("Activity", "GetOtherItemByExImInvoiceId");
        return rows("Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImForwarding.ExportForwardingDataByInvoiceId. */
    public List<Map<String, Object>> forwardingDataByInvoice(UserAccount u, int financialYearId, int exImInvoiceId) {
        return rows("[dbo].[USP_ExportForwardingDataByInvoiceId]", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", financialYearId, "ExImInvoiceId", exImInvoiceId));
    }

    public List<Map<String, Object>> grnHeader(int id) { return rows(GRN, params("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> grnDetails(int id) { return rows(GRN, params("Id", id, "Activity", "ReadByHeaderId_ExportReturnGrnDetail")); }
    public List<Map<String, Object>> grnOtherItems(int id) { return rows(GRN, params("Id", id, "Activity", "ReadByHeaderId_ExportReturnGrnOtherItems")); }
    public List<Map<String, Object>> grnHistory(Map<String, Object> p) { return rows(GRN, p); }

    public void grnDelete(UserAccount u, int documentTypeId, int id) {
        rows(GRN, params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId,
                "EntryUserId", u.getId(), "Id", id, "Activity", "DeleteById"));
    }

    /** DAL ExportReturnGrn.SetData - one transaction. vh is null when no row refers to a return invoice. */
    @Transactional(rollbackFor = Exception.class)
    public int grnSave(Map<String, Object> header, List<Map<String, Object>> details, List<Map<String, Object>> otherItems,
                       SaleInvoiceModels.VoucherHead vh, List<SaleInvoiceModels.VoucherDetail> vds) {
        int org = asInt(header.get("OrganizationId")), company = asInt(header.get("CompanyId")), documentTypeId = asInt(header.get("DocumentTypeId"));
        int num3 = ExportMProc.setMap(jdbc, "[dbo].[USP_ExportReturnGrn_InsertAndUpdate]", header);
        int id;
        if (num3 > 0) id = num3; else { id = asInt(header.get("Id")); num3 = id; }
        header.put("Id", id);

        boolean flag2 = false;
        for (Map<String, Object> d : details) if (asInt(d.get("ReturnInvoiceId")) > 0) { flag2 = true; break; }
        for (Map<String, Object> d : details) {
            d.put("LineId", asInt(d.get("LineId")) + 1);        // exportReturnGrnDetail2.LineId += num4 (num4 is always 1)
            d.put("ExportReturnGrnId", id);
            d.put("Id", ExportMProc.setMap(jdbc, "[dbo].[USP_ExportReturnGrnDetail_Insert]", d));
        }
        for (Map<String, Object> o : otherItems) {
            o.put("ExportReturnGrnId", id);
            ExportMProc.setMap(jdbc, "[dbo].[USP_ExportReturnGrnOtherItems_Insert]", o);
        }

        /* InventoryTransactions { OrganizationId, CompanyId, RefDocumentTypeId, RefDocIdNo } through SetProc. */
        Map<String, Object> it = new java.util.LinkedHashMap<>();
        it.put("IsApproved", false);
        for (String k : new String[]{"AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "ExpenseAmountIn", "ItemRate", "QtyIn", "QtyOut",
                "StockWeightIn", "StockWeightOut"}) it.put(k, 0d);
        for (String k : new String[]{"BranchesId", "CompanyId", "DocCodeNo", "InvPackingTypeId", "ItemId", "ItemUom", "JobLotId", "OrganizationId",
                "ProjectsId", "RateUom", "RefDocIdNo", "RefDocSubIdNo", "RefDocumentTypeId", "SupplierCustomerId", "WarehouseId"}) it.put(k, 0);
        it.put("Id", 0L);
        it.put("OrganizationId", org);
        it.put("CompanyId", company);
        it.put("RefDocumentTypeId", documentTypeId);
        it.put("RefDocIdNo", num3);
        ExportMProc.setMap(jdbc, "Sp_InventoryTransactions_GetALLMethod", it);

        if (flag2) stockEvaluationUpdate(org, company, documentTypeId, id);

        if (flag2 && vh != null) {
            int num2 = 0;
            if (asInt(header.get("ModifyUserId")) > 0) {                  // the look-up only runs on an update (DAL quirk)
                num2 = voucherHeadId(org, company, documentTypeId, id);
                if (num2 > 0) vh.Id = num2;
            }
            vh.DocumentTypeSrNo = id;
            int num = ExportMProc.setProc(jdbc, num2 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
            if (num > 0) vh.Id = num;
            for (SaleInvoiceModels.VoucherDetail item : vds) {
                for (Map<String, Object> d : details) {
                    int line = asInt(d.get("LineId"));
                    if (line == item.LineId && line > 0 && item.LineId > 0) { item.RefDocSubIdNo = asInt(d.get("Id")); break; }
                }
                item.VoucherHeadId = vh.Id;
                ExportMProc.setProc(jdbc, "Sp_VoucherDetail_Insert", item);
            }
            int documentTypeIdRef = ExportMProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", vh);
            for (SaleInvoiceModels.VoucherDetail d : vds) {
                d.VoucherHeadId = vh.Id;
                d.DocumentTypeIdRef = documentTypeIdRef;
                ExportMProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", d);
            }
        }
        return num3;
    }

    // ================================================================== plumbing

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    private static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
}
