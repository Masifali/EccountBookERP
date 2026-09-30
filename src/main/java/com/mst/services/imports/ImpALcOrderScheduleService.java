package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpADtos;
import com.mst.models.imports.ImpALcOrderScheduleHeader;
import com.mst.models.imports.ImpALcOrderSchedulePackingDetail;
import com.mst.repositories.imports.ImpARepository;
import com.mst.services.hrm.HrmSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
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
 * BLL of screen 526 "Import Sale Contract Schedule" - Architecture.WinApp.Import/ImLcOrderSchedule.cs
 * (BLL 0413 ImLcOrderScheduleHeader / DAL 0470). The form's second tab ("HistoryNotInUse") is removed by the form's own
 * Load (tabControl1.TabPages.Remove(tabPage2)), so it is not ported.
 */
@Service
public class ImpALcOrderScheduleService {

    public static final int SCREEN = 526;

    @Autowired private ImpASupport sup;
    @Autowired private ImpARepository repo;

    private HrmSupport hrm() { return sup.hrm(); }

    // ================================================================== load

    /**
     * frmSaleContractSchedule_Load: rights("ImLcOrderSchedule") - btnsave / btnSavedetailSchedule = Save, BtnHistoryPrint = Print;
     * CropYearFill, ItemPackingTypeFill, GetpendingContractsforShipmentSchedule(50), ComboBindForHistory; the sea ports feed the
     * schedule grid's DestinationPortId value list (grdContractSheduleSettings).
     */
    public Map<String, Object> setup() {
        UserAccount u = hrm().user(SCREEN);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", sup.rights(u, SCREEN));
        out.put("cropYears", pick(repo.cropYears(u), "Id", "CropYear"));
        out.put("packingTypes", pick(repo.packMaterialTypes(u), "Id", "Description"));
        out.put("ports", pick(repo.seaPorts(u), "Id", "PortName"));
        out.put("contracts", contractsOf(u, 50));
        out.putAll(historyCombosOf(u));
        return out;
    }

    /** BtnLoadAllPendingContracts_Click / BtnNewMain_Click: GetpendingContractsforShipmentSchedule() (NoOfRecords 0). */
    public List<Map<String, Object>> contracts(boolean all) { return contractsOf(hrm().user(SCREEN), all ? 0 : 50); }

    /** GridContactsExistInScheduleOrNot(Org, Comp, 1, NoOfRecords) -> tablePendingContracts. */
    private List<Map<String, Object>> contractsOf(UserAccount u, int noOfRecords) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.scheduleContracts(u, 1, noOfRecords)) {
            out.add(map("Id", r.get("ImLcOrderId"), "ContractNo", r.get("LcOrderNo"), "ContractDate", r.get("LcOrderDate"),
                    "NoOfContainers", r.get("NoOfContainers"), "WeightM_Ton", toDouble(r.get("WeightM_Ton")),
                    "SupplierCustomerId", r.get("SupplierCustomerId"), "SupplierName", r.get("Customer"),
                    "DestinationPortId", r.get("DestinationPortId"), "LastShipmentDate", r.get("LastShipmentDate")));
        }
        return out;
    }

    /** btnRefreshHistoryCombos_Click -> ComboBindForHistory(): one procedure, rows split by Activity. */
    public Map<String, Object> historyCombos() { return historyCombosOf(hrm().user(SCREEN)); }

    private Map<String, Object> historyCombosOf(UserAccount u) {
        List<Map<String, Object>> ports = new ArrayList<>(), customers = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.scheduleHistoryCombos(u)) {
            Map<String, Object> m = map("Id", r.get("Id"), "Name", r.get("ReferenceName"));
            String a = str(r.get("Activity"));
            if ("DestinationPort".equals(a)) ports.add(m);
            else if ("Customer".equals(a)) customers.add(m);
            else if ("Item".equals(a)) items.add(m);
        }
        return map("historyPorts", ports, "historySuppliers", customers, "historyItems", items);
    }

    // ================================================================== contract load

    /** The contract (ImLcOrder) only when it belongs to the signed-in company. */
    private Map<String, Object> contract(UserAccount u, int id) {
        List<Map<String, Object>> r = repo.lcOrderById(id);
        if (r.isEmpty() || !sameCompany(r.get(0), u)) throw invalid("Record not found.");
        return r.get(0);
    }

    /**
     * grdPendingContract_ColumnButtonClick(ContractNo) / DataGridHistory Edit: ReadbyMainScheduleByContractId,
     * ReadScheduleDetailByContactId, AttentiveLoadDateFillByContractId, ItemNameFillByContractId. The page builds the
     * one-row "new schedule" grid itself when no schedule exists (UpdateModeMain false), exactly as the form does.
     */
    public Map<String, Object> load(int contractId) {
        UserAccount u = hrm().user(SCREEN);
        contract(u, contractId);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> main = new ArrayList<>();
        for (Map<String, Object> r : repo.scheduleByContract(contractId)) {
            main.add(map("Id", r.get("Id"), "ContractId", r.get("ImLcOrderId"), "SupplierCustomerId", r.get("SupplierCustomerId"),
                    "SupplierName", r.get("CustomerName"), "NoOfContainers", r.get("NoOfContainer"),
                    "WeightM_Ton", dec(r.get("NetWeight")).divide(BigDecimal.valueOf(1000)), "AttentiveLoadingDate", r.get("AttentiveLoadingDate"),
                    "DestinationPortId", r.get("DestinationPortId"), "AttentiveProductionDate", r.get("AttentiveProductionDate"),
                    "AttentiveInspectionDate", r.get("AttentiveInspectionDate")));
        }
        out.put("main", main);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> d : repo.schedulePackingByContract(contractId)) {
            det.add(map("Id", d.get("Id"), "ScheduleId", d.get("ImLcOrderScheduleId"), "ContractId", d.get("ImLcOrderId"),
                    "AttentiveLoadingDate", d.get("AttentiveLoadingDate"), "ItemId", d.get("ImItemId"), "Item", d.get("ItemName"),
                    "ItemCode", d.get("ItemCode"), "CropYearId", d.get("CropYearId"), "CropYear", d.get("CropYear"),
                    "PackingTypeId", d.get("PackingTypeId"), "PackingType", d.get("PackingType"), "NoOfBags", d.get("NoOfBags"),
                    "PackUomId", d.get("PackUomId"), "PackUom", d.get("PackUom"),
                    "WeightM_Ton", dec(d.get("NetWeight")).divide(BigDecimal.valueOf(1000))));
        }
        out.put("detail", det);
        List<Map<String, Object>> dates = new ArrayList<>();
        for (Map<String, Object> r : repo.attentiveLoadDates(contractId)) {
            dates.add(map("ScheduleId", r.get("ScheduleId"), "AttentiveLoadingDate", r.get("AttentiveLoadingDate"), "ContractId", r.get("ContractId")));
        }
        out.put("dates", dates);
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.lcOrderItems(u, contractId)) {
            items.add(map("Id", r.get("ItemId"), "ItemName", r.get("ItemName"), "ItemCode", r.get("ItemCode")));
        }
        out.put("items", items);
        return out;
    }

    /** cmbAttentiveLoadDateBySchedule_ValueChanged -> GetTotalMTonOfDate: NetWeight / 1000 of that schedule row, else 0. */
    public Map<String, Object> scheduleMTon(int contractId, int scheduleId) {
        UserAccount u = hrm().user(SCREEN);
        contract(u, contractId);
        List<Map<String, Object>> r = repo.scheduleWeight(contractId, scheduleId);
        double mton = r.isEmpty() ? 0.0 : toDouble(r.get(0).get("NetWeight")) / 1000.0;
        return map("mton", mton);
    }

    /** cmbItem_Leave -> bindItemPackUom(): CommonServices.GetUomScheduleByItemId. */
    public List<Map<String, Object>> uoms(int itemId) { return sup.uoms(hrm().user(SCREEN), itemId); }

    // ================================================================== save main schedule

    /**
     * InsertScheduleMain(): the duplicate-date check, then per row (in grid order) the Row No checks and the model;
     * rows removed from the grid (LstRemoveRecordMain, ActionId 3) go first. DAL SetData: Id <= 0 -> _Insert else _Update,
     * one transaction. The attachment part of the DAL is not ported (see report).
     */
    public Map<String, Object> saveMain(ImpADtos.ScheduleMain b) {
        UserAccount u = hrm().user(SCREEN);
        hrm().require(u, SCREEN, "Save");
        if (b.rows == null || b.rows.isEmpty()) throw invalid("Schedule Grid have no row");
        boolean update = false;
        for (ImpADtos.ScheduleRow r : b.rows) if (r.id > 0) { update = true; break; }
        Set<String> dates = new HashSet<>();
        for (ImpADtos.ScheduleRow r : b.rows) {
            LocalDateTime d = toDate(r.attentiveLoadingDate);
            if (d != null && !dates.add(d.toString())) {
                throw invalid("The Grid Contact Schedule have more than 1 rows of same Attentive Loading date.Please check...");
            }
        }
        Set<Integer> ports = ids(repo.seaPorts(u), "Id");
        Map<Integer, Set<Integer>> savedByContract = new HashMap<>();
        LocalDateTime now = LocalDateTime.now();
        List<ImpALcOrderScheduleHeader> list = new ArrayList<>();
        if (b.removed != null) {
            for (ImpADtos.ScheduleRow r : b.removed) {
                if (r.id <= 0 || !savedIds(u, r.contractId, savedByContract).contains(r.id)) continue;
                ImpALcOrderScheduleHeader h = header(u, r, now);
                h.SupplierCustomerId = toInt(contract(u, r.contractId).get("SupplierCustomerId"));
                h.ActionId = 3;
                list.add(h);
            }
        }
        int rowNo = 0;
        for (ImpADtos.ScheduleRow r : b.rows) {
            rowNo++;
            if (toDate(r.attentiveLoadingDate) == null) throw invalid("AttentiveLoadingDate cannot be null in Row No:" + rowNo);
            Map<String, Object> c = contract(u, r.contractId);
            if (r.id > 0 && !savedIds(u, r.contractId, savedByContract).contains(r.id)) throw invalid("Record not found.");
            if (cellInt(r.noOfContainers) == 0) throw invalid("NoOfContainers Required in Row No:" + rowNo);
            if (dec(r.weightMTon).signum() == 0) throw invalid("WeightM_Ton Required in Row No:" + rowNo);
            if (r.destinationPortId == 0 || !ports.contains(r.destinationPortId)) throw invalid("DestinationPort Required in Row No:" + rowNo);
            ImpALcOrderScheduleHeader h = header(u, r, now);
            h.SupplierCustomerId = toInt(c.get("SupplierCustomerId"));
            h.ActionId = h.Id <= 0 ? 1 : 2;
            list.add(h);
        }
        repo.tx(() -> {
            for (ImpALcOrderScheduleHeader h : list) {
                repo.set(h.Id <= 0 ? "[dbo].[USP_ImLcOrderScheduleHeader_Insert]" : "[dbo].[USP_ImLcOrderScheduleHeader_Update]", h);
            }
            return 0;
        });
        return saved(0, update ? "Main Schedule Update SuccessFully" : "Main Schedule Save SuccessFully ");
    }

    private Set<Integer> savedIds(UserAccount u, int contractId, Map<Integer, Set<Integer>> cache) {
        return cache.computeIfAbsent(contractId, k -> { contract(u, k); return ids(repo.scheduleByContract(k), "Id"); });
    }

    private ImpALcOrderScheduleHeader header(UserAccount u, ImpADtos.ScheduleRow r, LocalDateTime now) {
        ImpALcOrderScheduleHeader h = new ImpALcOrderScheduleHeader();
        h.Id = Math.max(0, r.id);
        h.ImLcOrderId = r.contractId;
        h.SupplierCustomerId = r.supplierCustomerId;
        h.NoOfContainer = cellInt(r.noOfContainers);
        h.NetWeight = dec(r.weightMTon).multiply(BigDecimal.valueOf(1000));
        h.AttentiveLoadingDate = toDate(r.attentiveLoadingDate);
        h.DestinationPortId = r.destinationPortId;
        h.AttentiveProductionDate = orNow(r.attentiveProductionDate);
        h.AttentiveInspectionDate = orNow(r.attentiveInspectionDate);
        h.EntryDate = now;
        h.EntryUserId = u.getId();
        h.ModifyDate = now;
        h.ModifyUserId = u.getId();
        h.OrganizationId = u.getOrganizationId();
        h.FinancialYearId = hrm().financialYearId();
        h.CompanyId = u.getCompanyId();
        h.BranchesId = toInt(u.getBranchesId());
        return h;
    }

    // ================================================================== save detail schedule

    /**
     * InsertScheduleDetail(): removed rows (LstRemoveRecordDetail, ActionTypeId 3) first, then every grid row with the
     * Row No checks; ImLcOrderScheduleHeader.SaveDetail -> USP_ImLcOrderSchedulePackingDetail_Insert per row, one transaction.
     */
    public Map<String, Object> saveDetail(ImpADtos.SchedulePacking b) {
        UserAccount u = hrm().user(SCREEN);
        hrm().require(u, SCREEN, "Save");
        if (b.rows == null || b.rows.isEmpty()) throw invalid("Schedule Grid have no row");
        boolean update = false;
        for (ImpADtos.SchedulePackingRow r : b.rows) if (r.id > 0) { update = true; break; }
        Set<Integer> crop = ids(repo.cropYears(u), "Id");
        Set<Integer> packs = ids(repo.packMaterialTypes(u), "Id");
        Map<Integer, Set<Integer>> schedules = new HashMap<>(), savedRows = new HashMap<>(), itemsOf = new HashMap<>();
        Map<Integer, Set<Integer>> uomsOf = new HashMap<>();
        List<ImpALcOrderSchedulePackingDetail> list = new ArrayList<>();
        if (b.removed != null) {
            for (ImpADtos.SchedulePackingRow r : b.removed) {
                if (r.id <= 0) continue;
                contract(u, r.contractId);
                if (!savedRows.computeIfAbsent(r.contractId, k -> ids(repo.schedulePackingByContract(k), "Id")).contains(r.id)) continue;
                ImpALcOrderSchedulePackingDetail d = packing(r);
                d.ActionTypeId = 3;
                list.add(d);
            }
        }
        int rowNo = 0;
        for (ImpADtos.SchedulePackingRow r : b.rows) {
            rowNo++;
            contract(u, r.contractId);
            if (!schedules.computeIfAbsent(r.contractId, k -> ids(repo.attentiveLoadDates(k), "ScheduleId")).contains(r.scheduleId)) {
                throw invalid("Attentive Loading Date is Required!");
            }
            if (r.id > 0 && !savedRows.computeIfAbsent(r.contractId, k -> ids(repo.schedulePackingByContract(k), "Id")).contains(r.id)) {
                throw invalid("Record not found.");
            }
            if (r.itemId == 0 || !itemsOf.computeIfAbsent(r.contractId, k -> ids(repo.lcOrderItems(u, k), "ItemId")).contains(r.itemId)) {
                throw invalid("Item Required in Row No:" + rowNo);
            }
            if (r.cropYearId == 0 || !crop.contains(r.cropYearId)) throw invalid("CropYear Required in Row No:" + rowNo);
            if (r.packingTypeId == 0 || !packs.contains(r.packingTypeId)) throw invalid("PackingType Required in Row No:" + rowNo);
            if (cellInt(r.noOfBags) == 0) throw invalid("NoOfBags Required in Row No:" + rowNo);
            if (r.packUomId == 0 || !uomsOf.computeIfAbsent(r.itemId, k -> ids(repo.uomsByItem(u, k), "Id")).contains(r.packUomId)) {
                throw invalid("PackUom Required in Row No:" + rowNo);
            }
            if (dec(r.weightMTon).signum() == 0) throw invalid("WeightM_Ton Required in Row No:" + rowNo);
            ImpALcOrderSchedulePackingDetail d = packing(r);
            d.ActionTypeId = d.Id <= 0 ? 1 : 2;
            list.add(d);
        }
        repo.tx(() -> {
            for (ImpALcOrderSchedulePackingDetail d : list) repo.set("USP_ImLcOrderSchedulePackingDetail_Insert", d);
            return 0;
        });
        return saved(0, update ? "Detail Schedule Update SuccessFully" : "Detail Schedule Save SuccessFully");
    }

    private static ImpALcOrderSchedulePackingDetail packing(ImpADtos.SchedulePackingRow r) {
        ImpALcOrderSchedulePackingDetail d = new ImpALcOrderSchedulePackingDetail();
        d.Id = Math.max(0, r.id);
        d.ImLcOrderId = r.contractId;
        d.ImLcOrderScheduleId = r.scheduleId;
        d.ImItemId = r.itemId;
        d.CropYearId = r.cropYearId;
        d.PackingTypeId = r.packingTypeId;
        d.NoOfBags = cellInt(r.noOfBags);
        d.PackUomId = r.packUomId;
        d.NetWeight = dec(r.weightMTon).multiply(BigDecimal.valueOf(1000));
        return d;
    }

    /** Conversion.ToInt of a double grid cell: Convert.ToInt32(double) - rounded half to even. */
    private static int cellInt(String v) {
        return dec(v).setScale(0, RoundingMode.HALF_EVEN).intValue();
    }

    private static LocalDateTime orNow(String s) {
        LocalDateTime d = toDate(s);
        return d == null ? LocalDateTime.now() : d;
    }

    // ================================================================== history

    /**
     * DataGridHistoryFill(NoOfRecords): ContractSchedule_FormHistory with the Filters group (dates always set; port / supplier / item
     * only when picked). Show and the History tab use 50, Load All 0. Rows are the procedure's (the page shows the desktop's dtcol
     * columns and prints the checked ones with 239-ImLcOrderSchedule_FormHistory.rpt); the logo bytes are left out of the JSON.
     */
    public List<Map<String, Object>> history(String from, String to, int portId, int supplierId, int itemId, boolean all) {
        UserAccount u = hrm().user(SCREEN);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.scheduleHistory(u, dayNow(from), dayNow(to), supplierId, portId, itemId, all ? 0 : 50)) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) if (!"CompLogoImage".equalsIgnoreCase(e.getKey())) m.put(e.getKey(), e.getValue());
            out.add(m);
        }
        return out;
    }

    /** BtnHistoryPrint_Click needs the Print right (the button is enabled by it). */
    public Map<String, Object> printCheck() {
        UserAccount u = hrm().user(SCREEN);
        hrm().require(u, SCREEN, "Print");
        return map("ok", true);
    }
}
