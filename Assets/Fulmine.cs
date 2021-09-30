using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fulmine : MonoBehaviour
{
    Vector3 dir; //direzione fulmine
    [SerializeField] GameObject player; //target da seguire
    [SerializeField] PlayerHealth ph;

    public float tempoPercorso; //in quanto tempo raggiunge il target
    Vector2 endFulminePos; //posizione finale del fulmine
    Vector2 fulmineStartPos; //posizione inziaile del fulmine

    Vector2 fulmineStartScale; //scala iniziale fulmine
    Vector2 respawnScale; // x rimpicciolimento nel momento del respawn

    bool gira = true; //per far seguire gli spostamenti del player, ma solo quando non è ancora lanciato verso di lui
    bool canHit = false; //voglio che hitti il player solo quando è in fase di lancio

    bool doubleHitCheck=true;//evitare collisioni multiple

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && canHit )
        {
            if( doubleHitCheck==true)
            StartCoroutine(timerHit()); //rendo false la bool doublehitcheck per 1.5f evitare collisioni multiple
           
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canHit)
        {
            if (doubleHitCheck == true)
                StartCoroutine(timerHit()); //rendo false la bool doublehitcheck per 1.5f evitare collisioni multiple

        }
    }

    IEnumerator timerHit()
    {
        //ph.ChangeHp(-1);
        doubleHitCheck = false;
        yield return new WaitForSeconds(1.5f);
        doubleHitCheck = true;
        yield return null;
    }

    private void Start()
    {
        fulmineStartScale = transform.localScale;
        fulmineStartPos = transform.position;
        respawnScale = new Vector2(0.3f,transform.localScale.y);
    }

    IEnumerator Spara()
    {
        endFulminePos = player.transform.position; //prendiamo l ultima ps del player
        
        //non vogliamo che una volta sparato il fulmine continui a seguire il player durante il movimento
        float elapsedTime = 0;

      //  yield return new WaitForSeconds(2f);

        gira = false;

        while (elapsedTime < tempoPercorso)
        {
            canHit = true;
            elapsedTime += Time.deltaTime;

            //il fulmine va verso il player
            transform.position = Vector2.Lerp(fulmineStartPos, endFulminePos, elapsedTime / tempoPercorso);

            yield return null;

        }

        transform.position = endFulminePos; //forziamo la sua posizione per sicurezza

        StartCoroutine(Reset()); //riposizioniamo il fulmine
        gira = true; //adesso il fulmine può ritornare a seguire il player
        yield return null;
    }



    IEnumerator Reset()
    {
        canHit = false;
        transform.position = fulmineStartPos; //riposizioniamo il fulmine dove si trovava all inizio
        transform.localScale = respawnScale; //lo rimpiccioliamo
        float elapsedTime = 0;
        float waitTime = 0.5f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale= Vector2.Lerp(respawnScale, fulmineStartScale, elapsedTime / waitTime); //ritorna in mezzo secondo alla sua scala originale

            yield return null;

        }
        transform.position = fulmineStartPos; //forziamo il suo riposizionamento per sicurezza

        transform.localScale = fulmineStartScale; //forziamo la sua scala per sicurezza

        yield return new WaitForSeconds(2f); //dopo due sec riparte il ciclo

        StartCoroutine(Spara());

        yield return null;
    }

    IEnumerator AttivaFulmine() //timer forse provvisorio per attivare la coroutine sparo nell'onenable()
    {
        yield return new WaitForSeconds(4);
        StartCoroutine(Spara());
        yield return null;
    }

    private void OnEnable()
    {
        StartCoroutine(AttivaFulmine());
 
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.M)) //per test
        {
            endFulminePos = player.transform.position;
            StartCoroutine(Spara());
        }

        //la direzione che deve seguire il fulmine, sottraiamo alla pos del player la sua pos
        Vector3 dir = player.transform.position - transform.position; 
        //ricaviamo l'angolo in gradi
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Debug.Log("angolo " + angle);

        //impostiamo un range di rotazione
        if (gira)//deve girare solo quando non è in fase di lancio
        {

           //nota: valori "messi a sentimento" per evitare che il fumine ruoti di 360 gradi
            if (angle < -140 || angle > 150f)
            {
                angle = -140f;
            }
              if (angle > -25 )
            {
                angle = -25f;
            }

            transform.localRotation = Quaternion.Euler(0f, 0f, angle); //il fulmine ruota in direzione del player
        }

        
    }




}
