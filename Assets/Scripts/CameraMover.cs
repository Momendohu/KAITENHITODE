using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class CameraMover : MonoBehaviour {
    //=============================================================
    private float easingSpeed = 0.2f; //カメラ追従速度
    private Vector3 fixY = Vector3.up * 0.5f; //y軸補正(カメラを少し上にするため)
    private Vector3 fixZ = Vector3.back * 2; //z軸補正(描画されなくなるため)

    public GameObject attention; //注目オブジェクト
    private Camera cam; //カメラ

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        cam = this.GetComponent<Camera>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        if(!(attention == null)) {
            this.transform.position = Vector3.Lerp(this.transform.position,attention.transform.position,easingSpeed) + fixY + fixZ;
        }
    }
}