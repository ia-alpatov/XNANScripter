using System;

namespace XNANScripter.Engine.TextDisplaying
{
    public static class TextWorker
    {
        internal static StyledChar[,] GetEmptyArray(int y, int x)
        {
            StyledChar[,] arr = new StyledChar[y, x];

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    arr[i, j] = new StyledChar();

                    if (j == x - 1)
                    {
                        arr[i, j].C = Environment.NewLine;
                    }
                }
            }

            return arr;
        }
    }
}