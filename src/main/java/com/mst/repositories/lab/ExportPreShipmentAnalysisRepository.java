package com.mst.repositories.lab;

import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 168 "Export Pre Shipment Analysis" — desktop Architecture.WinApp.Export/EximPreProductionLab.cs,
 * BLL 0472 (InvLabPreProductionExportLotInspectionHeader) + 0473 (ExImLookUps) + 0583 (Item),
 * DAL 0524, Model 0866.
 *
 * Every statement is one the desktop form issues, with the parameters the BLL sends, in its order. A CLR
 * null / an "added only when" parameter is not sent (DesktopProc omits nulls, as ADO.NET does).
 *
 *   Sp_ExImLookUps_GetAllMethod  @Activity='ReadByOrganizationCompanyIdNExImLookUpTypeId', @OrganizationId,
 *                                @CompanyId, @ExImLookUptypesId
 *        LabInspectionNameFill (form :109) -> BLL 0473 GetByLookTypeId. The form builds
 *        "new ExImLookUps { ExImLookUptypesId = 3 }" and sets nothing else, so @OrganizationId and
 *        @CompanyId are BOTH 0 on the desktop. They are sent as 0 here too (see the service).
 *   Sp_Item_GetAllMethod         @OrganizationId, @CompanyId, @Activity='ReadAllForComboTwoColumns'
 *        ItemNameFill (form :131) -> CommonServices.ItemGetForComboServiceBind() (CommonServices.cs:1550)
 *        -> BLL 0583 Item.GetAllbyCombobind (:138); @InventoryParentCategoriesId only when != 0 (it is 0).
 *   Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod
 *        ReadAll (form :252) -> BLL 0472 Getall(ReportsParameters{OrganizationId, CompanyId}):
 *            @OrganizationId, @CompanyId, @IsApproved (sent because ApprovedFilter is null, i.e. != "All";
 *            value false), @CanViewAllRecord (false), @EntryUser (sent because !CanViewAllRecord; value 0),
 *            @Activity='ReadAll'. @NoOfRecords / @PageNumber / @PageSize are 0 and therefore not sent.
 *        ReadById (form :224) -> BLL 0472 GetByID: @Id, @Activity='ReadById'.
 *   USP_InvLabPreProductionExportLotInspectionHeader_InsertAndUpdate
 *        Insert (form :167) -> BLL 0472 Save -> DAL 0524 SetData -> GenericProvider.SetProc (0207:283): one
 *        parameter per non-virtual model property, in declaration order (Model 0866).
 */
@Repository
public class ExportPreShipmentAnalysisRepository {

    /** EximPreProductionLab.cs:118 — "ExImLookUptypesId = 3". */
    public static final int LAB_LOOKUP_TYPE_ID = 3;

    private static final String P_GETALL = "Sp_InvLabPreProductionExportLotInspectionHeader_GetAllMethod";

    private final JdbcTemplate jdbc;

    public ExportPreShipmentAnalysisRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** Columns: Id, ExImLookUptypesId, LookUpName, ActionId, CompanyId, OrganizationId, LookupTypeName. */
    public List<Map<String, Object>> labs(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "Sp_ExImLookUps_GetAllMethod", params(
                "Activity", "ReadByOrganizationCompanyIdNExImLookUpTypeId",
                "OrganizationId", organizationId, "CompanyId", companyId,
                "ExImLookUptypesId", LAB_LOOKUP_TYPE_ID));
    }

    /** Columns: Id, ItemName, ItemCategory, ItemCode, InventoryParentCategoriesId. */
    public List<Map<String, Object>> items(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", params(
                "OrganizationId", organizationId, "CompanyId", companyId,
                "Activity", "ReadAllForComboTwoColumns"));
    }

    /** ReadAll — the result set's own columns, in its order (the grid does RetrieveStructure on it). */
    public List<Map<String, Object>> readAll(int organizationId, int companyId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", organizationId, "CompanyId", companyId,
                "IsApproved", false, "CanViewAllRecord", false, "EntryUser", 0,
                "Activity", "ReadAll"));
    }

    public List<Map<String, Object>> readById(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
    }

    /** SetProc: ExecuteScalar — the procedure ends in SELECT @Id. The map is the model, in property order. */
    public Integer insertAndUpdate(Map<String, Object> model) {
        return DesktopProc.scalar(jdbc, "USP_InvLabPreProductionExportLotInspectionHeader_InsertAndUpdate", model);
    }
}
