package com.mst.models.dto;

import lombok.Data;

/**
 * DTO representing an allocation of an account to a custom group.
 */
@Data
public class AccountAllocationDto {
    private Integer allocationId; // primary key of AccountsCustomGroups (SortNo)
    private Integer accountId;   // ChartOfAccountId
    private String accountName;
    private Integer accountTypeId;
    private String accountTypeName;
    private Integer parentAccountId;
    private String parentAccountName;
    private String remarks;
}
