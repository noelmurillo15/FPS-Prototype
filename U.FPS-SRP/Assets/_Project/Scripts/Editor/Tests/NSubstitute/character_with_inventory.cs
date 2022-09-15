/*
 * character_with_inventory test -
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
    public class character_with_inventory
    {
        [Test]
        public void with_90_armor_takes_10_percent_dmg()
        {
            //  Arrange
            var character = Substitute.For<ITestCharacter>();
            var inventory = new TestInventory(character);
            var legItem = new TestItem() { EquipSlot = EquipSlots.Legs, Armor = 30 };
            var rArmItem = new TestItem() { EquipSlot = EquipSlots.RightHand, Armor = 10 };
            var lArmItem = new TestItem() { EquipSlot = EquipSlots.LeftHand, Armor = 10 };
            var chestItem = new TestItem() { EquipSlot = EquipSlots.Chest, Armor = 40 };

            inventory.EquipItem(legItem);
            inventory.EquipItem(rArmItem);
            inventory.EquipItem(lArmItem);
            inventory.EquipItem(chestItem);

            //  Used to access inventory from interface
            character.Inventory.Returns(inventory);

            //  Act
            var calcDmg = DamageCalculator.CalculateDamage(1000, character);

            //  Assert
            Assert.AreEqual(100, calcDmg);
        }
    }
}
#endif