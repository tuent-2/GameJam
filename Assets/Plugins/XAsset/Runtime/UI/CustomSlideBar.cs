using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CustomSlideBar : MonoBehaviour
{
    public RectTransform handle;
    public float minX = -341, maxX = 341;
    public Image progressBar;

    [Button]
    public void SetUpProgress(float progress)
    {
        progressBar.fillAmount = progress;
        handle.anchoredPosition = new Vector2((progress) * (maxX - minX) + minX, 0.5f);
    }
}