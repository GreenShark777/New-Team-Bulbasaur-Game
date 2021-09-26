using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiattaformaVerticaleContinua : MonoBehaviour
{
    Vector2 start;
    Vector2 end;
    public float tempoAttesaAttivazione;

    public float discesaY=4.5f;

    public float tempoPercorso;

    void Awake()
    {
        start = transform.localPosition;
        end = new Vector2(start.x, start.y - discesaY);
 
    }

    IEnumerator MoveDown()
    {
        yield return new WaitForSeconds(tempoAttesaAttivazione);

        float elapsedTime = 0f;;

        while (elapsedTime < tempoPercorso)
        {
            elapsedTime += Time.deltaTime;

            transform.localPosition = Vector2.Lerp(start, end, elapsedTime / tempoPercorso);

            yield return null;

        }

        transform.localPosition = end;
        StartCoroutine(MoveUP());
        yield return null;
    }

    IEnumerator MoveUP()
    { yield return new WaitForSeconds(tempoAttesaAttivazione);

    float elapsedTime = 0f;

        while (elapsedTime<tempoPercorso)
        {
            elapsedTime += Time.deltaTime;

            transform.localPosition = Vector2.Lerp(end, start, elapsedTime / tempoPercorso);

            yield return null;

        }

        transform.localPosition = start;
        StartCoroutine(MoveDown());
        yield return null;

    }

    private void OnEnable()
    {
        StartCoroutine(MoveDown());
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
