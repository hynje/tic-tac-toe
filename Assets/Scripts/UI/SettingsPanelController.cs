using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelController : PanelController
{
    
    /// <summary>
    /// SFX On/Off시 호출되는 함수
    /// </summary>
    /// <param name="value">On/Off</param>
    public void OnSFXToggleValueChanged(bool value)
    {
        
    }

    /// <summary>
    /// BGM On/Off시 호출되는 함수
    /// </summary>
    /// <param name="value">On/Off</param>
    public void OnBGMToggleValueChanged(bool value)
    {
        
    }
    
    public void OnClickCloseButton()
    {
        Hide();
    }
    
}
