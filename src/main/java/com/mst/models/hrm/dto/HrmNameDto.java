package com.mst.models.hrm.dto;

/**
 * Request body of the one-text-box HRM define forms (Designation, Employee Group, Employee Category,
 * Section ...): the record id being edited (0 = new, the form's RecId) and the text box.
 * Public fields: Jackson binds the JSON keys "id" / "name" to them exactly.
 */
public class HrmNameDto {
    public int id;
    public String name;
}
