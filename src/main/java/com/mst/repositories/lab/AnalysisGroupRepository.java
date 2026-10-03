package com.mst.repositories.lab;

import com.mst.repositories.support.ProcExec;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;

/**
 * Screen 157 "Analysis Group" — desktop Architecture.WinApp.Lab/InvLabAnalysisGroup.cs,
 * BLL 0400_Architecture.BLL.Lab.InvLabAnalysisGroup.cs, DAL 0356, Model 0615.
 *
 * Every statement is one the desktop form issues, with the parameters the BLL sends:
 *
 *   Sp_InventoryItemsOther_GetAllMethod  @Ids, @Activity='InventoryParentCategories'
 *        GroupTypeFill (InvLabAnalysisGroup.cs:299-326) -> Item.InventoryParentCategories
 *        (BLL.Inventory.Item.cs:829-853; @Ids added only when non-empty — here always "1,2,3,4,7,10,11").
 *   Sp_InvLabAnalysisGroup_GetAllMethod  [@Id], @OrganizationId, @CompanyId, @Activity='ReadAll'
 *        gridfill (:168-200) and grdfrm_DoubleClick (:225-252) -> BLL GetAllOrById (0400:51-94).
 *        @Id is added only when != 0 (0400:56); @ParentCategoryId only when != 0 (0400:64) — the form
 *        never sets it, so it is never sent. @Activity has no default in the procedure and is always sent.
 *   Sp_InvLabAnalysisGroup_Insert / _Update
 *        BLL Save (0400:15-31) -> DAL SetDate (0356:10-34) -> GenericProvider.SetProc (0207:283-311),
 *        which sends ONE PARAMETER PER MODEL PROPERTY, named after the property: CompanyId, Id,
 *        OrganizationId, AnalysisGroupCode, AnalysisGroupDescription, GroupType (Model 0615). So the
 *        insert receives @Id=0 too (the procedure overwrites it with Max(Id)+1 and SELECTs it back).
 */
@Repository
public class AnalysisGroupRepository {

    /** InvLabAnalysisGroup.cs:311 — the literal id list the form passes. */
    public static final String GROUP_TYPE_IDS = "1,2,3,4,7,10,11";

    private static final String SQL_GROUP_TYPES =
            "EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Ids=?, @Activity=?";
    private static final String SQL_READ_ALL =
            "EXEC dbo.Sp_InvLabAnalysisGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?";
    private static final String SQL_READ_BY_ID =
            "EXEC dbo.Sp_InvLabAnalysisGroup_GetAllMethod @Id=?, @OrganizationId=?, @CompanyId=?, @Activity=?";
    /* Parameter order = Model property order (0615), as SetProc enumerates them. */
    private static final String SQL_INSERT =
            "EXEC dbo.Sp_InvLabAnalysisGroup_Insert @CompanyId=?, @Id=?, @OrganizationId=?, "
          + "@AnalysisGroupCode=?, @AnalysisGroupDescription=?, @GroupType=?";
    private static final String SQL_UPDATE =
            "EXEC dbo.Sp_InvLabAnalysisGroup_Update @CompanyId=?, @Id=?, @OrganizationId=?, "
          + "@AnalysisGroupCode=?, @AnalysisGroupDescription=?, @GroupType=?";

    private final JdbcTemplate jdbc;

    public AnalysisGroupRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** Columns: Id, InvParentCateDescription. */
    public List<Map<String, Object>> groupTypes() {
        return jdbc.queryForList(SQL_GROUP_TYPES, GROUP_TYPE_IDS, "InventoryParentCategories");
    }

    /** Columns: Id, AnalysisGroupCode, AnalysisGroupDescription, OrganizationId, CompanyId, GroupType, InvParentCateDescription. */
    public List<Map<String, Object>> readAll(int organizationId, int companyId) {
        return jdbc.queryForList(SQL_READ_ALL, organizationId, companyId, "ReadAll");
    }

    public List<Map<String, Object>> readById(int organizationId, int companyId, int id) {
        return jdbc.queryForList(SQL_READ_BY_ID, id, organizationId, companyId, "ReadAll");
    }

    /** Ends in SELECT @Id (Max(Id)+1) — ExecuteScalar on the desktop, ProcExec here. */
    public Integer insert(int companyId, int organizationId, String code, String description, int groupType) {
        return ProcExec.call(jdbc, SQL_INSERT, companyId, 0, organizationId, code, description, groupType);
    }

    /** Returns no result set; the DAL then answers obj.Id (0356:22-25). */
    public void update(int companyId, int id, int organizationId, String code, String description, int groupType) {
        ProcExec.call(jdbc, SQL_UPDATE, companyId, id, organizationId, code, description, groupType);
    }
}
