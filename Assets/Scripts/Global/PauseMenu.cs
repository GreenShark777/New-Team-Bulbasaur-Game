using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //riferimento al men� di pausa
    private GameObject pauseMenu;

    private void Start()
    {
        //ottiene il riferimento al men� di pausa
        pauseMenu = transform.GetChild(0).gameObject;

    }

    public void Pause(bool isPaused)
    {
        //se bisogna mettere in pausa, il tempo viene fermato
        if (isPaused) { Time.timeScale = 0; }
        //altrimenti, viene fatto scorrere normalmente
        else { Time.timeScale = 1; }
        //il men� di pausa viene attivato o disattivato in base al parametro ricevuto
        pauseMenu.SetActive(isPaused);

    }

}
