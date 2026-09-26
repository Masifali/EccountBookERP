package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 347 "Store Issuance To Consumable Store" (frmStoreIssuanceToCosumableStore.cs,
 * DocumentTypeId 1616) and its loader LoadDepRequestToConsumableStore.cs — the reads that the
 * shared {@link StoreIssuanceRepository} does not already have, each exactly as its BLL builds it:
 *
 *   BLL 0058  Branches.GetAll                                   Sp_Branches_GetAllMethod 'GetAll'
 *   BLL 0078  Projects.GetAlldt                                 Sp_Projects_GetAllMethod @MethodType 'GetAll'
 *   BLL 0252  InvGsStoreIssuanceHeader.FormHistory              (with @NoOfRecords, which the shared
 *                                                                repository's formHistory does not send)
 *   BLL 0252  GetPendingDepartmentRequestForIssuanceNew         (with the work-station / work-order
 *                                                                filters the shared method omits)
 *   BLL 0252  GetDataForDropDownFromDepartmentRequest           (reused from StoreIssuanceRepository)
 *   BLL 0252  StoreIssuanceHistory (@Id only)                   Sp_InvGsStoreIssuanceHeader_SlipandRegister
 *   BLL 0056  GetAvgRatesAndStockInHand.AvgRateOnlyForCGS       'GetOnlyAvgRateForCGS'
 *
 * Every parameter was checked against /root/ddl/procs.json. A parameter the BLL only adds under a
 * condition is only added here under the same condition; a null is never bound (DesktopProc).
 */
@Repository
public class StoreIssuanceToConsumableRepository {

    private final JdbcTemplate jdbc;
    public StoreIssuanceToConsumableRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** CommonServices.BrancheServiceBind → BLL 0058 Branches.GetAll. */
    public List<Map<String, Object>> branches(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Branches_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Activity", "GetAll"));
    }

    /** CommonServices.ProjectServiceBind → BLL 0078 Projects.GetAlldt. */
    public List<Map<String, Object>> projects(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "MethodType", "GetAll"));
    }

    /**
     * CommonServices.StoreIssuanceHistory(1616, viewAll, NoOfRecords) → BLL 0252 FormHistory.
     * The ReportsParameters it builds sets only Org, Company, DocumentTypeId, FinancialYearId,
     * CanViewAllRecord, NoOfRecords and (when !viewAll) EntryUser — so no branch, base type,
     * dates or doc numbers are ever sent from this screen.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, int financialYearId,
                                                 boolean canViewAll, int noOfRecords, int entryUser) {
        Map<String, Object> p = params(
                "organizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (noOfRecords != 0)     p.put("NoOfRecords", noOfRecords);
        if (!canViewAll)          p.put("EntryUser", entryUser);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, StoreIssuanceRepository.P_HEADER_GETALL, p);
    }

    /** LoadDepRequestToConsumableStore.PendingDepRequestLoad:301 → BLL 0252 GetPendingDepartmentRequestForIssuanceNew. */
    public List<Map<String, Object>> pendingDepartmentRequests(UserAccount u, int documentTypeId, int financialYearId,
                                                               Timestamp from, Timestamp to, int docNoFrom, int docNoTo,
                                                               int itemId, int workStationFromId, int workStationToId,
                                                               int workOrderId, int departmentFromId, int departmentToId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0)   p.put("FinancialYearId", financialYearId);
        if (from != null)           p.put("FromDate", from);
        if (to != null)             p.put("ToDate", to);
        if (docNoFrom != 0)         p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0)           p.put("DocNoTo", docNoTo);
        if (itemId != 0)            p.put("ItemId", itemId);
        if (workStationFromId != 0) p.put("WorkStationFromId", workStationFromId);
        if (workStationToId != 0)   p.put("WorkStationToId", workStationToId);
        if (workOrderId != 0)       p.put("WorkOrderId", workOrderId);          // obj.OrderId
        if (departmentFromId != 0)  p.put("DepartmentFromId", departmentFromId);
        if (departmentToId != 0)    p.put("DepartmentToId", departmentToId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetPendingDepartmentRequestForIssuance]", p);
    }

    /** BLL 0056 AvgRateOnlyForCGS — only Org, Company, ItemId, DocDate are set by the form (:397). */
    public double avgRateOnlyForCgs(UserAccount u, int itemId, Timestamp docDate) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_GetAvgRatesAndStockInHand_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ItemId", itemId,
                "DocDate", docDate,
                "Activity", "GetOnlyAvgRateForCGS"));
        return r.isEmpty() ? 0d : StoreIssuanceRepository.toDouble(r.get(0).get("AvgRate"));
    }

    /** CommonServices.StoreIssuanceSlip1616 → BLL 0252 StoreIssuanceHistory with Org, Company, Id. */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvGsStoreIssuanceHeader_SlipandRegister", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", id));
    }

    /**
     * Web-only tenancy guard for a removed-row id: the detail must belong to a 1616 document of the
     * signed-in company. (The desktop sends whatever ids are in lstRemoveRecord.)
     */
    public boolean detailOfCompanyDocType(UserAccount u, int detailId, int documentTypeId) {
        Integer n = jdbc.queryForObject(
                "SELECT COUNT(1) FROM dbo.InvGsStoreIssuanceDetail d "
              + "INNER JOIN dbo.InvGsStoreIssuanceHeader h ON h.Id = d.InvGsStoreIssuanceHeaderId "
              + "WHERE d.Id = ? AND h.OrganizationId = ? AND h.CompanyId = ? AND h.DocumentTypeId = ?",
                Integer.class, detailId, u.getOrganizationId(), u.getCompanyId(), documentTypeId);
        return n != null && n > 0;
    }
}
