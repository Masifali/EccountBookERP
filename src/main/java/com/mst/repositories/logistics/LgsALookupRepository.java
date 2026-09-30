package com.mst.repositories.logistics;

import com.mst.models.UserAccount;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.map;
import static com.mst.services.hrm.HrmSupport.str;
import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * The desktop "global" caches and common lookups shared by the logistics screens ported here
 * (234 EximServicesDefine, 233 ExImClearingAgentBillDirect, 937 frmLogisticRateNegotiation,
 * 941 frmLogisticRateNegotiationTransporter, 939 frmLogisticAgreement). Each method is the
 * procedure the desktop's loader (BLL Main.GlobalServicesMethods 0379 / the BLL named) runs,
 * with the parameters it sends. Tenancy always from the signed-in user.
 *
 *   clsGlobalVariables.globalAllSupplierCustomer  USP_GetVendorsAndCustomersWithCityName @OrganizationId,@CompanyId
 *   clsGlobalVariables.getGlobalAllSerivesItems   [lgstcm].[USP_Item_AllServiesItems] @OrganizationId,@CompanyId
 *   clsGlobalVariables.globalMultiCurrency        Sp_MultiCurrency_GetAllMethod @OrganizationId,@CompanyId,@Activity='ReadAll'
 *   clsGlobalVariables.globalAllCities            USP_City_GetAllWithCountryAndTehsil @OrganizationId,@CompanyId
 *   clsGlobalVariables.AllAccountsWithCustomGroupId USP_GETAllAccountsFromCustomGroups @OrganizationId,@CompanyId
 *   clsGlobalVariables.globalUomSchedule          usp_getAllUomsByCompanyId @OrganizationId,@CompanyId,@ItemId,@Active=1
 *   GlobalVariables_Helper.GetConfigValueFromGlobal  Sp_ConfigrationsAllocation_GetAllMethod 'GetConfigurationByOrgCompandConfigDescription'
 *   SeaPorts.Getall                               Sp_SeaPorts_GetAllMethod @OrganizationId,@CompanyId,'ReadByCompanyNOrganizationId'
 *   lgstcm LookUps.LookUpAllServices              [lgstcm].[USP_LookAllServices] @OrganizationId,@CompanyId
 *   ExImInvoice.getExportInvoicePendingAndAll     usp_getExportInvoicePendingAndAll @OrganizationId,@CompanyId
 *   CommonServices.CompanyServiceBind             Sp_Company_GetAllMethod @OrgCompanyTypeId=Org,@Activity='ReadByOrganizationId'
 */
@Repository
public class LgsALookupRepository {

    private final HrmProcRepository db;

    public LgsALookupRepository(HrmProcRepository db) { this.db = db; }

    public HrmProcRepository db() { return db; }

    /** globalAllSupplierCustomer filtered r.CustomerGroupId == 10 (the forms' SupplierDtFillFromGlobal). */
    public List<Map<String, Object>> partiesGroup10(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_GetVendorsAndCustomersWithCityName", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            if (toInt(r.get("CustomerGroupId")) != 10) continue;
            out.add(map("Id", toInt(r.get("Id")), "CompanyName", str(r.get("CompanyName")), "PartyCode", str(r.get("PartyCode")),
                    "GlAccountId", toInt(r.get("GlAccountId")), "CityName", str(r.get("CityName")), "MobileNo", str(r.get("MobilePersonal"))));
        }
        return out;
    }

    /** getGlobalAllSerivesItems (GlobalServicesMethods.Item_AllServiesItems :96): ServiceMasterItem -> ServicesMasterItem. */
    public List<Map<String, Object>> servicesItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("[lgstcm].[USP_Item_AllServiesItems]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            out.add(map("Id", toInt(r.get("Id")), "ItemName", str(r.get("ItemName")), "ItemCode", str(r.get("ItemCode")),
                    "ServicesMasterItemId", toInt(r.get("ServicesMasterItemId")), "ServicesMasterItem", str(r.get("ServiceMasterItem")),
                    "ItemCategoryId", toInt(r.get("ItemCategoryId")), "ItemCategory", str(r.get("ItemCategory"))));
        }
        return out;
    }

    /** globalMultiCurrency: Id, CurrencyCode, CurrencyRate. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "ReadAll")) {
            out.add(map("Id", toInt(r.get("Id")), "CurrencyCode", str(r.get("CurrencyCode")), "CurrencyRate", r.get("CurrencyRate")));
        }
        return out;
    }

    /** globalAllCities -> DatatableHelper.PopulateDataTableAndReturn(Id, CityName) = (Id, Description). */
    public List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_City_GetAllWithCountryAndTehsil", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            out.add(map("Id", toInt(r.get("Id")), "Description", str(r.get("CityName"))));
        }
        return out;
    }

    /** SeaPorts.Getall: the first result set (Id, PortName). */
    public List<Map<String, Object>> ports(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_SeaPorts_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "ReadByCompanyNOrganizationId")) {
            out.add(map("Id", toInt(r.get("Id")), "PortName", str(r.get("PortName"))));
        }
        return out;
    }

    /**
     * AllAccountsWithCustomGroupId filtered by AccountTypeId in the given set, first row per
     * ChartOfAccountId (the forms' "group r by r.ChartOfAccountId into g select g.First()" and
     * DatatableHelper.GetAccountsFromGlobalByTypeIds): Id, AccountTitle, AccountCode.
     */
    public List<Map<String, Object>> accountsByTypes(UserAccount u, int... types) {
        Set<Integer> t = new LinkedHashSet<>();
        for (int x : types) t.add(x);
        Set<Integer> seen = new LinkedHashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("USP_GETAllAccountsFromCustomGroups", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            if (!t.contains(toInt(r.get("AccountTypeId")))) continue;
            int id = toInt(r.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            out.add(map("Id", id, "AccountTitle", str(r.get("AccountTitle")), "AccountCode", str(r.get("AccountCode"))));
        }
        return out;
    }

    /** CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId): the item's schedule rows (Id = UOMSchedule.Id, UOMCode). */
    public List<Map<String, Object>> itemUoms(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : db.rows("usp_getAllUomsByCompanyId", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ItemId", itemId, "Active", 1)) {
            if (toInt(r.get("ItemId")) != itemId) continue;
            out.add(map("Id", toInt(r.get("Id")), "UOMCode", str(r.get("UOMCode")), "Equivalent", r.get("Equivalent")));
        }
        return out;
    }

    /** GlobalVariables_Helper.GetConfigValueFromGlobal(name): the ConfigKey, "" when not configured. */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = db.rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription");
        return r.isEmpty() ? "" : str(r.get(0).get("ConfigKey")).trim();
    }

    /** lgstcm LookUps.LookUpAllServices(Org, Comp, null): Id, ReferenceName, OtherReference, Activity. */
    public List<Map<String, Object>> lookAllServices(UserAccount u) {
        return db.rows("[lgstcm].[USP_LookAllServices]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    /** ExImInvoice.getExportInvoicePendingAndAll (ActionId / RecId not set by these forms): Id, InvoiceNo, DocumentTypeId, NoOfContainers, MTon. */
    public List<Map<String, Object>> exportInvoices(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("usp_getExportInvoicePendingAndAll", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            out.add(map("Id", toInt(r.get("Id")), "InvoiceNo", str(r.get("InvoiceNo")), "DocumentTypeId", toInt(r.get("DocumentTypeId")),
                    "NoOfContainers", toInt(r.get("NoOfContainers")), "MTon", r.get("MTon")));
        }
        return out;
    }

    /** CommonServices.CompanyServiceBind: Id, CompName of the user's organization. */
    public List<Map<String, Object>> companies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_Company_GetAllMethod", "OrgCompanyTypeId", u.getOrganizationId(),
                "Activity", "ReadByOrganizationId")) {
            out.add(map("Id", toInt(r.get("Id")), "CompName", str(r.get("CompName"))));
        }
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in user's year (Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId). */
    public Object financialYearStart(UserAccount u, int yearId) {
        List<Map<String, Object>> years = db.rows("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (toInt(r.get("Id")) == yearId) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row == null ? null : row.get("Start_Period");
    }

    /**
     * CommonServices.VoucherHeadIdGet(Id, DocumentTypeId) / DAL CommonServices.GetVoucherHeadId: Sp_Vouchers_GetMethods
     * 'GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId' @OrganizationId,@CompanyId,@DocumentTypeId,@DocumentTypeSrNo; rows[0].Id or 0.
     */
    public int voucherHeadId(UserAccount u, int documentTypeId, int id) {
        List<Map<String, Object>> r = db.rows("Sp_Vouchers_GetMethods", "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId, "DocumentTypeSrNo", id);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }
}
