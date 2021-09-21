using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State //enum che definisce i vari stati del nemico
    {
        Walk,
        Idle,      
        Dead,
        Damage,
        Jump
    }

    private State currentState;
    bool groundDetected; //le due bool per il check di ground e wall
    bool wallDetected;

    [SerializeField] Transform groundCheck, wallCheck; //i due empty object usati per il check  

    [SerializeField] LayerMask ground; //i due layer mask usati per il raycast2d su mura e terreno
    [SerializeField] LayerMask wall;

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

    private void Start()
    {
        //enemyAlive = transform.Find("enemyAlive").gameObject;
        facingDirection = 1;
        currentHealth = maxHealth;
    }



    private void Update()
    {
        Debug.Log(currentState);

        // Debug.Log(wallDetected);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Damage(1);
        }

        switch (currentState) //in base al current state del nemico....
        {
            case State.Idle:

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

            case State.Jump:
                UpdateJumpState();
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
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, ground);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, wall);

        if(!groundDetected || wallDetected) //se grounddetect è falso OPPURE SE walldetected è vero flippiamo il nemico
        {
            //flippa il nemico
            Flip();

        }
        else //altrimenti lo facciamo muovere
        {
            //move
            movement.Set(movementSpeed * facingDirection, rb.velocity.y); //metodo set per i vector2= setta una nuova x e una nuova Y su un vettore esistente
            rb.velocity = movement;

           
        }

    }

    void ExitWalkingState()
    {

    }

    //damage state

    void EnterDamageState()
    {
        damageStartTime = Time.time; //quando parte il colpo

        movement.Set(damageSpeed.x * damageDirection, damageSpeed.y);

        rb.velocity = movement;

        anim.SetBool("enemy_damage", true);
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
        anim.SetBool("enemy_damage", false);
    }

    //dead state

    void EnterDeadState()
    {
        //aggiungere particle
        Destroy(gameObject);
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

    //jump state

    void EnterJumpState()
    {

    }

    void UpdateJumpState()
    {

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

    void SwitchState(State state) //si occupa di cambiare lo stato dell enemy
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

            case State.Jump:
                UpdateJumpState();
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
    }

}
