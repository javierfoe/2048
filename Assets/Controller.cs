using UnityEngine;
using UnityEngine.Events;

public static class Controller
{
    private static TwentyFortyeight game;

    public static int Score => game.Score;

    public static void SetListeners(UnityAction<int, int> move, UnityAction<int, int> merge, UnityAction<int, int> generate)
    {
        game = new TwentyFortyeight();
        game.AddMoveNumberListener(move);
        game.AddMergeNumberListener(merge);
        game.AddNumberGeneratorListener(generate);
        game.StartingNumbers();
    }

    public static void Slide(Direction direction)
    {
        game.Move(direction);
    }
}