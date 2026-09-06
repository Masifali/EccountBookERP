using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EDSDKLib;

namespace CameraControl;

internal class EvfPictureBox : PictureBox, IObserver
{
	private delegate void _Update(Observable from, CameraEvent e);

	private static ImageConverter imgconv = new ImageConverter();

	private CameraModel _model;

	private bool _active;

	private bool m_bDrawZoomFrame;

	private EDSDK.EdsRect vRect;

	private EDSDK.EdsFocusInfo m_focusInfo;

	private ActionSource _actionSource;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public EvfPictureBox()
	{
		_active = false;
	}

	public void Update(Observable from, CameraEvent e)
	{
		if (base.InvokeRequired)
		{
			Invoke(new _Update(Update), from, e);
			return;
		}
		CameraEvent.Type eventType = e.GetEventType();
		_model = (CameraModel)from;
		switch (eventType)
		{
		case CameraEvent.Type.EVFDATA_CHANGED:
		{
			EVFDataSet evfDataSet = (EVFDataSet)Marshal.PtrToStructure(e.GetArg(), typeof(EVFDataSet));
			OnDrawImage(evfDataSet);
			uint propertyID = 260u;
			_actionSource.FireEvent(ActionEvent.Command.GET_PROPERTY, (IntPtr)propertyID);
			_actionSource.FireEvent(ActionEvent.Command.DOWNLOAD_EVF, IntPtr.Zero);
			break;
		}
		case CameraEvent.Type.PROPERTY_CHANGED:
		{
			uint propertyID = (uint)(int)e.GetArg();
			switch (propertyID)
			{
			case 1280u:
			{
				uint device = _model.EvfOutputDevice;
				if (!_active && (device & 2u) != 0)
				{
					_active = true;
					_actionSource.FireEvent(ActionEvent.Command.DOWNLOAD_EVF, IntPtr.Zero);
				}
				if (_active && (device & 2) == 0)
				{
					_active = false;
				}
				return;
			}
			case 260u:
				if (base.Image != null)
				{
					float xRatio = 1f;
					float yRatio = 1f;
					m_focusInfo = _model.FocusInfo;
					xRatio = (float)base.Image.Width / (float)m_focusInfo.imageRect.width;
					yRatio = (float)base.Image.Height / (float)m_focusInfo.imageRect.height;
					for (uint i = 0u; i < m_focusInfo.pointNumber; i++)
					{
						m_focusInfo.focusPoint[i].rect.x = (int)((float)m_focusInfo.focusPoint[i].rect.x * xRatio);
						m_focusInfo.focusPoint[i].rect.y = (int)((float)m_focusInfo.focusPoint[i].rect.y * yRatio);
						m_focusInfo.focusPoint[i].rect.width = (int)((float)m_focusInfo.focusPoint[i].rect.width * xRatio);
						m_focusInfo.focusPoint[i].rect.height = (int)((float)m_focusInfo.focusPoint[i].rect.height * yRatio);
					}
					return;
				}
				break;
			}
			if (propertyID == 1294)
			{
				m_bDrawZoomFrame = _model.EvfAFMode != 2 && _model.isTypeDS;
			}
			break;
		}
		}
	}

	private void OnDrawImage(EVFDataSet evfDataSet)
	{
		EDSDK.EdsGetPointer(evfDataSet.stream, out var evfStream);
		EDSDK.EdsGetLength(evfDataSet.stream, out var streamLength);
		vRect = _model.VisibleRect;
		SolidBrush black = new SolidBrush(Color.Black);
		byte[] data = new byte[(uint)streamLength];
		Marshal.Copy(evfStream, data, 0, (int)streamLength);
		Image img = (Image)imgconv.ConvertFrom(data);
		Bitmap canvas = new Bitmap(img);
		int iWidth = canvas.Width;
		int iHeight = canvas.Height;
		Graphics g = Graphics.FromImage(canvas);
		g.DrawImage(img, 0, 0);
		if (_model.Aspect == 1 || _model.Aspect == 2)
		{
			float hvRatio = (float)vRect.width / (float)vRect.height;
			int rWidth = (int)((float)iWidth - (float)iHeight * hvRatio) / 2;
			Rectangle rectLeft = new Rectangle(0, 0, rWidth, iHeight);
			Rectangle rectRight = new Rectangle((int)((float)rWidth + (float)iHeight * hvRatio), 0, rWidth, iHeight);
			g.FillRectangle(black, rectLeft);
			g.FillRectangle(black, rectRight);
		}
		if (_model.Aspect == 7)
		{
			float vhRatio = (float)vRect.height / (float)vRect.width;
			int rHeight = (int)((float)iHeight - (float)iWidth * vhRatio) / 2;
			Rectangle rectTop = new Rectangle(0, 0, iWidth, rHeight);
			Rectangle rectBottom = new Rectangle(0, (int)((float)rHeight + (float)iWidth * vhRatio), iWidth, rHeight);
			g.FillRectangle(black, rectTop);
			g.FillRectangle(black, rectBottom);
		}
		if (evfDataSet.zoom == 1 && evfDataSet.sizeJpegLarge.width != 0 && evfDataSet.sizeJpegLarge.height != 0)
		{
			OnDrawFocusRect(ref g, ref evfDataSet, ref iWidth, ref iHeight);
		}
		g.Dispose();
		if (_model.isEvfEnable)
		{
			base.Image = canvas;
		}
		else
		{
			base.Image = null;
		}
	}

	private void OnDrawFocusRect(ref Graphics g, ref EVFDataSet evfDataSet, ref int iWidth, ref int iHeight)
	{
		if (m_bDrawZoomFrame)
		{
			int cx = iWidth;
			int cy = iHeight;
			int iw = evfDataSet.sizeJpegLarge.width;
			int ih = evfDataSet.sizeJpegLarge.height;
			long num = evfDataSet.zoomRect.x;
			long top = evfDataSet.zoomRect.y;
			long x = num * cx / iw;
			long y = top * cy / ih;
			long width = evfDataSet.zoomRect.width * cx / iw;
			long height = evfDataSet.zoomRect.height * cy / ih;
			Pen pen = new Pen(Color.FromArgb(255, 255, 255));
			pen.Width = 3f;
			g.DrawRectangle(pen, x, y, width, height);
		}
		Pen defaultPen = new Pen(Color.FromArgb(255, 255, 255));
		defaultPen.Width = 3f;
		Pen errPen = new Pen(Color.FromArgb(255, 0, 0));
		errPen.Width = 3f;
		Pen servoPen = new Pen(Color.FromArgb(65, 148, 232));
		servoPen.Width = 3f;
		Pen justPen = new Pen(Color.FromArgb(0, 255, 0));
		justPen.Width = 3f;
		Pen disablePen = new Pen(Color.FromArgb(128, 128, 128));
		disablePen.Width = 3f;
		Rectangle afRect = new Rectangle(0, 0, 0, 0);
		for (uint i = 0u; i < m_focusInfo.pointNumber; i++)
		{
			if (m_focusInfo.focusPoint[i].valid == 1)
			{
				Pen oldPenH = (((m_focusInfo.focusPoint[i].justFocus & 0xF) == 1) ? justPen : (((m_focusInfo.focusPoint[i].justFocus & 0xF) == 2) ? errPen : (((m_focusInfo.focusPoint[i].justFocus & 0xF) != 4) ? defaultPen : servoPen)));
				if (m_focusInfo.focusPoint[i].selected != 1)
				{
					oldPenH = disablePen;
				}
				afRect.X = m_focusInfo.focusPoint[i].rect.x;
				afRect.Y = m_focusInfo.focusPoint[i].rect.y;
				afRect.Width = m_focusInfo.focusPoint[i].rect.width;
				afRect.Height = m_focusInfo.focusPoint[i].rect.height;
				g.DrawRectangle(oldPenH, afRect);
			}
		}
	}
}
