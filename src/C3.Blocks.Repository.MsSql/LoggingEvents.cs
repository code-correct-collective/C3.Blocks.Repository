namespace C3.Blocks.Repository.MsSql
{
    /// <summary>
    /// Contains logging event IDs for various operations.
    /// </summary>
    public sealed class LoggingEvents
    {
        // Error Code layout [Success Codes][Facility][Code]
        // Success Codes
        // 0x2 = Success
        // 0x6 = Information
        // 0xA = Warning
        // 0xE = Error
        // Facilities
        // 0x001 Database Operations


        /// <summary>
        /// Event ID for tracing a method.
        /// </summary>
        public const int TraceMethod = unchecked((int)0x60010001);

        /// <summary>
        /// Event ID for debugging a method.
        /// </summary>
        public const int DebugMethod = unchecked((int)0x60010002);

        /// <summary>
        /// Event ID for an error in a method.
        /// </summary>
        public const int ErrorMethod = unchecked((int)0x60010003);

        /// <summary>
        /// Event ID for finding an entity.
        /// </summary>
        public const int FindEntity = unchecked((int)0x60010004);

        /// <summary>
        /// Event ID for adding an entity.
        /// </summary>
        public const int AddEntityInfo = unchecked((int)0x60010005);

        /// <summary>
        /// Event ID for adding an entity with an error.
        /// </summary>
        public const int AddEntityError = unchecked((int)0xE0010006);

        /// <summary>
        /// Event ID for updating an entity.
        /// </summary>
        public const int UpdateEntityInfo = unchecked((int)0x60010007);

        /// <summary>
        /// Event ID for updating an entity with an error.
        /// </summary>
        public const int UpdateEntityError = unchecked((int)0xE0010008);

        /// <summary>
        /// Event ID for removing an entity.
        /// </summary>
        public const int RemoveEntityInfo = unchecked((int)0x60010009);

        /// <summary>
        /// Event ID for removing an entity with an error.
        /// </summary>
        public const int RemoveEntityError = unchecked((int)0xE001000A);
    }
}
