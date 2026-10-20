package com.mst.repositories;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.DriverBiodata;
import com.mst.projections.DriverBiodataHistoryRow;
import com.mst.projections.DriverBiodataReadAllRow;

/**
 * dbo.driverBiodata - USP_DriverBiodata_InsertIfNotExists and USP_driverBiodata_GetAllMethod (@Activity branches).
 *
 * USP_DriverBiodata_InsertIfNotExists logic: if a row with the same cnicNo + cellNo exists its driverBiodataId is returned
 * ({@link #findIdByCnicNoAndCellNo}); otherwise driverBiodataId = ISNULL(MAX(driverBiodataId),0)+1 ({@link #maxDriverBiodataId()} + 1),
 * DocumentTypeId is forced to 146, ActionId to 1, RevisionNo = 1, IsApproved = 0, EntryDate = GETDATE() (the @EntryDate parameter is
 * ignored; ModifyUserId/ModifyDate/ApprovedUserId/ApprovedDate/RowVersion are not written), then INSERT -> save(); then
 * EXEC DAW.USp_DocumentApprovalDetail_Insert (OrganizationId, CompanyId, 146, newId) and EXEC USP_UserAudit_Insert
 * ('Created Driver Biodata record for: <name>', RefDocumentTypeId 146, ScreenName '', ActionId 1) - see UserAuditQueryRepository;
 * finally SELECT the id.
 */
@Repository
public interface DriverBiodataRepository extends JpaRepository<DriverBiodata, Integer> {

    // =====================================================================================
    // USP_DriverBiodata_InsertIfNotExists
    // =====================================================================================

    /** USP_DriverBiodata_InsertIfNotExists - KEEP-EXEC (insert-if-missing + DAW approval detail + user audit); returns the existing or new driverBiodataId. All parameters default to NULL. */
    // KEEP-EXEC: calls DAW.USp_DocumentApprovalDetail_Insert and USP_UserAudit_Insert after the insert
    @Transactional
    @Query(value = "EXEC dbo.USP_DriverBiodata_InsertIfNotExists @driverBiodataId = :driverBiodataId, @DocumentTypeId = :documentTypeId, "
         + "@driverName = :driverName, @cnicNo = :cnicNo, @cellNo = :cellNo, @whatsappNo = :whatsappNo, @alternateCellNo = :alternateCellNo, "
         + "@fatherName = :fatherName, @fatherCnicNo = :fatherCnicNo, @driverPicture = :driverPicture, @driverFingerPrint = :driverFingerPrint, "
         + "@OrganizationId = :organizationId, @CompanyId = :companyId, @BranchId = :branchId, @EntryUserId = :entryUserId, @EntryDate = :entryDate, "
         + "@ModifyUserId = :modifyUserId, @ModifyDate = :modifyDate, @IsApproved = :isApproved, @ApprovedUserId = :approvedUserId, "
         + "@ApprovedDate = :approvedDate, @ActionId = :actionId, @AttachmentsValues = :attachmentsValues, "
         + "@CustomAttachmentsValues = :customAttachmentsValues", nativeQuery = true)
    List<Integer> uspDriverBiodataInsertIfNotExists(@Param("driverBiodataId") Integer driverBiodataId, @Param("documentTypeId") Integer documentTypeId,
            @Param("driverName") String driverName, @Param("cnicNo") String cnicNo, @Param("cellNo") String cellNo, @Param("whatsappNo") String whatsappNo,
            @Param("alternateCellNo") String alternateCellNo, @Param("fatherName") String fatherName, @Param("fatherCnicNo") String fatherCnicNo,
            @Param("driverPicture") String driverPicture, @Param("driverFingerPrint") String driverFingerPrint,
            @Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId, @Param("branchId") Integer branchId,
            @Param("entryUserId") Integer entryUserId, @Param("entryDate") LocalDateTime entryDate, @Param("modifyUserId") Integer modifyUserId,
            @Param("modifyDate") LocalDateTime modifyDate, @Param("isApproved") Boolean isApproved, @Param("approvedUserId") Integer approvedUserId,
            @Param("approvedDate") LocalDateTime approvedDate, @Param("actionId") Integer actionId, @Param("attachmentsValues") String attachmentsValues,
            @Param("customAttachmentsValues") String customAttachmentsValues);

    /** USP_DriverBiodata_InsertIfNotExists step 1: SELECT driverBiodataId FROM driverBiodata WHERE cnicNo=@CnicNo AND cellNo=@CellNo. */
    @Query("SELECT d.driverBiodataId FROM DriverBiodata d WHERE d.cnicNo = :cnicNo AND d.cellNo = :cellNo")
    List<Integer> findIdByCnicNoAndCellNo(@Param("cnicNo") String cnicNo, @Param("cellNo") String cellNo);

    /** USP_DriverBiodata_InsertIfNotExists step 2: SELECT ISNULL(MAX(driverBiodataId),0) FROM driverBiodata (caller adds 1). */
    @Query("select coalesce(max(e.driverBiodataId),0) from DriverBiodata e")
    Integer maxDriverBiodataId();

    // =====================================================================================
    // USP_driverBiodata_GetAllMethod
    // =====================================================================================

    /** USP_driverBiodata_GetAllMethod @Activity='ReadById' (all entity columns; soft-deleted rows ActionId=3 excluded). */
    @Query("SELECT d FROM DriverBiodata d WHERE d.driverBiodataId = :id AND d.actionId <> 3")
    List<DriverBiodata> readById(@Param("id") Integer id);

    /**
     * USP_driverBiodata_GetAllMethod @Activity='FormHistory' (native: the proc's #tmpResult / #tmpDms pipeline rewritten as CTEs -
     * ROW_NUMBER() paging via @NoOfRecords, DMSAttachments count per row, UserAccount.UserTitle names).
     * Date filters are optional (NULL); @CanViewAllRecord 1 = all rows, 0 = only rows entered by @EntryUserId.
     */
    @Query(value = "WITH r AS (SELECT ROW_NUMBER() OVER (ORDER BY d.[driverBiodataId] DESC) RecordNo, "
         + "d.[driverBiodataId], d.[DocumentTypeId], d.[driverName], d.[cnicNo], d.[cellNo], d.[whatsappNo], d.[alternateCellNo], d.[fatherName], d.[fatherCnicNo], "
         + "d.[driverPicture], d.[driverFingerPrint], d.[OrganizationId], d.[CompanyId], d.[BranchId], d.[EntryUserId], d.[EntryDate], d.[ModifyUserId], d.[ModifyDate], "
         + "d.[IsApproved], d.[ApprovedUserId], d.[ApprovedDate], d.[ActionId], d.[RevisionNo], d.[RowVersion], d.[AttachmentsValues], d.[CustomAttachmentsValues] "
         + "FROM [dbo].[driverBiodata] d "
         + "WHERE d.OrganizationId = :organizationId AND d.CompanyId = :companyId "
         + "AND (:entryFromDate IS NULL OR CAST(d.EntryDate AS DATE) >= CAST(:entryFromDate AS DATE)) "
         + "AND (:entryToDate IS NULL OR CAST(d.EntryDate AS DATE) <= CAST(:entryToDate AS DATE)) "
         + "AND (:modifyFromDate IS NULL OR CAST(d.ModifyDate AS DATE) >= CAST(:modifyFromDate AS DATE)) "
         + "AND (:modifyToDate IS NULL OR CAST(d.ModifyDate AS DATE) <= CAST(:modifyToDate AS DATE)) "
         + "AND (:approvedFromDate IS NULL OR CAST(d.ApprovedDate AS DATE) >= CAST(:approvedFromDate AS DATE)) "
         + "AND (:approvedToDate IS NULL OR CAST(d.ApprovedDate AS DATE) <= CAST(:approvedToDate AS DATE)) "
         + "AND (:id IS NULL OR d.driverBiodataId = :id) AND d.ActionId <> 3 "
         + "AND (:canViewAllRecord = 1 OR (:canViewAllRecord = 0 AND d.EntryUserId = :entryUserId))), "
         + "dms AS (SELECT COUNT(d.Id) AS NoOfAttachments, d.RefDocumentTypeId, d.RefDocumentNo FROM dbo.DMSAttachments d "
         + "INNER JOIN r ON d.RefDocumentTypeId = r.DocumentTypeId AND d.RefDocumentNo = r.[driverBiodataId] "
         + "WHERE d.OrganizationId = :organizationId AND d.CompanyId = :companyId GROUP BY d.RefDocumentTypeId, d.RefDocumentNo) "
         + "SELECT d.[driverBiodataId], d.[DocumentTypeId], d.[driverName], d.[cnicNo], d.[cellNo], d.[whatsappNo], d.[alternateCellNo], d.[fatherName], d.[fatherCnicNo], "
         + "d.[driverPicture], d.[driverFingerPrint], d.[OrganizationId], d.[CompanyId], d.[BranchId], d.[EntryUserId], d.[EntryDate], d.[ModifyUserId], d.[ModifyDate], "
         + "d.[IsApproved], d.[ApprovedUserId], d.[ApprovedDate], d.[ActionId], d.[RevisionNo], d.[RowVersion], d.[AttachmentsValues], d.[CustomAttachmentsValues], "
         + "ISNULL(dms.NoOfAttachments, 0) AS NoOfAttachments, Esr.UserTitle AS EntryUserName, Msr.UserTitle AS ModifyUserName, Asr.UserTitle AS ApprovalUserName "
         + "FROM r d "
         + "LEFT JOIN dms ON dms.RefDocumentTypeId = d.DocumentTypeId AND dms.RefDocumentNo = d.[driverBiodataId] "
         + "INNER JOIN UserAccount Esr ON d.EntryUserId = Esr.ID "
         + "LEFT JOIN UserAccount Msr ON d.ModifyUserId = Msr.ID "
         + "LEFT JOIN UserAccount Asr ON d.ApprovedUserId = Asr.ID "
         + "WHERE (:noOfRecords IS NULL OR d.RecordNo <= :noOfRecords)", nativeQuery = true)
    List<DriverBiodataHistoryRow> formHistory(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId,
            @Param("id") Integer id, @Param("noOfRecords") Integer noOfRecords, @Param("canViewAllRecord") Integer canViewAllRecord,
            @Param("entryUserId") Integer entryUserId, @Param("entryFromDate") LocalDate entryFromDate, @Param("entryToDate") LocalDate entryToDate,
            @Param("modifyFromDate") LocalDate modifyFromDate, @Param("modifyToDate") LocalDate modifyToDate,
            @Param("approvedFromDate") LocalDate approvedFromDate, @Param("approvedToDate") LocalDate approvedToDate);

    /**
     * USP_driverBiodata_GetAllMethod @Activity='DeleteById' - KEEP-EXEC: RAISERROR 'Record cannot be updated because record has been approved'
     * when IsApproved=1, else soft delete (ActionId=3, ModifyDate=GETDATE(), ModifyUserId=@EntryUserId) + USP_UserAudit_Insert
     * ('Delete DriverBio With DriverName: ...', ActionId 3).
     */
    // KEEP-EXEC: RAISERROR validation + USP_UserAudit_Insert after the update
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.USP_driverBiodata_GetAllMethod @Id = :id, @EntryUserId = :entryUserId, @Activity = 'DeleteById'", nativeQuery = true)
    void deleteByIdProc(@Param("id") Integer id, @Param("entryUserId") Integer entryUserId);

    /** DeleteById, the UPDATE alone: soft delete (ActionId = 3, ModifyDate = now, ModifyUserId = @EntryUserId) of a not-yet-deleted row. */
    @Modifying
    @Transactional
    @Query("UPDATE DriverBiodata d SET d.actionId = 3, d.modifyDate = CURRENT_TIMESTAMP, d.modifyUserId = :entryUserId "
         + "WHERE d.driverBiodataId = :id AND d.actionId <> 3")
    int softDeleteById(@Param("id") Integer id, @Param("entryUserId") Integer entryUserId);

    /** DeleteById pre-check: IsApproved of the row (proc raises an error when it is 1). */
    @Query("SELECT d.isApproved FROM DriverBiodata d WHERE d.driverBiodataId = :id AND d.actionId <> 3")
    List<Boolean> isApprovedById(@Param("id") Integer id);

    /** USP_driverBiodata_GetAllMethod @Activity='ReadByCnicOrCellNo' (@CNICNo wins when given, else @CellNo; all entity columns). */
    @Query("SELECT d FROM DriverBiodata d WHERE d.actionId <> 3 "
         + "AND ((:cnicNo IS NOT NULL AND d.cnicNo = :cnicNo) OR (:cnicNo IS NULL AND :cellNo IS NOT NULL AND d.cellNo = :cellNo))")
    List<DriverBiodata> readByCnicOrCellNo(@Param("cnicNo") String cnicNo, @Param("cellNo") String cellNo);

    /** USP_driverBiodata_GetAllMethod @Activity='ReadAll' (native: ROW_NUMBER() RecordNo + UserAccount.UserTitle names). */
    @Query(value = "SELECT ROW_NUMBER() OVER (ORDER BY d.[driverBiodataId] DESC) RecordNo, "
         + "d.[driverBiodataId], d.[DocumentTypeId], d.[driverName], d.[cnicNo], d.[cellNo], d.[whatsappNo], d.[alternateCellNo], d.[fatherName], d.[fatherCnicNo], "
         + "d.[driverPicture], d.[driverFingerPrint], d.[OrganizationId], d.[CompanyId], d.[BranchId], d.[EntryUserId], d.[EntryDate], d.[ModifyUserId], d.[ModifyDate], "
         + "d.[IsApproved], d.[ApprovedUserId], d.[ApprovedDate], d.[ActionId], d.[RevisionNo], d.[RowVersion], d.[AttachmentsValues], d.[CustomAttachmentsValues], "
         + "Esr.UserTitle AS EntryUserName, Msr.UserTitle AS ModifyUserName, Asr.UserTitle AS ApprovalUserName "
         + "FROM [dbo].[driverBiodata] d "
         + "INNER JOIN UserAccount Esr ON d.EntryUserId = Esr.ID "
         + "LEFT JOIN UserAccount Msr ON d.ModifyUserId = Msr.ID "
         + "LEFT JOIN UserAccount Asr ON d.ApprovedUserId = Asr.ID "
         + "WHERE d.OrganizationId = :organizationId AND d.CompanyId = :companyId AND d.ActionId <> 3", nativeQuery = true)
    List<DriverBiodataReadAllRow> readAll(@Param("organizationId") Integer organizationId, @Param("companyId") Integer companyId);
}
