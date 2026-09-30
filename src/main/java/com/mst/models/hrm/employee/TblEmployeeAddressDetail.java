package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

/**
 * Architecture.Model.HRM.EmployeeManagement.tblEmployeeAddressDetail (model 0723) - 9 properties, as
 * SetProc sends them to Sp_tblEmployeeAddressDetail_Insert (9 parameters, all declared).
 */
public class TblEmployeeAddressDetail extends DesktopModel {
    public int EmployeeId;
    public int AddressTypeId;
    public int CityId;
    public int CompanyId;
    public int CountryId;
    public int Id;
    public int OrganizationId;
    public int ProvinceId;
    public String AddressDetail;
}
