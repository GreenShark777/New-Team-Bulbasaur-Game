using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassaItem : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject cassaAperta;
    [SerializeField] ParticleSystem ps;

    bool isClose = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isClose == true)
        {
            //isClose = false;
            Debug.Log("ACCHIAPPA");
            cassaAperta.SetActive(false);
            StartCoroutine(Particellare());

        }
    }


    IEnumerator Particellare()
    {

        ps.Play();

        yield return new WaitForSeconds(1);

        isClose = false;

        ps.Stop();

        yield return null;
    }

    void Start()
    {
        cassaAperta.SetActive(true);
    }


    void Update()
    {
        
    }
}
