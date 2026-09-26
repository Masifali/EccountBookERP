package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.PayablesForecastRequest;
import java.sql.ResultSet;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

/** GeneralReprots.cs:14-58. Both datasets belong to the forecast procedure. */
@Repository
public class PayablesForecastRepository {
    private final JdbcTemplate jdbc;
    public PayablesForecastRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public List<List<Map<String,Object>>> load(UserAccount user,PayablesForecastRequest request){
        StringBuilder sql=new StringBuilder("EXEC dbo.usp_shorttermDueDateAnalysisPayablesAndReceivablesForcast @OrganizationId=?, @CompanyId=?, @FromDate=?");
        List<Object> args=new ArrayList<>(Arrays.asList(user.getOrganizationId(),user.getCompanyId(),java.sql.Date.valueOf(request.getFromDate())));
        if(request.getToDate()!=null){sql.append(", @ToDate=?");args.add(java.sql.Date.valueOf(request.getToDate()));}
        if(request.getIntervalDays()!=0){sql.append(", @DaysInterval=?");args.add(request.getIntervalDays());}
        if(request.getSortNo()!=0){sql.append(", @SortNo=?");args.add(request.getSortNo());}
        return jdbc.execute(sql.toString(),(PreparedStatementCallback<List<List<Map<String,Object>>>>) statement->{
            for(int i=0;i<args.size();i++)statement.setObject(i+1,args.get(i));
            List<List<Map<String,Object>>> sets=new ArrayList<>();
            boolean result=statement.execute();
            while(true){
                if(result){try(ResultSet rs=statement.getResultSet()){
                    List<Map<String,Object>> rows=new ArrayList<>();ColumnMapRowMapper mapper=new ColumnMapRowMapper();
                    while(rs.next())rows.add(mapper.mapRow(rs,rows.size()));
                    sets.add(ReportValueSupport.decimalStrings(rows));
                }}else if(statement.getUpdateCount()==-1)break;
                result=statement.getMoreResults();
            }
            return sets;
        });
    }
    public int amountDecimals(UserAccount user){return ReportValueSupport.amountDecimals(jdbc,user);}
}
