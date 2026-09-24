package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleGdnRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
public class SaleGdnService {
    private final SaleGdnRepository repo; private final CurrentUserContext context;
    public SaleGdnService(SaleGdnRepository repo,CurrentUserContext context){this.repo=repo;this.context=context;}
    private UserAccount user(){return context.requireAccountingUser();}
    public Map<String,Object> initial(){return repo.initial(user(),context.currentFinancialYearId());}
    public List<Map<String,Object>> uoms(int item){if(item<=0)throw new IllegalArgumentException("Select Item first");return repo.uoms(context.requireAccountingUser(),item);}
    public List<Map<String,Object>> advanceOrders(int customer,int gdn){if(customer<=0)return List.of();return repo.advanceOrders(user(),context.currentFinancialYearId(),customer,gdn);}
    public List<Map<String,Object>> deliveryOrder(int customer,int id){if(customer<=0||id<=0)return List.of();return repo.deliveryOrder(user(),customer,id);}
    public List<Map<String,Object>> gatePassCustomers(int id){if(id<=0)return List.of();return repo.gatePassCustomers(user(),id);}
    public List<Map<String,Object>> expenses(String ids){return ids==null||ids.isBlank()?List.of():repo.expenses(ids.replaceAll("[^0-9,]",""));}
    public Map<String,Object> record(int id){return repo.record(user(),id);}
    public List<Map<String,Object>> history(){return repo.history(user(),context.currentFinancialYearId());}
    @Transactional public Map<String,Object> save(Map<String,Object> r){validate(r);int id=repo.save(user(),context.currentFinancialYearId(),r);return repo.record(user(),id);}
    @Transactional public void delete(int id){if(id<=0)throw new IllegalArgumentException("Record not found");repo.delete(user(),id);}
    private void validate(Map<String,Object> r){
        if(number(r.get("SupplierCustomerId"))<=0)throw new IllegalArgumentException("Customer Name is required");
        if(r.get("DocDate")==null||String.valueOf(r.get("DocDate")).isBlank())throw new IllegalArgumentException("Document Date is required");
        @SuppressWarnings("unchecked") List<Map<String,Object>> rows=(List<Map<String,Object>>)r.getOrDefault("details",List.of());
        var active=rows.stream().filter(x->number(x.get("ActionTypeId"))!=3).toList();if(active.isEmpty())throw new IllegalArgumentException("Grid Record Not Found");
        double gross=0;int line=0;for(var x:active){line++;required(x,"OrderId","Order No",line);required(x,"WarehouseId","Warehouse",line);required(x,"ItemId","Item",line);required(x,"CropYearId","Crop Year",line);required(x,"JobLotId","Job/Lot",line);required(x,"PackingTypeId","Packing Type",line);required(x,"ItemUomId","UOM",line);required(x,"CityId","City/Area",line);amount(x,"ItemQty","Qty",line);amount(x,"GrossWeight","Gross Weight",line);amount(x,"NetBillWeight","Net Bill Weight",line);amount(x,"StockWeight","Stock Weight",line);gross+=decimal(x.get("GrossWeight"));}
        if(decimal(r.get("FactoryWeight"))!=0&&Math.abs(gross-decimal(r.get("FactoryWeight")))>.01)throw new IllegalArgumentException("Mismatch in weights: Gross weight entered is "+gross+", but it should equal factory weight "+decimal(r.get("FactoryWeight")));
        if(decimal(r.get("CarriageAmount"))>0&&number(r.get("TransporterId"))<=0)throw new IllegalArgumentException("Transporter Account field required");
    }
    private void required(Map<String,Object>x,String k,String label,int row){if(number(x.get(k))<=0)throw new IllegalArgumentException(label+" required in detail row "+row);}
    private void amount(Map<String,Object>x,String k,String label,int row){if(decimal(x.get(k))==0)throw new IllegalArgumentException(label+" required in detail row "+row);}
    private int number(Object o){return o instanceof Number?((Number)o).intValue():0;}
    private double decimal(Object o){return o instanceof Number?((Number)o).doubleValue():0;}
}
