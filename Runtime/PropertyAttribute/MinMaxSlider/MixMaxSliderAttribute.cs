
namespace com.faith.core
{
    using UnityEngine;

    public class MixMaxSliderAttribute : PropertyAttribute
    {
        public float min;
        public float max;

        public MixMaxSliderAttribute(float min, float max) {

            this.min = min;
            this.max = max;
        }
    }
}


