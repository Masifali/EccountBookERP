package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of 192 "Pre Invoice" (Architecture.WinApp.Export.PreCommiercialInvoice, DocumentTypeId 209) and
 * 194 "Export Opening Balance" (CommiercialInvoiceForOpeningBalance, DocumentTypeId 212) - the two forms are
 * the same desktop code with a different document type. It also carries the lookup calls the other Export
 * invoice forms of this port (193 Packing Detail, 215 Proforma Invoice) share, each one the desktop's own
 * procedure with the desktop's own parameters (verified in procdure_index.csv / procdure.utf8.sql, 01-Oct-2026):
 *
 *   Sp_SupplierCustomer_GetAllMethod  @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyIdForExport'      SupplierCustomer.ReadByOrganizationCompanyIdForExport
 *                                     @OrganizationId @CompanyId [@CustomerGroupIds] @Activity='GetSupplierustomerByCustomerGroupId'
 *                                     @OrganizationId @CompanyId @Activity='GetGlAccountIdandCompanyNameBySupplierCustomerId' (voucher RefAccountId)
 *   Sp_ExImDeliveryTerm_GetAllMethod  @Activity='ReadAll'                                                               ExImDeliveryTerm.Getall
 *   Sp_ExImLcPaymentTerm_GetAllMethod @Activity='ReadAll' @OrganizationId @CompanyId                                   ExImLcPaymentTerm.Getall
 *   Sp_SeaPorts_GetAllMethod          @OrganizationId @CompanyId @Activity='ReadByCompanyNOrganizationId'              SeaPorts.Getall
 *   Sp_MultiCurrency_GetAllMethod     @OrganizationId @CompanyId @Activity='ReadAll'                                   MultiCurrency.GetAll
 *   Sp_Bank_GetAllMethod              @OrganizationId @CompanyId @Activity='ReadAll'                                   Bank.GetAll (IsHomeland split)
 *   Sp_ExImPackMaterilaType_GetAllMethod @Activity='ReadAll' @OrganizationId @CompanyId                                ExImPackMaterilaType.Getall
 *   Sp_InvCropYear_GetAllMethod       @OrganizationId @CompanyId @Activity='ReadAll'                                   CommonServices.CropYearGetAllService
 *   Sp_UOMSchedule_GetAllMethod       @OrganizationId @CompanyId @ItemId @Activity='ReadByItemID'                      CommonServices.GetUomScheduleByItemId
 *   Sp_Item_GetAllMethod              'ReadAllForComboTwoColumns' [@InventoryParentCategoriesId] / 'ReadAllForExportCombo' / @LookupTypeIds 'GetItemByItemTypeId'
 *   Sp_Branches_GetAllMethod          @OrganizationId @CompanyId @Activity='GetAll'                                    CommonServices.BrancheServiceBind
 *   Sp_Projects_GetAllMethod          @OrganizationId @CompanyId @MethodType='GetAll'                                  CommonServices.ProjectServiceBind
 *   Sp_COAAllocation_GetAllMethod     @OrganizationId @CompanyId @AppId [@AccountTypeIds] [@UserId] @Activity='GetAccountTitleByAccountTypeIds'
 *   Sp_InvWareHouse_GetAllMethod      'ReadByOrganizationCompanyId' / SP_JobLot_ReadMethod 'GetAll'                    WareHouseGetAllService / JobLotGetAllService
 *   USP_GetDataForDropDownFromExportInvoice  / USP_GetDataForDropDownFromExportContract  @OrganizationId @CompanyId   history Customer combos
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'; USP_GetERPFeaturesByCompanyId
 *
 *   ExImInvoice (BLL 0467 / DAL 0519):
 *   Sp_ExImInvoice_GetAllMethod  @OrganizationId @CompanyId @DocumentTypeId [@FinancialYearId] [@BranchesId] @Activity='GenerateDocNo'
 *                                @OrganizationId @CompanyId @DocumentTypeId [@FinancialYearId] @Activity='GenerateInvoiceNo'
 *                                @Id @Activity='ReadById' | 'ReadExImInvoicePackingDetailByHeaderId' | 'ReadExImInvoiceOtherItemsByHeaderId'
 *                                    | 'ReadExImInvoicePaymentTermsDetailByHeaderId'
 *                                @Ids @Activity='ReadExImInvoiceOtherItemsByProformaHeaderIds'      ExImLcOrder.ReadOtherItemsByProformaHeaderIds
 *   Sp_ExImLcOrder_GetAllMethod  @OrganizationId @CompanyId @Id(=0) @Activity='GetOtherItemByContractId'   ExImLcOrder.GetOtherItemByContractId
 *   USP_GetProformaDataForPreInvoices / USP_GetProformaDataForInvoices   (the loader; guarded parameters, see proformaData)
 *   usp_GetFINoForInvoice        @OrganizationId @CompanyId [@SupplierCustomerId]                    ExImEFormRegistration.GetFINoForInvoice
 *   Sp_ExImEFormRegistration_GetAllMethod @OrganizationId @CompanyId @DocumentTypeId @Id @Activity='GetFinancialInstrumentsBalance'
 *   USP_ExImInvoice_FormHistory  see history()
 *   Sp_ExImInvoice_Insert / Sp_ExImInvoice_Update, Sp_ExImInvoicePackingDetail_Insert, Sp_ExImInvoicePaymentTermsDetail_Insert,
 *   Sp_ExImInvoiceOtherItems_Insert, and for 212 the voucher: Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',
 *   Sp_VoucherHead_Insert | Sp_VoucherHead_Update, Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert,
 *   Sp_VoucherDetail_H_Insert - DAL ExImInvoice.SetData, one transaction.
 *
 * No table, column or procedure is created or changed. Tenancy is always the session user's.
 */
@Repository
public class ExportPreInvoiceRepository {

    private final JdbcTemplate jdbc;

    public ExportPreInvoiceRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private Map<String, Object> oc(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    // ============================================================== shared lookups

    public List<Map<String, Object>> exportCustomers(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadByOrganizationCompanyIdForExport");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** SupplierCustomer.GetSupplierustomerByCustomerGroupId: @ParentId only when != 0 (never here), @CustomerGroupIds when not empty. */
    public List<Map<String, Object>> customersByGroup(UserAccount u, String groupIds) {
        Map<String, Object> p = oc(u);
        if (groupIds != null && !groupIds.isEmpty()) p.put("CustomerGroupIds", groupIds);
        p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** CommonServies.GetSupplierCustomerListForFinancialEffects (DAL 0205:208). */
    public List<Map<String, Object>> customerGlAccounts(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll"));
    }

    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod", params("Activity", "ReadAll",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> seaPorts(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadByCompanyNOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_SeaPorts_GetAllMethod", p);
    }

    public List<Map<String, Object>> currencies(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", p);
    }

    public List<Map<String, Object>> banks(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_Bank_GetAllMethod", p);
    }

    public List<Map<String, Object>> packTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> cropYears(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadAll");
        return DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", p);
    }

    public List<Map<String, Object>> uomsForItem(UserAccount u, int itemId) {
        Map<String, Object> p = oc(u);
        p.put("ItemId", itemId);
        p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    /** CommonServices.ItemGetForComboServiceBind(parent) -> Item.GetAllbyCombobind. */
    public List<Map<String, Object>> itemsComboTwoColumns(UserAccount u, int parentCategoryId) {
        Map<String, Object> p = oc(u);
        if (parentCategoryId != 0) p.put("InventoryParentCategoriesId", parentCategoryId);
        p.put("Activity", "ReadAllForComboTwoColumns");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** Item.ReadAllForExportCombo. */
    public List<Map<String, Object>> itemsForExportCombo(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadAllForExportCombo");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    /** CommonServices.GetItemByItemTypeId(ids) -> Item.GetItemByItemTypeId: @LookupTypeIds always. */
    public List<Map<String, Object>> itemsByType(UserAccount u, String ids) {
        Map<String, Object> p = oc(u);
        p.put("LookupTypeIds", ids);
        p.put("Activity", "GetItemByItemTypeId");
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", p);
    }

    public List<Map<String, Object>> branches(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "GetAll");
        return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", p);
    }

    public List<Map<String, Object>> projects(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("MethodType", "GetAll");
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", p);
    }

    /** CommonServices.CoaAllocationAccountTitleByAccountTypeIds(ids) -> COAAllocation.GetAccountTitleByAccountTypeIds. */
    public List<Map<String, Object>> coaByAccountTypes(UserAccount u, String accountTypeIds) {
        Map<String, Object> p = oc(u);
        p.put("AppId", u.getAppId() == null ? 0 : u.getAppId());
        if (accountTypeIds != null && !accountTypeIds.isEmpty()) p.put("AccountTypeIds", accountTypeIds);
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    public List<Map<String, Object>> warehouses(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", p);
    }

    public List<Map<String, Object>> jobLots(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Activity", "GetAll");
        return DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", p);
    }

    /** ExImLcOrder.GetDataForDropDownFromExportInvoice(org, comp) - no Activity, no DocumentTypeIds. */
    public List<Map<String, Object>> dropDownsFromExportInvoice(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoice]", oc(u));
    }

    /** ExImLcOrder.GetDataForDropDownFromExportContract(org, comp) - no customer, no ActionId, no Activity. */
    public List<Map<String, Object>> dropDownsFromExportContract(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportContract]", oc(u));
    }

    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", oc(u))) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == featureId) return true;
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    // ============================================================== ExImInvoice reads

    /** ExImInvoice.GenerateCode: @FinancialYearId / @BranchesId only when != 0. */
    public int generateDocNo(UserAccount u, int documentTypeId, int financialYearId, int branchesId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        p.put("Activity", "GenerateDocNo");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("DocNo");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    /** ExImInvoice.GenerateInvoiceNo. */
    public String generateInvoiceNo(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateInvoiceNo");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("InvoiceNo");
        return v == null ? "" : String.valueOf(v);
    }

    /** ExImInvoice.GetByID header (DAL GetData's first call). */
    public Map<String, Object> invoiceById(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    public List<Map<String, Object>> invoiceDetail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", id, "Activity", "ReadExImInvoicePackingDetailByHeaderId"));
    }

    public List<Map<String, Object>> invoiceOtherItems(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", id, "Activity", "ReadExImInvoiceOtherItemsByHeaderId"));
    }

    public List<Map<String, Object>> invoicePaymentTerms(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", id, "Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"));
    }

    /** ExImLcOrder.ReadOtherItemsByProformaHeaderIds(ids) - the ids string exactly as the form built it (leading comma). */
    public List<Map<String, Object>> otherItemsByProformaIds(String ids) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Ids", ids, "Activity", "ReadExImInvoiceOtherItemsByProformaHeaderIds"));
    }

    /** ExImLcOrder.GetOtherItemByContractId with the form's empty ReportsParameters.Id (0, always sent). */
    public List<Map<String, Object>> otherItemsByContract(UserAccount u) {
        Map<String, Object> p = oc(u);
        p.put("Id", 0);
        p.put("Activity", "GetOtherItemByContractId");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    /**
     * ExImInvoice.GetProformaDataForPreInvoices (preInvoices = true) / GetProformaDataForInvoices: @OrganizationId
     * @CompanyId @BranchesId @FinancialYearId @DocumentTypeIds always; @Ids when not empty; @SupplierCustomerId /
     * @FcurrencyId / @ItemId / @SkipZero when != 0 (the loader and the forms never set FromDate / ToDate / schedules).
     */
    public List<Map<String, Object>> proformaData(boolean preInvoices, UserAccount u, int branchesId, int financialYearId,
                                                  String documentTypeIds, String ids, int supplierCustomerId, int fcyId,
                                                  int itemId, int skipZero) {
        Map<String, Object> p = oc(u);
        p.put("BranchesId", branchesId);
        p.put("FinancialYearId", financialYearId);
        p.put("DocumentTypeIds", documentTypeIds);
        if (ids != null && !ids.isEmpty()) p.put("Ids", ids);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (fcyId != 0) p.put("FcurrencyId", fcyId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (skipZero != 0) p.put("SkipZero", skipZero);
        return DesktopProc.rows(jdbc, preInvoices ? "USP_GetProformaDataForPreInvoices" : "USP_GetProformaDataForInvoices", p);
    }

    public List<Map<String, Object>> finoForInvoice(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = oc(u);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return DesktopProc.rows(jdbc, "usp_GetFINoForInvoice", p);
    }

    /** ExImEFormRegistration.GetFinancialInstrumentsBalance - first cell of the first row, 0 when none. */
    public Object fiBalance(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = oc(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Id", id);
        p.put("Activity", "GetFinancialInstrumentsBalance");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ExImEFormRegistration_GetAllMethod", p);
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        return r.get(0).values().iterator().next();
    }

    /** ExImInvoice.FormHistory: the guarded parameters as the BLL adds them (see the service). */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, Map<String, Object> guarded) {
        Map<String, Object> p = oc(u);
        p.put("FinancialYearId", financialYearId);
        p.putAll(guarded);
        return DesktopProc.rows(jdbc, "USP_ExImInvoice_FormHistory", p);
    }

    public int voucherHeadId(UserAccount u, int documentTypeId, int documentTypeSrNo) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", params(
                "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", documentTypeSrNo));
        if (r.isEmpty()) return 0;
        Object v = r.get(0).get("Id");
        return v instanceof Number ? ((Number) v).intValue() : 0;
    }

    // ============================================================== ExImInvoice save (DAL ExImInvoice.SetData)

    /**
     * DAL ExImInvoice.SetData for the parts these forms fill: header (Insert / Update), packing detail (LineId
     * 1..n in list order, removed rows first), payment terms, other items, then - when a voucher was built
     * (DocumentTypeId 212) - the voucher head insert or update, its details, USP_VoucherBalanceCheck and the
     * history mirrors. One transaction; any failure rolls everything back.
     */
    @Transactional(rollbackFor = Exception.class)
    public int saveInvoice(Map<String, Object> header, String proc, List<Map<String, Object>> details,
                           List<Map<String, Object>> paymentTerms, List<Map<String, Object>> otherItems,
                           UserAccount u, Map<String, Object> voucherHead, List<Map<String, Object>> voucherDetails) {
        int num3 = DesktopProc.setProc(jdbc, proc, header);
        if (num3 > 0) header.put("Id", num3);
        else num3 = asInt(header.get("Id"));
        int id = asInt(header.get("Id"));
        int line = 0;
        for (Map<String, Object> d : details) {
            line++;
            d.put("LineId", line);
            d.put("ExImInvoiceId", id);
            d.put("Id", DesktopProc.setProc(jdbc, "Sp_ExImInvoicePackingDetail_Insert", d));
        }
        for (Map<String, Object> t : paymentTerms) {
            t.put("ExImInvoiceId", id);
            DesktopProc.setProc(jdbc, "Sp_ExImInvoicePaymentTermsDetail_Insert", t);
        }
        for (Map<String, Object> o : otherItems) {
            o.put("ExImInvoiceId", id);
            o.put("Id", DesktopProc.setProc(jdbc, "Sp_ExImInvoiceOtherItems_Insert", o));
        }
        if (voucherHead != null && !voucherDetails.isEmpty()) {
            int existing = voucherHeadId(u, asInt(header.get("DocumentTypeId")), id);
            if (existing > 0) voucherHead.put("Id", existing);
            voucherHead.put("DocumentTypeSrNo", id);
            int num = DesktopProc.setProc(jdbc, existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", voucherHead);
            if (num > 0) voucherHead.put("Id", num);
            int vhId = asInt(voucherHead.get("Id"));
            for (Map<String, Object> d : voucherDetails) {
                d.put("VoucherHeadId", vhId);
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", d);
            }
            if (vhId > 0) {
                DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", params(
                        "OrganizationId", header.get("OrganizationId"), "CompanyId", header.get("CompanyId"), "Id", vhId));
            }
            int ref = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", voucherHead);
            for (Map<String, Object> d : voucherDetails) {
                d.put("VoucherHeadId", vhId);
                d.put("DocumentTypeIdRef", ref);
                DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", d);
            }
        }
        return num3;
    }

    static int asInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
