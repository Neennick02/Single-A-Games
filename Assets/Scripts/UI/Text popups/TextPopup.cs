using System.Collections;
using TMPro;
using UnityEngine;

public class TextPopup : MonoBehaviour
{
    private float duration = 3f;
    [SerializeField] private TextMeshProUGUI _textObject;

    public Color StartC;
    public Color EndC;

    private void OnEnable()
    {
        ShopManager.OnShopMessage += FadingMessage;
        ShootArm.InstantMessage += InstantMessage;
        _textObject.color = Color.clear;
    }

    private void OnDisable()
    {
        ShopManager.OnShopMessage -= FadingMessage;
        ShootArm.InstantMessage -= InstantMessage;
    }
    public void FadingMessage(string message)
    {
        _textObject.text = message;
       // StopAllCoroutines();
        StartCoroutine(FadeInAndOut());
    }

    public void InstantMessage(string message, bool clear)
    {
        _textObject.color = Color.black;
        _textObject.text = message;

        if (clear)
        {
            _textObject.color = Color.clear;
        }
    }

    IEnumerator FadeInAndOut()
    {
        float totalTimer = 0;
        float endTimer = 0f;
        //fade in
        while (totalTimer < duration / 3)
        {
            totalTimer += Time.deltaTime;

            _textObject.color = Color.Lerp(EndC, StartC, totalTimer / (duration / 3));

            yield return null;
        }

        //have text visable
        yield return new WaitForSeconds(duration / 3);
        totalTimer += duration / 3;

        //fade out
        while (totalTimer < duration)
        {
            totalTimer += Time.deltaTime;
            endTimer += Time.deltaTime;

            _textObject.color = Color.Lerp(StartC, EndC, endTimer / (duration / 3));

            yield return null;
        }
    }
}
