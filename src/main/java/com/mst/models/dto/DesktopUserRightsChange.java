package com.mst.models.dto;
import java.util.*;
/** Explicit edits only; tenant and acting user are resolved server-side. */
public class DesktopUserRightsChange {
 public int companyId,userId,appId,moduleId;
 public List<Integer> screenIds=new ArrayList<>();
 public List<RightEdit> edits=new ArrayList<>();
 public static class RightEdit { public int userId,screenId,rightId; public String name; public boolean value,previousValue; }
}
