using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Architecture.WebCamera;

public class Helper
{
	public static void SaveImageCapture(Image image, string FilePath)
	{
		FileStream fstream = new FileStream(FilePath, FileMode.Create);
		image.Save(fstream, ImageFormat.Jpeg);
		fstream.Close();
	}
}
