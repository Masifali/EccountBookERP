package com.mst.projections;

/** Row of Sp_MultiLanguages_GetAll @MethodType='ReadAll' (native; aliases kept as in SQL) */
public interface MultiLanguagesRow {
    Integer getId();
    String getLanguageName();
    String getLanguageDescription();
    String getFlagUrl();
    Boolean getRTL();
}
