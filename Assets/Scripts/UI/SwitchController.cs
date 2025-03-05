using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AudioSource))]
public class SwitchController : MonoBehaviour
{
    [SerializeField] private Image handleImage;
    [SerializeField] private AudioClip clickSound;
    
    public delegate void OnSwitchChangedDelegate(bool isOn);
    public OnSwitchChangedDelegate OnSwitchChange;
    
    private static readonly Color32 OnColor = new Color32(0, 255, 0, 255);
    private static readonly Color32 OffColor = new Color32(0, 0, 0, 255);
    
    private RectTransform _handleRectTransform;
    private Image _backgroundImage;
    private AudioSource _audioSource;
    
    private bool _isOn;
    
    void Awake()
    {
        _handleRectTransform = handleImage.GetComponent<RectTransform>();
        _backgroundImage = GetComponent<Image>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetOn(true);
    }

    private void SetOn(bool isOn)
    {
        if (isOn)
        {
            _handleRectTransform.DOAnchorPosX(14, 0.2f);
            _backgroundImage.DOBlendableColor(OnColor, 0.2f);
        }
        else
        {
            _handleRectTransform.DOAnchorPosX(-14, 0.2f);
            _backgroundImage.DOBlendableColor(OffColor, 0.2f);
        }
        
        OnSwitchChange?.Invoke(isOn);
        _isOn = isOn;
    }

    public void OnClickSwitch()
    {
        SetOn(!_isOn);
        
        // 효과음 재생 
        if(clickSound != null)
            _audioSource.PlayOneShot(clickSound);
    }
}
