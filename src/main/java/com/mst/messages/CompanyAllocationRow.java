package com.mst.messages;

import lombok.Data;

@Data
public class CompanyAllocationRow {
    private Integer companyId;
    private String companyName;
    private boolean allocated;

    public Integer getCompanyId() { return companyId; }
    public void setCompanyId(Integer companyId) { this.companyId = companyId; }
    public String getCompanyName() { return companyName; }
    public void setCompanyName(String companyName) { this.companyName = companyName; }
    public boolean isAllocated() { return allocated; }
    public void setAllocated(boolean allocated) { this.allocated = allocated; }
}
