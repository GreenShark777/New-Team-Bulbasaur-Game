using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    //private TMP_Text scoretext;
    private static TMP_Text scoretext;

    public static int recipientScore
    {

        get { Debug.Log("PARSE " + int.Parse(scoretext.text));  return int.Parse(scoretext.text); }

        set { scoretext.text = "" + (value /*+ recipientScore*/); } //se non tolgo recipiente score il punteggio viene continuamente moltiplicato per 2

    }


    private void Awake()
    {
        scoretext = GetComponent<TMP_Text>();
        scoretext.text = "0";
    }

    private void Update()
    {
        //scoretext.text =  GameManag.highscore.ToString();
        
    }

}
