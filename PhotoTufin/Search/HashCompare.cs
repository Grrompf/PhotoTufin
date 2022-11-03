using System.Collections.Generic;
using System.Linq;

namespace PhotoTufin.Search;

public class HashCompare
{
    private static bool isTuplet(IEnumerable<byte> myHash, IEnumerable<byte> mySecHash)
    {
        return myHash.SequenceEqual(mySecHash);
    }
}