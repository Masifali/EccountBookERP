package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 904 "frmPurchaseInvoiceDirectStoreWithVariant" (Architecture.WinApp.StoreManagement) - the
 * legacy combo lookups this older form binds with CommonServices (no clsGlobalVariables lists).
 *
 *   CommonServices.CompanyServiceBind          Sp_Company_GetAllMethod @OrgCompanyTypeId @Activity 'ReadByOrganizationId'
 *   CommonServices.BrancheServiceBind          Sp_Branches_GetAllMethod 'GetAll'
 *   CommonServices.ProjectServiceBind          Sp_Projects_GetAllMethod @MethodType 'GetAll'
 *   CommonServices.getActiveWareHouse          Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'
 *   CommonServices.JobLotGetAllService         SP_JobLot_ReadMethod 'GetAll'
 *   CommonServices.GetItemByItemTypeId("14,17") Sp_Item_GetAllMethod 'GetItemByItemTypeId'
 *   CommonServices.GetVendorsAndCustomers(1)   USP_GetVendorsAndCustomers @PartyTypeId 1   (ERP feature 4)
 *   CommonServices.SupplierCustomerGetAllServiceBind Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyId'
 *   CommonServices.CoaAllocationAccountTitleByAccountTypeIds Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'
 *   UOMSchedule.SearchByObject                 Sp_UOMSchedule_GetAllMethod 'ReadByItemID'
 *   ItemAttributeVarient.GetAllForCombo        USP_ItemAttributeVarient_GetAllMethod 'GetAllForCombo'
 *   GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions  Sp_GetAvgRatesAndStockInHand_GetAllMethod
 */
@Repository
public class PurchaseInvoiceDirectStoreVariantRepository {

    private final JdbcTemplate jdbc;
    public PurchaseInvoiceDirectStoreVariantRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> companies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Company_GetAllMethod", params(
                "OrgCompanyTypeId", u.getOrganizationId(), "Activity", "ReadByOrganizationId"))) {
            out.add(pair(toInt(ci(r, "Id")), "CompName", str(ci(r, "CompName"))));
        }
        return out;
    }

    public List<Map<String, Object>> branches(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"))) {
            out.add(pair(toInt(ci(r, "Id")), "BranchName", str(ci(r, "BranchName"))));
        }
        return out;
    }

    public List<Map<String, Object>> projects(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "MethodType", "GetAll"))) {
            out.add(pair(toInt(ci(r, "Id")), "ProjectName", str(ci(r, "ProjectName"))));
        }
        return out;
    }

    public List<Map<String, Object>> warehouses(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetActiveWareHouse"))) {
            out.add(pair(toInt(ci(r, "Id")), "WareHouseName", str(ci(r, "WareHouseName"))));
        }
        return out;
    }

    public List<Map<String, Object>> jobLots(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "SP_JobLot_ReadMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"))) {
            out.add(pair(toInt(ci(r, "Id")), "JobLotDescription", str(ci(r, "JobLotDescription"))));
        }
        return out;
    }

    /** item(): dtitem.Rows.Add(Id, ItemName, ItemCodeNew). */
    public List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "LookupTypeIds", "14,17", "Activity", "GetItemByItemTypeId"))) {
            Map<String, Object> o = pair(toInt(ci(r, "Id")), "ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCodeNew")));
            out.add(o);
        }
        return out;
    }

    /** suppliercustomer(): feature 4 -> GetVendorsAndCustomers(1); otherwise SupplierCustomerGetAllServiceBind. */
    public List<Map<String, Object>> suppliers(UserAccount u, boolean feature4) {
        List<Map<String, Object>> out = new ArrayList<>();
        List<Map<String, Object>> rows = feature4
                ? DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomers", params(
                        "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "PartyTypeId", 1))
                : DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                        "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "Activity", "ReadByOrganizationCompanyId"));
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = pair(toInt(ci(r, "Id")), "CompanyName", str(ci(r, "CompanyName")));
            o.put("PartyCode", str(ci(r, "PartyCode")));
            o.put("GlAccountId", toInt(ci(r, "GlAccountId")));
            out.add(o);
        }
        return out;
    }

    /** AccountsFill() without feature 4: dtAccountlst rows (Id, Id, AccountTitle). */
    public List<Map<String, Object>> accountTitles(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "AppId", u.getAppId(), "AccountTypeIds", "3, 6, 11, 12, 13, 14, 20, 21",
                "UserId", u.getId(), "Activity", "GetAccountTitleByAccountTypeIds"))) {
            out.add(pair(toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "AccountTitle"))));
        }
        return out;
    }

    /** PackUOM(): Id, UOMCode, Equivalent. */
    public List<Map<String, Object>> uoms(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "ReadByItemID"))) {
            Map<String, Object> o = pair(toInt(ci(r, "Id")), "UOMCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** BindVariant(): ItemAttributeVarientId, ItemAttribute, VarientEquivalent. */
    public List<Map<String, Object>> variants(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_ItemAttributeVarient_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "Activity", "GetAllForCombo"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ItemAttributeVarientId")));
            o.put("ItemAttribute", str(ci(r, "ItemAttribute")));
            o.put("VarientEquivalent", toDouble(ci(r, "VarientEquivalent")));
            out.add(o);
        }
        return out;
    }

    /** BalanceStock(): Rows[0][0]; @WarehouseId only when non-zero. */
    public double stockInHand(UserAccount u, int itemId, Timestamp docDate, int warehouseId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDate", docDate,
                "WarehouseId", warehouseId == 0 ? null : (Object) warehouseId,
                "Activity", "GetStockInHandFromInventoryTrasactions"));
        if (r.isEmpty() || r.get(0).isEmpty()) return 0d;
        return toDouble(r.get(0).values().iterator().next());
    }

    private static Map<String, Object> pair(int id, String nameKey, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put(nameKey, name);
        return o;
    }
}
