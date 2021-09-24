using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassaItem : MonoBehaviour
{

    GameObject player;
    [SerializeField] GameObject cassaAperta;
    [SerializeField] ParticleSystem ps;

   
    public  GameObject[] itemdDroppabili;

    bool isClose = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isClose == true)
        {
            cassaAperta.SetActive(false);
            ItemDrop();
            StartCoroutine(Particellare());

        }
    }

    void ItemDrop()
    {
        float scelta = Random.value;

        if (scelta < 0.5f)
        {
            itemdDroppabili[0].SetActive(true);
            StartCoroutine(MoveItem(itemdDroppabili[0]));

            itemdDroppabili[1].SetActive(false);
        }
        else
        {
            itemdDroppabili[0].SetActive(false);
            StartCoroutine(MoveItem(itemdDroppabili[1]));

            itemdDroppabili[1].SetActive(true);
        }


    }

    IEnumerator MoveItem(GameObject item) //animazione item una volta aperta la cassa
    {
        float elapsedTime = 0f;
        float waitTime = 8f;

        SpriteRenderer sprite = item.GetComponent<SpriteRenderer>();
        float alpha = sprite.material.color.a;

        float targetAlpha = 0f;

        Vector2 targetPos = new Vector2(item.transform.position.x, item.transform.position.y+1f);
        
        while (elapsedTime< waitTime)
        {
            elapsedTime += Time.deltaTime;

            item.transform.position = Vector2.Lerp(item.transform.position, targetPos, elapsedTime / waitTime * 0.5f);

            Color newColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(alpha, targetAlpha, elapsedTime / waitTime * 6));

            sprite.material.color = newColor;

            yield return null;
        }
        item.transform.position = targetPos;

        yield return null;
    }

    IEnumerator Particellare()
    {

        ps.Play();

        yield return new WaitForSeconds(1);

        isClose = false;

        ps.Stop();

        yield return null;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach (GameObject item in itemdDroppabili)
        {
            item.SetActive(false);
        }

    }

    void Start()
    {
        cassaAperta.SetActive(true);

        foreach (GameObject item in itemdDroppabili)
        {
            item.SetActive(false);
        }

    }


    void Update()
    {
        
    }
}
