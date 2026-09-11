package com.mst.repositories;

import org.springframework.data.jpa.repository.support.JpaEntityInformation;
import org.springframework.data.jpa.repository.support.SimpleJpaRepository;
import org.springframework.data.repository.history.support.RevisionEntityInformation;
import org.springframework.transaction.annotation.Transactional;

import javax.persistence.EntityManager;
import java.io.Serializable;

public class ExtendedRepositoryImpl<T, ID extends Serializable>
        extends SimpleJpaRepository<T, ID> implements IExtendedRepository<T, ID, Integer> {

    public JpaEntityInformation<T, ?> entityInformation;
    public RevisionEntityInformation revisionEntityInformation;
    private final EntityManager entityManager;


    public ExtendedRepositoryImpl(JpaEntityInformation<T, ?> entityInformation, EntityManager entityManager) {
        super(entityInformation, entityManager);

        this.entityInformation = entityInformation;
        this.revisionEntityInformation = revisionEntityInformation;
        this.entityManager = entityManager;

    }
/*public class ExtendedRepositoryImpl<T, ID extends Serializable, N extends Number & Comparable<N>>
        extends EnversRevisionRepositoryImpl<T, ID, N> implements IExtendedRepository<T, ID, N> {

    public JpaEntityInformation<T, ID> entityInformation;
    public RevisionEntityInformation revisionEntityInformation;
    public EntityManager entityManager;


    public ExtendedRepositoryImpl(JpaEntityInformation<T, ID> entityInformation, EntityManager entityManager, RevisionEntityInformation revisionEntityInformation) {
        super(entityInformation, revisionEntityInformation, entityManager);
        Assert.notNull(revisionEntityInformation);
        this.entityInformation = entityInformation;
        this.revisionEntityInformation = revisionEntityInformation;
        this.entityManager = entityManager;
    }*/

    @Override
    @Transactional
    public <S extends T> S refresh(S entity) {
        entityManager.refresh(entity);
        return entity;
    }


}
