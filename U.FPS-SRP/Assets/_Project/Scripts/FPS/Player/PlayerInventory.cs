/*
 * PlayerInventory -
 * Created by : Allan N. Murillo
 * Last Edited : 4/18/2020
 */

using System.Collections;
using System.Collections.Generic;
using ANM.Game_Management;
using UnityEngine;
using UnityEngine.UI;

namespace ANM.FPS.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        public Transform inventoryPlayerParent;
        public Transform inventoryUiParent;
        public GameObject uiButton;

        private int _counter;
        private const float EquipTime = .1f;
        private string _buttonText;

        private Transform _heldItem;
        private PlayerMaster _playerMaster;
        private Transform _timeToPlaceInHands;
        private GameToggleInventory _inventoryUiRef;
        private readonly List<Transform> _listInventory = new List<Transform>();


        private void Initialize()
        {
            _inventoryUiRef = GameObject.FindWithTag("GameController").GetComponent<GameToggleInventory>();
            _playerMaster = GetComponent<PlayerMaster>();
        }

        private void OnEnable()
        {
            Initialize();
            DeactivateAllItems();
            UpdateInventoryList();
            CheckHandsEmpty();

            _playerMaster.InventoryChangeEvent += UpdateInventoryList;
            _playerMaster.InventoryChangeEvent += CheckHandsEmpty;
            _playerMaster.HandsEmptyEvent += ClearHands;
        }

        private void OnDisable()
        {
            _playerMaster.InventoryChangeEvent -= UpdateInventoryList;
            _playerMaster.InventoryChangeEvent -= CheckHandsEmpty;
            _playerMaster.HandsEmptyEvent -= ClearHands;
        }

        private void UpdateInventoryList()
        {
            _counter = 0;
            _listInventory.Clear();
            _listInventory.TrimExcess();

            ClearInventoryUi();

            if (inventoryPlayerParent != null)
            {
                foreach (Transform child in inventoryPlayerParent)
                {
                    if (!child.CompareTag("Item")) continue;
                    _listInventory.Add(child);
                    GameObject go = Instantiate(uiButton) as GameObject;
                    _buttonText = child.name;
                    go.GetComponentInChildren<Text>().text = _buttonText;
                    int index = _counter;
                    go.GetComponent<Button>().onClick.AddListener(delegate { ActivateInventoryItem(index); });
                    go.GetComponent<Button>().onClick.AddListener(_inventoryUiRef.ToggleInventory);
                    go.transform.SetParent(inventoryUiParent, false);
                    _counter++;
                }
            }
            else
            {
                Debug.LogError("Player does not have Inventory Player Parent transform attached");
            }
        }

        private void CheckHandsEmpty()
        {
            if (_heldItem == null && _listInventory.Count > 0)
            {
                StartCoroutine(PlaceItemInHands(_listInventory[_listInventory.Count - 1]));
            }
        }

        private void ClearHands()
        {
            _heldItem = null;
        }

        private void ClearInventoryUi()
        {
            if (inventoryUiParent != null)
            {
                foreach (Transform child in inventoryUiParent)
                {
                    Destroy(child.gameObject);
                }
            }
            else
            {
                Debug.LogError("Player does not have Inventory UI Parent Transform attached");
            }
        }

        private void DeactivateAllItems()
        {
            if (inventoryPlayerParent != null)
            {
                foreach (Transform child in inventoryPlayerParent)
                {
                    if (child.CompareTag("Item"))
                    {
                        child.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogError("Player does not have Inventory Player Parent Transform attached");
            }
        }

        private IEnumerator PlaceItemInHands(Transform item)
        {
            yield return new WaitForSeconds(EquipTime);
            _heldItem = item;
            _heldItem.gameObject.SetActive(true);
        }

        public void ActivateInventoryItem(int index)
        {
            DeactivateAllItems();
            StartCoroutine(PlaceItemInHands(_listInventory[index]));
        }
    }
}