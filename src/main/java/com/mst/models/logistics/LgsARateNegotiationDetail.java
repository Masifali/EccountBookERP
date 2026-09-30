package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0599_Architecture.Model.lgstcm.logisticRateNegotiationDetail.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsARateNegotiationDetail extends DesktopModel {
    public LocalDateTime cutOffDate;
    public LocalDateTime OfferValidityDate;
    public LocalDateTime etdPOL;
    public double fcyAmount;
    public double lcyAmount;
    public double serviceRate;
    public double tcyAmount;
    public double tcyExchangeRate;
    public int ActionTypeId;
    public int DueDays;
    public int freeDaysAtPOD;
    public int logisticRateNegotiationDetailId;
    public int logisticRateNegotiationHeaderId;
    public int OffervalidityDays;
    public int partyIdAgent;
    public int paymentTermId;
    public int portOfDischargeId;
    public int portOfLoadingId;
    public int ServiceItemId;
    public int serviceProviderId;
    public int serviceRateBaseUomId;
    public int serviceTypeId;
    public int tcyCurrencyId;
    public int VesselRoutingTypeId;
    public int vesselTransitDays;
    public String Remarks;
    public String VesselRoutingDescription;
    public int CityFromId;
    public int CityToId;
}
