/*
 * HeartContainerTests -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using UnityEngine.UI;
using ANM.FPS.Player;
using NUnit.Framework;
using ANM.Editor.TDD.Infrastructure;

namespace ANM.Editor.TDD
{
    public class HeartContainerTests
    {
        public class TheReplenishMethod
        {
            private Image _target;

            
            [SetUp]
            public void BeforeEveryTest()
            {
                _target = An.Image();
            }

            [Test]
            public void _0_Replenish_From_0_Percent_To_0_Percent()
            {
                ((HeartContainer) A.HeartContainer().With(
                    A.Heart().With(_target))).Replenish(0);

                Assert.AreEqual(0, _target.fillAmount);
            }

            [Test]
            public void _1_Replenish_From_0_Percent_To_25_Percent()
            {
                ((HeartContainer) A.HeartContainer().With(
                    A.Heart().With(_target))).Replenish(1);

                Assert.AreEqual(0.25f, _target.fillAmount);
            }

            [Test]
            public void _Empty_Hearts_Are_Replenished()
            {
                _target.fillAmount = 0;
                ((HeartContainer) A.HeartContainer()
                    .With(
                        A.Heart().With(An.Image().WithFillAmount(1)),
                        A.Heart().With(_target))).Replenish(1);

                Assert.AreEqual(0.25f, _target.fillAmount);
            }

            [Test]
            public void _Hearts_Are_Replenished_In_Order()
            {
                ((HeartContainer) A.HeartContainer()
                    .With(
                        A.Heart(), A.Heart().With(_target))).Replenish(1);

                Assert.AreEqual(0, _target.fillAmount);
            }

            [Test]
            public void _Distributes_Heart_Pieces_Across_Multiple_Unfilled_Hearts()
            {
                ((HeartContainer) A.HeartContainer()
                        .With(
                            A.Heart().With(An.Image().WithFillAmount(0.75f)),
                            A.Heart().With(_target)))
                    .Replenish(2);

                Assert.AreEqual(0.25f, _target.fillAmount);
            }
        }

        public class TheDepleteMethod
        {
            private Image _target;

            [SetUp]
            public void BeforeEveryTest()
            {
                _target = An.Image().WithFillAmount(1);
            }

            [Test]
            public void _0_Deplete_Of_0_Does_Nothing()
            {
                ((HeartContainer) A.HeartContainer().With(
                    A.Heart().With(_target))).Deplete(0);

                Assert.AreEqual(1, _target.fillAmount);
            }

            [Test]
            public void _1_Deplete_Of_1_Removes_Single_Heart_Piece()
            {
                ((HeartContainer) A.HeartContainer().With(
                    A.Heart().With(_target))).Deplete(1);

                Assert.AreEqual(0.75f, _target.fillAmount);
            }

            [Test]
            public void _2_Deplete_Of_1_Removes_Single_Heart_Piece()
            {
                ((HeartContainer) A.HeartContainer().With(
                        A.Heart().With(_target),
                        A.Heart().With(An.Image().WithFillAmount(0.25f))))
                    .Deplete(2);

                Assert.AreEqual(0.75f, _target.fillAmount);
            }
        }
    }
}
