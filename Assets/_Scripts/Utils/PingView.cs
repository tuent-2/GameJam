using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PingView : MonoBehaviour
{
    [SerializeField] private Image img1P4;
    [SerializeField] private Image img1P2;
    [SerializeField] private Image img3P4;
    [SerializeField] private Image imgFull;

    public const int MAX_FULL_PING = 50;
    public const int MAX_3P4_PING = 100;
    public const int MAX_1P2_PING = 150;

    private void OnEnable()
    {
        SmartFoxConnection.OnPingPongUpdate.AddListener(OnUpdatePingPong);
        SmartFoxConnection.OnLostConnectionFromServer.AddListener(OnDisconnect);
    }

    private void OnDisable()
    {
        SmartFoxConnection.OnPingPongUpdate.RemoveListener(OnUpdatePingPong);
        SmartFoxConnection.OnLostConnectionFromServer.RemoveListener(OnDisconnect);
    }

    private void OnUpdatePingPong(int ping)
    {
        //Debug.Log($"Update PingPong with {ping} value");
        ResetPing();
        switch (ping)
        {
            case < MAX_FULL_PING:
                imgFull.Show();
                break;
            case < MAX_3P4_PING:
                img3P4.Show();
                break;
            case < MAX_1P2_PING:
                img1P2.Show();
                break;
            default:
                img1P4.Show();
                break;
        }
    }

    private void OnDisconnect()
    {
        ResetPing();
    }


    private void ResetPing()
    {
        img1P4.Hide();
        img1P2.Hide();
        img3P4.Hide();
        imgFull.Hide();
    }
}