using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   ISPacket.cs
    // Purpose:    Represents an interface for a serializable
    //             - structured packet.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-11-02
    // Tests:      N/A
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // 
    // [0.1.0] 2020-11-02
    // | [Added]
    // | - Initial Interface Implementation.
    /**********************************************************/

    /// <summary>
    /// Represents an interface for a serializable structured packet.
    /// </summary>
    public interface ISPacket
    {
        /**********************************************************/
        // Method:  public int GetTypeID()
        // Purpose: When implemented in a concrete class, it should
        //          - get the TypeID the implementing class is
        //          - registered with in SPacketRegister.
        // Returns: The TypeID this class is registered with.
        // Outputs: int typeID
        /**********************************************************/
        /// <summary>
        /// When implemented in a concrete class, it should get
        /// the TypeID the implementing class is registered with
        /// in <see cref="SPacketRegister"/>.
        /// </summary>
        /// <returns>The TypeID this class is registered with.</returns>
        public int GetTypeID();

        /**********************************************************/
        // Method:  public int CalculateDataSize()
        // Purpose: When implemented in a concrete class, it should
        //          - calculate the number of bytes required to store
        //          - this instance's data when serialized.
        // Returns: The number of bytes to store this instance's
        //          - serialized data.
        // Outputs: int dataSize
        /**********************************************************/
        /// <summary>
        /// When implemented in a concrete class, it should calculate
        /// the number of bytes required to store this instance's
        /// data when serialized.
        /// </summary>
        /// <returns>The number of bytes to store this instance's serialized data.</returns>
        public int CalculateDataSize();

        /**********************************************************/
        // Method:  public byte[] Serialize()
        // Purpose: When implemented in a concrete class, it should
        //          - serialize this instance's data into an
        //          - array of bytes.
        // Returns: This instance's data serialized into a byte[].
        // Outputs: byte[] serializedData
        /**********************************************************/
        /// <summary>
        /// When implemented in a concrete class, it should serialize
        /// this instance's data into an array of bytes.
        /// </summary>
        /// <returns>This instance's data serialized into a <c>byte[]</c>.</returns>
        public byte[] Serialize();

        /**********************************************************/
        // Method:  public void Deserialize(byte[] data)
        // Purpose: When implemented in a concrete class, it should
        //          - deserialize the data and populate this instance.
        // Inputs:  byte[] data
        /**********************************************************/
        /// <summary>
        /// When implemented in a concrete class, it should deserialize
        /// the data and populate this instance.
        /// </summary>
        /// <param name="data">The data to deserialize.</param>
        public void Deserialize(byte[] data);
    }
}
