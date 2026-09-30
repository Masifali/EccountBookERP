package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.Inventory.UOMSchedule (model 1075): the 15 non-virtual properties SetProc sends to
 * Sp_UOMSchedule_Insert (15 params). UOMCode / ScheduleUnitName are virtual.
 */
public class PpAUomScheduleModel extends DesktopModel {
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
