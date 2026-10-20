package com.mst.projections;

/** Row of USP_ItemMultiLingo_ReadAll (JPQL) */
public interface ItemMultiLingoReadAllRow {
    Integer getId();
    Integer getItemId();
    Integer getMultiLanguagesId();
    String getLanguageName();
    String getItemName();
    String getOtherLanguageTitle();
}
