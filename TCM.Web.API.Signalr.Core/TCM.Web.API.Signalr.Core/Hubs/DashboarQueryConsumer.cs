//  ******************************************************************
//  *  TCM.Web.Application
//  *  TCM.Web.Application - DashboarQueryHandler.cs
//  ******************************************************************

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonCore.Tcm.Common.Tcm.Logger;
using DashboardMessage;
using FlightControlCore.Consume;
using FlightControlCore.CoreApplication.Message;
using FlightControlCore.Endpoint;
using FlightControlCore.impl;
using FlightControlCore.Message;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using TCM.Web.API.Core.Signalr.GraphMessageTransformer;
using TCM.Web.API.Signalr.Core;

namespace TCM.Web.API.Core.Signalr
{

    public interface IDashboarQueryHandler
    {
        Task RegisterUser(string userName, string userRole, string connectionId);
    }


    public class DashboarQueryConsumer : IDashboarQueryHandler, IConsume<DashboardData>, IConsume<InitMessage>
    {
        private readonly BarChart _barChart;
        private readonly LineChart _lineChart;
        private readonly PieChart _pieChart;
        private readonly SimplePie _simplePie;

        EndpointAddress DashBoardGw { get; set; }
        private readonly IHubContext<DashboardQueryHub> _dashboardQueryHubContext;
        public IConfiguration Configuration { get; set; }
        public  DashboardSettings Settings { get; set; }

        public DashboarQueryConsumer(ILogger log, IFlightControl flightControl, IHubContext<DashboardQueryHub> hubContext, IConfiguration configuration)
        {
            Log = log;
            FlightControl = flightControl;
            Configuration = configuration;
            Settings = Configuration.GetSection("DashboardSettings").Get<DashboardSettings>();
            DashBoardGw = EndpointAddress.Create(Settings.DashboardgatewayQueue);
            _dashboardQueryHubContext = hubContext;
            #region Set_Handler_chain

            _barChart = new BarChart();
            _lineChart = new LineChart();
            _pieChart = new PieChart();
            _simplePie = new SimplePie();

            _barChart.SetSuccessor(_lineChart);
            _lineChart.SetSuccessor(_pieChart);
            _lineChart.SetSuccessor(_simplePie);

            #endregion
        }

        private ILogger Log { get; }
        private IFlightControl FlightControl { get; }

        //public string Name => "DashboardQueries";
        public string Name { get; set; }
        public void Consume(DashboardData applicationsMessage, IMessageContext context)
        {
            PostToWebClient(applicationsMessage);
        }
        private void PostToWebClient(DashboardData applicationsMessage)
        {
            try
            {
                Log.Debug(GetType(), $"Consume DashboardData query id {applicationsMessage.QueryId}");

                var transformedMessage = _barChart.ProcessRequest(applicationsMessage);
                if (transformedMessage != null)
                {
                    Log.Debug(GetType(), $"Writing Dashboard message to browser  {transformedMessage}");
                    _dashboardQueryHubContext.Clients.All.SendAsync("newMessage", transformedMessage);
                }
                else
                {
                    Log.Log(
                        $"Dashboard message not transformed. QueryId {applicationsMessage.QueryId}, Data {applicationsMessage.Data}",
                        "Error", SeverityType.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }



        public void Consume(InitMessage applicationsMessage, IMessageContext context)
        {
            Log.Information(GetType(), $"{GetType().Assembly.GetName()} Consumer init");
            FlightControl.AddConsumer(typeof(DashboardData), this);
            FlightControl.Post(DashBoardGw, new DashBoardRefresh());
        }

        public async Task RegisterUser(string userName, string userRole, string connectionId)
        {
            await _dashboardQueryHubContext.Groups.AddToGroupAsync(connectionId, userRole);
            FlightControl.Post(DashBoardGw, new DashBoardRefresh());
        }
    }
}