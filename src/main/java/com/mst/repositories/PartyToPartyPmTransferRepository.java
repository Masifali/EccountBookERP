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

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 335 "Packing Material Transfer (Party to Party)" — every read and write the desktop form
 * {@code Architecture.WinApp.StoreManagement.PartyToPartyPackingMaterialTransfer} makes.
 *
 * Traced from:
 *   BLL 0251  Architecture.BLL.PurchaseTrading.InvPmStockWithPartiesTransferHeader
 *             (Save, GetByID, FormHistory, P2PPM419Slip, GenerateCode)
 *   DAL 0220  Architecture.DAL.PurchaseTrading.InvPmStockWithPartiesTransferHeader (SetData, GetAll)
 *   Model 0196 InvPmStockWithPartiesTransferHeader / 0195 InvPmStockWithPartiesTransferDetail
 *   BLL 0379  GlobalServicesMethods — the global lists (AllItems, SupplierCustomerLists, UomSchedule)
 *
 * Procedures (all in /root/ddl/procs.sql):
 *   dbo.Sp_InvPmStockWithPartiesTransferHeader_GetAllMethod  ('GenerateCode', 'ReadById',
 *                                                              'ReadByDetailHeaderId', 'FormHistory')
 *   dbo.Sp_InvPmStockWithPartiesTransferHeader_Insert / _Update
 *   dbo.Sp_InvPmStockWithPartiesTransferDetail_Insert
 *   dbo.USP_PmStockWithPartiesTransfer_SlipAndRegister
 *   dbo.USP_Item_AllItemsWithModal, dbo.usp_getAllUomsByCompanyId, dbo.USP_GetVendorsAndCustomersWithCityName
 *
 * A parameter the BLL adds only under a condition is only added here under the same condition;
 * a null is never bound (DesktopProc omits it, as ADO.NET does with a CLR null).
 */
@Repository
public class PartyToPartyPmTransferRepository {

    public static final String P_GETALL = "Sp_InvPmStockWithPartiesTransferHeader_GetAllMethod";
    public static final String P_INSERT = "Sp_InvPmStockWithPartiesTransferHeader_Insert";
    public static final String P_UPDATE = "Sp_InvPmStockWithPartiesTransferHeader_Update";
    public static final String P_DETAIL_INSERT = "Sp_InvPmStockWithPartiesTransferDetail_Insert";
    public static final String P_SLIP = "USP_PmStockWithPartiesTransfer_SlipAndRegister";

    private final JdbcTemplate jdbc;
    private final StoreIssuanceRepository shared;

    public PartyToPartyPmTransferRepository(JdbcTemplate jdbc, StoreIssuanceRepository shared) {
        this.jdbc = jdbc;
        this.shared = shared;
    }

    // ============================================================================ numbering

    /** BLL 0251 GenerateCode:198 — @Activity, @OrganizationId, @CompanyId, @DocumentTypeId; reads DocNo. */
    public int generateCode(UserAccount u, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "Activity", "GenerateCode",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId));
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ================================================================================ reads

    /** BLL 0251 GetByID:32 → DAL 0220 GetAll:343 — the header rows ('ReadById', @Id then @Activity). */
    public List<Map<String, Object>> readById(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
    }

    /** DAL 0220 GetAll:359-362 — the detail rows ('ReadByDetailHeaderId', @Id then @Activity). */
    public List<Map<String, Object>> details(int headerId) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", headerId, "Activity", "ReadByDetailHeaderId"));
    }

    /**
     * BLL 0251 FormHistory:56 — @OrganizationId, @CompanyId, @DocumentTypeId, @Activity always;
     * @FromDocNo / @ToDocNo only when != 0; each date only when set (Conversion.CheckDateTimeNull).
     * The BLL spells the first date parameter "FromDate" without "@"; SqlClient adds it, so it is
     * the procedure's @FromDate.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, int fromDocNo, int toDocNo,
                                                 Timestamp fromDate, Timestamp toDate,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "Activity", "FormHistory");
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        p.put("FromDate", fromDate);
        p.put("ToDate", toDate);
        p.put("EntryFromDate", entryFrom);
        p.put("EntryToDate", entryTo);
        p.put("ModifyFromDate", modifyFrom);
        p.put("ModifyToDate", modifyTo);
        p.put("ApprovedFromDate", approvedFrom);
        p.put("ApprovedToDate", approvedTo);
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    /** BLL 0251 P2PPM419Slip:169 — @OrganizationId, @CompanyId, @Id (CommonServices.P2PPMSlip419:8180). */
    public List<Map<String, Object>> slip(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, P_SLIP, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", id));
    }

    // ============================================================================== globals

    /**
     * clsGlobalVariables.getGlobalAllItems (USP_Item_AllItemsWithModal) filtered as ItemBind:293
     * does: ItemTypeOfTypeId == 14. Columns as dtitem (Id, ItemName, ItemCode).
     */
    public List<Map<String, Object>> items(UserAccount u, int itemTypeOfTypeId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("ItemTypeOfTypeId")) != itemTypeOfTypeId) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalUomSchedule — getAllUomsByCompanyId(org, comp, 0, 1), mapped the way
     * CommonServices.dtUomFromGloablUomScheduleByItemId:2159 builds its table (plus ItemId, which the
     * page filters on exactly as that helper does).
     */
    public List<Map<String, Object>> uoms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("ItemId", toInt(r.get("ItemId")));
            out.add(o);
        }
        return out;
    }

    /**
     * clsGlobalVariables.globalAllSupplierCustomer — BLL 0379 getGlobalSupplierCustomer(org, comp,
     * 0, 0, 0, "") = USP_GetVendorsAndCustomersWithCityName with the two mandatory parameters, then
     * SupplierFrombind:370 keeps CustomerGroupId != 7 and projects (Id, CompanyName).
     */
    public List<Map<String, Object>> parties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("CustomerGroupId")) == 7) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CompanyName", str(r.get("CompanyName")));
            out.add(o);
        }
        return out;
    }

    /** clsGlobalVariables.globalItemConditions without Id 4 (ItemConditionBindFromGlobal:332). */
    public List<Map<String, Object>> itemConditions() { return shared.itemConditions(); }

    /** CommonServices.GetConfigurationByOrgCompandConfigDescription — the ConfigKey, "" when absent. */
    public String config(UserAccount u, String description) { return shared.config(u, description); }

    // ================================================================================ writes

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()), 0 when no row. */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }
}
