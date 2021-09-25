//Si occupa del movimento e fisica del giocatore
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField]
    private float movementSpeed = 10f, //indica quanto velocemente si muove il giocatore
        jumpSpeed = 10f, //indica quanto velocemente il giocatore va in alto durante il salto
        fallSpeed = -2, //indica quanto velocemente deve cadere il giocatore dopo aver finito un salto
        jumpMaxHeight = 10f; //indica quanto alto pu� saltare il giocatore rispetto alla sua posizione di salto iniziale

    //indica la posizione di quando il giocatore ha iniziato il salto
    private float startJumpPosition;
    //riferimento al Transform dello sprite del giocatore
    private Transform playerSprite;
    //riferimento all'animator del giocatore
    private Animator playerAnimator;
    //riferimento al Rigidbody del giocatore
    private Rigidbody2D rb;
    
    private bool canJump = false, //indica se il giocatore pu� saltare o meno(inizializzata a false in modo da far cadere all'inizio il giocatore)
        isJumping = false; //indica se il giocatore sta saltando o meno


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento al Transform dello sprite del giocatore
        playerSprite = transform.GetChild(0);
        //ottiene il riferimento all'animator del giocatore
        playerAnimator = playerSprite.GetComponent<Animator>();
        //ottiene il riferimento al Rigidbody del giocatore
        rb = GetComponent<Rigidbody2D>();
        //se per un errore la velocit� di caduta � stata messa ad un valore positivo, viene messo a negativo(facendo cadere il giocatore, non farlo salire)
        if (fallSpeed > 0) { fallSpeed = -fallSpeed; }

    }

    // Update is called once per frame
    void Update()
    {
        //nuova velocity da dare al Rigidbody del giocatore
        Vector2 newVelocity = CalculateMovement()/** movementSpeed*/;
        //imposta il nuovo movimento del giocatore
        rb.velocity = newVelocity;
        //Debug.Log(rb.velocity);
    }

    private void FixedUpdate()
    {
        //se si � in aria e non si sta saltando, inizia a cadere
        if (!canJump) { rb.velocity = new Vector2(rb.velocity.x, fallSpeed); /*Debug.Log("Si sta cadendo");*/ }

    }
    /// <summary>
    /// Calcola, in base ai tasti premuti, la direzione in cui il giocatore deve muoversi
    /// </summary>
    /// <returns></returns>
    private Vector2 CalculateMovement()
    {
        //indica la velocit� che sta per essere calcolata
        Vector2 calculatedVelocity = Vector2.zero;
        //se si vuole andare verso sinistra...
        if (Input.GetKey(KeyCode.A))
        {
            //...verr� attivata l'animazione di camminata del giocatore(se non lo � gi�)...
            if (!playerAnimator.GetBool("Move")) playerAnimator.SetBool("Move", true);
            //...il movimento verr� calcolato per far andare il giocatore a sinistra...
            calculatedVelocity = new Vector2(-movementSpeed/* * Time.deltaTime*/, rb.velocity.y);
            //...e lo sprite del giocatore verr� voltato a sinistra una sola volta
            playerSprite.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            //Debug.Log("Sinistra");
        }
        //altrimenti, se si vuole andare verso destra...
        else if (Input.GetKey(KeyCode.D))
        {
            //...verr� attivata l'animazione di camminata del giocatore(se non lo � gi�)...
            if (!playerAnimator.GetBool("Move")) playerAnimator.SetBool("Move", true);
            //...il movimento verr� calcolato per far andare il giocatore a destra...
            calculatedVelocity = new Vector2(movementSpeed/* * Time.deltaTime*/, rb.velocity.y);
            //...e lo sprite del giocatore verr� voltato a destra una sola volta
            playerSprite.rotation = new Quaternion(transform.rotation.x, 180, transform.rotation.z, transform.rotation.w);
            //Debug.Log("Destra");
        } //altrimenti, non si sta camminando, quindi rimette il giocatore in Idle
        else if(playerAnimator.GetBool("Move")) { playerAnimator.SetBool("Move", false); }
        //se si vuole saltare, il movimento far� andare il giocatore verso su fino ad un certo range
        if (Input.GetKey(KeyCode.W) && canJump) { calculatedVelocity = new Vector2(calculatedVelocity.x , Jump()); }
        //se non si sta premendo pi� il tasto di salto dopo aver gi� saltato...
        if (!Input.GetKey(KeyCode.W) && isJumping && canJump)
        {
            //...comunica che non si pu� pi� saltare, in quanto si� in aria e si sta cadendo
            canJump = false;
            //Debug.Log("JUMP STOP");
        }
        //se non si pu� pi� saltare, il giocatore sar� in aria, quindi...
        if (!canJump)
        {
            //...fa iniziare l'animazione di caduta
            playerAnimator.SetBool("IsFalling", true);
            playerAnimator.SetBool("IsJumping", false);

        }
        //infine, ritorna il movimento calcolato
        return calculatedVelocity;

    }
    /// <summary>
    /// Fa alzare il giocatore, calcolando anche fino a dove pu� alzarsi
    /// </summary>
    /// <returns></returns>
    private float Jump()
    {
        //fa partire l'animazione di salto
        playerAnimator.SetBool("IsJumping", true);
        //calcola quanto in alto deve andare il giocatore
        float jumpVelocity = 0;
        //se non si � gi� in salto, ottiene la posizione iniziale del giocatore
        if (!isJumping) { startJumpPosition = transform.position.y; /*Debug.Log("Start Jump Position = " + startJumpPosition);*/ }
        //comunica che il giocatore sta saltando
        isJumping = true;
        //se si arriva all'altezza massima di salto, il giocatore non potr� pi� saltare
        if (transform.position.y >= (startJumpPosition + jumpMaxHeight)) { canJump = false; }
        //altrimenti, continua a salire
        else { jumpVelocity = jumpSpeed/*new Vector2(rb.velocity.x, jumpSpeed/* * Time.deltaTime)*/; }
        //Debug.Log("Salto");
        //infine, ritorna il movimento calcolato durante il salto
        return jumpVelocity;

    }
    /// <summary>
    /// Permette al giocatore di saltare di nuovo
    /// </summary>
    public void TouchedTheGround(bool isGrounded)
    {
        //il giocatore potr� di nuovo saltare, se � per terra
        canJump = isGrounded;
        isJumping = false;
        //l'animazione di caduta viene fermata, se si � toccata terra
        playerAnimator.SetBool("IsFalling", !isGrounded);
        //Debug.Log("Toccata terra");
    }
    /// <summary>
    /// Permette ad altri script di sapere se il giocatore sta saltando o meno
    /// </summary>
    /// <returns></returns>
    public bool IsPlayerJumping() { return isJumping; }

}
