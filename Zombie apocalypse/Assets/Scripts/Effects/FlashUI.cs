using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// responsible for ui flash effect
/// </summary>
[RequireComponent(typeof(Image))]
public class FlashUI : MonoBehaviour
{
    [SerializeField] Color flashColor = Color.HSVToRGB(63/360f,62/100f,79/100f);
    [SerializeField] float duration = .4f;
    Color initialColor;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (initialColor == null || flashColor == null)
            Debug.LogError($"{nameof(FlashUI)} not set properly");
        initialColor = image.color;
    }

    public void Flash()
    {
        StartCoroutine(PerformFlash());
    }

    IEnumerator PerformFlash()
    {
        var rStep = initialColor.r - flashColor.r;
        var gStep = initialColor.g - flashColor.g;
        var bStep = initialColor.b - flashColor.b;
        var aStep = initialColor.a - flashColor.a;
        float counter = duration;
        float fadeDuration = duration / 2;

        while (counter > 0)
        {
            counter -= Time.deltaTime;
            if (counter < fadeDuration)
            {
                var fadeOutTime = fadeDuration - counter;
                image.color = Color.Lerp(flashColor, initialColor, fadeOutTime / fadeDuration);
            }
            else if (counter >= fadeDuration)
            {
                var fadeInTime = (fadeDuration - (counter - fadeDuration));
                image.color = Color.Lerp(initialColor, flashColor, fadeInTime / fadeDuration);
            }
            yield return new WaitForFixedUpdate();
        }
        counter = 0;
    }
}
