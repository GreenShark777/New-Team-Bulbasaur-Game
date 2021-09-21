using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public enum Environment
    {   
        Livello_0,
        Livello_1,
        Livello_2,
        Livello_3
    }

    public int targetScore = 100;
    public int currentScore = 0;

    public Environment currentEnvironment;
    [SerializeField] Sipario sipario;

    //qui vanno messi tutti i GO che compongo le scene. (direi di fare dei prefab per ogni scena, con dentro i vari GO con i loro "comportamenti")

    [SerializeField] GameObject livello_0_Prefab;
    [SerializeField] GameObject livello_1_Prefab;
    [SerializeField] GameObject livello_2_Prefab;
    [SerializeField] GameObject livello_3_Prefab;

   // [SerializeField] GameObject sipario;

    public bool isLivello_0=true, isLivello_1=false, isLivello_2=false, isLivello_3 = false; //check dell enviroment attivo in questo momento

    [SerializeField] AudioManager audioManager;

    public void SwitchEnvironment(Environment environment)
    {
        switch (currentEnvironment)
        {
            case Environment.Livello_0:
                StartCoroutine(siparioCo());
                audioManager.SwapMusicLevel(0,1);

                break;

            case Environment.Livello_1:
                break;
            case Environment.Livello_2:
                break;
            case Environment.Livello_3:
                break;
            default:
                break;
        }

        currentEnvironment = environment;

    }

    public void Sipario(GameObject levelPrefabDeact, GameObject levelPrefabActive)
    {
       // levelPrefab.SetActive(false);
    }

    IEnumerator siparioCo()
    {
        sipario.ChiudiSipario();

        yield return new WaitForSeconds(2f);

        Sipario_1();

        sipario.ApriSipario(); 

        yield return null;
    }

    public void Sipario_0()
    {
       
        livello_0_Prefab.SetActive(true);
    }

    public void Sipario_1()
    {
        //sipario.ChiudiSipario();
        livello_0_Prefab.SetActive(false);
        livello_1_Prefab.SetActive(true);
    }

    private void Awake()
    {
        livello_0_Prefab.SetActive(true);
        livello_1_Prefab.SetActive(false);
        livello_2_Prefab.SetActive(false);
        livello_3_Prefab.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //a scopo di test
        {
            currentScore += 1;

        }

        if (currentScore == 3)
        {
            SwitchEnvironment(Environment.Livello_1);
        }

    }
}
