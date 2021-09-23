using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sipario : MonoBehaviour
{
  public   Animator anim;

    public void ChiudiSipario()
    {
        anim.SetBool("apri", false);
        anim.SetBool("chiudi", true);
    }

    public void ApriSipario()
    {
        anim.SetBool("chiudi", false);
        anim.SetBool("apri", true);
    }

    public void PrimaApertura()
    {
        anim.SetTrigger("primaApertura");
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
