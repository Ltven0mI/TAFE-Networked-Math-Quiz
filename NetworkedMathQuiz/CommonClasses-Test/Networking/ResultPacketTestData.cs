using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses_Test.Networking
{
    public static class ResultPacketTestData
    {
        #region Data Group - CalculateDataSize
        public class CalculateDataSize_MultipleInstances : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { true };
                yield return new object[]
                    { false };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - CalculateDataSize

        #region Data Group - Serialize
        public class Serialize_MultipleInstances : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { true, new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x03, 0x00, 0x00, 0x00, // Header TypeID
                            0x01                    // WasCorrect
                        }
                    };
                yield return new object[]
                    { false, new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x03, 0x00, 0x00, 0x00, // Header TypeID
                            0x00                    // WasCorrect
                        }
                    };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Serialize

        #region Data Group - Deserialize
        public class Deserialize_MultipleByteArrays : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x03, 0x00, 0x00, 0x00, // Header TypeID
                            0x01                    // WasCorrect
                        }, true
                    };
                yield return new object[]
                    { new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x03, 0x00, 0x00, 0x00, // Header TypeID
                            0x00                    // WasCorrect
                        }, false
                    };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Deserialize
    }
}
