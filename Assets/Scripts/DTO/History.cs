using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Delegates;

/// <summary>
/// Class that keeps track of eveything that has occured in the game.
/// </summary>
public class History
{

    //Properties
    #region Properties

    /// <summary>
    /// The bulk of the class; <see cref="FrameHistory"/> implicitly indexable by frame.
    /// </summary>
    public List<FrameHistory> Game { get; private set; }

    /// <summary>
    /// The current last frame of the <see cref="Game"/>.
    /// </summary>
    public FrameHistory Last
    {
        get { return Game[Game.Count - 1]; }
    }

    #endregion

    // Events and Handlers
    #region Events

    /// <summary>
    /// Event notifying subscribers that the History has updated.
    /// </summary>
    public event Notify Updating;

    /// <summary>
    /// <see cref="Updating"/> event raiser.
    /// </summary>
    protected virtual void OnUpdate()
    {
        Updating?.Invoke();
    }

    #endregion

    // Class Construction
    #region Inititalization

    public History()
    {
        InitializeProperties();
    }

    /// <summary>
    /// Initializes properties on construction
    /// </summary>
    private void InitializeProperties()
    {
        Game = new List<FrameHistory>();
    }

    #endregion

    // Writing to History
    #region Writing

    /// <summary>
    /// Adds a frame to the <see cref="Game"/>.
    /// </summary>
    /// <param name="frame">The frame to be added</param>
    public void AddFrameHistory(FrameHistory frame)
    {
        Game.Add(frame);
        OnUpdate();
    }

    #endregion

    /// <summary>
    /// Class that contains the contents of a frame's history.
    /// </summary>
    public class FrameHistory
    {

        // Properties
        #region Properties

        /// <summary>
        /// The player's position on the given frame.
        /// </summary>
        public Vector3 Player_Position { get; private set; }

        /// <summary>
        /// Whether the player was dashing on the given frame.
        /// </summary>
        public bool Is_Dashing { get; private set; }

        /// <summary>
        /// Whether a beam was spawned on the given frame
        /// </summary>
        public bool Beam_Spawned { get; private set; }

        /// <summary>
        /// The position of the beam spawned this frame. 
        /// <c>null</c> if no beam was spawned.
        /// </summary>
        public Vector3 Beam_Position { get; private set; }

        /// <summary>
        /// The rotation of the beam spawned this frame. 
        /// <c>null</c> if no beam was spawned.
        /// </summary>
        public Quaternion Beam_Rotation { get; private set; }

        /// <summary>
        /// The scale of the beam spawned this frame. 
        /// <c>null</c> if no beam was spawned.
        /// </summary>
        public Vector3 Beam_Scale { get; private set; }

        #endregion

        // Class Construction
        #region Initialization

        /// <param name="player_position">The player's position.</param>
        /// <param name="is_dashing">Whether the player is currently dashing.</param>
        public FrameHistory(Vector3 player_position,
                            bool is_dashing)
                     
        {
            Player_Position = player_position;
            Is_Dashing = is_dashing;
            Beam_Spawned = false;
        }

        #endregion

        // Adding data to an existing FrameHistory
        #region Augmentation

        /// <summary>
        /// Adds a beam to this frame.
        /// </summary>
        /// <param name="position">The beam's position.</param>
        /// <param name="rotation">The beam's rotation.</param>
        /// <param name="local_scale">The beam's scale.</param>
        public void AddBeam(Vector3 position, Quaternion rotation, Vector3 local_scale)
        {
            Beam_Spawned = true;
            Beam_Position = position;
            Beam_Rotation = rotation;
            Beam_Scale= local_scale;
        }

        #endregion

    }
}
