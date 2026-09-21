package com.mst.repositories;
import com.mst.models.UserAccount;
import com.mst.models.dto.DesktopUserRightsChange;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
@Repository
public class DesktopUserRightsRepository {
 private final JdbcTemplate jdbc;
 public DesktopUserRightsRepository(JdbcTemplate jdbc){this.jdbc=jdbc;}
 private void execute(String sql,Object...args){jdbc.execute((org.springframework.jdbc.core.ConnectionCallback<Void>)con->{try(var statement=con.prepareStatement(sql)){for(int i=0;i<args.length;i++)statement.setObject(i+1,args[i]);boolean result=statement.execute();while(result||statement.getUpdateCount()!=-1){if(result){try(var rs=statement.getResultSet()){while(rs.next()){ /* consume desktop procedure result */ }}}result=statement.getMoreResults();}return null;}});}
 public boolean admin(UserAccount u){return jdbc.queryForObject("SELECT COUNT(*) FROM UserAccount u JOIN Role r ON r.Id=u.UserRoleId WHERE u.ID=? AND u.OrganizationId=? AND u.IsActive=1 AND r.Title='Admin'",Integer.class,u.getId(),u.getOrganizationId())>0;}
 public List<Map<String,Object>> companies(UserAccount u){return jdbc.queryForList("EXEC dbo.Sp_Company_GetAllMethod @OrgCompanyTypeId=?, @Activity='ReadByOrganizationId'",u.getOrganizationId()).stream().map(r->{Map<String,Object> m=new LinkedHashMap<>();m.put("Id",r.get("Id"));m.put("CompName",r.get("CompName"));return m;}).collect(Collectors.toList());}
 public List<Map<String,Object>> users(int company){return jdbc.queryForList("EXEC dbo.Sp_UserAccount_GetAllMethod @CompanyId=?, @ActionId=1, @Activity='ReadAllUser'",company).stream().map(r->{Map<String,Object> m=new LinkedHashMap<>();for(String k:List.of("Id","UserName","Title"))m.put(k,r.get(k));return m;}).collect(Collectors.toList());}
 public List<Map<String,Object>> userCompanies(UserAccount actor,int user){return jdbc.queryForList("EXEC dbo.sp_UserAccountAllocation_GetAllMethod @OrganizationId=?, @UserAccountId=?, @IsActive=1, @Activity='GetCompaniesByUserId'",actor.getOrganizationId(),user).stream().map(r->{Map<String,Object> m=new LinkedHashMap<>();m.put("Id",r.get("CompanyId"));m.put("CompName",r.get("CompName"));return m;}).collect(Collectors.toList());}
 public List<Map<String,Object>> apps(int company,int user){return jdbc.queryForList("EXEC dbo.USP_ApplicationsAllocateToUser_AllocatedData @CompanyId=?, @UserId=?",company,user).stream().map(r->{Map<String,Object> m=new LinkedHashMap<>();m.put("AppId",r.get("AppId"));m.put("App",r.get("App"));return m;}).collect(Collectors.toList());}
 public List<Map<String,Object>> modules(int company,int user){return jdbc.queryForList("EXEC dbo.USP_GetAppModulesByCompany @CompanyId=?, @UserId=?",company,user==0?null:user);}
 public List<Map<String,Object>> screens(int company,int user,int module,int app,boolean allocated){return jdbc.queryForList("EXEC dbo.USP_UserWiseAllocatedOrUnAllocatedScreens @CompanyId=?, @UserId=?, @ModuleId=?, @AppId=?, @ActionId=?",company,user,module,app==0?null:app,allocated?2:1);}
 public List<Map<String,Object>> companyScreens(int company,int module,boolean allocated){return jdbc.queryForList(allocated?"EXEC dbo.USP_CompanyRights_AllocatedData @CompanyId=?, @ModuleId=?":"EXEC dbo.USP_CompanyRights_UnAllocatedData @CompanyId=?, @ModuleId=?",company,module==0?null:module);}
 public List<Map<String,Object>> checkScreens(int company){return jdbc.queryForList("EXEC dbo.USP_getScreensFromUserRights @CompanyId=?",company);}
 public List<Map<String,Object>> checkScreenUsers(int company,int user,String screen,String right,boolean allocated){return jdbc.queryForList("EXEC dbo.USP_getUserByScreenName @CompanyId=?,@UserId=?,@ScreenName=?,@RightName=?,@ActionId=?",company,user==0?null:user,screen,right,allocated?1:0);}
 public List<Map<String,Object>> filterModules(int company,int user,boolean report){return jdbc.queryForList("SELECT DISTINCT AP.Id,AP.ModuleDescription,AP.ModuleTypeId FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId=S.Id AND S.IsActive=1 JOIN AppModules AP ON S.ModuleId=AP.Id WHERE UR.CompanyId=?"+(report?"":" AND AP.ModuleTypeId=1")+(user==0?"":" AND UR.UserId=?")+" OPTION(MAXDOP 1, MAX_GRANT_PERCENT=1, RECOMPILE)",user==0?new Object[]{company}:new Object[]{company,user});}
 public List<Map<String,Object>> filterScreens(int company,int user,boolean report){return jdbc.queryForList("SELECT DISTINCT S.Id,S.ScreenAlias,S.ModuleId FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId=S.Id JOIN AppModules AP ON S.ModuleId=AP.Id JOIN CompanyRights CR ON CR.ScreenId=S.Id AND CR.CompanyId=? WHERE UR.CompanyId=? AND ISNULL(CR.IsActive,0)=1"+(report?"":" AND AP.ModuleTypeId=1")+(user==0?"":" AND UR.UserId=?")+" OPTION(MAXDOP 1, MAX_GRANT_PERCENT=1, RECOMPILE)",user==0?new Object[]{company,company}:new Object[]{company,company,user});}
 public List<Map<String,Object>> rights(int company,int user,int module,int screen,int app){
  try(var in=new org.springframework.core.io.ClassPathResource("sql/user-rights-report.sql").getInputStream()){
   String sql=new String(in.readAllBytes(),java.nio.charset.StandardCharsets.UTF_8);
   var p=new org.springframework.jdbc.core.namedparam.MapSqlParameterSource().addValue("company",company).addValue("user",user==0?null:user,java.sql.Types.INTEGER).addValue("module",module==0?null:module,java.sql.Types.INTEGER).addValue("screen",screen==0?null:screen,java.sql.Types.INTEGER).addValue("app",app==0?null:app,java.sql.Types.INTEGER);
   return new org.springframework.jdbc.core.namedparam.NamedParameterJdbcTemplate(jdbc).queryForList(sql,p);
  }catch(java.io.IOException e){throw new IllegalStateException("Rights report SQL resource is missing",e);}
 }

 public List<Map<String,Object>> special(int company,int user){return jdbc.queryForList("EXEC dbo.USP_UserScreenRights_Special @CompanyId=?, @UserId=?",company,user);}
 public boolean hasViewDefinition(int screen){return jdbc.queryForObject("SELECT COUNT(*) FROM ScreenRights WHERE ScreenID=? AND RightName='View'",Integer.class,screen)>0;}
 public void allocate(UserAccount actor,DesktopUserRightsChange c){String ids=c.screenIds.stream().map(String::valueOf).collect(Collectors.joining(","))+",";execute("EXEC dbo.Sp_CompanyRights_GetAllMethod @CompanyId=?, @UserId=?, @AppId=?, @OrganizationId=?, @EntryUserId=?, @SelectedScreedIds=?, @Activity='ScreenRightstoAdmin'",c.companyId,c.userId,c.appId,actor.getOrganizationId(),actor.getId(),ids);}
 public void unallocate(UserAccount actor,DesktopUserRightsChange c){for(int id:c.screenIds)execute("EXEC dbo.Sp_CompanyRights_GetAllMethod @CompanyId=?, @UserId=?, @AppId=?, @OrganizationId=?, @EntryUserId=?, @Id=?, @Activity='DeleteByCompanyIdAndUserId'",c.companyId,c.userId,c.appId,actor.getOrganizationId(),actor.getId(),id);}
 public void update(DesktopUserRightsChange c,DesktopUserRightsChange.RightEdit e,boolean special){if(special)execute("EXEC dbo.Sp_tblUserRights_Update @CompanyId=?, @UserId=?, @ScreenId=?, @RightsID=?, @Value=?",c.companyId,e.userId,e.screenId,e.rightId,e.value);else execute("EXEC dbo.usp_UserRightsUpdate @CompanyId=?, @UserId=?, @ScreenId=?, @RightsID=?, @RightName=?, @Value=?, @AppId=?",c.companyId,e.userId,e.screenId,e.rightId,e.name,e.value,c.appId);}
 public void company(UserAccount actor,int company,Map<String,Object> row,boolean active){execute("EXEC dbo.Sp_CompanyRights_Insert @Id=?, @CompanyId=?, @ScreenId=?, @IsActive=?, @EntryUserId=?, @ModifyUserId=?",row.get("Id"),company,row.get("ScreenId"),active,actor.getId(),actor.getId());}
 public List<Map<String,Object>> rightDefinition(int screen,int right){return jdbc.queryForList("SELECT Id,RightName FROM ScreenRights WHERE ScreenID=? AND Id=?",screen,right);}
}
