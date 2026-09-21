package com.mst.controllers;
import com.mst.models.dto.PayablesForecastRequest;
import com.mst.services.PayablesForecastService;
import java.util.Map;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;
@Controller
public class PayablesForecastController {
    private final PayablesForecastService service;
    public PayablesForecastController(PayablesForecastService service){this.service=service;}
    @GetMapping("/accounts/reports/due-date-analysis") public String page(){return "accounts/reports/payables_forecast";}
    @GetMapping("/api/accounts/payables-forecast") @ResponseBody
    public Map<String,Object> report(@ModelAttribute PayablesForecastRequest request){return service.load(request);}
}
