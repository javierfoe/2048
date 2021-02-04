using System;
using System.Collections.Generic;
using UnityEngine.Events;

public enum Direction
{
    Up,
    Down,
    Right,
    Left
}

public class TwentyFortyeight
{
    private readonly static Random random = new Random();

    public int Score { get; private set; }

    //1st int -> int generated, 2nd int -> destination
    private readonly UnityEvent<int, int> numberGenerator = new UnityEventIntInt();
    //1st int -> origin, 2nd int -> destination
    private readonly UnityEvent<int, int> moveNumber = new UnityEventIntInt();
    //1st int -> origin, 2nd int -> destination
    private readonly UnityEvent<int, int> mergeNumber = new UnityEventIntInt();

    private readonly int[,] matrix = new int[4, 4];
    private readonly List<int> emptySquares = new List<int>();

    public void AddNumberGeneratorListener(UnityAction<int, int> action)
    {
        numberGenerator.AddListener(action);
    }

    public void AddMoveNumberListener(UnityAction<int, int> action)
    {
        moveNumber.AddListener(action);
    }

    public void AddMergeNumberListener(UnityAction<int, int> action)
    {
        mergeNumber.AddListener(action);
    }

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

        numberGenerator.Invoke(newNumber, xPosition * 4 + yPosition);
    }

    public bool Move(Direction direction)
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
        return CheckGameOver();
    }

    private bool MergeHorizontally(bool reverse)
    {
        bool movement = false;
        int start = reverse ? 3 : 0;
        int interval = reverse ? -1 : 1;
        int freeSpot;

        for (int i = 0; i < 4; i++)
        {
            freeSpot = start;
            for (int j = start; j < 4 && j > -1 && freeSpot < 4 && freeSpot > -1; j += interval)
            {
                if (matrix[i, j] == 0) continue;
                movement = Merge(i, j, interval, ref freeSpot, false, movement);
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

        for (int j = 0; j < 4; j++)
        {
            freeSpot = start;
            for (int i = start; i < 4 && i > -1 && freeSpot < 4 && freeSpot > -1; i += interval)
            {
                if (matrix[i, j] == 0) continue;
                movement = Merge(i, j, interval, ref freeSpot, true, movement);
            }
        }
        return movement;
    }

    private bool Merge(int i, int j, int interval, ref int freeSpot, bool vertical, bool movement)
    {
        bool newMovement = false;
        int destination = vertical ? freeSpot * 4 + j : i * 4 + freeSpot;
        int origin = i * 4 + j;
        //Empty square to move to
        if (vertical && matrix[freeSpot, j] == 0 || !vertical && matrix[i, freeSpot] == 0)
        {
            //Move the value
            if (vertical)
            {
                RemoveFreeSpace(freeSpot, j, matrix[i, j]);
            }
            else
            {
                RemoveFreeSpace(i, freeSpot, matrix[i, j]);
            }
            AddFreeSpace(i, j);
            newMovement = true;
            moveNumber.Invoke(origin, destination);
        }
        //Merging
        else if (vertical && matrix[i, j] == matrix[freeSpot, j] && i != freeSpot || !vertical && matrix[i, j] == matrix[i, freeSpot] && j != freeSpot)
        {
            int merge = 2 * (vertical ? matrix[freeSpot, j] : matrix[i, freeSpot]);
            if (vertical)
            {
                matrix[freeSpot, j] = merge;
            }
            else
            {
                matrix[i, freeSpot] = merge;
            }
            Score += merge;
            freeSpot += interval;
            AddFreeSpace(i, j);
            newMovement = true;
            mergeNumber.Invoke(origin, destination);
        }
        //Slide the value to the next empty cell
        else if (vertical && matrix[i, j] != matrix[freeSpot, j] || !vertical && matrix[i, j] != matrix[i, freeSpot])
        {
            freeSpot += interval;
            if (vertical && freeSpot != i)
            {
                RemoveFreeSpace(freeSpot, j, matrix[i, j]);
                newMovement = true;
            }
            else if (!vertical && freeSpot != j)
            {
                RemoveFreeSpace(i, freeSpot, matrix[i, j]);
                newMovement = true;
            }
            if (newMovement)
            {
                destination += interval * (vertical ? 4 : 1);
                moveNumber.Invoke(origin, destination);
                AddFreeSpace(i, j);
            }
        }
        return newMovement || movement;
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

    private bool CheckGameOver()
    {
        if (emptySquares.Count > 0) return false;
        bool merging = false;
        for (int i = 0; i < 15 && !merging; i++)
        {
            //Compare with the right value
            merging |= Compare(i, i + 1);
            //Compare with the bottom value
            merging |= Compare(i, i + 4);
        }
        return !merging;
    }

    private bool Compare(int origin, int destination)
    {
        if (destination > 15 || destination % 4 == 0 && origin % 4 != 0) return false;
        int xOri = origin / 4;
        int yOri = origin % 4;
        int xDest = destination / 4;
        int yDest = destination % 4;
        return matrix[xOri, yOri] == matrix[xDest, yDest];
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

    private class UnityEventIntInt : UnityEvent<int, int> { }

}
