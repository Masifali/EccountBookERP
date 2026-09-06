package com.mst.exception;

public enum ErrorCode implements SmartSalemErrorCode {

    INVALID_TOKEN("INVALID-TOKEN", "Invalid or expired token"),
    INVALID_CREDENTIONAL("InvalidCredentional", "Invalid username Or password"),
    TRANT_TYPE("grant_type", "grantType is Invalid"),
    USER_ALREADY_EXIST("USER_ALREADY_EXIST", "Username already created in the system. Please choose a different username."),
    EMAIL_NOT_SENT("EMAIL_NOT_SENT", "Email not sent to at this "),
    USER_EXIST("USER_EXIST", "This user will not be update"),
    RECORD_NOT_SAVED("RECORD_NOT_SAVED", "Record did not saved in database"),
    TECHNICAL_ERROR("TECHNICAL_ERROR", "This user id not exist in db"),
    ROLE_PERM("ROLE_PERM", "Assign Role did not exist in DataBase"),
    PASSWORD_NOT_MATCH("PasswordNotMatch", "The passwords do not match. Please verify"),
    APPLICATION_ALREADY_EXIST("APPLICATION_ALREADY_EXIST", "The application alreaday created in the system. Please insert a new one  "),
    FREEZONES_ALREADY_EXIST("FREEZONES_ALREADY_EXIST", "The freezones alreaday created in the system. Please insert a new one  "),
    AREA_ALREADY_EXIST("AREA_ALREADY_EXIST", "Duplicate area names are not allowed."),
    HEARD_ABOUT_ALREADY_EXIST("HEARD_ABOUT_EXIST", "Duplicate heard about names are not allowed."),
    THIS_EMAIL_ALREADY_EXIST("THIS_EMAIL_EXIST", "Duplicate email are not allowed."),
    NOQODI_CONFIGURATION_ALREADY_EXIST("NOQODI_CONFIGURATION_ALREADY_EXIST", "Noqodi configuration alreaday exist against this site."),
    ALREADY_EXIST("Name", "This name was already exist"),
    RECODR_NOT_FOUND("RECODR_NOT_FOUND", "Record not found"),

    SMTP_ALREADY_EXIST("SMTP_ALREADY_EXIST", "SMTP already created in the system. Please choose a different smtp."),
    WITH_OUT_PASSWORD("WITH_OUT_PASSWORD", "User will not be create with empty password "),
    DATE_FORMATE_ERROR("DATE_FORMATE_ERROR", "Invalid Date Formate "),
    SMTP_EXIST("SMTP_EXIST", "This smtp will not be update"),
    ALL_EXIST_READY_PROCESS("ALL_EXIST_READY_PROCESS", "This is already processed");


    private final String code;
    private String message;


    ErrorCode(String code, String message) {
        this.code = code;
        this.message = message;
    }


    @Override
    public String getCode() {
        return code;
    }

    @Override
    public String getMessage() {
        return message;
    }

    @Override
    public void setMessage(String message) {
        this.message = message;
    }


}
