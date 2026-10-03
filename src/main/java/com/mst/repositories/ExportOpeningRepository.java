package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.support.DesktopProc;
import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of 189 ExportOpening "Export Opening Balance" (Architecture.WinApp.Export.ExportOpening, the
 * "Commercial Invoice Opening" form, ExImInvoice DocumentTypeId 212) and its popup LoadSalesContractForExportOpening.
 * Every procedure / parameter read from procdure.utf8.sql; guarded BLL parameters only sent when set.
 *
 *  Reads
 *   Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'   DefaultDaysToLessFromHistoryFromDate,
 *     FinancialisActiveonCommercialInvoice, CommercialInvoicePrefix, RateAddLessOnCommercialInvoice
 *   USP_GetERPFeaturesByCompanyId @OrganizationId @CompanyId                                 feature 9 (AllowExportMultiCompanies)
 *   Sp_ExImInvoice_GetAllMethod 'GenerateDocNo' @OrganizationId @CompanyId @DocumentTypeId=212 [@FinancialYearId] [@BranchesId]
 *   Sp_ExImInvoice_GetAllMethod 'GenerateInvoiceNo' @OrganizationId @CompanyId @DocumentTypeId=212 [@FinancialYearId]
 *   Sp_Branches_GetAllMethod 'GetAll', Sp_Projects_GetAllMethod @MethodType='GetAll'          (hidden branch / project combos)
 *   [dbo].[USP_GetDataForDropDownFromExportContract] @OrganizationId @CompanyId [@Activity]    Customer / Items / Currency
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport'                   notify parties
 *   Sp_SupplierCustomer_GetAllMethod 'GetSupplierustomerByCustomerGroupId' @ParentId          consignee
 *   Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll', Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll', Sp_SeaPorts_GetAllMethod
 *     'ReadByCompanyNOrganizationId', Sp_Bank_GetAllMethod 'ReadAll', Sp_MultiCurrency_GetAllMethod 'ReadAll',
 *     Sp_InvCropYear_GetAllMethod 'ReadAll', SP_JobLot_ReadMethod 'GetAll', Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll',
 *     usp_getFarmingNTrade, usp_getExportCompaniesByCompanyId, Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'
 *   Sp_UOMSchedule_GetAllMethod @ItemId 'ReadByItemID'                                       pack / rate uom
 *   [dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod] 'GetRemarks' @ItemId [@SupplierCustomerId]   HS code
 *   usp_GetFINoForInvoice [@SupplierCustomerId]; Sp_ExImEFormRegistration_GetAllMethod 'GetFinancialInstrumentsBalance'
 *   [dbo].[USP_GetDataForDropDownFromExportInvoice] @Activity='Customer' @DocumentTypeIds='212' (history customer)
 *   USP_ExImInvoice_FormHistory @DocumentTypeIds='212' ...                                   history grid
 *   Sp_ExImInvoice_GetAllMethod 'ReadById' / 'ReadExImInvoicePackingDetailByHeaderId' /
 *     'ReadExImInvoicePaymentTermsDetailByHeaderId' / 'ReadExImInvoiceOtherItemsByHeaderId'   GetByID
 *   USP_GetProformaDataForPreInvoices @DocumentTypeIds='202,223' @SkipZero=1 ...             loader
 *   USP_GetOtherItemsFromLcorder @OrderIds, Sp_ExImLcOrder_GetAllMethod 'LcOrderPaymentTermsDetailByLcOrderIds' @Ids
 *  Save (DAL ExImInvoice.SetData, one transaction)
 *   Sp_ExImInvoice_Insert | Sp_ExImInvoice_Update, Sp_ExImInvoicePackingDetail_Insert (LineId 1..n),
 *   Sp_ExImInvoicePaymentTermsDetail_Insert, Sp_ExImInvoiceOtherItems_Insert, then the voucher (BLL
 *   MakeVoucherForExImInvoice, always built for 212): Sp_Vouchers_GetMethods 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId',
 *   Sp_VoucherHead_Insert | _Update, Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert,
 *   Sp_VoucherDetail_H_Insert. Voucher look-ups: Sp_SupplierCustomer_GetAllMethod 'GetGlAccountIdandCompanyNameBySupplierCustomerId',
 *   Sp_Item_GetAllMethod 'GetItemGlIdsandItemName'. Attachments are not part of the web port.
 */
@Repository
public class ExportOpeningRepository {

    private static final String INV = "Sp_ExImInvoice_GetAllMethod";
    private final JdbcTemplate jdbc;

    public ExportOpeningRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) { return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()); }
    private List<Map<String, Object>> rows(String proc, Map<String, Object> p) { return DesktopProc.rows(jdbc, proc, p); }
    private List<Map<String, Object>> rowsA(UserAccount u, String proc, String activity) { Map<String, Object> p = tenant(u); p.put("Activity", activity); return rows(proc, p); }

    public String config(UserAccount u, String description) {
        Map<String, Object> p = tenant(u);
        p.put("ConfigDescription", description);
        p.put("Activity", "GetConfigurationByOrgCompandConfigDescription");
        List<Map<String, Object>> r = rows("Sp_ConfigrationsAllocation_GetAllMethod", p);
        if (r.isEmpty()) return "";
        Object v = ExportReturnRepository.ci(r.get(0), "ConfigKey");
        return v == null ? "" : String.valueOf(v).trim();
    }

    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : rows("USP_GetERPFeaturesByCompanyId", tenant(u))) {
            Object id = ExportReturnRepository.ci(r, "Id");
            if (id != null && String.valueOf(id).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    public Object financialYearStart(int financialYearId) {
        if (financialYearId == 0) return null;
        List<Map<String, Object>> r = jdbc.queryForList("SELECT Start_Period FROM FinancialYear WHERE Id = ?", financialYearId);
        return r.isEmpty() ? null : ExportReturnRepository.ci(r.get(0), "Start_Period");
    }

    /** ExImInvoice.GenerateCode -> DocNo (0 when no row). */
    public int generateDocNo(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        int b = u.getBranchesId() == null ? 0 : u.getBranchesId();
        if (b != 0) p.put("BranchesId", b);
        p.put("Activity", "GenerateDocNo");
        List<Map<String, Object>> r = rows(INV, p);
        return r.isEmpty() ? 0 : asInt(ExportReturnRepository.ci(r.get(0), "DocNo"));
    }

    /** ExImInvoice.GenerateInvoiceNo -> InvoiceNo ("" when no row). */
    public String generateInvoiceNo(UserAccount u, int documentTypeId, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GenerateInvoiceNo");
        List<Map<String, Object>> r = rows(INV, p);
        Object v = r.isEmpty() ? null : ExportReturnRepository.ci(r.get(0), "InvoiceNo");
        return v == null ? "" : String.valueOf(v);
    }

    public List<Map<String, Object>> branches(UserAccount u) { return rowsA(u, "Sp_Branches_GetAllMethod", "GetAll"); }
    public List<Map<String, Object>> projects(UserAccount u) { Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll"); return rows("Sp_Projects_GetAllMethod", p); }

    public List<Map<String, Object>> exportContractDropDown(UserAccount u, String activity) {
        Map<String, Object> p = tenant(u);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return rows("[dbo].[USP_GetDataForDropDownFromExportContract]", p);
    }

    public List<Map<String, Object>> exportCustomers(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "ReadByOrganizationCompanyIdForExport"); }

    /** CommonServices.GetSupplierustomerByCustomerGroupId(null, ParentId). */
    public List<Map<String, Object>> consignees(UserAccount u, int parentId) {
        Map<String, Object> p = tenant(u);
        if (parentId != 0) p.put("ParentId", parentId);
        p.put("Activity", "GetSupplierustomerByCustomerGroupId");
        return rows("Sp_SupplierCustomer_GetAllMethod", p);
    }

    public List<Map<String, Object>> deliveryTerms() { return rows("Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll")); }
    public List<Map<String, Object>> paymentTerms(UserAccount u) { return rows("Sp_ExImLcPaymentTerm_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> seaPorts(UserAccount u) { return rowsA(u, "Sp_SeaPorts_GetAllMethod", "ReadByCompanyNOrganizationId"); }
    public List<Map<String, Object>> banks(UserAccount u) { return rowsA(u, "Sp_Bank_GetAllMethod", "ReadAll"); }
    public List<Map<String, Object>> currencies(UserAccount u) { return rowsA(u, "Sp_MultiCurrency_GetAllMethod", "ReadAll"); }
    public List<Map<String, Object>> cropYears(UserAccount u) { return rowsA(u, "Sp_InvCropYear_GetAllMethod", "ReadAll"); }
    public List<Map<String, Object>> jobLots(UserAccount u) { return rowsA(u, "SP_JobLot_ReadMethod", "GetAll"); }
    public List<Map<String, Object>> packTypes(UserAccount u) { return rows("Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())); }
    public List<Map<String, Object>> farmingNTrade() { return rows("usp_getFarmingNTrade", params()); }
    public List<Map<String, Object>> exportCompanies(UserAccount u) { return rows("usp_getExportCompaniesByCompanyId", tenant(u)); }

    public List<Map<String, Object>> creditAccounts(UserAccount u) {
        Map<String, Object> p = tenant(u);
        p.put("AppId", u.getAppId() == null ? 0 : u.getAppId());
        p.put("AccountTypeIds", "3,6,8,10");
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return rows("Sp_COAAllocation_GetAllMethod", p);
    }

    public List<Map<String, Object>> uomSchedule(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return rows("Sp_UOMSchedule_GetAllMethod", p);
    }

    public List<Map<String, Object>> commodityRemarks(UserAccount u, int itemId, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        p.put("ItemId", itemId);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        p.put("Activity", "GetRemarks");
        return rows("[dbo].[USP_CommodityDetailItemCustomerWise_GetAllMethod]", p);
    }

    public List<Map<String, Object>> fiNosForInvoice(UserAccount u, int supplierCustomerId) {
        Map<String, Object> p = tenant(u);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        return rows("usp_GetFINoForInvoice", p);
    }

    public Object fiBalance(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", documentTypeId);
        p.put("Id", id);
        p.put("Activity", "GetFinancialInstrumentsBalance");
        List<Map<String, Object>> r = rows("Sp_ExImEFormRegistration_GetAllMethod", p);
        if (r.isEmpty() || r.get(0).isEmpty()) return 0;
        return r.get(0).values().iterator().next();
    }

    /** HistoryCombosFill: GetDataForDropDownFromExportInvoice(DocumentTypeIds "212", Activity "Customer"). */
    public List<Map<String, Object>> historyCustomers(UserAccount u, String documentTypeIds) {
        Map<String, Object> p = tenant(u);
        p.put("Activity", "Customer");
        p.put("DocumentTypeIds", documentTypeIds);
        return rows("[dbo].[USP_GetDataForDropDownFromExportInvoice]", p);
    }

    public List<Map<String, Object>> formHistory(Map<String, Object> p) { return rows("USP_ExImInvoice_FormHistory", p); }

    public List<Map<String, Object>> headerById(int id) { return rows(INV, params("Id", id, "Activity", "ReadById")); }
    public List<Map<String, Object>> child(int id, String activity) { return rows(INV, params("Id", id, "Activity", activity)); }

    /** ExImInvoice.GetProformaDataForPreInvoices. */
    public List<Map<String, Object>> proformaDataForPreInvoices(Map<String, Object> p) { return rows("USP_GetProformaDataForPreInvoices", p); }

    public List<Map<String, Object>> otherItemsFromLcOrder(UserAccount u, String ids) {
        Map<String, Object> p = tenant(u); p.put("OrderIds", ids);
        return rows("USP_GetOtherItemsFromLcorder", p);
    }
    public List<Map<String, Object>> lcOrderPaymentTerms(String ids) { return rows("Sp_ExImLcOrder_GetAllMethod", params("Ids", ids, "Activity", "LcOrderPaymentTermsDetailByLcOrderIds")); }

    public List<Map<String, Object>> supplierCustomerGl(UserAccount u) { return rowsA(u, "Sp_SupplierCustomer_GetAllMethod", "GetGlAccountIdandCompanyNameBySupplierCustomerId"); }
    public List<Map<String, Object>> itemGl(UserAccount u) { return rowsA(u, "Sp_Item_GetAllMethod", "GetItemGlIdsandItemName"); }

    public int voucherHeadId(int org, int company, int documentTypeId, int id) {
        List<Map<String, Object>> r = rows("Sp_Vouchers_GetMethods", params("Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org, "CompanyId", company, "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id));
        return r.isEmpty() ? 0 : asInt(ExportReturnRepository.ci(r.get(0), "Id"));
    }

    // ================================================================== save (DAL ExImInvoice.SetData)

    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> packing, List<Map<String, Object>> paymentTerms,
                    List<Map<String, Object>> otherItems, SaleInvoiceModels.VoucherHead vh, List<SaleInvoiceModels.VoucherDetail> vds) {
        int org = asInt(header.get("OrganizationId")), company = asInt(header.get("CompanyId"));
        int documentTypeId = asInt(header.get("DocumentTypeId"));
        boolean insert = asInt(header.get("Id")) == 0;
        int num3 = DesktopProc.setProc(jdbc, insert ? "Sp_ExImInvoice_Insert" : "Sp_ExImInvoice_Update", header);
        int id;
        if (num3 > 0) id = num3; else { id = asInt(header.get("Id")); num3 = id; }
        int line = 0;
        for (Map<String, Object> d : packing) { d.put("LineId", ++line); d.put("ExImInvoiceId", id); d.put("Id", DesktopProc.setProc(jdbc, "Sp_ExImInvoicePackingDetail_Insert", d)); }
        for (Map<String, Object> d : paymentTerms) { d.put("ExImInvoiceId", id); DesktopProc.setProc(jdbc, "Sp_ExImInvoicePaymentTermsDetail_Insert", d); }
        for (Map<String, Object> d : otherItems) { d.put("ExImInvoiceId", id); d.put("Id", DesktopProc.setProc(jdbc, "Sp_ExImInvoiceOtherItems_Insert", d)); }

        if (vh != null && !vds.isEmpty()) {
            int num2 = voucherHeadId(org, company, documentTypeId, id);
            if (num2 > 0) vh.Id = num2;
            vh.DocumentTypeSrNo = id;
            int num = ExportMProc.setProc(jdbc, num2 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
            if (num > 0) vh.Id = num;
            for (SaleInvoiceModels.VoucherDetail d : vds) { d.VoucherHeadId = vh.Id; ExportMProc.setProc(jdbc, "Sp_VoucherDetail_Insert", d); }
            if (vh.Id > 0) ProcExec.run(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
            int documentTypeIdRef = ExportMProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", vh);
            for (SaleInvoiceModels.VoucherDetail d : vds) { d.VoucherHeadId = vh.Id; d.DocumentTypeIdRef = documentTypeIdRef; ExportMProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", d); }
        }
        return num3;
    }

    // ================================================================== model defaults

    /** Every non-virtual ExImInvoice value property at its CLR default (strings null = not sent). */
    public static Map<String, Object> blankHeader() {
        Map<String, Object> p = new LinkedHashMap<>();
        String[] ints = {"Id", "CommercialInvoiceId", "ApprovedUser", "BranchesId", "CompanyId", "PrefixTypeId", "DeliveryTermId", "ContinentId",
                "OriginCountryId", "ImporterCountryId", "PlaceOfDeliveryId", "DestinationPortId", "DocCode", "DocumentTypeId", "EFormId", "EntryUser",
                "ExporteBankId", "ExportCompanyId", "FcurrencyId", "ImporterBankId", "LcOrderNoId", "LoadingPortId", "ModifyUser", "OrganizationId",
                "PaymentTermId", "ProjectsId", "SupplierCustomerId", "CommissionAgentId", "FinancialYearId", "PreInvoiceId", "CommissionDebitAcId",
                "OtherCustomerId", "OtherDestinationPortId", "ConsigneeId", "ActionId", "NotifyParty1", "NotifyParty2", "NotifyParty3",
                "CreditAccountId", "SubContractExpiryDays"};
        String[] dbls = {"BuyerGLValueRs", "ConversionRate", "EquivalentAmount", "FCurrencyAmount", "FobValue", "GrossWeight", "InvoiceCommercialValue",
                "NetWeight", "NoOfContainers", "AddLessAmount", "CommissionPercentage", "CommissionAmount", "TotalAmount"};
        for (String k : ints) p.put(k, 0);
        for (String k : dbls) p.put(k, 0d);
        p.put("GrossMton", java.math.BigDecimal.ZERO);
        p.put("NetMton", java.math.BigDecimal.ZERO);
        p.put("IsApproved", false);
        return p;
    }

    /** ExImInvoicePackingDetail with every int/double property at its default (strings null = not sent). */
    public static Map<String, Object> blankPacking() {
        Map<String, Object> d = new LinkedHashMap<>();
        String[] ints = {"Id", "ExImInvoiceId", "ItemId", "ItemUOMId", "BrandId", "PackingMaterialTypeId", "InnerQtyUomId", "OuterQtyUomId",
                "RateUomId", "ItemSpicificationId", "CropYearId", "JobLotId", "ContractDetailId", "ContractId", "PreInvoiceId",
                "PreInvoiceDetailId", "LineId", "ContractScheduleId", "ActionTypeId", "AmountCalulationId", "ExImTradeTypeId",
                "ExImFarmingTypeId", "ExImFarmingNTradeId", "ContractScheduleDetailId", "LabInspectionId", "PmItemId"};
        String[] dbls = {"InnerQty", "OuterQty", "NetWeight", "RatePrice", "FcAmount", "PackingWeight", "TotalPackingWeight", "GrossWeight",
                "MTon", "NoofContainers", "NoofBagsPerContainer", "OtherRate", "OtherAmount", "RateAddLess", "RateWithoutAddLess", "PalletQty", "AddLessAmount"};
        for (String k : ints) d.put(k, 0);
        for (String k : dbls) d.put(k, 0d);
        return d;
    }

    static Timestamp ts(java.time.LocalDateTime t) { return Timestamp.valueOf(t); }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
}
