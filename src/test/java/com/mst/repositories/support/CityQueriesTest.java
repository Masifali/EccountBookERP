package com.mst.repositories.support;

import com.mst.repositories.CityRepository;
import org.junit.jupiter.api.Test;
import java.util.*;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

class CityQueriesTest {
    @Test
    void lookupPreservesNullsAliasesAndMutableCaseInsensitiveRows() {
        CityRepository repository = mock(CityRepository.class);
        Map<String, Object> source = new LinkedHashMap<>();
        source.put("Id", 17);
        source.put("CityName", "Lahore");
        source.put("Tehsil", null);
        source.put("CompanyId", 9);
        when(repository.getAllRows(3)).thenReturn(List.of(Collections.unmodifiableMap(source)));
        CityQueries queries = new CityQueries(repository);
        List<Map<String, Object>> result = queries.rows("OrganizationId", 3, "CompanyId", 1, "MethodType", "GetAll");
        assertEquals(source, result.get(0));
        assertEquals("Lahore", result.get(0).get("cityname"));
        assertTrue(result.get(0).containsKey("TEHSIL"));
        assertNull(result.get(0).get("Tehsil"));
        result.get(0).put("Extra", true);
        assertFalse(source.containsKey("Extra"));
        verify(repository).getAllRows(3);
        verifyNoMoreInteractions(repository);
    }

    @Test
    void optionalFiltersAndIdReadsRetainLegacyScope() {
        CityRepository repository = mock(CityRepository.class);
        when(repository.readOrganizationRows(3, null)).thenReturn(List.of());
        when(repository.readIdRows(17)).thenReturn(List.of());
        when(repository.readTehsilRows(3, null)).thenReturn(List.of());
        when(repository.readDistrictRows(3, 17)).thenReturn(List.of());
        CityQueries queries = new CityQueries(repository);
        queries.rows("OrganizationId", 3, "MethodType", "ReadByOrganizationCompanyId");
        queries.rows("OrganizationId", 99, "Id", 17, "MethodType", "ReadById");
        queries.rows("OrganizationId", 3, "MethodType", "ReadCityByTehsilId");
        queries.rows("OrganizationId", 3, "Id", 17, "MethodType", "GetDistrictAndTehsilByCityId");
        verify(repository).readOrganizationRows(3, null);
        verify(repository).readIdRows(17);
        verify(repository).readTehsilRows(3, null);
        verify(repository).readDistrictRows(3, 17);
        verifyNoMoreInteractions(repository);
    }

    @Test
    void missingOrUnknownMethodReturnsNoRows() {
        CityRepository repository = mock(CityRepository.class);
        CityQueries queries = new CityQueries(repository);
        assertTrue(queries.rows("OrganizationId", 3).isEmpty());
        assertTrue(queries.rows("MethodType", "Unknown").isEmpty());
        verifyNoInteractions(repository);
    }
}
