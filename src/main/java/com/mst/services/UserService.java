package com.mst.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import com.mst.models.UserAccount;
import com.mst.repositories.IUserRepository;

@Service
public class UserService {
    @Autowired
    private IUserRepository userRepository;

    public UserAccount findByUserName(String userName) {
        return userRepository.findByUserName(userName);
    }
}
