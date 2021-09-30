//Si occupa del comportamento del boss
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    //riferimento all'Animator del boss
    private Animator bossAnim;
    //riferimento al contenitore di tutte le carte
    private Transform cardsContainer;
    //lista degli script di tutte le carte del boss
    private List<BossCards> bossCards = new List<BossCards>();
    //indica quanto tempo bisogna aspettare tra un lancio e un altro
    [SerializeField]
    private float waitBeforeLaunch = 5;
    //indica quanto tempo deve passare da un lancio all'altro
    [SerializeField]
    private float waitBetweenLaunches = 0.5f;
    //indica quante carte devono essere lanciate in un singolo attacco
    [SerializeField]
    private int nCardsToLaunch = 2;
    //lista di tutti gli indici delle carte già lanciate
    private List<int> alreadyLaunchedCards = new List<int>();
    //indica quale delle 2 mani è stata usata per lanciare una carta l'ultima volta
    private bool usedLowerHand = false;
    //riferimenti alle mani del boss
    [SerializeField]
    private Transform lowerHand = default, //MANO DESTRA(QUELLA IN BASSO)
        upperHand = default; //MANO SINISTRA(QUELLA IN ALTO)

    //indica la direzione in cui lanciare le carte
    [SerializeField]
    private Vector2 launchDirection = default;
    //riferimento al cappello del boss
    [SerializeField]
    private Transform bossHat = default;
    //indica di quanto deve ingrandirsi il cappello ogni volta che il boss subisce danni
    [SerializeField]
    private float hatSizeIncreaseRate = 0.1f;
    //indica di quanto deve aumentare la velocità delle carte ogni volta che il boss subisce danni
    [SerializeField]
    private float cardsSpeedIncreaseRate = 2;

    //riferimento all'Animator del muro invisibile che spazzerà via il giocatore dopo aver subito danni
    //[SerializeField]
    //private Animator invisibleWallAnim = default;

    //indica con quanta forza il giocatore viene spazzato via dopo aver colpito il boss
    [SerializeField]
    private float afterHitSweep = -5;
    //indica per quanto tempo il giocatore non potrà muoversi dopo aver colpito il giocatore
    [SerializeField]
    private float pMDisabilitationDuration = 2;
    //riferimento al giocatore
    [SerializeField]
    private GameObject player = null;
    //riferimento allo script di movimento del giocatore
    private PlayerMovement pm;
    //riferimento al Rigidbody2D del giocatore
    private Rigidbody2D playerRb;


    // Start is called before the first frame update
    void Start()
    {
        //ottiene il riferimento all'Animator del boss
        bossAnim = transform.GetChild(0).GetComponent<Animator>();
        //ottiene il riferimento al contenitore di tutte le carte
        cardsContainer = transform.GetChild(1);
        //ottiene il riferimento allo script di tutte le carte del boss
        for (int card = 0; card < cardsContainer.childCount; card++) { bossCards.Add(cardsContainer.GetChild(card).GetComponent<BossCards>()); }
        //ottiene i riferimenti ai componenti del giocatore
        playerRb = player.GetComponent<Rigidbody2D>();
        pm = player.GetComponent<PlayerMovement>();
        //fa partire la coroutine(ricorsiva) per lanciare le carte
        StartCoroutine(LaunchCards());

    }
    /// <summary>
    /// Si occupa di lanciare carte aspettando il dovuto tempo
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaunchCards()
    {
        //svuota la lista di carte già lanciate
        alreadyLaunchedCards.Clear();
        //aspetta del tempo prima di lanciare delle carte
        yield return new WaitForSeconds(waitBeforeLaunch);
        //fa iniziare l'animazione del lancio di carte
        bossAnim.SetBool("ThrowCards", true);
        //lancia n carte a caso dalla lista
        for (int i = nCardsToLaunch; i > 0; i--)
        {
            //prende casualmente la carta da lanciare
            BossCards cardToLaunch = ChooseRandomCard();
            //lancia la carta scelta nella direzione indicata dalla mano da cui sta venendo lanciata
            StartCoroutine(cardToLaunch.LaunchCard(GetLaunchDirection(cardToLaunch.transform)));
            //Debug.Log("Carta lanciata: " + cardToLaunch);
            //aspetta del tempo tra un lancio e un altro
            yield return new WaitForSeconds(waitBetweenLaunches);

        }
        //fa terminare l'animazione del lancio di carte
        bossAnim.SetBool("ThrowCards", false);
        //fa ripartire la coroutine per lanciare le carte
        StartCoroutine(LaunchCards());

    }
    /// <summary>
    /// Sceglie casualmente, tra le carte disponibili, una carta da lanciare
    /// </summary>
    /// <returns></returns>
    private BossCards ChooseRandomCard()
    {
        //indica la carta da lanciare
        int cardChosen = 0;
        //fino a quando non si ha l'indice di una carta non ancora lanciata, prende casualmente una carta
        while (WasCardAlreadyLaunched(cardChosen)) { cardChosen = Random.Range(0, bossCards.Count); }
        //aggiunge la carta scelta nella lista di carte lanciate
        alreadyLaunchedCards.Add(cardChosen);
        //ritorna la carta scelta
        return bossCards[cardChosen];

    }
    /// <summary>
    /// Controlla se la carta è stata già lanciata o meno
    /// </summary>
    /// <param name="chosenCard"></param>
    /// <returns></returns>
    private bool WasCardAlreadyLaunched(int chosenCard)
    {
        //indica se la carta scelta è stata lanciata o meno
        bool wasCardLaunched = false;
        //se la lista di carte lanciate non è vuota...
        if (alreadyLaunchedCards.Count != 0)
        {
            //...controlla ogni numero nella lista e, se è uguale al numero ricevuto come parametro, indica che la carta scelta è stata lanciata ed esce dal ciclo
            foreach (int launchedCard in alreadyLaunchedCards) { if (chosenCard == launchedCard) { wasCardLaunched = true; break; } }

        }
        //infine, ritorna l'esito del controllo
        return wasCardLaunched;

    }
    /// <summary>
    /// Imposta la direzione del lancio in base alla mano che sta venendo utilizzata
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private Vector2 GetLaunchDirection(Transform card)
    {
        //se è stata usata la mano in basso per lanciare la carta precedente...
        if (usedLowerHand)
        {
            //...mette la carta nella mano in alto...
            card.position = upperHand.position;
            //...la direzione di lancio viene  calcolata in modo che la carta vada verso il basso
            launchDirection.y = -Mathf.Abs(launchDirection.y);

        }
        else //altrimenti, è stata usata la mano in alto, quindi...
        {
            //...mette la carta nella mano in basso...
            card.position = lowerHand.position;
            //...la direzione di lancio viene  calcolata in modo che la carta vada verso l'alto
            launchDirection.y = Mathf.Abs(launchDirection.y);

        }
        //cambia la mano che è stata usata per lanciare la carta
        usedLowerHand = !usedLowerHand;
        //infine, ritorna la direzione in cui lanciare la carta
        return launchDirection;

    }
    /// <summary>
    /// Si occupa di ciò che deve succedere quando il boss viene colpito dal giocatore
    /// </summary>
    public void HitByPlayer()
    {
        //il cappello diventa più alto(si allunga cambiando l'asse X per il modo in cui è messo il transform di cui si ha riferimento)
        bossHat.localScale = new Vector2(bossHat.localScale.x + hatSizeIncreaseRate, bossHat.localScale.y/* + hatSizeIncreaseRate*/);
        //spazza via il giocatore
        //invisibleWallAnim.SetTrigger("Activate");
        StartCoroutine(SweepPlayerAway());

        nCardsToLaunch++;

        launchDirection.x += cardsSpeedIncreaseRate;
        launchDirection.y -= cardsSpeedIncreaseRate;

    }

    private IEnumerator SweepPlayerAway()
    {
        //il giocatore non potrà muoversi(questo serve per poter spingere il giocatore senza che la velocity venga sovrascritta)
        pm.enabled = false;
        //spinge il giocatore via dal boss
        playerRb.velocity = new Vector2(afterHitSweep, playerRb.velocity.y);
        //aspetta che il giocatore sia abbastanza lontano dal boss
        yield return new WaitForSeconds(pMDisabilitationDuration);
        //dopodichè, il giocatore potrà muoversi di nuovo
        pm.enabled = true;

    }

    public void Defeat()
    {

        //Debug.Log("Boss sconfitto");
    }

    //DEBUG------------------------------------------------------------------------------------------------------------------------------------------------
    /*
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.B)) { HitByPlayer(); }

    }
    */
    //DEBUG------------------------------------------------------------------------------------------------------------------------------------------------

}
