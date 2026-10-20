package com.mst.projections;

import java.time.LocalDateTime;

/** Row of SP_JobLot_ReadMethod @Activity='GetById' (JPQL). */
public interface JobLotByIdRow {
    Integer getId();
    String getJobLotCode();
    String getJobLotDescription();
    LocalDateTime getStartDate();
    LocalDateTime getEndDate();
    Boolean getIsApproved();
    String getJobStatus();
    LocalDateTime getEntryDate();
    Integer getEntryUser();
    LocalDateTime getModifyDate();
    Integer getModifyUser();
    LocalDateTime getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getJobTypeId();
    Boolean getIsThirdParty();
    Boolean getIsCompany();
    Integer getAccountId();
    Integer getRefDocumentTypeId();
    Integer getRefDocNoId();
}
