package com.mst.models.partyprocessing;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.ReferenceParties (model 0076): the 7 properties SetProc sends to
 * Sp_ReferenceParties_Insert / _Update (7 params, all declared).
 */
public class PpAReferencePartyModel extends DesktopModel {
    public boolean IsActive;
    public int Id;
    public int OrganizationId;
    public int CompanyId;
    public int SupplierCustomerId;
    public int ReferencePartyTypeId;
    public String ReferencePartyName;
}
