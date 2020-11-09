using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonClasses.Networking
{
    /**********************************************************/
    // Filename:   SPacketRegister.cs
    // Purpose:    Provides a centralized collection of
    //             - manually curated SPacket classes.
    // Author:     Wade Rauschenbach
    // Version:    0.1.0
    // Date:       2020-10-31
    // Tests:      N/A
    /**********************************************************/

    /*********************** Changelog ************************/
    // [Unreleased]
    // 
    // [0.1.0] 2020-10-31
    // | [Added]
    // | - Initial Class Implementation.
    /**********************************************************/

    /// <summary>
    /// Provides a centralized collection of manually curated SPacket classes.
    /// </summary>
    public static class SPacketRegister
    {
        private static Dictionary<int, Type> _map = new Dictionary<int, Type>()
        {
            { 1, typeof(QuestionPacket) },
            { 2, typeof(AnswerPacket) },
            { 3, typeof(ResultPacket) }
        };

        public static int GetTypeID(Type type)
        {
            // State Validation //
            if (!_map.ContainsValue(type))
                throw new InvalidOperationException($"The Type '{type}' is not registered.");
            return _map.First(c => c.Value == type).Key;
        }

        public static Type GetRegisteredType(int id)
        {
            // State Validation //
            if (!_map.TryGetValue(id, out Type value))
                throw new InvalidOperationException($"No Type is registered with the ID: '{id}'.");
            return value;
        }
    }
}
