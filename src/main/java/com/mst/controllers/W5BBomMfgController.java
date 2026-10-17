package com.mst.controllers;

import com.mst.services.W5BBomProfile;
import com.mst.services.W5BBomService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Production hub - screen 774 "Bill Of Material" (Architecture.WinApp.Mfg.frmBillOfMaterial, DocumentTypeId 1617).
 * Page /production/mfg/bill-of-material, JSON under /production/mfg/bill-of-material/api. All handlers are inherited from {@link W5BBomControllerBase}.
 */
@Controller
@RequestMapping("/production/mfg/bill-of-material")
public class W5BBomMfgController extends W5BBomControllerBase {
    public W5BBomMfgController(W5BBomService service) { super(service, W5BBomProfile.MFG); }
}
