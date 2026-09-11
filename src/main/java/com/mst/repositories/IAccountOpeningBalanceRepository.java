package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.AccountOpeningBalance;

public interface IAccountOpeningBalanceRepository extends JpaRepository<AccountOpeningBalance, Integer> {

	AccountOpeningBalance findByAccountCode(String accountCode);

	AccountOpeningBalance findByChartOfAccountId(Integer chartOfAccountId);

	@Query("select coalesce(max(a.id), 0) from AccountOpeningBalance a")
	int findMaxId();

	@Query(value = "EXEC [dbo].[Sp_AccountsOpeningBalance_Slip] :organizationId, :companyId, :financialYearId", nativeQuery = true)
	java.util.List<java.util.Map<String, Object>> getOpeningBalanceReportData(
			@org.springframework.data.repository.query.Param("organizationId") Integer organizationId,
			@org.springframework.data.repository.query.Param("companyId") Integer companyId,
			@org.springframework.data.repository.query.Param("financialYearId") Integer financialYearId);
}
