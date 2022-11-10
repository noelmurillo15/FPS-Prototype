/*
 * HeartContainerBuilder -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using ANM.FPS.Player;
using System.Collections.Generic;

namespace ANM.Editor.TDD.Infrastructure
{
    public class HeartContainerBuilder : TestDataBuilder<HeartContainer>
    {
        private List<Heart> _hearts;

        
        public HeartContainerBuilder() : this(A.Heart()) { }

        public HeartContainerBuilder(Heart heart)
        {
            _hearts = new List<Heart> {heart};
        }

        public HeartContainerBuilder With(Heart heart)
        {
            _hearts = new List<Heart> {heart};
            return this;
        }

        public HeartContainerBuilder With(Heart heart, Heart heart2)
        {
            _hearts = new List<Heart> {heart, heart2};
            return this;
        }

        public override HeartContainer Build()
        {
            return new(_hearts);
        }
    }
}
