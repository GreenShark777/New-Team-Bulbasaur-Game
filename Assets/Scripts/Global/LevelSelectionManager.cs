using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    /// <summary>
    /// Cambia la variabile che indica il livello da caricare nella scena di gameplay
    /// </summary>
    /// <param name="index"></param>
    public void PrepareLevelToLoad(int index) { LevelsManager.levelToLoad = index; }

}
