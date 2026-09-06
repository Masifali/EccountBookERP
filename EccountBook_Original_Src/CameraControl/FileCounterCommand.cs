using System;
using System.Collections.Generic;
using EDSDKLib;

namespace CameraControl;

internal class FileCounterCommand : Command
{
	public struct FileNumber
	{
		public IntPtr DcimItem;

		public int dirnum;

		public int[] filenum;

		public FileNumber(int p1)
		{
			DcimItem = IntPtr.Zero;
			dirnum = p1;
			filenum = new int[dirnum];
		}
	}

	private IntPtr _volume;

	public FileCounterCommand(ref CameraModel model, ref IntPtr volume)
		: base(ref model)
	{
		_volume = volume;
	}

	public uint CountDirectory(IntPtr camera, ref IntPtr directoryItem, out int directory_count)
	{
		uint err = 0u;
		int item_count = 0;
		directory_count = 0;
		EDSDK.EdsDirectoryItemInfo dirItemInfo = default(EDSDK.EdsDirectoryItemInfo);
		dirItemInfo.szFileName = "";
		dirItemInfo.Size = 0uL;
		err = EDSDK.EdsGetChildCount(_volume, out item_count);
		if (err != 0)
		{
			return err;
		}
		for (int i = 0; i < item_count; i++)
		{
			if (EDSDK.EdsGetChildAtIndex(_volume, i, out var dirItem) == 0)
			{
				err = EDSDK.EdsGetDirectoryItemInfo(dirItem, out dirItemInfo);
				if (err != 0)
				{
					return err;
				}
				if (dirItemInfo.szFileName == "DCIM" && dirItemInfo.isFolder == 1)
				{
					directoryItem = dirItem;
					break;
				}
				if (dirItem != IntPtr.Zero)
				{
					EDSDK.EdsRelease(dirItem);
				}
			}
		}
		return err = EDSDK.EdsGetChildCount(directoryItem, out directory_count);
	}

	public uint CountImages(IntPtr camera, ref IntPtr directoryItem, ref int directory_count, ref int fileCount, ref FileNumber fileNumber, ref List<IntPtr> imageItems)
	{
		uint err = 0u;
		fileCount = 0;
		for (int i = 0; i < directory_count; i++)
		{
			int count = 0;
			err = CountImagesByDirectory(ref directoryItem, i, ref count, ref imageItems);
			if (err != 0)
			{
				return err;
			}
			fileCount += count;
			fileNumber.filenum[i] = count;
		}
		return 0u;
	}

	private uint CountImagesByDirectory(ref IntPtr directoryItem, int directoryNo, ref int image_count, ref List<IntPtr> imageItems)
	{
		int item_count = 0;
		uint err = EDSDK.EdsGetChildAtIndex(directoryItem, directoryNo, out var directoryfiles);
		if (err != 0)
		{
			return err;
		}
		err = EDSDK.EdsGetDirectoryItemInfo(directoryfiles, out var dirItemInfo);
		if (err != 0)
		{
			return err;
		}
		int index = 0;
		int filecount = 0;
		err = EDSDK.EdsGetChildCount(directoryfiles, out item_count);
		if (err != 0)
		{
			return err;
		}
		for (index = 0; index < item_count; index++)
		{
			err = EDSDK.EdsGetChildAtIndex(directoryfiles, index, out var fileitem);
			if (err != 0)
			{
				return err;
			}
			err = EDSDK.EdsGetDirectoryItemInfo(fileitem, out dirItemInfo);
			if (err != 0)
			{
				return err;
			}
			if (dirItemInfo.isFolder == 0)
			{
				imageItems.Add(fileitem);
				filecount++;
			}
		}
		image_count = filecount;
		return 0u;
	}
}
