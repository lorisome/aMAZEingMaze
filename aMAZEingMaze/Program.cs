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
            char characterToken = '*';
            Console.Write("Hi! Welcome to the aMAZEing maze! Please select your character (such as *, #, O, b, etc.): ");
            string userInput = Console.ReadLine();
            if(userInput == "")
            {
                Console.WriteLine("Defaulting to * character.");
                Console.ReadLine();
            }
            else
            {
                characterToken = userInput[0];
            }
            if(characterToken == ' ' || characterToken == 'X' || characterToken == '\u2588' || characterToken == '\u0020')
            {
                Console.WriteLine("That character is restricted. Defaulting to * character. Press enter to continue.");
                characterToken = '*';
                Console.ReadLine();
            }

            DataConversion data = new DataConversion();
            List<string> convertedData = data.dataToStrings();
            List<List<bool>> maze = data.MakeMaze(convertedData);


            //------------------------------------random ending point code--------------------------------------//
            //pick a random ending point around the edge of the board that is connected to the path in some way
            //this will assume that the board is a perfect rectangle- all rows must have the same number of columns
            //this assumes that the input maze is at a minimum 3x3, and has at least one square adjacent to the outside wall that is not solid
            //this assumes that all spaces on the path in the maze connect to one another - if there's a  break, the player and exit could be in different sections

            //by default the [1, 22] coordinate of the wall is open in the maze.  this closes it.  if we do nothing else, this will make the game unwinnable.
            maze[22][1] = true;
            //future iterations with actually random mazes will need to remove this, this is just for the single default input we currently have.

            int LastRowSquare = maze[0].Count -1;
            int LastColSquare = maze.Count -1;
            bool successfulExit = false;
            int exitXPosition = 0;
            int exitYPosition = 0;

            Random rng = new Random();
            do
            {
                //select a wall, 0 = top, 1 = bottom, 2 = left, 3 = right
                //then pick a random space along that wall, check if the adjacent square is open
                //if open, break out of the loop, otherwise restart and try again
                int selectedWall = rng.Next(5);

                if(selectedWall < 2)
                {
                    exitXPosition = rng.Next(LastRowSquare + 1);
                    if (selectedWall == 0)
                    {
                        exitYPosition = 0;
                        if (maze[1][exitXPosition] == false)
                        {
                            successfulExit = true;
                        }
                    }
                    else
                    {
                        exitYPosition = LastColSquare;
                        if (maze[LastColSquare - 1][exitXPosition] == false)
                        {
                            successfulExit = true;
                        }
                    }
                }
                else
                {
                    exitYPosition = rng.Next(LastColSquare + 1);
                    if (selectedWall == 2)
                    {
                        exitXPosition = 0;
                        if (maze[exitYPosition][1] == false)
                        {
                            successfulExit = true;
                        }
                    }
                    else
                    {
                        exitXPosition = LastRowSquare;

                        if (maze[exitYPosition][LastRowSquare - 1] == false)
                        {
                            successfulExit = true;
                        }
                    }
                }
            }
            while (successfulExit == false);

            //if converting this to a function, will need to return two ints (possibly as an object)

            //turn the outside square that the exit is on from wall to open
            maze[exitYPosition][exitXPosition] = false;
            
            //--------------------------------end random ending point code--------------------------------------//

            //Make maze from converted file
            Console.Clear();
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
            int[] victoryCoords = new int[] { exitXPosition, exitYPosition }; //modified to have the new exit position

            Console.CursorVisible = false;

            Console.SetCursorPosition(victoryCoords[0], victoryCoords[1]);
            Console.Write("X");
            Console.SetCursorPosition(innerLoop, outerLoop);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(characterToken);
            Console.ResetColor();
            

            Console.SetCursorPosition(innerLoop, outerLoop);

            while (true)
            {
                ConsoleKeyInfo info = Console.ReadKey(true);


                if ((info.Key == ConsoleKey.A ||info.Key == ConsoleKey.LeftArrow) && !maze[outerLoop][innerLoop - 1])
                {

                    Console.Write(mazechar);
                    innerLoop--;

                }
                if ((info.Key == ConsoleKey.W || info.Key == ConsoleKey.UpArrow) && !maze[outerLoop - 1][innerLoop])
                {

                    Console.Write(mazechar);
                    outerLoop--;

                }
                if ((info.Key == ConsoleKey.D || info.Key == ConsoleKey.RightArrow) && !maze[outerLoop][innerLoop + 1])
                {

                    Console.Write(mazechar);
                    innerLoop++;

                }
                if ((info.Key == ConsoleKey.S || info.Key == ConsoleKey.DownArrow) && !maze[outerLoop + 1][innerLoop])
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
                    //this should probably end the program - the player is still "in the maze" at this point, and if they hit enter to submit the readline
                    //and continue in the direction of the exit they will go out of bounds and crash the program (out of range on the position list).
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(characterToken);
                Console.ResetColor();
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
