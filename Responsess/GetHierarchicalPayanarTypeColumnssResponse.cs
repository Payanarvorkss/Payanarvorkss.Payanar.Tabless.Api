using Payanarvorkss.Payanar.Tabless.Api.DataModelss;

namespace Payanarvorkss.Payanar.Tabless.Api.Responsess
{
    public class GetHierarchicalPayanarTypeColumnssResponse
    {
        public IEnumerable<HierarchicalPayanarTypeColumn> HierarchicalPayanarTypeColumnss { get; set; }
        public IEnumerable<PayanarTableDesign> TableDesignss { get; set; }
    }
}
