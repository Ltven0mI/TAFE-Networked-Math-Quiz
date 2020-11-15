using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CommonClasses;

namespace CommonClasses_Test.Networking
{
    public static class QuestionPacketTestData
    {
        #region Data Group - CalculateDataSize
        public class CalculateDataSize_MultipleInstances : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { 23.23, 11.11, MathOperator.Plus };
                yield return new object[]
                    { 23.11, 11.23, MathOperator.Plus };
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
                    { 23, 11, MathOperator.Plus, new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x01, 0x00, 0x00, 0x00, // Header TypeID
                            0x00, 0x00, 0x00, 0x00, // LeftOP 1/2
                            0x00, 0x00, 0x37, 0x40, // LeftOP 2/2
                            0x00, 0x00, 0x00, 0x00, // RightOP 1/2
                            0x00, 0x00, 0x26, 0x40, // RightOP 2/2
                            0x00, 0x00, 0x00, 0x00, // Operator
                        }
                    };
                yield return new object[]
                    { 23.11, 20.16, MathOperator.Times, new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x01, 0x00, 0x00, 0x00, // Header TypeID
                            0x5C, 0x8F, 0xC2, 0xF5, // LeftOP 1/2
                            0x28, 0x1C, 0x37, 0x40, // LeftOP 2/2
                            0x29, 0x5C, 0x8F, 0xC2, // RightOP 1/2
                            0xF5, 0x28, 0x34, 0x40, // RightOP 2/2
                            0x02, 0x00, 0x00, 0x00, // Operator
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
                            0x01, 0x00, 0x00, 0x00, // Header TypeID
                            0x00, 0x00, 0x00, 0x00, // LeftOP 1/2
                            0x00, 0x00, 0x37, 0x40, // LeftOP 2/2
                            0x00, 0x00, 0x00, 0x00, // RightOP 1/2
                            0x00, 0x00, 0x26, 0x40, // RightOP 2/2
                            0x00, 0x00, 0x00, 0x00, // Operator
                        },
                        23, 11, MathOperator.Plus
                    };
                yield return new object[]
                    { new byte[]
                        {
                            0x53, 0x50, 0x4B,       // Header Token
                            0x00, 0x01, 0x00,       // Header Version
                            0x01, 0x00, 0x00, 0x00, // Header TypeID
                            0x5C, 0x8F, 0xC2, 0xF5, // LeftOP 1/2
                            0x28, 0x1C, 0x37, 0x40, // LeftOP 2/2
                            0x29, 0x5C, 0x8F, 0xC2, // RightOP 1/2
                            0xF5, 0x28, 0x34, 0x40, // RightOP 2/2
                            0x02, 0x00, 0x00, 0x00, // Operator
                        },
                        23.11, 20.16, MathOperator.Times
                    };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Deserialize
    }
}
