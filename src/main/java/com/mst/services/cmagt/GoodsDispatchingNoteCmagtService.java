package com.mst.services.cmagt;

import com.mst.models.cmagt.dto.GoodsDispatchingNoteCmagtDto;
import com.mst.repositories.cmagt.GoodsDispatchingNoteCmagtRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Goods Dispatching Note (1055) - frmGoodsDispatchingNoteCmagt.Insert() :1812-2070 and the BLL
 * 0487 calls behind it. The form's validations run here, in the form's order and with the form's
 * messages, because the page cannot be trusted to have run them.
 */
@Service
public class GoodsDispatchingNoteCmagtService {

    public static final int DOCUMENT_TYPE_ID = 1055;                       // :460
    public static final String DESKTOP_SCREEN_NAME = "frmGoodsDispatchingNoteCmagt";   // :463
    private static final String RIGHT_CAN_VIEW_ALL_RECORDS = "CanView AllRecord";

    @Autowired
    private GoodsDispatchingNoteCmagtRepository repository;

    @Autowired
    private CurrentUserContext currentUserContext;

    /* Same rights read the Purchase Order / Sale Order screens use for @CanViewAllRecord. */
    @Autowired
    private com.mst.repositories.cmagt.SaleOrderCmagtRepository rightsRepo;

    /**
     * btnsave_Click (:2072) forces RecId = 0 and inserts; btnUpdate_Click (:2085) refuses when
     * RecId == 0. isUpdate says which button was pressed.
     *
     * Organization, company, branch, financial year and the user come from the SESSION, never
     * from the request body (:1885-1893).
     */
    public Map<String, Object> saveOrUpdate(GoodsDispatchingNoteCmagtDto dto, boolean isUpdate) {
        Map<String, Object> result = new HashMap<>();
        try {
            int recId = isUpdate ? ni(dto.getGdnBuyerDispatchMasterId()) : 0;
            if (isUpdate && recId == 0) {
                throw new IllegalArgumentException("Record not update because Id not found");   // :2091
            }
            dto.setGdnBuyerDispatchMasterId(recId);
            dto.setOrganizationId(currentUserContext.currentOrganizationId());
            dto.setCompanyId(currentUserContext.currentCompanyId());
            dto.setBranchId(currentUserContext.currentBranchId());
            dto.setFinancialYearId(currentUserContext.currentFinancialYearId());
            dto.setEntryUserId(currentUserContext.currentUserId());
            dto.setModifyUserId(currentUserContext.currentUserId());
            dto.setDocumentTypeId(DOCUMENT_TYPE_ID);
            /* btnsave.Enabled = DoHaveSaveRight, btnUpdate.Enabled = DoHaveUpdateRights (:552-553). */
            Map<String, Boolean> fr = formRights();
            if (!isUpdate && !Boolean.TRUE.equals(fr.get("save")))
                throw new IllegalArgumentException("you don't have save rights...");
            if (isUpdate && !Boolean.TRUE.equals(fr.get("update")))
                throw new IllegalArgumentException("you don't have update rights...");
            if (recId > 0) requireOwnRecord(recId);
            if (recId == 0) {
                /* The desktop shows GenerateCode in txtDocNo on New (:677); the procedure
                   re-numbers on insert anyway, so this only satisfies the Doc No check. */
                dto.setDocNo(repository.generateCode(dto.getOrganizationId(), dto.getCompanyId(),
                        dto.getBranchId(), dto.getFinancialYearId(), DOCUMENT_TYPE_ID));
            }

            validateAndPrepare(dto, recId);

            int id = repository.save(dto, recId == 0);
            result.put("status", "SUCCESS");
            result.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");   // :2052/:2057
            result.put("id", id);
        } catch (Exception e) {
            result.put("status", "ERROR");
            result.put("message", rootMessage(e));
        }
        return result;
    }

    /** Insert() :1824-2046, in the form's order. */
    private void validateAndPrepare(GoodsDispatchingNoteCmagtDto dto, int recId) {
        List<GoodsDispatchingNoteCmagtDto.DetailDto> rows = dto.getGdnBuyerDispatchDetailList();
        if (rows == null || rows.isEmpty()) {
            throw new IllegalArgumentException("Detail Record Not Found");                      // :1824-1827
        }

        /* :1828-1850 - GDN date against the loaded GRN's date (datGrnDate). */
        String warning = dto.getWarningRemarks() == null ? "" : dto.getWarningRemarks();
        Date gdnDate = day(dto.getDocDate());
        String grnText = dto.getGrnDate();
        if (isBlank(grnText)) {
            for (GoodsDispatchingNoteCmagtDto.DetailDto d : rows) {
                if (!isBlank(d.getGrnDate())) { grnText = d.getGrnDate(); break; }
            }
        }
        if (gdnDate != null && !isBlank(grnText)) {
            Date grnDate = day(grnText);
            if (grnDate != null) {
                if (gdnDate.before(grnDate)) {
                    throw new IllegalArgumentException("Invalid Entry!\n\nGDN Date cannot be less than GRN Date.");
                }
                Date grnPlusOne = new Date(grnDate.getTime() + 24L * 3600 * 1000);
                if (gdnDate.equals(grnDate) && warning.isEmpty()) {
                    throw new IllegalArgumentException("GRN Date and GDN Date are the same.\n\nPlease provide remarks to proceed.");
                }
                if (gdnDate.after(grnPlusOne) && warning.isEmpty()) {
                    throw new IllegalArgumentException("GDN Date is more than 1 day after GRN Date.\n\nPlease provide remarks to proceed.");
                }
            }
        }

        /* :1870-1877 - freight without a transporter. */
        BigDecimal totalFreight = nd(dto.getTotalFreight());
        if (totalFreight.signum() > 0 && ni(dto.getTransporterId()) == 0 && isBlank(dto.getTransporterName())) {
            throw new IllegalArgumentException("Please Select Transporter or Fill Transporter Name");
        }

        /* :1854-1869 FormHelper.ValidateControls - combos: "<name> field is required";
           numeric text boxes: "<name> must be a non-zero number". */
        reqInt(dto.getDocNo(), "Doc No", true);
        reqInt(dto.getCommissionAgentId(), "Commission Agent", false);
        reqInt(dto.getBuyerId(), "Buyer Name", false);
        /* CmbGrnNoDetail "Grn No" (:1860) - set only by Load GRN (:3496) / ReadById (:2172). */
        boolean hasGrn = false;
        for (GoodsDispatchingNoteCmagtDto.DetailDto d : rows) if (ni(d.getGrnSupplierLoadingMasterId()) > 0) { hasGrn = true; break; }
        if (!hasGrn) throw new IllegalArgumentException("Grn No field is required");
        reqInt(dto.getLoadingCityId(), "Loading City", false);
        reqInt(dto.getUnloadingCityId(), "Un-Loading City", false);
        reqInt(dto.getVehicleTypeId(), "Vehicle Type", false);
        reqStr(dto.getVehicleNo(), "Vehicle No");
        reqStr(dto.getBiltyNo(), "Bilty No");
        reqInt(dto.getDeliveryTermId(), "Delivery Term", false);
        if (nd(dto.getBiltyQty()).signum() == 0) throw new IllegalArgumentException("Vehicle Qty must be a non-zero number");
        if (nd(dto.getScaleNetWeight()).signum() == 0) throw new IllegalArgumentException("Net Wb Weight must be a non-zero number");

        /* :1906-1913 - transporter and freight are stored only when Total Freight > 0;
           otherwise the model's defaults (0 / null) go to the procedure. */
        if (totalFreight.signum() <= 0) {
            dto.setTransporterId(0);
            dto.setTransporterName(null);
            dto.setBiltyFreight(BigDecimal.ZERO);
            dto.setOtherAdLesCharges(BigDecimal.ZERO);
            dto.setTotalFreight(BigDecimal.ZERO);
        }
        if (dto.getVehicleNo() != null) dto.setVehicleNo(dto.getVehicleNo().trim());      // :1915
        if (dto.getBiltyNo() != null) dto.setBiltyNo(dto.getBiltyNo().trim());            // :1917

        /* :1938-2001 - detail rows. */
        BigDecimal gross = BigDecimal.ZERO, net = BigDecimal.ZERO;
        boolean blockLate = configBool("BlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal");     // :607
        boolean warnLate  = configBool("ShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal");    // :608
        for (int i = 0; i < rows.size(); i++) {
            GoodsDispatchingNoteCmagtDto.DetailDto d = rows.get(i);
            int detailId = recId != 0 ? ni(d.getGdnBuyerDispatchDetailId()) : 0;           // :1942
            d.setGdnBuyerDispatchDetailId(detailId);
            d.setActionTypeId(detailId <= 0 ? 1 : 2);                                      // :1943
            reqField(d.getInventoryParentCategoryId(), "Parent Category", i);
            reqField(d.getItemId(), "Item Name", i);
            reqField(d.getCropYearId(), "Crop Year", i);
            reqField(d.getPackingTypeId(), "Packing Type", i);
            reqField(d.getPackUomId(), "Pack Uom", i);
            reqField(d.getLoadingQty(), "Loading Qty", i);
            reqField(d.getWbGrossWeight(), "Gross Weight", i);
            reqField(d.getNetBillWeight(), "Net Bill Weight", i);
            /* :1954-1990 late vehicle against the PO validity date. The page asks for the
               remarks (frmRemarks); this re-applies the rule so it cannot be skipped. */
            if (ni(d.getPurchaseOrderMasterId()) > 0 && gdnDate != null) {
                Date poExpiry = day(d.getPoExpiryDate());
                if (poExpiry != null) {
                    if (gdnDate.equals(poExpiry)) {
                        d.setWarningRemarks("");
                    } else if (gdnDate.after(poExpiry)) {
                        if (blockLate) {
                            throw new IllegalArgumentException("Vehicle has reached late compared to PO Expiry Date.\n\nEntry is not allowed.");
                        }
                        if (warnLate && isBlank(d.getWarningRemarks())) {
                            throw new IllegalArgumentException("Remarks are required for late vehicle entry.");
                        }
                    }
                }
            }
            d.setSortNo(ni(d.getSortNo()));
            gross = gross.add(nd(d.getWbGrossWeight()));
            net = net.add(nd(d.getNetBillWeight()));
        }

        /* :2002-2016 - an empty-bag row is kept only with both an item and a packing type. */
        List<GoodsDispatchingNoteCmagtDto.EmptyBagDto> ebs = new ArrayList<>();
        if (dto.getGdnBuyerDispatchEmptyBagDetailList() != null) {
            for (GoodsDispatchingNoteCmagtDto.EmptyBagDto e : dto.getGdnBuyerDispatchEmptyBagDetailList()) {
                if (ni(e.getEmptyBagPackingMaterialItemId()) > 0 && ni(e.getPackingTypeId()) > 0) {
                    e.setGdnBuyerDispatchEmptyBagDetailId(0);
                    e.setWeightCutKg(BigDecimal.ZERO);       // the form never sets it
                    e.setSortNo(0);
                    e.setRemarks(null);
                    ebs.add(e);
                }
            }
        }
        dto.setGdnBuyerDispatchEmptyBagDetailList(ebs);

        /* :2017-2036 - an expense row is kept only with an item and Amount > 0; blank remarks
           become "Expense : <item>  Qty<qty>  @<rate>". */
        List<GoodsDispatchingNoteCmagtDto.ExpenseDto> exs = new ArrayList<>();
        if (dto.getGdnBuyerDispatchExpenseDetailList() != null) {
            for (GoodsDispatchingNoteCmagtDto.ExpenseDto x : dto.getGdnBuyerDispatchExpenseDetailList()) {
                if (ni(x.getItemId()) != 0 && x.getAmount() != null && x.getAmount() > 0.0) {
                    x.setGdnBuyerDispatchExpenseDetailId(0);
                    x.setSortNo(0);
                    String r = x.getRemarks() == null ? "" : x.getRemarks().trim();
                    if (r.isEmpty() || "0".equals(r)) {
                        r = "Expense : " + (x.getOtherItemName() == null ? "" : x.getOtherItemName().trim())
                          + "  Qty" + nd(x.getQty()).toPlainString()
                          + "  @" + (x.getRate() == null ? "0" : new BigDecimal(x.getRate().toString()).stripTrailingZeros().toPlainString());
                    }
                    x.setRemarks(r);
                    exs.add(x);
                }
            }
        }
        dto.setGdnBuyerDispatchExpenseDetailList(exs);

        /* :2037-2044 - header weights must reconcile with the rows. */
        if (nd(dto.getScaleNetWeight()).compareTo(gross) != 0) {
            throw new IllegalArgumentException("Header Net Weight:" + fmt(dto.getScaleNetWeight())
                    + " not equal to detail gross weight:" + fmt(gross) + ". please Check!");
        }
        if (nd(dto.getBillWeight()).compareTo(net) != 0) {
            throw new IllegalArgumentException("Header Bill Weight:" + fmt(dto.getBillWeight())
                    + " not equal to detail NetBillWeight:" + fmt(net) + ". please Check!");
        }
    }

    /**
     * Load GRN - frmPendingGrnLoadingChallanLoader.PendingDataDbCall (:290-321); with recId it is
     * also the form's OutstandingOrdersdtFillDbCall used by ReadById (:2173-2177). Tenancy from
     * the session (UserAccount / clsGlobalVariables.ActiveYr), never from the request.
     */
    public Map<String, Object> pendingGrn(String fromDate, String toDate, int fromDocNo, int toDocNo,
                                          int recId, int commissionAgentId, int supplierId, int buyerId,
                                          int itemId, int deliverToPartyId, String shipToAddress) {
        Map<String, Object> result = new HashMap<>();
        try {
            if (recId > 0) requireOwnRecord(recId);
            Map<String, Object> data = repository.pendingGrnForGdn(
                    currentUserContext.currentOrganizationId(), currentUserContext.currentCompanyId(),
                    currentUserContext.currentBranchId(), currentUserContext.currentFinancialYearId(),
                    fromDate, toDate, fromDocNo, toDocNo, recId, commissionAgentId, supplierId,
                    buyerId, itemId, deliverToPartyId, shipToAddress);
            result.put("status", "SUCCESS");
            result.putAll(data);
        } catch (Exception e) {
            result.put("status", "ERROR");
            result.put("message", rootMessage(e));
        }
        return result;
    }

    /** Loader filter combos, split as CombosFill (:186-275): distinct Id per activity, distinct text for ship-to. */
    public Map<String, Object> pendingGrnLoaderCombos() {
        String[][] map = {
                {"CommissionAgent", "commissionAgents"}, {"SupplierName", "suppliers"},
                {"BuyerName", "buyers"}, {"Item", "items"},
                {"DeliveryToParty", "deliverToParties"}, {"DeliverToAddress", "shipToAddresses"}};
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Set<String>> seen = new HashMap<>();
        for (String[] m : map) { out.put(m[1], new ArrayList<Map<String, Object>>()); seen.put(m[0], new HashSet<>()); }
        List<Map<String, Object>> rows;
        try {
            rows = repository.pendingGrnLoaderCombos(currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());
        } catch (Exception e) {
            rows = new ArrayList<>();
        }
        for (Map<String, Object> r : rows) {
            Object a = pick(r, "Activity");
            String activity = a == null ? "" : a.toString();
            for (String[] m : map) {
                if (!m[0].equals(activity)) continue;
                int id = toInt(pick(r, "Id"));
                Object n = pick(r, "ReferenceName");
                String name = n == null ? "" : n.toString();
                String key = "DeliverToAddress".equals(activity) ? name : String.valueOf(id);
                if (!seen.get(activity).add(key)) break;
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("id", id);
                o.put("name", name);
                @SuppressWarnings("unchecked")
                List<Map<String, Object>> list = (List<Map<String, Object>>) out.get(m[1]);
                list.add(o);
                break;
            }
        }
        return out;
    }

    /** :607-608 - the two late-vehicle configurations, for the page's remarks prompt. */
    public Map<String, Object> lateVehicleConfig() {
        Map<String, Object> r = new HashMap<>();
        r.put("blockEntry", configBool("BlockEntryForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal"));
        r.put("showWarning", configBool("ShowWarningForLateVehicleArrivalAfterPoExpiryCommissionAgentPortal"));
        return r;
    }

    /** GetCommissionAgentConfigurationsFromGlobalandBind :705-735 (Conversion.ToInt of each config). */
    public Map<String, Object> portalDefaults() {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("commissionAgentId", configInt("DefaultCommissionAgentIdForCommissionAgentPortal"));
        r.put("deliveryTermId", configInt("DefaultDeliveryTermIdForCommissionAgentPortal"));
        r.put("loadingCityId", configInt("DefaultLoadingCityIdForCommissionAgentPortal"));
        r.put("unloadingCityId", configInt("DefaultUnloadingCityIdForCommissionAgentPortal"));
        r.put("defaultDaysToLessFromHistoryFromDate", configInt("DefaultDaysToLessFromHistoryFromDate"));
        return r;
    }

    private int configInt(String name) {
        String v = repository.configValue(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), name);
        if (v == null) return 0;
        try { return (int) Double.parseDouble(v.trim()); } catch (Exception e) { return 0; }
    }

    /** Conversion.ToBool(GetConfigValueFromGlobal(name)). */
    private boolean configBool(String name) {
        String v = repository.configValue(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), name);
        if (v == null) return false;
        v = v.trim();
        return "1".equals(v) || "true".equalsIgnoreCase(v);
    }

    public List<Map<String, Object>> getHistory(String fromDate, String toDate, String dateType,
                                                int commissionAgentId, int buyerId, int deliverToPartyId,
                                                String shipToAddress) {
        return repository.formHistory(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(), canViewAllRecords(),
                currentUserContext.currentUserId(), fromDate, toDate, dateType,
                commissionAgentId, buyerId, deliverToPartyId, shipToAddress);
    }

    /** HistoryComboDbCall :589-604 (OrganizationId, CompanyId; no Activity). */
    public List<Map<String, Object>> historyCombos() {
        try {
            return repository.historyCombos(currentUserContext.currentOrganizationId(),
                    currentUserContext.currentCompanyId());
        } catch (Exception e) {
            return new ArrayList<>();
        }
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();
        Map<String, Object> header = id == null ? null : repository.readById(id);
        if (header == null || !belongsToSession(header)) {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
            return result;
        }
        result.put("status", "SUCCESS");
        result.put("data", header);
        return result;
    }

    /** btnDelete_Click :2191-2212 -> BLL DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> delete(Integer id) {
        Map<String, Object> result = new HashMap<>();
        try {
            if (id == null || id <= 0) throw new IllegalArgumentException("No record found to Delete");
            /* btnDelete.Enabled = formright.DoHaveCanDelete (:554). */
            if (!Boolean.TRUE.equals(formRights().get("delete")))
                throw new IllegalArgumentException("you don't have delete rights...");
            requireOwnRecord(id);
            repository.deleteById(currentUserContext.currentUserId(), id);
            result.put("status", "SUCCESS");
            result.put("message", "Delete Record Successfully");
        } catch (Exception e) {
            result.put("status", "ERROR");
            result.put("message", rootMessage(e));
        }
        return result;
    }

    public Map<String, Object> generateCode() {
        Map<String, Object> r = new HashMap<>();
        r.put("docNo", repository.generateCode(currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), currentUserContext.currentBranchId(),
                currentUserContext.currentFinancialYearId(), DOCUMENT_TYPE_ID));
        return r;
    }

    /** ReadById / DeleteById / the update branch key on the id alone; refuse another tenant's id. */
    private void requireOwnRecord(int id) {
        Map<String, Object> h = repository.readById(id);
        if (h == null || !belongsToSession(h)) {
            throw new IllegalArgumentException("The Specified cmagt.gdnBuyerDispatchMaster Record Does Not Exist In The Database");
        }
    }

    private boolean belongsToSession(Map<String, Object> h) {
        return toInt(pick(h, "organizationId")) == currentUserContext.currentOrganizationId()
            && toInt(pick(h, "companyId")) == currentUserContext.currentCompanyId();
    }

    /**
     * CommonServices.SetRightsValueInRightsObject(ScreenName) (:522, CommonServices.cs:17565):
     * role "Admin" gets every right; otherwise Save / Update / Delete / Print / "CanView AllRecord"
     * come from the user's grant rows (dbo.Sp_tblUserRights_GetAllMethod @Activity='GetByUserId')
     * for frmGoodsDispatchingNoteCmagt. A right with no grant row is false (CLR default).
     */
    public Map<String, Boolean> formRights() {
        Map<String, Boolean> r = new LinkedHashMap<>();
        String role = currentUserContext.currentRoleName();
        boolean admin = "Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role);
        for (String k : new String[] {"view", "save", "update", "delete", "print", "canViewAllRecord"}) r.put(k, admin);
        if (admin) return r;
        try {
            for (Map<String, Object> row : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = pick(row, "RightName");
                if (name == null) continue;
                String n = name.toString().trim();
                String key = "View".equals(n) ? "view" : "Save".equals(n) ? "save" : "Update".equals(n) ? "update"
                        : "Delete".equals(n) ? "delete" : "Print".equals(n) ? "print"
                        : RIGHT_CAN_VIEW_ALL_RECORDS.equals(n) ? "canViewAllRecord" : null;
                if (key == null) continue;
                Object v = pick(row, "Value");
                boolean b = v instanceof Boolean ? (Boolean) v
                        : v instanceof Number ? ((Number) v).intValue() != 0
                        : v != null && ("1".equals(v.toString().trim()) || "true".equalsIgnoreCase(v.toString().trim()));
                r.put(key, b);
            }
        } catch (Exception ignored) { }
        return r;
    }

    public boolean canViewAllRecords() {
        String role = currentUserContext.currentRoleName();
        if ("Admin".equalsIgnoreCase(role) || "Administrator".equalsIgnoreCase(role)) return true;
        try {
            for (Map<String, Object> r : rightsRepo.userRightsForScreen(
                    currentUserContext.currentUserId(), DESKTOP_SCREEN_NAME, role,
                    currentUserContext.currentCompanyId())) {
                Object name = pick(r, "RightName");
                if (name != null && RIGHT_CAN_VIEW_ALL_RECORDS.equalsIgnoreCase(name.toString().trim())) {
                    Object v = pick(r, "Value");
                    if (v instanceof Boolean) return (Boolean) v;
                    if (v instanceof Number)  return ((Number) v).intValue() != 0;
                    return v != null && ("1".equals(v.toString().trim())
                            || "true".equalsIgnoreCase(v.toString().trim()));
                }
            }
        } catch (Exception ignored) { }
        return false;
    }

    private static Object pick(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet())
            if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        try { return v == null ? 0 : Integer.parseInt(v.toString().trim()); } catch (Exception e) { return 0; }
    }

    private static void reqInt(Integer v, String name, boolean textBox) {
        if (v == null || v == 0) {
            throw new IllegalArgumentException(name + (textBox ? " must be a non-zero number" : " field is required"));
        }
    }

    private static void reqStr(String v, String name) {
        if (isBlank(v)) throw new IllegalArgumentException(name + " field is required");
    }

    /** FormHelper.ValidateField :503-509. */
    private static void reqField(Object v, String name, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof BigDecimal && ((BigDecimal) v).signum() <= 0);
        if (bad) throw new IllegalArgumentException(name + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    private static Date day(String s) {
        if (isBlank(s)) return null;
        try {
            SimpleDateFormat f = new SimpleDateFormat("yyyy-MM-dd");
            f.setLenient(false);
            return f.parse(s.trim().substring(0, Math.min(10, s.trim().length())));
        } catch (Exception e) {
            return null;
        }
    }

    private static String fmt(BigDecimal v) {
        return new java.text.DecimalFormat("#,##0.###").format(nd(v));
    }

    private static String rootMessage(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        return m == null ? e.toString() : m;
    }

    private static boolean isBlank(String v) { return v == null || v.trim().isEmpty(); }
    private static int ni(Integer v) { return v == null ? 0 : v; }
    private static BigDecimal nd(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }
}
