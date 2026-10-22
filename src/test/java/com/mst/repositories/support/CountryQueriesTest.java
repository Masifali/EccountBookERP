package com.mst.repositories.support;
import com.mst.repositories.CountryRepository;
import org.junit.jupiter.api.Test;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
class CountryQueriesTest {
    @Test void countryListPreservesGlobalScopeColumnsAndNulls() {
        CountryRepository repository = mock(CountryRepository.class);
        Map<String,Object> source = new LinkedHashMap<>();
        source.put("Id",5); source.put("Description","Pakistan"); source.put("ModifyDate",null);
        when(repository.getAllRows()).thenReturn(List.of(Collections.unmodifiableMap(source)));
        var rows = new CountryQueries(repository).rows("OrganizationId",99,"CompanyId",123,"MethodType","GetAll");
        assertEquals(source,rows.get(0));
        assertEquals("Pakistan",rows.get(0).get("description"));
        assertTrue(rows.get(0).containsKey("modifydate"));
        rows.get(0).put("Extra",true); assertFalse(source.containsKey("Extra"));
        verify(repository).getAllRows(); verifyNoMoreInteractions(repository);
    }
    @Test void countryIdReadAndUnknownBranchKeepProcedureSemantics() {
        CountryRepository repository = mock(CountryRepository.class);
        when(repository.getIdRows(5)).thenReturn(List.of(Map.of("Id",5)));
        CountryQueries queries = new CountryQueries(repository);
        assertEquals(5,queries.rows("@Id",5,"MethodType","GetById").get(0).get("ID"));
        assertTrue(queries.rows("MethodType","Unknown").isEmpty());
        assertTrue(queries.rows("Id",5).isEmpty());
        verify(repository).getIdRows(5); verifyNoMoreInteractions(repository);
    }
}
