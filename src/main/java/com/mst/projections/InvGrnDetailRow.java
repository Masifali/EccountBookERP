package com.mst.projections;

/**
 * Row of Sp_InvGrnDetail_GetAllMethod @Activity='ReadByInvGrnID' / 'ReadByInvGrnID_Engr' / 'ReadByInvGrnIdDirect'
 * (native). Same columns as the Trading branch plus Expr1 (InvWareHouse.WareHouseCode) and WareHouseName.
 */
public interface InvGrnDetailRow extends InvGrnDetailTradingRow {
    String getExpr1();
    String getWareHouseName();
}
