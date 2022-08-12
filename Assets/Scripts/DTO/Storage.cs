using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTO.Storage
{
    
    public static class StoredGameObjects
    {
        public static GameObject Player { get; set; }
    }

    public static class StoredClasses
    { 
        public static Health Player_HP { get; set; }
        public static Inventory Player_Inventory { get; set; }
        public static History Player_History { get; set; }
        
    }

    public static class StoredComponents
    {
        public static Player Player { get; set; }
        public static Transform Player_Transform { get; set; }
    }

    public class Operations : MonoBehaviour
    {

        public static void Fill_Storage()
        {
            MakeFindCalls();
            MakeComponentCalls();
        }

        private static void MakeFindCalls()
        {
            StoredGameObjects.Player = GameObject.Find("Player");
        }

        private static void MakeComponentCalls()
        {
            StoredComponents.Player = StoredGameObjects.Player.GetComponent<Player>();
            StoredComponents.Player_Transform = StoredGameObjects.Player.transform;

            StoredClasses.Player_HP = StoredComponents.Player.HP;
            StoredClasses.Player_Inventory = StoredComponents.Player.Inventory;
            StoredClasses.Player_History = StoredComponents.Player.History;

        }

        
    }

}
