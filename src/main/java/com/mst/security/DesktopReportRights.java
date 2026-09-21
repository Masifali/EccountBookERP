package com.mst.security;

import com.mst.models.RealCompanyRight;
import com.mst.models.RealUserRight;
import com.mst.models.UserAccount;
import com.mst.repositories.*;
import java.util.List;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Component;

/** Existing desktop CompanyRights + ScreenRights + tblUserRights; never creates grants. */
@Component
public class DesktopReportRights {
    private final IRealCompanyRightRepository companies;
    private final IRealScreenRightRepository screens;
    private final IRealUserRightRepository users;

    public DesktopReportRights(IRealCompanyRightRepository companies, IRealScreenRightRepository screens, IRealUserRightRepository users) {
        this.companies = companies;
        this.screens = screens;
        this.users = users;
    }

    public void require(UserAccount user, int screen, String action) {
        if (user == null) {
            throw new AccessDeniedException("User session required");
        }
        List<Integer> ids = List.of(screen);
        List<RealCompanyRight> activeCompanyRights = companies.findByCompanyIdAndScreenIdInAndIsActiveTrue(user.getCompanyId(), ids);
        
        if (activeCompanyRights.isEmpty()) {
            List<RealCompanyRight> configuredCompanyRights = companies.findByCompanyIdAndScreenIdIn(user.getCompanyId(), ids);
            if (!configuredCompanyRights.isEmpty()) {
                throw new AccessDeniedException("This report is not enabled for the company");
            }
        }

        List<Integer> rightIds = screens.findByScreenIdInAndRightName(ids, action).stream()
                .map(r -> r.getId())
                .collect(Collectors.toList());

        if (!rightIds.isEmpty()) {
            List<RealUserRight> grantedUserRights = users.findByUserIdAndCompanyIdAndRightIdInAndValueTrue(user.getId(), user.getCompanyId(), rightIds);
            if (grantedUserRights.isEmpty()) {
                List<RealUserRight> configuredUserRights = users.findByUserIdAndCompanyIdAndRightIdIn(user.getId(), user.getCompanyId(), rightIds);
                if (!configuredUserRights.isEmpty() || !"View".equalsIgnoreCase(action)) {
                    throw new AccessDeniedException("The user does not have " + action + " rights for this report");
                }
            }
        }
    }
}

