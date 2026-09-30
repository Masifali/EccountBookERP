package com.mst.services.partyprocessing;

import com.mst.models.UserAccount;
import com.mst.models.partyprocessing.PpCModels.JobOrder;
import com.mst.repositories.partyprocessing.PpCRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;
import static com.mst.services.partyprocessing.PpCSupport.*;

/**
 * 676 Production Job Order (Party Processing) - Architecture.WinApp.PartyProcessing.ProductionJobOrderPartyProcessing.cs,
 * BLL 0304 / DAL 0307 InvProductionJobOrderPartyProcessing (DocumentTypeId 120).
 *
 * Load: SetRightsValueInRightsObject (btnsave / BtnSaveAs = Save, Print = Print, btnUpdate = Update), WareHouseFill,
 * ItemFill (ParentIds "6"), StockPartyFill, GeneratePlanCode, PlantFeader, ProductionTypeBind, StatusBind (fixed),
 * DefaultDaysToLessFromHistoryFromDate, HistoryComboFill. Save / Update / SaveAs = Insert(); Delete = DeleteByID.
 * The page runs the form's field checks before its confirm; the service re-runs them in the same order.
 * Attachments (DMS) are not ported: the save sends empty attachment lists, as the desktop does when none are added.
 */
@Service
public class PpCJobOrderService {

    public static final int SCREEN = 676;
    public static final int DOC_TYPE = 120;

    @Autowired private PpCRepository repo;
    @Autowired private PpCSupport pp;

    public Map<String, Object> setup() {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> out = lookups(u);
        out.put("rights", pp.rights(u, SCREEN));
        out.put("planCode", repo.jobOrderCode(u, pp.year()));
        out.put("productionTypes", project(repo.productionTypes(), "Id", "ProductionTypeDescription"));
        out.put("defaultDays", pp.defaultDays(u));
        out.putAll(historyCombos(u));
        return out;
    }

    /** btnRefresh_Click: WareHouseFill, ItemFill, StockPartyFill, PlantFeader. */
    public Map<String, Object> lookups(UserAccount u) {
        return m("warehouses", project(repo.warehouses(u), "Id", "WareHouseName"),
                "items", project(repo.itemsForPartyProcessing(u, "6"), "Id", "ItemName", "ItemCode"),
                "stockParties", project(repo.stockParties(u), "Id", "CompanyName"),
                "plants", project(repo.plants(u), "Id", "Description"));
    }

    public Map<String, Object> refresh() { return lookups(pp.user(SCREEN)); }

    public Map<String, Object> code() { UserAccount u = pp.user(SCREEN); return m("planCode", repo.jobOrderCode(u, pp.year())); }

    /** HistoryComboFill: rows with Activity "StockParty" / "JobOrderNo". */
    public Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> parties = new ArrayList<>(), jobs = new ArrayList<>();
        for (Map<String, Object> r : repo.jobOrderDropDown(u)) {
            String a = str(r.get("Activity"));
            if ("StockParty".equals(a)) parties.add(m("Id", r.get("Id"), "ReferenceName", r.get("ReferenceName")));
            if ("JobOrderNo".equals(a)) jobs.add(m("Id", r.get("Id"), "ReferenceName", r.get("ReferenceName")));
        }
        return m("historyParties", parties, "historyJobOrders", jobs);
    }

    public Map<String, Object> historyCombosApi() { return historyCombos(pp.user(SCREEN)); }

    /** ReadById: InvProductionJobOrderPartyProcessing.ReadById (only a job order of the user's own company). */
    public Map<String, Object> read(int id) {
        UserAccount u = pp.user(SCREEN);
        if (id == 0) throw invalid("Id not Found");
        List<Map<String, Object>> r = repo.jobOrder(id);
        if (r.isEmpty() || !PpCScreens.owns(u, r.get(0))) throw invalid("Record  not Found");
        Map<String, Object> h = r.get(0);
        return m("Id", h.get("Id"), "PlanDate", h.get("PlanDate"), "PlanCode", str(h.get("PlanCode")), "RefInvoiceNo", str(h.get("RefInvoiceNo")),
                "StockPartyId", toInt(h.get("StockPartyId")), "WipWareHouseId", toInt(h.get("WipWareHouseId")), "WipItemId", toInt(h.get("WipItemId")),
                "LotReference", str(h.get("LotReference")), "InvProductionPlantId", toInt(h.get("InvProductionPlantId")),
                "ProductionType", str(h.get("ProductionType")), "OtherInstructions", str(h.get("OtherInstructions")),
                "StartDate", h.get("StartDate"), "EndDate", h.get("EndDate"), "PlanStatus", str(h.get("PlanStatus")));
    }

    /** Insert(): FormValidation in the form's order, then the model exactly as the form fills it and BLL Save. */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        int recId = Math.max(0, toInt(b.get("id")));
        pp.require(u, SCREEN, recId > 0 ? "Update" : "Save");
        String planCode = trim(b.get("planCode"));
        if (planCode.isEmpty() || planCode.equals("0")) throw invalid("Plan Code Field Required");
        String jobOrderNo = str(b.get("jobOrderNo"));
        if (jobOrderNo.isEmpty() || jobOrderNo.equals("0")) throw invalid("Mannual ReportNo Field Required");
        if (toInt(b.get("stockPartyId")) == 0) throw invalid("StockParty Field Required");
        if (toInt(b.get("wipItemId")) == 0) throw invalid("WIPItem Field Required");
        if (toInt(b.get("plantId")) == 0) throw invalid("Plan/Feader Field Required");
        if (toInt(b.get("productionTypeId")) == 0) throw invalid("Production Type Field Required");
        if (toInt(b.get("statusId")) == 0) throw invalid("Status Field Required");
        if (recId > 0) {
            List<Map<String, Object>> r = repo.jobOrder(recId);
            if (r.isEmpty() || !PpCScreens.owns(u, r.get(0))) throw invalid("Record Not Update Because Record Not Found");
        }
        LocalDateTime now = LocalDateTime.now();
        JobOrder o = new JobOrder();
        o.Id = recId;
        o.OrganizationId = u.getOrganizationId();
        o.CompanyId = u.getCompanyId();
        o.BranchesId = branch(u);
        o.ProjectsId = branch(u);
        o.FinancialYearId = pp.year();
        o.DocumentTypeId = DOC_TYPE;
        o.EntryDate = now;
        o.EntryUser = uid(u);
        o.ModifyUser = uid(u);
        o.ModifyDate = now;
        o.ApprovedUserId = uid(u);
        o.ApprovedDate = now;
        o.PlanCode = toInt(planCode);
        o.PlanDate = orNow(picker(b.get("planDate")));
        o.RefInvoiceNo = jobOrderNo;
        o.StockPartyId = toInt(b.get("stockPartyId"));
        o.WipItemId = toInt(b.get("wipItemId"));
        o.WipWareHouseId = toInt(b.get("wareHouseId"));
        o.LotReference = str(b.get("lotReference"));
        o.InvProductionPlantId = toInt(b.get("plantId"));
        o.ProductionType = str(b.get("productionTypeText"));
        o.PlanType = "Party";
        o.StartDate = orNow(picker(b.get("startDate")));
        o.EndDate = orNow(picker(b.get("endDate")));
        o.PlanStatus = str(b.get("statusText"));
        o.OtherInstructions = str(b.get("otherInstructions"));
        o.WorkInProccessAcId = 0;
        o.FinishGoodsAcId = 0;
        o.ByProductacId = 0;
        o.ScreenName = "ProductionJobOrderPartyProcessing";
        o.IsApproved = false;
        /* BLL Save: EntryDate / ModifyDate = now again, Insert or Update by Id (no input / output rows from this form). */
        String proc = o.Id == 0 ? "Sp_InvProductionJobOrderPartyProcessing_Insert" : "Sp_InvProductionJobOrderPartyProcessing_Update";
        int id = repo.tx(() -> {
            int n = repo.set(proc, o);
            return n > 0 ? n : o.Id;
        });
        return saved(id, (recId == 0 ? "Save Successfully" : "Update Successfully") + o.PlanCode);
    }

    /** BtnDelete_Click: DeleteByID(UserAccount.ID, RECID). The desktop checks no right here. */
    public Map<String, Object> delete(int id) {
        UserAccount u = pp.user(SCREEN);
        if (id == 0) throw invalid("Record Id Not Found");
        List<Map<String, Object>> r = repo.jobOrder(id);
        if (r.isEmpty() || !PpCScreens.owns(u, r.get(0))) throw invalid("Record Id Not Found");
        repo.jobOrderDelete(uid(u), id);
        return saved(id, "Delete Record Successfully");
    }

    /** BindHistoryGrid: FormHistory with the checked radio's date pair, doc-no range, @StockPartyId, @Id. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = pp.user(SCREEN);
        Map<String, Object> p = pp.history(u, SCREEN, b, new Object[]{"OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", pp.year(), "DocumentTypeId", DOC_TYPE}, "FromDate", "ToDate", "DocNoFrom", "DocNoTo");
        int party = toInt(b.get("stockPartyId")), jo = toInt(b.get("jobOrderId"));
        if (party != 0) p.put("StockPartyId", party);
        if (jo != 0) p.put("Id", jo);
        p.put("Activity", "FormHistory");
        return project(repo.jobOrderHistory(p), "Id", "PlanDate", "PlanCode", "DocumentTypeId", "PlanType", "ProductionType", "JobOrderNo=RefInvoiceNo",
                "LotReference", "OtherInstructions", "StartDate", "EndDate", "PlanStatus", "StockPartyId", "StockParty", "WipWareHouseId", "WareHouseName",
                "WipItemId", "WipItem=ItemName", "ApprovalStatus", "EntryDate", "EntryUserName", "ModifyDate", "ModifyUserName", "ApprovedDate",
                "ApprovedUserName", "NoOfAttachments");
    }

}
