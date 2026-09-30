package com.mst.services;

import com.mst.models.*;
import com.mst.repositories.*;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.*;
import com.mst.reports.ReportRenderer;
import java.time.LocalDate;
import java.util.*;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class PackingMaterialReportsService {
    private static final Map<String,Integer> SCREENS=Map.of("grn-register",697,"requirement-planning-detail",699,"purchase-order-register",778,"inventory-transaction-report",695,"stock-with-supplier",696);
    private static final Map<String,String> ACTIONS=Map.of("Complete","CanChangeOrderStatusToComplete","Open","CanChangeOrderStatusToOpen","Cancel","CanChangeOrderStatusToCancel","UpdateExpiryDate","CanChangeOrderExpiryDate");
    private final PackingMaterialReportsRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final SaleReportApprovalRepository approvals;
    private final ICompanyRepository companies;
    private final ReportRenderer renderer;
    public PackingMaterialReportsService(PackingMaterialReportsRepository repo,CurrentUserContext context,DesktopReportRights rights,SaleReportApprovalRepository approvals,ICompanyRepository companies,ReportRenderer renderer) {
        this.repo=repo;this.context=context;this.rights=rights;this.approvals=approvals;this.companies=companies;this.renderer=renderer;
    }
    @org.springframework.beans.factory.annotation.Autowired
    private com.mst.reports.prints.ReportPdfService jasperPdf;
    public UserAccount require(String report,String action) {
        Integer screen=SCREENS.get(report);if(screen==null)throw new IllegalArgumentException("Unknown Packing Material report");
        var u=context.requireAccountingUser();rights.require(u,screen,action);return u;
    }
    public Map<String,Object> initial(String report) {
        var u=require(report,"View");var out=new LinkedHashMap<String,Object>();
        var branches=hasBranches(report)?repo.branches(u,report):List.<Map<String,Object>>of();
        out.put("branches",branches);out.put("branchId",u.getBranchesId()==null?0:u.getBranchesId());
        String chosen=branches.stream().anyMatch(r->integer(r,"BranchId")==integer(u.getBranchesId()))?String.valueOf(u.getBranchesId()):"";
        out.put("choices",lookups(u,report,chosen,branches));
        LocalDate today=LocalDate.now(),start=today;
        for(var year:repo.years(u))if(integer(year,"Id")==context.currentFinancialYearId()) {
            Object value=year.get("Start_Period");if(value!=null)start=LocalDate.parse(value.toString().substring(0,10));
        }
        out.put("yearStart",start.toString());out.put("today",today.toString());
        out.put("fromDate",report.equals("requirement-planning-detail")?today.toString():report.equals("inventory-transaction-report")?today.minusDays(7).toString():start.toString());
        out.put("permissions",report.equals("purchase-order-register")?repo.actionRights(u):Set.of());
        out.put("approvalDetail",report.equals("purchase-order-register")&&repo.approvalRight(u));
        out.put("printAvailable",renderer.available());
        return out;
    }
    public Map<String,Object> lookups(String report,String selected) {
        var u=require(report,"View");return lookups(u,report,selected,hasBranches(report)?repo.branches(u,report):List.of());
    }
    private Map<String,Object> lookups(UserAccount u,String report,String selected,List<Map<String,Object>> allowed) {
        var out=new LinkedHashMap<String,Object>();
        if(report.equals("stock-with-supplier")) {
            out.put("Supplier",options(repo.stockSuppliers(u),"Id","CompanyName"));
            out.put("Item",options(repo.stockItems(u),"Id","ItemName"));
            out.put("BagType",options(repo.staticColumns("PurchaseOrderEmptyBagsType"),"Id","type"));
            out.put("ItemCondition",options(repo.staticColumns("EmptyBagsCondition"),"Id","type"));return out;
        }
        if(!hasBranches(report))return out;
        String ids=branches(selected,allowed,false);if(ids.isEmpty())return out;
        for(var row:repo.choices(u,report,ids)) {
            boolean trans=report.equals("inventory-transaction-report");
            if(trans&&!"7,8".contains(Objects.toString(row.get("ParentCategoryId"),"")))continue;
            String group=Objects.toString(row.get(trans?"ActivityType":"Activity"),"");
            @SuppressWarnings("unchecked") var list=(List<Map<String,Object>>)out.computeIfAbsent(group,k->new ArrayList<Map<String,Object>>());
            int id=integer(row,"Id");if(list.stream().noneMatch(r->integer(r,"id")==id))list.add(Map.of("id",id,"text",Objects.toString(row.get(trans?"name":"ReferenceName"),"")));
        }
        return out;
    }
    private static List<Map<String,Object>> options(List<Map<String,Object>> rows,String id,String label) {
        return rows.stream().map(r->Map.<String,Object>of("id",integer(r,id),"text",Objects.toString(r.get(label),""))).toList();
    }
    public List<Map<String,Object>> data(String report,PackingReportFilter f) {
        var u=require(report,"View");f.validate();
        if(report.equals("purchase-order-register")) {
            if(!Set.of("Open","Complete","Cancel").contains(Objects.toString(f.status(),"")))throw new IllegalArgumentException("Select order status");
            if(!Set.of("Approved","NotApproved","All").contains(Objects.toString(f.approval(),"")))throw new IllegalArgumentException("Select approval status");
        }
        if(report.equals("stock-with-supplier")&&!Set.of("ledger","trial").contains(Objects.toString(f.mode(),"ledger")))throw new IllegalArgumentException("Select Ledger or Trial");
        String ids=hasBranches(report)?branches(f.branches(),repo.branches(u,report),true):"";
        return repo.rows(u,report,f,ids);
    }
    public record Action(PackingReportFilter filter,int id,String action,LocalDate expiryDate) {}
    public record Document(PackingReportFilter filter,int id,int documentTypeId) {}
    public record TransactionDocument(PackingReportFilter filter,Map<String,Object> row) {}
    public List<Map<String,Object>> transactionDocuments(TransactionDocument request) {
        if(request==null||request.filter()==null||request.row()==null)throw new IllegalArgumentException("Select a report row");
        var u=require("inventory-transaction-report","View");
        // Match a fresh authorized row by its visible identity, not a volatile row index.
        var keys=List.of("ItemId","DocDate","DocCodeNo","DocumentTypeDescription","WareHouseName","RackName","BranchName","UOMCode","ItemConditionId");
        var row=data("inventory-transaction-report",request.filter()).stream().filter(r->keys.stream().allMatch(k->
            identityValue(k,r.get(k)).equals(identityValue(k,request.row().get(k))))).findFirst()
            .orElseThrow(()->new AccessDeniedException("This row is no longer available. Press Show again"));
        String ids=branches(request.filter().branches(),repo.branches(u,"inventory-transaction-report"),true);
        var documents=repo.transactionDocuments(u,row,ids);
        if(documents.isEmpty())throw new IllegalArgumentException("No original document is available for this row");
        return documents.stream().map(d->Map.<String,Object>of("url",documentPath(integer(d,"DocumentTypeId"))+"?id="+integer(d,"Id"),
            "code",Objects.toString(d.get("Code"),""),"type",Objects.toString(d.get("Type"),""))).toList();
    }
    private static String identityValue(String key,Object value) {
        String text=Objects.toString(value,"");
        return key.equals("DocDate")&&text.length()>=10?text.substring(0,10):text;
    }
    public Map<String,Object> document(Document request) {
        if(request==null||request.filter()==null||request.id()<=0)throw new IllegalArgumentException("Select a document");
        if(data("stock-with-supplier",request.filter()).stream().noneMatch(r->integer(r,"Id")==request.id()&&integer(r,"DocumentTypeId")==request.documentTypeId()))
            throw new AccessDeniedException("The document is not available in this report");
        return Map.of("url",documentPath(request.documentTypeId())+"?id="+request.id());
    }
    private static String documentPath(int type) {
        return switch(type) {
            case 39 -> "/store/opening-stock-store";
            case 46 -> "/purchase/goods-receipt-notes";
            case 143 -> "/purchase/grn-sale-return";
            case 48 -> "/store/grn-store";
            case 57 -> "/purchase/purchase-invoice-direct";
            case 701 -> "/packing-material/grn";
            case 125 -> "/store/party-to-party-pm-transfer";
            case 451 -> "/store/store-issuance";
            case 452 -> "/store/store-issuance-direct";
            case 68 -> "/store/stock-transfer";
            case 70 -> "/store/stock-adjustment";
            case 215 -> "/packing-material/stock-adjustment";
            case 700 -> "/packing-material/purchase-order";
            case 702 -> "/packing-material/purchase-invoice";
            case 807 -> "/packing-material/stock-transfer-store";
            default -> throw new IllegalArgumentException("No original form is registered for document type "+type);
        };
    }
    public Map<String,Object> action(Action request) {
        if(request==null||request.filter()==null||!ACTIONS.containsKey(Objects.toString(request.action(),"")))throw new IllegalArgumentException("Select a valid order action");
        var u=require("purchase-order-register","View");
        if(!repo.actionRights(u).contains(ACTIONS.get(request.action())))throw new AccessDeniedException("This order action is not allowed for your user");
        var f=request.filter();
        if(!"Approved".equals(f.approval()) || (request.action().equals("Open")?"Open".equals(f.status()):!"Open".equals(f.status())))throw new IllegalArgumentException("This action is not available for the selected status");
        var selected=data("purchase-order-register",f).stream().filter(r->integer(r,"Id")==request.id()).toList();
        if(selected.isEmpty())throw new AccessDeniedException("The order is no longer available in this report. Press Show again");
        if(request.action().equals("Cancel")&&selected.stream().anyMatch(r->decimal(r.get("ReceivedQty"))>0))throw new IllegalArgumentException("The order cannot be cancelled because received quantity is greater than zero");
        if(request.action().equals("UpdateExpiryDate")&&request.expiryDate()==null)throw new IllegalArgumentException("Enter the new expiry date");
        repo.orderAction(u,request.id(),request.action(),request.action().equals("UpdateExpiryDate")?request.expiryDate():null);
        return Map.of("message","Order updated successfully");
    }
    public List<Map<String,Object>> approvalHistory(Action request) {
        var u=require("purchase-order-register","View");if(!repo.approvalRight(u))throw new AccessDeniedException("Approval detail is not allowed for this user");
        if(request==null||request.filter()==null||data("purchase-order-register",request.filter()).stream().noneMatch(r->integer(r,"Id")==request.id()))throw new AccessDeniedException("Select an order in this report");
        return approvals.history(700,request.id());
    }
    public byte[] print(String report,PackingReportFilter f,String format) throws Exception {
        var u=require(report,"Print");var rows=data(report,f);if(rows.isEmpty())throw new IllegalArgumentException("No records found for display");
        String template=switch(report) {
            case "grn-register"->"341-GrnPackingMaterialRegister.rpt";
            case "requirement-planning-detail"->"671_01_PackingMaterialRequirementPlanning_Detail.rpt";
            case "purchase-order-register"->"alternate".equals(format)?"357_01-PurchaseOrderRegister_PM.rpt":"357-PurchaseOrderRegister_PM.rpt";
            case "inventory-transaction-report"->"stock".equals(format)?"403-InvStockRptInventoryTransactionsStocks -Store.rpt":"402-InvStockRptInventoryTransactionsA-Store.rpt";
            case "stock-with-supplier"->"trial".equals(f.mode())?"405-RptEBGLBySupplier.rpt":"404-RptEBGLBySupplierandItem.rpt";
            default->throw new IllegalArgumentException("Unknown report");
        };
        var company=companies.findById(u.getCompanyId()).orElseThrow();String prefix=report.equals("stock-with-supplier")?"":"@";
        var params=new LinkedHashMap<String,Object>();params.put(prefix+"CompanyName",Objects.toString(company.getCompName(),""));params.put(prefix+"CompanyAddress",Objects.toString(company.getCompAddress(),""));
        if(report.equals("purchase-order-register"))params.put("@PrintedBy",u.getUserName());
        return jasperPdf.rowsPdf(template,rows,params); /* Jasper (converted .rpt template or generated) - no Crystal runtime */
    }
    public static boolean hasBranches(String report) {return Set.of("grn-register","purchase-order-register","inventory-transaction-report").contains(report);}
    static String branches(String selected,List<Map<String,Object>> allowed,boolean required) {
        var ids=new LinkedHashSet<Integer>();if(selected!=null&&!selected.isBlank())for(String part:selected.split(","))if(!part.isBlank()) {
            int id;try{id=Integer.parseInt(part.trim());}catch(NumberFormatException e){throw new IllegalArgumentException("Select valid branches");}
            if(id<=0||allowed.stream().noneMatch(row->integer(row,"BranchId")==id))throw new AccessDeniedException("The selected branch is not allocated to this user for this report");ids.add(id);
        }
        if(required&&ids.isEmpty())throw new IllegalArgumentException(allowed.isEmpty()?"No Packing Material records are available in your allocated branches":"Select Branch First");
        return ids.isEmpty()?"":","+String.join(",",ids.stream().map(String::valueOf).toList());
    }
    private static int integer(Map<String,Object> row,String key) {return integer(row.get(key));}
    private static int integer(Object value) {return value instanceof Number n?n.intValue():value==null?0:Integer.parseInt(value.toString());}
    private static double decimal(Object value) {return value instanceof Number n?n.doubleValue():0;}
}
