using System;
using Architecture.Model.Common;

namespace Architecture.BLL.Common;

public abstract class BaseRepository
{
	protected string CreatedBy { get; set; }

	protected DateTime Createdon { get; set; }

	protected long UserLogId { get; set; }

	public BaseRepository()
	{
		CreatedBy = LoginObject.LoginName;
		Createdon = DateTime.Now;
		UserLogId = LoginObject.LoginAppUserLogId;
	}

	public virtual void Log(object id, string requestFrom)
	{
		Console.WriteLine("Created by {0} and created on {1}, Id {2}, Request {3} ", CreatedBy, Createdon, id, requestFrom);
	}
}
