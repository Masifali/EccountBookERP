package com.mst.repositories;
import com.mst.models.dto.DesktopUserRightsChange;
import com.mst.security.CurrentUserContext;
import com.mst.services.DesktopUserRightsService;
import java.nio.file.*;
import java.util.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.condition.EnabledIfSystemProperty;
import org.springframework.jdbc.datasource.DataSourceTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;
import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;
@EnabledIfSystemProperty(named="inventory.rollback",matches="true")
class DesktopUserRightsLiveTest {
 @Test void checkScreenUsersMatchesDesktop() throws Exception{
  var jdbc=InventoryOpeningLiveTest.jdbc();var actor=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopUserRightsRepository(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(actor);var service=new DesktopUserRightsService(context,repo);
  assertEquals(jdbc.queryForList("EXEC dbo.USP_getScreensFromUserRights @CompanyId=?",actor.getCompanyId()),service.checkScreens());
  assertThrows(IllegalArgumentException.class,()->service.checkScreenUsers(0,0,""));assertThrows(IllegalArgumentException.class,()->service.checkScreenUsers(actor.getId(),-1,"View"));assertThrows(IllegalArgumentException.class,()->service.checkScreenUsers(actor.getId(),0,"unexpected"));
  var result=service.checkScreenUsers(actor.getId(),0,"View");for(boolean allocated:new boolean[]{true,false})assertEquals(new HashSet<>(jdbc.queryForList("EXEC dbo.USP_getUserByScreenName @CompanyId=?,@UserId=?,@RightName='View',@ActionId=?",actor.getCompanyId(),actor.getId(),allocated?1:0)),new HashSet<>((List<?>)result.get(allocated?"allocated":"unallocated")));
 }
 @Test void exportPermissionReconciliationInputs() throws Exception {
  var jdbc=InventoryOpeningLiveTest.jdbc();var actor=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopUserRightsRepository(jdbc);
  var apps=repo.apps(actor.getCompanyId(),actor.getId());assertEquals(1,apps.size());int app=((Number)apps.get(0).get("AppId")).intValue();
  var standard=repo.rights(actor.getCompanyId(),actor.getId(),0,0,app);var special=repo.special(actor.getCompanyId(),actor.getId());
  var grants=jdbc.queryForList("SELECT ur.Id AS GrantId,ur.RightId,ur.ScreenId,ur.AppId,ur.Value,s.ScreenName,s.ScreenAlias,s.ModuleId,sr.RightName FROM tblUserRights ur JOIN ScreenRights sr ON sr.Id=ur.RightId AND sr.ScreenID=ur.ScreenId JOIN ScreenDefinition s ON s.Id=ur.ScreenId WHERE ur.CompanyId=? AND ur.UserId=? ORDER BY ur.ScreenId,ur.RightId,ur.AppId",actor.getCompanyId(),actor.getId());
  var output=new LinkedHashMap<String,Object>();output.put("standardScreens",standard);output.put("special",special);output.put("grants",grants);
  new com.fasterxml.jackson.databind.ObjectMapper().writerWithDefaultPrettyPrinter().writeValue(Path.of("migration/user-rights/reconciliation-input.json").toFile(),output);
  assertFalse(standard.isEmpty());assertFalse(special.isEmpty());assertFalse(grants.isEmpty());
 }
 @Test void lookupFiltersMatchDesktopProcedureBodies() throws Exception{
  var jdbc=InventoryOpeningLiveTest.jdbc();jdbc.setQueryTimeout(110);var actor=InventoryOpeningLiveTest.user(jdbc);var repo=new DesktopUserRightsRepository(jdbc);
  for(int user: new int[]{0,actor.getId()})for(boolean report:new boolean[]{false,true})for(boolean screens:new boolean[]{false,true}){
   String name=screens?"usp_getScreensByModuleId":"usp_getModulesByCompanyId";
   String body=jdbc.queryForObject("SELECT OBJECT_DEFINITION(OBJECT_ID(?))",String.class,name);
   body=body.substring(body.indexOf("SELECT"),body.lastIndexOf("END")).trim();
   String sql="DECLARE @CompanyId int=?,@UserId int=?,@ModuleTypeId int=?,@ModuleId int=NULL; "+body+" OPTION(MAXDOP 1,MAX_GRANT_PERCENT=1,RECOMPILE)";
   var expected=jdbc.queryForList(sql,actor.getCompanyId(),user==0?null:user,report?null:1);
   var actual=screens?repo.filterScreens(actor.getCompanyId(),user,report):repo.filterModules(actor.getCompanyId(),user,report);
   assertEquals(new HashSet<>(expected),new HashSet<>(actual),name+" user="+user+" report="+report);
  }
 }
 @Test void desktopReadsAndTransactionalWritesLeavePermissionsUnchanged()throws Exception{
  var jdbc=InventoryOpeningLiveTest.jdbc();jdbc.setQueryTimeout(110);var actor=InventoryOpeningLiveTest.user(jdbc);var context=mock(CurrentUserContext.class);when(context.requireAccountingUser()).thenReturn(actor);var repo=new DesktopUserRightsRepository(jdbc);var service=new DesktopUserRightsService(context,repo);int c=actor.getCompanyId(),u=actor.getId(),app=DesktopUserRightsService.number(service.lookups().get("appId"));
  assertTrue(repo.admin(actor));assertFalse(service.lookups().isEmpty());var users=service.users(c);assertTrue(users.stream().anyMatch(r->DesktopUserRightsService.number(r.get("Id"))==u));assertTrue(users.stream().allMatch(r->r.keySet().equals(Set.of("Id","UserName","Title"))));assertFalse(service.apps(c,u).isEmpty());assertFalse(service.modules(c,u).isEmpty());assertFalse(service.filters(c,u,false).isEmpty());
  var rows=service.rights(c,u,0,0,app,false);assertFalse(rows.isEmpty());String reference=Files.readString(Path.of("migration/user-rights/procedures.txt"));reference=reference.substring(reference.indexOf("CREATE PROC [dbo].[usp_getScreenRightsByModuleAndUserId]"));reference=reference.substring(reference.indexOf("-- Drop the temporary table"),reference.indexOf("END -- END --"));reference=reference.replace("U.LastName;","U.LastName OPTION (MAXDOP 1, MAX_GRANT_PERCENT=1, RECOMPILE);");reference="DECLARE @CompanyId int=?,@UserId int=?,@AppId int=?,@ModuleId int=NULL,@ScreenId int=NULL,@ModuleTypeId int=NULL;\n"+reference;var direct=jdbc.queryForList(reference,c,u,app);assertEquals(direct.size(),rows.size());assertTrue(new HashSet<>(direct).equals(new HashSet<>(rows)),"All columns match original procedure body with a bounded memory execution hint.");var specials=service.special(c,u);assertFalse(specials.isEmpty());
  assertThrows(org.springframework.security.access.AccessDeniedException.class,()->service.users(-1));assertThrows(IllegalArgumentException.class,()->service.apps(c,Integer.MAX_VALUE));
  String snapshot="SELECT * FROM tblUserRights WHERE CompanyId="+c+" ORDER BY Id";String companySnapshot="SELECT * FROM CompanyRights WHERE CompanyId="+c+" ORDER BY Id";var before=jdbc.queryForList(snapshot);var companyBefore=jdbc.queryForList(companySnapshot);
  var tx=new TransactionTemplate(new DataSourceTransactionManager(jdbc.getDataSource()));tx.setIsolationLevel(org.springframework.transaction.TransactionDefinition.ISOLATION_SERIALIZABLE);
  tx.execute(status->{try{
   var row=rows.stream().filter(r->DesktopUserRightsService.number(r.get("SaveId"))>0).findFirst().orElseThrow();var change=new DesktopUserRightsChange();change.companyId=c;change.appId=app;var edit=new DesktopUserRightsChange.RightEdit();edit.userId=u;edit.screenId=DesktopUserRightsService.number(row.get("ScreenId"));edit.rightId=DesktopUserRightsService.number(row.get("SaveId"));edit.name="Save";edit.previousValue=DesktopUserRightsService.flag(row.get("Save"));edit.value=!edit.previousValue;change.edits.add(edit);service.update(change,false);assertEquals(edit.value,DesktopUserRightsService.flag(service.rights(c,u,0,edit.screenId,app,false).get(0).get("Save")));assertThrows(IllegalArgumentException.class,()->service.update(change,false));
   var special=specials.get(0);var sc=new DesktopUserRightsChange();sc.companyId=c;var se=new DesktopUserRightsChange.RightEdit();se.userId=u;se.screenId=DesktopUserRightsService.number(special.get("ScreenId"));se.rightId=DesktopUserRightsService.number(special.get("RightId"));se.previousValue=DesktopUserRightsService.flag(special.get("Value"));se.value=!se.previousValue;sc.edits.add(se);service.update(sc,true);assertEquals(se.value,DesktopUserRightsService.flag(service.special(c,u).stream().filter(r->DesktopUserRightsService.number(r.get("RightId"))==se.rightId).findFirst().orElseThrow().get("Value")));
   var candidate=repo.companyScreens(c,0,false).stream().filter(r->DesktopUserRightsService.number(r.get("ScreenId"))>0&&Boolean.TRUE.equals(jdbc.queryForObject("SELECT IsActive FROM ScreenDefinition WHERE Id=?",Boolean.class,r.get("ScreenId")))).findFirst().orElseThrow();int screen=DesktopUserRightsService.number(candidate.get("ScreenId"));int module=jdbc.queryForObject("SELECT ModuleId FROM ScreenDefinition WHERE Id=?",Integer.class,screen);var ca=new DesktopUserRightsChange();ca.companyId=c;ca.moduleId=module;ca.screenIds.add(screen);service.allocation(ca,true,true);assertTrue(repo.companyScreens(c,module,true).stream().anyMatch(r->DesktopUserRightsService.number(r.get("ScreenId"))==screen),"Company allocate screen "+screen);service.allocation(ca,false,true);assertFalse(repo.companyScreens(c,module,true).stream().anyMatch(r->DesktopUserRightsService.number(r.get("ScreenId"))==screen),"Company deactivate screen "+screen);
   var candidateUser=jdbc.queryForMap("SELECT TOP 1 s.Id,s.ModuleId FROM ScreenDefinition s JOIN CompanyRights cr ON cr.ScreenId=s.Id AND cr.CompanyId=? AND cr.IsActive=1 WHERE s.IsActive=1 AND EXISTS(SELECT 1 FROM ScreenRights sr WHERE sr.ScreenID=s.Id AND sr.RightName='View') AND NOT EXISTS(SELECT 1 FROM tblUserRights ur WHERE ur.ScreenId=s.Id AND ur.CompanyId=? AND ur.UserId=? AND ur.AppId=?) ORDER BY s.Id",c,c,u,app);var alloc=new DesktopUserRightsChange();alloc.companyId=c;alloc.userId=u;alloc.appId=app;alloc.moduleId=DesktopUserRightsService.number(candidateUser.get("ModuleId"));int sid=DesktopUserRightsService.number(candidateUser.get("Id"));alloc.screenIds.add(sid);service.allocation(alloc,true,false);assertTrue(repo.screens(c,u,alloc.moduleId,app,true).stream().anyMatch(r->DesktopUserRightsService.number(r.get("Id"))==sid),"User allocate "+sid);service.allocation(alloc,false,false);assertTrue(repo.screens(c,u,alloc.moduleId,app,false).stream().anyMatch(r->DesktopUserRightsService.number(r.get("Id"))==sid),"User unallocate "+sid);

   return null;
  }finally{status.setRollbackOnly();}});
  assertEquals(before,jdbc.queryForList(snapshot));assertEquals(companyBefore,jdbc.queryForList(companySnapshot));Files.writeString(Path.of("migration/user-rights/live-test.txt"),"Original procedure parity, company/user scope rejection, action-right optimistic update/reload, special-right update/reload, company allocate/deactivate, user allocate/unallocate passed. All writes rolled back; full company permission rows equal before snapshot. Read rows="+rows.size()+", special rows="+specials.size()+". No committed grants.\n");
 }
}
