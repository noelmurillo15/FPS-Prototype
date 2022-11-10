/*
 * DestroyProjectile -
 * Created by : Allan N. Murillo
 * Last Edited : 8/24/2020
 */

using UnityEngine;

namespace ANM.Utils
{
    public class DestroyProjectile : MonoBehaviour
    {
        [SerializeField] private float timer = 3f;


        private void Awake()
        {
            Invoke(nameof(DestroyThis), timer);
        }

        private void DestroyThis()
        {
            Destroy(this.gameObject);
        }
    }
}