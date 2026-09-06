namespace CameraControl;

public interface IObserver
{
	void Update(Observable observable, CameraEvent e);
}
