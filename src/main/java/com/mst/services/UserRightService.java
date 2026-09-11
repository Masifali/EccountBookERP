package com.mst.services;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.constants.RightType;
import com.mst.models.Screen;
import com.mst.models.UserAccount;
import com.mst.models.UserRight;
import com.mst.repositories.IScreenRepository;
import com.mst.repositories.IUserAccountRepository;
import com.mst.repositories.IUserRightRepository;
import com.mst.serviceInterface.IUserRightService;

@Service
public class UserRightService implements IUserRightService {

	@Autowired
	private IUserRightRepository userRightRepository;
	@Autowired
	private IUserAccountRepository userAccountRepository;
	@Autowired
	private IScreenRepository screenRepository;

	@Override
	public Map<String, Boolean> getGrantsForUser(Integer userId) {
		Map<String, Boolean> grants = new HashMap<>();
		if (userId == null) {
			return grants;
		}
		for (UserRight right : userRightRepository.findByUserId(userId)) {
			grants.put(key(right.getScreen().getId(), right.getRightType()), right.getValue());
		}
		return grants;
	}

	@Override
	public boolean hasAccess(Integer userId, Integer screenId, RightType rightType) {
		if (userId == null || screenId == null || rightType == null) {
			return false;
		}
		return userRightRepository.existsByUserIdAndScreenIdAndRightTypeAndValueTrue(userId, screenId, rightType);
	}

	@Override
	public void saveGrantsForUser(Integer userId, List<String> grantedKeys) {
		UserAccount user = userAccountRepository.findById(userId).orElse(null);
		if (user == null) {
			return;
		}

		List<UserRight> existing = userRightRepository.findByUserId(userId);
		Map<String, UserRight> byKey = new HashMap<>();
		for (UserRight right : existing) {
			byKey.put(key(right.getScreen().getId(), right.getRightType()), right);
		}

		for (Screen screen : screenRepository.findAll()) {
			for (RightType rightType : RightType.values()) {
				String k = key(screen.getId(), rightType);
				boolean granted = grantedKeys != null && grantedKeys.contains(k);

				UserRight right = byKey.get(k);
				if (right == null) {
					if (!granted) {
						continue; // nothing to persist for an ungranted, never-saved combination
					}
					right = new UserRight();
					right.setUser(user);
					right.setScreen(screen);
					right.setRightType(rightType);
				}
				right.setValue(granted);
				userRightRepository.save(right);
			}
		}
	}

	private String key(Integer screenId, RightType rightType) {
		return screenId + ":" + rightType.name();
	}
}
