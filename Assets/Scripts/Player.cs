﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
    public enum ERotateState {
        None = 0,
        InX = 1,
        InY = 2,
        InZ = 3,
    }

    [System.NonSerialized]
    public ERotateState RotateState = ERotateState.None;

    public GameObject NearPillar;

    //=============================================================
    private Rigidbody _rigidbody;

    private Vector3 prevRotatePos;

    private bool isStartedRotate = false;
    private bool isEndedRotate = true;
    private float rotateRad;

    private Vector3 speed = new Vector3(0.5f,0,0); //移動スピード
    private Vector3 jumpPower = new Vector3(0,10,0); //ジャンプスピード
    private float rotateSpeed = 4; //回転スピード
    private float centrifugalForce = 5; //遠心力
    //=============================================================
    private void Init () {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        //看板を動かす
        switch(RotateState) {
            case ERotateState.InX:
            case ERotateState.InY:
            case ERotateState.InZ:
            if(NearPillar) NearPillar.transform.parent.GetComponent<Pillar>().IsIndicate = true;

            break;

            default:
            break;
        }


        //Debug.Log(rotateRad);
        //回転
        if(InputUtil.IsPushButton(KeyCode.Space)) {
            //共通処理
            switch(RotateState) {
                case ERotateState.InX:
                case ERotateState.InY:
                case ERotateState.InZ:
                if(!isStartedRotate) {
                    StartRotate();
                }

                rotateRad += rotateSpeed * (Time.fixedDeltaTime * 60);
                break;

                default:
                break;
            }

            //位置制御
            switch(RotateState) {
                case ERotateState.InX:
                _rigidbody.position = new Vector3(
                    prevRotatePos.x,
                    NearPillar.transform.position.y + (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Cos(rotateRad * Mathf.Deg2Rad),
                    (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Sin(rotateRad * Mathf.Deg2Rad)
                    );
                break;

                case ERotateState.InY:
                _rigidbody.position = new Vector3(
                    NearPillar.transform.position.x + (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Cos(rotateRad * Mathf.Deg2Rad),
                    prevRotatePos.y,
                    (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Sin(rotateRad * Mathf.Deg2Rad)
                    );
                break;

                case ERotateState.InZ:
                break;

                default:
                break;
            }

        } else {
            if(!isEndedRotate) {
                switch(RotateState) {
                    case ERotateState.InX:
                    case ERotateState.InY:
                    case ERotateState.InZ:
                    _rigidbody.useGravity = true;
                    isStartedRotate = false;
                    isEndedRotate = true;
                    break;

                    default:
                    break;
                }

                switch(RotateState) {
                    case ERotateState.InX:
                    if(IsFrontArea()) {
                        _rigidbody.position = new Vector3(
                            prevRotatePos.x,
                            NearPillar.transform.position.y + (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Cos(0),
                            0
                            );
                    } else {
                        _rigidbody.position = new Vector3(
                            prevRotatePos.x,
                            NearPillar.transform.position.y + (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Cos(180),
                            0
                            );
                    }
                    break;

                    case ERotateState.InY:
                    if(IsFrontArea()) {
                        _rigidbody.position = new Vector3(
                            NearPillar.transform.position.x + (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Cos(0),
                            prevRotatePos.y,
                            0
                        );
                    } else {
                        _rigidbody.position = new Vector3(
                            NearPillar.transform.position.x + (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Cos(180),
                            prevRotatePos.y,
                            0
                        );
                    }
                    break;

                    case ERotateState.InZ:
                    break;

                    default:
                    break;
                }

                AddCentifugalForce(RotateState,IsFrontArea());
            }

            //左移動
            if(InputUtil.IsPushButton(KeyCode.LeftArrow)) {
                _rigidbody.velocity -= speed;
            }

            //右移動
            if(InputUtil.IsPushButton(KeyCode.RightArrow)) {
                _rigidbody.velocity += speed;
            }

            //上移動
            if(InputUtil.IsPushButtonDown(KeyCode.UpArrow)) {
                _rigidbody.velocity += jumpPower;
            }

            //z軸固定
            if(_rigidbody.position.z != 0) {
                Vector3 tmp = _rigidbody.position;
                _rigidbody.position = new Vector3(tmp.x,tmp.y,0);
            }
        }
    }

    //=============================================================
    /// <summary>
    /// 遠心力を加える
    /// </summary>
    private void AddCentifugalForce (ERotateState rotateState,bool isFront) {
        int num = isFront ? 1 : -1;

        switch(rotateState) {
            case ERotateState.InX:
            if((prevRotatePos.y - NearPillar.transform.position.y) > 0) {
                _rigidbody.velocity = new Vector3(0,rotateSpeed * centrifugalForce * num,0);
            } else {
                _rigidbody.velocity = new Vector3(0,-rotateSpeed * centrifugalForce * num,0);
            }
            break;

            case ERotateState.InY:
            if((prevRotatePos.x - NearPillar.transform.position.x) > 0) {
                _rigidbody.velocity = new Vector3(rotateSpeed * centrifugalForce * num,0,0);
            } else {
                _rigidbody.velocity = new Vector3(-rotateSpeed * centrifugalForce * num,0,0);
            }
            break;

            case ERotateState.InZ:
            break;
        }
    }

    //=============================================================
    /// <summary>
    /// 正面にいるかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsFrontArea () {
        return (rotateRad % 360 >= 0 && rotateRad % 360 <= 90) || (rotateRad % 360 >= 270 && rotateRad % 360 <= 360);
    }

    //=============================================================
    /// <summary>
    /// 回転開始処理
    /// </summary>
    private void StartRotate () {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.useGravity = false; //重力を切る
        isEndedRotate = false; //回転終了判定の初期化
        isStartedRotate = true; //回転開始判定
        rotateRad = 0; //回転角の初期化
        prevRotatePos = transform.position; //回転前の位置を保存
    }

    //=============================================================
    private void OnTriggerEnter (Collider other) {
        if(RotateState == ERotateState.None) {
            if(other.tag == "RotateAreaX") {
                RotateState = ERotateState.InX;
                NearPillar = other.gameObject;
            }

            if(other.tag == "RotateAreaY") {
                RotateState = ERotateState.InY;
                NearPillar = other.gameObject;
            }
        }
    }

    private void OnTriggerStay (Collider other) {
        if(RotateState == ERotateState.None) {
            if(other.tag == "RotateAreaX") {
                RotateState = ERotateState.InX;
                NearPillar = other.gameObject;
            }

            if(other.tag == "RotateAreaY") {
                RotateState = ERotateState.InY;
                NearPillar = other.gameObject;
            }
        }
    }

    private void OnTriggerExit (Collider other) {
        if(other.tag == "RotateAreaX" || other.tag == "RotateAreaY") {
            if(NearPillar) NearPillar.transform.parent.GetComponent<Pillar>().IsIndicate = false;
            NearPillar = null;
            RotateState = ERotateState.None;
        }
    }
}