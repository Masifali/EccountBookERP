package com.mst.messages;

import lombok.Data;

@Data
public class CustomGroupAssignmentRow {
    private Integer lookUpId;
    private String description;
    private boolean assigned;
}
