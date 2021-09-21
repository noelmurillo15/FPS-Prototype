/*
 * A - ( makes for a cleaner code base )
 * Created By : Allan Murillo
 * Last Edited : 2/21/2020
 */

namespace ANM.Editor.TDD.Infrastructure
{
    public static class A
    {
        public static HeartBuilder Heart()
        {
            return new();
        }

        public static HeartContainerBuilder HeartContainer()
        {
            return new HeartContainerBuilder().With(Heart());
        }
    }
}
