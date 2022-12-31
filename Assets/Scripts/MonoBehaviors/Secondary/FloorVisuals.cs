using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTO.Delegates;
using System.Linq;
using DTO.Visuals;

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
    public Theme Theme
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
    public Color BackgroundColor { get; private set; }


    #endregion

    // Class Construction and Initialization
    #region Initialization

    public FloorVisuals(Theme _theme = Theme.unspecified)
    {
        Theme = _theme;
        InitializeDependentComponents();
    }

    /// <summary>
    /// Initializes various properties that depend on the theme.
    /// </summary>
    private void InitializeDependentComponents()
    {
        BackgroundColor = ThemeHandler.Set(Theme, "background");
    }

    #endregion

    //Public Static Functions
    #region Convenience

    /// <summary>
    /// All possible <see cref="DTO.Visuals.Theme"/>s in list form, excluding the default <see cref="Theme.unspecified"/>.
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
