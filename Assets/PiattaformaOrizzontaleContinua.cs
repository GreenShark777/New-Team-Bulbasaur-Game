using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaOrizzontaleContinua : MonoBehaviour
{
    Vector2 start;
    Vector2 end;
    public float tempoAttesaAttivazione;

    public float movimentoX = 4.5f;
    public float tempoPercorso;

    void Start()
    {
        start = transform.position;
        end = new Vector2(start.x + movimentoX, start.y );

    }

    IEnumerator MoveLeft()
    {
        yield return new WaitForSeconds(tempoAttesaAttivazione);

        float elapsedTime = 0f;

        while (elapsedTime < tempoPercorso)
        {
            elapsedTime += Time.deltaTime;

            transform.localPosition = Vector2.Lerp(start, end, elapsedTime / tempoPercorso);

            yield return null;

        }
        transform.localPosition = end;
        StartCoroutine(MoveRight());
        yield return null;
    }

    IEnumerator MoveRight()
    {
        yield return new WaitForSeconds(tempoAttesaAttivazione);

        float elapsedTime = 0f;

        while (elapsedTime < tempoPercorso)
        {
            elapsedTime += Time.deltaTime;

            transform.localPosition = Vector2.Lerp(end, start, elapsedTime / tempoPercorso);

            yield return null;

        }

        transform.localPosition = start;
        StartCoroutine(MoveLeft());
        yield return null;

    }

    private void OnEnable()
    {
        StartCoroutine(MoveRight());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
