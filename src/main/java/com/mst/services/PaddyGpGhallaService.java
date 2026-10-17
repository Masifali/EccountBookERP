package com.mst.services;

import com.mst.models.InwardGatePass;
import com.mst.models.InwardGatePassDetail;
import com.mst.repositories.InwardGatePassRecordRepository;
import com.mst.repositories.InwardGatePassRepository;
import com.mst.repositories.PaddyPurchaseRepository;
import com.mst.security.CurrentUserContext;
import java.text.SimpleDateFormat;
import java.util.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * Screen 468 "GPI For Ghalla Mandi" - Architecture.WinApp.Paddy_Purchase.frmGatePassInwardForGhallaMandi
 * (Paddy Purchase module 48, DocumentTypeId 51, RefDocumentTypeId 111, GatepassType "Paddy", OtherSupCust "Market Purchase").
 * Line references in the comments are to frmGatePassInwardForGhallaMandi.cs.
 */
@Service
public class PaddyGpGhallaService {
    public static final String SCREEN = "frmGatePassInwardForGhallaMandi";
    public static final int DOCUMENT_TYPE_ID = 51, REF_DOCUMENT_TYPE_ID = 111;

    @Autowired private PaddyPurchaseRepository paddy;
    @Autowired private InwardGatePassRepository gatePass;
    @Autowired private InwardGatePassRecordRepository rights;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopAttachmentStore attachmentStore;

    private static int num(Object v) { return v instanceof Number ? ((Number) v).intValue() : (v == null || v.toString().isBlank() ? 0 : (int) dbl(v)); }
    private static double dbl(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return v == null || v.toString().isBlank() ? 0 : Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String text(Object v) { return v == null ? "" : v.toString().trim(); }
    private static Object pick(Map<String, Object> row, String key) {
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    private static Map<String, Object> option(Object id, Object name) { Map<String, Object> m = new LinkedHashMap<>(); m.put("id", id); m.put("name", name); return m; }
    private static java.time.LocalDateTime stamp(Object v) {
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        String s = text(v);
        if (s.isEmpty()) return null;
        try { return java.time.LocalDateTime.parse(s.replace(' ', 'T')); } catch (Exception e) { return null; }
    }

    /** InwardGatePass_Load (:719): rights, then GhallaMandiBind, LoadingContractorBind, CarriageContractorBind, gpnofill, ItemNameFill, vehicleTypefill, cmbWeighBridgeFill, PackingTypeBind, grdfrmfill. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", rights.hasScreenRight(SCREEN, "Save"));
        out.put("canUpdate", rights.hasScreenRight(SCREEN, "Update"));
        out.put("canPrint", rights.hasScreenRight(SCREEN, "Print"));
        out.putAll(lookups());
        out.put("weighBridges", List.of(option(1, "Auto"), option(2, "Mannual")));
        out.put("packingTypes", List.of(option(1, "JUTE BAGS"), option(2, "PP BAGS"), option(5, "OPEN BULK")));
        out.put("gpNo", nextGpNo());
        return out;
    }

    private List<Map<String, Object>> groupOptions(String groupIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : paddy.suppliersByGroupIds(context.currentOrganizationId(), context.currentCompanyId(), groupIds, 0)) out.add(option(num(pick(r, "Id")), text(pick(r, "CompanyName"))));
        return out;
    }
    private List<Map<String, Object>> ghallaOptions() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : paddy.ghallaMandi(context.currentOrganizationId(), context.currentCompanyId())) out.add(option(num(pick(r, "Id")), text(pick(r, "GhallaMandi"))));
        return out;
    }

    /** GhallaMandiBind (:333), LoadingContractorBind (:349, group "6"), CarriageContractorBind (:365, group "17"), ItemNameFill (:381), vehicleTypefill (:316); also what btnRefresh_Click (:1169) refills. */
    public Map<String, Object> lookups() {
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("ghallaMandi", ghallaOptions());
        out.put("loadingContractors", groupOptions("6"));
        out.put("carriageContractors", groupOptions("17"));
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : paddy.readAllItems(org, company)) if (num(pick(r, "InventoryParentCategoriesId")) == 1) items.add(option(num(pick(r, "Id")), text(pick(r, "ItemName"))));
        out.put("items", items);
        List<Map<String, Object>> vehicleTypes = new ArrayList<>();
        for (Map<String, Object> r : paddy.vehicleTypes()) vehicleTypes.add(option(num(pick(r, "Id")), text(pick(r, "VehicleDescription"))));
        out.put("vehicleTypes", vehicleTypes);
        return out;
    }

    /** gpnofill (:281): CommonServices.GatePassInwardGenerateCode(51); the box is only written when the code is above 0. */
    public int nextGpNo() {
        Integer code = gatePass.generateGpCode(context.currentOrganizationId(), context.currentCompanyId(), context.currentBranchId(), context.currentFinancialYearId(), DOCUMENT_TYPE_ID);
        return code == null ? 0 : code;
    }

    /** grdfrmfill (:859): GatePassInward.GetOpenGatePassForGhallaMandiGpDetail (Org, Company, 51, ActiveYr, Ref 111). */
    public List<Map<String, Object>> openGatePasses() {
        return paddy.openGatePassForGpDetail(context.currentOrganizationId(), context.currentCompanyId(), DOCUMENT_TYPE_ID, context.currentFinancialYearId(), REF_DOCUMENT_TYPE_ID, 0, "GetOpenGatePassForGhallaMandiGpDetail");
    }

    /** gridhistory (:769): GatePassInward.GatepassHistoryForGhallaMandi; rows shaped as the form's dtHistory (50 rows on tab switch, all on LoadAll). */
    public List<Map<String, Object>> history(int noOfRecords) {
        boolean canViewAll = rights.hasScreenRight(SCREEN, "CanView AllRecord");
        java.time.format.DateTimeFormatter longTime = java.time.format.DateTimeFormatter.ofPattern("h:mm:ss a", Locale.US);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : paddy.gatePassHistoryGhalla(context.currentOrganizationId(), context.currentCompanyId(), context.currentFinancialYearId(), context.currentUserId(), DOCUMENT_TYPE_ID, canViewAll, noOfRecords, REF_DOCUMENT_TYPE_ID)) {
            Map<String, Object> m = new LinkedHashMap<>();
            java.time.LocalDateTime gp = stamp(pick(r, "GpDate")), in = stamp(pick(r, "InTime")), outT = stamp(pick(r, "OutTime"));
            m.put("Id", pick(r, "Id")); m.put("RecordNo", pick(r, "RecordNo"));
            m.put("GpDate", gp == null ? "" : gp.toLocalDate().toString());                      // ToShortDateString()
            m.put("GpNo", pick(r, "GpSrNo")); m.put("WorkingReportNo", pick(r, "WorkingReportNo")); m.put("GhallaMandi", pick(r, "GhallaMandi"));
            m.put("LoadingContractor", pick(r, "LoadingContractor")); m.put("VehicleType", pick(r, "VehicleType")); m.put("VehicleNo", pick(r, "VehicleNo"));
            m.put("BiltyNo", pick(r, "BiltyNo")); m.put("ItemQty", pick(r, "ItemQty")); m.put("Remarks", pick(r, "OtherRemarks"));
            m.put("InTime", in == null ? "" : in.format(longTime)); m.put("OutTime", outT == null ? "" : outT.format(longTime));    // ToLongTimeString()
            m.put("UserName", pick(r, "UserName")); m.put("CarriageContractor", pick(r, "CarriageContractro")); m.put("Freight", pick(r, "Freight"));
            m.put("Attachments", pick(r, "NoOfAttachments"));
            out.add(m);
        }
        return out;
    }

    /** FillHistoryDetail (:1536): GatePassInward.ReadByHeaderId; columns ItemName, PackingType (PackTypecode), PackSize (PackUnit), Qty (ItemQty), Weight. */
    public List<Map<String, Object>> historyDetail(int id) {
        requireRecord(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : paddy.gatePassReadByHeaderId(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemName", pick(r, "ItemName")); m.put("PackingType", pick(r, "PackTypecode")); m.put("PackSize", pick(r, "PackUnit"));
            m.put("Qty", dbl(pick(r, "ItemQty"))); m.put("Weight", dbl(pick(r, "Weight")));
            out.add(m);
        }
        return out;
    }

    private Map<String, Object> requireRecord(int id) {
        Map<String, Object> header = paddy.gatePassHeader(id);
        if (header == null || num(pick(header, "OrganizationId")) != context.currentOrganizationId() || num(pick(header, "CompanyId")) != context.currentCompanyId()
                || num(pick(header, "DocumentTypeId")) != DOCUMENT_TYPE_ID || num(pick(header, "RefDocumentTypeId")) != REF_DOCUMENT_TYPE_ID)
            throw new IllegalArgumentException("Record not found");
        return header;
    }

    /** getUpdate (:512): GatePassInward.GetByID(ID); the grid rows are ItemId, ItemName, ItemQty, PackingTypeId, PackingType, PackUnit, Weight. */
    public Map<String, Object> getById(int id) {
        Map<String, Object> header = requireRecord(id);
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> r : paddy.gatePassDetails(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", num(pick(r, "ItemId"))); m.put("ItemName", text(pick(r, "ItemName"))); m.put("ItemQty", dbl(pick(r, "ItemQty")));
            m.put("PackingTypeId", num(pick(r, "PackingTypeId"))); m.put("PackingType", text(pick(r, "PackingType")));
            m.put("PackUnit", dbl(pick(r, "PackUnit"))); m.put("Weight", dbl(pick(r, "Weight")));
            details.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", header);
        out.put("details", details);
        out.put("canUpdate", rights.hasScreenRight(SCREEN, "Update"));
        return out;
    }

    /** Attachments link of grdhistory (:1233): DMSAttachments.GetByID(Id, base.Name). */
    public List<Map<String, Object>> attachments(int id) {
        requireRecord(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : paddy.attachmentsByScreen(id, SCREEN)) {
            if (num(pick(row, "OrganizationId")) != context.currentOrganizationId() || num(pick(row, "CompanyId")) != context.currentCompanyId()) continue;
            Map<String, Object> item = new LinkedHashMap<>();
            item.put("Id", num(pick(row, "Id"))); item.put("Attachment", text(pick(row, "Attachment"))); item.put("UploadedFileCustomName", text(pick(row, "UploadedFileCustomName")));
            out.add(item);
        }
        return out;
    }

    public record AttachmentFile(String name, byte[] bytes) {}
    public AttachmentFile attachmentFile(int id, int attachmentId) {
        for (Map<String, Object> row : attachments(id)) {
            if (num(row.get("Id")) != attachmentId) continue;
            String original = baseName(text(row.get("Attachment"))), custom = text(row.get("UploadedFileCustomName"));
            String stored = custom.isBlank() ? original : baseName(custom);
            return new AttachmentFile(original.isBlank() ? stored : original, attachmentStore.read(context.requireAccountingUser(), stored));
        }
        throw new IllegalArgumentException("Attachment not found");
    }
    private static String baseName(String value) {
        String name = value == null ? "" : value.replace('\\', '/'); name = name.substring(name.lastIndexOf('/') + 1);
        if (!name.isEmpty()) DesktopAttachmentStore.validateName(name);
        return name;
    }

    private static Date parseDate(Object v) {
        String s = text(v);
        if (s.isEmpty()) return null;
        try { return new SimpleDateFormat("yyyy-MM-dd").parse(s.substring(0, 10)); }
        catch (Exception e) { throw new IllegalArgumentException("GP Date is not a valid date"); }
    }
    private static boolean listed(List<Map<String, Object>> options, int id) {
        for (Map<String, Object> o : options) if (num(o.get("id")) == id) return true;
        return false;
    }

    /**
     * btnsave_Click (:584) / btnupdate_Click (:707) -> Insert() (:597): formvalidation (:430) in the desktop order, the model, then GatePassInward.Save:
     * USP_DriverBiodata_InsertIfNotExists, Sp_GatePassInward_Insert / _Update, Sp_GatePassInwardDetail_Insert per grid row, one transaction.
     * The "Are you sure to Save/Update?" confirmation is asked by the page.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        int id = num(b.get("id"));
        boolean updating = id > 0;
        if (!rights.hasScreenRight(SCREEN, updating ? "Update" : "Save")) throw new org.springframework.security.access.AccessDeniedException("You do not have " + (updating ? "Update" : "Save") + " permission for GPI For Ghalla Mandi");
        Map<String, Object> previous = updating ? requireRecord(id) : null;

        // formvalidation()
        String gpNo = text(b.get("gpNo"));
        if (gpNo.isEmpty() || gpNo.equals("0")) throw new IllegalArgumentException("GatePassNo Field Required");
        int ghallaMandiId = num(b.get("ghallaMandiId")), loadingId = num(b.get("loadingContractorId")), carriageId = num(b.get("carriageContractorId"));
        if (ghallaMandiId <= 0 || !listed(ghallaOptions(), ghallaMandiId)) throw new IllegalArgumentException("Ghalla_Mandi Field Required");
        if (loadingId <= 0 || !listed(groupOptions("6"), loadingId)) throw new IllegalArgumentException("Loading Contractor Field Required");
        if (carriageId <= 0 || !listed(groupOptions("17"), carriageId)) throw new IllegalArgumentException("Carriage Contractor Field Required");
        if (text(b.get("vehicleNo")).isEmpty()) throw new IllegalArgumentException("VehicleNo Field Required");
        if (text(b.get("vehicleType")).isEmpty()) throw new IllegalArgumentException("VehicleType Field Required");
        String workingReport = text(b.get("workingReportNo"));
        if (workingReport.isEmpty() || workingReport.equals("0")) throw new IllegalArgumentException("Working Report No Field Required");
        if (text(b.get("weighBridge")).isEmpty()) throw new IllegalArgumentException("WeighBridge Status Field Required");

        int org = context.currentOrganizationId(), company = context.currentCompanyId(), year = context.currentFinancialYearId(), user = context.currentUserId();
        int gpSrNo = num(gpNo);
        double freight = dbl(b.get("freight"));

        InwardGatePass obj = new InwardGatePass();
        List<InwardGatePassDetail> details = new ArrayList<>();
        Object rowsObj = b.get("details");
        if (!(rowsObj instanceof Collection) || ((Collection<?>) rowsObj).isEmpty()) throw new IllegalArgumentException("Grid detail record not found");
        int qty = 0; double compareWeight = 0, packUnitSum = 0; int rowCount = 0;
        for (Object o : (Collection<?>) rowsObj) {
            @SuppressWarnings("unchecked") Map<String, Object> r = (Map<String, Object>) o;
            InwardGatePassDetail d = new InwardGatePassDetail();
            d.setItemId(num(r.get("ItemId"))); d.setPackingTypeId(num(r.get("PackingTypeId")));
            d.setItemQty(dbl(r.get("ItemQty"))); d.setPackUnit(dbl(r.get("PackUnit"))); d.setWeight(dbl(r.get("Weight")));
            // the desktop never touches these; the int members of the model stay 0
            d.setItemUOMId(0); d.setJobLotId(0); d.setSupplyScheduleId(0); d.setWareHouseId(0); d.setRefDocumentTypeId(0);
            d.setSupplierCustomerId(0); d.setPurchaseOrderId(0); d.setPurchaseOrderDetailId(0); d.setCityId(0);
            compareWeight += d.getWeight(); qty += (int) d.getItemQty().doubleValue(); packUnitSum += d.getPackUnit(); rowCount++;
            details.add(d);
        }
        double packUnit = rowCount == 0 ? 0 : packUnitSum / rowCount;   // GetTotal(PackUnit, (AggregateFunction)3) = average

        Map<String, Object> exact = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        exact.put("Id", updating ? id : 0);
        exact.put("GpDate", parseDate(b.get("gpDate")));
        exact.put("GpSrNo", gpSrNo);
        exact.put("OtherSupCust", "Market Purchase");
        exact.put("GatepassType", "Paddy");
        exact.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        exact.put("RefDocumentTypeId", REF_DOCUMENT_TYPE_ID);
        exact.put("BiltyNo", text(b.get("biltyNo")));
        exact.put("Freight", freight);
        exact.put("NetPaid", freight);
        exact.put("InDateTimeStamp", new Date());
        exact.put("OutDateTimeStamp", new Date());
        exact.put("OtherRemarks", text(b.get("remarks")));
        exact.put("VehicleNo", text(b.get("vehicleNo")));
        exact.put("VehicleType", text(b.get("vehicleType")));
        exact.put("LoadingContractorId", loadingId);
        exact.put("GhallaMandiId", ghallaMandiId);
        exact.put("CarriageContractorId", carriageId);
        exact.put("WorkingReportNo", num(workingReport));
        exact.put("Status", "Open");
        exact.put("GpQRCode", updating ? previous.get("GpQRCode") : null);
        exact.put("DriverName", text(b.get("driverName")));
        exact.put("DriverMobileNo", text(b.get("driverMobile")));
        exact.put("DocAttachment", text(b.get("weighBridge")));
        exact.put("PurchaseOrderId", gpSrNo);
        exact.put("OrganizationId", org); exact.put("CompanyId", company);
        exact.put("EntryUser", user); exact.put("FinancialYearId", year);
        exact.put("WeightComparedToPoWt", compareWeight);
        exact.put("SupplierWeight", compareWeight);
        exact.put("NoOfPackages", qty);
        exact.put("PackUnit", packUnit);
        exact.put("driverBioDataId", 0);

        obj.setId(updating ? id : 0);
        obj.setOrganizationId(org); obj.setCompanyId(company); obj.setFinancialYearId(year);
        obj.setDocumentTypeId(DOCUMENT_TYPE_ID); obj.setEntryUser(user);
        obj.setDriverName(text(b.get("driverName"))); obj.setDriverMobileNo(text(b.get("driverMobile")));
        try {
            Integer bioId = gatePass.saveDriverBio(obj);          // driverBioDataId == 0 -> USP_DriverBiodata_InsertIfNotExists
            exact.put("driverBioDataId", bioId == null ? 0 : bioId);
            Integer headerId = gatePass.saveHeader(obj, exact);
            for (InwardGatePassDetail d : details) { d.setGatePassInwardId(headerId); gatePass.saveDetail(d); }
            Map<String, Object> res = new LinkedHashMap<>();
            res.put("success", true); res.put("id", headerId);
            res.put("message", (updating ? "Record Update Successfully[" : "Record Save Successfully[") + gpSrNo + "]");
            return res;
        } catch (org.springframework.dao.DataAccessException error) {
            Throwable cause = error.getMostSpecificCause();
            if (cause instanceof java.sql.SQLException && ((java.sql.SQLException) cause).getErrorCode() >= 50000)
                throw new IllegalStateException(cause.getMessage(), error);
            throw error;
        }
    }
}
