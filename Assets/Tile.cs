using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private const float generationTime = 0.1f;

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

    public void AnimateNumberGenerated()
    {
        StartCoroutine(NumberGenerated());
    }

    private IEnumerator NumberGenerated()
    {
        float time = 0;
        Vector3 initialScale = new Vector3(0.1f, 0.1f, 0.1f);
        while(time < generationTime)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.one, time / generationTime);
            yield return null;
            time += Time.deltaTime;
        }
        transform.localScale = Vector3.one;
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
