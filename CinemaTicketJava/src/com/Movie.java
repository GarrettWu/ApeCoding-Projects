/*
 * 电影类，保存电影名称、日期时间、所用影厅，实现显示电影信息
 */

package com;
import java.util.GregorianCalendar;;

public class Movie {
	//电影名称、日期时间、大厅
    public String name;
    public GregorianCalendar showTime;
    public Hall hall;

    //构造函数
    public Movie(String name, GregorianCalendar showTime, int hallNum)
    {
        this.name = name;
        this.showTime = showTime;
        this.hall = new Hall(hallNum, Hall.DEFAULT_ROWS, Hall.DEFAULT_COLS);
    }

    //控制台显示电影信息
    public void Display()
    {
        System.out.printf("%s 时间：%tc 影厅：%d%n", name, showTime, hall.hallNum);
    }
}
