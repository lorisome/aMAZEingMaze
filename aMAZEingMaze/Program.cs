using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMAZEingMaze
{
    class Program
    {
        private const char wallchar = '\u2588';
        private const char mazechar = '\u0020';
        static bool path = false;
        static bool wall = true;

        static void Main(string[] args)
        {


            DataConversion data = new DataConversion();
            List<string> convertedData = data.dataToStrings();
            List<List<bool>> maze = data.MakeMaze(convertedData);


            //Make maze from converted file
            for (int i = 0; i < maze.Count; i++)
            {
                for (int j = 0; j < maze[i].Count; j++)
                {
                    Console.Write(maze[i][j] ? wallchar : mazechar);
                }
                Console.Write("\n");
            }

            //pick a random starting point along the path
            List<List<int>> completePath = new List<List<int>>();
            completePath = GetAllPathSquares(maze);
            Random rnd = new Random();
            int randomStart = rnd.Next(completePath.Count);

            int outerLoop = completePath[randomStart][0];
            int innerLoop = completePath[randomStart][1];
            int[] victoryCoords = new int[] { 1, 22 };

            Console.CursorVisible = false;

            Console.SetCursorPosition(victoryCoords[0], victoryCoords[1]);
            Console.Write("X");
            Console.SetCursorPosition(innerLoop, outerLoop);
            Console.Write("*");

            Console.SetCursorPosition(innerLoop, outerLoop);

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);


                if (info.Key == ConsoleKey.A && !maze[outerLoop][innerLoop - 1])
                {

                    Console.Write(mazechar);
                    innerLoop--;

                }
                if (info.Key == ConsoleKey.W && !maze[outerLoop - 1][innerLoop])
                {

                    Console.Write(mazechar);
                    outerLoop--;

                }
                if (info.Key == ConsoleKey.D && !maze[outerLoop][innerLoop + 1])
                {

                    Console.Write(mazechar);
                    innerLoop++;

                }
                if (info.Key == ConsoleKey.S && !maze[outerLoop + 1][innerLoop])
                {

                    Console.Write(mazechar);
                    outerLoop++;

                }


                Console.SetCursorPosition(innerLoop, outerLoop);
                int[] currentPosition = new int[] { innerLoop, outerLoop };
                if (victoryCoords.SequenceEqual(currentPosition))
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Great Job!");
                    Console.ReadLine();
                }
                Console.Write("*");
                Console.SetCursorPosition(innerLoop, outerLoop);
            }
        }

        static public List<List<int>> GetAllPathSquares(List<List<bool>> maze)
        {
            List<List<int>> completePath = new List<List<int>>();
            List<int> currentRow = new List<int>();
            int rowCount = 0;
            int columnCount = 0;

            foreach (List<bool> row in maze)
            {
                columnCount = 0;

                foreach (bool square in row)
                {
                    if (!square)
                    {
                        currentRow.Add(rowCount);
                        currentRow.Add(columnCount);
                        completePath.Add(currentRow);
                        currentRow = new List<int>();
                    }
                    columnCount++;
                }
                rowCount++;
            }
            return completePath;

            //int[,] output = new int[rowCount, columnCount] { };
            //foreach  (List<int> square in completePath)
            //{

            //}

        }


    }




}
