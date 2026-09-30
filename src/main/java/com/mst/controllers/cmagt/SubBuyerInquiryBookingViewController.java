package com.mst.controllers.cmagt;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

/**
 * Sub Buyer Inquiry Booking - port of
 * Architecture.WinApp.CommissionAgent.Transactions/frmSubBuyerInquiryBooking.cs.
 *
 * The desktop form is opened by frmBuyerInquiryBooking.openForm(Id) (:2635) after a new
 * save with ChkLink ticked, from the history grid's "Add Sub Party" button and from BtnLink,
 * and immediately calls ReadById(Id). It uses the same BLL/DAL (InquiryBookingMaster, 0492/0544)
 * and DocumentTypeId 1050, so the page talks to the existing
 * /api/commission/buyer-inquiry-booking endpoints; no new data path is introduced.
 */
@Controller
@RequestMapping("/commission")
public class SubBuyerInquiryBookingViewController {

    @GetMapping("/sub-buyer-inquiry-booking")
    public String subBuyerInquiryBooking(@RequestParam(value = "id", required = false) Integer id, Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Sub Buyer Inquiry Booking");
        model.addAttribute("inquiryId", id == null ? 0 : id);
        return "cmagt/sub_buyer_inquiry_booking";
    }
}
