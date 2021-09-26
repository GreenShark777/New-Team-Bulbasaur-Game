//Si occupa del comportamento delle carte del boss
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCards : MonoBehaviour
{
    //riferimento al Rigidbody2D di questa carta
    private Rigidbody2D cardRb;
    //riferimento allo sprite di questa carta
    private Transform cardSprite;
    //indica quanto velocemente gira la carta
    [SerializeField]
    private float rotationSpeed = 10;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento al Rigidbody2D di questa carta
        cardRb = GetComponent<Rigidbody2D>();
        //ottiene il riferimento allo sprite di questa carta
        cardSprite = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        //ruota continuamente lo sprite della carta nell'asse Z
        cardSprite.RotateAround(cardSprite.position, Vector3.forward, rotationSpeed);

    }

    public void LaunchCard(Vector2 launchDirection)
    {
        //fa andare la carta verso la direzione ottenuta come parametros
        cardRb.velocity = launchDirection;

    }

}
