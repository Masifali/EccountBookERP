package com.mst.serviceInterface;

import java.util.List;
import java.util.Map;
import com.mst.constants.RightType;

public interface IUserRightService {
    Map<String, Boolean> getGrantsForUser(Integer userId);
    boolean hasAccess(Integer userId, Integer screenId, RightType rightType);
    void saveGrantsForUser(Integer userId, List<String> grantedKeys);
}
