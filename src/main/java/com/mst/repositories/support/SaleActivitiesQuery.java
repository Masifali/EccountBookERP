package com.mst.repositories.support;

import org.springframework.core.io.ClassPathResource;
import org.springframework.jdbc.core.JdbcTemplate;
import java.io.*;
import java.nio.charset.StandardCharsets;
import java.util.*;
import java.util.regex.Pattern;

/** Keeps the desktop calculations without its globally shared report scratch table. */
public final class SaleActivitiesQuery {
    private SaleActivitiesQuery() { }
    private static final String SQL=load("sale-invoice-activities.sql");
    private static final String LOOKUPS=load("sale-invoice-activities-lookups.sql");
    private static final Pattern PARAMETERS=Pattern.compile("(?m)^DECLARE @(\\w+) .+? = \\?;$");
    public static List<Map<String,Object>> rows(JdbcTemplate jdbc,Map<String,Object> values){
        return execute(jdbc,SQL,values);
    }
    public static List<Map<String,Object>> lookups(JdbcTemplate jdbc,Map<String,Object> values){
        return execute(jdbc,LOOKUPS,values);
    }
    private static List<Map<String,Object>> execute(JdbcTemplate jdbc,String sql,Map<String,Object> values){
        return jdbc.queryForList(sql,PARAMETERS.matcher(sql).results().map(match->values.get(match.group(1))).toArray());
    }
    private static String load(String name){
        try(var in=new ClassPathResource("sql/"+name).getInputStream()){
            return new String(in.readAllBytes(),StandardCharsets.UTF_8);
        }catch(IOException e){throw new UncheckedIOException("Sale Invoice Activities SQL is missing",e);}
    }
}
