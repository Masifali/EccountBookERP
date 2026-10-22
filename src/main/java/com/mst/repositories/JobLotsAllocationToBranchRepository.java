package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.JobLotsAllocationToBranch;
import com.mst.projections.JobLotBranchAllocationRow;

/**
 * dbo.JobLotsAllocationToBranch — USP_JobLotsAllocationToBranch_Insert, USP_JobLotsAllocationToBranchDeleteById,
 * USP_JobLotsAllocatedOrUnAllocatedToBranch.
 *
 * Pre-insert logic of USP_JobLotsAllocationToBranch_Insert (callers use save()):
 *  - Id = ISNULL(MAX(Id),0)+1 over the whole table (maxId() + 1); proc returns SELECT @Id.
 *  - Inserted columns: Id, JobLotId, BranchId, EntryDate, EntryUserId, OrganizationId, CompanyId, FinancialYearId
 *    (ModifyDate/ModifyUserId parameters are ignored). All parameters default to NULL.
 */
@Repository
public interface JobLotsAllocationToBranchRepository extends JpaRepository<JobLotsAllocationToBranch, Integer> {

    /** USP_JobLotsAllocationToBranch_Insert — next app-assigned Id: ISNULL(MAX(Id),0) (+1 by the caller). */
    @Query("select coalesce(max(e.id),0) from JobLotsAllocationToBranch e")
    Integer maxId();

    /**
     * USP_JobLotsAllocationToBranchDeleteById (single branch) — verbatim: @JobLotIds is a comma-separated list split by
     * dbo.fnSplitString. Both parameters default to NULL.
     */
    @Modifying
    @Transactional
    @Query(value = "DELETE FROM dbo.JobLotsAllocationToBranch WHERE (BranchId = :branchId) "
         + "AND JobLotId IN (SELECT data FROM dbo.fnSplitString(:jobLotIds, ','))", nativeQuery = true)
    int deleteByBranchIdAndJobLotIds(@Param("branchId") Integer branchId, @Param("jobLotIds") String jobLotIds);

    /** USP_JobLotsAllocationToBranchDeleteById — JPQL equivalent for callers that already hold the ids as a list. */
    @Modifying
    @Transactional
    @Query("DELETE FROM JobLotsAllocationToBranch a WHERE a.branchId = :branchId AND a.jobLotId IN :jobLotIds")
    int deleteByBranchIdAndJobLotIdIn(@Param("branchId") Integer branchId, @Param("jobLotIds") List<Integer> jobLotIds);

    /**
     * USP_JobLotsAllocatedOrUnAllocatedToBranch (single branch). @ActionId 1 = job lots not allocated to any branch of the
     * company (BranchId is ignored), 2 = job lots allocated to @BranchId. All parameters default to NULL.
     */
    @Query("SELECT g.id AS branchId, g.branchName AS branchName, i.id AS jobLotId, i.jobLotDescription AS jobLotName "
         + "FROM JobLot i "
         + "LEFT JOIN JobLotsAllocationToBranch c ON i.id = c.jobLotId AND i.companyId = c.companyId "
         + "LEFT JOIN Branch g ON c.branchId = g.id AND g.companyId = c.companyId "
         + "WHERE i.organizationId = :organizationId AND i.companyId = :companyId "
         + "AND ((:actionId = 1 AND i.id NOT IN (SELECT c1.jobLotId FROM JobLotsAllocationToBranch c1 WHERE c1.companyId = :companyId)) "
         + "OR (:actionId = 2 AND i.id IN (SELECT c2.jobLotId FROM JobLotsAllocationToBranch c2 WHERE c2.companyId = :companyId AND c2.branchId = :branchId)))")
    List<JobLotBranchAllocationRow> jobLotsAllocatedOrUnAllocatedToBranch(@Param("organizationId") Integer organizationId,
                                                                          @Param("companyId") Integer companyId,
                                                                          @Param("actionId") Integer actionId,
                                                                          @Param("branchId") Integer branchId);
}
