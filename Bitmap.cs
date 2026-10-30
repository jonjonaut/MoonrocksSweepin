using System;

namespace Moonrocks_Sweepin
{
    public class Bitmap
    {
        public ulong Field { get; set; }
        
        public static int BitPopcount(ulong mask)
        {
            int count = 0;
            while (mask != 0)
            {
                mask &= mask - 1;
                count++;
            }

            return count;
        }
        
        public static ulong Bitboardize(bool?[,] cells)
        {
            ulong bitboard = 0x0;
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++) // 0 Width 1 Height
                {
                    if (cells[x, y] == true)
                    {
                        int shift = y * cells.GetLength(0) + x;
                        
                        bitboard |= (ulong)1 << shift;
                    }
                }
            }
            
            Console.WriteLine(Convert.ToString((long)bitboard, 2).PadLeft(64, '0'));
            
            return bitboard;
        }

        public static ulong MaskOnClick(int x, int y) => 1UL << y  * GameConfig.Width + x;
        
        public ulong AdjacentMask(ulong mask)
        {
            ulong adjacent = 0x0;
            
            adjacent |= (mask << 1) & Field;
            adjacent |= (mask >> 1) & Field;
            
            //top
            for (int i = -1; i <= 1; i++) adjacent |= (mask << GameConfig.Width + i) & Field;
            
            //bottom
            for (int i = -1; i <= 1; i++) adjacent |= (mask >> GameConfig.Width + i) & Field;
            
            return adjacent;
        }
        
    }
}