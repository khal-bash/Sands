using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void UpdatingEventHandler();

public class History
{

    public event UpdatingEventHandler Updating;

    protected virtual void OnUpdate()
    {
        UpdatingEventHandler handler = Updating;
        handler?.Invoke();
    }

    public List<FrameHistory> Game { get; private set; }

    public FrameHistory Last
    {
        get {return Game[Game.Count - 1];}
    }

    public History()
    {
        Game = new List<FrameHistory>();
    }

    public void AddFrameHistory(FrameHistory frame)
    {
        Game.Add(frame);
        OnUpdate();
    }

    public class FrameHistory
    {

        public Vector3 Player_Position { get; private set; }
        public bool Is_Dashing { get; private set; }
        public bool Beam_Spawned { get; private set; }
        public Vector3 Beam_Position { get; private set; }
        public Quaternion Beam_Rotation { get; private set; }
        public Vector3 Beam_Scale { get; private set; }

        public FrameHistory(Vector3 player_position,
                            bool is_dashing)
                     
        {
            Player_Position = player_position;
            Is_Dashing = is_dashing;
            Beam_Spawned = false;
        }

        public void AddBeam(Vector3 position, Quaternion rotation, Vector3 local_scale)
        {
            Beam_Spawned = true;
            Beam_Position = position;
            Beam_Rotation = rotation;
            Beam_Scale= local_scale;
        }

    }

}
