using DTO.Setup;
using System;
using System.Linq;
using DTO.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public System.Random random { get; private set;}

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
        if (seed == -1) { random = new System.Random(); }
        else { random = new System.Random(seed); }
    }

    #endregion

}
