// Credit to Git Amend: https://www.youtube.com/watch?v=0_ZRHT2faQw
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using static System.Environment;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

public static class ProjectSetup 
{
    [MenuItem("Tools/Setup/Import Essential Assets")]
    private static void ImportEssentials()
    {
        Assets.ImportAsset("Sirenix/Editor ExtensionsSystem","Odin Inspector and Serializer.unitypackage");
        Assets.ImportAsset("IntenseNation/Editor ExtensionsUtilities", "Editor Auto Save.unitypackage");
        Assets.ImportAsset("kubacho lab/Editor ExtensionsUtilities", "vHierarchy 2.unitypackage");
        Assets.ImportAsset("Kyrylo Kuzyk/Editor ExtensionsAnimation", "PrimeTween High-Performance Animations and Sequences.unitypackage");
        Assets.ImportAsset("More Mountains/Editor ExtensionsEffects", "Feel.unitypackage");
        Assets.ImportAsset("Muka Schultze/Editor ExtensionsUtilities", "Fullscreen Editor.unitypackage");
        Assets.ImportAsset("Seaside Studios/VFX", "All In 1 Vfx Toolkit.unitypackage");
        Assets.ImportAsset("Staggart Creations/Editor ExtensionsUtilities", "Selection History.unitypackage");
        Assets.ImportAsset("The Naughty Cult/Editor ExtensionsUtilities", "Hot Reload Edit Code Without Compiling.unitypackage");
        Assets.ImportAsset("Warped Imagination/Editor ExtensionsAudio", "Audio Preview Tool.unitypackage");
        Assets.ImportAsset("Ekincan Tas/Textures MaterialsGUI Skins", "Damage Numbers Pro.unitypackage");
        Assets.ImportAsset("FlyingWorm/Editor ExtensionsSystem", "Editor Console Pro.unitypackage");
        Assets.ImportAsset("Sisus/Editor ExtensionsUtilities", "Component Names.unitypackage");
        Assets.ImportAsset("Warped Imagination/Editor ExtensionsUtilities", "Scene Selection Tool.unitypackage");
        Assets.ImportAsset("Tiny Giant Studio/Editor ExtensionsUtilities","Better Transform - Size Notes Global-Local workspace child parent transform.unitypackage");
        Assets.ImportAsset("KAMGAM/Editor ExtensionsUtilities", "UI Preview for Prefabs and Canvases.unitypackage");
    }
    
    private static class Assets
    {
        public static void ImportAsset(string folder, string asset)
        {
            var basePath = "/Users/marcrydzy/Library/"; //GetFolderPath(SpecialFolder.ApplicationData);
            var assetsFolder = Combine(basePath, "Unity/Asset Store-5.x");
            
            asset = asset.EndsWith(".unitypackage") ? asset : asset + ".unitypackage";
            string packagePath = Combine(assetsFolder, folder, asset);
            
            if (File.Exists(packagePath))
            {
                AssetDatabase.ImportPackage(packagePath, false);
                Debug.Log($"Imported: {packagePath}");
            }
            else
            {
                Debug.LogError($"Package not found: {packagePath}");
            }
        }
    }

    [MenuItem("Tools/Setup/Install Essential Packages")]
    private static void InstallPackages()
    {
        Packages.InstallPackages(new[] {
            "git+https://github.com/adammyhre/Unity-Improved-Timers",
            "git+https://github.com/adammyhre/Unity-Utils"
        });
    }
    private static class Packages
    {
        private static AddRequest _request;
        private static Queue<string> _packagesToInstall = new();
        public static void InstallPackages(string[] packages)
        {
            foreach (var package in packages)
            {
                _packagesToInstall.Enqueue(package);   
            }

            if (_packagesToInstall.Count > 0)
                StartNextPackageInstallation();
        }

        private static async void StartNextPackageInstallation()
        {
            _request = Client.Add(_packagesToInstall.Dequeue());

            while (!_request.IsCompleted) await Task.Delay(10);
            
            if (_request.Status == StatusCode.Success)
                Debug.Log("Installed: " + _request.Result.packageId);
            else if (_request.Status == StatusCode.Failure)
                Debug.Log(_request.Error.message);

            if (_packagesToInstall.Count > 0)
            {
                await Task.Delay(1000);
                StartNextPackageInstallation();
            }
        }
    }


    [MenuItem("Tools/Setup/ Create Folders")]
    public static void CreateFolders()
    {
        Folders.Create("_Project", "_Scripts", "Animations", "Art", "Materials", "Prefabs", "VFX", "SFX", "Scriptable Objects");
        Refresh();
        Folders.Move("_Project", "Scenes");
        Folders.Move("_Project", "Settings");
        Folders.Delete("TutorialInfo");
        Refresh();

        const string pathToInputActions = "Assets/Input_System_Actions.inputactions";
        string destination = "Assets/_Project/Settings/Assets/Input_System_Actions.inputactions";
        MoveAsset(pathToInputActions, destination);

        const string pathToReadMe = "Assets/Readme.asset";
        DeleteAsset(pathToReadMe);
    }
    
    private static class Folders
    {
        public static void Delete(string folderName)
        {
            string sourcePath = $"Assets/{folderName}";

            if (AssetDatabase.IsValidFolder(sourcePath))
                AssetDatabase.DeleteAsset(sourcePath);
        }
        public static void Move(string newParent, string folderName)
        {
            string sourcePath = $"Assets/{folderName}";

            if (AssetDatabase.IsValidFolder(sourcePath))
            {
                string destinationPath = $"Assets/{newParent}/{folderName}";
                string error = AssetDatabase.MoveAsset(sourcePath, destinationPath);
                
                if (!string.IsNullOrEmpty(error))
                    Debug.LogError($"Failed to move {folderName}: {error}");
            }
        }
        
        public static void Create(string root, params string[] folders)
        {
            string fullPath = Combine(Application.dataPath, root);

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            foreach (var folder in folders)
            {
                CreateSubFolder(fullPath, folder);
            }
        }

        private static void CreateSubFolder(string rootPath, string folderHierarchy)
        {
            string[] folders = folderHierarchy.Split('/');
            string currentPath = rootPath;

            foreach (var folder in folders)
            {
                string combinedPath = Combine(currentPath, folder);

                if (!Directory.Exists(combinedPath))
                    Directory.CreateDirectory(combinedPath);
            }
        }
    }
}

