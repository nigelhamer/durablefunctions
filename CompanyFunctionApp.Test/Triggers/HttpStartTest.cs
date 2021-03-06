﻿namespace CompanyFunctionApp.Test.Triggers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using CompanyFunctionApp.Triggers;

    using Microsoft.Azure.WebJobs;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    public class HttpStartTests
    {
        [Fact]
        public async Task HttpStart_returns_retryafter_header()
        {
            // Define constants
            const string functionName = "SampleFunction";
            const string instanceId = "7E467BDB-213F-407A-B86A-1954053D3C24";

            // Mock TraceWriter
            var loggerMock = new Mock<ILogger>();

            // Mock DurableOrchestrationClientBase
            var durableOrchestrationClientBaseMock = new Mock<DurableOrchestrationClientBase>();

            // Mock StartNewAsync method
            durableOrchestrationClientBaseMock.Setup(x => x.StartNewAsync(functionName, It.IsAny<object>()))
                .ReturnsAsync(instanceId);

            // Mock CreateCheckStatusResponse method
            durableOrchestrationClientBaseMock
                .Setup(x => x.CreateCheckStatusResponse(It.IsAny<HttpRequestMessage>(), instanceId)).Returns(
                    new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(string.Empty),
                        });

            // Call Orchestration trigger function
            var result = await HttpStart.Run(
                             new HttpRequestMessage()
                                 {
                                     Content = new StringContent(
                                         "{}",
                                         Encoding.UTF8,
                                         "application/json"),
                                     RequestUri = new Uri(
                                         "http://localhost:7071/orchestrators/E1_HelloSequence"),
                                 },
                             durableOrchestrationClientBaseMock.Object,
                             functionName,
                             loggerMock.Object);

            // Validate that output is not null
            Assert.NotNull(result.Headers.RetryAfter);

            // Validate output's Retry-After header value
            Assert.Equal(TimeSpan.FromSeconds(10), result.Headers.RetryAfter.Delta);
        }
    }
}