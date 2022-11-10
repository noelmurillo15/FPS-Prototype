/*
 * TestCharacter -
 * Created By : Allan Murillo
 * Last Edited : 3/1/2020
 */

using UnityEngine;

namespace ANM.UnitTests
{
    public class TestCharacter : MonoBehaviour, ITestCharacter
    {
        public TestCharacter(TestInventory inventory, int health, int level)
        {
            Inventory = inventory;
            Health = health;
            Level = level;
        }

        public TestInventory Inventory { get; }
        public int Health { get; }
        public int Level { get; }

        public void OnItemEquipped(TestItem item)
        {
            Debug.Log($"You Equipped the {item} in {item.EquipSlot}");
        }
    }
}
