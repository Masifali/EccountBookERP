package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.VoucherDetail;
import com.mst.models.partyprocessing.PpCModels.VoucherHead;
import com.mst.models.partyprocessing.PpCModels.WagesDetail;
import com.mst.models.partyprocessing.PpCModels.WagesHeader;
import com.mst.repositories.partyprocessing.PpCRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpCSupport.*;

/**
 * 614 Wages Bill (Party Processing) - Architecture.WinApp.PartyProcessing.frmWagesBillPartyProcessing.cs,
 * BLL 0485 / DAL 0536 InvContractorWagesBillHeader (DocumentTypeId 219) with its voucher (MakeVoucher, 219 branch).
 *
 * The form is opened standalone (pending list of every document awaiting wages) or by a party-processing screen after
 * its save with RefDocTypeId / RefDocId / GrossWeightTotal (page query string). Rights: btnsave Save, btnUpdate Update,
 * Print Print. Grid editing (rate lookup, free-of-cost check, weight / qty / bill weight / amount, add / delete rows,
 * "apply first row to all") runs in the page and calls rate() / freeOfCost() here, as the grid events call CommonServices.
 * Not ported: the "Wages Schedule" and "Wages Exempt" buttons (they open two other desktop forms), grid layouts.
 */
@Service
public class PpCWagesBillService {

    public static final int SCREEN = 614;
    public static final int DOC_TYPE = 219;
    private static final DateTimeFormatter DMY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    // ------------------------------------------------------------------ load

    /** Configuration reads of the Load / StichingWagesConfig / WagesAmountCalculationsConfigurations. */
    private Map<String, Object> config(UserAccount u) {
        return m("enableAddLess", pp.configBool(u, "EnableAddLessOnWagesPartyProccessing"),
                "addLessPercentage", toDouble(repo.config(u, "PercentageForRateAddLessPartyProcessing")),
                "amountOnQty", pp.configBool(u, "WagesAmountCalculateOnQty"),
                "otherCompulsory118", pp.configBool(u, "OtherWagesCompulsoryForProductionPartyProcessing"),
                "otherCompulsory176", pp.configBool(u, "OtherWagesCompulsoryForStockConversionPartyProcessing"));
    }

    /** accountName(DocumentTypeId): the wages account ids by reference document type. */
    private static String accountIds(int refDocTypeId, int documentTypeId) {
        if (refDocTypeId == 49 || refDocTypeId == 217 || documentTypeId == 49 || documentTypeId == 117) return "34,36,44";
        if (refDocTypeId == 89 || refDocTypeId == 118 || documentTypeId == 89 || documentTypeId == 118) return "33,36,44";
        if (refDocTypeId == 220 || documentTypeId == 220) return "33,34";
        return null;
    }

    private List<Map<String, Object>> accounts(UserAccount u, String ids) {
        return project(repo.wagesAccounts(u, ids), "Id", "WagesAccountName");
    }

    /** frmWagesBillPartyProcessing_Load (RefDocTypeId / RefDocId from the opener, 0 when opened from the menu). */
    public Map<String, Object> setup(int refDocTypeId, int refDocId) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = config(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("contractors", project(repo.contractors(u), "Id", "CompanyName"));
        out.put("wagesAccounts", accounts(u, accountIds(refDocTypeId, 0)));
        out.put("docNo", repo.wagesCode(u, pp.year()));
        out.put("docTypes", project(repo.wagesDocumentTypes(u), "Id", "DocumentTypeDescription"));
        int recId = 0;
        if (refDocTypeId > 0 && refDocId > 0) {
            recId = repo.wagesIdByReference(u, refDocTypeId, refDocId, pp.year(), null);
            if (recId > 0) {
                out.put("header", header(u, recId));
                out.put("retrieval", retrieval(u, recId));
            }
            out.put("pending", pendingRows(repo.wagesPendingByRef(u, refDocTypeId, refDocId)));
        } else {
            out.put("pending", pendingRows(repo.wagesPendingAll(u, refDocTypeId, refDocId)));
        }
        out.put("recId", recId);
        return out;
    }

    /** New / Reset: GenerateDocNo and the pending grid again (PendingGrnAndGdn or PendingTicket). */
    public Map<String, Object> reset(int refDocTypeId, int refDocId) {
        UserAccount u = pp.user(SCREEN);
        List<Map<String, Object>> pending = refDocTypeId > 0 && refDocId > 0 ? repo.wagesPendingByRef(u, refDocTypeId, refDocId)
                : repo.wagesPendingAll(u, refDocTypeId, refDocId);
        return m("docNo", repo.wagesCode(u, pp.year()), "pending", pendingRows(pending));
    }

    /** btnRefresh_Click: configurations, contractors, wages accounts (accountName() with the opener's type). */
    public Map<String, Object> refresh(int refDocTypeId) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = config(u);
        out.put("contractors", project(repo.contractors(u), "Id", "CompanyName"));
        out.put("wagesAccounts", accounts(u, accountIds(refDocTypeId, 0)));
        return out;
    }

    public Map<String, Object> docTypes() {
        UserAccount u = pp.user(SCREEN);
        return m("docTypes", project(repo.wagesDocumentTypes(u), "Id", "DocumentTypeDescription"));
    }

    /** The pending grid rows: RefDocQty = GrossWeight, RefDocWeight = TotalQty (as the form fills them), RefLineId per Id. */
    private static List<Map<String, Object>> pendingRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        Map<Integer, Integer> line = new HashMap<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            int n = line.merge(id, 1, Integer::sum);
            out.add(m("Id", id, "DocumentTypeId", r.get("DocumentTypeId"), "DocumentTypeDescription", r.get("DocumentTypeDescription"),
                    "DocDate", r.get("DocDate"), "DocNo", r.get("DocNo"), "StockPartyId", r.get("StockPartyId"), "StockParty", r.get("StockParty"),
                    "GpNo", r.get("GpNo"), "VehicleNo", r.get("VehicleNo"), "BiltyNo", r.get("BiltyNo"), "GrossWeight", r.get("GrossWeight"),
                    "TotalQty", r.get("TotalQty"), "JobOrderId", r.get("JobOrderId"), "RefDocQty", r.get("GrossWeight"),
                    "RefDocWeight", r.get("TotalQty"), "RefLineId", n));
        }
        return out;
    }

    private static String dmy(Object v) {
        LocalDateTime d = toDate(v);
        return d == null ? "" : d.format(DMY);
    }

    /** One dtdetail row, in the form's column order. */
    private static Map<String, Object> row(Object supplierId, Object contractorName, Object wagesId, Object wagesAccount, Object wagesType, Object date,
                                           Object packingTypeId, Object packingType, Object weight, Object packSize, Object quantity, Object weightCut,
                                           Object billQty, Object billWeight, Object rateWithoutAddLess, Object rateAddLess, Object rate, Object amount,
                                           Object itemId, Object item, Object jobLotId, Object jobLot, Object crop, Object moveFromId, Object moveFrom,
                                           Object moveToId, Object moveTo, Object purchaseGlac, Object warehouseType, Object isCompany, Object jobOrderId,
                                           Object refDocQty, Object refDocWeight, Object refLineId) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("SupplierId", supplierId); r.put("ContractorName", contractorName); r.put("WagesId", wagesId); r.put("WagesAccount", wagesAccount);
        r.put("WagesType", wagesType); r.put("Date", date); r.put("packingTypeId", packingTypeId); r.put("packingType", packingType);
        r.put("Weight", weight); r.put("PackSize", packSize); r.put("Quantity", quantity); r.put("WeightCut", weightCut); r.put("BillQty", billQty);
        r.put("BillWeight", billWeight); r.put("RateWithoutAddLess", rateWithoutAddLess); r.put("RateAddLess", rateAddLess); r.put("Rate", rate);
        r.put("Amount", amount); r.put("ItemId", itemId); r.put("Item", item); r.put("jobLotId", jobLotId); r.put("jobLot", jobLot); r.put("Crop", crop);
        r.put("MoveFromId", moveFromId); r.put("MoveFrom", moveFrom); r.put("MoveToId", moveToId); r.put("MoveTo", moveTo);
        r.put("PurchaseGLAC", purchaseGlac); r.put("WarehouseType", warehouseType); r.put("IsCompany", isCompany); r.put("JobOrderId", jobOrderId);
        r.put("RefDocQty", refDocQty); r.put("RefDocWeight", refDocWeight); r.put("RefLineId", refLineId);
        return r;
    }

    /**
     * LoadDataForWages (the pending row's Load button): stock party / reference type / numbers of the row; for a stock
     * conversion (176) the saved bill of the same entry type is looked up; the reference detail becomes the Regular Wages
     * rows, and the Other Wages rows / accounts by reference type. The "Reset Form First" check runs in the page.
     */
    public Map<String, Object> load(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int pageRefType = toInt(b.get("refDocTypeId"));
        int id = toInt(b.get("Id"));
        int docType = toInt(b.get("DocumentTypeId"));
        String entryType = trim(b.get("DocumentTypeDescription"));
        Map<String, Object> out = m("stockPartyId", b.get("StockPartyId"), "stockParty", b.get("StockParty"), "refDocTypeId", docType,
                "refDocType", b.get("DocumentTypeDescription"), "docNo", str(b.get("DocNo")), "grnId", str(b.get("Id")), "gpNo", str(b.get("GpNo")),
                "grossWeight", str(b.get("GrossWeight")), "qty", str(b.get("TotalQty")), "entryType", entryType);
        String reqType = null;
        if (docType == 176) {
            reqType = entryType;
            int recId = repo.wagesIdByReference(u, docType, id, pp.year(), reqType);
            out.put("recId", recId);
            if (recId > 0) out.put("header", header(u, recId));
        }
        List<Map<String, Object>> src = repo.wagesReferenceDetail(u, id, docType, reqType);
        List<Map<String, Object>> detail = new ArrayList<>();
        List<Map<String, Object>> other = new ArrayList<>();
        boolean onQty = pp.configBool(u, "WagesAmountCalculateOnQty");
        if (!src.isEmpty()) {
            double gross = 0, qty = 0;
            int i = 0;
            for (Map<String, Object> r : src) {
                i++;
                LocalDateTime d = toDate(r.get("DocDate"));
                boolean free = repo.freeOfCost(u, d, docType, toInt(r.get("ItemId")), 0);
                double eq = dbl(r.get("Equivalent")), q = dbl(r.get("Qty")), gw = dbl(r.get("GrossWeight"));
                if (onQty) {
                    detail.add(row(0, 0, 0, 0, free ? "Free Of Cost" : "Regular", dmy(d), r.get("PackingTypeId"), r.get("PackTypeCode"), q * eq,
                            r.get("Equivalent"), q, 0, 0, q * eq, 0, 0, 0, 0, r.get("ItemId"), r.get("ItemName"), r.get("JlId"), r.get("JobLotDescription"),
                            r.get("CropYear"), r.get("WhFId"), r.get("WarehouseFrom"), r.get("WhTId"), r.get("WareHouseTo"), r.get("PurchaseGLAC"),
                            r.get("WarehouseType"), true, r.get("JobOrderId"), q, q * eq, i));
                    gross += gw;
                    qty += q;
                } else {
                    detail.add(row(0, 0, 0, 0, free ? "Free Of Cost" : "Regular", dmy(d), r.get("PackingTypeId"), r.get("PackTypeCode"), r.get("GrossWeight"),
                            r.get("Equivalent"), gw / eq, 0, 0, r.get("GrossWeight"), 0, 0, 0, 0, r.get("ItemId"), r.get("ItemName"), r.get("JlId"),
                            r.get("JobLotDescription"), r.get("CropYear"), r.get("WhFId"), r.get("WarehouseFrom"), r.get("WhTId"), r.get("WareHouseTo"),
                            r.get("PurchaseGLAC"), r.get("WarehouseType"), true, r.get("JobOrderId"), gw / eq, gw, i));
                }
            }
            if (onQty) {
                out.put("grossWeight", clr(gross));
                out.put("qty", clr(qty));
            }
            List<Map<String, Object>> otherAccounts = null;
            boolean otherTab = false;
            if (pageRefType == 118 || docType == 118 || pageRefType == 176 || docType == 176) {
                otherAccounts = accounts(u, "35");
                for (Map<String, Object> r : detail) if (dbl(r.get("PackSize")) < 100.0) other.add(new LinkedHashMap<>(r));
                if (!other.isEmpty()) otherTab = true;
            }
            if (pageRefType == 220 || docType == 220) {
                otherTab = true;
                otherAccounts = accounts(u, "34");
                for (Map<String, Object> r : detail) other.add(new LinkedHashMap<>(r));
            }
            out.put("otherTab", otherTab);
            if (otherAccounts != null) out.put("otherAccounts", otherAccounts);
            out.put("wagesAccounts", accounts(u, accountIds(pageRefType, docType)));
            out.put("docDate", detail.get(0).get("Date"));
            out.put("detail", detail);
            out.put("other", other);
        }
        return out;
    }

    private Map<String, Object> header(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.wages(id);
        if (r.isEmpty() || !PpCScreens.owns(u, r.get(0))) throw invalid("Record Not Found");
        Map<String, Object> h = r.get(0);
        return m("Id", toInt(h.get("Id")), "DocNo", toInt(h.get("DocNo")), "DocDate", h.get("DocDate"), "RefDocumentTypeId", toInt(h.get("RefDocumentTypeId")),
                "DocumentTypeDescription", str(h.get("DocumentTypeDescription")), "RefDocNo", toInt(h.get("RefDocNo")), "RefDocNoId", toInt(h.get("RefDocNoId")),
                "ScaleSlipNo", toInt(h.get("ScaleSlipNo")), "WeightTotal", dbl(h.get("WeightTotal")), "QtyTotal", dbl(h.get("QtyTotal")),
                "OtherRemarks", str(h.get("OtherRemarks")), "RefDocument", str(h.get("RefDocument")), "IsAproved", toBool(h.get("IsAproved")),
                "StockPartyId", toInt(h.get("StockPartyId")), "StockPartyName", str(h.get("StockPartyName")));
    }

    /** RetreivalHistoryDetailGridBind: the saved rows shown under "Previous Record" when the opener's document already has a bill. */
    private List<Map<String, Object>> retrieval(UserAccount u, int id) {
        Map<String, Object> h = header(u, id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.wagesDetails(id)) {
            double rate = dbl(d.get("WageRate")), addLess = dbl(d.get("RateAddLess"));
            out.add(m("HeaderId", d.get("InvContractorWagesBillHeaderId"), "ContractorName", d.get("CompanyName"), "WagesAccount", d.get("WagesAccountName"),
                    "PackingType", d.get("PackTypeDesc"), "Weight", dbl(d.get("Weight")), "PackSize", d.get("PackSize"), "Quantity", dbl(d.get("Qty")),
                    "WeightCut", d.get("WeightCut"), "BillWeight", dbl(d.get("BillWeight")), "RateWithoutAddLess", rate - addLess, "RateAddLess", addLess,
                    "Rate", rate, "Amount", dbl(d.get("WagesAmount")), "HeaderWeight", h.get("WeightTotal"), "HeaderQty", h.get("QtyTotal"),
                    "IsCompany", toBool(d.get("IsCompany")), "JobOrderId", toInt(d.get("JobOrderId")), "RefDocQty", dbl(d.get("RefDocQty")),
                    "RefDocWeight", dbl(d.get("RefDocWeight")), "RefLineId", toInt(d.get("RefLineId"))));
        }
        return out;
    }

    private static Map<String, Object> savedRow(Map<String, Object> d, boolean fromOther) {
        double rate = dbl(d.get("WageRate")), addLess = dbl(d.get("RateAddLess"));
        return row(toInt(d.get("ContractorId")), d.get("CompanyName"), toInt(d.get("InvConractorWagesAccountsId")), toInt(d.get("WagesTypeId")),
                toBool(d.get("FreeOfCost")) ? "Free Of Cost" : "Regular", dmy(d.get("RefDocDate")), d.get("InvPackingTypeId"), d.get("PackTypeDesc"),
                dbl(d.get("Weight")), d.get("PackSize"), dbl(d.get("Qty")), d.get("WeightCut"), dbl(d.get("BillQty")), dbl(d.get("BillWeight")),
                rate - addLess, addLess, rate, dbl(d.get("WagesAmount")), d.get("ItemId"), d.get("ItemName"), d.get("JobLotId"), d.get("JobLotDescription"),
                d.get("Crop"), d.get("WareHouseFromId"), d.get("WareHouseFrom"), d.get("WareHouseToId"), d.get("WareHouseTo"), d.get("PurchaseGLAC"),
                fromOther ? d.get("WarehouseType") : "", toBool(d.get("IsCompany")), toInt(d.get("JobOrderId")), dbl(d.get("RefDocQty")),
                dbl(d.get("RefDocWeight")), toInt(d.get("RefLineId")));
    }

    /** ReadById (history Edit / double click): WagesTypeId 2 rows go to Other Wages; the rest are Regular Wages. */
    public Map<String, Object> read(int id) {
        UserAccount u = pp.user(SCREEN);
        if (id == 0) throw invalid("Record Not Found");
        Map<String, Object> h = header(u, id);
        int refType = toInt(h.get("RefDocumentTypeId"));
        List<Map<String, Object>> detail = new ArrayList<>(), other = new ArrayList<>();
        List<Map<String, Object>> otherAccounts = null;
        double gross = 0;
        for (Map<String, Object> d : repo.wagesDetails(id)) {
            if (toInt(d.get("WagesTypeId")) == 2) {
                otherAccounts = accounts(u, "35");
                other.add(savedRow(d, true));
            } else {
                detail.add(savedRow(d, false));
                gross += dbl(d.get("Weight"));
            }
        }
        String entryType = str(h.get("RefDocument"));
        if (other.isEmpty() && ((refType == 176 && !entryType.contains("Issue")) || refType == 118)) {
            otherAccounts = accounts(u, "34");
            for (Map<String, Object> r : detail) if (dbl(r.get("PackSize")) < 100.0) other.add(blankOther(r));
        } else if (other.isEmpty() && refType == 220) {
            otherAccounts = accounts(u, "34");
            for (Map<String, Object> r : detail) other.add(blankOther(r));
        }
        Map<String, Object> out = m("header", h, "grossWeight", clr(gross), "detail", detail, "other", other, "otherTab", !other.isEmpty(),
                "wagesAccounts", accounts(u, accountIds(0, refType)));
        if (otherAccounts != null) out.put("otherAccounts", otherAccounts);
        return out;
    }

    /** The Other Wages row ReadById builds from a Regular row: no contractor / account, rate and amount 0. */
    private static Map<String, Object> blankOther(Map<String, Object> r) {
        return row(0, "", 0, "", r.get("WagesType"), r.get("Date"), r.get("packingTypeId"), r.get("packingType"), r.get("Weight"), r.get("PackSize"),
                r.get("Quantity"), r.get("WeightCut"), r.get("BillQty"), r.get("BillWeight"), 0, 0, 0, 0, r.get("ItemId"), r.get("Item"), r.get("jobLotId"),
                r.get("jobLot"), r.get("Crop"), r.get("MoveFromId"), r.get("MoveFrom"), r.get("MoveToId"), r.get("MoveTo"), r.get("PurchaseGLAC"),
                r.get("WarehouseType"), r.get("IsCompany"), r.get("JobOrderId"), r.get("RefDocQty"), r.get("RefDocWeight"), r.get("RefLineId"));
    }

    // ------------------------------------------------------------------ grid helpers

    /** CommonServices.GetWagesRate(Date, PackSize, WagesId, SupplierId).WagesRate (0 when no schedule row). */
    public Map<String, Object> rate(String date, double packSize, int wagesId, int contractorId) {
        UserAccount u = pp.user(SCREEN);
        return m("rate", repo.wagesRate(u, clrDate(date), packSize, wagesId, contractorId));
    }

    /** CommonServices.CheckItemsFreeofcostforWages(Date, reference type, ItemId, WagesId). */
    public Map<String, Object> freeOfCost(String date, int refDocTypeId, int itemId, int wagesId) {
        UserAccount u = pp.user(SCREEN);
        return m("free", repo.freeOfCost(u, clrDate(date), refDocTypeId, itemId, wagesId));
    }

    /**
     * ValidationOnformClose: when the loaded rows no longer match the previously saved totals and the user confirmed,
     * WagesDeleteByRefDocTypeAndId. The desktop passes RefDocId in the qty branch but RECID in the weight branch;
     * the page sends the value the desktop would send.
     */
    public Map<String, Object> deleteByReference(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        repo.wagesDeleteByRef(u, toInt(b.get("refDocTypeId")), toInt(b.get("refDocId")));
        return m("ok", true);
    }

    // ------------------------------------------------------------------ save

    /** ValidationforRefRowWeight / ValidationforRefRowQty over one grid. */
    private static void refRowCheck(List<Map<String, Object>> rows, List<Map<String, Object>> accounts, String grid, boolean onQty) {
        Map<String, double[]> sums = new LinkedHashMap<>();
        Map<String, Object[]> keys = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            int w = toInt(r.get("WagesId")), line = toInt(r.get("RefLineId"));
            double ref = dbl(r.get(onQty ? "RefDocQty" : "RefDocWeight"));
            String k = w + "|" + line + "|" + ref;
            sums.computeIfAbsent(k, x -> new double[1])[0] += dbl(r.get(onQty ? "Quantity" : "BillWeight"));
            keys.put(k, new Object[]{w, line, ref});
        }
        for (Map.Entry<String, double[]> e : sums.entrySet()) {
            Object[] k = keys.get(e.getKey());
            double total = e.getValue()[0], ref = (double) k[2];
            if (total > ref) {
                Map<String, Object> a = find(accounts, "Id", (int) k[0]);
                String name = a == null ? "" : str(a.get("WagesAccountName"));
                if (onQty) throw invalid("TotalQty against Reference RowNo and Wages Account Should be Equal to or less than Reference Row Qty\n"
                        + "Here TotalQty (" + clr(total) + ") exceeds Reference Row Qty (" + clr(ref) + ") for WagesAccount (" + name + ") and RowNo " + k[1] + " in " + grid + " Grid");
                throw invalid("TotalBillWeight against Reference RowNo and Account Should be Equal to or less than Reference Row Weight\n"
                        + "Here TotalBillWeight (" + clr(total) + ") exceeds Reference Row Weight (" + clr(ref) + ") for WagesAccount (" + name + ") and RowNo " + k[1] + "  in " + grid + " Grid");
            }
        }
    }

    private static boolean blank(Map<String, Object> r, String k) { return toDouble(r.get(k)) == 0 || str(r.get(k)).trim().isEmpty(); }

    /** Insert(): FormValidation, total / reference-row checks, the detail rows as the form builds them, BLL Save. */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        String docNo = trim(b.get("docNo"));
        if (docNo.isEmpty() || docNo.equals("0")) throw invalid("document Number Field Required");
        int refType = toInt(b.get("refDocTypeId"));
        if (refType == 0) throw invalid("ReferenceDocType Field Required");
        if (toInt(b.get("stockPartyId")) == 0) throw invalid("Stock Party Field Required");
        String refNo = trim(b.get("refDocNo"));
        if (refNo.isEmpty() || refNo.equals("0")) throw invalid("Doc No Field Required");
        String grnId = trim(b.get("refDocNoId"));
        if (grnId.isEmpty() || grnId.equals("0")) throw invalid("DocNoId Field Required");
        if (recId > 0) header(u, recId);

        boolean onQty = pp.configBool(u, "WagesAmountCalculateOnQty");
        boolean otherCompulsory = refType == 118 ? pp.configBool(u, "OtherWagesCompulsoryForProductionPartyProcessing")
                : refType == 176 && pp.configBool(u, "OtherWagesCompulsoryForStockConversionPartyProcessing");
        List<Map<String, Object>> detail = list(b.get("detail")), other = list(b.get("other"));
        double grossWeight = toDouble(b.get("grossWeight"));
        if (!onQty) {
            double t = 0;
            for (Map<String, Object> r : detail) t += dbl(r.get("Weight"));
            if (t != grossWeight) throw invalid("Wages Grid Total Weight and GrossWeight not equal please check!");
            if (refType == 118) {
                double t2 = 0;
                for (Map<String, Object> r : other) t2 += dbl(r.get("Weight"));
                if (t2 != grossWeight) throw invalid("Other Wages Grid Total Weight and GrossWeight not equal please check!");
            }
        }
        int pageRefType = toInt(b.get("pageRefDocTypeId"));
        if (refType == 118 || refType == 176 || pageRefType == 118 || pageRefType == 176) {
            List<Map<String, Object>> accounts = accounts(u, accountIds(pageRefType, refType));
            refRowCheck(detail, accounts, "Regular_Wages", onQty);
            refRowCheck(other, accounts, "Other_Wages", onQty);
        }

        LocalDateTime now = LocalDateTime.now();
        WagesHeader o = new WagesHeader();
        o.Id = recId;
        o.CompanyId = u.getCompanyId();
        o.OrganizationId = u.getOrganizationId();
        o.BranchesId = branch(u);
        o.DocNo = toInt(docNo);
        o.DocDate = orNow(picker(b.get("docDate")));
        o.DocumentTypeId = DOC_TYPE;
        o.StockPartyId = toInt(b.get("stockPartyId"));
        o.RefDocumentTypeId = refType;
        o.RefDocNoId = toInt(grnId);
        o.RefDocNo = toInt(refNo);
        o.WeightTotal = grossWeight;
        o.OtherRemarks = str(b.get("remarks"));
        o.EntryUser = uid(u);
        o.ModifyUser = uid(u);
        o.ScaleSlipNo = toInt(trim(b.get("gpNo")));
        o.RefDocument = trim(b.get("entryType"));
        o.ModifyDate = now;
        o.EntryDate = now;
        o.FinancialYearId = pp.year();
        if (toBool(b.get("approved"))) {
            o.IsAproved = true;
            o.ApprovedDate = now;
            o.ApprovedUserId = uid(u);
        } else {
            o.IsAproved = false;
        }
        if (detail.isEmpty()) throw invalid("Regular Wages Grid... Record Not Found");
        for (Map<String, Object> r : detail) {
            WagesDetail d = new WagesDetail();
            /* Desktop: the header DocDate becomes each row's Date in turn (the last row's date is saved); the check below can then never fail. */
            o.DocDate = clrDate(r.get("Date"));
            d.RefDocDate = clrDate(r.get("Date"));
            if (o.DocDate.toLocalDate().isBefore(d.RefDocDate.toLocalDate())) throw invalid("Doc Date Can't be " + o.DocDate.toLocalDate() + " Beacuse Detail Date " + d.RefDocDate + " is Greater");
            String g = " In Regular Wages Grid...";
            if (toInt(r.get("WagesId")) == 0) throw invalid("WagesAccount Field required" + g);
            d.InvConractorWagesAccountsId = toInt(r.get("WagesId"));
            if ((int) dbl(r.get("Quantity")) == 0) throw invalid("Quantity Field required" + g);
            d.Qty = dbl(r.get("Quantity"));
            d.BillQty = d.Qty;
            if (blank(r, "Weight")) throw invalid("Weight Field required" + g);
            d.Weight = dbl(r.get("Weight"));
            d.BillWeight = dbl(r.get("Weight"));
            if (blank(r, "PackSize")) throw invalid("PackSize Field required" + g);
            d.PackSize = dbl(r.get("PackSize"));
            String type = str(r.get("WagesType"));
            if (type.equals("Free Of Cost") || str(r.get("WarehouseType")).equals("Dryer")) {
                d.WageRate = 0; d.RateAddLess = 0; d.WagesAmount = 0;
            } else {
                d.RateAddLess = dbl(r.get("RateAddLess"));
                d.WageRate = dbl(r.get("Rate"));
                if (d.WageRate == 0) throw invalid("Rate Field required" + g);
            }
            d.FreeOfCost = type.equals("Free Of Cost");
            if (d.FreeOfCost) d.WagesAmount = 0;
            else if (onQty) d.WagesAmount = round(d.Qty * d.WageRate, 2);
            else d.WagesAmount = round(d.Weight / d.PackSize * d.WageRate, 2);
            if (toInt(r.get("SupplierId")) == 0) throw invalid("ContractorAccount Field required" + g);
            d.ContractorId = toInt(r.get("SupplierId"));
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Name Field required" + g);
            d.ItemId = toInt(r.get("ItemId"));
            if (toInt(r.get("PurchaseGLAC")) == 0) throw invalid("PurchaseGLAC Field required" + g);
            d.ItemName = str(r.get("Item"));
            d.WagesAccountName = str(r.get("WagesText"));
            /* Conversion.ToInt("Regular" / "Free Of Cost") = 0. */
            d.WagesTypeId = 0;
            fillCommon(d, r);
            d.BillQty = dbl(r.get("BillQty"));
            o.details.add(d);
        }
        boolean issue = o.RefDocument.contains("Issue");
        if (other.isEmpty() && otherCompulsory && ((refType == 176 && !issue) || refType == 118 || refType == 220))
            throw invalid("Stiching Wages Grid... Record Not Found\n Stiching Wages Compulsory Configuration is On");
        for (Map<String, Object> r : other) {
            if (!otherCompulsory && toInt(r.get("WagesId")) == 0 && toInt(r.get("SupplierId")) == 0) continue;
            String g = " In Other Wages Grid ...";
            WagesDetail d = new WagesDetail();
            if (toInt(r.get("WagesId")) == 0) throw invalid("WagesAccount Field required" + g);
            d.InvConractorWagesAccountsId = toInt(r.get("WagesId"));
            if ((int) dbl(r.get("Quantity")) == 0) throw invalid("Quantity Field required" + g);
            d.Qty = dbl(r.get("Quantity"));
            d.BillQty = d.Qty;
            if (blank(r, "Weight")) throw invalid("Weight Field required" + g);
            d.Weight = dbl(r.get("Weight"));
            d.BillWeight = dbl(r.get("Weight"));
            if (blank(r, "PackSize")) throw invalid("PackSize Field Required" + g);
            d.PackSize = dbl(r.get("PackSize"));
            String type = str(r.get("WagesType"));
            if (type.equals("Free Of Cost") || str(r.get("WarehouseType")).equals("Dryer")) {
                d.RateAddLess = 0; d.WageRate = 0; d.WagesAmount = 0;
            } else {
                d.RateAddLess = dbl(r.get("RateAddLess"));
                d.WageRate = dbl(r.get("Rate"));
                if (d.WageRate == 0) throw invalid("Rate Field required" + g);
                d.WagesAmount = onQty ? round(d.Qty * d.WageRate, 2) : round(d.Weight / d.PackSize * d.WageRate, 2);
            }
            if (toInt(r.get("SupplierId")) == 0) throw invalid("ContractorAccount Field required" + g);
            d.ContractorId = toInt(r.get("SupplierId"));
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Name Field required" + g);
            d.ItemId = toInt(r.get("ItemId"));
            if (toInt(r.get("PurchaseGLAC")) == 0) throw invalid("PurchaseGLAC Field required" + g);
            d.ItemName = str(r.get("Item"));
            d.WagesAccountName = str(r.get("WagesText"));
            d.RefDocDate = clrDate(r.get("Date"));
            fillCommon(d, r);
            /* Other grid: BillQty = Conversion.ToInt(BillQty text). */
            d.BillQty = (int) dbl(r.get("BillQty"));
            d.FreeOfCost = type.equals("Free Of Cost");
            d.WagesTypeId = 2;
            o.details.add(d);
        }
        o.QtyTotal = toDouble(b.get("qty"));
        o.WeightTotal = grossWeight;

        VoucherHead v = makeVoucher(u, o);
        String proc = o.Id == 0 ? "Sp_InvContractorWagesBillHeader_Insert" : "Sp_InvContractorWagesBillHeader_Update";
        int id = repo.tx(() -> {
            int n = repo.set(proc, o);
            if (n > 0) o.Id = n; else n = o.Id;
            for (WagesDetail d : o.details) {
                d.InvContractorWagesBillHeaderId = o.Id;
                repo.set("Sp_InvContractorWagesBillDetail_Insert", d);
            }
            if (!v.voucherDetailList.isEmpty()) {
                pp.writeVoucher(u, v, DOC_TYPE, o.Id, false, true, false);
            } else {
                repo.exec("usp_ContractorWagesVoucherDeleteByWagesId", PpCRepository.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "DocumentTypeId", DOC_TYPE, "Id", o.Id));
            }
            if (o.RefDocumentTypeId == 806 || o.RefDocumentTypeId == 68) {
                repo.exec("[dbo].[usp_WagesProportionateToStockEvaluation]", PpCRepository.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "RefDocumentTypeId", o.RefDocumentTypeId, "RefDocNoId", o.RefDocNoId));
            }
            return n;
        });
        return m("ok", true, "id", id, "message", (recId > 0 ? "Updated SuccessFully  [" : "Saved SuccessFully  [") + o.DocNo + "]");
    }

    private static void fillCommon(WagesDetail d, Map<String, Object> r) {
        d.JobLotId = toInt(r.get("jobLotId"));
        d.Crop = str(r.get("Crop"));
        d.InvPackingTypeId = toInt(r.get("packingTypeId"));
        d.WareHouseFromId = toInt(r.get("MoveFromId"));
        d.WareHouseToId = toInt(r.get("MoveToId"));
        d.WeightCut = dbl(r.get("WeightCut"));
        d.IsCompany = toBool(r.get("IsCompany"));
        d.JobOrderId = toInt(r.get("JobOrderId"));
        d.RefDocQty = dbl(r.get("RefDocQty"));
        d.RefDocWeight = dbl(r.get("RefDocWeight"));
        d.RefLineId = toInt(r.get("RefLineId"));
    }

    /** BLL InvContractorWagesBillHeader.MakeVoucher, DocumentTypeId 219 branch (RefDocNoId = the header Id at that moment). */
    private VoucherHead makeVoucher(UserAccount u, WagesHeader o) {
        VoucherHead v = new VoucherHead();
        LocalDateTime now = LocalDateTime.now();
        v.DocumentTypeId = o.DocumentTypeId;
        v.DocumentTypeSrNo = o.Id;
        v.RefDocNoId = o.Id;
        v.VoucherCode = o.DocNo;
        v.VoucherDate = o.DocDate;
        v.Remarks = str(o.OtherRemarks);
        v.RemarksOtherLingo = "";
        v.ChequeDate = LocalDate.now().atStartOfDay();
        v.IncludeWHT = false;
        v.BranchId = o.BranchesId;
        v.ProjectId = o.ProjectsId;
        v.BillAmount = 0;
        v.ManualBillNo = String.valueOf(o.RefDocNo);
        v.DueDate = o.DocDate;
        v.DueDays = 0;
        v.OrganizationId = o.OrganizationId;
        v.CompanyId = o.CompanyId;
        v.FinancialYearId = o.FinancialYearId;
        v.EntryUser = o.EntryUser;
        v.EntryDate = now;
        v.ModifyDate = now;
        v.ModifyUser = o.ModifyUser;
        if (o.details.isEmpty()) return v;
        if (o.StockPartyId == 0) throw invalid("StockPartyId cannot be equal to zero");
        List<Map<String, Object>> parties = repo.supplierCustomerGl(u);
        List<Map<String, Object>> accounts = repo.wagesAccountsAll(u);
        for (WagesDetail d : o.details) {
            Map<String, Object> contractor = find(parties, "Id", d.ContractorId);
            if (contractor == null) throw invalid("Contactor GlAccountId not found against " + d.ContractorId);
            int debit;
            if (d.IsCompany) {
                if (accounts.isEmpty()) throw invalid("Contactor Wages Accounts data is empty");
                Map<String, Object> a = find(accounts, "Id", d.InvConractorWagesAccountsId);
                if (a == null) throw invalid("Activity GlAccountId not found against " + d.InvConractorWagesAccountsId);
                debit = toInt(a.get("GlAccountId"));
            } else {
                Map<String, Object> sp = find(parties, "Id", o.StockPartyId);
                if (sp == null) throw invalid("StockParty GlAccountId not found against " + o.StockPartyId);
                debit = toInt(sp.get("GlAccountId"));
            }
            if (debit == 0) throw invalid("DebitAccountId cannot be equal to zero for Party Processing");
            int credit = toInt(contractor.get("GlAccountId"));
            String text = o.RefDocument + "  :" + str(d.WagesAccountName) + " Item Name :" + str(d.ItemName) + "  Wages Qty :  " + clr(d.Qty)
                    + "  Wages Rate :  " + clr(d.WageRate) + "  Wages Amount :  " + clr(d.WagesAmount) + "  BillWeight :  " + clr(d.BillWeight);
            VoucherDetail dr = new VoucherDetail();
            dr.AccountId = debit;
            dr.AgainstAccountId = credit;
            dr.Comments = text;
            dr.DebitAmount = d.WagesAmount;
            dr.ItemAmount = d.WagesAmount;
            dr.ItemRate = d.WageRate;
            dr.OrderNo = o.JobOrderId;
            v.voucherDetailList.add(dr);
            VoucherDetail cr = new VoucherDetail();
            cr.AccountId = credit;
            cr.AgainstAccountId = debit;
            cr.Comments = text;
            cr.CreditAmount = d.WagesAmount;
            cr.ItemAmount = d.WagesAmount;
            cr.ItemRate = d.WageRate;
            cr.OrderNo = o.JobOrderId;
            v.voucherDetailList.add(cr);
        }
        return v;
    }

    // ------------------------------------------------------------------ history

    /** bindHistory: FormHistoryNew (DocumentTypeId 219, active year, dates when ticked, doc-no range, reference type). */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = PpCRepository.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", DOC_TYPE);
        int y = pp.year();
        if (y != 0) p.put("FinancialYearId", y);
        if (toBool(b.get("fromChecked")) && picker(b.get("from")) != null) p.put("FromDate", ts(picker(b.get("from"))));
        if (toBool(b.get("toChecked")) && picker(b.get("to")) != null) p.put("ToDate", ts(picker(b.get("to"))));
        int f = toInt(b.get("fromDocNo")), t = toInt(b.get("toDocNo")), rt = toInt(b.get("refDocTypeId"));
        if (f != 0) p.put("FromDocNo", f);
        if (t != 0) p.put("ToDocNo", t);
        if (rt != 0) p.put("RefDocumentTypeId", rt);
        return project(repo.wagesHistory(p), "Id", "DocumentTypeId", "RefDocumentTypeId", "RefDocNoId", "DocumentType=DocumentTypeDescription", "DocDate",
                "DocNo", "RefDocNo", "StockPartyId", "StockParty", "QtyTotal", "WeightTotal", "WagesAmount", "OtherRemarks", "JobOrderId", "JobOrderNo",
                "EntryDate", "EntryUser=UserName", "ModifyDate", "ModifyUser", "IsReferred");
    }

    /** HistoryDetailGridBind. */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = pp.user(SCREEN);
        header(u, id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.wagesDetails(id)) {
            double rate = dbl(d.get("WageRate")), addLess = dbl(d.get("RateAddLess"));
            out.add(m("HeaderId", d.get("InvContractorWagesBillHeaderId"), "ContractorName", d.get("CompanyName"), "WagesAccount", d.get("WagesAccountName"),
                    "packingType", d.get("PackTypeDesc"), "Weight", dbl(d.get("Weight")), "PackSize", d.get("PackSize"), "Quantity", dbl(d.get("Qty")),
                    "WeightCut", d.get("WeightCut"), "BillWeight", dbl(d.get("BillWeight")), "RateWithoutAddLess", rate - addLess, "RateAddLess", addLess,
                    "Rate", rate, "Amount", dbl(d.get("WagesAmount")), "IsCompany", toBool(d.get("IsCompany"))));
        }
        return out;
    }

    /** History "Voucher": VoucherHeadIdGet(Id, 219) -> VoucherReport_118. */
    public Map<String, Object> voucher(int id) {
        UserAccount u = pp.user(SCREEN);
        header(u, id);
        return m("voucherHeadId", repo.voucherHeadId(u, DOC_TYPE, id));
    }

    /** Print (Print right) / history Slip: ContractorWagesPartyProcessingSlip(Id). */
    public Map<String, Object> printCheck(int id, boolean fromHistory) {
        UserAccount u = pp.user(SCREEN);
        if (!fromHistory) pp.require(u, SCREEN, "Print");
        header(u, id);
        return m("id", id);
    }
}
