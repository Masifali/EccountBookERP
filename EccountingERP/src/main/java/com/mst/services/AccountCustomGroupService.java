package com.mst.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.AcLookUp;
import com.mst.repositories.IAcLookUpRepository;
import com.mst.repositories.IAccountsCustomGroupRepository;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.IAccountCustomGroupService;

@Service("accountCustomGroupService")
public class AccountCustomGroupService implements IAccountCustomGroupService {

	@Autowired
	private IAcLookUpRepository acLookUpRepository;
	@Autowired
	private IAccountsCustomGroupRepository accountsCustomGroupRepository;
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
		if (group.getId() == null) {
			group.setId(acLookUpRepository.findMaxId() + 1);
			group.setOrganizationId(currentUserContext.currentOrganizationId());
			group.setCompanyId(currentUserContext.currentCompanyId());
		}
		return acLookUpRepository.save(group);
	}

	@Override
	public void deleteGroup(int id) {
		// Real desktop guard: a custom group still assigned to accounts can't be removed
		// (dbo.AccountsCustomGroups.AcLookUpsId is not nullable, so deleting the parent
		// AcLookUps row would orphan those assignment rows).
		if (accountsCustomGroupRepository.countByAcLookUpsId(id) > 0) {
			return;
		}
		acLookUpRepository.deleteById(id);
	}
}
