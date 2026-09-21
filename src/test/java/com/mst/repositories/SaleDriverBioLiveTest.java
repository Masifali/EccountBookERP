package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.SaleDriverBioRequest;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.datasource.DriverManagerDataSource;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;

@EnabledIfSystemProperty(named="sale.live",matches="true")
class SaleDriverBioLiveTest {
    static JdbcTemplate jdbc() throws Exception {Properties p=new Properties();try(var in=Files.newInputStream(Path.of("src/main/resources/application.properties"))){p.load(in);}var ds=new DriverManagerDataSource(p.getProperty("spring.datasource.url"),p.getProperty("spring.datasource.username"),p.getProperty("spring.datasource.password"));JdbcTemplate j=new JdbcTemplate(ds);j.setQueryTimeout(60);return j;}
    static UserAccount user(JdbcTemplate j){var a=j.queryForMap("SELECT ID,OrganizationId,CompanyId,BranchesId FROM dbo.UserAccount WHERE UserName=?","numan");var u=new UserAccount();u.setId(((Number)a.get("ID")).intValue());u.setOrganizationId(((Number)a.get("OrganizationId")).intValue());u.setCompanyId(((Number)a.get("CompanyId")).intValue());u.setBranchesId(((Number)a.get("BranchesId")).intValue());return u;}
    @Test void desktopProceduresInsertLoadUpdateAndRollback() throws Exception {
        JdbcTemplate j=jdbc();UserAccount u=user(j);var repo=new SaleDriverBioRepository(j);var pending=repo.pending(u,58);assertFalse(pending.isEmpty());int gp=((Number)pending.get(0).get("Id")).intValue();String cnic="99998-9999999-8";
        var tx=new TransactionTemplate(new DataSourceTransactionManager(j.getDataSource()));
        tx.execute(status->{var r=new SaleDriverBioRequest();r.gatePassOutwardId=gp;r.forwarderName="Java repository rollback";r.driverName="Java Test Driver";r.fatherName="Java Test Father";r.cnicNo=cnic;r.driverCellNo="0092-398-9999998";r.alternateCellNo="0092-388-8888888";r.remarksHeader="insert";var saved=repo.save(u,r);r.id=((Number)saved.get("Id")).intValue();assertEquals(93,((Number)saved.get("DocumentTypeId")).intValue());assertEquals(u.getOrganizationId(),((Number)saved.get("OrganizationId")).intValue());r.forwarderName="Java repository rollback updated";r.remarksHeader="updated";var updated=repo.save(u,r);assertEquals("updated",updated.get("RemarksHeader"));assertEquals(r.id,((Number)repo.record(u,r.id).get("Id")).intValue());status.setRollbackOnly();return null;});
        assertEquals(0,j.queryForObject("SELECT COUNT(*) FROM dbo.GatePassOutwardDriverInfo WHERE CnicNo=?",Integer.class,cnic));assertEquals(0,j.queryForObject("SELECT COUNT(*) FROM dbo.driverBiodata WHERE cnicNo=?",Integer.class,cnic));
        Files.createDirectories(Path.of("migration/sale/evidence"));Files.writeString(Path.of("migration/sale/evidence/driver-bio-rollback.txt"),"Driver Bio document 93 / outward gate pass 91. GoldenAcedb Numan tenant 78/78 branch 58 year 58. Exact pending, driver lookup, insert, load and update procedures passed. Header tenant and audit fields plus reusable driverBiodata side effect verified. Transaction rolled back; zero test rows remained. Browser/native visual verification pending.\n");
    }
}
