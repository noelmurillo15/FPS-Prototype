/*
 * GameTime -
 * Created By : Allan Murillo
 * Last Edited : 7/28/2021
 */

using UnityEngine;

namespace ANM.Utils
{
    public class GameTime : MonoBehaviour
    {
        public Transform sun;
        [SerializeField] private float dayCycleinMinutes = 1f;

        public const float second = 1f;
        public const float minute = 60f * second;
        public const float hour = 60f * minute;
        public const float day = 24 * hour;

        private float degreesPerSecond = 360 / day;
        private float degreeRotation;
        private float timeOfDay;

        // Use this for initialization
        private void Start()
        {
            timeOfDay = 0;
            degreeRotation = degreesPerSecond * day / (dayCycleinMinutes * minute);
        }

        // Update is called once per frame
        private void Update()
        {
            sun.Rotate(new Vector3(degreeRotation, 0, 0) * Time.deltaTime);
            timeOfDay += Time.deltaTime;
        }
    }
}