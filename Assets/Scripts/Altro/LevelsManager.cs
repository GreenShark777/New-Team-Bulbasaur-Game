using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    //indica il livello da caricare
    // 1 - livello 1
    // 2 - livello 2
    // 3 - livello 3
    // 4 - livello boss
    public static int levelToLoad = 1;


    // Start is called before the first frame update
    void Start()
    {
        //carica il livello indicato dalla variabile statica
        LoadLevel(levelToLoad);

    }

    private void LoadLevel(int level)
    {
        Debug.Log("Caricato livello: " + level);
        switch (level)
        {

            case 1: { EnvironmentManager.instance.currentEnvironment = EnvironmentManager.Environment.Livello_0; break; }

            case 2: { EnvironmentManager.instance.currentEnvironment = EnvironmentManager.Environment.Livello_1; break; }

            case 3: { EnvironmentManager.instance.currentEnvironment = EnvironmentManager.Environment.Livello_2; break; }

            case 4: { EnvironmentManager.instance.currentEnvironment = EnvironmentManager.Environment.Livello_3; break; }

            default: { Debug.LogError("Inserito indice livello sbagliato!"); break; }

        }

    }

}
