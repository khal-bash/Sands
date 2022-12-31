using UnityEngine;
using DTO.Storage;

/// <summary>
/// Governs the behavior of a Heart in the On-Screen health display.
/// </summary>
public class Heart : MonoBehaviour
{

    // Properties set in the Inspector.
    #region Inspector Properties

    /// <summary>
    /// The ID of the Heart. Used to determine whether the heart should be visible.
    /// </summary>
    public int ID;

    #endregion

    // Properties set in code.
    #region Code Properties

    /// <summary>
    /// The <see cref="SpriteRenderer"/> component on the Heart object.
    /// </summary>
    private SpriteRenderer SR { get; set; }

    #endregion

    // Built-in Unity Functions
    #region Unity Functions

    private void Awake()
    {
        InitializeCodeProperties();
    }

    private void Start()
    {
        StoredClasses.Player_HP.HPUpdated += Health_Changed;
    }

    #endregion

    // Initialization
    #region Initialization

    private void InitializeCodeProperties()
    {
        SR = gameObject.GetComponent<SpriteRenderer>();
    }

    #endregion

    // Event Subscribers
    #region Subscribers

    /// <summary>
    /// Subscriber to the <see cref="Health.HPUpdated"/> event.
    /// Determines whether or not the Heart should be visible.
    /// </summary>
    /// <param name="new_HP">The new HP.</param>
    public void Health_Changed(int new_HP)
    {
        if (ID >= new_HP)
        {
            Color new_color = Utilities.Visual.ChangeOpacity(SR.color, 0f);
            SR.color = new_color;
        }
    }

    #endregion

}
