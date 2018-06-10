using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WarBender {
    public struct Range : IEnumerable<int> { 
        public int LowerBound;
        public int UpperBound;

        public Range(int lowerBound, int upperBound) {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public IEnumerator<int> GetEnumerator() =>
            Enumerable.Range(LowerBound, UpperBound - LowerBound + 1).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
