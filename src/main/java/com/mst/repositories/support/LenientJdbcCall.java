package com.mst.repositories.support;

import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.SqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;

import java.util.LinkedHashSet;
import java.util.Map;
import java.util.Set;

/**
 * SimpleJdbcCall that sends ONLY the parameters the caller supplied - the way the desktop DAL
 * builds its SqlParameter list (GenericProvider: a parameter that is not added is not sent, so
 * the procedure's own default applies).
 *
 * Plain SimpleJdbcCall reads every parameter from the procedure metadata and throws
 * "Required input parameter 'X' is missing" for any the caller left out, even when the
 * procedure declares a default for it (seen on cmagt generate-no / generate-code /
 * pending-purchase-order-tables, 2026-09-30). Limiting the in-parameter names to the supplied
 * keys, before the call is compiled, restores the desktop behaviour.
 */
public class LenientJdbcCall extends SimpleJdbcCall {

    public LenientJdbcCall(JdbcTemplate jdbcTemplate) { super(jdbcTemplate); }

    private void limitTo(Iterable<String> names) {
        if (isCompiled()) return;
        Set<String> s = new LinkedHashSet<>();
        for (String n : names) {
            if (n == null) continue;
            s.add(n.startsWith("@") ? n.substring(1) : n);
        }
        if (s.isEmpty()) s.add("__none__");   // no inputs supplied: send none
        setInParameterNames(s);
    }

    @Override
    public Map<String, Object> execute(Map<String, ?> args) {
        limitTo(args.keySet());
        return super.execute(args);
    }

    @Override
    public Map<String, Object> execute(SqlParameterSource parameterSource) {
        String[] names = parameterSource.getParameterNames();
        limitTo(names == null ? java.util.Collections.<String>emptyList() : java.util.Arrays.asList(names));
        return super.execute(parameterSource);
    }

    @Override
    public Map<String, Object> execute(Object... args) {
        if (args == null || args.length == 0) limitTo(java.util.Collections.<String>emptyList());
        return super.execute(args);
    }
}
