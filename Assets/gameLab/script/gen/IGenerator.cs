using System;

public interface IGenerator
{
	void Generate(int size);
	void GenerateSurface(int size, int seed);

    int[,] data
    {
        get;
    }
}