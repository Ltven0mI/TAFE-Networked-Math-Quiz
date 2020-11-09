using System;
using System.Collections.Generic;
using System.Text;

namespace CommonClasses.Utilities
{
    /**********************************************************/
    // Filename:   BinarySerialize.cs
    // Purpose:    Provides static methods for serializing /
    //             deserializing data types to byte arrays.
    // Author:     Wade Rauschenbach
    // Version:    0.2.0
    // Date:       2020-10-30
    // Tests:      Passed - 2020-10-30
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    //
    // [0.2.0] 2020-10-30
    // | [Added]
    // | - Add SerializeByte(byte, byte[], ref int).
    // | - Add Deserialize(byte[], ref int).
    // | - Add SerializeByteArray(byte[], byte[], ref int).
    // | - Add DeserializeByteArray(byte[], ref int).
    // 
    // [0.1.0] 2020-10-26
    // | [Added]
    // | - Add SerializeDouble(double, byte[], ref int).
    // | - Add DeserializeDouble(byte[], ref int).
    // | - Add SerializeInt(int, byte[], ref int).
    // | - Add DeserializeInt(byte[], ref int).
    // | - Add SerializeBool(bool, byte[], ref int).
    // | - Add DeserializeBool(byte[], ref int).
    /**********************************************************/

    /// <summary>
    /// Provides static methods for serilaizing / deserializing
    /// data types to byte arrays.
    /// </summary>
    public static class BinarySerialize
    {
        #region Serializer/Deserializer - Double

        /**********************************************************/
        // Method:  public static void SerializeDouble(double val, byte[] array, ref int offset)
        // Purpose: Serialize a double, place it into 'array' at
        //          'offset' and advance 'offset' by 'sizeof(double)'
        // Inputs:  double val, byte[] array, ref int offset
        /**********************************************************/
        /// <summary>
        /// Serialize a <c>double</c>, place it into <paramref name="array"/> at
        /// <paramref name="offset"/> and advance <paramref name="offset"/> by
        /// <c>sizeof(double)</c>
        /// </summary>
        /// <param name="val">The value to serialize.</param>
        /// <param name="array">The array to put the serialized value in.</param>
        /// <param name="offset">The position to start putting the bytes in the array.</param>
        public static void SerializeDouble(double val, byte[] array, ref int offset)
        {
            var bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;
        }

        /**********************************************************/
        // Method:  public static double DeserializeDouble(byte[] array, ref int offset)
        // Purpose: Deserializes a 'double' from 'array' at the
        //          index 'offset' and advances 'offset' by 'sizeof(double)'.
        // Returns: The deserialized double.
        // Inputs:  byte[] array, ref int offset
        // Outputs: double value
        /**********************************************************/
        /// <summary>
        /// Deserializes a <c>double</c> from <paramref name="array"/> at the index
        /// <paramref name="offset"/> and advances <paramref name="offset"/> by
        /// <c>sizeof(double)</c>.
        /// </summary>
        /// <param name="array">The array of bytes, containing the serialized value.</param>
        /// <param name="offset">The first index of the value.</param>
        /// <returns>The deserialized value.</returns>
        public static double DeserializeDouble(byte[] array, ref int offset)
        {
            var result = BitConverter.ToDouble(array, offset);
            offset += sizeof(double);
            return result;
        }

        #endregion Serializer/Deserializer - Double

        #region Serializer/Deserializer - Int

        /**********************************************************/
        // Method:  public static void SerializeInt(int val, byte[] array, ref int offset)
        // Purpose: Serialize an int, place it into 'array' at
        //          'offset' and advance 'offset' by 'sizeof(int)'
        // Inputs:  int val, byte[] array, ref int offset
        /**********************************************************/
        /// <summary>
        /// Serialize an <c>int</c>, place it into <paramref name="array"/> at
        /// <paramref name="offset"/> and advance <paramref name="offset"/> by
        /// <c>sizeof(int)</c>
        /// </summary>
        /// <param name="val">The value to serialize.</param>
        /// <param name="array">The array to put the serialized value in.</param>
        /// <param name="offset">The position to start putting the bytes in the array.</param>
        public static void SerializeInt(int val, byte[] array, ref int offset)
        {
            var bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;
        }

        /**********************************************************/
        // Method:  public static int DeserializeInt(byte[] array, ref int offset)
        // Purpose: Deserializes an 'int' from 'array' at the
        //          index 'offset' and advances 'offset' by 'sizeof(int)'.
        // Returns: The deserialized int.
        // Inputs:  byte[] array, ref int offset
        // Outputs: int value
        /**********************************************************/
        /// <summary>
        /// Deserializes an <c>int</c> from <paramref name="array"/> at the index
        /// <paramref name="offset"/> and advances <paramref name="offset"/> by
        /// <c>sizeof(int)</c>.
        /// </summary>
        /// <param name="array">The array of bytes, containing the serialized value.</param>
        /// <param name="offset">The first index of the value.</param>
        /// <returns>The deserialized value.</returns>
        public static int DeserializeInt(byte[] array, ref int offset)
        {
            var result = BitConverter.ToInt32(array, offset);
            offset += sizeof(int);
            return result;
        }

        #endregion Serializer/Deserializer - Int

        #region Serializer/Deserializer - Bool

        /**********************************************************/
        // Method:  public static void SerializeBool(bool val, byte[] array, ref int offset)
        // Purpose: Serialize a bool, place it into 'array' at
        //          'offset' and advance 'offset' by 'sizeof(bool)'
        // Inputs:  bool val, byte[] array, ref int offset
        /**********************************************************/
        /// <summary>
        /// Serialize a <c>bool</c>, place it into <paramref name="array"/> at
        /// <paramref name="offset"/> and advance <paramref name="offset"/> by
        /// <c>sizeof(bool)</c>
        /// </summary>
        /// <param name="val">The value to serialize.</param>
        /// <param name="array">The array to put the serialized value in.</param>
        /// <param name="offset">The position to start putting the bytes in the array.</param>
        public static void SerializeBool(bool val, byte[] array, ref int offset)
        {
            var bytes = BitConverter.GetBytes(val);
            Array.Copy(bytes, 0, array, offset, bytes.Length);
            offset += bytes.Length;
        }

        /**********************************************************/
        // Method:  public static bool DeserializeBool(byte[] array, ref int offset)
        // Purpose: Deserializes a 'bool' from 'array' at the
        //          index 'offset' and advances 'offset' by 'sizeof(bool)'.
        // Returns: The deserialized bool.
        // Inputs:  byte[] array, ref int offset
        // Outputs: bool value
        /**********************************************************/
        /// <summary>
        /// Deserializes a <c>bool</c> from <paramref name="array"/> at the index
        /// <paramref name="offset"/> and advances <paramref name="offset"/> by
        /// <c>sizeof(bool)</c>.
        /// </summary>
        /// <param name="array">The array of bytes, containing the serialized value.</param>
        /// <param name="offset">The first index of the value.</param>
        /// <returns>The deserialized value.</returns>
        public static bool DeserializeBool(byte[] array, ref int offset)
        {
            var result = BitConverter.ToBoolean(array, offset);
            offset += sizeof(bool);
            return result;
        }

        #endregion Serializer/Deserializer - Bool

        #region Serializer/Deserializer - Byte

        /**********************************************************/
        // Method:  public static void SerializeByte(byte val, byte[] array, ref int offset)
        // Purpose: Serialize a byte, place it into 'array' at
        //          'offset' and advance 'offset' by 'sizeof(byte)'
        // Inputs:  byte val, byte[] array, ref int offset
        /**********************************************************/
        /// <summary>
        /// Serialize a <c>byte</c>, place it into <paramref name="array"/> at
        /// <paramref name="offset"/> and advance <paramref name="offset"/> by
        /// <c>sizeof(byte)</c>
        /// </summary>
        /// <param name="val">The value to serialize.</param>
        /// <param name="array">The array to put the serialized value in.</param>
        /// <param name="offset">The position to start putting the bytes in the array.</param>
        public static void SerializeByte(byte val, byte[] array, ref int offset)
        {
            array[offset] = val;
            offset += sizeof(byte);
        }

        /**********************************************************/
        // Method:  public static byte DeserializeByte(byte[] array, ref int offset)
        // Purpose: Deserializes a 'byte' from 'array' at the
        //          index 'offset' and advances 'offset' by 'sizeof(byte)'.
        // Returns: The deserialized byte.
        // Inputs:  byte[] array, ref int offset
        // Outputs: byte value
        /**********************************************************/
        /// <summary>
        /// Deserializes a <c>byte</c> from <paramref name="array"/> at the index
        /// <paramref name="offset"/> and advances <paramref name="offset"/> by
        /// <c>sizeof(byte)</c>.
        /// </summary>
        /// <param name="array">The array of bytes, containing the serialized value.</param>
        /// <param name="offset">The first index of the value.</param>
        /// <returns>The deserialized value.</returns>
        public static byte DeserializeByte(byte[] array, ref int offset)
        {
            var result = array[offset];
            offset += sizeof(bool);
            return result;
        }

        #endregion Serializer/Deserializer - Byte

        #region Serializer/Deserializer - Byte[]

        /**********************************************************/
        // Method:  public static void SerializeByteArray(byte[] val, byte[] array, ref int offset)
        // Purpose: Serialize a byte[], place it into 'array' at
        //          'offset' and advance 'offset' by 'sizeof(int)'
        //          and 'val.Length'.
        // Inputs:  byte[] val, byte[] array, ref int offset
        /**********************************************************/
        /// <summary>
        /// Serialize a <c>byte[]</c>, place it into <paramref name="array"/> at
        /// <paramref name="offset"/> and advance <paramref name="offset"/> by
        /// <c>sizeof(int)</c> and <c>val.Length</c>.
        /// </summary>
        /// <param name="val">The value to serialize.</param>
        /// <param name="array">The array to put the serialized value in.</param>
        /// <param name="offset">The position to start putting the bytes in the array.</param>
        public static void SerializeByteArray(byte[] val, byte[] array, ref int offset)
        {
            SerializeInt(val.Length, array, ref offset);
            Array.Copy(val, 0, array, offset, val.Length);
            offset += val.Length;
        }

        /**********************************************************/
        // Method:  public static byte[] DeserializeByteArray(byte[] array, ref int offset)
        // Purpose: Deserializes a 'byte[]' from 'array' at the
        //          index 'offset' and advances 'offset' by 'sizeof(int)'
        //          and the length of the deserialized array.
        // Returns: The deserialized byte[].
        // Inputs:  byte[] array, ref int offset
        // Outputs: byte[] value
        /**********************************************************/
        /// <summary>
        /// Deserializes a <c>byte[]</c> from <paramref name="array"/> at the index
        /// <paramref name="offset"/> and advances <paramref name="offset"/> by
        /// <c>sizeof(int)</c> and the length of the deserialized array.
        /// </summary>
        /// <param name="array">The array of bytes, containing the serialized value.</param>
        /// <param name="offset">The first index of the value.</param>
        /// <returns>The deserialized value.</returns>
        public static byte[] DeserializeByteArray(byte[] array, ref int offset)
        {
            var length = DeserializeInt(array, ref offset);
            var result = new byte[length];
            Array.Copy(array, offset, result, 0, length);
            offset += length;
            return result;
        }

        #endregion Serializer/Deserializer - Byte[]
    }
}
