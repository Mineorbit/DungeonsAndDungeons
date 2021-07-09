using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript : MonoBehaviour
{
        [UnityEditor.MenuItem("Quick/Deploy")]
        public static void BuildGame ()
        {
            // Get filename.
            string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
            string[] levels = new string[] {"Assets/Scenes/main.unity", "Assets/Scenes/menu.unity",
                "Assets/Scenes/test.unity","Assets/Scenes/edit.unity","Assets/Scenes/play.unity",};
    
            // Build player.
            BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
    
            // Copy a file from the project folder to the build folder, alongside the built game.
            //FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");
    
        }
}
