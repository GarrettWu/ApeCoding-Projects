/*
 * 影厅类，记录影厅的编号、行列数和座位数组，实现显示影厅座位预定情况，及预定座位。
 */

package com;

public class Hall {
	//默认行列数
	public static final int DEFAULT_ROWS = 8;
	public static final int DEFAULT_COLS = 8;

	//影厅编号及行列
	public int hallNum;
	public int rows;
	public int cols;

	//座位数组
	public Seat[][] seats;

	//构造函数，生成座位数组
	public Hall(int hallNum, int rows, int cols)
	{
		this.hallNum = hallNum;
		this.rows = rows;
		this.cols = cols;
		seats = new Seat[rows][cols];
		for (int i = 0; i < rows; i++)
		{
			for (int j = 0; j < cols; j++)
			{
				seats[i][j] = new Seat(i, j, false);
			}
		}
	}

	//将座位情况显示在控制台
	public void Display()
	{
		String hallDisplay = "  ";
		for (int j = 0; j < cols; j++)
		{
			hallDisplay += Integer.toString(j);
			hallDisplay += ' ';
		}
		hallDisplay += '\n';

		for (int i = 0; i < rows; i++)
		{
			hallDisplay += Integer.toString(i);
			hallDisplay += ' ';
			for (int j = 0; j < cols; j++)
			{
				hallDisplay += Seat.ToChar(seats[i][j].booked);
				hallDisplay += ' ';
			}
			hallDisplay += '\n';
		}

		System.out.print(hallDisplay);
	}

	//预订某个座位
	public boolean BookSeat(int row, int col)
	{
		if (seats[row][col].booked)
		{
			return false;
		} else {
			seats[row][col].Book();
			return true;
		}
	}
}
