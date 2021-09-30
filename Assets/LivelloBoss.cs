using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivelloBoss : MonoBehaviour
{
    public GameObject piattaforma2;
    Vector2 p1StartPos, p2StartPos;
    public Vector2 p1TargetPos, p2TargetPos;

    public Vector2 p1StartRot;

    public GameObject genitori;
    public Vector2 genitoriStartPos;
    public Vector2 genitoriEndPos;

    [SerializeField] GameObject casetta;
    public Vector2 casettaStartPos;
    public Vector2 casettaEndPos;

    [SerializeField] GameObject lunaSole;
    public Vector2 lunaSoleStartPos;
    public Vector2 lunaSoleTargetPos;

    [SerializeField]GameObject luce1;
    [SerializeField] GameObject luce2;

    public GameObject[] backgrounds;

    public GameObject followPlayer;

    bool isStarted = false;


    [SerializeField] float tempoAttivazioneGenitori = 3f;
    [SerializeField] float tempoAttivazioneLuna = 3.5f;
    [SerializeField] float tempoAttivazioneLuci = 6;


    public IEnumerator AttivazioneGenitori()
    {
        yield return new WaitForSeconds(tempoAttivazioneGenitori);

        float elapsedTime = 0f;
        float waitTime = 1f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            genitori.transform.localPosition = Vector2.Lerp(genitoriStartPos, genitoriEndPos, elapsedTime / waitTime);

            yield return null;

        }

        genitori.transform.localPosition = genitoriEndPos;

       yield return null;
    }

    public IEnumerator AttivazioneCasetta()
    {
        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;
        float waitTime = 2f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            casetta.transform.localPosition = Vector2.Lerp(casettaStartPos, casettaEndPos, elapsedTime / waitTime*2);
 
            yield return null;

        }

        casetta.transform.localPosition = casettaEndPos;

        yield return null;
    }

    public IEnumerator AttivazioneLuna()
    {
        yield return new WaitForSeconds(tempoAttivazioneLuna);

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

            //piattaforma1.transform.position = Vector2.Lerp(p1StartPos, p1TargetPos, elapsedTime / waitTime * .5f);
            piattaforma2.transform.position = Vector2.Lerp(p2StartPos, p2TargetPos, elapsedTime / waitTime * .8f);

            yield return null;

        }

        yield return null;
    }

    
    IEnumerator AttivazioneLuci()
    {
        yield return new WaitForSeconds(tempoAttivazioneLuci);
        luce1.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        luce2.SetActive(true);

        yield return null;

    }

    private void Awake()
    {
        //p1StartPos = piattaforma1.transform.position;

        p2StartPos = piattaforma2.transform.position;

        luce1.SetActive(false);
        luce2.SetActive(false);

        //lunaSoleStartPos = lunaSole.transform.localPosition;

        genitori.SetActive(false);
        casetta.SetActive(false);
        //piattaforma1.SetActive(false);
        piattaforma2.SetActive(false);


    }

    private void OnEnable()
    {
 
    }

    void InitLivello_Boss()
    {
        AttivazioneBackground(backgrounds);

        //piattaforma1.SetActive(true);
        piattaforma2.SetActive(true);
        casetta.SetActive(true);
        genitori.SetActive(true);

        StartCoroutine(AttivazioneGenitori());
        StartCoroutine(AttivazioneCasetta());
        StartCoroutine(AttivazioneNuvole());
        StartCoroutine(AttivazioneLuna());
        StartCoroutine(AttivazioneLuci());
        StartCoroutine(timer());

    }

    void Start()
    {
       InitLivello_Boss();
    }


    /////LOOP BACKGROUND
    ///// AL 99% NON SERVIRA' E POTREMO TOGLIERLO, SERVE A LOOPARE ALL'INFINTO IL BACKGROUND, MA ADESSO
    ///NON CI SERVE
    ///
    Vector2 screendBounds;
    [SerializeField] Camera mainCamera;
    /*
    void LoadChildsObj(GameObject go)
    {
        float goWidth = go.GetComponent<SpriteRenderer>().bounds.size.x;
        int goNeeded = (int)Mathf.Ceil(screendBounds.x * 2 / goWidth);

        GameObject clone = Instantiate(go) as GameObject;

        for (int i = 0; i <= goNeeded; i++)
        {
            GameObject c = Instantiate(clone) as GameObject;
            c.transform.SetParent(go.transform);
            c.transform.position = new Vector3(goWidth * i, go.transform.position.y, go.transform.position.z);
            c.name = go.name + i;
        }

        Destroy(clone);
        Destroy(go.GetComponent<SpriteRenderer>());

        //Move(go, 10f);
    }

    GameObject _go;

    void RepositionChild(GameObject go)
    {
        _go = go;

        Transform[] children = _go.GetComponentsInChildren<Transform>();

        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild = children[children.Length - 1].gameObject;

            float halfObj = lastChild.GetComponent<SpriteRenderer>().bounds.extents.x;

            if (transform.position.x + screendBounds.x > lastChild.transform.position.x + halfObj)
            {
                firstChild.transform.SetAsLastSibling();

                firstChild.transform.position = new Vector3(lastChild.transform.position.x + halfObj * 2, lastChild.transform.position.y, lastChild.transform.position.z);
            }
            else if (transform.position.x - screendBounds.x < firstChild.transform.position.x - halfObj)
            {
                lastChild.transform.SetAsFirstSibling();

                lastChild.transform.position = new Vector3(firstChild.transform.position.x - halfObj * 2, firstChild.transform.position.y, firstChild.transform.position.z);

            }

            // Move(porco);

            Debug.Log("CHISTU" + _go.name);

        }
    }

   

    private void LateUpdate()
    {
        foreach (GameObject go in backgrounds)
        {
            RepositionChild(go);
        }
    }
    */

   
    void AttivazioneBackground(GameObject[] _backgrounds)
    {

        for (int i = 0; i < backgrounds.Length; i++)
        {
            StartCoroutine(PrimoMovimentoBackgrounds(backgrounds[i]));
        }

    }

    //muoviamo/ruotiamo i background quando il livello viene caricato
    IEnumerator PrimoMovimentoBackgrounds(GameObject go)
    {
        float randomDelay = Random.Range(1.0f, 3.0f); //per diversificare le velocità di comparsa dei background

        yield return new WaitForSeconds(2f);

        Quaternion startRot = Quaternion.Euler(90, 0, 0);
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        float elapsedTime = 0f;
        float waitTime = 1f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            go.transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / waitTime * randomDelay);

            //Debug.Log("POPOPO " + go.name + transform.position);

            yield return null;
        }

        go.transform.rotation = endRot;

        yield return null;
    }

    //non voglio che i background seguano l'inverso del mov del player (finta parallasse) nel momento stesso in cui compaiono in scena
    IEnumerator timer()
    {
        yield return null;
        float elapsedTime = 0f;
        float waitTime = 4f;

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


     void LateUpdate() //mi sembra funzionare meglio rispetto a update
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
