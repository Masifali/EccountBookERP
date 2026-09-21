package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PayablesForecastRequest;
import com.mst.repositories.PayablesForecastRepository;
import com.mst.security.CurrentUserContext;
import java.math.BigDecimal;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class PayablesForecastService {
    private final PayablesForecastRepository repository;private final CurrentUserContext context;
    public PayablesForecastService(PayablesForecastRepository repository,CurrentUserContext context){this.repository=repository;this.context=context;}
    public Map<String,Object> load(PayablesForecastRequest request){
        if(request.getFromDate()==null||request.getIntervalDays()<0||request.getSortNo()<0||request.getToDate()!=null&&request.getToDate().isBefore(request.getFromDate()))throw new IllegalArgumentException("Invalid forecast dates or interval");
        UserAccount user=context.requireAccountingUser();
        List<List<Map<String,Object>>> sets=repository.load(user,request);
        if(sets.size()<2)throw new IllegalStateException("Forecast procedure must return daily and interval results");
        List<Map<String,Object>> daily=sets.get(0),intervals=sets.get(1);
        Map<String,Object> totals=new LinkedHashMap<>();
        if(!daily.isEmpty()){
            Map<String,Object> first=daily.get(0);
            for(String field:List.of("NetReceivables","ReceivablesOverDue","ReceivablesNotYetDue","NetPayables","PayablesOverDue","PayablesNotYetDue")){
                BigDecimal amount=decimal(first.get(field));
                if(!field.equals("NetReceivables")&&!field.equals("ReceivablesOverDue"))amount=amount.negate();
                totals.put(field,amount.toPlainString());
            }
        }
        for(Map<String,Object> row:daily)negate(row,"PurchasePayable","ImportPayable","TotalPayables","RunningTotalPayables");
        for(Map<String,Object> row:intervals)negate(row,"PurchasePayable","ImportPayable","TotalPayables","ComulativePayables");
        return Map.of("daily",daily,"intervals",intervals,"totals",totals,"amountDecimals",repository.amountDecimals(user));
    }
    private static BigDecimal decimal(Object value){return value==null?BigDecimal.ZERO:new BigDecimal(value.toString());}
    private static void negate(Map<String,Object> row,String... fields){for(String field:fields)row.put(field,decimal(row.get(field)).negate().toPlainString());}
}
