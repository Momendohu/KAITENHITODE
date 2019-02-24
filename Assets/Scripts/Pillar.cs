using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Pillar : MonoBehaviour {
    public bool IsIndicate;

    //=============================================================
    private GameObject rotateArrow;

    private Coroutine coroutine;
    private bool onceIndicate;

    //=============================================================
    private void Init () {
        rotateArrow = transform.Find("Indication/RotateArrow").gameObject;
    }

    private void Awake () {
        Init();
    }

    private void Update () {
        if(IsIndicate) {
            if(!onceIndicate) {
                onceIndicate = true;

                coroutine = StartCoroutine(RotateArrow(new Vector3(0,0,-3)));
            }
        } else {
            onceIndicate = false;
            if(!(coroutine == null)) StopCoroutine(coroutine);
        }
    }

    private IEnumerator RotateArrow (Vector3 speed) {
        while(true) {
            rotateArrow.transform.eulerAngles += speed;
            yield return null;
        }
    }
}