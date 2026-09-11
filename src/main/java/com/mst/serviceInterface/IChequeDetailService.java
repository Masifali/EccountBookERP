package com.mst.serviceInterface;

import java.util.List;
import com.mst.models.ChequeDetail;

public interface IChequeDetailService {
    List<ChequeDetail> getAllChequeDetails(String chequeInHandAccount);
    ChequeDetail addOrUpdateChequeDetail(ChequeDetail chequeDetail);
    ChequeDetail findChequeDetailById(long id);
    void deleteChequeDetailById(long id);
}
