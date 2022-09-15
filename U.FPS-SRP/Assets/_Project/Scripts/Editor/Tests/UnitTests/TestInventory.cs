/*
 * TestInventory -
 * Created By : Allan Murillo
 * Last Edited : 3/1/2020
 */

using System.Linq;
using System.Collections.Generic;

namespace ANM.UnitTests
{
    public class TestInventory
    {
        private readonly Dictionary<EquipSlots, TestItem> _equippedItems = new();
        private readonly List<TestItem> _unEquippedItems = new();
        private readonly ITestCharacter _character;

        public TestInventory(ITestCharacter character)
        {
            _character = character;
        }

        public void EquipItem(TestItem item)
        {
            if (_equippedItems.ContainsKey(item.EquipSlot))
            {
                _unEquippedItems.Add(_equippedItems[item.EquipSlot]);
            }

            _equippedItems[item.EquipSlot] = item;

            _character.OnItemEquipped(item);
        }

        public TestItem GetItem(EquipSlots equipSlots)
        {
            return _equippedItems.ContainsKey(equipSlots) ? _equippedItems[equipSlots] : null;
        }

        public int GetTotalArmor()
        {
            var totalArmor = _equippedItems.Values.Sum(t => t.Armor);
            return totalArmor;
        }
    }
}
