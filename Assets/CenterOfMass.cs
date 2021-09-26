using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
       // rb = GetComponent<Rigidbody2D>();
        //rb.centerOfMass = Vector2.zero;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb.centerOfMass = Vector2.zero;

    }
}
