using System;
using System.Collections.Generic;

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
    public int this[int i, int j] => matrix[i, j];

    private readonly int[,] matrix = new int[4, 4];
    private readonly List<int> emptySquares = new List<int>();
    private readonly static Random random = new Random();

    public TwentyFortyeight()
    {
        for (int i = 0; i < 16; i++)
        {
            emptySquares.Add(i);
        }
        GenerateNewNumber();
        GenerateNewNumber();
    }

    public void GenerateNewNumber()
    {
        int twoOrFour = random.Next(0, 10);
        int newNumber = 4;
        //90% => 2, 10% => 4
        if (twoOrFour > 0)
        {
            newNumber = 2;
        }
        int randomPosition = random.Next(0, emptySquares.Count);
        int position = emptySquares[randomPosition];
        int xPosition = position / 4;
        int yPosition = position % 4;
        emptySquares.RemoveAt(randomPosition);

        matrix[xPosition, yPosition] = newNumber;
    }

    public void Merge(Direction direction)
    {
        bool reverse = direction == Direction.Down || direction == Direction.Right;
        bool movement;
        if (direction == Direction.Up || direction == Direction.Down)
        {
            movement = MergeVertically(reverse);
        }
        else
        {
            movement = MergeHorizontally(reverse);
        }
        if (movement)
        {
            GenerateNewNumber();
        }
    }

    private bool MergeHorizontally(bool reverse)
    {
        bool movement = false;
        int start = reverse ? 3 : 0;
        int interval = reverse ? -1 : 1;

        int freeSpot;
        //Merging 4 rows
        for (int i = 0; i < 4; i++)
        {
            freeSpot = start;
            //Moving individual cells
            for (int j = start; j < 4 && j > -1 && freeSpot < 4 && freeSpot > -1; j += interval)
            {
                if (matrix[i, j] == 0) continue;
                //Empty square to move to
                if (matrix[i, freeSpot] == 0)
                {
                    //Move the value
                    RemoveFreeSpace(i, freeSpot, matrix[i, j]);
                    AddFreeSpace(i, j);
                    movement = true;
                }
                //Merging
                else if (matrix[i, j] == matrix[i, freeSpot] && j != freeSpot)
                {
                    int merge = matrix[i, freeSpot] * 2;
                    matrix[i, freeSpot] = merge;
                    Score += merge;
                    freeSpot += interval;
                    AddFreeSpace(i, j);
                    movement = true;
                }
                //Slide the value to the next empty cell
                else if (matrix[i, j] != matrix[i, freeSpot])
                {
                    freeSpot += interval;
                    if (freeSpot != j)
                    {
                        RemoveFreeSpace(i, freeSpot, matrix[i, j]);
                        AddFreeSpace(i, j);
                        movement = true;
                    }
                }
            }
        }
        return movement;
    }

    private bool MergeVertically(bool reverse)
    {
        bool movement = false;
        int start = reverse ? 3 : 0;
        int interval = reverse ? -1 : 1;

        int freeSpot;
        //Merging 4 rows
        for (int j = 0; j < 4; j++)
        {
            freeSpot = start;
            //Moving individual cells
            for (int i = start; i < 4 && i > -1 && freeSpot < 4 && freeSpot > -1; i += interval)
            {
                if (matrix[i, j] == 0) continue;
                //Empty square to move to
                if (matrix[freeSpot, j] == 0)
                {
                    //Move the value
                    RemoveFreeSpace(freeSpot, j, matrix[i, j]);
                    AddFreeSpace(i, j);
                    movement = true;
                }
                //Merging
                else if (matrix[i, j] == matrix[freeSpot, j] && i != freeSpot)
                {
                    int merge = matrix[freeSpot, j] * 2;
                    matrix[freeSpot, j] = merge;
                    Score += merge;
                    freeSpot += interval;
                    AddFreeSpace(i, j);
                    movement = true;
                }
                //Slide the value to the next empty cell
                else if (matrix[i, j] != matrix[freeSpot, j])
                {
                    freeSpot += interval;
                    if (freeSpot != i)
                    {
                        RemoveFreeSpace(freeSpot, j, matrix[i, j]);
                        AddFreeSpace(i, j);
                        movement = true;
                    }
                }
            }
        }
        return movement;
    }

    private void AddFreeSpace(int i, int j)
    {
        matrix[i, j] = 0;
        int position = From2Dto1D(i, j);
        if (!emptySquares.Contains(position))
        {
            emptySquares.Add(position);
        }
    }

    private void RemoveFreeSpace(int i, int j, int value)
    {
        matrix[i, j] = value;
        int position = From2Dto1D(i, j);
        if (emptySquares.Contains(position))
        {
            emptySquares.Remove(position);
        }
    }

    private int From2Dto1D(int i, int j)
    {
        return i * 4 + j;
    }

    public override string ToString()
    {
        string result = "Score: " + Score + "\tEmpty squares: " + emptySquares.Count + "\n";
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                result += matrix[i, j] + "\t";
            }
            result += "\n";
        }
        return result;
    }

}
