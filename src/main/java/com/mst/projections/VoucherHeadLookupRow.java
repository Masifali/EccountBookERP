package com.mst.projections;

import java.time.LocalDate;
import java.time.LocalDateTime;

/** Row of the header lookup at the top of Sp_VoucherDetail_Insert (SELECT ... FROM VoucherHead WHERE Id = @VoucherHeadId). JPQL projection. */
public interface VoucherHeadLookupRow {
    Integer getOrganizationId();
    Integer getCompanyId();
    Integer getFinancialYearId();
    Integer getDocumentTypeId();
    Integer getDocumentTypeSrNo();
    LocalDate getVoucherDate();
    /** ISNULL(RefAccountId,0) -> @BankAccountId */
    Integer getBankAccountId();
    /** CheqId -> @ChequeId */
    Integer getChequeId();
    LocalDateTime getChequeDate();
}
