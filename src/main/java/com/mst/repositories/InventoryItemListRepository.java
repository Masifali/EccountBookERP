package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemListRequest;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Desktop Architecture.WinApp.Inventory_Reports.frmRptItemList (screen 171 "Item List").
 *  - ItemCategory()       -> BLL ItemCategory.Getall -> Sp_ItemCategory_GetAllMethod @OrganizationId, @CompanyId, @Activity='ReadByOrganizationCompanyId'
 *  - ItemTypes()          -> BLL ItemType.Getall     -> Sp_ItemType_GetAllMethod     @OrganizationId, @CompanyId, @Activity='ReadByOrganizationCompanyId'
 *  - ItemClass()          -> BLL ItemClass.GetAll    -> Sp_ItemClass_GetAllMethod    @Activity='ReadAll'
 *  - Purchase/Sale/CGS GL -> BLL Item.GLAccountReferedInItemDifinition -> Sp_Item_GetAllMethod @Activity='GetGLAccountReferedInItemDifinition'
 *  - GridBind / ShowReport -> BLL Item.RptItemList   -> SP_Item_List_Rpt (optional parameters passed only when set)
 */
@Repository
public class InventoryItemListRepository {
    private final JdbcTemplate jdbc;
    public InventoryItemListRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> itemCategories(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_ItemCategory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
    }

    public List<Map<String,Object>> itemTypes(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_ItemType_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
    }

    public List<Map<String,Object>> itemClasses() {
        return jdbc.queryForList("EXEC dbo.Sp_ItemClass_GetAllMethod @Activity=?", "ReadAll");
    }

    public List<Map<String,Object>> glAccounts(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_Item_GetAllMethod @Activity=?, @OrganizationId=?, @CompanyId=?",
                "GetGLAccountReferedInItemDifinition", u.getOrganizationId(), u.getCompanyId());
    }

    /** Item.RptItemList: @OrganizationId, @CompanyId, then each filter only when non-zero / checked, in the BLL's order. */
    public List<Map<String,Object>> itemList(UserAccount u, InventoryItemListRequest r) {
        Map<String,Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (r != null) {
            add(p, "ItemCategoryId", r.getItemCategoryId());
            add(p, "ItemTypeId", r.getItemTypeId());
            add(p, "ItemClassId", r.getItemClassId());
            add(p, "PurchaseGLAC", r.getPurchaseGLAC());
            add(p, "SaleGLAC", r.getSaleGLAC());
            add(p, "COGSGLAC", r.getCogsGLAC());
            if (r.isItemStatus()) p.put("ItemStatus", Boolean.TRUE);
        }
        StringJoiner sql = new StringJoiner(", ", "EXEC dbo.SP_Item_List_Rpt ", "");
        for (String key : p.keySet()) sql.add("@" + key + "=?");
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(), p.values().toArray()));
    }

    private static void add(Map<String,Object> p, String key, Integer value) {
        if (value == null || value == 0) return;
        p.put(key, value);
    }
}
