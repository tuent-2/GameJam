using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using AnimationState = Spine.AnimationState;

namespace Game.HotUpdateScripts.Utils.Localize
{
    [RequireComponent(typeof(SkeletonGraphic))]
    public class LocalizeAnimSpine : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic spine;
        [SerializeField] private bool autoStartAnim = true;
        [SerializeField] private AnimType animType;

        [Header("If has no localize")] [ShowIf("animType", AnimType.Normal)] [SerializeField]
        private string animationKey = "animation";

        [Header("If has one animation")] [ShowIf("animType", AnimType.Localize)] [SerializeField]
        private string cambodiaKey = "txt_cam";

        [SerializeField] private string englishKey = "txt_eng";

        [Header("If has two animation")] [ShowIf("animType", AnimType.LocalizeMix)] [SerializeField]
        private string popCambodiaKey = "pop_cam";

        [SerializeField] private string popEnglishKey = "pop_eng";
        [SerializeField] private string idleCambodiaKey = "idle_cam";
        [SerializeField] private string idleEnglishKey = "idle_eng";

        private void OnValidate()
        {
            spine ??= GetComponent<SkeletonGraphic>();
        }

        private void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
            if (autoStartAnim)
            {
                StartAnimByType();
            }
        }

        public void StartAnimByType()
        {
            switch (animType)
            {
                case AnimType.Localize:
                    StartAnim();
                    break;
                case AnimType.LocalizeMix:
                    StartMixAnim();
                    break;
                case AnimType.Normal:
                    StartNoLocalizeAnim();
                    break;
            }
        }

        private void OnDisable()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
        }

        private void OnSelectedLocaleChanged(Locale locale)
        {
            StartAnimByType();
        }

        public void Initialize(SkeletonDataAsset asset)
        {
            spine.skeletonDataAsset = asset;
            spine.Initialize(true);
        }

        public void StartNoLocalizeAnim()
        {
            animType = AnimType.Normal;
            spine.SetAnimation(animationKey);
        }

        public void StartAnim(bool isLoop = true, AnimationState.TrackEntryDelegate onComplete = null)
        {
            animType = AnimType.Localize;
            if (LocalizationSettings.SelectedLocale ==
                LocalizationSettings.AvailableLocales.Locales[LocalizeText.KHMER_POSITION])
            {
                spine.SetAnimation(cambodiaKey, isLoop);
            }
            else
            {
                spine.SetAnimation(englishKey, isLoop);
            }

            if (onComplete != null)
                spine.AnimationState.Complete += onComplete;
        }

        public void StartMixAnim()
        {
            animType = AnimType.LocalizeMix;
            autoStartAnim = false;
            if (LocalizationSettings.SelectedLocale ==
                LocalizationSettings.AvailableLocales.Locales[LocalizeText.KHMER_POSITION])
            {
                spine.SetAnimation(popCambodiaKey, false);
                spine.AddAnimation(idleCambodiaKey);
            }
            else
            {
                spine.SetAnimation(popEnglishKey, false);
                spine.AddAnimation(idleEnglishKey);
            }
        }
    }

    public enum AnimType
    {
        Localize,
        LocalizeMix,
        Normal
    }
}