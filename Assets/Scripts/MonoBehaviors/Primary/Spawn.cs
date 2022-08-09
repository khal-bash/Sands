using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Beam;
    public int Collision_Damage;

    Player Player;
    int Fixed_Frame = 0;

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<Player>();
        Player.Damage += DoDamageToPlayer;
    }

    private void FixedUpdate()
    {
        History.FrameHistory this_frame = Player.Game_Log.Game[Fixed_Frame];
        History.FrameHistory next_frame = Player.Game_Log.Game[Fixed_Frame + 1];

        Vector3 delta = next_frame.Player_Position - transform.position;
        Rigidbody2D rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = delta / Time.fixedDeltaTime;

        if (this_frame.Beam_Spawned)
        {
            GameObject beam = Instantiate(Beam, this_frame.Beam_Position,
                                                this_frame.Beam_Rotation);

            beam.transform.localScale = this_frame.Beam_Scale;

        }

        Fixed_Frame++;
    }

    public void DoDamageToPlayer(GameObject enemy)
    {
        if(enemy == gameObject)
        {
            Player.HP.ChangeHP(-Collision_Damage);
            Player.Damage -= DoDamageToPlayer;
            Destroy(gameObject);
        }
    }

}
