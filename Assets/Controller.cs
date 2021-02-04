public static class Controller
{
    private static TwentyFortyeight game;

    public static int Score => game.Score;

    public static void Restart()
    {
        game = new TwentyFortyeight();
    }

    public static void Slide(Direction direction)
    {
        game.Move(direction);
    }
}