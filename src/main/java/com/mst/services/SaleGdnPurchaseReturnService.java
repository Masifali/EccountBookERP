package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleGdnPurchaseReturnRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.beans.factory.annotation.Qualifier;

@Service
public class SaleGdnPurchaseReturnService {
    private final SaleGdnPurchaseReturnRepository repo;
    private final CurrentUserContext context;
    public SaleGdnPurchaseReturnService(@Qualifier("saleGdnPurchaseReturnRepository") SaleGdnPurchaseReturnRepository repo,CurrentUserContext context){this.repo=repo;this.context=context;}
    private UserAccount user(){return context.requireAccountingUser();}
    public Map<String,Object> initial(){return repo.initial(user(),context.currentFinancialYearId());}
    public List<Map<String,Object>> items(int supplier){if(supplier<=0)throw new IllegalArgumentException("Select Supplier first");return repo.items(user(),supplier);}
    public List<Map<String,Object>> uoms(int item){if(item<=0)throw new IllegalArgumentException("Select Item first");return repo.uoms(item);}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    public List<Map<String,Object>> history(){return repo.history(user(),context.currentFinancialYearId());}
    @Transactional public Map<String,Object> save(Map<String,Object> request){validate(request);int id=repo.save(user(),context.currentFinancialYearId(),request);return repo.record(user(),id);}
    @Transactional public void delete(int id){if(id<=0)throw new IllegalArgumentException("Record not found");repo.delete(user(),id);}
    private void validate(Map<String,Object> r){
        if(number(r.get("SupplierCustomerId"))<=0)throw new IllegalArgumentException("Supplier Name is required");
        Object date=r.get("DocDate");if(date==null||String.valueOf(date).isBlank())throw new IllegalArgumentException("Document Date is required");
        @SuppressWarnings("unchecked") List<Map<String,Object>> rows=(List<Map<String,Object>>)r.getOrDefault("details",List.of());
        List<Map<String,Object>> active=rows.stream().filter(x->number(x.get("ActionTypeId"))!=3).toList();if(active.isEmpty())throw new IllegalArgumentException("Grid Record Not Found");
        double gross=0;int line=0;for(Map<String,Object>x:active){line++;required(x,"WarehouseId","Warehouse",line);required(x,"ItemId","Item",line);required(x,"CropYearId","CropYear",line);required(x,"JobLotId","JobLot",line);required(x,"PackingTypeId","PackingType",line);required(x,"ItemUomId","Uom",line);requiredAmount(x,"ItemQty","Qty",line);requiredAmount(x,"GrossWeight","GrossWeight",line);requiredAmount(x,"NetBillWeight","NetBillWeight",line);requiredAmount(x,"StockWeight","StockWeight",line);gross+=decimal(x.get("GrossWeight"));}
        if(Math.round(gross*100)!=Math.round(decimal(r.get("FactoryWeight"))*100))throw new IllegalArgumentException("Mismatch in weights: Gross weight entered is "+gross+", but it should equal factory weight "+decimal(r.get("FactoryWeight")));
        boolean anyRef=active.stream().anyMatch(x->number(x.get("RefDocumentTypeId"))>0||number(x.get("RefDocIdNo"))>0||number(x.get("RefDocSubIdNo"))>0);boolean allRef=active.stream().allMatch(x->number(x.get("RefDocumentTypeId"))>0&&number(x.get("RefDocIdNo"))>0&&number(x.get("RefDocSubIdNo"))>0);if(anyRef&&!allRef)throw new IllegalArgumentException("All detail rows must either be loaded from a reference or all entered without a reference");
        if(decimal(r.get("CarriageAmount"))>0&&number(r.get("TransporterId"))<=0)throw new IllegalArgumentException("Transporter Account field required");
    }
    private void required(Map<String,Object>r,String k,String label,int line){if(number(r.get(k))<=0)throw new IllegalArgumentException(label+" required in detail row "+line);}
    private void requiredAmount(Map<String,Object>r,String k,String label,int line){if(decimal(r.get(k))==0)throw new IllegalArgumentException(label+" required in detail row "+line);}
    private int number(Object o){return o instanceof Number?((Number)o).intValue():0;}
    private double decimal(Object o){return o instanceof Number?((Number)o).doubleValue():0;}
}
