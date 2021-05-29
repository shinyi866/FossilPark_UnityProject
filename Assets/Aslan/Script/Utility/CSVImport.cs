using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class CSVImport : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            if (str.IndexOf("/GameDialog.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                string assetfile = str.Replace(".csv", ".asset");
                GameDialogData gameData = AssetDatabase.LoadAssetAtPath<GameDialogData>(assetfile);
                if (gameData == null)
                {
                    gameData = new GameDialogData();
                    AssetDatabase.CreateAsset(gameData, assetfile);
                }

                gameData.m_Data = CSVSerializer.Deserialize<GameDialogData.Data>(data.text);

                EditorUtility.SetDirty(gameData);
                AssetDatabase.SaveAssets();
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }

            if (str.IndexOf("/MainGuide.csv") != -1)
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(str);
                string assetfile = str.Replace(".csv", ".asset");
                MainGuideData gameData = AssetDatabase.LoadAssetAtPath<MainGuideData>(assetfile);
                if (gameData == null)
                {
                    gameData = new MainGuideData();
                    AssetDatabase.CreateAsset(gameData, assetfile);
                }

                gameData.m_Data = CSVSerializer.Deserialize<MainGuideData.GuideData>(data.text);

                EditorUtility.SetDirty(gameData);
                AssetDatabase.SaveAssets();
#if DEBUG_LOG || UNITY_EDITOR
                Debug.Log("Reimported Asset: " + str);
#endif
            }
        }
    }
}
#endif

