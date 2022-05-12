namespace MichaelSort
{
    public static class Helper
    {

        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Count();
            while (n > 1)
            {
                n--;
                int i = Random.Shared.Next(n + 1);
                (array[i], array[n]) = (array[n], array[i]);
            }
        }
    }
}