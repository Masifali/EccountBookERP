using System.Collections.Generic;
using System.Drawing;

namespace Architecture.WinApp.SQL;

public class SqlKeywords
{
	public static List<(string Word, Color Color)> GetKeywords()
	{
		return new List<(string, Color)>
		{
			("SELECT", Color.Blue),
			("DELETE", Color.Red),
			("FROM", Color.Blue),
			("IF EXISTS", Color.Purple),
			("JOIN", Color.Blue),
			("INNER JOIN", Color.Blue),
			("LEFT OUTER JOIN", Color.Blue),
			("RIGHT OUTER JOIN", Color.Blue),
			("CROSS JOIN", Color.Blue),
			("WHERE", Color.Blue),
			("IN", Color.Blue),
			("LIKE", Color.Blue),
			("AND", Color.Blue),
			("OR", Color.Blue),
			("NOT", Color.Blue),
			("GROUP BY", Color.Blue),
			("ORDER BY", Color.Blue),
			("HAVING", Color.Blue),
			("LIMIT", Color.Blue),
			("OFFSET", Color.Blue),
			("AS", Color.Blue),
			("DISTINCT", Color.Blue),
			("SET", Color.Blue),
			("INSERT INTO", Color.Green),
			("VALUES", Color.Green),
			("UPDATE", Color.Purple),
			("CREATE TABLE", Color.Orange),
			("ALTER TABLE", Color.Orange),
			("DROP TABLE", Color.Red),
			("INDEX", Color.Blue),
			("PRIMARY KEY", Color.Blue),
			("FOREIGN KEY", Color.Blue),
			("UNIQUE", Color.Blue),
			("CHECK", Color.Blue),
			("VIEW", Color.Blue),
			("TRIGGER", Color.Blue),
			("PROCEDURE", Color.Blue),
			("FUNCTION", Color.Blue),
			("CASE", Color.Blue),
			("WHEN", Color.Blue),
			("THEN", Color.Blue),
			("ELSE", Color.Blue),
			("END", Color.Blue),
			("EXISTS", Color.Blue),
			("ALL", Color.Blue),
			("ANY", Color.Blue),
			("SOME", Color.Blue)
		};
	}
}
