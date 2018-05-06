/*
 * main类，运行main函数，实现菜单操作。
 */

package com;
import java.util.GregorianCalendar;
import java.util.Scanner;;

public class CinemaTicket {
	//main函数
	public static void main(String[] args)
	{
		//实例化订票系统
		TicketSystem ticketSystem = new TicketSystem();

		//加入电影
		Movie beforeSunrise = new Movie("Before Sunrise", new GregorianCalendar(2017, 5, 1, 15, 0, 0), 1);
		Movie beforeSunset = new Movie("Before Sunset", new GregorianCalendar(2017, 5, 1, 19, 15, 0), 5);
		Movie beforeMidnight = new Movie("Before Midnight", new GregorianCalendar(2017, 5, 2, 20, 30, 0), 8);

		ticketSystem.AddMovie(beforeSunrise);
		ticketSystem.AddMovie(beforeSunset);
		ticketSystem.AddMovie(beforeMidnight);

		//初始化输入变量
		int movieNum = -1;
		int rowNum = -1;
		int colNum = -1;
		int inputStatus = 0; //0选择电影，1选择行，2选择列，3预定成功，4退出
		
		String temp;
		Scanner input = new Scanner(System.in);

		//使用状态机控制输入
		while (inputStatus != 4)
		{
			switch (inputStatus)
			{
			
			//选择电影界面，显示电影列表，输入数字选择电影
			case 0:
				System.out.println("请输入电影序号，输入q退出：");
				for (int i = 0; i < ticketSystem.movies.size(); i++)
				{
					System.out.printf("%d: ", i);
					ticketSystem.movies.get(i).Display();
				}
				temp = input.next();
				if (temp.equals("q")){
					inputStatus = 4;
					break;
				}
				movieNum = Integer.parseInt(temp);
				if (movieNum >= 0 || movieNum < ticketSystem.movies.size())
					inputStatus = 1;
				break;

			//选择座位界面，输入座位的横排序号选择
			case 1:
				ticketSystem.DisplayHall(movieNum);
				System.out.println("请输入座位横排序号，输入q重新选择电影：");
				temp = input.next();
				if (temp.equals("q"))
				{
					inputStatus = 0;
					break;
				}
				rowNum = Integer.parseInt(temp);
				if (rowNum >= 0 || rowNum < ticketSystem.movies.get(movieNum).hall.rows)
					inputStatus = 2;
				break;

			//选择座位界面，输入座位的竖排序号选择
			case 2:
				System.out.println("请输入座位竖排序号，输入q重新选择电影：");
				temp = input.next();
				if (temp.equals("q"))
				{
					inputStatus = 0;
					break;
				}
				colNum = Integer.parseInt(temp);
				if (colNum >= 0 || colNum < ticketSystem.movies.get(movieNum).hall.cols)
					inputStatus = 3;
				break;

			//预定结果界面，显示结果
			case 3:
				if (ticketSystem.BookSeat(movieNum, rowNum, colNum))
					System.out.println("预订成功！输入任意字符继续预定，输入q退出：");
				else
					System.out.println("预订失败！输入任意字符重新预定。输入q退出：");
				temp = input.next();
				if (temp.equals("q"))
					inputStatus = 4;
				else
					inputStatus = 0;
				break;

			}
		}
		
		input.close();
	}
}
