package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.UserGroup;

public interface IUserGroupService {
    List<UserGroup> getAll();
    default List<UserGroup> getAllUserGroups() { return getAll(); }
    UserGroup getById(Integer id);
    UserGroup addOrUpdate(UserGroup userGroup);
    void delete(Integer id);
}
