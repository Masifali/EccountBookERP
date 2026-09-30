package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.InvTransFifo;
import com.mst.models.partyprocessing.PpCModels.VoucherDetail;
import com.mst.models.partyprocessing.PpCModels.VoucherHead;
import com.mst.repositories.partyprocessing.PpCRepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * Shared BLL plumbing of the Party Processing "PpC" screens (676, 677, 675, 602, 603, 604, 614).
 *
 * Tenancy (organization, company, branch, user, active financial year) always comes from the signed-in
 * user; the page only sends what the operator typed or picked. Rights are the desktop's
 * SetRightsValueInRightsObject flags read from the screen's own dbo.ScreenDefinition id through
 * {@link HrmSupport}; "CanView AllRecord" feeds the history procedures' @CanViewAllRecord / @EntryUser.
 *
 * Also here, because several screens share them:
 *   - DAL CommonServices.FIFOImplementionForPartyProcessing (stock adjustment Loss, stock transfer)
 *   - the voucher write the Processing Bill and Wages Bill DALs do (Sp_VoucherHead_* / Sp_VoucherDetail_*)
 *   - the history filter the desktop builds from its date radio buttons and checked date pickers.
 */
@Component
public class PpCSupport {

    @Autowired HrmSupport hrm;
    @Autowired PpCRepository repo;

    /** Signed-in user with View on the screen (every API of the screen). */
    public UserAccount user(int screenId) { return hrm.user(screenId); }

    public void require(UserAccount u, int screenId, String right) { hrm.require(u, screenId, right); }

    /** SetRightsValueInRightsObject: save / update / delete / print / viewAll. */
    public Map<String, Object> rights(UserAccount u, int screenId) {
        Map<String, Object> r = hrm.rights(u, screenId);
        r.put("viewAll", viewAll(u, screenId));
        return r;
    }

    /** DoHaveCanViewAllRecordRights (RightName "CanView AllRecord"). */
    public boolean viewAll(UserAccount u, int screenId) { return hrm.can(u, screenId, "CanView AllRecord"); }

    /** clsGlobalVariables.ActiveYr.Id. */
    public int year() { return hrm.financialYearId(); }

    public static int branch(UserAccount u) { return toInt(u.getBranchesId()); }
    public static int uid(UserAccount u) { return toInt(u.getId()); }

    /** GetConfigurationByOrgCompandConfigDescription as Conversion.ToBool. */
    public boolean configBool(UserAccount u, String name) { return toBool(repo.config(u, name)); }

    /** DefaultDaysToLessFromHistoryFromDate (Conversion.ToInt). */
    public int defaultDays(UserAccount u) { return toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }

    /** clsGlobalVariables.WagesRefDocumentsStatusList.Find(RefDocumentTypeId == t)?.IsActive (false when there is no row). */
    public boolean wagesActive(int refDocumentTypeId) {
        try {
            for (Map<String, Object> r : repo.wagesRefDocuments()) {
                if (toInt(r.get("RefDocumentTypeId")) == refDocumentTypeId) return toBool(r.get("IsActive"));
            }
        } catch (RuntimeException e) { return false; }
        return false;
    }

    /**
     * DateTimePicker.Value: the picked day with the time of day the picker kept (the form opened "now"),
     * so a yyyy-MM-dd from the page becomes that day at the current time. A value with a time is kept.
     */
    public static LocalDateTime picker(Object v) {
        String s = str(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() == 10 && s.matches("\\d{4}-\\d{2}-\\d{2}")) return LocalDate.parse(s).atTime(LocalTime.now().withNano(0));
        return toDate(s);
    }

    /** A picker value the page left empty: the picker always has a value on the desktop (now). */
    public static LocalDateTime orNow(LocalDateTime d) { return d == null ? LocalDateTime.now() : d; }

    /** Conversion.ToDateTime: 1900-01-01 for an empty / unparsable value. */
    public static LocalDateTime clrDate(Object v) {
        LocalDateTime d = toDate(v);
        return d == null ? LocalDateTime.of(1900, 1, 1, 0, 0) : d;
    }

    /** Conversion.ToDouble of a text with thousands separators. */
    public static double dbl(Object v) { return toDouble(v); }

    /** Math.Round(v, d) - MidpointRounding.ToEven. */
    public static double round(double v, int d) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        return new BigDecimal(v).setScale(d, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** double.ToString() as .NET prints it in a message (15 significant digits, no trailing ".0"). */
    public static String clr(double v) {
        if (Double.isNaN(v)) return "NaN";
        if (Double.isInfinite(v)) return v > 0 ? "Infinity" : "-Infinity";
        if (v == 0d) return "0";
        BigDecimal b = new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros();
        String s = b.toPlainString();
        return s;
    }

    public static Map<String, Object> m(Object... kv) { return HrmSupport.map(kv); }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        if (v instanceof List) {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
            return out;
        }
        return new ArrayList<>();
    }

    // ================================================================================ history filter

    /**
     * The desktop's history filter: @OrganizationId, @CompanyId, [@BranchesId], [@FinancialYearId], [@DocumentTypeId],
     * @CanViewAllRecord, @EntryUser when not view-all, then the date pair of the checked radio (doc / entry / modify /
     * approved date, each only when its picker is checked), then the doc-no range when non-zero. The caller adds the
     * screen's own trailing filters in the BLL's order. The parameter NAMES for the doc-date pair and the doc-no pair
     * differ per BLL and are passed in.
     */
    public Map<String, Object> history(UserAccount u, int screenId, Map<String, Object> b, Object[] head,
                                       String fromName, String toName, String docNoFromName, String docNoToName) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (int i = 0; i + 1 < head.length; i += 2) if (head[i + 1] != null) p.put(String.valueOf(head[i]), head[i + 1]);
        boolean all = viewAll(u, screenId);
        p.put("CanViewAllRecord", all);
        if (!all) p.put("EntryUser", uid(u));
        String mode = str(b.get("dateMode"));
        LocalDateTime from = toBool(b.get("fromChecked")) ? picker(b.get("from")) : null;
        LocalDateTime to = toBool(b.get("toChecked")) ? picker(b.get("to")) : null;
        String f, t;
        switch (mode) {
            case "entry": f = "EntryFromDate"; t = "EntryToDate"; break;
            case "modify": f = "ModifyFromDate"; t = "ModifyToDate"; break;
            case "approved": f = "ApprovedFromDate"; t = "ApprovedToDate"; break;
            default: f = fromName; t = toName; break;
        }
        if (from != null) p.put(f, ts(from));
        if (to != null) p.put(t, ts(to));
        int dn1 = toInt(b.get("fromDocNo")), dn2 = toInt(b.get("toDocNo"));
        if (dn1 != 0) p.put(docNoFromName, dn1);
        if (dn2 != 0) p.put(docNoToName, dn2);
        return p;
    }

    // ================================================================================ FIFO (DAL 0205 CommonServices)

    /** The fields FIFOImplementionForPartyProcessing reads from its ReportsParameters. */
    public static class FifoArgs {
        public int itemId, stockPartyId, warehouseId, stockUom, jobLotId, cropYearId, packingTypeId, documentTypeId, id, lineId;
        public LocalDateTime docDate;
        public double itemQty, netWeight;
        public String itemName;
    }

    /**
     * CommonServices.FIFOImplementionForPartyProcessing(obj, lstReserveFIFO): USP_GetStockByFifoMethodPartyProcessing
     * with the rows already reserved in this save as @FIFOXML (ArrayOfFIFOPartyProcessing), then the desktop's walk
     * over NetBalWeight / NetBalQty. Messages are the DAL's, with doubles printed as .NET prints them.
     */
    public List<InvTransFifo> fifo(UserAccount u, FifoArgs a, List<InvTransFifo> reserved) {
        String xml = null;
        if (!reserved.isEmpty()) {
            StringBuilder sb = new StringBuilder("<ArrayOfFIFOPartyProcessing>");
            for (InvTransFifo r : reserved) {
                sb.append("<FIFOPartyProcessing><Id>0</Id><DocDate>0001-01-01T00:00:00</DocDate><ItemId>0</ItemId><WarehouseId>0</WarehouseId>")
                  .append("<ItemUomId>0</ItemUomId><RateUomId>0</RateUomId>")
                  .append("<RefDocumentTypeId>").append(r.RefRefDocumentTypeId).append("</RefDocumentTypeId>")
                  .append("<RefDocIdNo>").append(r.RefRefDocIdNo).append("</RefDocIdNo>")
                  .append("<RefDocSubIdNo>").append(r.RefRefDocSubIdNo).append("</RefDocSubIdNo>")
                  .append("<QtyIn>0</QtyIn><QtyOut>0</QtyOut><BalQty>0</BalQty><WeightIn>0</WeightIn><WeightOut>0</WeightOut><BalWeight>0</BalWeight>")
                  .append("<ReserveWeight>").append(clr(r.StockWeightOut)).append("</ReserveWeight>")
                  .append("<ReserveQty>").append(clr(r.QtyOut)).append("</ReserveQty>")
                  .append("<NetBalQty>0</NetBalQty><NetBalWeight>0</NetBalWeight></FIFOPartyProcessing>");
            }
            xml = sb.append("</ArrayOfFIFOPartyProcessing>").toString();
        }
        List<Map<String, Object>> rows = repo.stockByFifo(u, a.itemId, a.stockPartyId, a.docDate, a.stockUom, a.warehouseId, a.jobLotId,
                a.packingTypeId, a.cropYearId, a.documentTypeId, a.id, xml);
        if (rows.isEmpty()) throw invalid("Stock Not Found this Item " + a.itemName + " against FIFO Method .....");
        double sumW = 0d;
        for (Map<String, Object> r : rows) sumW += dbl(r.get("NetBalWeight"));
        double itemQty = a.itemQty, netWeight = a.netWeight;
        if (!(a.netWeight <= round(sumW, 2))) {
            throw invalid("Weight available is " + clr(sumW) + " and row Weight is " + clr(a.netWeight) + " this item " + a.itemName + " against FIFO....");
        }
        List<InvTransFifo> out = new ArrayList<>();
        double num = netWeight, num2 = itemQty, num4 = 0d, num6 = 0d;
        for (Map<String, Object> r : rows) {
            InvTransFifo it = new InvTransFifo();
            double num5 = dbl(r.get("NetBalWeight"));
            double num3 = dbl(r.get("NetBalQty"));
            it.Id = toInt(r.get("Id"));
            it.LineId = a.lineId;
            it.ItemId = a.itemId;
            it.WarehouseId = a.warehouseId;
            it.JobLotId = a.jobLotId;
            it.InvPackingTypeId = a.packingTypeId;
            it.ItemUom = a.stockUom;
            it.CropYearId = a.cropYearId;
            it.RefRefDocumentTypeId = toInt(r.get("RefDocumentTypeId"));
            it.RefRefDocIdNo = toInt(r.get("RefDocIdNo"));
            it.RefRefDocSubIdNo = toInt(r.get("RefDocSubIdNo"));
            if (num5 <= num - num6) {
                num6 += num5;
                num4 += num3;
                it.QtyOut = num3;
                it.BillWeightOut = num5;
                it.StockWeightOut = num5;
                out.add(it);
            } else if (num5 >= num - num6) {
                it.QtyOut = num2 - num4;
                it.BillWeightOut = num - num6;
                it.StockWeightOut = num - num6;
                num4 += it.QtyOut;
                num6 += it.BillWeightOut;
                out.add(it);
            }
            if (netWeight == num6) break;
        }
        return out;
    }

    // ================================================================================ voucher write

    /**
     * The voucher block of DAL InvProductionProcessingBill.SetData / InvContractorWagesBillHeader.SetData:
     * GetVoucherHeadId (Sp_Vouchers_GetMethods) -> Sp_VoucherHead_Insert or _Update, every detail through
     * Sp_VoucherDetail_Insert, [USP_VoucherBalanceCheck], Sp_VoucherHead_H_Insert, every detail through
     * Sp_VoucherDetail_H_Insert with DocumentTypeIdRef. {@code setRefDocNoId}: the processing-bill DAL also resets
     * RefDocNoId to the document id; the wages DAL does not. Runs inside the caller's transaction.
     */
    public void writeVoucher(UserAccount u, VoucherHead v, int documentTypeId, int docId, boolean setRefDocNoId,
                             boolean balanceCheck, boolean requireDetails) {
        int existing = repo.voucherHeadId(u, documentTypeId, docId);
        if (existing > 0) v.Id = existing;
        int n;
        v.DocumentTypeSrNo = docId;
        if (setRefDocNoId) v.RefDocNoId = docId;
        if (existing == 0) n = repo.set("Sp_VoucherHead_Insert", v);
        else n = repo.set("Sp_VoucherHead_Update", v);
        if (n > 0) v.Id = n;
        for (VoucherDetail d : v.voucherDetailList) {
            d.VoucherHeadId = v.Id;
            repo.set("Sp_VoucherDetail_Insert", d);
        }
        if (requireDetails && v.voucherDetailList.isEmpty()) throw invalid("Voucher Detail list Not Found");
        if (balanceCheck) repo.exec("USP_VoucherBalanceCheck", PpCRepository.p("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", v.Id));
        int ref = repo.set("Sp_VoucherHead_H_Insert", v);
        for (VoucherDetail d : v.voucherDetailList) {
            d.VoucherHeadId = v.Id;
            d.DocumentTypeIdRef = ref;
            repo.set("Sp_VoucherDetail_H_Insert", d);
        }
    }

    /** Row of a list whose idKey equals id (LINQ Where(...).ToList()[0]); null when none. */
    public static Map<String, Object> find(List<Map<String, Object>> rows, String idKey, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get(idKey)) == id) return r;
        return null;
    }

    /** Projection helper: only the named columns of each row, in order (the form's dtTarget). */
    public static List<Map<String, Object>> project(List<Map<String, Object>> rows, String... cols) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String c : cols) {
                String[] kv = c.split("=", 2);
                o.put(kv[0], r.get(kv.length > 1 ? kv[1] : kv[0]));
            }
            out.add(o);
        }
        return out;
    }
}
