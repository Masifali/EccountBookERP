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
 * Screen 464 "GPI FOR GATE PURCHASE" - Architecture.WinApp.Paddy_Purchase.frmGatePassInwardForGate
 * (Paddy Purchase module 48, DocumentTypeId 51, RefDocumentTypeId 110, GatepassType "Paddy", OtherSupCust "Gate Purchase").
 * Line references in the comments are to frmGatePassInwardForGate.cs.
 */
@Service
public class PaddyGpGateService {
    public static final String SCREEN = "frmGatePassInwardForGate";
    public static final int DOCUMENT_TYPE_ID = 51, REF_DOCUMENT_TYPE_ID = 110;

    @Autowired private PaddyPurchaseRepository paddy;
    @Autowired private InwardGatePassRepository gatePass;
    @Autowired private InwardGatePassRecordRepository rights;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopAttachmentStore attachmentStore;

    private static int num(Object v) { return v instanceof Number ? ((Number) v).intValue() : (v == null || v.toString().isBlank() ? 0 : (int) Double.parseDouble(v.toString().trim())); }
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

    /** InwardGatePass_Load (:769): rights, then SupplierCustomerFill, gpnofill, ItemNameFill, vehicleTypefill, CityFill, grdfrmfill, cmbWeighBridgeFill, PackingTypeBind. */
    public Map<String, Object> init() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", rights.hasScreenRight(SCREEN, "Save"));
        out.put("canUpdate", rights.hasScreenRight(SCREEN, "Update"));
        out.put("canPrint", rights.hasScreenRight(SCREEN, "Print"));
        out.putAll(lookups());
        out.put("gpNo", nextGpNo());
        return out;
    }

    /** SupplierCustomerFill (:380), ItemNameFill (:398), vehicleTypefill (:338), CityFill (:355), cmbWeighBridgeFill (:429), PackingTypeBind (:319); also what btnRefresh_Click (:1187) refills. */
    public Map<String, Object> lookups() {
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> supplierInfo = new ArrayList<>();
        for (Map<String, Object> r : paddy.supplierCustomerAll(org, company)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", num(pick(r, "Id"))); m.put("cnic", text(pick(r, "CNIC"))); m.put("mobile", text(pick(r, "MobilePersonal")));
            supplierInfo.add(m);
        }
        out.put("supplierInfo", supplierInfo);
        List<Map<String, Object>> suppliers = new ArrayList<>();
        for (Map<String, Object> r : paddy.suppliersByGroupIds(org, company, "1,2,3", 0)) suppliers.add(option(num(pick(r, "Id")), text(pick(r, "CompanyName"))));
        out.put("suppliers", suppliers);
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : paddy.readAllItems(org, company)) if (num(pick(r, "InventoryParentCategoriesId")) == 1) items.add(option(num(pick(r, "Id")), text(pick(r, "ItemName"))));
        out.put("items", items);
        List<Map<String, Object>> vehicleTypes = new ArrayList<>();
        for (Map<String, Object> r : paddy.vehicleTypes()) vehicleTypes.add(option(num(pick(r, "Id")), text(pick(r, "VehicleDescription"))));
        out.put("vehicleTypes", vehicleTypes);
        List<Map<String, Object>> cities = new ArrayList<>();
        for (Map<String, Object> r : paddy.cities(org, company)) cities.add(option(num(pick(r, "Id")), text(pick(r, "CityName"))));
        out.put("cities", cities);
        out.put("weighBridges", List.of(option(1, "Auto"), option(2, "Mannual")));
        out.put("packingTypes", List.of(option(1, "JUTE BAGS"), option(2, "PP BAGS"), option(5, "OPEN BULK")));
        return out;
    }

    /** gpnofill (:303): CommonServices.GatePassInwardGenerateCode(51); the box is only written when the code is above 0. */
    public int nextGpNo() {
        Integer code = gatePass.generateGpCode(context.currentOrganizationId(), context.currentCompanyId(), context.currentBranchId(), context.currentFinancialYearId(), DOCUMENT_TYPE_ID);
        return code == null ? 0 : code;
    }

    /** cmbcity_Leave (:1509): City.GetDistrictAndTehsilByCityId; the first row of each combo is activated. */
    public Map<String, Object> districtTehsil(int cityId) {
        List<Map<String, Object>> rows = paddy.districtAndTehsil(context.currentOrganizationId(), context.currentCompanyId(), cityId);
        List<Map<String, Object>> districts = new ArrayList<>(), tehsils = new ArrayList<>();
        Set<Object> seenDistrict = new HashSet<>(), seenTehsil = new HashSet<>();
        for (Map<String, Object> r : rows) {
            if (seenDistrict.add(pick(r, "DistrictId"))) districts.add(option(num(pick(r, "DistrictId")), text(pick(r, "District"))));
            if (seenTehsil.add(pick(r, "TehsilId"))) tehsils.add(option(num(pick(r, "TehsilId")), text(pick(r, "Tehsil"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("districts", rows.isEmpty() ? List.of() : districts);
        out.put("tehsils", rows.isEmpty() ? List.of() : tehsils);
        return out;
    }

    /** grdfrmfill (:872): GatePassInward.GetOpenGatePassForGpDetail(Org, Company, 51, ActiveYr, 110). */
    public List<Map<String, Object>> openGatePasses() {
        return paddy.openGatePassForGpDetail(context.currentOrganizationId(), context.currentCompanyId(), DOCUMENT_TYPE_ID, context.currentFinancialYearId(), REF_DOCUMENT_TYPE_ID, 0, "GetOpenGatePassForGpDetail");
    }

    /** gridhistory (:818): tabControl1_SelectedIndexChanged loads 50 rows, btnLoadAll_Click all of them. */
    public List<Map<String, Object>> history(int noOfRecords) {
        boolean canViewAll = rights.hasScreenRight(SCREEN, "CanView AllRecord");
        return paddy.gatePassHistoryByRefType(context.currentOrganizationId(), context.currentCompanyId(), context.currentFinancialYearId(), context.currentUserId(), DOCUMENT_TYPE_ID, canViewAll, noOfRecords, REF_DOCUMENT_TYPE_ID);
    }

    /** FillHistoryDetail (:1393): GatePassInward.ReadByHeaderId; columns ItemName, PackingType (PackTypecode), PackSize (PackUnit), Qty (ItemQty), Weight. */
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

    /** getUpdate (:553): GatePassInward.GetByID(ID); the grid rows are ItemId, ItemName, ItemQty, PackingTypeId, PackingType, PackUnit, Weight. */
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
        out.put("districtTehsil", districtTehsil(num(pick(header, "CityId"))));
        out.put("canUpdate", rights.hasScreenRight(SCREEN, "Update"));
        return out;
    }

    /** NoOfAttachments links of grdhistory (:1262): DMSAttachments.GetByID(Id, base.Name). */
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
        try { return new SimpleDateFormat(s.length() > 10 ? "yyyy-MM-dd'T'HH:mm" : "yyyy-MM-dd").parse(s.length() > 16 ? s.substring(0, 16) : s); }
        catch (Exception e) { throw new IllegalArgumentException("GP Date is not a valid date"); }
    }

    /**
     * btnsave_Click (:629) / btnupdate_Click (:757) -> Insert() (:642): formvalidation (:447) in the desktop order, the model, then GatePassInward.Save:
     * USP_DriverBiodata_InsertIfNotExists (driverBioDataId is 0 on this form), Sp_GatePassInward_Insert / _Update, Sp_GatePassInwardDetail_Insert per grid row,
     * all in one transaction. The "Are you sure to Save/Update?" confirmation is asked by the page.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        int id = num(b.get("id"));
        boolean updating = id > 0;
        if (!rights.hasScreenRight(SCREEN, updating ? "Update" : "Save")) throw new org.springframework.security.access.AccessDeniedException("You do not have " + (updating ? "Update" : "Save") + " permission for GPI FOR GATE PURCHASE");
        Map<String, Object> previous = updating ? requireRecord(id) : null;

        // formvalidation()
        if (text(b.get("vehicleNo")).isEmpty()) throw new IllegalArgumentException("VehicleNo Field Required");
        if (text(b.get("vehicleType")).isEmpty()) throw new IllegalArgumentException("VehicleType Field Required");
        String gpNo = text(b.get("gpNo"));
        if (gpNo.isEmpty() || gpNo.equals("0")) throw new IllegalArgumentException("GatePassNo Field Required");
        if (text(b.get("weighBridge")).isEmpty()) throw new IllegalArgumentException("WeighBridge Status Field Required");
        if (num(b.get("cityId")) <= 0) throw new IllegalArgumentException("City Field Required");
        if (num(b.get("districtId")) <= 0) throw new IllegalArgumentException("District Field Required");
        if (num(b.get("tehsilId")) <= 0) throw new IllegalArgumentException("Tehsil Field Required");
        String mobile = text(b.get("supplierMobileNo"));
        if (mobile.isEmpty() || mobile.equals("0")) throw new IllegalArgumentException("SupplierMobileNo Field Required");
        String driverName = text(b.get("driverName"));
        if (driverName.isEmpty() || driverName.equals("0")) throw new IllegalArgumentException("DriverName Field Required");
        String driverCnic = text(b.get("driverCnic"));
        if (driverCnic.isEmpty() || driverCnic.equals("0")) throw new IllegalArgumentException("DriverCNIC Field Required");
        String driverMobile = text(b.get("driverMobile"));
        if (driverMobile.isEmpty() || driverMobile.equals("0")) throw new IllegalArgumentException("DriverMobile Field Required");
        if (num(b.get("supplierId")) <= 0) throw new IllegalArgumentException("Supplier Name Field Required");

        int org = context.currentOrganizationId(), company = context.currentCompanyId(), branch = context.currentBranchId(),
                year = context.currentFinancialYearId(), user = context.currentUserId();
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
        exact.put("OtherSupCust", "Gate Purchase");
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
        exact.put("SupplierCustomerId", num(b.get("supplierId")));
        exact.put("Status", "Open");
        exact.put("GpQRCode", updating ? previous.get("GpQRCode") : null);
        exact.put("CityId", num(b.get("cityId")));
        exact.put("DestrictId", num(b.get("districtId")));
        exact.put("TehsilId", num(b.get("tehsilId")));
        exact.put("SupplierCNICNO", text(b.get("supplierCnic")));
        exact.put("SupplierMobileNo", mobile);
        exact.put("TransporterName", text(b.get("transporter")));
        exact.put("DriverName", driverName);
        exact.put("DriverCNICNO", driverCnic);
        exact.put("DriverMobileNo", driverMobile);
        exact.put("SupplierWeight", dbl(b.get("supplierWeight")));
        exact.put("DocAttachment", text(b.get("weighBridge")));
        exact.put("PurchaseOrderId", gpSrNo);
        exact.put("OrganizationId", org); exact.put("BranchesId", branch); exact.put("CompanyId", company);
        exact.put("EntryUser", user); exact.put("FinancialYearId", year);
        exact.put("WeightComparedToPoWt", compareWeight);
        exact.put("NoOfPackages", qty);
        exact.put("PackUnit", packUnit);
        exact.put("driverBioDataId", 0);

        obj.setId(updating ? id : 0);
        obj.setOrganizationId(org); obj.setCompanyId(company); obj.setBranchesId(branch); obj.setFinancialYearId(year);
        obj.setDocumentTypeId(DOCUMENT_TYPE_ID); obj.setEntryUser(user);
        obj.setDriverName(driverName); obj.setDriverCNICNO(driverCnic); obj.setDriverMobileNo(driverMobile);
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
