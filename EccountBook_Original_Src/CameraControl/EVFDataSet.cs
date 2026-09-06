using System;
using EDSDKLib;

namespace CameraControl;

public struct EVFDataSet
{
	public IntPtr stream;

	public uint zoom;

	public EDSDK.EdsRect zoomRect;

	public EDSDK.EdsRect visibleRect;

	public EDSDK.EdsPoint imagePosition;

	public EDSDK.EdsSize sizeJpegLarge;
}
