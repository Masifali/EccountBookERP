package com.mst.controllers;

import com.mst.models.dto.AccountFollowUpRequest;
import com.mst.services.AccountFollowUpService;
import java.util.Map;
import javax.validation.Valid;
import org.springframework.web.bind.annotation.*;

@RestController @RequestMapping("/api/accounts/trade-followup")
public class AccountFollowUpController {
    private final AccountFollowUpService service;
    public AccountFollowUpController(AccountFollowUpService service){this.service=service;}
    @GetMapping public Map<String,Object> history(@RequestParam int accountClass,@RequestParam(defaultValue="0") int reportScreenId){return service.history(accountClass,reportScreenId);}
    @PostMapping public Map<String,Integer> save(@Valid @RequestBody AccountFollowUpRequest request){return Map.of("id",service.save(request));}
}
