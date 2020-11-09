using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses_Test.Utilities
{
    public static class BinarySerializeTestData
    {
        #region Data Group - Double
        public class SerializeDouble_MultipleDoubles : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { 23.11, new byte[] { 0x5C,0x8F,0xC2,0xF5, 0x28,0x1C,0x37,0x40 } };
                yield return new object[]
                    { 2016.1123, new byte[] { 0x5D,0x6D,0xC5,0xFE, 0x72,0x80,0x9F,0x40 } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class DeserializeDouble_MultipleDoubles : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[] { 0x5C,0x8F,0xC2,0xF5, 0x28,0x1C,0x37,0x40 }, 23.11 };
                yield return new object[]
                    { new byte[] { 0x5D,0x6D,0xC5,0xFE, 0x72,0x80,0x9F,0x40 }, 2016.1123 };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Double

        #region Data Group - Int
        public class SerializeInt_MultipleInts : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { 11, new byte[] { 0x0B, 0, 0, 0 } };
                yield return new object[]
                    { 23, new byte[] { 0x17, 0, 0, 0 } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class DeserializeInt_MultipleInts : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[] { 0x0B, 0, 0, 0 }, 11 };
                yield return new object[]
                    { new byte[] { 0x17, 0, 0, 0 }, 23 };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Int

        #region Data Group - Bool
        public class SerializeBool_MultipleBools : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { false, new byte[] { 0 } };
                yield return new object[]
                    { true, new byte[] { 1 } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class DeserializeBool_MultipleBools : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[] { 0 }, false };
                yield return new object[]
                    { new byte[] { 1 }, true };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Bool

        #region Data Group - Byte
        public class SerializeByte_MultipleBytes : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { 0x23, new byte[] { 0x23 } };
                yield return new object[]
                    { 0x11, new byte[] { 0x11 } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class DeserializeByte_MultipleBytes : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[] { 0x23 }, 0x23 };
                yield return new object[]
                    { new byte[] { 0x11 }, 0x11 };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - Byte

        #region Data Group - ByteArray
        public class SerializeByteArray_MultipleByteArrays : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[] { 0x23, 0x11, 0x20, 0x16 }, new byte[] { 0x04,0x00,0x00,0x00, 0x23, 0x11, 0x20, 0x16 } };
                yield return new object[]
                    { new byte[] { 0x16, 0x20, 0x11, 0x23 }, new byte[] { 0x04,0x00,0x00,0x00, 0x16, 0x20, 0x11, 0x23 } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class DeserializeByteArray_MultipleByteArrays : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                    { new byte[] { 0x04,0x00,0x00,0x00, 0x23, 0x11, 0x20, 0x16 }, new byte[] { 0x23, 0x11, 0x20, 0x16 } };
                yield return new object[]
                    { new byte[] { 0x04,0x00,0x00,0x00, 0x16, 0x20, 0x11, 0x23 }, new byte[] { 0x16, 0x20, 0x11, 0x23 } };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion Data Group - ByteArray
    }
}
