using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableTiles
{
    [MenuItem("Assets/Create/ELARA/My Scriptable Tile")]
    public static void CreateMyAsset()
    {
        ScriptableTile asset = ScriptableObject.CreateInstance<ScriptableTile>();

        AssetDatabase.CreateAsset(asset, "Assets/Elara/Scriptables/NewScriptableTile.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}