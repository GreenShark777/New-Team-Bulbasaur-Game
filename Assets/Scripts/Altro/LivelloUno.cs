using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivelloUno : MonoBehaviour
{
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
 
    public IEnumerator AttivazionePiattaforme()
    {
        yield return new WaitForSeconds(3f);

        //InitLivello1();

        yield return null;
    }

    public IEnumerator AttivazioneAlbero()
    {
        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        float waitTime = 3.5f;

        while (elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;

            albero.transform.rotation= Quaternion.Lerp(albero.transform.rotation, Quaternion.AngleAxis (alberoTargetRot.x, Vector2.right), elapsedTime/waitTime * .5f);
 
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
        //    p1StartRot =  piattaforma1.transform.rotation.eulerAngles;
        p2StartPos = piattaforma2.transform.position;

        piattaforma1.SetActive(false);
        piattaforma2.SetActive(false);

        lunaSoleStartPos = lunaSole.transform.localPosition;
    }

    private void OnEnable()
    {
        //StartCoroutine(AttivazionePiattaforme());
        InitLivello1();
    }

    void InitLivello1()
    {
        piattaforma1.SetActive(true);
        piattaforma2.SetActive(true);

        StartCoroutine(AttivazioneAlbero());
        StartCoroutine(AttivazionePedane());
        StartCoroutine(AttivazioneLuna());
    }

    void Start()
    {
      
        alberoStartPos = albero.transform.position;
        alberoStartRot = albero.transform.rotation.eulerAngles;

        screendBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        foreach (GameObject go in backgrounds)
        {
            LoadChildsObj(go);
        }


    }


    /////LOOP BACKGROUND

    public GameObject[] backgrounds;
    [SerializeField] Camera mainCamera;

    Vector2 screendBounds;

    void LoadChildsObj(GameObject go)
    {
        float goWidth = go.GetComponent<SpriteRenderer>().bounds.size.x;
        int goNeeded = (int) Mathf.Ceil(screendBounds.x * 2 / goWidth);

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

    GameObject porco;

    void RepositionChild(GameObject go)
    {
        porco = go;

        Transform[] children = porco.GetComponentsInChildren<Transform>();

        if (children.Length > 1)
        {
            GameObject firstChild = children[1].gameObject;
            GameObject lastChild= children[children.Length - 1].gameObject;

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

            Debug.Log("CHISTU" + porco.name);

        }
    }

    public GameObject player;
    public float velocity;

    void MoveBackground(GameObject go, float _velocity)
    {
        velocity = _velocity;
        go.transform.position = new Vector3(-player.transform.position.x  * velocity, go.transform.position.y, go.transform.position.z);
    }

   
    private void LateUpdate()
    {
        foreach (GameObject go in backgrounds)
        {
            RepositionChild(go);
        }     
    }

    void Update()
    {
        
        if (backgrounds[0] != null)
        {
            MoveBackground(backgrounds[0], 0.8f);
        }

        if (backgrounds[1] != null)
        {
            MoveBackground(backgrounds[1], 0.2f);
        }
        

    }
}
