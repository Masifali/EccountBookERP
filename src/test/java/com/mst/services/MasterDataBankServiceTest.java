package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.AccountsDefinitionMasterRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.test.util.ReflectionTestUtils;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class MasterDataBankServiceTest {
    private final AccountsDefinitionMasterRepository repo = mock(AccountsDefinitionMasterRepository.class);
    private final CurrentUserContext context = mock(CurrentUserContext.class);
    private final DesktopReportRights rights = mock(DesktopReportRights.class);
    private final AccountsDefinitionMasterService service = new AccountsDefinitionMasterService();
    private final UserAccount user = new UserAccount();

    @BeforeEach void setup() {
        user.setId(7); user.setOrganizationId(8); user.setCompanyId(9);
        when(context.requireAccountingUser()).thenReturn(user);
        ReflectionTestUtils.setField(service, "repo", repo);
        ReflectionTestUtils.setField(service, "currentUserContext", context);
        ReflectionTestUtils.setField(service, "rights", rights);
    }

    @Test void bankSetupUsesCurrentCompanyAndIndependentGridRights() {
        var countries = List.<Map<String, Object>>of(Map.of("Id", 11, "Description", "Example Country"));
        when(repo.countries(user)).thenReturn(countries);
        doThrow(new AccessDeniedException("Denied")).when(rights).require(user, 413, "Grid Export");
        var setup = service.bankSetup();
        assertSame(countries, setup.get("countries"));
        assertEquals(true, setup.get("canSave"));
        assertEquals(true, setup.get("canGridPrint"));
        assertEquals(false, setup.get("canGridExport"));
        verify(rights).require(user, 413, "View");
        verify(repo).banks(user); verify(repo).cities(user); verify(repo).bankGlAccounts(user);
    }

    private Map<String, Object> bank() {
        var b = new LinkedHashMap<String, Object>();
        b.put("id", 0); b.put("isHomelandId", 2); b.put("isHomelandText", "Home Country");
        b.put("bankName", " Example Bank "); b.put("branchCode", " 123 ");
        b.put("bankAccountNo", "TEST-ACCOUNT"); b.put("bankIbanNo", "TEST-IBAN");
        b.put("bankAccountTitle", "Example Title"); b.put("chequeTemplate", "Standard");
        b.put("chartOfAccountId", 14); b.put("countryId", 11); b.put("branchCity", 12);
        b.put("companyId", 999); b.put("organizationId", 999);
        when(repo.bankGlAccounts(user)).thenReturn(List.of(Map.of("Id", 14)));
        return b;
    }

    @SuppressWarnings({"unchecked", "rawtypes"})
    @Test void selectedDropdownIdsAndTextsKeepTheDesktopBankContract() {
        var b = bank();
        when(repo.saveBank(anyMap())).thenReturn(23);
        assertEquals(23, service.saveBank(b).get("id"));
        ArgumentCaptor<Map<String, Object>> args = ArgumentCaptor.forClass((Class) Map.class);
        verify(repo).saveBank(args.capture());
        var p = args.getValue();
        assertEquals(27, p.size());
        assertEquals("Example Bank", p.get("@BranchName"));
        assertEquals("123", p.get("@BranchCode"));
        assertEquals("Home Country", p.get("@IsHomeland"));
        assertEquals("Standard", p.get("@ChequeTemplete"));
        assertEquals(11, p.get("@CountryId")); assertEquals(12, p.get("@BranchCity"));
        assertEquals(14, p.get("@ChartOfAccountId"));
        assertEquals(9, p.get("@CompanyId")); assertEquals(8, p.get("@OrganizationId"));
        assertEquals(7, p.get("@EntryUser")); assertEquals(7, p.get("@ModifyUser"));
        verify(rights).require(user, 413, "Save");
    }

    @Test void missingHomeCountryChequeTemplateDoesNotSave() {
        var b = bank(); b.put("chequeTemplate", "");
        assertEquals("Cheque Template Field Required", assertThrows(IllegalArgumentException.class, () -> service.saveBank(b)).getMessage());
        verify(repo, never()).saveBank(anyMap());
    }

    @Test void updateAndEditCannotReachAnotherCompanysBank() {
        var b = bank(); b.put("id", 23);
        when(repo.banks(user)).thenReturn(List.of(Map.of("Id", 24)));
        assertThrows(IllegalArgumentException.class, () -> service.saveBank(b));
        assertThrows(IllegalArgumentException.class, () -> service.bank(23));
        verify(rights).require(user, 413, "Update");
        verify(repo, never()).saveBank(anyMap()); verify(repo, never()).bank(anyInt());
    }

    @Test void deniedSaveDoesNotCallTheDatabaseWriter() {
        var b = bank();
        doThrow(new AccessDeniedException("Denied")).when(rights).require(user, 413, "Save");
        assertThrows(AccessDeniedException.class, () -> service.saveBank(b));
        verify(repo, never()).saveBank(anyMap());
    }
}
