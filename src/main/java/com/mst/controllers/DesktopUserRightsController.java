package com.mst.controllers;
import com.mst.services.DesktopUserRightsService;
import com.mst.models.dto.DesktopUserRightsChange;
import java.util.*;
import javax.servlet.http.HttpSession;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
import org.springframework.http.*;
import org.springframework.security.access.AccessDeniedException;
@Controller
public class DesktopUserRightsController {
 private final DesktopUserRightsService service;
 public DesktopUserRightsController(DesktopUserRightsService service){this.service=service;}
 @GetMapping({"/user-management/rights","/configurations/user-rights"}) public String page(){service.admin();return "userAccounts/desktop_rights";}
 @GetMapping("/user-management/rights/api/lookups") @ResponseBody public Map<String,Object> lookups(HttpSession session){Map<String,Object> result=new LinkedHashMap<>(service.lookups());synchronized(session){if(session.getAttribute("rightsCsrf")==null)session.setAttribute("rightsCsrf",UUID.randomUUID().toString());result.put("token",session.getAttribute("rightsCsrf"));}return result;}
 @GetMapping("/user-management/rights/api/check-screens") @ResponseBody public Object checkScreens(){return service.checkScreens();}
 @GetMapping("/user-management/rights/api/check-screen-users") @ResponseBody public Object checkScreenUsers(@RequestParam(defaultValue="0") int userId,@RequestParam(defaultValue="0") int screenId,@RequestParam(defaultValue="") String right){return service.checkScreenUsers(userId,screenId,right);}
 @GetMapping("/user-management/rights/api/users") @ResponseBody public Object users(@RequestParam int companyId){return service.users(companyId);}
 @GetMapping("/user-management/rights/api/user-companies") @ResponseBody public Object userCompanies(@RequestParam int userId){return service.userCompanies(userId);}
 @GetMapping("/user-management/rights/api/apps") @ResponseBody public Object apps(@RequestParam int companyId,@RequestParam int userId){return service.apps(companyId,userId);}
 @GetMapping("/user-management/rights/api/modules") @ResponseBody public Object modules(@RequestParam int companyId,@RequestParam(defaultValue="0") int userId){return service.modules(companyId,userId);}
 @GetMapping("/user-management/rights/api/screens") @ResponseBody public Object screens(@RequestParam int companyId,@RequestParam(defaultValue="0") int userId,@RequestParam int moduleId,@RequestParam(defaultValue="0") int appId,@RequestParam(defaultValue="false") boolean companyMode){return service.screens(companyId,userId,moduleId,appId,companyMode);}
 @GetMapping("/user-management/rights/api/filters") @ResponseBody public Object filters(@RequestParam int companyId,@RequestParam(defaultValue="0") int userId,@RequestParam(defaultValue="false") boolean report){return service.filters(companyId,userId,report);}
 @GetMapping("/user-management/rights/api/rows") @ResponseBody public Object rows(@RequestParam int companyId,@RequestParam(defaultValue="0") int userId,@RequestParam(defaultValue="0") int moduleId,@RequestParam(defaultValue="0") int screenId,@RequestParam(defaultValue="0") int appId,@RequestParam(defaultValue="false") boolean report){return service.rights(companyId,userId,moduleId,screenId,appId,report);}
 @GetMapping("/user-management/rights/api/special") @ResponseBody public Object special(@RequestParam int companyId,@RequestParam int userId){return service.special(companyId,userId);}
 @PostMapping("/user-management/rights/api/{action:allocate|unallocate|company-allocate|company-unallocate|update|special-update}") @ResponseBody public Object write(@PathVariable String action,@RequestBody DesktopUserRightsChange change,@RequestHeader("X-Rights-CSRF") String token,HttpSession session){if(!Objects.equals(token,session.getAttribute("rightsCsrf")))throw new AccessDeniedException("Reload User Rights before saving.");if(action.endsWith("update"))service.update(change,action.startsWith("special"));else service.allocation(change,!action.endsWith("unallocate"),action.startsWith("company"));return Map.of("message","Rights updated successfully.");}
 @ExceptionHandler(AccessDeniedException.class) @ResponseBody public ResponseEntity<?> denied(Exception e){return ResponseEntity.status(403).body(Map.of("message",e.getMessage()));}
 @ExceptionHandler(IllegalArgumentException.class) @ResponseBody public ResponseEntity<?> invalid(Exception e){return ResponseEntity.badRequest().body(Map.of("message",e.getMessage()));}
 @ExceptionHandler(org.springframework.dao.DataAccessException.class) @ResponseBody public ResponseEntity<?> database(Exception e){return ResponseEntity.status(409).body(Map.of("message","The desktop rights query could not complete. Refresh and retry; no partial changes were saved."));}
}
