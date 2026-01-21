using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
// using Game.HotUpdateScripts.PortalScene.Portal;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public abstract class UIScene : MonoBehaviour
{
    public SceneId SceneId => sceneId;
    [SerializeField] private SceneId sceneId;
    [SerializeField] private List<UIScreen> screens;
    private Image _screenTransition;

    [Header("For Transition")] [SerializeField]
    private float openDuration = 0.1f;

    [SerializeField] private float closeDuration = 0.1f;

    [SerializeField] private float delayAppear = 0.2f;
    [SerializeField] private int sortingOrder = 100;
    [SerializeField] private Color transitionColor = Color.black;
    [SerializeField] private ScreenOrientation Orientation;


    protected virtual void Awake()
    {
        Screen.orientation = Orientation;
        _screenTransition = CreateScreenTransition();
        _screenTransition.color = transitionColor;
        OpenScreen();
        ConnectToServer();
    }

    protected virtual void Start()
    {
        SetDefault();
    }

    private void SetDefault()
    {
        //AudioManager.Instance.SetToggleSoundEffect(!LocalStorageUtils.GetSceneSound(sceneId));
        //AudioManager.Instance.SetToggleMusic(!LocalStorageUtils.GetSceneMusic(sceneId));
    }

    private void ConnectToServer()
    {
        SmartFoxConnection.Instance.StartClientConnectTimeOut();
       // var gameItemResponse = PortalModel.Instance.GetGameResponseItemById((int)sceneId);
        // if (gameItemResponse == null || gameItemResponse.port == -1) return;
        // SmartFoxConnection.Instance.Connect(gameItemResponse.ip, gameItemResponse.port,
        //     gameItemResponse.zone);
    }

    private void OnValidate()
    {
        screens = GetComponentsInChildren<UIScreen>(true).ToList();
    }

    public UIScreen GetScreenById(ScreenId id)
    {
        return screens.Find(screen => screen.ScreenId == id);
    }

    protected virtual void OnEnable()
    {
        Logger.Log($"Open game: {sceneId}");
        ScreenManager.Instance.CurrentOpenScene = this;
        ScreenManager.OnCloseCurrentScene.AddListener(ClearSceneData);
    }

    protected virtual void OnDisable()
    {
        if (_screenTransition) _screenTransition.DOKill();
        ScreenManager.OnCloseCurrentScene.RemoveListener(ClearSceneData);
    }

    protected virtual void ClearSceneData(UIScene scene)
    {
        Pool.ClearAll();
        SmartFoxConnection.Instance.Disconnect();
    }

    public Tween OpenScreen(bool needTransition = true, bool isOpenScreen = false, float customOpenDuration = 0)
    {
        if (needTransition)
        {
            _screenTransition.DOKill();
            return _screenTransition.DOFade(0f, customOpenDuration == 0 ? openDuration : customOpenDuration)
                .SetDelay(isOpenScreen ? 0 : delayAppear)
                .OnComplete(() => { _screenTransition.Hide(); });
        }

        return null;
    }

    public Tween CloseScreen(bool needTransition = true, float customCloseDuration = 0)
    {
        if (needTransition)
        {
            _screenTransition.Show();
            _screenTransition.DOKill();
            return _screenTransition.DOFade(1f, customCloseDuration == 0 ? closeDuration : customCloseDuration);
        }

        return null;
    }

    private Image CreateScreenTransition()
    {
        var transitionGo = new GameObject("Transition");
        var rect = transitionGo.AddComponent<RectTransform>();
        rect.SetParent(transform);
        rect.localPosition = new Vector3(0, 0, -100);
        rect.anchorMax = Vector2.one;
        rect.anchorMin = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        var canvas = transitionGo.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder;
        var image = transitionGo.AddComponent<Image>();
        image.color = transitionColor;
        return image;
    }
}

public enum SceneId
{
    PortalScene = 0,
    Slot3x3Scene,
    FakeGame1,
    FakeGame2,
    TeangLenScene,
    PusoyScene,
    BorkDengScene,
    Slot3x5Scene,
    HooHeyHowScene,
    PiratesFishing,
    TeenPatti,
    RongHoScene
}