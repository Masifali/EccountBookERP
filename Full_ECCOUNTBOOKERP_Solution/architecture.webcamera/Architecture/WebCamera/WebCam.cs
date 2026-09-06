using System;
using System.Windows.Forms;
using WebCam_Capture;

namespace Architecture.WebCamera;

public class WebCam
{
	private WebCamCapture webcam;

	private PictureBox _FrameImage;

	private int FrameNumber = 30;

	public void InitializeWebCam(ref PictureBox ImageControl)
	{
		webcam = new WebCamCapture();
		webcam.FrameNumber = 0uL;
		webcam.TimeToCapture_milliseconds = FrameNumber;
		webcam.ImageCaptured += webcam_ImageCaptured;
		_FrameImage = ImageControl;
	}

	private void webcam_ImageCaptured(object source, WebcamEventArgs e)
	{
		if (e.WebCamImage == null)
		{
			throw new Exception("Object Isnull ImageCapture");
		}
		_FrameImage.Image = e.WebCamImage;
	}

	public void Start()
	{
		if (webcam == null)
		{
			throw new Exception("Web Cam Object Start Method IsNull");
		}
		webcam.TimeToCapture_milliseconds = FrameNumber;
		webcam.Start(0uL);
	}

	public void Stop()
	{
		webcam.Stop();
	}

	public void Continue()
	{
		webcam.TimeToCapture_milliseconds = FrameNumber;
		webcam.Start(webcam.FrameNumber);
	}

	public void ResolutionSetting()
	{
		webcam.Config();
	}

	public void AdvanceSetting()
	{
		webcam.Config2();
	}
}
