package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.VoucherHead_H;

/**
 * Sp_VoucherHead_H_Insert (single branch): a plain single-table
 * INSERT INTO VoucherHead_H (...) VALUES (@...) followed by SELECT SCOPE_IDENTITY().
 * Callers use {@code save(entity)}; the generated Id is populated on the entity.
 * Pre-insert logic in the proc: none. Parameters @Id, @CostCenterAmount, @AttachmentsValues,
 * @RefDocumentTypeId, @CustomAttachmentsValues, @CustomAccounts, @IsUploaded, @UploadedDate,
 * @UploadedById, @InclusiveTax and @FixedAssetEntryTypeId are accepted but NOT inserted.
 */
@Repository
public interface VoucherHead_HRepository extends JpaRepository<VoucherHead_H, Integer> {
}
