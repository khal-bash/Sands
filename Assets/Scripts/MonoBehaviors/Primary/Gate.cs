using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public int type0;
    public int type1;
    public int type2;
    public int type3;

    private bool Satisfied = false;
    private Inventory requirements;
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        requirements = new Inventory(type0, type1, type2, type3);
    }

    private void Update()
    {
        Satisfied = (Inventory.IsSubInventory(requirements, player.Inventory));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Satisfied)
        {
            Destroy(gameObject);
            player.Inventory -= requirements;
        }
    }

}
