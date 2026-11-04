using System;

namespace Moonrocks_Sweepin
{
    public class Bitmap
    {
        private ulong[] Field { get; }

        public Bitmap(bool[,] cells) => Field = Bitboardize(cells);

        public static int PopcountMask(ulong[] mask)
        {
            int count = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                ulong current = mask[i];
                
                while (current > 0)
                {
                    current &= (current - 1);
                    count++;
                }
            }

            return count;
        }
        
        public ulong[] Bitboardize(bool[,] cells)
        {
            ulong[] bitboard = new ulong[cells.GetLength(1)];
            
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++) // 0 Width 1 Height
                {
                    if (cells[x, y] == true)
                    {
                        bitboard[y] |= 1UL << (cells.GetLength(0) - x - 1);
                    }
                }
            }

            
            
            //debug
            foreach (long i in bitboard)
            {
                Console.WriteLine(Convert.ToString(i, 2).PadLeft(cells.GetLength(0), '0'));
            }
            
            return bitboard;
        }

        public static ulong MaskOnClick(int x) => 1UL << (GameConfig.Width - x - 1);
        
        public ulong[] AdjacentMask(ulong mask, int y)
        {
            ulong[] adjacent = new ulong[3];
            
            //top
            //for (int i = -1; i <= 1; i++) adjacent |= (mask << GameConfig.Width + i) & Field;
            if (y > 0)
            {
                adjacent[0] |= (mask << 1) & Field[y - 1];
                adjacent[0] |= mask & Field[y - 1];
                adjacent[0] |= (mask >> 1) & Field[y - 1];
            }
            
            adjacent[1] |= (mask << 1) & Field[y];
            adjacent[1] |= (mask >> 1) & Field[y];
            
            //bottom
            //for (int i = -1; i <= 1; i++) adjacent |= (mask >> GameConfig.Width + i) & Field;
            if (y < Field.Length - 1)
            {
                adjacent[2] |= (mask << 1) & Field[y + 1];
                adjacent[2] |= mask & Field[y + 1];
                adjacent[2] |= (mask >> 1) & Field[y + 1];
            }
            
            return adjacent;
        }
        
    }
}