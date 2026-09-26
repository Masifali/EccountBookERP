package com.mst.services;

import com.mst.models.dto.PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;

/** PurchsaeOrder.cs:2598-2615 and 3562-3570. */
public final class PurchaseOrderPaymentRules {
    private PurchaseOrderPaymentRules() {}
    public static void recalculate(List<PurchaseOrderPaymentTermsDetailDto> rows, BigDecimal total,
                                   boolean byPercent, boolean saving) {
        boolean singleSavedRow=saving && rows.size()==1 && rows.get(0).getAmount()!=null && rows.get(0).getAmount()>0;
        if(total.signum()<=0 || (!byPercent && !singleSavedRow))return;
        for(var row:rows){
            var percent=BigDecimal.valueOf(row.getPrcntOfTotal()==null?0:row.getPrcntOfTotal());
            row.setAmount(total.multiply(percent).divide(BigDecimal.valueOf(100)).setScale(4,RoundingMode.HALF_EVEN).doubleValue());
        }
    }
}
