/*
 * ITestCharacter -
 * Created By : Allan Murillo
 * Last Edited : 3/1/2020
 */

namespace ANM.UnitTests
{
    public interface ITestCharacter
    {
        TestInventory Inventory { get; }
        int Health { get; }
        int Level { get; }

        void OnItemEquipped(TestItem item);
    }
}
