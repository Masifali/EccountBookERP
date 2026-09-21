package com.mst.repositories;

import com.mst.models.Brand;
import com.mst.models.UserAccount;
import java.util.List;
import org.springframework.jdbc.core.BeanPropertyRowMapper;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

@Repository
public class DesktopBrandRepository {
    private final JdbcTemplate jdbc;
    public DesktopBrandRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public List<Brand> history(UserAccount u,Integer id) {
        return jdbc.query("EXEC dbo.USP_Brand_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?, @Id=?",
                BeanPropertyRowMapper.newInstance(Brand.class),u.getOrganizationId(),u.getCompanyId(),"FormHistory",id);
    }
    public Integer save(UserAccount u,Brand b) {
        return jdbc.queryForObject("EXEC dbo.USP_Brand_InsertAndUpdate @Id=?, @BrandName=?, @BrandCode=?, @isActive=?, @EntryUserId=?, @ModifyUserId=?, @OrganizationId=?, @CompanyId=?",
                Integer.class,b.getId()==null?0:b.getId(),b.getBrandName(),b.getBrandCode(),true,u.getId(),u.getId(),u.getOrganizationId(),u.getCompanyId());
    }
    public void delete(UserAccount u,int id) {
        jdbc.update("EXEC dbo.USP_Brand_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?, @Id=?",u.getOrganizationId(),u.getCompanyId(),"DeleteById",id);
    }
}
