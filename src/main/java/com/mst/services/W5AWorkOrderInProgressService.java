package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.W3BWorkOrderRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.day;
import static com.mst.services.Prod273Support.list;
import static com.mst.services.Prod273Support.now;
import static com.mst.services.Prod273Support.pickerValue;
import static com.mst.services.Prod273Support.stamp;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.ts;

/**
 * W5A - Mfg Production hub, screen 594 "Work Order In Progress".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.Mfg.frmWorkOrderInProgress (.cs, 3182 lines), base.Name "frmWorkOrderInProgress" (rights key)
 *   DocumentTypeId 1620 (:895)
 *   Route          /production/mfg/work-order-in-progress      API /production/mfg/work-order-in-progress/api
 *   Print          CommonServices.WorkOrderInProgress_Slip1620(id) -> 1620_WorkOrderInProgress_Slip.rpt (:1789), existing
 *                  /reports/print/1620-work-order-in-progress-slip
 *   BLL / DAL      recovered_source/projects/architecture.bll/0367 + architecture.dal/0320 (WorkOrderInProgress), via {@link W3BWorkOrderRepository}
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 * <ol>
 *  <li>Load:235 - rights, ItemFill:315 (CommonServices.ItemGetAllServiceBind), uomgetAll:356 (UOMSchedule.Getall), FillgrdWorkOrderMain:514
 *      (WorkOrderInProgress.GetWorkOrderInProgressData = [Mfg].[USP_GetWorkOrderInProgressData], org / company / branch / financial year).</li>
 *  <li>The main grid lists the saved Work Order In Progress documents. Clicking the WorkOrderNo link (:648) loads that document
 *      (ReadById:992 = [Mfg].[USP_WorkOrderInProgress_GetAllMethod] 'ReadById' + 'ReadByHeaderId_WorkOrderInProgressDetail').
 *      StartDate / EndDate / RemarksHeader of the main grid are editable (:550-:590) and are the values that are saved (:922-:929).</li>
 *  <li>Detail grid (:759): Edit button (grd_DoubleClick:679) loads the row into the Detail box; Update (:723) writes Item, UOM, dates / times,
 *      remarks back into the grid row. Qty, Work Station and Operation boxes are Enabled=false in the designer (read only).
 *      The "+" (Add) button is hidden at load (:307) and its click handler is empty (:675); there is no row delete in this screen.</li>
 *  <li>Save (btnUpdate -> btnsave_Click:852 -> Insert:871): "Please Select Any Record first..." when nothing is open, grid-empty messages, confirm,
 *      header (DocumentTypeId 1620, IsApproved false, ProjectsId = BranchesId), detail rows with a WorkStationOperationId (ActionTypeId 2 for a saved row),
 *      "StartDate can not be null in Grid Operations Row No: n" / "CompleteDate ...", then [Mfg].[USP_WorkOrderInProgress_InsertAndUpdate] (ActionId 2)
 *      and [Mfg].[USP_WorkOrderInProgressDetail_Insert] per row in one transaction (DAL SetData:0320), "Data Update Successfully....  DocNo".</li>
 * </ol>
 *
 * DEVIATIONS / NOTES
 * <ol>
 *  <li>History tab: the desktop REMOVES it from the tab control at load (:268 tabControl1.TabPages.Remove(tabPage3)), so it is unreachable there and is not ported.
 *      (Its procedure activity 'FormHistory' also filters DocumentTypeId in (1614) while the form never sets DocumentTypeId, so it would list nothing.)</li>
 *  <li>Attachments (Attachment dialog, FormHelper) are not ported; the DMS rows of the document are left untouched (the desktop re-saves them).</li>
 *  <li>The header DocNo / DocDate / WorkOrderId are read from the saved record, not trusted from the browser (the desktop takes them from the unedited
 *      grid columns, which are EditType 0). Detail ids must belong to the open document.</li>
 *  <li>The Update step does not refresh the ItemName / UOM text columns of the detail grid row (the desktop only writes the ids), the same here.</li>
 *  <li>Only the Save right gates the Update button, as btnUpdate.Enabled = DoHaveSaveRight (:241); the printed slip needs the Print right (:242).</li>
 * </ol>
 */
@Service
public class W5AWorkOrderInProgressService {

    public static final String SCREEN = "frmWorkOrderInProgress";
    public static final int DOC_TYPE = 1620;

    private final Prod273Support db;
    private final W3BWorkOrderRepository repo;

    public W5AWorkOrderInProgressService(Prod273Support db, W3BWorkOrderRepository repo) {
        this.db = db;
        this.repo = repo;
    }

    private static Map<String, Object> pair(Object id, Object name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    // ====================================================================================== load

    /** frmSaleContractSchedule_Load:235. */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.putAll(refresh());
        out.put("main", mainGrid());
        return out;
    }

    /** btnRefresh_Click:489 - ItemFill:315 + uomgetAll:356. */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", Prod273Support.project(db.itemsGetAll(u), "Id", "ItemName"));
        out.put("allUoms", db.allUoms(u));
        return out;
    }

    /** FillgrdWorkOrderMain:514 - [Mfg].[USP_GetWorkOrderInProgressData]. */
    public List<Map<String, Object>> mainGrid() {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.rows("[Mfg].[USP_GetWorkOrderInProgressData]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", db.branchId(u),
                "FinancialYearId", db.financialYearId()))) {
            Map<String, Object> o = new LinkedHashMap<>();                       // dtMain columns :255-:272
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", str(ci(r, "DocNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("WorkOrderId", toInt(ci(r, "WorkOrderId")));
            o.put("WorkOrderNo", str(ci(r, "WorkOrderNo")));
            o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BomHeaderNo", str(ci(r, "BomHeaderNo")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("ItemUomCode", str(ci(r, "ItemUomCode")));
            o.put("ItemQty", toDouble(ci(r, "ItemQty")));
            o.put("StartDate", stamp(ci(r, "StartDate")));
            o.put("EndDate", stamp(ci(r, "EndDate")));
            o.put("DeadLine", day(ci(r, "DeadLine")));
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== read

    private void requireMine(UserAccount u, Map<String, Object> h) {
        if (h == null || toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId())
                || toInt(ci(h, "DocumentTypeId")) != DOC_TYPE) {
            throw new IllegalArgumentException("Record Not Found");
        }
    }

    private List<Map<String, Object>> detailRows(int id) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.rows(W3BWorkOrderRepository.P_WIP, params("Id", id, "Activity", "ReadByHeaderId_WorkOrderInProgressDetail"))) {
            Map<String, Object> o = new LinkedHashMap<>();                       // dtdetail columns :279-:293, filled at :1010
            o.put("Id", toInt(ci(r, "WorkOrderInProgressDetailId")));
            o.put("WorkOrderOperationId", toInt(ci(r, "WorkOrderOperationId")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("UOMId", toInt(ci(r, "ItemUomId")));
            o.put("UOM", str(ci(r, "PackUom")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            o.put("WorkStationOperationId", toInt(ci(r, "WorkStationOperationId")));
            o.put("WorkStationName", str(ci(r, "WorkStationName")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("StartDate", stamp(ci(r, "WsOpStartDate")));
            o.put("StartTime", stamp(ci(r, "WsOpStartTime")));
            o.put("CompleteDate", stamp(ci(r, "WsOpCompleteDate")));
            o.put("CompleteTime", stamp(ci(r, "WsOpCompleteTime")));
            o.put("Remarks", str(ci(r, "Remarks")));
            rows.add(o);
        }
        return rows;
    }

    /** ReadById:992 - the document, its detail rows and (when it has details) the refreshed main grid (FillgrdWorkOrderMain). */
    public Map<String, Object> load(int id) {
        UserAccount u = db.user();
        List<Map<String, Object>> h = repo.rows(W3BWorkOrderRepository.P_WIP, params("Id", id, "Activity", "ReadById"));
        requireMine(u, h.isEmpty() ? null : h.get(0));
        List<Map<String, Object>> details = detailRows(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("details", details);
        if (!details.isEmpty()) out.put("main", mainGrid());
        return out;
    }

    // ====================================================================================== save

    private static Timestamp dateValue(Object v) {
        String s = str(v).trim();
        if (s.isEmpty()) return null;
        return s.length() <= 10 ? pickerValue(s) : ts(s);
    }

    /**
     * Insert:871. Body: id, startDate, endDate, remarksHeader (the open row of the main grid), details [{Id, WorkOrderOperationId, ItemId, UOMId, Qty,
     * WorkStationOperationId, StartDate, StartTime, CompleteDate, CompleteTime, Remarks}].
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = db.user();
        int recId = toInt(b.get("id"));
        if (!db.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right on Work Order In Progress.");
        if (recId == 0) throw new IllegalArgumentException("Please Select Any Record first...");                      // btnsave_Click:858
        List<Map<String, Object>> rows = list(b.get("details"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found...\nGrid Should have atleast one record");   // :883

        List<Map<String, Object>> hdr = repo.rows(W3BWorkOrderRepository.P_WIP, params("Id", recId, "Activity", "ReadById"));
        Map<String, Object> h = hdr.isEmpty() ? null : hdr.get(0);
        requireMine(u, h);
        if (toInt(ci(h, "FinancialYearId")) != db.financialYearId()) {
            throw new IllegalArgumentException("You Cannot Update This Record in the active Financial Year because it was entered in another one.");
        }
        List<Integer> owned = new ArrayList<>();
        for (Map<String, Object> d : repo.rows(W3BWorkOrderRepository.P_WIP, params("Id", recId, "Activity", "ReadByHeaderId_WorkOrderInProgressDetail"))) {
            owned.add(toInt(ci(d, "WorkOrderInProgressDetailId")));
        }

        // validate every row before any write (the desktop returns at the first bad row, before WorkOrderInProgress.Save :941-:956)
        int n = 0;
        for (Map<String, Object> r : rows) {
            n++;
            if (toInt(r.get("WorkStationOperationId")) == 0) continue;
            int did = toInt(r.get("Id"));
            if (did > 0 && !owned.contains(did)) throw new IllegalArgumentException("Record Not Found");
            if (dateValue(r.get("StartDate")) == null) throw new IllegalArgumentException("StartDate can not be null in Grid Operations Row No: " + n);
            if (dateValue(r.get("CompleteDate")) == null) throw new IllegalArgumentException("CompleteDate can not be null in Grid Operations Row No: " + n);
        }

        Timestamp stamp = now();
        int branch = db.branchId(u);
        Map<String, Object> p = params("WorkOrderInProgressId", recId, "DocNo", toInt(ci(h, "DocNo")), "DocDate", ci(h, "DocDate"),
                "DocumentTypeId", DOC_TYPE, "WorkOrderId", toInt(ci(h, "WorkOrderId")),
                "StartDate", dateValue(b.get("startDate")), "EndDate", dateValue(b.get("endDate")), "RemarksHeader", str(b.get("remarksHeader")),
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchesId", branch, "ProjectsId", branch,
                "FinancialYearId", db.financialYearId(), "EntryUserId", u.getId(), "EntryDate", stamp, "ModifyUserId", u.getId(), "ModifyDate", stamp,
                "IsApproved", false, "ApprovedUserId", u.getId(), "ApprovedDate", stamp, "ActionId", 2);
        repo.setProc("[Mfg].[USP_WorkOrderInProgress_InsertAndUpdate]", p);

        for (Map<String, Object> r : rows) {
            if (toInt(r.get("WorkStationOperationId")) == 0) continue;                                              // :958
            int did = toInt(r.get("Id"));
            repo.setProc("[Mfg].[USP_WorkOrderInProgressDetail_Insert]", params(
                    "WorkOrderInProgressDetailId", did > 0 ? did : null, "SortNo", 0, "WorkOrderInProgressId", recId,
                    "WorkOrderOperationId", toInt(r.get("WorkOrderOperationId")), "WorkStationOperationId", toInt(r.get("WorkStationOperationId")),
                    "ItemId", toInt(r.get("ItemId")), "ItemUomId", toInt(r.get("UOMId")), "Qty", toDouble(r.get("Qty")),
                    "Remarks", str(r.get("Remarks")),
                    "WsOpStartDate", dateValue(r.get("StartDate")), "WsOpStartTime", dateValue(r.get("StartTime")),
                    "WsOpCompleteDate", dateValue(r.get("CompleteDate")), "WsOpCompleteTime", dateValue(r.get("CompleteTime")),
                    "ActionTypeId", did > 0 ? 2 : 1));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Data Update Successfully....  " + toInt(ci(h, "DocNo")));                              // :986
        out.put("id", recId);
        return out;
    }
}
