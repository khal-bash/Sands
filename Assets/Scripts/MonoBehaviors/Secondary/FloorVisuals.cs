using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Delegates;
using System.Linq;

/// <summary>
/// The possible themes of floors.
/// </summary>
public enum Theme
{
    unspecified,
    desert,
    ice,
    grasslands,
    temple,
    snow
}

/// <summary>
/// Class governing the visuals of floors.
/// </summary>
public class FloorVisuals
{

    //Events and Invokers
    #region Events

    /// <summary>
    /// Event that notifies subscribers when the theme is changed.
    /// </summary>
    public event Notify ThemeSet;

    /// <summary>
    /// <see cref="ThemeSet"/> event invoker.
    /// </summary>
    protected virtual void OnThemeSet()
    {
        ThemeSet?.Invoke();
    }

    #endregion

    //Properties set in code
    #region Code Properties

    /// <summary>
    /// The theme determining the visuals.
    /// </summary>
    public Theme theme
    {
        get => _theme;
        set
        {
            _theme = value;
            InitializeDependentComponents();
            OnThemeSet();
        }
    }

    /// <summary>
    /// The background variable holding the Theme.
    /// </summary>
    private Theme _theme;

    /// <summary>
    /// The background color of the floor.
    /// </summary>
    public Color background_color { get; private set; }


    #endregion

    // Class Construction and Initialization
    #region Initialization

    public FloorVisuals(Theme _theme = Theme.unspecified)
    {
        theme = _theme;
        InitializeDependentComponents();
    }

    /// <summary>
    /// Initializes various properties that depend on the theme.
    /// </summary>
    private void InitializeDependentComponents()
    {
        switch (theme)
        {
            case Theme.desert:
                CookieCutterInitializer(background_color = new Color32(220, 104, 75, 255)
                    );
                break;
            case Theme.ice:
                CookieCutterInitializer(background_color = new Color32(60, 116, 189, 255)
                    );
                break;
            case Theme.grasslands:
                CookieCutterInitializer(background_color = new Color32(189, 242, 99, 255)
                    );
                break;
            case Theme.temple:
                CookieCutterInitializer(background_color = new Color32(45, 28, 20, 255)
                    );
                break;
            case Theme.snow:
                CookieCutterInitializer(background_color = new Color32(208, 216, 217, 255)
                    );
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets the dependent properties.
    /// </summary>
    /// <param name="background">The background color.</param>
    private void CookieCutterInitializer(Color background)
    {
        background_color = background;
    }

    #endregion

    //Public Static Functions
    #region Convenience

    /// <summary>
    /// All possible <see cref="Theme"/>s in list form, excluding the default <see cref="Theme.unspecified"/>.
    /// </summary>
    public static List<Theme> AllSpecifiedThemes { 
        get 
        {
            List<Theme> possibleThemes = Enum.GetValues(typeof(Theme)).Cast<Theme>().ToList();
            possibleThemes.Remove(Theme.unspecified);
            return possibleThemes;
        }
    }

    #endregion

}
