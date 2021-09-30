//Si occupa della gestione degli hp del giocatore e di ciò che succede quando subisce danno
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //riferimento al contenitore dei cuori del giocatore
    [SerializeField]
    private Transform healthContainer = default;
    //riferimento alla UI per la schermata di game over
    [SerializeField]
    private GameObject gameOverScreen = default;
    //riferimento allo sprite del giocatore
    private GameObject[] playerSprites;
    //indica quanta vita ha il giocatore(e quanti colpi può subire)
    [HideInInspector]
    public int currentHP = 3, 
        maxHp = 3;
    //riferimento all'animator del giocatore
    private Animator playerAnimator;
    //indica pe quanto tempo il giocatore diventa invincibile dopo essere stato colpito
    [SerializeField]
    private float invincibilityTimer = 2;
    //indica se il giocatore può subire danni o meno
    private bool canTakeDmg = true;
    //indica ogni quanto velocemente lo sprite del giocatore viene attivato e disattivato dopo aver preso danno
    [SerializeField]
    private float dmgAnimationTimer = 0.5f;


    void Start()
    {

        Transform spritesContainer = transform.GetChild(0);

        playerSprites = new GameObject[spritesContainer.childCount];

        for (int child = 0; child < spritesContainer.childCount; child++) { playerSprites[child] = spritesContainer.GetChild(child).gameObject; }
        //ottiene il riferimento agli sprite del giocatore
        //ottiene il riferimento all'animator del giocatore
        playerAnimator = spritesContainer.GetComponent<Animator>();
        //la vita del giocatore sarà uguale al numero di cuori(figli) nel contenitore della vita
        currentHP = healthContainer.childCount;
        //ottiene la vita massima del giocatore
        maxHp = currentHP;
        //Debug.Log("Vita giocatore: " + currentHP);

        //DEBUG
        //if (currentHP != healthContainer.childCount) { Debug.LogError("Gli Hp del giocatore sono diversi dal numero di cuori nel contenitore"); }

    }
    /// <summary>
    /// Cambia gli hp in base al parametro ricevuto
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHp(int value)
    {
        //se si sta prendendo danno...
        if (value < 0)
        {
            //...e lo si può subire...
            if (canTakeDmg)
            {
                //...viene attivata l'animazione di danno del giocatore...
                playerAnimator.SetTrigger("Hit");
                //...e fa partire partire il timer per non far prendere danno al giocatore per tot secondi
                StartCoroutine(GotHit());

            } //altrimenti, non potendo prendere danno, il valore di danno viene azzerato
            else { value = 0; }

        } //altrimenti, si sta recuperando vita, quindi...
        else { /*ATTIVARE PARTICELLARE O ALTRO*/ }
        //gli Hp vengono cambiati aggiungendo il valore ottenuto
        currentHP += value;
        //vengono attivati o disattivati i cuori in base agli hp
        for (int i = 0; i < healthContainer.childCount; i++) { healthContainer.GetChild(i).gameObject.SetActive(i < currentHP); }
        //se sono finiti gli hp, il giocatore perde
        if (currentHP <= 0) { Defeat(); }
        //Debug.Log("Vita giocatore: " + currentHP);
    }
    /// <summary>
    /// Il giocatore viene sconfitto
    /// </summary>
    private void Defeat()
    {
        //viene attivata l'animazione di sconfitta del giocatore
        playerAnimator.SetTrigger("Defeated");
        //il giocatore non potrà più muoversi
        GetComponent<PlayerMovement>().enabled = false;
        //attiva la schermata di Game Over
        gameOverScreen.SetActive(true);

    }
    /// <summary>
    /// Dopo che il giocatore subisce danni, non potrà essere colpito per dei secondi
    /// </summary>
    /// <returns></returns>
    private IEnumerator GotHit()
    {
        //il giocatore non potrà più prendere danno
        canTakeDmg = false;
        //richiama la coroutine per l'animazione di danno
        StartCoroutine(DamageAnimation());
        //aspetta dei secondi
        yield return new WaitForSeconds(invincibilityTimer);
        //il giocatore potrà di nuovo prendere danni
        canTakeDmg = true;

    }
    /// <summary>
    /// Fa in modo che lo sprite si veda ad intermittenza fino a quando il giocatore non potrà di nuovo prendere danno
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageAnimation()
    {
        //disattiva e attiva lo sprite del giocatore in base al suo stato d'attivazione
        foreach (GameObject sprite in playerSprites) { sprite.SetActive(!sprite.activeSelf); }
        //aspetta dei secondi
        yield return new WaitForSeconds(dmgAnimationTimer);
        //se non si può ancora prendere danno, continua l'animazione di danno
        if (!canTakeDmg) { StartCoroutine(DamageAnimation()); }
        //altrimenti, riattiva lo sprite del giocatore(se non lo è già) e termina l'animazione di danno
        else { foreach (GameObject sprite in playerSprites) { sprite.SetActive(true); } }

    }

    //DEBUG--------------------------------------------------------------------------------------------------------------------------------------------
    /*
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)) { ChangeHp(-1); }
        if (Input.GetKeyDown(KeyCode.O)) { ChangeHp(1); }

    }
    */
    //DEBUG--------------------------------------------------------------------------------------------------------------------------------------------

}
