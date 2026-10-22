package com.mst.projections;


/** Row of SP_JobLot_ReadMethod @Activity='GetJobLotGlIdsandName' (native). */
public interface JobLotGlIdsRow {
    Integer getId();
    String getJobLotCode();
    String getJobLotDescription();
    Integer getAccountId();
    String getAccountTitle();
    Integer getBranchId();
    String getBranchName();
}
