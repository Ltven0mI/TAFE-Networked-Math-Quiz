using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using CommonClasses;
using CommonClasses.Utilities;

namespace CommonClasses_Test.Utilities
{
    public class BinarySerializeTests
    {
        #region Test Group - Double
        [Theory]
        [ClassData(typeof(BinarySerializeTestData.SerializeDouble_MultipleDoubles))]
        public void SerializeDouble_MultipleDoubles_ValidBytes(
            double value, byte[] expectedBytes)
        {
            // Arrange
            var expectedOffset = sizeof(double);

            var offset = 0;
            var bytes = new byte[sizeof(double)];

            // Act
            BinarySerialize.SerializeDouble(value, bytes, ref offset);

            // Assert
            Assert.Equal(expectedBytes, bytes);
            Assert.Equal(expectedOffset, offset);
        }

        [Theory]
        [ClassData(typeof(BinarySerializeTestData.DeserializeDouble_MultipleDoubles))]
        public void DeserializeDouble_MultipleByteArrays_ValidDoubles(
            byte[] bytes, double expectedValue)
        {
            // Arrange
            var expectedOffset = sizeof(double);
            var offset = 0;

            // Act
            var value = BinarySerialize.DeserializeDouble(bytes, ref offset);

            // Assert
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedOffset, offset);
        }
        #endregion Test Group - Double

        #region Test Group - Int
        [Theory]
        [ClassData(typeof(BinarySerializeTestData.SerializeInt_MultipleInts))]
        public void SerializeInt_MultipleInts_ValidBytes(
            int value, byte[] expectedBytes)
        {
            // Arrange
            var expectedOffset = sizeof(int);
            var offset = 0;

            var bytes = new byte[sizeof(int)];

            // Act
            BinarySerialize.SerializeInt(value, bytes, ref offset);

            // Assert
            Assert.Equal(expectedBytes, bytes);
            Assert.Equal(expectedOffset, offset);
        }

        [Theory]
        [ClassData(typeof(BinarySerializeTestData.DeserializeInt_MultipleInts))]
        public void DeserializeInt_MultipleByteArrays_ValidInts(
            byte[] bytes, int expectedValue)
        {
            // Arrange
            var expectedOffset = sizeof(int);
            var offset = 0;

            // Act
            var value = BinarySerialize.DeserializeInt(bytes, ref offset);

            // Assert
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedOffset, offset);
        }
        #endregion Test Group - Int

        #region Test Group - Bool
        [Theory]
        [ClassData(typeof(BinarySerializeTestData.SerializeBool_MultipleBools))]
        public void SerializeBool_MultipleBools_ValidBytes(
            bool value, byte[] expectedBytes)
        {
            // Arrange
            var expectedOffset = sizeof(bool);
            var offset = 0;

            var bytes = new byte[sizeof(bool)];

            // Act
            BinarySerialize.SerializeBool(value, bytes, ref offset);

            // Assert
            Assert.Equal(expectedBytes, bytes);
            Assert.Equal(expectedOffset, offset);
        }

        [Theory]
        [ClassData(typeof(BinarySerializeTestData.DeserializeBool_MultipleBools))]
        public void DeserializeBool_MultipleByteArray_ValidBools(
            byte[] bytes, bool expectedValue)
        {
            // Arrange
            var expectedOffset = sizeof(bool);
            var offset = 0;

            // Act
            var value = BinarySerialize.DeserializeBool(bytes, ref offset);

            // Assert
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedOffset, offset);
        }
        #endregion Test Group - Bool

        #region Test Group - Byte
        [Theory]
        [ClassData(typeof(BinarySerializeTestData.SerializeByte_MultipleBytes))]
        public void SerializeByte_MultipleBytes_ValidBytes(
            byte value, byte[] expectedBytes)
        {
            // Arrange
            var expectedOffset = sizeof(byte);
            var offset = 0;
            var bytes = new byte[sizeof(byte)];

            // Act
            BinarySerialize.SerializeByte(value, bytes, ref offset);

            // Assert
            Assert.Equal(expectedBytes, bytes);
            Assert.Equal(expectedOffset, offset);
        }

        [Theory]
        [ClassData(typeof(BinarySerializeTestData.DeserializeByte_MultipleBytes))]
        public void DeserializeByte_MultipleByteArrays_ValidBytes(
            byte[] bytes, byte expectedValue)
        {
            // Arrange
            var expectedOffset = sizeof(byte);
            var offset = 0;

            // Act
            var value = BinarySerialize.DeserializeByte(bytes, ref offset);

            // Assert
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedOffset, offset);
        }
        #endregion Test Group - Byte

        #region Test Group - ByteArray
        [Theory]
        [ClassData(typeof(BinarySerializeTestData.SerializeByteArray_MultipleByteArrays))]
        public void SerializeByteArray_MultipleByteArrays_ValidBytes(
            byte[] val, byte[] expectedBytes)
        {
            // Arrange
            var expectedOffset = sizeof(int) + (sizeof(byte) * val.Length);
            var offset = 0;
            var bytes = new byte[sizeof(int) + (sizeof(byte) * val.Length)];

            // Act
            BinarySerialize.SerializeByteArray(val, bytes, ref offset);

            // Assert
            Assert.Equal(expectedBytes, bytes);
            Assert.Equal(expectedOffset, offset);
        }

        [Theory]
        [ClassData(typeof(BinarySerializeTestData.DeserializeByteArray_MultipleByteArrays))]
        public void DeserializeByteArray_MultipleByteArrays_ValidByteArrays(
            byte[] bytes, byte[] expectedValue)
        {
            // Arrange
            var expectedOffset = sizeof(int) + (sizeof(byte) * expectedValue.Length);
            var offset = 0;

            // Act
            var value = BinarySerialize.DeserializeByteArray(bytes, ref offset);

            // Assert
            Assert.Equal(expectedValue, value);
            Assert.Equal(expectedOffset, offset);
        }
        #endregion Test Group - ByteArray
    }
}
