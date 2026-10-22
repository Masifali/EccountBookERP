package com.mst.controllers.fixedassets;

import com.mst.controllers.hrm.HrmApi;
import com.mst.services.fixedassets.FaAssetsRegisterService;
import com.mst.services.fixedassets.FaPurchaseService;
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
 * Fixed Assets (dbo.App 9, AppModules 2032) pages and API.
 *
 *   353 /fixed-assets/assets-register       AssetSchema.frmAssetsRegister        API /api/fixed-assets/assets-register/...
 *   356 /fixed-assets/define-assets         FixedAsset.frmDefineAssets           API: the existing
 *       /api/store/define/fixed-asset-item/... (StoreDefineAssetsExtraController - a complete port of the same form,
 *       reused; its routes are not duplicated here)
 *   918 /fixed-assets/fixed-asset-purchase  Account_Definition.frmFixedAssetPurchase  API /api/fixed-assets/fixed-asset-purchase/...
 *
 * The page GETs only render the templates; every API call checks View on the screen (HrmSupport.user).
 */
@Controller
public class FixedAssetsController {


    @Autowired private FaAssetsRegisterService register;
    @Autowired private FaPurchaseService purchase;

    // ------------------------------------------------------------------ pages

    @GetMapping("/fixed-assets/assets-register")
    public String assetsRegisterPage(Model model) { model.addAttribute("activeMenu", "apps"); return "fixed_assets/assets-register"; }

    @GetMapping("/fixed-assets/define-assets")
    public String defineAssetsPage(Model model) { model.addAttribute("activeMenu", "apps"); return "fixed_assets/define-assets"; }

    @GetMapping("/fixed-assets/fixed-asset-purchase")
    public String purchasePage(Model model) { model.addAttribute("activeMenu", "apps"); return "fixed_assets/fixed-asset-purchase"; }

    // ------------------------------------------------------------------ 353 Assets Register

    @GetMapping("/api/fixed-assets/assets-register/setup") @ResponseBody
    public ResponseEntity<?> regSetup() { return HrmApi.run(() -> register.setup()); }

    @GetMapping("/api/fixed-assets/assets-register/items") @ResponseBody
    public ResponseEntity<?> regItems(@RequestParam("categoryId") int categoryId) { return HrmApi.run(() -> register.items(categoryId)); }

    @GetMapping("/api/fixed-assets/assets-register/serial-no") @ResponseBody
    public ResponseEntity<?> regSerial(@RequestParam("categoryId") int categoryId) { return HrmApi.run(() -> register.serialNo(categoryId)); }

    @GetMapping("/api/fixed-assets/assets-register/history") @ResponseBody
    public ResponseEntity<?> regHistory(@RequestParam(value = "dateType", required = false) String dateType,
                                        @RequestParam(value = "fromDate", required = false) String fromDate,
                                        @RequestParam(value = "toDate", required = false) String toDate) {
        return HrmApi.run(() -> register.history(dateType, fromDate, toDate));
    }

    @GetMapping("/api/fixed-assets/assets-register/by-id") @ResponseBody
    public ResponseEntity<?> regById(@RequestParam("id") long id) { return HrmApi.run(() -> register.asset(id)); }

    @PostMapping("/api/fixed-assets/assets-register/save") @ResponseBody
    public ResponseEntity<?> regSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> register.save(body)); }

    // ------------------------------------------------------------------ 918 Fixed Asset Purchase

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/setup") @ResponseBody
    public ResponseEntity<?> purSetup() { return HrmApi.run(() -> purchase.setup()); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/next-code") @ResponseBody
    public ResponseEntity<?> purNextCode() { return HrmApi.run(() -> purchase.nextCode()); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/history-accounts") @ResponseBody
    public ResponseEntity<?> purHistoryAccounts() { return HrmApi.run(() -> purchase.historyAccounts()); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/custom-accounts") @ResponseBody
    public ResponseEntity<?> purCustomAccounts(@RequestParam("customGroupId") int groupId) { return HrmApi.run(() -> purchase.customAccounts(groupId)); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/last-rate") @ResponseBody
    public ResponseEntity<?> purLastRate(@RequestParam("currencyId") int currencyId) { return HrmApi.run(() -> purchase.lastRate(currencyId)); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/tax-schedule") @ResponseBody
    public ResponseEntity<?> purTaxSchedule(@RequestParam("taxTypeId") int taxTypeId, @RequestParam(value = "date", required = false) String date) {
        return HrmApi.run(() -> purchase.taxSchedule(taxTypeId, date));
    }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/balance") @ResponseBody
    public ResponseEntity<?> purBalance(@RequestParam("accountId") int accountId, @RequestParam(value = "date", required = false) String date) {
        return HrmApi.run(() -> purchase.balance(accountId, date));
    }

    @PostMapping("/api/fixed-assets/fixed-asset-purchase/save") @ResponseBody
    public ResponseEntity<?> purSave(@RequestBody Map<String, Object> body) { return HrmApi.run(() -> purchase.save(body)); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/by-id") @ResponseBody
    public ResponseEntity<?> purById(@RequestParam("id") int id) { return HrmApi.run(() -> purchase.load(id)); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/history") @ResponseBody
    public ResponseEntity<?> purHistory(@RequestParam Map<String, String> q) { return HrmApi.run(() -> purchase.history(q)); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/history-detail") @ResponseBody
    public ResponseEntity<?> purHistoryDetail(@RequestParam("id") int id) { return HrmApi.run(() -> purchase.historyDetail(id)); }

    @GetMapping("/api/fixed-assets/fixed-asset-purchase/print-check") @ResponseBody
    public ResponseEntity<?> purPrintCheck(@RequestParam("id") int id) { return HrmApi.run(() -> purchase.printCheck(id)); }
}
