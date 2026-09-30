package com.mst.models.hrm;

import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Base of every HRM model class (package com.mst.models.hrm).
 *
 * A subclass declares the desktop model's properties as PUBLIC fields, spelled EXACTLY as the C#
 * property (DesignationId, DesignationName, CreatedOn ...). {@link #toParams()} then does what
 * GenericProvider.SetProc does on the desktop: every non-virtual property becomes "@" + Name.
 *
 * Type mapping (C# -> Java field):
 *   int / long            -> int / long (default 0, as the CLR default)
 *   decimal / double      -> BigDecimal / double (BigDecimal fields should be initialised to ZERO
 *                            when the C# property is a non-nullable decimal)
 *   bool                  -> boolean (default false)
 *   int? / decimal? / bool? / DateTime? -> Integer / BigDecimal / Boolean / LocalDateTime (null)
 *   string                -> String (null)
 *   DateTime              -> LocalDateTime - the desktop always sets these before a save; a null
 *                            here is omitted (the proc's default applies) instead of the
 *                            SqlDateTime overflow DateTime.MinValue would raise.
 *   virtual navigation properties / List<> children -> mark the field {@code transient}; they are
 *                            skipped exactly as SetProc skips virtual getters.
 *
 * A null value is omitted from the EXEC by DesktopProc (AddWithValue(null) sends nothing).
 */
public abstract class DesktopModel {

    /** "@"-less parameter map in declaration order (DesktopProc adds the '@'). */
    public Map<String, Object> toParams() {
        Map<String, Object> out = new LinkedHashMap<>();
        for (Class<?> c = getClass(); c != null && c != DesktopModel.class; c = c.getSuperclass()) {
            for (Field f : c.getDeclaredFields()) {
                int m = f.getModifiers();
                if (Modifier.isStatic(m) || Modifier.isTransient(m) || f.isSynthetic()) continue;
                try {
                    f.setAccessible(true);
                    out.put(f.getName(), jdbc(f.get(this)));
                } catch (IllegalAccessException e) {
                    throw new IllegalStateException(e);
                }
            }
        }
        return out;
    }

    private static Object jdbc(Object v) {
        if (v instanceof LocalDateTime) return Timestamp.valueOf((LocalDateTime) v);
        if (v instanceof LocalDate) return Timestamp.valueOf(((LocalDate) v).atStartOfDay());
        if (v instanceof Double || v instanceof Float) return BigDecimal.valueOf(((Number) v).doubleValue());
        return v;
    }
}
