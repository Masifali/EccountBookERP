package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.mst.models.VoucherDetail_H;

/**
 * Sp_VoucherDetail_H_Insert (single branch): a plain single-table
 * INSERT INTO VoucherDetail_H (...) VALUES (@...) followed by SELECT SCOPE_IDENTITY().
 * Callers use {@code save(entity)}; the generated Id is populated on the entity.
 * Pre-insert logic in the proc: none (no validations, no code generation).
 * The proc accepts but does NOT insert: @Id, @RefDocumentTypeId, @RefDocNoId, @RefDocNoDetailId,
 * @RefDocSubIdNo, @LineId, @ItemCgsRate, @TotalCreditAmount, @TotalDebitAmount, @SubNo, @ChequeStatus,
 * @PayeeTitle, @SubsidiaryTypeId, @EmployeeId, @SubsidiaryAccountId, @IsCGS, @SubsidiaryAgainstTypeId,
 * @SubsidiaryAgainstAccountId, the @ThirdCurrency... parameters, @SortNo, @PayeeOnly, @ChequeTypeId,
 * @InstrumentTypeId, @BranchesId, @CostCenterId, @SBRTaxAmount, @DiscountPercent, @DiscountAmount,
 * @ReferenceAccountId, @LocationTypeId, the @BaseFcy... parameters and @InventoryId.
 */
@Repository
public interface VoucherDetail_HRepository extends JpaRepository<VoucherDetail_H, Integer> {
}
