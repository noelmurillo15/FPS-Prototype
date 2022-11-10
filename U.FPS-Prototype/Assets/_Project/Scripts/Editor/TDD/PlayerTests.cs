/*
 * PlayerTests -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using System;
using NUnit.Framework;

namespace ANM.Editor.TDD
{
    public class PlayerTests
    {
        public class TheCurrentHealthProperty
        {
            [Test]
            public void _0_Health_Defaults_to_0()
            {
                var player = new FPS.Player.Player(0);
                Assert.AreEqual(0, player.CurrentHealth);
            }

            [Test]
            public void _1_Throw_Exception_When_CurrentHealth_is_less_than_0()
            {
                Assert.Throws<ArgumentOutOfRangeException>(()
                    => new FPS.Player.Player(-1));
            }

            [Test]
            public void _2_Throw_Exception_When_CurrentHealth_is_greater_than_max()
            {
                Assert.Throws<ArgumentOutOfRangeException>(()
                    => new FPS.Player.Player(2, 1));
            }
        }

        public class TheHealthMethod
        {
            [Test]
            public void _0_Does_Nothing()
            {
                var player = new FPS.Player.Player(0);
                player.Heal(0);
                Assert.AreEqual(0, player.CurrentHealth);
            }

            [Test]
            public void _1_Increment_Current_Health()
            {
                var player = new FPS.Player.Player(0);
                player.Heal(1);
                Assert.AreEqual(1, player.CurrentHealth);
            }

            [Test]
            public void _1_OverHeal_Is_Ignored()
            {
                var player = new FPS.Player.Player(0, 1);
                player.Heal(2);
                Assert.AreEqual(1, player.CurrentHealth);
            }
        }

        public class TheDamageMethod
        {
            [Test]
            public void _0_Does_Nothing()
            {
                var player = new FPS.Player.Player(1);
                player.Damage(0);
                Assert.AreEqual(1, player.CurrentHealth);
            }

            [Test]
            public void _1_Decrements_Current_Health()
            {
                var player = new FPS.Player.Player(12);
                player.Damage(1);
                Assert.AreEqual(11, player.CurrentHealth);
            }

            [Test]
            public void _1_Negative_Is_Ignored()
            {
                var player = new FPS.Player.Player(0, 1);
                player.Damage(1);
                Assert.AreEqual(0, player.CurrentHealth);
            }
        }

        public class TheDamagedEvent
        {
            [Test]
            public void Raises_Event_On_Damage()
            {
                var amount = -1;
                var player = new FPS.Player.Player(1);

                player.DamagedEvent += (sender, args) => { amount = args.Amount; };
                player.Damage(0);

                Assert.AreEqual(0, amount);
            }

            [Test]
            public void OverKill_Is_Ignored()
            {
                var amount = -1;
                var player = new FPS.Player.Player(0);

                player.DamagedEvent += (sender, args) => { amount = args.Amount; };
                player.Damage(1);

                Assert.AreEqual(0, amount);
            }
        }

        public class TheHealedEvent
        {
            [Test]
            public void Raises_Event_On_Heal()
            {
                var amount = -1;
                var player = new FPS.Player.Player(1);

                player.HealedEvent += (sender, args) => { amount = args.Amount; };
                player.Heal(0);

                Assert.AreEqual(0, amount);
            }

            [Test]
            public void OverHealing_Is_Ignored()
            {
                var amount = -1;
                var player = new FPS.Player.Player(1, 1);

                player.HealedEvent += (sender, args) => { amount = args.Amount; };
                player.Heal(1);

                Assert.AreEqual(0, amount);
            }
        }
    }
}
