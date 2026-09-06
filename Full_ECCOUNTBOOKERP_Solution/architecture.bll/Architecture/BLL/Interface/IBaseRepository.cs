using System.Collections.Generic;
using System.Data.SqlClient;

namespace Architecture.BLL.Interface;

public interface IBaseRepository<T>
{
	object Save(T obj);

	void SaveAll(List<T> lst);

	object Delete(T obj);

	void DeleteAll(List<T> obj);

	object New(T obj);

	object Edit(T obj);

	T GetById(long ID);

	List<T> GetAll();

	List<T> GetBySearch(List<SqlParameter> p);
}
