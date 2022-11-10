/*
 * HeartBuilder -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using ANM.FPS.Player;
using UnityEngine.UI;

namespace ANM.Editor.TDD.Infrastructure
{
    public class HeartBuilder : TestDataBuilder<Heart>
    {
        private Image _image;


        public HeartBuilder() : this(An.Image()) { }

        public HeartBuilder(Image image)
        {
            _image = image;
        }

        public HeartBuilder With(Image image)
        {
            _image = image;
            return this;
        }

        public override Heart Build()
        {
            return new(_image);
        }
    }
}
