﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace toofz.TestsShared.Tests
{
    class TestingHttpMessageHandlerTests
    {
        [TestClass]
        public class Constructor
        {
            [TestMethod]
            public void ReturnsInstance()
            {
                // Arrange
                var mockInnerHandler = new Mock<HttpMessageHandler>();
                var innerHandler = mockInnerHandler.Object;

                // Act
                var handler = new TestingHttpMessageHandler(innerHandler);

                // Assert
                Assert.IsInstanceOfType(handler, typeof(TestingHttpMessageHandler));
            }
        }

        [TestClass]
        public class TestSendAsync
        {
            [TestMethod]
            public async Task RequestIsNull_ThrowsArgumentNullException()
            {
                // Arrange
                var mockInnerHandler = new Mock<HttpMessageHandler>();
                var innerHandler = mockInnerHandler.Object;
                var handler = new TestingHttpMessageHandler(innerHandler);
                HttpRequestMessage request = null;

                // Act -> Assert
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                {
                    return handler.TestSendAsync(request);
                });
            }

            [TestMethod]
            public async Task CallsSendAsyncOnInnerHandler()
            {
                // Arrange
                var innerHandler = new MockHttpMessageHandler();
                var handler = new TestingHttpMessageHandler(innerHandler);
                HttpRequestMessage request = new HttpRequestMessage();

                // Act
                await handler.TestSendAsync(request);

                // Assert
                Assert.IsTrue(innerHandler.SendAsyncCalled);
            }

            class MockHttpMessageHandler : HttpMessageHandler
            {
                public bool SendAsyncCalled { get; private set; }

                protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                {
                    SendAsyncCalled = true;

                    return Task.FromResult(new HttpResponseMessage());
                }
            }
        }
    }
}
