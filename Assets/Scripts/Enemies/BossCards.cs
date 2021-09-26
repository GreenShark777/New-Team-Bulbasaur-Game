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
    //vettore che indica la direzione in cui questa carta sta andando
    private Vector2 cardDirection;
    //indica il magnitudo massimo che la velocity del rigidbody della carta può avere
    private float maxMagnitude;
    //indica la tolleranza di velocità massima della carta
    [SerializeField]
    private float magnitudeTolerance = 0.5f;


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
        //se la carta va troppo veloce, viene riportato alla velocità originale
        if (maxMagnitude != 0 && cardRb.velocity.magnitude > maxMagnitude)
        {
            //Debug.Log("Stava andando troppo veloce -> " + maxMagnitude + " : " + cardRb.velocity.magnitude);
            //calcola la velocità a cui deve andare la carta
            float directionX = Mathf.Clamp(cardRb.velocity.x, -cardDirection.x, cardDirection.x);
            float directionY = Mathf.Clamp(cardRb.velocity.y, -cardDirection.y, cardDirection.y);
            //imposta la velocità della carta a valori controllati
            cardRb.velocity = new Vector2(directionX, directionY);
        
        }
        //Debug.Log(cardRb.velocity.magnitude);
    }

    public void LaunchCard(Vector2 launchDirection)
    {
        //se questa carta è stata disattivata, la riattiva
        if (!gameObject.activeSelf) { gameObject.SetActive(true); }
        //fa andare la carta verso la direzione ottenuta come parametros
        cardRb.velocity = launchDirection;
        //prende come riferimento la direzione di lancio
        cardDirection = launchDirection;
        //ottiene la velocità massima
        maxMagnitude = launchDirection.magnitude + magnitudeTolerance;
        //Debug.Log(maxMagnitude);
    }

}
