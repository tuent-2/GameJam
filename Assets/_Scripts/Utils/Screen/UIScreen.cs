using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class UIScreen : MonoBehaviour
{
    [SerializeField] private ScreenId screenId;
    [SerializeField] private float openDuration = 0.1f;
    [SerializeField] private float closeDuration = 0.1f;
    public float OpenDuration => openDuration;
    public float CloseDuration => closeDuration;

    [Header("For Bg music")] [SerializeField]
    private bool enableMusic;

    [ShowIf("enableMusic")] [SerializeField]
    private AudioClip screenMusic;

    [FormerlySerializedAs("screenMusic")] [ShowIf("enableMusic")] [SerializeField]
    private float volumeScale = 1f;

    public ScreenId ScreenId => screenId;

    protected virtual void OnEnable()
    {
        Logger.Log($"Current Open Screen: {ScreenId}");


        ScreenManager.Instance.CurrentOpenScreen = this;
        if (enableMusic)
        {
            StartCoroutine(IEWaitUntilInitAudio());
        }

        IEnumerator IEWaitUntilInitAudio()
        {
            yield return new WaitUntil(
                () => true
                );
           // AudioManager.PlayBackgroundMusic(screenMusic, volumeScale);
        }
    }

    protected virtual void OnDisable()
    {
        if (screenMusic)
        {
            screenMusic.UnloadAudioData();
        }
    }
}

public enum ScreenId
{
    //PortalScence,
    LoginScreen,
    PortalScreen,

    //FakeGame1Scene,
    FakeGame1Lobby,
    FakeGame1GamePlay,

    //FakeGame2Scene,
    FakeGame2Lobby,
    FakeGame2GamePlay,

    //TeangLen
    RoomBoard,
    TeangLenGame,

    //Pusoy
    PusoyGame,

    //Slot3x3
    Slot3x3Main,

    //Slot3x5
    Slot3x5Main,

    //HooheyHow
    HooHeyHowMain,

    //PiratesFishing,
    FishingRoomScreen,
    ShootingFishScreen,

    //Teenpatti
    TeenPattiGame,

    //RongHo
    RongHoMain
}