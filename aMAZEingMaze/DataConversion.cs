using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMAZEingMaze
{
    public class DataConversion
    {

        public List<string> dataToStrings()
        {

            string directory = Environment.CurrentDirectory;
            string fileName = "data.txt";

            string fullPath = Path.Combine(directory, fileName);


            List<string> maze = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(fullPath))
                {


                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        maze.Add(line);
                    }

                }

                return maze;

            }
            catch (IOException e)
            {
                Console.WriteLine("error");
                throw;
            }

        }
        static public bool wall = true;
        static public bool path = false;

        public List<List<bool>> MakeMaze (List<string> input)
        {
            
            List<List<bool>> maze = new List<List<bool>>();
            
            foreach (string line in input)
            {
                List<bool> row = new List<bool>();
                char[] lineOfChars = line.ToCharArray();
                foreach (char character in lineOfChars)
                {
                    if(character == ' ' || character == 'X')
                    {
                        row.Add(path);
                    }
                    if(character == '#')
                    {
                        row.Add(wall);
                    }
                    
                }
                maze.Add(row);


            }
            return maze;
        }

    }

}




