package com.mst.services;

import com.mst.constants.MSTConstants;
import com.mst.constants.MSTPermissions;
import com.mst.controllers.WhatsAppController;
import com.mst.email.MailClient;
import com.mst.jasperReport.PurchaseVoucher;
import com.mst.messages.*;
import com.mst.models.*;
import com.mst.projections.BranchProjection;
import com.mst.projections.ItemStockEntryProject;
import com.mst.projections.ItemStockEntryProjectForProfit;
import com.mst.repositories.*;
import com.mst.serviceInterface.*;
import com.mst.specs.VoucherSpecs;
import com.mst.utilities.MSTUtilities;
import com.mst.utilities.PersistenceUtils;
import lombok.extern.slf4j.Slf4j;
import org.hibernate.envers.AuditReader;
import org.hibernate.envers.AuditReaderFactory;
import org.hibernate.envers.query.AuditEntity;
import org.hibernate.proxy.HibernateProxy;
import org.hibernate.proxy.LazyInitializer;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import javax.persistence.EntityManager;
import javax.persistence.PersistenceContext;
import javax.transaction.Transactional;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.*;
import java.util.stream.Collectors;


//import com.twilio.Twilio;
//import com.twilio.rest.api.v2010.account.Message;
@Slf4j
@Service("voucherService")

//@Repository
//@Controller
public class VoucherService implements IVoucherService {
    @Autowired
    WhatsAppController whatsAppController;
    @Autowired
    ICompanyRepository companyRepository;
    @Autowired
    MovingAverageCalculator movingAverageCalculator;
    @Autowired
    FifoCalculator fifoCalculator;
    @Autowired
    private IItemDefRepository itemDefRepository;
    @Autowired
    private JdbcTemplate jdbcTemplate;
    @Autowired
    private IVoucherEntryService voucherEntryService;
    @Autowired
    private IAccountService accountService;
    @Autowired
    private IBranchService branchService;
    @Autowired
    private IFinancialYearService financialYearService;
    @Autowired
    private IItemCategoryService iItemCategoryService;

    @Autowired
    private ItemDefService itemDefService;
    @Autowired
    private IVoucherRepository voucherRepository;
    @Autowired
    private IVoucherEntryRepository voucherEntryRepository;
    @Autowired
    private IVoucherEntryDocumentRepository voucherEntryDocumentRepository;
    @Autowired
    private IInvoicePaymentRepository invoicePaymentRepository;
    @Autowired
    private IItemStockEntryRepository itemStockEntryRepository;
    @Autowired
    private EntityManager entityManager;

    @PersistenceContext
    private EntityManager entityManagerAvoidAudit;

    @Autowired
    private MailClient mailClient;

    @Override
    public List<Voucher> getAllVouchers() {
        return voucherRepository.findAll();
    }

    @Override
    public List<Voucher> getAllActiveVouchers() {
        return voucherRepository.findByActiveTrue();
    }

    @Override
    public long getNextVoucherNumber(Voucher voucher) {
        Voucher previousVoucher = voucherRepository
                .findTopByCompanyAndVoucherTypeAndVoucherStatusAndFinancialYearOrderByVoucherNumberDesc(
                        voucher.getCompany(), voucher.getVoucherType(), voucher.getVoucherStatus(),
                        voucher.getFinancialYear());

        long nextVoucherNumber = MSTConstants.VOUCHER_NUMBER_START;

        if (previousVoucher != null) {
            nextVoucherNumber = previousVoucher.getVoucherNumber() + 1;
        }

        long nextVoucherCode = nextVoucherNumber;

        return nextVoucherCode;
    }

    @Override
    public List<Voucher> getVouchersByIdsIn(List<Long> voucherList) {
        return voucherRepository.findByIdIn(voucherList);

    }

    @Override
    public List<Voucher> getVouchersByIdsInAndWhatsappSmsSendFalse(List<Long> voucherList) {
        return voucherRepository.findByIdInAndWhatsappSmsSendFalse(voucherList);
    }


    @Override
    public List<Voucher> getAllVouchersBySpecification(VoucherRequest searchCriteria) {
        Specification<Voucher> voucherSpecification = VoucherSpecs.findVouchersBasedOnFilters(searchCriteria);
        return voucherRepository.findAll(voucherSpecification);
    }

	/*@Override
	public List<VoucherDetail> getAllVouchersByFilters(VoucherRequest searchCriteria) {

		List<Voucher> vouchers = getAllVouchersBySpecification(searchCriteria);

		List<VoucherDetail> VoucherDetailList = new ArrayList<VoucherDetail>();
		VoucherDetail voucherDetail;

		if (vouchers != null) {
			for (Voucher voucher : vouchers) {

				voucherDetail = new VoucherDetail();

				voucherDetail.setId(voucher.getId());
				voucherDetail.setVoucherCode(voucher.getVoucherCode());
				voucherDetail.setVoucherDate(voucher.getVoucherDate());
				voucherDetail.setCreatedAt(voucher.getCreatedAt());
				voucherDetail.setUpdatedAt(voucher.getModifiedAt());
				voucherDetail.setUserName(voucher.getModifiedBy());
				voucherDetail.setPostedUnPosted(
						voucher.isPosted() ? MSTConstants.VOUCHER_POSTED : MSTConstants.VOUCHER_UN_POSTED);

				BigDecimal voucherAmount = voucher.getVoucherEntries().stream().map(VoucherEntry::getCredit)
						.reduce(MSTConstants.BIG_DECIMAL_ZERO, BigDecimal::add);
				voucherAmount = voucherAmount.setScale(MSTConstants.BIG_DECIMAL_ROUNDING,
						MSTConstants.BIG_DECIMAL_ROUND_HALF_UP);
				voucherDetail.setVoucherAmount(voucherAmount);

				VoucherDetailList.add(voucherDetail);
			}
		}
		return VoucherDetailList;
	}*/

    @Override
    public List<VoucherDetail> getAllVouchersByFilters(VoucherRequest searchCriteria) {

        List<Object[]> dataList;

        String postedUnPosted;
        if (searchCriteria.getPostedUnPosted().trim().equals(MSTConstants.VOUCHER_POSTED)) {
            postedUnPosted = "1";
        } else if (searchCriteria.getPostedUnPosted().trim().equals(MSTConstants.VOUCHER_UN_POSTED)) {
            postedUnPosted = "0";
        } else {
            postedUnPosted = "2";
        }

        if (searchCriteria.getSearchByDate() == MSTConstants.SEARCH_BY_CREATED_DATE) {
            dataList = voucherRepository.findVoucherListByCreatedDate(searchCriteria.getCompany().getId(), searchCriteria.getVoucherType().getId(), searchCriteria.getVoucherStatus().getId(), searchCriteria.getVoucherCode(), searchCriteria.getVoucherNarration(), postedUnPosted, MSTUtilities.zeroTime(searchCriteria.getFromDate()), MSTUtilities.endTime(searchCriteria.getToDate()));
        }
        if (searchCriteria.getSearchByDate() == MSTConstants.SEARCH_BY_UPDATED_DATE) {
            dataList = voucherRepository.findVoucherListByUpdatedDate(searchCriteria.getCompany().getId(), searchCriteria.getVoucherType().getId(), searchCriteria.getVoucherStatus().getId(), searchCriteria.getVoucherCode(), searchCriteria.getVoucherNarration(), postedUnPosted, MSTUtilities.zeroTime(searchCriteria.getFromDate()), MSTUtilities.endTime(searchCriteria.getToDate()));
        } else {
            dataList = voucherRepository.findVoucherListByVoucherDate(searchCriteria.getCompany().getId(), searchCriteria.getVoucherType().getId(), searchCriteria.getVoucherStatus().getId(), searchCriteria.getVoucherCode(), searchCriteria.getVoucherNarration(), postedUnPosted, MSTUtilities.zeroTime(searchCriteria.getFromDate()), MSTUtilities.endTime(searchCriteria.getToDate()));
        }

        List<VoucherDetail> VoucherDetailList = new ArrayList<VoucherDetail>();
        VoucherDetail voucherDetail;

        if (dataList != null) {
            for (Object[] data : dataList) {

                voucherDetail = new VoucherDetail();

                voucherDetail.setId(Long.valueOf(data[0].toString()));
                voucherDetail.setVoucherCode(data[1].toString());
                voucherDetail.setVoucherDate((java.sql.Date) data[2]);
                voucherDetail.setCreatedAt((Date) data[3]);
                voucherDetail.setUpdatedAt((Date) data[4]);
                voucherDetail.setUserName(data[5].toString());
                voucherDetail.setPostedUnPosted(Boolean.parseBoolean(data[6].toString()) ? MSTConstants.VOUCHER_POSTED : MSTConstants.VOUCHER_UN_POSTED);
                voucherDetail.setVoucherAmount(((BigDecimal) data[7]).setScale(MSTConstants.BIG_DECIMAL_ROUNDING, RoundingMode.HALF_UP));

                VoucherDetailList.add(voucherDetail);
            }
        }
        return VoucherDetailList;
    }

    public Voucher getVoucherById(long voucherId) {
        return voucherRepository.findById(voucherId);
    }

    public String togglePostVoucher(long voucherId) {
        Voucher voucher = voucherRepository.findById(voucherId);
        voucher.setPosted(!voucher.isPosted());
        voucherRepository.save(voucher);

        // send email
        voucherPostUpdate(voucher);

        return voucher.isPosted() ? MSTConstants.VOUCHER_POSTED : MSTConstants.VOUCHER_UN_POSTED;
    }

    @Override
    public String getNextVoucherCode(Voucher voucher) {

        Voucher previousVoucher = voucherRepository
                .findTopByCompanyAndVoucherTypeAndVoucherStatusAndFinancialYearOrderByVoucherNumberDesc(
                        voucher.getCompany(), voucher.getVoucherType(), voucher.getVoucherStatus(),
                        voucher.getFinancialYear());

        long nextVoucherNumber = MSTConstants.VOUCHER_NUMBER_START;

        if (previousVoucher != null) {
            nextVoucherNumber = previousVoucher.getVoucherNumber() + 1;
        }

        String nextVoucherCode = voucher.getVoucherType().getId() + "-" + nextVoucherNumber;

        return nextVoucherCode;
    }

    @Override
    public List<VoucherCodesPojo> getProductionVoucherCode(StockLedgerReportRequest stockLedgerReportRequest) {
        List<VoucherCodesPojo> productionvoucherCodeList = voucherRepository.findVoucherListByCreatedDate(stockLedgerReportRequest.getCompanyId(), "PRO", stockLedgerReportRequest.getFromDate(), stockLedgerReportRequest.getToDate());
        log.info("successfully get the productionCode list size {}:", productionvoucherCodeList.size());
        return productionvoucherCodeList;
    }

    @Override
    public List<Voucher> getAllVouchersLessThanIdByAccountCode(long voucherId, long AccountCode, boolean status) {
        return voucherRepository.findByIdLessThanEqualAndVoucherEntriesAccountCodeAndVoucherEntriesLockLine(voucherId, AccountCode, status);

    }

    @Override
    public Voucher addOrUpdateVoucher(Voucher voucher) {
        for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {
            // System.out.print(voucherEntry.getInvoiceNumber().replace(",", ""));
            try {
                voucherEntry.setCredit(new BigDecimal(voucherEntry.getInvoiceNumber().replace(",", "")));
            } catch (Exception e) {

            }
            try {

                voucherEntry.setDebit(new BigDecimal(voucherEntry.getRecBookPageNumber().replace(",", "")));
            } catch (Exception e) {

            }
        }
        Voucher existingVocuher = voucherRepository.findTop1ByVoucherDateAndVehicalNumberAndVoucherEntriesAccountCode(voucher.getVoucherDate(), "COSTING", 55111011004l);
        if (existingVocuher != null) {
            for (VoucherEntry voucherEntry : existingVocuher.getVoucherEntries()) {
                Optional<VoucherEntry> first = voucher.getVoucherEntries().stream().filter(a -> a.getAccount().getCode() == 55111011004l).findFirst();
                if (voucherEntry.getAccount().getCode() == 55111011004l) {
                    voucherEntry.setDebit(first.get().getDebit());
                } else {
                    voucherEntry.setCredit(first.get().getCredit());
                }
            }
            voucher = existingVocuher;
        }

        if ((voucher.getVoucherType().getId().equals(MSTConstants.BANK_PAYMENT_VOUCHER_CODE_PREFIX)
                || voucher.getVoucherType().getId().equals(MSTConstants.BANK_RECEIPT_VOUCHER_CODE_PREFIX)
                || voucher.getVoucherType().getId().equals(MSTConstants.CASH_PAYMENT_VOUCHER_CODE_PREFIX)
                || voucher.getVoucherType().getId().equals(MSTConstants.CASH_RECEIPT_VOUCHER_CODE_PREFIX)
                || voucher.getVoucherType().getId().equals(MSTConstants.ONLINE_JOURNAL_VOUCHER_CODE_PREFIX))
                && voucher.getId() < 1) {

            List<VoucherEntry> tempVoucherEntries = new ArrayList<VoucherEntry>();
            VoucherEntry firstVoucherEntry = voucher.getVoucherEntries().get(0);
            voucher.getVoucherEntries().remove(0); // removes the first item

            int index = 1;

            for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {

                VoucherEntry relativeVoucherEntry = new VoucherEntry();
                relativeVoucherEntry.setBranch(firstVoucherEntry.getBranch());
                relativeVoucherEntry.setAccount(firstVoucherEntry.getAccount());

                relativeVoucherEntry.setNarration(voucherEntry.getNarration());
                relativeVoucherEntry.setChequeNumber(voucherEntry.getChequeNumber());
                relativeVoucherEntry.setRecBookNumber(voucherEntry.getRecBookNumber());
                //relativeVoucherEntry.setRecBookPageNumber(voucherEntry.getRecBookPageNumber());

                if (voucher.getVoucherType().getId().equals(MSTConstants.BANK_RECEIPT_VOUCHER_CODE_PREFIX)
                        || voucher.getVoucherType().getId().equals(MSTConstants.CASH_RECEIPT_VOUCHER_CODE_PREFIX)
                        || voucher.getVoucherType().getId().equals(MSTConstants.ONLINE_JOURNAL_VOUCHER_CODE_PREFIX)) {
                    relativeVoucherEntry.setDebit(voucherEntry.getCredit());
                    relativeVoucherEntry.setCredit(MSTConstants.BIG_DECIMAL_ZERO);
                } else {
                    relativeVoucherEntry.setCredit(voucherEntry.getDebit());
                    relativeVoucherEntry.setDebit(MSTConstants.BIG_DECIMAL_ZERO);
                }

                relativeVoucherEntry.setSequence(index);
                voucherEntry.setSequence(index + 1);

                tempVoucherEntries.add(relativeVoucherEntry);
                tempVoucherEntries.add(voucherEntry);

                index = index + 2;

            }

            voucher.setVoucherEntries(tempVoucherEntries);
        }
		/*Voucher voucherExists = voucherRepository
				.findByCompanyAndVoucherTypeAndVoucherStatusAndFinancialYearAndVoucherCode(voucher.getCompany(),
						voucher.getVoucherType(), voucher.getVoucherStatus(), voucher.getFinancialYear(),
						voucher.getVoucherCode());*/
        Voucher voucherExists = voucherRepository.findById(voucher.getId());

        if (voucherExists != null) {

            voucher.setId(voucherExists.getId());
            voucher.setVoucherNumber(voucherExists.getVoucherNumber());
            voucher.setVoucherCode(voucherExists.getVoucherCode());

            List<VoucherEntry> voucherEntriesToRemove = new ArrayList<VoucherEntry>();

            for (VoucherEntry voucherEntryExists : voucherExists.getVoucherEntries()) {

                boolean entryMatch = false;

                for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {
                    if (voucherEntry.getId() == voucherEntryExists.getId()) {
                        entryMatch = true;

						/*// if account of voucher entry is different from previous delete all invoice
						// payments
						if (voucherEntry.getAccount().getCode() != voucherEntryExists.getAccount().getCode()) {
							invoicePaymentRepository.deleteByVoucherEntryId(voucherEntryExists.getId());
						}
						// if amount of voucher entry is different from previous delete all invoice
						// payments
						if (voucherEntry.getCredit().compareTo(voucherEntryExists.getCredit()) != 0
								|| voucherEntry.getDebit().compareTo(voucherEntryExists.getDebit()) != 0) {
							invoicePaymentRepository.deleteByVoucherEntryId(voucherEntryExists.getId());
						}*/
                        break;
                    }
                }

                if (!entryMatch) {
                    //voucherEntryExists.setActive(false);
                    voucherEntryDocumentRepository.deleteByVoucherEntryId(voucherEntryExists.getId());
                    invoicePaymentRepository.deleteByVoucherEntryId(voucherEntryExists.getId());
                    voucherEntriesToRemove.add(voucherEntryExists);
                    //voucherEntryRepository.deleteVoucherEntryById(voucherEntryExists.getId());
                }
            }

            for (VoucherEntry voucherEntryToRemove : voucherEntriesToRemove) {
                voucherEntryRepository.deleteVoucherEntryById(voucherEntryToRemove.getId());
            }

        } else {
            String nextVoucherCode = getNextVoucherCode(voucher);
            voucher.setVoucherCode(nextVoucherCode);
            voucher.setVoucherNumber(Long.parseLong(nextVoucherCode.split("-")[1]));
        }

        for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {

            voucherEntry.setVoucherDate(voucher.getVoucherDate());

            if (voucherEntry.getNarration() != null) {
                voucherEntry.setNarration(voucherEntry.getNarration().toUpperCase());
            }

            if (voucherEntry.getDebit() == null) {
                voucherEntry.setDebit(MSTConstants.BIG_DECIMAL_ZERO);
            }
            if (voucherEntry.getCredit() == null) {
                voucherEntry.setCredit(MSTConstants.BIG_DECIMAL_ZERO);
            }
            voucherEntry.setActive(true);
            voucherEntry.setAccount(accountService.getAccountByCode(voucherEntry.getAccount().getCode()));
            voucherEntry.setVoucher(voucher);
            if (voucherEntry.getThirdParty() != null) {
                voucherEntry.setThirdParty(accountService.getAccountByCode(voucherEntry.getThirdParty().getCode()));
            }
        }
        voucher.setActive(true);
        voucher.setPosted(false);

        voucher = voucherRepository.save(voucher);
        /*if (voucher.getCompany().isSendSms()) {
            for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {
                BigDecimal bigDecimal = voucherEntryService.getPreviousBalanceFeforeThisVoucher(voucherEntry.getAccount().getCode(), voucher.getId());
                try {

                    whatsAppController.sendSMS(voucherEntry, bigDecimal);
                } catch (Exception e) {

                }
            }
        }*/
        // send email
        //voucherPostUpdate(voucher);

        return voucher;
    }


    @Override
    public boolean isVoucherPostDateValid(int financialYearId, Date voucherDate) {

        boolean isVoucherPostDateValid = true;
        FinancialYear financialYear = financialYearService.getFinancialYear(financialYearId);
        for (FinancialYearMonth financialYearMonth : financialYear.getFinancialYearMonths()) {
            if (MSTUtilities.getMonthInt(financialYearMonth.getMonthYear()) == MSTUtilities.getMonthInt(voucherDate)
                    && !financialYearMonth.isActive()) {
                isVoucherPostDateValid = false;
            }
        }
        return isVoucherPostDateValid;
    }

    @Override
    public boolean isVoucherCreditDebitEqual(Voucher voucher) {

        for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {
            if (voucherEntry.getDebit() == null) {
                voucherEntry.setDebit(MSTConstants.BIG_DECIMAL_ZERO);
            }
            if (voucherEntry.getCredit() == null) {
                voucherEntry.setCredit(MSTConstants.BIG_DECIMAL_ZERO);
            }
        }

        BigDecimal voucherCredit = voucher.getVoucherEntries().stream().map(VoucherEntry::getCredit)
                .reduce(MSTConstants.BIG_DECIMAL_ZERO, BigDecimal::add);
        voucherCredit = voucherCredit.setScale(MSTConstants.BIG_DECIMAL_ROUNDING,
                RoundingMode.HALF_UP);

        BigDecimal voucherDebit = voucher.getVoucherEntries().stream().map(VoucherEntry::getDebit)
                .reduce(MSTConstants.BIG_DECIMAL_ZERO, BigDecimal::add);
        voucherDebit = voucherDebit.setScale(MSTConstants.BIG_DECIMAL_ROUNDING, RoundingMode.HALF_UP);

        return voucherCredit.compareTo(voucherDebit) == 0;
    }

    @Override
    public void voucherPostUpdate(Voucher voucher) {
        System.out.println("VOUCHER EMAIL FOR ID: " + voucher.getId());
        //Voucher managedVoucherEntity = entityManager.find(Voucher.class, voucher.getId());
        voucherRepository.refresh(voucher);
        mailClient.prepareAndSend(
                "VOUCHER # " + voucher.getVoucherCode() + " (VOUCHER ID: " + voucher.getId() + ") ["
                        + new SimpleDateFormat("dd-MM-yyyy hh:mm:ss a").format(Calendar.getInstance().getTime()) + "]",
                "voucher", voucher, new HashMap<String, Object>(), "email/email_voucher");
    }

    @Override
    public Voucher findRevision(long voucherId, int revId) {

        Voucher voucher = voucherRepository.findById(voucherId);

        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        //Voucher voucherRevision = auditReader.find(Voucher.class, voucher.getId(), revId);
        Voucher voucherRevision = auditReader.find(Voucher.class, Voucher.class.getName(), voucher.getId(), revId, false);
        List<VoucherEntry> voucherEntryRevisionList = auditReader.createQuery().forRevisionsOfEntity(VoucherEntry.class, VoucherEntry.class.getName(), true, false)
                .add(AuditEntity.revisionNumber().eq(revId))
                .getResultList();

        voucherRevision.setVoucherEntries(voucherEntryRevisionList);
		
		/*List<VoucherEntry> voucherEntriesToRemove = new ArrayList<VoucherEntry>();
		for (VoucherEntry voucherEntry : voucherRevision.getVoucherEntries()) {
			if (!voucherEntry.isActive()) {
				voucherEntriesToRemove.add(voucherEntry);
			}
		}		
		voucherRevision.getVoucherEntries().removeAll(voucherEntriesToRemove);*/

        for (VoucherEntry voucherEntry : voucherRevision.getVoucherEntries()) {
            //Hibernate.initialize(voucherEntry.getAccount());
            if (voucherEntry.getAccount() instanceof HibernateProxy) {
                LazyInitializer lazyInitializer = ((HibernateProxy) voucherEntry.getAccount()).getHibernateLazyInitializer();
                if (lazyInitializer.isUninitialized()) {
                    voucherEntry.setAccount(accountService.getAccountById(((Integer) lazyInitializer.getIdentifier())));
                }
            }
            if (voucherEntry.getBranch() instanceof HibernateProxy) {
                LazyInitializer lazyInitializer = ((HibernateProxy) voucherEntry.getBranch()).getHibernateLazyInitializer();
                if (lazyInitializer.isUninitialized()) {
                    voucherEntry.setBranch(branchService.getBranchById((Integer) lazyInitializer.getIdentifier()));
                }
            }
        }
//		if(voucherId==87 &&(revId==210))
//		{
//			voucherRepository.save(voucherRevision);
//		}
        PersistenceUtils.cleanFromProxies(voucherRevision);
        return voucherRevision;
    }

    @Override
    public List<PurchaseVoucher> prepareVoucherPrintWithCommission(List<Long> voucherList) {
        PurchaseVoucher purchaseVoucher = new PurchaseVoucher();
        List<PurchaseVoucher> pur = new ArrayList<>();

        List<Voucher> vouchers = getVouchersByIdsIn(voucherList);
        for (Voucher voucher : vouchers) {
            purchaseVoucher = new PurchaseVoucher();
            purchaseVoucher.setAccountName(voucher.getItemStock().getAccount().getAccountName());
            purchaseVoucher.setPartyAddress(voucher.getItemStock().getAccount().getAddress());
            purchaseVoucher.setAccountCode(voucher.getItemStock().getAccount().getCode());
            purchaseVoucher.setPartyMobile(voucher.getItemStock().getAccount().getMobile());
            purchaseVoucher.setCreatedDate(voucher.getCreatedAt());
            purchaseVoucher.setBags(voucher.getItemStock().getItemStockEntries().get(0).getBags());
            String subcateglory = voucher.getItemStock().getItemStockEntries().get(0).getItemDef().getItemSubCategory().getName();
            if (subcateglory == null)
                subcateglory = "";
            purchaseVoucher.setItemDefName(subcateglory + " " + voucher.getItemStock().getItemStockEntries().get(0).getItemDef().getName());
            purchaseVoucher.setPurchaseRate(voucher.getItemStock().getItemStockEntries().get(0).getPrice().subtract(voucher.getCompany().getCommission()));
            purchaseVoucher.setPrice(voucher.getItemStock().getItemStockEntries().get(0).getPrice());
            purchaseVoucher.setNetAmount(voucher.getItemStock().getNetAmount().setScale(0, RoundingMode.HALF_UP));
            purchaseVoucher.setVoucherDate(voucher.getVoucherDate());
            purchaseVoucher.setVoucherCode(voucher.getVoucherCode().split("-")[1]);
            purchaseVoucher.setCompleteVoucherCode(voucher.getVoucherCode());
            purchaseVoucher.setVehicalNumber(voucher.getVehicalNumber());
            purchaseVoucher.setTotalKg(voucher.getItemStock().getItemStockEntries().get(0).getTotalKg());
            purchaseVoucher.setTotalKatoti(voucher.getItemStock().getItemStockEntries().get(0).getTotalKatoti());
            purchaseVoucher.setTotalBagWeightDeduction(voucher.getItemStock().getItemStockEntries().get(0).getTotalBagWeightDeduction());
            purchaseVoucher.setSilaiRate(voucher.getItemStock().getSilaiRate());
            purchaseVoucher.setSilaiAmount(voucher.getItemStock().getSilaiAmount());
            purchaseVoucher.setSafiKg(voucher.getItemStock().getItemStockEntries().get(0).getSafiKg());
            purchaseVoucher.setPropMobile(voucher.getItemStock().getBranch().getProprietorPhone());
            purchaseVoucher.setPropName(voucher.getItemStock().getBranch().getProprietorName());
            purchaseVoucher.setNtn(voucher.getItemStock().getBranch().getNtn());
            purchaseVoucher.setBookNumber(voucher.getBookNumber());
            purchaseVoucher.setEmail(voucher.getItemStock().getBranch().getEmail());
            purchaseVoucher.setBrokriRate(voucher.getItemStock().getBrokriRate());
            purchaseVoucher.setBrokriAmount(voucher.getItemStock().getBrokriAmount());
            purchaseVoucher.setAmount(voucher.getItemStock().getItemStockEntries().get(0).getSafiKg().divide(voucher.getItemStock().getItemStockEntries().get(0).getItemDef().getWeightParBag(), 5, RoundingMode.HALF_UP).multiply(purchaseVoucher.getPurchaseRate()).setScale(0, RoundingMode.HALF_UP));
            purchaseVoucher.setInvoiceAmount(voucher.getItemStock().getItemStockEntries().get(0).getAmount());
            purchaseVoucher.setJuteBag(voucher.getItemStock().getItemStockEntries().get(0).getJuteBag());
            purchaseVoucher.setJuteBagRate(voucher.getItemStock().getItemStockEntries().get(0).getJuteBagRate());
            purchaseVoucher.setPlasticBag(voucher.getItemStock().getItemStockEntries().get(0).getPlasticBag());
            purchaseVoucher.setPlasticBagRate(voucher.getItemStock().getItemStockEntries().get(0).getPlasticBagRate());
            if (voucher.getItemStock().getThirdParty() != null) {
                purchaseVoucher.setPermintNumber(voucher.getItemStock().getThirdParty().getAccountName() + " " + voucher.getItemStock().getPermintNumber());
            } else {
                purchaseVoucher.setPermintNumber(voucher.getItemStock().getPermintNumber());
            }
            try {

                Optional<VoucherEntry> first = voucher.getVoucherEntries().stream().filter(s -> s.getSequence() == 10).findFirst();
                if (first.isPresent()) {
                    boolean aDefault = first.get().getAccount().getAccountName().contains("DEFULT");
                    if (!aDefault) {
                        purchaseVoucher.setBroker(first.get().getAccount().getAccountName());
                    }
                }

            } catch (Exception e) {
            }
            Authentication auth = SecurityContextHolder.getContext().getAuthentication();
            if (auth.getAuthorities().stream().anyMatch(r -> r.getAuthority().equals(MSTPermissions.PRINT_NARRATION_ON_PDF))) {
                purchaseVoucher.setNarration("Description:" + voucher.getVoucherNarration());
            }
            try {
                String[] split = voucher.getItemStock().getItemStockEntries().get(0).getRem().split(",");
                purchaseVoucher.setJuteBagDescription(split[0]);
                purchaseVoucher.setPlasticBagDescription("");
                purchaseVoucher.setMounds(voucher.getItemStock().getItemStockEntries().get(0).getManWithKg());
                if (split.length > 1) {
                    purchaseVoucher.setPlasticBagDescription(split[1]);
                }
            } catch (Exception e) {

            }
            purchaseVoucher.setJuteBagAmount(purchaseVoucher.getJuteBag().multiply(purchaseVoucher.getJuteBagRate()));
            purchaseVoucher.setPlasticBagAmount(purchaseVoucher.getPlasticBag().multiply(purchaseVoucher.getPlasticBagRate()));
            purchaseVoucher.setPrintedBy(MSTUtilities.getCurrentUserName());
            VoucherEntry voucherEntry = voucher.getVoucherEntries().stream().filter(a -> a.getSequence() == 10).findFirst().get();
            if (voucherEntry.getAccount().getCode() == voucher.getItemStock().getAccount().getCode()) {
                purchaseVoucher.setBrokriRate(voucher.getItemStock().getBrokriRate().setScale(3, RoundingMode.HALF_UP));
                purchaseVoucher.setBrokriAmount(voucher.getItemStock().getBrokriAmount());
            } else {
                purchaseVoucher.setBrokriRate(MSTConstants.BIG_DECIMAL_ZERO);
                purchaseVoucher.setBrokriAmount(MSTConstants.BIG_DECIMAL_ZERO);
            }
            // System.out.println(voucher.getVoucherCode().indexOf("PTV") + "  PTV");
            if (voucher.getVoucherCode().indexOf("PTV") > -1 || voucher.getVoucherCode().indexOf("PJV") > -1) {
                purchaseVoucher.setReportTitle("Purchase Invoice");
            } else {
                purchaseVoucher.setReportTitle("Sale Invoice");
            }
            purchaseVoucher.setOtherExp(voucher.getItemStock().getOtherExp());
            purchaseVoucher.setMillTaxRate(voucher.getItemStock().getMillTaxRate());
            purchaseVoucher.setMillTaxAmount(voucher.getItemStock().getMillTaxAmount().setScale(0, RoundingMode.HALF_UP));
            purchaseVoucher.setFreightCharges(voucher.getItemStock().getFreightCharges());
            purchaseVoucher.setFormattedCode(voucher.getItemStock().getAccount().getFormattedCode());
            purchaseVoucher.setCompanyName(voucher.getCompany().getName());
            PrePurchaseEntry prePurchaseEntry = voucher.getItemStock().getItemStockEntries().get(0).getPrePurchaseEntry();
            String millKhataName = "";
            //  String accountName = "";

            if (prePurchaseEntry != null) {
                MillKhata millKhata = prePurchaseEntry.getMillKhata();
                if (millKhata != null) {
                    Account account = millKhata.getAccountCode();
                    if (account != null) {
                        String[] splitDummy = millKhata.getAccountName().split(" ");
                        millKhataName = splitDummy[0];
                        if (splitDummy.length >= 2) {
                            millKhataName = splitDummy[0] + " " + splitDummy[1];
                        }
                        MSTConstants.FILE_NAME = millKhataName + "-" + account.getAccountName().split(" ")[0];
                    }
                }
            }
            BigDecimal commission = voucher.getItemStock().getItemStockEntries().get(0).getSafiKg().divide(voucher.getItemStock().getItemStockEntries().get(0).getItemDef().getWeightParBag(), 2, RoundingMode.HALF_UP).multiply(voucher.getCompany().getCommission());
            purchaseVoucher.setCommission(commission.setScale(0, RoundingMode.HALF_UP));
            purchaseVoucher.setBranchName(voucher.getItemStock().getBranch().getName());
            purchaseVoucher.setBranchAddress(voucher.getItemStock().getBranch().getAddress());
            purchaseVoucher.setBankTaxAmount(voucher.getItemStock().getBankTaxAmount());
            purchaseVoucher.setBankTaxRate(voucher.getItemStock().getBankTaxRate());
            purchaseVoucher.setBardanaRate(voucher.getItemStock().getBardanaRate());
            purchaseVoucher.setBardanaAmount(voucher.getItemStock().getBardanaAmount());
            purchaseVoucher.setUnloadingCharges(voucher.getItemStock().getUnloadingCharges());
            purchaseVoucher.setUnloadingRate(voucher.getItemStock().getUnloadingRate());
            pur.add(purchaseVoucher);
        }
        return pur;
    }

    @Override
    public List<PurchaseVoucher> prepareVoucherPrint(List<Long> voucherList) {
        PurchaseVoucher purchaseVoucher = new PurchaseVoucher();
        List<PurchaseVoucher> pur = new ArrayList<>();
        List<Voucher> vouchers = getVouchersByIdsIn(voucherList);
        for (Voucher voucher : vouchers) {
            purchaseVoucher = new PurchaseVoucher();
            purchaseVoucher.setAccountName(voucher.getItemStock().getAccount().getAccountName());
            purchaseVoucher.setAmount(voucher.getItemStock().getItemStockEntries().get(0).getAmount());
            purchaseVoucher.setBags(voucher.getItemStock().getItemStockEntries().get(0).getBags());
            String subcateglory = voucher.getItemStock().getItemStockEntries().get(0).getItemDef().getItemSubCategory().getName();
            if (subcateglory == null)
                subcateglory = "";
            purchaseVoucher.setItemDefName(subcateglory + " " + voucher.getItemStock().getItemStockEntries().get(0).getItemDef().getName());
            purchaseVoucher.setPurchaseRate(voucher.getItemStock().getItemStockEntries().get(0).getPrice());
            purchaseVoucher.setNetAmount(voucher.getItemStock().getNetAmount());
            purchaseVoucher.setVoucherDate(voucher.getVoucherDate());
            purchaseVoucher.setVoucherCode(voucher.getVoucherCode());
            purchaseVoucher.setVehicalNumber(voucher.getVehicalNumber());
            purchaseVoucher.setTotalKg(voucher.getItemStock().getItemStockEntries().get(0).getTotalKg());
            purchaseVoucher.setTotalKatoti(voucher.getItemStock().getItemStockEntries().get(0).getTotalKatoti());
            purchaseVoucher.setTotalBagWeightDeduction(voucher.getItemStock().getItemStockEntries().get(0).getTotalBagWeightDeduction());
            purchaseVoucher.setSilaiRate(voucher.getItemStock().getSilaiRate());
            purchaseVoucher.setSilaiAmount(voucher.getItemStock().getSilaiAmount());
            purchaseVoucher.setSafiKg(voucher.getItemStock().getItemStockEntries().get(0).getSafiKg());
            purchaseVoucher.setPropMobile(voucher.getItemStock().getBranch().getProprietorPhone());
            purchaseVoucher.setPropName(voucher.getItemStock().getBranch().getProprietorName());
            purchaseVoucher.setNtn(voucher.getItemStock().getBranch().getNtn());
            purchaseVoucher.setBookNumber(voucher.getBookNumber());
            BigDecimal commission = voucher.getItemStock().getItemStockEntries().get(0).getSafiKg().divide(new BigDecimal(40)).multiply(new BigDecimal(5));
            purchaseVoucher.setCommission(commission.setScale(0, RoundingMode.HALF_UP));
            // System.out.println(voucher.getVoucherCode().indexOf("PTV") + "  PTV");
            if (voucher.getVoucherCode().indexOf("PTV") > -1 || voucher.getVoucherCode().indexOf("PJV") > -1) {
                purchaseVoucher.setReportTitle("Purchase Invoice");
            } else {
                purchaseVoucher.setReportTitle("Sale Invoice");
            }
            purchaseVoucher.setOtherExp(voucher.getItemStock().getOtherExp());
            purchaseVoucher.setMillTaxRate(voucher.getItemStock().getMillTaxRate());
            purchaseVoucher.setMillTaxAmount(voucher.getItemStock().getMillTaxAmount());
            purchaseVoucher.setFreightCharges(voucher.getItemStock().getFreightCharges());
            purchaseVoucher.setFormattedCode(voucher.getItemStock().getAccount().getFormattedCode());
            purchaseVoucher.setCompanyName(voucher.getCompany().getName());
            purchaseVoucher.setBranchName(voucher.getItemStock().getBranch().getName());
            purchaseVoucher.setBranchAddress(voucher.getItemStock().getBranch().getAddress());
            purchaseVoucher.setBankTaxAmount(voucher.getItemStock().getBankTaxAmount());
            purchaseVoucher.setBankTaxRate(voucher.getItemStock().getBankTaxRate());
            purchaseVoucher.setBardanaRate(voucher.getItemStock().getBardanaRate());
            purchaseVoucher.setBardanaAmount(voucher.getItemStock().getBardanaAmount());
            pur.add(purchaseVoucher);
        }

        return pur;
    }

    @Override
    public List<Number> findRevisions(long voucherId) {
        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        List<Number> revNumbers = auditReader.getRevisions(Voucher.class, voucherId);
        return revNumbers;
    }

    @Override
    public Map<Number, Voucher> findRevisionsOfVoucherList(Set<Number> voucherList) {
        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        auditReader.createQuery()
                .forRevisionsOfEntity(Voucher.class, false, true);

        Map<Number, Voucher> revisions = auditReader.findRevisions(Voucher.class, voucherList);
        System.out.println(revisions.keySet());
        List<VoucherEntry> voucherEntryRevisionList = auditReader.createQuery().forRevisionsOfEntity(Voucher.class, VoucherEntry.class.getName(), true, false)
                .add(AuditEntity.revisionNumber().in(revisions.keySet()))
                .getResultList();

        return revisions;
    }

    @Override
    public List findRevisionsOfVoucherEntity() {
        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        List resultList = auditReader.createQuery()
                .forRevisionsOfEntity(Voucher.class, false, false)
                .getResultList();

        return resultList;
    }

    @Override
    public List findRevisionsOfVoucherEntryEntity() {
        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        List resultList = auditReader.createQuery()
                .forRevisionsOfEntity(VoucherEntry.class, false, false)
                .getResultList();

        return resultList;
    }

    @Override
    public List findRevisionsOfItemStockEntity() {
        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        List resultList = auditReader.createQuery()
                .forRevisionsOfEntity(ItemStock.class, false, false)
                .getResultList();

        return resultList;
    }

    @Override
    public List findRevisionsOfItemStockEntryEntity() {
        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        List resultList = auditReader.createQuery()
                .forRevisionsOfEntity(ItemStockEntry.class, false, false)
                .getResultList();

        return resultList;
    }

    @Override
    public boolean isVoucherPosted(long voucherId) {
        return voucherRepository.findVoucherPostStatusById(voucherId);
    }


    @Override
    public boolean isVoucherCompanyBranchMatches(Voucher voucher) {

        boolean isVoucherCompanyBranchMatches = true;

        List<BranchProjection> branches = branchService.getAllByCompanyId(voucher.getCompany().getId());
        for (VoucherEntry voucherEntry : voucher.getVoucherEntries()) {
            if (!branches.stream().anyMatch(t -> t.getId() == voucherEntry.getBranch().getId())) {
                isVoucherCompanyBranchMatches = false;
                break;
            }
        }
        return isVoucherCompanyBranchMatches;

    }

    @Override
    public void savedVoucher(Voucher voucher) {
        voucherRepository.save(voucher);
    }

    @Override
    public Voucher findRevisionOfSjvOrPjv(long voucherId, int revId) {
        Voucher voucher = voucherRepository.findById(voucherId);

        AuditReader auditReader = AuditReaderFactory.get(entityManager);
        //Voucher voucherRevision = auditReader.find(Voucher.class, voucher.getId(), revId);
        Voucher voucherRevision = auditReader.find(Voucher.class, Voucher.class.getName(), voucher.getId(), revId, false);

        List<VoucherEntry> voucherEntryRevisionList = auditReader.createQuery().forRevisionsOfEntity(VoucherEntry.class, VoucherEntry.class.getName(), true, false)
                .add(AuditEntity.revisionNumber().eq(revId))
                .getResultList();

        List<ItemStockEntry> StockEntryRevisionList = auditReader.createQuery().forRevisionsOfEntity(ItemStockEntry.class, ItemStockEntry.class.getName(), true, false)
                .add(AuditEntity.revisionNumber().eq(revId))
                .getResultList();

        for (ItemStockEntry itemStockEntry : StockEntryRevisionList) {
            if (itemStockEntry.getItemCategory() instanceof HibernateProxy) {
                LazyInitializer lazyInitializer = ((HibernateProxy) itemStockEntry.getItemCategory()).getHibernateLazyInitializer();
                if (lazyInitializer.isUninitialized()) {
                    itemStockEntry.setItemCategory(iItemCategoryService.getItemCategoryById(((long) lazyInitializer.getIdentifier())));

                }
                LazyInitializer lazyInitializerItemDefination = ((HibernateProxy) itemStockEntry.getItemDef()).getHibernateLazyInitializer();

                if (lazyInitializerItemDefination.isUninitialized()) {
                    itemStockEntry.setItemDef(itemDefService.getItemDefById(((long) lazyInitializerItemDefination.getIdentifier())));

                }
            }
        }   // END OF FOR LOOP

        voucherRevision.getItemStock().setItemStockEntries(StockEntryRevisionList);
        voucherRevision.setVoucherEntries(voucherEntryRevisionList);
		
		
		/*List<VoucherEntry> voucherEntriesToRemove = new ArrayList<VoucherEntry>();
		for (VoucherEntry voucherEntry : voucherRevision.getVoucherEntries()) {
			if (!voucherEntry.isActive()) {
				voucherEntriesToRemove.add(voucherEntry);
			}
		}		
		voucherRevision.getVoucherEntries().removeAll(voucherEntriesToRemove);*/

        for (VoucherEntry voucherEntry : voucherRevision.getVoucherEntries()) {
            //Hibernate.initialize(voucherEntry.getAccount());
            if (voucherEntry.getAccount() instanceof HibernateProxy) {
                LazyInitializer lazyInitializer = ((HibernateProxy) voucherEntry.getAccount()).getHibernateLazyInitializer();
                if (lazyInitializer.isUninitialized()) {
                    voucherEntry.setAccount(accountService.getAccountById(((Integer) lazyInitializer.getIdentifier())));
                }
            }
            if (voucherEntry.getBranch() instanceof HibernateProxy) {
                LazyInitializer lazyInitializer = ((HibernateProxy) voucherEntry.getBranch()).getHibernateLazyInitializer();
                if (lazyInitializer.isUninitialized()) {
                    voucherEntry.setBranch(branchService.getBranchById((Integer) lazyInitializer.getIdentifier()));
                }
            }
        }

        PersistenceUtils.cleanFromProxies(voucherRevision);
        return voucherRevision;
    }

    @Override
    public List<Voucher> getAllPaymentVouchersBySpecification(VoucherRequest searchCriteria) {
        searchCriteria.setFromDate(MSTUtilities.zeroTime(searchCriteria.getFromDate()));
        searchCriteria.setToDate(MSTUtilities.endTime(searchCriteria.getToDate()));
        Specification<Voucher> voucherSpecification = VoucherSpecs.findPaymentVouchersBasedOnFilters(searchCriteria);
        //voucherRepository.findAll(voucherSpecification).sort(Comparator.comparing(m-> m.,Comparator.nullsLast(Comparator.naturalOrder())));
        List<Voucher> listvoucher = voucherRepository.findAll(voucherSpecification);

        listvoucher.sort(Comparator.comparingLong(Voucher::getId));
        return listvoucher;
    }

    @Override
    public ResponseEntity<?> getSaleEntyForProfitLastPurchase(long itemCategoryId, long itemDefId, long accountCode,
                                                              int companyId, int branchId, int financialYearId, Date fromDate, Date toDate) {

        List<Map<String, Object>> itemStopEntryRespo = new ArrayList<>();
        Map<String, Object> response = new HashMap<>();
        Map<String, Object> saleEntry = new HashMap<>();
        DecimalFormat df = new DecimalFormat("#.00");
        BigDecimal totalMovRateProfit = new BigDecimal(0);
        // TODO Auto-generated method stub
        List<ItemStockEntryProject> itemStockEntryProjects = voucherRepository.getSaleEntyForProfit(itemDefId, itemCategoryId, accountCode, companyId, branchId, financialYearId, fromDate, toDate);
        for (ItemStockEntryProject itemStockEntryProject : itemStockEntryProjects) {
            Double costingRate = itemStockEntryProject.getOpeningRate().doubleValue();//calculateAverageRateItem(itemStockEntryProject.getItemDefId(), itemStockEntryProject.getVoucherDate());
            saleEntry = new HashMap<>();

            saleEntry.put("id", itemStockEntryProject.getId());
            saleEntry.put("formatedCode", itemStockEntryProject.getFormatedCode());
            saleEntry.put("accountName", itemStockEntryProject.getAccountName());
            saleEntry.put("voucherCode", itemStockEntryProject.getVoucherCode());
            saleEntry.put("openingQty", itemStockEntryProject.getOpeningQty());
            saleEntry.put("itemName", itemStockEntryProject.getItemName());
            saleEntry.put("openingRate", df.format(costingRate));
            //  saleEntry.put("openingRate", calculateAverageRateItem(itemStockEntryProject.getItemDefId(), itemStockEntryProject.getVoucherDate()));
            saleEntry.put("fRate", "0");
            saleEntry.put("fProfit", "0");
            if (itemStockEntryProject.getFifoRate() != null) {
                saleEntry.put("fRate", df.format(itemStockEntryProject.getFifoRate()));
            }
            if (itemStockEntryProject.getFifoProfit() != null) {
                saleEntry.put("fProfit", df.format(itemStockEntryProject.getFifoProfit()));
            }
            if (itemStockEntryProject.getVoucherCode().equalsIgnoreCase("SJV-5")) {
                System.out.println("SJV-5");
            }
            if (costingRate.doubleValue() == 0) {
                saleEntry.put("openingRate", saleEntry.get("fRate").toString());
            }
            saleEntry.put("itemQuantity", itemStockEntryProject.getItemQuantity());
            saleEntry.put("saleRate", itemStockEntryProject.getSaleRate());
            saleEntry.put("voucherDate", itemStockEntryProject.getVoucherDate());
            BigDecimal saleAmount = itemStockEntryProject.getSaleRate().multiply(itemStockEntryProject.getItemQuantity());
            try {
                BigDecimal purchaseAmount = new BigDecimal(costingRate).multiply(itemStockEntryProject.getItemQuantity());
                // BigDecimal fPurchaseAmount = itemStockEntryProject.getProfit().multiply(itemStockEntryProject.getItemQuantity());
                saleEntry.put("profit", saleAmount.subtract(purchaseAmount));
                totalMovRateProfit = totalMovRateProfit.add(saleAmount.subtract(purchaseAmount));
                saleEntry.put("createdBy", itemStockEntryProject.getCreatedBy());
                itemStopEntryRespo.add(saleEntry);
            } catch (Exception e) {
                System.out.println("Error Exception:" + costingRate);
            }
        }

        response.put("entries", itemStopEntryRespo);
        Company company = null;
        if (companyId != 0) {
            company = companyRepository.getOne(companyId);
        } else {
            company = companyRepository.getOne(1);
        }
        String[] split = null;
        List<Long> totalExp = new ArrayList<>();
        List<Long> frightAccountExpList = new ArrayList<>();
        BigDecimal totalExpance = MSTConstants.BIG_DECIMAL_ZERO;
        BigDecimal frightExpance = MSTConstants.BIG_DECIMAL_ZERO;
        if (!company.isExpHeadUsed()) {
            try {
                split = company.getExpHead().split(",");
                for (int i = 0; i < split.length; i++) {
                    totalExp.add(Long.parseLong(split[i].trim().toString()));

                }
                totalExpance = voucherEntryRepository.getFinancialThirdLevelBalance(companyId, branchId, "0", totalExp, fromDate, toDate);

            } catch (Exception e) {

            }
        }
        if (totalExpance == null) {
            totalExpance = MSTConstants.BIG_DECIMAL_ZERO;
        }
        ItemDef itemDefById = itemDefService.getItemDefById(itemDefId);
        if (itemDefId != 0 && itemDefById.getItemExpAccounts().length() > 7 && !company.isExpHeadUsed()) {
            totalExp = new ArrayList<>();
            split = itemDefById.getItemExpAccounts().split(",");
            for (int i = 0; i < split.length; i++) {
                totalExp.add(Long.parseLong(split[i].trim().toString()));

            }

            totalExpance = voucherEntryRepository.getFinancialFourthLevelBalance(companyId, branchId, totalExp, fromDate, toDate);
            //    totalExp.add(Long.parseLong());
            frightAccountExpList = new ArrayList<>();
            split = itemDefById.getSameExpAccounts().split(",");
            for (int i = 0; i < split.length; i++) {
                frightAccountExpList.add(Long.parseLong(split[i].trim().toString()));

            }
            frightExpance = voucherEntryRepository.getFirghtBalanceOfSjv("OUT", companyId, branchId, frightAccountExpList, fromDate, toDate, itemDefId);

        } else if (itemDefId == 0 && !company.isExpHeadUsed()) {
            totalExp = new ArrayList<>();

            for (ItemDef allActiveItemDef : itemDefService.getAllActiveItemDefs()) {
                if (allActiveItemDef.getItemExpAccounts() != null && allActiveItemDef.getItemExpAccounts().length() > 7) {
                    split = allActiveItemDef.getItemExpAccounts().split(",");
                    for (int i = 0; i < split.length; i++) {
                        totalExp.add(Long.parseLong(split[i].trim().toString()));

                    }
                }
                if (allActiveItemDef.getSameExpAccounts() != null && allActiveItemDef.getSameExpAccounts().length() > 7) {
                    frightAccountExpList = new ArrayList<>();
                    split = allActiveItemDef.getSameExpAccounts().split(",");
                    for (int i = 0; i < split.length; i++) {
                        frightAccountExpList.add(Long.parseLong(split[i].trim().toString()));

                    }
                }
            }
            if (totalExp == null || totalExp.isEmpty()) {
                totalExp = Arrays.asList(-1L); // a value that will never match
            }
            totalExpance = voucherEntryRepository.getFinancialFourthLevelBalance(companyId, branchId, totalExp, fromDate, toDate);

            if (frightAccountExpList.size() != 0) {
                frightExpance = voucherEntryRepository.getFirghtBalanceOfSjv("OUT", companyId, branchId, frightAccountExpList, fromDate, toDate, itemDefId);
            }
        }
        if (frightExpance == null) {
            frightExpance = MSTConstants.BIG_DECIMAL_ZERO;
        }
        if (totalExpance == null) {
            totalExpance = MSTConstants.BIG_DECIMAL_ZERO;
        }
        DecimalFormat format = new DecimalFormat("#,###");
        response.put("expenses", format.format(totalExpance.setScale(0, RoundingMode.HALF_UP)));
        response.put("others", format.format(frightExpance.setScale(0, RoundingMode.HALF_UP)));
        response.put("grossProfit", format.format(totalMovRateProfit.setScale(0, RoundingMode.HALF_UP)));
        response.put("netProfit", format.format((totalMovRateProfit.subtract(totalExpance).subtract(frightExpance)).setScale(0, RoundingMode.HALF_UP)));


        return new ResponseEntity<>(response, HttpStatus.OK);
    }

    @Override

    public List<ItemStockEntryProjectForProfit> getSaleEntyForProfit(long itemDefId,
                                                                     long itemCategoryId,
                                                                     long accountCode,
                                                                     int companyId,
                                                                     int branchId,
                                                                     int financialYearId,
                                                                     Date fromDate, Date toDate) {
        return voucherRepository.getSaleEntyForProfitNew(itemDefId, itemCategoryId,
                accountCode, companyId,
                branchId, financialYearId, fromDate, toDate);
    }

    @Override
    public List<ItemStockEntryProject> getSaleEntyForProfitOld(long itemDefId, long itemCategoryId, long accountCode,
                                                               int companyId, int branchId, int financialYearId, Date fromDate, Date toDate) {

        List<Map<String, Object>> itemStopEntryRespo = new ArrayList<>();
        Map<String, Object> saleEntry = new HashMap<>();
        // TODO Auto-generated method stub
        List<ItemStockEntryProject> itemStockEntryProjects = voucherRepository.getSaleEntyForProfit(itemDefId, itemCategoryId, accountCode, companyId, branchId, financialYearId, fromDate, toDate);

        return itemStockEntryProjects;
    }

    public String VoucherDeleteById(Long id) {
        voucherRepository.deleteById(id);
        return "deleted";
    }

    public Double calculateAverageRateItem(long itemDefId, Date voucherDate) {

        List<Object[]> myRequest = itemStockEntryRepository.getItemStockEntryRates(itemDefId, voucherDate);
        //List<AverageRateRequest> myRequest = itemStockEntryRepsotory.getItemStockEntryRates();
        BigDecimal myCostingRate = BigDecimal.valueOf(0);

        //Op. Bal	IN	OUT	Cl. Bal	Rate
        Double openingBalance = Double.valueOf(0);
        Double qtyIn = Double.valueOf(0);
        Double qtyOut = Double.valueOf(0);
        Double closingBalance = Double.valueOf(0);
        Double lastBalance = Double.valueOf(0);
        Double itemBalance = Double.valueOf(0);
        Double itemAmount = Double.valueOf(0);
        Double currentRate = Double.valueOf(0);
        Double lastRate = Double.valueOf(0);
        Integer firstEntry = 0;

        BigDecimal itemQuantity = MSTConstants.BIG_DECIMAL_ZERO;
        BigDecimal itemAmounted = BigDecimal.valueOf(0);

        BigDecimal itemIn = MSTConstants.BIG_DECIMAL_ZERO;
        BigDecimal itemOut = MSTConstants.BIG_DECIMAL_ZERO;

        if (myRequest != null) {
            for (Object[] eRequest : myRequest) {
                // System.out.println("Date: " + eRequest[0] + " Type: " + eRequest[1] + " Qty: " + eRequest[2] + " Amount " + eRequest[3]);

                if (firstEntry == 0) {
                    openingBalance = Double.valueOf(0);
                } else {
                    openingBalance = Double.parseDouble(eRequest[8].toString());
                    //Long.parseLong((String) eRequest[7]);
                }

                itemQuantity = (BigDecimal) eRequest[3];
                itemAmounted = (BigDecimal) eRequest[4];

                itemIn = (BigDecimal) eRequest[6];
                itemOut = (BigDecimal) eRequest[7];

                itemBalance = itemQuantity.doubleValue();
                itemAmount = itemAmounted.doubleValue();

                qtyIn = itemIn.doubleValue();
                qtyOut = itemOut.doubleValue();
                System.out.println(eRequest[9].toString());
                closingBalance = Double.parseDouble(eRequest[9].toString());

                if (lastBalance < 0) {
                    currentRate = itemAmount / itemBalance;
                    //System.out.println("ID : " + eRequest[0] + " Rate 1 Calculation : " + currentRate + " Last Balance " + lastBalance + " AMOUNT : " + itemAmount + " QTY " + itemBalance);
                } else {
                    if (eRequest[2].toString().equalsIgnoreCase("IN")) {
                        if (closingBalance == 0) {
                            currentRate = ((lastBalance * lastRate) + itemAmount);
                        } else {
                            currentRate = ((lastBalance * lastRate) + itemAmount) / closingBalance;
                        }
                        //System.out.println("ID : " + eRequest[0] +" Rate 2 Calculation : " + currentRate + " Last Balance : " + lastBalance + " Last Rate : " + lastRate + " Amount : " + itemAmount + " Closing Bal " + closingBalance);
                    } else {
                        if (closingBalance == 0) {
                            currentRate = lastRate;
                        } else {
                            if (closingBalance == 0) {
                                currentRate = ((lastBalance * lastRate) - (lastRate * qtyOut));
                            } else {
                                currentRate = ((lastBalance * lastRate) - (lastRate * qtyOut)) / closingBalance;
                            }
                        }

                        //System.out.println("ID : " + eRequest[0] + " Rate 3 Calculation : " + currentRate + " ULast " + lastBalance + " VLast " + lastRate + " IQTY " + itemBalance + " CBal " + closingBalance);
                    }
                }

                lastBalance = closingBalance;

                if (currentRate > 0)
                    lastRate = currentRate;

                firstEntry++;
                // System.out.println("Opening Balance : " + openingBalance + " Closing Balance : " + closingBalance + " Last Balance : " + lastBalance + " Current Rate : " + currentRate + " Last Rate : " + lastRate);
            }
        }
        return currentRate;
    }

    public void updateAvgRateInItemStockEntry() {
        List<ItemDef> allItemDefs = itemDefService.getAllItemDefs();


        for (ItemDef itemDef : allItemDefs) {
            if (itemDef.getId() != 6) {
                //System.out.println("ASIF");
                continue;

            }
            List<ItemStockEntry> allByItemDefIdOrderByIdAsc = itemStockEntryRepository.findAllByItemDefIdOrderByIdAsc(itemDef.getId());
            movingAverageCalculator.calculateMovingAverage(allByItemDefIdOrderByIdAsc);
            fifoCalculator.calculateFifo(allByItemDefIdOrderByIdAsc);
            try {
                // itemStockEntryRepository.save(itemStockEntries);
                //  itemStockEntryRepository.updateItemStockEntryRates(itemDef.getId());
                itemStockEntryRepository.updateItemDefOpeningRate(itemDef.getId());
            } catch (Exception e) {
                itemStockEntryRepository.updateItemDefOpeningRate(itemDef.getId());
                System.out.println(e.getMessage() + "ERROR AVG: " + itemDef.getId());
                //  itemStockEntryRepository.updateItemStockEntryRates(46l);
            }

        }

        //List<AverageRateRequest> myRequest = itemStockEntryRepsotory.getItemStockEntryRates();

    }

    @Transactional
    public void saveWithoutAudit(List<ItemStockEntry> entries) {
        String sql = "UPDATE item_stock_entry SET fifo_rate=?, opening_rate=?, opening_qty=? WHERE id=?";
        entityManager.clear(); // clears ALL managed entities
        for (ItemStockEntry e : entries) {
            // entityManager.detach(e);
            itemStockEntryRepository.updateFifoWithoutAudit(e.getId(), e.getFifoRate(), e.getOpeningRate(), e.getOpeningQty());
        }
    }

    @Transactional
    public void calculateFifoItemStockEntry() {
        List<ItemDef> allItemDefs = itemDefService.getAllItemDefs();


        for (ItemDef itemDef : allItemDefs) {
           /* if (itemDef.getId() != 34) {
                System.out.println("ASIF");
                continue;

            }*/

            List<ItemStockEntry> out = itemStockEntryRepository.findAllByTransactionTypeEqualsAndItemDefIdOrderByIdAsc("OUT", itemDef.getId());
            List<ItemStockEntry> in = itemStockEntryRepository.findAllByTransactionTypeEqualsAndItemDefIdOrderByIdAsc("IN", itemDef.getId());
            Map<Long, AvailableStock> longAvailableStockMap = fifoCalculator.calculateFifoRate(in, out);
            AvailableStock availableStock = longAvailableStockMap.getOrDefault(itemDef.getId(), new AvailableStock());
            //itemStockEntryRepository.save(out);
            saveWithoutAudit(out);
            for (ItemStockEntry itemStokRem : in.stream().filter(id -> id.getId() > availableStock.getEntryID()).collect(Collectors.toList())) {
                BigDecimal unitCost = itemStokRem.getPrice().add(itemStokRem.getExp() == null ? BigDecimal.ZERO : itemStokRem.getExp());
                BigDecimal inQty = itemStokRem.getItemQuantity();
                BigDecimal inAmount = unitCost.multiply(inQty);
                BigDecimal existAmount = availableStock.getAvailableQty().multiply(availableStock.getMovingAvgRate());
                BigDecimal totalQty = availableStock.getAvailableQty().add(inQty);
                BigDecimal totalAmount = existAmount.add(inAmount);
                if (totalQty.compareTo(MSTConstants.BIG_DECIMAL_ZERO) > 0) {
                    availableStock.setMovingAvgRate(totalAmount.divide(totalQty, 4, BigDecimal.ROUND_HALF_UP));
                    availableStock.setAvailableQty(totalQty);
                }
                //itemStokRem.setOpeningRate(availableStock.getMovingAvgRate());

            }
            BigDecimal qtyIn = in.stream().filter(entry -> entry.getItemDef().getId() == itemDef.getId()).filter(e -> e.getItemQuantity() != null).map(entry -> entry.getItemQuantity()).reduce(MSTConstants.BIG_DECIMAL_ZERO, BigDecimal::add);
            BigDecimal qtyOut = out.stream().filter(entry -> entry.getItemDef().getId() == itemDef.getId()).filter(e -> e.getItemQuantity() != null).map(entry -> entry.getItemQuantity()).reduce(MSTConstants.BIG_DECIMAL_ZERO, BigDecimal::add);
            if (qtyIn == null) {
                qtyIn = MSTConstants.BIG_DECIMAL_ZERO;
            }
            if (qtyOut == null) {
                qtyOut = MSTConstants.BIG_DECIMAL_ZERO;
            }
            itemDef.setOpeningQty(qtyIn.subtract(qtyOut).setScale(2, BigDecimal.ROUND_UP));
            itemDef.setAvailableQuantity(qtyIn.subtract(qtyOut).setScale(2, BigDecimal.ROUND_UP));
            itemDef.setOpeningRate(availableStock.getMovingAvgRate());
            // itemDefService.addOrUpdateItemDef(itemDef);
            itemDefRepository.save(itemDef);
           /* //fifoCalculator.calculateFifo(allByItemDefIdOrderByIdAsc);
            try {
                // itemStockEntryRepository.save(itemStockEntries);
                //  itemStockEntryRepository.updateItemStockEntryRates(itemDef.getId());
                itemStockEntryRepository.updateItemDefOpeningRate(itemDef.getId());
            } catch (Exception e) {
                itemStockEntryRepository.updateItemDefOpeningRate(itemDef.getId());
                System.out.println(e.getMessage() + "ERROR AVG: " + itemDef.getId());
                //  itemStockEntryRepository.updateItemStockEntryRates(46l);
            }*/

        }

        //List<AverageRateRequest> myRequest = itemStockEntryRepsotory.getItemStockEntryRates();

    }
}
