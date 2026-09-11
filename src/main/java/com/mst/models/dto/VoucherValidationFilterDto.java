package com.mst.models.dto;

public class VoucherValidationFilterDto {
    private String fromDate;
    private String toDate;
    private String manualNo;
    private Integer accountId;
    private String documentTypeIds;
    private Integer customGroupId;
    private Integer docNoFrom;
    private Integer docNoTo;
    private String approvedFilter; // "Approved", "UnApproved", "All"
    private String filterType; // "DocDate", "EntryDate", "ModifyDate", "ApproveDate"
    private Boolean skipCgs;
    private Integer languageId;

    public VoucherValidationFilterDto() {
        this.approvedFilter = "All";
        this.filterType = "DocDate";
        this.skipCgs = false;
    }

    public String getFromDate() {
        return fromDate;
    }

    public void setFromDate(String fromDate) {
        this.fromDate = fromDate;
    }

    public String getToDate() {
        return toDate;
    }

    public void setToDate(String toDate) {
        this.toDate = toDate;
    }

    public String getManualNo() {
        return manualNo;
    }

    public void setManualNo(String manualNo) {
        this.manualNo = manualNo;
    }

    public Integer getAccountId() {
        return accountId;
    }

    public void setAccountId(Integer accountId) {
        this.accountId = accountId;
    }

    public String getDocumentTypeIds() {
        return documentTypeIds;
    }

    public void setDocumentTypeIds(String documentTypeIds) {
        this.documentTypeIds = documentTypeIds;
    }

    public Integer getCustomGroupId() {
        return customGroupId;
    }

    public void setCustomGroupId(Integer customGroupId) {
        this.customGroupId = customGroupId;
    }

    public Integer getDocNoFrom() {
        return docNoFrom;
    }

    public void setDocNoFrom(Integer docNoFrom) {
        this.docNoFrom = docNoFrom;
    }

    public Integer getDocNoTo() {
        return docNoTo;
    }

    public void setDocNoTo(Integer docNoTo) {
        this.docNoTo = docNoTo;
    }

    public String getApprovedFilter() {
        return approvedFilter;
    }

    public void setApprovedFilter(String approvedFilter) {
        this.approvedFilter = approvedFilter;
    }

    public String getFilterType() {
        return filterType;
    }

    public void setFilterType(String filterType) {
        this.filterType = filterType;
    }

    public Boolean getSkipCgs() {
        return skipCgs;
    }

    public void setSkipCgs(Boolean skipCgs) {
        this.skipCgs = skipCgs;
    }

    public Integer getLanguageId() {
        return languageId;
    }

    public void setLanguageId(Integer languageId) {
        this.languageId = languageId;
    }
}
