package com.mst.models.hrm.dto;

import java.util.List;

/**
 * 650/651 PullAttendanceByMachine.MakeProductDeviceData(JobTask): the ticked machine rows (their Id),
 * the ticked employee rows (their EmployeeNo) and the job task of the button pressed
 * (PullLog | Download | Upload | Delete).
 */
public class DevicePullJobDto {
    public String jobTask;
    public List<Integer> devices;
    public List<String> employeeNos;
}
