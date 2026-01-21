using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LocalizeImage : MonoBehaviour
{
    [SerializeField] private Image imageLocalize;
    [SerializeField] private Sprite sprEnglish;
    [SerializeField] private Sprite sprKhmer;
   
     private void OnValidate()
     {
         imageLocalize ??= GetComponent<Image>();
     }

     private void OnEnable()
     {
         LocalizationSettings.SelectedLocaleChanged += UpdateIconAnimation;
     }

     private void OnDisable()
     {
         LocalizationSettings.SelectedLocaleChanged -= UpdateIconAnimation;
     }

     private void UpdateIconAnimation(Locale obj)
     {  
         bool isKhmer = LocalizationSettings.SelectedLocale ==
                        LocalizationSettings.AvailableLocales.Locales[LocalizeText.VN_POSITION];    
        imageLocalize.sprite = isKhmer ? sprKhmer : sprEnglish;   
     }
}
