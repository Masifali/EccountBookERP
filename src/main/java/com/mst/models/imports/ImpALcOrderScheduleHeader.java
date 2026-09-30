package com.mst.models.imports;

import com.mst.models.hrm.DesktopModel;

import java.math.BigDecimal;
import java.time.LocalDateTime;

/**
 * Model.Import.ImLcOrderScheduleHeader (0642): one public field per NON-virtual property, spelled as the C# property, so
 * {@link #toParams()} binds exactly what GenericProvider.SetProc binds. Virtual display properties are not fields.
 */
@SuppressWarnings("unused")
public class ImpALcOrderScheduleHeader extends DesktopModel {
    public LocalDateTime AttentiveInspectionDate;
    public LocalDateTime AttentiveLoadingDate;
    public LocalDateTime AttentiveProductionDate;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public BigDecimal NetWeight = BigDecimal.ZERO;
    public int BranchesId;
    public int ActionId;
    public int CompanyId;
    public int DestinationPortId;
    public int EntryUserId;
    public int ImLcOrderId;
    public int Id;
    public int ModifyUserId;
    public int NoOfContainer;
    public int OrganizationId;
    public int FinancialYearId;
    public int SortNo;
    public int SupplierCustomerId;
    public String ScheduleCode;
    public String ScheduleRemarks;
}
