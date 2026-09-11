package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.CustomerGroup;
import com.mst.models.SupCustBankDetail;
import com.mst.models.SupplierCustomer;
import com.mst.models.SupplierCustomerMultiLingo;
import com.mst.models.SupplierCustomerShipToAddress;
import com.mst.models.SupplierCustomerType;

public interface ISupplierCustomerService {
    List<SupplierCustomer> getAll();
    List<SupplierCustomer> getByCustomerTypeId(Integer customerTypeId);
    SupplierCustomer getById(int id);
    SupplierCustomer addOrUpdate(SupplierCustomer supplierCustomer);
    void delete(int id);
    List<SupplierCustomerType> getAllTypes();
    List<CustomerGroup> getAllGroups();

    List<SupCustBankDetail> getBankDetailsByPartyId(Integer partyId);
    List<SupCustBankDetail> saveBankDetails(Integer partyId, List<SupCustBankDetail> bankDetails);

    List<SupplierCustomerShipToAddress> getShipToAddressesByPartyId(Integer partyId);
    List<SupplierCustomerShipToAddress> saveShipToAddresses(Integer partyId, List<SupplierCustomerShipToAddress> addresses);

    List<SupplierCustomerMultiLingo> getMultiLanguageNamesByPartyId(Integer partyId);
    List<SupplierCustomerMultiLingo> saveMultiLanguageNames(Integer partyId, List<SupplierCustomerMultiLingo> names);

    String generateNextPartyCode(Integer customerTypeId);
}
