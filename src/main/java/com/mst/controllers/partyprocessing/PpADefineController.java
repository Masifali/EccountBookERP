package com.mst.controllers.partyprocessing;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.partyprocessing.PpADefineService;
import com.mst.services.partyprocessing.PpAStockOpeningService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Party Processing definition pages of group PpA and their API (/api/party-processing/<page>/...).
 *
 *   683 /party-processing/define-item             DefineItemPartyProcessing.cs
 *   684 /party-processing/stock-party-define      frmPartyProcessingDefineSupplier.cs
 *   685 /party-processing/item-category           DefItemCatagoryPartyProcessing.cs
 *   686 /party-processing/item-type               DefPartyProcessingItemType.cs
 *   689 /party-processing/reference-parties       DefineReferenceParties.cs
 *   678 /party-processing/stock-opening-balance   StockOpeningBalancePartyProcessing.cs
 *
 * Page GETs only render; every API call checks the screen's View right (and Save / Update / Delete where the
 * desktop checks them) in the service.
 */
@Controller
public class PpADefineController {

    private static final String API = "/api/party-processing";

    @Autowired private PpADefineService define;
    @Autowired private PpAStockOpeningService opening;

    private static String page(Model model, String view) { model.addAttribute("activeMenu", "apps"); return view; }

    // ------------------------------------------------------------------ 686 Item Type

    @GetMapping("/party-processing/item-type")
    public String itemTypePage(Model model) { return page(model, "partyprocessing/item-type"); }

    @GetMapping(API + "/item-type/setup") @ResponseBody
    public ResponseEntity<?> itemTypeSetup() { return HrmApi.run(() -> define.itemTypeSetup()); }

    @GetMapping(API + "/item-type/list") @ResponseBody
    public ResponseEntity<?> itemTypeList() { return HrmApi.run(() -> define.itemTypeList()); }

    @GetMapping(API + "/item-type/by-id") @ResponseBody
    public ResponseEntity<?> itemType(@RequestParam("id") int id) { return HrmApi.run(() -> define.itemType(id)); }

    @PostMapping(API + "/item-type/save") @ResponseBody
    public ResponseEntity<?> itemTypeSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> define.saveItemType(b)); }

    // ------------------------------------------------------------------ 685 Item Category

    @GetMapping("/party-processing/item-category")
    public String itemCategoryPage(Model model) { return page(model, "partyprocessing/item-category"); }

    @GetMapping(API + "/item-category/setup") @ResponseBody
    public ResponseEntity<?> itemCategorySetup() { return HrmApi.run(() -> define.itemCategorySetup()); }

    @GetMapping(API + "/item-category/list") @ResponseBody
    public ResponseEntity<?> itemCategoryList() { return HrmApi.run(() -> define.itemCategoryList()); }

    @GetMapping(API + "/item-category/by-id") @ResponseBody
    public ResponseEntity<?> itemCategory(@RequestParam("id") int id) { return HrmApi.run(() -> define.itemCategory(id)); }

    @PostMapping(API + "/item-category/save") @ResponseBody
    public ResponseEntity<?> itemCategorySave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> define.saveItemCategory(b)); }

    // ------------------------------------------------------------------ 683 Define Item

    @GetMapping("/party-processing/define-item")
    public String itemPage(Model model) { return page(model, "partyprocessing/define-item"); }

    @GetMapping(API + "/define-item/setup") @ResponseBody
    public ResponseEntity<?> itemSetup() { return HrmApi.run(() -> define.itemSetup()); }

    @GetMapping(API + "/define-item/refresh") @ResponseBody
    public ResponseEntity<?> itemRefresh() { return HrmApi.run(() -> define.itemRefresh()); }

    @GetMapping(API + "/define-item/allocation") @ResponseBody
    public ResponseEntity<?> itemAllocation() { return HrmApi.run(() -> define.itemAllocation()); }

    @GetMapping(API + "/define-item/history") @ResponseBody
    public ResponseEntity<?> itemHistory(@RequestParam(value = "noOfRecords", defaultValue = "50") int n) { return HrmApi.run(() -> define.itemHistory(n)); }

    @GetMapping(API + "/define-item/code") @ResponseBody
    public ResponseEntity<?> itemCode(@RequestParam("categoryId") int categoryId) { return HrmApi.run(() -> define.itemCode(categoryId)); }

    @GetMapping(API + "/define-item/by-id") @ResponseBody
    public ResponseEntity<?> item(@RequestParam("id") int id) { return HrmApi.run(() -> define.item(id)); }

    @PostMapping(API + "/define-item/save") @ResponseBody
    public ResponseEntity<?> itemSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> define.saveItem(b)); }

    // ------------------------------------------------------------------ 689 Reference Parties

    @GetMapping("/party-processing/reference-parties")
    public String referencePage(Model model) { return page(model, "partyprocessing/reference-parties"); }

    @GetMapping(API + "/reference-parties/setup") @ResponseBody
    public ResponseEntity<?> referenceSetup() { return HrmApi.run(() -> define.referenceSetup()); }

    @GetMapping(API + "/reference-parties/refresh") @ResponseBody
    public ResponseEntity<?> referenceRefresh() { return HrmApi.run(() -> define.referenceRefresh()); }

    @GetMapping(API + "/reference-parties/list") @ResponseBody
    public ResponseEntity<?> referenceList() { return HrmApi.run(() -> define.referenceList()); }

    @GetMapping(API + "/reference-parties/by-id") @ResponseBody
    public ResponseEntity<?> referenceParty(@RequestParam("id") int id) { return HrmApi.run(() -> define.referenceParty(id)); }

    @PostMapping(API + "/reference-parties/save") @ResponseBody
    public ResponseEntity<?> referenceSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> define.saveReferenceParty(b)); }

    // ------------------------------------------------------------------ 684 Stock Party

    @GetMapping("/party-processing/stock-party-define")
    public String stockPartyPage(Model model) { return page(model, "partyprocessing/stock-party-define"); }

    @GetMapping(API + "/stock-party-define/setup") @ResponseBody
    public ResponseEntity<?> stockPartySetup() { return HrmApi.run(() -> define.stockPartySetup()); }

    @GetMapping(API + "/stock-party-define/list") @ResponseBody
    public ResponseEntity<?> stockPartyList() { return HrmApi.run(() -> define.stockPartyList()); }

    @GetMapping(API + "/stock-party-define/by-id") @ResponseBody
    public ResponseEntity<?> stockParty(@RequestParam("id") int id) { return HrmApi.run(() -> define.stockParty(id)); }

    @GetMapping(API + "/stock-party-define/by-gl") @ResponseBody
    public ResponseEntity<?> stockPartyByGl(@RequestParam("glAccountId") int gl) { return HrmApi.run(() -> define.stockPartyByGl(gl)); }

    @PostMapping(API + "/stock-party-define/save") @ResponseBody
    public ResponseEntity<?> stockPartySave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> define.saveStockParty(b)); }

    @GetMapping(API + "/stock-party-define/print-check") @ResponseBody
    public ResponseEntity<?> stockPartyPrintCheck(@RequestParam(value = "supplierCustomerId", defaultValue = "0") int id,
                                                  @RequestParam(value = "cityId", defaultValue = "0") int cityId,
                                                  @RequestParam(value = "countryId", defaultValue = "0") int countryId,
                                                  @RequestParam(value = "register", defaultValue = "false") boolean register) {
        return HrmApi.run(() -> define.stockPartyPrintCheck(id, cityId, countryId, register));
    }

    // ------------------------------------------------------------------ 678 Stock Opening Balance

    @GetMapping("/party-processing/stock-opening-balance")
    public String openingPage(Model model) { return page(model, "partyprocessing/stock-opening-balance"); }

    @GetMapping(API + "/stock-opening-balance/setup") @ResponseBody
    public ResponseEntity<?> openingSetup() { return HrmApi.run(() -> opening.setup()); }

    @GetMapping(API + "/stock-opening-balance/refresh") @ResponseBody
    public ResponseEntity<?> openingRefresh() { return HrmApi.run(() -> opening.refresh()); }

    @GetMapping(API + "/stock-opening-balance/doc-no") @ResponseBody
    public ResponseEntity<?> openingDocNo() { return HrmApi.run(() -> opening.newDocNo()); }

    @GetMapping(API + "/stock-opening-balance/uoms") @ResponseBody
    public ResponseEntity<?> openingUoms(@RequestParam("itemId") int itemId) { return HrmApi.run(() -> opening.uoms(itemId)); }

    @GetMapping(API + "/stock-opening-balance/by-id") @ResponseBody
    public ResponseEntity<?> openingRecord(@RequestParam("id") int id) { return HrmApi.run(() -> opening.record(id)); }

    @PostMapping(API + "/stock-opening-balance/save") @ResponseBody
    public ResponseEntity<?> openingSave(@RequestBody Map<String, Object> b) { return HrmApi.run(() -> opening.save(b)); }

    @PostMapping(API + "/stock-opening-balance/delete") @ResponseBody
    public ResponseEntity<?> openingDelete(@RequestBody Map<String, Object> b) {
        return HrmApi.run(() -> opening.delete(com.mst.services.hrm.HrmSupport.toInt(b.get("id"))));
    }

    @PostMapping(API + "/stock-opening-balance/history") @ResponseBody
    public ResponseEntity<?> openingHistory(@RequestBody Map<String, Object> f) { return HrmApi.run(() -> opening.history(f)); }

    @GetMapping(API + "/stock-opening-balance/print-check") @ResponseBody
    public ResponseEntity<?> openingPrintCheck(@RequestParam("id") int id) { return HrmApi.run(() -> opening.printCheck(id)); }
}
