﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class View : MonoBehaviour
{
    public static bool OnGoingAnimation = false;
    private const float AnimationTime = 0.1f;

    private Tile[] squares;
#if UNITY_ANDROID
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
#endif

    public void Slide(Direction direction)
    {
        Controller.Slide(direction);
    }

    private void GenerateNumber(int value, int position)
    {
        OnGoingAnimation = true;
        squares[position].Value = value;
        squares[position].AnimateNumberGenerated(AnimationTime);
    }

    private void MoveNumber(int origin, int destination)
    {
        OnGoingAnimation = true;
        StartCoroutine(MoveAnimation(origin, destination));
    }

    private void MergeNumber(int origin, int destination)
    {
        OnGoingAnimation = true;
        StartCoroutine(MoveAnimation(origin, destination, 0.8f));
    }

    private IEnumerator MoveAnimation(int origin, int destination, float moveTimePercentage = 1f)
    {
        int oldValue = squares[origin].Value;
        int originX = origin / 4;
        int originY = origin % 4;
        int destinationX = destination / 4;
        int destinationY = destination % 4;
        if (moveTimePercentage < 1)
        {
            StartCoroutine(squares[destination].MergeAnimation(AnimationTime, moveTimePercentage));
        }
        if (originX == destinationX)
        {
            yield return squares[origin].MoveHorizontally(destinationY - originY, AnimationTime);
        }
        else if (originY == destinationY)
        {
            yield return squares[origin].MoveVertically(destinationX - originX, AnimationTime);
        }
        squares[destination].Value = oldValue;
    }

    private void Start()
    {
        squares = GetComponentsInChildren<Tile>();
#if UNITY_ANDROID
        dragDistance = Screen.width * 10 / 100;
#endif
        Controller.SetListeners(MoveNumber, MergeNumber, GenerateNumber);
    }

    private void Update()
    {
        if (OnGoingAnimation) return;
#if !UNITY_ANDROID
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Slide(Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Slide(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Slide(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide(Direction.Down);
        }
#else
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            Slide(Direction.Right);
                            Debug.Log("Right Swipe");
                        }
                        else
                        {   //Left swipe
                            Slide(Direction.Left);
                            Debug.Log("Left Swipe");
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            Slide(Direction.Up);
                            Debug.Log("Up Swipe");
                        }
                        else
                        {   //Down swipe
                            Slide(Direction.Down);
                            Debug.Log("Down Swipe");
                        }
                    }
                    UpdateValues();
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
#endif
    }

    private class UnityEventDirection : UnityEvent<Direction> { }
}
