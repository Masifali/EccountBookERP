package com.mst.models.hrm.employee;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.EmployeeManagement.GenEmployeeEducation (model 0709) - 18 properties, as
 * SetProc sends them to Sp_GenEmployeeEducation_Insert (18 parameters, all declared).
 */
public class GenEmployeeEducation extends DesktopModel {
    public boolean PostState;
    public LocalDateTime EntryDate;
    public LocalDateTime ModifyDate;
    public LocalDateTime PassingYear;
    public LocalDateTime PostDate;
    public LocalDateTime StartYear;
    public double CGPA;
    public int CityId;
    public int CompanyId;
    public int DegreeId;
    public int EmployeeId;
    public int EntryUser;
    public int Id;
    public int ModifyUser;
    public int OrganizationId;
    public int PostUser;
    public String DegreeTitle;
    public String Institute;
}
