package com.mst.controllers;

import com.mst.services.W5BBomProfile;
import com.mst.services.W5BBomService;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Production hub - screen 598 "Bill Of Material" (Architecture.WinApp.pcc.frmBillOfMaterialConcrete, DocumentTypeId 1851).
 * Page /production/pcc/bill-of-material, JSON under /production/pcc/bill-of-material/api. All handlers are inherited from {@link W5BBomControllerBase}.
 */
@Controller
@RequestMapping("/production/pcc/bill-of-material")
public class W5BBomConcreteController extends W5BBomControllerBase {
    public W5BBomConcreteController(W5BBomService service) { super(service, W5BBomProfile.CONCRETE); }
}
