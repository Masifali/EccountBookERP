package com.mst.repositories.support;

import com.mst.repositories.CountryRepository;
import org.springframework.stereotype.Component;
import java.util.*;

/** Country read procedure equivalents; organization/company never filtered these rows. */
@Component
public class CountryQueries {
    public static final String COLUMNS = "SELECT Id, Code, Description, EntryDate, EntryUser, ModifyDate, ModifyUser, PostDate, PostUser, PostState, OrganizationId, CompanyId";
    private final CountryRepository repository;
    public CountryQueries(CountryRepository repository) { this.repository = repository; }

    public List<Map<String, Object>> rows(Object... parameters) {
        return rows(DesktopProc.params(parameters));
    }

    public List<Map<String, Object>> rows(Map<String, Object> parameters) {
        Map<String, Object> p = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        parameters.forEach((key, value) -> p.put(key.replace("@", ""), value));
        Object method = p.get("MethodType");
        List<Map<String, Object>> result;
        if (method == null) return new ArrayList<>();
        switch (method.toString().toLowerCase(Locale.ROOT)) {
            case "getall": result = repository.getAllRows(); break;
            case "getbyid":
                Object id = p.get("Id");
                result = repository.getIdRows(id == null ? null : id instanceof Number ? ((Number) id).intValue() : Integer.valueOf(id.toString()));
                break;
            default: return new ArrayList<>();
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> row : result) {
            Map<String, Object> copy = new org.springframework.util.LinkedCaseInsensitiveMap<>();
            copy.putAll(row);
            out.add(copy);
        }
        return out;
    }
}
