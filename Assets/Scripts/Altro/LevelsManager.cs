//Si occupa di preparare il livello da caricare
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    //indica il livello da caricare
    // 1 - livello 1
    // 2 - livello 2
    // 3 - livello 3
    // 4 - livello boss
    public static int levelToLoad
    {
        //ritorna il livello da caricare
        get { return level; }
        //imposta il livello da caricare e lo carica
        set { level = value; LoadLevel(levelToLoad); }

    }
    //indica il livello da caricare
    public static int level;
    //riferimento all'immagine da attivare quando si sceglie un livello che non è 1
    [SerializeField]
    private GameObject warningImage = default;
    private static GameObject staticWarningImage;
    //indica se il giocatore è stato avvisato di non aver scelto il livello 1
    private static bool playerWarned = false;


    private void Start()
    {
        //imposta il livello da caricare a 0
        level = 0;
        //indica se il giocatore è stato avvisato che il punteggio non verrà salvato(quando sceglie un livello non iniziale)
        playerWarned = false;
        //rende statico il riferimento all'immagine d'avviso
        staticWarningImage = warningImage;

    }
    /// <summary>
    /// Carica il livello ricevuto come parametro
    /// </summary>
    /// <param name="level"></param>
    private static void LoadLevel(int level)
    {
        //controlla se il giocatore è stato avvisato
        playerWarned = staticWarningImage.activeSelf;
        //se non è stato scelto il livello inziale e non è ancora stato avvisato il giocatore, lo avvisa e non carica il livello
        if (level > 1 && !playerWarned) { staticWarningImage.SetActive(true); return; }
        //in base al livello scelto, carica quel livello
        switch (level)
        {
            //carica livello 1
            case 1: { EnvironmentManager.currentEnvironment = EnvironmentManager.Environment.Livello_0; break; }
            //carica livello 2
            case 2: { EnvironmentManager.currentEnvironment = EnvironmentManager.Environment.Livello_1; break; }
            //carica livello 3
            case 3: { EnvironmentManager.currentEnvironment = EnvironmentManager.Environment.Livello_2; break; }
            //carica livello boss
            case 4: { EnvironmentManager.currentEnvironment = EnvironmentManager.Environment.Livello_3; break; }
            //comunica di aver ricevuto un valore errato
            default: { Debug.LogError("Inserito indice livello sbagliato!"); break; }

        }
        Debug.Log("Caricato livello: " + EnvironmentManager.currentEnvironment);
    }
    /// <summary>
    /// Usato dal bottone Continua dell'immagine di Warning per caricare il livello previamente selezionato
    /// </summary>
    public void LoadAfterWarning() { LoadLevel(levelToLoad); }

}
