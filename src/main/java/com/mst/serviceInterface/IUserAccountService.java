package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.UserAccount;

public interface IUserAccountService {
    List<UserAccount> getAll();
    UserAccount getById(Integer id);
    UserAccount getByUserName(String userName);
    UserAccount addOrUpdate(UserAccount userAccount, String plainTextPassword);
    void delete(Integer id);
    boolean userNameExists(String userName);
}
