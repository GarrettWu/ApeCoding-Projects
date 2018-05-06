/*
 * 简单的图书馆借阅系统，实现借书还书检阅信息功能
 */
using System;
using System.Collections.Generic;
using System.Data;

namespace BookManagement
{
    //图书类
    class Book
    {
        public int number;            //编号
        public string bookName;       //书名
        public bool isIdle;           //是否闲置
        public string borrower;       //借书人名字

        //构造函数
        public Book(int number, string bookName, bool isIdle, string borrower)
        {
            this.number = number;
            this.bookName = bookName;
            this.isIdle = isIdle;
            this.borrower = borrower;
        }

        //借书
        public bool Borrow(string borrower)
        {
            if (isIdle)
            {
                isIdle = false;
                this.borrower = borrower;

                string sql = String.Format("UPDATE library SET isIdle = {0} WHERE number = {1}", 0, this.number);
				MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);

                sql = String.Format("UPDATE library SET borrower = '{0}' WHERE number = {1}", borrower, this.number);
				MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);

                return true;
            } 
            else
            {
                return false;
            }
        }

        //还书
        public void Return()
        {
            isIdle = true;
            borrower = "";

			string sql = String.Format("UPDATE library SET isIdle = {0} WHERE number = {1}", 1, this.number);
			MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);

			sql = String.Format("UPDATE library SET borrower = '' WHERE number = {0}", this.number);
			MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);
        }

        //空闲状态转化为字符串
        public string Idle2Str()
        {
            return isIdle ? "是" : "否";
        }
    }

    //图书馆类
    class Library
    {
        public List<Book> books = new List<Book>();

        //构造函数
        public Library()
        {
            LoadDataBase();
        }

		//读取数据库
		public void LoadDataBase()
		{
			String sql = "SELECT number, bookName, isIdle, borrower FROM library";
			DataSet ds = MySqlHelper.GetDataSet(MySqlHelper.Conn, CommandType.Text, sql);

			foreach (DataRow dataRow in ds.Tables[0].Rows)
			{
                int number = Convert.ToInt32(dataRow["number"]);
                string bookName = Convert.ToString(dataRow["bookName"]);
                bool isIdle = Convert.ToBoolean(dataRow["isIdle"]);
				string borrower = Convert.ToString(dataRow["borrower"]);
                Book book = new Book(number, bookName, isIdle, borrower);
                books.Add(book);
			}
		}
    }

    //借书系统类
    class BookSystem
    {
        Library library = new Library();

        //主菜单
        public void Menu()
        {
            Console.WriteLine("*******************请选择操作************************");
            Console.WriteLine("                   1.借书");
            Console.WriteLine("                   2.还书");
            Console.WriteLine("                   3.信息查询");
            Console.WriteLine("                   4.退出 exit");
            Console.Write("请输入您要选择的操作：  ");

            int input = Convert.ToInt32(Console.ReadLine());
            switch (input)
            {
                case 1:
                    Borrow();
                    break;
                case 2:
                    Return();
                    break;
                case 3:
                    Information();
                    break;
                case 4:
                    Exit();
                    break;
            }
        }

        //借书子菜单
        public void Borrow()
        {
			Console.Write("请输入借阅人：  ");
            string borrower = Console.ReadLine();
			Console.Write("请输入要借阅的图书编号：  ");
            int number = Convert.ToInt32(Console.ReadLine());

            if (library.books[number].Borrow(borrower))
            {
                Console.WriteLine("顾客 {0} 已成功借阅图书 {1}", borrower, library.books[number].bookName);
            }
            else
            {
                Console.WriteLine("图书 {0} 已被借出，无法借阅", library.books[number]);
            }

			Console.Write("输入任意字符返回目录：  ");
            Console.ReadLine();
            Menu();

		}

        //还书子菜单
        public void Return()
        {
			Console.Write("请输入要归还的图书编号：  ");
			int number = Convert.ToInt32(Console.ReadLine());
            library.books[number].Return();
            Console.WriteLine("已成功归还图书 {0}", library.books[number].bookName);

			Console.Write("输入任意字符返回目录：  ");
			Console.ReadLine();
			Menu();
        }

        //信息查询子菜单
        public void Information()
        {
            for (int i = 0; i < library.books.Count; i++)
            {
                Console.WriteLine("编号{0}：{1, -30}  可借阅：{2}  借阅人：{3}", 
                                  library.books[i].number, library.books[i].bookName, library.books[i].Idle2Str(), library.books[i].borrower);
            }

			Console.Write("输入任意字符返回目录：  ");
			Console.ReadLine();
			Menu();
        }

        //系统退出
        public void Exit()
        {
            Console.WriteLine("系统已退出");
        }
    }

    //Main类
    class MainClass
    {
        public static void Main(string[] args)
        {
            BookSystem bookSys = new BookSystem();
            bookSys.Menu();
        }
    }
}
