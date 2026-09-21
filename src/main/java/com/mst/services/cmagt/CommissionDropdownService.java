package com.mst.services.cmagt;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Commission Trading dropdowns - /api/commission/dropdowns/*
 *
 * Every list here was traced from the desktop control back to its stored procedure before a line
 * of this class was written. Nothing is hard-coded and nothing is invented; an endpoint exists
 * only where the desktop source established the contract.
 *
 * CONSUMERS (10 screens):
 *   frmBuyerInquiryBooking       1050  countx_cmagt_buyer_inquiry_booking.js
 *   frmPurchaseOrderCmagt        1052  countx_cmagt_purchase_order.js
 *   frmGoodsDispatchingNoteCmagt       goods_dispatching_note_cmagt.html
 *   frmGrnLoadingChallanCmagt          grn_loading_challan_cmagt.html
 *   frmCommissionAgentTradeBillAgainstGdn  trade_bill_against_gdn.html
 *   + the five CMAGT report screens
 *
 * ---------------------------------------------------------------------------------------------
 * THE PARTY LISTS ARE ONE LIST, NOT THREE
 * ---------------------------------------------------------------------------------------------
 * CommonBindings.SupplierBind (CommonBindings.cs:160-198) binds whichever DataTable it is handed
 * and changes only the DISPLAY MEMBER - "PartyCode" when the form's party-code radio is on,
 * otherwise the caption's column. It applies NO party-type filter. So on the desktop:
 *
 *     Commission Agent, Buyer Name, Supplier Name, Deliver To Party and Transporter
 *     all show THE SAME rows.
 *
 * frmGoodsDispatchingNoteCmagt:694-697, frmGrnLoadingChallanCmagt:780-784 and
 * frmCommissionAgentTradeBillAgainstGdn:1272-1279 each call SupplierBind four or five times
 * against one table. /buyers, /suppliers and /commission-agents therefore return the same rows
 * by design. Inventing a PartyTypeId filter to make them differ would not match the desktop.
 *
 * ---------------------------------------------------------------------------------------------
 * AND THE CMAGT PARTY LIST IS NOT THE PURCHASE ORDER PARTY LIST
 * ---------------------------------------------------------------------------------------------
 * There are ~50 SupplierDtFillFromGlobal implementations in the desktop. The CMAGT one
 * (CommonBindings.cs:118-158) applies NO CustomerGroupId filter. PurchsaeOrder.cs:986 - the one
 * PurchaseOrderFullService was modelled on - DOES drop CustomerGroupId == 7. These endpoints
 * must NOT copy that filter: it belongs to a different form.
 *
 * ---------------------------------------------------------------------------------------------
 * RESPONSE SHAPE - both consumer styles, one payload
 * ---------------------------------------------------------------------------------------------
 * The eight templates read {id, name}; the two LOOKUPS-driven JS files read the desktop column
 * names ({Id, CompanyName, NickName, ...}). Every row carries BOTH: the desktop column names are
 * authoritative and the lower-case pair are aliases onto the same values, so neither consumer had
 * to be rewritten and no field means something different from its desktop original.
 */
@Service
public class CommissionDropdownService {

    private static final Logger LOG = LoggerFactory.getLogger(CommissionDropdownService.class);

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;

    private int org()  { return currentUserContext.currentOrganizationId(); }
    private int comp() { return currentUserContext.currentCompanyId(); }

    // =============================================================== parties

    /**
     * Commission Agent / Buyer / Supplier / Deliver To Party / Transporter - ONE list.
     *
     * clsGlobalVariables.globalAllSupplierCustomer
     *   <- GlobalServicesMethods.getGlobalSupplierCustomer(org, comp, 0, 0, 0, "")
     *   -> USP_GetVendorsAndCustomersWithCityName @OrganizationId, @CompanyId
     *      (@PartyTypeId, @PageSize, @PageNumber, @Keyword are all omitted - the caller passes
     *       0/"" and the BLL drops them. Omitting is not the same as sending NULL.)
     *
     * Then CommonBindings.SupplierDtFillFromGlobal (:118-158) shapes each row:
     *   name = useBusinessName ? CompanyName : NickName
     *   and for a sub-party (IsSubSupCust && ParentsSupCustId > 0 whose parent is in the list)
     *   name = parentName + " / " + name
     *
     * @param useBusinessName GDN and GRN Loading Challan pass true outright
     *                        (frmGoodsDispatchingNoteCmagt:564, frmGrnLoadingChallanCmagt:603);
     *                        1050 and Trade Bill pass their RadBusinessName radio.
     */
    public List<Map<String, Object>> parties(boolean useBusinessName) {
        List<Map<String, Object>> rows;
        try {
            rows = jdbcTemplate.queryForList(
                    "EXEC USP_GetVendorsAndCustomersWithCityName @OrganizationId=?, @CompanyId=?",
                    org(), comp());
        } catch (Exception e) {
            LOG.error("USP_GetVendorsAndCustomersWithCityName failed; every CMAGT party dropdown "
                    + "will be empty", e);
            return new ArrayList<>();
        }

        /* Parent lookup for the sub-party name rule, built before the loop the way the desktop
           builds customerLookup (:125). */
        Map<Integer, Map<String, Object>> byId = new HashMap<>();
        for (Map<String, Object> r : rows) byId.put(asInt(col(r, "Id")), r);

        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            String companyName = str(col(r, "CompanyName"));
            String nickName    = str(col(r, "NickName"));
            String name = useBusinessName ? companyName : nickName;

            if (asBool(col(r, "IsSubSupCust")) && asInt(col(r, "ParentsSupCustId")) > 0) {
                Map<String, Object> parent = byId.get(asInt(col(r, "ParentsSupCustId")));
                if (parent != null) {
                    String pn = useBusinessName ? str(col(parent, "CompanyName"))
                                                : str(col(parent, "NickName"));
                    name = pn + " / " + name;          // :131-134
                }
            }

            Map<String, Object> m = new LinkedHashMap<>();
            /* desktop column names - what the LOOKUPS-driven JS reads */
            m.put("Id",           asInt(col(r, "Id")));
            m.put("CompanyName",  name);               // the COMPOSED name, as the desktop binds
            m.put("NickName",     nickName);
            m.put("PartyCode",    str(col(r, "PartyCode")));
            m.put("GlAccountId",  asInt(col(r, "GlAccountId")));
            m.put("CityId",       asInt(col(r, "CityId")));
            m.put("CityName",     str(col(r, "CityName")));
            m.put("MobileNo",     str(col(r, "MobilePersonal")));
            m.put("PartyTypeId",  asInt(col(r, "PartyTypeId")));
            m.put("CustomerGroupId", asInt(col(r, "CustomerGroupId")));
            /* aliases for the eight templates that read {id, name} */
            m.put("id",   asInt(col(r, "Id")));
            m.put("name", name);
            out.add(m);
        }
        return out;
    }

    // ================================================================= items

    /**
     * Item Name.
     *
     * clsGlobalVariables.getGlobalAllItems
     *   <- GlobalServicesMethods.AllItemsWithModal(org, comp, 0, 0, "")
     *   -> [dbo].[USP_Item_AllItemsWithModal] @OrganizationId, @CompanyId
     *      (@PageSize, @PageNumber, @Keyword omitted)
     *
     * CommonBindings.ItemdtFillFromGlobal (:46-74) and frmGrnLoadingChallanCmagt's own copy
     * (:1038-1084) both read that global and both narrow by ParentCategoryId when one is given -
     * the same optional filter, so it is a query parameter here rather than two endpoints.
     */
    public List<Map<String, Object>> items(int parentCategoryId) {
        List<Map<String, Object>> rows;
        try {
            rows = jdbcTemplate.queryForList(
                    "EXEC [dbo].[USP_Item_AllItemsWithModal] @OrganizationId=?, @CompanyId=?",
                    org(), comp());
        } catch (Exception e) {
            LOG.error("USP_Item_AllItemsWithModal failed; the Item dropdowns will be empty", e);
            return new ArrayList<>();
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            int pc = asInt(col(r, "InventoryParentCategoriesId"));
            if (parentCategoryId != 0 && pc != parentCategoryId) continue;   // :62
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id",       asInt(col(r, "Id")));
            m.put("ItemName", str(col(r, "ItemName")));
            m.put("ItemCode", str(col(r, "ItemCode")));
            m.put("InventoryParentCategoriesId", pc);
            m.put("InvParentCateDescription", str(col(r, "InvParentCateDescription")));
            m.put("ItemCategoryId", asInt(col(r, "ItemCategoryId")));
            m.put("ItemCategory",   str(col(r, "ItemCategory")));
            m.put("id",   asInt(col(r, "Id")));
            m.put("name", str(col(r, "ItemName")));
            out.add(m);
        }
        return out;
    }

    /**
     * Parent Category - the distinct parent categories carried by the same item rows.
     * CommonBindings.ParentCategoryBindFromGlobal (:18-44) builds it from getGlobalAllItems with a
     * HashSet on InvParentCateDescription, so it is derived, never queried separately.
     */
    public List<Map<String, Object>> parentCategories() {
        List<Map<String, Object>> out = new ArrayList<>();
        java.util.Set<String> seen = new java.util.LinkedHashSet<>();
        for (Map<String, Object> r : items(0)) {
            String desc = str(r.get("InvParentCateDescription"));
            if (desc.isEmpty() || !seen.add(desc)) continue;                  // :31-34
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(r.get("InventoryParentCategoriesId")));
            m.put("InvParentCateDescription", desc);
            m.put("id", asInt(r.get("InventoryParentCategoriesId")));
            m.put("name", desc);
            out.add(m);
        }
        return out;
    }

    // ============================================================== the rest

    /** CommonServices.CompanyServiceBind -> Company.GetAlldt(OrgCompanyTypeId = OrganizationId). */
    public List<Map<String, Object>> companies() {
        return simple("EXEC Sp_Company_GetAllMethod @OrganizationId=?, @Activity=?",
                new Object[]{ org(), "ReadAll" }, "Id", "CompName", "companies");
    }

    /**
     * Ship To Address - SupplierCustomerShipToAddress.GetAll_Combo
     *   Sp_SupplierCustomerShipToAddress_GetAllMethod
     *     @OrganizationId, @CompanyId, @Activity='ReadByOrganizationCompanyId_Combo'
     *     @SupplierCustomerId only when > 0 (the buyer cascade,
     *     CommonBindings.BindShipToAddressAgainstBuyer :296)
     */
    public List<Map<String, Object>> shipToAddresses(int supplierCustomerId) {
        List<String> n = new ArrayList<>(); List<Object> a = new ArrayList<>();
        n.add("@OrganizationId"); a.add(org());
        n.add("@CompanyId");      a.add(comp());
        if (supplierCustomerId > 0) { n.add("@SupplierCustomerId"); a.add(supplierCustomerId); }
        n.add("@Activity"); a.add("ReadByOrganizationCompanyId_Combo");
        List<Map<String, Object>> rows = run("Sp_SupplierCustomerShipToAddress_GetAllMethod", n, a,
                                             "ship-to addresses");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(col(r, "Id")));
            m.put("AddressLine1", str(col(r, "AddressLine1")));
            m.put("SupplierCustomerId", asInt(col(r, "SupplierCustomerId")));
            m.put("CompanyName", str(col(r, "CompanyName")));
            m.put("id", asInt(col(r, "Id")));
            m.put("name", str(col(r, "AddressLine1")));
            out.add(m);
        }
        return out;
    }

    /** globalPaymentTerm <- Sp_InvDueTerms_GetAllMethod @OrganizationId, @CompanyId, @Activity='GetAll'. */
    public List<Map<String, Object>> paymentTerms() {
        return simple("EXEC Sp_InvDueTerms_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",
                new Object[]{ org(), comp(), "GetAll" }, "Id", "TermsDescription", "payment terms");
    }

    /** globalDeliveryTermList <- [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity='FormHistory'. */
    public List<Map<String, Object>> deliveryTerms() {
        return simple("EXEC [dbo].[USP_DeliveryTerm_GetAllMethod] @Activity=?",
                new Object[]{ "FormHistory" }, "Id", "Description", "delivery terms");
    }

    /** globalInvPackingType <- [dbo].[Sp_InvPackingType_GetAllMethod] @Activity='ReadAll'. */
    public List<Map<String, Object>> packingTypes() {
        return simple("EXEC [dbo].[Sp_InvPackingType_GetAllMethod] @Activity=?",
                new Object[]{ "ReadAll" }, "Id", "PackTypeDesc", "packing types");
    }

    /** globalCropYear <- [dbo].[Sp_InvCropYear_GetAllMethod] @OrganizationId, @CompanyId, @Activity='ReadAll'. */
    public List<Map<String, Object>> cropYears() {
        return simple("EXEC [dbo].[Sp_InvCropYear_GetAllMethod] @OrganizationId=?, @CompanyId=?, @Activity=?",
                new Object[]{ org(), comp(), "ReadAll" }, "Id", "CropYear", "crop years");
    }

    /**
     * Item UOMs - CommonServices.dtUomFromGloablUomScheduleByItemId filters
     * clsGlobalVariables.globalUomSchedule (<- usp_getAllUomsByCompanyId org, comp, 0, 1) by ItemId.
     *
     * Equivalent is returned exactly as stored. It is NEVER defaulted to 1: a missing or
     * non-positive factor must reach the caller as it is so the screen can refuse, the way the
     * desktop does, rather than quietly computing a wrong weight or amount.
     */
    public List<Map<String, Object>> itemUoms(int itemId) {
        if (itemId <= 0) return new ArrayList<>();
        List<Map<String, Object>> rows;
        try {
            rows = jdbcTemplate.queryForList(
                    "EXEC usp_getAllUomsByCompanyId @OrganizationId=?, @CompanyId=?, @Active=?",
                    org(), comp(), 1);
        } catch (Exception e) {
            LOG.error("usp_getAllUomsByCompanyId failed; UOM dropdowns will be empty", e);
            return new ArrayList<>();
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(col(r, "ItemId")) != itemId) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id",         asInt(col(r, "Id")));
            m.put("UOMCode",    str(col(r, "UOMCode")));
            m.put("Equivalent", col(r, "Equivalent"));      // raw - no substitution
            m.put("BaseRateUom", asBool(col(r, "BaseRateUom")));
            m.put("BasePackUom", asBool(col(r, "BasePackUom")));
            m.put("BaseSecondaryUom", asBool(col(r, "BaseSecondaryUom")));
            m.put("id",   asInt(col(r, "Id")));
            m.put("name", str(col(r, "UOMCode")));
            out.add(m);
        }
        return out;
    }


    // ========================================================== 1052 lookups
    // Traced from frmPurchaseOrderCmagt.cs. Nothing below is a generic "master data" endpoint:
    // each one reproduces one desktop call with its own procedure and its own omitted parameters.

    /**
     * CmbBranch - frmPurchaseOrderCmagt:673 BranchesFill() ->
     * BranchesAllocationToUser.GetBranchsAllocatedToUser(OrganizationId, CompanyId, UserId)
     * (0017_Architecture.BLL.BranchesAllocationToUser.cs:85-105)
     *   -> [dbo].[USP_GetBranchsAllocatedToUser] @OrganizationId, @CompanyId, @UserId
     *
     * All three parameters are unconditional in the BLL. The user id is the SIGNED-IN user taken
     * from the server-side context, never a client-supplied id: this procedure is what limits the
     * screen to the branches allocated to that user, so trusting the caller for it would hand the
     * caller another user's branches.
     *
     * Value member BranchId, display member BranchName (InfragisticsHelper call at :674).
     */
    public List<Map<String, Object>> branches() {
        return simple("EXEC [dbo].[USP_GetBranchsAllocatedToUser] "
                        + "@OrganizationId=?, @CompanyId=?, @UserId=?",
                new Object[]{ org(), comp(), currentUserContext.currentUserId() },
                "BranchId", "BranchName", "branches");
    }

    /**
     * CmbTaxName - NOT a list of all taxes.
     *
     * frmPurchaseOrderCmagt:1202 CmbItemName_Leave -> TaxTypeDbCall(ItemId, datDocDate.Value)
     *   -> CommonServices.GetTaxTypeIdAndPercentByItemId(ItemId, EffectedDate)   (:11168)
     *   -> ItemTaxSchedule.GetTaxScheduleForItemId                               (0592:133)
     *   -> Sp_ItemTaxSchedule_GetAllMethod
     *          @OrganizationId, @CompanyId                       always
     *          @ItemId                                           only when ItemId != 0
     *          @EffectedDate                                     only when the date is not null
     *          @Activity = 'GetItemTaxScheduleForItemId'         always
     *
     * The combo therefore REPOPULATES on every item change and is empty until an item is chosen.
     * Columns are TaxNameId / TaxName / TaxPercent; CmbTaxName_Leave (:1382) reads the percent
     * from SelectedRow.Cells[2], which is TaxPercent.
     *
     * One decompiler artefact to be aware of rather than to copy: TaxTypeDbCall builds a local
     * dtTax with columns Id/TaxType/TaxPrcnt and then returns `dt`, the raw table - so dtTax is
     * dead. The bound table is the raw procedure output, which is what this returns.
     *
     * @param docDate plain yyyy-MM-dd. Blank or unparseable means the date parameter is OMITTED,
     *                exactly as the BLL omits it for a null date - it is never sent as NULL and
     *                never defaulted to today, because a different date selects a different
     *                schedule row and so a different tax percent.
     */
    public List<Map<String, Object>> taxes(int itemId, String docDate) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(org());
        names.add("@CompanyId");      args.add(comp());
        if (itemId != 0) { names.add("@ItemId"); args.add(itemId); }
        if (docDate != null && !docDate.trim().isEmpty()) {
            names.add("@EffectedDate"); args.add(docDate.trim());
        }
        names.add("@Activity"); args.add("GetItemTaxScheduleForItemId");

        List<Map<String, Object>> rows =
                run("Sp_ItemTaxSchedule_GetAllMethod", names, args, "item tax schedule");
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("TaxNameId",  asInt(col(r, "TaxNameId")));
            m.put("TaxName",    str(col(r, "TaxName")));
            m.put("TaxPercent", col(r, "TaxPercent"));   // raw - no rounding, no 0 fallback
            m.put("id",   asInt(col(r, "TaxNameId")));
            m.put("name", str(col(r, "TaxName")));
            out.add(m);
        }
        return out;
    }

    /**
     * CmbCommissionAc and CmbBrokeryAc - these are PARTIES, not chart-of-accounts rows.
     *
     * frmPurchaseOrderCmagt SupplierBind (:897-906) binds ALL FIVE combos to the same dtSupplier:
     *     CommissionAndBuyerBind(CmbCommissionAgent,  dtSupplier, "Commission Agent");
     *     CommissionAndBuyerBind(CmbSupplierName,     dtSupplier, "Supplier Name");
     *     CommissionAndBuyerBind(CmbDeliveryToParty,  dtSupplier, "Deliver To Party");
     *     CommissionAndBuyerBind(CmbCommissionAc,     dtSupplier, "Commission Account");
     *     CommissionAndBuyerBind(CmbBrokeryAc,        dtSupplier, "Brokery Account");
     * and CommissionAndBuyerBind (:908-925) only swaps the display member between PartyCode and
     * CompanyName. dtSupplier is filled at :868-894 from clsGlobalVariables.globalAllSupplierCustomer
     * - the same global list every other party combo uses.
     *
     * So /accounts is /suppliers under the caption the desktop gives it. An earlier note of mine
     * recorded this as an AccountTitle lookup against a chart of accounts; that was wrong, and
     * building it that way would have put account rows where the desktop saves a party id
     * (comm.commissionAgentId = CmbCommissionAc.Value, :3140 and :3151).
     */
    public List<Map<String, Object>> accounts(boolean useBusinessName) {
        return parties(useBusinessName);
    }

    /**
     * grdEmptyBagsPm "EmptyBagItem" value list (:2250, populated from dtItemForEmptyBag).
     *
     * frmPurchaseOrderCmagt:644 -> CommonServices.GetPackingMaterialItemsAllocateToFlow() with NO
     * arguments, so the C# defaults apply: TransactionFlowId = 1, ItemTypeId = 0, ItemCategoryId = 0
     * (CommonServices.cs:1688).
     *   -> PackingMaterialItemsAllocateToTransactionFlow_GetForCombo (0010:101-140)
     *   -> [dbo].[USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo]
     *          @OrganizationId, @CompanyId    always
     *          @TransactionFlowId             only when != 0   -> sent, = 1
     *          @ItemTypeId                    only when != 0   -> OMITTED
     *          @ItemCategoryId                only when != 0   -> OMITTED
     *
     * transactionFlowId is a parameter here because the flow is NOT constant across the CMAGT
     * screens: 1052, GRN Loading Challan (:571) and Supplier Offer (:643) use 1, while the GDN
     * (:532, :2314) uses 2 and Trade Bill fills two tables, one per flow (:929-930). Defaulting
     * every screen to 1 would give the GDN the purchase-side packing items.
     *
     * Value member ItemId, display member ItemName (:2250).
     */
    public List<Map<String, Object>> emptyBagItems(int transactionFlowId, int itemTypeId,
                                                   int itemCategoryId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(org());
        names.add("@CompanyId");      args.add(comp());
        /* The BLL omits each of these when it is zero - it does not send a zero or a NULL. */
        if (transactionFlowId != 0) { names.add("@TransactionFlowId"); args.add(transactionFlowId); }
        if (itemTypeId != 0)        { names.add("@ItemTypeId");        args.add(itemTypeId); }
        if (itemCategoryId != 0)    { names.add("@ItemCategoryId");    args.add(itemCategoryId); }

        List<Map<String, Object>> rows = run(
                "[dbo].[USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo]",
                names, args, "packing material items");
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            m.put("id",   asInt(col(r, "ItemId")));
            m.put("name", str(col(r, "ItemName")));
            out.add(m);
        }
        return out;
    }

    /**
     * grdInvExp "ItemId" value list (:2033, GridEX_Helper.ComboBind against dtOtherItem).
     *
     * frmPurchaseOrderCmagt:840-850 OtherItemsBind -> InventoryItemsOther.GetAll(obj)
     * (0573_Architecture.BLL.Inventory.InventoryItemsOther.cs:53-77)
     *   -> Sp_InventoryItemsOther_GetAllMethod
     *          @Activity = 'ReadAll', @organizationId, @CompanyId     all unconditional
     *
     * @organizationId is spelled lower-case in the BLL and is kept lower-case here.
     * Value member Id, display member OtherItemName (:2033).
     */
    public List<Map<String, Object>> otherItems() {
        return simple("EXEC Sp_InventoryItemsOther_GetAllMethod "
                        + "@Activity=?, @organizationId=?, @CompanyId=?",
                new Object[]{ "ReadAll", org(), comp() },
                "Id", "OtherItemName", "other items");
    }

    /**
     * cmbcommtype / CmbBrokeryType / CmbCommUom / CmbBrokeryRateUom - FOUR combos, ONE call.
     *
     * frmPurchaseOrderCmagt:695 AllComboServices_Views ->
     * purchaseOrderMaster.AllComboServices_FromViews (0490:383-410)
     *   -> [cmagt].[USP_AllComboServices]
     *          @OrganizationId, @CompanyId    always
     *          @Activity                      only when non-empty - the form NEVER sets it,
     *                                         so it is OMITTED and every activity comes back
     *
     * BindViewCombos (:716-762) then splits that one table on its Activity column:
     *     CommissionType        -> cmbcommtype AND CmbBrokeryType   (the same table, twice)
     *     CommissionRateUom     -> CmbCommUom  AND CmbBrokeryRateUom
     *     PaymentBaseDate       -> dtBaseDateType
     *     AllocatedPackingType  -> dtEbGridPackingTypes
     * taking Id and ReferenceName from each row.
     *
     * The rows are returned FLAT with their Activity column intact - one list, exactly the shape
     * of the desktop's single DataTable - and the screen splits them the way BindViewCombos does.
     * Splitting them here into four named lists would have been tidier but would have changed the
     * payload shape the screen already reads, and would have silently dropped any activity the
     * procedure returns that this screen does not yet consume.
     */
    public List<Map<String, Object>> viewCombos(String activity) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@OrganizationId"); args.add(org());
        names.add("@CompanyId");      args.add(comp());
        /* Omitted when blank - the BLL guards on `!= string.Empty && != null`. */
        if (activity != null && !activity.trim().isEmpty()) {
            names.add("@Activity"); args.add(activity.trim());
        }

        List<Map<String, Object>> rows =
                run("[cmagt].[USP_AllComboServices]", names, args, "cmagt view combos");
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Activity",      str(col(r, "Activity")));
            m.put("Id",            asInt(col(r, "Id")));
            m.put("ReferenceName", str(col(r, "ReferenceName")));
            m.put("id",   asInt(col(r, "Id")));
            m.put("name", str(col(r, "ReferenceName")));
            out.add(m);
        }
        return out;
    }

    /**
     * CmbAnalysisGroup - frmBuyerInquiryBooking GetAnalysisGroup(ParentCategoryId) (:884-917),
     * reached from CmbItemName_Leave (:824-845, via the item's InventoryParentCategoriesId) and
     * from cmbParentItem_Leave (:869-882).
     *
     * -> InvLabAnalysisGroup.GetAllOrById (0400:GetAllOrById)
     *   -> Sp_InvLabAnalysisGroup_GetAllMethod
     *          @Id                 only when != 0   -> OMITTED here (the form never sets Id)
     *          @ParentCategoryId   only when != 0
     *          @OrganizationId, @CompanyId, @Activity = 'ReadAll'   always
     *
     * Note the parameter ORDER in the BLL: @Id and @ParentCategoryId are added BEFORE
     * @OrganizationId. Irrelevant for named binding, recorded so the contract reads the same on
     * both sides.
     *
     * A parentCategoryId of 0 omits the filter and returns every group, which is what the desktop
     * does when no item is chosen - it is not a "show nothing" case and must not be turned into one.
     *
     * The desktop reshapes the result at :903-911 into Id / AnalysisGroupDescription /
     * GroupTypeId / GroupType, and while doing so it shifts the last two columns: the int column
     * GroupTypeId is fed dtLAG["GroupType"] and the string column GroupType is fed
     * dtLAG["InvParentCateDescription"]. Only Id and AnalysisGroupDescription reach the combo
     * (value and display member at :912), so the shift never surfaces on screen. The raw
     * procedure columns are returned here under their real names rather than the shifted ones.
     */
    public List<Map<String, Object>> analysisGroups(int parentCategoryId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        if (parentCategoryId != 0) { names.add("@ParentCategoryId"); args.add(parentCategoryId); }
        names.add("@OrganizationId"); args.add(org());
        names.add("@CompanyId");      args.add(comp());
        names.add("@Activity");       args.add("ReadAll");

        List<Map<String, Object>> rows =
                run("Sp_InvLabAnalysisGroup_GetAllMethod", names, args, "analysis groups");
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            m.put("id",   asInt(col(r, "Id")));
            m.put("name", str(col(r, "AnalysisGroupDescription")));
            out.add(m);
        }
        return out;
    }

    /**
     * The quality parameter rows that fill grdParameters when an analysis group is chosen -
     * frmBuyerInquiryBooking BindQualityParamsAgainstAnalysisGroup (:932-988).
     *
     * -> InvLabGroupAnalysisStandards.GetParametersFromGroupStandards (0406)
     *   -> Sp_InvLabGroupAnalysisStandards_GetAllMethod
     *          @Id = the selected analysis group id,  @OrganizationId, @CompanyId,
     *          @Activity = 'GetParametersFromGroupStandards'
     *      - all four unconditional, @Id included, so a zero id IS sent rather than omitted.
     *
     * The form reads InvLabAnalysisItemsId and AnalysisParameterDescription from each row and
     * seeds rangeFrom / rangeTo at 0 (:971-977). Those two zeros are the desktop's own starting
     * values for a NEW row, not data from the procedure, so they belong to the screen and are
     * left to it rather than being manufactured here.
     *
     * De-duplication is the caller's too: the desktop keeps a HashSet of parameter ids already in
     * ParamList and only adds the ones not already there (:969-978), which is what preserves
     * ranges the user has already typed when the group is re-selected. Doing it here would lose
     * that.
     */
    public List<Map<String, Object>> analysisGroupParameters(int analysisGroupId) {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@Id");             args.add(analysisGroupId);
        names.add("@OrganizationId"); args.add(org());
        names.add("@CompanyId");      args.add(comp());
        names.add("@Activity");       args.add("GetParametersFromGroupStandards");

        List<Map<String, Object>> rows = run("Sp_InvLabGroupAnalysisStandards_GetAllMethod",
                names, args, "analysis group parameters");
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>(r);
            m.put("id",   asInt(col(r, "InvLabAnalysisItemsId")));
            m.put("name", str(col(r, "AnalysisParameterDescription")));
            out.add(m);
        }
        return out;
    }

    // ============================================================== plumbing

    private List<Map<String, Object>> simple(String sql, Object[] args, String idCol, String nameCol,
                                             String what) {
        List<Map<String, Object>> rows;
        try {
            rows = jdbcTemplate.queryForList(sql, args);
        } catch (Exception e) {
            LOG.error("{} lookup failed; that dropdown will be empty", what, e);
            return new ArrayList<>();
        }
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put(idCol, asInt(col(r, idCol)));
            m.put(nameCol, str(col(r, nameCol)));
            m.put("id", asInt(col(r, idCol)));
            m.put("name", str(col(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    private List<Map<String, Object>> run(String proc, List<String> names, List<Object> args,
                                          String what) {
        StringBuilder b = new StringBuilder("EXEC ").append(proc).append(' ');
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) b.append(", ");
            b.append(names.get(i)).append("=?");
        }
        try {
            return jdbcTemplate.queryForList(b.toString(), args.toArray());
        } catch (Exception e) {
            LOG.error("{} lookup failed; that dropdown will be empty", what, e);
            return new ArrayList<>();
        }
    }

    private static Object col(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }
    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }
    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return (int) Double.parseDouble(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }
    private static boolean asBool(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
