/* 
 * 座位类，包含座位的位置信息，及座位是否已被预定
 */

package com;

public class Seat {
	//座位位置及是否已预定
    public int row, col;
    public boolean booked = false;

    //构造函数
    public Seat(int row, int col, boolean booked)
    {
        this.row = row;
        this.col = col;
        this.booked = booked;
    }

    //预定函数
    public void Book()
    {
        booked = true;
    }

    //将座位预定信息转化为字符
    public static char ToChar(boolean booked)
    {
        return booked ? 'X' : 'O';
    }
}
