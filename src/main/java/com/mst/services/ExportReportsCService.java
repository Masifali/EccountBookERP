package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportReportsCRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.ExportReportsCRepository.ci;

/**
 * The BLL side of eleven Export report screens (agent C):
 *
 *   241 FcyReceiptsSummaryRegister           246 FIBalanceSummary            258 Commercial_Invoice_Shipments
 *   242 ShipmentdataForBrokeryTax            256 CommissionAgentFcyLedger    203 frmshippedConsignment_followup
 *   243 PackingListRegister_Export           257 EEReport_ExportGD           859 frmThirdPartyInspectionLotTrackingReport
 *   244 PendingForwardingForCommercialInvoice 245 GDBreakUpandRealized_Register
 *
 * Every method takes the ScreenDefinition.Id so DesktopReportRights.require(user, screen, action)
 * checks the real chain; "View" for loads and grids, "Save" / "Update" for the two forms that write
 * (257 EE statements from the grid, 203 follow-up header + grid updates). Tenancy (organisation,
 * company, branch, user, financial year) always comes from the session. Row projections keep the
 * desktop's projected DataTable column names, in the desktop's order, so the pages' grids match.
 */
@Service
public class ExportReportsCService {

    public static final int FCY_RECEIPTS = 241, BROKERY_TAX = 242, PACKING_LIST_REGISTER = 243, PENDING_FORWARDING = 244,
            GD_REALIZED = 245, FI_BALANCE = 246, COMM_AGENT_LEDGER = 256, EE_REPORT = 257, CI_SHIPMENTS = 258,
            CONSIGNMENT_FOLLOWUP = 203, LOT_TRACKING = 859;

    @Autowired private ExportReportsCRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(int screen, String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, screen, action);
        return u;
    }

    private boolean allowed(UserAccount u, int screen, String action) {
        try { rights.require(u, screen, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** CommonServices.DateType() - a fixed DataTable on the desktop (Id / Parameters). */
    private static List<Map<String, Object>> dateTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        String[] names = { "This Day", "This Week", "This Month", "This Year", "Financial Year" };
        for (int i = 0; i < names.length; i++) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", i + 1); m.put("Parameters", names[i]); out.add(m); }
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period as yyyy-MM-dd ("" when unknown). */
    private String yearStart(UserAccount u) {
        String s = repo.yearStart(u, currentUserContext.currentFinancialYearId());
        return iso(s);
    }

    private static Map<String, Object> common(UserAccount u, String yearStart) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("dateTypes", dateTypes());
        out.put("yearStart", yearStart);
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** Id/Name pairs of the rows whose {@code typeCol} equals {@code type}. */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String typeCol, String type, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!type.equals(text(ci(r, typeCol)))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, idCol));
            m.put("Name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 241 Fcy Receipts Summary Register

    /** FcyReceiptsSummaryRegister_Load: ParameterFill + AllComboBind (the combos), Rows[2] = "This Month" is the page's default. */
    public Map<String, Object> fcyReceiptsSetup() {
        UserAccount u = user(FCY_RECEIPTS, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.putAll(fcyReceiptsCombos(u));
        return out;
    }

    /** AllComboBind - "No Data Found For Binding Combos" when the procedure returns nothing. */
    public Map<String, Object> fcyReceiptsCombos() { return fcyReceiptsCombos(user(FCY_RECEIPTS, "View")); }

    private Map<String, Object> fcyReceiptsCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> dt = repo.fcyReceiptDropDowns(u);
        if (dt.isEmpty()) { out.put("combosError", "No Data Found For Binding Combos"); dt = new ArrayList<>(); }
        out.put("senders", pick(dt, "Activity", "Customer", "Id", "ReferenceName"));
        out.put("receivers", pick(dt, "Activity", "ReceiverAccount", "Id", "ReferenceName"));
        out.put("invoices", pick(dt, "Activity", "InvoiceNo", "Id", "ReferenceName"));
        out.put("paymentTerms", pick(dt, "Activity", "PaymentTerm", "Id", "ReferenceName"));
        return out;
    }

    /**
     * GridBind: dates always sent (DateTimePicker values), Sender = CmbSender.Value, Receiver = CmbReceiver.Value,
     * Invoice = CmbInvoiceNo.Value, PaymentTerm = CmbPaymentTerm.TEXT (the caption, not the id - desktop quirk).
     */
    public List<Map<String, Object>> fcyReceiptsShow(Map<String, Object> f) {
        UserAccount u = user(FCY_RECEIPTS, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.fcyReceiptsSummaryRegister(u, date(f.get("fromDate")), date(f.get("toDate")),
                asInt(f.get("senderId")), asInt(f.get("receiverId")), asInt(f.get("invoiceId")), text(f.get("paymentTerm")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("VoucherDate", iso(ci(r, "VoucherDate")));
            m.put("VoucherNo", text(ci(r, "VoucherNo")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("Invoice/Contract", text(ci(r, "ExImInvoiceNo")));
            m.put("CurrencyName", text(ci(r, "CurrencyName")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("SenderAccountTitle", text(ci(r, "SenderAccountTitle")));
            m.put("FcySenderCr", asDouble(ci(r, "FcySenderCr")));
            m.put("LcySenderCr", asDouble(ci(r, "LcySenderCr")));
            m.put("ReceiverAccountTitle", text(ci(r, "ReceiverAccountTitle")));
            m.put("FcyReceiverDr", asDouble(ci(r, "FcyReceiverDr")));
            m.put("LcyReceiverDr", asDouble(ci(r, "LcyReceiverDr")));
            m.put("FcyDecutionsDr", asDouble(ci(r, "FcyDecutionsDr")));
            m.put("LcyDecutionsDr", asDouble(ci(r, "LcyDecutionsDr")));
            m.put("FcyGdsAdjusted", asDouble(ci(r, "FcyGdsAdjusted")));
            m.put("FcyGdsCommission", asDouble(ci(r, "FcyGdsCommission")));
            m.put("FcyGdsRealized", asDouble(ci(r, "FcyGdsRealized")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 242 Shipment Data For Brokery Tax

    public Map<String, Object> brokeryTaxSetup() {
        UserAccount u = user(BROKERY_TAX, "View");
        return common(u, yearStart(u));
    }

    /** GridBind - also the Total Lcy Diff (Sum(LcyDiffAmount)) for the "Extra Information" box. */
    public Map<String, Object> brokeryTaxShow(Map<String, Object> f) {
        UserAccount u = user(BROKERY_TAX, "View");
        List<Map<String, Object>> rows = new ArrayList<>();
        double total = 0;
        for (Map<String, Object> r : repo.shipmentDataForBrokeryTax(u, dateOrToday(f.get("fromDate")), dateOrToday(f.get("toDate")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("InvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("PTYInvoiceNo", text(ci(r, "PTYInvoiceNo")));
            m.put("FCL", asDouble(ci(r, "FCL")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("InvoiceAmount", asDouble(ci(r, "InvoiceAmount")));
            m.put("InvoiceExRate", asDouble(ci(r, "InvExRate")));
            m.put("LcyAmount", asDouble(ci(r, "LcyAmount")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("GdNo", text(ci(r, "GDNO")));
            m.put("GdDate", iso(ci(r, "GDDate")));
            m.put("GdValue", asDouble(ci(r, "GDValue")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("LcyGdValue", asDouble(ci(r, "LcyGdValue")));
            m.put("DiffAmount", asDouble(ci(r, "DiffAmount")));
            m.put("LcyDiffAmount", asDouble(ci(r, "LcyDiffAmount")));
            total += asDouble(ci(r, "LcyDiffAmount"));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("totalLcyDiff", total);
        return out;
    }

    // ================================================================= 243 Packing List Register Export

    /** LoadInvoices_Load: ComboFill + ParameterFill (Rows[1] = "This Week" default). */
    public Map<String, Object> packingListSetup() {
        UserAccount u = user(PACKING_LIST_REGISTER, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.putAll(packingListCombos(u));
        return out;
    }

    public Map<String, Object> packingListCombos() { return packingListCombos(user(PACKING_LIST_REGISTER, "View")); }

    private Map<String, Object> packingListCombos(UserAccount u) {
        List<Map<String, Object>> dt = repo.packingListDropDowns(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("warehouses", pick(dt, "ActivityType", "Warehouse", "Id", "name"));
        out.put("items", pick(dt, "ActivityType", "Items", "Id", "name"));
        out.put("customers", pick(dt, "ActivityType", "Customer", "Id", "name"));
        out.put("packingTypes", pick(dt, "ActivityType", "PackingType", "Id", "name"));
        out.put("crops", pick(dt, "ActivityType", "Crop", "Id", "name"));
        out.put("jobLots", pick(dt, "ActivityType", "JobLot", "Id", "name"));
        out.put("destinations", pick(dt, "ActivityType", "DestinationPort", "Id", "name"));
        out.put("invoices", pick(dt, "ActivityType", "Invoice", "Id", "name"));
        return out;
    }

    /** GridSummaryFill -> dtSalesRegister columns. */
    public List<Map<String, Object>> packingListShow(Map<String, Object> f) {
        UserAccount u = user(PACKING_LIST_REGISTER, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.packingListRegister(u, date(f.get("fromDate")), date(f.get("toDate")), asInt(f.get("itemId")),
                asInt(f.get("invoiceId")), asInt(f.get("customerId")), asInt(f.get("cropYearId")), asInt(f.get("jobLotId")),
                asInt(f.get("warehouseId")), asInt(f.get("packingTypeId")), asInt(f.get("destinationPortId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("DocCode", text(ci(r, "DocCode")));
            m.put("ContractNos", text(ci(r, "ContractNos")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("Customer", text(ci(r, "CustomerName")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("CropYear", text(ci(r, "CropYear")));
            m.put("JobLot", text(ci(r, "JobLotDescription")));
            m.put("PackingType", text(ci(r, "PackTypeDesc")));
            m.put("WareHouse", text(ci(r, "WareHouseName")));
            m.put("ContainerNo", text(ci(r, "ContainerNo")));
            m.put("SealNo", text(ci(r, "SealNo")));
            m.put("ItemQty", asDouble(ci(r, "ItemQty")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("PackingWeight", asDouble(ci(r, "PackingWeight")));
            m.put("PackingWeightTotal", asDouble(ci(r, "PackingWeightTotal")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("RatePrice", asDouble(ci(r, "RatePrice")));
            m.put("RateUom", text(ci(r, "RateUom")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 244 Pending Forwarding For Commercial Invoice

    /** LoadInvoices_Load: FromDate = ActiveYr.Start_Period, AllDropDownFill, CurrencyFill, PackingExpiryDateFill, ProductionNoFill. */
    public Map<String, Object> pendingForwardingSetup() {
        UserAccount u = user(PENDING_FORWARDING, "View");
        Map<String, Object> out = common(u, yearStart(u));
        List<Map<String, Object>> dt = repo.forwardingDropDowns(u);
        out.put("proformas", pick(dt, "Activity", "ContractNo", "Id", "ReferenceName"));
        out.put("parties", pick(dt, "Activity", "Customer", "Id", "ReferenceName"));
        out.put("items", pick(dt, "Activity", "Item", "Id", "ReferenceName"));
        out.put("currencies", pick2(repo.currencies(u), "Id", "CurrencyName"));
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> r : repo.packingExpiryDates(u)) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", text(ci(r, "PackingExpiryDate"))); m.put("Name", text(ci(r, "PackingExpiryDate"))); exp.add(m); }
        out.put("packingExpiryDates", exp);
        List<Map<String, Object>> prod = new ArrayList<>();
        for (Map<String, Object> r : repo.productionNos(u)) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", text(ci(r, "ProductionNo"))); m.put("Name", text(ci(r, "ProductionNo"))); prod.add(m); }
        out.put("productionNos", prod);
        return out;
    }

    private static List<Map<String, Object>> pick2(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) { Map<String, Object> m = new LinkedHashMap<>(); m.put("Id", ci(r, idCol)); m.put("Name", text(ci(r, nameCol))); out.add(m); }
        return out;
    }

    /**
     * ExportPreInvoicesLoad. Desktop quirk kept: the From/To DateTimePickers are never sent to the
     * procedure; BranchesId and FinancialYearId come from the session; Skip Zero -> @SkipZero = 1.
     */
    public List<Map<String, Object>> pendingForwardingShow(Map<String, Object> f) {
        UserAccount u = user(PENDING_FORWARDING, "View");
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.pendingForwarding(u, branch, currentUserContext.currentFinancialYearId(), asInt(f.get("proformaId")),
                asInt(f.get("partyId")), asInt(f.get("currencyId")), asInt(f.get("itemId")), text(f.get("packingExpiryDate")),
                text(f.get("productionNo")), asBool(f.get("skipZero")) ? 1 : 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("PartyName", text(ci(r, "CustomerName")));
            m.put("ProformaNo", text(ci(r, "ProformaNo")));
            m.put("CommissionAgentId", asInt(ci(r, "CommissionAgentId")));
            m.put("CommissionAgent", text(ci(r, "CommissionAgent")));
            m.put("CommRate", asDouble(ci(r, "CommRate")));
            m.put("FCurrencyId", asInt(ci(r, "FCurrencyId")));
            m.put("CurrencyCode", text(ci(r, "CurrencyCode")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("PackingMaterialId", asInt(ci(r, "PackingMaterialId")));
            m.put("PackingType", text(ci(r, "PackingType")));
            m.put("CropYearId", asInt(ci(r, "CropYearId")));
            m.put("CropYear", text(ci(r, "CropYear")));
            m.put("PackUomId", asInt(ci(r, "PackUomId")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("EbUnit", asDouble(ci(r, "EbUnit")));
            m.put("EbTotal", asDouble(ci(r, "EbTotal")));
            m.put("GrossWeight", asDouble(ci(r, "GrossWeight")));
            m.put("MTon", asDouble(ci(r, "M_Ton")));
            m.put("ShipMTon", asDouble(ci(r, "ShipMton")));
            m.put("BalMTon", asDouble(ci(r, "BalMTon")));
            m.put("NoOfBags", asDouble(ci(r, "NoOfBags")));
            m.put("ShipBags", asDouble(ci(r, "ShipBags")));
            m.put("BalBags", asDouble(ci(r, "BalNoofBags")));
            m.put("NetWeight", asDouble(ci(r, "NetWeight")));
            m.put("ShipWeight", asDouble(ci(r, "ShipWeight")));
            m.put("BalWeight", asDouble(ci(r, "BalWeight")));
            m.put("ItemRate", asDouble(ci(r, "RatePrice")));
            m.put("RateUomId", asInt(ci(r, "RateUomId")));
            m.put("RateUom", text(ci(r, "RateUom")));
            m.put("Amount", asDouble(ci(r, "Amount")));
            m.put("ShipAmount", asDouble(ci(r, "ShipAmount")));
            m.put("BalAmount", asDouble(ci(r, "BalShipAmount")));
            m.put("OtherRate", asDouble(ci(r, "OtherRate")));
            m.put("PackingExpiryDate", text(ci(r, "PackingExpiryDate")));
            m.put("ProductionNo", text(ci(r, "ProductionNo")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 245 GD Break Up and Realized Register

    public Map<String, Object> gdRealizedSetup() {
        UserAccount u = user(GD_REALIZED, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.putAll(gdRealizedCombos(u));
        return out;
    }

    public Map<String, Object> gdRealizedCombos() { return gdRealizedCombos(user(GD_REALIZED, "View")); }

    private Map<String, Object> gdRealizedCombos(UserAccount u) {
        List<Map<String, Object>> dt = repo.gdBreakUpDropDowns(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("invoices", pick(dt, "ActivityType", "Invoice", "Id", "name"));
        out.put("banks", pick(dt, "ActivityType", "Bank", "Id", "name"));
        return out;
    }

    /** Gridfill -> dt1 columns (the desktop's own headings, "FTT%" and "FTT/M-FORM" included). */
    public List<Map<String, Object>> gdRealizedShow(Map<String, Object> f) {
        UserAccount u = user(GD_REALIZED, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gdBreakUpAndRealized(u, date(f.get("fromDate")), date(f.get("toDate")),
                asInt(f.get("bankId")), asInt(f.get("invoiceId")), asBool(f.get("skipZero")) ? 1 : 0)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("ShipDate", text(ci(r, "ShipDate")));
            m.put("Tenor", text(ci(r, "Tenor")));
            m.put("DueDate", text(ci(r, "DueDate")));
            m.put("PTYInvoiceNo", text(ci(r, "PTYInvoiceNo")));
            m.put("GDNO", text(ci(r, "GDNO")));
            m.put("GDDate", iso(ci(r, "GDDate")));
            m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("BankInvoiceDate", iso(ci(r, "BankInvoiceDate")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("Freight", asDouble(ci(r, "Freight")));
            m.put("FOBValue", asDouble(ci(r, "FOBValue")));
            m.put("FTT%", asDouble(ci(r, "FTT%")));
            m.put("FTT/M-FORM", asDouble(ci(r, "FTT/M-FORM")));
            m.put("NetToBeRealized", asDouble(ci(r, "NetToBeRealized")));
            m.put("Realized", asDouble(ci(r, "Realized")));
            m.put("Balance", asDouble(ci(r, "Balance")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("GdLcyAmount", asDouble(ci(r, "GdLcyAmount")));
            m.put("RealizedLcyAmount", asDouble(ci(r, "RealizedLcyAmount")));
            m.put("UnRealizedLcyAmount", asDouble(ci(r, "UnRealizedLcyAmount")));
            m.put("GdStatus", text(ci(r, "GdStatus")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 246 FI Balance Summary

    /** FIBalanceSummary_Load runs btnShow_Click at once (Skip Zero unticked). */
    public Map<String, Object> fiBalanceSetup() {
        UserAccount u = user(FI_BALANCE, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", fiBalanceRows(u, 0));
        return out;
    }

    public List<Map<String, Object>> fiBalanceShow(Map<String, Object> f) {
        return fiBalanceRows(user(FI_BALANCE, "View"), asBool(f.get("skipZero")) ? 1 : 0);
    }

    private List<Map<String, Object>> fiBalanceRows(UserAccount u, int skipZero) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.fiBalanceSummary(u, skipZero)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("EFormNo", text(ci(r, "EFormNo")));
            m.put("PaymentTermId", asInt(ci(r, "PaymenttermId")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("UtilizeAmountInInvoice", asDouble(ci(r, "UtilizeAmountInInvoice")));
            m.put("AdvanceUtilizeAgainstGDs", asDouble(ci(r, "AdvanceUtilizeagainstGDs")));
            m.put("BalFIAmount", asDouble(ci(r, "BalFIAmount")));
            out.add(m);
        }
        return out;
    }

    /** grdfrm_LinkClicked "EFormNo" -> FIBalanceDetailByFI(GdId = row Id).BindGdbreakUp. */
    public List<Map<String, Object>> fiBalanceDetail(int id) {
        UserAccount u = user(FI_BALANCE, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.fiBalanceDetailByFI(u, id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("RefDocumentTypeId", asInt(ci(r, "RefDocumentTypeId")));
            m.put("GDId", asInt(ci(r, "GDId")));
            m.put("EximInvoiceId", asInt(ci(r, "EximInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("FIId", asInt(ci(r, "FIId")));
            m.put("FINo", text(ci(r, "FINo")));
            m.put("GdNo", text(ci(r, "GdNo")));
            m.put("UtilizeAmount", asDouble(ci(r, "UtilizeAmount")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 256 Commission Agent Fcy Ledger

    public Map<String, Object> commAgentSetup() {
        UserAccount u = user(COMM_AGENT_LEDGER, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.put("agents", commAgents(u));
        return out;
    }

    public List<Map<String, Object>> commAgents() { return commAgents(user(COMM_AGENT_LEDGER, "View")); }

    /** CommissionAgentFill: GetDataForDropDownFromExportInvoice(Activity "CommissionAgent", DocumentTypeIds "211"). */
    private List<Map<String, Object>> commAgents(UserAccount u) {
        return pick2(repo.exportInvoiceDropDowns(u, "CommissionAgent", "211"), "Id", "name");
    }

    /** GridBind: "Commission Agent Required!" when no agent; dates always sent. */
    public List<Map<String, Object>> commAgentShow(Map<String, Object> f) {
        UserAccount u = user(COMM_AGENT_LEDGER, "View");
        int agent = asInt(f.get("commissionAgentId"));
        if (agent <= 0) throw new IllegalArgumentException("Commission Agent Required!");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.commissionAgentFcyLedger(u, dateOrToday(f.get("fromDate")), dateOrToday(f.get("toDate")), agent)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("V.Type", text(ci(r, "DocumentTypeCode")));
            m.put("V.Code", text(ci(r, "DocCode")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceAmount", asDouble(ci(r, "InvoiceAmount")));
            m.put("Party", text(ci(r, "CommissionAgent")));
            m.put("Comm%", asDouble(ci(r, "CommPercnt")));
            m.put("FcyCode", text(ci(r, "FcyCode")));
            m.put("FcyDebit", asDouble(ci(r, "FcyDebit")));
            m.put("FcyCredit", asDouble(ci(r, "FcyCredit")));
            m.put("FcyBalance", asDouble(ci(r, "FcyBalance")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("Debit", asDouble(ci(r, "Debit")));
            m.put("Credit", asDouble(ci(r, "Credit")));
            m.put("Balance", asDouble(ci(r, "Balance")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 257 EE Report Export GD

    /** frmStockReport_Load: ParameterFill (Rows[1] = "This Week"), RealizedBankFill, BankFill; plus the FE Return month list. */
    public Map<String, Object> eeSetup() {
        UserAccount u = user(EE_REPORT, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.putAll(eeCombos(u));
        out.put("months", eeMonths(text(out.get("yearStart"))));
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, EE_REPORT, "Save") || allowed(u, EE_REPORT, "Update"));
        out.put("permissions", perm);
        return out;
    }

    public Map<String, Object> eeCombos() { return eeCombos(user(EE_REPORT, "View")); }

    private Map<String, Object> eeCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("banks", pick2(repo.gdBanks(u), "BankId", "BankName"));
        out.put("realizedBanks", pick(repo.realizedBankDropDowns(u), "Activity", "RealizedBank_Gd", "Id", "ReferenceName"));
        return out;
    }

    /**
     * GridComboBind: the FE Return month list is the July-June year around ActiveYr.Start_Period
     * (MonthId, YearId, "MMM yyyy").
     */
    private static List<Map<String, Object>> eeMonths(String yearStart) {
        List<Map<String, Object>> out = new ArrayList<>();
        LocalDate start = asDate(yearStart);
        if (start == null) start = LocalDate.now();
        int year = start.getMonthValue() >= 7 ? start.getYear() : start.getYear() - 1;
        LocalDate d = LocalDate.of(year, 7, 1), end = LocalDate.of(year + 1, 6, 30);
        String[] mon = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        while (!d.isAfter(end)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("MonthId", d.getMonthValue());
            m.put("YearId", d.getYear());
            m.put("Month", mon[d.getMonthValue() - 1] + " " + d.getYear());
            out.add(m);
            d = d.plusMonths(1);
        }
        return out;
    }

    /** btnshow_Click: Detail -> GridBind (Activity "Detail"), else GridBindSummary ("Summary"). */
    public Map<String, Object> eeShow(Map<String, Object> f) {
        UserAccount u = user(EE_REPORT, "View");
        boolean detail = !"Summary".equalsIgnoreCase(text(f.get("mode")));
        List<Map<String, Object>> rows = new ArrayList<>();
        Set<String> salesTerms = new LinkedHashSet<>(), schedules = new LinkedHashSet<>();
        for (Map<String, Object> r : repo.eeReport(u, date(f.get("fromDate")), date(f.get("toDate")), asInt(f.get("bankId")),
                asInt(f.get("realizedBankId")), detail ? "Detail" : "Summary")) {
            Map<String, Object> m = new LinkedHashMap<>();
            if (detail) {
                m.put("Id", asInt(ci(r, "Id")));
                m.put("FcyBankReceiptId", asInt(ci(r, "FcyBankReceiptId")));
                m.put("Sr.No.", text(ci(r, "RwNo")));
            }
            m.put("GdRefDocTypeId", asInt(ci(r, "GdRefDocTypeId")));
            m.put("GDId", asInt(ci(r, "GDId")));
            m.put("GdNo", text(ci(r, "GdNo")));
            m.put("Consignee", text(ci(r, "Consignee")));
            m.put("Commodity", text(ci(r, "Commodity")));
            m.put("HsCode", text(ci(r, "HsCode")));
            m.put("ContractNo", text(ci(r, "ContractNo")));
            m.put("ContractDate", iso(ci(r, "ContractDate")));
            m.put("ContractCurrency", text(ci(r, "ContractCurrency")));
            m.put("ContractAmount", asDouble(ci(r, "ContractAmount")));
            m.put("DateOfShipment", iso(ci(r, "DateOfShipment")));
            m.put("DateOfNegotion", iso(ci(r, "DateOfNegotion")));
            m.put("RealizedCurrency", text(ci(r, "RealizedCurrency")));
            m.put("RealizedAmount", asDouble(ci(r, "RealizedAmount")));
            m.put("FTT/Comm", asDouble(ci(r, "AgencyComm")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("RealizedAmountInPKR", asDouble(ci(r, "RealizedAmountInPKR")));
            m.put("DateofRealization", iso(ci(r, "DateofRealization")));
            m.put("MultiCurrencyId", asInt(ci(r, "MultiCurrencyId")));
            if (detail) m.put("EEId", asInt(ci(r, "EEId")));
            m.put("FEReturnPageNo", asInt(ci(r, "FEReturnPageNo")));
            m.put("ITRSNo", asInt(ci(r, "ITRSNo")));
            m.put("FEReturnMonth", text(ci(r, "FEReturnMonth")));
            m.put("Schedule", text(ci(r, "Schedule")));
            m.put("SalesTerm", text(ci(r, "SalesTerm")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("EERemarks", text(ci(r, "EERemarks")));
            salesTerms.add(text(ci(r, "SalesTerm")));
            schedules.add(text(ci(r, "Schedule")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("mode", detail ? "Detail" : "Summary");
        out.put("rows", rows);
        out.put("salesTerms", new ArrayList<>(salesTerms));
        out.put("schedules", new ArrayList<>(schedules));
        return out;
    }

    /**
     * grdfrm_CellUpdated (FEReturnPageNo / FEReturnMonth / ITRSNo / Schedule / SalesTerm / EERemarks):
     * one EEStatement from the row, EEStatement.Save -> USP_EEStatement_InsertAndUpdate. The
     * UpdatingCell guard "Please Type Only Numeric Value" is repeated for ITRSNo / FEReturnPageNo.
     * Desktop quirk kept: FEReturnMonthId = ToInt(cell Value) - 0 unless the month was picked from the
     * list in this session (the stored text does not parse); FEReturnYearId from the month list.
     */
    public Map<String, Object> eeSaveRow(Map<String, Object> b) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (!allowed(u, EE_REPORT, "Save") && !allowed(u, EE_REPORT, "Update")) rights.require(u, EE_REPORT, "Save");
        if (!numeric(b.get("ITRSNo")) || !numeric(b.get("FEReturnPageNo"))) throw new IllegalArgumentException("Please Type Only Numeric Value");
        int monthId = asInt(b.get("FEReturnMonthValue"));
        int yearId = 0;
        for (Map<String, Object> m : eeMonths(yearStart(u))) if (asInt(m.get("MonthId")) == monthId) { yearId = asInt(m.get("YearId")); break; }
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> m = new LinkedHashMap<>();       // Architecture.Model.Export.EEStatement, property order
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("CompanyId", u.getCompanyId());
        m.put("EntryUserId", u.getId());
        m.put("FcyReceiptId", asInt(b.get("FcyBankReceiptId")));
        m.put("FcyReceiptsDetailId", asInt(b.get("Id")));
        m.put("FEReturnMonthId", monthId);
        m.put("FEReturnYearId", yearId);
        m.put("FEReturnPageNo", asInt(b.get("FEReturnPageNo")));
        m.put("Id", asInt(b.get("EEId")));
        m.put("ITRSNo", asInt(b.get("ITRSNo")));
        m.put("ModifyUserId", u.getId());
        m.put("OrganizationId", u.getOrganizationId());
        m.put("FEReturnMonth", nullIfEmpty(text(b.get("FEReturnMonth"))));
        m.put("Remarks", nullIfEmpty(text(b.get("EERemarks"))));
        m.put("SalesTerm", nullIfEmpty(text(b.get("SalesTerm"))));
        m.put("Schedule", nullIfEmpty(text(b.get("Schedule"))));
        List<Map<String, Object>> items = new ArrayList<>();
        items.add(m);
        int id = repo.saveEeStatements(items);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        return out;
    }

    private static boolean numeric(Object v) {
        String s = text(v);
        return s.isEmpty() || s.matches("-?\\d+");
    }

    // ================================================================= 258 Commercial Invoice Shipments

    /** Commercial_Invoice_Shipments_Load: combos, Rows[2] = "This Month", then GridBind at once. */
    public Map<String, Object> ciShipmentsSetup() {
        UserAccount u = user(CI_SHIPMENTS, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.put("customers", ciCustomers(u));
        return out;
    }

    public List<Map<String, Object>> ciCustomers() { return ciCustomers(user(CI_SHIPMENTS, "View")); }

    private List<Map<String, Object>> ciCustomers(UserAccount u) {
        return pick2(repo.exportInvoiceDropDowns(u, "Customer", "211"), "Id", "name");
    }

    public List<Map<String, Object>> ciShipmentsShow(Map<String, Object> f) {
        UserAccount u = user(CI_SHIPMENTS, "View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.commercialInvoiceShipments(u, date(f.get("fromDate")), date(f.get("toDate")), asInt(f.get("customerId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Ship/BL Date", iso(ci(r, "DocDate")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("Fcl", asDouble(ci(r, "FCL")));
            m.put("Consignee", text(ci(r, "Consignee")));
            m.put("Brand", text(ci(r, "ItemName")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("ExchangeRate", asDouble(ci(r, "ConversionRate")));
            m.put("LcyAmount", asDouble(ci(r, "LcyAmount")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= 203 Shipping Consignment FollowUp

    /** frmshippedConsignment_followup_Load: InvoiceNoFill, CurrentStatusfill, DocStatusfill, GridFill. */
    public Map<String, Object> followUpSetup() {
        UserAccount u = user(CONSIGNMENT_FOLLOWUP, "View");
        Map<String, Object> out = followUpCombos(u);
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, CONSIGNMENT_FOLLOWUP, "Save"));
        perm.put("Update", allowed(u, CONSIGNMENT_FOLLOWUP, "Update"));
        out.put("permissions", perm);
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** btnRefresh_Click. */
    public Map<String, Object> followUpCombos() { return followUpCombos(user(CONSIGNMENT_FOLLOWUP, "View")); }

    private Map<String, Object> followUpCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("invoices", pick2(repo.exportInvoiceNos(u), "Id", "InvoiceNo"));
        out.put("currentStatuses", pick2(repo.lookUpsByType(u, 6), "Id", "LookUpName"));
        out.put("documentStatuses", pick2(repo.lookUpsByType(u, 5), "Id", "LookUpName"));
        return out;
    }

    /**
     * GridFill: @DocumentStatusIds is built from the checked statuses' NAMES looked up back to ids, each
     * prefixed by "," (the desktop string therefore starts with a comma - kept). PaymentStatus =
     * RealizedAmount > 0 ? "Received" : "Not Received".
     */
    public List<Map<String, Object>> followUpGrid(Map<String, Object> f) {
        UserAccount u = user(CONSIGNMENT_FOLLOWUP, "View");
        StringBuilder status = new StringBuilder();
        List<Object> ids = rawList(f.get("statusIds"));
        if (!ids.isEmpty()) {
            List<Map<String, Object>> doc = repo.lookUpsByType(u, 5);
            for (Object idObj : ids) {
                int id = asInt(idObj);
                for (Map<String, Object> d : doc) if (asInt(ci(d, "Id")) == id) { status.append(",").append(asInt(ci(d, "Id"))); break; }
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.followUpRegister(u, 0, status.toString())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ShipmentId", asInt(ci(r, "ShipmentId")));
            m.put("InvoiceId", asInt(ci(r, "InvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("InvoiceDate", iso(ci(r, "InvoiceDate")));
            m.put("Customer", text(ci(r, "CustomerName")));
            m.put("DestinationPort", text(ci(r, "DestinationPort")));
            m.put("CRO_Number", text(ci(r, "CRO_Number")));
            m.put("ShippingLine", text(ci(r, "ShippingLine")));
            m.put("ShippingAgent", text(ci(r, "ShippingAgent")));
            m.put("Forwarder", text(ci(r, "Forwarder")));
            m.put("FclQty", asDouble(ci(r, "FclQty")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("FcurrencyId", asInt(ci(r, "FcurrencyId")));
            m.put("PaymentStatus", asDouble(ci(r, "RealizedAmount")) > 0.0 ? "Received" : "Not Received");
            m.put("DispatchedFactory", iso(ci(r, "DispatchedFactoryDate")));
            m.put("LoadOnTrainDate", iso(ci(r, "LoadOnTrainDate")));
            m.put("ReachedAtLoadingPortDate", iso(ci(r, "ReachedAtLoadingPortDate")));
            m.put("LoadedOnVesselDate", iso(ci(r, "LoadedOnVesselDate")));
            m.put("CuttOfDate", iso(ci(r, "CuttOfDate")));
            m.put("ETDLoadingPort", iso(ci(r, "ETDLoadingPort")));
            m.put("ETDDestinationPort", iso(ci(r, "ETDDestinationPort")));
            m.put("DocToBank", iso(ci(r, "DocToBank")));
            m.put("DocToParty", iso(ci(r, "DocToParty")));
            m.put("ReachedAtDestinationPort", iso(ci(r, "ReachedAtDestinationPort")));
            m.put("CourierTracingNo", text(ci(r, "CourierTracingNo")));
            m.put("DocumentStatus", asInt(ci(r, "DocumentStatusId")));
            m.put("CurrentStatus", asInt(ci(r, "CurrentStatusId")));
            m.put("GoodsDeclarationNo", text(ci(r, "GdNo")));
            m.put("GoodsDeclarationDate", iso(ci(r, "GdDate")));
            m.put("PortPaymentDate", iso(ci(r, "PortPayment")));
            m.put("ContainerNos", text(ci(r, "ContainerNos")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /** InvoiceNo_Leave / ReadById: GetByID(invoice) + GetIdByInvoiceId(invoice). */
    public Map<String, Object> followUpByInvoice(int invoiceId) {
        user(CONSIGNMENT_FOLLOWUP, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = repo.followUpByInvoiceId(invoiceId);
        out.put("recId", repo.followUpIdByInvoiceId(invoiceId));
        out.put("found", !rows.isEmpty());
        if (!rows.isEmpty()) {
            Map<String, Object> r = rows.get(0), m = new LinkedHashMap<>();
            m.put("DocumentStatusId", asInt(ci(r, "DocumentStatusId")));
            m.put("CurrentStatusId", asInt(ci(r, "CurrentStatusId")));
            m.put("CourierTracingNo", text(ci(r, "CourierTracingNo")));
            m.put("GdNo", text(ci(r, "GdNo")));
            for (String k : new String[] { "CuttOfDate", "DocToBankDate", "DocCourierToPartyDate", "LoadOnTrainDate", "ReachedAtLoadingPortDate",
                    "LoadedOnVesselDate", "ETDFinalDate", "ReachedAtDestinationPort", "GdDate", "PortPayment", "ETDLoadingPort" })
                m.put(k, iso(ci(r, k)));
            out.put("record", m);
        }
        return out;
    }

    /**
     * Insert(): "Please Select Invoice!" without an invoice; Id = RecId (> 0 -> Update proc); the
     * ticked dates only. The Yes/No confirmations are asked on the page.
     */
    public Map<String, Object> followUpSave(Map<String, Object> b) {
        int recId = asInt(b.get("recId"));
        UserAccount u = user(CONSIGNMENT_FOLLOWUP, recId > 0 ? "Update" : "Save");
        int invoiceId = asInt(b.get("invoiceId"));
        if (invoiceId == 0) throw new IllegalArgumentException("Please Select Invoice!");
        Map<String, Object> m = followUpModel(u, recId, invoiceId, asInt(b.get("currentStatusId")), asInt(b.get("documentStatusId")),
                text(b.get("courierTrackingNo")).trim(), text(b.get("gdNo")).trim(), b);
        int id = repo.saveFollowUp(m);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId > 0 ? "Record Update Successfully" : "Record Save Successfully");
        return out;
    }

    /**
     * UpdateByGrid(row): the row's dates / statuses against GetByID(InvoiceId); a difference (or a new
     * value where the record had none) marks the row for update, else "No Change found. Please Change at
     * least one date to update record". CourierTracingNo / GdNo come from the stored record (not the row).
     */
    public Map<String, Object> followUpUpdateRow(Map<String, Object> row) {
        UserAccount u = user(CONSIGNMENT_FOLLOWUP, "Update");
        int invoiceId = asInt(row.get("InvoiceId"));
        int shipmentId = asInt(row.get("ShipmentId"));
        List<Map<String, Object>> stored = repo.followUpByInvoiceId(invoiceId);
        boolean change = false;
        Map<String, Object> dates = new LinkedHashMap<>();
        String[][] map = { { "DocToBank", "DocToBankDate" }, { "DocToParty", "DocCourierToPartyDate" }, { "LoadOnTrainDate", "LoadOnTrainDate" },
                { "ReachedAtLoadingPortDate", "ReachedAtLoadingPortDate" }, { "LoadedOnVesselDate", "LoadedOnVesselDate" }, { "CuttOfDate", "CuttOfDate" },
                { "ETDLoadingPort", "ETDLoadingPort" }, { "ETDDestinationPort", "ETDFinalDate" }, { "ReachedAtDestinationPort", "ReachedAtDestinationPort" },
                { "GoodsDeclarationDate", "GdDate" }, { "PortPaymentDate", "PortPayment" } };
        int docStatusId = 0, currStatusId = 0;
        String courier = "", gdNo = "";
        if (!stored.isEmpty()) {
            Map<String, Object> s = stored.get(0);
            docStatusId = asInt(ci(s, "DocumentStatusId"));
            currStatusId = asInt(ci(s, "CurrentStatusId"));
            courier = text(ci(s, "CourierTracingNo"));
            gdNo = text(ci(s, "GdNo"));
            for (String[] k : map) {
                LocalDate rowD = asDate(row.get(k[0])), oldD = asDate(iso(ci(s, k[1])));
                if (oldD != null) {
                    dates.put(k[1], rowD);            // the row value goes through in both branches
                    if (rowD == null || !rowD.equals(oldD)) change = true;
                } else if (rowD != null) { change = true; dates.put(k[1], rowD); }
            }
            int rowCurr = asInt(row.get("CurrentStatus")), rowDoc = asInt(row.get("DocumentStatus"));
            if (currStatusId > 0) { if (currStatusId != rowCurr) { change = true; currStatusId = rowCurr; } }
            else if (rowCurr > 0) { change = true; currStatusId = rowCurr; }
            if (docStatusId > 0) { if (docStatusId != rowDoc) { change = true; docStatusId = rowDoc; } }
            else if (rowDoc > 0) { change = true; docStatusId = rowDoc; }
        } else {
            for (String[] k : map) { LocalDate rowD = asDate(row.get(k[0])); if (rowD != null) { change = true; dates.put(k[1], rowD); } }
            int rowCurr = asInt(row.get("CurrentStatus")), rowDoc = asInt(row.get("DocumentStatus"));
            if (rowCurr > 0) { change = true; currStatusId = rowCurr; }
            if (rowDoc > 0) { change = true; docStatusId = rowDoc; }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        if (!change) throw new IllegalArgumentException("No Change found. Please Change at least one date to update record");
        Map<String, Object> m = followUpModelFromDates(u, shipmentId, invoiceId, currStatusId, docStatusId, courier, gdNo, dates);
        int id = repo.saveFollowUp(m);
        out.put("success", true);
        out.put("id", id);
        out.put("message", shipmentId > 0 ? "Record Update Successfully" : "Record Save Successfully");
        return out;
    }

    /**
     * btnUpdateStatus_Click: "Please Select Rows first to update document status of multi rows" when
     * nothing is checked; each checked row (its dates, statuses, CourierTracingNo and GoodsDeclarationNo)
     * through MultiSave (Insert when ShipmentId == 0 else Update, one transaction).
     */
    public Map<String, Object> followUpUpdateStatuses(Map<String, Object> b) {
        UserAccount u = user(CONSIGNMENT_FOLLOWUP, "Update");
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Please Select Rows first to update document status of multi rows");
        List<Map<String, Object>> models = new ArrayList<>();
        String[][] map = { { "DocToBank", "DocToBankDate" }, { "DocToParty", "DocCourierToPartyDate" }, { "LoadOnTrainDate", "LoadOnTrainDate" },
                { "ReachedAtLoadingPortDate", "ReachedAtLoadingPortDate" }, { "LoadedOnVesselDate", "LoadedOnVesselDate" }, { "CuttOfDate", "CuttOfDate" },
                { "ETDLoadingPort", "ETDLoadingPort" }, { "ETDDestinationPort", "ETDFinalDate" }, { "ReachedAtDestinationPort", "ReachedAtDestinationPort" },
                { "GoodsDeclarationDate", "GdDate" }, { "PortPaymentDate", "PortPayment" } };
        for (Map<String, Object> row : rows) {
            Map<String, Object> dates = new LinkedHashMap<>();
            for (String[] k : map) { LocalDate d = asDate(row.get(k[0])); if (d != null) dates.put(k[1], d); }
            int curr = asInt(row.get("CurrentStatus")) > 0 ? asInt(row.get("CurrentStatus")) : 0;
            int doc = asInt(row.get("DocumentStatus")) > 0 ? asInt(row.get("DocumentStatus")) : 0;
            models.add(followUpModelFromDates(u, asInt(row.get("ShipmentId")), asInt(row.get("InvoiceId")), curr, doc,
                    text(row.get("CourierTracingNo")), text(row.get("GoodsDeclarationNo")), dates));
        }
        int id = repo.saveFollowUps(models);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        return out;
    }

    /** Insert() model: a date only when its picker is ticked (body key "<name>Checked"). */
    private Map<String, Object> followUpModel(UserAccount u, int id, int invoiceId, int currStatus, int docStatus, String courier, String gdNo, Map<String, Object> b) {
        Map<String, Object> dates = new LinkedHashMap<>();
        String[][] pick = { { "txtDocToBank", "DocToBankDate" }, { "txtDocToParty", "DocCourierToPartyDate" }, { "txtLoadOnTrainDate1", "LoadOnTrainDate" },
                { "txtReachedAtLoadingPortDate2", "ReachedAtLoadingPortDate" }, { "txtLoadedOnVesselDate3", "LoadedOnVesselDate" }, { "txtETDFinalDate4", "ETDFinalDate" },
                { "txtReachedDestinationPortDate", "ReachedAtDestinationPort" }, { "txtGoodsDeclarationDate", "GdDate" }, { "txtPortPayment", "PortPayment" },
                { "datCuttOfDate", "CuttOfDate" }, { "txtETDLoadingPort", "ETDLoadingPort" } };
        for (String[] p : pick) if (asBool(b.get(p[0] + "Checked"))) { LocalDate d = asDate(b.get(p[0])); if (d != null) dates.put(p[1], d); }
        return followUpModelFromDates(u, id, invoiceId, currStatus, docStatus, courier, gdNo, dates);
    }

    /** Architecture.Model.Export.ExImShippedConsignmentFollowUps, property order; an unset DateTime? is omitted. */
    private static Map<String, Object> followUpModelFromDates(UserAccount u, int id, int invoiceId, int currStatus, int docStatus,
                                                              String courier, String gdNo, Map<String, Object> dates) {
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("DocCourierToPartyDate", ts(dates.get("DocCourierToPartyDate")));
        m.put("DocToBankDate", ts(dates.get("DocToBankDate")));
        m.put("LoadOnTrainDate", ts(dates.get("LoadOnTrainDate")));
        m.put("ReachedAtLoadingPortDate", ts(dates.get("ReachedAtLoadingPortDate")));
        m.put("LoadedOnVesselDate", ts(dates.get("LoadedOnVesselDate")));
        m.put("ETDFinalDate", ts(dates.get("ETDFinalDate")));
        m.put("GdDate", ts(dates.get("GdDate")));
        m.put("PortPayment", ts(dates.get("PortPayment")));
        m.put("CuttOfDate", ts(dates.get("CuttOfDate")));
        m.put("EntryDate", now);
        m.put("ModifyDate", now);
        m.put("ETDLoadingPort", ts(dates.get("ETDLoadingPort")));
        m.put("EntryUserId", u.getId());
        m.put("ExImInvoiceId", invoiceId);
        m.put("Id", id);
        m.put("ModifyUserId", u.getId());
        m.put("CurrentStatusId", currStatus);
        m.put("DocumentStatusId", docStatus);
        m.put("CourierTracingNo", courier == null ? null : courier);
        m.put("GdNo", gdNo == null ? null : gdNo);
        m.put("ReachedAtDestinationPort", ts(dates.get("ReachedAtDestinationPort")));
        return m;
    }

    private static Timestamp ts(Object d) {
        if (d == null) return null;
        LocalDate ld = d instanceof LocalDate ? (LocalDate) d : asDate(d);
        return ld == null ? null : Timestamp.valueOf(ld.atStartOfDay());
    }

    /** PrintCro_Slip(InvoiceId): the row-count check before the 560 print ("No Record Found For Display"). */
    public Map<String, Object> croSlipCheck(int invoiceId) {
        UserAccount u = user(CONSIGNMENT_FOLLOWUP, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("count", repo.croSlipData(u, invoiceId).size());
        return out;
    }

    // ================================================================= 859 Third Party Inspection Lot Tracking Report

    /** Load: DateTypeFill (Rows[1] = "This Week"), AddCardsRows, TrackingFill, AllcomboBind. */
    public Map<String, Object> lotTrackingSetup() {
        UserAccount u = user(LOT_TRACKING, "View");
        Map<String, Object> out = common(u, yearStart(u));
        out.putAll(lotTrackingCombos(u));
        out.put("stages", lotStages());
        return out;
    }

    public Map<String, Object> lotTrackingCombos() { return lotTrackingCombos(user(LOT_TRACKING, "View")); }

    private Map<String, Object> lotTrackingCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> tracks = new ArrayList<>();
        for (Map<String, Object> r : repo.lotTrackingNos(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("LotRefNo", text(ci(r, "LotRefNo")));
            m.put("LodgeDate", iso(ci(r, "LodgeDate")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("FarmingNTrade", text(ci(r, "FarmingNTrade")));
            tracks.add(m);
        }
        out.put("trackingNos", tracks);
        List<Map<String, Object>> dt = repo.exportInvoiceDropDowns(u, null, null);
        out.put("customers", pick(dt, "ActivityType", "Customer", "Id", "name"));
        out.put("items", pick(dt, "ActivityType", "Items", "Id", "name"));
        return out;
    }

    private List<Map<String, Object>> lotStages() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.lotTrackingCurrentStages()) {
            if (ci(r, "Id") == null || ci(r, "Description") == null) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Description", text(ci(r, "Description")));
            out.add(m);
        }
        return out;
    }

    /**
     * Gridfill: dates only when ticked, ActionId 1 Approved&Accepted / 2 Pending&Inprocess / 3 Rejected&Cancel
     * (All -> 0, not sent), SkipZero 1/0 (0 not sent). The cards are grouped by LotCurrentStageId:
     * distinct LotCodeTrackingNo count and Sum(MTons), with every V_LotTrackingCurrentStage stage present.
     */
    public Map<String, Object> lotTrackingShow(Map<String, Object> f) {
        UserAccount u = user(LOT_TRACKING, "View");
        int actionId = asInt(f.get("actionId"));
        List<Map<String, Object>> raw = repo.lotTrackingReport(u, asBool(f.get("fromChecked")) ? date(f.get("fromDate")) : null,
                asBool(f.get("toChecked")) ? date(f.get("toDate")) : null, asInt(f.get("customerId")), asInt(f.get("itemId")),
                asInt(f.get("recId")), actionId, asBool(f.get("skipZero")) ? 1 : 0);
        List<Map<String, Object>> rows = new ArrayList<>();
        Map<Integer, Map<String, Object>> cards = new LinkedHashMap<>();
        Map<Integer, Set<String>> lots = new LinkedHashMap<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ExImLcOrderId", asInt(ci(r, "ExImLcOrderId")));
            m.put("LotCodeTrackingNo", text(ci(r, "LotCodeTrackingNo")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("InspectionAgency", text(ci(r, "InspectionAgency")));
            m.put("TradeType", text(ci(r, "TradeType")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("LcOrderNo", text(ci(r, "LcOrderNo")));
            m.put("ScheduleCode", text(ci(r, "ScheduleCode")));
            m.put("CustomerContractNo", text(ci(r, "CustomerContractNo")));
            m.put("InvoiceNo", text(ci(r, "ExImInvoiceNo")));
            m.put("LoadingScheduleDate", iso(ci(r, "LoadingScheduleDate")));
            m.put("InspectionScheduleDate", iso(ci(r, "InspectionScheduleDate")));
            m.put("MTons", asDouble(ci(r, "MTons")));
            m.put("LotMTons", asDouble(ci(r, "LotMTons")));
            m.put("UsedMTons", asDouble(ci(r, "UsedMTons")));
            m.put("BalMTons", asDouble(ci(r, "BalanceMTon")));
            m.put("ContractScheduleId", asInt(ci(r, "ContractScheduleId")));
            m.put("InvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("ExporterLotRefNo", text(ci(r, "ExporterLotRefNo")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("SamplingResponsibility", text(ci(r, "SamplingResponsibility")));
            m.put("ProcessStatus", text(ci(r, "ProcessStatus")));
            m.put("StockReservedDate", iso(ci(r, "StockReservedDate")));
            m.put("SampleTakenDate", iso(ci(r, "SampleTakenDate")));
            m.put("StockSealedDate", iso(ci(r, "StockSealedDate")));
            m.put("SampleDispatchedDate", iso(ci(r, "SampleDispatchedDate")));
            m.put("CourierNo", text(ci(r, "CourierNo")));
            m.put("LabCountry", text(ci(r, "LabCountry")));
            m.put("SampleETADestination", iso(ci(r, "SampleETADestination")));
            m.put("SampleATADestination", iso(ci(r, "SampleATADestination")));
            m.put("ReportReferenceNo", text(ci(r, "ReportReferenceNo")));
            m.put("ReportDate", iso(ci(r, "ReportDate")));
            m.put("DateOfInspection", iso(ci(r, "DateOfInspection")));
            m.put("RequiredAnalysis", text(ci(r, "RequiredAnalysis")));
            m.put("ResultStatus", text(ci(r, "ResultStatus")));
            m.put("ResultRemarks", text(ci(r, "ResultRemarks")));
            m.put("SampleStatus", text(ci(r, "SampleStatus")));
            m.put("InstructionsOrRemarks", text(ci(r, "InstructionsOrRemarks")));
            rows.add(m);
            int stage = asInt(ci(r, "LotCurrentStageId"));
            Map<String, Object> c = cards.get(stage);
            if (c == null) {
                c = new LinkedHashMap<>();
                c.put("Id", stage);
                c.put("LotInspectionStage", ci(r, "LotCurrentStage") == null ? null : String.valueOf(ci(r, "LotCurrentStage")));
                c.put("NoOfLots", 0);
                c.put("MTons", 0.0);
                cards.put(stage, c);
                lots.put(stage, new LinkedHashSet<>());
            }
            if (ci(r, "LotCodeTrackingNo") != null) lots.get(stage).add(String.valueOf(ci(r, "LotCodeTrackingNo")));
            c.put("MTons", asDouble(c.get("MTons")) + (ci(r, "MTons") != null ? asDouble(ci(r, "MTons")) : 0.0));
        }
        for (Map.Entry<Integer, Map<String, Object>> e : cards.entrySet()) e.getValue().put("NoOfLots", lots.get(e.getKey()).size());
        List<Map<String, Object>> cardRows = new ArrayList<>(cards.values());
        for (Map<String, Object> s : lotStages()) {           // AddCardsRows: missing stages with 0 / 0
            int id = asInt(s.get("Id"));
            if (id != 0 && !cards.containsKey(id)) {
                Map<String, Object> c = new LinkedHashMap<>();
                c.put("Id", id); c.put("LotInspectionStage", s.get("Description")); c.put("NoOfLots", 0); c.put("MTons", 0.0);
                cardRows.add(c);
            }
        }
        cardRows.sort((a, b) -> Integer.compare(asInt(a.get("Id")), asInt(b.get("Id"))));   // SortKeys Id ascending
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("cards", cardRows);
        return out;
    }

    // ================================================================= helpers

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    private static List<Object> rawList(Object v) {
        List<Object> out = new ArrayList<>();
        if (v instanceof List) out.addAll((List<Object>) v);
        else if (v != null) for (String s : String.valueOf(v).split(",")) if (!s.trim().isEmpty()) out.add(s.trim());
        return out;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }
    private static String nullIfEmpty(String s) { return s == null || s.isEmpty() ? null : s; }

    private static int asInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    private static double asDouble(Object v) {
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        if (v instanceof Number) return ((Number) v).doubleValue();
        if (v == null) return 0;
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }

    private static boolean asBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v == null) return false;
        String s = String.valueOf(v).trim().toLowerCase();
        return s.equals("true") || s.equals("1") || s.equals("yes") || s.equals("on");
    }

    /** Date-only ISO text of a date/timestamp cell ("" for null, DBNull or 01-Jan-0001 / 1900 sentinels kept as-is). */
    private static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate().toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 && s.charAt(4) == '-' ? s.substring(0, 10) : s;
    }

    private static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof LocalDate) return (LocalDate) v;
        String s = iso(v);
        if (s.isEmpty()) return null;
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    private static java.sql.Date date(Object v) {
        LocalDate d = asDate(v);
        return d == null ? null : java.sql.Date.valueOf(d);
    }

    /** DateTimePicker.Value can never be empty - today when the page sends nothing. */
    private static java.sql.Date dateOrToday(Object v) {
        java.sql.Date d = date(v);
        return d == null ? java.sql.Date.valueOf(LocalDate.now()) : d;
    }
}
