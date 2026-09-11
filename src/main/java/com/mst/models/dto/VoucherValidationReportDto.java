package com.mst.models.dto;

import java.math.BigDecimal;

public class VoucherValidationReportDto {
    private Long id;
    private Long accountId;
    private String documentType;
    private Integer documentTypeId;
    private Integer documentTypeSrNo;
    private Integer voucherCode;
    private String voucherDate;
    private String accountCode;
    private String accountTitle;
    private String offsetAccount;
    private BigDecimal debitAmount;
    private BigDecimal creditAmount;
    private String chequeNo;
    private String manualBillNo;
    private String refInvoiceNo;
    private String comments;
    private Integer noOfAttachments;
    // Voucher Report (VoucherReport.cs) needs these extra audit/cheque columns from the
    // same real proc (Sp_Accounts_VouchersValidation_Rpt) that Voucher Validation Report
    // doesn't display - additive fields, harmless for Voucher Validation's own response.
    private String chequePartyName;
    private String entryUser;
    private String entryDate;
    private String modifyUser;
    private String modifyDate;
    private String approvedUser;
    private String approvedDate;

    public VoucherValidationReportDto() {
        this.debitAmount = BigDecimal.ZERO;
        this.creditAmount = BigDecimal.ZERO;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getAccountId() {
        return accountId;
    }

    public void setAccountId(Long accountId) {
        this.accountId = accountId;
    }

    public String getDocumentType() {
        return documentType;
    }

    public void setDocumentType(String documentType) {
        this.documentType = documentType;
    }

    public Integer getDocumentTypeId() {
        return documentTypeId;
    }

    public void setDocumentTypeId(Integer documentTypeId) {
        this.documentTypeId = documentTypeId;
    }

    public Integer getDocumentTypeSrNo() {
        return documentTypeSrNo;
    }

    public void setDocumentTypeSrNo(Integer documentTypeSrNo) {
        this.documentTypeSrNo = documentTypeSrNo;
    }

    public Integer getVoucherCode() {
        return voucherCode;
    }

    public void setVoucherCode(Integer voucherCode) {
        this.voucherCode = voucherCode;
    }

    public String getVoucherDate() {
        return voucherDate;
    }

    public void setVoucherDate(String voucherDate) {
        this.voucherDate = voucherDate;
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

    public String getOffsetAccount() {
        return offsetAccount;
    }

    public void setOffsetAccount(String offsetAccount) {
        this.offsetAccount = offsetAccount;
    }

    public BigDecimal getDebitAmount() {
        return debitAmount;
    }

    public void setDebitAmount(BigDecimal debitAmount) {
        this.debitAmount = debitAmount;
    }

    public BigDecimal getCreditAmount() {
        return creditAmount;
    }

    public void setCreditAmount(BigDecimal creditAmount) {
        this.creditAmount = creditAmount;
    }

    public String getChequeNo() {
        return chequeNo;
    }

    public void setChequeNo(String chequeNo) {
        this.chequeNo = chequeNo;
    }

    public String getManualBillNo() {
        return manualBillNo;
    }

    public void setManualBillNo(String manualBillNo) {
        this.manualBillNo = manualBillNo;
    }

    public String getRefInvoiceNo() {
        return refInvoiceNo;
    }

    public void setRefInvoiceNo(String refInvoiceNo) {
        this.refInvoiceNo = refInvoiceNo;
    }

    public String getComments() {
        return comments;
    }

    public void setComments(String comments) {
        this.comments = comments;
    }

    public Integer getNoOfAttachments() {
        return noOfAttachments;
    }

    public void setNoOfAttachments(Integer noOfAttachments) {
        this.noOfAttachments = noOfAttachments;
    }

    public String getChequePartyName() {
        return chequePartyName;
    }

    public void setChequePartyName(String chequePartyName) {
        this.chequePartyName = chequePartyName;
    }

    public String getEntryUser() {
        return entryUser;
    }

    public void setEntryUser(String entryUser) {
        this.entryUser = entryUser;
    }

    public String getEntryDate() {
        return entryDate;
    }

    public void setEntryDate(String entryDate) {
        this.entryDate = entryDate;
    }

    public String getModifyUser() {
        return modifyUser;
    }

    public void setModifyUser(String modifyUser) {
        this.modifyUser = modifyUser;
    }

    public String getModifyDate() {
        return modifyDate;
    }

    public void setModifyDate(String modifyDate) {
        this.modifyDate = modifyDate;
    }

    public String getApprovedUser() {
        return approvedUser;
    }

    public void setApprovedUser(String approvedUser) {
        this.approvedUser = approvedUser;
    }

    public String getApprovedDate() {
        return approvedDate;
    }

    public void setApprovedDate(String approvedDate) {
        this.approvedDate = approvedDate;
    }
}
