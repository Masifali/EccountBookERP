package com.mst.controllers;

import com.mst.services.MasterDataDefinitionService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;

/**
 * The nine screens of dbo.App 19 "Master Data Definition" / AppModules 2039 "System_Level"
 * (Architecture.WinApp). The hub itself is AppMenuController.masterDataDefinition().
 *
 *   749 /master-data/country      DefineCountry          752 /master-data/sea-ports   SeaPortsDefine
 *   753 /master-data/province     DefineProvince         754 /master-data/date-lock   DateLock
 *   756 /master-data/district     DefineDistrict         742 /master-data/other-items InvOtherItems
 *   755 /master-data/tehsil       DefineTehsil
 *   750 /master-data/city         DefineCity
 *   751 /master-data/currency     DefineMultiCurrency
 *
 * API: /api/master-data/{key}/{setup|list|by-id|save} (+ /districts on tehsil and /tehsils on city
 * for their Refresh buttons). No parameter carries tenancy or a user id.
 */
@Controller
public class MasterDataDefinitionController {

    private static final String API = "/api/master-data";

    @Autowired private MasterDataDefinitionService service;

    // ------------------------------------------------------------------ pages

    @GetMapping("/master-data/{page:country|province|district|tehsil|city|currency|sea-ports|date-lock|other-items}")
    public String page(@PathVariable("page") String page, Model model) {
        model.addAttribute("activeMenu", "apps");
        return "master_data/" + page.replace('-', '_');
    }

    // ------------------------------------------------------------------ setup / list / by-id

    @GetMapping(API + "/{key:country|province|district|tehsil|city|currency|sea-ports|date-lock|other-items}/setup")
    @ResponseBody
    public ResponseEntity<?> setup(@PathVariable("key") String key) {
        try {
            switch (key) {
                case "country":     return ResponseEntity.ok(service.countries());
                case "province":    return ResponseEntity.ok(service.provinceSetup());
                case "district":    return ResponseEntity.ok(service.districtSetup());
                case "tehsil":      return ResponseEntity.ok(service.tehsilSetup());
                case "city":        return ResponseEntity.ok(service.citySetup());
                case "currency":    return ResponseEntity.ok(service.currencies());
                case "sea-ports":   return ResponseEntity.ok(service.seaPortSetup());
                case "date-lock":   return ResponseEntity.ok(service.dateLockSetup());
                case "other-items": return ResponseEntity.ok(service.otherItemSetup());
                default: return ResponseEntity.notFound().build();
            }
        } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/{key:country|province|district|tehsil|city|currency|sea-ports|date-lock|other-items}/list")
    @ResponseBody
    public ResponseEntity<?> list(@PathVariable("key") String key) {
        try {
            switch (key) {
                case "country":     return ResponseEntity.ok(service.countries());
                case "province":    return ResponseEntity.ok(service.provinces());
                case "district":    return ResponseEntity.ok(service.districts());
                case "tehsil":      return ResponseEntity.ok(service.tehsils());
                case "currency":    return ResponseEntity.ok(service.currencies());
                case "sea-ports":   return ResponseEntity.ok(service.seaPorts());
                case "date-lock":   return ResponseEntity.ok(service.dateLocks());
                case "other-items": return ResponseEntity.ok(service.otherItems());
                default: return ResponseEntity.notFound().build();
            }
        } catch (Exception e) { return error(e, "Load failed."); }
    }

    @GetMapping(API + "/{key:country|province|district|tehsil|city|currency|sea-ports|date-lock|other-items}/by-id")
    @ResponseBody
    public ResponseEntity<?> byId(@PathVariable("key") String key, @RequestParam("id") int id) {
        try {
            switch (key) {
                case "country":     return ResponseEntity.ok(service.country(id));
                case "province":    return ResponseEntity.ok(service.province(id));
                case "currency":    return ResponseEntity.ok(service.currency(id));
                case "sea-ports":   return ResponseEntity.ok(service.seaPort(id));
                case "date-lock":   return ResponseEntity.ok(service.dateLock(id));
                case "other-items": return ResponseEntity.ok(service.otherItem(id));
                default: return ResponseEntity.notFound().build();
            }
        } catch (IllegalArgumentException e) {
            return ResponseEntity.badRequest().body(fail(root(e, "Record not found.")));
        } catch (Exception e) { return error(e, "Load failed."); }
    }

    /** DefineTehsil.btnRefresh_Click - DistrictBind. */
    @GetMapping(API + "/tehsil/districts")
    @ResponseBody
    public ResponseEntity<?> tehsilDistricts() {
        try { return ResponseEntity.ok(service.districtsForTehsil()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    /** DefineCity.btnRefresh_Click - TehsilBind. */
    @GetMapping(API + "/city/tehsils")
    @ResponseBody
    public ResponseEntity<?> cityTehsils() {
        try { return ResponseEntity.ok(service.tehsilsForCity()); } catch (Exception e) { return error(e, "Load failed."); }
    }

    // ------------------------------------------------------------------ save

    @PostMapping(API + "/{key:country|province|district|tehsil|city|currency|sea-ports|date-lock|other-items}/save")
    @ResponseBody
    public ResponseEntity<?> save(@PathVariable("key") String key, @RequestBody Map<String, Object> body) {
        try {
            switch (key) {
                case "country":     return ResponseEntity.ok(service.saveCountry(body));
                case "province":    return ResponseEntity.ok(service.saveProvince(body));
                case "district":    return ResponseEntity.ok(service.saveDistrict(body));
                case "tehsil":      return ResponseEntity.ok(service.saveTehsil(body));
                case "city":        return ResponseEntity.ok(service.saveCity(body));
                case "currency":    return ResponseEntity.ok(service.saveCurrency(body));
                case "sea-ports":   return ResponseEntity.ok(service.saveSeaPort(body));
                case "date-lock":   return ResponseEntity.ok(service.saveDateLock(body));
                case "other-items": return ResponseEntity.ok(service.saveOtherItem(body));
                default: return ResponseEntity.notFound().build();
            }
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, "Save failed.")));
        } catch (Exception e) { return error(e, "Save failed."); }
    }

    // ------------------------------------------------------------------ plumbing

    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static ResponseEntity<Map<String, Object>> error(Exception e, String fallback) {
        if (e instanceof AccessDeniedException) return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
