package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Model.Import.ImLcOrder (0652): one public field per NON-virtual property, spelled as the C# property, so
 * {@link #toParams()} binds exactly what GenericProvider.SetProc binds. Virtual display properties are not fields.
 */
@SuppressWarnings("unused")
public class ImpALcOrder extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime DocDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ExpiryDate;
    public LocalDateTime LastShipmentDate;
    public LocalDateTime LcOrderDate;
    public LocalDateTime ModifyDate;
    public BigDecimal ExchangeRate = BigDecimal.ZERO;
    public BigDecimal FcyAmount = BigDecimal.ZERO;
    public BigDecimal GrossWeightKgs = BigDecimal.ZERO;
    public BigDecimal LcyAmount = BigDecimal.ZERO;
    public BigDecimal NetWeightKgs = BigDecimal.ZERO;
    public int ActionId;
    public int ApprovedUserId;
    public int BranchesId;
    public int CompanyId;
    public int DeliveryTermId;
    public int DestinationPortId;
    public int DocNo;
    public int DocumentTypeId;
    public int EntryUserId;
    public int ExporterBankId;
    public int FcurrencyId;
    public int FinancialYearId;
    public int Id;
    public int ImporterBankId;
    public int LoadingPortId;
    public int ModifyUserId;
    public int NoOfContainers;
    public int NotifyPartyId;
    public int OrganizationId;
    public int PaymentTermId;
    public int ProjectsId;
    public int SalesPersonId;
    public int ShipedToId;
    public int SupplierCustomerId;
    public String CommodityDetial;
    public String ExpiryPlace;
    public String InquiryReference;
    public String InsepctionDescription;
    public String InsepctionRequired;
    public String LcOrderNo;
    public String LegalizationDescription;
    public String LegalizationRequired;
    public String PartialShipment;
    public String QuotReference;
    public String RemarksHeader;
    public String ShippingMarks;
    public String Status;
    public String TransShipment;
}
