package com.mst.messages;

import lombok.Data;

@Data
public class CompanyAllocationRow {
    private Integer companyId;
    private String companyName;
    private boolean allocated;
}
