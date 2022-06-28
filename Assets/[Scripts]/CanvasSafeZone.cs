
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSafeZone : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] CanvasScaler canvasScaler;
    private ScreenOrientation screenOrientation;

    private void Start()
    {
        screenOrientation = Screen.orientation;
        StartCoroutine(UpdateSafeZone());
    }

    private void FixedUpdate()
    {
        if (Screen.orientation != screenOrientation)
        {
            screenOrientation = Screen.orientation;
            StartCoroutine(UpdateSafeZone());
        }
    }

    IEnumerator UpdateSafeZone()
    {
        yield return null;

        Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
        Vector2 screenRatio = canvasScaler.referenceResolution / screenResolution;

        rectTransform.offsetMin = Screen.safeArea.min * screenRatio;
        rectTransform.offsetMax = (Screen.safeArea.max - screenResolution) * screenRatio;
    }
}
