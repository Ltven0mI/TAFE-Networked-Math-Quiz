using CommonClasses.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   ResultPacket.cs
    // Purpose:    Provides a serializable class for sending
    //             - a QnA result across a network.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-10-31
    // Tests:      Passed - 2020-10-31
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // 
    // [0.1.0] 2020-10-31
    // | [Added]
    // | - Initial Class Implementation.
    /**********************************************************/

    /// <summary>
    /// Provides a serializable class for sending a QnA result across a network.
    /// </summary>
    public class ResultPacket : SPacketBase
    {

        public bool WasCorrect { get; set; }


        public ResultPacket()
        {
            WasCorrect = default;
        }

        public ResultPacket(bool wasCorrect)
        {
            WasCorrect = wasCorrect;
        }


        #region SPacketBase Members

        private static readonly int TYPE_ID = SPacketRegister.GetTypeID(typeof(ResultPacket));
        private const int DATA_SIZE = sizeof(bool);

        public override int GetTypeID() => TYPE_ID;
        public override int CalculateDataSize() => DATA_SIZE;

        public override byte[] Serialize()
        {
            // Set-Up //
            var bytes = CreateDataArray();
            var offset = 0;

            // Serialization //
            SerializeHeader(bytes, ref offset);
            BinarySerialize.SerializeBool(WasCorrect, bytes, ref offset);

            // Return //
            return bytes;
        }

        public override void Deserialize(byte[] data)
        {
            // Set-Up //
            var offset = HEADER_SIZE;

            // Deserialization //
            var wasCorrect = BinarySerialize.DeserializeBool(data, ref offset);

            // Assignment //
            WasCorrect = wasCorrect;
        }

        #endregion SPacketBase Members
    }
}
