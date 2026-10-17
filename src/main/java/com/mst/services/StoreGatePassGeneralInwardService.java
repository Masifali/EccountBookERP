package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.services.sale.engr.SaleEngrAttachments;
import com.mst.services.sale.engr.SaleEngrSupport;
import com.mst.services.sale.engr.SaleEngrSupport.Warning;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Store Purchase, ModuleId 67 - screen 342 "Inward Gate Pass General",
 * {@code Architecture.WinApp.Inventory_Definition.GatePassGeneral} (form Name "GatePassGeneral", DocumentTypeId 52,
 * table GatePassGeneral + GatePassGeneralDetail). Page /store/inward-gate-pass-general.
 *
 * Not shared with an already-ported form: 134 InwardGatePass (DocumentTypeId 51) and 343 GatePassGeneralOutward (92) are different
 * classes with different grids and rules. The BLL/DAL class Architecture.BLL.Inventory.GatePassGeneral and its procedures are the same
 * as the ones the 343 port uses (Sp_GatePassGeneral_Insert / _Update), plus the detail list (USP_GatePassGeneralDetail_Insert).
 *
 * Desktop map (GatePassGeneral.cs, line numbers of the cleaned copy): InwardGatePass_Load :190, PartiesAndItemdtFillFromLookUp :263,
 * PartiesAndItemdtFillByRefdoc :317, DocumentNoFill :435, vehicleTypefill :464, GatePassType :480, WarehouseFillFromGlobal :548,
 * JobFillFromGlobal :563, CmbGatePassType_Leave :603, RefDocNodtFillDbCall :645, formvalidation :832, FormValidationDetail :872,
 * ReadById :1245, Insert :1365, HistoryComboDbCall :1536, grdfrmfill :1581, GetDetailGrdByHeadId :2052, Reset :2086, btnPrint_Click :2217.
 */
@Service
public class StoreGatePassGeneralInwardService {
    public static final String SCREEN = "GatePassGeneral";
    public static final int DOC_TYPE = 52;
    private static final String PROC = "Sp_GatePassGeneral_GetAllMethod";

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;

    public StoreGatePassGeneralInwardService(SaleEngrSupport sup, SaleEngrAttachments attachments) {
        this.sup = sup;
        this.attachments = attachments;
    }

    // ------------------------------------------------------------------ Load (InwardGatePass_Load)

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                                       // DocumentNoFill
        m.put("warehouses", warehouses());                                               // WarehouseFillFromGlobal
        m.put("jobLots", jobLots());                                                     // JobFillFromGlobal
        m.put("vehicleTypes", sup.rows("Sp_VehicleType_GetAllMethod"));                  // vehicleTypefill: VehicleType.GetAll
        m.put("wbRows", sup.rows("SP_WbPartyandItemsDefine_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Activity", "ReadAll"));                                                 // PartiesAndItemdtFillFromLookUp: WbPartyandItemsDefine.GetAll
        m.put("historyParties", historyParties());                                       // HistoryComboBind(HistoryComboDbCall())
        return m;
    }

    /** DocumentNoFill: GatePassGeneral.GenerateCode; 0 when there is no row (the text is then left unchanged on the desktop). */
    public int nextNo() {
        UserAccount u = sup.user();
        return sup.firstInt(sup.rows(PROC, "Activity", "GenerateCode", "DocumentTypeId", DOC_TYPE, "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "FinancialYearId", idOrNull(sup.fy()), "BranchesId", idOrNull(u.getBranchesId())), "GpSrNo");
    }

    public List<Map<String, Object>> warehouses() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_InvWareHouse_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Activity", "GetActiveWareHouse")) {
            Object id = ci(r, "Id");
            Object name = ci(r, "WareHouseName");
            out.add(row("Id", id, "Description", name));                                 // dtware: Id / Description
        }
        return out;
    }

    public List<Map<String, Object>> jobLots() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_JobLot_ReadMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll")) {
            out.add(row("Id", ci(r, "Id"), "Description", ci(r, "JobLotDescription")));  // dtJobLot: Id / Description
        }
        return out;
    }

    /** CommonBindings.ItemUomFromGlobalBind(itemId, cmbUOM): the UOM schedule of the item (Id, UOMCode, Equivalent). */
    public List<Map<String, Object>> uoms(int itemId) {
        if (itemId <= 0) return new ArrayList<>();
        return sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID");
    }

    // ------------------------------------------------------------------ reference documents (CmbGatePassType_Leave -> RefDocNodtFillDbCall)

    /** type 6 Delivery_Challan (148), 7 Purchase_Order (42), 8 Purchase_Demand (42 is what the desktop passes; the procedure ignores it). */
    public List<Map<String, Object>> refDocs(int gatePassTypeId, int recId) {
        UserAccount u = sup.user();
        String proc;
        int docType;
        switch (gatePassTypeId) {
            case 6: proc = "USP_DeliveryChallanHeader_GetNoForGp"; docType = 148; break;
            case 7: proc = "USP_PurchaseOrder_GetNoForGeneralInward"; docType = 42; break;
            case 8: proc = "USP_InvPurchaseDemand_GetNoForGeneralInward"; docType = 42; break;
            default: return new ArrayList<>();
        }
        return sup.rows(proc, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", u.getBranchesId(),
                "FinancialYearId", sup.fy(), "DocumentTypeId", docType, "GpRecId", recId);
    }

    // ------------------------------------------------------------------ history / open list (grdfrmfill)

    public List<Map<String, Object>> historyParties() {
        UserAccount u = sup.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("USP_GetDataForDropDownFromGatePassGeneral", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId())) {
            if ("Supplier".equals(str(ci(r, "Activity")))) out.add(row("Id", ci(r, "Id"), "Name", ci(r, "ReferenceName")));
        }
        return out;
    }

    public static class HistoryFilter {
        public String status;           // "Open" for the list on the Form tab, null for the History tab
        public String dateMode;         // doc | entry | modify | approved
        public boolean useFrom, useTo;
        public String fromDate, toDate;
        public int fromDocNo, toDocNo, partyId;
    }

    public List<Map<String, Object>> history(HistoryFilter f) {
        UserAccount u = sup.user();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Activity", "ReadAll");
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("BranchesId", u.getBranchesId());
        p.put("FinancialYearId", sup.fy());
        String status = f.status == null ? "" : f.status.trim();
        if (!status.isEmpty()) p.put("GpStatus", status);
        if (status.isEmpty()) {                                                          // dates are only used when no status is asked for
            String mode = f.dateMode == null ? "doc" : f.dateMode;
            String fk = "FromDate", tk = "ToDate";
            if ("entry".equals(mode)) { fk = "EntryFromDate"; tk = "EntryToDate"; }
            else if ("modify".equals(mode)) { fk = "ModifyFromDate"; tk = "ModifyToDate"; }
            else if ("approved".equals(mode)) { fk = "ApprovedFromDate"; tk = "ApprovedToDate"; }
            LocalDate from = date(f.fromDate), to = date(f.toDate);
            if (f.useFrom && from != null) p.put(fk, from);
            if (f.useTo && to != null) p.put(tk, to);
        }
        p.put("FromDocNo", f.fromDocNo);
        p.put("ToDocNo", f.toDocNo);
        p.put("SupplierCustomerId", f.partyId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : com.mst.repositories.support.DesktopProc.rows(sup.jdbc(), PROC, p)) {
            out.add(row("Id", ci(r, "Id"), "GpType", ci(r, "gatePassType"), "GpDate", ci(r, "GpDate"), "GpNo", ci(r, "GpSrNo"),
                    "DeliveryChallanNo", ci(r, "DeliveryChallanNo"), "ManualNo", ci(r, "ManualNo"), "VehicleType", ci(r, "VehicleType"),
                    "VehicleNo", ci(r, "VehicleNo"), "PartyName", ci(r, "PartyName"), "ItemName", ci(r, "VarietyName"), "ItemQty", ci(r, "ItemQty"),
                    "Warehouse", ci(r, "WarehouseName"), "GpStatus", ci(r, "DocAttachment"), "WBStatus", ci(r, "Status"),
                    "FactoryWeight", ci(r, "FactoryWeight"), "PartyWeight", ci(r, "SupplierWeight"), "EntryUser", ci(r, "EntryUser"),
                    "EntryDate", ci(r, "EntryDate"), "ModifyUser", ci(r, "ModifyUser"), "ModifyDate", ci(r, "ModifyDate"),
                    "ApprovedUser", ci(r, "ApprovedUser"), "ApprovedDate", ci(r, "PostDate"), "NoOfAttachments", ci(r, "NoOfAttachments"),
                    "Remarks", ci(r, "OtherRemarks")));
        }
        return out;
    }

    // ------------------------------------------------------------------ ReadById (GatePassGeneral.GetByID) and the history detail grid

    private Map<String, Object> load(int id) {
        List<Map<String, Object>> rows = sup.rows(PROC, "Id", id, "Activity", "ReadById");
        if (rows.isEmpty() || toInt(ci(rows.get(0), "OrganizationId")) != sup.org() || toInt(ci(rows.get(0), "CompanyId")) != sup.company()
                || toInt(ci(rows.get(0), "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Gate pass not found in this company");
        return rows.get(0);
    }

    public List<Map<String, Object>> details(int id) {
        load(id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows(PROC, "Id", id, "Activity", "ReadByHeaderId_GatePassGeneralDetail")) {
            out.add(row("Id", ci(r, "Id"), "RefDocumentTypeId", ci(r, "RefDocumentTypeId"), "RefDocId", ci(r, "RefDocId"),
                    "RefDocDetailId", ci(r, "RefDocDetailId"), "RefDocNo", ci(r, "RefDocNo"), "WareHouseId", ci(r, "WareHouseId"),
                    "WareHouseName", ci(r, "WareHouseName"), "JobLotId", ci(r, "JobLotId"), "JobLot", ci(r, "JobLotDescription"),
                    "ItemId", ci(r, "ItemId"), "ItemCode", ci(r, "ItemCode"), "ItemName", ci(r, "ItemName"),
                    "PackUomId", ci(r, "ItemUomId"), "PackUom", ci(r, "ItemUomCode"), "PackUomEquivalent", ci(r, "ItemUomEquivalent"),
                    "ItemQty", ci(r, "ItemQty"), "Remarks", ci(r, "RemarksDetail")));
        }
        return out;
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> h = load(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        out.put("details", details(id));
        out.put("attachments", attachments.list(SCREEN, id));
        // ReadById: WbTransactions.GetNetWeightFromWbTransactions (ReferenceDocTypeId 52, ReferenceDocNoId = Id) -> txtFactoryWeight
        List<Map<String, Object>> wb = sup.rows("Sp_WbTransation_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "Id", id, "RefDocumentTypeId", DOC_TYPE, "Activity", "GetNetWeightFromWbTransactions");
        double net = 0;
        for (Map<String, Object> r : wb) net += toDouble(ci(r, "NetWbWeight"));
        out.put("wbFound", !wb.isEmpty());
        out.put("wbNetWeight", net);
        return out;
    }

    // ------------------------------------------------------------------ Save (formvalidation + Insert)

    public static class Line {
        public int id, refDocumentTypeId, refDocId, refDocDetailId, refDocNo, itemId, wareHouseId, jobLotId, packUomId;
        public String itemCode, itemName, wareHouseName, jobLotName, packUom, remarks;
        public double packUomEquivalent, itemQty;
    }

    public static class Request {
        public int id;
        public String gpDate, gpNo, manualNo, vehicleNo, remarks, partyName, factoryWeight, supplierWeight, entryItemText, entryQty;
        public int gatePassTypeId, vehicleTypeId, statusId, weighBridgeId, refDocId, entryWarehouseId;
        public String gatePassTypeText, vehicleTypeText, statusText, weighBridgeText;
        public List<Line> lines = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    /** Conversion.ToInt on a double/decimal cell: truncating, 0 when it is not a number. */
    private static int toIntTrunc(double d) { return (int) d; }

    @Transactional
    public Map<String, Object> save(Request r) {
        UserAccount u = sup.user();
        // formvalidation() :832 - same order, same texts
        if (text(r.gatePassTypeText).isEmpty() || r.gatePassTypeId == 0) throw new Warning("Please Select CmbGatePassType");
        if (text(r.gpNo).isEmpty() || "0".equals(text(r.gpNo))) throw new Warning("Gp No Filed Required");
        if (text(r.vehicleNo).isEmpty()) throw new Warning("Please Select Vehicle No");
        if (text(r.vehicleTypeText).isEmpty() || r.vehicleTypeId == 0) throw new Warning("Please Select Vehicle Type");
        if (text(r.weighBridgeText).isEmpty() || r.weighBridgeId == 0) throw new Warning("Please Select WeighBridge");
        if (text(r.statusText).isEmpty() || r.statusId == 0) throw new Warning("Please Select Status");
        // Insert() :1379
        int gpTypeId = r.gatePassTypeId;
        if ((gpTypeId == 6 || gpTypeId == 7 || gpTypeId == 8) && r.refDocId == 0) {
            throw new Warning(gpTypeId == 6 ? "Please Select Delivery Challan No" : gpTypeId == 7 ? "Please Select Order No" : "Please Select Demand No");
        }
        int gpSrNo;
        try { gpSrNo = Integer.parseInt(text(r.gpNo)); } catch (RuntimeException e) { throw new Warning("Input string was not in a correct format."); }

        double factory = 0;
        String fw = text(r.factoryWeight);
        if (!fw.isEmpty() && !"0".equals(fw)) { try { factory = Double.parseDouble(fw); } catch (RuntimeException e) { factory = 0; } }
        double supplierWeight = 0;
        try { supplierWeight = Double.parseDouble(text(r.supplierWeight)); } catch (RuntimeException e) { supplierWeight = 0; }

        List<Line> lines = r.lines == null ? new ArrayList<>() : r.lines;
        String varietyName = lines.isEmpty() ? "" : text(lines.get(0).itemName);          // GetFirstOrDefaultValueOfColumn(grd, "ItemName")
        varietyName = varietyName.isEmpty() ? text(r.entryItemText) : varietyName;
        double qtySum = 0;
        for (Line l : lines) qtySum += l.itemQty;
        int noOfPackages = toIntTrunc(qtySum);                                            // GetColumnSum(grd, "ItemQty") -> ToInt
        if (noOfPackages == 0) { try { noOfPackages = toIntTrunc(Double.parseDouble(text(r.entryQty))); } catch (RuntimeException e) { noOfPackages = 0; } }
        int firstWarehouse = lines.isEmpty() ? 0 : lines.get(0).wareHouseId;
        int warehouseId = firstWarehouse == 0 ? r.entryWarehouseId : firstWarehouse;

        // detail rows (validated before anything is written, like the desktop loop; only rows with ToInt(ItemQty) > 0 are saved)
        List<Line> save = new ArrayList<>();
        for (int i = 0; i < lines.size(); i++) {
            Line d = lines.get(i);
            if (toIntTrunc(d.itemQty) > 0) {
                required(d.itemId > 0, "Item", i);
                required(d.wareHouseId > 0, "Warehouse", i);
                required(d.jobLotId > 0, "JobLot", i);
                required(d.itemQty > 0, "Qty", i);
                save.add(d);
            }
        }

        Timestamp now = now();
        Timestamp today = Timestamp.valueOf(LocalDate.now().atStartOfDay());
        Map<String, Object> p = new LinkedHashMap<>();                                    // GenericProvider.SetProc: every non-null model property
        p.put("Id", r.id > 0 ? r.id : 0);
        p.put("DocumentTypeId", DOC_TYPE);
        p.put("GpSrNo", gpSrNo);
        p.put("GpDate", date(r.gpDate));
        p.put("GatepassType", text(r.gatePassTypeText));
        p.put("GpTypeSrNo", 0);
        p.put("SupplierCustomerId", 0);
        p.put("OtherSupCust", text(r.partyName));
        p.put("VehicleType", text(r.vehicleTypeText));
        p.put("VehicleNo", text(r.vehicleNo));
        p.put("NoOfPackages", noOfPackages);
        p.put("OtherRemarks", text(r.remarks));
        p.put("RefDocumentTypeId", DOC_TYPE);
        p.put("RefDocumentEntryNo", 0);
        p.put("InDateTimeStamp", now);
        p.put("OutDateTimeStamp", now);
        p.put("DocAttachment", text(r.statusText));
        p.put("IsApproved", r.id > 0);                                                    // BLL Save: IsApproved = true on update
        p.put("EntryDate", today);
        p.put("EntryUser", u.getId());
        p.put("ModifyDate", today);
        p.put("ModifyUser", u.getId());
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("PostUser", 0);
        p.put("PostState", false);
        p.put("CityId", 0);
        p.put("Freight", 0.0);
        p.put("Status", text(r.weighBridgeText));
        p.put("NetPaid", 0.0);
        p.put("SupplierWeight", supplierWeight);
        p.put("FactoryWeight", factory);
        p.put("DifferenceWeight", 0.0);
        p.put("WeighBridgeId", 0);
        p.put("WarehouseId", warehouseId);
        p.put("VarietyName", varietyName);
        p.put("PurchaseOrderId", gpTypeId == 7 ? r.refDocId : 0);
        p.put("FinancialYearId", sup.fy());
        p.put("ManualNo", text(r.manualNo));
        p.put("BranchesId", u.getBranchesId());
        p.put("DeliveryChallanHeaderId", gpTypeId == 6 ? r.refDocId : 0);
        p.put("PurchaseDemandId", gpTypeId == 8 ? r.refDocId : 0);
        String proc = r.id > 0 ? "Sp_GatePassGeneral_Update" : "Sp_GatePassGeneral_Insert";
        int num = sup.setProcMap(proc, p);
        int id = num > 0 ? num : r.id;                                                    // DAL SetData: if (num > 0) obj.Id = num
        if (id <= 0) throw new IllegalStateException("The gate pass could not be saved.");

        for (Line d : save) {                                                             // Sp_GatePassGeneral_Update has already deleted the old detail rows
            Map<String, Object> dp = new LinkedHashMap<>();
            dp.put("Id", d.id);
            dp.put("GatePassGeneralId", id);
            dp.put("RefDocumentTypeId", d.refDocumentTypeId);
            dp.put("RefDocId", d.refDocId);
            dp.put("RefDocDetailId", d.refDocDetailId);
            dp.put("ItemId", d.itemId);
            dp.put("WareHouseId", d.wareHouseId);
            dp.put("PackingTypeId", 0);
            dp.put("CropYearId", 0);
            dp.put("JobLotId", d.jobLotId);
            dp.put("ItemUomId", d.packUomId);
            dp.put("ItemQty", BigDecimal.valueOf(d.itemQty));
            dp.put("Weight", BigDecimal.valueOf(d.itemQty));                              // pd.Weight = qty
            dp.put("RemarksDetail", text(d.remarks));
            dp.put("ItemName", text(d.itemName));
            dp.put("ItemCode", text(d.itemCode));
            sup.setProcMap("USP_GatePassGeneralDetail_Insert", dp);
        }
        attachments.apply(SCREEN, DOC_TYPE, id, 0, r.attachments);                        // FormHelper.UpdateAttachmentsForObject + DeleteAllAttachmentsFromDb
        return row("id", id, "gpSrNo", gpSrNo, "updated", r.id > 0,
                "message", (r.id > 0 ? "Record Update Successfully[" : "Record Save Successfully[") + gpSrNo + "]");
    }

    /** FormHelper.ValidateField: "{field} is required in Detail Grid at row No: {n}". */
    private static void required(boolean ok, String field, int rowIndex) {
        if (!ok) throw new Warning(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    public List<Map<String, Object>> attachmentList(int id) { load(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { load(id); return attachments.download(SCREEN, id, attachmentId); }

    private static LocalDate date(String s) {
        if (s == null || s.isBlank()) return null;
        try { return LocalDate.parse(s.trim().substring(0, Math.min(10, s.trim().length()))); } catch (RuntimeException e) { return null; }
    }
}
