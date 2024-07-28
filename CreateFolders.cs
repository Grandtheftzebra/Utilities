using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreateFolders : EditorWindow
{
   private static string _projectName = "PROJECT-NAME";

   [MenuItem("Assets/Create Default Folders")]
   private static void SetUpFolders()
   {
      CreateFolders window = ScriptableObject.CreateInstance<CreateFolders>();
      window.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
      window.ShowPopup();
   }

   private static void CreateAllFolders()
   {
      List<string> folders = new List<string>
      {
         "_Scripts",
         "_Data",
         "Animations",
         "Audio",
         "Editor",
         "Materials",
         "Meshes",
         "Prefabs",
         "Scriptable Objects",
         "Scenes",
         "Shaders",
         "Textures",
         "UI"
      };

      foreach (var folder in folders)
      {
         if (!Directory.Exists("Assets/" + folder))
            Directory.CreateDirectory("Assets/" + _projectName + "/" + folder);
      }

      List<string> uiFolders = new List<string>
      {
         "Assets",
         "Fonts",
         "Icon"
      };

      foreach (var subfolder in uiFolders)
      {
         if (!Directory.Exists("Assets/" + _projectName + "UI/" + subfolder))
            Directory.CreateDirectory("Assets/" + _projectName + "UI/" + subfolder);
      }
      
      AssetDatabase.Refresh();
   }
   
   private void OnGUI()
   {
      EditorGUILayout.LabelField("Insert the Project name used as the root folder");
      _projectName = EditorGUILayout.TextField("Project Name:", _projectName);
      this.Repaint();
      if (GUILayout.Button("Generate!"))
      {
         CreateAllFolders();
         this.Close();
      }
   }
}
