using DTO.Setup;
using System;
using System.Linq;
using DTO.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class governing the generation of the level in the <see cref="LevelSetupWizard"/>
/// </summary>
public class LevelMetaData : MonoBehaviour
{

    //Properties set in the inspector
    #region Inspector Properties

    /// <summary>
    /// The number of floors to generate.
    /// </summary>
    public int numberOfFloors;

    /// <summary>
    /// The seed for the random generator to use. -1 represents an unspecified value.
    /// </summary>
    public int seed = -1;

    #endregion

    //Properties set in code
    #region Code Properties

    /// <summary>
    /// The random generator to use.
    /// </summary>
    public System.Random Random { get; private set;}

    /// <summary>
    /// The <see cref="LevelRegistry"/> holding the components in the level.
    /// </summary>
    public LevelRegistry Registry { get; set; }

    #endregion

    //Initialization
    #region Initialization

    private void Awake()
    {
        InitializeCodeProperties();   
    }

    /// <summary>
    /// Initializes various propereties set in code.
    /// </summary>
    private void InitializeCodeProperties()
    {
        if (seed == -1) { Random = new System.Random(); }
        else { Random = new System.Random(seed); }
        Registry = new LevelRegistry();
    }

    #endregion

}
