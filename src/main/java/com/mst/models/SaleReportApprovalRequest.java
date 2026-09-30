package com.mst.models;

/** Report filters are re-applied before the unscoped native commentary procedure is called. */
public record SaleReportApprovalRequest<T>(T filter, int documentTypeId, int id, boolean summary) { }
