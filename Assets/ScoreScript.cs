using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    //private TMP_Text scoretext;
    private static TMP_Text scoretext;
    //riferimento al testo dell'highscore nel menù principale
    [SerializeField]
    private Text highscoreText;

    public static int recipientScore
    {

        get { Debug.Log("PARSE " + int.Parse(scoretext.text));  return int.Parse(scoretext.text); }

        set { scoretext.text = "" + (value /*+ recipientScore*/); } //se non tolgo recipiente score il punteggio viene continuamente moltiplicato per 2

    }


    private void Awake()
    {
        //ottiene il riferimento al testo dello score attuale
        scoretext = GetComponent<TMP_Text>();
        //se esiste il riferimento al testo dello score attuale, lo inizializza a 0
        if (scoretext) { scoretext.text = "0"; }
        //se esiste il riferimento al testo dell'highscore, lo aggiorna
        if (highscoreText) { highscoreText.text = "Highscore: " + GameManag.highscore; }

    }

}
