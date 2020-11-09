using CommonClasses.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CommonClasses_Test.Networking
{
    public class AnswerPacketTests
    {
        #region Test Group - GetTypeID
        [Fact]
        public void GetTypeID_CorrectTypeID()
        {
            // Arrange
            var expectedTypeID = 2;
            var instance = new AnswerPacket();

            // Act
            var typeID = instance.GetTypeID();

            // Assert
            Assert.Equal(expectedTypeID, typeID);
        }
        #endregion Test Group - GetTypeID

        #region Test Group - CalculateDataSize
        [Theory]
        [ClassData(typeof(AnswerPacketTestData.CalculateDataSize_MultipleInstances))]
        public void CalculateDataSize_MultipleInstances_CorrectSizes(
            double answer)
        {
            // Arrange
            var expectedSize = sizeof(double);
            var instance = new AnswerPacket(answer);

            // Act
            var size = instance.CalculateDataSize();

            // Assert
            Assert.Equal(expectedSize, size);
        }
        #endregion Test Group - CalculateDataSize

        #region Test Group - Serialize
        [Theory]
        [ClassData(typeof(AnswerPacketTestData.Serialize_MultipleInstances))]
        public void Serialize_MultipleInstances_CorrectBytes(
            double answer, byte[] expectedBytes)
        {
            // Arrange
            var instance = new AnswerPacket(answer);

            // Act
            var bytes = instance.Serialize();

            // Assert
            Assert.Equal(expectedBytes, bytes);
        }
        #endregion Test Group - Serialize

        #region Test Group - Deserialize
        [Theory]
        [ClassData(typeof(AnswerPacketTestData.Deserialize_MultipleByteArrays))]
        public void Deserialize_MultipleByteArrays_CorrectInstances(
            byte[] bytes, double expectedAnswer)
        {
            // Arrange
            var instance = new AnswerPacket();

            // Act
            instance.Deserialize(bytes);

            // Assert
            Assert.Equal(expectedAnswer, instance.Answer);
        }
        #endregion Test Group - Deserialize
    }
}
