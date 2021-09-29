using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendolo : MonoBehaviour
{
    [SerializeField] GameObject pedana;

    Vector2 start;
    Vector2 end;

    public float tempoAttesa = 2f;
    public float amplitude = 70f;

    private void Awake()
    {
        start = transform.position;
        end = new Vector2(0, start.y - 6 );

    }


    private void OnEnable()
    {
        StartCoroutine(ObjectRotate());
    }
    

    IEnumerator ObjectRotate()
    {
        yield return new WaitForSeconds(tempoAttesa);

        float timer = 0;
        while (true) //foreva
        {
            float angle = Mathf.Sin(timer) * amplitude;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            timer += Time.deltaTime;
            yield return null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //forza la pedana (figlia dell'oggetto a cui è attaccato questo script)
        //a non seguire la rotazione del proprio parent (un empty che fa da perno)  
        if (pedana != null)
        {
            pedana.transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.z * -1.0f);
        }
  
    }
}
