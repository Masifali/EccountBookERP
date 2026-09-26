package com.mst.models.dto;

/**
 * Store Management Reports group A — the one write these three report screens can make:
 * 452 Store Purchase Demand Report's Complete / Cancel row buttons
 * (StorePurchaseDemandRegister.cs CompleteStatus:728 / CancelStatus:775).
 *
 * Public fields named exactly like the JSON the page posts. Organization, company and user are
 * never read from here — they come from the signed-in user.
 */
public class StoreReportsADto {

    /** POST /api/store/reports/store-purchase-demand-register/status */
    public static class DemandStatus {
        /** r.Cells["Id"] — the demand header id. */
        public Integer Id = 0;
        /** r.Cells["OrderDetailId"] — the grid row's detail; its ReceivedQty is re-derived server-side. */
        public Integer OrderDetailId = 0;
        /** "Complete" or "Cancel" — the button's column key. */
        public String ReqType;
        /** r.Cells["StatusRemarks"].Text — the grid cell the user typed into. */
        public String StatusRemarks;
        /** r.Cells["ReceivedQty"].Value — CancelStatus refuses when it is above zero. */
        public Double ReceivedQty = 0d;
        /** CmbApproved.Text at click time — Complete only acts when it is "Approved". */
        public String ApprovedText;
    }
}
