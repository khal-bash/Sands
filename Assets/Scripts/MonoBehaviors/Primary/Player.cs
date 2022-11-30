using DTO.Delegates;
using UnityEngine;

/// <summary>
/// Governs the behavior of the user-controlled player.
/// </summary>
public class Player : MonoBehaviour
{


    // Properties set in the Inspector
    #region Inspector Properties

    /// <summary>
    /// Gets or sets the base speed of the player.
    /// Do not modify unless you know what you're doing.
    /// </summary>
    public float Speed;

    /// <summary>
    /// Gets or sets the number of frames it takes for the player's dash to recharge.
    /// Do not modify unless you know what you're doing.
    /// </summary>
    public int Dash_Recharge;

    /// <summary>
    /// Gets or sets the strength of the player's dash (measured as a multiplier applied to <c>speed</c>).
    /// Do not modify unless you know what you're doing.
    /// </summary>
    public float Dash_Power;

    /// <summary>
    /// Gets or sets the number of frames that the player's dash is applied. 
    /// Do not modify unless you know what you're doing.
    /// </summary>
    public int Dash_Length;

    /// <summary>
    /// Gets or sets the <c>GameObject</c> instantiated when the player fires the mining beam.
    /// </summary>
    public GameObject Beam;

    #endregion

    // Properties set in code
    #region Code Properties

    /// <summary>
    /// Gets or sets the number of frames since the player dashed.
    /// </summary>
    private int Frames_Since_Dash { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is currently dashing.
    /// </summary>
    private bool Dashing { get; set; }

    /// <summary>
    /// Gets the Inventory instance associated with the player.
    /// </summary>
    public Inventory Inventory { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the beam will glitch if the player attempts to fire.
    /// </summary>
    public bool Glitchy { get; set; }

    /// <summary>
    /// If <see cref="Glitchy"/>, gets the <see cref="GameObject"/> of the <see cref="Glitch"/> that is causing the beam to be glitchy.
    /// </summary>
    public GameObject Glitch_Zone { get; set; }

    /// <summary>
    /// The complete <see cref="global::History"/> of the game. Used to determine spawn behavior.
    /// </summary>
    public History History { get; set; }

    /// <summary>
    /// The Player's <see cref="Health"/>.
    /// </summary>
    public Health HP { get; set; }

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    private void Awake()
    {
        InitializeCodeProperties();
    }

    private void Start()
    {
        transform.position = Vector3.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryFire();
        }

        bool dash_charged = Frames_Since_Dash == Dash_Recharge;
        if (Input.GetMouseButtonDown(1) & dash_charged)
        {
            StartDash();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory();
        }

    }

    private void FixedUpdate()
    {
        Move();
        ManageDash();
        SaveFrameDataToHistory();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            OnDamage(collider.gameObject);
        }
    }

    #endregion

    // Events and Raisers
    #region Events

    /// <summary>
    /// An event notifying subscribers which object has damaged the player.
    /// </summary>
    public event GameObjectInputAction Damage;

    /// <summary>
    /// The event raiser for <see cref="Damage"/>.
    /// </summary>
    /// <param name="enemy">The <see cref="GameObject"/> that caused the damage.</param>
    protected virtual void OnDamage(GameObject enemy)
    {
        Damage?.Invoke(enemy);
    }

    #endregion

    // Initialization
    #region Initialization

    /// <summary>
    /// Initializes all properties in the CodeProperties region.
    /// </summary>
    private void InitializeCodeProperties()
    {
        Frames_Since_Dash = Dash_Recharge;
        Dashing = false;
        Inventory = new Inventory();
        Glitchy = false;
        Glitch_Zone = null;
        History = new History();
        HP = GameObject.Find("Health Display").GetComponent<Health>();
        DTO.Storage.Operations.Fill_Storage();
    }
    
    #endregion

    // Player Movement
    #region Movement

    /// <summary>
    /// Moves the player (WASD only) by directly adjusting the position.
    /// </summary>
    void Move()
    {
        float dash_adjusted_speed = Speed;
        Vector3 proposed_move = Vector3.zero;

        if (Dashing) 
        {
            dash_adjusted_speed *= Dash_Power;
        }

        if (Input.GetKey(KeyCode.W))
        {
            proposed_move += (dash_adjusted_speed * Vector3.up);
        }

        if (Input.GetKey(KeyCode.A))
        {
            proposed_move += (dash_adjusted_speed * Vector3.left);
        }

        if (Input.GetKey(KeyCode.S))
        {
            proposed_move += (dash_adjusted_speed * Vector3.down);
        }

        if (Input.GetKey(KeyCode.D))
        {
            proposed_move += (dash_adjusted_speed * Vector3.right);
        }

        if (CheckForClip(proposed_move))
        {
            transform.position += proposed_move;
        }
    }

    /// <summary>
    /// Determines whether the player would clip through an Impassable object given a proposed move.
    /// </summary>
    /// <param name="proposed_move">The proposed change in position.</param>
    /// <returns><c>True</c> if the move doesn't clip through an object; <c>False</c> if it would. </returns>
    bool CheckForClip(Vector3 proposed_move)
    {
        LayerMask mask = LayerMask.GetMask("Impassable");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, proposed_move, proposed_move.magnitude, layerMask:mask);
        return (hit.collider == null);
    }

    /// <summary>
    /// Initializes parameters to start a dash.
    /// </summary>
    private void StartDash()
    {
        Dashing = true;
        Frames_Since_Dash = 0;
    }

    /// <summary>
    /// Increments the recharge parameters and ends the dash.
    /// </summary>
    private void ManageDash()
    {
        if (Frames_Since_Dash < Dash_Recharge)
        {
            Frames_Since_Dash += 1;
        }

        if (Frames_Since_Dash == Dash_Length)
        {
            Dashing = false;
        }
    }

    #endregion

    // Non-movement inputs
    #region Other Inputs

    /// Inventory Functions
    #region Inventory
    /// <summary>
    /// Displays the player's <see cref="Inventory"/>.
    /// </summary>
    void ShowInventory()
    {
        string inventory_contents = Inventory.DebugRead();
        Debug.Log(inventory_contents);
    }
    #endregion

    /// Shooting
    #region Firing

    /// <summary>
    /// Attempts to fire the beam.
    /// </summary>
    void TryFire()
    {
        if (!Glitchy)
        {
            SpawnBeam();
        }
        else
        {
            Glitch_Zone.GetComponent<Glitch>().ShowGlitchZone();
        }

    }

    /// <summary>
    /// Spawns a beam bridging the crosshairs and player.
    /// </summary>
    void SpawnBeam()
    {

        Transform crosshairs_transform = gameObject.transform.GetChild(0);

        ///localPosition essentially uses parent.localScale as the unit vectors, so this need to be corrected.
        Vector3 line_of_fire = Vector3.Scale(crosshairs_transform.localPosition, transform.localScale);

        Vector3 euler = Utilities.Vector.Get2DRotationFromVector3(line_of_fire);

        Vector3 position = transform.position + (line_of_fire / 2);
        Quaternion rotation = Quaternion.Euler(euler);

        GameObject created_beam = Instantiate(Beam, position, rotation);
        created_beam.transform.localScale = new Vector3(created_beam.transform.localScale.x,
                                                        line_of_fire.magnitude);

    }

    #endregion

    #endregion

    // Saving history
    #region Archiving

    /// <summary>
    /// Saves out the relevant values to <see cref="History"/>.
    /// </summary>
    void SaveFrameDataToHistory()
    {
        History.FrameHistory frame = new History.FrameHistory(transform.position, Dashing);
        History.AddFrameHistory(frame);
    }

    #endregion

}

