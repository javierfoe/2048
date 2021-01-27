using UnityEngine;

public class Test : MonoBehaviour
{

    TwentyFortyeight game = new TwentyFortyeight();
    bool gameOver = false;

    private void Start()
    {
        Debug.Log(game);
    }

    private void Update()
    {
        if (gameOver) return;
        Direction direction = Direction.Up;
        bool move = false;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            move = true;
            direction = Direction.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            move = true;
            direction = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            move = true;
            direction = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            move = true;
            direction = Direction.Down;
        }
        if (move)
        {
            gameOver = game.Move(direction);
            Debug.Log(game);
            if (gameOver)
            {
                Debug.Log("GAME OVER");
            }
        }
    }

}
