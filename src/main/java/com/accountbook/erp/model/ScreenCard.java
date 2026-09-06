package com.accountbook.erp.model;

public class ScreenCard {
    private String id;
    private String title;
    private String targetUrl;
    private String category;
    private Boolean isFav;

    public ScreenCard() {}

    public ScreenCard(String id, String title, String targetUrl, String category, Boolean isFav) {
        this.id = id;
        this.title = title;
        this.targetUrl = targetUrl;
        this.category = category;
        this.isFav = isFav;
    }

    public String getId() { return id; }
    public void setId(String id) { this.id = id; }

    public String getTitle() { return title; }
    public void setTitle(String title) { this.title = title; }

    public String getTargetUrl() { return targetUrl; }
    public void setTargetUrl(String targetUrl) { this.targetUrl = targetUrl; }

    public String getCategory() { return category; }
    public void setCategory(String category) { this.category = category; }

    public Boolean getIsFav() { return isFav; }
    public void setIsFav(Boolean isFav) { this.isFav = isFav; }
}
