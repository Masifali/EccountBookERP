package com.mst.services;

import com.mst.models.Brand;
import com.mst.models.UserAccount;
import com.mst.repositories.DesktopBrandRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.util.List;
import org.junit.jupiter.api.Test;
import org.springframework.web.server.ResponseStatusException;
import static org.mockito.Mockito.*;
import static org.mockito.ArgumentMatchers.*;
import static org.junit.jupiter.api.Assertions.*;

class BrandServiceContractTest {
    private final DesktopBrandRepository repository=mock(DesktopBrandRepository.class);
    private final CurrentUserContext context=mock(CurrentUserContext.class);
    private final DesktopReportRights rights=mock(DesktopReportRights.class);
    private final UserAccount user=new UserAccount();
    private BrandService service(){user.setId(8);user.setCompanyId(3);user.setOrganizationId(2);when(context.requireAccountingUser()).thenReturn(user);return new BrandService(repository,context,rights);}
    @Test void missingSavePermissionPreventsDatabaseWrites(){
        BrandService service=service();doThrow(new org.springframework.security.access.AccessDeniedException("Denied")).when(rights).require(user,876,"Save");
        Brand brand=new Brand();brand.setBrandCode("TEST");brand.setBrandName("TEST");assertThrows(org.springframework.security.access.AccessDeniedException.class,()->service.addOrUpdate(brand));verifyNoInteractions(repository);
    }
    @Test void cannotUpdateOrDeleteAnotherCompanyBrand(){
        BrandService service=service();Brand input=new Brand();input.setId(90);input.setBrandCode("B1");input.setBrandName("Brand");
        when(repository.history(user,90)).thenReturn(List.of());
        assertThrows(ResponseStatusException.class,()->service.addOrUpdate(input));
        assertThrows(ResponseStatusException.class,()->service.delete(90));
        verify(repository,never()).save(any(),any());verify(repository,never()).delete(any(),anyInt());
    }
    @Test void preventsDuplicateAndMissingCodeBeforeWriting(){
        BrandService service=service();Brand input=new Brand();input.setBrandName("Rice");
        assertThrows(ResponseStatusException.class,()->service.addOrUpdate(input));
        Brand existing=new Brand();existing.setId(4);existing.setBrandCode("B1");existing.setBrandName("Rice");
        when(repository.history(user,null)).thenReturn(List.of(existing));input.setBrandCode("B2");
        assertThrows(ResponseStatusException.class,()->service.addOrUpdate(input));verify(repository,never()).save(any(),any());
    }
    @Test void usesDesktopSaveAndReloadsAuditFields(){
        BrandService service=service();Brand input=new Brand();input.setBrandCode(" B1 ");input.setBrandName(" Rice ");
        Brand saved=new Brand();saved.setId(11);saved.setEntryUserId(8);
        when(repository.history(user,null)).thenReturn(List.of());when(repository.save(user,input)).thenReturn(11);when(repository.history(user,11)).thenReturn(List.of(saved));
        assertSame(saved,service.addOrUpdate(input));assertEquals("B1",input.getBrandCode());assertEquals("Rice",input.getBrandName());
    }
}
