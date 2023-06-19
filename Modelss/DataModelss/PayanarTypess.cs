using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Payanarvorkss.Payanar.Tabless.Api.DataModelss
{
    public interface IPayanarType
    {
        string ParentUniqueId { get; set; }
        string UniqueId { get; set; }
        string Name { get; set; }
        bool IsSystemType { get; set; }
    }
    public class PayanarType : IPayanarType
    {
        [BsonId]
        public string UniqueId { get; set; } = String.Empty;
        [BsonElement("parentUniqueId")]
        public string ParentUniqueId { get; set; } = String.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;
        [BsonElement("isSystemType")]
        public bool IsSystemType { get; set; }
    }
    public interface IHierarchicalPayanarType : IPayanarType
    {
        IEnumerable<IHierarchicalPayanarType> Children { get; set; }
    }
    public class HierarchicalPayanarType : PayanarType, IHierarchicalPayanarType
    {
        public IEnumerable<IHierarchicalPayanarType> Children { get; set; }
    }
    public interface IHierarchicalPayanarTypeColumn : IPayanarType
    {
        bool IsPayanarTableDesign { get; set; }
        IEnumerable<HierarchicalPayanarTypeColumn> Children { get; set; }
    }
    public class HierarchicalPayanarTypeColumn : PayanarType, IHierarchicalPayanarTypeColumn
    {
        private static object _ogj = new object();
        public HierarchicalPayanarTypeColumn()
        {
        }
        public bool IsPayanarTableDesign { get; set; } = false;
        public PayanarTableDesign TableDesign { get; set; }
        private IEnumerable<HierarchicalPayanarTypeColumn> _children = null;
        public IEnumerable<HierarchicalPayanarTypeColumn> Children
        {
            get
            {
                lock (_ogj)
                {
                    if (_children == null)
                        _children = new List<HierarchicalPayanarTypeColumn>();
                }

                return _children;
            }
            set { _children = value; }
        }
    }
    public interface IPayanarTableDesign : IPayanarType
    {
        System.String OriginalName { get; set; }
        IEnumerable<IPayanarTableColumnDesign> Columns { get; set; }
    }
    public class PayanarTableDesign : PayanarType
    {
        public PayanarTableDesign()
        {
            Columns = new List<PayanarTableColumnDesign>();
        }
        [BsonElement("originalName")]
        public System.String OriginalName { get; set; } = String.Empty;
        [BsonElement("columns")]
        public IEnumerable<PayanarTableColumnDesign> Columns { get; set; }
    }
    public interface IPayanarTableColumnDesign : IPayanarType
    {
        System.String OriginalName { get; set; }
        System.String ReferencedPayanarTableDesignUniqueId { get; set; }
        System.String ReferencedPayanarTableColumnDesignUniqueId { get; set; }
        System.String LeastPayanarTableDesignUniqueId { get; set; }
        System.String LeastPayanarTableColumnDesignUniqueId { get; set; }
    }
    public class PayanarTableColumnDesign : PayanarType, IPayanarTableColumnDesign
    {
        [BsonElement("originalName")]
        public System.String OriginalName { get; set; }
        [BsonElement("referencedPayanarTableDesignUniqueId")]
        public System.String ReferencedPayanarTableDesignUniqueId { get; set; } = String.Empty;
        [BsonElement("referencedPayanarTableColumnDesignUniqueId")]
        public System.String ReferencedPayanarTableColumnDesignUniqueId { get; set; } = String.Empty;
        [BsonElement("leastPayanarTableDesignUniqueId")]
        public System.String LeastPayanarTableDesignUniqueId { get; set; } = String.Empty;
        [BsonElement("leastPayanarTableColumnDesignUniqueId")]
        public System.String LeastPayanarTableColumnDesignUniqueId { get; set; } = String.Empty;
    }
    public class PayanarTable
    {
        public PayanarTable()
        {
            Rows = new List<PayanarTableRow>();
        }
        [BsonId]
        public string UniqueId { get; set; } = String.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = String.Empty;
        [BsonElement("rows")]
        public IEnumerable<PayanarTableRow> Rows { get; set; }
        public void AddRow(DtoModelss.PayanarTableRow row)
        {
            (Rows as IList<PayanarTableRow>).Add(new PayanarTableRow(row.Cells) { UniqueId = row.UniqueId, IsDirty = row.IsDirty, IsNew = row.IsNew });
        }
    }
    public class PayanarTableRow
    {
        public PayanarTableRow() { }
        public PayanarTableRow(IDictionary<string, DtoModelss.PayanarTableCell> cells)
        {
            Cells = new Dictionary<string, PayanarTableCell>();
            AddCells(cells);
        }
        [BsonId]
        public string UniqueId { get; set; } = String.Empty;
        [BsonElement("cells")]
        public IDictionary<string, PayanarTableCell> Cells { get; set; }
        [BsonIgnore]
        public bool IsNew { get; set; }
        [BsonIgnore]
        public bool IsDirty { get; set; }
        public void SetValue(string columnName, string value)
        {
            Cells[columnName].Value = value;
        }
        private void AddCells(IDictionary<string, DtoModelss.PayanarTableCell> cells)
        {
            cells?.ToList().ForEach(v =>
            {
                Cells.Add(v.Key, new PayanarTableCell() { ColumnDesignUniqueId = v.Value.ColumnDesignUniqueId, Value = v.Value.Value, ReferencedPayanarTableDesignUniqueId = v.Value.ReferencedPayanarTableDesignUniqueId, ReferencedPayanarTableRowUniqueId = v.Value.ReferencedPayanarTableRowUniqueId });
            });
        }
    }
    public class PayanarTableCell
    {
        [BsonElement("columnDesignUniqueId")]
        public string ColumnDesignUniqueId { get; set; }
        [BsonElement("value")]
        public string Value { get; set; }
        [BsonElement("referencedPayanarTableDesignUniqueId")]
        public System.String ReferencedPayanarTableDesignUniqueId { get; set; }
        [BsonElement("referencedPayanarTableRowUniqueId")]
        public System.String ReferencedPayanarTableRowUniqueId { get; set; }
        [BsonIgnore]
        public bool IsDirty { get; set; }
    }
}
