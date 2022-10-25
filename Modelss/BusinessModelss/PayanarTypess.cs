using MongoDB.Bson.Serialization.Attributes;

namespace Payanarvorkss.Payanar.Tabless.Api.BusinessModelss
{
    public interface IPayanarType
    {
        string UniqueId { get; set; }
        string ParentUniqueId { get; set; }
        string Name { get; set; }
        string FullName { get; }
    }
    public class PayanarType : IPayanarType
    {
        [BsonId]
        public string UniqueId { get; set; } = String.Empty;
        public string ParentUniqueId { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string FullName { get; } = String.Empty;
    }
    public interface IHierarchicalPayanarTableDesign : IPayanarType
    {
        IEnumerable<HierarchicalPayanarTableColumnDesign> Columns { get; set; }
    }
    public class HierarchicalPayanarTableDesign : PayanarType, IHierarchicalPayanarTableDesign
    {
        public HierarchicalPayanarTableDesign()
        {
            Columns = new List<HierarchicalPayanarTableColumnDesign>();
        }
        public IEnumerable<HierarchicalPayanarTableColumnDesign> Columns { get; set; }
    }
    public interface IHierarchicalPayanarTableColumnDesign : IPayanarType
    {
    }
    public class HierarchicalPayanarTableColumnDesign : PayanarType, IHierarchicalPayanarTableColumnDesign
    {
    }
    public interface IPayanarTableDesign : IPayanarType
    {
        System.String OriginalName { get; set; }
        IEnumerable<IPayanarTableColumnDesign> Columnss { get; set; }
    }
    public class PayanarTableDesign : PayanarType, IPayanarTableDesign
    {
        public System.String OriginalName { get; set; } = String.Empty;
        public IEnumerable<IPayanarTableColumnDesign> Columnss { get; set; }
    }
    public interface IPayanarTableColumnDesign : IPayanarType
    {
        System.String LeastPayanarTableColumnDesignUniqueIdentifier { get; set; }
        System.String ReferencedPayanarTableColumnDesignUniqueIdentifier { get; set; }
        System.String PayanarTableDesignUniqueIdentifier { get; set; }
        IPayanarTableDesign ParentPayanarTableDesignInstance { get; set; }
        IPayanarType ColumnTypeDesignTime { get; set; }
        System.String ColumnTypeOriginal { get; set; }
        System.String ColumnType { get; set; }
        System.String LeastPayanarTableColumnDesignFullName { get; set; }
        System.String LeastPayanarTableColumnDesignName { get; set; }
    }
    public class PayanarTableColumnDesign : PayanarType, IPayanarTableColumnDesign
    {
        public System.String LeastPayanarTableColumnDesignUniqueIdentifier { get; set; } = String.Empty;
        public System.String ReferencedPayanarTableColumnDesignUniqueIdentifier { get; set; } = String.Empty;
        public System.String PayanarTableDesignUniqueIdentifier { get; set; } = String.Empty;
        public IPayanarTableDesign ParentPayanarTableDesignInstance { get; set; }
        public IPayanarType ColumnTypeDesignTime { get; set; }
        public System.String ColumnTypeOriginal { get; set; } = String.Empty;
        public System.String ColumnType { get; set; } = String.Empty;
        public System.String LeastPayanarTableColumnDesignFullName { get; set; } = String.Empty;
        public System.String LeastPayanarTableColumnDesignName { get; set; } = String.Empty;
    }
}
