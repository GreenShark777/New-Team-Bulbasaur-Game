//Si occupa del comportamento della marionetta
using System.Collections;
using UnityEngine;

public class MarionettaBehaviour : MonoBehaviour
{
    //riferimento all'Animator di questa marionetta
    private Animator puppetAnim;
    //riferimento al Rigidbody di questa marionetta
    private Rigidbody2D puppetRb;
    //riferimento allo sprite di questa marionetta
    private Transform puppetSprite;
    //indica ogni quanto la marionetta deve saltare
    private float jumpTimer = 2;
    //indica quanto tempo deve passare prima che il nemico salti
    private float anticipationDuration;
    //indica quanto potente sarà la forza di salto della marionetta
    [SerializeField]
    private float jumpForce = 10;
    //indica quanto si avvicina la marionetta al giocatore durante il salto
    [SerializeField]
    private float closingInSpeed = 2;
    //riferimento statico, per tutte le marionette, al giocatore
    private static Transform player;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento all'Animator di questa marionetta
        puppetSprite = transform.GetChild(0);
        //ottiene il riferimento al Rigidbody di questa marionetta
        puppetAnim = puppetSprite.GetComponent<Animator>();
        //ottiene il riferimento allo sprite di questa marionetta
        puppetRb = GetComponent<Rigidbody2D>();
        //ottiene un array contenente tutte le animazioni dell'Animator del maiale
        AnimationClip[] clips = puppetAnim.runtimeAnimatorController.animationClips;
        //cicla ogni animazione nell'array appena ottenuto e...
        foreach (AnimationClip clip in clips)
        {
            //...se trova un'animazione con il nome giusto, ne ottiene la lunghezza e la salva
            if (clip.name == "PuppetJumpAnticipation")
            {
                anticipationDuration = clip.length;
                //Debug.Log("Durata animazione marionetta: " + clip.length);
                break;

            }

        }
        //ottiene il riferimento statico al giocatore(se non esiste già)
        if(!player) player = GameObject.FindGameObjectWithTag("Player").transform;
        //infine, fa iniziare la coroutine di salto
        //StartCoroutine(Jump());

    }

    private void Update()
    {
        //se questa marionetta sta saltando...
        if (puppetAnim.GetBool("Jumping"))
        {
            //...controlla se sta cadendo, nel qual caso...
            if(puppetRb.velocity.y < 0)
            {
                //...fa iniziare l'animazione di caduta
                puppetAnim.SetBool("Jumping", false);
                puppetAnim.SetBool("Falling", true);

            }

        }

    }

    private IEnumerator Jump()
    {
        //aspetta del tempo per saltare
        yield return new WaitForSeconds(jumpTimer);
        //dopodichè, fa iniziare l'animazione di anticipazione
        puppetAnim.SetBool("Jumping", true);
        //controlla se il giocatore è a destra o a sinistra della marionetta
        closingInSpeed = (player.position.x > transform.position.x) ? Mathf.Abs(closingInSpeed) : -Mathf.Abs(closingInSpeed);
        //fa voltare la marionetta verso il giocatore
        ChangeFacingDirection();
        //dopo che l'animazione è finita...
        yield return new WaitForSeconds(anticipationDuration);
        //...da una spinta alla marionetta, facendola saltare verso il giocatore
        puppetRb.velocity = new Vector2(closingInSpeed, jumpForce);
        //Debug.Log("SALTO MARIONETTA");
    }

    public void TouchedGround()
    {
        //fa tornare la marionetta in animazione di idle
        puppetAnim.SetBool("Falling", false);
        //fa ripartire la coroutine per saltare di nuovo
        StartCoroutine(Jump());

    }

    public void ChangeFacingDirection()
    {
        //calcola la direzione in cui la marionetta deve voltarsi
        float facingDirection = (closingInSpeed > 0) ? 0 : 180;
        //volta la marionetta dalla direzione del giocatore
        puppetSprite.rotation = new Quaternion(puppetSprite.rotation.x, facingDirection, puppetSprite.rotation.z, puppetSprite.rotation.w);

    }

}
