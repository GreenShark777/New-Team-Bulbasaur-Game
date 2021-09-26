//Si occupa della gestione degli hp del giocatore e di ciò che succede quando subisce danno
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //riferimento al contenitore dei cuori del giocatore
    [SerializeField]
    private Transform healthContainer = default;
    //riferimento alla UI per la schermata di game over
    [SerializeField]
    private GameObject gameOverScreen = default;
    //indica quanta vita ha il giocatore(e quanti colpi può subire)
    public int currentHP = 3;
    //riferimento all'animator del giocatore
    private Animator playerAnimator;


    void Start()
    {
        //ottiene il riferimento all'animator del giocatore
        playerAnimator = transform.GetChild(0).GetComponent<Animator>();

        //DEBUG
        if (currentHP != healthContainer.childCount) { Debug.LogError("Gli Hp del giocatore sono diversi dal numero di cuori nel contenitore"); }

    }
    /// <summary>
    /// Cambia gli hp in base al parametro ricevuto
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHp(int value)
    {
        //se si sta prendendo danno, viene attivata l'animazione di danno del giocatore
        if (value < 0) 
        { 
            playerAnimator.SetTrigger("Hit");

            currentHP = currentHP + value;///
        }
        //altrimenti, si sta recuperando vita, quindi (METTERE PARTICELLARE O ALTRO)

        else { currentHP += value; }
        //gli Hp vengono cambiati aggiungendo il valore ottenuto     
        //vengono attivati o disattivati i cuori in base agli hp
        for (int i = 0; i < healthContainer.childCount; i++) { healthContainer.GetChild(i).gameObject.SetActive(i < currentHP); }
        //se sono finiti gli hp, il giocatore perde
        if (currentHP <= 0) { Defeat(); }

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

    //DEBUG--------------------------------------------------------------------------------------------------------------------------------------------

    void Update()
    {
       // Debug.Log("CURRENT HEALTH " + currentHP);


        if (Input.GetKeyDown(KeyCode.P)) { ChangeHp(-1); }
        if (Input.GetKeyDown(KeyCode.O)) { ChangeHp(1); }

    }

    //DEBUG--------------------------------------------------------------------------------------------------------------------------------------------

}
