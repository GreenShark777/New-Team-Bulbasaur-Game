//Si occupa del comportamento del nemico maiale
using System.Collections;
using UnityEngine;

public class MaialeBehaviour : MonoBehaviour
{
    //riferimento all'Animator del nemico maiale
    private Animator pigAnim;
    //riferimento al Rigidbody2D del nemico
    private Rigidbody2D pigRb;
    //riferimento allo sprite del nemico maiale
    private Transform pigSprite;
    //indica se il nemico sta caricando o meno
    private bool isCharging = false;
    //indica quanto velocemente cammina il nemico maiale
    [SerializeField]
    private float walkSpeed = 3;
    //indica quanto velocemente si muove il nemico maiale durante la carica
    [SerializeField]
    private float chargeSpeed = 7;
    //indica quanto dura la carica del nemico
    [SerializeField]
    private float chargeDuration = 2;
    //indica quanto tempo deve passare prima che il nemico attacchi
    private float anticipationDuration;
    //riferimento all'empty da cui parte il controllo della distanza dal giocatore
    [SerializeField]
    Transform playerCheck = default;
    //indica da quanto lontano il nemico maiale nota il giocatore per iniziare la carica
    [SerializeField]
    private float playerCheckDistance = 3;
    //indica il layer del giocatore
    [SerializeField]
    private LayerMask playerMask = default;
    //indica quanto tempo deve stare fermo il nemico dopo aver colpito il giocatore, per permettergli di scappare
    [SerializeField]
    private float hitPlayerTimer = 3;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento allo sprite del nemico maiale
        pigSprite = transform.GetChild(0);
        //ottiene il riferimento all'Animator del nemico maiale
        pigAnim = pigSprite.GetComponent<Animator>();
        //ottiene il riferimento al Rigidbody2D del nemico
        pigRb = GetComponent<Rigidbody2D>();
        //ottiene un array contenente tutte le animazioni dell'Animator del maiale
        AnimationClip[] clips = pigAnim.runtimeAnimatorController.animationClips;
        //cicla ogni animazione nell'array appena ottenuto e...
        foreach (AnimationClip clip in clips)
        {
            //...se trova un'animazione con il nome giusto, ne ottiene la lunghezza e la salva
            if (clip.name == "ChargeAnticipation")
            {
                anticipationDuration = clip.length;
                //Debug.Log("Durata animazione maiale: " + clip.length);
                break;
            
            }
            
        }
        //cambia la direzione in cui il maiale va
        ChangeFacingDirection();

        //DEBUG----------------------------------------------------------------------------------------------------------------------------------------
        if (Mathf.Abs(walkSpeed) > Mathf.Abs(chargeSpeed))
        { Debug.LogError("La velocità di carica del nemico maiale " + gameObject.name + " è minore della velocità di camminata."); }

    }

    // Update is called once per frame
    void Update()
    {
        //se non sta caricando e non è in esultanza, il nemico cammina
        if (!isCharging && !pigAnim.GetBool("Exultation")) { PigWalk(); } //{ StartCoroutine(Charge()); }
        //else { Debug.Log("NOT WALK"); }
        //altrimenti, continua a camminare
        //else { PigWalk(); }

        //DEBUG----------------------------------------------------------------------------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.T)) { PrepareCharge(); }

    }

    private void PigWalk()
    {
        //fa avvenire il movimento del nemico maiale
        pigRb.velocity = new Vector2(walkSpeed, pigRb.velocity.y);
        //Debug.Log("WALK");
        //se il giocatore si trova dentro il check, il nemico inizia la carica
        if(Physics2D.Raycast(playerCheck.position, transform.right, playerCheckDistance, playerMask))
        { PrepareCharge(); }

    }

    public void PrepareCharge() { StartCoroutine(Charge()); }

    private IEnumerator Charge()
    {
        //se il nemico non è in esultanza, carica il giocatore
        if (!pigAnim.GetBool("Exultation"))
        {
            //comunica che si sta caricando
            isCharging = true;
            //ferma il nemico
            pigRb.velocity = Vector2.zero;
            //fa cominciare l'animazione di anticipazione alla carica al nemico maiale
            pigAnim.SetTrigger("StartCharge");
            //aspetta il tempo di anticipazione della mossa di carica
            yield return new WaitForSeconds(anticipationDuration);
            //fa cominciare l'animazione di carica al nemico maiale
            pigAnim.SetBool("Charge", true);
            //da una forza di spinta al nemico maiale
            //pigRb.AddForce(new Vector2(chargeSpeed, pigRb.velocity.y));
            pigRb.velocity = new Vector2(chargeSpeed, pigRb.velocity.y);
            //aspetta del tempo...
            yield return new WaitForSeconds(chargeDuration);
            //...dopodichè, ferma il nemico
            pigRb.velocity = Vector2.zero;
            //fa cominciare l'animazione di camminata al nemico maiale
            pigAnim.SetBool("Charge", false);
            //comunica che non si sta più caricando
            isCharging = false;

        }

    }

    public void ChangeFacingDirection()
    {
        //cambia le direzioni di movimento del nemico
        walkSpeed = -walkSpeed;
        chargeSpeed = -chargeSpeed;
        //se il nemico sta caricando, aggiorna la direzione in cui deve caricare
        if (isCharging) { pigRb.velocity = new Vector2(chargeSpeed, pigRb.velocity.y); }
        //cambia la direzione in cui il nemico sta guardando
        if (walkSpeed > 0) { pigSprite.rotation = new Quaternion(pigSprite.rotation.x, 0, pigSprite.rotation.z, pigSprite.rotation.w); }
        else { pigSprite.rotation = new Quaternion(pigSprite.rotation.x, 180, pigSprite.rotation.z, pigSprite.rotation.w); }
        //cambia la direzione in cui il nemico vede il giocatore
        playerCheckDistance = -playerCheckDistance;
        //Debug.Log("CAMBIO DIREZIONE: " + pigSprite.rotation.y);
    }

    public IEnumerator Exultation()
    {
        //il nemico va in animazione di esultazione
        pigAnim.SetBool("Exultation", true);
        //ferma il nemico
        pigRb.velocity = Vector2.zero;
        //aspetta del tempo dopo aver colpito il giocatore
        yield return new WaitForSeconds(hitPlayerTimer);
        //il nemico va in animazione di movimento
        pigAnim.SetBool("Exultation", false);

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x + playerCheckDistance, playerCheck.position.y));

    }

}
