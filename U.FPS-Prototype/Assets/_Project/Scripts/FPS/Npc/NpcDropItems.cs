/*
 * NpcDropItems -
 * Created by : Allan N. Murillo
 * Last Edited : 4/7/2020
 */

using UnityEngine;
using System.Collections;
using ANM.FPS.Items;

namespace ANM.FPS.Npc
{
    public class NpcDropItems : MonoBehaviour
    {
        private NpcMaster _npcMaster;
        [SerializeField] private GameObject[] itemsToDrop;
        private const float TimeToWait = 0.05f;


        private void Initialize()
        {
            _npcMaster = GetComponent<NpcMaster>();
        }

        private void OnEnable()
        {
            Initialize();
            _npcMaster.EventNpcDie += DropItems;
        }

        private void OnDisable()
        {
            _npcMaster.EventNpcDie -= DropItems;
        }

        private void DropItems()
        {
            if (itemsToDrop == null) return;
            if (itemsToDrop.Length <= 0) return;

            foreach (var item in itemsToDrop)
            {
                StartCoroutine(PauseBeforeDrop(item));
            }
        }

        private static IEnumerator PauseBeforeDrop(GameObject itemToDrop)
        {
            yield return new WaitForSeconds(TimeToWait);
            itemToDrop.SetActive(true);
            itemToDrop.transform.parent = null;
            yield return new WaitForSeconds(TimeToWait);

            if (itemToDrop.GetComponent<ItemMaster>() != null)
            {
                itemToDrop.GetComponent<ItemMaster>().CallEventObjectThrow();
            }
        }
    }
}
