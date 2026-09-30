package com.mst.models.logistics;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/** Generated from 1075_Architecture.Model.Inventory.UOMSchedule.cs - every non-virtual property, as GenericProvider.SetProc binds it. */
public class LgsAUomSchedule extends DesktopModel {
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public double Equivalent;
    public double QtyEquivalent;
    public boolean BaseRateUom;
    public boolean BasePackUom;
    public boolean BaseSecondaryUom;
    public boolean Active;
    public int CompanyId;
    public int EntryUser;
    public int Id;
    public int ItemId;
    public int ModifyUser;
    public int OrganizationId;
    public int ScheduleUnitId;
}
