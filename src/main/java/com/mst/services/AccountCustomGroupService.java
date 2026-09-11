package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.util.Collections;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import com.mst.models.AcLookUp;
import com.mst.models.AccountTypes;
import com.mst.models.ChartofAccount;
import com.mst.repositories.IAcLookUpRepository;
import com.mst.repositories.IAccountTypesRepository;
import com.mst.repositories.IAccountsCustomGroupRepository;
import com.mst.repositories.IChartofAccountRepository;
import com.mst.serviceInterface.IAccountCustomGroupService;

@Service("accountCustomGroupService")
public class AccountCustomGroupService implements IAccountCustomGroupService {

	@Autowired
	private JdbcTemplate jdbcTemplate;
	@Autowired
	private IAcLookUpRepository acLookUpRepository;
	@Autowired
	private IAccountsCustomGroupRepository accountsCustomGroupRepository;
	@Autowired
	private IAccountTypesRepository accountTypesRepository;
	@Autowired
	private IChartofAccountRepository chartofAccountRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<AcLookUp> getAllGroups() {
		return acLookUpRepository.findByAcLookUpTypesIdOrderByAcLookUpsDescription(AcLookUp.ACCOUNTS_CUSTOM_GROUP_TYPE_ID);
	}

	@Override
	public AcLookUp getGroupById(int id) {
		AcLookUp group = acLookUpRepository.findById(id).orElse(null);
		return group != null && AcLookUp.ACCOUNTS_CUSTOM_GROUP_TYPE_ID == group.getAcLookUpTypesId() ? group : null;
	}

	@Override
	public AcLookUp addOrUpdateGroup(AcLookUp group) {
		group.setAcLookUpTypesId(AcLookUp.ACCOUNTS_CUSTOM_GROUP_TYPE_ID);
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		group.setOrganizationId(orgId);
		group.setCompanyId(compId);

		if (group.getId() == null || group.getId() == 0) {
			group.setId(acLookUpRepository.findMaxId() + 1);
		}
		return acLookUpRepository.save(group);
	}

	@Override
	public void deleteGroup(int id) {
		if (accountsCustomGroupRepository.countByAcLookUpsId(id) > 0) {
			throw new IllegalStateException("Cannot delete group because accounts are currently allocated to it.");
		}
		acLookUpRepository.deleteById(id);
	}

	@Override
	public List<AccountTypes> getAccountTypes() {
		try {
			return accountTypesRepository.findAllByOrderById();
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	@Override
	public List<ChartofAccount> getParentAccounts(Integer accountTypeId) {
		try {
			List<ChartofAccount> list = chartofAccountRepository.findByAccountLevelOrderByAccountTitle(3);
			if (accountTypeId != null && accountTypeId > 0) {
				return list.stream()
						.filter(a -> a.getAccountTypeId() != null && a.getAccountTypeId().equals(accountTypeId))
						.toList();
			}
			return list;
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	@Override
	public List<Map<String, Object>> getUnAllocatedData(int customGroupId, Integer accountTypeId, Integer parentAccountId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int accTypeId = (accountTypeId != null) ? accountTypeId : 0;
		int parentAccId = (parentAccountId != null) ? parentAccountId : 0;

		try {
			String sql = "EXEC [dbo].[USP_AccountCustomGroup_UnAllocatedData] @OrganizationId=?, @CompanyId=?, @CustomGroupId=?, @AccountTypeId=?, @ParentAccountId=?";
			return jdbcTemplate.queryForList(sql, orgId, compId, customGroupId, accTypeId, parentAccId);
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	@Override
	public List<Map<String, Object>> getAllocatedData(int customGroupId, Integer accountTypeId, Integer parentAccountId) {
		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int accTypeId = (accountTypeId != null) ? accountTypeId : 0;
		int parentAccId = (parentAccountId != null) ? parentAccountId : 0;

		try {
			String sql = "EXEC [dbo].[USP_AccountCustomGroup_AllocatedData] @OrganizationId=?, @CompanyId=?, @CustomGroupId=?, @AccountTypeId=?, @ParentAccountId=?";
			return jdbcTemplate.queryForList(sql, orgId, compId, customGroupId, accTypeId, parentAccId);
		} catch (Exception ex) {
			return Collections.emptyList();
		}
	}

	@Override
	public void allocateAccounts(int customGroupId, List<Integer> chartOfAccountIds) {
		if (chartOfAccountIds == null || chartOfAccountIds.isEmpty()) return;

		int orgId = currentUserContext.currentOrganizationId();
		int compId = currentUserContext.currentCompanyId();
		int userId = currentUserContext.currentUserId();

		for (Integer coaId : chartOfAccountIds) {
			try {
				String sql = "EXEC [dbo].[Sp_AccountsCustomGroups_Insert] @ChartOfAccountId=?, @AcLookUpsId=?, @organizationId=?, @companyId=?, @EntryUserId=?, @ModifyUserId=?";
				jdbcTemplate.update(sql, coaId, customGroupId, orgId, compId, userId, userId);
			} catch (Exception ex) {
				// Prevent duplicate allocation failure
				try {
					String directInsert = "INSERT INTO AccountsCustomGroups (AcLookUpsId, ChartOfAccountId, SortNo, EntryDate, EntryUserId, organizationId, companyId) " +
							"SELECT ?, ?, ISNULL(MAX(SortNo), 0) + 1, GETDATE(), ?, ?, ? FROM AccountsCustomGroups";
					jdbcTemplate.update(directInsert, customGroupId, coaId, userId, orgId, compId);
				} catch (Exception ignored) {}
			}
		}
	}

	@Override
	public List<ChartofAccount> getParentAccounts() {
		return getParentAccounts(0);
	}

	@Override
	public List<Map<String, Object>> getAllocatedAccounts(int groupId) {
		return getAllocatedData(groupId, 0, 0);
	}

	@Override
	public List<Map<String, Object>> getUnAllocatedAccounts(int groupId) {
		return getUnAllocatedData(groupId, 0, 0);
	}

	@Override
	public void unAllocateAccounts(int customGroupId, List<Integer> chartOfAccountIds) {
		if (chartOfAccountIds == null || chartOfAccountIds.isEmpty()) return;

		for (Integer coaId : chartOfAccountIds) {
			try {
				String sql = "EXEC [dbo].[Sp_ChartofAccount_GetAllMethodFromCOA] @CoaType='DeleteAccountsFromCustomGroup', @AcLookUpsId=?, @Id=?";
				jdbcTemplate.update(sql, customGroupId, coaId);
			} catch (Exception ex) {
				try {
					String directDelete = "DELETE FROM AccountsCustomGroups WHERE AcLookUpsId = ? AND ChartOfAccountId = ?";
					jdbcTemplate.update(directDelete, customGroupId, coaId);
				} catch (Exception ignored) {}
			}
		}
	}

	@Override
	public void unAllocateAccounts(int allocationId) {
		try {
			String directDelete = "DELETE FROM AccountsCustomGroups WHERE Id = ? OR ChartOfAccountId = ?";
			jdbcTemplate.update(directDelete, allocationId, allocationId);
		} catch (Exception ignored) {}
	}
}


