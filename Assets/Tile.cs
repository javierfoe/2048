using System.Collections;
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

    public void AnimateNumberGenerated(float maxTime)
    {
        StartCoroutine(NumberGenerated(maxTime));
    }

    public IEnumerator MoveHorizontally(int movement, float maxTime)
    {
        Tile animation = Instantiate(this, transform.parent);
        animation.transform.SetAsFirstSibling();
        Empty();
        RectTransform rectTransform = transform as RectTransform;
        RectTransform animationTransfrom = animation.transform as RectTransform;
        Vector2 origin = rectTransform.anchoredPosition;
        float destinationX = origin.x + ((RectTransform)transform).sizeDelta.x * movement;
        Vector2 destination = new Vector2(destinationX, origin.y);
        float time = 0;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            animationTransfrom.anchoredPosition = Vector3.Lerp(origin, destination, time / maxTime);
            yield return null;
        }
        Destroy(animation.gameObject);
    }

    public IEnumerator MoveVertically(int movement, float maxTime)
    {
        Tile animation = Instantiate(this, transform.parent);
        animation.transform.SetAsFirstSibling();
        Empty();
        RectTransform rectTransform = transform as RectTransform;
        RectTransform animationTransfrom = animation.transform as RectTransform;
        Vector2 origin = rectTransform.anchoredPosition;
        float destinationY = origin.y - ((RectTransform)transform).sizeDelta.y * movement;
        Vector2 destination = new Vector3(origin.x, destinationY);
        float time = 0;
        while (time < maxTime)
        {
            time += Time.deltaTime;
            animationTransfrom.anchoredPosition = Vector3.Lerp(origin, destination, time / maxTime);
            yield return null;
        }
        Destroy(animation.gameObject);
    }

    public IEnumerator MergeAnimation(float maxTime, float percentage)
    {
        yield return new WaitForSeconds(maxTime * percentage);
        yield return ScaleAnimation(1.3f, maxTime * (1 - percentage));
        Double();
    }

    private IEnumerator NumberGenerated(float maxTime)
    {
        yield return ScaleAnimation(0.1f, maxTime);
        View.OnGoingAnimation = false;
    }

    private IEnumerator ScaleAnimation(float start, float maxTime)
    {
        transform.SetAsLastSibling();
        float time = 0;
        Vector3 initialScale = new Vector3(start, start, start);
        while (time < maxTime)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, Vector3.one, time / maxTime);
            yield return null;
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
