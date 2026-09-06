package com.mst.services;

import java.time.LocalDateTime;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.mst.models.CustomerGroup;
import com.mst.models.SupplierCustomer;
import com.mst.models.SupplierCustomerType;
import com.mst.repositories.ICustomerGroupRepository;
import com.mst.repositories.ISupplierCustomerRepository;
import com.mst.repositories.ISupplierCustomerTypeRepository;
import com.mst.security.CurrentUserContext;
import com.mst.serviceInterface.ISupplierCustomerService;

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
	public void delete(int id) {
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
}
