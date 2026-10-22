package com.mst.repositories;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.JobLot;
import com.mst.projections.JobLotByIdRow;
import com.mst.projections.JobLotDocumentTypeRow;
import com.mst.projections.JobLotGetAllRow;
import com.mst.projections.JobLotGlIdsRow;
import com.mst.projections.JobLotLookupRow;
import com.mst.projections.JobLotReferenceNoRow;

/**
 * dbo.JobLot — Proc_JobLot_Insert, Proc_JobLot_Update, SP_JobLot_ReadMethod,
 * usp_getDocumentsForJobLot, usp_getReferenceNoByDocumentTypeIdFromJobLot.
 *
 * Pre-insert logic of Proc_JobLot_Insert (for callers using save()):
 *  - Id = ISNULL(MAX(Id),0)+1 over the whole table (maxId() + 1).
 *  - RAISERROR 'This JobLot already exist in table.' when a row with the same OrganizationId/CompanyId/JobLotCode exists
 *    (existsByOrganizationIdAndCompanyIdAndJobLotCode).
 *  - RAISERROR 'StartDate cannot be greater than EndDate' when CAST(StartDate AS DATE) > CAST(EndDate AS DATE).
 *  - RAISERROR 'This JobOrder already exist in table.' when RefDocNoId > 0 and a row with the same
 *    OrganizationId/CompanyId/RefDocumentTypeId/RefDocNoId exists (existsByOrganizationIdAndCompanyIdAndRefDocumentTypeIdAndRefDocNoId).
 *  - Only Id, JobLotCode, JobLotDescription, StartDate, EndDate, IsApproved, JobStatus, EntryDate, EntryUser,
 *    OrganizationId, CompanyId, JobTypeId, IsCompany, IsThirdParty, AccountId, RefDocumentTypeId, RefDocNoId are inserted
 *    (ModifyDate/ModifyUser/PostDate/PostUser/PostState parameters are ignored). Proc returns SELECT @Id.
 *  - AFTER the insert, if dbo.fnCheckERPFeaturesExistorNot(@OrganizationId,@CompanyId,11) = 0 the proc also inserts one
 *    JobLotsAllocationToBranch row per Branch of the company (latest FinancialYear, first UserAccount of the company).
 *    That block is only reproduced by procJobLotInsert() (KEEP-EXEC).
 *
 * Pre-update logic of Proc_JobLot_Update: same JobLotCode / date-range / JobOrder duplicate checks (excluding @Id),
 * plus 'Job Order mapping cannot be removed...' when the stored RefDocNoId > 0 and @RefDocNoId = 0 (findRefDocNoIdById).
 */
@Repository
public interface JobLotRepository extends JpaRepository<JobLot, Integer> {

    /** Proc_JobLot_Insert — next app-assigned Id: ISNULL(MAX(Id),0) (+1 by the caller). */
    @Query("select coalesce(max(e.id),0) from JobLot e")
    Integer maxId();

    /** Proc_JobLot_Insert duplicate-JobLotCode check (RAISERROR 'This JobLot already exist in table.'). */
    boolean existsByOrganizationIdAndCompanyIdAndJobLotCode(Integer organizationId, Integer companyId, String jobLotCode);

    /** Proc_JobLot_Update duplicate-JobLotCode check (excludes the row being updated). */
    boolean existsByOrganizationIdAndCompanyIdAndJobLotCodeAndIdNot(Integer organizationId, Integer companyId, String jobLotCode, Integer id);

    /** Proc_JobLot_Insert duplicate-JobOrder check (only evaluated when RefDocNoId > 0). */
    boolean existsByOrganizationIdAndCompanyIdAndRefDocumentTypeIdAndRefDocNoId(Integer organizationId, Integer companyId, Integer refDocumentTypeId, Integer refDocNoId);

    /** Proc_JobLot_Update duplicate-JobOrder check (only evaluated when RefDocNoId > 0; excludes the row being updated). */
    boolean existsByOrganizationIdAndCompanyIdAndRefDocumentTypeIdAndRefDocNoIdAndIdNot(Integer organizationId, Integer companyId, Integer refDocumentTypeId, Integer refDocNoId, Integer id);

    /** Proc_JobLot_Update: SELECT RefDocNoId FROM JobLot WHERE Id=@Id (proc raises if it is > 0 and @RefDocNoId = 0). */
    @Query("SELECT j.refDocNoId FROM JobLot j WHERE j.id = :id")
    Integer findRefDocNoIdById(@Param("id") Integer id);

    /**
     * Proc_JobLot_Insert (single branch) — full procedure incl. validations, SELECT @Id and the conditional
     * JobLotsAllocationToBranch auto-allocation. All proc parameters default to NULL; @Id is recomputed by the proc.
     * // KEEP-EXEC: calls dbo.fnCheckERPFeaturesExistorNot and inserts into JobLotsAllocationToBranch from
     * Company/Branches/FinancialYear/UserAccount (no entities for FinancialYear) after the main insert.
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Proc_JobLot_Insert @Id = :id, @JobLotCode = :jobLotCode, @JobLotDescription = :jobLotDescription, "
         + "@StartDate = :startDate, @EndDate = :endDate, @IsApproved = :isApproved, @JobStatus = :jobStatus, "
         + "@EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @JobTypeId = :jobTypeId, @IsCompany = :isCompany, @IsThirdParty = :isThirdParty, "
         + "@AccountId = :accountId, @RefDocumentTypeId = :refDocumentTypeId, @RefDocNoId = :refDocNoId", nativeQuery = true)
    void procJobLotInsert(@Param("id") Integer id,
                          @Param("jobLotCode") String jobLotCode,
                          @Param("jobLotDescription") String jobLotDescription,
                          @Param("startDate") LocalDateTime startDate,
                          @Param("endDate") LocalDateTime endDate,
                          @Param("isApproved") Boolean isApproved,
                          @Param("jobStatus") String jobStatus,
                          @Param("entryDate") LocalDateTime entryDate,
                          @Param("entryUser") Integer entryUser,
                          @Param("modifyDate") LocalDateTime modifyDate,
                          @Param("modifyUser") Integer modifyUser,
                          @Param("postDate") LocalDateTime postDate,
                          @Param("postUser") Integer postUser,
                          @Param("postState") Boolean postState,
                          @Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("jobTypeId") Integer jobTypeId,
                          @Param("isCompany") Boolean isCompany,
                          @Param("isThirdParty") Boolean isThirdParty,
                          @Param("accountId") Integer accountId,
                          @Param("refDocumentTypeId") Integer refDocumentTypeId,
                          @Param("refDocNoId") Integer refDocNoId);

    /**
     * Proc_JobLot_Update (single branch) — the UPDATE statement with every SET column of the proc.
     * EntryDate/EntryUser/PostDate/PostUser/PostState are NOT updated by the proc. Validations listed in the class Javadoc
     * must be performed by the caller (existsBy... / findRefDocNoIdById helpers above).
     */
    @Modifying
    @Transactional
    @Query("UPDATE JobLot j SET j.jobLotCode = :jobLotCode, j.jobLotDescription = :jobLotDescription, "
         + "j.startDate = :startDate, j.endDate = :endDate, j.isApproved = :isApproved, j.jobStatus = :jobStatus, "
         + "j.modifyDate = :modifyDate, j.modifyUser = :modifyUser, j.organizationId = :organizationId, "
         + "j.companyId = :companyId, j.jobTypeId = :jobTypeId, j.isCompany = :isCompany, j.isThirdParty = :isThirdParty, "
         + "j.accountId = :accountId, j.refDocumentTypeId = :refDocumentTypeId, j.refDocNoId = :refDocNoId "
         + "WHERE j.id = :id")
    int updateById(@Param("id") Integer id,
                   @Param("jobLotCode") String jobLotCode,
                   @Param("jobLotDescription") String jobLotDescription,
                   @Param("startDate") LocalDateTime startDate,
                   @Param("endDate") LocalDateTime endDate,
                   @Param("isApproved") Boolean isApproved,
                   @Param("jobStatus") String jobStatus,
                   @Param("modifyDate") LocalDateTime modifyDate,
                   @Param("modifyUser") Integer modifyUser,
                   @Param("organizationId") Integer organizationId,
                   @Param("companyId") Integer companyId,
                   @Param("jobTypeId") Integer jobTypeId,
                   @Param("isCompany") Boolean isCompany,
                   @Param("isThirdParty") Boolean isThirdParty,
                   @Param("accountId") Integer accountId,
                   @Param("refDocumentTypeId") Integer refDocumentTypeId,
                   @Param("refDocNoId") Integer refDocNoId);

    /**
     * Proc_JobLot_Update (single branch) — full procedure incl. all RAISERROR validations, for callers that want the
     * exact server-side behaviour. All proc parameters default to NULL.
     * // KEEP-EXEC: procedural validation (duplicate code, date range, job-order mapping removal, duplicate job order).
     */
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Proc_JobLot_Update @Id = :id, @JobLotCode = :jobLotCode, @JobLotDescription = :jobLotDescription, "
         + "@StartDate = :startDate, @EndDate = :endDate, @IsApproved = :isApproved, @JobStatus = :jobStatus, "
         + "@EntryDate = :entryDate, @EntryUser = :entryUser, @ModifyDate = :modifyDate, @ModifyUser = :modifyUser, "
         + "@PostDate = :postDate, @PostUser = :postUser, @PostState = :postState, @OrganizationId = :organizationId, "
         + "@CompanyId = :companyId, @JobTypeId = :jobTypeId, @IsCompany = :isCompany, @IsThirdParty = :isThirdParty, "
         + "@AccountId = :accountId, @RefDocumentTypeId = :refDocumentTypeId, @RefDocNoId = :refDocNoId", nativeQuery = true)
    void procJobLotUpdate(@Param("id") Integer id,
                          @Param("jobLotCode") String jobLotCode,
                          @Param("jobLotDescription") String jobLotDescription,
                          @Param("startDate") LocalDateTime startDate,
                          @Param("endDate") LocalDateTime endDate,
                          @Param("isApproved") Boolean isApproved,
                          @Param("jobStatus") String jobStatus,
                          @Param("entryDate") LocalDateTime entryDate,
                          @Param("entryUser") Integer entryUser,
                          @Param("modifyDate") LocalDateTime modifyDate,
                          @Param("modifyUser") Integer modifyUser,
                          @Param("postDate") LocalDateTime postDate,
                          @Param("postUser") Integer postUser,
                          @Param("postState") Boolean postState,
                          @Param("organizationId") Integer organizationId,
                          @Param("companyId") Integer companyId,
                          @Param("jobTypeId") Integer jobTypeId,
                          @Param("isCompany") Boolean isCompany,
                          @Param("isThirdParty") Boolean isThirdParty,
                          @Param("accountId") Integer accountId,
                          @Param("refDocumentTypeId") Integer refDocumentTypeId,
                          @Param("refDocNoId") Integer refDocNoId);

    /**
     * SP_JobLot_ReadMethod @Activity='GetAll' — native because InvProductionJobOrder has no entity
     * (InvLookUp / ChartofAccount / DocumentType joins kept verbatim).
     */
    @Query(value = "SELECT j.[Id], [JobLotCode], [JobLotDescription], j.[StartDate], j.[EndDate], j.[IsApproved], [JobStatus], "
         + "j.[EntryDate], j.[EntryUser], j.[ModifyDate], j.[ModifyUser], j.[PostDate], j.[PostUser], j.[PostState], "
         + "j.[OrganizationId], j.[CompanyId], j.JobTypeId, l.LookupName JobType, j.IsThirdParty, j.IsCompany, J.AccountId, "
         + "C.AccountTitle, j.RefDocumentTypeId, DType.DocumentTypeDescription As RefDocumentType, j.RefDocNoId, "
         + "P.RefInvoiceNo As RefDocNo "
         + "FROM [dbo].[JobLot] j "
         + "LEFT JOIN InvLookUp l on j.JobTypeId = l.Id "
         + "LEFT JOIN ChartofAccount C ON J.AccountId = C.Id "
         + "LEFT JOIN DocumentType DType ON DType.Id = j.RefDocumentTypeId "
         + "LEFT JOIN InvProductionJobOrder P On P.Id = j.RefDocNoId "
         + "WHERE j.OrganizationId = :organizationId AND j.CompanyId = :companyId", nativeQuery = true)
    List<JobLotGetAllRow> getAll(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /**
     * SP_JobLot_ReadMethod @Activity='AllJobLotForPartyProcessing'. The proc's LEFT JOIN InvLookUp (on its PK) contributes
     * no columns and cannot duplicate rows, so it is omitted here.
     */
    @Query("SELECT j.id AS id, j.jobLotCode AS jobLotCode, j.jobLotDescription AS jobLotDescription "
         + "FROM JobLot j "
         + "WHERE j.organizationId = :organizationId AND j.companyId = :companyId "
         + "AND COALESCE(j.isThirdParty, false) = true AND COALESCE(j.jobStatus, '') = 'InComplete'")
    List<JobLotLookupRow> allJobLotForPartyProcessing(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** SP_JobLot_ReadMethod @Activity='GetById' (take first; Id is the PK so at most one row). */
    @Query("SELECT j.id AS id, j.jobLotCode AS jobLotCode, j.jobLotDescription AS jobLotDescription, j.startDate AS startDate, "
         + "j.endDate AS endDate, j.isApproved AS isApproved, j.jobStatus AS jobStatus, j.entryDate AS entryDate, "
         + "j.entryUser AS entryUser, j.modifyDate AS modifyDate, j.modifyUser AS modifyUser, j.postDate AS postDate, "
         + "j.postUser AS postUser, j.postState AS postState, j.organizationId AS organizationId, j.companyId AS companyId, "
         + "j.jobTypeId AS jobTypeId, j.isThirdParty AS isThirdParty, j.isCompany AS isCompany, j.accountId AS accountId, "
         + "j.refDocumentTypeId AS refDocumentTypeId, j.refDocNoId AS refDocNoId "
         + "FROM JobLot j WHERE j.id = :id")
    List<JobLotByIdRow> getJobLotById(@Param("id") Integer id);

    /**
     * SP_JobLot_ReadMethod @Activity='GetJobLotGlIdsandName' — the #Data temp table + UPDATE-from-join rewritten as one
     * statement. The proc's UPDATE picks an arbitrary BranchId when a job lot is allocated to several branches; this
     * rewrite picks MAX(BranchId) (deterministic) and 0 when there is no allocation, exactly like the proc's default.
     */
    @Query(value = "WITH Data AS ("
         + "SELECT DISTINCT j.[Id], [JobLotCode], [JobLotDescription], J.AccountId, "
         + "ISNULL((SELECT MAX(ccc.BranchId) FROM JobLotsAllocationToBranch ccc WHERE ccc.JobLotId = j.Id AND ccc.CompanyId = :companyId), 0) AS BranchId "
         + "FROM [dbo].[JobLot] j WHERE j.OrganizationId = :organizationId AND j.CompanyId = :companyId) "
         + "SELECT DISTINCT j.[Id], [JobLotCode], [JobLotDescription], J.AccountId, C.AccountTitle, j.BranchId, g.BranchName "
         + "FROM Data j "
         + "LEFT JOIN ChartofAccount C ON J.AccountId = C.Id "
         + "LEFT JOIN dbo.Branches AS g ON j.BranchId = g.Id", nativeQuery = true)
    List<JobLotGlIdsRow> getJobLotGlIdsandName(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);

    /** usp_getDocumentsForJobLot (no parameters): DocumentType rows with Id IN (403, 401). */
    @Query("SELECT d.id AS id, d.documentTypeDescription AS documentTypeDescription FROM DocumentType d WHERE d.id IN (403, 401)")
    List<JobLotDocumentTypeRow> getDocumentsForJobLot();

    /**
     * usp_getReferenceNoByDocumentTypeIdFromJobLot — #TmpResult rewritten as one statement: rows come from
     * InvProductionJobOrder (no entity) only when @DocumentTypeId IN (401,403), otherwise the result is empty.
     * The proc's @RecId parameter (default NULL) is never used and is therefore not a method parameter.
     */
    @Query(value = "SELECT J.Id, J.RefInvoiceNo AS ReferenceNo FROM InvProductionJobOrder J "
         + "WHERE J.DocumentTypeId = :documentTypeId AND :documentTypeId IN (401, 403)", nativeQuery = true)
    List<JobLotReferenceNoRow> getReferenceNoByDocumentTypeIdFromJobLot(@Param("documentTypeId") Integer documentTypeId);
}
