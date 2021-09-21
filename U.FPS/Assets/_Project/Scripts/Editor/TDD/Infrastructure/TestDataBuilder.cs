/*
 * TestDataBuilder<T> -
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

namespace ANM.Editor.TDD.Infrastructure
{
    public abstract class TestDataBuilder<T>
    {
        public abstract T Build();

        //    Casts Builder to the generic type <T> - helps code look cleaner
        public static implicit operator T(TestDataBuilder<T> builder)
        {
            return builder.Build();
        }
    }
}
