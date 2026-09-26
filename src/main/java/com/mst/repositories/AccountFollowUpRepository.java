package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.AccountFollowUpRequest;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

@Repository
public class AccountFollowUpRepository {
    private final JdbcTemplate jdbc;
    public AccountFollowUpRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
    public List<Map<String,Object>> history(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_AccountsPayRecFollowUpHistory_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",u.getOrganizationId(),u.getCompanyId());}
    public List<Map<String,Object>> accounts(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @UserId=?, @Activity='COAForCombobindig'",u.getOrganizationId(),u.getCompanyId(),u.getId());}
    @Transactional(rollbackFor=Exception.class)
    public int save(UserAccount u,AccountFollowUpRequest r){
        if(accounts(u).stream().noneMatch(a->a.get("Id") instanceof Number&&((Number)a.get("Id")).intValue()==r.getAccountId()))throw new IllegalArgumentException("Account is not allocated to this user");
        // Desktop INSERT allocates MAX(Id)+1. Hold a table lock across that procedure call.
        jdbc.queryForList("SELECT TOP (1) Id FROM dbo.AccountsPayRecFollowUpHistory WITH (TABLOCKX,HOLDLOCK)");
        if(r.getId()>0&&history(u).stream().noneMatch(h->((Number)h.get("Id")).intValue()==r.getId()))throw new IllegalArgumentException("Follow-up record does not belong to the current company");
        String sql="EXEC dbo.Sp_AccountsPayRecFollowUpHistory_"+(r.getId()==0?"Insert":"Update")+" @Id=?, @DocumentTypeIdRef=0, @ChartOfAccountId=?, @CommentsDetail=?, @CommentsDate=?, @FollowupDate=?, @NextFollowupDays=?, @PromiseDate=?, @EntryDate=?, @EntryUser=?, @OrganizationId=?, @CompanyId=?";
        Timestamp now=Timestamp.valueOf(LocalDateTime.now());
        Object[] args={r.getId(),r.getAccountId(),r.getComments(),now,Date.valueOf(r.getFollowupDate()),r.getNextFollowupDays(),Date.valueOf(r.getPromiseDate()),now,u.getId(),u.getOrganizationId(),u.getCompanyId()};
        if(r.getId()>0){jdbc.update(sql,args);return r.getId();}
        Integer id=jdbc.queryForObject(sql,Integer.class,args);
        if(id==null||id<=0)throw new IllegalStateException("The follow-up procedure did not return a saved record ID");return id;
    }
}
