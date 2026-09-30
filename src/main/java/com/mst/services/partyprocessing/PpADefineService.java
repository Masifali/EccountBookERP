package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpAItemAllocationModel;
import com.mst.models.partyprocessing.PpAItemCategoryModel;
import com.mst.models.partyprocessing.PpAItemModel;
import com.mst.models.partyprocessing.PpAItemTypeModel;
import com.mst.models.partyprocessing.PpAReferencePartyModel;
import com.mst.models.partyprocessing.PpASupplierCustomerModel;
import com.mst.models.partyprocessing.PpAUomScheduleModel;
import com.mst.repositories.partyprocessing.PpARepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpASupport.*;

/**
 * BLL of the Party Processing definition forms of group PpA. Each method names the desktop method it
 * reproduces; validation order and MessageBox wording are the form's.
 *
 *   686 Item Type          PartyProcessing/DefPartyProcessingItemType.cs
 *   685 Item Category      PartyProcessing/DefItemCatagoryPartyProcessing.cs
 *   683 Define Item        PartyProcessing/DefineItemPartyProcessing.cs
 *   689 Reference Parties  WinApp/DefineReferenceParties.cs
 *   684 Stock Party        PartyProcessing/frmPartyProcessingDefineSupplier.cs
 */
@Service
public class PpADefineService {

    public static final int SCREEN_DEFINE_ITEM = 683;
    public static final int SCREEN_STOCK_PARTY = 684;
    public static final int SCREEN_ITEM_CATEGORY = 685;
    public static final int SCREEN_ITEM_TYPE = 686;
    public static final int SCREEN_REFERENCE_PARTIES = 689;

    @Autowired private PpARepository repo;
    @Autowired private PpASupport pp;

    // ================================================================== 686 Item Type (DefPartyProcessingItemType.cs)

    /** InvDeffrmItemType_Load: gridfill(), CombTypeFill() (InvLookUp type 6, BindDDLNew ZeroIndex false, Rows[0] active). */
    public Map<String, Object> itemTypeSetup() {
        UserAccount u = pp.user(SCREEN_ITEM_TYPE);
        return map("rows", itemTypeGrid(u), "types", pick(repo.invLookups(u, 6), "Id", "Id", "LookupName", "LookupName"));
    }

    public List<Map<String, Object>> itemTypeList() { return itemTypeGrid(pp.user(SCREEN_ITEM_TYPE)); }

    /** gridfill(): Id (hidden), Code = TypeCode, Description = TypeDescription, Type = LookupName. */
    private List<Map<String, Object>> itemTypeGrid(UserAccount u) {
        return pick(repo.itemTypes(u), "Id", "Id", "Code", "TypeCode", "Description", "TypeDescription", "Type", "LookupName");
    }

    /** grdfrm_DoubleClick: ItemTypePartyProcessing.GetByID. */
    public Map<String, Object> itemType(int id) {
        UserAccount u = pp.user(SCREEN_ITEM_TYPE);
        Map<String, Object> r = one(repo.itemType(id));
        if (!mine(u, r)) throw invalid("Record not found.");
        return map("Id", r.get("Id"), "TypeCode", str(r.get("TypeCode")), "TypeDescription", str(r.get("TypeDescription")), "LookUpId", toInt(r.get("LookUpId")));
    }

    /** Insert(): formvalidation() then ItemTypePartyProcessing.Save (Insert when Id 0, else Update). */
    public Map<String, Object> saveItemType(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN_ITEM_TYPE);
        int id = Math.max(0, i(b, "id"));
        if (trim(b.get("code")).isEmpty()) throw invalid("Please Insert Code");
        if (trim(b.get("description")).isEmpty()) throw invalid("Please Insert Description");
        int typeId = i(b, "typeId");
        if (typeId == 0 || !offered(repo.invLookups(u, 6), "Id", typeId)) throw invalid("Please select type");
        if (id > 0 && !mine(u, one(repo.itemType(id)))) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        PpAItemTypeModel m = new PpAItemTypeModel();
        m.Id = id;
        m.TypeCode = trim(b.get("code"));
        m.TypeDescription = trim(b.get("description"));
        m.LookUpId = typeId;
        m.EntryUser = u.getId();
        m.ModifyUser = u.getId();
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.EntryDate = now;
        m.ModifyDate = now;
        int n = repo.tx(() -> {
            int r = repo.set(id == 0 ? "Sp_ItemTypePartyProcessing_Insert" : "Sp_ItemTypePartyProcessing_Update", m);
            return r > 0 ? r : id;
        });
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 685 Item Category (DefItemCatagoryPartyProcessing.cs)

    /** InvDeffrmItemCatagory_Load: gridfill(), ItemParentCategoryFill() (BindDDL ZeroIndex false, Rows[0] active). */
    public Map<String, Object> itemCategorySetup() {
        UserAccount u = pp.user(SCREEN_ITEM_CATEGORY);
        return map("rows", itemCategoryGrid(u), "parents", pick(repo.inventoryParentCategories(), "Id", "Id", "InvParentCateDescription", "InvParentCateDescription"));
    }

    public List<Map<String, Object>> itemCategoryList() { return itemCategoryGrid(pp.user(SCREEN_ITEM_CATEGORY)); }

    /** gridfill(): Id (hidden), CategoryCode, CategoryDescription, SerialFrom, SerialTo, CategoryStatus ('Active' / 'InActive'). */
    private List<Map<String, Object>> itemCategoryGrid(UserAccount u) {
        return pick(repo.itemCategories(u), "Id", "Id", "CategoryCode", "CategoryCode", "CategoryDescription", "CategoryDescription",
                "SerialFrom", "SerialFrom", "SerialTo", "SerialTo", "CategoryStatus", "CategoryStatus");
    }

    public Map<String, Object> itemCategory(int id) {
        UserAccount u = pp.user(SCREEN_ITEM_CATEGORY);
        Map<String, Object> r = one(repo.itemCategory(id));
        if (!mine(u, r)) throw invalid("Record not found.");
        return map("Id", r.get("Id"), "CategoryCode", str(r.get("CategoryCode")), "CategoryDescription", str(r.get("CategoryDescription")),
                "SerialFrom", str(r.get("SerialFrom")), "SerialTo", str(r.get("SerialTo")), "ParentCategoriesId", toInt(r.get("ParentCategoriesId")),
                "CategoryStatus", toBool(r.get("CategoryStatus")));
    }

    /** Insert(): formvalidation() in the form's order, then ItemCategoryPartyProcessing.Save. */
    public Map<String, Object> saveItemCategory(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN_ITEM_CATEGORY);
        int id = Math.max(0, i(b, "id"));
        String code = s(b, "code"), desc = s(b, "description"), from = s(b, "serialFrom"), to = s(b, "serialTo");
        if (code.trim().isEmpty() || code.equals("0")) throw invalid("Please Insert Code");
        if (desc.trim().isEmpty() || desc.trim().equals("0")) throw invalid("Please Insert Description");
        if (from.trim().isEmpty() || from.trim().equals("0")) throw invalid("Please Insert Serial From");
        if (to.trim().isEmpty() || to.trim().equals("0")) throw invalid("Please Insert Serial To");
        int parent = i(b, "parentId");
        if (parent == 0 || !offered(repo.inventoryParentCategories(), "Id", parent)) throw invalid("Please Select Parent Category");
        if (id > 0 && !mine(u, one(repo.itemCategory(id)))) throw invalid("Record not found.");
        LocalDateTime now = LocalDateTime.now();
        PpAItemCategoryModel m = new PpAItemCategoryModel();
        m.Id = id;
        m.CategoryCode = code.trim();
        m.CategoryDescription = desc.trim();
        m.SerialFrom = cint(from.trim());
        m.SerialTo = cint(to.trim());
        m.ParentCategoriesId = parent;
        m.CategoryStatus = flag(b, "status");
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.EntryUser = u.getId();
        m.ModifyUser = u.getId();
        m.EntryDate = now;
        m.ModifyDate = now;
        int n = repo.tx(() -> {
            int r = repo.set(id == 0 ? "Sp_ItemCategoryPartyProcessing_Insert" : "Sp_ItemCategoryPartyProcessing_Update", m);
            return r > 0 ? r : id;
        });
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 683 Define Item (DefineItemPartyProcessing.cs)

    /**
     * InvDefrmAddItem_Load: rights of "DefPartProcessingAddItem" (Save / Print / Update / CanView AllRecord),
     * ItemTypeFill, ItemCatagoryHistoryFill, ItemGroupNameFill, BaseUnitFill, CompaniesBindInGrid.
     */
    public Map<String, Object> itemSetup() {
        UserAccount u = pp.user(SCREEN_DEFINE_ITEM);
        Map<String, Object> out = itemCombos(u);
        out.put("rights", pp.rights(u, SCREEN_DEFINE_ITEM));
        out.put("uoms", pick(repo.uoms(u), "Id", "Id", "UOMCode", "UOMCode"));
        out.put("allocation", allocationGrid(u));
        return out;
    }

    /** toolStripButton1_Click (Refresh): ItemTypeFill, ItemCatagoryHistoryFill, ItemGroupNameFill. */
    public Map<String, Object> itemRefresh() { return itemCombos(pp.user(SCREEN_DEFINE_ITEM)); }

    private Map<String, Object> itemCombos(UserAccount u) {
        return map("types", pick(repo.itemTypes(u), "Id", "Id", "TypeDescription", "TypeDescription"),
                "categories", pick(repo.itemCategories(u), "Id", "Id", "CategoryDescription", "CategoryDescription"),
                "groups", pick(repo.itemGroups(u), "GroupId", "GroupId", "ItemGroupName", "ItemGroupName"));
    }

    /** CompaniesBindInGrid(): Id (hidden), Location = CompName, Value = true (check column). refresh() rebinds it. */
    public List<Map<String, Object>> itemAllocation() { return allocationGrid(pp.user(SCREEN_DEFINE_ITEM)); }

    private List<Map<String, Object>> allocationGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.companies(u)) out.add(map("Id", r.get("Id"), "Location", r.get("CompName"), "Value", true));
        return out;
    }

    /** grdfrmfill(NoOfRecords): 50 when the History tab opens, 0 (all) on its New button. */
    public List<Map<String, Object>> itemHistory(int noOfRecords) {
        UserAccount u = pp.user(SCREEN_DEFINE_ITEM);
        return repo.items(u, pp.can(u, SCREEN_DEFINE_ITEM, "CanView AllRecord"), noOfRecords);
    }

    /** cmbItemCategory_Leave: GenerateCode -> ItemCode of row 0, else empty. */
    public Map<String, Object> itemCode(int categoryId) {
        UserAccount u = pp.user(SCREEN_DEFINE_ITEM);
        List<Map<String, Object>> r = repo.itemCode(u, categoryId);
        return map("ItemCode", r.isEmpty() ? "" : str(r.get(0).get("ItemCode")));
    }

    /** grdhistory_DoubleClick: ItemPartyProcessing.GetByID. */
    public Map<String, Object> item(int id) {
        UserAccount u = pp.user(SCREEN_DEFINE_ITEM);
        Map<String, Object> r = one(repo.item(id));
        if (!mine(u, r)) throw invalid("Record not found.");
        return map("Id", r.get("Id"), "ItemCode", str(r.get("ItemCode")), "ItemName", str(r.get("ItemName")),
                "ItemBaseUnitId", toInt(r.get("ItemBaseUnitId")), "UomGroupId", toInt(r.get("UomGroupId")),
                "ItemCategoryId", toInt(r.get("ItemCategoryId")), "ItemTypeId", toInt(r.get("ItemTypeId")), "IsActive", toBool(r.get("IsActive")));
    }

    /**
     * Insert(): FormValidationForAddItem(), the allocation list from grdAllocation (one row -> that company
     * whatever its tick; several -> the ticked ones, none ticked -> "Please select Location first"), then
     * ItemPartyProcessing.Save -> DAL SetData in one transaction: the header; when the procedure returns an id
     * (Insert only - Update returns none) Sp_ItemAllocation_Insert per company and, when a Uom Group is set,
     * Sp_UOMSchedule_Insert per ReadByItemGroupId row for every company.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> saveItem(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN_DEFINE_ITEM);
        int id = Math.max(0, i(b, "id"));
        pp.require(u, SCREEN_DEFINE_ITEM, id > 0 ? "Update" : "Save");
        if (trim(b.get("itemName")).isEmpty()) throw invalid("Item Name Field is Required");
        int baseUnit = i(b, "baseUnitId"), category = i(b, "categoryId"), type = i(b, "typeId"), group = i(b, "groupId");
        if (baseUnit == 0 || !offered(repo.uoms(u), "Id", baseUnit)) throw invalid("Base Unit Field is Required");
        if (category == 0 || !offered(repo.itemCategories(u), "Id", category)) throw invalid("Item Category Field is Required");
        if (type == 0 || !offered(repo.itemTypes(u), "Id", type)) throw invalid("Item Type Field is Required");
        if (group == 0 || !offered(repo.itemGroups(u), "GroupId", group)) throw invalid("Item Group Field is Required");
        if (id > 0 && !mine(u, one(repo.item(id)))) throw invalid("Record not found.");

        List<Map<String, Object>> companies = repo.companies(u);
        List<Map<String, Object>> grid = b.get("allocation") instanceof List ? (List<Map<String, Object>>) b.get("allocation") : new ArrayList<>();
        if (grid.isEmpty()) throw invalid("Grid Record Not Found");
        List<PpAItemAllocationModel> alloc = new ArrayList<>();
        int countFalse = 0;
        for (Map<String, Object> r : grid) {
            int companyId = toInt(r.get("Id"));
            if (!owns(companies, "Id", companyId)) throw invalid("Grid Record Not Found");
            boolean ticked = toBool(r.get("Value"));
            if (grid.size() == 1 || ticked) {
                PpAItemAllocationModel a = new PpAItemAllocationModel();
                a.CompanyId = companyId;
                a.IsActive = true;
                a.BranchId = toInt(u.getBranchesId());
                a.OrganizationId = u.getOrganizationId();
                alloc.add(a);
            } else {
                countFalse++;
            }
        }
        if (grid.size() > 1 && grid.size() == countFalse) throw invalid("Please select Location first");

        LocalDateTime now = LocalDateTime.now();
        PpAItemModel m = new PpAItemModel();
        m.Id = id;
        m.ItemCode = trim(b.get("itemCode"));
        m.ItemName = trim(b.get("itemName"));
        m.ItemBaseUnitId = baseUnit;
        m.ItemCategoryId = category;
        m.ItemTypeId = type;
        m.UomGroupId = group;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.EntryUser = u.getId();
        /* The form never copies chkstatus into IsActive, so the desktop stores every item as inactive (and an
           update deactivates it). The web sends the ticked value - see the port notes. */
        m.IsActive = flag(b, "isActive");
        m.EntryDate = now;
        m.ModifyDate = now;
        m.ItemAllocationlist = alloc;
        int n = repo.tx(() -> {
            int num = repo.set(id == 0 ? "Sp_ItemPartyProcessing_Insert" : "Sp_ItemPartyProcessing_Update", m);
            if (num > 0) {
                if (m.ItemAllocationlist.isEmpty()) throw new IllegalStateException("ItemPartyProcessing Allocation List Empty");
                for (PpAItemAllocationModel a : m.ItemAllocationlist) {
                    a.ItemId = num;
                    repo.set("Sp_ItemAllocation_Insert", a);
                    if (m.UomGroupId <= 0) continue;
                    for (Map<String, Object> s : repo.uomScheduleByGroup(m.UomGroupId)) {
                        PpAUomScheduleModel us = new PpAUomScheduleModel();
                        us.Equivalent = (double) (float) toDouble(s.get("Equivalent"));   // Conversion.ToSingle
                        us.ScheduleUnitId = toInt(s.get("UOMId"));
                        us.ItemId = num;
                        us.EntryDate = LocalDateTime.now();
                        us.ModifyDate = LocalDateTime.now();
                        us.EntryUser = m.EntryUser;
                        us.OrganizationId = a.OrganizationId;
                        us.CompanyId = a.CompanyId;
                        us.Active = true;
                        repo.set("Sp_UOMSchedule_Insert", us);
                    }
                }
                return num;
            }
            return m.Id;
        });
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 689 Reference Parties (DefineReferenceParties.cs)

    /** DefineCropYear_Load: GetERPFeatureById(4), bindGrid(), SupplierNameFill(), TypeNameFill(). */
    public Map<String, Object> referenceSetup() {
        UserAccount u = pp.user(SCREEN_REFERENCE_PARTIES);
        return map("rows", referenceGrid(u), "agents", agents(u), "types", referenceTypes(u));
    }

    /** btnRefresh_Click: SupplierNameFill(), TypeNameFill(). */
    public Map<String, Object> referenceRefresh() {
        UserAccount u = pp.user(SCREEN_REFERENCE_PARTIES);
        return map("agents", agents(u), "types", referenceTypes(u));
    }

    public List<Map<String, Object>> referenceList() { return referenceGrid(pp.user(SCREEN_REFERENCE_PARTIES)); }

    /** SupplierNameFill(): feature 4 -> GetVendorsAndCustomers(2), else SupplierCustomerGetforComboServiceBind; Id / CompanyName. */
    private List<Map<String, Object>> agents(UserAccount u) {
        List<Map<String, Object>> rows = pp.feature(u, 4) ? repo.vendorsAndCustomers(u, 2) : repo.supplierCustomersForCombo(u);
        return pick(rows, "Id", "Id", "CompanyName", "CompanyName");
    }

    /** TypeNameFill(): ReadAllReferencePartyType - ReferencePartyTypeId / ReferencePartyType (with a default row). */
    private List<Map<String, Object>> referenceTypes(UserAccount u) {
        return pick(repo.referencePartyTypes(u), "ReferencePartyTypeId", "ReferencePartyTypeId", "ReferencePartyType", "ReferencePartyType");
    }

    /**
     * bindGrid(): FormHistory; the type filter is only applied when the type combo is disabled (the form opened
     * from another screen with a fixed type) - on this page it is enabled, so every type is listed.
     */
    private List<Map<String, Object>> referenceGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.referencePartyHistory(u, 0)) {
            out.add(map("Id", r.get("Id"), "ReferencePartyTypeId", r.get("ReferencePartyTypeId"), "ReferencePartyType", r.get("ReferencePartyType"),
                    "ReferencePartyName", r.get("ReferencePartyName"), "IsActive", toBool(r.get("IsActive")), "OrganizationId", r.get("OrganizationId"),
                    "CompanyId", r.get("CompanyId"), "SupplierCustomerId", r.get("SupplierCustomerId"), "SupplierCustomer", r.get("CompanyName")));
        }
        return out;
    }

    /** grdcropyear_DoubleClick: ReferenceParties.GetById. */
    public Map<String, Object> referenceParty(int id) {
        UserAccount u = pp.user(SCREEN_REFERENCE_PARTIES);
        Map<String, Object> r = one(repo.referenceParty(id));
        if (!mine(u, r)) throw invalid("Record not found.");
        return map("Id", r.get("Id"), "ReferencePartyName", str(r.get("ReferencePartyName")), "ReferencePartyTypeId", toInt(r.get("ReferencePartyTypeId")),
                "SupplierCustomerId", toInt(r.get("SupplierCustomerId")), "IsActive", toBool(r.get("IsActive")));
    }

    /** Insert(): FormValidation() ("PartyName Field Required"), then ReferenceParties.save. */
    public Map<String, Object> saveReferenceParty(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN_REFERENCE_PARTIES);
        int id = Math.max(0, i(b, "id"));
        if (trim(b.get("name")).isEmpty()) throw invalid("PartyName Field Required");
        int typeId = i(b, "typeId"), agentId = i(b, "agentId");
        if (!offered(referenceTypes(u), "ReferencePartyTypeId", typeId)) throw invalid("Record not found.");
        if (!offered(agents(u), "Id", agentId)) throw invalid("Record not found.");
        if (id > 0 && !mine(u, one(repo.referenceParty(id)))) throw invalid("Record not found.");
        PpAReferencePartyModel m = new PpAReferencePartyModel();
        m.Id = id;
        m.ReferencePartyName = trim(b.get("name"));
        m.IsActive = flag(b, "isActive");
        m.OrganizationId = u.getOrganizationId();
        m.ReferencePartyTypeId = typeId;
        m.SupplierCustomerId = agentId;
        m.CompanyId = u.getCompanyId();
        int n = repo.tx(() -> {
            int r = repo.set(id == 0 ? "Sp_ReferenceParties_Insert" : "Sp_ReferenceParties_Update", m);
            return r == 0 ? id : r;
        });
        return saved(n, id > 0 ? "Record Update Successfully..." : "Record Save Successfully...");
    }

    // ================================================================== 684 Stock Party (frmPartyProcessingDefineSupplier.cs)

    /** supfrmDefineSupplier1_Load: rights, cmbglacfill, cmbcountryfill, CityFill, datagridviewform. */
    public Map<String, Object> stockPartySetup() {
        UserAccount u = pp.user(SCREEN_STOCK_PARTY);
        return map("rights", pp.rights(u, SCREEN_STOCK_PARTY),
                "glAccounts", pick(repo.coaForCombo(u), "Id", "Id", "AccountTitle", "AccountTitle"),
                "countries", pick(repo.countries(u), "Id", "Id", "Description", "Description"),
                "cities", pick(repo.cities(u), "Id", "Id", "CityName", "CityName"),
                "rows", stockPartyGrid(u));
    }

    public List<Map<String, Object>> stockPartyList() { return stockPartyGrid(pp.user(SCREEN_STOCK_PARTY)); }

    /** datagridviewform(): Id (hidden), SupplierName, GlAccount, MobilePersonal, CountryName, CityName, CNIC, Address1. */
    private List<Map<String, Object>> stockPartyGrid(UserAccount u) {
        return pick(repo.stockPartyHistory(u), "Id", "Id", "SupplierName", "CompanyName", "GlAccount", "GlAccount", "MobilePersonal", "MobilePersonal",
                "CountryName", "CountryName", "CityName", "CityName", "CNIC", "CNIC", "Address1", "Address1");
    }

    /**
     * cmbglac_Leave: SupplierCustomer.Getall scanned for rows whose GlAccountId is the picked account; each match is
     * read (ReadById), so the form ends on the LAST match in update mode. No match -> nothing changes.
     */
    public Map<String, Object> stockPartyByGl(int glAccountId) {
        UserAccount u = pp.user(SCREEN_STOCK_PARTY);
        int found = 0;
        for (Map<String, Object> r : repo.supplierCustomersAll(u)) {
            if (toInt(r.get("GlAccountId")) == glAccountId) found = toInt(r.get("Id"));
        }
        if (found == 0) return map("found", false);
        Map<String, Object> rec = stockPartyRecord(u, found);
        rec.put("found", true);
        return rec;
    }

    /** ReadById(ID). */
    public Map<String, Object> stockParty(int id) {
        return stockPartyRecord(pp.user(SCREEN_STOCK_PARTY), id);
    }

    private Map<String, Object> stockPartyRecord(UserAccount u, int id) {
        Map<String, Object> r = one(repo.supplierCustomer(id));
        if (!mine(u, r)) throw invalid("Id Not Found");
        return map("Id", r.get("Id"), "GlAccountId", toInt(r.get("GlAccountId")), "CompanyName", str(r.get("CompanyName")),
                "CityId", toInt(r.get("CityId")), "MobilePersonal", str(r.get("MobilePersonal")), "CountryId", toInt(r.get("CountryId")),
                "CNIC", str(r.get("CNIC")), "Address1", str(r.get("Address1")), "Status", toBool(r.get("Status")),
                "PictureURL", str(r.get("PictureURL")));
    }

    /**
     * Insert(): formvalidation() in the form's order (the Active Status check only while Save is the visible,
     * enabled button), then SupplierCustomer.Save with exactly the fields the form sets (CustomerGroupId 12,
     * ReportingTitle = CompanyName, Branch / Project = session branch, Entry / Modify / Post user = user,
     * dates = now, CNIC_EXPIRY_DATE = now by the BLL); DAL: Insert / Update and
     * USP_SupplierCustomerTaxSchedule_SyncFromMapping in one transaction.
     */
    public Map<String, Object> saveStockParty(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN_STOCK_PARTY);
        int id = Math.max(0, i(b, "id"));
        pp.require(u, SCREEN_STOCK_PARTY, id > 0 ? "Update" : "Save");
        int gl = i(b, "glAccountId"), country = i(b, "countryId"), city = i(b, "cityId");
        if (gl == 0 || !offered(repo.coaForCombo(u), "Id", gl)) throw invalid("Please Select GL Account!!!");
        if (trim(b.get("companyName")).isEmpty()) throw invalid("Please Enter Company Name!!!");
        if (country == 0 || !offered(repo.countries(u), "Id", country)) throw invalid("Please Select Country!!!");
        if (trim(b.get("mobile")).isEmpty()) throw invalid("Please Enter Phone Number");
        if (city == 0 || !offered(repo.cities(u), "Id", city)) throw invalid("Please Select City!!!");
        if (id == 0 && !flag(b, "status")) throw invalid("Please check the Active Status!!!");
        String picture = "";
        Map<String, Object> old = null;
        if (id > 0) {
            old = one(repo.supplierCustomer(id));
            if (!mine(u, old)) throw invalid("Id Not Found");
            picture = str(old.get("PictureURL"));
        }
        LocalDateTime now = LocalDateTime.now();
        PpASupplierCustomerModel m = new PpASupplierCustomerModel();
        if (old != null) keepUnshown(m, old);
        m.Id = id;
        m.CompanyName = trim(b.get("companyName"));
        m.ReportingTitle = trim(b.get("companyName"));
        m.GlAccountId = gl;
        m.CNIC = trim(b.get("cnic"));
        m.Address1 = trim(b.get("address"));
        m.Status = flag(b, "status");
        m.CountryId = country;
        m.CityId = city;
        m.MobilePersonal = trim(b.get("mobile"));
        m.PictureURL = picture;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        m.BranchId = toInt(u.getBranchesId());
        m.ProjectId = toInt(u.getBranchesId());
        m.EntryUser = u.getId();
        m.ModifyUser = u.getId();
        m.PostUser = u.getId();
        m.EntryDate = now;
        m.ModifyDate = now;
        m.PostDate = now;
        if (m.CNIC_EXPIRY_DATE == null) m.CNIC_EXPIRY_DATE = now;                     // BLL: CheckDateTimeNull -> now
        m.CustomerGroupId = 12;
        int n = repo.tx(() -> {
            int r = repo.set(id == 0 ? "Sp_SupplierCustomer_Insert" : "Sp_SupplierCustomer_Update", m);
            if (r == 0) r = id;
            repo.syncTaxSchedule(u);
            return r;
        });
        return saved(n, id == 0 ? "Data Save Successfully" : "Data Update Successfully");
    }

    /**
     * The desktop builds a NEW SupplierCustomer on Update, so Sp_SupplierCustomer_Update overwrites every column the
     * form does not hold (codes, NTN / STRN, party type, taxable flag, credit limit ...) with null / 0 - and the Gl
     * Account's Leave can load ANY party of that account, not only a stock party. That silently wipes the party's other
     * data, so on Update the web carries the stored values of those columns (ReadById) instead; the fields the form
     * sets are then set as the form sets them.
     */
    private static void keepUnshown(PpASupplierCustomerModel m, Map<String, Object> o) {
        m.IsDebitCredit = toBool(o.get("IsDebitCredit"));
        m.IsSubSupCust = toBool(o.get("IsSubSupCust"));
        m.PostState = toBool(o.get("PostState"));
        m.CNIC_EXPIRY_DATE = toDate(o.get("CNIC_EXPIRY_DATE"));
        m.CreditLimit = toDouble(o.get("CreditLimit"));
        m.DebitCreditAmount = toDouble(o.get("DebitCreditAmount"));
        m.ActionId = toInt(o.get("ActionId"));
        m.AdvanceGlAcId = toInt(o.get("AdvanceGlAcId"));
        m.ParentsSupCustId = toInt(o.get("ParentsSupCustId"));
        m.DiscountPolicyId = toInt(o.get("DiscountPolicyId"));
        m.ProfileGroupId = toInt(o.get("ProfileGroupId"));
        m.StateProvinceId = toInt(o.get("StateProvinceId"));
        m.CustomerTypeId = toInt(o.get("CustomerTypeId"));
        m.PartyTypeId = toInt(o.get("PartyTypeId"));
        m.BusinessTypeId = toInt(o.get("BusinessTypeId"));
        m.PartyTypePrefix = nz(o.get("PartyTypePrefix"));
        m.Address2 = nz(o.get("Address2"));
        m.Email = nz(o.get("Email"));
        m.FirstName = nz(o.get("FirstName"));
        m.FTN_No = nz(o.get("FTN_No"));
        m.LastName = nz(o.get("LastName"));
        m.MobileOffice = nz(o.get("MobileOffice"));
        m.NTN_No = nz(o.get("NTN_No"));
        m.Phone = nz(o.get("Phone"));
        m.NickName = nz(o.get("NickName"));
        m.STRN_No = nz(o.get("STRN_No"));
        m.SupCustCode = nz(o.get("SupCustCode"));
        m.Title = nz(o.get("Title"));
        m.Town = nz(o.get("Town"));
        m.WebPage = nz(o.get("WebPage"));
        m.ZipCode = nz(o.get("ZipCode"));
        m.ManualPartyCode = nz(o.get("ManualPartyCode"));
        m.WhatsAppNo = nz(o.get("WhatsAppNo"));
        m.CompanyIndividuals = nz(o.get("CompanyIndividuals"));
        m.IsTaxable = toBool(o.get("IsTaxable"));
    }

    /**
     * GeneralReprots.SupplierCustomerRegister row check before a print: grid Print (293, SupplierCustomerId = row id)
     * or 292-Print (CityId / CountryId when != 0, GroupId 12). Empty -> "Record Not Found For Display".
     */
    public Map<String, Object> stockPartyPrintCheck(int supplierCustomerId, int cityId, int countryId, boolean register) {
        UserAccount u = pp.user(SCREEN_STOCK_PARTY);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (!register && supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (register) p.put("GroupId", 12);
        if (register && countryId != 0) p.put("CountryId", countryId);
        if (register && cityId != 0) p.put("CityId", cityId);
        p.put("IsTaxable", false);
        if (repo.supplierCustomerRegister(p).isEmpty()) throw invalid("Record Not Found For Display");
        return map("rows", 1);
    }
}
