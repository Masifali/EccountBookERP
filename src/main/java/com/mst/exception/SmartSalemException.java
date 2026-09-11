package com.mst.exception;


import lombok.Data;

@Data
public class SmartSalemException extends RuntimeException {
    private SmartSalemErrorCode smartSalemErrorCode;

    public SmartSalemException(SmartSalemErrorCode smartSalemErrorCode) {
        super(smartSalemErrorCode.getCode());
        this.smartSalemErrorCode = smartSalemErrorCode;
    }

}
