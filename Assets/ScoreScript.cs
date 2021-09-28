using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    TMP_Text scoretext;

    private void Awake()
    {
        scoretext = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        scoretext.text =  GameManag.score.ToString();
    }

}
