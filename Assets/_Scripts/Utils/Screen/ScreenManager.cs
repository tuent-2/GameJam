using System;
using System.Collections;
using DG.Tweening;

using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    public UIScene CurrentOpenScene { get; set; }
    public UIScreen CurrentOpenScreen { get; set; }
    public static readonly Signal<UIScene> OnCloseCurrentScene = new();

    public bool IsOpenScreen(ScreenId screenId) => CurrentOpenScreen && (screenId == CurrentOpenScreen.ScreenId);
    public bool IsOpenScene(SceneId sceneId) => CurrentOpenScene && (sceneId == CurrentOpenScene.SceneId);

    public static event Action OpenShopView;
    public static event Action OpenWithDrawView;
    public static event Action OpenBonusGoldView;

    public void BackToPortal()
    {
        OpenScene(SceneId.PortalScene, ScreenId.PortalScreen);
    }

    public void OpenScene(SceneId sceneId)
    {
       
        if (sceneId == CurrentOpenScene.SceneId) return;

        Instance.StartCoroutine(IEOpenScene());

        IEnumerator IEOpenScene()
        {
            yield return CurrentOpenScene.CloseScreen().WaitForCompletion();
            var loadSceneHandle = Addressables.LoadSceneAsync(sceneId.ToString());
            OnCloseCurrentScene.Dispatch(CurrentOpenScene);
            yield return loadSceneHandle;
            if (loadSceneHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Logger.LogError("Load Scene Error");
            }

           
            HeaderNoticeUtils.Instance.ClearMessages();
        }
    }

    public void OpenScene(SceneId sceneId, ScreenId screenId, Action onOpenComplete = null, bool needTransition = true)
    {
        
        if (sceneId == CurrentOpenScene.SceneId) return;

        Instance.StartCoroutine(IEOpenScene());

        IEnumerator IEOpenScene()
        {
            yield return CurrentOpenScene.CloseScreen().WaitForCompletion();
            var loadSceneHandle = Addressables.LoadSceneAsync(sceneId.ToString());
            HeaderNoticeUtils.Instance.ClearMessages();
            OnCloseCurrentScene.Dispatch(CurrentOpenScene);
            yield return loadSceneHandle;

           
            if (loadSceneHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Logger.LogError("Load Scene Error");
            }
            else if (loadSceneHandle.Status == AsyncOperationStatus.Succeeded)
            {
                if (CurrentOpenScreen)
                {
                    if (screenId != CurrentOpenScreen.ScreenId)
                    {
                        CurrentOpenScreen.Hide();
                        ShowNewScreen(screenId);
                        onOpenComplete?.Invoke();
                    }
                }
                else
                {
                    ShowNewScreen(screenId);
                    onOpenComplete?.Invoke();
                }
            }
        }
    }

    public void OpenScreen(ScreenId screenId, Action onOpenComplete = null, bool needTransition = true)
    {
     
        if (CurrentOpenScreen && screenId == CurrentOpenScreen.ScreenId) return;
        Instance.StartCoroutine(IEOpenScreen());

        IEnumerator IEOpenScreen()
        {
            yield return CurrentOpenScene.CloseScreen(needTransition, CurrentOpenScreen.CloseDuration)
                ?.WaitForCompletion();
            CurrentOpenScreen.Hide();
            ShowNewScreen(screenId);
            yield return CurrentOpenScene.OpenScreen(needTransition, true, CurrentOpenScreen.OpenDuration)
                ?.WaitForCompletion();
            onOpenComplete?.Invoke();
        }
    }

    private void ShowNewScreen(ScreenId screenId)
    {
        var newScreen = CurrentOpenScene.GetScreenById(screenId);
        newScreen.Show();
    }

    public void ChangeToPortalAndOpenShopView()
    {
        OpenScene(SceneId.PortalScene, ScreenId.PortalScreen, () =>
        {
            SmartFoxConnection.OnConnectionSuccess.AddListener(OpenToShopView);

            void OpenToShopView()
            {
                OpenShopView?.Invoke();
                SmartFoxConnection.OnConnectionSuccess.RemoveListener(OpenToShopView);
            }
        });
    }

    public void ChangeToPortalAndOpenWithDrawView()
    {
        OpenScene(SceneId.PortalScene, ScreenId.PortalScreen, () =>
        {
            SmartFoxConnection.OnConnectionSuccess.AddListener(OpenToShopView);

            void OpenToShopView()
            {
                OpenWithDrawView?.Invoke();
                SmartFoxConnection.OnConnectionSuccess.RemoveListener(OpenToShopView);
            }
        });
    }

    public void ChangeToPortalAndOpenBonusGoldView()
    {
        OpenScene(SceneId.PortalScene, ScreenId.PortalScreen, () =>
        {
            SmartFoxConnection.OnConnectionSuccess.AddListener(OpenToShopView);

            void OpenToShopView()
            {
                OpenBonusGoldView?.Invoke();
                SmartFoxConnection.OnConnectionSuccess.RemoveListener(OpenToShopView);
            }
        });
    }

    public void ChangeToPortalAndSendNLDCheckRequest()
    {
        OpenScene(SceneId.PortalScene, ScreenId.PortalScreen, () =>
        {
            SmartFoxConnection.OnConnectionSuccess.AddListener(OpenToShopView);

            void OpenToShopView()
            {
                // if (UserModel.Instance.UserInfoData.bonus.Value == false)
                // {
                //     Toast.Show(GameMsg.DEPOSIT_UNLOCKED_BONNUS);
                //     OpenShopView?.Invoke();
                //     SmartFoxConnection.OnConnectionSuccess.RemoveListener(OpenToShopView);
                //     return;
                // }

              //  PortalModel.Instance.SendNLDRequest();

                SmartFoxConnection.OnConnectionSuccess.RemoveListener(OpenToShopView);
            }
        });
    }
}