using CommonClasses;
using CommonClasses.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static CommonClasses.Networking.QuestionPacket;

namespace CommonClasses_Test.Networking
{
    public class QuestionPacketTests
    {
        #region Test Group - GetTypeID
        [Fact]
        public void GetTypeID_CorrectTypeID()
        {
            // Arrange
            var expectedTypeID = 1;
            var instance = new QuestionPacket();

            // Act
            var typeID = instance.GetTypeID();

            // Assert
            Assert.Equal(expectedTypeID, typeID);
        }
        #endregion Test Group - GetTypeID

        #region Test Group - CalculateDataSize
        [Theory]
        [ClassData(typeof(QuestionPacketTestData.CalculateDataSize_MultipleInstances))]
        public void CalculateDataSize_MultipleInstances_CorrectSizes(
            double leftOp, double rightOp, MathOperator op)
        {
            // Arrange
            var expectedSize = sizeof(double) + sizeof(int) + sizeof(double);
            var instance = new QuestionPacket(leftOp, rightOp, op);

            // Act
            var size = instance.CalculateDataSize();

            // Assert
            Assert.Equal(expectedSize, size);
        }
        #endregion Test Group - CalculateDataSize

        #region Test Group - Serialize
        [Theory]
        [ClassData(typeof(QuestionPacketTestData.Serialize_MultipleInstances))]
        public void Serialize_MultipleInstances_CorrectBytes(
            double leftOp, double rightOp, MathOperator op, byte[] expectedBytes)
        {
            // Arrange
            var instance = new QuestionPacket(leftOp, rightOp, op);

            // Act
            var bytes = instance.Serialize();

            // Assert
            Assert.Equal(expectedBytes, bytes);
        }
        #endregion Test Group - Serialize

        #region Test Group - Deserialize
        [Theory]
        [ClassData(typeof(QuestionPacketTestData.Deserialize_MultipleByteArrays))]
        public void Deserialize_MultipleByteArrays_CorrectInstances(
            byte[] bytes, double expectedLeftOp, double expectedRightOp, MathOperator expectedOp)
        {
            // Arrange
            var instance = new QuestionPacket();

            // Act
            instance.Deserialize(bytes);

            // Assert
            Assert.Equal(expectedLeftOp, instance.LeftOperand);
            Assert.Equal(expectedRightOp, instance.RightOperand);
            Assert.Equal(expectedOp, instance.Operator);
        }
        #endregion Test Group - Deserialize
    }
}
