using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassaItem : MonoBehaviour
{

    [SerializeField] GameObject cassaChiusa; //disattivo questo gameobj e attivo cassachiusa
    [SerializeField] ParticleSystem ps;

    public  GameObject[] itemdDroppabili; //coin o cuore

    public bool droppatoCuore, droppatoCoin = false; //lo uso per il check nellio script playercollisionmanager
    public bool isClose = true; //per evitare operazioni multiple

    private void OnTriggerEnter2D(Collider2D other) //quando trova il player
    {
        if (other.CompareTag("Player") && isClose == true) //e deve essere la prima volta che succede
        {

           // isClose = false;
            cassaChiusa.SetActive(false); //disattivo l oggetto cassa aperta
            ItemDrop(); //droppo un item
            StartCoroutine(Particellare()); //inizia il particellare
        }
    }


    void ItemDrop()
    {
        //scelgo a caso far droppare alla cassa  tra i due elementi dell'array (50/50)
        float scelta = Random.value; 

        if (scelta < 0.5f) // se scelta è inferiore 0.5 droppa un coin
        {
            itemdDroppabili[0].SetActive(true);
            itemdDroppabili[1].SetActive(false); //mi assicuro che l item cuore rimanga disattivato

            StartCoroutine(MoveItem(itemdDroppabili[0])); //animazione di risalita e dissolvenza alpha

            droppatoCoin = true; //check da usare nel collisionmanager per decidere cosa aumentare tra current hp e score
           // Debug.Log("droppato coin");

           
        }
        else
        {
            itemdDroppabili[1].SetActive(true); //mi assicuro che l item coin sia disattivato
            itemdDroppabili[0].SetActive(false); //mi assicuro che l item coin rimanga disattivato
            StartCoroutine(MoveItem(itemdDroppabili[1])); //animazione di risalita e dissolvenza alpha

            droppatoCuore = true; // check da usare nel collisionmanager per decidere cosa aumentare tra current hp e score
             //Debug.Log("droppato cuore");

            
        }


    }

    IEnumerator MoveItem(GameObject item) //animazione item una volta aperta la cassa. l'item sale verso l'alto
    {
        float elapsedTime = 0f;
        float waitTime = 8f;

        //voglio inoltre che ci sia una animazione di dissolvenza
        SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
        float alpha = sprite.material.color.a; //parte a 255 alpha

        float targetAlpha = 0f;

        Vector2 targetPos = new Vector2(item.transform.position.x, item.transform.position.y+1f);
        
        while (elapsedTime< waitTime)
        {
            elapsedTime += Time.deltaTime;

            item.transform.position = Vector2.Lerp(item.transform.position, targetPos, elapsedTime / waitTime * 0.5f);

            //lerpo l'alpha a 255 verso 0
            Color newColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(alpha, targetAlpha, elapsedTime / waitTime * 6));

            //assegno alla sprite i valori di newColor
            sprite.material.color = newColor;

            yield return null;
        }

        //mi assicuro che l'item raggiunga la sua pos finale
        item.transform.position = targetPos;

        yield return null;
    }

    IEnumerator Particellare()
    {

        ps.Play();

        yield return new WaitForSeconds(1);


        ps.Stop();

        yield return null;
    }

    private void Awake()
    {
        cassaChiusa.SetActive(true);

        foreach (GameObject item in itemdDroppabili) //mi assicuro che gli item cuore e coin siano disattivati prima del detect della collisione col player
        {
            item.SetActive(false);
        }

    }


}
