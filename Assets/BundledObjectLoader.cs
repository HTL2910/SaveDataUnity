using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class BundledObjectLoader : MonoBehaviour
{
    public string assetName = "BundledSpriteObject";
    //public string assetName = "home building";
    public string bundleName = "testbundle";
    //// Start is called before the first frame update
    //void Start()
    //{
    //    AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleName));
    //    if (localAssetBundle == null)
    //    {
    //        Debug.LogError("Failed to load AssetBundle!");
    //        return;
    //    }
    //    Debug.Log(Path.Combine(Application.streamingAssetsPath, bundleName));
    //    GameObject asset = localAssetBundle.LoadAsset<GameObject>(assetName);
    //    //if (asset == null)
    //    //{
    //    //    Debug.LogError("Failed to load GameObject from AssetBundle!");
    //    //    localAssetBundle.Unload(false);
    //    //    return;
    //    //}
    //    Debug.Log("Name: "+localAssetBundle.GetAllAssetNames());
    //    Instantiate(asset);
    //    localAssetBundle.Unload(false);
    //}
    void Start()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "testbundle"));
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
        var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("BundledSpriteObject");
        Instantiate(prefab);
        myLoadedAssetBundle.Unload(false);
    }
}

