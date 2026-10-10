package com.mst.controllers.sale.pcc;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Pages of 548 SaleOrderConcrete, 551 GoodsDispatchNotesConcrete, 552 GdnDirectConcrete (Architecture.WinApp.pcc.Sale.*). */
@Controller
@RequestMapping("/sale/pcc")
public class SalePccP2ViewController {

    private String page(Model model, String title, String template) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", title);
        return template;
    }

    @GetMapping("/sale-order")
    public String saleOrder(Model model) { return page(model, "Sale Order", "sale/pcc/sale_order_concrete"); }

    @GetMapping("/goods-dispatch-note")
    public String goodsDispatchNote(Model model) { return page(model, "Goods Dispatch Note", "sale/pcc/gdn_concrete"); }

    @GetMapping("/gdn-direct-concrete")
    public String gdnDirectConcrete(Model model) { return page(model, "Gdn Direct Concrete", "sale/pcc/gdn_direct_concrete"); }
}
