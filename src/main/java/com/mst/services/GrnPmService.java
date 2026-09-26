package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.GrnPmRequest;
import com.mst.repositories.GrnPmRepository;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.PurchaseOrderPmRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

import static com.mst.repositories.GrnPmRepository.*;
import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 500 "Goods Receipt Notes PM" - Packing Material module 54. Desktop form
 * {@code Architecture.WinApp.PackingMaterial_Store.GrnPackingMaterial}, DocumentTypeId 701.
 *
 * Save follows {@code Insert()} :534 in its own order: FormValidation (:452), "Grid Record Not
 * Found", the two transporter / freight refusals (:564-575), the per-row ValidateField calls
 * (:630-636, message format of FormHelper.ValidateField), the gross-weight match (:642-666), then
 * BLL {@code InvGrn.Save} (the detail-id guard on insert) and DAL {@code InvGrn.SetData}.
 *
 * On the desktop every header value except Doc Date, Transporter, Freight and Remarks sits in a
 * disabled control that only a gate-pass load, an order load or ReadById can fill. The server
 * therefore takes those values from the same three sources, not from the request.
 */
@Service
public class GrnPmService {

    public static final int SCREEN_ID = 500;
    private static final Set<String> PONCH = Set.of("Ponch", "Ponch & PartyWeight", "Ponch & FactoryWeight");

    private final GrnPmRepository repo;
    private final PurchaseOrderPmRepository po;
    private final InventoryOpeningRepository shared;
    private final DesktopAttachmentStore store;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public GrnPmService(GrnPmRepository repo, PurchaseOrderPmRepository po, InventoryOpeningRepository shared,
                        DesktopAttachmentStore store, JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.po = po; this.shared = shared; this.store = store; this.jdbc = jdbc;
        this.context = context; this.rights = rights;
    }

    private UserAccount user(String action) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int financialYear(UserAccount u) {
        var years = shared.years(u);
        if (years.isEmpty()) throw new IllegalArgumentException("No active financial year allocated to this company");
        return intOf(years.get(0).get("Id"));
    }

    // ================================================================== load

    /** InitializeComponentMethod :377 and its continuation :393-449. */
    public Map<String, Object> lookups() {
        UserAccount u = user("View");
        int fy = financialYear(u);
        boolean subsidiary = po.features(u).contains(4);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("suppliers", po.suppliers(u, false));                 // globalSupplierCustomer, unfiltered (:941)
        r.put("vehicleTypes", repo.vehicleTypes());
        r.put("deliveryTerms", repo.deliveryTerms());
        r.put("transporters", repo.transporters(u, subsidiary));
        r.put("cities", repo.cities(u));
        r.put("cropYears", repo.cropYears(u));
        r.put("itemConditions", repo.itemConditions());
        r.put("racks", repo.racks(u));
        r.put("historySuppliers", repo.historySuppliers(u));
        r.put("pendingGatePasses", repo.pendingGatePasses(u, fy, 0));

        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("subsidiaryAccounts", subsidiary);
        cfg.put("wagesStatus", bool(po.config(u, "WagesCompulsoryOnGrnPackingMaterial")));
        cfg.put("wagesActive", repo.wagesActive());
        cfg.put("wbNotCompulsory", bool(po.config(u, "WbNotCompulsoryForPmPurchaseOrder")));
        cfg.put("defaultDaysToLessFromHistoryFromDate", intOf(po.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        cfg.put("freightInwardAc", intOf(po.config(u, "FreightInwardAc")));
        cfg.put("defaultWarehouseId", intOf(po.config(u, "PackingMaterialDefaultWarehouse")));
        r.put("configuration", cfg);

        Map<String, Boolean> p = new LinkedHashMap<>();
        for (String a : List.of("Save", "Update", "Print", "Delete", "CanView AllRecord")) p.put(a, allowed(u, a));
        r.put("permissions", p);
        r.put("docNo", repo.nextDocNo(u, fy));
        // LoadPurchaseOrderPM FromDate = clsGlobalVariables.ActiveYr.Start_Period
        var year = shared.years(u).get(0);
        Object start = col(year, "Start_Period");
        r.put("financialYearStart", start == null ? "" : String.valueOf(start).substring(0, Math.min(10, String.valueOf(start).length())));
        return r;
    }

    /** reset() :1112-1113 - a fresh number and the pending gate passes. */
    public Map<String, Object> fresh(int recId) {
        UserAccount u = user("View");
        int fy = financialYear(u);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(u, fy));
        m.put("pendingGatePasses", repo.pendingGatePasses(u, fy, recId));
        return m;
    }

    /** BindGridByOrderId :2085 - one order, no zero-balance filter, the gate pass id passed through. */
    public List<Map<String, Object>> orderLines(int orderId, int gpId) {
        UserAccount u = user("View");
        if (orderId <= 0) return List.of();
        return repo.orderLoader(u, financialYear(u), 0, orderId, null, null, 0, gpId);
    }

    /** LoadPurchaseOrderPM - supplier list and PendingTradingPurchaseOrderLoad (ZeroBalanceType 1). */
    public Map<String, Object> orderLoader(int supplierId, String from, String to) {
        UserAccount u = user("View");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("suppliers", repo.orderSuppliers(u));
        m.put("rows", repo.orderLoader(u, financialYear(u), supplierId, 0,
                blank(from) ? null : date(from, "From Date"), blank(to) ? null : date(to, "To Date"), 1, 0));
        return m;
    }

    // ================================================================== history

    public List<Map<String, Object>> history(String dateMode, String from, String to, int fromDocNo, int toDocNo,
                                             int supplierId, String referred) {
        UserAccount u = user("View");
        HistoryFilter f = new HistoryFilter();
        f.canViewAll = allowed(u, "CanView AllRecord");
        f.financialYearId = financialYear(u);
        f.dateMode = dateMode == null ? "doc" : dateMode;
        f.from = blank(from) ? null : date(from, "From Date");
        f.to = blank(to) ? null : date(to, "To Date");
        f.fromDocNo = fromDocNo;
        f.toDocNo = toDocNo;
        f.supplierId = supplierId;
        f.actionId = "referred".equals(referred) ? 1 : "notReferred".equals(referred) ? 2 : 0;   // :1424-1431
        return repo.history(u, f);
    }

    /** ReadById :775 / BindHistoryDetail :1605. */
    public Map<String, Object> record(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = repo.header(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        out.put("isGeneralGp", intOf(col(h, "RefDocumentTypeId")) == 52);          // :830
        List<Map<String, Object>> lines = new ArrayList<>();
        for (var d : repo.details(id)) lines.add(line(d));                          // FillDetailFromListCommonForReadById :848
        out.put("lines", lines);
        out.put("attachments", repo.attachments(u, id));
        return out;
    }

    private static Map<String, Object> line(Map<String, Object> d) {
        Map<String, Object> l = new LinkedHashMap<>();
        l.put("Id", intOf(col(d, "Id")));
        l.put("OrderId", intOf(col(d, "PurchaseOrderId")));
        l.put("OrderDetailId", intOf(col(d, "PurchaseOrderDetailId")));
        l.put("OrderDate", col(d, "PurchaseOrderDate"));
        l.put("OrderNo", intOf(col(d, "PurchaserOrderNo")));
        l.put("ItemId", intOf(col(d, "ItemId")));
        l.put("ItemName", col(d, "Item"));
        l.put("CropYearId", intOf(col(d, "CropYearId")));
        l.put("CropYear", col(d, "CropYear"));
        l.put("PackingDate", col(d, "PackingDate"));
        l.put("ExpiryDate", col(d, "ExpiryDate"));
        l.put("PackUOMId", intOf(col(d, "ItemUomId")));
        l.put("PackUOM", col(d, "UOMCode"));
        l.put("ItemQty", dbl(col(d, "ItemQty")));
        l.put("QtyValidate", dbl(col(d, "ItemQty")));
        l.put("WtPerQty", dbl(col(d, "AdLsWeight")));                               // :871 - AdLsWeight holds Wt/Qty
        l.put("GrossWeight", dbl(col(d, "GrossWeight")));
        l.put("SupplierQty", dbl(col(d, "SupplierQty")));
        l.put("CityName", col(d, "AreaCity"));
        l.put("WarehouseId", intOf(col(d, "WarehouseId")));
        l.put("Warehouse", col(d, "WareHouseCode"));
        l.put("RemarksDetail", col(d, "CommentsDetail"));
        l.put("ItemConditionId", intOf(col(d, "ItemConditionId")));
        l.put("ItemCondition", col(d, "ConditionStatus"));
        l.put("RackId", intOf(col(d, "RackId")));
        l.put("RackName", col(d, "rackName"));
        return l;
    }

    // ================================================================== save

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(GrnPmRequest r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Record not update because Id not found");
        boolean insert = r.Id == 0;
        UserAccount u = user(insert ? "Save" : "Update");
        int fy = financialYear(u);
        boolean subsidiary = po.features(u).contains(4);
        boolean wbNotCompulsory = bool(po.config(u, "WbNotCompulsoryForPmPurchaseOrder"));
        List<GrnPmRequest.Line> lines = r.lines == null ? List.of() : r.lines;

        // -------------------------------------------------------- the disabled controls, from their real sources
        Map<String, Object> old = insert ? null : repo.header(u, r.Id);
        int docNo, gpId, gpNo, supplierId, billCalculateTypeId;
        String vehicleType, vehicleNo, biltyNo, deliveryTerm;
        double partyWeight, factoryWeight;
        boolean generalGp;
        Integer gpOrderId = null;
        Map<Integer, Map<String, Object>> orders = new HashMap<>();

        if (insert) {
            docNo = repo.nextDocNo(u, fy);
            Map<String, Object> gp = r.InwardGatePassId > 0 ? repo.pendingGatePass(u, fy, r.InwardGatePassId) : null;
            if (r.InwardGatePassId > 0 && gp == null) throw new IllegalArgumentException("Gate pass is no longer pending for GRN");
            if (gp != null && !"Accepted".equals(Objects.toString(col(gp, "Status"), ""))) {
                throw new IllegalArgumentException("Status Not Accepted Please check status");                    // :1296
            }
            gpId = gp == null ? 0 : intOf(col(gp, "Id"));
            gpNo = gp == null ? 0 : intOf(col(gp, "GpSrNo"));
            generalGp = gp != null && intOf(col(gp, "RefDocumentTypeId")) == 52;
            if (gp != null && !generalGp) gpOrderId = intOf(col(gp, "PurchaseOrderId"));
            vehicleType = gp == null ? "" : Objects.toString(col(gp, "VehicleType"), "");
            vehicleNo = gp == null ? "" : Objects.toString(col(gp, "VehicleNo"), "").trim();
            biltyNo = gp == null ? "" : Objects.toString(col(gp, "BiltyNo"), "").trim();
            // LoadGpData :1309-1310 writes FactoryWeight, then SupplierWeight - and txtsuppwt_TextChanged
            // (:1914) copies Supp Weight into Fact Weight. The desktop therefore saves the supplier weight
            // in both. Ported as written; see the project note.
            partyWeight = gp == null ? 0d : dbl(col(gp, "SupplierWeight"));
            factoryWeight = partyWeight;
            // supplier / delivery term / bill type come from the order rows the lines were loaded from
            // Supplier and Delivery Term are disabled combos that only a gate-pass load fills (:443-444),
            // so without a gate pass they stay empty and FormValidation refuses below.
            supplierId = 0; deliveryTerm = ""; billCalculateTypeId = 0;
            for (GrnPmRequest.Line l : gp == null ? List.<GrnPmRequest.Line>of() : lines) {
                if (l == null) continue;
                Map<String, Object> oh = order(u, orders, l.OrderId);
                if (oh == null) continue;
                int s = intOf(col(oh, "OrderSupCustId"));
                if (supplierId > 0 && s != supplierId) throw new IllegalArgumentException("Data against another Party Already Exist in Detail");   // :2550
                if (supplierId == 0) {
                    deliveryTerm = Objects.toString(col(oh, "DeliveryTerm"), "").trim();                    // Rows[0]["DeliveryTerm"]
                    billCalculateTypeId = intOf(col(oh, "BillCalculateTypeId"));                         // Rows[0]["BillCalculateTypeId"]
                }
                supplierId = s;
            }
        } else {
            docNo = intOf(col(old, "DocNo"));
            gpId = intOf(col(old, "InwardGatePassId"));
            gpNo = intOf(col(old, "GpNo"));
            generalGp = intOf(col(old, "RefDocumentTypeId")) == 52;
            supplierId = intOf(col(old, "SupplierCustomerId"));
            vehicleType = Objects.toString(col(old, "VehicleType"), "");
            vehicleNo = Objects.toString(col(old, "VehicleNo"), "").trim();
            biltyNo = Objects.toString(col(old, "BiltyNo"), "").trim();
            deliveryTerm = Objects.toString(col(old, "DeliveryTerm"), "").trim();
            partyWeight = dbl(col(old, "PartyWeight"));
            factoryWeight = dbl(col(old, "FactoryWeight"));
            billCalculateTypeId = intOf(col(old, "BillCalculateTypeId"));
            for (GrnPmRequest.Line l : lines) {
                if (l == null) continue;
                Map<String, Object> oh = order(u, orders, l.OrderId);
                if (oh != null && intOf(col(oh, "OrderSupCustId")) != supplierId) {
                    throw new IllegalArgumentException("Data against another Party Already Exist in Detail");
                }
            }
        }

        // -------------------------------------------------------- FormValidation :452
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        int supplier = supplierId;
        if (supplier == 0 || po.suppliers(u, false).stream().noneMatch(s -> intOf(s.get("Id")) == supplier)) {
            throw new IllegalArgumentException("Supplier Field is Required");
        }
        if (deliveryTerm.isEmpty() || "0".equals(deliveryTerm)) throw new IllegalArgumentException("DeliveryTerm Field is Required");
        // "Vehicle Type Field is Required" (:472) tests ActiveRow; reset() activates the blank default
        // row (:1095), so after the first reset the desktop never refuses. Neither does the port.
        if (vehicleNo.isEmpty() || "0".equals(vehicleNo)) throw new IllegalArgumentException("Vehicle No Field is Required");
        if (partyWeight == 0d && !wbNotCompulsory) throw new IllegalArgumentException("Supplier Weight Field is Required");
        if (factoryWeight == 0d && !generalGp && !wbNotCompulsory) throw new IllegalArgumentException("Factory Weight Field is Required");

        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");                        // :546

        // -------------------------------------------------------- transporter / freight :564-608
        double carriage = amount(r.CarriageAmount);
        Map<String, Object> transporter = null;
        if (!PONCH.contains(deliveryTerm)) {                        // CmbDeliveryTerm_TextChanged :1941 clears both
            if (r.TransporterValue > 0) {
                String key = subsidiary ? "SupplierCustomerId" : "Id";
                for (var t : repo.transporters(u, subsidiary)) if (intOf(t.get(key)) == r.TransporterValue) { transporter = t; break; }
            }
        } else {
            carriage = 0d;
        }
        if (carriage > 0 && transporter == null) throw new IllegalArgumentException("Transporter Account field required");
        if (transporter != null && carriage == 0d) throw new IllegalArgumentException("Freight field required");

        // -------------------------------------------------------- the rows :624-639
        Map<Integer, Map<String, Object>> conditions = new HashMap<>();
        for (var c : repo.itemConditions()) conditions.put(intOf(c.get("Id")), c);
        Map<Integer, String> crops = new HashMap<>();
        for (var c : repo.cropYears(u)) crops.put(intOf(c.get("Id")), Objects.toString(c.get("Description"), ""));
        List<Map<String, Object>> racks = repo.racks(u);
        Map<Integer, Map<String, Object>> savedLines = new HashMap<>();
        if (!insert) for (var d : repo.details(r.Id)) savedLines.put(intOf(col(d, "Id")), d);
        Map<Integer, List<Map<String, Object>>> orderDetails = new HashMap<>();

        List<Map<String, Object>> details = new ArrayList<>();
        Set<Integer> keptIds = new HashSet<>();
        double grossTotal = 0d;
        int rowIndex = 0;
        for (GrnPmRequest.Line l : lines) {
            if (l == null) throw new IllegalArgumentException("Grid Record Not Found");
            if (insert && l.Id > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");   // BLL :67
            Map<String, Object> saved = null;
            if (l.Id > 0) {
                saved = savedLines.get(l.Id);
                if (saved == null) throw new IllegalArgumentException("Detail row does not belong to this GRN");
                keptIds.add(l.Id);
            }
            Map<String, Object> oh = order(u, orders, l.OrderId);
            if (oh == null) throw new IllegalArgumentException("Item is required in Detail Grid at row No: " + (rowIndex + 1));
            if (gpOrderId != null && l.OrderId != gpOrderId) throw new IllegalArgumentException("Order does not belong to the loaded gate pass");
            Map<String, Object> od = null;
            for (var d : orderDetails.computeIfAbsent(l.OrderId, po::details)) if (intOf(col(d, "Id")) == l.OrderDetailId) { od = d; break; }
            if (od == null) throw new IllegalArgumentException("Item is required in Detail Grid at row No: " + (rowIndex + 1));

            int itemId = intOf(col(od, "OrderItemId"));
            int uomId = intOf(col(od, "OrderItemUOMId"));
            double wtPerQty = saved != null ? dbl(col(saved, "AdLsWeight")) : dbl(col(od, "WeightPerQty"));
            String city = l.CityName == null ? "" : l.CityName.trim();

            // FormHelper.ValidateField - same fields, same order, same message
            required(itemId != 0, "Item", rowIndex);
            required(uomId != 0, "Uom", rowIndex);
            required(l.ItemQty > 0, "Item Qty", rowIndex);
            required(l.ItemConditionId != 0, "ItemCondition", rowIndex);
            required(!city.isEmpty(), "City", rowIndex);
            required(l.WarehouseId != 0, "WareHouseName", rowIndex);
            required(l.RackId != 0, "Rack Name", rowIndex);

            if (!conditions.containsKey(l.ItemConditionId)) {
                throw new IllegalArgumentException("ItemCondition is required in Detail Grid at row No: " + (rowIndex + 1));
            }
            if (l.CropYearId != 0 && !crops.containsKey(l.CropYearId)) throw new IllegalArgumentException("Invalid Crop Year in row No: " + (rowIndex + 1));
            boolean sameAsSaved = saved != null && intOf(col(saved, "WarehouseId")) == l.WarehouseId && intOf(col(saved, "RackId")) == l.RackId;
            boolean fromList = racks.stream().anyMatch(k -> intOf(k.get("ItemId")) == itemId
                    && intOf(k.get("WarehouseId")) == l.WarehouseId && intOf(k.get("Id")) == l.RackId);
            if (!sameAsSaved && !fromList) throw new IllegalArgumentException("Select Warehouse and Rack from the list in row No: " + (rowIndex + 1));

            Map<String, Object> d = detailDefaults();
            d.put("Id", l.Id);
            d.put("PurchaseOrderId", l.OrderId);
            d.put("PurchaseOrderDetailId", l.OrderDetailId);
            d.put("PurchaserOrderNo", intOf(col(oh, "DocNo")));
            d.put("ItemId", itemId);
            d.put("CropYearId", l.CropYearId);
            d.put("CropYear", l.CropYearId == 0 ? "" : crops.get(l.CropYearId));
            d.put("PackingDate", optionalDate(l.PackingDate));
            d.put("ExpiryDate", optionalDate(l.ExpiryDate));
            d.put("ItemUomId", uomId);
            d.put("ItemQty", l.ItemQty);
            d.put("WarehouseId", l.WarehouseId);
            d.put("ItemConditionId", l.ItemConditionId);
            d.put("AreaCity", city);
            d.put("GrossWeight", l.GrossWeight);                    // :521 - Stock = Gross = NetBill
            d.put("StockWeight", l.GrossWeight);
            d.put("NetBillWeight", l.GrossWeight);
            d.put("AdLsWeight", wtPerQty);                          // :523 - Wt/Qty goes into AdLsWeight
            d.put("SupplierQty", l.SupplierQty == 0d ? l.ItemQty : l.SupplierQty);   // :525
            d.put("RackId", l.RackId);
            d.put("CommentsDetail", l.RemarksDetail == null ? "" : l.RemarksDetail);
            d.put("LineId", rowIndex + 1);                          // DAL :92
            details.add(d);
            grossTotal += l.GrossWeight;
            rowIndex++;
        }
        if (!insert) {
            for (Integer savedId : savedLines.keySet()) {
                if (!keptIds.contains(savedId)) throw new IllegalArgumentException("You Can't Delete Already Saved Row......");  // :2272
            }
        }

        // -------------------------------------------------------- gross weight match :642-666
        if (billCalculateTypeId == 1 && factoryWeight > 0d) {
            if (Set.of("Load", "Load & PartyWeight", "Ponch & PartyWeight").contains(deliveryTerm) && grossTotal != partyWeight) {
                throw new IllegalArgumentException("GrossWeight and Supplier Weight Not Match");
            }
            if (Set.of("Load & FactoryWeight", "Ponch & FactoryWeight").contains(deliveryTerm) && grossTotal != factoryWeight) {
                throw new IllegalArgumentException("GrossWeight and Factory Weight Not Match");
            }
            if ("Ponch".equals(deliveryTerm)) {
                if (partyWeight > factoryWeight) {
                    if (grossTotal != factoryWeight) throw new IllegalArgumentException("GrossWeight and Factory Weight Not Match");
                } else if (grossTotal != partyWeight) {
                    throw new IllegalArgumentException("GrossWeight and Supplier Weight Not Match");
                }
            }
        }

        // -------------------------------------------------------- header :576-619
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = headerDefaults();
        if (!insert) h.put("Id", r.Id);
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("DocDate", date(r.DocDate, "Doc Date"));
        h.put("DocNo", docNo);
        h.put("SupplierCustomerId", supplierId);
        h.put("PartyWeight", partyWeight);
        h.put("FactoryWeight", factoryWeight);
        h.put("VehicleType", vehicleType);
        h.put("VehicleNo", vehicleNo);
        h.put("InwardGatePassId", gpId);
        h.put("GpNo", gpNo);
        h.put("DeliveryTerm", deliveryTerm);
        h.put("BiltyNo", biltyNo);
        if (carriage > 0) {
            if (!subsidiary) {
                h.put("TransporterId", intOf(transporter.get("Id")));
                h.put("TransporterSupCustId", 0);
            } else {
                h.put("TransporterSupCustId", intOf(transporter.get("SupplierCustomerId")));    // SelectedRow.Cells[3]
                h.put("TransporterId", intOf(transporter.get("Id")));                           // SelectedRow.Cells[0]
            }
            h.put("CarriageAmount", carriage);
        }
        h.put("RemarksHeader", r.RemarksHeader == null ? "" : r.RemarksHeader.trim());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("FinancialYearId", fy);
        h.put("BranchesId", u.getBranchesId());
        h.put("ScreenName", SCREEN);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("EntryUser", u.getId());
        h.put("ModifyUser", u.getId());
        h.put("BillCalculateTypeId", billCalculateTypeId);
        h.put("ActionId", insert ? 1 : 2);                                                      // BLL :66 / :74

        // attachments are written to disk before the header so AttachmentsValues can carry them (DAL :72-76)
        List<Map<String, Object>> newFiles = storeFiles(u, r);
        List<Map<String, Object>> kept = new ArrayList<>();
        if (!insert) {
            Set<Integer> drop = new HashSet<>(r.removeAttachmentIds == null ? List.of() : r.removeAttachmentIds);
            for (var a : repo.attachments(u, r.Id)) if (!drop.contains(intOf(col(a, "Id")))) kept.add(a);
        }
        List<String> names = new ArrayList<>(), custom = new ArrayList<>();
        for (var a : kept) { names.add(Objects.toString(col(a, "Attachment"), "")); custom.add(Objects.toString(col(a, "UploadedFileCustomName"), "")); }
        for (var a : newFiles) { names.add((String) a.get("name")); custom.add((String) a.get("stored")); }
        if (!names.isEmpty()) {
            h.put("AttachmentsValues", String.join(",", names));
            h.put("CustomAttachmentsValues", String.join(",", custom));
        }

        // -------------------------------------------------------- DAL InvGrn.SetData
        int id;
        try {
            int num = repo.saveHeader(h, insert);
            id = num > 0 ? num : r.Id;                                                          // DAL :80-87
            if (id <= 0) throw new IllegalArgumentException("Record could not be saved");
            for (Map<String, Object> d : details) {
                d.put("InvGrnId", id);
                repo.saveDetail(d);
            }
            attachments(u, id, supplierId, r, newFiles);
            repo.inventoryTransactions(u, id);
            repo.stockInTransit(u, id);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }

        int savedNo = intOf(col(repo.header(u, id), "DocNo"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", savedNo);
        out.put("message", (insert ? "Record Save Successfully [" : "Record Update Successfully [") + savedNo + "]");
        out.put("grossWeight", grossTotal);
        // :682 - frmwagesBillHeader opens here on the desktop; it is not a web screen yet.
        out.put("wagesRequired", bool(po.config(u, "WagesCompulsoryOnGrnPackingMaterial")) && repo.wagesActive());
        return out;
    }

    /** btnDelete_Click :739. */
    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> delete(int id) {
        UserAccount u = user("Delete");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        repo.header(u, id);
        try {
            repo.delete(u, id);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        return Map.of("message", "Delete Record Successfully");
    }

    // ================================================================== files

    private List<Map<String, Object>> storeFiles(UserAccount u, GrnPmRequest r) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (r.files == null) return out;
        if (r.files.size() > 10) throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        for (var file : r.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(file);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("name", file.name);
            m.put("stored", store.store(u, file.name, bytes));
            m.put("size", bytes.length / 1048576d);
            out.add(m);
        }
        return out;
    }

    /** DAL :115-139 - RefAccountId = supplier, RefDocumentTypeId = 701, RefDocumentNo = the GRN. */
    private void attachments(UserAccount u, int id, int supplierId, GrnPmRequest r, List<Map<String, Object>> newFiles) {
        var existing = repo.attachments(u, id);
        for (Integer removed : new LinkedHashSet<>(r.removeAttachmentIds == null ? List.<Integer>of() : r.removeAttachmentIds)) {
            if (removed == null || existing.stream().noneMatch(a -> intOf(col(a, "Id")) == removed)) {
                throw new IllegalArgumentException("Attachment does not belong to this GRN");
            }
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?, @Activity=?", removed, "AttachmentDeleteById");
        }
        for (var f : newFiles) {
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Object[] v = {0, supplierId, 0, DOCUMENT_TYPE_ID, id, f.get("name"), now, u.getId(), now, u.getId(),
                    u.getOrganizationId(), u.getCompanyId(), 0, SCREEN, false, f.get("stored"), f.get("size"), 0};
            ProcExec.run(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?, @RefAccountId=?, @DMSFoldersLabelsId=?, @RefDocumentTypeId=?, @RefDocumentNo=?, @Attachment=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @ScreenName=?, @DetailWiseAttachment=?, @UploadedFileCustomName=?, @UploadedFileSizeMb=?, @LineId=?", v);
        }
    }

    public DesktopInventoryItemFileService.Download attachment(int id, int attachmentId) {
        UserAccount u = user("View");
        repo.header(u, id);
        var row = repo.attachments(u, id).stream().filter(a -> intOf(col(a, "Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(col(row, "UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(col(row, "Attachment"), "");
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(col(row, "Attachment"), stored)),
                store.read(u, basename(stored)), "application/octet-stream");
    }

    // ================================================================== helpers

    /** The order header behind a line - this company's, DocumentTypeId 700 - or null. */
    private Map<String, Object> order(UserAccount u, Map<Integer, Map<String, Object>> cache, int orderId) {
        if (orderId <= 0) return null;
        if (cache.containsKey(orderId)) return cache.get(orderId);
        Map<String, Object> h;
        try { h = po.header(u, orderId); } catch (org.springframework.web.server.ResponseStatusException e) { h = null; }
        cache.put(orderId, h);
        return h;
    }

    private static void required(boolean ok, String field, int rowIndex) {
        if (!ok) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** Architecture.Model.Inventory.InvGrn - non-virtual properties at their CLR defaults (strings / DateTime? null). */
    private static Map<String, Object> headerDefaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : List.of("IsApproved", "AddWages", "ScaleShortWeightApply", "SupplierShortWeightApply")) m.put(k, false);
        for (String k : List.of("CarriageAmount", "FreightDeduction", "BiltyFreight", "AdvanceByFactoryFreight", "AdvanceByPartyFreight",
                "AccessWeight", "FactoryWeight", "OtherCharges", "PartyWeight", "ScaleKart")) m.put(k, 0d);
        for (String k : List.of("BranchesId", "CompanyId", "FinancialYearId", "DocNo", "SupplierDispatchId", "DocumentTypeId",
                "BaseDocumentTypeId", "EntryUser", "GpNo", "Id", "InwardGatePassId", "ModifyUser", "OrganizationId", "PostUser",
                "ProjectsId", "ReferencePartyId", "StockPartyId", "SupplierCustomerId", "TransporterId", "TransporterSupCustId",
                "GhallaMandiId", "BillCalculateTypeId", "GrnTypeId", "ReqestedById", "ActionId")) m.put(k, 0);
        return m;
    }

    /** Architecture.Model.Inventory.InvGrnDetail - non-virtual properties at their CLR defaults. */
    private static Map<String, Object> detailDefaults() {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String k : List.of("AdLsWeight", "EBWPerUnit", "EBWTotal", "EbPurAgainstWeight", "GrossWeight", "SupplierQty", "ItemQty",
                "NetBillWeight", "StockWeight", "WtCut", "WtCutTotal", "ScaleKart", "ScaleShortWeight", "SupplierShortWeight",
                "FreightAmount", "StockEbUnit", "StockEbTotal", "QtyForWtCut")) m.put(k, 0d);
        for (String k : List.of("Id", "InvGrnId", "ItemId", "ItemUomId", "JobLotId", "PackingTypeId", "RefDocumentTypeId", "RefDocNoId",
                "RefDocSubIdNo", "PurchaseOrderId", "PurchaseOrderDetailId", "PurchaserOrderNo", "SupplySchedulId", "GatePassInwarDetailId",
                "WarehouseId", "WareHouseFromId", "InvPurchasedemondId", "PurchaseDemondDetailId", "CropYearId", "ContractorId", "CityId",
                "WbTicketId", "LineId", "WeightCutOnId", "LabId", "GdnId", "GdnDetailId", "GdnDocumentTypeId", "AssetId", "ConditionId",
                "DeliveryChallanId", "DeliveryChallanDetailId", "ItemConditionId", "RackId")) m.put(k, 0);
        return m;
    }

    private static boolean blank(String v) { return v == null || v.isBlank(); }

    private static Date date(String v, String name) {
        try { return Date.valueOf(LocalDate.parse(v.trim().substring(0, 10))); }
        catch (Exception e) { throw new IllegalArgumentException(name + " is not a valid date"); }
    }

    private static Timestamp optionalDate(String v) {
        if (blank(v)) return null;
        try { return Timestamp.valueOf(LocalDate.parse(v.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid packing / expiry date"); }
    }

    /** txtcarramount accepts digits only (:2698); Conversion.ToDouble. */
    private static double amount(String v) {
        if (blank(v)) return 0d;
        double d;
        try { d = Double.parseDouble(v.replace(",", "").trim()); } catch (NumberFormatException e) { throw new IllegalArgumentException("Freight field required"); }
        if (d < 0 || Double.isNaN(d) || Double.isInfinite(d)) throw new IllegalArgumentException("Freight field required");
        return d;
    }

    private static boolean bool(String v) {
        return v != null && Set.of("true", "1", "yes").contains(v.trim().toLowerCase(Locale.ROOT));
    }

    private static String basename(String name) {
        String n = name.replace('\\', '/');
        String r = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(r);
        return r;
    }
}
