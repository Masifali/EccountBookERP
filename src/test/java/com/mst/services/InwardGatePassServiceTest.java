package com.mst.services;

import com.mst.models.InwardGatePass;
import com.mst.repositories.InwardGatePassRecordRepository;
import com.mst.repositories.InwardGatePassRepository;
import com.mst.security.CurrentUserContext;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.Captor;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.sql.Timestamp;
import java.util.Date;
import java.util.Map;
import java.util.Set;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class InwardGatePassServiceTest {
    @Mock InwardGatePassRepository repository;
    @Mock InwardGatePassRecordRepository records;
    @Mock CurrentUserContext context;
    @InjectMocks InwardGatePassService service;
    @Captor ArgumentCaptor<Map<String, Object>> header;

    @BeforeEach
    void scope() {
        when(context.currentOrganizationId()).thenReturn(1);
        when(context.currentCompanyId()).thenReturn(2);
        when(context.currentBranchId()).thenReturn(3);
        when(context.currentFinancialYearId()).thenReturn(4);
        when(context.currentUserId()).thenReturn(5);
        when(repository.saveHeader(any(), anyMap())).thenReturn(901);
    }

    @Test
    void insertSuppliesServerCreationTimeEvenWhenRequestHasNoAuditFields() {
        InwardGatePass gatePass = validGatePass();
        long before = System.currentTimeMillis();

        Map<String, Object> result = service.saveRecord(gatePass, Set.of(), false);

        assertEquals(true, result.get("success"));
        assertEquals(901, result.get("igpId"));
        verify(repository).saveHeader(same(gatePass), header.capture());
        assertCurrentTime(header.getValue().get("EntryDate"), before);
        assertEquals(5, header.getValue().get("EntryUser"));
        assertNull(header.getValue().get("ModifyDate"));
        assertEquals(0, header.getValue().get("ModifyUser"));
        verify(records).requireRight("Save");
        verify(repository, never()).saveDriverBio(any());
    }

    @Test
    void insertIgnoresClientAuditValuesAndSuppliesTimestampToNewDriver() {
        InwardGatePass gatePass = validGatePass();
        gatePass.setEntryDate(new Date(0));
        gatePass.setEntryUser(999);
        gatePass.setModifyDate(new Date(0));
        gatePass.setDriverBioDataId(0);
        when(repository.saveDriverBio(gatePass)).thenReturn(77);
        long before = System.currentTimeMillis();

        service.saveRecord(gatePass, Set.of(), false);

        verify(repository).saveDriverBio(gatePass);
        verify(repository).saveHeader(same(gatePass), header.capture());
        assertCurrentTime(gatePass.getEntryDate(), before);
        assertEquals(5, gatePass.getEntryUser());
        assertNull(gatePass.getModifyDate());
        assertEquals(gatePass.getEntryDate(), header.getValue().get("EntryDate"));
        assertEquals(77, header.getValue().get("driverBioDataId"));
    }

    @Test
    void updatePreservesCreationDetailsAndSetsModificationTime() {
        InwardGatePass gatePass = validGatePass();
        gatePass.setId(901);
        gatePass.setEntryDate(new Date(0));
        gatePass.setEntryUser(999);
        Timestamp created = Timestamp.valueOf("2025-01-02 03:04:05");
        when(records.require(901)).thenReturn(Map.of("EntryDate", created, "EntryUser", 6));
        long before = System.currentTimeMillis();

        service.saveRecord(gatePass, Set.of(), false);

        verify(records).requireRight("Update");
        verify(repository).saveHeader(same(gatePass), header.capture());
        assertEquals(created, header.getValue().get("EntryDate"));
        assertEquals(6, header.getValue().get("EntryUser"));
        assertCurrentTime(header.getValue().get("ModifyDate"), before);
        assertEquals(5, header.getValue().get("ModifyUser"));
        assertEquals(header.getValue().get("ModifyDate"), gatePass.getModifyDate());
    }

    @Test
    void newDriverDuringUpdateUsesCurrentUserAndTime() {
        InwardGatePass gatePass = validGatePass();
        gatePass.setId(901);
        gatePass.setDriverBioDataId(0);
        when(records.require(901)).thenReturn(Map.of("EntryDate", new Date(0), "EntryUser", 6));
        when(repository.saveDriverBio(gatePass)).thenReturn(77);
        long before = System.currentTimeMillis();

        service.saveRecord(gatePass, Set.of(), false);

        verify(repository).saveDriverBio(gatePass);
        assertCurrentTime(gatePass.getEntryDate(), before);
        assertEquals(5, gatePass.getEntryUser());
    }

    private static void assertCurrentTime(Object value, long before) {
        assertTrue(value instanceof Date, "The database must receive an audit timestamp");
        long actual = ((Date) value).getTime();
        assertTrue(actual >= before && actual <= System.currentTimeMillis());
    }

    private static InwardGatePass validGatePass() {
        InwardGatePass gatePass = new InwardGatePass();
        gatePass.setGatepassType("Paddy");
        gatePass.setVehicleNo("TEST-123");
        gatePass.setCityId(1);
        gatePass.setVehicleType("Truck");
        gatePass.setNoOfPackages(1);
        gatePass.setPackUnit(40d);
        gatePass.setWeightComparedToPoWt(40d);
        gatePass.setGpSrNo(1);
        gatePass.setStatus("Open");
        gatePass.setDocAttachment("Pending");
        gatePass.setSupplierContractCode("1");
        gatePass.setRefDocumentTypeId(104);
        gatePass.setDriverBioDataId(1);
        return gatePass;
    }
}
