package com.mst.services.imports;

import com.mst.models.UserAccount;
import com.mst.models.imports.ImpBInvoicePackingList;
import com.mst.repositories.imports.ImpBRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.imports.ImpBSupport.*;

/**
 * 784 Invoice Packing Detail - Architecture.WinApp.Import.Transactions.frmInvoicePackingDetail (ScreenName
 * frmInvoicePackingDetail). BLL Architecture.BLL.Import.Transaction.invoicePackingList, DAL invoicePackingListProvider.
 *
 * The brand / brand-UOM / packing-type / rate / amount controls of the designer sit below the 105 px entry panel
 * (panel15) and are never visible on the desktop; FilldtBrandForPMItem / ValidatePmQty / PmGrid*QtyValidation /
 * CheckBomHeaderIds are never called from a wired event - none of that is ported.
 */
@Service
public class ImpBPackingDetailService {

    public static final int SCREEN = 784;
    public static final String SCREEN_NAME = "frmInvoicePackingDetail";

    @Autowired private ImpBRepository repo;
    @Autowired private ImpBSupport sup;

    /** InvoiceNoFill(): invoicePackingList.GetInvoiceNo(Org, Company, 902, ActiveYr, RecId) -> {Id, InvoiceNo}. */
    private List<Map<String, Object>> invoices(UserAccount u, long recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.packingInvoices(u, sup.financialYearId(), recId))
            out.add(map("Id", r.get("invoiceMasterId"), "InvoiceNo", str(r.get("invoiceMasterNo"))));
        return out;
    }

    /** PackingItemFill(): CommonServices.GetItemByItemTypeId("14") -> dtPMItem {Id, ItemName, ItemCode = ItemCodeNew}. */
    private List<Map<String, Object>> items(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsByType(u, "14")) out.add(map("Id", r.get("Id"), "ItemName", str(r.get("ItemName")), "ItemCode", str(r.get("ItemCodeNew"))));
        return out;
    }

    /** JobLotFill(): JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId -> BindDDLNew(Id, JobLotDescription). */
    private List<Map<String, Object>> jobLots(UserAccount u) { return pick(repo.jobLotsOfBranch(u), "Id", "JobLotDescription"); }

    /** HistoryCombosFill(): ImportRelated rows with Activity "Customer" -> {Id, Customer}. */
    private List<Map<String, Object>> customers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : activity(repo.importRelated(u), "Customer")) out.add(map("Id", r.get("Id"), "Customer", r.get("name")));
        return out;
    }

    /** frmInvoicePackingDetail_Load: ConfigureRights (+ ItemSearchByCode), ConfigureControls, BindData. */
    public Map<String, Object> setup() {
        UserAccount u = sup.user(SCREEN);
        return map("rights", sup.formRights(u, SCREEN, SCREEN_NAME), "historyDays", sup.historyDays(),
                "itemSearchByCode", toBool(sup.config("ItemSearchByCode")),
                "invoices", invoices(u, 0), "items", items(u), "jobLots", jobLots(u), "customers", customers(u));
    }

    /** btnRefresh_Click: InvoiceNoFill, PackingItemFill, JobLotFill. */
    public Map<String, Object> refresh(long recId) {
        UserAccount u = sup.user(SCREEN);
        return map("invoices", invoices(u, Math.max(0, recId)), "items", items(u), "jobLots", jobLots(u));
    }

    /** btnRefreshHistory_Click: HistoryCombosFill. */
    public List<Map<String, Object>> historyCombos() { return customers(sup.user(SCREEN)); }

    /** CmbPMItem_Leave -> BindPmItemPackUom: CommonServices.GetUomScheduleByItemId (Id, UOMCode, Equivalent ...). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = sup.user(SCREEN);
        if (itemId <= 0) return new ArrayList<>();
        return pick(repo.uoms(u, itemId), "Id", "UOMCode", "Equivalent", "QtyEquivalent");
    }

    private void ownInvoice(UserAccount u, long invoiceId) {
        List<Map<String, Object>> r = repo.invoice(invoiceId);
        if (r.isEmpty() || toInt(r.get(0).get("OrganizationId")) != toInt(u.getOrganizationId())
                || toInt(r.get(0).get("CompanyId")) != toInt(u.getCompanyId())) throw invalid("Record not found.");
    }

    /** ReadById(ID) / GetDetailGrdByHeadId(Id): invoicePackingList.GetById -> the grid rows (null list = nothing). */
    public Map<String, Object> byInvoice(long invoiceId) {
        UserAccount u = sup.user(SCREEN);
        ownInvoice(u, invoiceId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.packingByInvoice(invoiceId)) {
            out.add(map("Id", r.get("invoicePackingListId"), "ContainerNo", str(r.get("containerNo")), "ItemId", toInt(r.get("ItemId")),
                    "ItemCode", str(r.get("ItemCode")), "ItemName", str(r.get("ItemName")), "ItemDescription", str(r.get("ItemDescription")),
                    "JobLotId", toInt(r.get("JobLotId")), "JobLot", str(r.get("JobLot")), "PackUomId", toInt(r.get("packSizeIdOuter")),
                    "PackUom", str(r.get("PackUom")), "Qty", toDouble(r.get("qtyOuter")), "Weight", toDouble(r.get("NetWeightKgs")),
                    "Remarks", str(r.get("RemarksDetail"))));
        }
        return map("found", !out.isEmpty(), "rows", out);
    }

    /**
     * HistoryFill() -> invoiceMaster.SEARCHHistory: @OrganizationId, @CompanyId, @DocumentTypeId, @CanViewAllRecord always.
     * The form never sets DocumentTypeId, so @DocumentTypeId = 0 is sent (desktop behaviour, reproduced: the SEARCH
     * branch filters R.documentTypeId = 0). Date pair of the chosen radio when ticked, @DocNoFrom / @DocNoTo /
     * @exporterId when != 0, @createdUserId when the user cannot view all records.
     */
    public List<Map<String, Object>> history(Map<String, Object> f) {
        UserAccount u = sup.user(SCREEN);
        boolean all = sup.canViewAllRecord(SCREEN_NAME);
        Map<String, Object> p = map("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "DocumentTypeId", 0, "CanViewAllRecord", all);
        historyDates(p, f);
        int fromNo = toInt(f.get("fromDocNo")), toNo = toInt(f.get("toDocNo")), exp = toInt(f.get("customerId"));
        if (fromNo != 0) p.put("DocNoFrom", fromNo);
        if (toNo != 0) p.put("DocNoTo", toNo);
        if (exp != 0) p.put("exporterId", exp);
        if (!all) p.put("createdUserId", u.getId());
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoiceSearch(p)) {
            out.add(map("Id", r.get("invoiceMasterId"), "InvoiceNo", r.get("invoiceMasterNo"), "InvoiceDate", r.get("docDate"),
                    "EntryUser", r.get("CreatedUserName"), "EntryDate", r.get("CreatedOn"), "ModifyUser", r.get("LastModifiedUserName"),
                    "ModifyDate", r.get("lastModifiedOn")));
        }
        return out;
    }

    /** The four-radio date block shared by the history tabs: doc / entry / modify / approved, each box only when ticked. */
    static void historyDates(Map<String, Object> p, Map<String, Object> f) {
        String kind = str(f.get("dateType"));
        LocalDateTime from = toBool(f.get("fromChecked")) ? toDay(f.get("fromDate")) : null;
        LocalDateTime to = toBool(f.get("toChecked")) ? toDay(f.get("toDate")) : null;
        String fk, tk;
        switch (kind) {
            case "entry": fk = "EntryFromDate"; tk = "EntryToDate"; break;
            case "modify": fk = "ModifyFromDate"; tk = "ModifyToDate"; break;
            case "approved": fk = "ApprovedFromDate"; tk = "ApprovedToDate"; break;
            default: fk = "FromDate"; tk = "ToDate";
        }
        if (from != null) p.put(fk, ts(from));
        if (to != null) p.put(tk, ts(to));
    }

    /**
     * btnSave_Click (RecId = 0) / btnUpdate_Click -> Insert(): FormValidation ("Sale Invoice Is required"), the row checks
     * with the row number, then invoicePackingList.Save: one usp_Set_invoicePackingList per grid row in one transaction,
     * @Activity INSERT (Id 0) / UPDATE (Id > 0). Rows removed from the grid (LstRemoveRecordDetail) are never sent by
     * the desktop - reproduced (see the report).
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = sup.user(SCREEN);
        boolean update = toBool(b.get("update"));
        long recId = toLong(b.get("recId"));
        if (update) {                                            // btnUpdate.Enabled = DoHaveUpdateRights || DoHaveSaveRight
            if (!sup.can(u, SCREEN, "Update") && !sup.can(u, SCREEN, "Save")) sup.require(u, SCREEN, "Update");
        }                                                        // btnSave has no right check on this form (ConfigureRights)
        if (update && recId == 0) throw invalid("RecId not Found");
        long invoiceId = toLong(b.get("invoiceMasterId"));
        if (invoiceId == 0) throw invalid("Sale Invoice Is required");
        ownInvoice(u, invoiceId);
        List<Map<String, Object>> rows = list(b.get("rows"));
        if (rows.isEmpty()) throw invalid("Packing Material Grid record not found");
        Set<Long> existing = new HashSet<>();
        for (Map<String, Object> r : repo.packingByInvoice(invoiceId)) existing.add(toLong(r.get("invoicePackingListId")));
        LocalDateTime now = LocalDateTime.now();
        List<ImpBInvoicePackingList> list = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            i++;
            ImpBInvoicePackingList vd = new ImpBInvoicePackingList();
            vd.invoicePackingListId = toLong(r.get("Id"));
            if (vd.invoicePackingListId > 0 && !existing.contains(vd.invoicePackingListId)) throw invalid("Record not found.");
            vd.appActionId = vd.invoicePackingListId <= 0 ? 1 : 2;
            if (toInt(r.get("ItemId")) == 0) throw invalid("Item Is Required In Packing Material Grid in Row No " + i);
            if (toInt(r.get("PackUomId")) == 0) throw invalid("PackUomId Is Required In Packing Material Grid in Row No " + i);
            if (toInt(r.get("Qty")) == 0) throw invalid("Qty Is Required In Packing Material Grid in Row No " + i);
            vd.containerNo = str(r.get("ContainerNo"));
            vd.ItemId = toInt(r.get("ItemId"));
            vd.ItemDescription = str(r.get("ItemDescription"));
            vd.JobLotId = toInt(r.get("JobLotId"));
            vd.packSizeIdOuter = toInt(r.get("PackUomId"));
            vd.packSizeIdinner = toInt(r.get("PackUomId"));
            vd.qtyOuter = toDouble(r.get("Qty"));
            vd.qtyInner = toDouble(r.get("Qty"));
            vd.netWeightOuter = toDouble(r.get("Weight"));
            vd.netWeightInner = toDouble(r.get("Weight"));
            vd.NetWeightKgs = toDouble(r.get("Weight"));
            vd.RemarksDetail = str(r.get("Remarks"));
            vd.createdUserId = u.getId();
            vd.lastModifiedUserId = u.getId();
            vd.invoiceMasterId = invoiceId;                          // DAL Set: item.invoiceMasterId = obj.invoiceMasterId
            vd.lastModifiedOn = now; vd.createdOn = now; vd.approvedOn = now;   // BLL FillEntity
            list.add(vd);
        }
        repo.tx(() -> {
            for (ImpBInvoicePackingList vd : list)
                repo.set(ImpBRepository.IMEX + "[usp_Set_invoicePackingList]", vd, vd.appActionId == 1 ? "INSERT" : "UPDATE");
            return null;
        });
        return saved((int) invoiceId, update ? "Record Update Successfully" : "Record Save Successfully");
    }
}
