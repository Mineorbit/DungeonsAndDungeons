using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript : MonoBehaviour
{
    public static void Build()
    {
        string path = "./Build";
        string[] levels = new string[] {"Assets/Scenes/main.unity", "Assets/Scenes/menu.unity",
            "Assets/Scenes/test.unity", "Assets/Scenes/edit.unity", "Assets/Scenes/play.unity"};

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "Linux64/BuiltGame.exe", BuildTarget.StandaloneLinux64, BuildOptions.None);
        EditorApplication.Exit(0);
        BuildPipeline.BuildPlayer(levels, path + "Win64/BuiltGame.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}
