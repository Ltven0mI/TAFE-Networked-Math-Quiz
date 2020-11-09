using CommonClasses.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   SPacketBase.cs
    // Purpose:    Provides an abstract implementation of the
    //             ISPacket interface, with multiple helper methods.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-10-28
    // Tests:      N/A
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // | [Changed]
    // | - Change HEADER_SIZE access from protected to public.
    // 
    // [0.1.0] 2020-10-28
    // | [Added]
    // | - Initial class implementation.
    /**********************************************************/

    /// <summary>
    /// Provides an abstract implementation of the <see cref="ISPacket"/> interface,
    /// with additional helper methods.
    /// </summary>
    public abstract class SPacketBase : ISPacket
    {
        #region Header Members

        private static readonly byte[] HEADER_TOKEN = new byte[] { 0x53, 0x50, 0x4B };

        // Version 0.1.0
        private const byte HEADER_VERSION_MAJOR = 0x00;
        private const byte HEADER_VERSION_MINOR = 0x01;
        private const byte HEADER_VERSION_PATCH = 0x00;

        public static readonly int HEADER_SIZE =
            (sizeof(byte) * HEADER_TOKEN.Length) +      // Token
            (sizeof(byte) * 3) +                        // Version
            sizeof(int);                                // Type ID

        #endregion Header

        public abstract int GetTypeID();
        public abstract int CalculateDataSize();
        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] data);


        #region Helper Methods

        /**********************************************************/
        // Method:  protected byte[] CreateDataArray()
        // Purpose: Creates a new 'byte[]' large enough to store
        //          the packet's Header and Data content.
        // Returns: The new byte[].
        // Outputs: byte[] bytes
        /**********************************************************/
        /// <summary>
        /// Creates a new <c>byte[]</c> large enough to store the
        /// packet's Header and Data content.
        /// </summary>
        /// <returns>The new <c>byte[]</c>.</returns>
        protected byte[] CreateDataArray() => new byte[HEADER_SIZE + CalculateDataSize()];

        /**********************************************************/
        // Method:  protected void SerializeHeader(byte[] array, ref int offset)
        // Purpose: Serializes the packet's Header content to 'array'
        //          and advances 'offset' accordingly.
        // Inputs:  byte[] array, ref int offset
        /**********************************************************/
        /// <summary>
        /// Serializes the packet's Header content to <paramref name="array"/>
        /// and advances <paramref name="offset"/> accordingly.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="offset">The current serialization position in the array.</param>
        protected void SerializeHeader(byte[] array, ref int offset)
        {
            // Token //
            foreach (var b in HEADER_TOKEN)
                BinarySerialize.SerializeByte(b, array, ref offset);

            // Version //
            BinarySerialize.SerializeByte(HEADER_VERSION_MAJOR, array, ref offset);
            BinarySerialize.SerializeByte(HEADER_VERSION_MINOR, array, ref offset);
            BinarySerialize.SerializeByte(HEADER_VERSION_PATCH, array, ref offset);

            // Type ID //
            BinarySerialize.SerializeInt(GetTypeID(), array, ref offset);
        }

        #endregion Helper Methods

        #region Validation Methods

        /**********************************************************/
        // Method:  private static bool ValidateToken(byte[] array, ref int offset, out byte[] token)
        // Purpose: Validates whether 'array' contains a valid
        //          Header Token, then advances 'offset'.
        // Returns: true if a valid Header Token is found, otherwise false.
        // Inputs:  byte[] array, ref int offset, out byte[] token
        // Outputs: bool isValid
        /**********************************************************/
        /// <summary>
        /// Validates whether <paramref name="array"/> contains a valid
        /// Header Token, then advances <paramref name="offset"/>.
        /// </summary>
        /// <param name="array">An array containing the serialized packet.</param>
        /// <param name="offset">The current deserialization position in the array.</param>
        /// <param name="token">The token pulled from <paramref name="array"/>.</param>
        /// <returns><c>true</c> if a valid Header Token is found, otherwise <c>false</c>.</returns>
        private static bool ValidateToken(byte[] array, ref int offset, out byte[] token)
        {
            // Set-Up //
            var isMatching = true;
            token = new byte[HEADER_TOKEN.Length];

            // Deserialization //
            for (int i=0; i<HEADER_TOKEN.Length; i++)
            {
                token[i] = BinarySerialize.DeserializeByte(array, ref offset);
                if (token[i] != HEADER_TOKEN[i])
                    isMatching = false;
            }

            // Result //
            return isMatching;
        }

        /**********************************************************/
        // Method:  private static bool ValidateVersion(byte[] array, ref int offset, out byte major, out byte minor, out byte patch)
        // Purpose: Validates whether 'array' contains a valid
        //          and compatable Header Version.
        // Returns: true if a compatable version was found, otherwise false.
        // Inputs:  byte[] array, ref int offset, out byte major, out byte minor, out byte patch
        // Outputs: bool isCompatable
        /**********************************************************/
        /// <summary>
        /// Validates whether <paramref name="array"/> contains a valid and compatable Header Version.
        /// </summary>
        /// <param name="array">An array containing the serialized packet.</param>
        /// <param name="offset">The current deserialization position in the array.</param>
        /// <param name="major">The Major version number pulled from <paramref name="array"/>.</param>
        /// <param name="minor">The Minor version number pulled from <paramref name="array"/>.</param>
        /// <param name="patch">The Patch version number pulled from <paramref name="array"/>.</param>
        /// <returns><c>true</c> if a compatable version was found, otherwise <c>false</c>.</returns>
        private static bool ValidateVersion(byte[] array, ref int offset, out byte major, out byte minor, out byte patch)
        {
            // Deserialization //
            major = BinarySerialize.DeserializeByte(array, ref offset);
            minor = BinarySerialize.DeserializeByte(array, ref offset);
            patch = BinarySerialize.DeserializeByte(array, ref offset);

            // Comparisons //
            if (major != HEADER_VERSION_MAJOR)
                return false;
            if (minor > HEADER_VERSION_MINOR)
                return false;

            // Success //
            return true;
        }

        /**********************************************************/
        // Method:  private static bool ValidateTypeID(byte[] array, ref int offset, out int typeID, out Type type)
        // Purpose: Validates whether 'array' contains a valid
        //          SPacket TypeID.
        // Returns: true if a valid TypeID was found, otherwise false.
        // Inputs:  byte[] array, ref int offset, out int typeID, out Type type
        // Outputs: bool isValid
        /**********************************************************/
        /// <summary>
        /// Validates whether <paramref name="array"/> contains a valid SPacket TypeID.
        /// </summary>
        /// <param name="array">An array containing the serialized packet.</param>
        /// <param name="offset">The current deserialization position in the array.</param>
        /// <param name="typeID">The TypeID pulled from <paramref name="array"/>.</param>
        /// <param name="type">The <see cref="Type"/> registered in <see cref="SPacketRegister"/> with <paramref name="typeID"/>.</param>
        /// <returns><c>true</c> if a valid TypeID was found, otherwise <c>false</c>.</returns>
        private static bool ValidateTypeID(byte[] array, ref int offset, out int typeID, out Type type)
        {
            typeID = BinarySerialize.DeserializeInt(array, ref offset);
            type = SPacketRegister.GetRegisteredType(typeID);
            return (type != null);
        }

        /**********************************************************/
        // Method:  public static Type ValidateHeader(byte[] bytes)
        // Purpose: Deserializes and validates the SPacket Header
        //          from 'bytes', then returns the Type of SPacket.
        // Returns: The Type of the SPacket stored in 'bytes'.
        // Inputs:  byte[] bytes
        // Outputs: Type spacketType
        // Throws:  ArgumentNullException - when 'bytes' is null.
        //          ArgumentException - when 'bytes' is too small to contain a valid Header.
        //          InvalidHeaderException - when any of the following conditions apply,
        //          - The Header Token is invalid,
        //          - The Header Version is invalid or incompatable, or
        //          - The Header TypeID is invalid or unknown.
        /**********************************************************/
        /// <summary>
        /// Deserializes and validates the SPacket Header from <paramref name="bytes"/>,
        /// then returns the Type of the SPacket.
        /// </summary>
        /// <param name="bytes">The serialized SPacket</param>
        /// <returns>The <see cref="Type"/> of the SPacket stored in <paramref name="bytes"/>.</returns>
        /// <exception cref="ArgumentNullException">when <paramref name="bytes"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">when <paramref name="bytes"/> is too small to contain a valid Header.</exception>
        /// <exception cref="InvalidHeaderException">
        /// when any of the following conditions apply,<br/>
        /// - The Header Token is invalid,<br/>
        /// - The Header Version is invalid or incompatable, or<br/>
        /// - The Header TypeID is invalid or unknown.
        /// </exception>
        public static Type ValidateHeader(byte[] bytes)
        {
            // Argument Validation //
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length < HEADER_SIZE)
                throw new ArgumentException($"'bytes' was too small to contain a valid header: '{bytes.Length}'", nameof(bytes));

            var offset = 0;

            if (!ValidateToken(bytes, ref offset, out byte[] token))
                throw new InvalidHeaderException(
                    $"The header contained an invalid TOKEN: '{Encoding.UTF8.GetString(token)}'");

            if (!ValidateVersion(bytes, ref offset, out byte major, out byte minor, out byte patch))
                throw new InvalidHeaderException(
                    $"The header contained an incompatable VERSION: '{major}.{minor}.{patch}'");

            if (!ValidateTypeID(bytes, ref offset, out int typeID, out Type type))
                throw new InvalidHeaderException(
                    $"The header contained an unknown TYPE_ID: '{typeID}'");

            return type;
        }

        #endregion Validation Methods
    }
}
