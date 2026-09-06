package com.accountbook.erp.model;

public class BankDefinition {
    private String bankCode;
    private String bankName;
    private String accountNumber;
    private String branchName;

    public BankDefinition() {}

    public BankDefinition(String bankCode, String bankName, String accountNumber, String branchName) {
        this.bankCode = bankCode;
        this.bankName = bankName;
        this.accountNumber = accountNumber;
        this.branchName = branchName;
    }

    public String getBankCode() { return bankCode; }
    public void setBankCode(String bankCode) { this.bankCode = bankCode; }

    public String getBankName() { return bankName; }
    public void setBankName(String bankName) { this.bankName = bankName; }

    public String getAccountNumber() { return accountNumber; }
    public void setAccountNumber(String accountNumber) { this.accountNumber = accountNumber; }

    public String getBranchName() { return branchName; }
    public void setBranchName(String branchName) { this.branchName = branchName; }
}
