package com.mst.projections;


/** Row of USP_JobLotsAllocatedOrUnAllocatedToBranch (JPQL). */
public interface JobLotBranchAllocationRow {
    Integer getBranchId();
    String getBranchName();
    Integer getJobLotId();
    String getJobLotName();
}
