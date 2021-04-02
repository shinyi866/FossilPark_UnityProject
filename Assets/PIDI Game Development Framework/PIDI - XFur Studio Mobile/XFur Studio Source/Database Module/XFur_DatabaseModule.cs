/*
XFur Studio™ Mobile v1.3

You cannot sell, redistribute, share nor make public this code, even modified, through any means on any platform.
Modifications are allowed only for your own use and to make this product suit better your project's needs.
These modifications may not be redistributed, sold or shared in any way.

For more information, contact us at contact@irreverent-software.com

Copyright© 2018-2019, Jorge Pinal Negrete. All Rights Reserved.
*/


using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XFurStudioMobile {

    [CreateAssetMenu(menuName = "XFur Studio/XFur Modules/Database Asset")]
    public class XFur_DatabaseModule : ScriptableObject {

        public Shader[] highQualityShaders = new Shader[0];
        public Shader[] lowQualityShaders = new Shader[0];
        public Shader DynamicShellShader;
        public Shader DynamicShellSkin;

        public Texture2D[] furNoiseMaps = new Texture2D[0];

        public XFur_FurMeshContainer[] meshData = new XFur_FurMeshContainer[0];

        public string XFurVersion = "v1.3_Mobile";

        public Texture2D bumpEmpty;


        //Checks if the database contains the needed mesh.

        public int XFur_ContainsMeshData(Mesh originalMesh) {
            int exists = -1;

            for (int i = 0; i < meshData.Length; i++) {
                if (meshData[i].originalMesh == originalMesh) {
                    return i;
                }
            }

            return exists;
        }

        public int XFur_ContainsMeshData(int index) {
            int exists = -1;

            if (index > 0 && index < meshData.Length)
                exists = index;

            return exists;
        }

#if UNITY_EDITOR
        public void XFur_AddBaseMeshData(Mesh originalMesh, Mesh patchedMesh, int[] furVertices, string version) {
            var index = XFur_ContainsMeshData(originalMesh);
            var temp = new List<XFur_FurMeshContainer>(meshData);
            var path = AssetDatabase.GetAssetPath(this);

            if (index == -1) {
                var x = new XFur_FurMeshContainer();
                x.originalMesh = originalMesh;
                x.XFurVersion = version;
                x.shadowMeshes = new XFur_ShadowMeshData[originalMesh.subMeshCount];
                var meshes = new List<Mesh>();
                meshes.Add(patchedMesh);
                x.furMeshes = meshes.ToArray();
                temp.Add(x);
                meshData = temp.ToArray();
                x.furVertices = furVertices;
                patchedMesh.name = originalMesh.name + "_XFMSH_PATCHED";
                AssetDatabase.AddObjectToAsset(patchedMesh, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

            }
            else {
                meshData[index].originalMesh = originalMesh;
                if (meshData[index].furMeshes[0]) {
                    DestroyImmediate(meshData[index].furMeshes[0], true);
                }

                meshData[index].furMeshes[0] = patchedMesh;
                meshData[index].furVertices = furVertices;
                patchedMesh.name = originalMesh.name + "_XFMSH_PATCHED";
                AssetDatabase.AddObjectToAsset(patchedMesh, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }


        public void XFur_AddShadowMeshData(Mesh originalMesh, Mesh shadowMesh, int materialIndex) {
            var index = XFur_ContainsMeshData(originalMesh);
            var path = AssetDatabase.GetAssetPath(this);

            if (index == -1) {
                return;
            }
            else {
                var l = new List<Mesh>(meshData[index].shadowMeshes[materialIndex].shadowMeshes);
                l.Add(shadowMesh);
                shadowMesh.name = originalMesh.name + "_XFMSH_SHELLS";

                meshData[index].shadowMeshes[materialIndex].shadowMeshes = l.ToArray();
                AssetDatabase.AddObjectToAsset(shadowMesh, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        public void XFur_RemoveMeshData(Mesh originalMesh) {

        }

        public string[] GetMeshNames() {
            var names = new List<string>();

            foreach ( XFur_FurMeshContainer mesh in meshData ) {
                names.Add( mesh.originalMesh ? mesh.originalMesh.name.ToUpper() : "UNNAMED MESH" );
            }

            return names.ToArray();
        }


#else
        public void XFur_AddBaseMeshData( Mesh originalMesh, Mesh patchedMesh, int[] furVertices, string version ){
            var index = XFur_ContainsMeshData( originalMesh );
            var temp = new List<XFur_FurMeshContainer>(meshData);
            
            if ( index == -1 ){
                var x = new XFur_FurMeshContainer();
                x.originalMesh = originalMesh;
                x.XFurVersion = version;
                x.shadowMeshes = new XFur_ShadowMeshData[originalMesh.subMeshCount];
                var meshes = new List<Mesh>();
                meshes.Add(patchedMesh);
                x.furMeshes = meshes.ToArray();
                temp.Add(x);
                meshData = temp.ToArray();
                x.furVertices = furVertices;
                patchedMesh.name = originalMesh.name+"_XFMSH_PATCHED";
                
            }
            else{
                meshData[index].originalMesh = originalMesh;
                if ( meshData[index].furMeshes[0] ){
                    DestroyImmediate( meshData[index].furMeshes[0], true );
                }

                meshData[index].furMeshes[0] = patchedMesh;
                meshData[index].furVertices = furVertices;
                patchedMesh.name = originalMesh.name+"_XFMSH_PATCHED";
            }
        }

        public void XFur_AddShadowMeshData( Mesh originalMesh, Mesh shadowMesh, int materialIndex ){
            var index = XFur_ContainsMeshData( originalMesh );
            
            if ( index == -1 ){
                return;
            }
            else{
                var l = new List<Mesh>(meshData[index].shadowMeshes[materialIndex].shadowMeshes);
                l.Add( shadowMesh );
                shadowMesh.name = originalMesh.name+"_XFMSH_SHELLS";

                meshData[index].shadowMeshes[materialIndex].shadowMeshes = l.ToArray();
            }
        }
#endif


        public Mesh XFur_GetPatchedMesh(Mesh originalMesh) {
            var index = XFur_ContainsMeshData(originalMesh);

            return index == -1 ? null : meshData[index].furMeshes[0];
        }

        public Mesh XFur_GetPatchedMesh(int index) {

            return index < 0 || index > meshData.Length - 1 ? null : meshData[index].furMeshes[0];
        }

        public void XFur_GetOriginalMesh(Mesh derivativeMesh, ref SkinnedMeshRenderer renderer) {

        }

        public bool XFur_GetShadowMesh(Mesh originalMesh, int materialIndex, SkinnedMeshRenderer[] renderers) {

            var index = XFur_ContainsMeshData(originalMesh);

            if (index != -1) {
                if (meshData[index].shadowMeshes[materialIndex].shadowMeshes.Length > 0) {
                    for (int i = 0; i < renderers.Length; i++) {
                        renderers[i].sharedMesh = meshData[index].shadowMeshes[materialIndex].shadowMeshes[i];
                    }
                    return true;
                }
            }

            return false;
        }

        public bool XFur_GetShadowMesh(Mesh originalMesh, int materialIndex, MeshFilter[] renderers) {
            var index = XFur_ContainsMeshData(originalMesh);

            if (index != -1) {
                if (meshData[index].shadowMeshes[materialIndex].shadowMeshes.Length > 0) {
                    for (int i = 0; i < renderers.Length; i++) {
                        renderers[i].sharedMesh = meshData[index].shadowMeshes[materialIndex].shadowMeshes[0];
                    }
                    return true;
                }
            }

            return false;
        }


        public void XFur_DeleteMeshData(int index) {
            var l = new List<XFur_FurMeshContainer>(meshData);

            for (int f = 0; f < meshData[index].furMeshes.Length; f++)
                DestroyImmediate(meshData[index].furMeshes[f], true);

            for (int f = 0; f < meshData[index].shadowMeshes.Length; f++) {
                if (meshData[index].shadowMeshes[f] != null) {
                    for (int sh = 0; sh < meshData[index].shadowMeshes[f].shadowMeshes.Length; sh++)
                        DestroyImmediate(meshData[index].shadowMeshes[f].shadowMeshes[sh], true);
                }
            }

            l.RemoveAt(index);

            meshData = l.ToArray();
        }


    }



    [System.Serializable]
    public class XFur_ShadowMeshData {
        public Mesh[] shadowMeshes = new Mesh[0];
    }

    [System.Serializable]
    public class XFur_FurMeshContainer {

        public Mesh originalMesh; //Original Mesh reference
        public Mesh[] furMeshes = new Mesh[1]; //Patched Mesh
        public XFur_ShadowMeshData[] shadowMeshes = new XFur_ShadowMeshData[0];
        public int[] furVertices = new int[0];
        public string XFurVersion;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(XFur_DatabaseModule))]
    public class XFur_DatabaseEditor : Editor {

        private XFur_DatabaseModule m;

        public override void OnInspectorGUI() {



            m = (XFur_DatabaseModule)target;

            m.bumpEmpty = (Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("NullNormal t:Texture2D")[0]), typeof(Texture2D));

            foreach (XFur_FurMeshContainer fur in m.meshData) {
                foreach (Mesh mesh in fur.furMeshes) {
                    if (mesh && !mesh.name.Contains("_XFMSH_PATCHED") && !mesh.name.Contains("_XFMSH_LAYER")) {
                        DestroyImmediate(mesh, true);
                    }
                }
            }

            EditorGUI.indentLevel++;

            Undo.RecordObject(m, "DATABASEXFUROBJ_" + m.GetInstanceID());

            EditorGUILayout.Space();



            var lqShaders = serializedObject.FindProperty("lowQualityShaders");

            EditorGUILayout.PropertyField(lqShaders);

            if (lqShaders.isExpanded) {
                EditorGUI.indentLevel++;

                lqShaders.arraySize = EditorGUILayout.IntField("Size", lqShaders.arraySize);

                for (int i = 0; i < lqShaders.arraySize; i++) {
                    EditorGUILayout.PropertyField(lqShaders.GetArrayElementAtIndex(i));
                }

                EditorGUI.indentLevel--;
            }

            var hqShaders = serializedObject.FindProperty("highQualityShaders");

            EditorGUILayout.PropertyField(hqShaders);

            if (hqShaders.isExpanded) {
                EditorGUI.indentLevel++;

                hqShaders.arraySize = EditorGUILayout.IntField("Size", hqShaders.arraySize);

                for (int i = 0; i < hqShaders.arraySize; i++) {
                    EditorGUILayout.PropertyField(hqShaders.GetArrayElementAtIndex(i));
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();



            for (int i = 0; i < m.meshData.Length; i++) {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(m.meshData[i].originalMesh ? m.meshData[i].originalMesh.name : ("MESH DATA " + i));
                if (GUILayout.Button("Delete")) {
                    m.XFur_DeleteMeshData(i);
                }
                GUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(m);

            EditorGUI.indentLevel--;

        }


    }



#endif

}





