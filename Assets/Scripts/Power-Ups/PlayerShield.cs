//Si occupa del modo in cui si comporta il power-up bolla
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    //riferimento al collider del tetto
    [SerializeField]
    private Collider2D ceilingColl = default;


    // Start is called before the first frame update
    void Start()
    {
        //fa in modo che questo power-up non collida con il tetto
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ceilingColl);

    }

}
