package com.mst.services;

import com.mst.models.MillRate;
import com.mst.models.MillRateEntry;
import com.mst.repositories.IMillRateEntryRepository;
import com.mst.repositories.IMillRateRepository;
import com.mst.serviceInterface.IAccountService;
import com.mst.serviceInterface.IItemDefService;
import com.mst.serviceInterface.IMillService;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.*;


@Slf4j
@Service("millRateService")
public class MillRateService {
    @Autowired
    IMillRateRepository millRateRepository;
    @Autowired
    IMillRateEntryRepository millRateEntryRepository;
    @Autowired
    IAccountService accountService;
    @Autowired
    IItemDefService itemDefService;
    @Autowired
    IMillService millService;

    public List<MillRate> addOrUpdate(List<MillRate> millRateList) {
        for (MillRate millRate : millRateList) {
            millRate.setMill(millService.findById(millRate.getMill().getId()));
            millRate.setItemDef(itemDefService.getItemDefById(millRate.getItemDef().getId()));
            for (MillRateEntry millRateEntry : millRate.getMillRateEntries()) {
                millRateEntry.setSupplierAccount(accountService.getAccountByCode(millRateEntry.getSupplierAccount().getCode()));
                millRateEntry.setMillRate(millRate);
                millRateEntry.setEntryDate(millRate.getEntryDate());
            }
            List<MillRateEntry> millRateEntriesToRemove = new ArrayList<MillRateEntry>();
            MillRate millRateExist = millRateRepository.findById(millRate.getId());
            if (millRateExist != null) {
                for (MillRateEntry millRateEntryExist : millRateExist.getMillRateEntries()) {

                    boolean entryMatch = false;

                    for (MillRateEntry millRateEntry : millRate.getMillRateEntries()) {
                        if (millRateEntry.getId() == millRateEntryExist.getId()) {
                            entryMatch = true;
                            break;
                        }
                    }

                    if (!entryMatch) {

                        millRateEntriesToRemove.add(millRateEntryExist);
                    }
                }

                for (MillRateEntry millRateEntryToRemove : millRateEntriesToRemove) {
                    millRateEntryRepository.deleteById(millRateEntryToRemove.getId());
                    log.info("Success", "Record Successfully Deleted");

                }
            }
        }
        return millRateRepository.saveAll(millRateList);
    }

    public MillRate updateMillRateEntry(MillRate millRate) {
        return millRateRepository.save(millRate);
    }

    public List<Map<String, Object>> getMilllRate(long entryID) {
        List<MillRate> millRateListDummy = new ArrayList<>();
        MillRate milllRate = millRateRepository.findById(entryID);
        millRateListDummy.add(milllRate);
        List<Map<String, Object>> millrates = new ArrayList<>();
        List<Map<String, Object>> millrateEntries = new ArrayList<>();
        Map<String, Object> temMillRate = new HashMap<>();
        Map<String, Object> mapMillRateEntry = new HashMap<>();
        for (MillRate millRateDummy : millRateListDummy) {
            millrateEntries = new ArrayList<>();
            temMillRate = new HashMap<>();
            temMillRate.put("mill", millRateDummy.getMill().getId());
            // temMillRate.put("item", millRateEntry.getItemDef().getFormattedCode() + "  " + millRateEntry.getItemDef().getItemSubCategory().getName() + "  " + millRateEntry.getItemDef().getName());
            temMillRate.put("item", millRateDummy.getItemDef().getItemSubCategory().getName() + "  " + millRateDummy.getItemDef().getName());
            temMillRate.put("itemDef", millRateDummy.getItemDef().getId());
            temMillRate.put("millId", millRateDummy.getMill().getId());
            temMillRate.put("entryDate", millRateDummy.getEntryDate());
            temMillRate.put("id", millRateDummy.getId());
            temMillRate.put("expand", false);
            temMillRate.put("saleRate", millRateDummy.getSaleRate());
            temMillRate.put("millRate", millRateDummy.getMillRate());
            temMillRate.put("cashRate", millRateDummy.getCashRate());
            temMillRate.put("moisture", millRateDummy.getMoisture());
            for (MillRateEntry millRateEntry : millRateDummy.getMillRateEntries()) {
                mapMillRateEntry = new HashMap<>();
                mapMillRateEntry.put("supplierAccount", millRateEntry.getSupplierAccount() != null ? millRateEntry.getSupplierAccount().getCode() : 0);
                mapMillRateEntry.put("qty", millRateEntry.getQty());
                mapMillRateEntry.put("code", millRateEntry.getSupplierAccount() != null ? millRateEntry.getSupplierAccount().getCode() : 0);
                mapMillRateEntry.put("date", millRateEntry.getEntryDate());
                mapMillRateEntry.put("rate", millRateEntry.getRate());
                mapMillRateEntry.put("id", millRateEntry.getId());
                //  mapMillRateEntry.put("expand", false);
                millrateEntries.add(mapMillRateEntry);
            }
            temMillRate.put("millRateEntries", millrateEntries);
            // temMillRate.put("cashRate", millRateEntry.getCashRate());
            temMillRate.put("remarks", millRateDummy.getRemarks());
            millrates.add(temMillRate);

        }
        return millrates;
    }

    public void deleteById(long id) {
        millRateRepository.deleteById(id);
    }

    public List<Map<String, Object>> getMilRatesList(long itemDefId, long millRate, Date fromDate, Date toDate) {
        List<Map<String, Object>> millrates = new ArrayList<>();
        List<Map<String, Object>> millrateEntries = new ArrayList<>();
        Map<String, Object> temMillRate = new HashMap<>();
        Map<String, Object> mapMillRateEntry = new HashMap<>();
        //List<MillRate> ratesByFilter = millRateRepository.getRatesByFilter(itemDefId, millRate, fromDate, toDate);
        for (MillRate millRateDummy : millRateRepository.getRatesByFilter(itemDefId, millRate, fromDate, toDate)) {
            millrateEntries = new ArrayList<>();
            temMillRate = new HashMap<>();
            temMillRate.put("mill", millRateDummy.getMill().getAccountName());
            // temMillRate.put("item", millRateEntry.getItemDef().getFormattedCode() + "  " + millRateEntry.getItemDef().getItemSubCategory().getName() + "  " + millRateEntry.getItemDef().getName());
            temMillRate.put("item", millRateDummy.getItemDef().getItemSubCategory().getName() + "  " + millRateDummy.getItemDef().getName());
            temMillRate.put("itemDefId", millRateDummy.getItemDef().getId());
            temMillRate.put("imageUrl", millRateDummy.getItemDef().getImageUrl());
            // temMillRate.put("itemDef", millRateDummy.getItemDef().getItemSubCategory().getName()+" "+millRateDummy.getItemDef().getName());
            temMillRate.put("millId", millRateDummy.getMill().getId());
            temMillRate.put("entryDate", millRateDummy.getEntryDate());
            temMillRate.put("id", millRateDummy.getId());
            temMillRate.put("saleRate", millRateDummy.getSaleRate());
            temMillRate.put("millRate", millRateDummy.getMillRate());
            temMillRate.put("cashRate", millRateDummy.getCashRate());
            temMillRate.put("moisture", millRateDummy.getMoisture());
            for (MillRateEntry millRateEntry : millRateDummy.getMillRateEntries()) {
                mapMillRateEntry = new HashMap<>();
                mapMillRateEntry.put("name", millRateEntry.getSupplierAccount() != null ? millRateEntry.getSupplierAccount().getAccountName() : "");
                mapMillRateEntry.put("qty", millRateEntry.getQty());
                mapMillRateEntry.put("date", millRateEntry.getEntryDate());
                mapMillRateEntry.put("rate", millRateEntry.getRate());
                mapMillRateEntry.put("id", millRateEntry.getId());
                millrateEntries.add(mapMillRateEntry);
            }
            temMillRate.put("millRateEntries", millrateEntries);
            // temMillRate.put("cashRate", millRateEntry.getCashRate());
            temMillRate.put("remarks", millRateDummy.getRemarks());
            millrates.add(temMillRate);
        }
        return millrates;
    }
}
