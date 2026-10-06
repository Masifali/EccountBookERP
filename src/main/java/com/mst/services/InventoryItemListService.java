package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryItemListRequest;
import com.mst.repositories.InventoryItemListRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.web.server.ResponseStatusException;

/** Screen 171 "Item List" - desktop Architecture.WinApp.Inventory_Reports.frmRptItemList. */
@Service
public class InventoryItemListService {
    private static final int SCREEN_ID = 171;
    private final InventoryItemListRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public InventoryItemListService(InventoryItemListRepository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository; this.context = context; this.rights = rights;
    }

    private UserAccount user() {
        UserAccount user = context.requireAccountingUser();
        rights.require(user, SCREEN_ID, "View");
        return user;
    }

    /** frmRptItemList_Load: ItemClass(), ItemTypes(), ItemCategory(), PurchaseGlAccount(), SaleGLAccount(), CGSGLAccount(). */
    public Map<String,Object> lookups() {
        UserAccount u = user();
        Map<String,Object> result = new LinkedHashMap<>();
        result.put("itemClasses", repository.itemClasses());
        result.put("itemTypes", repository.itemTypes(u));
        result.put("itemCategories", repository.itemCategories(u));
        result.put("glAccounts", repository.glAccounts(u));
        return result;
    }

    /** btnShow_Click / btnnew_Click -> GridBind(): the desktop passes only OrganizationId and CompanyId (no filters). */
    public List<Map<String,Object>> load() {
        UserAccount u = user();
        return repository.itemList(u, null);
    }

    /** btnPrint_Click -> ShowReport(): the same procedure with every chosen filter. */
    public List<Map<String,Object>> printRows(InventoryItemListRequest r) {
        UserAccount u = user();
        if (r == null) r = new InventoryItemListRequest();
        check(r.getItemCategoryId(), repository.itemCategories(u), "Id", "Item Category");
        check(r.getItemTypeId(), repository.itemTypes(u), "Id", "Item Types");
        check(r.getItemClassId(), repository.itemClasses(), "ClassId", "Item Class");
        List<Map<String,Object>> gl = repository.glAccounts(u);
        check(r.getPurchaseGLAC(), gl, "PurchaseGLAC", "Purchase GL Account");
        check(r.getSaleGLAC(), gl, "SaleGLAC", "Sale GL Account");
        check(r.getCogsGLAC(), gl, "COGSGLAC", "CGS GL Account");
        return repository.itemList(u, r);
    }

    /** UltraCombo LimitToList: a chosen value must be one of the combo's own rows. */
    private static void check(Integer id, List<Map<String,Object>> rows, String key, String caption) {
        if (id == null || id == 0) return;
        for (Map<String,Object> row : rows) {
            Object v = row.get(key);
            if (v instanceof Number && ((Number) v).intValue() == id) return;
        }
        throw new ResponseStatusException(HttpStatus.BAD_REQUEST, "Choose a valid " + caption + " from the dropdown");
    }
}
