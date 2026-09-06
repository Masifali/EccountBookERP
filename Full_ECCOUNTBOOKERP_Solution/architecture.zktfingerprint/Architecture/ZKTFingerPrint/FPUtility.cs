using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using libzkfpcsharp;

namespace Architecture.ZKTFingerPrint;

public class FPUtility : Form
{
	private PictureBox picFPImg;

	private IntPtr mDevHandle = IntPtr.Zero;

	private IntPtr mDBHandle = IntPtr.Zero;

	private IntPtr FormHandle = IntPtr.Zero;

	private bool bIsTimeToDie;

	private bool IsRegister;

	private bool bIdentify = true;

	private byte[] FPBuffer;

	private int RegisterCount;

	private const int REGISTER_FINGER_COUNT = 3;

	private byte[][] RegTmps = new byte[3][];

	private byte[] RegTmp = new byte[2048];

	private byte[] CapTmp = new byte[2048];

	private int cbCapTmp = 2048;

	private int cbRegTmp;

	private int iFid = 1;

	private int mfpWidth;

	private int mfpHeight;

	private int mfpDpi;

	private const int MESSAGE_CAPTURED_OK = 1030;

	[DllImport("user32.dll", EntryPoint = "SendMessageA")]
	public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

	public void Intilize(PictureBox pic)
	{
		picFPImg = pic;
		List<string> cmbIdx = new List<string>();
		int ret = zkfperrdef.ZKFP_ERR_OK;
		if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
		{
			int nCount = zkfp2.GetDeviceCount();
			if (nCount > 0)
			{
				for (int i = 0; i < nCount; i++)
				{
					cmbIdx.Add(i.ToString());
				}
				return;
			}
			zkfp2.Terminate();
			throw new Exception("No device connected!");
		}
		throw new Exception("Initialize fail, ret=" + ret + " !");
	}

	public void Connect(int Idx)
	{
		_ = zkfp.ZKFP_ERR_OK;
		if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(Idx)))
		{
			throw new Exception("OpenDevice fail");
		}
		if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
		{
			zkfp2.CloseDevice(mDevHandle);
			mDevHandle = IntPtr.Zero;
			throw new Exception("Init DB fail");
		}
		RegisterCount = 0;
		cbRegTmp = 0;
		iFid = 1;
		for (int i = 0; i < 3; i++)
		{
			RegTmps[i] = new byte[2048];
		}
		byte[] paramValue = new byte[4];
		int size = 4;
		zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
		zkfp2.ByteArray2Int(paramValue, ref mfpWidth);
		size = 4;
		zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
		zkfp2.ByteArray2Int(paramValue, ref mfpHeight);
		FPBuffer = new byte[mfpWidth * mfpHeight];
		size = 4;
		zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
		zkfp2.ByteArray2Int(paramValue, ref mfpDpi);
		Thread thread = new Thread(DoCapture);
		thread.IsBackground = true;
		thread.Start();
		bIsTimeToDie = false;
	}

	public void DoCapture()
	{
		while (!bIsTimeToDie)
		{
			cbCapTmp = 2048;
			if (zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp) == zkfp.ZKFP_ERR_OK)
			{
				SendMessage(FormHandle, 1030, IntPtr.Zero, IntPtr.Zero);
			}
			Thread.Sleep(200);
		}
	}

	protected override void DefWndProc(ref Message m)
	{
		if (m.Msg == 1030)
		{
			MemoryStream ms = new MemoryStream();
			BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
			Bitmap bmp = new Bitmap(ms);
			picFPImg.Image = bmp;
			zkfp2.BlobToBase64(CapTmp, cbCapTmp);
			if (IsRegister)
			{
				int ret = zkfp.ZKFP_ERR_OK;
				int fid = 0;
				int score = 0;
				ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
				if (zkfp.ZKFP_ERR_OK == ret)
				{
					throw new Exception("This finger was already register by " + fid + "!\n");
				}
				if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
				{
					throw new Exception("Please press the same finger 3 times for the enrollment.\n");
				}
				Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
				zkfp2.Base64ToBlob(zkfp2.BlobToBase64(CapTmp, cbCapTmp));
				RegisterCount++;
				if (RegisterCount >= 3)
				{
					RegisterCount = 0;
					if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)) && zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBAdd(mDBHandle, iFid, RegTmp)))
					{
						iFid++;
						IsRegister = false;
						return;
					}
					throw new Exception("enroll fail, error code=" + ret + "\n");
				}
				throw new Exception("You need to press the " + (3 - RegisterCount) + " times fingerprint\n");
			}
			if (cbRegTmp <= 0)
			{
				throw new Exception("Please register your finger first!\n");
			}
			if (bIdentify)
			{
				int ret2 = zkfp.ZKFP_ERR_OK;
				int fid2 = 0;
				int score2 = 0;
				ret2 = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid2, ref score2);
				if (zkfp.ZKFP_ERR_OK != ret2)
				{
					throw new Exception("Identify fail, ret= " + ret2 + "\n");
				}
			}
			else
			{
				int ret3 = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
				if (0 >= ret3)
				{
					throw new Exception("Match finger fail, ret= " + ret3 + "\n");
				}
			}
		}
		else
		{
			base.DefWndProc(ref m);
		}
	}

	public void Disconnect()
	{
		zkfp2.Terminate();
		cbRegTmp = 0;
	}

	public void CaptureBmp(string FileName)
	{
		if (!(FileName != "") || FileName == null || picFPImg.Image == null)
		{
			return;
		}
		Bitmap bmp = new Bitmap(picFPImg.Image.Width, picFPImg.Image.Height, PixelFormat.Format24bppRgb);
		using (Graphics g = Graphics.FromImage(bmp))
		{
			g.DrawImage(picFPImg.Image, 0, 0, bmp.Width, bmp.Height);
		}
		Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
		BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
		IntPtr scan = bmpData.Scan0;
		int bytes = bmpData.Stride * bmpData.Height;
		byte[] rgbValues = new byte[bytes];
		Marshal.Copy(scan, rgbValues, 0, bytes);
		Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);
		Bitmap bit = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format8bppIndexed);
		BitmapData bmpData2 = bit.LockBits(rect2, ImageLockMode.ReadWrite, bit.PixelFormat);
		IntPtr ptr2 = bmpData2.Scan0;
		int bytes2 = bmpData2.Stride * bmpData2.Height;
		byte[] rgbValues2 = new byte[bytes2];
		Marshal.Copy(ptr2, rgbValues2, 0, bytes2);
		double colorTemp = 0.0;
		for (int j = 0; j < bmpData.Height; j++)
		{
			for (int k = 0; k < bmpData.Width * 3; k += 3)
			{
				colorTemp = (double)(int)rgbValues[j * bmpData.Stride + k + 2] * 0.299 + (double)(int)rgbValues[j * bmpData.Stride + k + 1] * 0.578 + (double)(int)rgbValues[j * bmpData.Stride + k] * 0.114;
				rgbValues2[j * bmpData2.Stride + k / 3] = (byte)colorTemp;
			}
		}
		Marshal.Copy(rgbValues2, 0, ptr2, bytes2);
		bmp.UnlockBits(bmpData);
		ColorPalette tempPalette;
		using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
		{
			tempPalette = tempBmp.Palette;
		}
		for (int i = 0; i < 256; i++)
		{
			tempPalette.Entries[i] = Color.FromArgb(i, i, i);
		}
		bit.Palette = tempPalette;
		bit.UnlockBits(bmpData2);
		bit.Save(FileName, picFPImg.Image.RawFormat);
		bit.Dispose();
	}
}
