package com.mst.services.cmagt;

import com.mst.models.UserAccount;
import com.mst.repositories.cmagt.CommissionAgentOrder159Repository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/** BLL contract and form rules for frmCommissionAgentOrder, screen 470 / document 159. */
@Service
public class CommissionAgentOrder159Service {
    public static final int SCREEN_ID=470, DOCUMENT_TYPE_ID=159;
    private final CommissionAgentOrder159Repository repo;
    private final CommissionDropdownService dropdowns;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public CommissionAgentOrder159Service(CommissionAgentOrder159Repository repo, CommissionDropdownService dropdowns,
                                          CurrentUserContext context, DesktopReportRights rights) {
        this.repo=repo;this.dropdowns=dropdowns;this.context=context;this.rights=rights;
    }

    public Map<String,Object> initialize(){UserAccount u=user("View");Map<String,Object> r=new LinkedHashMap<>();
        r.put("today",LocalDate.now().toString());r.put("parties",dropdowns.parties(true));r.put("paymentTerms",dropdowns.paymentTerms());
        r.put("deliveryTerms",dropdowns.deliveryTerms());r.put("items",dropdowns.items(0));r.put("cropYears",dropdowns.cropYears());
        r.put("packingTypes",dropdowns.packingTypes());r.put("cities",dropdowns.cities());
        r.put("permissions",Map.of("save",allowed(u,"Save"),"update",allowed(u,"Update"),"print",allowed(u,"Print"),
                "gridPrint",allowed(u,"Grid Print"),"gridExport",allowed(u,"Grid Export"),"canViewAllRecords",allowed(u,"CanView AllRecord")));
        r.put("documentTypeId",DOCUMENT_TYPE_ID);return r;}

    public int generateCode(){UserAccount u=user("View");List<Map<String,Object>> rows=repo.call("[dbo].[Sp_InvCommAgentOrder_ReadAll]",map(
            "OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"Activity","GenerateCode"));
        return rows.isEmpty()?0:integer(col(rows.get(0),"DocNo"));}

    public List<Map<String,Object>> history(){UserAccount u=user("View");return repo.call("[dbo].[Sp_InvCommAgentOrder_ReadAll]",map(
            "OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"Activity","FormHistory"));}

    public List<Map<String,Object>> itemUoms(int itemId){user("View");return itemId<=0?List.of():dropdowns.itemUoms(itemId);}

    public Map<String,Object> record(int id){user("View");if(id<=0)throw new IllegalArgumentException("Select an order first.");
        List<Map<String,Object>> header=repo.call("[dbo].[Sp_InvCommAgentOrder_ReadAll]",map("Id",id,"Activity","ReadById"));
        if(header.isEmpty())throw new IllegalArgumentException("The selected commission agent order was not found.");
        assertOwned(context.requireAccountingUser(),header.get(0));
        List<Map<String,Object>> details=repo.call("[dbo].[Sp_InvCommAgentOrder_ReadAll]",map("Id",id,"Activity","ReadDetailByHeaderId"));
        Map<String,Object> result=new LinkedHashMap<>();result.put("header",header.get(0));result.put("details",details);return result;}

    @Transactional
    public Map<String,Object> save(Map<String,Object> body){
        int id=integer(body.get("id"));UserAccount u=user(id>0?"Update":"Save");
        if(id>0){List<Map<String,Object>> existing=repo.call("[dbo].[Sp_InvCommAgentOrder_ReadAll]",map("Id",id,"Activity","ReadById"));
            if(existing.isEmpty())throw new IllegalArgumentException("The selected commission agent order was not found.");assertOwned(u,existing.get(0));}
        int docNo=integer(body.get("docNo"));if(docNo<=0)throw new IllegalArgumentException("DocNo Field is Required");
        int supplier=positive(body,"supplierId","Supplier Field is Required"),customer=positive(body,"customerId","Customer Field is Required"),agent=positive(body,"commissionAgentId","Comm Agent Field is Required");
        BigDecimal comm=decimalRequired(body.get("commPrcnt"),"Comm% Field Required");int term=positive(body,"invDueTermsId","Payment Term Field is Required");
        required(body.get("dueDays"),"Due Days Field Required");String delivery=text(body.get("deliveryTerm"));if(delivery.isBlank())throw new IllegalArgumentException("Delivery Term Field is Required");
        Object raw=body.get("details");if(!(raw instanceof List<?> rows)||rows.isEmpty())throw new IllegalArgumentException("Grid Record Not Found");
        Timestamp now=new Timestamp(System.currentTimeMillis());
        LinkedHashMap<String,Object> h=map("Id",id==0?null:id,"DocNo",docNo,"DocDate",sqlDate(body.get("docDate"),"Doc Date"),
                "SupplierId",supplier,"CustomerId",customer,"CommissionAgentId",agent,"BrokerId",integer(body.get("brokerId")),
                "CommPrcnt",comm,"BrokeryPrcnt",decimal(body.get("brokeryPrcnt")),"InvDueTermsId",term,"DueDays",integer(body.get("dueDays")),
                "DeliveryTerm",delivery,"DeliveryStartDate",sqlDate(body.get("deliveryStartDate"),"Delivery Start Date"),
                "OrderStatus",text(body.getOrDefault("orderStatus","Open")),"DeliveryDays",integer(body.get("deliveryDays")),
                "RemarksHeader",text(body.get("remarksHeader")),"OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),
                "BranchesId",0,"ProjectsId",0,"EnteryUserId",u.getId(),"EnteryDate",now,
                "ModifyUserId",u.getId(),"ModifyDate",now,"IsApproved",false,"ApprovalUserId",null,"ApprovedDate",null,"DocumentTypeId",DOCUMENT_TYPE_ID);
        int savedId=integer(repo.saveHeader(id==0?"[dbo].[Sp_InvCommAgentOrder_Insert]":"[dbo].[Sp_InvCommAgentOrder_Update]",h,id==0));
        int sort=0;
        for(Object item:rows){if(!(item instanceof Map<?,?> d))continue;validateDetail(d,++sort);
            LinkedHashMap<String,Object> x=detail(d,savedId,integer(d.get("actionId"))==0?(integer(d.get("id"))>0?2:1):integer(d.get("actionId")));
            repo.saveDetail(x);
        }
        Object removed=body.get("removedDetails");if(removed instanceof List<?> removedRows)for(Object item:removedRows)if(item instanceof Map<?,?> d)repo.saveDetail(detail(d,savedId,3));
        return Map.of("success",true,"id",savedId,"docNo",docNo,"message",id==0?"Data Save Successfully.... ":"Data Update Successfully.... ");
    }

    public Map<String,Object> printPermission(){UserAccount u=user("View");rights.require(u,SCREEN_ID,"Print");return Map.of("screenId",SCREEN_ID,"documentTypeId",DOCUMENT_TYPE_ID);}
    private LinkedHashMap<String,Object> detail(Map<?,?> d,int headerId,int action){return map("Id",integer(d.get("id"))==0?null:integer(d.get("id")),"InvCommAgentOrderId",headerId,
            "ItemId",integer(d.get("itemId")),"Crop",integer(d.get("cropYearId")),"PackingTypeId",integer(d.get("packingTypeId")),"PackUomId",integer(d.get("packUomId")),
            "Qty",decimal(d.get("qty")),"Weight",decimal(d.get("weight")),"SupplierRate",decimal(d.get("supplierRate")),"CustomerRate",decimal(d.get("customerRate")),
            "Amount",decimal(d.get("supplierAmount")),"CityAreaId",integer(d.get("cityId")),"RemarksDetail",text(d.get("remarksDetail")),"ActionId",action,"CustomerAmount",decimal(d.get("customerAmount")));}
    private void validateDetail(Map<?,?> d,int row){for(String f:new String[]{"itemId","cropYearId","packingTypeId","packUomId"})if(integer(d.get(f))<=0)throw new IllegalArgumentException(label(f)+" Field Required");
        for(String f:new String[]{"qty","weight","supplierRate","customerRate"})required(d.get(f),label(f)+" Field Required");}
    private String label(String key){return switch(key){case "itemId"->"Item";case "cropYearId"->"Crop Year";case "packingTypeId"->"Pack Type";case "packUomId"->"Pack Uom";case "qty"->"Quantity";case "weight"->"Weight";case "supplierRate"->"Supplier Rate";default->"Customer Rate";};}
    private UserAccount user(String action){UserAccount u=context.requireAccountingUser();rights.require(u,SCREEN_ID,action);return u;}
    private static void assertOwned(UserAccount u,Map<String,Object> h){if(integer(col(h,"OrganizationId"))!=u.getOrganizationId()||integer(col(h,"CompanyId"))!=u.getCompanyId()||integer(col(h,"DocumentTypeId"))!=DOCUMENT_TYPE_ID)throw new IllegalArgumentException("The selected commission agent order was not found.");}
    private boolean allowed(UserAccount u,String a){try{rights.require(u,SCREEN_ID,a);return true;}catch(AccessDeniedException e){return false;}}
    private static LinkedHashMap<String,Object> map(Object... kv){LinkedHashMap<String,Object> m=new LinkedHashMap<>();for(int i=0;i<kv.length;i+=2)m.put(String.valueOf(kv[i]),kv[i+1]);return m;}
    private static Object col(Map<String,Object> m,String k){if(m.containsKey(k))return m.get(k);return m.entrySet().stream().filter(e->e.getKey().equalsIgnoreCase(k)).map(Map.Entry::getValue).findFirst().orElse(null);}
    private static int positive(Map<String,Object> m,String k,String msg){int v=integer(m.get(k));if(v<=0)throw new IllegalArgumentException(msg);return v;}
    private static BigDecimal decimal(Object v){try{return new BigDecimal(v==null||String.valueOf(v).isBlank()?"0":String.valueOf(v));}catch(Exception e){return BigDecimal.ZERO;}}
    private static BigDecimal decimalRequired(Object v,String msg){required(v,msg);return decimal(v);}
    private static void required(Object v,String msg){if(v==null||String.valueOf(v).isBlank())throw new IllegalArgumentException(msg);}
    private static int integer(Object v){if(v instanceof Number n)return n.intValue();try{return v==null?0:Integer.parseInt(String.valueOf(v));}catch(Exception e){return 0;}}
    private static String text(Object v){return v==null?"":String.valueOf(v).trim();}
    private static Date sqlDate(Object v,String label){String s=text(v);if(s.isBlank())return Date.valueOf(LocalDate.now());try{return Date.valueOf(s);}catch(Exception e){throw new IllegalArgumentException(label+" is invalid.");}}
}
