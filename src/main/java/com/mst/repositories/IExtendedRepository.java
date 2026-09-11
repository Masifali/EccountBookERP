package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.repository.NoRepositoryBean;

import java.io.Serializable;

@NoRepositoryBean
public interface IExtendedRepository<T, ID extends Serializable, N extends Number & Comparable<N>>
        extends JpaRepository<T, ID> {
/*public interface IExtendedRepository<T, ID extends Serializable, N extends Number & Comparable<N>>
        extends RevisionRepository<T, ID, N>, JpaRepository<T, ID> {*/

    <S extends T> S refresh(S entity);

    // public <S extends T> S saveAndCommit(S entity);

/*@NoRepositoryBean
public interface IExtendedRepository<T, ID extends Serializable, N extends Number & Comparable<N>>
        extends EnversRevisionRepository<T, ID, N> {

    public <S extends T> S refresh(S entity);*/
}
