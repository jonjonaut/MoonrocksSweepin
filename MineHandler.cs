using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;

namespace Moonrocks_Sweepin
{
    public class MineHandler
    {
        private const int Seed = 0;

        public bool?[,] Cells;

        public void Init()
        {
            Cells = new bool?[GameConfig.Width, GameConfig.Height];

            int threshold = (int)Math.Floor((double)(GameConfig.Width * GameConfig.Height) / 2);
            Random rand = new Random(Seed);

            List<(int x, int y)> minesLocation = new List<(int x, int y)>();
            
            int count = 0;

            do
            {
                count++;
                minesLocation.Add((rand.Next(GameConfig.Width), rand.Next(GameConfig.Height)));

            } while (count < threshold);
            
            //debug
            foreach ((int x, int y) mineLocation in minesLocation)
            {
                Console.WriteLine(mineLocation.x + ", " + mineLocation.y);
            }

            for (int i = 0; i < Cells.GetLength(0); i++)
            {
                for (int j = 0; j < Cells.GetLength(1); j++) 
                {
                    Cells[i, j] = null;

                    if (minesLocation.Contains((i, j)))
                    {
                        Console.WriteLine("yes");
                        Cells[i, j] = true;
                    }
                }
            }
            
            Bitmap map = new Bitmap {Field = Bitmap.Bitboardize(Cells)};
            Console.WriteLine(Bitmap.BitPopcount(map.AdjacentMask(Bitmap.MaskOnClick(5, 4))));
            

        }

        

    }
}