package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.*;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.ConnectionCallback;
import org.springframework.stereotype.Repository;

@Repository
public class SaleGdnPurchaseReturnRepository {
    public static final int DOCUMENT_TYPE_ID=703;
    protected final JdbcTemplate jdbc;
    private final Map<String,List<ProcParam>> procedureParameters=new ConcurrentHashMap<>();
    public SaleGdnPurchaseReturnRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    protected int documentTypeId(){return DOCUMENT_TYPE_ID;}
    protected String screenName(){return "frmGdnForPurchaseReturn";}
    protected String recordLabel(){return "GDN Purchase Return";}
    protected List<Map<String,Object>> q(String sql,Object...args){return jdbc.queryForList(sql,args);}
    public Map<String,Object> initial(UserAccount u,int year){
        Map<String,Object> m=new LinkedHashMap<>();
        var no=q("EXEC dbo.Sp_InvGdn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@Activity='GenerateInvGdnCode'",u.getOrganizationId(),u.getCompanyId(),documentTypeId(),year,u.getBranchesId());
        m.put("nextNo",no.isEmpty()?1:no.get(0).get("DocNo"));
        m.put("suppliers",q("EXEC dbo.USP_PurchaseInvoice_GetVendorsAndCustomers @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?",u.getOrganizationId(),u.getCompanyId(),"56,57,702,64"));
        m.put("gatePasses",q("EXEC dbo.USP_GatePassOutward_PendingForGdnPurchaseReturn @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@BranchesId=?",u.getOrganizationId(),u.getCompanyId(),year,u.getBranchesId()));
        m.put("warehouses",q("EXEC dbo.USP_GetWarehousesAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()));
        m.put("crops",q("EXEC dbo.Sp_InvCropYear_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId()));
        m.put("jobLots",q("EXEC dbo.USP_GetJobLotsAllocatedToBranch @OrganizationId=?,@CompanyId=?,@BranchId=?",u.getOrganizationId(),u.getCompanyId(),u.getBranchesId()));
        m.put("packingTypes",q("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"));
        m.put("otherItems",q("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='ReadAll',@organizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()));
        m.put("cities",q("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'",u.getOrganizationId(),u.getCompanyId()));
        m.put("transporters",q("EXEC dbo.USP_Accounts_GetAccountTitleByAccountTypeIds @OrganizationId=?,@CompanyId=?,@AppId=?,@UserId=?,@AccountTypeIds=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),u.getId(),"6,8"));
        m.put("feature5",q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()).stream().anyMatch(r->number(r.get("Id"))==5));
        return m;
    }
    public List<Map<String,Object>> items(UserAccount u,int supplier){return q("EXEC dbo.USP_GetItemsFromPurchaseInvoiceStoreAgainstPartyId @OrganizationId=?,@CompanyId=?,@SupplierCustomerId=?,@DocumentTypeIds=?",u.getOrganizationId(),u.getCompanyId(),supplier,"56,57,64,702");}
    public List<Map<String,Object>> uoms(int itemId){return q("EXEC dbo.Sp_UOMSchedule_GetAllMethod @ItemId=?,@Activity='ReadByItemID'",itemId);}
    public Map<String,Object> record(UserAccount u,int id){var h=q("EXEC dbo.Sp_InvGdn_GetAllMethod @Id=?,@Activity='GetById'",id);if(h.isEmpty()||number(h.get(0).get("OrganizationId"))!=u.getOrganizationId()||number(h.get(0).get("CompanyId"))!=u.getCompanyId()||number(h.get(0).get("DocumentTypeId"))!=documentTypeId())throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND,recordLabel()+" not found");var out=new LinkedHashMap<>(h.get(0));out.put("details",q("EXEC dbo.Sp_InvGdn_GetAllMethod @InvGdnMainId=?,@Activity='GetGDNDetailByGdnId'",id));out.put("expenses",q("EXEC dbo.Sp_InvGdn_GetAllMethod @Id=?,@Activity='GetInvGdnExpensesByHeaderId'",id));return out;}
    public List<Map<String,Object>> history(UserAccount u,int year){return q("EXEC dbo.Sp_InvGdn_GetAllMethod @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@FinancialYearId=?,@BranchesId=?,@CanViewAllRecord=1,@EntryUser=?,@Activity='GDNFormHistory'",u.getOrganizationId(),u.getCompanyId(),documentTypeId(),year,u.getBranchesId(),u.getId());}
    public int save(UserAccount u,int year,Map<String,Object> request){
        int requestedId=number(request.get("id"));
        Map<String,Object> old=requestedId>0?record(u,requestedId):Map.of();
        Map<String,Object> h=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        h.putAll(request);h.put("Id",requestedId);h.put("DocumentTypeId",documentTypeId());h.put("OrganizationId",u.getOrganizationId());h.put("CompanyId",u.getCompanyId());h.put("BranchesId",u.getBranchesId());h.put("ProjectsId",u.getBranchesId());h.put("FinancialYearId",year);h.put("EntryUser",requestedId>0?number(old.get("EntryUser")):u.getId());h.put("ModifyUser",u.getId());h.put("EntryDate",requestedId>0?old.get("EntryDate"):new Timestamp(System.currentTimeMillis()));h.put("ModifyDate",new Timestamp(System.currentTimeMillis()));h.put("IsApproved",false);h.put("ScreenName",screenName());h.put("ActionId",1);h.put("AutoUpdateId",0);h.put("IsStockReserved",false);
        int id=call(requestedId>0?"Sp_InvGdn_Update":"Sp_InvGdn_Insert",h,true);if(id<=0)id=requestedId;if(id<=0)throw new IllegalStateException("GDN identity was not returned");
        @SuppressWarnings("unchecked") List<Map<String,Object>> details=(List<Map<String,Object>>)request.getOrDefault("details",List.of());
        int line=0;for(Map<String,Object> source:details){Map<String,Object>d=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);d.putAll(source);d.put("InvGdnId",id);d.put("LineId",++line);d.put("GpDate",request.get("GPDate"));d.put("GpNo",request.get("GpNo"));d.put("VehicleNo",request.get("VehicleNo"));int detailId=call("Sp_InvGdnDetail_Insert",d,true);source.put("LineId",line);source.put("InvGdnId",id);if(detailId>0)source.put("Id",detailId);}
        @SuppressWarnings("unchecked") List<Map<String,Object>> expenses=(List<Map<String,Object>>)request.getOrDefault("expenses",List.of());
        for(Map<String,Object> source:expenses){Map<String,Object>x=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);x.putAll(source);x.put("InvGdnId",id);call("Sp_InvGdnExpense_Insert",x,true);}
        call("Sp_InventoryTransactions_GetALLMethod",Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"RefDocumentTypeId",documentTypeId(),"RefDocIdNo",id),false);
        boolean hasReferences=details.stream().filter(d->number(d.get("ActionTypeId"))!=3).allMatch(d->number(d.get("RefDocumentTypeId"))>0&&number(d.get("RefDocIdNo"))>0&&number(d.get("RefDocSubIdNo"))>0);
        boolean feature5=q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",u.getOrganizationId(),u.getCompanyId()).stream().anyMatch(r->number(r.get("Id"))==5);
        if(hasReferences)call("usp_StockEvalautionInsert_FromGdn",Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"RefDocumentTypeId",documentTypeId(),"RefDocIdNo",id),false);
        else if(!feature5)call("Sp_InventoryStockEvalautionDetail_Update",Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"RefDocumentTypeId",documentTypeId(),"RefDocIdNo",id),false);
        else fifo(u,id,requestedId>0,request,details);
        for(Map<String,Object>d:details)if(number(d.get("ActionTypeId"))!=3){Map<String,Object>x=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);x.putAll(d);x.put("OrganizationId",u.getOrganizationId());x.put("CompanyId",u.getCompanyId());x.put("DocumentTypeId",documentTypeId());x.put("DocDate",request.get("DocDate"));x.put("NetWeight",d.get("StockWeight"));x.put("InvPackingTypeId",d.get("PackingTypeId"));x.put("PackUomId",d.get("ItemUomId"));x.put("RefDocNoId",d.get("RefDocIdNo"));call("USP_InventoryValidation",x,false);}
        call("usp_StockInTransitUpdate_VoucherInsertFromGdnOrForwarding",Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"DocumentTypeId",documentTypeId(),"Id",id),false);
        return id;
    }
    private void fifo(UserAccount u,int id,boolean update,Map<String,Object> header,List<Map<String,Object>> details){
        if(update)call("USP_InventoryQtyReverseAndDeleteByReferenceId",Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"RefDocumentTypeId",documentTypeId(),"RefDocIdNo",id),false);
        Map<String,double[]> reserved=new HashMap<>();
        for(Map<String,Object>d:details){if(number(d.get("ActionTypeId"))==3)continue;List<Map<String,Object>> stocks=q("EXEC dbo.USP_GetStockByFifoMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@DocDate=?,@PackUomId=?,@WarehouseId=?,@CropYearId=?,@JobLotId=?,@PackingTypeId=?,@CropYear=?,@DocumentTypeId=?,@Id=?",u.getOrganizationId(),u.getCompanyId(),number(d.get("ItemId")),header.get("DocDate"),number(d.get("ItemUomId")),number(d.get("WarehouseId")),number(d.get("CropYearId")),number(d.get("JobLotId")),number(d.get("PackingTypeId")),text(d.get("CropYear")),documentTypeId(),id);
            double needWeight=decimal(d.get("StockWeight")),needQty=decimal(d.get("ItemQty")),available=0;for(Map<String,Object>stock:stocks){String key=fifoKey(stock);double used=reserved.getOrDefault(key,new double[2])[1];available+=Math.max(0,decimal(stock.get("NetBalWeight"))-used);}if(needWeight>Math.round(available*100d)/100d)throw new IllegalStateException("Weight available is "+available+" and row Weight is "+needWeight+" this item "+text(d.get("Item"))+" against FIFO");
            double usedWeight=0,usedQty=0;for(Map<String,Object>stock:stocks){if(Math.abs(needWeight-usedWeight)<0.000001)break;String key=fifoKey(stock);double[] prior=reserved.computeIfAbsent(key,k->new double[2]);double stockQty=Math.max(0,decimal(stock.get("NetBalQty"))-prior[0]),stockWeight=Math.max(0,decimal(stock.get("NetBalWeight"))-prior[1]);if(stockWeight<=0)continue;double takeWeight=Math.min(stockWeight,needWeight-usedWeight);double takeQty=takeWeight==stockWeight?stockQty:needQty-usedQty;int rateUom=number(stock.get("RateUomId"));double avgRate=decimal(stock.get("AvgRate"));if(avgRate<=0)throw new IllegalStateException("Rate Not Found this Item "+text(d.get("Item"))+" against FIFO Method");if(rateUom<=0)throw new IllegalStateException("RateUomId not found this "+text(d.get("Item"))+" against FIFO Method");var equivalents=q("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@ItemId=?,@ScheduleId=?,@Activity='GetEqvilentByItemIdAndUomScheduleId'",u.getOrganizationId(),u.getCompanyId(),number(d.get("ItemId")),rateUom);double equivalent=equivalents.isEmpty()?0:decimal(equivalents.get(0).get("Equivalent"));if(equivalent==0)throw new IllegalStateException("RateUom Not Found");
                Map<String,Object>x=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);x.put("OrganizationId",u.getOrganizationId());x.put("CompanyId",u.getCompanyId());x.put("RefDocumentTypeId",documentTypeId());x.put("RefDocIdNo",id);x.put("DocCodeNo",number(header.get("DocNo")));x.put("SupplierCustomerId",number(header.get("SupplierCustomerId")));x.put("BranchesId",u.getBranchesId());x.put("ProjectsId",u.getBranchesId());x.put("EntryUser",u.getId());x.put("ModifyUser",u.getId());x.put("OtherDocumentTypeId",documentTypeId());x.put("OtherDocNoId",id);x.put("DocDate",header.get("DocDate"));x.put("VehicleNo",header.get("VehicleNo"));x.put("GpNoDcNo",header.get("GpNo"));x.put("BiltyNo",header.get("BiltyNo"));x.put("OtherSubDocNoId",number(d.get("Id")));x.put("LineId",number(d.get("LineId")));x.put("ItemId",d.get("ItemId"));x.put("WarehouseId",d.get("WarehouseId"));x.put("JobLotId",d.get("JobLotId"));x.put("InvPackingTypeId",d.get("PackingTypeId"));x.put("ItemUom",d.get("ItemUomId"));x.put("CropBatch",d.get("CropYear"));x.put("CityId",d.get("CityId"));x.put("RefRefDocumentTypeId",stock.get("RefDocumentTypeId"));x.put("RefRefDocIdNo",stock.get("RefDocIdNo"));x.put("RefRefDocSubIdNo",stock.get("RefDocSubIdNo"));x.put("QtyOut",takeQty);x.put("BillWeightOut",takeWeight);x.put("StockWeightOut",takeWeight);x.put("CgsRate",avgRate*equivalent);x.put("CgsAmount",takeWeight/equivalent*(avgRate*equivalent));x.put("RateUom",rateUom);x.put("CalcType","Weight");call("USP_InventoryStockEvalautionDetail_Insert",x,true);prior[0]+=takeQty;prior[1]+=takeWeight;usedQty+=takeQty;usedWeight+=takeWeight;
            }
        }
    }
    private String fifoKey(Map<String,Object> r){return number(r.get("RefDocumentTypeId"))+":"+number(r.get("RefDocIdNo"))+":"+number(r.get("RefDocSubIdNo"));}
    public void delete(UserAccount u,int id){record(u,id);call("Sp_InvoicesVouchersandStocksDelete",Map.of("OrganizationId",u.getOrganizationId(),"CompanyId",u.getCompanyId(),"Id",id,"DocumentTypeId",documentTypeId(),"UserId",u.getId()),false);}
    private int call(String procedure,Map<String,Object> values,boolean scalar){
        List<ProcParam> params=procedureParameters.computeIfAbsent(procedure,this::loadParams);String marks=String.join(",",Collections.nCopies(params.size(),"?"));
        return jdbc.execute((ConnectionCallback<Integer>)c->{try(CallableStatement st=c.prepareCall("{call dbo."+procedure+"("+marks+")}")){for(int i=0;i<params.size();i++){ProcParam p=params.get(i);Object value=get(values,p.name.substring(1));if(value==null)value=defaultValue(p.type);if(value instanceof String text&&(p.type==Types.DATE||p.type==Types.TIMESTAMP))value=text.isBlank()?null:(p.type==Types.DATE?java.sql.Date.valueOf(text.substring(0,10)):Timestamp.valueOf(text.length()==10?text+" 00:00:00":text.replace('T',' ')));st.setObject(i+1,value,p.type);}boolean result=st.execute();int found=0;while(true){if(result){try(ResultSet rs=st.getResultSet()){if(rs.next()){Object first=rs.getObject(1);if(scalar&&first instanceof Number)found=((Number)first).intValue();}}}else if(st.getUpdateCount()==-1)break;result=st.getMoreResults();}return found;}});
    }
    private Object get(Map<String,Object> values,String key){for(var e:values.entrySet())if(e.getKey().equalsIgnoreCase(key))return e.getValue();return null;}
    private double decimal(Object o){if(o instanceof Number)return((Number)o).doubleValue();try{return Double.parseDouble(String.valueOf(o));}catch(Exception ignored){return 0;}}
    private String text(Object o){return o==null?"":String.valueOf(o);}
    private Object defaultValue(int type){return switch(type){case Types.BIT,Types.BOOLEAN->false;case Types.TINYINT,Types.SMALLINT,Types.INTEGER->0;case Types.BIGINT->0L;case Types.FLOAT,Types.REAL,Types.DOUBLE,Types.NUMERIC,Types.DECIMAL->0d;default->null;};}
    private List<ProcParam> loadParams(String procedure){return q("SELECT p.name,t.system_type_id FROM sys.parameters p JOIN sys.types t ON p.user_type_id=t.user_type_id WHERE p.object_id=OBJECT_ID(?) ORDER BY p.parameter_id",procedure).stream().map(r->new ProcParam(String.valueOf(r.get("name")),jdbcType(number(r.get("system_type_id"))))).toList();}
    private int jdbcType(int sql){return switch(sql){case 40->Types.DATE;case 42,43,58,61->Types.TIMESTAMP;case 104->Types.BIT;case 48->Types.TINYINT;case 52->Types.SMALLINT;case 56->Types.INTEGER;case 127->Types.BIGINT;case 59->Types.REAL;case 62->Types.DOUBLE;case 106,108->Types.DECIMAL;case 165->Types.VARBINARY;default->Types.NVARCHAR;};}
    private record ProcParam(String name,int type){}
    protected static int number(Object o){return o instanceof Number?((Number)o).intValue():0;}
}
