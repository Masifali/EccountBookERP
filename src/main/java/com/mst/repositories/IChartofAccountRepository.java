package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import org.springframework.data.repository.query.Param;

import com.mst.models.ChartofAccount;

public interface IChartofAccountRepository extends JpaRepository<ChartofAccount, Integer> {

	ChartofAccount findByAccountCode(String accountCode);

	List<ChartofAccount> findByParentAccountCodeOrderByAccountTitle(String parentAccountCode);

	List<ChartofAccount> findByAccountGroupOrderByAccountTitle(String accountGroup);

	List<ChartofAccount> findAllByOrderByAccountCode();

	boolean existsByAccountCode(String accountCode);

	long countByParentAccountCode(String parentAccountCode);

	@Query("select coalesce(max(c.id), 0) from ChartofAccount c")
	int findMaxId();

	List<ChartofAccount> findByAccountLevelOrderByAccountTitle(Integer accountLevel);

	List<ChartofAccount> findByAccountTitleContainingIgnoreCaseOrAccountCodeContainingIgnoreCaseOrderByAccountTitle(String titleQuery, String codeQuery);

	List<ChartofAccount> findByAccountLevelInOrderByAccountCode(List<Integer> accountLevels);

	/**
	 * Ditto of the desktop's Sp_COAAllocation_GetAllMethod @Activity='Get3rdLevelGroupAccounts'
	 * (Architecture.BLL.Accounts.COAAllocation.Get3rdLevelGroupAccounts) - the level-3 Group
	 * account(s) carrying a given AccountTypeId. Backs the "Filter Accounts In Child Grid ->
	 * Account Type" dropdown, which REPLACES the Child Of Selected Parent grid with this result
	 * (see CmbAccountTypeFilter_Leave), independent of whichever Parent Account is selected.
	 */
	List<ChartofAccount> findByAccountLevelAndAccountGroupAndAccountTypeIdOrderByAccountCode(
			Integer accountLevel, String accountGroup, Integer accountTypeId);

	@Query(value = "SELECT DISTINCT c.* FROM ChartofAccount c " +
	       "LEFT JOIN AccountsCustomGroups g ON c.Id = g.ChartOfAccountId " +
	       "WHERE g.AcLookUpsId = 15 OR c.AccountGroup = 'Detail' " +
	       "ORDER BY c.AccountTitle", nativeQuery = true)
	List<ChartofAccount> findBankAccounts();

	@Query("SELECT c FROM ChartofAccount c WHERE " +
	       "(:parentCode IS NULL OR :parentCode = '' OR :parentCode = '0' OR c.parentAccountCode = :parentCode OR c.accountCode LIKE CONCAT(:parentCode, '%')) AND " +
	       "(:accountType IS NULL OR :accountType = '' OR LOWER(c.accountGroup) = LOWER(:accountType) OR CAST(c.accountTypeId AS string) = :accountType) AND " +
	       "(:accountTitle IS NULL OR :accountTitle = '' OR LOWER(c.accountTitle) LIKE LOWER(CONCAT('%', :accountTitle, '%')) OR LOWER(c.accountCode) LIKE LOWER(CONCAT('%', :accountTitle, '%'))) " +
	       "ORDER BY c.accountCode")
	List<ChartofAccount> findFilteredChildren(
			@Param("parentCode") String parentCode,
			@Param("accountType") String accountType,
			@Param("accountTitle") String accountTitle);
}



