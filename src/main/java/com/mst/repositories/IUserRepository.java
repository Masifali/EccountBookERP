package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import com.mst.models.UserAccount;

public interface IUserRepository extends JpaRepository<UserAccount, Integer> {
    UserAccount findByUserName(String userName);
}
