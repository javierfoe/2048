using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Right,
    Left
}

public class TwentyFortyeight
{

    public int Score { get; private set; }
    public int this [int i,int j] => matrix[i,j];

    private readonly int[,] matrix = new int[4, 4];
    private readonly List<int> emptySquares = new List<int>();

    public TwentyFortyeight()
    {
        for(int i = 0; i < 16; i++)
        {
            emptySquares.Add(i);
        }
        GenerateNewNumber();
        GenerateNewNumber();
    }

    public void GenerateNewNumber()
    {
        int twoOrFour = Random.Range(0, 10);
        int newNumber = 4;
        //90% => 2, 10% => 4
        if(twoOrFour > 0)
        {
            newNumber = 2;
        }
        int randomPosition = Random.Range(0, emptySquares.Count);
        int position = emptySquares[randomPosition];
        int xPosition = position / 4;
        int yPosition = position % 4;
        emptySquares.RemoveAt(randomPosition);

        matrix[xPosition, yPosition] = newNumber;
    }

    public void Merge(Direction direction)
    {
        bool reverse = direction == Direction.Down || direction == Direction.Right;
        if(direction == Direction.Up || direction == Direction.Down)
        {
            MergeVertically(reverse);
        }
        else
        {
            MergeHorizontally(reverse);
        }
    }

    private void MergeVertically(bool reverse)
    {

    }

    private void MergeHorizontally(bool reverse)
    {

    }

    public override string ToString()
    {
        string result = "\n";
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                result += matrix[i, j] + " ";
            }
            result += "\n";
        }
        return result;
    }

}
