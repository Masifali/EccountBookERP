package com.mst.models.hrm.dto;

/**
 * Request body of the two-text-box Profile define forms: Employee Group (txtShortName / txtGroupName),
 * Employee Category (txtShortName / txtCategoryName) and Profile Types (txtprefix / txtprofilename).
 * id = the form's RecId (0 = new). Public fields: Jackson binds "id" / "shortName" / "name" exactly.
 */
public class ProfileCodeNameDto {
    public int id;
    public String shortName;
    public String name;
}
