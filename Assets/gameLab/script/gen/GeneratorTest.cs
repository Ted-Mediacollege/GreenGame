using UnityEngine;
using System.Collections;

public class GeneratorTest : IGenerator {
    private int[,] privateData;

    public int[,] data
    {
        get
        {
            return privateData;
        }
    }

	public void Generate(int size) {
		int seed = (int)Random.Range(-10000,10000);
		GenerateSurface(size, seed);
	}

	public void GenerateSurface(int size, int seed) {
		int[,] tiles;
		tiles = new int[size,size];
		
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				int t = (int) (Mathf.PerlinNoise((float)i / 7f + (float)seed, (float)j / 7f + (float)seed) * 5) - 1;
				t = t < 0 ? 0 : t > 2 ? 2 : t;
				tiles [i, j] = t;
			}
		}

        privateData = tiles;
	}
}
