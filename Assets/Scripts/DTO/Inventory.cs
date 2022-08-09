using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object with storage and methods relating to the player's inventory.
/// </summary>
public class Inventory
{

    public Inventory()
    {
        PopulateItemsDict();
    }

    public Inventory(int type0 = 0, int type1 = 0, int type2 = 0, int type3 = 0)
    {
        PopulateItemsDict();
        Items[CollectableType.type0] = type0;
        Items[CollectableType.type1] = type1;
        Items[CollectableType.type2] = type2;
        Items[CollectableType.type3] = type3;
    }

    // Important Properties
    #region Properties

    /// <summary>
    /// Contains all allowable values of the <see cref="Collectable.Type"/> attribute.
    /// </summary>
    public enum CollectableType
    {
        type0,
        type1,
        type2,
        type3
    }

    /// <summary>
    /// The amount of each item in the player's inventory.
    /// </summary>
    public SortedDictionary<CollectableType, int> Items { get; private set; }

    #endregion

    // Initialization on Construction
    #region Initialization

    /// <summary>
    /// Initializes the <see cref="Items"/> property.
    /// </summary>
    void PopulateItemsDict() 
    {
        Items = new SortedDictionary<CollectableType, int>();
        foreach (CollectableType type in System.Enum.GetValues(typeof(CollectableType)))
        {
            Items.Add(type, 0);
        }
    }

    #endregion

    // Operations on the Inventory
    #region Operations

    /// <summary>
    /// Adds a collectable to <see cref="Items"/>
    /// </summary>
    /// <param name="type">The <see cref="CollectableType"/> of the item to be added.</param>
    public void AddItem(CollectableType type)
    {
        Items[type]++;
    }

    /// <summary>
    /// A Debug method from querying the inventory. Not to be used in production.
    /// </summary>
    /// <returns>A string encoding <see cref="Items"/></returns>
    public string DebugRead()
    {
        string print = "";

        foreach (CollectableType type in Items.Keys)
        {
            print += Items[type].ToString();
            print += ", ";
        }

        if (Items.Count == 0)
        {
            print = "Inventory is empty.";
        }

        return print;
    }

    #endregion

    public static Inventory operator -(Inventory a, Inventory b)
    {
        var c = new Inventory();

        foreach (CollectableType item in a.Items.Keys)
        {
            c.Items[item] = Mathf.Max(0, a.Items[item] - b.Items[item]);
        }

        return c;

    }

    public static bool IsSubInventory(Inventory inventory1, Inventory inventory2)
    {
        bool truth = true;

        foreach (CollectableType item in inventory1.Items.Keys)
        {
            truth = truth & (inventory1.Items[item] <= inventory2.Items[item]);
        }

        return truth;
    }

}


