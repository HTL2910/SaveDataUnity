using UnityEngine;
using TriLibCore;
using TriLibCore.General;
using TriLibCore.Interfaces;
using TriLibCore.Mappers;
using System.IO;
using SFB;
using System;
using System.Collections;
using IfcToolkit;

public class FileChooser : MonoBehaviour
{
    public void OpenModelFile()
    {
        var extensions = new[]
        {
            new ExtensionFilter("Model Files", "ifc", "fbx", "obj", "gltf", "glb", "stl", "ply", "3mf", "dae", "zip")
        };

        // Mở hộp thoại chọn file và chỉ cho phép chọn file đơn lẻ
        StandaloneFileBrowser.OpenFilePanelAsync("Chọn file model", "", extensions, false, (string[] paths) =>
        {
            if (paths != null && paths.Length > 0)
            {
                string path = paths[0];
                Debug.Log("Selected file: " + path);
                HandleFile(path);
            }
            else
            {
                Debug.Log("No file selected or selection was cancelled.");
            }
        });
    }

    // Hàm xử lý file đã chọn
    private void HandleFile(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();

        if (extension == ".ifc")
        {
            Debug.Log("File IFC");
            // Add your IFC file handling logic here
            StartCoroutine(Demo(filePath));
        }
        else
        {
            LoadModel(filePath);
        }
    }
    IEnumerator Demo(string filePath)
    {
        Debug.Log("Importing " + filePath + ", please wait.");
        DateTime starttime = System.DateTime.Now;     // for measuring optimization
        //An optional parameter to toggle various importing option. All are true by default.
        IfcSettingsOptionHolder options = new IfcSettingsOptionHolder();
        //Run the runtime importer with an optional callback function to get access to the root GameObject of the IFC hierarchy.
        yield return IfcImporter.RuntimeImportCoroutine(filePath, options, (rootObject) => {
            //Various parts of the building are rootObject's children.
            Debug.Log("Name of the created GameObject: " + rootObject.name);
            DateTime endtime = System.DateTime.Now;       // for measuring optimization
            Debug.Log(endtime - starttime);
        });
    }
    private void LoadModel(string filePath)
    {
        AssetLoaderOptions assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        AssetLoader.LoadModelFromFile(filePath, null, null, OnProgress, OnError, null, assetLoaderOptions, null, false);
    }

    private void OnLoad(GameObject loadedGameObject)
    {
        Debug.Log("Model loaded successfully.");
        // Xử lý khi model đã được tải thành công
    }

    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
        Debug.Log($"Loading progress: {progress * 100}%");
        // Xử lý tiến trình tải model
    }

    private void OnMaterialsLoad(GameObject loadedGameObject, MaterialMapperContext materialMapperContext)
    {
        Debug.Log("Materials loaded successfully.");
        // Xử lý khi các vật liệu đã được tải thành công
    }

    private void OnError(IContextualizedError contextualizedError)
    {
        Debug.LogError($"Error loading model: {contextualizedError.GetInnerException().Message}");
        // Xử lý khi có lỗi xảy ra trong quá trình tải model
    }

}
