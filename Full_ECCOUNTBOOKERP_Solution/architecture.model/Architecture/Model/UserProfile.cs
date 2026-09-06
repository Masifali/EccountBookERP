using System;

namespace Architecture.Model;

public class UserProfile
{
	public int Age { get; set; }

	public int Id { get; set; }

	public int UserId { get; set; }

	public string AuthenticationMode { get; set; }

	public string CellNo { get; set; }

	public string Education { get; set; }

	public string Email { get; set; }

	public string FatherName { get; set; }

	public string WhatsApp { get; set; }

	public string ProfileImageFileName { get; set; }

	public string CNICNumber { get; set; }

	public DateTime JoiningDate { get; set; }

	public virtual string PictureToDeleteInUpdateIfAny { get; set; }
}
