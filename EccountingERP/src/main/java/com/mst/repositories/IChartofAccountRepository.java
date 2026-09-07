package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

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

	@Query(value = "SELECT DISTINCT c.* FROM ChartofAccount c " +
	       "LEFT JOIN AccountsCustomGroups g ON c.Id = g.ChartOfAccountId " +
	       "WHERE g.AcLookUpsId = 15 OR c.AccountGroup = 'Detail' " +
	       "ORDER BY c.AccountTitle", nativeQuery = true)
	List<ChartofAccount> findBankAccounts();
}



