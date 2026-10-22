package com.mst.projections;

/** Proc_ChartofAccount_Insert: PLNoteId/BSNoteId/AccountTypeId/AccountClass of the parent account (JPQL). */
public interface CoaParentNoteRow {
    Integer getPlNoteId();
    Integer getBsNoteId();
    Integer getAccountTypeId();
    Integer getAccountClass();
}
