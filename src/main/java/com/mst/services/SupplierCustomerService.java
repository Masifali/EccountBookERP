package com.mst.services;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.CustomerGroup;
import com.mst.models.SupplierCustomer;
import com.mst.security.CurrentUserContext;
import com.mst.models.SupplierCustomerType;
import com.mst.models.SupCustBankDetail;
import com.mst.models.SupplierCustomerMultiLingo;
import com.mst.models.SupplierCustomerShipToAddress;
import com.mst.repositories.ICustomerGroupRepository;
import com.mst.repositories.ISupCustBankDetailRepository;
import com.mst.repositories.ISupplierCustomerMultiLingoRepository;
import com.mst.repositories.ISupplierCustomerRepository;
import com.mst.repositories.ISupplierCustomerShipToAddressRepository;
import com.mst.repositories.ISupplierCustomerTypeRepository;
import com.mst.serviceInterface.ISupplierCustomerService;
import org.springframework.transaction.annotation.Transactional;

/**
 * Ditto of the desktop's Supplier_Purchase BLL behind supfrmDefineSupplier.cs - see
 * SupplierCustomer.java for the real-table scope notes.
 *
 * ID GENERATION - the real dbo.SupplierCustomer.Id has no IDENTITY clause
 * (app-assigned), so save() computes the next id itself (max(id)+1), same
 * convention as every other non-identity master table in this port.
 */
@Service
public class SupplierCustomerService implements ISupplierCustomerService {

	@Autowired
	private ISupplierCustomerRepository supplierCustomerRepository;
	@Autowired
	private ISupplierCustomerTypeRepository supplierCustomerTypeRepository;
	@Autowired
	private ICustomerGroupRepository customerGroupRepository;
	@Autowired
	private ISupCustBankDetailRepository supCustBankDetailRepository;
	@Autowired
	private ISupplierCustomerShipToAddressRepository supplierCustomerShipToAddressRepository;
	@Autowired
	private ISupplierCustomerMultiLingoRepository supplierCustomerMultiLingoRepository;
	@Autowired
	private CurrentUserContext currentUserContext;

	@Override
	public List<SupplierCustomer> getAll() {
		return supplierCustomerRepository.findAllByOrderByCompanyName();
	}

	@Override
	public List<SupplierCustomer> getByCustomerTypeId(Integer customerTypeId) {
		if (customerTypeId == null) {
			return getAll();
		}
		return supplierCustomerRepository.findByCustomerTypeIdOrderByCompanyName(customerTypeId);
	}

	@Override
	public SupplierCustomer getById(int id) {
		return supplierCustomerRepository.findById(id).orElse(null);
	}

	@Override
	public SupplierCustomer addOrUpdate(SupplierCustomer supplierCustomer) {
		LocalDateTime now = LocalDateTime.now();
		if (supplierCustomer.getId() == null) {
			supplierCustomer.setId(supplierCustomerRepository.findMaxId() + 1);
			supplierCustomer.setOrganizationId(currentUserContext.currentOrganizationId());
			supplierCustomer.setCompanyId(currentUserContext.currentCompanyId());
			supplierCustomer.setEntryDate(now);
			supplierCustomer.setEntryUser(currentUserContext.currentUserId());
			if (supplierCustomer.getStatus() == null) {
				supplierCustomer.setStatus(true);
			}
		} else {
			supplierCustomer.setModifyDate(now);
			supplierCustomer.setModifyUser(currentUserContext.currentUserId());
		}
		return supplierCustomerRepository.save(supplierCustomer);
	}

	@Override
	@Transactional
	public void delete(int id) {
		try {
			supCustBankDetailRepository.deleteBySupCustId(id);
			supplierCustomerShipToAddressRepository.deleteBySupplierCustomerId(id);
			supplierCustomerMultiLingoRepository.deleteBySupplierCustomerId(id);
		} catch (Exception ignored) {}
		supplierCustomerRepository.deleteById(id);
	}

	@Override
	public List<SupplierCustomerType> getAllTypes() {
		return supplierCustomerTypeRepository.findAllByOrderBySupplierCustomerType();
	}

	@Override
	public List<CustomerGroup> getAllGroups() {
		return customerGroupRepository.findAllByOrderByDescription();
	}

	@Override
	public List<SupCustBankDetail> getBankDetailsByPartyId(Integer partyId) {
		if (partyId == null) return java.util.Collections.emptyList();
		return supCustBankDetailRepository.findBySupCustId(partyId);
	}

	@Override
	@Transactional
	public List<SupCustBankDetail> saveBankDetails(Integer partyId, List<SupCustBankDetail> bankDetails) {
		if (partyId == null) return java.util.Collections.emptyList();
		supCustBankDetailRepository.deleteBySupCustId(partyId);
		if (bankDetails != null && !bankDetails.isEmpty()) {
			for (SupCustBankDetail b : bankDetails) {
				b.setId(null);
				b.setSupCustId(partyId);
				b.setOrganizationId(currentUserContext.currentOrganizationId());
				b.setCompanyId(currentUserContext.currentCompanyId());
				b.setEntryDate(LocalDateTime.now());
				b.setEntryUser(currentUserContext.currentUserId());
				supCustBankDetailRepository.save(b);
			}
		}
		return getBankDetailsByPartyId(partyId);
	}

	@Override
	public List<SupplierCustomerShipToAddress> getShipToAddressesByPartyId(Integer partyId) {
		if (partyId == null) return java.util.Collections.emptyList();
		return supplierCustomerShipToAddressRepository.findBySupplierCustomerId(partyId);
	}

	@Override
	@Transactional
	public List<SupplierCustomerShipToAddress> saveShipToAddresses(Integer partyId, List<SupplierCustomerShipToAddress> addresses) {
		if (partyId == null) return java.util.Collections.emptyList();
		supplierCustomerShipToAddressRepository.deleteBySupplierCustomerId(partyId);
		if (addresses != null && !addresses.isEmpty()) {
			for (SupplierCustomerShipToAddress a : addresses) {
				a.setId(null);
				a.setSupplierCustomerId(partyId);
				a.setOrganizationId(currentUserContext.currentOrganizationId());
				a.setCompanyId(currentUserContext.currentCompanyId());
				a.setEntryDate(LocalDateTime.now());
				a.setEntryUser(currentUserContext.currentUserId());
				supplierCustomerShipToAddressRepository.save(a);
			}
		}
		return getShipToAddressesByPartyId(partyId);
	}

	@Override
	public List<SupplierCustomerMultiLingo> getMultiLanguageNamesByPartyId(Integer partyId) {
		if (partyId == null) return java.util.Collections.emptyList();
		return supplierCustomerMultiLingoRepository.findBySupplierCustomerId(partyId);
	}

	@Override
	@Transactional
	public List<SupplierCustomerMultiLingo> saveMultiLanguageNames(Integer partyId, List<SupplierCustomerMultiLingo> names) {
		if (partyId == null) return java.util.Collections.emptyList();
		supplierCustomerMultiLingoRepository.deleteBySupplierCustomerId(partyId);
		if (names != null && !names.isEmpty()) {
			for (SupplierCustomerMultiLingo m : names) {
				m.setId(null);
				m.setSupplierCustomerId(partyId);
				supplierCustomerMultiLingoRepository.save(m);
			}
		}
		return getMultiLanguageNamesByPartyId(partyId);
	}

	@Override
	public String generateNextPartyCode(Integer customerTypeId) {
		String prefix = "PRT";
		if (customerTypeId != null) {
			SupplierCustomerType type = supplierCustomerTypeRepository.findById(customerTypeId).orElse(null);
			if (type != null && type.getSupplierCustomerType() != null) {
				String tName = type.getSupplierCustomerType().toLowerCase();
				if (tName.contains("vendor") || tName.contains("supplier")) prefix = "SUP";
				else if (tName.contains("customer")) prefix = "CUST";
			}
		}
		int nextId = supplierCustomerRepository.findMaxId() + 1;
		return String.format("%s-%05d", prefix, nextId);
	}
}
