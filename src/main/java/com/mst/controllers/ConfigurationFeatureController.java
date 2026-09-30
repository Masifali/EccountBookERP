package com.mst.controllers;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import com.mst.services.VoucherDesktopConfigService;

/**
 * Configuration.cs Configurations_Load() hides groups by ERP feature (CommonServices.GetERPFeatureById
 * 4 / 8 / 11); the page asks for the company's active feature ids once.
 */
@RestController
public class ConfigurationFeatureController {

	@Autowired
	private VoucherDesktopConfigService service;

	@GetMapping("/api/configurations/erp-features")
	public List<Integer> erpFeatures() {
		return service.erpFeatureIds();
	}
}
