package com.mst.models.dto;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class PartyPaymentVoucherDto {
    private Integer id;
    private Integer voucherCode;
    private String voucherCodeDisplay; // e.g. PPV-2026-0001
    private String voucherDate;
    private Integer refAccountId; // Dr Account ID
    private String refAccountTitle;
    private Integer subsidiaryAccountDrId;
    private String remarks;
    private BigDecimal totalAmount;
    private Integer branchId;
    private Integer multiCurrencyId;
    private Double exchangeRate;
    private Boolean preview1;
    private Boolean preview2;
    private List<PartyPaymentLineItemDto> lineItems;

    public PartyPaymentVoucherDto() {
        this.lineItems = new ArrayList<>();
        this.totalAmount = BigDecimal.ZERO;
        this.preview1 = false;
        this.preview2 = false;
        this.exchangeRate = 1.0;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getVoucherCode() {
        return voucherCode;
    }

    public void setVoucherCode(Integer voucherCode) {
        this.voucherCode = voucherCode;
    }

    public String getVoucherCodeDisplay() {
        return voucherCodeDisplay;
    }

    public void setVoucherCodeDisplay(String voucherCodeDisplay) {
        this.voucherCodeDisplay = voucherCodeDisplay;
    }

    public String getVoucherDate() {
        return voucherDate;
    }

    public void setVoucherDate(String voucherDate) {
        this.voucherDate = voucherDate;
    }

    public Integer getRefAccountId() {
        return refAccountId;
    }

    public void setRefAccountId(Integer refAccountId) {
        this.refAccountId = refAccountId;
    }

    public String getRefAccountTitle() {
        return refAccountTitle;
    }

    public void setRefAccountTitle(String refAccountTitle) {
        this.refAccountTitle = refAccountTitle;
    }

    public Integer getSubsidiaryAccountDrId() {
        return subsidiaryAccountDrId;
    }

    public void setSubsidiaryAccountDrId(Integer subsidiaryAccountDrId) {
        this.subsidiaryAccountDrId = subsidiaryAccountDrId;
    }

    public String getRemarks() {
        return remarks;
    }

    public void setRemarks(String remarks) {
        this.remarks = remarks;
    }

    public BigDecimal getTotalAmount() {
        return totalAmount;
    }

    public void setTotalAmount(BigDecimal totalAmount) {
        this.totalAmount = totalAmount;
    }

    public Integer getBranchId() {
        return branchId;
    }

    public void setBranchId(Integer branchId) {
        this.branchId = branchId;
    }

    public Integer getMultiCurrencyId() {
        return multiCurrencyId;
    }

    public void setMultiCurrencyId(Integer multiCurrencyId) {
        this.multiCurrencyId = multiCurrencyId;
    }

    public Double getExchangeRate() {
        return exchangeRate;
    }

    public void setExchangeRate(Double exchangeRate) {
        this.exchangeRate = exchangeRate;
    }

    public Boolean getPreview1() {
        return preview1;
    }

    public void setPreview1(Boolean preview1) {
        this.preview1 = preview1;
    }

    public Boolean getPreview2() {
        return preview2;
    }

    public void setPreview2(Boolean preview2) {
        this.preview2 = preview2;
    }

    public List<PartyPaymentLineItemDto> getLineItems() {
        return lineItems;
    }

    public void setLineItems(List<PartyPaymentLineItemDto> lineItems) {
        this.lineItems = lineItems;
    }
}
