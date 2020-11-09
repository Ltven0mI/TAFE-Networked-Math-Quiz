using CommonClasses.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   AnswerPacket.cs
    // Purpose:    Provides a serializable class for sending
    //             - a math answer across a network.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-10-31
    // Tests:      Passed = 2020-10-31
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // 
    // [0.1.0] 2020-10-31
    // | [Added]
    // | - Initial Class Implementation.
    /**********************************************************/

    /// <summary>
    /// Provides a serializable class for sending a math answer across a network.
    /// </summary>
    public class AnswerPacket : SPacketBase
    {
        public double Answer { get; set; }


        public AnswerPacket()
        {
            Answer = default;
        }
        public AnswerPacket(double answer)
        {
            Answer = answer;
        }


        #region SPacketBase Members

        private static readonly int TYPE_ID = SPacketRegister.GetTypeID(typeof(AnswerPacket));
        private const int DATA_SIZE = sizeof(double);

        public override int GetTypeID() => TYPE_ID;
        public override int CalculateDataSize() => DATA_SIZE;

        public override byte[] Serialize()
        {
            // Set-Up //
            var bytes = CreateDataArray();
            var offset = 0;

            // Serialization //
            SerializeHeader(bytes, ref offset);
            BinarySerialize.SerializeDouble(Answer, bytes, ref offset);

            // Return //
            return bytes;
        }

        public override void Deserialize(byte[] data)
        {
            // Set-Up //
            var offset = HEADER_SIZE;

            // Deserialization //
            var answer = BinarySerialize.DeserializeDouble(data, ref offset);

            // Assign
            Answer = answer;
        }

        #endregion SPacketBase Members
    }
}
