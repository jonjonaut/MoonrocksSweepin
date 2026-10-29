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
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j] == true)
                    {
                        int shift = i * cells.GetLength(0) + j;
                        
                        bitboard |= (ulong)1 << shift;
                    }
                }
            }
            
            return bitboard;
        }

        public static ulong MaskOnClick(int x, int y) => 1UL << x * y;
        
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