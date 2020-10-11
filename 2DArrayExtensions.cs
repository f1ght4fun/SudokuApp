namespace ExtensionMethods
{
    public static class ArrayExtensions
    {
        public static T[][] Slice<T>(this T[][] a, int x1, int y1, int size)
        {
            var result = new T[size][];
            for (var i = x1; i < x1 + size; i++)
            {
                result[i - x1] = new T[size];

                for (var j = y1; j < y1 + size; j++)
                {
                    result[i - x1][j - y1] = a[i][j];
                }
            }
            return result;
        }
    }
}