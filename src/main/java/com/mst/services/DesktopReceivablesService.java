package com.mst.services;

import com.mst.models.dto.DesktopReceivablesRequest;
import com.mst.repositories.DesktopReceivablesRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

@Service
public class DesktopReceivablesService {
    private final DesktopReceivablesRepository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    public DesktopReceivablesService(DesktopReceivablesRepository repository,CurrentUserContext context,DesktopReportRights rights) { this.repository=repository; this.context=context; this.rights=rights; }
    public List<Map<String,Object>> load(String report,DesktopReceivablesRequest r) {
        dates(r.getFromDate(),r.getToDate()); dates(r.getDueFrom(),r.getDueTo()); dates(r.getSaleFrom(),r.getSaleTo());
        if(!report.equals("receivables-receipt-schedule") && r.getToDate()==null) throw new IllegalArgumentException("Select a To date");
        if(!Set.of(0,2,3).contains(r.getShowAssetLiability())) throw new IllegalArgumentException("Invalid account classification");
        for(String ids:List.of(r.getControls(),r.getGroups(),r.getBranches()))
            if(!ids.isBlank() && !ids.matches("[0-9]+(,[0-9]+)*")) throw new IllegalArgumentException("Invalid selected accounts or groups");
        var user=context.requireAccountingUser();
        Integer screen=Map.of("receivables-by-due-dates",68,"receivables-receipt-schedule",71,"receivables-report",80,"payables-report",48).get(report);
        if(screen==null) throw new IllegalArgumentException("Unknown report");
        rights.require(user,screen,"View");
        return repository.load(report,user,r);
    }
    private void dates(LocalDate from,LocalDate to) {
        if(from!=null && to!=null && from.isAfter(to)) throw new IllegalArgumentException("From date must be on or before To date");
    }
}
