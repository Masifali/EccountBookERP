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

internal class DeleteAllFilesCommand : Command
{
	private static FileCounterCommand.FileNumber _fileNumber;

	private static int _currentFileNum;

	private static List<IntPtr> _imageItems;

	private CameraEvent _event;

	private IntPtr _volume;

	public DeleteAllFilesCommand(ref CameraModel model, ref IntPtr volume)
		: base(ref model)
	{
		_volume = volume;
	}

	public override bool Execute()
	{
		IntPtr camera = _model.Camera;
		IntPtr directoryItem = IntPtr.Zero;
		FileCounterCommand fileCounter = new FileCounterCommand(ref _model, ref _volume);
		_imageItems = new List<IntPtr>();
		if (_currentFileNum == 0)
		{
			int directoryCount = 0;
			int fileCount = 0;
			_event = new CameraEvent(CameraEvent.Type.DELETE_START, (IntPtr)fileCount);
			_model.NotifyObservers(_event);
			fileCounter.CountDirectory(camera, ref directoryItem, out directoryCount);
			_fileNumber = new FileCounterCommand.FileNumber(directoryCount);
			_fileNumber.DcimItem = directoryItem;
			if (fileCounter.CountImages(camera, ref directoryItem, ref directoryCount, ref fileCount, ref _fileNumber, ref _imageItems) != 0)
			{
				return false;
			}
			_event = new CameraEvent(CameraEvent.Type.DELETE_START, (IntPtr)fileCount);
			_model.NotifyObservers(_event);
			if (fileCount == 0)
			{
				_event = new CameraEvent(CameraEvent.Type.DELETE_COMPLETE, (IntPtr)fileCount);
				_model.NotifyObservers(_event);
			}
		}
		if (DeleteFileByDirectory(camera) != 0)
		{
			return false;
		}
		return true;
	}

	private uint DeleteFileByDirectory(IntPtr camera)
	{
		int index = 0;
		for (index = 0; index < _imageItems.Count; index++)
		{
			uint err = EDSDK.EdsDeleteDirectoryItem(_imageItems[index]);
			if (err != 0)
			{
				return err;
			}
			EDSDK.EdsRelease(_imageItems[index]);
			if (_model._ExecuteStatus == CameraModel.Status.CANCELING)
			{
				_event = new CameraEvent(CameraEvent.Type.DELETE_COMPLETE, (IntPtr)index);
				_model.NotifyObservers(_event);
				_currentFileNum = 0;
				_imageItems.Clear();
				return 0u;
			}
			_event = new CameraEvent(CameraEvent.Type.PROGRESS_REPORT, (IntPtr)index);
			_model.NotifyObservers(_event);
		}
		_event = new CameraEvent(CameraEvent.Type.DELETE_COMPLETE, camera);
		_model.NotifyObservers(_event);
		_currentFileNum = 0;
		return 0u;
	}
}

}
