using System;
using EDSDKLib;

namespace CameraControl;

public class CameraController : ActionListener
{
	private CameraModel _model;

	private CommandProcessor _processor = new CommandProcessor();

	public int ClickType;

	public CameraController(ref CameraModel model)
	{
		_model = model;
	}

	public void Run()
	{
		_processor.Start();
		_processor.PostCommand(new OpenSessionCommand(ref _model));
		_processor.PostCommand(new GetPropertyCommand(ref _model, 2u));
	}

	public CameraModel GetModel()
	{
		return _model;
	}

	public override void ActionPerformed(ActionEvent e)
	{
		ActionEvent.Command command = e.GetActionCommand();
		switch (command)
		{
		case ActionEvent.Command.GET_PROPERTY:
		{
			uint inPropertyID = (uint)(int)e.GetArg();
			_processor.PostCommand(new GetPropertyCommand(ref _model, inPropertyID));
			break;
		}
		case ActionEvent.Command.GET_PROPERTYDESC:
		{
			uint inPropertyID = (uint)(int)e.GetArg();
			_processor.PostCommand(new GetPropertyDescCommand(ref _model, inPropertyID));
			break;
		}
		case ActionEvent.Command.TAKE_PICTURE:
			_processor.PostCommand(new TakePictureCommand(ref _model));
			break;
		case ActionEvent.Command.PRESS_COMPLETELY:
			_processor.PostCommand(new PressShutterCommand(ref _model, 3u));
			break;
		case ActionEvent.Command.PRESS_HALFWAY:
			_processor.PostCommand(new PressShutterCommand(ref _model, 1u));
			break;
		case ActionEvent.Command.PRESS_OFF:
			_processor.PostCommand(new PressShutterCommand(ref _model, 0u));
			break;
		case ActionEvent.Command.START_EVF:
			_model.isEvfEnable = true;
			_processor.PostCommand(new StartEvfCommand(ref _model));
			break;
		case ActionEvent.Command.END_EVF:
			_model.isEvfEnable = false;
			_processor.PostCommand(new DoEvfAFCommand(ref _model, 0u));
			_processor.PostCommand(new EndEvfCommand(ref _model));
			break;
		case ActionEvent.Command.DOWNLOAD_EVF:
			_processor.PostCommand(new DownloadEvfCommand(ref _model));
			break;
		case ActionEvent.Command.SET_AF_MODE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1028u, data));
			break;
		}
		case ActionEvent.Command.SET_AE_MODE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1078u, data));
			break;
		}
		case ActionEvent.Command.SET_AV:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1029u, data));
			break;
		}
		case ActionEvent.Command.SET_TV:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1030u, data));
			break;
		}
		case ActionEvent.Command.SET_ISO_SPEED:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1026u, data));
			break;
		}
		case ActionEvent.Command.SET_METERING_MODE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1027u, data));
			break;
		}
		case ActionEvent.Command.SET_DRIVE_MODE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1025u, data));
			break;
		}
		case ActionEvent.Command.SET_WHITE_BALANCE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 262u, data));
			break;
		}
		case ActionEvent.Command.SET_EXPOSURE_COMPENSATION:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1031u, data));
			break;
		}
		case ActionEvent.Command.SET_IMAGEQUALITY:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 256u, data));
			break;
		}
		case ActionEvent.Command.SET_EVF_AFMODE:
		{
			_processor.PostCommand(new DoEvfAFCommand(ref _model, 0u));
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1294u, data));
			break;
		}
		case ActionEvent.Command.SET_ZOOM:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1536u, data));
			break;
		}
		case ActionEvent.Command.SET_FLASH_MODE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1537u, data));
			break;
		}
		case ActionEvent.Command.SET_MOVIEQUALITY:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 16778275u, data));
			break;
		}
		case ActionEvent.Command.SET_MOVIE_HFR:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 16778333u, data));
			break;
		}
		case ActionEvent.Command.SET_PICTURESTYLE:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 276u, data));
			break;
		}
		case ActionEvent.Command.SET_ASPECT:
		{
			uint data = (uint)(int)e.GetArg();
			_processor.PostCommand(new SetPropertyCommand(ref _model, 16778289u, data));
			break;
		}
		case ActionEvent.Command.CLICK_WB:
			ClickType = 1;
			_processor.PostCommand(new ClickWBCommand(ref _model, 1u));
			break;
		case ActionEvent.Command.CLICK_MOUSE_WB:
			_processor.PostCommand(new ClickWBCommand(ref _model, 2u));
			break;
		case ActionEvent.Command.CLICK_AF_POINT:
			ClickType = 2;
			_processor.PostCommand(new ClickAFCommand(ref _model, 1u));
			break;
		case ActionEvent.Command.CLICK_MOUSE_AF:
			_processor.PostCommand(new ClickAFCommand(ref _model, 2u));
			break;
		case ActionEvent.Command.FOCUS_NEAR1:
			_processor.PostCommand(new DriveLensCommand(ref _model, 1u));
			break;
		case ActionEvent.Command.FOCUS_NEAR2:
			_processor.PostCommand(new DriveLensCommand(ref _model, 2u));
			break;
		case ActionEvent.Command.FOCUS_NEAR3:
			_processor.PostCommand(new DriveLensCommand(ref _model, 3u));
			break;
		case ActionEvent.Command.FOCUS_FAR1:
			_processor.PostCommand(new DriveLensCommand(ref _model, 32769u));
			break;
		case ActionEvent.Command.FOCUS_FAR2:
			_processor.PostCommand(new DriveLensCommand(ref _model, 32770u));
			break;
		case ActionEvent.Command.FOCUS_FAR3:
			_processor.PostCommand(new DriveLensCommand(ref _model, 32771u));
			break;
		case ActionEvent.Command.DOWNLOAD:
		{
			IntPtr inRef = e.GetArg();
			_processor.PostCommand(new DownloadCommand(ref _model, ref inRef));
			break;
		}
		case ActionEvent.Command.CLOSING:
			_processor.PostCommand(new CloseSessionCommand(ref _model));
			_processor.Stop();
			break;
		case ActionEvent.Command.SHUT_DOWN:
		{
			_processor.Stop();
			CameraEvent shotDownEvent = new CameraEvent(CameraEvent.Type.SHUT_DOWN, IntPtr.Zero);
			_model.NotifyObservers(shotDownEvent);
			break;
		}
		case ActionEvent.Command.EVF_AF_ON:
			if (_model.isEvfEnable)
			{
				_processor.PostCommand(new DoEvfAFCommand(ref _model, 1u));
			}
			break;
		case ActionEvent.Command.EVF_AF_OFF:
			_processor.PostCommand(new DoEvfAFCommand(ref _model, 0u));
			break;
		case ActionEvent.Command.ZOOM_FIT:
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1287u, 1));
			break;
		case ActionEvent.Command.ZOOM_ZOOM:
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1287u, 5));
			break;
		case ActionEvent.Command.POSITION_UP:
		case ActionEvent.Command.POSITION_DOWN:
		{
			EDSDK.EdsPoint point = _model.GetZoomPosition();
			if (command == ActionEvent.Command.POSITION_UP)
			{
				point.y -= 128;
				if (point.y < 0)
				{
					point.y = 0;
				}
			}
			if (command == ActionEvent.Command.POSITION_DOWN)
			{
				point.y += 128;
			}
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1288u, point));
			break;
		}
		case ActionEvent.Command.POSITION_LEFT:
		case ActionEvent.Command.POSITION_RIGHT:
		{
			EDSDK.EdsPoint point = _model.GetZoomPosition();
			if (command == ActionEvent.Command.POSITION_LEFT)
			{
				point.x -= 128;
				if (point.x < 0)
				{
					point.x = 0;
				}
			}
			if (command == ActionEvent.Command.POSITION_RIGHT)
			{
				point.x += 128;
			}
			_processor.PostCommand(new SetPropertyCommand(ref _model, 1288u, point));
			break;
		}
		case ActionEvent.Command.REMOTESHOOTING_START:
			_processor.PostCommand(new SetRemoteShootingCommand(ref _model, 1u));
			break;
		case ActionEvent.Command.REMOTESHOOTING_STOP:
			_processor.PostCommand(new SetRemoteShootingCommand(ref _model, 0u));
			break;
		case ActionEvent.Command.PRESS_STILL:
			_processor.PostCommand(new SwitchStillMovieCommand(ref _model, 264u));
			break;
		case ActionEvent.Command.PRESS_MOVIE:
			_processor.PostCommand(new SwitchStillMovieCommand(ref _model, 263u));
			break;
		case ActionEvent.Command.REC_START:
			_processor.PostCommand(new SetRecCommand(ref _model, 4u));
			break;
		case ActionEvent.Command.REC_END:
			_processor.PostCommand(new SetRecCommand(ref _model, 0u));
			_processor.PostCommand(new SwitchStillMovieCommand(ref _model, 264u));
			break;
		case ActionEvent.Command.MIRRORUP_ON:
			_processor.PostCommand(new SwitchMirrorUpCommand(ref _model, 1u));
			break;
		case ActionEvent.Command.MIRRORUP_OFF:
			_processor.PostCommand(new SwitchMirrorUpCommand(ref _model, 0u));
			break;
		case ActionEvent.Command.ROLLPITCH:
			if (_model.isEvfEnable)
			{
				if (_model.RollPitch == 0)
				{
					_model.RollPitch = 1u;
				}
				else
				{
					_model.RollPitch = 0u;
				}
			}
			_processor.PostCommand(new SetRollPitchCommand(ref _model, 265u));
			break;
		case ActionEvent.Command.END_ROLLPITCH:
			_model.RollPitch = 1u;
			_processor.PostCommand(new SetRollPitchCommand(ref _model, 265u));
			break;
		case ActionEvent.Command.SET_CAMERASETTING:
		case ActionEvent.Command.CLICK_FBS:
			break;
		}
	}

	public void DownloadFile(IntPtr volume)
	{
		_processor.PostCommand(new DownloadAllFilesCommand(ref _model, ref volume));
	}

	public void DeleteFile(IntPtr volume)
	{
		_processor.PostCommand(new DeleteAllFilesCommand(ref _model, ref volume));
	}

	public void FormatVolumeCommand(IntPtr volume)
	{
		_processor.PostCommand(new FormatVolumeCommand(ref _model, ref volume));
	}
}
