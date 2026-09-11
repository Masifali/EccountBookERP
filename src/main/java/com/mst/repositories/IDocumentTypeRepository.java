package com.mst.repositories;

import org.springframework.data.jpa.repository.JpaRepository;

import com.mst.models.DocumentType;

public interface IDocumentTypeRepository extends JpaRepository<DocumentType, Integer> {

	DocumentType findByDocumentTypeCode(String documentTypeCode);
}
