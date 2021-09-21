/*
 * ItemSounds -
 * Created by : Allan N. Murillo
 * Last Edited : 4/10/2020
 */

using UnityEngine;

namespace ANM.FPS.Items
{
    public class ItemSounds : MonoBehaviour
    {
        [SerializeField] private float defaultVolume = 0.5f;
        [SerializeField] private AudioClip throwSound = null;
        private ItemMaster _itemMaster;
        

        private void OnEnable()
        {
            Initialize();
        }
        
        private void OnDisable()
        {
            _itemMaster.EventObjectThrow -= PlaySound;
        }
        
        private void Initialize()
        {
            _itemMaster = GetComponent<ItemMaster>();
            _itemMaster.EventObjectThrow += PlaySound;
        }

        private void PlaySound()
        {
            if (throwSound != null)
            {
                AudioSource.PlayClipAtPoint(throwSound, transform.position, defaultVolume);
            }
        }
    }
}
