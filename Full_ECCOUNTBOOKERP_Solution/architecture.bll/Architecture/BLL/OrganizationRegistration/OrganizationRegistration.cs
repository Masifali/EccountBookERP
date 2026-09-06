using System;
using Architecture.DAL.OrganizationRegistration;
using Architecture.Model.OrganizationRegistration;

namespace Architecture.BLL.OrganizationRegistration;

public class OrganizationRegistration
{
	public static int save(Architecture.Model.OrganizationRegistration.OrganizationRegistration obj)
	{
		try
		{
			return new Architecture.DAL.OrganizationRegistration.OrganizationRegistration().SetData(obj, "SP_RegisterNewOrganization");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
}
