package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;
import com.mst.messages.CompanyAllocationRow;
import com.mst.messages.CustomGroupAssignmentRow;
import com.mst.models.ChartofAccount;

public interface IChartofAccountService {
	List<ChartofAccount> getAllAccounts();
	List<ChartofAccount> getRootAccounts();
	List<ChartofAccount> getAccountsByParent(String parentAccountCode);
	List<ChartofAccount> getFilteredChildAccounts(String parentAccountCode, String accountType, String accountTitle);
	Map<String, List<ChartofAccount>> getCascadedAccountsByParent(String parentAccountCode);
	List<ChartofAccount> getDetailAccounts();
	ChartofAccount getByCode(String accountCode);
	ChartofAccount getById(Integer id);
	ChartofAccount save(ChartofAccount account);
	ChartofAccount save(ChartofAccount account, Double openingBalance);
	Double getOpeningBalance(Integer chartOfAccountId);
	void delete(String accountCode);
	boolean hasChildren(String accountCode);
	List<CompanyAllocationRow> getAllocations(Integer chartofAccountId);
	void setAllocation(Integer chartofAccountId, Integer companyId, boolean allocated);
	List<CustomGroupAssignmentRow> getCustomGroupAssignments(Integer chartofAccountId);
	void setCustomGroupAssignment(Integer chartofAccountId, Integer lookUpId, boolean assigned);
	List<ChartofAccount> searchAccounts(String query);
	List<ChartofAccount> getParentLookup();
	List<ChartofAccount> getHistoryAccounts(List<Integer> levels, String status, Integer customGroupId);
	ChartofAccount getNextChildCodeDetails(String parentAccountCode);
	List<com.mst.models.City> getAllCities();
	com.mst.models.City saveCity(String cityName, Integer tehsilId);
	List<com.mst.models.CustomerGroup> getCustomerGroups();
	List<com.mst.models.AcLookUp> getCustomGroups();
	List<com.mst.models.AccountTypes> getAccountTypes();
	List<ChartofAccount> getThirdLevelGroupAccountsByType(Integer accountTypeId);
	String getParentCodeByChartofAccountId(Integer chartofAccountId);
	List<ChartofAccount> getLevel4Accounts();
}

