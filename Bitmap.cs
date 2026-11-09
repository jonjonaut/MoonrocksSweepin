using System;
using System.Collections;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;

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
            /*foreach (long i in bitboard)
            {
                Console.WriteLine(Convert.ToString(i, 2).PadLeft(cells.GetLength(0), '0'));
            }*/
            
            return bitboard;
        }

        public bool[,] ReverseBitboardize(ulong[] mask)
        {
            bool[,] rev = new bool[GameConfig.Width, GameConfig.Height];

            for (int x = 0; x < rev.GetLength(0); x++)
            {
                for (int y = 0; y < rev.GetLength(1); y++)
                {
                    rev[x, y] = ((mask[y] >> (rev.GetLength(0) - x - 1)) & 1UL) != 0;
                }
            }
            
            return rev;
        }

        public void MaskEmpty(int[,] popcount, bool[,] bombs)
        {
            ulong[] mask = new ulong[popcount.GetLength(1)];

            for (int x = 0; x < popcount.GetLength(0); x++)
            {
                for (int y = 0; y < popcount.GetLength(1); y++)
                {
                    if (popcount[x, y] == 0 && !bombs[x, y])
                    {
                        mask[y] |= 1UL << (popcount.GetLength(0) - x - 1);
                    }
                }
            }
            
            EmptyField = mask;
        }
        
        public static ulong MaskOnClick(int x) => 1UL << (GameConfig.Width - x - 1);

        public ulong[] EmptyMask(int startC, int startR)
        {
            ulong[] empty = new ulong[GameConfig.Height];
            empty[startR] |= MaskOnClick(startC);
            
            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((startC, startR));
            
            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();

                foreach (var coords in AdjacentCoords(MaskOnClick(x), y))
                {
                    int nx = x + coords.Item1;
                    int ny = y + coords.Item2;
                    
                    queue.Enqueue((nx, ny));
                    empty[ny] |= MaskOnClick(nx);
                }
            }
            
            return empty;
        }

        public ulong[] BorderMask(ulong[] mask)
        {
            ulong[] bordered = mask.ToArray();
            
            for (int i = 0; i < mask.Length; i++)
            {
                if (bordered[i] == 0) continue;

                bordered[i] |= mask[i] >> 1;
                bordered[i] |= mask[i] << 1;
                if (i > 0)
                {
                    bordered[i - 1] |= mask[i];
                    bordered[i - 1] |= mask[i] << 1;
                    bordered[i - 1] |= mask[i] >> 1;
                }
                if (i < mask.Length - 1)
                {
                    bordered[i + 1] |= mask[i];
                    bordered[i + 1] |= mask[i] << 1;
                    bordered[i + 1] |= mask[i] >> 1;
                }

            }

            return bordered;
        }
        
        public List<(int, int)> AdjacentCoords(ulong mask, int y)
        {
            List<(ulong, int, int)> list = new List<(ulong, int, int)>();

            EmptyField[y] &= ~mask;
            
            if (y > 0)
            {
                list.AddRange(new[]
                {
                    ((mask << 1) & EmptyField[y - 1], -1 , -1),
                    (mask & EmptyField[y - 1], 0, -1),
                    ((mask >> 1) & EmptyField[y - 1], 1 , -1)
                });
            }

            if (y < EmptyField.Length - 1)
            {
                list.AddRange(new[] {
                    ((mask << 1) & EmptyField[y + 1], -1, 1),
                    (mask & EmptyField[y + 1], 0 , 1),
                    ((mask >> 1) & EmptyField[y + 1], 1 , 1)
                });
            }
            
            list.AddRange(new[]
            {
                ((mask << 1) & EmptyField[y], -1 , 0),
                ((mask >> 1) & EmptyField[y], 1 , 0)
            });
            
            List<(int, int)> coords = new List<(int, int)>();
            
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Item1 != 0)
                {
                    EmptyField[y + list[i].Item3] &= ~list[i].Item1; 
                    coords.Add((list[i].Item2, list[i].Item3));
                } 
            }

            return coords;
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