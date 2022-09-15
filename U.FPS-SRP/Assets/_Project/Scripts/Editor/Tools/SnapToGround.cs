/*
 * SnapToGround -
 * Created by : Allan N. Murillo
 * Last Edited : 7/28/2021
 */


using UnityEditor;
using UnityEngine;

namespace ANM.Editor.Tools
{
    public class SnapToGround : MonoBehaviour
    {
        [MenuItem("Custom/Snap To Ground %g")]
        public static void Ground()
        {
            foreach (var transform in Selection.transforms)
            {
                RaycastHit hit;
                var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 15f);
                foreach (var raycastHit in hits)
                {
                    if (raycastHit.collider.gameObject == transform.gameObject)
                        continue;

                    transform.position = raycastHit.point;
                    break;
                }
            }
        }
    }
}
