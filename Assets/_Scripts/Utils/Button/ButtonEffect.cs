using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonEffect : MonoBehaviour
{
    [SerializeField] private Button btnEffect;
    [SerializeField] private AudioClip clickSound;

    private void OnValidate()
    {
        btnEffect ??= GetComponent<Button>();
    }

    private void Reset()
    {
        btnEffect = GetComponent<Button>();
    }

    private void Start()
    {
       // btnEffect.onClick.AddListener(() => { AudioManager.PlaySound(clickSound); });
    }
}