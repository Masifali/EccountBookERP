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
 * BLL of Architecture.WinApp.PartyProcessing.GDNForSaleToPartyProcessing (ScreenId 615, DocumentTypeId 221).
 *
 * Save: InvGdn.Save (BLL 0575: insert ModifyUser 0 / ActionId 1 - "Record cannot be inserted because detailId greater than zero" -,
 * update EntryUser 0 / ActionId 2) -> DAL 0428 SetData, one transaction: Sp_InvGdn_Insert / _Update -> Sp_InvGdnDetail_Insert per row
 * (LineId 1..n) -> Sp_InventoryTransactions_GetALLMethod -> with ERP feature 5 the stock evaluation FIFO (USP_GetStockByFifoMethod,
 * USP_InventoryQtyReverseAndDeleteByReferenceId on update, USP_InventoryStockEvalautionDetail_Insert), otherwise
 * Sp_InventoryStockEvalautionDetail_Update -> USP_InventoryValidation per row -> usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding
 * -> Sp_InventoryTransactionsPartyProcessing_Insert. Delete: Sp_InvoicesVouchersandStocksDelete (DocumentTypeId 221).
 */
@Service
public class PpBGdnSaleService {

    public static final int SCREEN = 615;
    public static final int DOC = 221;
    private static final String PROC = "Sp_InvGdn_GetAllMethod";

    @Autowired private PpBSupport pp;

    // ------------------------------------------------------------------ Load

    /** GDNForSaleToPartyProcessing_Load: rights (btnSave Save, BtnPrint Print, btnUpdate Update), IsStockReservedPerParty, feature 4,
     *  GenerateCode, the combos, TicketNofill, DefaultDaysToLessFromHistoryFromDate, HistoryComboFill. */
    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lists(u, 0);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("isStockReservedPerParty", toBool(pp.lookups().config(u, "IsStockReservedPerParty")));
        out.put("defaultDays", toInt(pp.lookups().config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("docNo", docNo(u));
        out.put("historyCustomers", historyCustomers(u));
        return out;
    }

    public Map<String, Object> refresh(int recId) { return lists(pp.user(SCREEN), Math.max(0, recId)); }

    private Map<String, Object> lists(UserAccount u, int recId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> src = pp.lookups().feature(u, 4) ? pp.lookups().vendorsAndCustomers(u, 1) : pp.lookups().supplierCustomerCombo(u);
        out.put("customers", pick(src, "Id", "CustomerName=CompanyName", "GLAccountId=GlAccountId"));    // SupplierNameFilll()
        out.put("transporters", transporters(u));
        out.put("vehicleTypes", pick(pp.lookups().vehicleTypes(), "Id", "VehicleDescription"));
        out.put("warehouses", pick(pp.lookups().activeWarehouses(u), "Id", "WareHouseName"));
        out.put("jobLots", pick(pp.lookups().jobLotsAll(u), "Id", "JobLotDescription"));
        out.put("items", pick(pp.lookups().readAllItems(u), "Id", "ItemName", "ItemCode=ItemCodeNew"));
        List<Map<String, Object>> packs = new ArrayList<>();                                          // PackingTypeFill(): Ids 1 and 2 only
        for (Map<String, Object> r : pick(pp.lookups().packingTypes(), "Id", "PackTypeDesc")) {
            int id = toInt(r.get("Id"));
            if (id == 1 || id == 2) packs.add(r);
        }
        out.put("packingTypes", packs);
        out.put("cities", pick(pp.lookups().cities(u), "Id", "CityName"));
        out.put("cropYears", pick(pp.lookups().cropYears(u), "Id", "CropYear"));
        out.put("tickets", tickets(u, recId));
        return out;
    }

    /** TransportFill(): as the GRN for purchase form. */
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

    /** TicketNofill(): usp_getTicketNoGrnForPurchase (@Org @Company; @DocumentTypeId 221 and @Id only when RecId != 0). */
    private List<Map<String, Object>> tickets(UserAccount u, int recId) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        if (recId != 0) { p.put("DocumentTypeId", DOC); p.put("Id", recId); }
        return pick(pp.db().rows("usp_getTicketNoGrnForPurchase", p), "Id", "TicketNo", "NetWbWeight");
    }

    /** GenerateCode(): InvGdn.GenerateInvGdnCode (@Org @Company @DocumentTypeId 221 @FinancialYearId 'GenerateInvGdnCode') -> DocNo. */
    private int docNo(UserAccount u) {
        List<Map<String, Object>> r = pp.db().rows(PROC, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", DOC, "FinancialYearId", nz0(pp.hrm().financialYearId()), "Activity", "GenerateInvGdnCode");
        return r.isEmpty() ? 0 : toInt(col(r.get(0), "DocNo"));
    }

    public Map<String, Object> code() { return map("docNo", docNo(pp.user(SCREEN))); }

    public List<Map<String, Object>> uoms(int itemId) {
        return pick(pp.lookups().uoms(pp.user(SCREEN), itemId), "Id", "UOMCode", "Equivalent");
    }

    /**
     * AvailableStock(): GetAvgRatesAndStockInHand.GetStockInHandFromInventoryTrasactions -> Sp_GetAvgRatesAndStockInHand_GetAllMethod
     * (@Org @Company @ItemId @DocDate; @WarehouseId / @JobLotId / @CropYear / @ItemUomId when set; the packing type is not sent).
     */
    public Map<String, Object> stock(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("ItemId", toInt(b.get("itemId")));
        Timestamp dd = stamp(b.get("docDate"));
        p.put("DocDate", dd == null ? Timestamp.valueOf(LocalDateTime.now()) : dd);
        p.put("WarehouseId", nz0(toInt(b.get("warehouseId"))));
        p.put("JobLotId", nz0(toInt(b.get("jobLotId"))));
        p.put("CropYear", nzs(b.get("cropYear")));
        p.put("ItemUomId", nz0(toInt(b.get("uomId"))));
        p.put("Activity", "GetStockInHandFromInventoryTrasactions");
        List<Map<String, Object>> r = pp.db().rows("Sp_GetAvgRatesAndStockInHand_GetAllMethod", p);
        double v = 0;
        if (!r.isEmpty()) { Object first = r.get(0).values().isEmpty() ? null : r.get(0).values().iterator().next(); v = d(first); }
        return map("stock", v);
    }

    // ------------------------------------------------------------------ history

    /** HistoryComboFill(): InvGdn.GetDataForDropDownFromGdn (@Org @Company @DocumentTypeIds "221") -> Activity "Supplier". */
    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : pp.db().rows("USP_GetDataForDropDownFromGdn", "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "DocumentTypeIds", "221")) {
            if ("Supplier".equals(str(col(r, "Activity")))) out.add(map("Id", col(r, "Id"), "name", col(r, "ReferenceName")));
        }
        return out;
    }

    public List<Map<String, Object>> historyCustomers() { return historyCustomers(pp.user(SCREEN)); }

    /** HistoryGridFill(): InvGdn.GetHisoty -> Sp_InvGdn_GetAllMethod 'GDNFormHistory'. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        boolean all = pp.canViewAll(u, SCREEN);
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOC);
        p.put("FinancialYearId", nz0(pp.hrm().financialYearId()));
        p.put("BranchesId", nz0(branch(u)));
        dateFilter(p, b, "FromDate", "ToDate", "EntryFromDate", "EntryToDate", "ModifyFromDate", "ModifyToDate", "ApprovedFromDate", "ApprovedToDate");
        p.put("DocNoFrom", nz0(toInt(b.get("fromDocNo"))));
        p.put("DocNoTo", nz0(toInt(b.get("toDocNo"))));
        p.put("SupplierCustomerId", nz0(toInt(b.get("customerId"))));
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", u.getId());
        p.put("ActionId", nz0(toInt(b.get("actionId"))));
        p.put("Activity", "GDNFormHistory");
        return pick(pp.db().rows(PROC, p), "RecordNo", "Id", "DocumentTypeId", "DocDate", "DocNo", "InvoiceNo", "CustomerName", "GpId", "GpNo",
                "VehicleNo", "BiltyNo", "FactoryWeight", "WbTicketId", "TicketNo", "WagesId", "WagesNo", "AccountTitle", "Freight", "RemarksHeader",
                "EntryUser=UserName", "EntryDate", "ModifyUser=ModifyUserName", "ModifyDate", "ApprovedUser=ApprovedUserName", "ApprovedDate",
                "NoOfAttachments", "AddWages");
    }

    // ------------------------------------------------------------------ read

    /** ReadById: InvGdn.GetByID -> 'GetById' + DAL GetDate 'GetGDNDetailByGdnId' (@InvGdnMainId). */
    public Map<String, Object> byId(int id) { return read(pp.user(SCREEN), id); }

    private Map<String, Object> read(UserAccount u, int id) {
        List<Map<String, Object>> h = pp.db().rows(PROC, "Id", id, "Activity", "GetById");
        if (h.isEmpty()) throw invalid("Record not found");
        Map<String, Object> r = h.get(0);
        if (toInt(col(r, "CompanyId")) != u.getCompanyId() || toInt(col(r, "OrganizationId")) != u.getOrganizationId()
                || toInt(col(r, "DocumentTypeId")) != DOC) throw invalid("Record not found");
        Map<String, Object> out = pick(h, "Id", "DocNo", "DocDate", "SupplierCustomerId", "DeliveryTerm", "TransporterId", "CarriageAmount",
                "RemarksHeader", "GpNo", "VehicleType", "VehicleNo", "BiltyNo", "FactoryWeight", "AddWages", "IsStockReserved").get(0);
        out.put("details", pick(pp.db().rows(PROC, "InvGdnMainId", id, "Activity", "GetGDNDetailByGdnId"), "Id", "WarehouseId",
                "Warehouse=WareHouseCode", "WarehouseToId=WareHouseToId", "WarehouseTo=WareHouseToCode", "ItemId", "ItemCode", "ItemName=Item",
                "CropYear", "JobLotId", "JobLot", "PackingTypeId", "PackingType", "UOMId=ItemUomId", "UOM=UOMCode", "UOMEquivalent", "Qty=ItemQty",
                "GrossWight=GrossWeight", "EbUnit=EBWPerUnit", "EbTotal=EBWTotal", "AddLesswt=AdLsWeight", "NetWeight=NetBillWeight", "StockWeight",
                "CityId", "City=AreaCity", "WbTicketId"));
        return out;
    }

    // ------------------------------------------------------------------ save

    /** Insert(): FormValidation(), "Grid Record Not Found", the transporter / freight checks, the detail row checks, the weight check and
     *  InvGdn.Save. The confirm boxes run on the page. */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.hrm().require(u, SCREEN, recId > 0 ? "Update" : "Save");
        Map<String, Object> existing = recId > 0 ? read(u, recId) : null;

        String docNoText = trim(b.get("docNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw invalid("DocNo Field is Required");
        Map<String, Object> ls = lists(u, recId);
        int customerId = toInt(b.get("customerId"));
        if (customerId == 0 || !has(PpBGrnGdnStoreService.list(ls.get("customers")), "Id", customerId)) throw invalid("Customer Field is Required");
        String term = trim(b.get("deliveryTerm"));
        if (term.isEmpty() || "0".equals(term) || toInt(b.get("deliveryTermId")) == 0) throw invalid("DeliveryTerm Field is Required");
        double factory = d(trim(b.get("factoryWeight")));
        if (factory == 0.0) throw invalid("Factory Weight Field is Required");
        List<Map<String, Object>> rows = PpBGrnGdnStoreService.list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Grid Record Not Found");

        PpBModels.InvGdn g = new PpBModels.InvGdn();
        g.Id = recId;
        double carriage = d(trim(b.get("carriageAmount")));
        int transporterId = toInt(b.get("transporterId"));
        if (carriage > 0.0) {
            Map<String, Object> t = null;
            for (Map<String, Object> r : PpBGrnGdnStoreService.list(ls.get("transporters"))) if (toInt(r.get("Id")) == transporterId) t = r;
            if (transporterId == 0 || t == null) throw invalid("Transporter Account field required");
            g.TransporterId = transporterId;
            g.CarriageAmount = carriage;
            g.TransporterSupCustId = pp.lookups().feature(u, 4) ? toInt(t.get("SupplierCustomerId")) : 0;
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
        g.ScreenName = "GDNForSaleToPartyProcessing";
        g.EntryDate = now;
        g.ModifyDate = now;
        g.PostDate = now;
        g.GPDate = now;
        g.EntryUser = u.getId();
        g.ModifyUser = u.getId();
        g.PostUser = u.getId();
        g.DocNo = toInt(docNoText);
        LocalDateTime docDate = toDate(b.get("docDate"));
        g.DocDate = docDate == null ? now : docDate;
        g.SupplierCustomerId = customerId;
        g.DeliveryTerm = term;
        g.RemarksHeader = trim(b.get("remarks"));
        g.GpNo = toInt(trim(b.get("gpNo")));
        g.VehicleType = str(b.get("vehicleType"));
        g.VehicleNo = trim(b.get("vehicleNo"));
        g.BiltyNo = trim(b.get("biltyNo"));
        g.FactoryWeight = factory;
        g.AddWages = toBool(b.get("addWages"));
        if (toBool(pp.lookups().config(u, "IsStockReservedPerParty"))) g.IsStockReserved = toBool(b.get("isStockReserved"));

        int ticketId = toInt(b.get("ticketId"));
        if (ticketId > 0 && !has(PpBGrnGdnStoreService.list(ls.get("tickets")), "Id", ticketId)) throw invalid("Record not found...");

        List<Integer> own = new ArrayList<>();
        if (existing != null) for (Map<String, Object> d0 : PpBGrnGdnStoreService.list(existing.get("details"))) own.add(toInt(d0.get("Id")));
        List<PpBModels.InvGdnDetail> details = new ArrayList<>();
        double gross = 0;
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
            PpBModels.InvGdnDetail x = new PpBModels.InvGdnDetail();
            x.Id = toInt(r.get("Id"));
            if (x.Id > 0 && !own.contains(x.Id)) x.Id = 0;
            x.WarehouseId = toInt(r.get("WarehouseId"));
            x.WareHouseToId = toInt(r.get("WarehouseToId"));
            x.ItemId = toInt(r.get("ItemId"));
            x.CropYear = str(r.get("CropYear"));
            x.JobLotId = toInt(r.get("JobLotId"));
            x.PackingTypeId = toInt(r.get("PackingTypeId"));
            x.ItemUomId = toInt(r.get("UOMId"));
            x.ItemQty = d(r.get("Qty"));
            x.GrossWeight = d(r.get("GrossWight"));
            gross += x.GrossWeight;
            x.EBWPerUnit = d(r.get("EbUnit"));
            x.EBWTotal = d(r.get("EbTotal"));
            x.AdLsWeight = d(r.get("AddLesswt"));
            x.NetBillWeight = d(r.get("NetWeight"));
            x.StockWeight = d(r.get("StockWeight"));
            x.CityId = toInt(r.get("CityId"));
            x.AreaCity = str(r.get("City"));
            x.WbTicketId = ticketId;
            x.GpDate = now;
            details.add(x);
        }
        if (gross != factory) throw invalid("GrossWeight and FactoryWeight Weight Must be Equal ");

        // InvGdn.Save
        if (g.Id == 0) {
            for (PpBModels.InvGdnDetail x : details) if (x.Id > 0) throw invalid("Record cannot be inserted because detailId greater than zero");
            g.ModifyUser = 0;
            g.ActionId = 1;
        } else {
            g.EntryUser = 0;
            g.ActionId = 2;
        }
        final boolean feature5 = pp.lookups().feature(u, 5);
        final List<Map<String, Object>> glItems = feature5 ? pp.lookups().itemGlIdsAndNames(u) : null;
        pp.db().tx(() -> {
            int num = pp.db().set(g.Id == 0 ? "Sp_InvGdn_Insert" : "Sp_InvGdn_Update", g);
            if (num > 0) g.Id = num; else num = g.Id;
            int line = 0;
            for (PpBModels.InvGdnDetail x : details) {
                x.LineId = ++line;
                x.InvGdnId = g.Id;
                x.Id = pp.db().set("Sp_InvGdnDetail_Insert", x);
            }
            PpBModels.InventoryTransactions it = new PpBModels.InventoryTransactions();
            it.OrganizationId = g.OrganizationId; it.CompanyId = g.CompanyId; it.RefDocumentTypeId = g.DocumentTypeId; it.RefDocIdNo = g.Id;
            pp.db().set("Sp_InventoryTransactions_GetALLMethod", it);
            if (feature5) fifo(u, g, details, glItems);
            else {
                PpBModels.StockEvalautionDetail e = new PpBModels.StockEvalautionDetail();
                e.OrganizationId = g.OrganizationId; e.CompanyId = g.CompanyId; e.RefDocumentTypeId = g.DocumentTypeId; e.RefDocIdNo = g.Id;
                pp.db().set("Sp_InventoryStockEvalautionDetail_Update", e);
            }
            for (PpBModels.InvGdnDetail x : details) {
                pp.db().rows("USP_InventoryValidation", "OrganizationId", g.OrganizationId, "CompanyId", g.CompanyId, "DocumentTypeId", g.DocumentTypeId,
                        "DocDate", Timestamp.valueOf(g.DocDate), "ItemId", x.ItemId, "WarehouseId", x.WarehouseId, "JobLotId", x.JobLotId,
                        "CropYear", x.CropYear, "InvPackingTypeId", x.PackingTypeId, "PackUomId", x.ItemUomId, "NetWeight", x.StockWeight,
                        "RefDocumentTypeId", x.RefDocumentTypeId, "RefDocNoId", x.RefDocIdNo, "RefDocSubIdNo", x.RefDocSubIdNo,
                        "ItemConditionId", x.ItemConditionId);
            }
            pp.db().rows("usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding", "OrganizationId", g.OrganizationId, "CompanyId", g.CompanyId,
                    "DocumentTypeId", g.DocumentTypeId, "Id", g.Id);
            PpBModels.InventoryTransactionsPartyProcessing t = new PpBModels.InventoryTransactionsPartyProcessing();
            t.OrganizationId = g.OrganizationId; t.CompanyId = g.CompanyId; t.RefDocumentTypeId = g.DocumentTypeId; t.RefDocIdNo = num;
            pp.db().set("Sp_InventoryTransactionsPartyProcessing_Insert", t);
            return num;
        });
        return saved(g.Id, (recId > 0 ? "Record Update Successfully [" : "Record Save Successfully [") + g.DocNo + "]");
    }

    /** DAL 0428 SetData with ERP feature 5: CommonServices.FIFOImplemention per detail row and USP_InventoryStockEvalautionDetail_Insert. */
    private void fifo(UserAccount u, PpBModels.InvGdn g, List<PpBModels.InvGdnDetail> details, List<Map<String, Object>> glItems) {
        List<PpBModels.StockEvalautionDetail> all = new ArrayList<>();
        Timestamp docDate = Timestamp.valueOf(g.DocDate);
        boolean upd = g.ModifyUser > 0;
        for (PpBModels.InvGdnDetail x : details) {
            String name = itemName(glItems, x.ItemId);
            if (name == null) continue;
            for (PpBModels.StockEvalautionDetail e : pp.fifo(u, x.ItemId, docDate, x.ItemUomId, x.WarehouseId, 0, x.JobLotId, x.PackingTypeId,
                    x.CropYear, upd ? g.DocumentTypeId : 0, upd ? g.Id : 0, x.ItemQty, x.StockWeight, x.LineId, name, all)) {
                e.CityId = x.CityId;
                all.add(e);
            }
        }
        if (all.isEmpty()) return;
        if (upd) {
            pp.db().rows("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", "OrganizationId", g.OrganizationId, "CompanyId", g.CompanyId,
                    "RefDocumentTypeId", g.DocumentTypeId, "RefDocIdNo", g.Id);
        }
        for (PpBModels.StockEvalautionDetail e : all) {
            e.OrganizationId = g.OrganizationId;
            e.CompanyId = g.CompanyId;
            e.DocDate = g.DocDate;
            e.DocCodeNo = g.DocNo;
            e.VehicleNo = g.VehicleNo;
            e.GpNoDcNo = g.GpNo;
            e.BiltyNo = g.BiltyNo;
            e.SupplierCustomerId = g.SupplierCustomerId;
            e.BranchesId = g.BranchesId;
            e.OtherDocumentTypeId = g.DocumentTypeId;
            e.EntryUser = g.EntryUser;
            e.ModifyUser = g.ModifyUser;
            e.CalcType = "Weight";
            for (PpBModels.InvGdnDetail x : details) {
                if (x.LineId == e.LineId && x.LineId > 0 && e.LineId > 0) { e.OtherDocNoId = x.InvGdnId; e.OtherSubDocNoId = x.Id; break; }
            }
            pp.db().set("USP_InventoryStockEvalautionDetail_Insert", e);
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

    /** GenerateReport(PrintId) -> CommonServices.GdnForSaleToPartyProcssingSlip221(Id, 221): that CommonServices body and its procedure are
     *  not in the available sources, so the page prints the saved document itself (this record) through the grid-to-PDF printer. */
    public Map<String, Object> slip(int id) {
        UserAccount u = pp.user(SCREEN);
        pp.hrm().require(u, SCREEN, "Print");
        return read(u, id);
    }
}
