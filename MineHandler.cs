using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;

namespace Moonrocks_Sweepin
{
    public class MineHandler
    {
        public bool[,] Cells;

        public bool[,] Init(int? seed = null)
        {
            Cells = new bool[GameConfig.Width, GameConfig.Height];

            int threshold = (int)Math.Floor((double)(GameConfig.Width * GameConfig.Height) / 5);
            Random rand;
            
            if (seed is null) rand = new Random();
            else rand = new Random(seed.Value);
            
            List<(int x, int y)> minesLocation = new List<(int x, int y)>();
            
            int count = 0;

            do
            {
                count++;
                minesLocation.Add((rand.Next(GameConfig.Width), rand.Next(GameConfig.Height)));

            } while (count < threshold);

            for (int y = 0; y < Cells.GetLength(1); y++)
            {
                for (int x = 0; x < Cells.GetLength(0); x++) 
                {
                    Cells[x, y] = false;

                    if (minesLocation.Contains((x, y)))
                    {
                        Cells[x, y] = true;
                    }

                }
            }
            
            return Cells;

        }

        

    }
}