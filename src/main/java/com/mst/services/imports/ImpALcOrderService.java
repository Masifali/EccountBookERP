package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpADtos;
import com.mst.models.imports.ImpALcOrder;
import com.mst.models.imports.ImpALcOrderPackingDetail;
import com.mst.models.imports.ImpALcOrderPaymnetTerm;
import com.mst.repositories.imports.ImpARepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpASupport.*;

/**
 * BLL of screen 525 "Import : Lc Order" - Architecture.WinApp.Import/ImLcOrder.cs (BLL 0419 / DAL 0476).
 * Each method names the form method it reproduces; the validation order and the MessageBox texts are the form's.
 */
@Service
public class ImpALcOrderService {

    public static final int SCREEN = 525;
    /** DocumentTypeId of the Import Lc Order (DocNo(), Insert(), FormHistory, GenerateSlip). */
    public static final int DOC_TYPE = 231;
    /** CommonServices.GetSupplierustomerByCustomerGroupId("9") - the Sales Person group. */
    private static final String SALES_PERSON_GROUP = "9";

    @Autowired private ImpASupport sup;
    @Autowired private ImpARepository repo;

    private HrmSupport hrm() { return sup.hrm(); }

    private int branch(UserAccount u) { return toInt(u.getBranchesId()); }

    // ================================================================== load / combos

    /**
     * LcOrder_Load: SetRightsValueInRightsObject("ImLcOrder") (Save / Update / Print), DocNo(), the combo fills and
     * the ItemSearchByCode configuration (rdSearchByCode / rdSearchByName).
     */
    public Map<String, Object> setup() {
        UserAccount u = hrm().user(SCREEN);
        Map<String, Object> out = combos(u);
        out.put("rights", sup.rights(u, SCREEN));
        out.put("docNo", docNo(u));
        out.put("formats", sup.formats(u));
        out.put("itemSearchByCode", toBool(repo.config(u, "ItemSearchByCode")));
        return out;
    }

    /** btnRefresh_Click: every fill again (the form keeps the picked values where they are still listed). */
    public Map<String, Object> refresh() { return combos(hrm().user(SCREEN)); }

    private Map<String, Object> combos(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("suppliers", pick(repo.suppliersForExport(u), "Id", "CompanyName"));                       // SupplierFill
        m.put("salesPersons", pick(repo.suppliersByGroupIds(u, SALES_PERSON_GROUP), "Id", "CompanyName")); // salesPersonFill
        m.put("deliveryTerms", pick(repo.deliveryTerms(), "Id", "Code"));                                 // DeliveryTermFill
        m.put("paymentTerms", pick(repo.lcPaymentTerms(u), "Id", "lcOrderTerm"));                         // PaymentTermsFill
        m.put("ports", pick(repo.seaPorts(u), "Id", "PortName"));                                         // LoadingPortFill
        m.put("currencies", pick(repo.currencies(u), "Id", "CurrencyCode"));                              // MultiCurrencyfill
        Map<String, List<Map<String, Object>>> banks = sup.banks(u);                                      // ImporterandExportBankFill
        m.put("importerBanks", banks.get("foreign"));
        m.put("exporterBanks", banks.get("home"));
        m.put("items", pick(repo.itemsForExport(u), "Id", "ItemName", "ItemCode"));                        // ItemDetailFill
        m.put("packingTypes", pick(repo.packMaterialTypes(u), "Id", "Description"));                      // ItemPacktype
        return m;
    }

    /** DocNo(): ImLcOrder.GenerateCode (Company, Organization, Branch, active year, 231) -> rows[0].DocNo. */
    private int docNo(UserAccount u) {
        List<Map<String, Object>> r = repo.lcOrderGenerateCode(u, branch(u), hrm().financialYearId(), DOC_TYPE);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    public Map<String, Object> newDocNo() { return map("docNo", docNo(hrm().user(SCREEN))); }

    /** cmbitem_Leave -> bindRateUomAndItemPackUom(): CommonServices.GetUomScheduleByItemId(item). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = hrm().user(SCREEN);
        return sup.uoms(u, itemId);
    }

    // ================================================================== history

    /**
     * GridHistoryFill(NoOfRecords): tab History opens with 50, "Load All" with 0 (not sent). CanViewAllRecord from the
     * user's "CanView AllRecord" right; without it only the user's own entries (@EntryUserId). dtcol is built from these columns.
     */
    public List<Map<String, Object>> history(boolean all) {
        UserAccount u = hrm().user(SCREEN);
        boolean canViewAll = hrm().can(u, SCREEN, "CanView AllRecord");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.lcOrderHistory(u, branch(u), hrm().financialYearId(), all ? 0 : 50, canViewAll)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RecordNo", r.get("RecordNo"));
            m.put("Id", r.get("Id"));
            m.put("DocumentTypeId", r.get("DocumentTypeId"));
            m.put("DocNo", str(r.get("DocNo")));
            m.put("DocDate", r.get("DocDate"));
            m.put("SupOrderNo", r.get("LcOrderNo"));
            m.put("SupOrderDate", r.get("LcOrderDate"));
            m.put("SupplierCustomerId", r.get("SupplierCustomerId"));
            m.put("SupplierName", r.get("SupplierName"));
            m.put("NotifyParty", r.get("NotifyParty"));
            m.put("ExpiryDate", r.get("ExpiryDate"));
            m.put("ExpiryPlace", r.get("ExpiryPlace"));
            m.put("PartialShipment", r.get("PartialShipment"));
            m.put("TransShipment", r.get("TransShipment"));
            m.put("LastShipmentDate", r.get("LastShipmentDate"));
            m.put("QuotReference", r.get("QuotReference"));
            m.put("InquiryReference", r.get("InquiryReference"));
            m.put("ImporterBank", r.get("ImporterBank"));
            m.put("ExporterBank", r.get("ExporterBank"));
            m.put("LoadingPort", r.get("LoadingPort"));
            m.put("DestinationPort", r.get("DestinationPort"));
            m.put("PaymentTerm", r.get("lcOrderTermDesc"));
            m.put("FcyAmount", r.get("FcyAmount"));
            m.put("CurrencyCode", r.get("CurrencySymbol"));
            m.put("ExchangeRate", r.get("ExchangeRate"));
            m.put("LcyAmount", r.get("LcyAmount"));
            m.put("GrossWeightKgs", r.get("GrossWeightKgs"));
            m.put("NetWeightKgs", r.get("NetWeightKgs"));
            m.put("NoOfContainers", r.get("NoOfContainers"));
            m.put("Status", r.get("Status"));
            m.put("CommodityDetial", r.get("CommodityDetial"));
            m.put("RemarksHeader", r.get("RemarksHeader"));
            m.put("EntryDate", r.get("EntryDate"));
            m.put("EntryUserName", r.get("EntryUserName"));
            m.put("NoOfAttachments", r.get("NoOfAttachments"));
            out.add(m);
        }
        return out;
    }

    /** DataGridHistory_SelectionChanged -> GridHistoryDetailBind(Id): ImLcOrder.FormHistoryDetail. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = hrm().user(SCREEN);
        header(u, id);
        return pick(repo.lcOrderHistoryDetail(id), "Id", "ImLcOrderId", "ImItemId", "ItemName", "ItemCode", "ItemDescription", "PackingType",
                "PackUom", "Qty", "NetWeight", "RateUom", "ItemRate", "FcyAmount", "LcyAmount", "RemarksDetail");
    }

    /** The order's header, only when it belongs to the signed-in company (ReadById itself does not filter the tenant). */
    private Map<String, Object> header(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.lcOrderById(id);
        if (r.isEmpty() || !sameCompany(r.get(0), u)) throw invalid("Record No found against Current Id" + id + "....");
        return r.get(0);
    }

    // ================================================================== ReadById

    /**
     * ReadById(Id): ImLcOrder.ReadById -> header + packing detail (231: ReadByLcOrderHeaderId) + payment terms.
     * No detail -> "Detail Record No found....". The desktop loads txtdocdate from LcOrderDate; DocDate is loaded here
     * instead, otherwise every Update would overwrite the saved DocDate with the order date (see report).
     */
    public Map<String, Object> byId(int id) {
        UserAccount u = hrm().user(SCREEN);
        Map<String, Object> h = header(u, id);
        List<Map<String, Object>> det = repo.lcOrderPackingDetail(id, toInt(h.get("DocumentTypeId")));
        if (det.isEmpty()) throw invalid("Detail Record No found....");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", pick(List.of(h), "Id", "DocumentTypeId", "DocNo", "DocDate", "LcOrderNo", "LcOrderDate", "SupplierCustomerId",
                "SalesPersonId", "NotifyPartyId", "ShipedToId", "BranchesId", "PartialShipment", "TransShipment", "LastShipmentDate",
                "QuotReference", "InquiryReference", "ExpiryDate", "ExpiryPlace", "LoadingPortId", "DestinationPortId", "InsepctionRequired",
                "LegalizationRequired", "ImporterBankId", "ExporterBankId", "PaymentTermId", "FcurrencyId", "FcyAmount", "ExchangeRate",
                "LcyAmount", "DeliveryTermId", "GrossWeightKgs", "NetWeightKgs", "NoOfContainers", "Status", "RemarksHeader", "CommodityDetial",
                "InsepctionDescription", "LegalizationDescription", "ShippingMarks", "IsApproved").get(0));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : det) {
            rows.add(map("Id", d.get("Id"), "ItemId", d.get("ImItemId"), "ItemName", d.get("ItemName"), "ItemCode", d.get("ItemCode"),
                    "Description", str(d.get("ItemDescription")), "PackingTypeId", d.get("InvPackingTypeId"), "PackingType", d.get("PackingType"),
                    "ItemUomId", d.get("ItemUomId"), "ItemUom", d.get("PackUom"), "Qty", d.get("Qty"), "NetWeight", d.get("NetWeight"),
                    "ItemRate", d.get("ItemRate"), "RateUomId", d.get("RateUomId"), "RateUom", d.get("RateUom"), "FcyAmount", d.get("FcyAmount"),
                    "LcyAmount", d.get("LcyAmount"), "RemarksDetail", str(d.get("RemarksDetail"))));
        }
        out.put("details", rows);
        List<Map<String, Object>> pays = new ArrayList<>();
        for (Map<String, Object> p : repo.lcOrderPaymentTerms(id)) {
            pays.add(map("Id", p.get("Id"), "PaymentTermId", p.get("PaymentTermId"), "PaymentTerm", p.get("PaymentTerm"),
                    "%ofTotal", p.get("PrcntOfTotal"), "FcyAmount", p.get("FcyAmount")));
        }
        out.put("payments", pays);
        return out;
    }

    // ================================================================== save (Insert)

    /**
     * Insert(): "Please Check Detail Grid", FormValidation() in the form's order, the payment grid checks, then the
     * model exactly as the form fills it and ImLcOrder.Save (USP_ImLcOrder_Insert when Id 0, else _Update; every packing row
     * through USP_ImLcOrderPackingDetail_Insert with ActionTypeId 1/2/3; every payment row through
     * USP_ImLcOrderPaymnetTerm_Insert) in one transaction. Derived values (row weight / amount / Lcy, header totals)
     * are recomputed here the way the form computes them.
     */
    public Map<String, Object> save(ImpADtos.LcOrder b) {
        UserAccount u = hrm().user(SCREEN);
        int recId = b.saveAs ? 0 : Math.max(0, b.id);
        hrm().require(u, SCREEN, recId > 0 ? "Update" : "Save");
        if (b.details == null || b.details.isEmpty()) throw invalid("Please Check Detail Grid");
        if (recId > 0) header(u, recId);

        Set<Integer> suppliers = ids(repo.suppliersForExport(u), "Id");
        Set<Integer> salesPersons = ids(repo.suppliersByGroupIds(u, SALES_PERSON_GROUP), "Id");
        Set<Integer> ports = ids(repo.seaPorts(u), "Id");
        Map<String, List<Map<String, Object>>> banks = sup.banks(u);
        Set<Integer> payTerms = ids(repo.lcPaymentTerms(u), "Id");
        Set<Integer> currencies = ids(repo.currencies(u), "Id");
        Set<Integer> delivery = ids(repo.deliveryTerms(), "Id");
        Set<Integer> items = ids(repo.itemsForExport(u), "Id");
        Set<Integer> packs = ids(repo.packMaterialTypes(u), "Id");
        Map<String, Object> fmt = sup.formats(u);
        int fcyDec = toInt(fmt.get("fcyDecimals"));

        // ---------------- detail rows as the grid holds them (btnaddnew_Click / CalculateWeight / CalculateAmount)
        BigDecimal exRate = dec(b.exchangeRate);
        List<ImpALcOrderPackingDetail> rows = new ArrayList<>();
        BigDecimal sumWeight = BigDecimal.ZERO, sumFcy = BigDecimal.ZERO;
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        for (ImpADtos.LcOrderDetail d : b.details) {
            ImpALcOrderPackingDetail vh = detail(u, d, items, packs, uomCache, exRate, b.currencyId);
            vh.Id = recId > 0 ? Math.max(0, d.id) : 0;
            vh.ActionTypeId = vh.Id == 0 ? 1 : 2;
            rows.add(vh);
            sumWeight = sumWeight.add(vh.NetWeight);
            sumFcy = sumFcy.add(vh.FcyAmount);
        }
        // CalculateNetWeightAndFcyAmountInMain: txtTotalFcyAmountMain / txtNetWeightMain are the grid totals as formatted text.
        BigDecimal fcyAmount = fmtRound(sumFcy, fcyDec);
        BigDecimal netWeight = fmtRound(sumWeight, 2);
        // CalculateLocalAmount: Lcy = Fcy * ExchangeRate when both > 0.
        BigDecimal lcyAmount = fcyAmount.signum() > 0 && exRate.signum() > 0 ? fcyAmount.multiply(exRate) : BigDecimal.ZERO;

        // ---------------- FormValidation()
        if (toInt(trim(b.docNo)) == 0) throw invalid("Doc No Field is Required!");
        if (b.supplierId == 0 || !suppliers.contains(b.supplierId)) throw invalid("Supplier Field is Required!");
        if (str(b.lcOrderNo).isEmpty() || "0".equals(str(b.lcOrderNo))) throw invalid("Supplier Order No Field is Required!");
        if (b.salesPersonId == 0 || !salesPersons.contains(b.salesPersonId)) throw invalid("SalesPerson Field is Required!");
        if (b.loadingPortId == 0 || !ports.contains(b.loadingPortId)) throw invalid("Loading Port Field is Required!");
        if (b.destinationPortId == 0 || !ports.contains(b.destinationPortId)) throw invalid("Destination Port Field is Required!");
        if (b.importerBankId == 0 || !ids(banks.get("foreign"), "Id").contains(b.importerBankId)) throw invalid("Importer Bank Field is Required!");
        if (b.exporterBankId == 0 || !ids(banks.get("home"), "Id").contains(b.exporterBankId)) throw invalid("Exporter Bank Field is Required!");
        if (b.paymentTermId == 0 || !payTerms.contains(b.paymentTermId)) throw invalid("Payment Term Field is Required!");
        if (fcyAmount.signum() == 0) throw invalid("Fcy Amount Field is Required!");
        if (b.currencyId == 0 || !currencies.contains(b.currencyId)) throw invalid("Fcy Code Field is Required!");
        if (exRate.signum() == 0) throw invalid("Exchange Rate Field is Required!");
        if (lcyAmount.signum() == 0) throw invalid("Local Amount Field is Required!");
        if (b.deliveryTermId == 0 || !delivery.contains(b.deliveryTermId)) throw invalid("Delivery Term Field is Required!");
        BigDecimal gross = dec(b.grossWeight);
        if (gross.signum() == 0) throw invalid("Gross Weight Field is Required!");
        if (netWeight.signum() == 0) throw invalid("Net Weight Field is Required!");
        // Conversion.ToInt of the two texts (Convert.ToInt32(string): a formatted "1,500" or "12.5" reads as 0).
        if (netInt(b.grossWeight) < netInt(fmtText(netWeight))) throw invalid("Gross Weight Must Be Greater then Net Weight");
        String status = str(b.status);
        if (!("Open".equals(status) || "Complete".equals(status) || "Cancel".equals(status))) throw invalid("Status Field is Required!");

        // ---------------- payment grid (Insert(): row count, totals)
        if (b.payments == null || b.payments.isEmpty()) throw invalid("Payment Term Grid record not found");
        BigDecimal payTotal = BigDecimal.ZERO;
        List<ImpALcOrderPaymnetTerm> pays = new ArrayList<>();
        for (ImpADtos.LcOrderPayment p : b.payments) {
            // The row checks (term, %, duplicates, 100 %) are the Add / Update buttons' (client side); Insert() itself only
            // compares the grid total with the order total. The term id is still checked against the company's list.
            if (p.paymentTermId == 0 || !payTerms.contains(p.paymentTermId)) throw invalid("Payment Term field required");
            ImpALcOrderPaymnetTerm vd = new ImpALcOrderPaymnetTerm();
            vd.Id = Math.max(0, p.id);
            vd.PaymentTermId = p.paymentTermId;
            vd.PrcntOfTotal = BigDecimal.valueOf(toDouble(p.percent));
            vd.FcyAmount = BigDecimal.valueOf(toDouble(p.fcyAmount));
            payTotal = payTotal.add(vd.FcyAmount);
            pays.add(vd);
        }
        if (fcyAmount.compareTo(payTotal) != 0) {
            throw invalid("Payment Term Grid Total Amount not Equal To Total FcyAmount.. PaymentGridAmount is = '" + payTotal.stripTrailingZeros().toPlainString()
                    + "' and TotalFcyAmount is = '" + fcyAmount.toPlainString() + "'");
        }

        // ---------------- the model as Insert() fills it
        LocalDateTime now = LocalDateTime.now();
        ImpALcOrder h = new ImpALcOrder();
        h.Id = recId;
        h.DocumentTypeId = DOC_TYPE;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.BranchesId = branch(u);
        h.FinancialYearId = hrm().financialYearId();
        h.DocNo = toInt(trim(b.docNo));
        h.DocDate = orNow(b.docDate);
        h.SupplierCustomerId = b.supplierId;
        h.LcOrderNo = str(b.lcOrderNo);
        h.LcOrderDate = orNow(b.lcOrderDate);
        h.SalesPersonId = b.salesPersonId;
        h.NotifyPartyId = suppliers.contains(b.notifyPartyId) ? b.notifyPartyId : 0;
        h.ShipedToId = suppliers.contains(b.shippedToId) ? b.shippedToId : 0;
        h.PartialShipment = str(b.partialShipment);
        h.TransShipment = str(b.transShipment);
        h.LastShipmentDate = orNow(b.lastShipmentDate);
        h.QuotReference = str(b.quotReference);
        h.InquiryReference = str(b.inquiryReference);
        h.ExpiryDate = orNow(b.expiryDate);
        h.ExpiryPlace = str(b.expiryPlace);
        h.LoadingPortId = b.loadingPortId;
        h.DestinationPortId = b.destinationPortId;
        h.LegalizationRequired = requiredText(b.legalizationRequired);
        h.InsepctionRequired = requiredText(b.inspectionRequired);
        h.ImporterBankId = b.importerBankId;
        h.ExporterBankId = b.exporterBankId;
        h.PaymentTermId = b.paymentTermId;
        h.FcyAmount = fcyAmount;
        h.FcurrencyId = b.currencyId;
        h.ExchangeRate = exRate;
        h.LcyAmount = lcyAmount;
        h.DeliveryTermId = b.deliveryTermId;
        h.GrossWeightKgs = gross;
        h.NetWeightKgs = netWeight;
        h.NoOfContainers = netInt(b.noOfContainers);
        h.Status = status;
        h.RemarksHeader = str(b.remarksHeader);
        h.CommodityDetial = str(b.commodity);
        h.LegalizationDescription = str(b.legalizationDescription);
        h.InsepctionDescription = str(b.inspectionDescription);
        h.ShippingMarks = str(b.shippingMarks);
        h.IsApproved = false;
        h.EntryDate = now;
        h.EntryUserId = u.getId();
        h.ModifyDate = now;
        h.ModifyUserId = u.getId();
        h.ApprovedDate = now;
        h.ApprovedUserId = u.getId();
        h.ActionId = recId == 0 ? 1 : 2;                        // BLL Save

        // lstRemoveRecord - only on an Update of the loaded order (see report: on Save As they would delete the source order's rows).
        if (recId > 0 && b.removed != null) {
            Set<Integer> saved = ids(repo.lcOrderPackingDetail(recId, DOC_TYPE), "Id");
            for (ImpADtos.LcOrderDetail d : b.removed) {
                if (d.id <= 0 || !saved.contains(d.id)) continue;
                ImpALcOrderPackingDetail vd = detail(u, d, items, packs, uomCache, exRate, b.currencyId);
                vd.Id = d.id;
                vd.ActionTypeId = 3;
                rows.add(vd);
            }
        }
        // Payment rows: the procedure only inserts (a new Id every time) and nothing deletes the old rows, so on an Update
        // only the rows added in this session are sent - the saved ones are already in the table (see report).
        List<ImpALcOrderPaymnetTerm> paysToInsert = new ArrayList<>();
        for (ImpALcOrderPaymnetTerm p : pays) if (recId == 0 || p.Id == 0) paysToInsert.add(p);

        int id = repo.tx(() -> {
            int n = repo.set(recId == 0 ? "[dbo].[USP_ImLcOrder_Insert]" : "[dbo].[USP_ImLcOrder_Update]", h);
            int hid = n > 0 ? n : h.Id;
            for (ImpALcOrderPackingDetail d : rows) {
                d.ImLcOrderId = hid;
                repo.set("USP_ImLcOrderPackingDetail_Insert", d);
            }
            for (ImpALcOrderPaymnetTerm p : paysToInsert) {
                p.ImLcOrderId = hid;
                repo.set("USP_ImLcOrderPaymnetTerm_Insert", p);
            }
            return hid;
        });
        return saved(id, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }

    /** One grdDetail row -> ImLcOrderPackingDetail, with DetailFormValidation()'s checks and the form's calculations. */
    private ImpALcOrderPackingDetail detail(UserAccount u, ImpADtos.LcOrderDetail d, Set<Integer> items, Set<Integer> packs,
                                            Map<Integer, List<Map<String, Object>>> uomCache, BigDecimal exRate, int currencyId) {
        if (d.itemId == 0 || !items.contains(d.itemId)) throw invalid("Item Field is Required!");
        if (d.packingTypeId == 0 || !packs.contains(d.packingTypeId)) throw invalid("Packing Type Field is Required!");
        BigDecimal qty = dec(d.qty);
        if (qty.signum() == 0) throw invalid("Inner Qty Field is Required!");
        List<Map<String, Object>> uoms = uomCache.computeIfAbsent(d.itemId, k -> repo.uomsByItem(u, k));
        Map<String, Object> packUom = find(uoms, d.itemUomId);
        if (d.itemUomId == 0 || packUom == null) throw invalid("Pack UOM Field is Required!");
        // CalculateWeight: Equivalent(pack uom) * Qty shown as "##,#.##".
        BigDecimal eq = dec(packUom.get("Equivalent"));
        BigDecimal weight = eq.signum() > 0 && qty.signum() > 0 ? fmtRound(eq.multiply(qty), 2) : BigDecimal.ZERO;
        if (weight.signum() == 0) throw invalid("Net Weight Field is Required!");
        BigDecimal rate = dec(d.itemRate);
        if (rate.signum() == 0) throw invalid("Rate Field is Required!");
        Map<String, Object> rateUom = find(uoms, d.rateUomId);
        if (d.rateUomId == 0 || rateUom == null) throw invalid("Rate UOM Field is Required!");
        // CalculateAmount: Rate / RateUOM(Equivalent) * Weight (decimal, unformatted).
        BigDecimal req = dec(rateUom.get("Equivalent"));
        BigDecimal amount = weight.signum() > 0 && req.signum() > 0 && rate.signum() > 0
                ? rate.divide(req, 28, java.math.RoundingMode.HALF_EVEN).multiply(weight).setScale(10, java.math.RoundingMode.HALF_EVEN).stripTrailingZeros()
                : BigDecimal.ZERO;
        if (amount.signum() == 0) throw invalid("Amount Field is Required!");
        ImpALcOrderPackingDetail vh = new ImpALcOrderPackingDetail();
        vh.ImItemId = d.itemId;
        vh.ItemBrandId = d.itemId;
        vh.ItemDescription = str(d.description);
        vh.InvPackingTypeId = d.packingTypeId;
        vh.ItemUomId = d.itemUomId;
        vh.Qty = qty;
        vh.NetWeight = weight;
        vh.ItemRate = rate;
        vh.RateUomId = d.rateUomId;
        vh.FcyAmount = amount;
        vh.CurrencyId = currencyId;
        vh.ExchangeRate = exRate;
        // CalculateLcyAmountInDetail: Fcy * ExchangeRate (Fcy itself when the rate is 0).
        vh.LcyAmount = exRate.signum() > 0 ? amount.multiply(exRate) : amount;
        vh.ItemAmount = vh.LcyAmount;
        vh.RemarksDetail = str(d.remarks);
        return vh;
    }

    private static Map<String, Object> find(List<Map<String, Object>> rows, int id) {
        if (id == 0) return null;
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return r;
        return null;
    }

    private static String requiredText(String s) {
        String t = str(s);
        return "Required".equals(t) || "Non-Required".equals(t) ? t : "";
    }

    private static LocalDateTime orNow(String s) {
        LocalDateTime d = toDate(s);
        return d == null ? LocalDateTime.now() : d;
    }

    /** decimal.ToString("##,#.##") as the TextBox holds it (group separators kept, as Convert.ToInt32 then rejects them). */
    private static String fmtText(BigDecimal v) {
        java.text.DecimalFormat f = new java.text.DecimalFormat("#,##0.##");
        String s = f.format(v);
        return "0".equals(s) ? "" : s;
    }

    // ================================================================== delete / print

    /** btnDelete_Click: ImLcOrder.DeleteByID(UserAccount.ID, RecId) (the procedure refuses approved / referred orders). */
    public Map<String, Object> delete(int id) {
        UserAccount u = hrm().user(SCREEN);
        if (id <= 0) throw invalid("No record found to Delete");
        if (!hrm().can(u, SCREEN, "Delete") && !hrm().can(u, SCREEN, "Update")) hrm().require(u, SCREEN, "Delete");
        header(u, id);
        repo.lcOrderDelete(u.getId(), id);
        return saved(id, "Delete Record Seccessfully");
    }

    /**
     * GenerateSlip(PrintId): ImLcOrder_SlipAndRegister (USP_ImLcOrder_Register) must return rows, else
     * "Record Not Found For Display"; the page then prints impa-231 (231-ImLcOrder-Slip.rpt + its payment sub-report).
     */
    public Map<String, Object> printCheck(int id) {
        UserAccount u = hrm().user(SCREEN);
        hrm().require(u, SCREEN, "Print");
        if (id > 0) header(u, id);
        if (repo.lcOrderSlip(u, branch(u), hrm().financialYearId(), Math.max(0, id)).isEmpty()) throw invalid("Record Not Found For Display");
        return map("id", Math.max(0, id));
    }
}
