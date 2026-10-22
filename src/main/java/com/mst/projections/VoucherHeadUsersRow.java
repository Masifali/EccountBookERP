package com.mst.projections;

/** Row of "Select EntryUser, ModifyUser From VoucherHead Where Id = @VoucherHeadId" in Sp_VoucherDetail_Insert. JPQL projection. */
public interface VoucherHeadUsersRow {
    Integer getEntryUser();
    Integer getModifyUser();
}
