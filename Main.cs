using System;


namespace Main
{
    class Program
    {
        static int Level = 1;
        static int TetrisRows = 20;
        static int TetrisCols = 10;

        static int Score = 0;

        private char Determine(int rliczba)
        {
            if (rliczba % 7 == 0)
            {
                return 'Z';
            }
            if (rliczba % 7 == 1)
            {
                return 'R';
            }
            if (rliczba % 7 == 2)
            {
                return 'L';
            }
            if (rliczba % 7 == 3)
            {
                return 'T';
            }
            if (rliczba % 7 == 4)
            {
                return 'S';
            }
            if (rliczba % 7 == 5)
            {
                return 'I';
            }
            if (rliczba % 7 == 6)
            {
                return 'B';
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Tetris";

    	}
	static void DrawBorder()
	{
		Console.SetCursorPosition(0, 0);
		string line = "╔";
		line += new string('═', TetrisCols);


		for (int i = 0; i < TetrisRows; i++)
		{
			string middleLine = "║";
			middleLine += new string(' ', TetrisCols);
			middleLine += "║";
			Console.Write(middleLine);
		}

		string endLine = "╚";
		endLine += new string('═', TetrisCols);
		endLine += "╝";
		Console.Write(endLine);
		}
	}
