using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivelloUno : MonoBehaviour
{
    public GameObject piattaforma1, piattaforma2;
    Vector2 p1StartPos, p2StartPos;
    public Vector2 p1TargetPos, p2TargetPos;

    public Vector2 p1StartRot;

    public GameObject cactus;
    public Vector2 cactusStartPos;
    public Vector2 cactusStartRot;
    public Vector2 cactusTargetRot = new Vector2(0, 0);

    [SerializeField] GameObject lunaSole;
    Vector2 lunaSoleStartPos;
    public Vector2 lunaSoleTargetPos;

    public GameObject[] backgrounds;
    public GameObject player;


    public IEnumerator AttivazioneCactus()
    {
        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        float waitTime = 3.5f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            cactus.transform.rotation= Quaternion.Lerp(cactus.transform.rotation, Quaternion.AngleAxis (cactusTargetRot.x, Vector2.right), elapsedTime/waitTime * .5f);
 
            yield return null;

        }

        yield return null;
    }

    public IEnumerator AttivazioneLuna()
    {
        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;
        float waitTime = 1f;


        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            lunaSole.transform.localPosition = Vector2.Lerp(lunaSoleStartPos, lunaSoleTargetPos, elapsedTime / waitTime );
            
            yield return null;

        }

        yield return null;
    }

    public IEnumerator AttivazionePedane()
    {
        float elapsedTime = 0f;
        float waitTime = 2.5f;


        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
          
            piattaforma1.transform.position = Vector2.Lerp(p1StartPos, p1TargetPos, elapsedTime / waitTime * .5f);
            piattaforma2.transform.position= Vector2.Lerp(p2StartPos, p2TargetPos, elapsedTime / waitTime * .8f);

            yield return null;

        }

        yield return null;
    }

    private void Awake()
    {
        p1StartPos = piattaforma1.transform.position;

        p2StartPos = piattaforma2.transform.position;

        piattaforma1.SetActive(false);
        piattaforma2.SetActive(false);

        lunaSoleStartPos = lunaSole.transform.localPosition;
    }

    private void OnEnable()
    {
    }

    void InitLivello1()
    {
        piattaforma1.SetActive(true);
        piattaforma2.SetActive(true);

        StartCoroutine(AttivazioneCactus());
        StartCoroutine(AttivazionePedane());
        StartCoroutine(AttivazioneLuna());

    }

    void Start()
    {
        InitLivello1();
    }


    void MoveBackground(GameObject go, float _velocity)
    {
        go.transform.position = new Vector3(-player.transform.position.x  * _velocity, go.transform.position.y, go.transform.position.z);
    }


    void LateUpdate()
    {
        
        if (backgrounds[0] != null)
        {
            MoveBackground(backgrounds[0], 0.4f);
        }

        if (backgrounds[1] != null)
        {
            MoveBackground(backgrounds[1], 0.1f);
        }

        if (backgrounds[2] != null)
        {
            MoveBackground(backgrounds[2], 0.05f);
        }

    }
}
