package com.mst.models.imports;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

import java.util.ArrayList;
import java.util.List;

/**
 * Request bodies of the Import pages 525 / 526 / 221 (screens owned by the ImpA* classes). Numbers that the
 * desktop reads from a TextBox arrive as the text the page shows, so the service can apply the desktop's
 * Conversion.ToInt / ToDecimal semantics to exactly that text.
 */
public final class ImpADtos {

    private ImpADtos() { }

    // ------------------------------------------------------------------ 525 ImLcOrder

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class LcOrderDetail {
        public int id;
        public int itemId;
        public String description;
        public int packingTypeId;
        public int itemUomId;
        public String qty;
        public String itemRate;
        public int rateUomId;
        public String remarks;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class LcOrderPayment {
        public int id;
        public int paymentTermId;
        public String percent;
        public String fcyAmount;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class LcOrder {
        public int id;
        /** BtnSaveAs_Click: RecId = 0 then Insert(). */
        public boolean saveAs;
        public String docNo;
        public String docDate;
        public int supplierId;
        public String lcOrderNo;
        public String lcOrderDate;
        public int salesPersonId;
        public int notifyPartyId;
        public int shippedToId;
        public String partialShipment;
        public String transShipment;
        public String lastShipmentDate;
        public String quotReference;
        public String inquiryReference;
        public String expiryDate;
        public String expiryPlace;
        public int loadingPortId;
        public int destinationPortId;
        public String legalizationRequired;
        public String inspectionRequired;
        public int importerBankId;
        public int exporterBankId;
        public int paymentTermId;
        public int currencyId;
        public String exchangeRate;
        public int deliveryTermId;
        public String grossWeight;
        public String noOfContainers;
        public String status;
        public String remarksHeader;
        public String commodity;
        public String legalizationDescription;
        public String inspectionDescription;
        public String shippingMarks;
        public List<LcOrderDetail> details = new ArrayList<>();
        /** lstRemoveRecord: saved rows the user removed from grdDetail (ActionTypeId 3). */
        public List<LcOrderDetail> removed = new ArrayList<>();
        public List<LcOrderPayment> payments = new ArrayList<>();
    }

    // ------------------------------------------------------------------ 526 ImLcOrderSchedule

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class ScheduleRow {
        public int id;
        public int contractId;
        public int supplierCustomerId;
        public String noOfContainers;
        public String weightMTon;
        public String attentiveLoadingDate;
        public int destinationPortId;
        public String attentiveProductionDate;
        public String attentiveInspectionDate;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class ScheduleMain {
        public List<ScheduleRow> rows = new ArrayList<>();
        public List<ScheduleRow> removed = new ArrayList<>();
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class SchedulePackingRow {
        public int id;
        public int scheduleId;
        public int contractId;
        public int itemId;
        public int cropYearId;
        public int packingTypeId;
        public String noOfBags;
        public int packUomId;
        public String weightMTon;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class SchedulePacking {
        public List<SchedulePackingRow> rows = new ArrayList<>();
        public List<SchedulePackingRow> removed = new ArrayList<>();
    }

    // ------------------------------------------------------------------ 221 ImCommercialInvoice

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class InvoiceDetail {
        public int itemId;
        public int packTypeId;
        public String qtyMTon;
        public int packSizeId;
        public String noOfBags;
        public String costMTon;
        public int rateUomId;
        public String amount;
        public int wareHouseId;
        public int jobLotId;
        public String itemDetail;
        public String addLessAmount;
        public String expenses;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class InvoiceExpense {
        public int accountId;
        public String remarks;
        public String fcAmount;
        public String localAmount;
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class Invoice {
        public int id;
        public int branchesId;
        public int projectsId;
        public String docNo;
        public String docDate;
        public String invoiceNo;
        public int supplierId;
        public int orderId;
        public int notifyParty1;
        public int notifyParty2;
        public String lotRef;
        public int paymentTermId;
        public int importerBankId;
        public int exporterBankId;
        public int deliveryTermId;
        public int loadingPortId;
        public int destinationPortId;
        public String carierType;
        public int fcyId;
        public int homeCurrencyId;
        public String fcyAmount;
        public String exchangeRate;
        public String localAmount;
        public String grossWeight;
        public String netWeight;
        public String noOfContainers;
        public String iFormNo;
        public String iFormDate;
        public String certificate1;
        public String certificate2;
        public String addLessComments;
        public String addLessAmount;
        public String totalAmount;
        public List<InvoiceDetail> details = new ArrayList<>();
        public List<InvoiceExpense> expenses = new ArrayList<>();
    }
}
