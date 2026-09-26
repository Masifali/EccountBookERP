package com.mst.services;
import com.mst.models.UserAccount;
import com.mst.models.dto.DesktopUserRightsChange;
import com.mst.repositories.DesktopUserRightsRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.transaction.annotation.*;
@Service
public class DesktopUserRightsService {
 private final CurrentUserContext context; private final DesktopUserRightsRepository repo;
 private static final Map<String,String> ACTIONS=Map.of("Save","Save","Update","Update","Delete","Delete","Print","Print","CanView AllRecord","CanViewAllRecord","Grid Print","GridPrint","Grid Export","GridExport");
 public DesktopUserRightsService(CurrentUserContext context,DesktopUserRightsRepository repo){this.context=context;this.repo=repo;}
 public boolean canManage(){try{return repo.admin(context.requireAccountingUser());}catch(org.springframework.security.core.AuthenticationException|AccessDeniedException e){return false;}}
 public UserAccount admin(){UserAccount u=context.requireAccountingUser();if(!repo.admin(u))throw new AccessDeniedException("User Rights requires the desktop Admin role.");return u;}
 private UserAccount company(int company){UserAccount u=admin();if(repo.companies(u).stream().noneMatch(r->number(r.get("Id"))==company))throw new AccessDeniedException("Company is outside your organization.");return u;}
 private void user(int company,int user){if(user<=0||repo.users(company).stream().noneMatch(r->number(r.get("Id"))==user))throw new IllegalArgumentException("Select a user allocated to this company.");}
 private void app(int company,int user,int app){if(app<=0||repo.apps(company,user).stream().noneMatch(r->number(r.get("AppId"))==app))throw new IllegalArgumentException("Select an application allocated to this user.");}
 public static int number(Object v){return v instanceof Number?((Number)v).intValue():0;}
 public static boolean flag(Object v){return Boolean.TRUE.equals(v)||"true".equalsIgnoreCase(String.valueOf(v))||"1".equals(String.valueOf(v));}
 private int activeApp(UserAccount u){var apps=repo.apps(u.getCompanyId(),u.getId());if(u.getAppId()!=null&&u.getAppId()>0&&apps.stream().anyMatch(r->number(r.get("AppId"))==u.getAppId()))return u.getAppId();return apps.size()==1?number(apps.get(0).get("AppId")):0;}
 public Map<String,Object> lookups(){UserAccount u=admin();return Map.of("companies",repo.companies(u),"companyId",u.getCompanyId(),"userId",u.getId(),"appId",activeApp(u));}
 public List<Map<String,Object>> checkScreens(){return repo.checkScreens(admin().getCompanyId());}
 public Map<String,Object> checkScreenUsers(int userId,int screenId,String right){UserAccount actor=admin();int c=actor.getCompanyId();if(userId==0&&screenId==0&&(right==null||right.isBlank()))throw new IllegalArgumentException("Please select at least one criterion first.");if(userId!=0)user(c,userId);if(right!=null&&!right.isBlank()&&!Set.of("View","Save","Update").contains(right))throw new IllegalArgumentException("Select a desktop right name.");String screen=null;if(screenId!=0)screen=String.valueOf(repo.checkScreens(c).stream().filter(r->number(r.get("ScreenId"))==screenId).findFirst().orElseThrow(()->new IllegalArgumentException("Screen is outside the current company.")).get("ScreenName"));String name=right==null||right.isBlank()?null:right;return Map.of("allocated",repo.checkScreenUsers(c,userId,screen,name,true),"unallocated",repo.checkScreenUsers(c,userId,screen,name,false));}
 public List<Map<String,Object>> users(int c){company(c);return repo.users(c);}
 public List<Map<String,Object>> userCompanies(int u){UserAccount actor=admin();user(actor.getCompanyId(),u);return repo.userCompanies(actor,u);}
 public List<Map<String,Object>> apps(int c,int u){company(c);user(c,u);return repo.apps(c,u);}
 public List<Map<String,Object>> modules(int c,int u){company(c);if(u!=0)user(c,u);return repo.modules(c,u);}
 public Map<String,Object> screens(int c,int u,int m,int a,boolean companyMode){company(c);if(m<=0)throw new IllegalArgumentException("Select a module first.");if(companyMode)return Map.of("allocated",repo.companyScreens(c,m,true),"unallocated",repo.companyScreens(c,m,false));user(c,u);app(c,u,a);return Map.of("allocated",repo.screens(c,u,m,a,true),"unallocated",repo.screens(c,u,m,a,false));}
 public Map<String,Object> filters(int c,int u,boolean report){company(c);if(u!=0)user(c,u);return Map.of("modules",repo.filterModules(c,u,report),"screens",repo.filterScreens(c,u,report));}
 public List<Map<String,Object>> rights(int c,int u,int m,int s,int a,boolean report){UserAccount actor=company(c);if(c!=actor.getCompanyId())throw new AccessDeniedException("Use your current company for the rights grid.");if(u!=0)user(c,u);return repo.rights(c,u,m,s,report?0:a);}
 public List<Map<String,Object>> special(int c,int u){company(c);user(c,u);return repo.special(c,u);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public void allocation(DesktopUserRightsChange c,boolean active,boolean companyMode){UserAccount actor=company(c.companyId);if(c.screenIds==null||c.screenIds.isEmpty()||c.screenIds.size()>1000||new HashSet<>(c.screenIds).size()!=c.screenIds.size())throw new IllegalArgumentException("Select distinct screen rows first.");List<Map<String,Object>> eligible;if(companyMode)eligible=repo.companyScreens(c.companyId,c.moduleId,!active);else{user(c.companyId,c.userId);app(c.companyId,c.userId,c.appId);eligible=repo.screens(c.companyId,c.userId,c.moduleId,c.appId,!active);}String key=companyMode?"ScreenId":"Id";for(Integer id:c.screenIds)if(id==null||eligible.stream().noneMatch(r->number(r.get(key))==id))throw new IllegalArgumentException("Screen allocation changed. Refresh before saving.");if(!companyMode&&active&&c.screenIds.stream().anyMatch(id->!repo.hasViewDefinition(id)))throw new IllegalArgumentException("A selected screen has no View right definition in the desktop database.");if(companyMode){for(int id:c.screenIds)repo.company(actor,c.companyId,eligible.stream().filter(r->number(r.get(key))==id).findFirst().orElseThrow(),active);}else if(active)repo.allocate(actor,c);else repo.unallocate(actor,c);}
 @Transactional(isolation=Isolation.SERIALIZABLE)
 public void update(DesktopUserRightsChange c,boolean special){
  UserAccount actor=company(c.companyId);
  if(!special&&c.companyId!=actor.getCompanyId())throw new AccessDeniedException("Use your current company for the rights grid.");
  if(c.edits==null||c.edits.isEmpty()||c.edits.size()>10000)throw new IllegalArgumentException("No rights changes to save.");
  Set<String> seen=new HashSet<>();
  Map<Integer,List<Map<String,Object>>> snapshots=new HashMap<>();
  // Validate every edit before writing; reuse one snapshot per target user for bulk changes.
  for(var e:c.edits){
   if(e==null||e.userId<=0||e.screenId<=0||e.rightId<=0)throw new IllegalArgumentException("Choose valid rights from the loaded grid.");
   if(!seen.add(e.userId+":"+e.screenId+":"+e.rightId))throw new IllegalArgumentException("Duplicate right edit.");
   if(!snapshots.containsKey(e.userId)){
    user(c.companyId,e.userId);
    if(!special)app(c.companyId,e.userId,c.appId);
    snapshots.put(e.userId,special?repo.special(c.companyId,e.userId):repo.rights(c.companyId,e.userId,0,0,c.appId));
   }
   boolean current;
   if(special){
    var row=snapshots.get(e.userId).stream().filter(r->number(r.get("ScreenId"))==e.screenId&&number(r.get("RightId"))==e.rightId).findFirst().orElseThrow(()->new IllegalArgumentException("Special right is no longer allocated."));
    current=flag(row.get("Value"));
   }else{
    String column=ACTIONS.get(e.name);
    if(column==null)throw new IllegalArgumentException("Unsupported action right.");
    var row=snapshots.get(e.userId).stream().filter(r->number(r.get("ScreenId"))==e.screenId).findFirst().orElseThrow(()->new IllegalArgumentException("Screen is no longer allocated."));
    if(number(row.get(column+"Id"))!=e.rightId)throw new IllegalArgumentException("Right changed; reload before saving.");
    current=flag(row.get(column));
   }
   if(current!=e.previousValue)throw new IllegalArgumentException("Another administrator changed this right. Reload before saving.");
  }
  for(var e:c.edits)repo.update(c,e,special);
 }
}
