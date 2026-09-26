package com.mst.services;

import com.mst.models.dto.PurchaseOrderFullDto;
import com.mst.repositories.PurchaseOrderRecordRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.test.util.ReflectionTestUtils;
import org.springframework.web.server.ResponseStatusException;
import static org.junit.jupiter.api.Assertions.*;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.Mockito.*;

class PurchaseOrderSafetyTest {
    @Test void otherDocumentTypesAreRejectedBeforeDatabaseAccess() {
        var service=new PurchaseOrderFullService(); var jdbc=mock(JdbcTemplate.class);
        ReflectionTestUtils.setField(service,"jdbcTemplate",jdbc);
        var dto=new PurchaseOrderFullDto(); assertEquals(1052,dto.getDocumentTypeId()); dto.setDocumentTypeId(9999);
        assertEquals(false,service.savePurchaseOrder(dto,null).get("success")); verifyNoInteractions(jdbc);
    }
    @Test void desktopUnsupportedDeleteCannotRemoveRows() {
        var service=new PurchaseOrderFullService(); var jdbc=mock(JdbcTemplate.class);
        ReflectionTestUtils.setField(service,"jdbcTemplate",jdbc);
        assertThrows(UnsupportedOperationException.class,()->service.deletePurchaseOrder(1));
        verifyNoInteractions(jdbc);
    }
    @Test void unknownOrOtherTenantRecordStopsBeforeReadProcedureAndWrites() {
        var jdbc=mock(JdbcTemplate.class); var context=mock(CurrentUserContext.class);
        when(context.currentOrganizationId()).thenReturn(78); when(context.currentCompanyId()).thenReturn(78);
        var guard=new PurchaseOrderRecordRepository(jdbc,context);
        assertEquals(404,assertThrows(ResponseStatusException.class,()->guard.require(123)).getStatus().value());
        verify(jdbc).queryForList("SELECT Id,DocNo,OrderSupCustId,IsAproved,BranchesId,EntryUser,AttachmentsValues,CustomAttachmentsValues FROM dbo.PurchaseOrder WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=41",123,78,78);
        verifyNoMoreInteractions(jdbc);
    }

    @Test void anotherUsersOrderIsRefusedWithoutCanViewAllRight() {
        var jdbc=mock(JdbcTemplate.class); var context=mock(CurrentUserContext.class);
        when(context.currentOrganizationId()).thenReturn(78); when(context.currentCompanyId()).thenReturn(78);
        when(context.currentUserId()).thenReturn(85);
        when(jdbc.queryForList("SELECT Id,DocNo,OrderSupCustId,IsAproved,BranchesId,EntryUser,AttachmentsValues,CustomAttachmentsValues FROM dbo.PurchaseOrder WHERE Id=? AND OrganizationId=? AND CompanyId=? AND DocumentTypeId=41",123,78,78))
                .thenReturn(List.of(Map.of("Id",123,"EntryUser",99,"BranchesId",58)));
        assertEquals(404,assertThrows(ResponseStatusException.class,()->new PurchaseOrderRecordRepository(jdbc,context).require(123)).getStatus().value());
    }
}
