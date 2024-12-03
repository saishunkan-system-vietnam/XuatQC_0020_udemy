using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Moq;
using EntityFrameworkCoreMock;
using RepositoryContracts;
using Repositories;
using AutoFixture;
using FluentAssertions;
using System;
using static System.Collections.Specialized.BitVector32;

namespace xUnitFinhub
{
    public class StockServiceTest
    {
        private readonly IStocksService _stockService;
        private readonly IFixture _fixture;
        public StockServiceTest()
        {
            var buyOrdersInitialData = new List<BuyOrder>() { };
            var sellOrdersInitialData = new List<SellOrder>() { };

            DbContextMock<TradeOrderDBContext> dbContextMock = new DbContextMock<TradeOrderDBContext>(
                new DbContextOptionsBuilder<TradeOrderDBContext>().Options
                );

            TradeOrderDBContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.BuyOrders, buyOrdersInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.SellOrders, sellOrdersInitialData);

            IStocksRepository stocksRepository = new StocksRepository(dbContext);
            _stockService = new StocksService(stocksRepository);
            _fixture = new Fixture();
        }

        #region createBuyOrder
        /// <summary>
        /// When supply BuyOrderRequest is null, it should throw ArgumentNullException
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_BuyOrderRequestNullAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = null;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        /// <summary>
        /// When supply quantity of BuyOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(0)] // passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityIsZeroAsync(uint buyOrderQuantity)
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, buyOrderQuantity).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When  supply buyOrderQuantity is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_QuantityIsBiggerThanMaxAsync(uint buyOrderQuantity)
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Build<BuyOrderRequest>()
               .With(temp => temp.Quantity, buyOrderQuantity).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply price of BuyOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceIsZeroAsync(uint buyOrderRquestPrice)
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Build<BuyOrderRequest>()
               .With(temp => temp.Price, buyOrderRquestPrice).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply price of BuyOrderRequest is greater than 10000, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateBuyOrder_PriceIsBiggerThanMaxAsync(uint buyOrderRquestPrice)
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Build<BuyOrderRequest>()
               .With(temp => temp.Price, buyOrderRquestPrice).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };


            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply StockSymbo of BuyOrderRequest is blank, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] //passing parameters to the tet method
        public async Task CreateBuyOrder_StockSymbolIsBlankAsync(string StockSymbol)
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Build<BuyOrderRequest>()
               .With(temp => temp.StockSymbol, StockSymbol).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply Order datetime of BuyOrderRequest is smaller than min date, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("1999-11-25")] //passing parameters to the tet method
        public async Task CreateBuyOrder_OrderDatetimeIsSmallerThanMindateAsync(DateTime dateAndTimeOfOrder)
        {

            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Build<BuyOrderRequest>()
               .With(temp => temp.DateAndTimeOfOrder, dateAndTimeOfOrder).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateBuyOrder(buyOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_SuccessAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = _fixture.Create<BuyOrderRequest>();

            // 2. Act
            BuyOrderResponse buyOrderResponse = await _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            buyOrderResponse.BuyOrderID.Should().NotBe(Guid.Empty);
        }
        #endregion

        #region createSellOrder
        /// <summary>
        /// When supply SellOrderRequest is null, it should throw ArgumentNullException
        /// </summary>
        [Fact]
        public async Task createSellOrder_SellOrderRequestNullAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = null;


            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        /// <summary>
        /// When supply quantity of SellOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_QuantityIsZeroAsync(uint sellOrderRquestQuantity)
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Build<SellOrderRequest>()
               .With(temp => temp.Quantity, sellOrderRquestQuantity).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When  supply SellOrder Quantity is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(100001)] //passing parameters to the tet method
        public async Task CreateSellOrderQuantityIsBiggerThanMaxAsync(uint sellOrderRquestQuantity)
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Build<SellOrderRequest>()
               .With(temp => temp.Quantity, sellOrderRquestQuantity).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply price of SellOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(0)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceIsZeroAsync(uint sellOrderRquestPrice)
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Build<SellOrderRequest>()
               .With(temp => temp.Price, sellOrderRquestPrice).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply price of SellOrderRequest is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(10001)] //passing parameters to the tet method
        public async Task CreateSellOrder_PriceIsBiggerThanMaxAsync(uint sellOrderRquestPrice)
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Build<SellOrderRequest>()
               .With(temp => temp.Price, sellOrderRquestPrice).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply StockSymbo of SellOrderRequest is blank, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] //passing parameters to the tet method
        public async Task CreateSellOrder_StockSymbolIsBlankAsync(string stockSymbol)
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Build<SellOrderRequest>()
               .With(temp => temp.StockSymbol, stockSymbol).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply Order datetime of SellOrderRequest is smaller than min date, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("1999-11-25")] //passing parameters to the tet method
        public async Task CreateSellOrder_OrderDatetimeIsSmallerThanMindateAsync(DateTime dateAndTimeOfOrder)
        {

            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Build<SellOrderRequest>()
               .With(temp => temp.DateAndTimeOfOrder, dateAndTimeOfOrder).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _stockService.CreateSellOrder(sellOrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        /// </summary>
        [Fact]
        public async Task CreateSellOrder_SuccessAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = _fixture.Create<SellOrderRequest>();

            // 2. Act
            SellOrderResponse sellOrderResponse = await _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            sellOrderResponse.SellOrderID.Should().NotBe(Guid.Empty);
        }

        #endregion

        #region getAllBuyOrders
        /// <summary>
        /// #1 When the buy order not added yet, it should be return an empty list of buy order
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllBuyOrder_EmptyList()
        {
            // 1. Arrange
            List<BuyOrderResponse> buyOrderResponses;

            // 2. Act
            buyOrderResponses = await _stockService.GetBuyOrders();

            // 3. Assert
            buyOrderResponses.Should().BeEmpty();

        }


        /// <summary>
        /// #2 When the buy order not added yet, it should be return added buy order list
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllBuyOrder_ReturnAddedList()
        {
            // 1. Arrange
            // Add buy order
            BuyOrderRequest buyOrderRquest = _fixture.Create<BuyOrderRequest>();

            BuyOrderRequest buyOrderRquest1 = _fixture.Create<BuyOrderRequest>();

            // 2. Act
            var buyOrderAddedresult = await _stockService.CreateBuyOrder(buyOrderRquest);
            var buyOrderAddedresult1 = await _stockService.CreateBuyOrder(buyOrderRquest1);

            // Make expected list to compare with actual response
            List<BuyOrderResponse> expectedBuyOrderList = new List<BuyOrderResponse>() { buyOrderAddedresult, buyOrderAddedresult1 };

            // Call GetAllBuyOders
            List<BuyOrderResponse> actualBuyOrderList = await _stockService.GetBuyOrders();

            // Loop the actual list return, and check each item is in the expected list
            foreach (var buyOrderGetRepsponse in actualBuyOrderList)
            {
                // 3. Assert
                buyOrderGetRepsponse.BuyOrderID.Should().NotBe(Guid.Empty);
                expectedBuyOrderList.Should().Contain(buyOrderGetRepsponse);

            }

        }
        #endregion

        #region getAllSellOrders
        /// <summary>
        /// When the sell order not added yet, it should be return an empty list of sell order
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllSellOrder_EmptyList()
        {
            // 1. Arrange

            // 2. Act
            List<SellOrderResponse> sellOrderResponses = await _stockService.GetSellOrders();

            // 3. Assert
            sellOrderResponses.Should().BeEmpty();

        }


        /// <summary>
        /// #2 When the sell order not added yet, it should be return added sell order list
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllSellOrder_ReturnAddedList()
        {
            // 1. Arrange
            // Add buy order
            SellOrderRequest sellOrderRquest = _fixture.Create<SellOrderRequest>();

            SellOrderRequest sellOrderRquest1 = _fixture.Create<SellOrderRequest>();

            // 2. Act
            var sellOrderAddedresult = await _stockService.CreateSellOrder(sellOrderRquest);
            var sellOrderAddedresult1 = await _stockService.CreateSellOrder(sellOrderRquest1);

            // Make expected list to compare with actual response
            List<SellOrderResponse> expectedSellOrderList = new List<SellOrderResponse>() { sellOrderAddedresult, sellOrderAddedresult1 };

            // Call GetAllSellOders
            List<SellOrderResponse> actualSellOrderList = await _stockService.GetSellOrders();

            // Loop the actual list return, and check each item is in the expected list
            foreach (var sellOrderGetRepsponse in actualSellOrderList)
            {
                // 3. Assert
                sellOrderGetRepsponse.SellOrderID.Should().NotBe(Guid.Empty);
                expectedSellOrderList.Should().Contain(sellOrderGetRepsponse);
            }
        }
        #endregion
    }
}