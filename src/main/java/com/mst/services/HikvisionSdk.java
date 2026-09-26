package com.mst.services;

import com.sun.jna.Library;
import com.sun.jna.Native;
import com.sun.jna.Structure;
import com.sun.jna.ptr.IntByReference;

import java.util.Arrays;
import java.util.List;

/**
 * JNA binding of the Hikvision Device Network SDK ({@code HCNetSDK.dll}) — only the functions the
 * desktop uses, with the same signatures as {@code Architecture.HvLibrary.CHCNetSDK}
 * (CHCNetSDK.cs:17266 ff, [DllImport("HCNetSDK.dll")]).
 *
 * The two structures are laid out byte for byte as the C# ones:
 *  - NET_DVR_DEVICEINFO_V30: 48 + 14 + 2 + 7 + 9 = 80 bytes
 *  - NET_DVR_JPEGPARA:       2 + 2               =  4 bytes
 *
 * The DLL must match the JVM's bitness. The desktop is a Windows build that loads the DLL from its
 * own folder; a 64-bit JVM needs the 64-bit HCNetSDK.dll (with HCCore.dll and the HCNetSDKCom
 * folder beside it).
 */
public interface HikvisionSdk extends Library {

    static HikvisionSdk load(String libraryPath) {
        return Native.load(libraryPath, HikvisionSdk.class);
    }

    boolean NET_DVR_Init();

    boolean NET_DVR_Cleanup();

    boolean NET_DVR_SetLogToFile(int bLogEnable, String strLogDir, boolean bAutoDel);

    boolean NET_DVR_SetConnectTime(int dwWaitTime, int dwTryTimes);

    int NET_DVR_Login_V30(String sDVRIP, int wDVRPort, String sUserName, String sPassword,
                          NET_DVR_DEVICEINFO_V30 lpDeviceInfo);

    boolean NET_DVR_Logout(int iUserID);

    int NET_DVR_GetLastError();

    /** HVUtility.CreateJPG — capture straight to a file. */
    boolean NET_DVR_CaptureJPEGPicture(int lUserID, int lChannel, NET_DVR_JPEGPARA lpJpegPara, String sPicFileName);

    /** Same capture into memory — used for the page's live view (the desktop draws the video into
     *  a PictureBox with NET_DVR_RealPlay_V40, which a browser cannot host). */
    boolean NET_DVR_CaptureJPEGPicture_NEW(int lUserID, int lChannel, NET_DVR_JPEGPARA lpJpegPara,
                                           byte[] sJpegPicBuffer, int dwPicSize, IntByReference lpSizeReturned);

    class NET_DVR_DEVICEINFO_V30 extends Structure {
        public byte[] sSerialNumber = new byte[48];
        public byte byAlarmInPortNum;
        public byte byAlarmOutPortNum;
        public byte byDiskNum;
        public byte byDVRType;
        public byte byChanNum;
        public byte byStartChan;
        public byte byAudioChanNum;
        public byte byIPChanNum;
        public byte byZeroChanNum;
        public byte byMainProto;
        public byte bySubProto;
        public byte bySupport;
        public byte bySupport1;
        public byte bySupport2;
        public short wDevType;
        public byte bySupport3;
        public byte byMultiStreamProto;
        public byte byStartDChan;
        public byte byStartDTalkChan;
        public byte byHighDChanNum;
        public byte bySupport4;
        public byte byLanguageType;
        public byte[] byRes2 = new byte[9];

        @Override
        protected List<String> getFieldOrder() {
            return Arrays.asList("sSerialNumber", "byAlarmInPortNum", "byAlarmOutPortNum", "byDiskNum", "byDVRType",
                    "byChanNum", "byStartChan", "byAudioChanNum", "byIPChanNum", "byZeroChanNum", "byMainProto",
                    "bySubProto", "bySupport", "bySupport1", "bySupport2", "wDevType", "bySupport3",
                    "byMultiStreamProto", "byStartDChan", "byStartDTalkChan", "byHighDChanNum", "bySupport4",
                    "byLanguageType", "byRes2");
        }
    }

    class NET_DVR_JPEGPARA extends Structure {
        public short wPicSize;
        public short wPicQuality;

        @Override
        protected List<String> getFieldOrder() {
            return Arrays.asList("wPicSize", "wPicQuality");
        }
    }
}
