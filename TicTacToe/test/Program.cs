using MySql.Data;
using MySql.Data.MySqlClient;
using System;

namespace Test
{
	public class HelloWorld
	{
		public static void Main(string[] args)
		{
            MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, System.Data.CommandType.Text, 
                                        "CREATE TABLE CUSTOMERS111(\n   ID   INT              NOT NULL,\n   NAME VARCHAR (20)     NOT NULL,\n   AGE  INT              NOT NULL,\n   ADDRESS  CHAR (25) ,\n   SALARY   DECIMAL (18, 2),       \n   PRIMARY KEY (ID)\n);\n");
		}
	}
}