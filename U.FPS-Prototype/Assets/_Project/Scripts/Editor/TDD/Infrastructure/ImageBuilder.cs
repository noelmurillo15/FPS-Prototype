/*
 * ImageBuilder -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

using UnityEngine;
using UnityEngine.UI;

namespace ANM.Editor.TDD.Infrastructure
{
    public class ImageBuilder : TestDataBuilder<Image>
    {
        private float _fillAmount;

        
        public ImageBuilder() : this(0) { }

        public ImageBuilder(float amount)
        {
            _fillAmount = amount;
        }

        public ImageBuilder WithFillAmount(float amount)
        {
            _fillAmount = amount;
            return this;
        }

        public override Image Build()
        {
            var image = new GameObject().AddComponent<Image>();
            image.fillAmount = _fillAmount;
            return image;
        }
    }
}
