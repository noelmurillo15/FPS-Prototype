/*
 * NotifyOnDestroy -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace ANM.Utils.Addressables
{
    public class NotifyOnDestroy : MonoBehaviour
    {
        public event Action<AssetReference, NotifyOnDestroy> DestroyedEvent;

        public AssetReference AssetReference { get; set; }


        public void OnDestroy()
        {
            DestroyedEvent?.Invoke(AssetReference, this);
        }
    }
}
