using System;
using System.Collections;
using System.Collections.Generic;
using static Inventory;
using static Utilities.Visual;
using static DTO.Visuals.ThemeHandler;
using DTO.Storage;
using UnityEngine;
using TMPro;

/// <summary>
/// Class governing the inventory display.
/// </summary>
public class InventoryDisplay : MonoBehaviour
{

    //Properties set in code.
    #region Code Properties

    /// <summary>
    /// The ordering of which <see cref="CollectableType"></see> is being displayed in which slot.
    /// </summary>
    private CollectableType[] CurrentDisplayOrdering = new CollectableType[5] { CollectableType.unspecified,
                                                                                CollectableType.unspecified,
                                                                                CollectableType.unspecified,
                                                                                CollectableType.unspecified,
                                                                                CollectableType.unspecified};

    #endregion

    //Built-in Unity Functions
    #region Unity Functions

    private void Start()
    {
        InitializeDisplay();
        Beam.ItemCollected += OnItemCollected;
        Gate.GateOpened += OnItemsRemoved;
    }

    #endregion

    //Initialization
    #region Initialization

    /// <summary>
    /// Initializes the Inventory Display at the beginning of the game.
    /// </summary>
    private void InitializeDisplay()
    {
        Transform colorObjectGroup = transform.GetChild(0);
        Transform textObjectGroup = transform.GetChild(1);

        foreach (var renderer in colorObjectGroup.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = ChangeOpacity(renderer.color, 0f);
        }

        foreach (var textbox in textObjectGroup.GetComponentsInChildren<TextMeshPro>())
        {
            textbox.text = "";
        }
    }

    /// <summary>
    /// Initializes a new entry to the display. Doesn't effect the text value.
    /// </summary>
    /// <param name="type"></param>
    private void InitializeNewEntry(CollectableType type)
    {
        int firstAvailableEntry = Array.IndexOf(CurrentDisplayOrdering, CollectableType.unspecified);
        CurrentDisplayOrdering[firstAvailableEntry] = type;

        var renderer = transform.GetChild(0).GetChild(firstAvailableEntry).GetComponent<SpriteRenderer>();
        renderer.color = Accord(type);
        renderer.color = ChangeOpacity(renderer.color, 1f);

        var textBox = transform.GetChild(1).GetChild(firstAvailableEntry).GetComponent<TextMeshPro>();
        textBox.color = Accord(type, "accent");
    }

    #endregion

    //Collection
    #region Adding Items

    /// <summary>
    /// Method subscribing to the <see cref=Beam.ItemCollected"/> event.
    /// </summary>
    /// <param name="collectable"></param>
    private void OnItemCollected(GameObject collectable)
    {
        CollectableType type = collectable.GetComponent<Collectable>().Type;

        var index = Array.IndexOf(CurrentDisplayOrdering, type);

        if (index == -1)
        {
            InitializeNewEntry(type);
        }

        Increment(type);

    }

    /// <summary>
    /// Increments a slot in the display.
    /// </summary>
    /// <param name="type">The collectable type to add to the display.</param>
    private void Increment(CollectableType type)
    {
        var index = Array.IndexOf(CurrentDisplayOrdering, type);
        var textBox = transform.GetChild(1).GetChild(index).GetComponent<TextMeshPro>();

        if (textBox.text == "")
        {
            textBox.text = "1";
        }
        else
        {
            int.TryParse(textBox.text, out int currentValue);
            currentValue++;
            textBox.text = currentValue.ToString();

        }
    }

    #endregion

    //Subtraction
    #region Removing Items

    /// <summary>
    /// Method subscribing to the <see cref="Gate.GateOpened"/>
    /// </summary>
    /// <param name="inventory"></param>
    private void OnItemsRemoved(Inventory inventory)
    {
        foreach (var type in inventory.TypesRequired)
        {
            Decrement(type, inventory.Items[type]);
        }
    }

    /// <summary>
    /// Remove a collectable from the display.
    /// </summary>
    /// <param name="type">The collectable type to add to the display.</param>
    /// <param name="value">The number of collectables to remove.</param>
    private void Decrement(CollectableType type, int value)
    {
        var index = Array.IndexOf(CurrentDisplayOrdering, type);
        var textBox = transform.GetChild(1).GetChild(index).GetComponent<TextMeshPro>();

        int.TryParse(textBox.text, out int currentValue);
        currentValue -= value;

        if (currentValue > 0)
        {
            textBox.text = currentValue.ToString();
            return;
        }

        var renderer = transform.GetChild(0).GetChild(index).GetComponent<SpriteRenderer>();
        renderer.color = ChangeOpacity(renderer.color, 0);
        textBox.text = "";
        CurrentDisplayOrdering[index] = CollectableType.unspecified;

    }

    #endregion

}
