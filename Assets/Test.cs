using UnityEngine;

public class Test : MonoBehaviour
{

    TwentyFortyeight game = new TwentyFortyeight();

    private void Start()
    {
        Debug.Log(game);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            game.Merge(Direction.Right);
            Debug.Log(game);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            game.Merge(Direction.Left);
            Debug.Log(game);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            game.Merge(Direction.Up);
            Debug.Log(game);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            game.Merge(Direction.Down);
            Debug.Log(game);
        }
    }

}
