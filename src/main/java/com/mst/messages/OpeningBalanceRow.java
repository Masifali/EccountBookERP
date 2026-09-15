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

    public Integer getAccountId() { return accountId; }
    public void setAccountId(Integer accountId) { this.accountId = accountId; }
    public String getAccountCode() { return accountCode; }
    public void setAccountCode(String accountCode) { this.accountCode = accountCode; }
    public String getAccountTitle() { return accountTitle; }
    public void setAccountTitle(String accountTitle) { this.accountTitle = accountTitle; }
    public String getAccountType() { return accountType; }
    public void setAccountType(String accountType) { this.accountType = accountType; }
    public String getParentAccountTitle() { return parentAccountTitle; }
    public void setParentAccountTitle(String parentAccountTitle) { this.parentAccountTitle = parentAccountTitle; }
    public Double getDebit() { return debit; }
    public void setDebit(Double debit) { this.debit = debit; }
    public Double getCredit() { return credit; }
    public void setCredit(Double credit) { this.credit = credit; }
    public BigDecimal getOpeningDebit() { return openingDebit; }
    public BigDecimal getOpeningCredit() { return openingCredit; }
}
