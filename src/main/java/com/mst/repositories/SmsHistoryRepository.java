package com.mst.repositories;

import java.time.LocalDateTime;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;

import com.mst.models.SmsHistory;

/**
 * dbo.SmsHistory — written by Sp_ItemPricingSchedule_Insert: when no SmsHistory row exists with CAST(SmsDate AS DATE) = CAST(@EffectedDate AS DATE)
 * it inserts (SmsDate=@EffectedDate, SmsSourceDoc='Price list', SmsSourceId=0, SmsBody='Price list published and effected date is  '+CONVERT(NVARCHAR(50),@EffectedDate),
 * SmsCellNo='', SendStatus=0, EntryDate=GETDATE(), EntryUser=@EntryUser, OrganizationId, CompanyId, ScreenName='frmItemSalesPricingSchedule',
 * ToUserId=0, ReceiverId=0, AutoManual='', SupplierCustomerId=0). Callers use save().
 */
@Repository
public interface SmsHistoryRepository extends JpaRepository<SmsHistory, Integer> {

    /** Sp_ItemPricingSchedule_Insert guard: IF NOT EXISTS(SELECT 1 FROM SmsHistory WHERE CAST(SmsDate AS DATE) = CAST(@EffectedDate AS DATE)). */
    @Query(value = "SELECT COUNT(1) FROM SmsHistory WHERE CAST(SmsDate AS DATE) = CAST(:effectedDate AS DATE)", nativeQuery = true)
    Long countBySmsDay(@Param("effectedDate") LocalDateTime effectedDate);
}
