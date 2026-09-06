using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Architecture.WinApp.Helper
{
	public static class FormHelper
	{
		public enum ValidType
		{
			String,
			Double,
			INT,
			DateTime
		}

		public static bool ValidateControls(List<(Control control, string fieldName, ValidType validationType)> controlsToValidate)
		{
			return true;
		}

		public static bool ConfirmAction(string Message)
		{
			return MessageBox.Show(Message + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
		}

		public static void FormEventHandler(Control parent)
		{
		}

		public static void SetCtrlGrdBarRight(object ctrlGrd, object formright)
		{
		}

		public static void ValidateField(object value, string fieldName, int rowIndex, string GridName = "Detail Grid")
		{
		}

		public static void UpdateAttachmentsForObject<T>(T obj, string screenName, object AT, string atSaveListName, string atDeleteListName)
		{
		}

		public static void LoadAttachmentsForObject(object AT, int recId, string screenName = null, int documentTypeId = 0)
		{
		}

		public static void DeleteAllAttachmentsFromDb(object AT, int RecId, int DocumentTypeId, string screenName)
		{
		}

		public static void OpenFormInPanel(params object[] args)
		{
		}
	}
}
