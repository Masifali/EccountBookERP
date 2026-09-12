package com.mst.messages;

import java.math.BigDecimal;
import lombok.Data;

@Data
public class OpeningBalanceRow {
    private Integer accountId;
    private String accountCode;
    private String accountTitle;
    private String accountType;
    private String parentAccountTitle;
    private Double debit = 0.0;
    private Double credit = 0.0;
    private BigDecimal openingDebit = BigDecimal.ZERO;
    private BigDecimal openingCredit = BigDecimal.ZERO;

    public Double getOpeningDebitValue() {
        if (openingDebit != null) return openingDebit.doubleValue();
        return debit != null ? debit : 0.0;
    }

    public Double getOpeningCreditValue() {
        if (openingCredit != null) return openingCredit.doubleValue();
        return credit != null ? credit : 0.0;
    }

    public void setOpeningDebit(Double d) {
        this.debit = d != null ? d : 0.0;
        this.openingDebit = BigDecimal.valueOf(this.debit);
    }

    public void setOpeningCredit(Double c) {
        this.credit = c != null ? c : 0.0;
        this.openingCredit = BigDecimal.valueOf(this.credit);
    }

    public void setOpeningDebit(BigDecimal d) {
        this.openingDebit = d != null ? d : BigDecimal.ZERO;
        this.debit = this.openingDebit.doubleValue();
    }

    public void setOpeningCredit(BigDecimal c) {
        this.openingCredit = c != null ? c : BigDecimal.ZERO;
        this.credit = this.openingCredit.doubleValue();
    }
}
