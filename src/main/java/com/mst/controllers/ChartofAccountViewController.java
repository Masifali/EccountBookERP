package com.mst.controllers;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ModelAttribute;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

import com.mst.models.ChartofAccount;
import com.mst.repositories.IChartofAccountRepository;
import com.mst.serviceInterface.IChartofAccountService;

@Controller
@RequestMapping("/accounts/chart_of_accounts")
public class ChartofAccountViewController {

	@Autowired
	private IChartofAccountRepository repository;

	@Autowired
	private IChartofAccountService service;

	@GetMapping
	public String viewChartOfAccounts(
			@RequestParam(value = "level1", required = false, defaultValue = "") String level1,
			@RequestParam(value = "level2", required = false, defaultValue = "") String level2,
			@RequestParam(value = "level3", required = false, defaultValue = "") String level3,
			@RequestParam(value = "editCode", required = false) String editCode,
			Model model) {

		List<ChartofAccount> allAccounts = service.getAllAccounts();
		List<ChartofAccount> level1List = service.getRootAccounts();

		List<ChartofAccount> level2List = new ArrayList<>();
		if (level1 != null && !level1.isEmpty()) {
			level2List = service.getAccountsByParent(level1);
		}

		List<ChartofAccount> level3List = new ArrayList<>();
		if (level2 != null && !level2.isEmpty()) {
			level3List = service.getAccountsByParent(level2);
		}

		List<ChartofAccount> level4List = new ArrayList<>();
		if (level3 != null && !level3.isEmpty()) {
			level4List = service.getAccountsByParent(level3);
		}


		ChartofAccount accountForm = new ChartofAccount();
		if (editCode != null && !editCode.isEmpty()) {
			ChartofAccount existing = repository.findByAccountCode(editCode);
			if (existing != null) {
				accountForm = existing;
			}
		} else {
			if (level3 != null && !level3.isEmpty()) {
				accountForm.setParentAccountCode(level3);
			} else if (level2 != null && !level2.isEmpty()) {
				accountForm.setParentAccountCode(level2);
			} else if (level1 != null && !level1.isEmpty()) {
				accountForm.setParentAccountCode(level1);
			}
		}

		model.addAttribute("allAccounts", allAccounts);
		model.addAttribute("level1List", level1List);
		model.addAttribute("level2List", level2List);
		model.addAttribute("level3List", level3List);
		model.addAttribute("level4List", level4List);
		model.addAttribute("selLevel1", level1);
		model.addAttribute("selLevel2", level2);
		model.addAttribute("selLevel3", level3);
		model.addAttribute("accountForm", accountForm);
		model.addAttribute("cities", service.getAllCities());
		model.addAttribute("customerGroups", service.getCustomerGroups());
		model.addAttribute("customGroups", service.getCustomGroups());
		model.addAttribute("accountTypes", service.getAccountTypes());

		return "accounts/chart_of_accounts";
	}

	@PostMapping("/save")
	public String saveAccount(@ModelAttribute("accountForm") ChartofAccount accountForm,
	                          @RequestParam(value = "level1", required = false, defaultValue = "") String level1,
	                          @RequestParam(value = "level2", required = false, defaultValue = "") String level2,
	                          @RequestParam(value = "level3", required = false, defaultValue = "") String level3) {
		service.save(accountForm);
		return "redirect:/accounts/chart_of_accounts?level1=" + level1 + "&level2=" + level2 + "&level3=" + level3;
	}

	@GetMapping("/delete/{code}")
	public String deleteAccount(@PathVariable("code") String code,
	                            @RequestParam(value = "level1", required = false, defaultValue = "") String level1,
	                            @RequestParam(value = "level2", required = false, defaultValue = "") String level2,
	                            @RequestParam(value = "level3", required = false, defaultValue = "") String level3) {
		ChartofAccount existing = repository.findByAccountCode(code);
		if (existing != null) {
			repository.delete(existing);
		}
		return "redirect:/accounts/chart_of_accounts?level1=" + level1 + "&level2=" + level2 + "&level3=" + level3;
	}
}
