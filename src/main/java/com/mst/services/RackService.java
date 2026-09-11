package com.mst.services;

import com.mst.security.CurrentUserContext;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.Rack;
import com.mst.repositories.IRackRepository;
import com.mst.serviceInterface.IRackService;

@Service("rackService")
public class RackService implements IRackService {

	@Autowired
	private IRackRepository rackRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<Rack> getAll() {
		return rackRepository.findAllByOrderBySortNo();
	}

	@Override
	public Rack getById(int id) {
		return rackRepository.findById(id).orElse(null);
	}

	@Override
	public Rack addOrUpdate(Rack rack) {
		LocalDateTime now = LocalDateTime.now();
		if (rack.getId() == null || rack.getId() == 0) {
			rack.setId(rackRepository.findMaxId() + 1);
			rack.setEntryDate(now);
			rack.setEntryUserId(currentUserContext.currentUserId());
		} else {
			rack.setModifyDate(now);
			rack.setModifyUserId(currentUserContext.currentUserId());
		}
		return rackRepository.save(rack);
	}

	@Override
	public void delete(int id) {
		rackRepository.deleteById(id);
	}
}
