using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

/// <summary>
/// Object with storage and methods relating to the player's inventory.
/// </summary>
public class Inventory
{

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

    // Important Properties
    #region Properties

    /// <summary>
    /// An <see cref="IEnumerable"/> of all the types in <see cref="CollectableType"/>, including the default
    /// <see cref="CollectableType.unspecified"/>.
    /// </summary>
    private readonly IEnumerable<CollectableType> AllCollectableTypes = (IEnumerable<CollectableType>)System.Enum.GetValues(typeof(CollectableType));

    /// <summary>
    /// Gets the number of different required <see cref="CollectableType"/> in this inventory.
    /// </summary>
    public int Dimension { get => TypesRequired.Count; }

    /// <summary>
    /// The amount of each item in the player's inventory.
    /// </summary>
    public SortedDictionary<CollectableType, int> Items { get; private set; }

    /// <summary>
    /// A list of all <see cref="CollectableType"/> required in this inventory.
    /// </summary>
    public List<CollectableType> TypesRequired {
        get
        {
            var output = new List<CollectableType>();
            foreach (CollectableType type in AllCollectableTypes)
            {
                if (type == CollectableType.unspecified) { continue; }
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

    public Inventory(int diamond = 0, int seashell = 0, int lavender = 0, int ruby = 0, int coal = 0)
    {
        PopulateItemsDict();
        Items[CollectableType.diamond] = diamond;
        Items[CollectableType.seashell] = seashell;
        Items[CollectableType.lavender] = lavender;
        Items[CollectableType.ruby] = ruby;
        Items[CollectableType.coal] = coal;
    }

    /// <summary>
    /// Initializes the <see cref="Items"/> property.
    /// </summary>
    void PopulateItemsDict() 
    {
        Items = new SortedDictionary<CollectableType, int>();
        foreach (CollectableType type in AllCollectableTypes)
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


