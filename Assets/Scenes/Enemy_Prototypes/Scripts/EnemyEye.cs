using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : MonoBehaviour
{
    public int vision = 5;//視界
    public GameObject target;//標的
    public float speed = 1;
    SphereCollider scope;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        scope = GetComponent<SphereCollider>();
        rb = GetComponentInParent<Rigidbody>();//親のRigidbody
        //Debug.Log(rb);
        scope.radius = this.vision;//視界の反映
        this.target = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {//標的を見つけているなら、
            rb.useGravity = false;
            Vector3 position = (target.transform.position - this.transform.position);
            rb.AddForce(position * speed);
            //Debug.Log(position);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.target = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            this.target = null;
        }
    }

}
