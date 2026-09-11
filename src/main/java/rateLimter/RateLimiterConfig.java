package rateLimter;

import org.springframework.context.annotation.Configuration;


@Configuration
public class RateLimiterConfig {

    /*@Bean
    public RateLimiterConfigCustomizer voucherRateLimiter() {
        return RateLimiterConfigCustomizer
                .of("voucherLimiter", builder -> builder
                        .limitForPeriod(5)          // allow 5 requests
                        .limitRefreshPeriod(Duration.ofSeconds(10)) // per 10 seconds
                        .timeoutDuration(Duration.ofMillis(0)));
    }*/
}
