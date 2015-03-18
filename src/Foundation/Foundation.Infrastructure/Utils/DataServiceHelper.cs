using SE.DSP.Foundation.Infrastructure.BE.Entities.DataService;
using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Enumerations;
using SE.DSP.Foundation.Infrastructure.Enumerations.Energy;
using SE.DSP.Foundation.Infrastructure.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SE.DSP.Foundation.Infrastructure.Utils
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataServiceQueryRouteParameterAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DataServiceQueryRouteAttribute : Attribute
    {
        public string RouteFormat { get; set; }

        public DataServiceQueryRouteAttribute(string format)
        {
            this.RouteFormat = format;
        }
    }

    public interface IDataServiceQuery
    {
        string[] Columns { get; set; }

        bool? IsReverse { get; set; }

        int? Top { get; set; }
    }

    public abstract class DataServiceQueryBase : IDataServiceQuery
    {
        public string[] Columns { get; set; }

        public bool? IsReverse { get; set; }

        public int? Top { get; set; }
    }

    public interface IDataServiceEntity
    {
    }

    public static class DataServiceColumns
    {
        public const string Id = "Id";
        public const string New = "New";
        public const string SPId = "SPId";
        public const string Time = "Time";
        public const string Value = "Value";
        public const string RuleId = "RuleId";
        public const string Status = "Status";
        public const string Quality = "Quality";
        public const string TouPeak = "TouPeak";
        public const string IsModify = "IsModify";
        public const string MaxValue = "MaxValue";
        public const string MinValue = "MinValue";
        public const string Original = "Original";
        public const string TouPlain = "TouPlain";
        public const string UserName = "UserName";
        public const string TotalArea = "TotalArea";
        public const string TouValley = "TouValley";
        public const string TotalCount = "TotalCount";
        public const string CoolingArea = "CoolingArea";
        public const string HeatingArea = "HeatingArea";
        public const string TotalPerson = "TotalPerson";
        public const string AverageValue = "AverageValue";
        public const string RankingValue = "RankingValue";
        public const string WorkDayRatio = "WorkDayRatio";
        public const string DayNightRatio = "DayNightRatio";
    }

    public static class DataService<E> where E : IDataServiceEntity
    {
        public static void Create(E[] entities, string[] columns = null)
        {
            string url = DataServiceRouteHelper.GetNonQueryUrl<E>(columns);
            string data = JsonHelper.Serialize2String<E[]>(entities);

            HttpHelper.Post(url, data);
        }

        public static void Update(E[] entities, string[] columns = null)
        {
            string url = DataServiceRouteHelper.GetNonQueryUrl<E>(columns);
            string data = JsonHelper.Serialize2String<E[]>(entities);

            HttpHelper.Put(url, data);
        }

        public static void Delete(E[] entities)
        {
            string url = DataServiceRouteHelper.GetNonQueryUrl<E>(null);
            string data = JsonHelper.Serialize2String<E[]>(entities);

            HttpHelper.Delete(url, data);
        }
    }

    public static class DataService<Q, E>
        where Q : IDataServiceQuery
        where E : IDataServiceEntity
    {

        public static E[] Get(Q query)
        {
            string url = DataServiceRouteHelper.GetQueryUrl<Q>(query);
            string data = HttpHelper.Get(url);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<E[]>(data);
        }

        public static void Delete(Q query)
        {
            var data = Get(query);
            DataService<E>.Delete(data);
        }
    }

    public static class HttpHelper
    {
        public static string Get(string url)
        {
            return MakeHttpRequest("GET", url, null);
        }

        public static string Post(string url, string body)
        {
            return MakeHttpRequest("POST", url, body);
        }

        public static string Put(string url, string body)
        {
            return MakeHttpRequest("PUT", url, body);
        }

        public static string Delete(string url, string body)
        {
            return MakeHttpRequest("DELETE", url, body);
        }

        private static string MakeHttpRequest(string method, string url, string body)
        {
            HttpClientHandler handler = new HttpClientHandler();

            if (!String.IsNullOrEmpty(ConfigHelper.Get("ProxyHost")))
            {
                handler.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), Convert.ToInt32(ConfigHelper.Get("ProxyPort")));
                handler.UseProxy = true;
            }

            HttpClient client = new HttpClient(handler);

            client.Timeout = TimeSpan.FromMinutes(1);
            HttpResponseMessage output;
            if (method == "PUT")
            {
                output = client.PutAsync(url, new StringContent(body, Encoding.UTF8, "application/json")).Result;
            }
            else if (method == "POST")
            {
                output = client.PostAsync(url, new StringContent(body, Encoding.UTF8, "application/json")).Result;
            }
            else if (method == "DELETE")
            {
                output = client.SendAsync(new HttpRequestMessage()
                {
                    Method = HttpMethod.Delete,
                    Content = new StringContent(body, UTF8Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(url)
                }).Result;
            }
            else
            {
                output = client.GetAsync(url).Result;
            }

            return output.Content.ReadAsStringAsync().Result;

            /*var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method;
            request.Timeout = 60000 * 5;
            request.ContentType = "application/json";
            request.ContentLength = 0;
            if (!String.IsNullOrEmpty(ConfigHelper.Get("ProxyHost")))
                request.Proxy = new WebProxy(ConfigHelper.Get("ProxyHost"), Convert.ToInt32(ConfigHelper.Get("ProxyPort")));

            if (!String.IsNullOrEmpty(body))
            {
                var requestBody = Encoding.UTF8.GetBytes(body);
                request.ContentLength = requestBody.Length;
                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(requestBody, 0, requestBody.Length);
                }
            }
            string output = "";
            using (var response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    output = sr.ReadToEnd();
                }
            }

            return output;*/
        }
    }

    public static class DataServiceRouteHelper
    {
        static string DataServiceHost = ConfigHelper.Get("DataServiceHost");

        public static string GetQueryUrl<Q>(Q query) where Q : IDataServiceQuery
        {
            var routeAttribute = (DataServiceQueryRouteAttribute)typeof(Q).GetCustomAttribute(typeof(DataServiceQueryRouteAttribute));
            var parameters = query.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.IsDefined(typeof(DataServiceQueryRouteParameterAttribute), false));

            string route = routeAttribute.RouteFormat.ToLower();

            foreach (var parameter in parameters)
            {
                string target = "{" + parameter.Name.ToLower() + "}";

                object value = parameter.GetValue(query);
                if (value is Enum)
                    value = (int)value;

                route = route.Replace(target, value == null ? "null" : value.ToString());
            }

            route = DataServiceHost + GetBaseRouteByQuery<Q>() + route;

            route += GetAdditionalParameter(query.Columns, isReverse: query.IsReverse, top: query.Top);

            return route;
        }

        public static string GetNonQueryUrl<E>(string[] columns) where E : IDataServiceEntity
        {
            string route = DataServiceHost + GetBaseRouteByEntity<E>();

            if (columns != null && columns.Length > 0)
                route += GetAdditionalParameter(columns);

            return route;
        }

        private static string GetAdditionalParameter(string[] columns, bool? isReverse = null, int? top = null)
        {
            if ((columns == null || columns.Length <= 0) && !isReverse.HasValue && !top.HasValue)
                return String.Empty;

            StringBuilder parameter = new StringBuilder("?");
            if (columns != null && columns.Length > 0)
            {
                foreach (var column in columns)
                    parameter.AppendFormat("columns={0}&", column);
            }

            if (isReverse.HasValue)
            {
                parameter.AppendFormat("isreverse={0}&", isReverse.Value);
            }

            if (top.HasValue)
            {
                parameter.AppendFormat("top={0}&", top.Value);
            }

            string paramString = parameter.ToString();

            return paramString.Substring(0, paramString.Length - 1);
        }

        private static string GetBaseRouteByQuery<Q>() where Q : IDataServiceQuery
        {
            string name = typeof(Q).Name.ToString().Replace("Query", String.Empty);

            return GetBaseRoute(name);
        }

        private static string GetBaseRouteByEntity<E>() where E : IDataServiceEntity
        {
            string name = typeof(E).Name.ToString().Replace("Entity", String.Empty);

            return GetBaseRoute(name);
        }

        private static string GetBaseRoute(string name)
        {
            name = name + "ApiPath";
            return ConfigHelper.Get(name);
        }
    }

    public static class DataServiceHelper
    {
        private static int MaxRetrieveRawCount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MaxRetrieveRowCount"]);

        #region IBenchmarkDataDA
        public static EnergyConsumptionBenchmarkDataEntity[] RetrieveEnergyConsumptionUnitData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = !unitType.HasValue ? null : GetColumns(unitType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveEnergyConsumptionBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns: columns, topCount: topCount);
        }

        public static EnergyConsumptionBenchmarkDataEntity[] RetrieveEnergyConsumptionRatioData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, RatioType? ratioType = null)
        {
            var columns = !ratioType.HasValue ? null : GetColumns(ratioType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveEnergyConsumptionBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns: columns, topCount: topCount);
        }

        public static EnergyConsumptionBenchmarkDataEntity[] RetrieveEnergyConsumptionLabelingData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, LabelingType? labelingType = null)
        {
            var columns = !labelingType.HasValue ? null : GetColumns(labelingType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveEnergyConsumptionBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns: columns, topCount: topCount);
        }

        private static EnergyConsumptionBenchmarkDataEntity[] RetrieveEnergyConsumptionBenchmarkData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, string[] columns = null, int? topCount = null)
        {
            var range = utcTimeRange.ToTickRange();
            var query = new EnergyConsumptionBenchmarkDataQuery()
            {
                CommodityId = commodityId,
                IndustryId = industryId,
                ZoneId = zoneId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Top = topCount,
                Columns = columns,
            };

            var data = DataService<EnergyConsumptionBenchmarkDataQuery, EnergyConsumptionBenchmarkDataEntity>.Get(query);

            return data;
        }

        public static CostBenchmarkDataEntity[] RetrieveCostUnitData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = !unitType.HasValue ? null : GetColumnsWithOrig(unitType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveCostBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns, topCount);
        }

        public static CostBenchmarkDataEntity[] RetrieveCostRatioData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, RatioType? ratioType = null)
        {
            var columns = !ratioType.HasValue ? null : GetColumns(ratioType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveCostBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns, topCount);
        }

        private static CostBenchmarkDataEntity[] RetrieveCostBenchmarkData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, string[] columns, int? topCount)
        {
            var range = utcTimeRange.ToTickRange();
            var query = new CostBenchmarkDataQuery()
            {
                CommodityId = commodityId,
                IndustryId = industryId,
                ZoneId = zoneId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Top = topCount,
                Columns = columns
            };

            return DataService<CostBenchmarkDataQuery, CostBenchmarkDataEntity>.Get(query);
        }

        public static StandardCoalBenchmarkDataEntity[] RetrieveCoalUnitData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = !unitType.HasValue ? null : GetColumns(unitType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveStandardCoalBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns, topCount);
        }

        public static StandardCoalBenchmarkDataEntity[] RetrieveCoalRatioData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, PagingOrder pagingOrder = null, RatioType? ratioType = null)
        {
            var columns = !ratioType.HasValue ? null : GetColumns(ratioType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            return RetrieveStandardCoalBenchmarkData(commodityId, industryId, zoneId, aggregationStep, utcTimeRange, columns, topCount);
        }
        private static StandardCoalBenchmarkDataEntity[] RetrieveStandardCoalBenchmarkData(long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, TimeRange utcTimeRange, string[] columns, int? topCount)
        {
            var range = utcTimeRange.ToTickRange();
            var query = new StandardCoalBenchmarkDataQuery()
            {
                CommodityId = commodityId,
                IndustryId = industryId,
                ZoneId = zoneId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Top = topCount,
                Columns = columns
            };

            return DataService<StandardCoalBenchmarkDataQuery, StandardCoalBenchmarkDataEntity>.Get(query);
        }
        #endregion

        #region ICostDataDA
        public static CostDataEntity[] RetrieveHierarchyCostData(long hierarchyId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = GetColumnsWithOrig(unitType);
            var touColumns = new string[] { DataServiceColumns.TouPeak, DataServiceColumns.TouPlain, DataServiceColumns.TouValley };
            var range = utcTimeRange.ToTickRange();
            var query = new CostDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns.Union(touColumns).ToArray()
            };
            return DataService<CostDataQuery, CostDataEntity>.Get(query);
        }

        public static CostDataEntity[] RetrieveSystemDimensionCostData(long hierarchyId, long systemDimensionTemplateItemId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = GetColumnsWithOrig(unitType);
            var touColumns = new string[] { DataServiceColumns.TouPeak, DataServiceColumns.TouPlain, DataServiceColumns.TouValley };
            var range = utcTimeRange.ToTickRange();
            var query = new CostDataQuery()
            {
                HierarchyId = hierarchyId,
                SystemDimensionItemId = systemDimensionTemplateItemId,
                CommodityId = commodityId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns.Union(touColumns).ToArray()
            };
            return DataService<CostDataQuery, CostDataEntity>.Get(query);
        }

        public static CostDataEntity[] RetrieveAreaDimensionCostData(long areaDimensionId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = GetColumnsWithOrig(unitType);
            var touColumns = new string[] { DataServiceColumns.TouPeak, DataServiceColumns.TouPlain, DataServiceColumns.TouValley };
            var range = utcTimeRange.ToTickRange();
            var query = new CostDataQuery()
            {
                AreaDimensionId = areaDimensionId,
                CommodityId = commodityId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns.Union(touColumns).ToArray()
            };
            return DataService<CostDataQuery, CostDataEntity>.Get(query);
        }
        #endregion

        #region ICostTBDataDA
        public static CostTargetBaselineDataEntity[] RetrieveHierarchyCostTBData(long hierarchyId, long commodityId, TimeRange utcTimeRange,
            TimeGranularity aggregationStep, string tbcode = TargetBaselineColumnName.Target,
            PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = GetColumns(unitType);
            var range = utcTimeRange.ToTickRange();
            var query = new CostTargetBaselineDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                TargetBaselineCode = ((int)ConvertTargetBaselineType(tbcode)).ToString(),
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
            };
            return DataService<CostTargetBaselineDataQuery, CostTargetBaselineDataEntity>.Get(query);
        }

        public static CostTargetBaselineDataEntity[] RetrieveSystemDimensionCostTBData(long hierarchyId, long systemDimensionTemplateItemId,
            long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep,
            string tbcode = TargetBaselineColumnName.Target, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = GetColumns(unitType);
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var query = new CostTargetBaselineDataQuery()
            {
                HierarchyId = hierarchyId,
                SystemDimensionItemId = systemDimensionTemplateItemId,
                CommodityId = commodityId,
                TargetBaselineCode = ((int)ConvertTargetBaselineType(tbcode)).ToString(),
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
                Top = top,
            };
            return DataService<CostTargetBaselineDataQuery, CostTargetBaselineDataEntity>.Get(query);
        }

        public static CostTargetBaselineDataEntity[] RetrieveAreaDimensionCostTBData(long areaDimensionId, long commodityId, TimeRange utcTimeRange,
            TimeGranularity aggregationStep, string tbcode = TargetBaselineColumnName.Target,
            PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = GetColumns(unitType);
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var query = new CostTargetBaselineDataQuery()
            {
                AreaDimensionId = areaDimensionId,
                CommodityId = commodityId,
                TargetBaselineCode = ((int)ConvertTargetBaselineType(tbcode)).ToString(),
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
            };
            return DataService<CostTargetBaselineDataQuery, CostTargetBaselineDataEntity>.Get(query);
        }
        #endregion

        #region IEnergyDataDA
        public static TagDataEntity[] GetTagData(long tagId, TimeRange utcTimeRange, TimeGranularity step, PagingOrder pagingOrder, int? topCount = null, UnitType? unitType = null, RatioType? ratioType = null, LabelingType? labelingType = null)
        {
            if (pagingOrder != null)
                topCount = (int?)MaxRetrieveRawCount;
            var range = utcTimeRange.ToTickRange();
            string[] columns = GetColumns(unitType, ratioType, labelingType);
            var query = new TagDataQuery()
            {
                TagId = tagId,
                Step = step,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
                Top = topCount
            };
            return DataService<TagDataQuery, TagDataEntity>.Get(query);
        }


        public static RawDataEntity[] GetTopRawData(long tagId, TimeRange utcTimeRange, int? topCount = null, bool? isReserve = null, bool withoutBlank = false)
        {
            if (topCount > MaxRetrieveRawCount)
                throw new ParameterException(Layer.DA, SE.DSP.Foundation.Infrastructure.Enumerations.Module.Energy, Convert.ToInt32(EnergyErrorCode.MaxRetrieveRowCountIsIllegal));

            var range = utcTimeRange.ToTickRange();
            if (isReserve.HasValue && isReserve.Value)
                range = new Range() { Start = range.End, End = range.Start };
            var query = new RawDataQuery()
            {
                TagId = tagId,
                StartTime = range.Start,
                EndTime = range.End,
                Top = MaxRetrieveRawCount,
                IsReverse = isReserve
            };

            var data = DataService<RawDataQuery, RawDataEntity>.Get(query);

            if (topCount.HasValue)
            {
                List<RawDataEntity> validate = new List<RawDataEntity>();
                int localCount = topCount.Value;
                int idx = 0;
                for (; idx < data.Length && localCount > 0; idx++)
                {
                    if (!withoutBlank || data[idx].Value.HasValue)
                    {
                        validate.Insert(0, data[idx]);
                        localCount--;
                    }
                }

                data = validate.ToArray();
            }

            return data;
        }

        public static RawDataEntity[] GetRawData(long tagId, TimeRange utcTimeRange, PagingOrder pagingOrder)
        {
            var range = utcTimeRange.ToTickRange();
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var query = new RawDataQuery()
            {
                TagId = tagId,
                StartTime = range.Start,
                EndTime = range.End,
                Top = topCount
            };

            var data = DataService<RawDataQuery, RawDataEntity>.Get(query);

            return data;
        }

        public static TagDataEntity[] ConvertRawDataToTagData(RawDataEntity[] rawData)
        {
            return rawData.Select(e => new TagDataEntity()
            {
                TagId = e.TagId,
                Time = e.Time,
                Step = TimeGranularity.Minite,
                Value = e.Value,
                Quality = e.Quality,
            }).ToArray();
        }

        private static string[] GetColumns(UnitType? unitType, RatioType? ratioType, LabelingType? labelingType)
        {
            List<string> columns = new List<string>() { DataServiceColumns.Value, DataServiceColumns.Quality };
            string[] unit = GetColumns(unitType), ratio = GetColumns(ratioType), labeling = GetColumns(labelingType);

            if (unitType.HasValue && unit != null)
                columns.AddRange(unit);
            if (ratioType.HasValue && ratio != null)
                columns.AddRange(ratio);
            if (labelingType.HasValue && labeling != null)
                columns.AddRange(labeling);

            return columns.ToArray();
        }

        private static string[] GetColumns(UnitType? unitType)
        {
            var columnName = DataServiceColumns.Value;
            if (unitType.HasValue)
            {
                if (unitType.Value == UnitType.CoolingAreaUnit) columnName = DataServiceColumns.CoolingArea;
                else if (unitType.Value == UnitType.HeatingAreaUnit) columnName = DataServiceColumns.HeatingArea;
                else if (unitType.Value == UnitType.TotalAreaUnit) columnName = DataServiceColumns.TotalArea;
                else if (unitType.Value == UnitType.TotalPersonUnit) columnName = DataServiceColumns.TotalPerson;
                else if (unitType.Value == UnitType.OrignValue) columnName = DataServiceColumns.Value;
            };

            return String.IsNullOrEmpty(columnName) ? null : new string[] { columnName };
        }
        private static string[] GetColumnsWithOrig(UnitType? unitType)
        {
            var columnName = DataServiceColumns.Value;
            if (unitType.HasValue)
            {
                if (unitType.Value == UnitType.CoolingAreaUnit)
                    return new string[] { DataServiceColumns.CoolingArea, columnName };
                else if (unitType.Value == UnitType.HeatingAreaUnit)
                    return new string[] { DataServiceColumns.HeatingArea, columnName };
                else if (unitType.Value == UnitType.TotalAreaUnit)
                    return new string[] { DataServiceColumns.TotalArea, columnName };
                else if (unitType.Value == UnitType.TotalPersonUnit)
                    return new string[] { DataServiceColumns.TotalPerson, columnName };
                else if (unitType.Value == UnitType.OrignValue)
                    return new string[] { columnName };
            };

            return new string[] { columnName };
        }
        private static string[] GetColumns(RatioType? ratioType)
        {
            var columnName = string.Empty;
            if (ratioType.HasValue)
            {
                if (ratioType.Value == RatioType.DayNight) columnName = DataServiceColumns.DayNightRatio;
                else if (ratioType.Value == RatioType.WorkDay) columnName = DataServiceColumns.WorkDayRatio;
            }
            return String.IsNullOrEmpty(columnName) ? null : new string[] { columnName };
        }

        private static string[] GetColumns(RankingType? rankingType)
        {
            string[] columns = null;
            if (rankingType == RankingType.CustomerRanking)
                columns = new string[] { DataServiceColumns.Value, DataServiceColumns.RankingValue, DataServiceColumns.TotalCount };
            else if (rankingType == RankingType.TotalPersonUnit)
                columns = new string[] { DataServiceColumns.Value, DataServiceColumns.TotalPerson, };
            else if (rankingType == RankingType.TotalAreaUnit)
                columns = new string[] { DataServiceColumns.Value, DataServiceColumns.TotalArea };
            else if (rankingType == RankingType.HeatingAreaUnit)
                columns = new string[] { DataServiceColumns.Value, DataServiceColumns.HeatingArea };
            else if (rankingType == RankingType.CoolingAreaUnit)
                columns = new string[] { DataServiceColumns.Value, DataServiceColumns.CoolingArea };
            else if (rankingType == RankingType.Corporation)
                columns = new string[] { DataServiceColumns.Value };
            else
                columns = new string[] { DataServiceColumns.Value, DataServiceColumns.RankingValue, DataServiceColumns.TotalCount };

            return columns;
        }

        private static string[] GetColumns(LabelingType? labelingType)
        {
            var columnName = string.Empty;
            if (labelingType.HasValue)
            {
                if (labelingType.Value == LabelingType.CoolingAreaUnit) columnName = DataServiceColumns.CoolingArea;
                else if (labelingType.Value == LabelingType.HeatingAreaUnit) columnName = DataServiceColumns.HeatingArea;
                else if (labelingType.Value == LabelingType.TotalAreaUnit) columnName = DataServiceColumns.TotalArea;
                else if (labelingType.Value == LabelingType.TotalPersonUnit) columnName = DataServiceColumns.TotalPerson;
                else if (labelingType.Value == LabelingType.DayNightRatioValue) columnName = DataServiceColumns.DayNightRatio;
                else if (labelingType.Value == LabelingType.WorkDayRatioValue) columnName = DataServiceColumns.WorkDayRatio;
            };

            return String.IsNullOrEmpty(columnName) ? null : new string[] { columnName };
        }

        public static void UpdateTagRawData(RawDataEntity[] entities)
        {
            DataService<RawDataEntity>.Update(entities);
        }
        #endregion

        #region IEnergyTBDataDA
        public static TargetBaselineUnitDataEntity[] RetrieveTagTBData(long tbId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder, UnitType? unitType = null)
        {
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            string[] columns = GetColumns(unitType);
            var query = new TargetBaselineUnitDataQuery()
            {
                TargetBaselineId = tbId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
                Top = top,
            };
            return DataService<TargetBaselineUnitDataQuery, TargetBaselineUnitDataEntity>.Get(query);
        }
        #endregion

        #region ILabelingLevelDataDA
        public static LabellingDataEntity RetrieveLabelingLevelData(long spId, LabelingType labelingType, long commodityId, long industryId, long zoneId, TimeGranularity aggregationStep, long month)
        {
            var query = new LabellingDataQuery()
            {
                SpId = spId,
                LabellingType = (int)labelingType,
                CommodityId = commodityId,
                IndustryId = industryId,
                ZoneId = zoneId,
                Month = month
            };

            return DataService<LabellingDataQuery, LabellingDataEntity>.Get(query).FirstOrDefault();
        }
        #endregion

        #region IRankingDataDA
        public static StandardCoalDataEntity[] RetrieveRankingCoalData(RankingType rankingType, long hierarchyId, long commodityId, TimeRange timeRange, TimeGranularity step, PagingOrder pagingOrder = null)
        {
            var range = timeRange.ToTickRange();
            var query = new StandardCoalDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                Step = step,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = GetColumns(rankingType),
            };
            return DataService<StandardCoalDataQuery, StandardCoalDataEntity>.Get(query);
        }

        public static CostDataEntity[] RetrieveHierarchyCostData(RankingType rankingType, long hierarchyId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, OtsDataType dataType = OtsDataType.TotalData)
        {
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var columns = GetColumns(rankingType);
            var query = new CostDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Top = top,
                Columns = columns
            };
            return DataService<CostDataQuery, CostDataEntity>.Get(query);
        }

        public static CostDataEntity[] RetrieveSystemDimensionCostData(RankingType rankingType, long hierarchyId, long systemDimensionTemplateItemId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, OtsDataType dataType = OtsDataType.TotalData)
        {
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var query = new CostDataQuery()
            {
                HierarchyId = hierarchyId,
                SystemDimensionItemId = systemDimensionTemplateItemId,
                CommodityId = commodityId,
                Step = aggregationStep,
                StartTime = range.Start,
                EndTime = range.End,
                Top = top,
                Columns = GetColumns(rankingType),
            };
            return DataService<CostDataQuery, CostDataEntity>.Get(query);
        }

        public static EnergyConsumptionDataEntity[] RetrieveHierarchyEnergyConsumptionData(RankingType rankingType, long hierarchyId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, OtsDataType dataType = OtsDataType.TotalData)
        {
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var query = new EnergyConsumptionDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                StartTime = range.Start,
                EndTime = range.End,
                Step = aggregationStep,
                Top = top,
                Columns = GetColumns(rankingType),
            };

            return DataService<EnergyConsumptionDataQuery, EnergyConsumptionDataEntity>.Get(query);
        }

        public static EnergyConsumptionDataEntity[] RetrieveSystemDimensionEnergyConsumptionData(RankingType rankingType, long hierarchyId, long systemDimensionTemplateItemId, long commodityId, TimeRange utcTimeRange, TimeGranularity aggregationStep, PagingOrder pagingOrder = null, OtsDataType dataType = OtsDataType.TotalData)
        {
            var range = utcTimeRange.ToTickRange();
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var query = new EnergyConsumptionDataQuery()
            {
                HierarchyId = hierarchyId,
                SystemDimensionItemId = systemDimensionTemplateItemId,
                CommodityId = commodityId,
                StartTime = range.Start,
                EndTime = range.End,
                Step = aggregationStep,
                Top = top,
                Columns = GetColumns(rankingType),
            };

            return DataService<EnergyConsumptionDataQuery, EnergyConsumptionDataEntity>.Get(query);
        }
        #endregion

        #region IRatioDataDA
        public static TargetBaselineUnitDataEntity[] RetrieveTagRatioTBData(long tbId, TimeRange utcTimeRange, TimeGranularity timeGranularity, PagingOrder pagingOrder, RatioType? ratioType)
        {
            string[] columns = GetColumns(ratioType);
            var top = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var range = utcTimeRange.ToTickRange();
            var query = new TargetBaselineUnitDataQuery()
            {
                TargetBaselineId = tbId,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
                Step = timeGranularity,
                Top = top,
            };
            return DataService<TargetBaselineUnitDataQuery, TargetBaselineUnitDataEntity>.Get(query);
        }
        #endregion


        #region IStandardCoalDataDA
        public static StandardCoalDataEntity[] RetrieveStandardCoalData(long hierarchyId, long commodityId, TimeRange utcTimeRange, TimeGranularity step, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = !unitType.HasValue ? null : GetColumnsWithOrig(unitType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var range = utcTimeRange.ToTickRange();

            var query = new StandardCoalDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                Step = step,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
                Top = topCount,
            };

            return DataService<StandardCoalDataQuery, StandardCoalDataEntity>.Get(query);
        }
        #endregion

        #region IStandardCoalTBDataDA
        public static StandardCoalTargetBaselineDataEntity[] RetrieveStandardCoalTBData(long hierarchyId, long commodityId, TimeRange utcTimeRange, TimeGranularity step, string tbcode = TargetBaselineColumnName.Target, PagingOrder pagingOrder = null, UnitType? unitType = null)
        {
            var columns = !unitType.HasValue ? null : GetColumns(unitType.Value);
            var topCount = pagingOrder == null ? null : (int?)MaxRetrieveRawCount;
            var range = utcTimeRange.ToTickRange();

            var query = new StandardCoalTargetBaselineDataQuery()
            {
                HierarchyId = hierarchyId,
                CommodityId = commodityId,
                TargetBaselineCode = ((int)ConvertTargetBaselineType(tbcode)).ToString(),
                Step = step,
                StartTime = range.Start,
                EndTime = range.End,
                Columns = columns,
                Top = topCount,
            };

            return DataService<StandardCoalTargetBaselineDataQuery, StandardCoalTargetBaselineDataEntity>.Get(query);
        }

        private static TargetBaselineType ConvertTargetBaselineType(string tbcode)
        {
            return tbcode == TargetBaselineColumnName.Target ? TargetBaselineType.Target : TargetBaselineType.Baseline;
        }

        #endregion

        #region ITagDataModifyLogDA
        public static TagDataModifyLogEntity[] RetrieveModifyLog(long tagId, long userId)
        {
            var query = new TagDataModifyLogDataQuery() { TagId = tagId, UserId = userId };
            return DataService<TagDataModifyLogDataQuery, TagDataModifyLogEntity>.Get(query);
        }
        public static void UpdateModifyLog(TagDataModifyLogEntity[] entities)
        {
            DataService<TagDataModifyLogEntity>.Update(entities, null);
        }
        #endregion

    }

}
