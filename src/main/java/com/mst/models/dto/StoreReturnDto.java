package com.mst.models.dto;

import java.util.ArrayList;
import java.util.List;

/**
 * Screen 330 "Store Return" (StoreReturn.cs, DocumentTypeId 140) — what the page posts.
 *
 * Column names are the desktop grid's ("table" in InitializeComponentCustom:312). Everything the
 * server owns — organization, company, branch, financial year, users, entry/approval dates — is
 * filled by the service from the signed-in user.
 */
public class StoreReturnDto {

    public Integer Id = 0;                 // RECID
    public Integer DocNo = 0;              // txtDocNo
    public String  DocDate;                // "yyyy-MM-dd"
    public String  Remarks;

    public List<Row> rows = new ArrayList<>();

    public static class Row {
        public Integer IssuanceId = 0;
        public Integer IssuanceDetailId = 0;
        public Integer IssuanceNo = 0;
        public Integer ItemId = 0;
        public String  ItemCode;
        public String  ItemName;
        public Integer WareHouseId = 0;
        public String  WareHouseName;
        public Integer RackId = 0;
        public String  RackName;
        public Integer ItemConditionId = 0;
        public String  ItemCondition;       // the combo cell's display text
        public Integer PackUomId = 0;
        public String  PackUom;
        public Double  BalanceQty = 0d;
        public Double  ItemQty = 0d;
        public Integer SecondaryUomId = 0;
        public String  SecondaryUom;
        public Double  SecondaryUomQty = 0d;
        public Double  PerItemWeight = 0d;
        public Double  SecondaryUomItemRate = 0d;
        public Double  ItemRate = 0d;
        public Double  IssuanceRate = 0d;
        public Double  ItemAmount = 0d;
        public Integer DepartmentId = 0;
        public Integer AssetId = 0;
        public String  Remarks;
        public Integer CrAccountId = 0;
        public String  CreditAccount;
        public Integer RecordNo = 0;
    }
}
