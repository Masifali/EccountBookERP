package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpBModels;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpBSupport.*;

/**
 * BLL of Architecture.WinApp.PartyProcessing.AdvanceDeliveryOrderPP (ScreenId 875, DocumentTypeId 910, DeliveryOrderType "AdvanceDO").
 *
 * Save: InvDeliveryOrder.Save (BLL 0558: Entry / Modify dates now; insert "Record cannot be inserted because ActionTypeId not equal to 1",
 * ActionId 1 / update ActionId 2) -> DAL 0411 SetData, one transaction: Sp_InvDeliveryOrder_Insert / _Update -> Sp_InvDeliveryOrderDetail_Insert
 * per row (removed rows first, ActionTypeId 3) -> [DAW].[USp_DocumentApprovalDetail_Insert] (LimitAmount 0).
 * Delete: CommonServices.RemoveByID(910, Id) -> CommonRepository.RemoveByID -> USP_RecoredRemoveByOrgCompDocAndByID.
 * The desktop's lists come from the application caches (GlobalServicesMethods): customers USP_GetVendorsAndCustomersWithCityName,
 * warehouses USP_GetWarehousesAllocatedToBranch, items USP_Item_AllItemsWithModal, crop years / packing types 'ReadAll',
 * job lots SP_JobLot_ReadMethod 'GetJobLotGlIdsandName', UOMs usp_getAllUomsByCompanyId.
 */
@Service
public class PpBAdvanceDoService {

    public static final int SCREEN = 875;
    public static final int DOC = 910;
    private static final String PROC = "Sp_InvDeliveryOrder_GetAllMethod";

    @Autowired private PpBSupport pp;

    // ------------------------------------------------------------------ Load

    /** InitializeComponentMethod(): rights (btnsave / btnSaveAs Save, btnupdate Update, btnDelete Delete, Print), GetConfigurationsFromGlobal,
     *  DeliveryOrderGenerateCode, the combos, GetConfigurationsFromGlobalAndBindValuesInColumns, HistoryComboDBCall. */
    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lists(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("itemSearchByCode", toBool(pp.lookups().config(u, "ItemSearchByCode")));
        out.put("defaultDays", toInt(pp.lookups().config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("warningPackingWeight", toBool(pp.lookups().config(u, "WarningMessageOnDOForPackingWeight")));
        out.put("tolerance", d(pp.lookups().config(u, "ToleranceForStockWeightAndNetWeightOnADO")));
        out.put("allowMultipleAdo", toBool(pp.lookups().config(u, "AllowMultipleOrPartialInvoicingForADO")));
        out.put("defaults", defaults(u));
        out.put("docNo", docNo(u));
        out.put("historyCustomers", historyCustomers(u));
        return out;
    }

    /** GetConfigurationsFromGlobalAndBindValuesInColumns(): "Job/Lot", "Default Crop Year", "Paking Type", "Warehouse". */
    private Map<String, Object> defaults(UserAccount u) {
        return map("jobLotId", toInt(pp.lookups().config(u, "Job/Lot")), "cropYearId", toInt(pp.lookups().config(u, "Default Crop Year")),
                "packingTypeId", toInt(pp.lookups().config(u, "Paking Type")), "warehouseId", toInt(pp.lookups().config(u, "Warehouse")));
    }

    /** btnRefresh_Click: the caches re-read, then the combos rebound. */
    public Map<String, Object> refresh() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lists(u);
        out.put("defaults", defaults(u));
        return out;
    }

    private Map<String, Object> lists(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        boolean f4 = pp.lookups().feature(u, 4);
        List<Map<String, Object>> customers = new ArrayList<>();                                     // SupplierDtFillFromGlobal()
        for (Map<String, Object> r : pp.db().rows("USP_GetVendorsAndCustomersWithCityName", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            if (f4 && toInt(col(r, "PartyTypeId")) != 2) continue;
            customers.add(map("Id", col(r, "Id"), "CompanyName", col(r, "CompanyName"), "PartyCode", col(r, "PartyCode"),
                    "CityName", col(r, "CityName"), "MobileNo", col(r, "MobilePersonal")));
        }
        out.put("customers", customers);
        out.put("refParties", pick(pp.lookups().referenceParties(u), "Id", "ReferencePartyName"));
        out.put("vehicleTypes", pick(pp.lookups().vehicleTypes(), "Id", "VehicleDescription"));
        out.put("warehouses", pick(pp.db().rows("USP_GetWarehousesAllocatedToBranch", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "BranchId", branch(u)), "Id=WarehouseId", "WareHouseName=WarehouseName"));
        List<Map<String, Object>> items = new ArrayList<>();                                         // ItemDtFillFromGlobal(): not item types 14 / 17
        for (Map<String, Object> r : pp.db().rows("[dbo].[USP_Item_AllItemsWithModal]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId())) {
            int t = toInt(col(r, "ItemTypeOfTypeId"));
            if (t == 14 || t == 17) continue;
            items.add(map("Id", col(r, "Id"), "ItemName", col(r, "ItemName"), "ItemCode", col(r, "ItemCode")));
        }
        out.put("items", items);
        out.put("cropYears", pick(pp.lookups().cropYears(u), "Id", "CropYear"));
        out.put("jobLots", pick(pp.db().rows("[dbo].[SP_JobLot_ReadMethod]", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetJobLotGlIdsandName"), "Id", "JobLotDescription"));
        out.put("packingTypes", pick(pp.lookups().packingTypes(), "Id", "PackTypeDesc"));
        return out;
    }

    /** CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId): usp_getAllUomsByCompanyId (@Org @Company @ItemId). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = pp.user(SCREEN);
        if (itemId == 0) return new ArrayList<>();
        return pick(pp.db().rows("usp_getAllUomsByCompanyId", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "ItemId", itemId),
                "Id", "UOMCode", "Equivalent");
    }

    /** DeliveryOrderGenerateCode(910): Sp_InvDeliveryOrder_GetAllMethod 'GenerateCode' (@Org @Company @DocumentTypeId @FinancialYearId). */
    private int docNo(UserAccount u) {
        List<Map<String, Object>> r = pp.db().rows(PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "FinancialYearId", pp.hrm().financialYearId(), "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    public Map<String, Object> code() { return map("docNo", docNo(pp.user(SCREEN))); }

    /** UpdateAvailableStockLabel(): InvDeliveryOrder.GetAvailableStockForDeliveryOrder -> USP_GetAvailableStockForDeliveryOrder -> AvailableStock. */
    public Map<String, Object> stock(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Timestamp dd = stamp(b.get("docDate"));
        List<Map<String, Object>> r = pp.db().rows("USP_GetAvailableStockForDeliveryOrder", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ItemId", toInt(b.get("itemId")), "DocDateTo", dd == null ? Timestamp.valueOf(LocalDateTime.now()) : dd,
                "WareHouseId", toInt(b.get("warehouseId")), "JobLotId", toInt(b.get("jobLotId")), "CropYear", str(b.get("cropYear")),
                "InvPackingTypeId", toInt(b.get("packingTypeId")), "ItemUomId", toInt(b.get("uomId")));
        return map("stock", r.isEmpty() ? 0 : d(col(r.get(0), "AvailableStock")));
    }

    /** UpdateAvailableStockInGridDetailRow(r) for every grid row (DocDate_ValueChanged): InventoryStockEvalautionDetail.GetCurrentStockByItemId
     *  -> Sp_SaleOrder_GetAllMethod 'GetCurrentStockByItemId' -> AvailableStock. */
    public List<Double> rowStock(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Timestamp dd = stamp(b.get("docDate"));
        List<Double> out = new ArrayList<>();
        for (Map<String, Object> r : PpBGrnGdnStoreService.list(b.get("rows"))) {
            List<Map<String, Object>> x = pp.db().rows("Sp_SaleOrder_GetAllMethod", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "ItemId", toInt(r.get("ItemId")), "DocDateTo", dd == null ? Timestamp.valueOf(LocalDateTime.now()) : dd,
                    "WareHouseId", toInt(r.get("WareHouseId")), "JobLotId", toInt(r.get("JobLotId")), "CropYear", str(r.get("CropYear")),
                    "InvPackingTypeId", toInt(r.get("PackingTypeId")), "ItemUomId", toInt(r.get("ItemUOMId")), "Activity", "GetCurrentStockByItemId");
            out.add(x.isEmpty() ? 0d : d(col(x.get(0), "AvailableStock")));
        }
        return out;
    }

    // ------------------------------------------------------------------ Load Sale Invoice (LoadSaleInvoiceForADO)

    /** ComboDbCall(): InvSaleInvoice.AllComboBindAgainstSaleInvoice (@Org @Company @AppId @UserId @DocumentTypeIds "95,171" @Activity "Supplier");
     *  FromDate = ActiveYr.Start_Period; AllowMultipleOrPartialInvoicingForADO. */
    public Map<String, Object> invoiceSetup() {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> combo = pick(pp.db().rows("[dbo].[Usp_AllComboAgainstSaleInvoice]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "AppId", u.getAppId(), "UserId", u.getId(), "DocumentTypeIds", "95,171", "Activity", "Supplier"),
                "Id", "ReferenceName");
        Object from = pp.loaderSetup(u).get("fromDate");
        return map("customers", combo, "fromDate", from, "allowMultipleAdo", toBool(pp.lookups().config(u, "AllowMultipleOrPartialInvoicingForADO")));
    }

    /** PendingDataDbCall(): InvSaleInvoice.PendingReservedSaleInvoiceForAdvanceDeliveryOrder -> [dbo].[usp_PendingReservedSaleInvoiceForAdvanceDeliveryOrder]. */
    public List<Map<String, Object>> invoices(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        return pending(u, day(b.get("fromDate")), day(b.get("toDate")), toInt(b.get("customerId")), 0);
    }

    private List<Map<String, Object>> pending(UserAccount u, Timestamp from, Timestamp to, int customerId, int invoiceId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FinancialYearId", pp.hrm().financialYearId());
        p.put("SupplierCustomerId", nz0(customerId));
        p.put("InvoiceId", nz0(invoiceId));
        p.put("FromDate", from);
        p.put("ToDate", to);
        return pp.db().rows("[dbo].[usp_PendingReservedSaleInvoiceForAdvanceDeliveryOrder]", p);
    }

    // ------------------------------------------------------------------ history

    /** HistoryComboDBCall(): InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (@FinancialYearId, @BranchesIds, @Activity "Customer",
     *  @DeliveryOrderType "AdvanceDO") -> Id / ReferenceName. */
    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        return pick(pp.db().rows("USP_GetDataForDropDownFromDeliveryOrder", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", nz0(pp.hrm().financialYearId()), "BranchesIds", String.valueOf(branch(u)), "Activity", "Customer",
                "DeliveryOrderType", "AdvanceDO"), "Id", "ReferenceName");
    }

    public List<Map<String, Object>> historyCustomers() { return historyCustomers(pp.user(SCREEN)); }

    /** gridhistoryfill(): InvDeliveryOrder.FormHistoryNew -> SP_DeliveryOrderFormHistory (@DeliveryOrderType "AdvanceDO"); one row per Id. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        boolean all = pp.canViewAll(u, SCREEN);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOC);
        p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", nz0(pp.hrm().financialYearId()));
        if (!all) p.put("EntryUser", u.getId());
        p.put("DeliveryOrderType", "AdvanceDO");
        dateFilter(p, b, "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", "ApprovedFromDate", "ApprovedToDate");
        p.put("DocNoFrom", nz0(toInt(b.get("fromDocNo"))));
        p.put("DocNoTo", nz0(toInt(b.get("toDocNo"))));
        p.put("SupplierCustomerId", nz0(toInt(b.get("customerId"))));
        List<Map<String, Object>> out = new ArrayList<>();
        List<Integer> seen = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows("SP_DeliveryOrderFormHistory", p)) {
            int id = toInt(col(r, "Id"));
            if (seen.contains(id)) continue;
            seen.add(id);
            out.add(pick(List.of(r), "Id", "DoType=DeliveryOrderType", "DocDate", "DocNo", "ExpiryDate", "VehicleType", "VehicleNo", "SupplierCustomerId",
                    "CustomerName", "RefPartyId", "ReferencePartyName", "ApprovalStatus", "EntryUser", "EntryDate", "ModifyUser", "ModifyDate",
                    "ApprovedUser", "ApprovedDate", "OrderStatus", "Remarks=HeaderRemarks", "NoOfAttachments").get(0));
        }
        return out;
    }

    // ------------------------------------------------------------------ read

    /** ReadById: InvDeliveryOrder.GetByID -> 'ReadById' + DAL GetData 'ReadByIdDetailId' (@Id). */
    public Map<String, Object> byId(int id) { return read(pp.user(SCREEN), id); }

    private Map<String, Object> read(UserAccount u, int id) {
        List<Map<String, Object>> h = pp.db().rows(PROC, "Id", id, "Activity", "ReadById");
        if (h.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = h.get(0);
        if (toInt(col(r, "CompanyId")) != u.getCompanyId() || toInt(col(r, "OrganizationId")) != u.getOrganizationId()
                || toInt(col(r, "DocumentTypeId")) != DOC) throw invalid("Record not found");
        Map<String, Object> out = pick(h, "Id", "DocNo", "DocDate", "ExpiryDate", "VehicleType", "VehicleNo", "LoadingInstructions", "IsApproved").get(0);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d0 : pp.db().rows(PROC, "Id", id, "Activity", "ReadByIdDetailId")) {       // FilldtDetailFromListCommonForReadById
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", col(d0, "Id"));
            x.put("RefDocumentTypeId", col(d0, "RefDocumentTypeId"));
            x.put("RefDocIdNo", col(d0, "RefDocIdNo"));
            x.put("RefDocSubIdNo", col(d0, "RefDocSubIdNo"));
            x.put("RefDocType", col(d0, "RefDocumentType"));
            x.put("InvoiceNo", col(d0, "RefDocNo"));
            x.put("InvoiceManualBillNo", col(d0, "RefManualBillNo"));
            x.put("WareHouseId", col(d0, "WarehouseId"));
            x.put("ItemId", col(d0, "ItemId"));
            x.put("ItemCode", col(d0, "ItemCode"));
            x.put("Item", col(d0, "ItemName"));
            x.put("CropYearId", col(d0, "CropYearId"));
            x.put("JobLotId", col(d0, "JobLotId"));
            x.put("PackingTypeId", col(d0, "InvPackingTypeId"));
            x.put("ItemUOMId", col(d0, "PackUomId"));
            x.put("ItemUOM", col(d0, "PackUOM"));
            x.put("PUomEquivalent", col(d0, "PUomEquivalent"));
            x.put("QTY", col(d0, "DoQty"));
            x.put("Weight", col(d0, "DoWeight"));
            x.put("PackingUnit", col(d0, "PackingWeight"));
            x.put("PackingWeight", col(d0, "OuterEbTotal"));
            x.put("GrossWeight", col(d0, "GrossWeight"));
            x.put("Remarks", col(d0, "LoadingRemarks"));
            x.put("AvailableStock", 0);
            x.put("OtherRemarks", col(d0, "InspectionRemarks"));
            x.put("StockWeight", col(d0, "StockWeight"));
            x.put("InvoiceQty", col(d0, "RefDocQty"));
            x.put("InvoiceWeight", col(d0, "RefDocWeight"));
            x.put("WareHouseName", col(d0, "WareHouseName"));
            x.put("CropYear", col(d0, "CropYear"));
            x.put("JobLot", col(d0, "JobLotDescription"));
            x.put("PackingType", col(d0, "PackTypeDesc"));
            x.put("SupplierCustomerId", col(d0, "SupplierCustomerId"));
            x.put("RefPartyId", col(d0, "RefPartyId"));
            det.add(x);
        }
        out.put("details", det);
        return out;
    }

    // ------------------------------------------------------------------ save

    /**
     * Insert(): "Grid Record Not Found", FormHelper.ValidateControls (Doc No, Customer), the expiry date check, the per-row
     * FormHelper.ValidateField checks and the stock weight tolerance (when partial invoicing is off), then InvDeliveryOrder.Save.
     * The confirm boxes (save / update / "no Packing Weight") run on the page.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.hrm().require(u, SCREEN, recId > 0 ? "Update" : "Save");
        Map<String, Object> existing = recId > 0 ? read(u, recId) : null;

        List<Map<String, Object>> rows = PpBGrnGdnStoreService.list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");
        int docNo = toInt(trim(b.get("docNo")));
        if (docNo == 0) throw invalid("Doc No must be a non-zero number");
        int customerId = toInt(b.get("customerId"));
        Map<String, Object> ls = lists(u);
        if (customerId == 0 || !has(PpBGrnGdnStoreService.list(ls.get("customers")), "Id", customerId)) throw invalid("Customer field is required");
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime docDate = toDate(b.get("docDate"));
        if (docDate == null) docDate = now;
        LocalDateTime expiry = toDate(b.get("expiryDate"));
        if (expiry == null) expiry = now;
        if (docDate.toLocalDate().isAfter(expiry.toLocalDate())) throw invalid("Expiry Date Should be Greater than or Equal to Doc Date");

        boolean allowMultiple = toBool(pp.lookups().config(u, "AllowMultipleOrPartialInvoicingForADO"));
        double tolerance = d(pp.lookups().config(u, "ToleranceForStockWeightAndNetWeightOnADO"));

        PpBModels.InvDeliveryOrder po = new PpBModels.InvDeliveryOrder();
        po.Id = recId;
        if (existing != null) po.IsApproved = toBool(existing.get("IsApproved"));
        po.OrganizationId = u.getOrganizationId();
        po.CompanyId = u.getCompanyId();
        po.FinancialYearId = pp.hrm().financialYearId();
        po.BranchesId = branch(u);
        po.DocumentTypeId = DOC;
        po.ScreenName = "AdvanceDeliveryOrderPP";
        po.EntryUser = u.getId();
        po.ModifyUser = u.getId();
        po.DocNo = docNo;
        po.DocDate = docDate;
        po.ExpiryDate = expiry;
        po.DeliveryOrderType = "AdvanceDO";
        po.VehicleType = str(b.get("vehicleType"));
        po.VehicleNo = trim(b.get("vehicleNo"));
        po.LoadingInstructions = str(b.get("remarks"));
        int refPartyId = toInt(b.get("refPartyId"));

        List<Integer> own = new ArrayList<>();
        if (existing != null) for (Map<String, Object> x : PpBGrnGdnStoreService.list(existing.get("details"))) own.add(toInt(x.get("Id")));
        List<PpBModels.InvDeliveryOrderDetail> details = new ArrayList<>();
        if (recId > 0) {                                                             // lstRemoveRecord (DeleteDetailRow)
            for (Map<String, Object> r : PpBGrnGdnStoreService.list(b.get("removed"))) {
                int did = toInt(r.get("Id"));
                if (did <= 0 || !own.contains(did)) continue;
                PpBModels.InvDeliveryOrderDetail vd = detail(r, customerId, refPartyId);
                vd.Id = did;
                vd.ActionTypeId = 3;
                details.add(vd);
            }
        }
        // the sale invoice references must be ones the loader offers this company (or the saved ones)
        List<String> ownRefs = new ArrayList<>();
        if (existing != null) for (Map<String, Object> x : PpBGrnGdnStoreService.list(existing.get("details"))) {
            ownRefs.add(toInt(x.get("RefDocumentTypeId")) + ":" + toInt(x.get("RefDocIdNo")) + ":" + toInt(x.get("RefDocSubIdNo")));
        }
        List<Map<String, Object>> offered = null;
        BigDecimal doTotalQty = BigDecimal.ZERO;
        double net = 0, packing = 0, gross = 0;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            PpBModels.InvDeliveryOrderDetail vd = detail(r, customerId, refPartyId);
            vd.Id = recId != 0 ? toInt(r.get("Id")) : 0;
            if (vd.Id > 0 && !own.contains(vd.Id)) vd.Id = 0;
            vd.ActionTypeId = vd.Id <= 0 ? 1 : 2;
            String at = " is required in Detail Grid at row No: " + (i + 1);
            if (vd.WarehouseId == 0) throw invalid("Warehouse" + at);
            if (vd.ItemId == 0) throw invalid("Item" + at);
            if (vd.CropYearId == 0) throw invalid("CropYear" + at);
            if (vd.JobLotId == 0) throw invalid("JobLot" + at);
            if (vd.InvPackingTypeId == 0) throw invalid("PackingType" + at);
            if (vd.PackUomId == 0) throw invalid("PackUom" + at);
            if (vd.DoQty <= 0) throw invalid("Qty" + at);
            if (vd.LoadingWeight <= 0) throw invalid("DoWeight" + at);
            if (vd.GrossWeight <= 0) throw invalid("GrossWeight" + at);
            if (!allowMultiple) {
                if (vd.StockWeight > vd.GrossWeight + tolerance) {
                    throw invalid("Stock Weight should be less than or equal to Gross Weight + Tolerance : " + net(tolerance) + " in Detail Grid Row No " + (i + 1));
                }
                if (vd.StockWeight < vd.GrossWeight - tolerance) {
                    throw invalid("Stock Weight should be greater than or equal to Gross Weight - Tolerance : " + net(tolerance) + " in Detail Grid Row No " + (i + 1));
                }
            }
            if (vd.RefDocumentTypeId > 0 || vd.RefDocIdNo > 0 || vd.RefDocSubIdNo > 0) {
                if (!ownRefs.contains(vd.RefDocumentTypeId + ":" + vd.RefDocIdNo + ":" + vd.RefDocSubIdNo)) {
                    if (offered == null) offered = pending(u, null, null, customerId, 0);
                    boolean ok = false;
                    for (Map<String, Object> o : offered) {
                        if (toInt(col(o, "DocumentTypeId")) == vd.RefDocumentTypeId && toInt(col(o, "Id")) == vd.RefDocIdNo
                                && toInt(col(o, "DetailId")) == vd.RefDocSubIdNo) ok = true;
                    }
                    if (!ok) throw invalid("Record not found...");
                }
            }
            doTotalQty = doTotalQty.add(BigDecimal.valueOf(vd.DoQty));
            net += vd.LoadingWeight;
            packing += vd.OuterEbTotal;
            vd.TotalPackingWeight = vd.OuterEbTotal;
            gross += vd.GrossWeight;
            details.add(vd);
        }
        po.DoTotalQty = doTotalQty;
        po.NetWeight = net;
        po.PackingWeight = packing;
        po.GrossWeight = gross;

        // InvDeliveryOrder.Save
        po.EntryDate = LocalDateTime.now();
        po.ModifyDate = LocalDateTime.now();
        if (po.Id == 0) {
            for (PpBModels.InvDeliveryOrderDetail vd : details) if (vd.ActionTypeId != 1) throw invalid("Record cannot be inserted because ActionTypeId not equal to 1");
            po.ActionId = 1;
        } else {
            po.ActionId = 2;
        }
        pp.db().tx(() -> {
            int num = pp.db().set(po.Id == 0 ? "Sp_InvDeliveryOrder_Insert" : "Sp_InvDeliveryOrder_Update", po);
            if (num > 0) po.Id = num; else num = po.Id;
            for (PpBModels.InvDeliveryOrderDetail vd : details) {
                vd.InvDeliveryOrderId = po.Id;
                pp.db().set("Sp_InvDeliveryOrderDetail_Insert", vd);
            }
            PpBModels.DocumentApprovalDetail a = new PpBModels.DocumentApprovalDetail();
            a.OrganizationId = po.OrganizationId; a.CompanyId = po.CompanyId; a.DocumentTypeId = po.DocumentTypeId; a.Id = po.Id;
            a.LimitAmount = BigDecimal.ZERO;
            pp.db().set("[DAW].[USp_DocumentApprovalDetail_Insert]", a);
            return num;
        });
        return saved(po.Id, (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + po.DocNo);
    }

    /** FillDetailListCommonForInsertAndDelete(vd, r). */
    private static PpBModels.InvDeliveryOrderDetail detail(Map<String, Object> r, int customerId, int refPartyId) {
        PpBModels.InvDeliveryOrderDetail vd = new PpBModels.InvDeliveryOrderDetail();
        vd.SupplierCustomerId = customerId;
        vd.RefPartyId = refPartyId;
        vd.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
        vd.RefDocIdNo = toInt(r.get("RefDocIdNo"));
        vd.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
        vd.WarehouseId = toInt(r.get("WareHouseId"));
        vd.ItemId = toInt(r.get("ItemId"));
        vd.CropYearId = toInt(r.get("CropYearId"));
        vd.JobLotId = toInt(r.get("JobLotId"));
        vd.InvPackingTypeId = toInt(r.get("PackingTypeId"));
        vd.PackUomId = toInt(r.get("ItemUOMId"));
        vd.DoQty = d(r.get("QTY"));
        vd.DoWeight = d(r.get("Weight"));
        vd.LoadingQty = d(r.get("QTY"));
        vd.LoadingWeight = d(r.get("Weight"));
        vd.PackingWeight = d(r.get("PackingUnit"));
        vd.OuterEbTotal = d(r.get("PackingWeight"));
        vd.GrossWeight = d(r.get("GrossWeight"));
        vd.StockWeight = d(r.get("StockWeight"));
        vd.LoadingRemarks = str(r.get("Remarks"));
        vd.InspectionRemarks = str(r.get("OtherRemarks"));
        return vd;
    }

    // ------------------------------------------------------------------ delete / print

    /** btnDelete_Click: "Record Id Not Found" / "Record has been approved" / CommonServices.RemoveByID(910, Id) ->
     *  USP_RecoredRemoveByOrgCompDocAndByID (@Org @Company @DocumentTypeId @Id @UserId) -> "Record Deleted Successfully". */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.hrm().require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("Record Id Not Found");
        Map<String, Object> r = read(u, id);
        if (toBool(r.get("IsApproved"))) throw invalid("Record has been approved");
        pp.db().tx(() -> pp.db().rows("USP_RecoredRemoveByOrgCompDocAndByID", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "Id", id, "UserId", u.getId()));
        return saved(id, "Record Deleted Successfully");
    }

    /** AdvanceDeliveryOrderSlip910 / 910_01 / AdvanceDeliveryOrderChallanSlip910A: those CommonServices bodies are not in the available
     *  sources, so the page prints the saved document itself (this record) through the grid-to-PDF printer. */
    public Map<String, Object> slip(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.hrm().require(u, SCREEN, "Print");
        return read(u, id);
    }
}
