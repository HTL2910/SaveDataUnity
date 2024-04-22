using IfcToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Import : MonoBehaviour
{
    private void Start()
    {
        // Truy cập Singleton FileSelect
        string filePath = FileSelect.instance.filePath;

        if (!string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Đường dẫn tệp: " + filePath);
        }
        else
        {
            Debug.Log("Đường dẫn tệp rỗng.");
        }
    }
    public void ImportIFC()
    {
        // Truy cập Singleton FileSelect
        string filePath = FileSelect.instance.filePath;

        if (!string.IsNullOrEmpty(filePath))
        {
            Debug.Log("Đường dẫn tệp: " + filePath);
        }
        else
        {
            Debug.Log("Đường dẫn tệp rỗng.");
        }
        StartCoroutine(Demo());

    }

    ///<summary>A coroutine for starting the Importer coroutine and printing out some debug information.</summary>
    IEnumerator Demo()
    {
        Debug.Log("Importing " + FileSelect.instance.filePath + ", please wait.");
        DateTime starttime = System.DateTime.Now;     // for measuring optimization
        //An optional parameter to toggle various importing option. All are true by default.
        IfcSettingsOptionHolder options = new IfcSettingsOptionHolder();
        //Run the runtime importer with an optional callback function to get access to the root GameObject of the IFC hierarchy.
        yield return IfcImporter.RuntimeImportCoroutine(FileSelect.instance.filePath, options, (rootObject) => {
            //Various parts of the building are rootObject's children.
            Debug.Log("Name of the created GameObject: " + rootObject.name);
            DateTime endtime = System.DateTime.Now;       // for measuring optimization
            Debug.Log(endtime - starttime);
        });
    }
}
