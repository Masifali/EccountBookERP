package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;

import com.mst.models.AcLookUp;
import com.mst.models.AccountTypes;
import com.mst.models.ChartofAccount;

public interface IAccountCustomGroupService {
	List<AcLookUp> getAllGroups();

	AcLookUp getGroupById(int id);

	AcLookUp addOrUpdateGroup(AcLookUp group);

	void deleteGroup(int id);

	List<AccountTypes> getAccountTypes();

	List<ChartofAccount> getParentAccounts(Integer accountTypeId);

	List<ChartofAccount> getParentAccounts();

	List<Map<String, Object>> getUnAllocatedData(int customGroupId, Integer accountTypeId, Integer parentAccountId);

	List<Map<String, Object>> getAllocatedData(int customGroupId, Integer accountTypeId, Integer parentAccountId);

	List<Map<String, Object>> getAllocatedAccounts(int groupId);

	List<Map<String, Object>> getUnAllocatedAccounts(int groupId);

	void allocateAccounts(int customGroupId, List<Integer> chartOfAccountIds);

	void unAllocateAccounts(int customGroupId, List<Integer> chartOfAccountIds);

	void unAllocateAccounts(int allocationId);
}

