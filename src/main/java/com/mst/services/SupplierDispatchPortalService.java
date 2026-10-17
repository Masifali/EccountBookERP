package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.StockInTransitRepository;
import com.mst.repositories.SupplierDispatchPortalRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Supplier Portal (ModuleId 131) - screen 788 "Supplier Dispatch".
 *
 * <pre>
 * Screen id        788 (dbo.ScreenDefinition; TargetUrl Architecture.WinApp.Purchase.frmSupplierDispatch)
 * ScreenName       frmSupplierDispatch (base.Name; rights key)
 * DocumentTypeId   246   (frmSupplierDispatch.cs :689, :717, :1913, :2053, :2728)
 * Route            /purchase/supplier-dispatch ; API /api/purchase/supplier-dispatch
 * Desktop          Architecture.WinApp.Purchase/frmSupplierDispatch.cs (6,734 lines, designer :3765-6734)
 * BLL / DAL        BLL 0517 / DAL 0383 Inventory.SupplierDispatch (the same classes frmSupplierDispatchPreBill uses,
 *                  with DocumentTypeId 246 instead of 251) - the procedure calls are made through
 *                  {@link StockInTransitRepository}; the calls only this form makes are in {@link SupplierDispatchPortalRepository}.
 * Models           0927 SupplierDispatch, 0926 SupplierDispatchDetail, 0940 SupplierDispatchCommDetail, 0941 SupplierDispatchExpense
 * Print            CommonServices.SupplierDispatchSlip246 -> 246_SupplierDispatchSlip.rpt (USP_SupplierDispatch_SlipAndRegister)
 * </pre>
 *
 * No voucher and no stock posting: SetData (DAL 0383) writes the header, the detail rows, and (the form leaves
 * the lists empty) no commission / expense rows. Numbering, the financial-year check and "DocDate cannot be
 * greater than ServerDate" are inside USP_SupplierDispatch_InsertAndUpdate.
 *
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED
 *  D1  FormHistory sends @EntryUser (BLL 0517) but the procedure filters on @EntryUserId: a user without
 *      "CanView AllRecord" gets no history rows.
 *  D2  Header ProjectId is saved as the user's BranchesId (:1913 area, po.ProjectId = UserAccount.BranchesId).
 *  D3  Update does not check "approved" (only Delete does, :1840).
 *  D4  "Data Save Successfully....  n" shows the Doc No displayed before the save (the procedure recomputes it).
 *  D5  Total Qty / Total Weight are Math.Round(sum, 2) (banker's), Total Amount / Fcy Amount go through the
 *      amount / fcy number formats (away from zero) before they are converted back (CalculateTotalInformation :2412).
 *
 * DEVIATIONS (web only)
 *  W1  Doc No / Branch Sr No: on a new save the server asks the generator, on update it keeps the stored numbers.
 *  W2  Open / update / delete / print only when the record is DocumentTypeId 246 and belongs to the user's
 *      organization and company (ReadById and DeleteById filter by Id alone).
 *  W3  Every posted reference must be in the list the form offers (supplier, reference party, commission agent,
 *      city, currency, item, crop year, packing type, pack/rate UOM of the item, receiver location); a row
 *      with an order line must be a line USP_PurchaseOrder_ForSupplierDispatch offers now for that supplier.
 *  W4  Header totals and the Fcy amounts are recomputed from the rows (the disabled boxes are not trusted).
 *      The rows' own Weight / Amount are the page's values (txtWeightDetail / txtAmountDetail are disabled
 *      boxes the page fills from CalculateWeight / TotalAmount) - the server does not re-derive them.
 *  W5  Vehicle / driver pictures: the browser uploads the picture, the server stores it with the attachment
 *      store (the desktop copied the file to the configured Attachment Folder Path) and saves the stored name.
 */
@Service
public class SupplierDispatchPortalService {
    private static final Logger LOG = LoggerFactory.getLogger(SupplierDispatchPortalService.class);
    public static final String SCREEN_NAME = "frmSupplierDispatch";
    public static final int SCREEN_ID = 788;
    public static final int DOCUMENT_TYPE_ID = 246;
    private static final int FEATURE_SUBSIDIARY = 4;       // GetERPFeatureById(4)  (SubsidiaryAccountAllownOnVouchers)
    private static final int FEATURE_MULTI_CURRENCY = 6;   // GetERPFeatureById(6)
    private static final int FEATURE_BRANCH = 9;           // GetERPFeatureById(9)

    private final StockInTransitRepository repo;
    private final SupplierDispatchPortalRepository own;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final SupplierDispatchAttachmentService attachments;
    private final DesktopAttachmentStore store;

    public SupplierDispatchPortalService(StockInTransitRepository repo, SupplierDispatchPortalRepository own, StoreScreenRights rights,
                                         CurrentUserContext ctx, SupplierDispatchAttachmentService attachments, DesktopAttachmentStore store) {
        this.repo = repo;
        this.own = own;
        this.rights = rights;
        this.ctx = ctx;
        this.attachments = attachments;
        this.store = store;
    }

    /** The attachment list / download of a record: View + the ownership check of load(). */
    public void requireReadable(int id) {
        UserAccount u = requireView();
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
    }

    // ======================================================================== form load

    /** InitializeDataAsync (:475) + InitializeComponentCustom (:584) - everything the form binds on open. */
    public Map<String, Object> lookups() {
        UserAccount u = requireView();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("screenId", SCREEN_ID);
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        out.put("multiCurrency", repo.feature(u, FEATURE_MULTI_CURRENCY));
        out.put("branchFeature", repo.feature(u, FEATURE_BRANCH));
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));                   // DocumentNoFill
        out.put("branchSrNo", repo.generateBranchCode(u, fy, DOCUMENT_TYPE_ID));        // BranchDocumentNoFill
        out.put("decimals", decimals(u));
        putConfigs(u, out);
        putGlobals(u, out);
        putHistoryBranches(u, out);
        return out;
    }

    /** btnRefresh_Click (:2219) - the combos rebound, configurations re-read. */
    public Map<String, Object> formRefresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        putConfigs(u, out);
        putGlobals(u, out);
        return out;
    }

    /** Reset (:2254) - the two generators asked again, ConfigurationDefault re-read. */
    public Map<String, Object> numbers() {
        UserAccount u = requireView();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.generateCode(u, fy, DOCUMENT_TYPE_ID));
        out.put("branchSrNo", repo.generateBranchCode(u, fy, DOCUMENT_TYPE_ID));
        putConfigs(u, out);
        return out;
    }

    /** btnRefreshHistory_Click (:2694) - HistoryBranchComboFill. */
    public Map<String, Object> historyRefresh() {
        UserAccount u = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        putHistoryBranches(u, out);
        return out;
    }

    private void putConfigs(UserAccount u, Map<String, Object> out) {
        out.put("branchImplemented", toBool(repo.config(u, "SupplierDispatchBranchWise")));
        out.put("itemSearchByCode", toBool(repo.config(u, "ItemSearchByCode")));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("defaultCropYear", toInt(repo.config(u, "Default Crop Year")));        // ConfigurationDefault (:651)
        out.put("cityArea", toInt(repo.config(u, "City Area")));
        out.put("baseCurrency", toInt(repo.config(u, "Base Currency")));                // GetBaseCurrencyAndRate (:2390)
        out.put("baseCurrencyRate", toDouble(repo.config(u, "BaseCurrencyRate")));
    }

    private void putGlobals(UserAccount u, Map<String, Object> out) {
        out.put("cities", cities(u));                         // cmbCityFill
        out.put("currencies", currencies(u));                 // CurrencyFill
        out.put("suppliers", suppliers(u));                   // SupplierNameFill
        out.put("referenceParties", referenceParties(u));     // ReferencePartyFill
        out.put("items", items(u));                           // ItemWithoutOrderFill
        out.put("cropYears", cropYears(u));                   // CropYear
        out.put("packingTypes", packingTypes());              // CmbPackingTypeFill
        out.put("receiverLocations", receiverLocations(u));   // BranchDetailFill
    }

    /** HistoryBranchComboFill (:2613): SupplierDispatchBranchWise on -> only the user's own branch. */
    private void putHistoryBranches(UserAccount u, Map<String, Object> out) {
        boolean branchImplemented = toBool(repo.config(u, "SupplierDispatchBranchWise"));
        int userBranch = branch(u);
        String userBranchName = "";
        for (Map<String, Object> b : repo.branchesAllocatedToUser(u)) {
            if (toInt(ci(b, "BranchId")) == userBranch) { userBranchName = str(ci(b, "BranchName")); break; }
        }
        List<Map<String, Object>> list = new ArrayList<>();
        if (branchImplemented) {
            list.add(idName(userBranch, userBranchName));
        } else {
            for (Map<String, Object> b : repo.historyBranches(u)) list.add(idName(toInt(ci(b, "BranchId")), str(ci(b, "BranchName"))));
        }
        out.put("historyBranches", list);
        out.put("userBranchId", userBranch);
        out.put("userBranchName", userBranchName);
    }

    // ============================================================================ lists

    /** cmbCityFill: City.GetAll, Id / CityName. */
    private List<Map<String, Object>> cities(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : own.cities(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CityName"))));
        return out;
    }

    /** CurrencyFill: MultiCurrency.GetAll, Id / CurrencyCode. */
    private List<Map<String, Object>> currencies(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.currencies(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CurrencyCode"))));
        return out;
    }

    /**
     * SupplierNameFill (:776): SubsidiaryAccountAllownOnVouchers (ERP feature 4) -> GetVendorsAndCustomers(1),
     * otherwise SupplierCustomerGetforComboServiceBind; Id, CompanyName, GlAccountId.
     */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        List<Map<String, Object>> rows = repo.feature(u, FEATURE_SUBSIDIARY) ? own.vendorsAndCustomers(u, 1) : own.supplierCustomersForCombo(u);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("CompanyName", str(ci(r, "CompanyName")));
            o.put("GlAccountId", toInt(ci(r, "GlAccountId")));
            out.add(o);
        }
        return out;
    }

    /** ReferencePartyFill: ReferenceParties.GetAll, Id / ReferencePartyName. */
    private List<Map<String, Object>> referenceParties(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.referenceParties(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "ReferencePartyName"))));
        return out;
    }

    /** ItemWithoutOrderFill (:1263): CommonServices.ReadAllItems - Id, ItemName, ItemCodeNew. */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : own.allItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCodeNew")));
            out.add(o);
        }
        return out;
    }

    private List<Map<String, Object>> cropYears(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cropYears(u)) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "CropYear"))));
        return out;
    }

    private List<Map<String, Object>> packingTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.packingTypes()) out.add(idName(toInt(ci(r, "Id")), str(ci(r, "PackTypeDesc"))));
        return out;
    }

    /** BranchDetailFill (:958): BranchesAllocationToUser.GetBranchsAllocatedToUser - BranchId / BranchName. */
    private List<Map<String, Object>> receiverLocations(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.branchesAllocatedToUser(u)) out.add(idName(toInt(ci(r, "BranchId")), str(ci(r, "BranchName"))));
        return out;
    }

    /**
     * bindRateUomAndItemPackUom (:871): CommonServices.GetUomScheduleByItemId(ItemId) - Id, UOMCode, Equivalent
     * (SelectedRow.Cells[2] is the equivalent, :1429 / :2511 / :2584).
     */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = requireView();
        return uomsFor(u, itemId);
    }

    private List<Map<String, Object>> uomsFor(UserAccount u, int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (itemId <= 0) return out;
        for (Map<String, Object> r : own.uomScheduleByItem(u, itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("UOMCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", toDouble(ci(r, "Equivalent")));
            out.add(o);
        }
        return out;
    }

    /**
     * OrderBind (:1218): USP_PurchaseOrder_ForSupplierDispatch for the supplier; @BranchesId only when the
     * Branch feature and SupplierDispatchBranchWise are both on.
     */
    public List<Map<String, Object>> orders(int supplierId) {
        UserAccount u = requireView();
        return plainRows(ordersFor(u, supplierId));
    }

    private List<Map<String, Object>> ordersFor(UserAccount u, int supplierId) {
        if (supplierId <= 0) return new ArrayList<>();
        boolean branchFeature = repo.feature(u, FEATURE_BRANCH);
        boolean branchImplemented = toBool(repo.config(u, "SupplierDispatchBranchWise"));
        Integer branchId = null;
        if (branchFeature && branchImplemented && branch(u) != 0) branchId = branch(u);
        return own.purchaseOrdersForSupplierDispatch(u, ctx.currentFinancialYearId(), supplierId, branchId);
    }

    // ========================================================================== history

    /**
     * gridhistoryfill (:2718). dateType: doc / entry / modify / approved (the four radio buttons);
     * branchIds is the ticked branches. No branch ticked is "Select branch first".
     */
    public List<Map<String, Object>> history(String branchIds, String dateType, boolean fromChecked, String from,
                                             boolean toChecked, String to, String fromDocNo, String toDocNo) {
        UserAccount u = requireView();
        String ids = cleanIds(branchIds);
        if (ids.isEmpty()) throw new IllegalArgumentException("Select branch first");
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Timestamp f = fromChecked && !isBlank(from) ? pickerDate(from) : null;
        Timestamp t = toChecked && !isBlank(to) ? pickerDate(to) : null;
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null, af = null, at = null;
        switch (dateType == null ? "doc" : dateType) {
            case "entry":    ef = f; et = t; break;
            case "modify":   mf = f; mt = t; break;
            case "approved": af = f; at = t; break;
            default:         df = f; dt = t; break;
        }
        /* :2764 BranchIds = BranchIds + "," + id, starting from "" - a leading comma. */
        StringBuilder sb = new StringBuilder();
        for (String id : ids.split(",")) sb.append(',').append(id);
        List<Map<String, Object>> rows = repo.formHistory(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID, viewAll,
                df, dt, cvIntText(fromDocNo), cvIntText(toDocNo), ef, et, mf, mt, af, at, sb.toString());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                                     // dtcol, :2775-2813
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("BranchSrNo", toInt(ci(r, "BranchSrNo")));
            o.put("DocDate", plain(ci(r, "DocDate")));
            o.put("VehicleNo", ci(r, "VehicleNo"));
            o.put("BiltyNo", ci(r, "BiltyNo"));
            o.put("DriverName", ci(r, "DriverName"));
            o.put("Driver1CellNo", ci(r, "Driver1CellNo"));
            o.put("Driver2CellNo", ci(r, "Driver2CellNo"));
            o.put("DepartureDate", plain(ci(r, "DepartureDate")));
            o.put("TransitDays", toInt(ci(r, "TransitDays")));
            o.put("ETADestination", plain(ci(r, "ETAdestination")));
            o.put("SupplierWbWeight", toDouble(ci(r, "SupplierWbWeight")));
            o.put("TotalQty", toDouble(ci(r, "TotalQty")));
            o.put("TotalWeight", toDouble(ci(r, "TotalWeight")));
            o.put("TotalAmount", toDouble(ci(r, "TotalAmount")));
            o.put("FcyAmount", toDouble(ci(r, "FcyAmount")));
            o.put("Freight", toDouble(ci(r, "Freight")));
            o.put("OtherExpense", toDouble(ci(r, "OtherExpense")));
            o.put("CityName", ci(r, "CityName"));
            o.put("RemarksHeader", ci(r, "RemarksHeader"));
            o.put("EntryDate", plain(ci(r, "EntryDate")));
            o.put("EntryUser", ci(r, "EntryUser"));
            o.put("ModifyDate", plain(ci(r, "ModifyDate")));
            o.put("ModifyUser", ci(r, "ModifyUser"));
            o.put("IsApproved", toBool(ci(r, "IsApproved")) ? "Approved" : "Not Approved");
            o.put("ApprovedDate", plain(ci(r, "ApprovedDate")));
            o.put("ApprovedUser", ci(r, "ApprovedUser"));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // =============================================================================== read

    /** ReadById (:2118) / GridDetailBind (:3037) - header and detail rows. Nothing is returned when the record has no detail rows. */
    public Map<String, Object> load(int id) {
        UserAccount u = requireView();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        List<Map<String, Object>> details = repo.details(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocDate", plain(ci(h, "DocDate")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("BranchSrNo", toInt(ci(h, "BranchSrNo")));
        out.put("VehicleNo", str(ci(h, "VehicleNo")));
        out.put("BiltyNo", str(ci(h, "BiltyNo")));
        out.put("DriverName", str(ci(h, "DriverName")));
        out.put("Driver1CellNo", str(ci(h, "Driver1CellNo")));
        out.put("Driver2CellNo", str(ci(h, "Driver2CellNo")));
        out.put("DepartureDate", plain(ci(h, "DepartureDate")));
        out.put("TransitDays", toInt(ci(h, "TransitDays")));
        out.put("ETAdestination", plain(ci(h, "ETAdestination")));
        out.put("Freight", toDouble(ci(h, "Freight")));
        out.put("OtherExpense", toDouble(ci(h, "OtherExpense")));
        out.put("CityId", toInt(ci(h, "CityId")));
        out.put("SupplierWbWeight", toDouble(ci(h, "SupplierWbWeight")));
        out.put("TotalQty", toDouble(ci(h, "TotalQty")));
        out.put("TotalWeight", toDouble(ci(h, "TotalWeight")));
        out.put("TotalAmount", toDouble(ci(h, "TotalAmount")));
        out.put("CurrencyId", toInt(ci(h, "CurrencyId")));
        out.put("ExchangeRate", toDouble(ci(h, "ExchangeRate")));
        out.put("FcyAmount", toDouble(ci(h, "FcyAmount")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("IsApproved", toBool(ci(h, "IsApproved")));
        out.put("HasVehicleImage", imageReadable(u, str(ci(h, "VehicleImage"))));
        out.put("HasDriverImage", imageReadable(u, str(ci(h, "DriverPic"))));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {                                                     // :2186-2188 / :3060
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("SupplierId", toInt(ci(d, "SupplierId")));
            o.put("Supplier", str(ci(d, "SupplierName")));
            o.put("OrderId", toInt(ci(d, "OrderId")));
            o.put("OrderNo", ci(d, "OrderNo") == null ? "" : String.valueOf(toInt(ci(d, "OrderNo"))));
            o.put("CommissionAgentId", toInt(ci(d, "CommissionAgentId")));
            o.put("CommissionAgent", str(ci(d, "CommissionAgentName")));
            o.put("ReferencePartyId", toInt(ci(d, "ReferencePartyId")));
            o.put("ReferenceParty", str(ci(d, "ReferencePartyName")));
            o.put("OrderDetailId", toInt(ci(d, "OrderDetailId")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemCode", str(ci(d, "ItemCode")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("CropYearId", toInt(ci(d, "CropYearId")));
            o.put("CropYear", str(ci(d, "CropYear")));
            o.put("PackingTypeId", toInt(ci(d, "PackingTypeId")));
            o.put("PackingType", str(ci(d, "PackingType")));
            o.put("PackUomId", toInt(ci(d, "PackSizeId")));
            o.put("PackUom", str(ci(d, "PackUomCode")));
            o.put("PackUomEquivalent", toDouble(ci(d, "PackUomEquivalent")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("Weight", toDouble(ci(d, "WeightKgs")));
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", str(ci(d, "RateUomCode")));
            o.put("RateUomEquivalent", toDouble(ci(d, "RateUomEquivalent")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
            o.put("Remarks", str(ci(d, "Remarks")));
            o.put("ReceiverLocationId", toInt(ci(d, "BranchId")));
            o.put("ReceiverLocation", str(ci(d, "BranchName")));
            rows.add(o);
        }
        out.put("rows", rows);
        return out;
    }

    /** The vehicle / driver picture of a record (PicVehicleImage / PicDriverImage), or null when it is not readable. */
    public Map<String, Object> image(int id, String which) {
        UserAccount u = requireView();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        String name = str(ci(h, "driver".equals(which) ? "DriverPic" : "VehicleImage"));
        if (isBlank(name)) return null;
        try {
            String base = baseName(name);
            byte[] bytes = store.read(u, base);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("name", base);
            m.put("bytes", bytes);
            m.put("type", com.mst.services.DesktopInventoryItemFileService.imageType(bytes));
            return m;
        } catch (RuntimeException e) {
            return null;
        }
    }

    private boolean imageReadable(UserAccount u, String name) {
        if (isBlank(name)) return false;
        try { store.read(u, baseName(name)); return true; } catch (RuntimeException e) { return false; }
    }

    // ============================================================================= delete

    /** btnDelete_Click (:1835) -> BLL 0517 DeleteByID (its own transaction). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new AccessDeniedException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> h = ownedHeader(u, id);                                                  // W2
        if (h == null) throw new IllegalArgumentException("Record Not Found");
        if (toBool(ci(h, "IsApproved"))) throw new IllegalArgumentException("Record Not Delete because Record has approved");
        repo.deleteById(u.getId(), id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Delete Record Successfully");
        m.put("id", id);
        return m;
    }

    /** The print buttons: Print right (btnprint.Enabled, :532) and the record's ownership (W2). */
    public Map<String, Object> checkPrint(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new AccessDeniedException("You do not have the Print right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("PrintId not found...");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("id", id);
        m.put("template", "246_SupplierDispatchSlip.rpt");
        return m;
    }

    // =============================================================================== save

    /** btnsave_Click / btnupdate_Click -> Insert() (:1877). */
    @Transactional
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Map<String, Object> dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = in(dto, "Id");
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new AccessDeniedException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);                                                        // W2
            if (existing == null) throw new IllegalArgumentException("Record Not Update because RecId Not Found");
        }
        int fy = ctx.currentFinancialYearId();
        boolean multiCurrency = repo.feature(u, FEATURE_MULTI_CURRENCY);
        boolean branchFeature = repo.feature(u, FEATURE_BRANCH);
        boolean branchImplemented = toBool(repo.config(u, "SupplierDispatchBranchWise"));
        Map<String, Object> dec = decimals(u);
        int amountDec = (Integer) dec.get("amount");
        int fcyDec = (Integer) dec.get("fcy");
        List<Map<String, Object>> rows = listOf(dto, "rows");
        List<Map<String, Object>> removed = listOf(dto, "removed");

        // ------------------------------------------------------------ :1883 + FormValidation (:1321)
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.generateCode(u, fy, DOCUMENT_TYPE_ID);            // W1
        int branchSrNo = existing != null ? toInt(ci(existing, "BranchSrNo")) : repo.generateBranchCode(u, fy, DOCUMENT_TYPE_ID);
        if (docNo == 0) throw new IllegalArgumentException("Doc No Field is Required");
        String vehicleNo = sv(dto, "VehicleNo");
        if (vehicleNo.trim().isEmpty() || vehicleNo.trim().equals("0")) throw new IllegalArgumentException("Vehicle No Field is Required");
        String biltyNo = sv(dto, "BiltyNo");
        if (biltyNo.trim().isEmpty() || biltyNo.trim().equals("0")) throw new IllegalArgumentException("Bilty No Field is Required");
        String driverName = sv(dto, "DriverName");
        if (driverName.trim().isEmpty() || driverName.trim().equals("0")) throw new IllegalArgumentException("Driver Name Field is Required");
        String driverCell1 = sv(dto, "Driver1CellNo");
        if (driverCell1.trim().isEmpty() || driverCell1.trim().equals("0")) throw new IllegalArgumentException("Driver CellNo Field is Required");
        String transitText = sv(dto, "TransitDays").trim();
        if (transitText.isEmpty() || cvIntText(transitText) == 0) throw new IllegalArgumentException("TransitDays Field is Required");
        String freightText = sv(dto, "Freight").trim();
        if (freightText.isEmpty() || cvIntText(freightText) == 0) throw new IllegalArgumentException("Freight Field is Required");
        int cityId = in(dto, "CityId");
        if (cityId == 0 || !contains(cities(u), cityId)) throw new IllegalArgumentException("CityName Field is Required");
        int currencyId = in(dto, "CurrencyId");
        boolean currencyOk = currencyId != 0 && contains(currencies(u), currencyId);
        String exText = sv(dto, "ExchangeRate").trim();
        double exRate = cvDouble(exText);

        // Row FcyAmount and the FcyAmount box as txtExchangeRate_TextChanged + CalculateTotalInformation leave them (:2441 / :2412)
        List<BigDecimal> rowFcy = new ArrayList<>();
        BigDecimal fcySum = BigDecimal.ZERO;
        for (Map<String, Object> r : rows) {
            BigDecimal f = BigDecimal.ZERO;
            BigDecimal ex = BigDecimal.valueOf(exRate);
            if (ex.compareTo(BigDecimal.ZERO) > 0) {
                f = dec(db(r, "Amount")).divide(ex, MathContext.DECIMAL128).setScale(fcyDec, RoundingMode.HALF_EVEN);
            }
            rowFcy.add(f);
            fcySum = fcySum.add(f);
        }
        String fcyText = formatFixed(fcySum, fcyDec);
        if (multiCurrency) {
            if (!currencyOk) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (exText.isEmpty() || exText.equals("0")) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (fcyText.isEmpty() || fcyText.equals("0")) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");
        } else {
            if (!currencyOk) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exText.isEmpty() || exText.equals("0")) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }

        // ------------------------------------------------------------------ the detail rows (:1937-2031)
        List<Map<String, Object>> supplierList = suppliers(u);
        Map<Integer, Map<String, Object>> supplierById = byId(supplierList);
        List<Map<String, Object>> refParties = referenceParties(u);
        Map<Integer, Map<String, Object>> itemById = byId(items(u));
        List<Map<String, Object>> crops = cropYears(u);
        List<Map<String, Object>> packs = packingTypes();
        List<Map<String, Object>> locations = receiverLocations(u);
        Map<Integer, Map<String, Object>> stored = new HashMap<>();
        if (existing != null) for (Map<String, Object> d : repo.details(recId)) stored.put(toInt(ci(d, "Id")), d);
        Map<Integer, List<Map<String, Object>>> orderBySupplier = new HashMap<>();
        Map<Integer, List<Map<String, Object>>> uomByItem = new HashMap<>();
        Set<Integer> postedIds = new HashSet<>();
        double sumAmount = 0d, sumWeight = 0d, sumQty = 0d;
        List<Map<String, Object>> details = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {
            Map<String, Object> r = rows.get(idx);
            int rowNo = idx + 1;
            Map<String, Object> vd = detailModel();
            int rowId = recId == 0 ? 0 : in(r, "Id");
            vd.put("ActionTypeId", rowId > 0 ? 2 : 1);
            vd.put("Id", rowId);
            int supplierId = in(r, "SupplierId");
            if (supplierId == 0) throw new IllegalArgumentException("Supplier Field Required in Grid Row no : " + rowNo);
            if (in(r, "ItemId") == 0) throw new IllegalArgumentException("Item Field Required in Grid Row no : " + rowNo);
            if (in(r, "CropYearId") == 0) throw new IllegalArgumentException("CropYear Field Required in Grid Row no : " + rowNo);
            if (in(r, "PackingTypeId") == 0) throw new IllegalArgumentException("PackingType Field Required in Grid Row no : " + rowNo);
            if (in(r, "PackUomId") == 0) throw new IllegalArgumentException("PackUom Field Required in Grid Row no : " + rowNo);
            if (db(r, "Qty") == 0d) throw new IllegalArgumentException("Qty Field Required in Grid Row no : " + rowNo);
            if (db(r, "Weight") == 0d) throw new IllegalArgumentException("Weight Field Required in Grid Row no : " + rowNo);
            if (in(r, "RateUomId") == 0) throw new IllegalArgumentException("RateUom Field Required in Grid Row no : " + rowNo);
            if (db(r, "Rate") == 0d) throw new IllegalArgumentException("Rate Field Required in Grid Row no : " + rowNo);
            if (db(r, "Amount") == 0d) throw new IllegalArgumentException("Amount Field Required in Grid Row no : " + rowNo);
            if (branchFeature && !branchImplemented && in(r, "ReceiverLocationId") == 0) {
                throw new IllegalArgumentException("ReceiverLocation Field Required in Grid Row no : " + rowNo);
            }
            /* W3 - the posted references must be the form's own lists. */
            if (!supplierById.containsKey(supplierId)) throw new IllegalArgumentException("Supplier Not Found in Grid Row no : " + rowNo);
            int itemId = in(r, "ItemId");
            int refId = in(r, "ReferencePartyId");
            if (refId != 0 && !contains(refParties, refId)) throw new IllegalArgumentException("Reference Party Not Found in Grid Row no : " + rowNo);
            int agentId = in(r, "CommissionAgentId");
            if (agentId != 0 && !supplierById.containsKey(agentId)) throw new IllegalArgumentException("Commission Agent Not Found in Grid Row no : " + rowNo);
            if (!contains(crops, in(r, "CropYearId"))) throw new IllegalArgumentException("CropYear Not Found in Grid Row no : " + rowNo);
            if (!contains(packs, in(r, "PackingTypeId"))) throw new IllegalArgumentException("PackingType Not Found in Grid Row no : " + rowNo);
            List<Map<String, Object>> uoms = uomByItem.computeIfAbsent(itemId, k -> uomsFor(u, k));
            if (!contains(uoms, in(r, "PackUomId"))) throw new IllegalArgumentException("PackUom Not Found in Grid Row no : " + rowNo);
            if (!contains(uoms, in(r, "RateUomId"))) throw new IllegalArgumentException("RateUom Not Found in Grid Row no : " + rowNo);
            if (in(r, "ReceiverLocationId") != 0 && !contains(locations, in(r, "ReceiverLocationId"))) {
                throw new IllegalArgumentException("ReceiverLocation Not Found in Grid Row no : " + rowNo);
            }
            if (in(r, "OrderId") > 0) {
                List<Map<String, Object>> lines = orderBySupplier.computeIfAbsent(supplierId, k -> ordersFor(u, k));
                boolean found = false;
                for (Map<String, Object> l : lines) {
                    if (toInt(ci(l, "OrderId")) == in(r, "OrderId") && toInt(ci(l, "OrderDetailId")) == in(r, "OrderDetailId")
                            && toInt(ci(l, "OrderItemId")) == itemId) { found = true; break; }
                }
                if (!found) {
                    Map<String, Object> s = rowId > 0 ? stored.get(rowId) : null;       // an already saved line may no longer be open
                    boolean same = s != null && toInt(ci(s, "OrderId")) == in(r, "OrderId")
                            && toInt(ci(s, "OrderDetailId")) == in(r, "OrderDetailId") && toInt(ci(s, "ItemId")) == itemId;
                    if (!same) throw new IllegalArgumentException("Purchase Order line is not pending in Grid Row no : " + rowNo);
                }
            } else if (!itemById.containsKey(itemId)) {
                throw new IllegalArgumentException("Item Not Found in Grid Row no : " + rowNo);
            }
            if (rowId > 0) {
                Map<String, Object> s = stored.get(rowId);
                if (s == null || !postedIds.add(rowId)) throw new IllegalArgumentException("Record Not Found in Grid Row no : " + rowNo);
            }
            vd.put("SupplierId", supplierId);
            vd.put("OrderId", in(r, "OrderId"));
            vd.put("CommissionAgentId", agentId);
            vd.put("ReferencePartyId", refId);
            vd.put("OrderDetailId", in(r, "OrderId") > 0 ? in(r, "OrderDetailId") : 0);
            vd.put("ItemId", itemId);
            vd.put("CropYearId", in(r, "CropYearId"));
            vd.put("PackingTypeId", in(r, "PackingTypeId"));
            vd.put("PackSizeId", in(r, "PackUomId"));
            vd.put("Qty", db(r, "Qty"));
            vd.put("WeightKgs", db(r, "Weight"));
            vd.put("RateUomId", in(r, "RateUomId"));
            vd.put("Rate", db(r, "Rate"));
            vd.put("Amount", db(r, "Amount"));
            vd.put("CurrencyId", currencyId);
            vd.put("ExchangeRate", BigDecimal.valueOf(exRate));
            vd.put("FcyAmount", rowFcy.get(idx));
            vd.put("Remarks", sv(r, "Remarks"));
            vd.put("BranchId", in(r, "ReceiverLocationId"));
            sumAmount += db(r, "Amount");
            sumWeight += db(r, "Weight");
            sumQty += db(r, "Qty");
            details.add(vd);
        }
        if (details.isEmpty()) throw new IllegalArgumentException("At least enter value/quantity in one of the rows");
        /* lstRemoveRecord rows (DeleteDetailRow :1583) are appended after the grid rows, ActionTypeId 3, only on an update. */
        if (existing != null) {
            Set<Integer> seen = new HashSet<>();
            for (Map<String, Object> rm : removed) {
                int rid = in(rm, "Id");
                Map<String, Object> s = stored.get(rid);
                if (rid <= 0 || s == null || postedIds.contains(rid) || !seen.add(rid)) throw new IllegalArgumentException("Record Not Found");
                Map<String, Object> vd = detailModel();
                vd.put("ActionTypeId", 3);
                vd.put("Id", rid);
                vd.put("SupplierId", toInt(ci(s, "SupplierId")));
                vd.put("OrderId", toInt(ci(s, "OrderId")));
                vd.put("CommissionAgentId", toInt(ci(s, "CommissionAgentId")));
                vd.put("ReferencePartyId", toInt(ci(s, "ReferencePartyId")));
                vd.put("OrderDetailId", toInt(ci(s, "OrderDetailId")));
                vd.put("ItemId", toInt(ci(s, "ItemId")));
                vd.put("CropYearId", toInt(ci(s, "CropYearId")));
                vd.put("PackingTypeId", toInt(ci(s, "PackingTypeId")));
                vd.put("PackSizeId", toInt(ci(s, "PackSizeId")));
                vd.put("Qty", toDouble(ci(s, "Qty")));
                vd.put("WeightKgs", toDouble(ci(s, "WeightKgs")));
                vd.put("RateUomId", toInt(ci(s, "RateUomId")));
                vd.put("Rate", toDouble(ci(s, "Rate")));
                vd.put("Amount", toDouble(ci(s, "Amount")));
                vd.put("CurrencyId", currencyId);
                vd.put("ExchangeRate", BigDecimal.valueOf(exRate));
                vd.put("FcyAmount", dec(toDouble(ci(s, "FcyAmount"))));
                vd.put("Remarks", str(ci(s, "Remarks")));
                vd.put("BranchId", toInt(ci(s, "BranchId")));
                details.add(vd);
            }
        }

        // ---------------------------------------------------- the header totals (CalculateTotalInformation :2412)
        double totalAmount = formatSingle(sumAmount, amountDec);
        BigDecimal totalQty = dec(sumQty).setScale(2, RoundingMode.HALF_EVEN);
        BigDecimal totalWeight = dec(sumWeight).setScale(2, RoundingMode.HALF_EVEN);

        // ------------------------------------------------------------------------- the header (:1913-1966)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        Timestamp docDate = pickerKeepTime(sv(dto, "DocDate"), existing == null ? null : asTimestamp(ci(existing, "DocDate")));
        LocalTime docTime = docDate.toLocalDateTime().toLocalTime();
        Timestamp departure = isBlank(sv(dto, "DepartureDate")) ? docDate
                : Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(sv(dto, "DepartureDate").trim().substring(0, 10)), docTime));
        Timestamp eta = isBlank(sv(dto, "ETAdestination")) ? docDate
                : Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(sv(dto, "ETAdestination").trim().substring(0, 10)), docTime));
        Map<String, Object> head = new LinkedHashMap<>();                                            // model 0927, declaration order
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        head.put("DepartureDate", departure);
        head.put("DocDate", docDate);
        head.put("EntryDate", now);                                                                   // BLL Save
        head.put("ETAdestination", eta);
        head.put("ModifyDate", now);
        head.put("ExchangeRate", BigDecimal.valueOf(exRate));
        head.put("FcyAmount", new BigDecimal(fcyText.replace(",", "")));
        head.put("Freight", BigDecimal.valueOf(cvDouble(freightText)));
        head.put("OtherExpense", BigDecimal.valueOf(cvDouble(sv(dto, "OtherExpense"))));
        head.put("SupplierWbWeight", BigDecimal.valueOf(cvDouble(sv(dto, "SupplierWbWeight"))));
        head.put("TotalAmount", BigDecimal.valueOf(totalAmount));
        head.put("TotalQty", totalQty);
        head.put("TotalWeight", totalWeight);
        head.put("BranchSrNo", branchSrNo);
        head.put("FinancialYearId", fy);
        head.put("ActionId", recId == 0 ? 1 : 2);                                                    // BLL Save
        head.put("ApprovedUserId", u.getId());
        head.put("BranchesId", branch(u));
        head.put("CityId", cityId);
        head.put("CompanyId", u.getCompanyId());
        head.put("CurrencyId", currencyId);
        head.put("DocNo", docNo);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUserId", u.getId());
        head.put("Id", recId);
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PendingForView", 0);
        head.put("ProjectId", branch(u));                                                            // D2
        head.put("TransitDays", cvIntText(transitText));
        head.put("BiltyNo", biltyNo);
        head.put("Driver1CellNo", driverCell1);
        head.put("Driver2CellNo", sv(dto, "Driver2CellNo"));
        head.put("DriverName", driverName);
        head.put("RemarksHeader", sv(dto, "RemarksHeader"));
        head.put("VehicleNo", vehicleNo);
        head.put("AdvanceFreight", BigDecimal.ZERO);
        head.put("VehicleImage", pictureName(u, existing, "VehicleImage", dto.get("VehicleImagePick")));
        head.put("DriverPic", pictureName(u, existing, "DriverPic", dto.get("DriverImagePick")));
        head.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        head.put("CustomAttachmentsValues", existing == null ? "" : str(ci(existing, "CustomAttachmentsValues")));
        SupplierDispatchAttachmentService.Prepared attached = attachments.prepare(recId, DOCUMENT_TYPE_ID, dto.get("attachments"));
        if (attached != null && attached.changed()) {                                                // DAL 0383 SetData
            head.put("AttachmentsValues", attached.names());
            head.put("CustomAttachmentsValues", attached.storedNames());
        }

        int id = setData(head, details);
        attachments.persist(id, DOCUMENT_TYPE_ID, id, attached);                                     // RefAccountId = the record id (desktop)
        LOG.debug("Supplier Dispatch {} saved (DocNo {})", id, docNo);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }

    /**
     * po.VehicleImage / po.DriverPic: the path of a newly browsed picture (BtnBrowseVehicleImage_Click :3640), else the
     * stored one when the file still exists (ReadById :2186-2213 clears it when it does not), else "".
     */
    @SuppressWarnings("unchecked")
    private String pictureName(UserAccount u, Map<String, Object> existing, String column, Object pick) {
        if (pick instanceof Map) {
            Map<String, Object> p = (Map<String, Object>) pick;
            String name = sv(p, "name");
            String b64 = sv(p, "base64");
            if (!name.isEmpty() && !b64.isEmpty()) {
                com.mst.models.dto.InventoryPosItemRequest.Upload up = new com.mst.models.dto.InventoryPosItemRequest.Upload();
                up.name = name;
                up.base64 = b64;
                byte[] bytes = DesktopInventoryItemFileService.decode(up);
                DesktopInventoryItemFileService.imageType(bytes);                  // only a real jpg / png / gif / bmp
                return store.store(u, name, bytes);
            }
        }
        if (existing == null) return "";
        String old = str(ci(existing, column));
        return imageReadable(u, old) ? old : "";
    }

    /** DAL 0383 SetData - header, then the detail rows (the commission and expense lists are empty on this form); one transaction. */
    private int setData(Map<String, Object> head, List<Map<String, Object>> details) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");
        int recId = toInt(head.get("Id"));
        int num = repo.setProc("[dbo].[USP_SupplierDispatch_InsertAndUpdate]", head);
        int id = num > 0 ? num : recId;
        if (id <= 0) throw new IllegalStateException("Save returned no document id.");
        for (Map<String, Object> d : details) {
            d.put("SupplierDispatchId", id);
            repo.setProc("[dbo].[USP_SupplierDispatchDetail_Insert]", d);
        }
        return id;
    }

    /** Model 0926 SupplierDispatchDetail - non-virtual properties. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("ExchangeRate", BigDecimal.ZERO);
        p.put("FcyAmount", BigDecimal.ZERO);
        for (String k : new String[] { "AdLsWeight", "Amount", "EBTotalWt", "EBWeight", "GrossWeight", "Qty", "Rate", "WeightKgs" }) p.put(k, 0d);
        for (String k : new String[] { "ActionTypeId", "BranchId", "CommissionAgentId", "CropYearId", "CurrencyId", "Id", "ItemId",
                "OrderId", "OrderDetailId", "PackingTypeId", "PackSizeId", "RateUomId", "ReferencePartyId", "SortNo",
                "SupplierDispatchId", "SupplierId" }) p.put(k, 0);
        p.put("Remarks", null);
        return p;
    }

    // ============================================================================ helpers

    private Map<String, Object> decimals(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        int a = toInt(repo.config(u, "Default NoofDecimal Points For Amount"));
        m.put("amount", a >= 1 && a <= 4 ? a : 0);
        int r = toInt(repo.config(u, "Default NoofDecimal Points For Rate"));
        m.put("rate", r >= 1 && r <= 4 ? r : (r == 0 ? 2 : 0));
        int f = toInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        m.put("fcy", Math.max(0, Math.min(f, 10)));
        return m;
    }

    private static double formatSingle(double x, int decimals) {
        if (!Double.isFinite(x)) return 0d;
        return new BigDecimal(com.mst.repositories.StoreIssuanceRepository.clr(x)).setScale(decimals, RoundingMode.HALF_UP).doubleValue();
    }

    private static String formatFixed(BigDecimal v, int decimals) {
        return v.setScale(decimals, RoundingMode.HALF_UP).toPlainString();
    }

    private UserAccount requireView() {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "view")) throw new AccessDeniedException("You do not have the View right for Supplier Dispatch.");
        return u;
    }

    /** The header must exist (ActionId &lt;&gt; 3), be DocumentTypeId 246 and belong to the user's organization and company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != i(u.getCompanyId())) return null;
        if (toInt(ci(h, "OrganizationId")) != i(u.getOrganizationId())) return null;
        return h;
    }

    private static Timestamp pickerKeepTime(String day, Timestamp stored) {
        LocalDate d = isBlank(day) ? (stored != null ? stored.toLocalDateTime().toLocalDate() : LocalDate.now())
                : LocalDate.parse(day.trim().substring(0, 10));
        LocalTime t = stored != null ? stored.toLocalDateTime().toLocalTime() : LocalTime.now().withNano(0);
        return Timestamp.valueOf(LocalDateTime.of(d, t));
    }

    private static Timestamp pickerDate(String day) {
        return Timestamp.valueOf(LocalDateTime.of(LocalDate.parse(day.trim().substring(0, 10)), LocalTime.now().withNano(0)));
    }

    private static Timestamp asTimestamp(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v instanceof LocalDateTime) return Timestamp.valueOf((LocalDateTime) v);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime());
        return null;
    }

    private static Map<String, Object> idName(int id, String name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    private static Map<Integer, Map<String, Object>> byId(List<Map<String, Object>> rows) {
        Map<Integer, Map<String, Object>> m = new HashMap<>();
        for (Map<String, Object> r : rows) m.put(toInt(r.get("Id")), r);
        return m;
    }

    private static boolean contains(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    /** "1,2,x,3" -> "1,2,3" (digits only, no empties). */
    private static String cleanIds(String ids) {
        if (ids == null) return "";
        StringBuilder sb = new StringBuilder();
        for (String s : ids.split(",")) {
            String t = s.trim();
            if (!t.matches("\\d+") || Integer.parseInt(t) <= 0) continue;
            if (sb.length() > 0) sb.append(',');
            sb.append(Integer.parseInt(t));
        }
        return sb.toString();
    }

    private static String baseName(String v) {
        String name = v.replace('\\', '/');
        name = name.substring(name.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(name);
        return name;
    }

    /** Date values leave as local "yyyy-MM-dd HH:mm:ss" text, never as a JSON timestamp. */
    static Object plain(Object v) { return StockInTransitService.plain(v); }

    static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) { return StockInTransitService.plainRows(rows); }

    private static BigDecimal dec(double v) { return new BigDecimal(com.mst.repositories.StoreIssuanceRepository.clr(v)); }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }

    /** Conversion.ToInt on text - thousands separators allowed; anything unparseable is 0. */
    private static int cvIntText(String s) {
        if (s == null) return 0;
        try { return (int) Math.rint(Double.parseDouble(s.trim().replace(",", ""))); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble on text - thousands separators allowed; anything unparseable is 0. */
    private static double cvDouble(String s) {
        if (s == null || s.trim().isEmpty()) return 0d;
        try { return Double.parseDouble(s.trim().replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }

    /** A posted text value ("" when absent). */
    private static String sv(Map<String, Object> m, String key) {
        Object v = m == null ? null : m.get(key);
        return v == null ? "" : String.valueOf(v);
    }

    /** A posted integer ("" / absent / unparseable = 0). */
    private static int in(Map<String, Object> m, String key) {
        Object v = m == null ? null : m.get(key);
        if (v instanceof Number) return ((Number) v).intValue();
        return cvIntText(v == null ? "" : String.valueOf(v));
    }

    /** A posted number. */
    private static double db(Map<String, Object> m, String key) {
        Object v = m == null ? null : m.get(key);
        if (v instanceof Number) return ((Number) v).doubleValue();
        return cvDouble(v == null ? "" : String.valueOf(v));
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> listOf(Map<String, Object> m, String key) {
        Object v = m == null ? null : m.get(key);
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }
}
