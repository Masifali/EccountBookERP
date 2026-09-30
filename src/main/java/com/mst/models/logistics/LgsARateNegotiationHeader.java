package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0602_Architecture.Model.lgstcm.logisticRateNegotiationHeader.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsARateNegotiationHeader extends DesktopModel {
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime documentDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double fcyExchangeRate;
    public double transactionExchangeRate;
    public int ActionId;
    public int ApprovedUserId;
    public int BranchesId;
    public int CompanyId;
    public int DealStatusId;
    public int documentNo;
    public int documentTypeId;
    public int EntryUserId;
    public int fcyCurrencyId;
    public int FinancialYearId;
    public int logisticRateNegotiationHeaderId;
    public int ModifyUserId;
    public int OrganizationId;
    public int ProjectsId;
    public int ServiceTypeId;
    public int transactionCurrencyId;
    public String AttachmentsValues;
    public String CustomAttachmentsValues;
    public String RemarksHeader;
    public int CityFromId;
    public int CityToId;
}
