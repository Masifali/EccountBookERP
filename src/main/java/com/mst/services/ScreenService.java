package com.mst.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.Screen;
import com.mst.repositories.IScreenRepository;
import com.mst.serviceInterface.IScreenService;

@Service
public class ScreenService implements IScreenService {

	@Autowired
	private IScreenRepository screenRepository;

	@Override
	public List<Screen> getAll() {
		return screenRepository.findAllByOrderByModuleDescriptionAscScreenNameAsc();
	}
}
