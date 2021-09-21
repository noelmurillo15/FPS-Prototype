/*
 * SpawnProximity - spawns prefabs when the player is close enough (used for large open world maps)
 * Created By : Allan Murillo
 * Last Edited : 7/28/2021
 */

using UnityEngine;

namespace ANM.Utils
{
    public class SpawnProximity : MonoBehaviour
    {
        public GameObject objectToSpawn;
        public int numberToSpawn;
        public float proximity;
        private float checkRate;
        private float nextCheck;

        private Transform myTransform;
        private Transform playerTransform;
        private Vector3 spawnPosition;


        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            myTransform = transform;
            //playerTransform = GameControl.gControl._playerRef.transform;
            checkRate = Random.Range(0.8f, 1.2f);
        }

        private void Update()
        {
            CheckDistance();
        }

        private void CheckDistance()
        {
            if (!(Time.time > nextCheck)) return;
            nextCheck = Time.time + checkRate;
            if (!(Vector3.Distance(myTransform.position, playerTransform.position) < proximity)) return;
            Spawn();
            this.enabled = false;
        }

        private void Spawn()
        {
            for (int i = 0; i < numberToSpawn; i++)
            {
                spawnPosition = myTransform.position + Random.insideUnitSphere * 30;
                Instantiate(objectToSpawn, new Vector3(spawnPosition.x, 0f, spawnPosition.z), myTransform.rotation);
            }
        }
    }
}
