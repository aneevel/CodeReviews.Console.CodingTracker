namespace CodingTrackerEngine.Database
{
    /// <summary>
    /// Interface <c>IDatabaseManager</c> provides an interface for an object which handles database operations
    /// </summary>
    public interface IDatabaseManager
    {
        /// <summary>
        /// Read the records from main table
        /// </summary>
        public abstract void ReadRecords();

        /// <summary>
        /// Insert a record with the passed parameters
        /// </summary>
        /// <param name="id">id of the record</param>
        /// <param name="startDate">Start Time of the record</param>
        /// <param name="endDate">End Time of the record</param>
        /// <param name="duration">Duration of record</param>
        public abstract void InsertRecord(
            string id,
            DateTime startDate,
            DateTime endDate,
            TimeSpan duration
        );

        /// <summary>
        /// Update a record with the given ID
        /// </summary>
        /// <param name="id">ID of the record</param>
        /// <param name="startDate">Start Time of the record</param>
        /// <param name="endDate">End Time of the record</param>
        /// <param name="duration">Duration of record</param>
        public abstract void UpdateRecord(
            int id,
            DateTime startDate,
            DateTime endDate,
            TimeSpan duration
        );

        /// <summary>
        /// Delete a record with the given ID
        /// </summary>
        /// <param name="id">ID of the record</param>
        public abstract void DeleteRecord(int id);

        /// <summary>
        /// Check if a record exists with given ID
        /// </summary>
        /// <param name="id">Id of the record</param>
        public abstract bool RecordExists(int id);

        /// <summary>
        /// Check if the main table in database exists
        /// </summary>
        /// <returns> true if it exists, false otherwise </returns>
        public abstract bool TableExists();
    }
}
