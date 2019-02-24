using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
    //[System.NonSerialized]
    public bool IsInRotateAreaX;
    public bool IsInRotateAreaY;
    public GameObject NearPillar;

    //=============================================================
    private Rigidbody rigidbody;

    private Vector3 prevRotatePos;
    private bool isStartedRotateX = false;
    private bool isEndedRotateX = true;
    private bool isStartedRotateY = false;
    private bool isEndedRotateY = true;
    private float rotateRad;

    private Vector3 speed = new Vector3(0.5f,0,0); //移動スピード
    private Vector3 jumpPower = new Vector3(0,10,0); //ジャンプスピード
    private float rotateSpeed = 4; //回転スピード
    private float centrifugalForce = 5; //遠心力
    //=============================================================
    private void Init () {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        if(rigidbody.position.z != 0) {
            Vector3 tmp = rigidbody.position;
            rigidbody.position = new Vector3(tmp.x,tmp.y,0);
        }

        //看板を動かす
        if(IsInRotateAreaX || IsInRotateAreaY) {
            if(NearPillar) NearPillar.transform.parent.GetComponent<Pillar>().IsIndicate = true;
        }

        //回転
        if(InputUtil.IsPushButton(KeyCode.Space)) {
            if(IsInRotateAreaX) {
                if(!isStartedRotateX) {
                    rigidbody.useGravity = false;
                    isEndedRotateX = false;
                    isStartedRotateX = true;
                    rotateRad = 0;
                    prevRotatePos = transform.position;
                }

                rotateRad += rotateSpeed;
                rigidbody.position = new Vector3(
                    prevRotatePos.x,
                    NearPillar.transform.position.y + (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Cos(rotateRad * Mathf.Deg2Rad),
                    (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Sin(rotateRad * Mathf.Deg2Rad)
                    );
            }

            if(IsInRotateAreaY) {
                if(!isStartedRotateY) {
                    rigidbody.useGravity = false;
                    isEndedRotateY = false;
                    isStartedRotateY = true;
                    rotateRad = 0;
                    prevRotatePos = transform.position;
                }

                rotateRad += rotateSpeed;
                rigidbody.position = new Vector3(
                    NearPillar.transform.position.x + (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Cos(rotateRad * Mathf.Deg2Rad),
                    prevRotatePos.y,
                    (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Sin(rotateRad * Mathf.Deg2Rad)
                    );
            }
        } else {
            if(!isEndedRotateX) {
                if((rotateRad % 360 >= 0 && rotateRad % 360 <= 90) || (rotateRad % 360 >= 270 && rotateRad % 360 <= 360)) {
                    rigidbody.position = new Vector3(
                    prevRotatePos.x,
                    NearPillar.transform.position.y + (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Cos(rotateRad * Mathf.Deg2Rad),
                    0
                    );

                    if((prevRotatePos.y - NearPillar.transform.position.y) > 0) {
                        rigidbody.velocity = new Vector3(0,centrifugalForce,0);
                    } else {
                        rigidbody.velocity = new Vector3(0,centrifugalForce,0);
                    }

                } else {
                    rigidbody.position = new Vector3(
                    prevRotatePos.x,
                    NearPillar.transform.position.y + (prevRotatePos.y - NearPillar.transform.position.y) * Mathf.Cos(rotateRad * Mathf.Deg2Rad),
                    0
                    );

                    if((prevRotatePos.y - NearPillar.transform.position.y) > 0) {
                        rigidbody.velocity = new Vector3(0,-rotateSpeed * centrifugalForce,0);
                    } else {
                        rigidbody.velocity = new Vector3(0,rotateSpeed * centrifugalForce,0);
                    }
                }

                rigidbody.useGravity = true;
                isStartedRotateX = false;
                isEndedRotateX = true;
            }

            if(!isEndedRotateY) {
                if((rotateRad % 360 >= 0 && rotateRad % 360 <= 90) || (rotateRad % 360 >= 270 && rotateRad % 360 <= 360)) {
                    rigidbody.position = new Vector3(
                    NearPillar.transform.position.x + (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Cos(0),
                    prevRotatePos.y,
                    0
                    );

                    if((prevRotatePos.x - NearPillar.transform.position.x) > 0) {
                        rigidbody.velocity = new Vector3(rotateSpeed * centrifugalForce,0,0);
                    } else {
                        rigidbody.velocity = new Vector3(-rotateSpeed * centrifugalForce,0,0);
                    }

                } else {
                    rigidbody.position = new Vector3(
                    NearPillar.transform.position.x + (prevRotatePos.x - NearPillar.transform.position.x) * Mathf.Cos(180),
                    prevRotatePos.y,
                    0
                    );

                    if((prevRotatePos.x - NearPillar.transform.position.x) > 0) {
                        rigidbody.velocity = new Vector3(-rotateSpeed * centrifugalForce,0,0);
                    } else {
                        rigidbody.velocity = new Vector3(rotateSpeed * centrifugalForce,0,0);
                    }
                }

                rigidbody.useGravity = true;
                isStartedRotateY = false;
                isEndedRotateY = true;
            }

            //左移動
            if(InputUtil.IsPushButton(KeyCode.LeftArrow)) {
                rigidbody.velocity -= speed;
            }

            //右移動
            if(InputUtil.IsPushButton(KeyCode.RightArrow)) {
                rigidbody.velocity += speed;
            }

            //上移動
            if(InputUtil.IsPushButtonDown(KeyCode.UpArrow)) {
                rigidbody.velocity += jumpPower;
            }
        }
    }

    //=============================================================
    private void OnTriggerEnter (Collider other) {
        if(other.tag == "RotateAreaX") {
            IsInRotateAreaX = true;
            NearPillar = other.gameObject;
        }

        if(other.tag == "RotateAreaY") {
            IsInRotateAreaY = true;
            NearPillar = other.gameObject;
        }
    }

    private void OnTriggerStay (Collider other) {
        if(other.tag == "RotateAreaX") {
            IsInRotateAreaX = true;
            NearPillar = other.gameObject;
        }

        if(other.tag == "RotateAreaY") {
            IsInRotateAreaY = true;
            NearPillar = other.gameObject;
        }
    }

    private void OnTriggerExit (Collider other) {
        if(other.tag == "RotateAreaX") {
            NearPillar.transform.parent.GetComponent<Pillar>().IsIndicate = false;
            NearPillar = null;
            IsInRotateAreaX = false;
        }

        if(other.tag == "RotateAreaY") {
            NearPillar.transform.parent.GetComponent<Pillar>().IsIndicate = false;
            NearPillar = null;
            IsInRotateAreaY = false;
        }
    }
}