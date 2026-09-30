package com.mst.controllers.hrm;

import com.mst.models.hrm.dto.HrmNameDto;
import com.mst.models.hrm.dto.ProfileBenefitDto;
import com.mst.models.hrm.dto.ProfileCodeNameDto;
import com.mst.models.hrm.dto.ProfileDefineDto;
import com.mst.models.hrm.dto.ProfileDepartmentDto;
import com.mst.models.hrm.dto.ProfileLocationDto;
import com.mst.models.hrm.dto.ProfileSectionDto;
import com.mst.services.hrm.HrmProfileService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

/**
 * HRM "Profile Management" (AppModules 2018) pages and API.
 *
 *   634 /hrm/employee-group     genEmployeeGroup.cs     API /api/hrm/profile/employee-group/{setup|list|by-id|save}
 *   635 /hrm/employee-category  frmEmployeeCategory.cs  API /api/hrm/profile/employee-category/{setup|list|by-id|save}
 *   636 /hrm/benefit            BenifitDefine.cs        API /api/hrm/profile/benefit/{setup|list|combos|by-id|save}
 *   637 /hrm/designation        genDesignation.cs       API /api/hrm/profile/designation/{setup|list|by-id|save}
 *   638 /hrm/department         genDepartment.cs        API /api/hrm/profile/department/{setup|list|combos|by-id|save}
 *   639 /hrm/section            DefineSection.cs        API /api/hrm/profile/section/{setup|list|by-id|save}
 *   640 /hrm/location           frmgenLocation.cs       API /api/hrm/profile/location/{setup|list|combos|by-id|save}
 *   641 /hrm/profile-type       ProfileTypes.cs         API /api/hrm/profile/profile-type/{setup|list|by-id|save}
 *   642 /hrm/define-profile     ProfileDefine.cs        API /api/hrm/profile/define-profile/{setup|list|by-id|save}  (?profileTypeId= preselects the type)
 */
@Controller
public class HrmProfileController {

    private static final String API = "/api/hrm/profile";

    @Autowired private HrmProfileService service;

    private static String page(Model model, String view) { model.addAttribute("activeMenu", "apps"); return "hrm/profile/" + view; }

    // ------------------------------------------------------------------ 637 Designation

    @GetMapping("/hrm/designation")
    public String designationPage(Model model) { return page(model, "designation"); }

    @GetMapping(API + "/designation/setup") @ResponseBody
    public ResponseEntity<?> designationSetup() { return HrmApi.run(() -> service.designationSetup()); }

    @GetMapping(API + "/designation/list") @ResponseBody
    public ResponseEntity<?> designationList() { return HrmApi.run(() -> service.designations()); }

    @GetMapping(API + "/designation/by-id") @ResponseBody
    public ResponseEntity<?> designationById(@RequestParam("id") int id) { return HrmApi.run(() -> service.designation(id)); }

    @PostMapping(API + "/designation/save") @ResponseBody
    public ResponseEntity<?> designationSave(@RequestBody HrmNameDto body) { return HrmApi.run(() -> service.saveDesignation(body)); }

    // ------------------------------------------------------------------ 634 Employee Group

    @GetMapping("/hrm/employee-group")
    public String employeeGroupPage(Model model) { return page(model, "employee-group"); }

    @GetMapping(API + "/employee-group/setup") @ResponseBody
    public ResponseEntity<?> employeeGroupSetup() { return HrmApi.run(() -> service.employeeGroupSetup()); }

    @GetMapping(API + "/employee-group/list") @ResponseBody
    public ResponseEntity<?> employeeGroupList() { return HrmApi.run(() -> service.employeeGroups()); }

    @GetMapping(API + "/employee-group/by-id") @ResponseBody
    public ResponseEntity<?> employeeGroupById(@RequestParam("id") int id) { return HrmApi.run(() -> service.employeeGroup(id)); }

    @PostMapping(API + "/employee-group/save") @ResponseBody
    public ResponseEntity<?> employeeGroupSave(@RequestBody ProfileCodeNameDto body) { return HrmApi.run(() -> service.saveEmployeeGroup(body)); }

    // ------------------------------------------------------------------ 635 Employee Category

    @GetMapping("/hrm/employee-category")
    public String employeeCategoryPage(Model model) { return page(model, "employee-category"); }

    @GetMapping(API + "/employee-category/setup") @ResponseBody
    public ResponseEntity<?> employeeCategorySetup() { return HrmApi.run(() -> service.employeeCategorySetup()); }

    @GetMapping(API + "/employee-category/list") @ResponseBody
    public ResponseEntity<?> employeeCategoryList() { return HrmApi.run(() -> service.employeeCategories()); }

    @GetMapping(API + "/employee-category/by-id") @ResponseBody
    public ResponseEntity<?> employeeCategoryById(@RequestParam("id") int id) { return HrmApi.run(() -> service.employeeCategory(id)); }

    @PostMapping(API + "/employee-category/save") @ResponseBody
    public ResponseEntity<?> employeeCategorySave(@RequestBody ProfileCodeNameDto body) { return HrmApi.run(() -> service.saveEmployeeCategory(body)); }

    // ------------------------------------------------------------------ 636 Benefit

    @GetMapping("/hrm/benefit")
    public String benefitPage(Model model) { return page(model, "benefit"); }

    @GetMapping(API + "/benefit/setup") @ResponseBody
    public ResponseEntity<?> benefitSetup() { return HrmApi.run(() -> service.benefitSetup()); }

    @GetMapping(API + "/benefit/list") @ResponseBody
    public ResponseEntity<?> benefitList() { return HrmApi.run(() -> service.benefits()); }

    @GetMapping(API + "/benefit/combos") @ResponseBody
    public ResponseEntity<?> benefitCombos() { return HrmApi.run(() -> service.benefitCombos()); }

    @GetMapping(API + "/benefit/by-id") @ResponseBody
    public ResponseEntity<?> benefitById(@RequestParam("id") int id) { return HrmApi.run(() -> service.benefit(id)); }

    @PostMapping(API + "/benefit/save") @ResponseBody
    public ResponseEntity<?> benefitSave(@RequestBody ProfileBenefitDto body) { return HrmApi.run(() -> service.saveBenefit(body)); }

    // ------------------------------------------------------------------ 638 Department

    @GetMapping("/hrm/department")
    public String departmentPage(Model model) { return page(model, "department"); }

    @GetMapping(API + "/department/setup") @ResponseBody
    public ResponseEntity<?> departmentSetup() { return HrmApi.run(() -> service.departmentSetup()); }

    @GetMapping(API + "/department/list") @ResponseBody
    public ResponseEntity<?> departmentList() { return HrmApi.run(() -> service.departments()); }

    @GetMapping(API + "/department/combos") @ResponseBody
    public ResponseEntity<?> departmentCombos() { return HrmApi.run(() -> service.departmentCombos()); }

    @GetMapping(API + "/department/by-id") @ResponseBody
    public ResponseEntity<?> departmentById(@RequestParam("id") int id) { return HrmApi.run(() -> service.department(id)); }

    @PostMapping(API + "/department/save") @ResponseBody
    public ResponseEntity<?> departmentSave(@RequestBody ProfileDepartmentDto body) { return HrmApi.run(() -> service.saveDepartment(body)); }

    // ------------------------------------------------------------------ 639 Section

    @GetMapping("/hrm/section")
    public String sectionPage(Model model) { return page(model, "section"); }

    @GetMapping(API + "/section/setup") @ResponseBody
    public ResponseEntity<?> sectionSetup() { return HrmApi.run(() -> service.sectionSetup()); }

    @GetMapping(API + "/section/list") @ResponseBody
    public ResponseEntity<?> sectionList() { return HrmApi.run(() -> service.sections()); }

    @GetMapping(API + "/section/by-id") @ResponseBody
    public ResponseEntity<?> sectionById(@RequestParam("id") int id) { return HrmApi.run(() -> service.section(id)); }

    @PostMapping(API + "/section/save") @ResponseBody
    public ResponseEntity<?> sectionSave(@RequestBody ProfileSectionDto body) { return HrmApi.run(() -> service.saveSection(body)); }

    // ------------------------------------------------------------------ 640 Location

    @GetMapping("/hrm/location")
    public String locationPage(Model model) { return page(model, "location"); }

    @GetMapping(API + "/location/setup") @ResponseBody
    public ResponseEntity<?> locationSetup() { return HrmApi.run(() -> service.locationSetup()); }

    @GetMapping(API + "/location/list") @ResponseBody
    public ResponseEntity<?> locationList() { return HrmApi.run(() -> service.locations()); }

    @GetMapping(API + "/location/combos") @ResponseBody
    public ResponseEntity<?> locationCombos() { return HrmApi.run(() -> service.locationCombos()); }

    @GetMapping(API + "/location/by-id") @ResponseBody
    public ResponseEntity<?> locationById(@RequestParam("id") int id) { return HrmApi.run(() -> service.location(id)); }

    @PostMapping(API + "/location/save") @ResponseBody
    public ResponseEntity<?> locationSave(@RequestBody ProfileLocationDto body) { return HrmApi.run(() -> service.saveLocation(body)); }

    // ------------------------------------------------------------------ 641 Profile Type

    @GetMapping("/hrm/profile-type")
    public String profileTypePage(Model model) { return page(model, "profile-type"); }

    @GetMapping(API + "/profile-type/setup") @ResponseBody
    public ResponseEntity<?> profileTypeSetup() { return HrmApi.run(() -> service.profileTypeSetup()); }

    @GetMapping(API + "/profile-type/list") @ResponseBody
    public ResponseEntity<?> profileTypeList() { return HrmApi.run(() -> service.profileTypes()); }

    @GetMapping(API + "/profile-type/by-id") @ResponseBody
    public ResponseEntity<?> profileTypeById(@RequestParam("id") int id) { return HrmApi.run(() -> service.profileType(id)); }

    @PostMapping(API + "/profile-type/save") @ResponseBody
    public ResponseEntity<?> profileTypeSave(@RequestBody ProfileCodeNameDto body) { return HrmApi.run(() -> service.saveProfileType(body)); }

    // ------------------------------------------------------------------ 642 Define Profile

    @GetMapping("/hrm/define-profile")
    public String defineProfilePage(Model model) { return page(model, "define-profile"); }

    @GetMapping(API + "/define-profile/setup") @ResponseBody
    public ResponseEntity<?> defineProfileSetup() { return HrmApi.run(() -> service.profileSetup()); }

    @GetMapping(API + "/define-profile/list") @ResponseBody
    public ResponseEntity<?> defineProfileList() { return HrmApi.run(() -> service.profiles()); }

    @GetMapping(API + "/define-profile/by-id") @ResponseBody
    public ResponseEntity<?> defineProfileById(@RequestParam("id") int id) { return HrmApi.run(() -> service.profile(id)); }

    @PostMapping(API + "/define-profile/save") @ResponseBody
    public ResponseEntity<?> defineProfileSave(@RequestBody ProfileDefineDto body) { return HrmApi.run(() -> service.saveProfile(body)); }
}
