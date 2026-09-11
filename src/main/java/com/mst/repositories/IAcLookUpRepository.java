package com.mst.repositories;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import com.mst.models.AcLookUp;

public interface IAcLookUpRepository extends JpaRepository<AcLookUp, Integer> {

	List<AcLookUp> findByAcLookUpTypesIdOrderByAcLookUpsDescription(Integer acLookUpTypesId);

	@Query("select coalesce(max(a.id), 0) from AcLookUp a")
	int findMaxId();
}
