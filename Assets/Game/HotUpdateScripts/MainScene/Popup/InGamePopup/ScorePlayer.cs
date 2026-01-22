using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePlayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image imgValue;

    public void UpdateUI(int score)
    {
        scoreText.text = score.ToString();
       // imgValue.fillAmount
    }
}
