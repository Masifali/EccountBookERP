package com.mst.configuration;

import org.springframework.data.domain.AuditorAware;
import org.springframework.security.core.context.SecurityContextHolder;

import java.util.Optional;

public class AuditorAwareImpl implements AuditorAware<String> {

    @Override
    public Optional<String> getCurrentAuditor() {
        try {
            System.out.println(SecurityContextHolder.getContext().getAuthentication().getName());
        } catch (Exception e) {
            return Optional.of("Asif");
        }

        return Optional.ofNullable(SecurityContextHolder.getContext().getAuthentication().getName());
    }
}
