using UnityEngine;

public static class Controller
{
    private static TwentyFortyeight game;

    public static int Score => game.Score;

    public static void Restart()
    {
        game = new TwentyFortyeight();
        game.AddMergeNumberListener((int first, int second) => Debug.Log(string.Format("Merge {0} into {1}", first, second)));
        game.AddMoveNumberListener((int first, int second) => Debug.Log(string.Format("Move {0} into {1}", first, second)));
        game.AddNumberGeneratorListener((int first, int second) => Debug.Log(string.Format("Generate {0} on {1}", first, second)));
        game.StartingNumbers();
    }

    public static void Slide(Direction direction)
    {
        game.Move(direction);
    }
}