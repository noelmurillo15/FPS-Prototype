/*
 * SpawnAddressable -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ANM.Utils.Addressables
{
    public class SpawnAddressable : MonoBehaviour
    {
        [SerializeField] private List<AssetReference> _particleReferences;

        private readonly Dictionary<AssetReference, List<GameObject>> _spawnedObject = new();

        //  The Queue holds requests to spawn an instanced that were made while we are already loading the asset
        //  They are spawned once the addressable is loaded, in the order requested
        private readonly Dictionary<AssetReference, Queue<Vector3>> _queuedSpawnRequests = new();
        private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationsHandles = new();


        public void Spawn(int index)
        {
            if (index < 0 || index >= _particleReferences.Count)
                return;

            AssetReference assetReference = _particleReferences[index];

            if (assetReference.RuntimeKeyIsValid() == false)
            {
                Debug.Log("Invalid Key " + assetReference.RuntimeKey);
                return;
            }

            if (_asyncOperationsHandles.ContainsKey(assetReference))
            {
                if (_asyncOperationsHandles[assetReference].IsDone)
                {
                    SpawnObjectFromLoadedReference(assetReference, GetRandomPosition());
                }
                else
                {
                    EnqueueSpawnForAfterInitialization(assetReference);
                }

                return;
            }

            LoadAndSpawn(_particleReferences[index]);
        }

        private void LoadAndSpawn(AssetReference assetReference)
        {
            var op = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(assetReference);
            _asyncOperationsHandles[assetReference] = op;
            op.Completed += _ =>
            {
                //  OnComplete Callback Lambda statement
                SpawnObjectFromLoadedReference(assetReference, GetRandomPosition());
                if (!_queuedSpawnRequests.ContainsKey(assetReference)) return;
                while (_queuedSpawnRequests[assetReference]?.Any() == true)
                {
                    var position = _queuedSpawnRequests[assetReference].Dequeue();
                    SpawnObjectFromLoadedReference(assetReference, position);
                }
            };
        }

        private void EnqueueSpawnForAfterInitialization(AssetReference assetRef)
        {
            if (_queuedSpawnRequests.ContainsKey(assetRef) == false)
            {
                _queuedSpawnRequests[assetRef] = new Queue<Vector3>();
            }

            _queuedSpawnRequests[assetRef].Enqueue(GetRandomPosition());
        }

        private void SpawnObjectFromLoadedReference(AssetReference assetReference, Vector3 position)
        {
            assetReference.InstantiateAsync(position, Quaternion.identity).Completed += (asyncOperationHandle) =>
            {
                if (_spawnedObject.ContainsKey(assetReference) == false)
                {
                    _spawnedObject[assetReference] = new List<GameObject>();
                }

                _spawnedObject[assetReference].Add(asyncOperationHandle.Result);
                var notify = asyncOperationHandle.Result.AddComponent<NotifyOnDestroy>();
                notify.DestroyedEvent += Remove;
                notify.AssetReference = assetReference;
            };
        }

        private static Vector3 GetRandomPosition()
        {
            return new(Random.Range(-5, 5), 1, Random.Range(-5, 5));
        }

        private void Remove(AssetReference assetReference, NotifyOnDestroy obj)
        {
            UnityEngine.AddressableAssets.Addressables.ReleaseInstance(obj.gameObject);
            _spawnedObject[assetReference].Remove(obj.gameObject);
            if (_spawnedObject[assetReference].Count != 0) return;
            Debug.Log($"Removed all {assetReference.RuntimeKey}");
            if (!_asyncOperationsHandles[assetReference].IsValid()) return;
            UnityEngine.AddressableAssets.Addressables.Release(_asyncOperationsHandles[assetReference]);
            _asyncOperationsHandles.Remove(assetReference);
        }
    }
}
