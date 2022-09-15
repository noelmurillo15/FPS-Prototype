/*
 * damage_calculator test -
 * Created By : Allan Murillo
 * Last Edited : 3/1/2020
 */

#if UNITY_EDITOR
using ANM.UnitTests;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;

namespace ANM.Tests.NSubstitute
{
    public class damage_calculator
    {
        [Test]
        public void damage_with_half_mitigation()
        {
            //  Acting on method
            var finalDamage = DamageCalculator.CalculateDamage(10, 0.5f);

            //  Assert Result
            UnityEngine.Assertions.Assert.AreEqual(5, finalDamage);
        }

        [Test]
        public void damage_with_80_percent_mitigation()
        {
            //  Acting on method
            var finalDamage = DamageCalculator.CalculateDamage(10, 0.8f);

            //  Assert Result
            UnityEngine.Assertions.Assert.AreEqual(2, finalDamage);
        }

        [Test]
        public void damage_with_20_percent_mitigation()
        {
            //  Acting on method
            var finalDamage = DamageCalculator.CalculateDamage(10, 0.2f);

            //  Assert Result
            UnityEngine.Assertions.Assert.AreEqual(8, finalDamage);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator damage_calculatorWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
#endif