// Create a folder (right click in the Assets directory, click Create>New Folder)
// and name it “Editor” if one doesn’t exist already. Place this script in that folder.

// This script creates a new menu item Examples>Create Prefab in the main menu.
// Use it to create Prefab(s) from the selected GameObject(s).
// It will be placed in the root Assets folder.

using System.IO;
using UnityEngine;
using UnityEditor;

public class Example
{
    // Creates a new menu item 'Examples > Create Prefab' in the main menu.
    [MenuItem("Tools/Create Prefab")]
    static void CreatePrefab()
    {
        // Keep track of the currently selected GameObject(s)
        GameObject[] objectArray = Selection.gameObjects;

        // Loop through every GameObject in the array above
        foreach (GameObject gameObject in objectArray)
        {
            Debug.Log(gameObject.name);
            Debug.Log("Mesh time...");
            MeshFilter[] mfs = gameObject.transform.GetComponentsInChildren<MeshFilter>();
            int mSize = mfs.Length;
            int count = 0;
            
            foreach (MeshFilter mf in mfs)
            {
                if (mf)
                {
                    string name = mf.name;
                    count++;
                    name = name.Replace(":", "_");
                    name = name.Replace("/", "#");
                    string savePath = "Assets/Meshes/" + name + ".asset";
                    //AssetDatabase.CreateAsset(mf.mesh, savePath);
                    
                    //Debug.Log(count + "/" + mSize + " - Mesh");
                }
            }

            

            
            Debug.Log("Terrain time...");
            Terrain td = gameObject.transform.GetComponent<Terrain>();
            Object[] savedTDs = Resources.LoadAll("Terrain", typeof(TerrainData));
            Debug.Log(savedTDs.Length);
            Debug.Log("------");

            string correctedName = td.name;
            correctedName = correctedName.Replace(":", "_");
            correctedName = correctedName.Replace("/", "#");
            TerrainData newTD = (TerrainData)ArrayUtility.Find(savedTDs, t => t.name == correctedName);

            td.terrainData = null;
            td.gameObject.GetComponent<TerrainCollider>().terrainData = null;
            Debug.Log("Loaded new terrain for " + correctedName + " with " + newTD.name);
            
            /*
            count = 0;
            foreach (Terrain td in tds)
            {
                if (td)
                {
                    string name = td.name;
                    name = name.Replace(":", "_");
                    name = name.Replace("/", "#");
                    string savePath = "Assets/Terrain/" + name  + ".asset";
                    TerrainData tdData = td.terrainData;
                    count++;
                    //Debug.Log(count + "/" + tSize + " - Terrain");
                }
            }
            */

            // Set the path as within the Assets folder,
            // and name it as the GameObject's name with the .Prefab format
            string localPath = "Assets/" + gameObject.name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            //localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            // Create the new Prefab.
            //PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
        }
    }

    // Disable the menu item if no selection is in place.
    [MenuItem("Examples/Create Prefab", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject);
    }
}