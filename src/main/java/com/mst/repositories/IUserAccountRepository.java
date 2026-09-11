package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.UserAccount;

public interface IUserAccountRepository extends JpaRepository<UserAccount, Integer> {

	/**
	 * left join fetch UserGroup here, in the SAME query/session that loads the
	 * UserAccount - fixes a real crash: UserAccount.userGroup is @ManyToOne(LAZY),
	 * and CustomUserDetailsService (and CurrentUserContext) read
	 * account.getUserGroup().getUserGroupRole() well after the repository call's
	 * own transaction/session has already closed, which throws
	 * org.hibernate.LazyInitializationException: could not initialize proxy - no
	 * Session. Fetching it eagerly here, rather than switching the mapping itself to
	 * EAGER, keeps every other UserAccount query (list screens, etc.) from always
	 * paying for the join.
	 */
	@Query("select u from UserAccount u left join fetch u.userGroup where u.userName = ?1")
	UserAccount findByUserName(String userName);

	List<UserAccount> findAllByOrderByUserName();

	boolean existsByUserName(String userName);

	@Query("select coalesce(max(u.id), 0) from UserAccount u")
	int findMaxId();
}
