//游戏双方在3*3的棋盘里交替下子，任何一方在横竖斜某一方向上率先拥有3子的获胜。若棋盘下满无人获胜，则平局。

using System;

namespace TicTacToe
{
    //表示棋盘中每个格子的状态
	enum Marker
	{
		EMPTY,
        X,
		O
	}

    //棋盘类
    class Board
    {
		public const int SIZE = 3; 
		public const int MIN_POS = 1;
        public const int MAX_POS = SIZE * SIZE;
        public Marker[,] board = new Marker[SIZE,SIZE];

        //把1-9表示的棋盘位置转化为board二维数组的行
		public static int GetRowIndex(int boardPos)
		{
            return (boardPos - 1) / SIZE;
		}

		//把1-9表示的棋盘位置转化为board二维数组的列
		public static int GetColIndex(int boardPos)
		{
			return (boardPos - 1) % SIZE;
		}

        //把格子的状态转化为字符
		public static char ToChar(Marker marker)
		{
			switch (marker)
			{
				case Marker.X:
					return 'X';
				case Marker.O:
					return 'O';
				default:
					return ' ';
			}
		}

        //将棋盘状态转化为字符串输出
		public string GetDisplay()
		{
            string boardString = "-------------\n";
            for (int i = 0; i < SIZE; i++)
			{
                string temp;
                temp = String.Format("| {0} | {1} | {2} |\n",
						ToChar(board[i, 0]),
						ToChar(board[i, 1]),
						ToChar(board[i, 2]));
				boardString += temp;
				boardString += "-------------\n";
			}
			return boardString;

		}

        //在棋盘上落子
		public void Mark(Marker marker, int pos)
		{
			int i = GetRowIndex(pos);
			int j = GetColIndex(pos);
			board[i, j] = marker;
		}

        //判断是否一方获胜
        public bool HasWon(Marker marker)
        {
            bool allEqualM = true;
            //判断列
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    allEqualM &= (board[i, j] == marker);
                }
                if (allEqualM)
                    return true;
            }

            //判断行
            for (int j = 0; j < SIZE; j++)
            {
                allEqualM = true;
                for (int i = 0; i < SIZE; i++)
                {
                    allEqualM &= (board[i, j] == marker);
                }
                if (allEqualM)
                    return true;
            }

            //判断对角线
            allEqualM = true;
            for (int i = 0; i < SIZE; i++)
            {
                allEqualM &= (board[i, i] == marker);
            }
            if (allEqualM)
                return true;

            allEqualM = true;
            for (int i = 0; i < SIZE; i++)
            {
                int j = SIZE - 1 - i;
                allEqualM &= (board[i, j] == marker);
            }
            if (allEqualM)
                return true;

            return false;
        }

        //判断是否平局
		public bool IsTie()
		{
			//若有空格，不是平局
            for (int i = 0; i < SIZE; i++)
                for (int j = 0; j < SIZE; j++)
				{
					if (board[i, j] == Marker.EMPTY)
						return false;
				}
			//若无空格且双方都未获胜，是平局
            return !(HasWon(Marker.X) || HasWon(Marker.O));
		}
	}

    class User
    {
        public String username;
        public int score;

        public void SaveScore()
        {
            
        }

        public void LoadScore()
        {
            
        }


    }

    //游戏类
    class TicTacToe
    {
        //main函数
		public static void Main(string[] args)
		{
            //初始化棋盘并输出帮助指导
            Board board = new Board();
            Marker turn = Marker.X;

            Console.WriteLine("如下图所示输入1-9，选择位置落子。");
			Console.WriteLine("-------------");
            for (int boardPos = Board.MIN_POS; boardPos <= Board.MAX_POS; boardPos += Board.SIZE)
			{
                Console.WriteLine("| {0} | {1} | {2} |", boardPos, boardPos + 1, boardPos + 2);
				Console.WriteLine("-------------");
			}
            Console.WriteLine();

            //双方交替落子直到游戏结束
            while (!board.IsTie() && !board.HasWon(Marker.X) && !board.HasWon(Marker.O))
            {
                bool validInput = false;
                int boardPos = 0;

                //等候有效输入
                while (!validInput)
                {
                    Console.WriteLine("玩家{0}, 请输入想要落子的位置：", Board.ToChar(turn));
                    boardPos = Convert.ToInt32(Console.ReadLine());
                    if (boardPos < Board.MIN_POS || boardPos > Board.MAX_POS ||
                        board.board[Board.GetRowIndex(boardPos), Board.GetColIndex(boardPos)] != Marker.EMPTY)
                    {
                        continue;
                    }
                    validInput = true;
                }

                //落子并显示棋盘状态
                board.Mark(turn, boardPos);

                string boardDisplay = board.GetDisplay();
                Console.WriteLine(boardDisplay);

                //转换另一方
                switch (turn)
                {
                    case Marker.X:
                        turn = Marker.O;
                        break;
                    case Marker.O:
                        turn = Marker.X;
                        break;
                }
            }

			//游戏结束，输出结果
            if (board.IsTie())
			{
                Console.WriteLine("平局，游戏结束。");
			}
            else if (board.HasWon(Marker.X))
			{
                Console.WriteLine("玩家X获胜！");
			}
			else
			{
                Console.WriteLine("玩家O获胜！");
			}

		}
    }
}
