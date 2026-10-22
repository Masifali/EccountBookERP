package com.mst.projections;

import java.sql.Timestamp;

/** Row of Sp_COAAllocation_GetAllMethod @Activity='FormHistory' (native). */
public interface CoaAllocationFormHistoryRow {
    Integer getId();
    String getParentAcCode();
    String getParentAcTitle();
    String getAccountCode();
    String getAccountTitle();
    String getAccountClass();
    String getAccountType();
    Integer getAccount_Level();
    String getNoteTitle();
    String getAccountGroup();
    /** 'True' / 'False' text, as the procedure emits it. */
    String getIsActive();
    String getOtherErpCode();
    Timestamp getEntryDate();
    /** FirstName-LastName of the entry user. */
    String getEntryUser();
    Timestamp getModifyDate();
    /** FirstName-LastName of the modify user. */
    String getModifyUser();
}
