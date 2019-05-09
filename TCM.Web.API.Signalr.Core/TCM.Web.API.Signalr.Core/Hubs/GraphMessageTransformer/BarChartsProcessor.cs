using System;
using Unity;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using CommonCore.Tcm.Common.Tcm.Logger;
using CommonCore.Tcm.Common.UnityContainer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using DashboardMessage;

namespace TCM.Web.API.Core.Signalr.GraphMessageTransformer
{
    public class BarChartsProcessor
    {
        public BarChartsProcessor() { }
        public DashboardData Process(DashboardData dashboardData)
        {
            var log = UnityHelper.Container.Resolve<ILogger>();

            dynamic dashboardDataDynamic = new ExpandoObject();
            var barChartObject = new ChartObject();
            DashboardData transformedDashBoardData = null;

            #region Private 
            var queryMetaData = new QueryMetaData();
            var rounding = AmountRounding.None;
            #endregion Private


            for (var index = 0; index < dashboardData.MetaDictionary.Length; index++)
            {
                var keyValue = dashboardData.MetaDictionary[index];
                switch (keyValue[0].Trim())
                {
                    case "xAxis":
                        queryMetaData.XAxis = keyValue[1];
                        break;
                    case "Name":
                        queryMetaData.Name = keyValue[1];
                        break;
                    case "yAxis":
                        queryMetaData.YAxis = keyValue[1];
                        break;
                    case "splitBy":
                        queryMetaData.SplitBy = keyValue[1];
                        break;
                    case "Legend1":
                    case "Legend2":
                        queryMetaData.Legends.Add(keyValue[1]);
                        break;
                    case "yAxisLabel":
                        queryMetaData.YAxisLabel = keyValue[1];
                        break;
                    case "xAxisLabel":
                        queryMetaData.XAxisLabel = keyValue[1];
                        break;
                    case "AmountRounding":
                        Enum.TryParse(keyValue[1], out rounding);
                        queryMetaData.RoundingScheme = rounding;
                        break;
                    default:
                        break;
                }
            }

            try
            {
                log.Debug(GetType(), $"QueryId: {dashboardData.QueryId} -> {dashboardData.Data}");

                dynamic obj = JsonConvert.DeserializeObject<ExpandoObject>(dashboardData.Data,
                    new ExpandoObjectConverter());
                if (obj.Rows.Count == 0) return null;

                for (var index = 0; index < obj.Rows.Count; index++)
                {
                    object xAxisValue;
                    object yAxisValue;
                    object splitByValue;

                    var row = (IDictionary<string, object>)obj.Rows[index];
                    row.TryGetValue(queryMetaData.XAxis, out xAxisValue);

                    row.TryGetValue(queryMetaData.YAxis, out yAxisValue);
                    row.TryGetValue(queryMetaData.SplitBy, out splitByValue);

                    var identifier = splitByValue != null ? ((string)splitByValue).Trim() : queryMetaData.YAxis.Trim();

                    var bObj = barChartObject.BarChartData.Find(s => s.label == identifier);
                    if (bObj == null)
                    {
                        bObj = new ChartData()
                        {
                            label = identifier
                        };
                        barChartObject.BarChartData.Add(bObj);
                    }
                    bObj.data.Add(FormatNumber(queryMetaData.RoundingScheme, yAxisValue));
                    DateTime res;
                    if (DateTime.TryParse(xAxisValue.ToString(), out res))
                    {
                        xAxisValue = res.ToShortDateString();
                    }

                    if (!barChartObject.Labels.Contains(xAxisValue.ToString()))
                        barChartObject.Labels.Add(xAxisValue.ToString());
                }
                barChartObject.Options = new ChartOptions()
                {
                    responsive = true
                };
                barChartObject.Legend = true;
                barChartObject.ChartType = dashboardData.ViewType == DashboardViewType.LineChart ? "line" : "bar";

                dashboardDataDynamic.BarChartObject = barChartObject;

                if (!string.IsNullOrEmpty(queryMetaData.Name))
                    dashboardDataDynamic.Name = queryMetaData.Name;

                if (queryMetaData.Legends.Count > 0)
                    dashboardDataDynamic.Legends = queryMetaData.Legends;

                if (!string.IsNullOrEmpty(queryMetaData.YAxisLabel))
                    dashboardDataDynamic.YAxisLabel = queryMetaData.YAxisLabel;

                if (!string.IsNullOrEmpty(queryMetaData.XAxisLabel))
                    dashboardDataDynamic.XAxisLabel = queryMetaData.XAxisLabel;

                var jsonString = JsonConvert.SerializeObject(dashboardDataDynamic);
                log.Debug(GetType(), $"{dashboardData.QueryId}, transformed data: {jsonString}");

                transformedDashBoardData = new DashboardData
                {
                    Description = dashboardData.Description,
                    QueryId = dashboardData.QueryId,
                    ViewRole = dashboardData.ViewRole,
                    ViewType = dashboardData.ViewType,
                    Data = jsonString
                };
            }
            catch (Exception exception)
            {
                log.Debug(GetType(), $"Exception when processing ueryId: {dashboardData.QueryId}, {exception.GetType().Name}");
                log.Exception(exception);
                return null;
            }
            return transformedDashBoardData;
        }
        private decimal FormatNumber(AmountRounding rounding, object yAxisValue)
        {
            decimal _in = Convert.ToDecimal(yAxisValue);
            if (rounding != AmountRounding.None)
            {
                switch (rounding)
                {
                    case AmountRounding.HTH:
                        _in = _in / 100000;
                        break;
                    case AmountRounding.TH:
                        _in = _in / 1000;
                        break;
                    case AmountRounding.ML:
                        _in = _in / 1000000;
                        break;
                }
            }
            return _in;
        }
    }
}