using System;
using System.Data;
using System.IO;

namespace Simplic.Data
{
    /// <summary>
    /// Customized DataTable to work with ObservableDataRow
    /// </summary>
    public class ObservableDataTable : DataTable
    {
        /// <summary>
        /// Initializes a new instance of the System.Data.DataTable class with no arguments.
        /// </summary>
        public ObservableDataTable() : base()
        {
        }

        /// <summary>
        /// Create datatable from existing table and copy schema
        /// </summary>
        /// <param name="table">Table which contains a schema to copy</param>
        public ObservableDataTable(DataTable table) : base()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Get xml schema for copy purpose
                table.WriteXmlSchema(stream);
                stream.Position = 0;
                this.ReadXmlSchema(stream);
            }
        }

        /// <summary>
        /// Initializes a new instance of the System.Data.DataTable class with the specified
        /// table name.
        /// </summary>
        /// <param name="tableName">The name to give the table. If tableName is null or an empty string, a default
        /// name is given when added to the System.Data.DataTableCollection.
        /// </param>
        public ObservableDataTable(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// The name to give the table. If tableName is null or an empty string, a default
        /// name is given when added to the System.Data.DataTableCollection.
        /// </summary>
        /// <param name="tableName">The name to give the table. If tableName is null or an empty string, a default
        /// name is given when added to the System.Data.DataTableCollection.</param>
        /// <param name="tableNamespace">The namespace for the XML representation of the data stored in the DataTable.</param>
        public ObservableDataTable(string tableName, string tableNamespace) : base(tableName, tableNamespace)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.Data.DataTable class with the System.Runtime.Serialization.SerializationInfo
        /// and the System.Runtime.Serialization.StreamingContext.
        /// </summary>
        /// <param name="info">The data needed to serialize or deserialize an object.</param>
        /// <param name="context">The source and destination of a given serialized stream.</param>
        protected ObservableDataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Override type detection for ObservabelDataRow
        /// </summary>
        /// <returns>typeof(ObservabelDataRow)</returns>
        protected override Type GetRowType()
        {
            return typeof(ObservableDataRow);
        }

        /// <summary>
        /// Factory for create new ObservableDataRow instances
        /// </summary>
        /// <param name="builder">Intern .net framework usage. See documentation of DataRowBuilder.</param>
        /// <returns>Instance of ObservableDataRow</returns>
        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new ObservableDataRow(builder);
        }
    }
}