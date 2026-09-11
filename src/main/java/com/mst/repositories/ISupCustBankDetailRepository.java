package com.mst.repositories;

import com.mst.models.SupCustBankDetail;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface ISupCustBankDetailRepository extends JpaRepository<SupCustBankDetail, Integer> {
    List<SupCustBankDetail> findBySupCustId(Integer supCustId);
    void deleteBySupCustId(Integer supCustId);
}
