using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTA IMPORTANTE: HO CAMBIATO LA MATRICE DEI LAYER COLLISION NEL PROJECT SETTING IN MODO DA NON FAR COLLIDERE TRA LORO
//GLI OGGETTI CON LAYER DAMAGEABLE. 
//INFATTI AL MOMENTO I NEMICI HANNO UN COLLIDER NON-TRIGGER
//MA QUANDO SI SOVRAPPONGO NON COLLIDONO, PASSANO UNO SOPRA L'ALTRO
// https://docs.unity3d.com/Manual/LayerBasedCollision.html

public class EnemyController : MonoBehaviour
{
    public enum State //enum che definisce i vari stati del nemico
    {
 
        Spawn,
        Walk,
        Idle,      
        Dead,
        Damage,
        Jump,
        Attack
    
    }

    public bool nemicoMaiale = false; //bool probabilmente provvisoria

    GameObject player;

    public State currentState; //lo stato corrente del nemico
    bool groundDetected; //le due bool per il check di ground e wall
    bool wallDetected;
    bool playerDetected; ///
    bool playerHitted;

    [SerializeField] Transform groundCheck, wallCheck; //i due empty object usati per il check  

    [SerializeField] LayerMask ground; //i due layer mask usati per il raycast2d su mura e terreno
    [SerializeField] LayerMask wall;
    [SerializeField] LayerMask playerMask;

    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float movementSpeed;
    [SerializeField] int maxHealth;
    int currentHealth;

    public float damageStartTime; //quando parte il colpo
    public float damageDuration=1; //quanto dura lo stato Damage
    Vector2 damageSpeed;
    int damageDirection=1; //per capire da dove proviene il colpo (da rimuovere)

    int facingDirection; //da usare per flippare il nemico

    Vector2 movement; //aggiorniamo il valore di movement per aggiungerlo alla velocity dell rb invece di dichiarare un nuovo V2 ogni volta che serve

    [SerializeField]GameObject enemy;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] Animator anim;

    [SerializeField] PlayerHealth playerHealth; //ref allo script che gestisce la vita del player

    [SerializeField] int enemyDamage = -1; //per come è scritto il metodo in playerhealth, bisogna usare numeri negativi

    bool delayhit = false;

    Vector2 jumpSpeed = new Vector2(0, 10f); //il valore del dash su asse x

    bool isJumping = false;

    IEnumerator delayHit()
    {
        delayhit = true;

        SwitchState(State.Damage); //per adesso uso questo giusto per bloccare un secondo il nemico

        yield return new WaitForSeconds(1f);

        SwitchState(State.Walk);
        delayhit = false;
        yield return null;

    }

 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && delayhit==false)
        {
            // PlayertakeDamage(enemyDamage);
            StartCoroutine(delayHit()); // per evitare collisioni multiple nel giro di pochi frame
        }

        else if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("SUCALORA");
            Physics2D.IgnoreLayerCollision(7,8);
        }
    }

    private void Awake()
    {
        playerHitted = false;
        playerHealth= GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>(); // dato che i nemici vengono spawnati a runtime, andiamo a prendere il ref a questo componente a runtime cercando nella scena
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player"); //// dato che i nemici vengono spawnati a runtime, andiamo a prendere il ref a questo componente a runtime cercando nella scena   
    }

    private void Start()
    {
        //facingDirection = 1;

        // facingDirection = (int)gameObject.transform.parent.forward.x;

        //facingDirection = Mathf.Clamp((int)gameObject.transform.parent.right.x, -1,1);

        //Debug.Log("FACING " + gameObject.transform.parent.forward);

        currentHealth = maxHealth;
    }


    void PlayertakeDamage(int damageAmount)
    {   
        /*
        if (delayhit == false)
        {
            StartCoroutine(delayHit()); //////
            playerHealth.ChangeHp(damageAmount);
        }
        */

       // playerHealth.ChangeHp(damageAmount);

        playerHitted = true;
        StopAllCoroutines(); //elementare watson...
        
        SwitchState(State.Walk);
        //playerHealth.currentHP -= damageAmount;
    }

    void Attack()
    {
        SwitchState(State.Attack);
    }

    void Jump()
    {
        SwitchState(State.Jump);
    }

    private void Update()
    {
        Debug.Log("HP " + playerHealth.currentHP);

        if (playerHitted)
        {
            SwitchState(State.Walk);     
        }

        Debug.Log("HIT "+playerHitted);
        Debug.Log("JUMP " + isJumping);
        Debug.Log("STATE " + currentState);
        Debug.Log("delayhit " + delayhit);
        // Debug.Log("groundetec " + groundDetected);
        //Debug.Log("jump " + isJumping);

        //Debug.Log(groundDetected);

        if (Input.GetKeyDown(KeyCode.Q)) //per test danno
        {
            Damage(1);
        }

        if (Input.GetKeyDown(KeyCode.K)) //per test attack
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.L)) //per test jump
        {
            Jump();
        }


        switch (currentState) //all'interno dell update(), in base al current state del nemico....
        {
            case State.Idle: //richiama la funzione UpdateState() dello stato specificato, c'è un update per ciascuno stato

                UpdateIdleState();
                break;

            case State.Walk:

                UpdateWalkingState();
                break;

            case State.Dead:
                UpdateDeadState();
                break;

            case State.Damage:
                UpdateDamageState();
                break;

            case State.Attack:
                UpdateAttackState();
                break;

            case State.Jump:
                UpdateJumpState();
                break;

            case State.Spawn:
                UpdateSpawnState();
                break;

            default:
                break;
        }
    }

    //QUI SOTTO tutte le funzioni che definiscino i vari stati, espandi la regione

    #region Unity states 

    //walking state

    void EnterWalkingState()
    {
        
    }

    void UpdateWalkingState()
    {
        isJumping = false;
        playerHitted = false;
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance+50, ground); //check di ground e mura
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wall);
        playerDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance + 2, playerMask);

        /*
             if (!groundDetected || wallDetected) //se grounddetect è falso OPPURE SE walldetected è vero flippiamo il nemico
             {
                 //flippa il nemico
                 Flip();

             }
             else //altrimenti lo facciamo muovere
             {
                  //move

                 movement.Set(movementSpeed * facingDirection, rb.velocity.y); //metodo set per i vector2= setta una nuova x e una nuova Y su un vettore esistente
                 rb.velocity = movement; //assegno alla velocity del rigidbody 2d il valore del vettore movement

                 //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position *facingDirection,player.transform.position, 2*Time.deltaTime);

             }
        */

        /*
        if (!groundDetected || wallDetected) //se grounddetect è falso OPPURE SE walldetected è vero flippiamo il nemico
        {
            //flippa il nemico
            Flip();

        }
        */
         if (playerDetected && !playerHitted && nemicoMaiale==true)
        {
            
            Debug.Log("player det " + playerDetected);
            StartCoroutine(delayAttack());

          //  SwitchState(State.Attack);
           // playerDetected = false;

        }
        else//altrimenti lo facciamo muovere
        {
            //move

            movement.Set(movementSpeed * facingDirection, rb.velocity.y); //metodo set per i vector2= setta una nuova x e una nuova Y su un vettore esistente
            rb.velocity = movement; //assegno alla velocity del rigidbody 2d il valore del vettore movement

            //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position *facingDirection,player.transform.position, 2*Time.deltaTime);

        }


    }

      IEnumerator delayAttack()
    {
        movement.Set(0f, enemy.transform.position.y);
        yield return new WaitForSeconds(.5f);
        if(!playerHitted)
        SwitchState(State.Attack);
     
        yield return null;

    }

    void ExitWalkingState()
    {

    }

    //damage state

    void EnterDamageState()
    {
        damageStartTime = Time.time; //quando parte il colpo

        //nota: damagespeed=0, il nemico si freeza per un istante
        movement.Set(damageSpeed.x * damageDirection, damageSpeed.y); //NOTA:DE DEFINIRE IL DAMAGE DIRECTION (da dove arriva il colpo, quindi usare la x della pos del player)

        rb.velocity = movement;

       //  anim.SetBool("enemy_damage", true); //parte l'animazione
    }

    void UpdateDamageState()
    {
        if (Time.time >= damageStartTime + damageDuration) //durata del colpo
        {
            SwitchState(State.Walk); //torna a camminare
        }

    }

    void ExitDamageState()
    {
        // anim.SetBool("enemy_damage", false);
    }

    //dead state

    EnvironmentManager environmentManager = EnvironmentManager.instance;////

    void EnterDeadState()
    {
        //aggiungere particle
        Destroy(gameObject);
        environmentManager.nemiciUccisi += 1; //nemiciUccisi gestisce i cambi di scenario nell' environment manager (classe public static)
        Debug.Log("NEMICI UCCISI " + environmentManager.nemiciUccisi);
    }

    void UpdateDeadState()
    {

    }

    void ExitDeadState()
    {

    }

    //idle state

    void EnterIdleState()
    {

    }

    void UpdateIdleState()
    {

    }

    void ExitIdleState()
    {

    }

    //attack state

    float dashStart; // quando inizia il dash
    float dashDurantion =0.3f; //quanto deve durare
    Vector2 dashSpeed = new Vector2(15,0); //il valore del dash su asse x


    void EnterAttackState() 
    {

        dashStart = Time.time; //cache di quando parte il dash
        movement.Set(dashSpeed.x * facingDirection, enemy.transform.position.y); // direzione del dash = facedirection dell enemy, moltiplichiamo per dashspeed

        rb.velocity = movement; //assegniamo il risultato di movement.set alla velocity dell rb

    }

  

    void UpdateAttackState() 
    {
        //Debug.Log("STO DASHANDO");
        
        if (Time.time >= dashStart + dashDurantion || playerHitted) //se superiamo la durata del dash 
        {
            
            SwitchState(State.Walk); //torna a camminare
        }

        //per evitare che il nemico vada fuori scena o si impalli contro una parete
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, ground); //check di ground e mura
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wall);

        //se grounddetect è falso OPPURE SE walldetected è vero torniamo a walk state
        if (!groundDetected || wallDetected || isJumping==false) 
        {
            SwitchState(State.Walk); //torna a camminare

        }

    }

    void ExitAttackState()
    { /*
        movement.Set(0, enemy.transform.position.y); //NOTA:DE DEFINIRE IL DAMAGE DIRECTION (da dove arriva il colpo, quindi usare la x della pos del player)

        rb.velocity = movement;
        */
    }

    //spawn state

    void EnterSpawnState()
    {
    

    }

    void UpdateSpawnState()
    {

        movement.Set( facingDirection, rb.velocity.y); //metodo set per i vector2= setta una nuova x e una nuova Y su un vettore esistente

        rb.velocity = movement;

        if(groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, ground))
        SwitchState(State.Walk);

    }

    void ExitSpawnState()
    {

    }

    //jump state


    void EnterJumpState()
    {
        movement.Set(rb.velocity.x , jumpSpeed.y); // direzione del dash = facedirection dell enemy, moltiplichiamo per dashspeed

        rb.velocity = movement; //assegniamo il risultato di movement.set alla velocity dell rb

    }

    void UpdateJumpState()
    {

        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, ground); //check di ground e mura
        isJumping = true;

        if (groundDetected )
        {
            isJumping = false;//////
            StartCoroutine(delayStopJump());     

        }

    }

    IEnumerator delayStopJump()
    {
        yield return new WaitForSeconds(0.3f);
        SwitchState(State.Walk);
        yield return null;

    }

    void ExitJumpState()
    {

    }

    #endregion 

    //altre funzioni

    void Flip()
    {
        facingDirection *= -1; //flippiamo la direzione del character
 
        enemy.transform.Rotate(0.0f, 180f, 0.0f); //flippiamo l'oggetto character
    }

  
    void Damage(int amount)
    {
        if (enemy != null)
        {
            currentHealth -= amount;

            if (currentHealth > 0)
            {
                SwitchState(State.Damage);
            }

            else if (currentHealth <= 0)
            {
                SwitchState(State.Dead);
            }
        }

    }

    public void SwitchState(State state) //si occupa di cambiare lo stato dell enemy
    {
        switch (currentState) //se currentState è X allora richiamo la funzione per uscire dallo stato X
        {
            case State.Idle:
                ExitIdleState();
                break;

            case State.Walk:
                ExitWalkingState();
                break;

            case State.Dead: // nota: nel caso volessimo usare objectpooling o qualsiasi altro meccanismo di attivazione/disattivazione del nemico
                ExitDeadState();
                break;

            case State.Damage:
                ExitDamageState();
                break;

            case State.Attack:
                ExitAttackState();
                break;

            case State.Jump:
                ExitJumpState();
                break;

            case State.Spawn:
                ExitSpawnState();
                break;

            default:
                break;
        }

        switch (state) //state è il parametro passato come argomento. richiamo, in base al case, la funzione per entrare in questo state
        {
            case State.Idle:
                EnterIdleState();
                break;

            case State.Walk:
                EnterWalkingState();
                break;

            case State.Dead:
                EnterDeadState(); 
                break;

            case State.Damage:
                EnterDamageState();
                break;

            case State.Attack:
                EnterAttackState();
                break;

            case State.Jump:
                EnterJumpState();
                break;


            default:
                break;
        }

        currentState = state; //il nuovo valore di currentState è state
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y- groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + 8, wallCheck.position.y)); ////////

    }

}
