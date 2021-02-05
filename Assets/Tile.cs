using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int Value { get { return value; } set { SetValue(value); } }

    private Text text;
    private Image image;
    private int value;

    public void Empty()
    {
        Value = 0;
    }

    public void Double()
    {
        Value *= 2;
    }

    private void SetValue(int value)
    {
        this.value = value;
        bool enabled = value > 0;
        image.enabled = enabled;
        text.enabled = enabled;
        text.text = value.ToString();
    }

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
    }
}
