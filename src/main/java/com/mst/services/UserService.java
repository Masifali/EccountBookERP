package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.mst.models.UserAccount;
import com.mst.repositories.IUserAccountRepository;

@Service
public class UserService {
    @Autowired
    @org.springframework.beans.factory.annotation.Qualifier("IUserAccountRepository")
    private IUserAccountRepository userRepository;

    public UserAccount findByUserName(String userName) {
        return userRepository.findByUserName(userName);
    }
}
