package com.mst.models.dto;

import lombok.Data;

/**
 * DTO representing a custom group (header information).
 */
@Data
public class AccountCustomGroupDto {
    private Integer id; // corresponds to AcLookUps.Id
    private String name;
    private String description;
}
