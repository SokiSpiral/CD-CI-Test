using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    static string[] buildScenes = {
        "Assets/Scenes/SampleScene.unity"
    };

    [MenuItem("Build/Android/APK")]
    public static void AndroidAPKBuild()
    {
        if (!CheckDebugToolSymbol())
            return;

        SetKeyStoreInfo();

        EditorUserBuildSettings.buildAppBundle = false;
        AndroidBuild();
    }

    [MenuItem("Build/Android/AAB")]
    public static void AndroidAABBuild()
    {
        if (!CheckDebugToolSymbol())
            return;

        var isCompleteSetKeyStore = SetKeyStoreInfo();
        if (!isCompleteSetKeyStore)
            return;

        EditorUserBuildSettings.buildAppBundle = true;
        AndroidBuild();
    }

    [MenuItem("Build/Android/AAB - BundleNumber Increment")]
    public static void AndroidAABBuildWithIncrementBuildNumber()
    {
        if (!CheckDebugToolSymbol())
            return;

        IncrementBundleNumber();
        AndroidAABBuild();
    }

    [MenuItem("Build/Android/AAB - Version Increment")]
    public static void AndroidAABBuildWithIncrementAppVersion()
    {
        if (!CheckDebugToolSymbol())
            return;

        IncrementAppVersion();
        AndroidAABBuildWithIncrementBuildNumber();
    }

    private static void IncrementBundleNumber()
    {
        int bundleNumber = PlayerSettings.Android.bundleVersionCode;
        bundleNumber++;
        PlayerSettings.Android.bundleVersionCode = bundleNumber;
    }

    [MenuItem("Build/iOS/Build")]
    public static void BuildiOS()
    {
        if (!CheckDebugToolSymbol())
            return;

        string buildPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../Build/iOSBuild"));
        if (!Directory.Exists(buildPath))
            Directory.CreateDirectory(buildPath);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = buildScenes,
            locationPathName = buildPath,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("iOS Build succeeded: " + summary.totalSize + " bytes");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError("iOS Build failed");
        }
    }

    [MenuItem("Build/iOS/Build - Version Increment")]
    public static void BuildiOSWithIncrementAppVersion()
    {
        if (!CheckDebugToolSymbol())
            return;

        IncrementAppVersion();
        BuildiOS();
    }

    private static void IncrementAppVersion()
    {
        string currentVersion = PlayerSettings.bundleVersion;
        string[] versionParts = currentVersion.Split('.');
        if (versionParts.Length != 3)
        {
            Debug.LogError("Current version format is incorrect. Current version: " + currentVersion);
            return;
        }

        int majorVersion = int.Parse(versionParts[0]);
        int minorVersion = int.Parse(versionParts[1]);
        int patchVersion = int.Parse(versionParts[2]);
        patchVersion++;
        string newVersion = $"{majorVersion}.{minorVersion}.{patchVersion}";
        PlayerSettings.bundleVersion = newVersion;

        Debug.Log("App Version Incremented to: " + newVersion);
    }

    static bool SetKeyStoreInfo()
    {
        string configFilePath = Path.GetFullPath(Path.Combine(Application.dataPath, "../keystore-config.json"));
        if (!File.Exists(configFilePath))
        {
            Debug.LogError("Config File not found! " + configFilePath);
            return false;
        }

        string jsonText = File.ReadAllText(configFilePath);
        KeystoreConfig config = JsonUtility.FromJson<KeystoreConfig>(jsonText);

        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = config.keystoreName;
        PlayerSettings.Android.keystorePass = config.keystorePass;
        PlayerSettings.Android.keyaliasName = config.keyaliasName;
        PlayerSettings.Android.keyaliasPass = config.keyaliasPass;
        return true;
    }

    static void AndroidBuild()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        var extension = EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk";
        string outputPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../Build/build" + extension));
        BuildReport report = BuildPipeline.BuildPlayer(buildScenes, outputPath, BuildTarget.Android, BuildOptions.None);

        BuildSummary summary = report.summary;
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Android Build succeeded: " + summary.totalSize + " bytes");
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError("Android Build failed");
        }
    }

    static bool CheckDebugToolSymbol(bool isAndroid = true)
    {
        var targetGroup = isAndroid ? BuildTargetGroup.Android : BuildTargetGroup.iOS;
        if (EditorUserBuildSettings.development)
        {
            if (!PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Contains("ENABLE_DEBUG_TOOLS"))
            {
                Debug.LogWarning("ENABLE_DEBUG_TOOLSシンボルが定義されていないのでデバッグツールは表示されません OK?");
            }
        }
        else
        {
            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup).Contains("ENABLE_DEBUG_TOOLS"))
            {
                Debug.LogError("ENABLE_DEBUG_TOOLSシンボルが定義されています。削除してください。");
                return false;
            }
        }
        return true;
    }

    [System.Serializable]
    private class KeystoreConfig
    {
        public string keystoreName;
        public string keystorePass;
        public string keyaliasName;
        public string keyaliasPass;
    }
}