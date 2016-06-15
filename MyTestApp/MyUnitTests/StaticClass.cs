using System.Diagnostics;

namespace MyUnitTests
{
    static class StaticClass
    {
        static StaticClass()
        {
            Debug.WriteLine("StaticClass static ctor!");
        }
    }

    public class SemiStaticClass
    {
        public SemiStaticClass()
        {
            Debug.WriteLine("2? SemiStaticClass simple public ctor!");
        }

        static SemiStaticClass()
        {
            Debug.WriteLine("1? SemiStaticClass static ctor!");
        }
    }
}
