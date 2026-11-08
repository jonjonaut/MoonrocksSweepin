using System;
using System.Collections;

namespace Moonrocks_Sweepin
{
    public class Bitmap
    {
        public ulong[] EmptyField { get; set; } 
        
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
                    if (cells[x, y])
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

        public void MaskEmpty(int[,] popcount)
        {
            ulong[] mask = new ulong[popcount.GetLength(1)];

            for (int x = 0; x < popcount.GetLength(0); x++)
            {
                for (int y = 0; y < popcount.GetLength(1); y++)
                {
                    if (popcount[x, y] == 0)
                    {
                        mask[y] |= 1UL << (popcount.GetLength(0) - x);
                    }
                }
            }
            
            EmptyField = mask;
        }
        
        public static ulong MaskOnClick(int x) => 1UL << (GameConfig.Width - x - 1);

        public ulong[] EmptyMask(int x, int y)
        {
            ulong[] empty = new ulong[Field.GetLength(1)];
            ulong click = MaskOnClick(x);

            Queue queue = new Queue();
            
            empty[y] |= AdjacentMask(click, y, true);

            return empty;
        }

        public ulong[] AdjacentMask(ulong mask, int y, bool findEmpty = false)
        {
            ulong[] adjacent = new ulong[3];
            ulong[] option;

            if (findEmpty) option = EmptyField;
            else option = Field;
            
            //top
            if (y > 0)
            {
                adjacent[0] |= (mask << 1) & option[y - 1];
                adjacent[0] |= mask & option[y - 1];
                adjacent[0] |= (mask >> 1) & option[y - 1];
            }
            
            adjacent[1] |= (mask << 1) & option[y];
            adjacent[1] |= (mask >> 1) & option[y];
            
            //bottom
            if (y < option.Length - 1)
            {
                adjacent[2] |= (mask << 1) & option[y + 1];
                adjacent[2] |= mask & option[y + 1];
                adjacent[2] |= (mask >> 1) & option[y + 1];
            }
            
            return adjacent;
        }
        
    }
}