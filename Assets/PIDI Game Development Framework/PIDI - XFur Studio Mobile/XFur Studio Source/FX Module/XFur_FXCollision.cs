﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XFurStudioMobile {
    public enum FXToApply { Blood, Snow, Water };
    public enum FXApplyMode { OnEnter, Constant };


    [ExecuteInEditMode]
    public class XFur_FXCollision : MonoBehaviour {


        [HideInInspector] public SphereCollider fxCollider;

        public FXToApply effectToApply;
        public FXApplyMode contactMode = FXApplyMode.Constant;
        [Range(0, 1)] public float effectIntensity = 1.0f;
        public float effectRadius = 0.1f;
        private Mesh m;
        private float timer = 0;

        private void OnEnable() {
            if (!fxCollider) {
                fxCollider = gameObject.AddComponent<SphereCollider>();
                fxCollider.isTrigger = true;
                fxCollider.hideFlags = HideFlags.HideInInspector;
            }
            else {
                fxCollider.hideFlags = HideFlags.HideInInspector;
            }

        }

        // Use this for initialization
        void Start() {
            m = new Mesh();
        }

        void Update() {
            fxCollider.radius = effectRadius;
        }


        private void OnTriggerEnter(Collider other) {

            if (contactMode == FXApplyMode.Constant) {
                return;
            }

            XFurMobile_System targetFur;
            if (targetFur = other.transform.root.GetComponentInChildren<XFurMobile_System>()) {
                if (targetFur) {
                    var fV = targetFur.database.meshData[targetFur.database.XFur_ContainsMeshData(targetFur.OriginalMesh)].furVertices;
                    if (targetFur.GetComponent<SkinnedMeshRenderer>()) {
                        targetFur.GetComponent<SkinnedMeshRenderer>().BakeMesh(m);
                    }
                    else {
                        m = targetFur.Mesh;
                    }

                    var verts = m.vertices;
                    List<int> vertexIndex = new List<int>();

                    for (int i = 0; i < fV.Length; i++) {
                        if (Vector3.Distance(targetFur.transform.TransformPoint(verts[fV[i]]), transform.TransformPoint(fxCollider.center)) < fxCollider.radius) {
                            vertexIndex.Add(fV[i]);
                        }
                    }

                    targetFur.fxModule.ApplyEffect((int)effectToApply, vertexIndex.ToArray(), effectIntensity);

                }
            }
        }

        private void OnTriggerStay(Collider other) {

            if (contactMode == FXApplyMode.OnEnter) {
                return;
            }


            if (Time.timeSinceLevelLoad > timer) {

                XFurMobile_System targetFur;
                if (targetFur = other.transform.root.GetComponentInChildren<XFurMobile_System>()) {
                    if (targetFur) {
                        var fV = targetFur.database.meshData[targetFur.database.XFur_ContainsMeshData(targetFur.OriginalMesh)].furVertices;
                        if (targetFur.GetComponent<SkinnedMeshRenderer>()) {
                            targetFur.GetComponent<SkinnedMeshRenderer>().BakeMesh(m);
                        }
                        else {
                            m = targetFur.Mesh;
                        }

                        var verts = m.vertices;
                        List<int> vertexIndex = new List<int>();

                        for (int i = 0; i < fV.Length; i++) {
                            if (Vector3.Distance(targetFur.transform.TransformPoint(verts[fV[i]]), transform.TransformPoint(fxCollider.center)) < fxCollider.radius) {
                                vertexIndex.Add(fV[i]);
                            }
                        }

                        targetFur.fxModule.ApplyEffect((int)effectToApply, vertexIndex.ToArray(), effectIntensity);

                    }
                }

                timer += Random.Range(0.1f, 0.5f);

            }
        }





        private void OnDestroy() {
            if (fxCollider) {
                fxCollider.hideFlags = HideFlags.None;

            }
        }

    }
}