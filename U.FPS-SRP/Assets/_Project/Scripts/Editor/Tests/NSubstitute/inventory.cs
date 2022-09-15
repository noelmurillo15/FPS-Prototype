/*
 * inventory test -
 * Created By : Allan Murillo
 * Last Edited : 3/1/2020
 */

#if UNITY_EDITOR
using NSubstitute;
using ANM.UnitTests;
using NUnit.Framework;

//  Plugin used to create an interface since we cannot use class = new class : Monobehaviour when doing Unit Tests

namespace ANM.Tests.NSubstitute
{
    public class inventory
    {
        [Test]
        public void only_allow_1_item_equipped_to_slot()
        {
            //  Arrange
            var character = Substitute.For<ITestCharacter>();
            var inventory = new TestInventory(character);
            var chest = new TestItem() { EquipSlot = EquipSlots.Chest };
            var chest2 = new TestItem() { EquipSlot = EquipSlots.Chest };

            //  Act
            inventory.EquipItem(chest);
            inventory.EquipItem(chest2);

            //  Assert
            var equippedItem = inventory.GetItem(EquipSlots.Chest);
            Assert.AreEqual(chest2, equippedItem);
        }

        [Test]
        public void tells_character_when_an_item_is_equipped()
        {
            //  Arrange
            var character = Substitute.For<ITestCharacter>();
            var inventory = new TestInventory(character);
            var legs = new TestItem() { EquipSlot = EquipSlots.Legs };

            //  Act
            inventory.EquipItem(legs);

            //  Assert
            character.Received().OnItemEquipped(legs);
        }
    }
}
#endif