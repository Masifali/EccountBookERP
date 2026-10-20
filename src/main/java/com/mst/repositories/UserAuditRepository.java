package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.mst.models.UserAudit;

/**
 * UserAudit rows are written by USP_InvGrn_UpdateSpecialApproval (step 3).
 * Pre-insert logic from the proc: Id = ISNULL(MAX(Id),0) + ROW_NUMBER() (TABLOCKX), ActivityDate = GETDATE(),
 * ClientIP = NULL, ActionId = 2, ActivityDescription = 'Approve "Grn (<reason>)" With doc No <DocNo>' /
 * 'UnApprove "Grn (<reason>)" With doc No <DocNo>', ScreenName = DocumentType.ScreenName,
 * RefDocumentTypeId = DocumentTypeId, ReferenceId = GRN Id.
 * Caller: id = maxId()+n then save(); or use GrnSpecialApprovalRepository.insertUserAuditForSpecialApproval
 * which keeps the INSERT ... SELECT verbatim.
 */
@Repository
public interface UserAuditRepository extends JpaRepository<UserAudit, Long> {

    /** USP_InvGrn_UpdateSpecialApproval: SELECT ISNULL(MAX(Id),0) FROM dbo.UserAudit */
    @Query("select coalesce(max(e.id),0L) from UserAudit e")
    Long maxId();

    List<UserAudit> findByRefDocumentTypeIdAndReferenceId(Integer refDocumentTypeId, Long referenceId);
}
