package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** Desktop LoginNew equivalent entry point for the web session. */
@Controller
public class LoginController {
    @GetMapping("/login")
    public String login() {
        return "login";
    }
}
