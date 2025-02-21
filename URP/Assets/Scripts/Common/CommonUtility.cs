using UnityEngine;

public static class CommonUtility
{
    public static bool IsPC => Application.platform == RuntimePlatform.WindowsPlayer ||
                               Application.platform == RuntimePlatform.WindowsEditor ||
                               Application.platform == RuntimePlatform.OSXPlayer ||
                               Application.platform == RuntimePlatform.OSXEditor ||
                               Application.platform == RuntimePlatform.LinuxPlayer ||
                               Application.platform == RuntimePlatform.LinuxEditor;
}
