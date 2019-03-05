using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject EnemyPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player")
        {
            Instantiate(EnemyPrefab, new Vector3(Random.Range(-20, 20), 1, 0), new Quaternion(0, 0, 0, 0));
            Destroy(this.gameObject);
        }
    }
}
