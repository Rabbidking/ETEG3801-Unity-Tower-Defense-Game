using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenuScripts : MonoBehaviour
{
    public int resolutionWidth;
    public int resolutionHeight;
    private bool fullOrWindowed = true;

    public void setResolutionSize()
    {
        Screen.SetResolution(resolutionWidth, resolutionHeight, fullOrWindowed);
    }
    public void setWidth(int newWidth)
    {
        resolutionWidth = newWidth;
    }
    
    public void setHeight(int newHeight)
    {
        resolutionHeight = newHeight;
    }

    public void setWindowedMode()
    {
        fullOrWindowed = false;
    }

    public void setFullscreen()
    {
        fullOrWindowed = true;
    }
}
