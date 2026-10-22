package com.mst.projections;

/** Row of USP_GetUserRightsForViewbyUserId (every branch returns this shape) (native). */
public interface UserRightsViewRow {
    Integer getId();
    Integer getModuleID();
    String getModuleDescription();
    Integer getScreenID();
    String getScreenName();
    String getScreenAlias();
    Integer getUserId();
    Integer getRightsID();
    String getRightName();
    Boolean getValue();
    String getMenuControllName();
    String getModelMenuControllName();
    String getTargetUrl();
    String getIconUrl();
    Integer getModuleTypeId();
    Integer getAppId();
    String getApp();
    Boolean getIsFavorite();
    String getTblUserRightslst();
    Integer getCompanyId();
}
