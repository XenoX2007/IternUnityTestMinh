#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public static class MobileBrowserDetector
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern int IsMobileBrowser();
#endif

    public static bool IsMobile()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return IsMobileBrowser() == 1;
#elif UNITY_EDITOR
        return false;
#else
        return Application.isMobilePlatform;
#endif
    }
}