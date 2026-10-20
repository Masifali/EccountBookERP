package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import com.mst.models.DMSAttachments;
import com.mst.projections.DmsAttachmentJpqlRow;
import com.mst.projections.DmsAttachmentRow;

/**
 * dbo.DMSAttachments - Proc_DMSAttachments_Insert (plain single-table INSERT, Id IDENTITY, proc returns SCOPE_IDENTITY():
 * use save(), the id comes back on the entity; ModifyDate/ModifyUser are NOT written by the proc) and
 * Sp_DMSAttachments_GetAllMethod (@Activity branches). The proc's @IdsTable (TRY_CAST of fnSplitString(@Ids)) is inlined.
 */
@Repository
public interface DMSAttachmentsRepository extends JpaRepository<DMSAttachments, Integer> {

    /** Sp_DMSAttachments_GetAllMethod @Activity='ReadById' (by ScreenName, optional @Id = RefDocumentNo, optional CSV @Ids; native: fnSplitString). */
    @Query(value = "SELECT dms.[Id], dms.[RefAccountId], dms.[DMSFoldersLabelsId], dms.[RefDocumentTypeId], dms.[RefDocumentNo], dms.[Attachment], dms.[EntryDate], "
         + "dms.[EntryUser], dms.[ModifyDate], dms.[ModifyUser], dms.[OrganizationId], dms.[CompanyId], dms.[BranchId], dms.[ScreenName], dms.[DetailWiseAttachment], "
         + "dms.[UploadedFileCustomName], dms.[UploadedFileSizeMb], dms.[LineId], Ur.FirstName + ' ' + Ur.LastName AS EntryUserName, "
         + "dms.UploadedFileCustomName AS FileCustomName, '' AttachmentList "
         + "FROM DMSAttachments dms LEFT JOIN UserAccount Ur ON dms.EntryUser = Ur.ID "
         + "WHERE dms.ScreenName = :screenName AND (:id IS NULL OR dms.RefDocumentNo = :id) "
         + "AND (:ids IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:ids, ',') p WHERE TRY_CAST(p.data AS INT) = dms.RefDocumentNo))", nativeQuery = true)
    List<DmsAttachmentRow> readById(@Param("screenName") String screenName, @Param("id") Integer id, @Param("ids") String ids);

    /** Sp_DMSAttachments_GetAllMethod @Activity='ReadAttachmentsbyRefDocumentTypeId' (optional @Id, optional CSV @Ids; native: fnSplitString). */
    @Query(value = "SELECT dms.[Id], dms.[RefAccountId], dms.[DMSFoldersLabelsId], dms.[RefDocumentTypeId], dms.[RefDocumentNo], dms.[Attachment], dms.[EntryDate], "
         + "dms.[EntryUser], dms.[ModifyDate], dms.[ModifyUser], dms.[OrganizationId], dms.[CompanyId], dms.[BranchId], dms.[ScreenName], dms.[DetailWiseAttachment], "
         + "dms.[UploadedFileCustomName], dms.[UploadedFileSizeMb], dms.[LineId], Ur.FirstName + ' ' + Ur.LastName AS EntryUserName, "
         + "dms.UploadedFileCustomName AS FileCustomName, '' AttachmentList "
         + "FROM dbo.DMSAttachments dms LEFT JOIN UserAccount Ur ON dms.EntryUser = Ur.ID "
         + "WHERE (:id IS NULL OR dms.RefDocumentNo = :id) AND (dms.RefDocumentTypeId = :refDocumentTypeId) "
         + "AND (:ids IS NULL OR EXISTS (SELECT 1 FROM fnSplitString(:ids, ',') p WHERE TRY_CAST(p.data AS INT) = dms.RefDocumentNo))", nativeQuery = true)
    List<DmsAttachmentRow> readAttachmentsbyRefDocumentTypeId(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("ids") String ids);

    /** Sp_DMSAttachments_GetAllMethod @Activity='ReadAttachmentsbyRefDocumentTypeIdAndAccountId' (@AccountId is matched on LineId). */
    @Query("SELECT d.id AS id, d.refAccountId AS refAccountId, d.dmsFoldersLabelsId AS dmsFoldersLabelsId, d.refDocumentTypeId AS refDocumentTypeId, "
         + "d.refDocumentNo AS refDocumentNo, d.attachment AS attachment, d.entryDate AS entryDate, d.entryUser AS entryUser, d.modifyDate AS modifyDate, "
         + "d.modifyUser AS modifyUser, d.organizationId AS organizationId, d.companyId AS companyId, d.branchId AS branchId, d.screenName AS screenName, "
         + "d.detailWiseAttachment AS detailWiseAttachment, d.uploadedFileCustomName AS uploadedFileCustomName, d.uploadedFileSizeMb AS uploadedFileSizeMb, "
         + "d.lineId AS lineId, CONCAT(u.firstName, ' ', u.lastName) AS entryUserName, d.uploadedFileCustomName AS fileCustomName, '' AS attachmentList "
         + "FROM DMSAttachments d LEFT JOIN UserAccount u ON d.entryUser = u.id "
         + "WHERE d.refDocumentNo = :id AND d.refDocumentTypeId = :refDocumentTypeId AND d.lineId = :accountId")
    List<DmsAttachmentJpqlRow> readAttachmentsbyRefDocumentTypeIdAndAccountId(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("accountId") Integer accountId);

    /** Sp_DMSAttachments_GetAllMethod @Activity='DeleteById' (DELETE WHERE RefDocumentNo=@Id AND ScreenName=@ScreenName). */
    @Modifying
    @Transactional
    @Query("DELETE FROM DMSAttachments d WHERE d.refDocumentNo = :id AND d.screenName = :screenName")
    int deleteById(@Param("id") Integer id, @Param("screenName") String screenName);

    /** Sp_DMSAttachments_GetAllMethod @Activity='AttachmentDeleteById' (DELETE WHERE Id=@Id). */
    @Modifying
    @Transactional
    @Query("DELETE FROM DMSAttachments d WHERE d.id = :id")
    int attachmentDeleteById(@Param("id") Integer id);

    /** Sp_DMSAttachments_GetAllMethod @Activity='DetailDeleteById' (DELETE detail-wise attachments: ISNULL(DetailWiseAttachment,0)=1). */
    @Modifying
    @Transactional
    @Query("DELETE FROM DMSAttachments d WHERE d.refDocumentNo = :id AND d.screenName = :screenName AND COALESCE(d.detailWiseAttachment, false) = true")
    int detailDeleteById(@Param("id") Integer id, @Param("screenName") String screenName);

    /**
     * Sp_DMSAttachments_GetAllMethod @Activity='RemoveByIdAndName' - KEEP-EXEC: first blanks AttachmentsValues/CustomAttachmentsValues on the
     * owning document (VoucherHead, PurchaseOrder, InvGrn, InvPurchaseInvoice, InvStockOpeningBalanceHeader, SupplierDispatch, SaleOrder,
     * InvDeliveryOrder, InvGdn, InvSaleInvoice, mfg.BomHeader / WorkOrder / ProductionHeader / InvStockTransferHeader, ExImInvoice,
     * ExImLcOrderShipmentSchedule - chosen by @RefDocumentTypeId / @ScreenName), then deletes the DMSAttachments rows
     * (@ActionId NULL = all, 1 = header attachments only, 2 = detail attachments only). @AttachmentNames is accepted but unused.
     */
    // KEEP-EXEC: updates up to 16 different document tables (several without entity, two in schema mfg) before the delete
    @Modifying
    @Transactional
    @Query(value = "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id = :id, @ScreenName = :screenName, @RefDocumentTypeId = :refDocumentTypeId, "
         + "@AttachmentNames = :attachmentNames, @ActionId = :actionId, @Activity = 'RemoveByIdAndName'", nativeQuery = true)
    void removeByIdAndName(@Param("id") Integer id, @Param("screenName") String screenName, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("attachmentNames") String attachmentNames, @Param("actionId") Integer actionId);

    /** RemoveByIdAndName, final statement: the DMSAttachments delete alone (same @ActionId semantics as the proc). */
    @Modifying
    @Transactional
    @Query("DELETE FROM DMSAttachments d WHERE d.refDocumentNo = :id "
         + "AND ((:refDocumentTypeId > 0 AND d.refDocumentTypeId = :refDocumentTypeId) OR (d.screenName = :screenName)) "
         + "AND (:actionId IS NULL OR (:actionId = 1 AND COALESCE(d.detailWiseAttachment, false) = false) OR (:actionId = 2 AND COALESCE(d.detailWiseAttachment, false) = true))")
    int removeByIdAndNameDeleteAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId,
            @Param("screenName") String screenName, @Param("actionId") Integer actionId);

    /** RemoveByIdAndName step for @RefDocumentTypeId IN (1,2,3,4,5,6,7,8,10,26): blank VoucherHead attachment columns. */
    @Modifying
    @Transactional
    @Query("UPDATE VoucherHead h SET h.attachmentsValues = '', h.customAttachmentsValues = '' WHERE h.id = :id AND h.documentType.id = :refDocumentTypeId")
    int clearVoucherHeadAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** RemoveByIdAndName step for @RefDocumentTypeId = 41: blank PurchaseOrder attachment columns (native: columns not on the existing PurchaseOrder entity). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE PurchaseOrder SET AttachmentsValues = '', CustomAttachmentsValues = '' WHERE Id = :id AND DocumentTypeId = :refDocumentTypeId", nativeQuery = true)
    int clearPurchaseOrderAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** RemoveByIdAndName step for @RefDocumentTypeId = 46: blank InvGrn attachment columns (native: InvGrn entity owned by another batch). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE InvGrn SET AttachmentsValues = '', CustomAttachmentsValues = '' WHERE Id = :id AND DocumentTypeId = :refDocumentTypeId", nativeQuery = true)
    int clearInvGrnAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** RemoveByIdAndName step for @RefDocumentTypeId IN (56,57,59,64,131,138,245,702,1103,1104,1502,1503,1603,1604,1653,1654,1655): blank InvPurchaseInvoice attachment columns (native: columns not on the existing entity). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE InvPurchaseInvoice SET AttachmentsValues = '', CustomAttachmentsValues = '' WHERE Id = :id AND DocumentTypeId = :refDocumentTypeId", nativeQuery = true)
    int clearInvPurchaseInvoiceAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** RemoveByIdAndName step for @RefDocumentTypeId IN (39,40): blank InvStockOpeningBalanceHeader attachment columns (native: entity owned by another batch). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE invstockopeningbalanceheader SET AttachmentsValues = '', CustomAttachmentsValues = '' WHERE Id = :id AND DocumentTypeId = :refDocumentTypeId", nativeQuery = true)
    int clearInvStockOpeningBalanceHeaderAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId);

    /** RemoveByIdAndName step for @RefDocumentTypeId IN (81,1656): blank SaleOrder attachment columns (native: entity owned by another batch). */
    @Modifying
    @Transactional
    @Query(value = "UPDATE SaleOrder SET AttachmentsValues = '', CustomAttachmentsValues = '' WHERE Id = :id AND DocumentTypeId = :refDocumentTypeId", nativeQuery = true)
    int clearSaleOrderAttachments(@Param("id") Integer id, @Param("refDocumentTypeId") Integer refDocumentTypeId);
}
