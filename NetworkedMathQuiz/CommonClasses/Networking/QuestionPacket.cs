using CommonClasses.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using CommonClasses;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   QuestionPacket.cs
    // Purpose:    Provides a serializable class for sending
    //             - a math question across a network.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-10-30
    // Tests:      Passed - 2020-10-30
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // | [Removed]
    // | - Remove MathOperator enum. Use MathQuesiton.MathOperator instead.
    // 
    // [0.1.0] 2020-10-30
    // | [Added]
    // | - Initial Class Implementation.
    /**********************************************************/

    /// <summary>
    /// Provides a serializable class for sending a math question across a network.
    /// </summary>
    public class QuestionPacket : SPacketBase
    {
        public double LeftOperand { get; private set; }
        public double RightOperand { get; private set; }
        public MathOperator Operator { get; private set; }


        public QuestionPacket()
        {
            LeftOperand = default;
            RightOperand = default;
            Operator = default;
        }
        public QuestionPacket(double leftOperand, double rightOperand, MathOperator mathOperator)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Operator = mathOperator;
        }


        #region SPacketBase Members

        private static readonly int TYPE_ID = SPacketRegister.GetTypeID(typeof(QuestionPacket));
        private const int DATA_SIZE = sizeof(double) + sizeof(int) + sizeof(double);

        public override int GetTypeID() => TYPE_ID;
        public override int CalculateDataSize() => DATA_SIZE;

        public override byte[] Serialize()
        {
            // Set-Up //
            var bytes = CreateDataArray();
            var offset = 0;
            
            // Serialization //
            SerializeHeader(bytes, ref offset);
            BinarySerialize.SerializeDouble(LeftOperand, bytes, ref offset);
            BinarySerialize.SerializeDouble(RightOperand, bytes, ref offset);
            BinarySerialize.SerializeInt((int)Operator, bytes, ref offset);

            // Return //
            return bytes;
        }

        public override void Deserialize(byte[] data)
        {
            // Set-Up //
            var offset = HEADER_SIZE;

            // Deserialization //
            var leftOperand = BinarySerialize.DeserializeDouble(data, ref offset);
            var rightOperand = BinarySerialize.DeserializeDouble(data, ref offset);
            var mathOperator = (MathOperator)BinarySerialize.DeserializeInt(data, ref offset);

            // Assignment //
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Operator = mathOperator;
        }

        #endregion SPacketBase Members
    }
}
