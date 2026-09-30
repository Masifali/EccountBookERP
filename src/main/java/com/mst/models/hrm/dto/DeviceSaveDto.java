package com.mst.models.hrm.dto;

/**
 * 649 frmDeviceManagement Insert(): the record being edited (RecId, 0 = new) and the form's controls.
 * The location's text (DeviceLocationText) is taken from the company's own location list on the server.
 */
public class DeviceSaveDto {
    public int id;
    public int locationId;
    public String deviceName;
    public String networkPort;
    public String networkIP;
    public boolean isActive;
}
