using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

/// <summary>
/// Object with storage and methods relating to the player's inventory.
/// </summary>
public class Inventory
{

    // Important Properties
    #region Properties

    /// <summary>
    /// Contains all allowable values of the <see cref="Collectable.Type"/> attribute.
    /// </summary>
    public enum CollectableType
    {
        diamond,
        seashell,
        lavender,
        ruby,
        coal,
        unspecified
    }

    /// <summary>
    /// The amount of each item in the player's inventory.
    /// </summary>
    public SortedDictionary<CollectableType, int> Items { get; private set; }

    private IEnumerable<CollectableType> collectableTypes = (IEnumerable<CollectableType>) System.Enum.GetValues(typeof(CollectableType));

    public int dimension
    {
        get 
        {
            int _dimension = 0;
            foreach(CollectableType type in collectableTypes)
            {
                if(Items[type] > 0) { _dimension++; }
            }
            return _dimension;
        }
    }

    public List<CollectableType> typesRequired {
        get
        {
            var output = new List<CollectableType>();
            foreach (CollectableType type in collectableTypes)
            {
                if (Items[type] != 0) { output.Add(type); }
            }
            return output;
        }
    }

    #endregion

    // Class Construction
    #region Initialization

    public Inventory()
    {
        PopulateItemsDict();
    }

    public Inventory(int type0 = 0, int type1 = 0, int type2 = 0, int type3 = 0, int type4 = 0)
    {
        PopulateItemsDict();
        Items[CollectableType.diamond] = type0;
        Items[CollectableType.seashell] = type1;
        Items[CollectableType.lavender] = type2;
        Items[CollectableType.ruby] = type3;
        Items[CollectableType.coal] = type4;
    }

    /// <summary>
    /// Initializes the <see cref="Items"/> property.
    /// </summary>
    void PopulateItemsDict() 
    {
        Items = new SortedDictionary<CollectableType, int>();
        foreach (CollectableType type in collectableTypes)
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
        var sb = new StringBuilder();

        foreach (CollectableType type in Items.Keys)
        {
            sb.Append(Items[type].ToString());
            sb.Append(", ");
        }
        sb = sb.Remove(sb.Length - 5, 5);

        if (Items.Count == 0)
        {
            return "Inventory is empty.";
        }

        return sb.ToString();
    }

    #endregion

    // Static Methods on Inventory
    #region Static Methods

    public static Inventory operator -(Inventory a, Inventory b)
    {
        var c = new Inventory();

        foreach (CollectableType item in a.Items.Keys)
        {
            c.Items[item] = Mathf.Max(0, a.Items[item] - b.Items[item]);
        }

        return c;

    }

    /// <summary>
    /// Whether inventory1 is a subset of inventory2.
    /// </summary>
    public static bool IsSubInventory(Inventory inventory1, Inventory inventory2)
    {
        bool truth = true;

        foreach (CollectableType item in inventory1.Items.Keys)
        {
            truth &= (inventory1.Items[item] <= inventory2.Items[item]);
        }

        return truth;
    }

    #endregion

}


