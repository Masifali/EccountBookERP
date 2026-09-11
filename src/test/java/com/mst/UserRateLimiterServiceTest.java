package com.mst;

///import io.github.bonigarcia.wdm.WebDriverManager;

public class UserRateLimiterServiceTest {
    public static void main(String[] args) throws Exception {
        // Setup ChromeDriver
     //   WebDriverManager.chromedriver().setup();

        try {
            Runtime.getRuntime().exec("taskkill /F /IM chrome.exe");
            Thread.sleep(2000);
        } catch (Exception ignored) {
        }
    }
}