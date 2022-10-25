using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using Payanarvorkss.Payanar.Tabless.Api.DataModelss;
using Payanarvorkss.Payanar.Tabless.Api.Responsess;
using Payanarvorkss.Payanar.Tabless.Api.Utilitiess;
using System.Text.Json;

namespace Payanarvorkss.Payanar.Tabless.Api.Controllerss
{
    [Route("vtree/api/[controller]")]
    [ApiController]
    public class HierarchicalPayanarTypeController : ControllerBase
    {
        [HttpPost]
        public async Task<HierarchicalPayanarType> Create([FromBody] HierarchicalPayanarType tableDesign)
        {
            var client = new MongoDB.Driver.MongoClient("mongodb+srv://bhuvaness:Kg3dQIRhQeKmKAt7@cluster0.dt0ycsn.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("PayanarTabless");
            var payanarTabless = database.GetCollection<HierarchicalPayanarType>("HierarchicalPayanarTypess");
            ////var tableDesign = JsonSerializer.Deserialize<PayanarTableDesign>(json);
            await payanarTabless.InsertOneAsync(tableDesign);
            return tableDesign;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<HierarchicalPayanarTypeColumn>>> Read(string id)
        {
            IEnumerable<HierarchicalPayanarTypeColumn> results = new List<HierarchicalPayanarTypeColumn>();
            var client = new MongoDB.Driver.MongoClient("mongodb+srv://bhuvaness:Kg3dQIRhQeKmKAt7@cluster0.dt0ycsn.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("PayanarTabless");
            var payanarTypess = database.GetCollection<HierarchicalPayanarType>("HierarchicalPayanarTypess");
            var payanarTableDesignss = database.GetCollection<PayanarTableDesign>("PayanarTableDesignss");
            var query = await payanarTypess.FindAsync(x => x.ParentUniqueId.Equals(id));
            var query2 = await payanarTableDesignss.FindAsync(x => x.ParentUniqueId.Equals(id));

            var hierarchicalTypess = query.ToList();
            var tableDesignss = query2.ToList();

            hierarchicalTypess?.ForEach(x =>
            {
                (results as IList<HierarchicalPayanarTypeColumn>).Add(new HierarchicalPayanarTypeColumn() { UniqueId = x.UniqueId, ParentUniqueId = x.ParentUniqueId, Name = x.Name, IsPayanarTableDesign = false });
            });

            tableDesignss?.ForEach(x =>
            {
                (results as IList<HierarchicalPayanarTypeColumn>).Add(new HierarchicalPayanarTypeColumn() { UniqueId = x.UniqueId, ParentUniqueId = x.ParentUniqueId, Name = x.Name, IsPayanarTableDesign = true });
            });

            return Ok(results);
        }
        [HttpGet]
        public async Task<ActionResult<GetHierarchicalPayanarTypeColumnssResponse>> Read()
        {
            GetHierarchicalPayanarTypeColumnssResponse response = new GetHierarchicalPayanarTypeColumnssResponse();

            IEnumerable<HierarchicalPayanarTypeColumn> results = new List<HierarchicalPayanarTypeColumn>();
            var client = new MongoDB.Driver.MongoClient("mongodb+srv://bhuvaness:Kg3dQIRhQeKmKAt7@cluster0.dt0ycsn.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("PayanarTabless");
            var payanarTypess = database.GetCollection<HierarchicalPayanarType>("HierarchicalPayanarTypess");
            var payanarTableDesignss = database.GetCollection<PayanarTableDesign>("PayanarTableDesignss");
            var query = await payanarTypess.FindAsync(x => !string.IsNullOrEmpty(x.UniqueId));
            var query2 = await payanarTableDesignss.FindAsync(x => !string.IsNullOrEmpty(x.UniqueId));

            var hierarchicalTypess = query.ToList().OrderBy(x => x.ParentUniqueId).ToList();
            var tableDesignss = query2.ToList().OrderBy(x => x.ParentUniqueId).ToList();

            var dic = ArrangeByParentId(hierarchicalTypess);
            ArrangeTreeStructure(results, dic, tableDesignss, Utility.FirstParentId);

            response.HierarchicalPayanarTypeColumnss = results;
            response.TableDesignss = tableDesignss;

            return Ok(response);
        }
        private IDictionary<string, IEnumerable<HierarchicalPayanarTypeColumn>> ArrangeByParentId(IEnumerable<HierarchicalPayanarType> hierarchicals)
        {
            IDictionary<string, IEnumerable<HierarchicalPayanarTypeColumn>> dic = new Dictionary<string, IEnumerable<HierarchicalPayanarTypeColumn>>();
            foreach (var eachItem in hierarchicals)
            {
                if (!dic.ContainsKey(eachItem.ParentUniqueId))
                    dic[eachItem.ParentUniqueId] = new List<HierarchicalPayanarTypeColumn>();

                (dic[eachItem.ParentUniqueId] as IList<HierarchicalPayanarTypeColumn>).Add(new HierarchicalPayanarTypeColumn() { UniqueId = eachItem.UniqueId, ParentUniqueId = eachItem.ParentUniqueId, Name = eachItem.Name, IsPayanarTableDesign = false });
            }

            return dic;
        }
        private void ArrangeTreeStructure(IEnumerable<HierarchicalPayanarTypeColumn> results, IDictionary<string, IEnumerable<HierarchicalPayanarTypeColumn>> dic, IEnumerable<PayanarTableDesign> tableDesignss, string parentId)
        {
            foreach (var eachItem in dic)
            {
                if (eachItem.Key.Equals(parentId))
                {
                    foreach (var child in eachItem.Value)
                    {
                        (results as IList<HierarchicalPayanarTypeColumn>).Add(child);
                        ArrangeTreeStructure(child.Children, dic, tableDesignss, child.UniqueId);

                        foreach (var eachTable in tableDesignss)
                        {
                            if (eachTable.ParentUniqueId.Equals(child.UniqueId))
                            {
                                (child.Children as IList<HierarchicalPayanarTypeColumn>).Add(
                                    new HierarchicalPayanarTypeColumn()
                                    {
                                        UniqueId = eachTable.UniqueId,
                                        ParentUniqueId = eachTable.ParentUniqueId,
                                        Name = eachTable.Name,
                                        IsPayanarTableDesign = true,
                                        TableDesign = eachTable
                                    });
                            }
                        }
                    }
                }
            }
        }
        [HttpPatch]
        public IEnumerable<HierarchicalPayanarType> Update()
        {
            var client = new MongoDB.Driver.MongoClient("mongodb+srv://bhuvaness:Kg3dQIRhQeKmKAt7@cluster0.dt0ycsn.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("PayanarTabless");
            var payanarTabless = database.GetCollection<HierarchicalPayanarType>("HierarchicalPayanarTypess");

            return null;
        }
        [HttpDelete]
        public IEnumerable<HierarchicalPayanarType> Delete()
        {
            var client = new MongoDB.Driver.MongoClient("mongodb+srv://bhuvaness:Kg3dQIRhQeKmKAt7@cluster0.dt0ycsn.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("PayanarTabless");
            var payanarTabless = database.GetCollection<HierarchicalPayanarType>("HierarchicalPayanarTypess");

            return null;
        }
    }
}
