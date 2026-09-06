package com.accountbook.erp.model;

public class SubModuleCard {
    private Long id;
    private String title;
    private Integer count;
    private String icon;

    public SubModuleCard() {}

    public SubModuleCard(Long id, String title, Integer count, String icon) {
        this.id = id;
        this.title = title;
        this.count = count;
        this.icon = icon;
    }

    public Long getId() { return id; }
    public void setId(Long id) { this.id = id; }

    public String getTitle() { return title; }
    public void setTitle(String title) { this.title = title; }

    public Integer getCount() { return count; }
    public void setCount(Integer count) { this.count = count; }

    public String getIcon() { return icon; }
    public void setIcon(String icon) { this.icon = icon; }
}
