using UnityEngine;

public class RandomData
{
    private static int w, h;
    public static int[,] RandomTestData(int width, int height, int[] choice)
    {
        int[,] data;
        data = new int[width, height];
        for (h = 0; h < height; h++)
        {
            for (w = 0; w < width; w++)
            {
                data[w, h] = (int)Random.Range(0, choice.Length);
                data[w, h] = choice[data[w, h]];
            }
        }
        return data;
    }
}
