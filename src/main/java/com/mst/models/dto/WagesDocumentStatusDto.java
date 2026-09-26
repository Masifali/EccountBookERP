package com.mst.models.dto;

/** The editable status row from Configuration.grdWagesRefDocuments. */
public record WagesDocumentStatusDto(int id, int refDocumentTypeId, Boolean isActive) { }
