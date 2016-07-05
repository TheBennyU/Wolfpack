using System;
using SQLite;

namespace Wolfpack
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}

