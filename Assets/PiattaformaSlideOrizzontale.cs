using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaSlideOrizzontale : MonoBehaviour
{
    [SerializeField] GameObject pedana;
    Vector2 start;
    Vector2 end;

    public float movimentoX = 4.5f;
    public float tempoPercorso=4;

    

    IEnumerator Slide()
    {
        float elapsedTime=0;

        while (elapsedTime < tempoPercorso)
        {
            elapsedTime += Time.deltaTime;

            pedana.transform.position = Vector2.Lerp(start, end, elapsedTime / tempoPercorso);

            yield return null;

        }

        pedana.transform.position = end;
        pedana.gameObject.SetActive(false);
        StartCoroutine(Reset());

       yield return null;

    }

    IEnumerator Reset()
    {
        pedana.transform.position = start;
        pedana.SetActive(true);

        StartCoroutine(Slide());
        yield return null;

    }

    private void OnEnable()
    {
        StartCoroutine(Slide());
    }

    void Awake()
    {
        start = pedana.transform.position;
        end = new Vector2(start.x + movimentoX, start.y);
    }

   

}
