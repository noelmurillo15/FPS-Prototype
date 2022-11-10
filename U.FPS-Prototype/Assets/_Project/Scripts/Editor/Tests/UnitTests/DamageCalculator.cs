/*
 * DamageCalculator -
 * Created By : Allan Murillo
 * Last Edited : 3/1/2020
 */

using System;

namespace ANM.UnitTests
{
    public class DamageCalculator
    {
        public static int CalculateDamage(int amount, float mitigationPercent)
        {
            var multiplier = 1f - mitigationPercent;
            return Convert.ToInt32(amount * multiplier);
        }

        public static int CalculateDamage(int amount, ITestCharacter character)
        {
            var totalArmor = character.Inventory.GetTotalArmor() + (character.Level * 10);
            var multiplier = 100f - totalArmor;
            multiplier /= 100f;
            return Convert.ToInt32(amount * multiplier);
        }
    }
}
