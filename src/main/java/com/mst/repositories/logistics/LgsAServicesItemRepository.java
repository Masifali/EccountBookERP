package com.mst.repositories.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsAFeedItemPricingSchedule;
import com.mst.models.logistics.LgsAItem;
import com.mst.models.logistics.LgsAItemAllocation;
import com.mst.models.logistics.LgsAUomSchedule;
import com.mst.repositories.hrm.HrmProcRepository;
import org.springframework.stereotype.Repository;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.toDouble;
import static com.mst.services.hrm.HrmSupport.toInt;

/**
 * DAL of 234 "Define Services Item's" (Architecture.WinApp.Service/EximServicesDefine.cs).
 *
 *   ItemCategory.Getall (BLL 0584)          Sp_ItemCategory_GetAllMethod @OrganizationId,@CompanyId,@InventoryParentCategoriesId=9,@Activity='ReadByOrganizationCompanyId'
 *   ItemGroup.GetAll (BLL 0586)             Sp_ItemGroup_GetAllMethod @OrganizationId,@CompanyId,@Activity='ReadAll'
 *   lgstcm LookUps.GetByLookUpTypeId(,,3)   [lgstcm].[USP_LookUps_GetAllMethod] @OrganizationId,@CompanyId,@Id=3,@Activity='GetByLookUpTypeId'
 *   Item.GenerateCode                       Sp_Item_GetAllMethod @OrganizationId,@CompanyId,@ItemCategoryId,@ItemTypeId=0,@Activity='GenerateItemCodeByCategoryId'
 *   Item.GetGLAccountbyItemCategoryId       Sp_Item_GetAllMethod @OrganizationId,@CompanyId,@Id=category,@Activity='GetGLAccountbyItemCategoryId'
 *   Item.FormHistory                        Sp_Item_GetAllMethod ... @Activity='FormHistory'
 *   Item.GetByID                            Sp_Item_GetAllMethod @Id,@Activity='ReadById'
 *   Item.Save -> DAL 0436 Item.SetData      Sp_Item_Insert / Sp_Item_Update, then (insert, id > 0) Sp_UOMSchedule_GetAllMethod
 *                                           'ReadByItemGroupId' + Sp_UOMSchedule_Insert per row, Sp_ItemAllocation_Insert and
 *                                           [fed].[usp_ItemPricingSchedule_Insert] per allocated company.
 */
@Repository
public class LgsAServicesItemRepository {

    private final HrmProcRepository db;

    public LgsAServicesItemRepository(HrmProcRepository db) { this.db = db; }

    public List<Map<String, Object>> categories(UserAccount u) {
        return db.rows("Sp_ItemCategory_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "InventoryParentCategoriesId", 9, "Activity", "ReadByOrganizationCompanyId");
    }

    public List<Map<String, Object>> itemGroups(UserAccount u) {
        return db.rows("Sp_ItemGroup_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll");
    }

    public List<Map<String, Object>> masterItems(UserAccount u) {
        return db.rows("[lgstcm].[USP_LookUps_GetAllMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", 3, "Activity", "GetByLookUpTypeId");
    }

    public List<Map<String, Object>> generateCode(UserAccount u, int categoryId) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemCategoryId", categoryId, "ItemTypeId", 0, "Activity", "GenerateItemCodeByCategoryId");
    }

    public List<Map<String, Object>> categoryGl(UserAccount u, int categoryId) {
        return db.rows("Sp_Item_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", categoryId, "Activity", "GetGLAccountbyItemCategoryId");
    }

    /**
     * Item.FormHistory(ReportsParameters) as HistoryGridFill builds it: @OrganizationId, @CompanyId, @CanViewAllRecord,
     * @EntryUser only when !CanViewAllRecord, @IsTaxable = false (ApprovedFilter is null, and null != "All"),
     * @ParentIds '9', @ScreenName 'EximServicesDefine', @Activity 'FormHistory'. ItemCategoryId / ItemTypeId /
     * NoOfRecords / InventoryParentCategories / MasterItemId / Ids are 0 or null and not sent.
     */
    public List<Map<String, Object>> history(UserAccount u, boolean canViewAll) {
        Map<String, Object> p = new java.util.LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());
        p.put("IsTaxable", Boolean.FALSE);
        p.put("ParentIds", "9");
        p.put("ScreenName", "EximServicesDefine");
        p.put("Activity", "FormHistory");
        return db.rows("Sp_Item_GetAllMethod", p);
    }

    public List<Map<String, Object>> item(int id) {
        return db.rows("Sp_Item_GetAllMethod", "Id", id, "Activity", "ReadById");
    }

    /**
     * BLL Item.Save(items, true) -> DAL Item.SetData(obj, proc, AutoCoaInsertIsOn = true), one transaction.
     * The chart-of-account branch (:36) needs a category whose parent is not 9; this form only offers parent-9
     * categories (and the service refuses any other), so that branch is never reached from here.
     */
    public int save(LgsAItem m, int itemGroupId, List<LgsAItemAllocation> allocations) {
        return db.tx(() -> {
            LocalDateTime now = LocalDateTime.now();
            m.EntryDate = now; m.ModifyDate = now; m.PostDate = now; m.PostState = false;        // BLL Save :19-22
            boolean insert = m.Id == 0;
            int num5 = db.set(insert ? "Sp_Item_Insert" : "Sp_Item_Update", m);                   // DAL :143
            if (num5 > 0) {
                // ApplyGST is false on this form -> no Sp_ItemTaxSchedule_Insert (:146)
                if (itemGroupId > 0) {                                                            // :160-201
                    for (Map<String, Object> g : db.rows("Sp_UOMSchedule_GetAllMethod", "ItemGroupId", itemGroupId, "Activity", "ReadByItemGroupId")) {
                        LgsAUomSchedule s = new LgsAUomSchedule();
                        s.Equivalent = toDouble(g.get("Equivalent"));
                        s.ScheduleUnitId = toInt(g.get("UOMId"));
                        s.QtyEquivalent = toDouble(g.get("QtyEquivalent"));
                        s.ItemId = num5;
                        s.EntryDate = LocalDateTime.now(); s.ModifyDate = LocalDateTime.now();
                        s.EntryUser = m.EntryUser; s.OrganizationId = m.OrganizationId; s.CompanyId = m.CompanyId;
                        s.Active = true;
                        if (m.ItemClassId == 8 && s.Equivalent == 1000.0) s.BaseRateUom = true;
                        else if (s.Equivalent == 40.0) s.BaseRateUom = true;
                        db.set("Sp_UOMSchedule_Insert", s);
                    }
                } else {                                                                          // :203-225 (not reachable: UOM Group is required on insert)
                    LgsAUomSchedule s = new LgsAUomSchedule();
                    s.Equivalent = 0; s.ScheduleUnitId = m.BaseUnitId; s.ItemId = num5;
                    s.EntryDate = LocalDateTime.now(); s.ModifyDate = LocalDateTime.now();
                    s.EntryUser = m.EntryUser; s.OrganizationId = m.OrganizationId; s.CompanyId = m.CompanyId;
                    s.QtyEquivalent = 1.0; s.Active = true;
                    db.set("Sp_UOMSchedule_Insert", s);
                }
                if (allocations == null || allocations.isEmpty()) throw new IllegalStateException("Item Allocation List Empty");   // :226
                for (LgsAItemAllocation a : allocations) {
                    a.ItemId = num5;
                    db.set("Sp_ItemAllocation_Insert", a);
                    // Cost / Purchase / WholeSale / Retail price, ReorderLevel and commission are 0 / false on this form (:234-327).
                    LgsAFeedItemPricingSchedule f = new LgsAFeedItemPricingSchedule();                // :328-345
                    f.ItemId = num5;
                    f.EffectiveDate = LocalDateTime.now().minusDays(7);
                    f.ItemRate = 1.0; f.PricingCustomGroupId = 1; f.IsActive = true; f.DocumentTypeId = 1;
                    f.EntryDate = LocalDateTime.now(); f.ModifyDate = LocalDateTime.now();
                    f.EntryUserId = m.EntryUser; f.ModifyUserId = m.ModifyUser;
                    f.IsApproved = false; f.ApprovedUserId = 0; f.ApprovedDate = LocalDateTime.now(); f.FinancialYearId = 0;
                    f.OrganizationId = a.OrganizationId; f.CompanyId = a.CompanyId;
                    db.set("[fed].[usp_ItemPricingSchedule_Insert]", f);
                }
            } else {
                num5 = m.Id;                                                                      // :348 (Sp_Item_Update returns 0)
            }
            return num5;
        });
    }
}
