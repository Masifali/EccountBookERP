package com.mst.models.dto;

import java.math.BigDecimal;

public class PartyPaymentLineItemDto {
    private Integer lineId;
    private Integer accountId; // Credit Account ID
    private String accountCode;
    private String accountTitle;
    private Integer accountTypeId;
    private Integer subsidiaryAccountId;
    private String subsidiaryAccountTitle;
    private Integer jobLotId;
    private String jobLotDescription;
    private String chequeDate;
    private Integer chequeId;
    private String chequeNo;
    private BigDecimal amount;
    private String remarks;
    private Integer branchId;
    private String branchName;

    public PartyPaymentLineItemDto() {
        this.amount = BigDecimal.ZERO;
    }

    public Integer getLineId() {
        return lineId;
    }

    public void setLineId(Integer lineId) {
        this.lineId = lineId;
    }

    public Integer getAccountId() {
        return accountId;
    }

    public void setAccountId(Integer accountId) {
        this.accountId = accountId;
    }

    public String getAccountCode() {
        return accountCode;
    }

    public void setAccountCode(String accountCode) {
        this.accountCode = accountCode;
    }

    public String getAccountTitle() {
        return accountTitle;
    }

    public void setAccountTitle(String accountTitle) {
        this.accountTitle = accountTitle;
    }

    public Integer getAccountTypeId() {
        return accountTypeId;
    }

    public void setAccountTypeId(Integer accountTypeId) {
        this.accountTypeId = accountTypeId;
    }

    public Integer getSubsidiaryAccountId() {
        return subsidiaryAccountId;
    }

    public void setSubsidiaryAccountId(Integer subsidiaryAccountId) {
        this.subsidiaryAccountId = subsidiaryAccountId;
    }

    public String getSubsidiaryAccountTitle() {
        return subsidiaryAccountTitle;
    }

    public void setSubsidiaryAccountTitle(String subsidiaryAccountTitle) {
        this.subsidiaryAccountTitle = subsidiaryAccountTitle;
    }

    public Integer getJobLotId() {
        return jobLotId;
    }

    public void setJobLotId(Integer jobLotId) {
        this.jobLotId = jobLotId;
    }

    public String getJobLotDescription() {
        return jobLotDescription;
    }

    public void setJobLotDescription(String jobLotDescription) {
        this.jobLotDescription = jobLotDescription;
    }

    public String getChequeDate() {
        return chequeDate;
    }

    public void setChequeDate(String chequeDate) {
        this.chequeDate = chequeDate;
    }

    public Integer getChequeId() {
        return chequeId;
    }

    public void setChequeId(Integer chequeId) {
        this.chequeId = chequeId;
    }

    public String getChequeNo() {
        return chequeNo;
    }

    public void setChequeNo(String chequeNo) {
        this.chequeNo = chequeNo;
    }

    public BigDecimal getAmount() {
        return amount;
    }

    public void setAmount(BigDecimal amount) {
        this.amount = amount;
    }

    public String getRemarks() {
        return remarks;
    }

    public void setRemarks(String remarks) {
        this.remarks = remarks;
    }

    public Integer getBranchId() {
        return branchId;
    }

    public void setBranchId(Integer branchId) {
        this.branchId = branchId;
    }

    public String getBranchName() {
        return branchName;
    }

    public void setBranchName(String branchName) {
        this.branchName = branchName;
    }
}
