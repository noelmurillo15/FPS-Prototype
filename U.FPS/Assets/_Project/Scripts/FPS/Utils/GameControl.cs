/*
 * GameControl -
 * Created by : Allan N. Murillo
 * Last Edited : 2/5/2020
 */

using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace ANM.Utils
{
    public class GameControl : MonoBehaviour
    {
        //public static GameControl gControl;

        public float health;
        public float experience;

        public GameObject _playerRef;


        private void Awake()
        {
            //  Singleton Design Pattern
            /*if (gControl == null)
            {
                DontDestroyOnLoad(gameObject);
                gControl = this;
                gControl._playerRef = GameObject.FindGameObjectWithTag("Player");
            }
            else if (gControl != this)
            {
                Destroy(gameObject);
            }*/
        }

        private void OnGUI()
        {
            /*GUI.Label(new Rect(10, 10, 100, 30), "Health : " + health);
            GUI.Label(new Rect(10, 40, 150, 30), "Experience : " + experience);*/
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

            PlayerData data = new PlayerData();
            data.health = health;
            data.experience = experience;

            bf.Serialize(file, data);
            file.Close();
        }

        public void Load()
        {
            if (!File.Exists(Application.persistentDataPath + "/playerInfo.dat")) return;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            experience = data.experience;
        }
    }

    [Serializable]
    internal class PlayerData
    {
        public float health;
        public float experience;

        public void DeductHealth(float damage)
        {
            health -= damage;
        }
    }
}