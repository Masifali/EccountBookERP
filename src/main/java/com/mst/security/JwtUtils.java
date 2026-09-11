package com.mst.security;

import org.springframework.stereotype.Component;

@Component
public class JwtUtils {
    public boolean validateJwtToken(String authToken) {
        return true;
    }

    public boolean validateToken(String authToken, String username) {
        return true;
    }

    public String getUserNameFromJwtToken(String token) {
        return "Admin";
    }
}
