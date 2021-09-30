using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivelloDue : MonoBehaviour
{
    public ParticleSystem psRain;
    public GameObject piattaforma1, piattaforma2;
    Vector2 p1StartPos, p2StartPos;
    public Vector2 p1TargetPos, p2TargetPos;

    public Vector2 p1StartRot;

    public GameObject albero;
    public Vector2 alberoStartPos;
    public Vector2 alberoStartRot;
    public Vector2 alberoTargetRot = new Vector2(0, 0);

    [SerializeField] GameObject lunaSole;
    Vector2 lunaSoleStartPos;
    public Vector2 lunaSoleTargetPos;

    public GameObject[] backgrounds;


    [SerializeField] float tempoAttivazioneAlbero = 3f;

    public GameObject followPlayer;

    bool isStarted = false;

    public IEnumerator AttivazioneAlbero()
    {
        yield return new WaitForSeconds(tempoAttivazioneAlbero);

        float elapsedTime = 0f;
        float waitTime = 3.5f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            albero.transform.rotation = Quaternion.Lerp(albero.transform.rotation, Quaternion.AngleAxis(alberoTargetRot.x, Vector2.right), elapsedTime / waitTime * .5f);

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

            lunaSole.transform.localPosition = Vector2.Lerp(lunaSoleStartPos, lunaSoleTargetPos, elapsedTime / waitTime);

            yield return null;

        }

        yield return null;
    }

    [SerializeField] float tempoAttivazioneNuvole = 3.3f;
    public IEnumerator AttivazioneNuvole()
    {
        yield return new WaitForSeconds(tempoAttivazioneNuvole);

        float elapsedTime = 0f;
        float waitTime = 2.5f;


        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            piattaforma1.transform.position = Vector2.Lerp(p1StartPos, p1TargetPos, elapsedTime / waitTime * .5f);
            piattaforma2.transform.position = Vector2.Lerp(p2StartPos, p2TargetPos, elapsedTime / waitTime * .8f);

            yield return null;

        }

        yield return null;
    }

    public GameObject[] alberiSfondo;
 

    public IEnumerator AttivazioneAlberi()
    {
        yield return new WaitForSeconds(3f);

        float elapsedTime = 0f;
        float waitTime = 1.5f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            float randomDelay = Random.Range(0.0f, 1.0f); //per diversificare le velocità di comparsa dei background

            foreach (GameObject albero in alberiSfondo)
            { 
                //non so perchè funziona ma funziona. i valori su y nel lerp sono insensatamente piccoli, boh. e sto muovendo un parent...
                albero.transform.position = new Vector2 (albero.transform.position.x, Mathf.Lerp(albero.transform.position.y, albero.transform.position.y +0.1f, elapsedTime / waitTime * randomDelay));
            }
         
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

    void InitLivello_2()
    {
        AttivazioneBackground(backgrounds);

        psRain.Play();

        piattaforma1.SetActive(true);
        piattaforma2.SetActive(true);

        alberoStartPos = albero.transform.position;
        alberoStartRot = albero.transform.rotation.eulerAngles;

        StartCoroutine(AttivazioneAlbero());
        StartCoroutine(AttivazioneAlberi());
        StartCoroutine(AttivazioneNuvole());
        StartCoroutine(AttivazioneLuna());
        StartCoroutine(timer());

    }

    void Start()
    {

        InitLivello_2();
    }


    void AttivazioneBackground(GameObject[] _backgrounds)
    {

        for (int i = 0; i < backgrounds.Length; i++)
        {
            StartCoroutine(PrimoMovimentoBackgrounds(backgrounds[i]));
        }

    }

    IEnumerator PrimoMovimentoBackgrounds(GameObject go)
    {

        yield return new WaitForSeconds(1f);

        Vector2 startPos = go.transform.localPosition;
        Vector2 endPos = new Vector2(0, startPos.y);

        float elapsedTime = 0f;
        float waitTime = 1f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            go.transform.localPosition = Vector2.Lerp(startPos, endPos, elapsedTime / waitTime);

            yield return null;
        }

        go.transform.localPosition = endPos;

        yield return null;
    }

    IEnumerator timer()
    {
        yield return null;
        float elapsedTime = 0f;
        float waitTime = 6f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isStarted = true;
        yield return null;
    }

    void MoveBackground(GameObject go, float _velocity)
    {
        //nota: nella scena la ref player è un empty che segue via script il player(non uso direttamente il player per evitare conflitti quando questo è imparentato a una piattaforma)
        go.transform.position = new Vector3(-followPlayer.transform.position.x * _velocity, go.transform.position.y, go.transform.position.z);
    }

    //float velocityPlayer;

    void LateUpdate()
    {

        if (isStarted) 
        {
            if (backgrounds[0] != null)
            {
                MoveBackground(backgrounds[0], 0.15f);
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
}
