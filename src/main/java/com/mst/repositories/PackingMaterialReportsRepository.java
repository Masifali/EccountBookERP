package com.mst.repositories;

import com.mst.models.PackingReportFilter;
import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Exact recovered BLL contracts. No alternative tables/queries are substituted on SQL failure. */
@Repository
public class PackingMaterialReportsRepository {
    private final JdbcTemplate jdbc;
    public PackingMaterialReportsRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public static Map<String,Object> tenant(UserAccount u) {
        return DesktopProc.params("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId());
    }
    public List<Map<String,Object>> call(String proc, Map<String,Object> parameters) { return DesktopProc.rows(jdbc,proc,parameters); }
    public List<Map<String,Object>> branches(UserAccount u,String report) {
        var p=tenant(u);p.put("UserId",u.getId());
        String proc="USP_GetBranchsAllocatedToUser";
        if (report.equals("grn-register")) {proc="USP_GetBranchsAllocatedToUserFromGrn";p.put("DocumentTypeId",701);}
        if (report.equals("purchase-order-register")) {proc="USP_GetBranchsAllocatedToUserFromPurchaseOrder";p.put("DocumentTypeId",700);}
        return call(proc,p);
    }
    public List<Map<String,Object>> choices(UserAccount u,String report,String branches) {
        var p=tenant(u);text(p,"BranchesIds",branches);
        if(report.equals("inventory-transaction-report")) {
            p.put("InventoryParentCategory","7,8");
            return call("Sp_Inventory_InventoryTransactions_DropDownAndLists",p);
        }
        boolean grn=report.equals("grn-register");p.put("DocumentTypeIds",grn?"701":"700");
        return call(grn?"USP_GetDataForDropDownFromGrn":"USP_GetDataForDropDownFromPurchaseOrder",p);
    }
    public List<Map<String,Object>> stockSuppliers(UserAccount u) {
        var p=tenant(u);p.put("Activity","ReadByOrganizationIdCompanyIdForBinding");return call("Sp_SupplierCustomer_GetAllMethod",p);
    }
    public List<Map<String,Object>> stockItems(UserAccount u) {
        var p=tenant(u);p.put("LookupTypeIds","14");p.put("Activity","GetItemByItemTypeId");return call("Sp_Item_GetAllMethod",p);
    }
    public List<Map<String,Object>> staticColumns(String activity) {return call("SpStaticColumnNames",DesktopProc.params("Activity",activity));}
    public List<Map<String,Object>> years(UserAccount u) {return call("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",tenant(u));}
    public List<Map<String,Object>> rows(UserAccount u,String report,PackingReportFilter f,String branches) {
        return call(procedure(report,f.mode()),parameters(u,report,f,branches));
    }
    /** Resolve the underlying document identity; a printed document number is not a primary key. */
    public List<Map<String,Object>> transactionDocuments(UserAccount u,Map<String,Object> row,String branches) {
        return jdbc.queryForList("""
            SELECT DISTINCT it.RefDocIdNo AS Id,it.RefDocumentTypeId AS DocumentTypeId,
                   it.DocCodeNo AS Code,d.DocumentTypeDescription AS Type
            FROM dbo.InventoryTransactions it
            JOIN dbo.DocumentType d ON d.Id=it.RefDocumentTypeId
            LEFT JOIN dbo.InvWareHouse wh ON wh.Id=it.WarehouseId
            LEFT JOIN dbo.invWarehouseRack rack ON rack.Id=it.RackId
            LEFT JOIN dbo.Branches branch ON branch.Id=it.BranchesId
            LEFT JOIN dbo.V_UomScheduleAndUom uom ON uom.Id=it.ItemUom AND uom.CompanyId=it.CompanyId
            WHERE it.OrganizationId=? AND it.CompanyId=? AND it.ItemId=?
              AND EXISTS(SELECT 1 FROM dbo.fnSplitString(?,',') b WHERE b.data=it.BranchesId)
              AND CAST(it.DocDate AS date)=? AND it.DocCodeNo=?
              AND d.DocumentTypeDescription=?
              AND COALESCE(wh.WareHouseName,'')=? AND COALESCE(rack.RackName,'')=?
              AND COALESCE(branch.BranchName,'')=? AND COALESCE(uom.UOMCode,'')=?
              AND COALESCE(it.ItemConditionId,0)=? AND it.RefDocIdNo>0
            ORDER BY it.RefDocumentTypeId,it.RefDocIdNo
            """,u.getOrganizationId(),u.getCompanyId(),row.get("ItemId"),branches,
            Date.valueOf(row.get("DocDate").toString().substring(0,10)),row.get("DocCodeNo"),row.get("DocumentTypeDescription"),
            Objects.toString(row.get("WareHouseName"),""),Objects.toString(row.get("RackName"),""),
            Objects.toString(row.get("BranchName"),""),Objects.toString(row.get("UOMCode"),""),
            row.get("ItemConditionId")==null?0:row.get("ItemConditionId"));
    }
    public static String procedure(String report,String mode) {
        return switch(report) {
            case "grn-register" -> "USp_InvGrnStore_Register";
            case "requirement-planning-detail" -> "usp_PackingMaterialRequirementPlanning";
            case "purchase-order-register" -> "Sp_PurchaseOrder_PackingMaterial_Rpt";
            case "inventory-transaction-report" -> "Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt";
            case "stock-with-supplier" -> "trial".equals(mode)?"Sp_InventoryEBBalancesByPartyAndItem_Rpt":"Sp_InventoryEBGLBySupplierandItem_Rpt";
            default -> throw new IllegalArgumentException("Unknown Packing Material report");
        };
    }
    public static Map<String,Object> parameters(UserAccount u,String report,PackingReportFilter f,String branches) {
        var p=tenant(u);
        switch(report) {
            case "grn-register":
                p.put("DocumentTypeId",701);dates(p,f,"GrnDateF","GrnDateT");
                number(p,"SupplierCustomerId",f.supplierId());number(p,"ItemId",f.itemId());number(p,"WarehouseId",f.warehouseId());
                number(p,"GrnNoF",f.fromNo());number(p,"GrnNoT",f.toNo());number(p,"GpSrNoF",f.gpFrom());number(p,"GpSrNoT",f.gpTo());
                number(p,"PoNoFrom",f.orderFrom());number(p,"PoNoTo",f.orderTo());text(p,"AreaCity",f.city());text(p,"BranchesIds",branches);break;
            case "requirement-planning-detail":
                dates(p,f,"LoadingDate","LoadingToDate");number(p,"ItemId",f.itemId());p.put("Activity","Detail");break;
            case "purchase-order-register":
                p.put("DocumentTypeId",700);dates(p,f,"StartOrderDate","EndOrderDate");
                number(p,"SupplierCustomerId",f.supplierId());number(p,"ItemId",f.itemId());number(p,"PoSrFrom",f.fromNo());number(p,"PoSrTo",f.toNo());
                if(!"All".equals(f.approval()))p.put("IsApproved",!"NotApproved".equals(f.approval()));
                text(p,"Status",f.status());p.put("PostState",0);
                // PurchaseOrderRegister_PM BLL deliberately does not forward BranchesIds.
                break;
            case "inventory-transaction-report":
                dates(p,f,"DateFrom","DateTo");number(p,"ItemId",f.itemId());number(p,"WarehouseId",f.warehouseId());
                number(p,"DocumentTypeId",f.documentTypeId());number(p,"ItemTypeId",f.itemTypeId());
                number(p,"ItemConditionId",f.conditionId());number(p,"RackId",f.rackId());text(p,"BranchesIds",branches);break;
            case "stock-with-supplier":
                p.put("FromDate",Date.valueOf(f.fromDate()));
                // StocksReport.InventoryEBTrialBalancesByPartyAndItem only adds ToDate when
                // CheckDateTimeNull is true. Preserve its omission for valid dates.
                if(!"trial".equals(f.mode()))p.put("ToDate",Date.valueOf(f.toDate()));
                number(p,"SupplierCustomerId",f.supplierId());number(p,"ItemId",f.itemId());
                number(p,"TypeId",f.itemTypeId());number(p,"BagsConditionId",f.conditionId());break;
            default:throw new IllegalArgumentException("Unknown Packing Material report");
        }
        return p;
    }
    public Set<String> actionRights(UserAccount u) {
        return new LinkedHashSet<>(jdbc.queryForList("SELECT DISTINCT r.RightName FROM dbo.ScreenRights r JOIN dbo.tblUserRights g ON g.RightId=r.Id AND g.ScreenId=r.ScreenID WHERE r.ScreenID=778 AND g.CompanyId=? AND g.UserId=? AND g.Value=1 AND EXISTS(SELECT 1 FROM dbo.CompanyRights c WHERE c.ScreenId=r.ScreenID AND c.CompanyId=g.CompanyId AND c.IsActive=1)",String.class,u.getCompanyId(),u.getId()));
    }
    public boolean approvalRight(UserAccount u) {
        var p=tenant(u);p.put("UserId",u.getId());p.put("DocumentTypeId",700);
        var rows=call("USP_MultiApprovalDocumentRightCheckagainstUser",p);
        if(rows.isEmpty()||rows.get(0).isEmpty())return false;
        return Set.of("true","1").contains(String.valueOf(rows.get(0).values().iterator().next()).toLowerCase());
    }
    public void orderAction(UserAccount u,int id,String action,LocalDate expiry) {
        var p=tenant(u);p.put("Id",id);p.put("PostUser",u.getId());p.put("ReqType",action.equals("Complete")?"Status":action);
        if(expiry!=null)p.put("OrderExpiryDate",Date.valueOf(expiry));
        p.put("Activity","UpdateStatusandIsapprovedByOrderId");call("Sp_PurchaseOrder_GetAllMethod",p);
    }
    private static void dates(Map<String,Object> p,PackingReportFilter f,String from,String to) {p.put(from,Date.valueOf(f.fromDate()));p.put(to,Date.valueOf(f.toDate()));}
    private static void number(Map<String,Object> p,String key,Integer value) {if(value!=null&&value!=0)p.put(key,value);}
    private static void text(Map<String,Object> p,String key,String value) {if(value!=null&&!value.isBlank())p.put(key,value);}
}
