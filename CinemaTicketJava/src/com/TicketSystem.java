/*
 * 订票系统类，记录电影List，实现预定座位及显示电影预订情况
 */

package com;
import java.util.ArrayList;

public class TicketSystem {
	//所有电影
    public ArrayList<Movie> movies = new ArrayList<Movie>();

    //增加电影
	public boolean AddMovie(Movie movie)
	{
		movies.add(movie);
		return true;
	}

    //预订座位
    public boolean BookSeat(int movieNum, int row, int col)
    {
        return movies.get(movieNum).hall.BookSeat(row, col);
    }

    //显示影厅预订情况
    public void DisplayHall(int movieNum)
    {
    	movies.get(movieNum).hall.Display();
    }
}
