using MongoDB.Bson.Serialization.Attributes;

namespace Payanarvorkss.Payanar.Tabless.Api.CommonModelss
{
    public interface IPayanarType
    {
        string ParentUniqueId { get; set; }
        string UniqueId { get; set; }
        string Name { get; set; }
    }
    public interface IPayanarTableDesign : IPayanarType
    {
        System.String OriginalName { get; set; }
        IEnumerable<IPayanarTableColumnDesign> Columns { get; set; }
    }
    public interface IHierarchicalPayanarTableDesign : IPayanarType
    {
        IEnumerable<IHierarchicalPayanarTableColumnDesign> Columns { get; set; }
    }
    public interface IHierarchicalPayanarTableColumnDesign : IPayanarType
    {
    }
    public interface IPayanarTableColumnDesign : IPayanarType
    {
        System.String LeastPayanarTableColumnDesignUniqueIdentifier { get; set; }
        System.String ReferencedPayanarTableColumnDesignUniqueIdentifier { get; set; }
        System.String PayanarTableDesignUniqueIdentifier { get; set; }
        System.String ReferencedPayanarTableDesignUniqueIdentifier { get; set; }
    }
}
