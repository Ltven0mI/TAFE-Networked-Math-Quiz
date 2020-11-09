using CommonClasses.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CommonClasses_Test.Networking
{
    public class ResultPacketTests
    {
        #region Test Group - GetTypeID
        [Fact]
        public void GetTypeID_CorrectTypeID()
        {
            // Arrange
            var expectedTypeID = 3;
            var instance = new ResultPacket();

            // Act
            var typeID = instance.GetTypeID();

            // Assert
            Assert.Equal(expectedTypeID, typeID);
        }
        #endregion Test Group - GetTypeID

        #region Test Group - CalculateDataSize
        [Theory]
        [ClassData(typeof(ResultPacketTestData.CalculateDataSize_MultipleInstances))]
        public void CalculateDataSize_MultipleInstances_CorrectSizes(
            bool wasCorrect)
        {
            // Arrange
            var expectedSize = sizeof(bool);
            var instance = new ResultPacket(wasCorrect);

            // Act
            var size = instance.CalculateDataSize();

            // Assert
            Assert.Equal(expectedSize, size);
        }
        #endregion Test Group - CalculateDataSize

        #region Test Group - Serialize
        [Theory]
        [ClassData(typeof(ResultPacketTestData.Serialize_MultipleInstances))]
        public void Serialize_MultipleInstances_CorrectBytes(
            bool wasCorrect, byte[] expectedBytes)
        {
            // Arrange
            var instance = new ResultPacket(wasCorrect);

            // Act
            var bytes = instance.Serialize();

            // Assert
            Assert.Equal(expectedBytes, bytes);
        }
        #endregion Test Group - Serialize

        #region Test Group - Deserialize
        [Theory]
        [ClassData(typeof(ResultPacketTestData.Deserialize_MultipleByteArrays))]
        public void Deserialize_MultipleByteArrays_CorrectInstances(
            byte[] bytes, bool expectedWasCorrect)
        {
            // Arrange
            var instance = new ResultPacket();

            // Act
            instance.Deserialize(bytes);

            // Assert
            Assert.Equal(expectedWasCorrect, instance.WasCorrect);
        }
        #endregion Test Group - Deserialize
    }
}
