using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName,txtScore;
    [SerializeField] private Image ImgAvatar;
    [SerializeField] private Image ImgBG;
    [SerializeField] private AvatarConfigs AvatarConfig;
    
    public void SetUpUI(bool isUser, string playerName, int camel, int score)
    {
        ImgBG.color = isUser ? Color.chocolate : Color.blueViolet;
        txtName.text = name;
        ImgAvatar.sprite = AvatarConfig.GetAvatarById(camel).AvatarIcon;
        txtScore.DOIncreaseMoney(long.Parse(txtScore.text), (long)score, 1f);
    }
}
