using System;
using System.Collections.Generic;
using DTO.Setup;
using Unity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace DTO.Storage
{

    /// <summary>
    /// A special implementation of <see cref="IList"/> that destroys duplicates added to the list.
    /// CAREFUL: This will destroy the original <see cref="UnityEngine.GameObject"/>.
    /// </summary>
    public class SingletonLevelComponentCollection<T> : IList<LevelComponent>
    {
        readonly IList<LevelComponent> _list = new List<LevelComponent>();

        /// <summary>
        /// Adds a new <see cref="LevelComponent"/> to the set. If this component already exists,
        /// it will destroy the original GameObject.
        /// </summary>
        /// <param name="item"></param>
        public void Add(LevelComponent item)
        {
            if (_list.Contains(item))
            {
                UnityEngine.Object.Destroy(item.gameObject);
                return;
            }

            _list.Add(item);
        }
        
        //The functionality in this region is identical to a normal List.
        #region Boiler Plate Implementation

        public LevelComponent this[int index] { get => _list[index]; set => _list[index] = value; }
        public int Count => _list.Count;
        public bool IsReadOnly => _list.IsReadOnly;
        public bool Contains(LevelComponent item)
        {
            return _list.Contains(item);
        }
        public void Clear()
        {
            _list.Clear();
        }
        public void CopyTo(LevelComponent[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
        public IEnumerator<LevelComponent> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        public int IndexOf(LevelComponent item)
        {
            return _list.IndexOf(item);
        }
        public void Insert(int index, LevelComponent item)
        {
            _list.Insert(index, item);
        }
        public bool Remove(LevelComponent item)
        {
            return _list.Remove(item);
        }
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Class that stores all of the generated <see cref="LevelComponent"/>.
    /// </summary>
    public class LevelRegistry
    {

        //Properties set in code
        #region Code Properties

        /// <summary>
        /// The floors in the level.
        /// </summary>
        public SingletonLevelComponentCollection<Floor> Floors { get; set; }

        /// <summary>
        /// The floor matrix in the level.
        /// </summary>
        public LevelInitializationMatrix FloorMatrix { get; set; }

        /// <summary>
        /// The gates in the level.
        /// </summary>
        public SingletonLevelComponentCollection<Gate> Gates { get; set; }

        /// <summary>
        /// The sensors in the level.
        /// </summary>
        public SingletonLevelComponentCollection<BoundarySensor> Sensors { get; set; }

        /// <summary>
        /// The walls in the level.
        /// </summary>
        public SingletonLevelComponentCollection<Wall> Walls { get; set; }

        #endregion

        // Class Construction
        #region Initialization

        public LevelRegistry()
        {
            Floors = new SingletonLevelComponentCollection<Floor>();
            Walls = new SingletonLevelComponentCollection<Wall>();
            Gates = new SingletonLevelComponentCollection<Gate>();
            Sensors = new SingletonLevelComponentCollection<BoundarySensor>();
            FloorMatrix = new LevelInitializationMatrix();
        }

        #endregion

    }
}
