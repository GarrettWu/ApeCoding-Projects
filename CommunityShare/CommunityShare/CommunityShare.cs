/*
 * 简单的社区互助系统（共享经济）, 打开程序后，有个菜单：发布需求（需求名称，可提供的报酬），
 * 提供需求，查看当前需求（排除达成的交易），查看达成的交易 ，退出系统。
 * 
 * 使用文件输入输出记录需求。
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace CommunityShare
{
    //需求状态，已发布和已完成
	enum Status
	{
		Published,
		Completed
	}

    //需求类
    class Need
    {
        public int nid;             //需求序号
        public string desc;         //需求描述
        public string user;         //发布者
        public int pay;             //金额
        public Status status;       //需求状态
        public string helper;       //接受者

        //构造函数
        public Need(int nid, string desc, string user, int pay, Status status, string helper)
        {
            this.nid = nid;
            this.desc = desc;
            this.user = user;
            this.pay = pay;
            this.status = status;
            this.helper = helper;
        }

        //接受需求
        public bool Accept(string helper)
        {
            if (status == Status.Published)
            {
                status = Status.Completed;
                this.helper = helper;
                return true;
            }
            else
            {
                return false;
            }
        }

        //打印需求标题行
        public static void PrintHeadline()
        {
            Console.WriteLine("编号      描述                          发布人    金额      状态        接受人");
        }

        //打印需求
        public void Print()
        {
            Console.WriteLine("{0, -10}{1, -30}{2, -10}{3, -10}{4,-12}{5,-10}", nid, desc, user, pay, status, helper);
        }
    }

    //平台类
    class Platform
    {
        //需求List
        public List<Need> needs = new List<Need>();

        //构造函数，读取文件
        public Platform()
        {
            LoadPlatform();
        }

        //发布需求
        public void PublishNeed(Need need)
        {
            needs.Add(need);
			string sql = String.Format("INSERT INTO `needs` (`nid`, `descript`, `user`, `pay`, `status`, `helper`) " +
                                       "VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", 
                                       need.nid, need.desc, need.user, need.pay, (int)need.status, need.helper);
			MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);

        }

        //接受需求
        public bool AcceptNeed(int nid, String helper)
        {
            if (nid < 0 || nid >= needs.Count)
                return false;
            
            bool success = needs[nid].Accept(helper);

            string sql = String.Format("UPDATE needs SET status = {0} WHERE nid = {1}", 1, nid);
			MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);

            sql = String.Format("UPDATE needs SET helper = '{0}' WHERE nid = {1}", needs[nid].helper, nid);
			MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, sql);

            return success;
        }

        //打印部分需求
        public void PrintNeeds(Status status)
        {
            Need.PrintHeadline();
			for (int i = 0; i < needs.Count; i++)
			{
                if (needs[i].status == status)
				{
					needs[i].Print();
				}
			}
        }

        //读取数据库
        public void LoadPlatform()
        {
			String sql = "SELECT nid, descript, user, pay, status, helper FROM needs";
			DataSet ds = MySqlHelper.GetDataSet(MySqlHelper.Conn, CommandType.Text, sql);

			foreach (DataRow dataRow in ds.Tables[0].Rows)
			{
				int nid = Convert.ToInt32(dataRow["nid"]);
				string desc = Convert.ToString(dataRow["descript"]);
                string user = Convert.ToString(dataRow["user"]);
                int pay = Convert.ToInt32(dataRow["pay"]);
                Status status = (Status)dataRow["status"];
                string helper = Convert.ToString(dataRow["helper"]);

                Need need = new Need(nid, desc, user, pay, status, helper);
                needs.Add(need);
			}

        }
    }

    //系统类
    class CommunityShare
    {
        Platform platform = new Platform();
        public string user;

		//登陆
		public void LogIn()
		{
			Console.Write("请输入登陆昵称：");
            user = Console.ReadLine();

            Console.WriteLine("欢迎登陆，{0}", user);
			Console.Write("输入任意字符进入目录：  ");
			Console.ReadLine();
            Menu();
		}

		//主菜单
		public void Menu()
		{
            Console.WriteLine("昵称： {0}", user);
			Console.WriteLine("*******************请选择操作************************");
			Console.WriteLine("                   1.发布需求");
			Console.WriteLine("                   2.提供服务");
			Console.WriteLine("                   3.查看当前需求");
			Console.WriteLine("                   4.查看已完成交易");
			Console.WriteLine("                   5.更改登陆昵称");
			Console.WriteLine("                   6.退出系统");
			Console.Write("请输入您要选择的操作：  ");

			int input = Convert.ToInt32(Console.ReadLine());
			switch (input)
			{
				case 1:
					PublishNeed();
					break;
				case 2:
					AcceptNeed();
					break;
				case 3:
					ViewPublishedNeeds();
					break;
				case 4:
					ViewAcceptedNeeds();
					break;
				case 5:
                    LogIn();
					break;
				case 6:
					Exit();
					break;
			}
		}

        //发布需求
        public void PublishNeed()
        {
            Console.WriteLine("*发布需求：");
            int nid = platform.needs.Count;
			Console.Write("请输入需求描述：  ");
            string desc = Console.ReadLine();
			Console.Write("请输入需求金额：  ");
			int pay = Convert.ToInt32(Console.ReadLine());

            Need need = new Need(nid, desc, user, pay, Status.Published, "");
            platform.PublishNeed(need);

			Console.WriteLine("发布成功！");
            Need.PrintHeadline();
            need.Print();

            Console.WriteLine("输入任意字符返回目录：  ");
			Console.ReadLine();
			Menu();
        }

        //接受需求
        public void AcceptNeed()
        {
            Console.WriteLine("*接受需求：");
            platform.PrintNeeds(Status.Published);
            Console.WriteLine("请输入需求编号：  ");

			int nid = Convert.ToInt32(Console.ReadLine());

            bool success = platform.AcceptNeed(nid, user);

            if (success)
                Console.Write("接受成功！\n输入任意字符返回目录：  ");
            else
                Console.Write("接受失败！该交易不存在或已完成。\n输入任意字符返回目录：  ");
            
			Console.ReadLine();
			Menu();
        }

        //查看当前需求
        public void ViewPublishedNeeds()
        {
            Console.WriteLine("*查看当前需求：");
            platform.PrintNeeds(Status.Published);

			Console.WriteLine("输入任意字符返回目录：  ");
			Console.ReadLine();
			Menu();
        }

        //查看已完成交易
        public void ViewAcceptedNeeds()
        {
			Console.WriteLine("*查看已完成交易：");
            platform.PrintNeeds(Status.Completed);

			Console.WriteLine("输入任意字符返回目录：  ");
			Console.ReadLine();
			Menu();
        }

        //退出系统
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
            CommunityShare communityShare = new CommunityShare();
            communityShare.LogIn();
        }
    }
}
