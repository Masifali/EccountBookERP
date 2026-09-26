package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 506 "Delivery Order Packing Material" (StoreManagement/DeliveryOrderPackingMaterial.cs, DocumentTypeId 85):
 * the BLL 0558 / DAL 0411 calls {@link DeliveryOrderTransferRepository} fixes at 84, taken here with 85, and the
 * form's own lookups. The shared ones (branches, customers, vehicle types, header, setProc) stay on that class.
 */
@Repository
public class DeliveryOrderPmRepository {

    public static final int DOC = 85;
    private static final String P = DeliveryOrderTransferRepository.P_GETALL;

    private final JdbcTemplate jdbc;
    public DeliveryOrderPmRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** DocumentNoFill:905 - CommonServices.DeliveryOrderGenerateCode(85): FY and the user's branch when non-zero. */
    public int generateCode(UserAccount u, int financialYearId, int branchesId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        p.put("Activity", "GenerateCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    /** txtdocno_TextChanged:766 (wired to Leave) - BLL 0558 ReadIdByDocNo, Rows[0]["Id"] or 0. */
    public int idByDocNo(UserAccount u, int docNo) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P, params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocNo", docNo, "DocumentTypeId", DOC, "Activity", "ReadIdByDocNo"));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("Id"));
    }

    /** CombProjectFill:828 - Projects.GetAlldt: Sp_Projects_GetAllMethod @MethodType 'GetAll'. */
    public List<Map<String, Object>> projects(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "MethodType", "GetAll"));
    }

    /** CmbPurchaseOrderNo:989 - PurchaseOrderIdandNoGetForSupplierCustomerId with DocumentTypeId 41. */
    public List<Map<String, Object>> orders(UserAccount u, int customerId) {
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrder_GetAllMethod", params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "OrderSupCustId", customerId, "DocumentTypeId", 41,
                "Activity", "PurchaseOrderIdandNoGetForSupplierCustomerId"));
    }

    /** BalRetainQty:1126 - GetBalRetainBagQtyAgainstSupplierAndItem, Rows[0]["RetainBalQty"] or 0. */
    public double balRetainQty(UserAccount u, int customerId, int itemId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "SupplierCustomerId", customerId);
        if (itemId != 0) p.put("ItemId", itemId);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "[dbo].[USp_GetBalRetainBagQtyAgainstSupplierAndItem]", p);
        return r.isEmpty() ? 0d : toDouble(r.get(0).get("RetainBalQty"));
    }

    /** gridhistoryfill:1561 - FormHistoryForPackingMaterial; each optional parameter as the BLL adds it. */
    public List<Map<String, Object>> history(UserAccount u, int financialYearId, boolean canViewAll, int entryUser,
                                             Timestamp from, Timestamp to, Timestamp entryFrom, Timestamp entryTo,
                                             Timestamp modifyFrom, Timestamp modifyTo, Timestamp approvedFrom,
                                             Timestamp approvedTo, int docNoFrom, int docNoTo, int customerId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", entryUser);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (entryFrom != null) p.put("EntryFromDate", entryFrom);
        if (entryTo != null) p.put("EntryToDate", entryTo);
        if (modifyFrom != null) p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null) p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null) p.put("ApprovedToDate", approvedTo);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        if (customerId != 0) p.put("SupplierCustomerId", customerId);
        p.put("Activity", "FormHistoryForPackingMaterial");
        return DesktopProc.rows(jdbc, P, p);
    }

    /** HistoryComboDBCall:1950 - Activity 'Customer', the user's branch as text, no DeliveryOrderType (Tag is not "DeliveryOrder"). */
    public List<Map<String, Object>> historyCustomers(UserAccount u, int financialYearId, String branchesIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        p.put("Activity", "Customer");
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromDeliveryOrder", p);
    }

    /** DAL 0411 GetData - DocumentTypeId 85: @Activity 'ReadByIdPackingMaterialDetail'. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P, params("Id", id, "Activity", "ReadByIdPackingMaterialDetail"));
    }
}
