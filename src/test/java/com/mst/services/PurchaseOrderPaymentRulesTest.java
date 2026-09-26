package com.mst.services;
import com.mst.models.dto.PurchaseOrderFullDto.PurchaseOrderPaymentTermsDetailDto;
import java.math.BigDecimal;
import java.util.List;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class PurchaseOrderPaymentRulesTest {
    private PurchaseOrderPaymentTermsDetailDto row(double percent,double amount){var r=new PurchaseOrderPaymentTermsDetailDto();r.setPrcntOfTotal(percent);r.setAmount(amount);return r;}
    @Test void screenshotSingleRowTracksNewOrderAmountBeforeUpdate(){
        var r=row(100,199952.5);r.setDueDays(2);r.setDueDate("2026-09-25");
        PurchaseOrderPaymentRules.recalculate(List.of(r),new BigDecimal("202070"),false,true);
        assertEquals(202070,r.getAmount());assertEquals(2,r.getDueDays());assertEquals("2026-09-25",r.getDueDate());
    }
    @Test void splitPercentageScheduleRecalculatesButManualSplitDoesNot(){
        var a=row(25,50);var b=row(75,150);
        PurchaseOrderPaymentRules.recalculate(List.of(a,b),new BigDecimal("400"),false,true);
        assertEquals(50,a.getAmount());assertEquals(150,b.getAmount());
        PurchaseOrderPaymentRules.recalculate(List.of(a,b),new BigDecimal("400"),true,true);
        assertEquals(100,a.getAmount());assertEquals(300,b.getAmount());
    }
    @Test void blankScheduleAndIncompletePercentageAreNotSilentlyCompleted(){
        var blank=row(0,0);PurchaseOrderPaymentRules.recalculate(List.of(blank),new BigDecimal("400"),false,true);assertEquals(0,blank.getAmount());
        var partial=row(40,100);PurchaseOrderPaymentRules.recalculate(List.of(partial),new BigDecimal("400"),false,true);assertEquals(160,partial.getAmount());assertEquals(40,partial.getPrcntOfTotal());
    }
    @Test void desktopMidpointEvenRounding(){
        var r=row(50,1);PurchaseOrderPaymentRules.recalculate(List.of(r),new BigDecimal("2.0001"),true,false);assertEquals(1,r.getAmount());
    }
}
