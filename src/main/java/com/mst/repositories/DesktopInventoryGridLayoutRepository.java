package com.mst.repositories;

import com.mst.models.UserAccount;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

@Repository
public class DesktopInventoryGridLayoutRepository {
    private final JdbcTemplate jdbc;
    public DesktopInventoryGridLayoutRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public boolean exists(UserAccount u,String name) {
        return !jdbc.queryForList("EXEC dbo.USP_GridLayout_ReadById @OrganizationId=?, @CompanyId=?, @Layout=?",u.getOrganizationId(),u.getCompanyId(),name).isEmpty();
    }
    public void save(UserAccount u,String form,String name) {
        // CtrlGrdBar stores the XML in C:\SCS\Layout and only the registration in SQL.
        jdbc.queryForList("EXEC dbo.Sp_GridLayout_Insert @Id=0, @ScreenName=?, @Layout=?, @EntryUserId=?, @OrganizationId=?, @CompanyId=?",form,name,u.getId(),u.getOrganizationId(),u.getCompanyId());
    }
    public void remove(UserAccount u,String name) {
        jdbc.update("EXEC dbo.USP_GridLayout_DeleteById @OrganizationId=?, @CompanyId=?, @Layout=?",u.getOrganizationId(),u.getCompanyId(),name);
    }
}
