package com.mst.messages;

import lombok.Data;

@Data
public class CustomGroupAssignmentRow {
    private Integer lookUpId;
    private String description;
    private boolean assigned;

    public Integer getLookUpId() { return lookUpId; }
    public void setLookUpId(Integer lookUpId) { this.lookUpId = lookUpId; }
    public String getDescription() { return description; }
    public void setDescription(String description) { this.description = description; }
    public boolean isAssigned() { return assigned; }
    public void setAssigned(boolean assigned) { this.assigned = assigned; }
}
