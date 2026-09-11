package com.mst.services;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.ChequeDetail;
import com.mst.repositories.IChequeDetailRepository;
import com.mst.serviceInterface.IChequeDetailService;

/** New (this reconstruction) - single-cheque CRUD, see ChequeDetail.java for scope notes. */
@Service
public class ChequeDetailService implements IChequeDetailService {

	@Autowired
	private IChequeDetailRepository chequeDetailRepository;

	@Override
	public List<ChequeDetail> getAllChequeDetails(String chequeInHandAccount) {
		return chequeDetailRepository.findByActiveTrueAndChequeInHandAccountOrderByChequeDate(chequeInHandAccount);
	}

	@Override
	public ChequeDetail addOrUpdateChequeDetail(ChequeDetail chequeDetail) {
		return chequeDetailRepository.save(chequeDetail);
	}

	@Override
	public ChequeDetail findChequeDetailById(long id) {
		return chequeDetailRepository.findById(id).orElse(null);
	}

	@Override
	public void deleteChequeDetailById(long id) {
		chequeDetailRepository.deleteById(id);
	}
}
