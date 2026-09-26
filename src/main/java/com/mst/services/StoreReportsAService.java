package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.StoreReportsADto;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.StoreReportsARepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management Reports (module 46), group A. Three report screens, each on its own page:
 *
 * <pre>
 *  id  | caption                       | ScreenName (base.Name)       | route
 *  333 | Store Purchase Register       | StorePurchaseRegister        | /store/reports/store-purchase-register
 *  452 | Store Purchase Demand Report  | StorePurchaseDemandRegister  | /store/reports/store-purchase-demand-register
 *  453 | Store Issuance Return Report  | StoreIssuanceReturnRegister  | /store/reports/store-issuance-return-register
 * </pre>
 * Desktop forms: Architecture.WinApp.Inventory_Reports.{StorePurchaseRegister, StorePurchaseDemandRegister,
 * StoreIssuanceReturnRegister}.cs. Reports have no DocumentTypeId of their own; the documents they
 * list are Purchase Invoice 58/61/64/131 (333), Purchase Demand 141 (452), Store Return 140 (453).
 *
 * BLL/DAL used: 0017 BranchesAllocationToUser.GetBranchsAllocatedToUser; 0581
 * InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice; 0252 InvGsStoreIssuanceHeader.
 * {StorePurchaseRegister, StoreIssuanceReturnRegister, GetItemsForReturntoStore,
 * GetDebitAccountForStoreReturn}; 0557 InvPurchaseDemandHeader.{InvStorePurchaseDemandRegister,
 * GetDataForDropDownFromPurchaseDemand, GetBranchesAllocatedToUserFromStorePurchaseDemadRegister,
 * UpdateStatusandIsApprovedbyId, InvPurchaseDemondSlip}; 0132 InvPurchaseInvoiceReports.{PurchaseInvoice_PM231,
 * PurchaseInvoiceStoreBillWithTax_238, InvPurchaseInvoiceSlipReport220SupReprt}; 0131
 * InvGrnandGdnReports.GrnSlipStore; 0134 PurchaseOrderReports.PurchaseOrderSlipReport201; 0067
 * Department.GetAll; 0223 FixedAssetsRegister.GetAll; 0582 InvWareHouse.Getall; 0141
 * VoucherReports.VoucherValidationReport; DAL 0207 GenericProvider.GetDataTableProc.
 *
 * Source: the three forms were checked for a newer copy under
 * D:\CShapEccorErp\recovered_source\recovery\resolved-source (2026-09-25): that folder is no longer on
 * the device and no other copy of these classes exists anywhere under D:\CShapEccorErp — the
 * ECCOUNTBOOKERP\Architecture.WinApp.Inventory_Reports files (byte-identical to the ones ported,
 * same MD5) are the newest available.
 *
 * None of the three forms reads user rights (no SetRightsValueInRightsObject call): whoever can
 * open the form can use every button on it, including 452's Complete / Cancel. Access to the page
 * is the sidebar Authority's job; the one write (452 status) also checks the screen's View right (D7).
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *  1. 333 GridFill:347-360 — the Branch tick-list is only a gate ("Select Branch First" when it is
 *     empty). The ids are put in obj.BranchesIds, which BLL StorePurchaseRegister never reads (it
 *     sends @BranchesId only from obj.BranchesId, never set) — the register is company-wide
 *     whatever branches are ticked. ALlDropDown also builds the ids and never passes them.
 *  2. 333 GridFill:329 — dt.Rows.Clear() runs BEFORE the branch check, so after a "Select Branch
 *     First" the grid keeps showing the previous result while Print-458 reports "Record Not Found
 *     For Display".
 *  3. 333 Reset (New) clears Item Category / Item Name / Parent Category / Item Type and the grid,
 *     resets the dates, and leaves Supplier, Warehouse, the six number boxes, and the data Print-458
 *     prints untouched.
 *  4. 333 cmbBranchName_Leave:833 — branch text present: the six combos are rebuilt (their
 *     selections reset); no branch ticked: all six are emptied.
 *  5. 452 gridHisory:454 never sets BranchesId, FinancialYearId or JobLotId. BLL :788-797 sends
 *     @BranchesId and @FinancialYearId unconditionally, so the procedure is called with
 *     @BranchesId = 0 and @FinancialYearId = 0, and its WHERE ({@code @BranchesId is null or
 *     PoH.BranchesId = @BranchesId}, same for the year) then matches only rows stored with 0. The
 *     JobLot combo is filled but never filters. The Branch tick-list is filled but never used
 *     (CmbBranchName_Leave exists but is not wired in InitializeComponent).
 *  6. 452 Approval Status starts with no row selected (ComboApprovedfill activates none), so its
 *     text is "" → ApprovedFilter stays null → the BLL sends @IsApproved = 0: by default the report
 *     lists NOT approved demands only. "All" is the only choice that omits @IsApproved.
 *  7. 452 Complete / Cancel columns (and an editable StatusRemarks cell) exist only when the grid
 *     was filled with Approval Status "Approved". Complete acts only if the combo still reads
 *     "Approved" at click time; Cancel does not check. Cancel refuses a row whose ReceivedQty
 *     (the grid value) is above zero and then runs reset() — which clears four combos and
 *     re-runs the report. Order of checks: ReceivedQty, "StatusRemarks Required", confirm.
 *  8. 452 New (reset) clears Item / Parent Category / Item Category / Item Type and RE-RUNS the
 *     report; it does not clear the grid.
 *  9. 452 "This Month" uses DateTime.UtcNow for the first day of the month (cmbperemeter_ValueChanged:339).
 * 10. 452 print_Click prints dtGrid — the LAST fill's rows, including an empty one; any failure
 *     shows "Record Not Found".
 * 11. 453 the Account Title combo is filled once at load (and on Refresh) with ItemId 0 — the
 *     item combo has no change handler — so it lists every (Id, AccountTitle, ItemId) row, one
 *     account repeated per item.
 * 12. 453 an empty result clears the grid and says "Record Not found For Display"; dt is the
 *     empty result, so the Print list then says "Record Not Found For Display".
 * 13. 453 Reset (New) clears the grid but not dt — Print still prints the previous result.
 * 14. 453 the row Print re-runs the register with only @Id (no dates) and prints 457.
 *
 * DEVIATIONS (web only, with reason)
 *  D1. Crystal layouts (458, 456, 457, 454, 231, 238, 233, 237, 212, 201, 118) are not rendered
 *      here: each endpoint returns the procedure's rows, which the page prints as a table. For 201,
 *      whose contract is hand-registered in ReportRegistry (po-201), the page first asks the shared
 *      Crystal endpoint for the real PDF and falls back to the rows. 212 is NOT sent there: the
 *      registry's grn-212 omits @DocumentTypeId, and Sp_InvGrn_StoreSlip_Rpt filters
 *      "Grh.DocumentTypeId = @DocumentTypeId", so it would always come back empty; the slip
 *      endpoint sends 48 as CommonServices.GrnSlipReport212 does.
 *  D2. 453's Print drop-down lists the .rpt files found in a folder on the desktop machine
 *      (CommonServices.DynamicReportsLoad("StoreIssuancReturn")); a browser cannot list that
 *      folder, so the page has one Print button that prints dt's rows.
 *  D3. 452 Complete / Cancel: the demand header must be the user's organization's and company's
 *      Purchase Demand (DocumentTypeId 141) before the status procedure runs (tenancy guard; the
 *      procedure itself also filters by organization and company). Cancel's ReceivedQty is the
 *      grid row's value as on the desktop, but never lower than the GRN quantity re-derived
 *      server-side for that detail row (the procedure's own #TmpRecivings sum), so a posted value
 *      cannot bypass the "Received Qty greater than zero" refusal.
 *  D4. Dates are sent as midnight: all three procedures compare on the DATE part only
 *      (333 @DateFrom date; 452 CONVERT(date,…); 453 @FromDate DATE), so the picker's time of
 *      day cannot change a result.
 *  D5. Date values leave as local "yyyy-MM-dd HH:mm:ss" text (never a UTC JSON timestamp) and
 *      binary columns (CompLogoImage) are dropped from the JSON.
 *  D6. Branch ids posted by the page are matched against the user's own allocation before they
 *      count (333 gate).
 *  D7. 452 Complete / Cancel require the StorePurchaseDemandRegister "View" right (StoreScreenRights,
 *      the grant that shows the form in the desktop menu). The desktop form itself checks nothing.
 *  D8. NoOfAttachments links (333 / 452 / 453 LinkClicked → GetNoofAttachmentsByRefDocumentTypeID)
 *      list the document's DMS attachments with download links; the procedure has no company
 *      filter, so rows of another organization/company are dropped, and the document type must be
 *      one the screen lists (333: 58/61/64/131, 452: 141, 453: 140).
 */
@Service
public class StoreReportsAService {

    private static final Logger LOG = LoggerFactory.getLogger(StoreReportsAService.class);

    public static final String SCREEN_STORE_PURCHASE_REGISTER = "StorePurchaseRegister";
    public static final String SCREEN_STORE_PURCHASE_DEMAND_REGISTER = "StorePurchaseDemandRegister";
    public static final String SCREEN_STORE_ISSUANCE_RETURN_REGISTER = "StoreIssuanceReturnRegister";

    /** The only document type 452 reads (BranchesFill:246, AllDropDownBind:178). */
    private static final int PURCHASE_DEMAND_DOCUMENT_TYPE_ID = 141;
    /** Store Return — the only document type 453 lists (StoreReturnService.DOCUMENT_TYPE_ID). */
    private static final int STORE_RETURN_DOCUMENT_TYPE_ID = 140;

    private final StoreReportsARepository repo;
    private final StoreIssuanceRepository common;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;
    private final DesktopAttachmentStore attachmentStore;

    public StoreReportsAService(StoreReportsARepository repo, StoreIssuanceRepository common, CurrentUserContext ctx,
                                StoreScreenRights rights, DesktopAttachmentStore attachmentStore) {
        this.repo = repo;
        this.common = common;
        this.ctx = ctx;
        this.rights = rights;
        this.attachmentStore = attachmentStore;
    }

    // =========================================================================================
    // 333 Store Purchase Register
    // =========================================================================================

    /** frmGatePassReport_Load:142 — dates, BranchesFill, ALlDropDown; plus the grid's number formats. */
    public Map<String, Object> purchaseRegisterLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", yearStart(u));                 // FromDate.Value = ActiveYr.Start_Period (:153)
        out.put("toDate", LocalDate.now().toString());     // ToDate.Value = DateTime.Now (:154)
        out.putAll(purchaseRegisterBranches(u));
        out.put("combos", purchaseRegisterCombos(u));
        out.put("formats", formats(u));
        return out;
    }

    /** btnRefresh_Click:552 — BranchesFill + ALlDropDown. */
    public Map<String, Object> purchaseRegisterRefresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>(purchaseRegisterBranches(u));
        out.put("combos", purchaseRegisterCombos(u));
        return out;
    }

    /** cmbBranchName_Leave:839 → ALlDropDown. */
    public Map<String, Object> purchaseRegisterCombos() {
        return purchaseRegisterCombos(ctx.requireAccountingUser());
    }

    /**
     * BranchesFill:164 — the tick-list, and cmbBranchName.Text = UserAccount.BranchName (:185):
     * the signed-in user's own branch starts ticked.
     */
    private Map<String, Object> purchaseRegisterBranches(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> list = new ArrayList<>();
        for (Map<String, Object> r : repo.branchesAllocatedToUser(u)) {
            list.add(row("BranchId", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        }
        out.put("branches", list);
        out.put("defaultBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        return out;
    }

    /**
     * ALlDropDown:194 — one procedure, rows split by Activity into six lists (Id, name), in the
     * procedure's row order. When the procedure returns nothing the form returns before binding
     * (:236), so the combos keep what they had: {@code bound = false}.
     */
    private Map<String, Object> purchaseRegisterCombos(UserAccount u) {
        List<Map<String, Object>> all = repo.purchaseInvoiceCombos(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bound", !all.isEmpty());
        List<Map<String, Object>> suppliers = new ArrayList<>(), parents = new ArrayList<>(),
                categories = new ArrayList<>(), types = new ArrayList<>(), warehouses = new ArrayList<>(),
                items = new ArrayList<>();
        for (Map<String, Object> r : all) {
            String activity = str(ci(r, "Activity"));
            Map<String, Object> o = row("Id", ci(r, "Id"), "Name", str(ci(r, "ReferenceName")));
            switch (activity) {
                case "Supplier":           suppliers.add(o); break;
                case "ItemParentCategory": parents.add(o); break;
                case "ItemCategory":       categories.add(o); break;
                case "ItemType":           types.add(o); break;
                case "WareHouse":          warehouses.add(o); break;
                case "ItemName":           items.add(o); break;
                default: break;
            }
        }
        out.put("suppliers", suppliers);          // CmbSupplier           "Supplier Name"
        out.put("itemTypes", types);              // CmbItemType           "Item Type"
        out.put("itemCategories", categories);    // CmbItemCategory       "Item Category"
        out.put("parentCategories", parents);     // CmbItemParentCategory "Parent Category"
        out.put("warehouses", warehouses);        // CmbWarehouseName      "Warehouse"
        out.put("items", items);                  // CmbItemName           "Item Name"
        return out;
    }

    /**
     * btnSearch_Click → GridFill:323. The rows go back exactly as the procedure returned them
     * (they are the dt that Print-458 prints); the page builds the desktop's dtHistory columns.
     */
    public List<Map<String, Object>> purchaseRegister(String fromDate, String toDate, int parentCategoryId,
                                                      int itemCategoryId, int itemTypeId, int itemId,
                                                      int grnNoFrom, int grnNoTo, int gpNoFrom, int gpNoTo,
                                                      int poNoFrom, int poNoTo, int supplierId, int warehouseId,
                                                      List<Integer> branchIds) {
        UserAccount u = ctx.requireAccountingUser();
        /* :348 — with no branch in the combo the form focuses it and throws. */
        if (allowedBranches(u, branchIds).isEmpty()) throw new IllegalArgumentException("Select Branch First");
        return plainRows(repo.storePurchaseRegister(u,
                StoreIssuanceService.dateOnly(fromDate), StoreIssuanceService.dateOnly(toDate),
                parentCategoryId, itemCategoryId, itemTypeId, itemId, grnNoFrom, grnNoTo, gpNoFrom, gpNoTo,
                poNoFrom, poNoTo, supplierId, warehouseId));
    }

    /**
     * grdfrm_ColumnButtonClick:493 / grdfrm_LinkClicked:518 — the slips a row opens.
     * kind: print (231), printWithTax (238), grn (212), order (201), invoice (233 when the row's
     * DocumentTypeId is 61, else 237 — CommonServices.PurchaseInvoiceStoreDirectSlip:7450).
     */
    public Map<String, Object> purchaseRegisterSlip(String kind, int id, int documentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        String k = kind == null ? "" : kind;
        switch (k) {
            case "print":
            case "printWithTax": {
                /* PurchaseInvoicePMSlip_231:7341 / PurchaseInvoiceStoreBillWithtax_238:7421 */
                if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
                List<Map<String, Object>> rows = "print".equals(k)
                        ? repo.purchaseInvoicePm231(u, id) : repo.purchaseInvoiceStoreBillWithTax238(u, id);
                if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
                out.put("template", "print".equals(k) ? "231-InvRptPurchaseBillPackingMaterialSlip.rpt"
                                                      : "238-InvPurchaseInvoice_StoreBillWithTax.rpt");
                out.put("rows", plainRows(rows));
                /* Sub-report InvRptPurchaseBillSupplierOthers — its procedure takes only @PihId, so it
                   runs only after the main slip proved the invoice is this company's. */
                out.put("subReport", plainRows(repo.supplierOthersSubReport(id)));
                return out;
            }
            case "grn": {
                if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
                List<Map<String, Object>> rows = repo.grnStoreSlip212(u, id);
                if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
                out.put("template", "212-InvRptGoodsReceiptsNotesStoreSlip.rpt");
                out.put("rows", plainRows(rows));
                return out;
            }
            case "order": {
                if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
                List<Map<String, Object>> rows = repo.purchaseOrderGeneralSlip201(u, id);
                if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
                out.put("template", "201-InvRptPurchaseOrderGeneralSlip.rpt");
                out.put("rows", plainRows(rows));
                return out;
            }
            case "invoice": {
                if (id == 0) throw new IllegalArgumentException("Record Id Not Found");
                boolean withTax = documentTypeId != 61;
                out.put("template", withTax ? "237-InvPurchaseInvoice_StoreBillDirectWithTax.rpt"
                                            : "233-SpInvPurchaseInvoice_StoreBillDirect.rpt");
                out.put("rows", plainRows(repo.purchaseInvoiceStoreDirectSlip(u, id, withTax)));
                return out;
            }
            default:
                throw new IllegalArgumentException("Unknown slip.");
        }
    }

    // =========================================================================================
    // 452 Store Purchase Demand Report
    // =========================================================================================

    /**
     * frmSaleOrderHistory_Load:134 — AllDropDownBind, ComboStatusFill, ComboApprovedfill,
     * ParameterFill, BranchesFill. The status / approval / date-type lists are the form's own
     * literal tables (ComboStatusFill:277, ComboApprovedfill:298, CommonServices.DateType:15857).
     */
    public Map<String, Object> demandRegisterLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("combos", demandRegisterCombos(u));
        out.put("statuses", idNames(1, "Open", 2, "Complete", 3, "Cancel", 4, "All"));
        out.put("defaultStatus", 4);                               // Rows[3].Activate() — "All"
        out.put("approvals", idNames(1, "Approved", 2, "Not Approved", 3, "All"));
        out.put("dateTypes", idNames(1, "This Day", 2, "This Week", 3, "This Month", 4, "This Year", 5, "Financial Year"));
        out.put("financialYearStart", yearStart(u));
        List<Map<String, Object>> branches = new ArrayList<>();
        for (Map<String, Object> r : repo.demandBranches(u, PURCHASE_DEMAND_DOCUMENT_TYPE_ID)) {
            branches.add(row("BranchId", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        }
        out.put("branches", branches);
        out.put("defaultBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        return out;
    }

    /** toolStripButton1_Click:397 (Refresh) → AllDropDownBind. */
    public Map<String, Object> demandRegisterCombos() {
        return demandRegisterCombos(ctx.requireAccountingUser());
    }

    /** AllDropDownBind:161 — rows split by Activity; nothing is rebound when the call returns no row. */
    private Map<String, Object> demandRegisterCombos(UserAccount u) {
        List<Map<String, Object>> all = repo.demandDropDowns(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("bound", !all.isEmpty());
        out.put("parentCategories", activity(all, "ParentCategory"));  // CmbParentCategory "Parent Category"
        out.put("itemCategories", activity(all, "ItemCategories"));     // CmbItemCategory   "Item Category"
        out.put("itemTypes", activity(all, "ItemTypes"));               // CmbItemType       "Item Type"
        out.put("jobLots", activity(all, "JobLot"));                    // CmbJobLotName     "JobLot"
        out.put("items", activity(all, "Item"));                        // CmbItem           "Item Name"
        out.put("departments", activity(all, "Department"));            // CmbDepartment     "Department Name"
        out.put("assets", activity(all, "Asset"));                      // cmbAssetsRef      "Asset Name"
        return out;
    }

    /**
     * btnshow_Click_1 → gridHisory:448.
     *
     * @param statusText   CmbStatus.Text — sent unless empty or "All" (:467).
     * @param approvedText CmbApproved.Text — "Approved" → @IsApproved 1, "Not Approved" or anything
     *                     else except "All" → @IsApproved 0 (IsApproved defaults to false), "All" →
     *                     omitted (:471-482, BLL :131).
     */
    public List<Map<String, Object>> demandRegister(String fromDate, String toDate, int docNoFrom, int docNoTo,
                                                    int parentCategoryId, int itemCategoryId, int itemTypeId,
                                                    int itemId, int departmentId, int assetId,
                                                    String statusText, String approvedText) {
        UserAccount u = ctx.requireAccountingUser();
        String status = statusText == null ? "" : statusText.trim();
        String sendStatus = (!status.isEmpty() && !"All".equals(status)) ? status : null;
        String approved = approvedText == null ? "" : approvedText;
        Boolean isApproved = "All".equals(approved) ? null : Boolean.valueOf("Approved".equals(approved));
        return plainRows(repo.storePurchaseDemandRegister(u,
                StoreIssuanceService.dateOnly(fromDate), StoreIssuanceService.dateOnly(toDate),
                docNoFrom, docNoTo, parentCategoryId, itemCategoryId, itemTypeId, itemId, departmentId,
                assetId, sendStatus, isApproved));
    }

    /** Slip column → CommonServices.PurchaseDemandSlip454:8141. */
    public Map<String, Object> demandSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.purchaseDemandSlip454(u, id);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Dispaly");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("template", "454-InvPurchaseDemanSlip.rpt");
        out.put("rows", plainRows(rows));
        return out;
    }

    /**
     * CompleteStatus:728 / CancelStatus:775 — the checks in the desktop's order, then
     * UpdateStatusandIsApprovedbyId. The page asks "Are you sure to … Status?" before posting.
     */
    public Map<String, Object> demandStatus(StoreReportsADto.DemandStatus in) {
        UserAccount u = ctx.requireAccountingUser();
        /* D7 — the desktop form reads no rights, but only a user whose menu shows the form (the
           screen's "View" right) can reach these buttons; the API enforces the same gate. */
        if (!rights.has(SCREEN_STORE_PURCHASE_DEMAND_REGISTER, "view")) {
            throw new IllegalStateException("You do not have the View right for this screen.");
        }
        if (in == null) throw new IllegalArgumentException("Nothing to update.");
        int id = in.Id == null ? 0 : in.Id;
        int detailId = in.OrderDetailId == null ? 0 : in.OrderDetailId;
        String reqType = in.ReqType == null ? "" : in.ReqType;
        String remarks = in.StatusRemarks == null ? "" : in.StatusRemarks;
        Map<String, Object> out = new LinkedHashMap<>();
        if (!"Complete".equals(reqType) && !"Cancel".equals(reqType)) throw new IllegalArgumentException("Unknown request.");
        /* D3 — tenancy: the demand must be this organization's and company's Purchase Demand (141). */
        if (id == 0 || repo.ownedDemandHeader(u, id).isEmpty()) throw new IllegalArgumentException("Record not found.");
        if ("Complete".equals(reqType)) {
            /* grdPurchaseDemandRegister_ColumnButtonClick:695 — Complete only while the combo reads "Approved". */
            if (!"Approved".equals(in.ApprovedText)) { out.put("done", false); return out; }
        } else {
            /* CancelStatus:786 — the grid row's ReceivedQty; D3: never lower than the stored GRN quantity. */
            double received = in.ReceivedQty == null ? 0d : in.ReceivedQty;
            if (detailId != 0) received = Math.max(received, repo.demandDetailReceivedQty(id, detailId));
            if (received > 0.0) {
                out.put("done", false);
                out.put("reset", true);                                  // :789 reset()
                out.put("message", "Record Not Cancel because Received Qty greater than zero");
                return out;
            }
        }
        if (remarks.isEmpty()) throw new IllegalArgumentException("StatusRemarks Required");
        repo.purchaseDemandStatusUpdate(u, id, reqType, remarks);
        out.put("done", true);
        out.put("message", "Complete".equals(reqType) ? "Record Complete Successfully" : "Record Cancel Successfully");
        return out;
    }

    // =========================================================================================
    // Attachments — the NoOfAttachments link on all three grids
    // =========================================================================================

    /** The document types each screen's NoOfAttachments link can open (333 row type, 452 fixed 141, 453 row type). */
    private static List<Integer> attachmentTypes(String screen) {
        switch (screen == null ? "" : screen) {
            case "store-purchase-register":        return List.of(58, 61, 64, 131);
            case "store-purchase-demand-register": return List.of(PURCHASE_DEMAND_DOCUMENT_TYPE_ID);
            case "store-issuance-return-register": return List.of(STORE_RETURN_DOCUMENT_TYPE_ID);
            default: return List.of();
        }
    }

    /**
     * CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId):4552 — the AttachmentView
     * list (AttachmentName, CustomName, EntryDate). Rows of another organization or company are dropped.
     */
    public List<Map<String, Object>> attachments(String screen, int id, int documentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        if (!attachmentTypes(screen).contains(documentTypeId)) throw new IllegalArgumentException("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        if (id == 0) return out;
        for (Map<String, Object> r : repo.attachmentsByRefDocument(id, documentTypeId)) {
            if (toInt(ci(r, "OrganizationId")) != toInt(u.getOrganizationId())
                    || toInt(ci(r, "CompanyId")) != toInt(u.getCompanyId())) continue;
            out.add(row("Id", toInt(ci(r, "Id")), "AttachmentName", str(ci(r, "Attachment")),
                    "CustomName", str(ci(r, "UploadedFileCustomName")), "EntryDate", plain(ci(r, "EntryDate"))));
        }
        return out;
    }

    public DesktopInventoryItemFileService.Download attachmentDownload(String screen, int id, int documentTypeId, int attachmentId) {
        UserAccount u = ctx.requireAccountingUser();
        for (Map<String, Object> r : attachments(screen, id, documentTypeId)) {
            if (toInt(r.get("Id")) != attachmentId) continue;
            String stored = str(r.get("CustomName"));
            if (stored.isBlank()) stored = str(r.get("AttachmentName"));
            String name = str(r.get("AttachmentName")).isBlank() ? stored : str(r.get("AttachmentName"));
            return new DesktopInventoryItemFileService.Download(basename(name), attachmentStore.read(u, basename(stored)),
                    "application/octet-stream");
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    // =========================================================================================
    // 453 Store Issuance Return Report
    // =========================================================================================

    /** frmGatePassReport_Load:270 — dates and the five combos (ReportsLoad: see D2). */
    public Map<String, Object> returnRegisterLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("fromDate", yearStart(u));              // :276
        out.put("toDate", LocalDate.now().toString());  // :277
        out.putAll(returnRegisterCombos(u));
        out.put("formats", formats(u));
        return out;
    }

    /** toolStripButton1_Click:568 (Refresh) — the five fills again. */
    public Map<String, Object> returnRegisterCombos() {
        return returnRegisterCombos(ctx.requireAccountingUser());
    }

    private Map<String, Object> returnRegisterCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("departments", project(repo.departments(u), "Id", "DepartmentName"));          // "Department"
        out.put("items", project(repo.itemsForReturnToStore(u), "Id", "ItemName"));             // "Item Name"
        out.put("assets", project(repo.fixedAssets(u), "Id", "AssetName"));                     // "Fixed Asset Name"
        out.put("warehouses", project(repo.warehouses(u), "Id", "WareHouseName"));              // "WareHouseName"
        out.put("accounts", project(repo.debitAccountsForStoreReturn(u, 0), "Id", "AccountTitle")); // "Credit Account "
        return out;
    }

    /** btnSearch_Click → GridFill:290. Empty → the grid clears and the form says so (note 12). */
    public Map<String, Object> returnRegister(String fromDate, String toDate, int departmentId, int itemId,
                                              int assetId, int warehouseId, int accountId, int fromDocNo,
                                              int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = plainRows(repo.storeIssuanceReturnRegister(u, 0,
                StoreIssuanceService.dateOnly(fromDate), StoreIssuanceService.dateOnly(toDate),
                fromDocNo, toDocNo, itemId, assetId, warehouseId, departmentId, accountId));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        if (rows.isEmpty()) out.put("message", "Record Not found For Display");
        return out;
    }

    /** Row "Print" (:459) — the register for one document, template 457-StoreIssuanceReturnSlip.rpt. */
    public Map<String, Object> returnSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = id == 0 ? new ArrayList<>()
                : repo.storeIssuanceReturnRegister(u, id, null, null, 0, 0, 0, 0, 0, 0, 0);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("template", "457-StoreIssuanceReturnSlip.rpt");
        out.put("rows", plainRows(rows));
        return out;
    }

    /** Row "Voucher" (:453) → CommonServices.VoucherReport_118:5647. */
    public Map<String, Object> returnVoucher(int voucherHeadId, int documentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        if (voucherHeadId == 0) throw new IllegalArgumentException("VoucherId Not Found");                          // :5656
        if (voucherHeadId < 0) throw new IllegalArgumentException("Record Not Found For Display because VoucherHeadId not found"); // :5684
        List<Map<String, Object>> rows = repo.voucher118(u, voucherHeadId, documentTypeId);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");                     // :5679
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("template", "118-AcRptVoucherSlip.rpt");
        out.put("rows", plainRows(rows));
        return out;
    }

    // =========================================================================================
    // helpers
    // =========================================================================================

    /** clsGlobalVariables.ActiveYr.Start_Period — the active year this session is in. */
    private String yearStart(UserAccount u) {
        int yearId;
        try { yearId = ctx.currentFinancialYearId(); } catch (Exception e) { yearId = 0; }
        try {
            List<Map<String, Object>> years = repo.activeYears(u);
            Map<String, Object> hit = null;
            for (Map<String, Object> r : years) {
                if (toInt(ci(r, "Id")) == yearId) { hit = r; break; }
            }
            if (hit == null && !years.isEmpty()) hit = years.get(0);
            if (hit != null) {
                Object v = plain(ci(hit, "Start_Period"));
                return v == null ? null : String.valueOf(v).substring(0, 10);
            }
        } catch (Exception e) {
            LOG.warn("Could not read the active financial year's start period", e);
        }
        return null;
    }

    /**
     * CommonServices.GetDecimalConfiguration:5376 — stringFormatsingle "#,##0." + N zeros (N 1-4,
     * otherwise none) and DecimalRateFormate "#,#0." + N zeros (0 → two, 1-4 → N, otherwise none).
     */
    private Map<String, Object> formats(UserAccount u) {
        int amountRaw = toInt(common.config(u, "Default NoofDecimal Points For Amount"));
        int rateRaw = toInt(common.config(u, "Default NoofDecimal Points For Rate"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("amountDecimals", amountRaw >= 1 && amountRaw <= 4 ? amountRaw : 0);
        m.put("rateDecimals", rateRaw >= 1 && rateRaw <= 4 ? rateRaw : (rateRaw == 0 ? 2 : 0));
        return m;
    }

    /** Only the ticked ids that are in this user's own allocation (D6). */
    private List<Integer> allowedBranches(UserAccount u, List<Integer> requested) {
        List<Integer> out = new ArrayList<>();
        if (requested == null || requested.isEmpty()) return out;
        List<Integer> allowed = new ArrayList<>();
        for (Map<String, Object> r : repo.branchesAllocatedToUser(u)) allowed.add(toInt(ci(r, "BranchId")));
        for (Integer id : requested) if (id != null && allowed.contains(id)) out.add(id);
        return out;
    }

    private static List<Map<String, Object>> activity(List<Map<String, Object>> all, String activity) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : all) {
            if (activity.equals(str(ci(r, "Activity")))) {
                out.add(row("Id", ci(r, "Id"), "Name", str(ci(r, "ReferenceName"))));
            }
        }
        return out;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(row("Id", ci(r, id), "Name", str(ci(r, name))));
        return out;
    }

    private static List<Map<String, Object>> idNames(Object... kv) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (int i = 0; i + 1 < kv.length; i += 2) out.add(row("Id", kv[i], "Name", kv[i + 1]));
        return out;
    }

    private static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int i = 0; i + 1 < kv.length; i += 2) m.put(String.valueOf(kv[i]), kv[i + 1]);
        return m;
    }

    /** D5 — dates as local text; binary columns dropped. */
    static Object plain(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
        if (v instanceof java.time.OffsetDateTime) return ((java.time.OffsetDateTime) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof byte[]) return null;
        return v;
    }

    static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (rows == null) return out;
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                if (e.getValue() instanceof byte[]) continue;
                o.put(e.getKey(), plain(e.getValue()));
            }
            out.add(o);
        }
        return out;
    }
}
