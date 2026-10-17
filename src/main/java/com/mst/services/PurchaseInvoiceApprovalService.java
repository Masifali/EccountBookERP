package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseInvoiceApprovalRepository;
import com.mst.repositories.PurchaseOrderForApprovalRepository;
import com.mst.security.CurrentUserContext;
import java.time.LocalDate;
import java.util.*;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class PurchaseInvoiceApprovalService {
    public record Filter(int documentTypeId, int branchId, LocalDate fromDate, LocalDate toDate,
                         int supplierId, int itemId, boolean unApprove) {
        public Filter(int documentTypeId, int branchId, LocalDate fromDate, LocalDate toDate, int supplierId, int itemId) {
            this(documentTypeId, branchId, fromDate, toDate, supplierId, itemId, false);
        }
    }
    public record Approval(Filter filter, List<Integer> ids) { }
    private final PurchaseInvoiceApprovalRepository repo;
    private final PurchaseOrderForApprovalRepository common;
    private final CurrentUserContext context;
    private final AccountReportsDesktopService config;

    public PurchaseInvoiceApprovalService(PurchaseInvoiceApprovalRepository repo,
            PurchaseOrderForApprovalRepository common, CurrentUserContext context, AccountReportsDesktopService config) {
        this.repo = repo; this.common = common; this.context = context; this.config = config;
    }

    private UserAccount authorized(Filter f) {
        if (f == null || f.documentTypeId() != 56) throw new IllegalArgumentException("Unsupported purchase invoice type");
        if (f.toDate() == null || (f.fromDate() != null && f.fromDate().isAfter(f.toDate())))
            throw new IllegalArgumentException("Select a valid From Date and To Date");
        if (f.branchId() < 0 || f.supplierId() < 0 || f.itemId() < 0) throw new IllegalArgumentException("Invalid filter selection");
        var u = context.requireAccountingUser();
        if (!repo.canApprove(u)) throw new AccessDeniedException("You do not have purchase invoice approval permission");
        return u;
    }

    private int branch(UserAccount u, Filter f) {
        if (!config.configBool("PurchaseInvoiceBranchWise")) return 0;
        int branch = f.branchId() > 0 ? f.branchId() : Objects.requireNonNullElse(u.getBranchesId(), 0);
        if (branch <= 0 || !repo.branchAllowed(u, branch)) throw new AccessDeniedException("The selected branch is not allocated to you");
        return branch;
    }

    public Map<String,Object> setup(Filter f) {
        var u = authorized(f); int branch = branch(u, f);
        var suppliers = new ArrayList<Map<String,Object>>(); var items = new ArrayList<Map<String,Object>>();
        for (var r : repo.combos(u, branch)) {
            var list = "Supplier".equals(r.get("Activity")) ? suppliers : "ItemName".equals(r.get("Activity")) ? items : null;
            if (list != null) list.add(Map.of("id", value(r,"Id"), "name", value(r,"ReferenceName")));
        }
        String yearStart = "";
        for (var r : common.activeYears(u.getOrganizationId(), u.getCompanyId())) {
            if (number(r.get("Id")) == context.currentFinancialYearId() && r.get("Start_Period") != null)
                yearStart = r.get("Start_Period").toString().substring(0,10);
        }
        return Map.of("suppliers", suppliers, "items", items, "yearStart", yearStart,
                "fromDate", f.fromDate() == null ? yearStart : f.fromDate().toString(), "toDate", f.toDate().toString());
    }

    public Map<String,Object> history(Filter f) {
        var u = authorized(f);
        var headers = new LinkedHashMap<Integer,Map<String,Object>>();
        var lines = new LinkedHashMap<Integer,List<Map<String,Object>>>();
        for (var r : repo.pending(u, f, branch(u, f))) {
            int id = number(r.get("Id"));
            if (id <= 0 || number(r.get("DocumentTypeId")) != 56 || approved(r) != f.unApprove()) continue;
            headers.computeIfAbsent(id, key -> project(r,
                    "Id,VoucherHeadId,InvoiceNo,InvoiceDate,BranchSrNo,BranchName,ManualBillNo,SupplierName,CommissionAgent,CommType,CommRate,CommAmount,HeaderQty,HeaderNetWeight,AvgRate,BillAmount,RemarksHeader,EntryDate,UserName,NoOfAttachments"));
            lines.computeIfAbsent(id, key -> new ArrayList<>()).add(project(r,
                    "ItemId,PurchaseOrderId,OrderNo,WareHouseName,ItemName,PackTypeDesc,CropYear,JobLotDescription,GpNo,VehicleNo,ItemQty,PackUom,GrossWeight,EBTotalWt,WeightCutTotal,NetWeight,NetStockWeight,ItemRate,RateUom,RateCut,RateCutAmount,ItemAmount,CommissionAmount,ExpenseAmount,FreightAmount,LabPurchaseId,LabAnalisysNo,InvLabSampleLogRegisterId,SampleNo"));
        }
        return Map.of("rows", new ArrayList<>(headers.values()), "lines", lines);
    }

    @Transactional(rollbackFor = Exception.class, timeout = 30)
    public Map<String,Object> approve(Approval request) {
        return changeApproval(request, false);
    }

    @Transactional(rollbackFor = Exception.class, timeout = 30)
    public Map<String,Object> unapprove(Approval request) {
        return changeApproval(request, true);
    }

    private Map<String,Object> changeApproval(Approval request, boolean unApprove) {
        if (request == null) throw new IllegalArgumentException("Select invoices first");
        var f = request.filter(); var u = authorized(f); int branch = branch(u, f);
        if (f.unApprove() != unApprove) throw new IllegalArgumentException("The approval action does not match this list. Reopen the dashboard");
        var requested = request.ids();
        if (requested == null || requested.isEmpty() || requested.size() > 100
                || requested.stream().anyMatch(id -> id == null || id <= 0)
                || new HashSet<>(requested).size() != requested.size())
            throw new IllegalArgumentException("Select between 1 and 100 valid invoices");
        var ids = requested.stream().sorted().toList();
        // Lock before re-reading the list; concurrent edits or status changes cannot slip through.
        for (int id : ids) {
            var locked = repo.lockInvoice(u, id);
            if (locked.size() != 1) throw new AccessDeniedException("Invoice not found in your company");
            var row = locked.get(0);
            if (branch > 0 && number(row.get("BranchesId")) != branch)
                throw new AccessDeniedException("Invoice is outside the selected branch");
            if (approved(row) != unApprove) throw new IllegalArgumentException(unApprove
                    ? "An invoice is no longer approved. Refresh the list"
                    : "An invoice was already approved. Refresh the list");
        }
        var visible = new HashSet<Integer>();
        for (var r : repo.pending(u, f, branch)) {
            if (number(r.get("DocumentTypeId")) == 56 && approved(r) == unApprove) visible.add(number(r.get("Id")));
        }
        if (!visible.containsAll(ids)) throw new AccessDeniedException("An invoice is no longer in this list. Refresh and try again");
        for (int id : ids) {
            if (unApprove) repo.unapprove(u, id); else repo.approve(u, id);
        }
        String message = unApprove ? "Approval undone for " : "Approved ";
        return Map.of("message", message + ids.size() + " invoice(s) successfully", "count", ids.size());
    }

    private static Map<String,Object> project(Map<String,Object> row, String columns) {
        var out = new LinkedHashMap<String,Object>();
        for (var c : columns.split(",")) out.put(c, value(row,c));
        return out;
    }
    private static Object value(Map<String,Object> r, String key) { return Objects.requireNonNullElse(r.get(key), ""); }
    private static int number(Object v) { return v instanceof Number n ? n.intValue() : 0; }
    private static boolean approved(Map<String,Object> r) { return Boolean.TRUE.equals(r.get("IsApproved")) || "1".equals(String.valueOf(r.get("IsApproved"))); }
}
