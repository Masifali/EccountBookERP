package com.mst.services.logistics;

import com.mst.models.UserAccount;
import com.mst.models.logistics.LgsAItem;
import com.mst.models.logistics.LgsAItemAllocation;
import com.mst.repositories.logistics.LgsALookupRepository;
import com.mst.repositories.logistics.LgsAServicesItemRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of 234 "Define Services Item's" - Architecture.WinApp.Service/EximServicesDefine.cs (ScreenName
 * "EximServicesDefine"). Each method names the form method it reproduces; validation order and texts are
 * the form's (FormHelper.ValidateControls: a combo -> "&lt;name&gt; field is required").
 */
@Service
public class LgsAServicesItemService {

    public static final int SCREEN = 234;

    @Autowired private LgsAServicesItemRepository repo;
    @Autowired private LgsALookupRepository look;
    @Autowired private HrmSupport hrm;

    /**
     * InitializeComponentMethod: rights (btnsave = Save, btnupdate = Update), CatagoryBind (cmbItemCategory and
     * cmbitemcathistory, default row "0"), AccountlstDtFillFromGlobal (types 20, 21), ItemGroupNameBind (GroupId ->
     * Id, ItemGroupName -> GroupName), CompaniesBindInGrid (Id, Location = CompName, Value = true), MasterItemBind.
     */
    public Map<String, Object> setup() {
        UserAccount u = hrm.user(SCREEN);
        Map<String, Object> r = lists(u);
        r.put("rights", rights(u));
        r.put("allocation", allocationRows(u));
        return r;
    }

    /** toolStripButton1_Click (Refresh): every list re-bound, selections kept by the page. */
    public Map<String, Object> refresh() { return lists(hrm.user(SCREEN)); }

    private Map<String, Object> rights(UserAccount u) {
        Map<String, Object> r = hrm.rights(u, SCREEN);
        r.put("canViewAllRecord", hrm.can(u, SCREEN, "CanView AllRecord"));
        return r;
    }

    private Map<String, Object> lists(UserAccount u) {
        List<Map<String, Object>> cats = new ArrayList<>();
        for (Map<String, Object> c : repo.categories(u)) cats.add(map("Id", toInt(c.get("Id")), "CategoryDescription", str(c.get("CategoryDescription"))));
        List<Map<String, Object>> groups = new ArrayList<>();
        for (Map<String, Object> g : repo.itemGroups(u)) groups.add(map("Id", toInt(g.get("GroupId")), "GroupName", str(g.get("ItemGroupName"))));
        List<Map<String, Object>> masters = new ArrayList<>();
        for (Map<String, Object> m : repo.masterItems(u)) masters.add(map("Id", toInt(m.get("Id")), "LookupName", str(m.get("LookupName"))));
        return map("categories", cats, "groups", groups, "masterItems", masters, "accounts", look.accountsByTypes(u, 20, 21));
    }

    /** CompaniesBindInGrid(CommonServices.CompanyServiceBind()): Id (hidden), Location, Value (true). */
    private List<Map<String, Object>> allocationRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> c : look.companies(u)) out.add(map("Id", c.get("Id"), "Location", c.get("CompName"), "Value", true));
        return out;
    }

    /** FormReset -> CompaniesBindInGrid again (all locations ticked). */
    public List<Map<String, Object>> allocation() { return allocationRows(hrm.user(SCREEN)); }

    /**
     * cmbItemCategory_Leave: Item.GenerateCode -> txtItemCode = rows[0].ItemCode; Item.GetGLAccountbyItemCategoryId ->
     * cmbPurchaseGL = rows[0].InventoryAccountId. Any failure -> "Record Not Found" (the form's catch).
     */
    public Map<String, Object> categoryLeave(int categoryId) {
        UserAccount u = hrm.user(SCREEN);
        try {
            Map<String, Object> out = map("found", false);
            List<Map<String, Object>> code = repo.generateCode(u, categoryId);
            if (!code.isEmpty()) out.put("ItemCode", str(code.get(0).get("ItemCode")));
            List<Map<String, Object>> gl = repo.categoryGl(u, categoryId);
            if (!gl.isEmpty()) out.put("InventoryAccountId", toInt(gl.get(0).get("InventoryAccountId")));
            return out;
        } catch (RuntimeException e) {
            throw invalid("Record Not Found");
        }
    }

    /**
     * HistoryGridFill: Item.FormHistory -> Id, Category, ServiceCode, ServiceName, GlAccount ("Services GL A/C"),
     * MasterItem (hidden, grouped), Active, EntryUser, EntryDate (ToShortDateString), ModifyUser, ModifyDate, NoOfAttachments.
     * reports.Id = cmbitemcathistory.Value is set by the form but FormHistory never sends it (desktop defect: the
     * history category filter has no effect) - reproduced, the category is not sent.
     */
    public List<Map<String, Object>> history() {
        UserAccount u = hrm.user(SCREEN);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, hrm.can(u, SCREEN, "CanView AllRecord"))) {
            out.add(map("Id", toInt(r.get("Id")), "Category", str(r.get("CategoryDescription")), "ServiceCode", str(r.get("ItemCode")),
                    "ServiceName", str(r.get("ItemName")), "GlAccount", str(r.get("GlStockAccountTitle")), "MasterItem", str(r.get("ServicesMasterItem")),
                    "Active", toBool(r.get("ItemStatus")), "EntryUser", str(r.get("EntryUserName")), "EntryDate", r.get("EntryDate"),
                    "ModifyUser", str(r.get("ModifyUserName")), "ModifyDate", r.get("ModifyDate"), "NoOfAttachments", toInt(r.get("NoOfAttachments"))));
        }
        return out;
    }

    /** grdhistory_DoubleClick: "You don't have right to update" without the Update right, then ReadById(Id): Item.GetByID. */
    public Map<String, Object> read(int id) {
        UserAccount u = hrm.user(SCREEN);
        if (!hrm.can(u, SCREEN, "Update")) throw invalid("You don't have right to update");
        List<Map<String, Object>> rows = repo.item(id);
        if (rows.isEmpty()) throw invalid("Record Not Found");
        Map<String, Object> r = rows.get(0);
        if (toInt(r.get("OrganizationId")) != u.getOrganizationId() || toInt(r.get("CompanyId")) != u.getCompanyId()) throw invalid("Record Not Found");
        return map("Id", toInt(r.get("Id")), "ItemCategoryId", toInt(r.get("ItemCategoryId")), "ItemCode", str(r.get("ItemCode")),
                "ItemName", str(r.get("ItemName")), "PurchaseGLAC", toInt(r.get("PurchaseGLAC")), "ItemStatus", toBool(r.get("ItemStatus")),
                "ServicesMasterItemId", toInt(r.get("ServicesMasterItemId")));
    }

    /**
     * Insert(): the checks in the form's order, then Item.Save(items, true). body: id, itemCategoryId, itemCode, itemName,
     * purchaseGl, itemGroupId, itemStatus, masterItemId, allocation [{Id, Value}].
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = hrm.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        hrm.require(u, SCREEN, recId == 0 ? "Save" : "Update");
        List<Map<String, Object>> grid = b.get("allocation") instanceof List ? (List<Map<String, Object>>) b.get("allocation") : new ArrayList<>();
        if (grid.isEmpty()) throw invalid("Company Grid Record Not Found");
        int category = toInt(b.get("itemCategoryId"));
        if (category == 0 || !owns(repo.categories(u), "Id", category)) throw invalid("Services Category field is required");
        String itemName = trim(b.get("itemName"));
        if (itemName.isEmpty()) throw invalid("Services Name field is required");
        int gl = toInt(b.get("purchaseGl"));
        if (gl == 0 || !owns(look.accountsByTypes(u, 20, 21), "Id", gl)) throw invalid("Services GL A/C field is required");
        int group = toInt(b.get("itemGroupId"));
        if (recId == 0 && (group == 0 || !owns(repo.itemGroups(u), "GroupId", group))) throw invalid("UOM Group field is required");
        int master = toInt(b.get("masterItemId"));
        if (master != 0 && !owns(repo.masterItems(u), "Id", master)) master = 0;
        if (recId > 0) {                                                         // an id of this company only
            List<Map<String, Object>> rows = repo.item(recId);
            if (rows.isEmpty() || toInt(rows.get(0).get("OrganizationId")) != u.getOrganizationId()
                    || toInt(rows.get(0).get("CompanyId")) != u.getCompanyId()) throw invalid("Record not update because RecId not found");
        }

        LgsAItem m = new LgsAItem();
        m.Id = recId;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchesId = toInt(u.getBranchesId());
        m.EntryUser = u.getId();
        m.ModifyUser = u.getId();
        m.ItemCategoryId = category;
        m.ItemClassId = 6;
        m.ItemCode = trim(b.get("itemCode"));
        m.ItemName = itemName;
        m.HSCode = "0";
        m.PackQty = 0.0;
        m.ItemStatus = toBool(b.get("itemStatus"));
        m.PurchaseGLAC = gl;
        m.SaleGLAC = gl;
        m.COGSGLAC = gl;
        m.ServicesMasterItemId = master;

        List<LgsAItemAllocation> alloc = new ArrayList<>();
        if (recId == 0) {
            Set<Integer> companies = new HashSet<>();
            for (Map<String, Object> c : look.companies(u)) companies.add(toInt(c.get("Id")));
            int inactive = 0;
            for (Map<String, Object> row : grid) {
                boolean active = toBool(row.get("Value"));
                int companyId = toInt(row.get("Id"));
                if (grid.size() == 1 || active) {
                    if (!companies.contains(companyId)) throw invalid("Please select Location first");
                    LgsAItemAllocation a = new LgsAItemAllocation();
                    a.CompanyId = companyId;
                    a.IsActive = true;
                    a.BranchId = toInt(u.getBranchesId());
                    a.OrganizationId = u.getOrganizationId();
                    alloc.add(a);
                } else inactive++;
            }
            if (grid.size() > 1 && inactive == grid.size()) throw invalid("Please select Location first");
        }
        int id = repo.save(m, recId == 0 ? group : 0, alloc);
        return saved(id, recId == 0 ? "Save Successfully" : "Update Successfully");
    }
}
