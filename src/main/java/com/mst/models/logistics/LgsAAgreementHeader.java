package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 0600_Architecture.Model.lgstcm.AgreementHeader.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAAgreementHeader extends DesktopModel {
    public boolean isActive;
    public boolean IsApproved;
    public LocalDateTime ApprovedDate;
    public LocalDateTime DocumentDate;
    public LocalDateTime effectiveDateFrom;
    public LocalDateTime effectiveDateTo;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double fcyExchangeRate;
    public double transactionExchangeRate;
    public int ActionId;
    public int AgreementHeaderId;
    public int DocumentTypeId;
    public int ApprovedUserId;
    public int BranchesId;
    public int CompanyId;
    public int DocumentNo;
    public int EntryUserId;
    public int fcyCurrencyId;
    public int FinancialYearId;
    public int ModifyUserId;
    public int OrganizationId;
    public int ProjectsId;
    public int serviceProviderId;
    public int BrokerAgentId;
    public int serviceTypeId;
    public int transactionCurrencyId;
    public String AttachmentsValues;
    public String CustomAttachmentsValues;
    public String RemarksHeader;
}
