package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0783_Architecture.Model.FeedMill.PricingSchedule.ItemPricingSchedule.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAFeedItemPricingSchedule extends DesktopModel {
    public boolean IsActive;
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime EffectiveDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double ItemRate;
    public int ApprovedUserId;
    public int CompanyId;
    public int DocumentTypeId;
    public int EntryUserId;
    public int FinancialYearId;
    public int Id;
    public int ItemId;
    public int ModifyUserId;
    public int OrganizationId;
    public int PricingCustomGroupId;
    public int RateUomId;
}
