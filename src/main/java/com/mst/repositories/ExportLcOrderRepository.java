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
 * 210 "Export Lc Order (Not Use)" - Architecture.WinApp.Export.LcOrder (DocumentTypeId 201).
 * BLL ExImLcOrder (0469) / DAL ExImLcOrder (0521) and the form's combo sources, as read from procdure.utf8.sql
 * (01-Oct-2026):
 *
 *   Sp_Branches_GetAllMethod 'GetAll' @OrganizationId @CompanyId                     CommonServices.BrancheServiceBind (Id, BranchName)
 *   Sp_Projects_GetAllMethod @MethodType='GetAll' @OrganizationId @CompanyId          CommonServices.ProjectServiceBind (Id, ProjectName)
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExport'          cmbsupplierfill (Id, CompanyName)
 *   Sp_MultiCurrency_GetAllMethod 'ReadAll'                                          cmbforeigncurrency (Id, CurrencyCode)
 *   Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId'                          LoadingPort (Id, PortName) - both port combos
 *   Sp_Item_GetAllMethod 'GetExportItemsByOrganizationCompanyId'                     ItemDetailFill (Id, ItemName)
 *   Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll' @OrganizationId @CompanyId         ItemPacktype (Id, Description)
 *   Sp_UOMSchedule_GetAllMethod 'ReadByItemID' @OrganizationId @CompanyId @ItemId     cmbitem_Leave (Id, Equivalent) - three UOM combos
 *   Sp_Bank_GetAllMethod 'ReadAll'                                                   importerbank (Foreign / Home Country split)
 *   Sp_ExImDeliveryTerm_GetAllMethod 'ReadAll'                                       DeliveryTerm (Id, Code)
 *   Sp_ExImLcPaymentTerm_GetAllMethod 'ReadAll' @OrganizationId @CompanyId            PaymentTerm (Id, LcOrderTerm)
 *   Sp_ExImLcOrder_GetAllMethod 'GenerateDocNoCompanyIdOrganizationId' @OrganizationId @CompanyId @DocumentTypeId=201   DocNo
 *   Sp_ExImProformaInvoice_GetAllMethod 'ReadByOrganizationCompanyId'                PerformaInvoie (the form keeps SupplierCustomerId == customer)
 *   Sp_ExImLcOrder_GetAllMethod 'ReadByOrganizationCompanyId' @OrganizationId @CompanyId @DocumentTypeId=201   gridFill (History)
 *   Sp_ExImLcOrder_GetAllMethod 'ReadById' @Id + 'ReadByLcOrderHeaderId' @Id          GetByID (DocumentTypeId 201 branch of DAL GetDate)
 *   Sp_ExImLcOrder_Insert / Sp_ExImLcOrder_Update   the 90 non-virtual ExImLcOrder properties
 *   Sp_ExImLcOrderPackingDetail_Insert              the 34 non-virtual ExImLcOrderPackingDetail properties, per grid row
 *
 * DAL SetDate runs the header and the detail rows in one transaction, rolled back on any error (the procedure's
 * RAISERROR text is what the operator sees). No table, column or procedure is created or changed.
 */
@Repository
public class ExportLcOrderRepository {

    public static final int DOCUMENT_TYPE_ID = 201;

    private final JdbcTemplate jdbc;

    public ExportLcOrderRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    private List<Map<String, Object>> act(String proc, UserAccount u, String activity) {
        Map<String, Object> p = tenant(u); p.put("Activity", activity);
        return DesktopProc.rows(jdbc, proc, p);
    }

    public List<Map<String, Object>> branches(UserAccount u) { return act("Sp_Branches_GetAllMethod", u, "GetAll"); }

    public List<Map<String, Object>> projects(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll");
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", p);
    }

    public List<Map<String, Object>> customers(UserAccount u) { return act("Sp_SupplierCustomer_GetAllMethod", u, "ReadByOrganizationCompanyIdForExport"); }

    public List<Map<String, Object>> currencies(UserAccount u) { return act("Sp_MultiCurrency_GetAllMethod", u, "ReadAll"); }

    public List<Map<String, Object>> seaPorts(UserAccount u) { return act("Sp_SeaPorts_GetAllMethod", u, "ReadByCompanyNOrganizationId"); }

    public List<Map<String, Object>> exportItems(UserAccount u) { return act("Sp_Item_GetAllMethod", u, "GetExportItemsByOrganizationCompanyId"); }

    public List<Map<String, Object>> packTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> uomByItem(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    public List<Map<String, Object>> banks(UserAccount u) { return act("Sp_Bank_GetAllMethod", u, "ReadAll"); }

    public List<Map<String, Object>> deliveryTerms() { return DesktopProc.rows(jdbc, "Sp_ExImDeliveryTerm_GetAllMethod", params("Activity", "ReadAll")); }

    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcPaymentTerm_GetAllMethod", params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** ExImLcOrder.GenerateCode - FinancialYearId is never set by this form (not sent). */
    public List<Map<String, Object>> generateCode(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("DocumentTypeId", DOCUMENT_TYPE_ID); p.put("Activity", "GenerateDocNoCompanyIdOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    public List<Map<String, Object>> proformaInvoices(UserAccount u) { return act("Sp_ExImProformaInvoice_GetAllMethod", u, "ReadByOrganizationCompanyId"); }

    public List<Map<String, Object>> history(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("DocumentTypeId", DOCUMENT_TYPE_ID); p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", p);
    }

    public List<Map<String, Object>> header(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id, "Activity", "ReadById"));
    }

    /** DAL GetDate detail: DocumentTypeId 201 -> 'ReadByLcOrderHeaderId', any other -> 'ReadByLcOrderHeaderIdForSaleContracttExport'. */
    public List<Map<String, Object>> detail(int id, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_ExImLcOrder_GetAllMethod", params("Id", id,
                "Activity", documentTypeId == DOCUMENT_TYPE_ID ? "ReadByLcOrderHeaderId" : "ReadByLcOrderHeaderIdForSaleContracttExport"));
    }

    /** DAL ExImLcOrder.SetDate (header + packing detail rows), one transaction. */
    @Transactional(rollbackFor = Exception.class)
    public int save(Map<String, Object> header, List<Map<String, Object>> details) {
        int id = ((Number) header.get("Id")).intValue();
        int num = DesktopProc.setProc(jdbc, id == 0 ? "Sp_ExImLcOrder_Insert" : "Sp_ExImLcOrder_Update", header);
        if (num > 0) id = num; else num = id;
        for (Map<String, Object> d : details) {
            d.put("ExImLcOrderId", id);
            DesktopProc.scalar(jdbc, "Sp_ExImLcOrderPackingDetail_Insert", d);
        }
        return num;
    }
}
