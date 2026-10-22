package com.mst.projections;

import java.sql.Timestamp;

/** Row of SP_JobLot_ReadMethod @Activity='GetAll' (native: datetime -> Timestamp). */
public interface JobLotGetAllRow {
    Integer getId();
    String getJobLotCode();
    String getJobLotDescription();
    Timestamp getStartDate();
    Timestamp getEndDate();
    Boolean getIsApproved();
    String getJobStatus();
    Timestamp getEntryDate();
    Integer getEntryUser();
    Timestamp getModifyDate();
    Integer getModifyUser();
    Timestamp getPostDate();
    Integer getPostUser();
    Boolean getPostState();
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getJobTypeId();
    String getJobType();
    Boolean getIsThirdParty();
    Boolean getIsCompany();
    Integer getAccountId();
    String getAccountTitle();
    Integer getRefDocumentTypeId();
    String getRefDocumentType();
    Integer getRefDocNoId();
    String getRefDocNo();
}
