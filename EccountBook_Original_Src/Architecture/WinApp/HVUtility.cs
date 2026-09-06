using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Architecture.HvLibrary;

namespace Architecture.WinApp;

public class HVUtility
{
	private short RequestChannel = 1;

	private PictureBox RealPlayWnd;

	private Form RequestForm;

	private uint iLastErr;

	private int m_lUserID = -1;

	private bool m_bInitSDK;

	private bool m_bRecord;

	private bool m_bTalk;

	private int m_lRealHandle = -1;

	private int lVoiceComHandle = -1;

	private string str;

	private CHCNetSDK.REALDATACALLBACK RealData;

	public CHCNetSDK.NET_DVR_PTZPOS m_struPtzCfg;

	public HVUtility(Form frm, PictureBox Pb)
	{
		RequestForm = frm;
		RealPlayWnd = Pb;
		m_bInitSDK = CHCNetSDK.NET_DVR_Init();
		if (!m_bInitSDK)
		{
			MessageBox.Show("NET_DVR_Init error!");
		}
		else
		{
			CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", bAutoDel: true);
		}
	}

	public void ConnectCamera(string DVRIPAddress, short DVRPortNumber, string DVRUserName, string DVRPassword)
	{
		try
		{
			if (m_lUserID < 0)
			{
				CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = default(CHCNetSDK.NET_DVR_DEVICEINFO_V30);
				m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
				if (m_lUserID < 0)
				{
					iLastErr = CHCNetSDK.NET_DVR_GetLastError();
					str = "NET_DVR_Login_V30 failed, error code= " + iLastErr;
					throw new Exception(str);
				}
				return;
			}
			if (m_lRealHandle >= 0)
			{
				throw new Exception("Please stop live view firstly");
			}
			if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
			{
				iLastErr = CHCNetSDK.NET_DVR_GetLastError();
				str = "NET_DVR_Logout failed, error code= " + iLastErr;
				throw new Exception(str);
			}
			m_lUserID = -1;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void Preview()
	{
		if (m_lUserID < 0)
		{
			MessageBox.Show("Please login the device firstly");
		}
		else if (m_lRealHandle < 0)
		{
			CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = default(CHCNetSDK.NET_DVR_PREVIEWINFO);
			lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;
			lpPreviewInfo.lChannel = RequestChannel;
			lpPreviewInfo.dwStreamType = 0u;
			lpPreviewInfo.dwLinkMode = 0u;
			lpPreviewInfo.bBlocked = true;
			lpPreviewInfo.dwDisplayBufNum = 1u;
			lpPreviewInfo.byProtoType = 0;
			lpPreviewInfo.byPreviewMode = 0;
			if (RealData == null)
			{
				RealData = RealDataCallBack;
			}
			m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null, default(IntPtr));
			if (m_lRealHandle < 0)
			{
				iLastErr = CHCNetSDK.NET_DVR_GetLastError();
				str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr;
				MessageBox.Show(str);
			}
		}
		else if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
		{
			iLastErr = CHCNetSDK.NET_DVR_GetLastError();
			str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
			MessageBox.Show(str);
		}
		else
		{
			m_lRealHandle = -1;
		}
	}

	public void RealDataCallBack(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser)
	{
		if (dwBufSize != 0)
		{
			byte[] sData = new byte[dwBufSize];
			Marshal.Copy(pBuffer, sData, 0, (int)dwBufSize);
			FileStream fileStream = new FileStream("ÊµÊ±Á÷Êý¾Ý.ps", FileMode.Create);
			fileStream.Write(sData, 0, (int)dwBufSize);
			fileStream.Close();
		}
	}

	public void CreateBMP(string RequestFilePath)
	{
		if (!CHCNetSDK.NET_DVR_CapturePicture(m_lRealHandle, RequestFilePath))
		{
			iLastErr = CHCNetSDK.NET_DVR_GetLastError();
			str = "NET_DVR_CapturePicture failed, error code= " + iLastErr;
			MessageBox.Show(str);
		}
		else
		{
			str = "Successful to capture the BMP file and the saved file is " + RequestFilePath;
		}
	}

	public void CreateJPG(string RequestFilePath)
	{
		int lChannel = RequestChannel;
		CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = default(CHCNetSDK.NET_DVR_JPEGPARA);
		lpJpegPara.wPicQuality = 0;
		lpJpegPara.wPicSize = 255;
		if (!CHCNetSDK.NET_DVR_CaptureJPEGPicture(m_lUserID, lChannel, ref lpJpegPara, RequestFilePath))
		{
			iLastErr = CHCNetSDK.NET_DVR_GetLastError();
			str = "NET_DVR_CaptureJPEGPicture failed, error code= " + iLastErr;
			MessageBox.Show(str);
		}
		else
		{
			str = "Successful to capture the JPEG file and the saved file is " + RequestFilePath;
		}
	}

	public void Disconnect()
	{
		if (m_lRealHandle >= 0)
		{
			CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
			m_lRealHandle = -1;
		}
		if (m_lUserID >= 0)
		{
			CHCNetSDK.NET_DVR_Logout(m_lUserID);
			m_lUserID = -1;
		}
		CHCNetSDK.NET_DVR_Cleanup();
		Dispose();
	}

	public void Dispose()
	{
		if (m_lRealHandle >= 0)
		{
			CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
		}
		if (m_lUserID >= 0)
		{
			CHCNetSDK.NET_DVR_Logout(m_lUserID);
		}
		if (m_bInitSDK)
		{
			CHCNetSDK.NET_DVR_Cleanup();
		}
	}
}
