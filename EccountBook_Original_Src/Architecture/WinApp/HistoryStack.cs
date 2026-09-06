using System.Collections.Generic;
using System.Windows.Forms;

namespace Architecture.WinApp;

public static class HistoryStack
{
	public static List<Form> history = new List<Form>();

	private static int currentIndex = -1;

	public static void Push(Form form)
	{
		history.Add(form);
		currentIndex = history.Count - 1;
	}

	public static Form GetPrevious()
	{
		if (currentIndex > 0)
		{
			currentIndex--;
			return history[currentIndex];
		}
		return null;
	}

	public static Form GetNext()
	{
		if (currentIndex < history.Count - 1)
		{
			currentIndex++;
			return history[currentIndex];
		}
		return null;
	}

	public static bool CanNavigateForward()
	{
		return currentIndex < history.Count - 1;
	}

	public static bool CanNavigateBackward()
	{
		return currentIndex > 0;
	}

	public static void Remove(Form form)
	{
		history.Remove(form);
		if (currentIndex >= history.Count)
		{
			currentIndex = history.Count - 1;
		}
	}

	public static Form GetCurrentForm()
	{
		return history[currentIndex];
	}
}
