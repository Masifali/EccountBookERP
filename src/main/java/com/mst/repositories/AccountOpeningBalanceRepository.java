package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.AccountOpeningBalance;

/**
 * dbo.AccountsOpeningBalances - Sp_AccountsOpeningBalances_Insert (single-table INSERT -> save()).
 * Pre-insert logic of the proc, reproduced by callers:
 *  - Id = ISNULL(MAX(CONVERT(int,Id)),0)+1 -> {@link #maxId()} + 1;
 *  - FinancialYearId is overwritten with FinancialYear.Id of (CompanyId, OrganizationId) -> {@link #financialYearIdOf};
 *  - YearObDebit / YearObCredit are stored as ABS(value);
 *  - ModifyDate / ModifyUser are NOT written; AccountCode is not written by this proc;
 *  - the proc returns the new Id (SELECT @Id) - with save() the caller has it on the entity.
 */
@Repository
public interface AccountOpeningBalanceRepository extends JpaRepository<AccountOpeningBalance, Integer> {

    /** Sp_AccountsOpeningBalances_Insert: SELECT ISNULL(MAX(CONVERT(int,Id)),0) FROM AccountsOpeningBalances (caller adds 1). */
    @Query("select coalesce(max(e.id),0) from AccountOpeningBalance e")
    Integer maxId();

    /** Sp_AccountsOpeningBalances_Insert: SELECT Id FROM FinancialYear WHERE CompanyId=@CompanyId AND OrganizationId=@OrganizationId (no FinancialYear entity -> native; proc keeps the last row when several). */
    @Query(value = "SELECT Id FROM FinancialYear WHERE CompanyId = :companyId AND OrganizationId = :organizationId", nativeQuery = true)
    List<Integer> financialYearIdOf(@Param("companyId") Integer companyId, @Param("organizationId") Integer organizationId);
}
