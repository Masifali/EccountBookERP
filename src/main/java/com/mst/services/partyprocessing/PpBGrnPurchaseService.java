package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpBModels;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpBSupport.*;

/**
 * BLL of Architecture.WinApp.PartyProcessing.GRNForPurchaseFromPartyProcessing (ScreenId 613, DocumentTypeId 217).
 *
 * Save: InvGrn.Save (BLL 0576: insert ActionId 1 - "Record cannot be inserted because detailId greater than zero" -, update ActionId 2)
 * -> DAL 0429 SetData, one transaction: Sp_InvGrn_Insert / _Update -> Sp_InvGrnDetail_Insert per row (LineId 1..n) ->
 * Sp_InvGrnDetailEmptyBags_Insert -> Sp_InventoryTransactions_GetALLMethod -> without loader rows the party processing FIFO
 * (USP_GetStockByFifoMethodPartyProcessing, USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId, USP_InventoryTransactionsPartyProcessing_Insert),
 * with loader rows Sp_InventoryTransactionsPartyProcessing_Insert -> USP_InventoryValidationPartyProcessing per detail row.
 * Delete: InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete (@Org @Company @Id @DocumentTypeId 217 @UserId).
 */
@Service
public class PpBGrnPurchaseService {

    public static final int SCREEN = 613;
    public static final int DOC = 217;
    private static final String PROC = "Sp_InvGRN_GetAllMethod";
    private static final String CONFIGS = "WeightCutForJuteBags,WeightCutForPPBags,WeightCutForOpenBulk,EmptyBagsWeightCutEditableOnGRN,"
            + "AddLessWeightCutEditableOnGRN,WagesCompulsoryOnGrnForPurchaseFromPartyProcessing,EmptyBagsInofrmationCompulsoryOnGRN,"
            + "LabCompulsoryForWeighBridgeAgainstGatePurchase,BillWeightAndStockWeightDifferenceTolerance";

    @Autowired private PpBSupport pp;

    // ------------------------------------------------------------------ Load

    /**
     * GRNForPurchaseFromPartyProcessing_Load: GetConfiguration() (ConfigrationsAllocation.GetMultipleConfigurationsByConfigDescriptions),
     * rights (btnSave Save, BtnPrint Print, btnUpdate Update), ERP feature 4, EmptyBagsTypeFill, GenerateCode, the combos, TicketNofill,
     * DefaultDaysToLessFromHistoryFromDate, HistoryComboFill.
     */
    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lists(u, 0);
        out.put("rights", pp.rights(u, SCREEN));
        Map<String, Object> cfg = new LinkedHashMap<>();
        for (Map<String, Object> r : pp.db().rows("Sp_ConfigrationsAllocation_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "ConfigDescription", CONFIGS, "Activity", "GetMultipleConfigurationsByConfigDescriptions")) {
            cfg.put(str(col(r, "ConfigDescription")), col(r, "ConfigKey"));
        }
        out.put("emptyBagsWeightCutEditable", toBool(cfg.get("EmptyBagsWeightCutEditableOnGRN")));
        out.put("addLessWeightEditable", toBool(cfg.get("AddLessWeightCutEditableOnGRN")));
        out.put("defaultDays", toInt(pp.lookups().config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("emptyBagTypes", pick(pp.lookups().staticColumns("PurchaseOrderEmptyBagsType"), "Id", "Type"));
        out.put("bagsConditions", pick(pp.lookups().staticColumns("EmptyBagsCondition"), "Id", "Type"));
        out.put("emptyBagItems", pick(pp.lookups().itemsByItemType(u, "14"), "Id", "ItemName"));
        out.put("docNo", docNo(u));
        out.put("historySuppliers", historySuppliers(u));
        return out;
    }

    /** BtnRefresh_Click: SupplierNameFilll, TransportFill, VehicleTypesFill, WareHouseFill, ItemNameFill, CropYear, JobLotFill,
     *  PackingTypeFill, CityFill, TicketNofill (the ticket list of the record being edited when recId is not 0). */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = pp.user(SCREEN);
        return lists(u, Math.max(0, recId));
    }

    private Map<String, Object> lists(UserAccount u, int recId) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers(u));
        out.put("transporters", transporters(u));
        out.put("vehicleTypes", pick(pp.lookups().vehicleTypes(), "Id", "VehicleDescription"));
        out.put("warehouses", pick(pp.lookups().activeWarehouses(u), "Id", "WareHouseName"));
        out.put("jobLots", pick(pp.lookups().jobLotsAll(u), "Id", "JobLotDescription"));
        out.put("items", pick(pp.lookups().readAllItems(u), "Id", "ItemName", "ItemCode=ItemCodeNew"));
        out.put("packingTypes", pick(pp.lookups().packingTypes(), "Id", "PackTypeDesc"));
        out.put("cities", pick(pp.lookups().cities(u), "Id", "CityName"));
        out.put("cropYears", pick(pp.lookups().cropYears(u), "Id", "CropYear"));
        out.put("tickets", tickets(u, recId));
        return out;
    }

    /** SupplierNameFilll(): feature 4 -> GetVendorsAndCustomers(1), else SupplierCustomerGetforComboServiceBind (Id, SupplierName, GLAccountId). */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        List<Map<String, Object>> src = pp.lookups().feature(u, 4) ? pp.lookups().vendorsAndCustomers(u, 1) : pp.lookups().supplierCustomerCombo(u);
        return pick(src, "Id", "SupplierName=CompanyName", "GLAccountId=GlAccountId");
    }

    /** TransportFill(): feature 4 -> GetVendorsAndCustomersForTransporter (Id = GLAccountId, SupplierCustomerId = Id), else the COA
     *  allocation without account types 2, 4, 10, 11, 12, 15 (SupplierCustomerId 0). */
    private List<Map<String, Object>> transporters(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (pp.lookups().feature(u, 4)) {
            for (Map<String, Object> r : pp.lookups().vendorsForTransporter(u)) {
                out.add(map("Id", col(r, "GLAccountId"), "AccountTitle", col(r, "CompanyName"), "SupplierCustomerId", col(r, "Id")));
            }
        } else {
            for (Map<String, Object> r : pp.lookups().coaAllocations(u)) {
                int t = toInt(col(r, "AccountTypeId"));
                if (t != 2 && t != 4 && t != 10 && t != 11 && t != 12 && t != 15) {
                    out.add(map("Id", col(r, "Id"), "AccountTitle", col(r, "AccountTitle"), "SupplierCustomerId", 0));
                }
            }
        }
        return out;
    }

    /** TicketNofill(): WbTransactions.GetTicketNoGrnForPurchaseFromPartyProcessing -> usp_getTicketNoGrnForPurchase
     *  (@Org @Company; @DocumentTypeId 217 and @Id only when RecId != 0). */
    private List<Map<String, Object>> tickets(UserAccount u, int recId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (recId != 0) { p.put("DocumentTypeId", DOC); p.put("Id", recId); }
        return pick(pp.db().rows("usp_getTicketNoGrnForPurchase", p), "Id", "TicketNo", "NetWbWeight");
    }

    /** GenerateCode(): InvGrn.GenerateInvGrnCode (@Org @Company @DocumentTypeId 217 @FinancialYearId 'GenerateInvGrnCode') -> DocNo. */
    private int docNo(UserAccount u) {
        List<Map<String, Object>> r = pp.db().rows(PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "FinancialYearId", nz0(pp.hrm().financialYearId()), "Activity", "GenerateInvGrnCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    public Map<String, Object> code() { return map("docNo", docNo(pp.user(SCREEN))); }

    public List<Map<String, Object>> uoms(int itemId) {
        return pick(pp.lookups().uoms(pp.user(SCREEN), itemId), "Id", "UOMCode", "Equivalent");
    }

    public Map<String, Object> loaderSetup() { return pp.loaderSetup(pp.user(SCREEN)); }

    public List<Map<String, Object>> loader(Map<String, Object> b) { return pp.loaderRows(pp.user(SCREEN), b); }

    // ------------------------------------------------------------------ history

    /** HistoryComboFill(): InvGrn.GetDataForDropDownFromGrn(@Org @Company @DocumentTypeIds "217") -> rows with Activity "Supplier". */
    private List<Map<String, Object>> historySuppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows("[dbo].[USP_GetDataForDropDownFromGrn]", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocumentTypeIds", "217")) {
            if ("Supplier".equals(str(col(r, "Activity")))) out.add(map("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
        }
        return out;
    }

    public List<Map<String, Object>> historySuppliers() { return historySuppliers(pp.user(SCREEN)); }

    /** HistoryGridFill(): InvGrn.GetHisoty -> Sp_InvGRN_GetAllMethod 'GRNFormHistory'. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        boolean all = pp.canViewAll(u, SCREEN);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOC);
        p.put("CanViewAllRecord", all);
        p.put("FinancialYearId", nz0(pp.hrm().financialYearId()));
        p.put("BranchesId", nz0(branch(u)));
        if (!all) p.put("EntryUser", u.getId());
        dateFilter(p, b, "fromDate", "toDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", "ApprovedFromDate", "ApprovedToDate");
        p.put("GrnNoF", nz0(toInt(b.get("fromDocNo"))));
        p.put("GrnNoT", nz0(toInt(b.get("toDocNo"))));
        p.put("SupplierCustomerId", nz0(toInt(b.get("supplierId"))));
        p.put("ActionId", nz0(toInt(b.get("actionId"))));
        p.put("Activity", "GRNFormHistory");
        return pick(pp.db().rows(PROC, p), "RecordNo", "Id", "InvoiceNo", "DocNo", "DocDate", "DocumentTypeId", "DeliveryTerm", "SupplierName",
                "GpNo", "VehicleNo", "BiltyNo", "WbTicketId", "TicketNo", "FactoryWeight", "WagesId", "WagesNo", "Transporter",
                "FrieghtAmount=CarriageAmount", "RemarksHeader", "EntryDate", "EntryUser", "ModifyDate", "ModifyUser", "ApprovedDate=PostDate",
                "ApprovedUser", "NoOfAttachments", "AttachmentsCount", "DetailIdsCount", "AddWages");
    }

    // ------------------------------------------------------------------ read

    /** ReadById / GrdHistory_SelectionChanged: InvGrn.GetByID -> 'ReadByID' + Sp_InvGrnDetail_GetAllMethod 'ReadByInvGrnIdDirect' +
     *  'ReadByInvGrnIdEmptyBagsDetail'. */
    public Map<String, Object> byId(int id) { return read(pp.user(SCREEN), id); }

    private Map<String, Object> read(UserAccount u, int id) {
        List<Map<String, Object>> h = pp.db().rows(PROC, "Id", id, "Activity", "ReadByID");
        if (h.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = h.get(0);
        if (toInt(col(r, "CompanyId")) != u.getCompanyId() || toInt(col(r, "OrganizationId")) != u.getOrganizationId()
                || toInt(col(r, "DocumentTypeId")) != DOC) throw invalid("Record not found");
        Map<String, Object> out = pick(h, "Id", "DocNo", "DocDate", "SupplierCustomerId", "DeliveryTerm", "TransporterId", "CarriageAmount",
                "RemarksHeader", "GpNo", "VehicleType", "VehicleNo", "BiltyNo", "FactoryWeight", "AddWages").get(0);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : pp.db().rows("Sp_InvGrnDetail_GetAllMethod", "Id", id, "Activity", "ReadByInvGrnIdDirect")) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", col(d, "Id"));
            x.put("RefDocumentTypeId", col(d, "RefDocumentTypeId"));
            x.put("RefDocIdNo", col(d, "RefDocNoId"));
            x.put("RefDocSubIdNo", col(d, "RefDocSubIdNo"));
            x.put("WarehouseId", col(d, "WareHouseFromId"));
            x.put("Warehouse", col(d, "WareHouseFromCode"));
            x.put("WarehouseToId", col(d, "WarehouseId"));
            x.put("WarehouseTo", col(d, "WareHouseCode"));
            x.put("ItemId", col(d, "ItemId"));
            x.put("ItemCode", col(d, "ItemCode"));
            x.put("ItemName", col(d, "Item"));
            x.put("CropYear", col(d, "CropYear"));
            x.put("JobLotId", col(d, "JobLotId"));
            x.put("JobLot", col(d, "JobLot"));
            x.put("PackingTypeId", col(d, "PackingTypeId"));
            x.put("PackingType", col(d, "PackingType"));
            x.put("UOMId", col(d, "ItemUomId"));
            x.put("UOM", col(d, "UOMCode"));
            x.put("UOMEquivalent", col(d, "UOM"));
            x.put("Qty", col(d, "ItemQty"));
            x.put("GrossWight", col(d, "GrossWeight"));
            x.put("EbUnit", col(d, "EBWPerUnit"));
            x.put("EbTotal", col(d, "EBWTotal"));
            x.put("AddLesswt", col(d, "AdLsWeight"));
            x.put("NetWeight", col(d, "NetBillWeight"));
            x.put("StockWeight", col(d, "StockWeight"));
            x.put("BalWeight", col(d, "StockWeight"));
            x.put("CityId", col(d, "CityId"));
            x.put("City", col(d, "AreaCity"));
            x.put("WbTicketId", col(d, "WbTicketId"));
            x.put("OrderId", col(d, "PurchaseOrderId"));
            x.put("OrderNo", col(d, "PurchaseOrder"));
            det.add(x);
        }
        out.put("details", det);
        out.put("emptyBags", pick(pp.db().rows("Sp_InvGrnDetail_GetAllMethod", "Id", id, "Activity", "ReadByInvGrnIdEmptyBagsDetail"),
                "OrderId=PurchaseOrderId", "Type=TypeId", "ItemId", "Condition=BagsCondition", "RecQty=ReceivedQty", "PurQty=PurchaseQty", "Remarks"));
        return out;
    }

    // ------------------------------------------------------------------ save

    /** Insert(): FormValidation(), "Grid Record Not Found", the transporter / freight checks, the detail row checks, the weight
     *  check, the empty bags checks and InvGrn.Save. The confirm boxes run on the page. */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.hrm().require(u, SCREEN, recId > 0 ? "Update" : "Save");
        if (recId > 0) read(u, recId);

        String docNoText = trim(b.get("docNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw invalid("DocNo Field is Required");
        int supplierId = toInt(b.get("supplierId"));
        List<Map<String, Object>> suppliers = suppliers(u);
        if (supplierId == 0 || !has(suppliers, "Id", supplierId)) throw invalid("Supplier Field is Required");
        String term = trim(b.get("deliveryTerm"));
        int termId = toInt(b.get("deliveryTermId"));
        if (term.isEmpty() || "0".equals(term) || termId == 0) throw invalid("DeliveryTerm Field is Required");
        double factory = d(trim(b.get("factoryWeight")));
        if (factory == 0.0) throw invalid("Factory Weight Field is Required");
        List<Map<String, Object>> rows = PpBGrnGdnStoreService.list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");

        boolean feature4 = pp.lookups().feature(u, 4);
        PpBModels.InvGrn g = new PpBModels.InvGrn();
        g.Id = recId;
        double carriage = d(trim(b.get("carriageAmount")));
        int transporterId = toInt(b.get("transporterId"));
        if (carriage > 0.0) {
            Map<String, Object> t = null;
            for (Map<String, Object> r : transporters(u)) if (toInt(r.get("Id")) == transporterId) t = r;
            if (transporterId == 0 || t == null) throw invalid("Transporter Account field required");
            g.TransporterId = transporterId;
            g.CarriageAmount = carriage;
            g.TransporterSupCustId = feature4 ? toInt(t.get("SupplierCustomerId")) : 0;
        } else if (transporterId > 0) {
            throw invalid("Frieght field required");
        }
        LocalDateTime now = LocalDateTime.now();
        g.OrganizationId = u.getOrganizationId();
        g.CompanyId = u.getCompanyId();
        g.BranchesId = branch(u);
        g.ProjectsId = branch(u);
        g.FinancialYearId = pp.hrm().financialYearId();
        g.DocumentTypeId = DOC;
        g.ScreenName = "GRNForPurchaseFromPartyProcessing";
        g.EntryDate = now;
        g.ModifyDate = now;
        g.PostDate = now;
        g.EntryUser = u.getId();
        g.ModifyUser = u.getId();
        g.PostUser = u.getId();
        g.DocNo = toInt(docNoText);
        LocalDateTime docDate = toDate(b.get("docDate"));
        g.DocDate = docDate == null ? now : docDate;
        g.SupplierCustomerId = supplierId;
        g.DeliveryTerm = term;
        g.RemarksHeader = trim(b.get("remarks"));
        g.GpNo = toInt(trim(b.get("gpNo")));
        g.VehicleType = str(b.get("vehicleType"));
        g.VehicleNo = trim(b.get("vehicleNo"));
        g.BiltyNo = trim(b.get("biltyNo"));
        g.FactoryWeight = factory;
        g.AddWages = toBool(b.get("addWages"));
        g.AttachmentsValues = null;
        g.CustomAttachmentsValues = null;

        int ticketId = toInt(b.get("ticketId"));
        if (ticketId > 0 && !has(tickets(u, recId), "Id", ticketId)) throw invalid("Record not found...");

        List<Map<String, Object>> avail = null;
        List<PpBModels.InvGrnDetail> details = new ArrayList<>();
        double gross = 0, grnQty = 0;
        boolean otherThanOpenBulk = false;
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            String rn = " in Detail in row No : " + (i + 1);
            if (toInt(r.get("WarehouseId")) == 0) throw invalid("Warehouse Required" + rn);
            if (toInt(r.get("WarehouseToId")) == 0) throw invalid("WarehouseTo Required" + rn);
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Required" + rn);
            if (str(r.get("CropYear")).isEmpty()) throw invalid("CropYear Required" + rn);
            if (toInt(r.get("JobLotId")) == 0) throw invalid("JobLot Required" + rn);
            if (toInt(r.get("PackingTypeId")) == 0) throw invalid("PackingType Required" + rn);
            if (toInt(r.get("UOMId")) == 0) throw invalid("UOM Required" + rn);
            if (d(r.get("Qty")) == 0.0) throw invalid("Qty Required" + rn);
            if (d(r.get("GrossWight")) == 0.0) throw invalid("GrossWight Required" + rn);
            if (d(r.get("NetWeight")) == 0.0) throw invalid("NetWeight Required" + rn);
            if (d(r.get("StockWeight")) == 0.0) throw invalid("StockWeight Required" + rn);
            PpBModels.InvGrnDetail x = new PpBModels.InvGrnDetail();
            x.Id = toInt(r.get("Id"));
            x.RefDocNoId = toInt(r.get("RefDocIdNo"));
            x.RefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
            x.RefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
            x.WareHouseFromId = toInt(r.get("WarehouseId"));
            x.WarehouseId = toInt(r.get("WarehouseToId"));
            x.ItemId = toInt(r.get("ItemId"));
            x.CropYear = str(r.get("CropYear"));
            x.JobLotId = toInt(r.get("JobLotId"));
            x.PackingTypeId = toInt(r.get("PackingTypeId"));
            x.ItemUomId = toInt(r.get("UOMId"));
            x.ItemQty = d(r.get("Qty"));
            if (x.PackingTypeId != 5) { otherThanOpenBulk = true; grnQty += x.ItemQty; }
            x.GrossWeight = d(r.get("GrossWight"));
            gross += x.GrossWeight;
            x.EBWPerUnit = d(r.get("EbUnit"));
            x.EBWTotal = d(r.get("EbTotal"));
            x.AdLsWeight = d(r.get("AddLesswt"));
            x.NetBillWeight = d(r.get("NetWeight"));
            x.StockWeight = d(r.get("StockWeight"));
            double bal = d(r.get("BalWeight"));
            boolean loaderRow = x.RefDocumentTypeId > 0 || x.RefDocNoId > 0 || x.RefDocSubIdNo > 0;
            if (recId == 0 && loaderRow) {                                    // the loader's balance, re-read for this company
                if (avail == null) avail = pp.available(u, null, null, supplierId, 0, 0, 0, 0, 0, 0, 0, 0);
                Map<String, Object> a = null;
                for (Map<String, Object> s : avail) {
                    if (toInt(s.get("RefDocumentTypeId")) == x.RefDocumentTypeId && toInt(s.get("RefDocIdNo")) == x.RefDocNoId
                            && toInt(s.get("RefDocSubIdNo")) == x.RefDocSubIdNo) a = s;
                }
                if (a == null) throw invalid("Record not found...");
                bal = d(a.get("WeightBalance"));
            }
            if (recId == 0 && x.StockWeight > bal) {
                throw invalid("StockWeight cannot greater than Bal Weight '" + net(bal) + "' Please check! in Detail Grid (Row No : " + (i + 1) + " )");
            }
            if (x.StockWeight > x.GrossWeight) {
                throw invalid("StockWeight cannot greater than Gross Weight '" + net(x.GrossWeight) + "' Please check! in Detail Grid (Row No : " + (i + 1) + " )");
            }
            x.CityId = toInt(r.get("CityId"));
            x.AreaCity = str(r.get("City"));
            x.WbTicketId = ticketId;
            details.add(x);
        }
        if (gross != factory) throw invalid("GrossWeight and FactoryWeight Weight Must be Equal ");

        List<PpBModels.InvGrnDetailEmptyBags> bags = new ArrayList<>();
        List<Map<String, Object>> ebRows = PpBGrnGdnStoreService.list(b.get("emptyBags"));
        if (!ebRows.isEmpty()) {
            double recQty = 0, purQty = 0;
            for (Map<String, Object> e : ebRows) {
                if (toInt(e.get("Type")) > 0 && toInt(e.get("ItemId")) > 0) {
                    if (toInt(e.get("Condition")) == 0) throw invalid("Please Select Bags_Condition First");
                    recQty += d(e.get("RecQty"));
                    purQty += d(e.get("PurQty"));
                }
            }
            if (otherThanOpenBulk) {
                double q = recQty + purQty;
                if (q > 0.0 && grnQty != q) {
                    throw invalid("Empty Bags Qty must be equal to GrnQty\nGrn Qty is " + net(grnQty) + " and Empty Bags Qty is " + net(q));
                }
            }
            for (Map<String, Object> e : ebRows) {
                if (toInt(e.get("ItemId")) <= 0 || toInt(e.get("Type")) <= 0) continue;
                PpBModels.InvGrnDetailEmptyBags o = new PpBModels.InvGrnDetailEmptyBags();
                o.PurchaseOrderId = toInt(e.get("OrderId"));
                o.TypeId = toInt(e.get("Type"));
                o.ItemId = toInt(e.get("ItemId"));
                o.BagsCondition = toInt(e.get("Condition"));
                o.ReceivedQty = d(e.get("RecQty"));
                o.PurchaseQty = d(e.get("PurQty"));
                o.Remarks = str(e.get("Remarks"));
                if (o.ReceivedQty > 0.0 || o.PurchaseQty > 0.0) bags.add(o);
                switch (o.TypeId) {
                    case 1: if (recQty == 0.0 && purQty == 0.0) throw invalid("Received Qty Or Purchase Qty Required in EmptyBags grid"); break;
                    case 2: case 3: if (purQty == 0.0) throw invalid("Purchase Qty Required in EmptyBags grid"); break;
                    case 4: case 5: if (recQty == 0.0) throw invalid("Received Qty Required in EmptyBags grid"); break;
                    default: break;
                }
                if (o.TypeId > 0 && grnQty != recQty + purQty) throw invalid("Empty Bags Quantity must be equal to GrnQty");
            }
        }

        // InvGrn.Save
        if (g.Id == 0) {
            g.ActionId = 1;
            for (PpBModels.InvGrnDetail x : details) if (x.Id > 0) throw invalid("Record cannot be inserted because detailId greater than zero");
        } else {
            g.ActionId = 2;
            List<Integer> own = new ArrayList<>();
            for (Map<String, Object> d0 : PpBGrnGdnStoreService.list(read(u, recId).get("details"))) own.add(toInt(d0.get("Id")));
            for (PpBModels.InvGrnDetail x : details) if (x.Id > 0 && !own.contains(x.Id)) x.Id = 0;
        }
        final List<Map<String, Object>> glItems = pp.lookups().itemGlIdsAndNames(u);
        final List<Map<String, Object>> cropList = pp.db().rows("Sp_InvCropYear_GetAllMethod", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "Activity", "GetAll");
        pp.db().tx(() -> {
            int num = pp.db().set(g.Id == 0 ? "Sp_InvGrn_Insert" : "Sp_InvGrn_Update", g);
            if (num > 0) g.Id = num; else num = g.Id;
            int line = 1;
            boolean loaderRows = false;
            for (PpBModels.InvGrnDetail x : details) {
                if (x.RefDocumentTypeId > 0 && x.RefDocNoId > 0 && x.RefDocSubIdNo > 0) loaderRows = true;
                x.LineId = line++;
                x.InvGrnId = g.Id;
                x.Id = pp.db().set("Sp_InvGrnDetail_Insert", x);
            }
            for (PpBModels.InvGrnDetailEmptyBags o : bags) {
                o.InvGrnId = g.Id;
                pp.db().set("Sp_InvGrnDetailEmptyBags_Insert", o);
            }
            PpBModels.InventoryTransactions it = new PpBModels.InventoryTransactions();
            it.OrganizationId = g.OrganizationId; it.CompanyId = g.CompanyId; it.RefDocumentTypeId = g.DocumentTypeId; it.RefDocIdNo = num;
            pp.db().set("Sp_InventoryTransactions_GetALLMethod", it);
            if (!loaderRows) fifo(u, g, details, glItems, cropList);
            else {
                PpBModels.InventoryTransactionsPartyProcessing t = new PpBModels.InventoryTransactionsPartyProcessing();
                t.OrganizationId = g.OrganizationId; t.CompanyId = g.CompanyId; t.RefDocumentTypeId = g.DocumentTypeId; t.RefDocIdNo = num;
                pp.db().set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
            }
            for (PpBModels.InvGrnDetail x : details) {
                pp.db().rows("USP_InventoryValidationPartyProcessing", "OrganizationId", g.OrganizationId, "CompanyId", g.CompanyId,
                        "DocumentTypeId", g.DocumentTypeId, "DocDate", Timestamp.valueOf(g.DocDate), "StockPartyId", g.SupplierCustomerId,
                        "ItemId", x.ItemId, "WarehouseId", x.WareHouseFromId, "JobLotId", x.JobLotId, "CropYear", x.CropYear,
                        "InvPackingTypeId", x.PackingTypeId, "PackUomId", x.ItemUomId, "NetWeight", x.NetBillWeight);
            }
            return num;
        });
        return saved(g.Id, (recId > 0 ? "Record Update Successfully [" : "Record Save Successfully [") + g.DocNo + "]");
    }

    /** DAL 0429 SetData, DocumentTypeId 217 without loader rows: FIFOImplementionForPartyProcessing per detail row (the supplier is the
     *  stock party, Warehouse From the warehouse, the crop year id by the crop year text) and the stock rows. */
    private void fifo(UserAccount u, PpBModels.InvGrn g, List<PpBModels.InvGrnDetail> details, List<Map<String, Object>> glItems,
                      List<Map<String, Object>> cropList) {
        List<PpOut> reserve = new ArrayList<>();
        List<PpBModels.TransPpFifo> out = new ArrayList<>();
        Timestamp docDate = Timestamp.valueOf(g.DocDate);
        boolean upd = g.ModifyUser > 0;                       // the form always sets ModifyUser: true on insert as well (desktop behaviour)
        for (PpBModels.InvGrnDetail x : details) {
            String name = itemName(glItems, x.ItemId);
            if (name == null) continue;
            int cropYearId = 0;
            for (Map<String, Object> c : cropList) if (str(col(c, "CropYear")).equals(str(x.CropYear))) { cropYearId = toInt(col(c, "Id")); break; }
            List<PpOut> got = pp.fifoPartyProcessing(u, x.ItemId, g.SupplierCustomerId, docDate, x.ItemUomId, x.WareHouseFromId, x.JobLotId,
                    x.PackingTypeId, cropYearId, upd ? g.DocumentTypeId : 0, upd ? g.Id : 0, x.ItemQty, x.StockWeight, x.LineId, name, reserve);
            for (PpOut o : got) {
                PpBModels.TransPpFifo t = toFifoModel(o);
                t.RefWarehouseId = x.WarehouseId;
                t.CityId = x.CityId;
                t.GpNo = g.GpNo;
                t.VehicleNo = g.VehicleNo;
                t.TranRemarks = "Grn For Purchase Party Processing";
                t.GrossWeight = x.GrossWeight;
                t.EbUnit = x.EBWPerUnit;
                t.EbTotal = x.EBWTotal;
                t.AddLess = x.AdLsWeight;
                t.TransporterId = g.TransporterId;
                t.FreightAmount = g.CarriageAmount;
                t.DetailRemarks = x.CommentsDetail;
                out.add(t);
                reserve.add(o);
            }
        }
        if (out.isEmpty()) return;
        if (upd) {
            pp.db().rows("[dbo].[USP_InventoryPartyProcessingQtyReverseAndDeleteByReferenceId]", "OrganizationId", g.OrganizationId,
                    "CompanyId", g.CompanyId, "RefDocumentTypeId", g.DocumentTypeId, "RefDocIdNo", g.Id);
        }
        for (PpBModels.TransPpFifo t : out) {
            t.OrganizationId = g.OrganizationId;
            t.CompanyId = g.CompanyId;
            t.DocDate = g.DocDate;
            t.DocCodeNo = g.DocNo;
            t.StockPartyId = g.SupplierCustomerId;
            t.BranchesId = g.BranchesId;
            t.RefDocumentTypeId = g.DocumentTypeId;
            t.CalcType = "Weight";
            t.EntryUserId = g.EntryUser;
            t.ModifyUserId = g.ModifyUser;
            t.EntryDate = LocalDateTime.now();
            t.ModifyDate = LocalDateTime.now();
            for (PpBModels.InvGrnDetail x : details) {
                if (x.LineId == t.LineId && x.LineId > 0 && t.LineId > 0) { t.RefDocIdNo = x.InvGrnId; t.RefDocSubIdNo = x.Id; break; }
            }
            pp.db().set("USP_InventoryTransactionsPartyProcessing_Insert", t);
        }
    }

    // ------------------------------------------------------------------ delete / print

    /** btnDelete_Click: "Record Not Found" / InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete -> "Delete Record Successfully". */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.hrm().require(u, SCREEN, "Delete");
        if (id <= 0) throw invalid("Record Not Found");
        read(u, id);
        pp.db().tx(() -> pp.db().rows("Sp_InvoicesVouchersandStocksDelete", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "DocumentTypeId", DOC, "UserId", u.getId()));
        return saved(id, "Delete Record Successfully");
    }

    /** GenerateReport(PrintId) -> CommonServices.GrnSlipWithSubReports(Id, 217): the Print right and the record are checked here; the page
     *  then prints the 211 slip (key grn-211, Sp_InvGrn_RiceSlip_Rpt @Id with the empty bags sub report). */
    public Map<String, Object> slip(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.hrm().require(u, SCREEN, "Print");
        read(u, id);
        return map("id", id);
    }
}
