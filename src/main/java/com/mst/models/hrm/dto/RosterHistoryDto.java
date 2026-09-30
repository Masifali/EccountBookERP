package com.mst.models.hrm.dto;

/**
 * History tab filters of frmDutyRoasterNew (HistoryGridFill): FromDateHistory / ToDateHistory with their
 * check boxes, the radio (doc = "Duter Roaster Dates", entry = "Entry Date", modify = "Modify Date") and
 * the Roaster From / To text boxes (read with Conversion.ToInt).
 */
public class RosterHistoryDto {
    public boolean fromChecked;
    public String fromDate;
    public boolean toChecked;
    public String toDate;
    public String mode;
    public String rosterFrom;
    public String rosterTo;
}
