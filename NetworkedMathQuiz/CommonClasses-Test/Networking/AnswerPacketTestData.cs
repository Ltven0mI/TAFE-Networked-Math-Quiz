using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses_Test.Networking
{
    public static class AnswerPacketTestData
    {
        #region Data Group - CalculateDataSize
        public class CalculateDataSize_MultipleInstances : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { 23.112016 };
                yield return new object[]
                    { 2016.1123 };
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
                    { 23.112016, new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x02, 0x00, 0x00, 0x00, // Header TypeID
                            0xF5, 0xA0, 0xA0, 0x14, // Answer 1/2
                            0xAD, 0x1C, 0x37, 0x40, // Answer 2/2
                        }
                    };
                yield return new object[]
                    { 2016.1123, new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x02, 0x00, 0x00, 0x00, // Header TypeID
                            0x5D, 0x6D, 0xC5, 0xFE, // Answer 1/2
                            0x72, 0x80, 0x9F, 0x40, // Answer 2/2
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
                            0x02, 0x00, 0x00, 0x00, // Header TypeID
                            0xF5, 0xA0, 0xA0, 0x14, // Answer 1/2
                            0xAD, 0x1C, 0x37, 0x40  // Answer 2/2
                        }, 23.112016
                    };
                yield return new object[]
                    { new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x02, 0x00, 0x00, 0x00, // Header TypeID
                            0x5D, 0x6D, 0xC5, 0xFE, // Answer 1/2
                            0x72, 0x80, 0x9F, 0x40  // Answer 2/2
                        }, 2016.1123
                    };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Deserialize
    }
}
