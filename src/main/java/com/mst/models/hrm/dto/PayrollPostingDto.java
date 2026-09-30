package com.mst.models.hrm.dto;

import java.util.List;

/**
 * Request body of frmPayrollPosting.btnsave_Click / btnUpdate_Click (screen 670): the form's RecId,
 * txtDocDate, cmbMonth / cmbYear values, txtDocNo, every grdDetails row and the lstDelete rows (the
 * "X" column's removed rows that already had an Id).
 */
public class PayrollPostingDto {
    public int id;
    public String docDate;
    public Object month;
    public Object year;
    public Object docNo;
    public List<PayrollRowDto> details;
    public List<PayrollRowDto> deleted;
}
