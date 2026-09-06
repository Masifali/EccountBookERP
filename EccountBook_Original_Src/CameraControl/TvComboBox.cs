using System;
using EDSDKLib;

namespace CameraControl;

internal class TvComboBox : PropertyComboBox, IObserver
{
	private ActionSource _actionSource;

	private EDSDK.EdsPropertyDesc _desc;

	public void SetActionSource(ref ActionSource actionSource)
	{
		_actionSource = actionSource;
	}

	public TvComboBox()
	{
		map.Add(4u, "Auto");
		map.Add(12u, "Bulb");
		map.Add(16u, "30''");
		map.Add(19u, "25''");
		map.Add(20u, "20''");
		map.Add(21u, "20''");
		map.Add(24u, "15''");
		map.Add(27u, "13''");
		map.Add(28u, "10''");
		map.Add(29u, "10''");
		map.Add(32u, "8''");
		map.Add(35u, "6''");
		map.Add(36u, "6''");
		map.Add(37u, "5''");
		map.Add(40u, "4''");
		map.Add(43u, "3''2");
		map.Add(44u, "3''");
		map.Add(45u, "2''5");
		map.Add(48u, "2''");
		map.Add(51u, "1''6");
		map.Add(52u, "1''5");
		map.Add(53u, "1''3");
		map.Add(56u, "1''");
		map.Add(59u, "0''8");
		map.Add(60u, "0''7");
		map.Add(61u, "0''6");
		map.Add(64u, "0''5");
		map.Add(67u, "0''4");
		map.Add(68u, "0''3");
		map.Add(69u, "0''3");
		map.Add(72u, "4");
		map.Add(75u, "5");
		map.Add(76u, "6");
		map.Add(77u, "6");
		map.Add(80u, "8");
		map.Add(83u, "10");
		map.Add(84u, "10");
		map.Add(85u, "13");
		map.Add(88u, "15");
		map.Add(91u, "20");
		map.Add(92u, "20");
		map.Add(93u, "25");
		map.Add(96u, "30");
		map.Add(99u, "40");
		map.Add(100u, "45");
		map.Add(101u, "50");
		map.Add(104u, "60");
		map.Add(107u, "80");
		map.Add(108u, "90");
		map.Add(109u, "100");
		map.Add(112u, "125");
		map.Add(115u, "160");
		map.Add(116u, "180");
		map.Add(117u, "200");
		map.Add(120u, "250");
		map.Add(123u, "320");
		map.Add(124u, "350");
		map.Add(125u, "400");
		map.Add(128u, "500");
		map.Add(131u, "640");
		map.Add(132u, "750");
		map.Add(133u, "800");
		map.Add(136u, "1000");
		map.Add(139u, "1250");
		map.Add(140u, "1500");
		map.Add(141u, "1600");
		map.Add(144u, "2000");
		map.Add(147u, "2500");
		map.Add(148u, "3000");
		map.Add(149u, "3200");
		map.Add(152u, "4000");
		map.Add(155u, "5000");
		map.Add(156u, "6000");
		map.Add(157u, "6400");
		map.Add(160u, "8000");
		map.Add(163u, "10000");
		map.Add(165u, "12800");
		map.Add(168u, "16000");
		map.Add(uint.MaxValue, "unknown");
	}

	protected override void OnSelectionChangeCommitted(EventArgs e)
	{
		if (base.SelectedItem != null)
		{
			uint key = (uint)_desc.PropDesc[SelectedIndex];
			_actionSource.FireEvent(ActionEvent.Command.SET_TV, (IntPtr)key);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		CameraModel model = (CameraModel)from;
		CameraEvent.Type eventType = CameraEvent.Type.NONE;
		if (((eventType = e.GetEventType()) == CameraEvent.Type.PROPERTY_CHANGED || eventType == CameraEvent.Type.PROPERTY_DESC_CHANGED) && (int)e.GetArg() == 1030)
		{
			uint property = model.Tv;
			switch (eventType)
			{
			case CameraEvent.Type.PROPERTY_CHANGED:
				UpdateProperty(property);
				break;
			case CameraEvent.Type.PROPERTY_DESC_CHANGED:
				_desc = model.TvDesc;
				UpdatePropertyDesc(ref _desc);
				UpdateProperty(property);
				break;
			}
		}
	}
}
