using System;
using System.Collections.Generic;
using EDSDKLib;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace CameraControl
{

internal class DownloadAllFilesCommand : Command
{
	private static FileCounterCommand.FileNumber _fileNumber;

	private static int _currentFileNum = 0;

	private static int _fileCount = 0;

	private static List<IntPtr> _imageItems = new List<IntPtr>();

	private CameraEvent _event;

	private IntPtr _volume;

	public DownloadAllFilesCommand(ref CameraModel model, ref IntPtr volume)
		: base(ref model)
	{
		_volume = volume;
	}

	public override bool Execute()
	{
		IntPtr camera = _model.Camera;
		IntPtr directoryItem = IntPtr.Zero;
		FileCounterCommand fileCounter = new FileCounterCommand(ref _model, ref _volume);
		if (_currentFileNum == 0)
		{
			fileCounter.CountDirectory(camera, ref directoryItem, out var directoryCount);
			_fileNumber = new FileCounterCommand.FileNumber(directoryCount);
			_fileNumber.DcimItem = directoryItem;
			if (fileCounter.CountImages(camera, ref directoryItem, ref directoryCount, ref _fileCount, ref _fileNumber, ref _imageItems) != 0)
			{
				return false;
			}
			_event = new CameraEvent(CameraEvent.Type.DOWNLOAD_START, (IntPtr)_fileCount);
			_model.NotifyObservers(_event);
		}
		if (DownloadImageByDirectory(camera) != 0)
		{
			return false;
		}
		return true;
	}

	private uint DownloadImageByDirectory(IntPtr camera)
	{
		EDSDK.EdsDirectoryItemInfo dirItemInfo = default(EDSDK.EdsDirectoryItemInfo);
		dirItemInfo.szFileName = "";
		dirItemInfo.Size = 0uL;
		int index = 0;
		for (index = 0; index < _imageItems.Count; index++)
		{
			IntPtr fileitem = _imageItems[_currentFileNum];
			uint err = EDSDK.EdsGetDirectoryItemInfo(fileitem, out dirItemInfo);
			if (err != 0)
			{
				return err;
			}
			err = EDSDK.EdsCreateFileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\" + dirItemInfo.szFileName, EDSDK.EdsFileCreateDisposition.CreateAlways, EDSDK.EdsAccess.ReadWrite, out var stream);
			if (err != 0)
			{
				return err;
			}
			err = EDSDK.EdsDownload(fileitem, dirItemInfo.Size, stream);
			if (err != 0)
			{
				return err;
			}
			err = EDSDK.EdsDownloadComplete(fileitem);
			if (err != 0)
			{
				return err;
			}
			err = EDSDK.EdsRelease(fileitem);
			fileitem = IntPtr.Zero;
			err = EDSDK.EdsRelease(stream);
			stream = IntPtr.Zero;
			_currentFileNum++;
			if (_model._ExecuteStatus == CameraModel.Status.CANCELING)
			{
				_event = new CameraEvent(CameraEvent.Type.DOWNLOAD_COMPLETE, (IntPtr)index);
				_model.NotifyObservers(_event);
				_currentFileNum = 0;
				_imageItems.Clear();
			}
			else
			{
				_event = new CameraEvent(CameraEvent.Type.PROGRESS_REPORT, (IntPtr)_currentFileNum);
				_model.NotifyObservers(_event);
			}
		}
		_event = new CameraEvent(CameraEvent.Type.DOWNLOAD_COMPLETE, camera);
		_model.NotifyObservers(_event);
		_currentFileNum = 0;
		_imageItems.Clear();
		return 0u;
	}
}

}
