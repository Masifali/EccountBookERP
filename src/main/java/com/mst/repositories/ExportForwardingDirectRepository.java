package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.TreeMap;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Data layer of two desktop forms that save ExImForwarding through the same BLL / DAL:
 *
 *   195 EximForwardingDirect             "Forwarding Direct (Not Use)"               DocumentTypeId 206
 *   196 EximForwardingWithoutWeighBridge "Forwarding Without WeighBridge (Not Use)"  DocumentTypeId 205
 *
 * Calls (procedures and parameters read from procdure.utf8.sql, 01-Oct-2026):
 *   Sp_ExImForwarding_GetAllMethod 'GenerateDocNoCompanyIdOrganizationId' @OrganizationId @CompanyId [@FinancialYearId] [@DocumentTypeId]
 *   Sp_ExImForwarding_GetAllMethod 'ReadById' (196) | 'ReadByIdDirect' (195) @Id, then 'ReadByForwardingHeaderId' / 'ReadOtherItemsByHeaderId' @Id
 *   Sp_ExImForwarding_GetAllMethod 'ExImForwardingFormHistory' (196) @OrganizationId @CompanyId @CanViewAllRecord [@NoOfRecords] [@FinancialYearId]
 *   Sp_ExImForwarding_GetAllMethod 'ExImForwardingFormHistoryDirect' (195) @OrganizationId @CompanyId @DocumentTypeId @CanViewAllRecord [@NoOfRecords] [@FinancialYearId]
 *   Sp_ExImForwarding_GetAllMethod 'GetDatByReferencenoForForwardingDirect' (195) @OrganizationId @CompanyId @DocumentTypeId @ReferenceNo @CanViewAllRecord [@NoOfRecords] [@FinancialYearId]
 *   USP_FarwardingByReferenceNoRegister (195 Register) @OrganizationId @CompanyId [@ReferencNo] [@FromDate] [@ToDate] [@DocumentTypeId] [@SupplierCustomerId]
 *                                        [@ItemId] [@CarrierTypId] [@ShippingLineId] [@ShippingAgentId] [@TransporterId] [@DestinationPortId] [@loadingPortId]
 *   Sp_SupplierCustomer_GetAllMethod 'ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents' | 'ReadByOrganizationCompanyIdForExport'
 *   Sp_Projects_GetAllMethod @MethodType='GetAll' | Sp_Branches_GetAllMethod 'GetAll' | Sp_SeaPorts_GetAllMethod 'ReadByCompanyNOrganizationId'
 *   SP_JobLot_ReadMethod 'GetAll' | Sp_InvCropYear_GetAllMethod 'ReadAll' | Sp_InvWareHouse_GetAllMethod 'GetActiveWareHouse'
 *   Sp_ExImPackMaterilaType_GetAllMethod 'ReadAll' | Sp_UOMSchedule_GetAllMethod 'ReadByItemID' @ItemId
 *   Sp_Item_GetAllMethod 'ReadAllForComboTwoColumns' (195 items) | Sp_InvLookup_GetAllMethod 'ReadByInvlookTypeId' @InvLookupTypeId=12 (195 carrier type)
 *   Sp_ExImInvoice_GetAllMethod 'GetInvoicesForForwarding' | 'GetInvoiceDetailByHeaderId' @Id | 'ReadByOrganizationCompanyId' @DocumentTypeIds='204' [@FinancialYearId]
 *                               | 'GetOtherItemByExImInvoiceId' @Id | 'ReadById' @Id + 'ReadExImInvoiceOtherItemsByHeaderId' @Id      (196)
 *   Sp_ExImExportShipingLineBooking_GetAllMethod 'ReadByInvoiceId' @OrganizationId @CompanyId @ExImInvoiceId                              (196)
 *
 * DAL ExImForwarding.SetDate (one transaction, rolled back on any error):
 *   Sp_ExImForwarding_Insert / _Update (51 parameters, the 50 non-virtual model properties) -> id
 *   Sp_ExImForwardingPackingDetail_Insert per row (LineId 1..n, ExImForwardingId) -> row id
 *   Sp_ExImForwardingOtherItems_Insert per other item
 *   ERP feature 5 on (USP_GetERPFeaturesByCompanyId): FIFO - Sp_Item_GetAllMethod 'GetItemGlIdsandItemName', USP_GetStockByFifoMethod,
 *     Sp_Item_GetAllMethod 'GetEqvilentByItemIdAndUomScheduleId', Sp_InvCropYear_GetAllMethod 'ReadById',
 *     [USP_InventoryQtyReverseAndDeleteByReferenceId on update], USP_InventoryStockEvalautionDetail_Insert per allocation;
 *     "InventoryStockEvalautionDetailslist not Fill" when nothing was allocated.
 *   feature 5 off: usp_StockEvaluationUpdateForExportForwarding @ActionId=1
 *   usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding, Sp_InventoryTransactions_GetALLMethod (InventoryTransactions model),
 *   USP_InventoryValidation per row (@NetWeight = StockWeight), usp_getBalNoOfContainersByContractId per distinct contract.
 * The FIFO allocation follows CommonServices.FIFOImplemention as ported in SaleGdnPurchaseReturnRepository.
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportForwardingDirectRepository {

    private final JdbcTemplate jdbc;

    public ExportForwardingDirectRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    private static Map<String, Object> tenant(UserAccount u) {
        return params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
    }

    private List<Map<String, Object>> act(String proc, UserAccount u, String activity) {
        Map<String, Object> p = tenant(u); p.put("Activity", activity);
        return DesktopProc.rows(jdbc, proc, p);
    }

    // ------------------------------------------------------------------ combos

    public List<Map<String, Object>> generateCode(UserAccount u, int financialYearId, int documentTypeId) {
        Map<String, Object> p = tenant(u);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        p.put("Activity", "GenerateDocNoCompanyIdOrganizationId");
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", p);
    }

    public List<Map<String, Object>> shippingParties(UserAccount u) { return act("Sp_SupplierCustomer_GetAllMethod", u, "ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents"); }

    public List<Map<String, Object>> customersForExport(UserAccount u) { return act("Sp_SupplierCustomer_GetAllMethod", u, "ReadByOrganizationCompanyIdForExport"); }

    public List<Map<String, Object>> projects(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("MethodType", "GetAll");
        return DesktopProc.rows(jdbc, "Sp_Projects_GetAllMethod", p);
    }

    public List<Map<String, Object>> branches(UserAccount u) { return act("Sp_Branches_GetAllMethod", u, "GetAll"); }

    public List<Map<String, Object>> seaPorts(UserAccount u) { return act("Sp_SeaPorts_GetAllMethod", u, "ReadByCompanyNOrganizationId"); }

    public List<Map<String, Object>> jobLots(UserAccount u) { return act("SP_JobLot_ReadMethod", u, "GetAll"); }

    public List<Map<String, Object>> cropYears(UserAccount u) { return act("Sp_InvCropYear_GetAllMethod", u, "ReadAll"); }

    public List<Map<String, Object>> warehouses(UserAccount u) { return act("Sp_InvWareHouse_GetAllMethod", u, "GetActiveWareHouse"); }

    public List<Map<String, Object>> packTypes(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ExImPackMaterilaType_GetAllMethod", params("Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    public List<Map<String, Object>> uomByItem(UserAccount u, int itemId) {
        Map<String, Object> p = tenant(u); p.put("ItemId", itemId); p.put("Activity", "ReadByItemID");
        return DesktopProc.rows(jdbc, "Sp_UOMSchedule_GetAllMethod", p);
    }

    /** CommonServices.ItemGetForComboServiceBind() - Item.GetAllbyCombobind, ItemCategoryId 0 (not sent). */
    public List<Map<String, Object>> itemsForCombo(UserAccount u) { return act("Sp_Item_GetAllMethod", u, "ReadAllForComboTwoColumns"); }

    public List<Map<String, Object>> carrierTypes(UserAccount u) {
        Map<String, Object> p = tenant(u); p.put("InvLookupTypeId", 12); p.put("Activity", "ReadByInvlookTypeId");
        return DesktopProc.rows(jdbc, "Sp_InvLookup_GetAllMethod", p);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId, the session's year (else the first row). */
    public Object financialYearStart(UserAccount u, int financialYearId) {
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", tenant(u));
        Map<String, Object> row = null;
        for (Map<String, Object> r : years) if (asInt(r.get("Id")) == financialYearId) { row = r; break; }
        if (row == null && !years.isEmpty()) row = years.get(0);
        return row == null ? null : row.get("Start_Period");
    }

    // ------------------------------------------------------------------ 196 invoice sources

    public List<Map<String, Object>> invoicesForForwarding(UserAccount u) { return act("Sp_ExImInvoice_GetAllMethod", u, "GetInvoicesForForwarding"); }

    public List<Map<String, Object>> invoiceDetail(UserAccount u, int invoiceId) {
        Map<String, Object> p = tenant(u); p.put("Id", invoiceId); p.put("Activity", "GetInvoiceDetailByHeaderId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    /** ExImInvoice.GetData(ReportsParameters { Ids = "204", FinancialYearId }). */
    public List<Map<String, Object>> invoiceData(UserAccount u, int financialYearId) {
        Map<String, Object> p = tenant(u); p.put("DocumentTypeIds", "204");
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "ReadByOrganizationCompanyId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    public List<Map<String, Object>> invoiceHeader(int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", invoiceId, "Activity", "ReadById"));
    }

    public List<Map<String, Object>> invoiceOtherItems(int invoiceId) {
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", params("Id", invoiceId, "Activity", "ReadExImInvoiceOtherItemsByHeaderId"));
    }

    public List<Map<String, Object>> otherItemCombo(UserAccount u, int invoiceId) {
        Map<String, Object> p = tenant(u); p.put("Id", invoiceId); p.put("Activity", "GetOtherItemByExImInvoiceId");
        return DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", p);
    }

    public List<Map<String, Object>> shippingBookingByInvoice(UserAccount u, int invoiceId) {
        Map<String, Object> p = tenant(u); p.put("ExImInvoiceId", invoiceId); p.put("Activity", "ReadByInvoiceId");
        return DesktopProc.rows(jdbc, "Sp_ExImExportShipingLineBooking_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ read / history / register

    public List<Map<String, Object>> header(int id, boolean direct) {
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", params("Id", id, "Activity", direct ? "ReadByIdDirect" : "ReadById"));
    }

    public List<Map<String, Object>> detail(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", params("Id", id, "Activity", "ReadByForwardingHeaderId"));
    }

    public List<Map<String, Object>> otherItems(int id) {
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", params("Id", id, "Activity", "ReadOtherItemsByHeaderId"));
    }

    /** GridBind: CanViewAllRecord = true (so @EntryUser is never sent). 195 also sends @DocumentTypeId = 206. */
    public List<Map<String, Object>> history(UserAccount u, boolean direct, int noOfRecords, int financialYearId) {
        Map<String, Object> p = tenant(u);
        if (direct) p.put("DocumentTypeId", 206);
        p.put("CanViewAllRecord", true);
        if (noOfRecords != 0) p.put("NoOfRecords", noOfRecords);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", direct ? "ExImForwardingFormHistoryDirect" : "ExImForwardingFormHistory");
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", p);
    }

    /** GrdReferenceHistoryBind (txtReferenceNo_Leave). */
    public List<Map<String, Object>> referenceHistory(UserAccount u, String referenceNo, int financialYearId) {
        Map<String, Object> p = tenant(u);
        p.put("DocumentTypeId", 206);
        p.put("ReferenceNo", referenceNo);
        p.put("CanViewAllRecord", true);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        p.put("Activity", "GetDatByReferencenoForForwardingDirect");
        return DesktopProc.rows(jdbc, "Sp_ExImForwarding_GetAllMethod", p);
    }

    public List<Map<String, Object>> register(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "USP_FarwardingByReferenceNoRegister", p);
    }

    // ------------------------------------------------------------------ save

    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", tenant(u))) {
            Object v = r.get("Id");
            if (v instanceof Number && ((Number) v).intValue() == featureId) return true;
            if (v != null && String.valueOf(v).trim().equals(String.valueOf(featureId))) return true;
        }
        return false;
    }

    /**
     * DAL ExImForwarding.SetDate. header = the 50 non-virtual ExImForwarding properties; details = the 41 packing detail
     * properties (CropYear text carried separately under "_CropYear"); returns the header id.
     */
    @Transactional(rollbackFor = Exception.class)
    public int save(UserAccount u, Map<String, Object> header, List<Map<String, Object>> details, List<Map<String, Object>> otherItems) {
        int id = ((Number) header.get("Id")).intValue();
        boolean update = id != 0;
        int num = DesktopProc.setProc(jdbc, update ? "Sp_ExImForwarding_Update" : "Sp_ExImForwarding_Insert", header);
        if (num > 0) id = num; else num = id;
        int documentTypeId = ((Number) header.get("DocumentTypeId")).intValue();

        int line = 0;
        for (Map<String, Object> d : details) {
            d.put("LineId", ++line);
            d.put("ExImForwardingId", id);
            Map<String, Object> send = new LinkedHashMap<>(d);
            send.remove("_CropYear");
            Integer detailId = DesktopProc.scalar(jdbc, "Sp_ExImForwardingPackingDetail_Insert", send);
            d.put("Id", detailId == null ? 0 : detailId);
        }
        for (Map<String, Object> o : otherItems) {
            o.put("ExImForwardingId", id);
            DesktopProc.scalar(jdbc, "Sp_ExImForwardingOtherItems_Insert", o);
        }

        boolean feature5 = erpFeature(u, 5);
        int allocated = 0;
        if (feature5 && (documentTypeId == 205 || documentTypeId == 206)) {
            allocated = fifo(u, id, update, header, details);
        } else {
            DesktopProc.rows(jdbc, "usp_StockEvaluationUpdateForExportForwarding", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                    "RefDocumentTypeId", documentTypeId, "RefDocIdNo", id, "ActionId", 1));
        }
        if (feature5 && (documentTypeId == 205 || documentTypeId == 206) && allocated == 0)
            throw new IllegalStateException("InventoryStockEvalautionDetailslist not Fill");

        DesktopProc.rows(jdbc, "usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId, "Id", id));
        DesktopProc.scalar(jdbc, "Sp_InventoryTransactions_GetALLMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", documentTypeId, "RefDocIdNo", num));
        for (Map<String, Object> d : details) {
            if (asInt(d.get("ActionTypeId")) == 3) continue;
            DesktopProc.rows(jdbc, "USP_InventoryValidation", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", documentTypeId,
                    "DocDate", header.get("DocDate"), "ItemId", d.get("ItemId"), "WarehouseId", d.get("WarehouseId"),
                    "JobLotId", d.get("LotJobId"), "CropYearId", d.get("CropYearId"), "InvPackingTypeId", d.get("PackingMaterialId"),
                    "PackUomId", d.get("UOMScheduleIdOuter"), "NetWeight", d.get("StockWeight")));
        }
        Set<Object> contracts = new LinkedHashSet<>();
        for (Map<String, Object> d : details) contracts.add(d.get("ExImLcOrderId"));
        for (Object c : contracts) DesktopProc.rows(jdbc, "usp_getBalNoOfContainersByContractId", params("ContractId", c));
        return num;
    }

    /** CommonServices.FIFOImplemention per row, then the reverse (update) and the inserts, as DAL ExImForwarding.SetDate (205/206 branch). */
    private int fifo(UserAccount u, int id, boolean update, Map<String, Object> header, List<Map<String, Object>> details) {
        Map<Integer, Map<String, Object>> items = new LinkedHashMap<>();
        Map<String, Object> ip = tenant(u); ip.put("Activity", "GetItemGlIdsandItemName");
        for (Map<String, Object> it : DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", ip)) items.putIfAbsent(asInt(it.get("Id")), it);
        List<Map<String, Object>> allocations = new ArrayList<>();
        int documentTypeId = asInt(header.get("DocumentTypeId"));
        for (Map<String, Object> d : details) {
            if (asInt(d.get("ActionTypeId")) == 3) continue;
            Map<String, Object> item = items.get(asInt(d.get("ItemId")));
            if (item == null) continue;
            String itemName = item.get("ItemName") == null ? "" : String.valueOf(item.get("ItemName"));
            Map<String, Object> fp = new LinkedHashMap<>();
            fp.put("CompanyId", u.getCompanyId());
            fp.put("OrganizationId", u.getOrganizationId());
            fp.put("ItemId", asInt(d.get("ItemId")));
            if (asInt(d.get("UOMScheduleIdOuter")) != 0) fp.put("PackUomId", asInt(d.get("UOMScheduleIdOuter")));
            fp.put("DocDate", header.get("DocDate"));
            if (asInt(d.get("WarehouseId")) != 0) fp.put("WarehouseId", asInt(d.get("WarehouseId")));
            if (asInt(d.get("LotJobId")) != 0) fp.put("JobLotId", asInt(d.get("LotJobId")));
            if (asInt(d.get("PackingMaterialId")) != 0) fp.put("PackingTypeId", asInt(d.get("PackingMaterialId")));
            Object cropYear = d.get("_CropYear");
            if (cropYear != null && !String.valueOf(cropYear).trim().isEmpty()) fp.put("CropYear", cropYear);
            if (update) { fp.put("DocumentTypeId", documentTypeId); fp.put("Id", id); }
            if (!allocations.isEmpty()) fp.put("FIFOXML", fifoXml(allocations));
            List<Map<String, Object>> stocks = DesktopProc.rows(jdbc, "USP_GetStockByFifoMethod", fp);
            if (stocks.isEmpty()) throw new IllegalStateException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
            double needWeight = asDouble(d.get("StockWeight")), needQty = asDouble(d.get("OuterQty"));
            double available = 0;
            for (Map<String, Object> s : stocks) available += asDouble(s.get("NetBalWeight"));
            double roundedAvailable = Math.abs(available) < 1e16 ? Math.rint(available * 100d) / 100d : available;
            if (!(needWeight <= roundedAvailable))
                throw new IllegalStateException("Weight available is " + available + " and row Weight is " + needWeight + " this item " + itemName + " against FIFO....");
            String cropBatch = null;
            if (asInt(d.get("CropYearId")) > 0) {
                List<Map<String, Object>> cy = DesktopProc.rows(jdbc, "Sp_InvCropYear_GetAllMethod", params("Id", asInt(d.get("CropYearId")), "Activity", "ReadById"));
                if (!cy.isEmpty() && cy.get(0).get("CropYear") != null) cropBatch = String.valueOf(cy.get(0).get("CropYear"));
            }
            double usedWeight = 0, usedQty = 0;
            for (Map<String, Object> stock : stocks) {
                double stockQty = asDouble(stock.get("NetBalQty")), stockWeight = asDouble(stock.get("NetBalWeight"));
                int rateUom = asInt(stock.get("RateUomId"));
                double avgRate = asDouble(stock.get("AvgRate"));
                if (avgRate <= 0) throw new IllegalStateException("Rate Not Found this Item " + itemName + " against FIFO Method");
                if (rateUom == 0) throw new IllegalStateException("RateUomId not found  this " + itemName + " against FIFO Method");
                Map<String, Object> ep = tenant(u); ep.put("ItemId", asInt(d.get("ItemId"))); ep.put("ScheduleId", rateUom); ep.put("Activity", "GetEqvilentByItemIdAndUomScheduleId");
                List<Map<String, Object>> eq = DesktopProc.rows(jdbc, "Sp_Item_GetAllMethod", ep);
                double equivalent = eq.isEmpty() ? 0 : asDouble(eq.get(0).get("Equivalent"));
                if (equivalent == 0) throw new IllegalStateException("RateUom Not Found");
                boolean whole = stockWeight <= needWeight - usedWeight;
                double takeWeight = whole ? stockWeight : needWeight - usedWeight;
                double takeQty = whole ? stockQty : needQty - usedQty;
                Map<String, Object> x = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
                x.put("Id", asInt(stock.get("Id")));
                x.put("OrganizationId", u.getOrganizationId());
                x.put("CompanyId", u.getCompanyId());
                x.put("DocDate", header.get("DocDate"));
                x.put("DocCodeNo", asInt(header.get("DocNo")));
                x.put("SupplierCustomerId", header.get("SupplierCustomerId"));
                x.put("BranchesId", header.get("BranchesId"));
                x.put("OtherDocumentTypeId", documentTypeId);
                x.put("OtherDocNoId", id);
                x.put("OtherSubDocNoId", asInt(d.get("Id")));
                x.put("EntryUser", header.get("EntryUser"));
                x.put("ModifyUser", header.get("ModifyUser"));
                x.put("LineId", asInt(d.get("LineId")));
                x.put("ItemId", d.get("ItemId"));
                x.put("WarehouseId", d.get("WarehouseId"));
                x.put("JobLotId", d.get("LotJobId"));
                x.put("InvPackingTypeId", d.get("PackingMaterialId"));
                x.put("ItemUom", d.get("UOMScheduleIdOuter"));
                x.put("CropBatch", cropBatch);
                x.put("InvoiceId", header.get("ExImInvoiceId"));
                x.put("InvoiceDetailId", d.get("InvoiceDetailId"));
                x.put("RefRefDocumentTypeId", stock.get("RefDocumentTypeId"));
                x.put("RefRefDocIdNo", stock.get("RefDocIdNo"));
                x.put("RefRefDocSubIdNo", stock.get("RefDocSubIdNo"));
                x.put("QtyOut", takeQty);
                x.put("BillWeightOut", takeWeight);
                x.put("StockWeightOut", takeWeight);
                x.put("CgsRate", avgRate * equivalent);
                x.put("CgsAmount", takeWeight / equivalent * (avgRate * equivalent));
                x.put("RateUom", rateUom);
                x.put("CalcType", "Weight");
                allocations.add(x);
                usedQty += takeQty; usedWeight += takeWeight;
                if (needWeight == usedWeight) break;
            }
        }
        if (!allocations.isEmpty()) {
            if (update) DesktopProc.rows(jdbc, "USP_InventoryQtyReverseAndDeleteByReferenceId", params(
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "RefDocumentTypeId", documentTypeId, "RefDocIdNo", id));
            for (Map<String, Object> a : allocations) {
                Map<String, Object> send = new LinkedHashMap<>(a);
                DesktopProc.scalar(jdbc, "USP_InventoryStockEvalautionDetail_Insert", send);
            }
        }
        return allocations.size();
    }

    private static String fifoXml(List<Map<String, Object>> reserved) {
        StringBuilder xml = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
        for (Map<String, Object> a : reserved) {
            xml.append("<FIFOStockEvaluation>")
               .append("<RefDocumentTypeId>").append(asInt(a.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
               .append("<RefDocIdNo>").append(asInt(a.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
               .append("<RefDocSubIdNo>").append(asInt(a.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
               .append("<ReserveQty>").append(asDouble(a.get("QtyOut"))).append("</ReserveQty>")
               .append("<ReserveWeight>").append(asDouble(a.get("StockWeightOut"))).append("</ReserveWeight>")
               .append("</FIFOStockEvaluation>");
        }
        return xml.append("</ArrayOfFIFOStockEvaluation>").toString();
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }

    static Timestamp now() { return new Timestamp(System.currentTimeMillis()); }
}
