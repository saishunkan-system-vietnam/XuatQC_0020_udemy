using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace xUnitFinhub
{
    public class StockServiceTest
    {
        private readonly IStocksService _stockService;
        public StockServiceTest()
        {
            _stockService = new StocksService();
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
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentNullException>(result);

        }

        /// <summary>
        /// When supply quantity of BuyOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_QuantityIsZeroAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 2000;
            buyOrderRquest.Quantity = 0;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);

        }

        /// <summary>
        /// When  supply buyOrderQuantity is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_QuantityIsBiggerThanMaxAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 2000;
            buyOrderRquest.Quantity = 100001;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }

        /// <summary>
        /// When supply price of BuyOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_PriceIsZeroAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 0;
            buyOrderRquest.Quantity = 2;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);

        }

        /// <summary>
        /// When supply price of BuyOrderRequest is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_PriceIsBiggerThanMaxAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 10001;
            buyOrderRquest.Quantity = 2;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }

        /// <summary>
        /// When supply StockSymbo of BuyOrderRequest is blank, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsBlankAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = string.Empty;
            buyOrderRquest.Price = 2000;
            buyOrderRquest.Quantity = 2;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }

        /// <summary>
        /// When supply Order datetime of BuyOrderRequest is smaller than min date, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_OrderDatetimeIsSmallerThanMindateAsync()
        {

            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 2000;
            buyOrderRquest.Quantity = 2;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("1999-11-25");

            // 2.Act
            Task result() => _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        /// </summary>
        [Fact]
        public async Task CreateBuyOrder_SuccessAsync()
        {
            // 1. Arrange
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 2000;
            buyOrderRquest.Quantity = 2;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2. Act
            var result = await _stockService.CreateBuyOrder(buyOrderRquest);

            // 3. Assert
            Assert.NotEqual(Guid.Empty, result.BuyOrderID);
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
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentNullException>(result);

        }

        /// <summary>
        /// When supply quantity of SellOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Fact]
        public async Task CreateSellOrder_QuantityIsZeroAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = "MSFT";
            sellOrderRquest.Price = 2000;
            sellOrderRquest.Quantity = 0;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);

        }

        /// <summary>
        /// When  supply SellOrder Quantity is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateSellOrderQuantityIsBiggerThanMaxAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = "MSFT";
            sellOrderRquest.Price = 2000;
            sellOrderRquest.Quantity = 100001;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }

        /// <summary>
        /// When supply price of SellOrderRequest is 0, it should throw ArgumentException
        /// </summary>
        [Fact]
        public async Task CreateSellOrder_PriceIsZeroAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = "MSFT";
            sellOrderRquest.Price = 0;
            sellOrderRquest.Quantity = 2;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);

        }

        /// <summary>
        /// When supply price of SellOrderRequest is greater than 100000, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateSellOrder_PriceIsBiggerThanMaxAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = "MSFT";
            sellOrderRquest.Price = 10001;
            sellOrderRquest.Quantity = 2;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }

        /// <summary>
        /// When supply StockSymbo of SellOrderRequest is blank, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsBlankAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = string.Empty;
            sellOrderRquest.Price = 2000;
            sellOrderRquest.Quantity = 2;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2.Act
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);
        }

        /// <summary>
        /// When supply Order datetime of SellOrderRequest is smaller than min date, it should throw ArgumentException.
        /// </summary>
        [Fact]
        public async Task CreateSellOrder_OrderDatetimeIsSmallerThanMindateAsync()
        {

            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = "MSFT";
            sellOrderRquest.Price = 2000;
            sellOrderRquest.Quantity = 2;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("1999-11-25");

            // 2.Act
            Task result() => _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            await Assert.ThrowsAsync<ArgumentException>(result);

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        /// </summary>
        [Fact]
        public async Task CreateBuySellOrder_SuccessAsync()
        {
            // 1. Arrange
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
            sellOrderRquest.StockName = "Microsoft";
            sellOrderRquest.StockSymbol = "MSFT";
            sellOrderRquest.Price = 2000;
            sellOrderRquest.Quantity = 2;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            // 2. Act
            var result = await _stockService.CreateSellOrder(sellOrderRquest);

            // 3. Assert
            Assert.NotEqual(Guid.Empty, result.SellOrderID);
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
            Assert.Empty(buyOrderResponses);

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
            BuyOrderRequest buyOrderRquest = new BuyOrderRequest();
            buyOrderRquest.StockName = "Microsoft";
            buyOrderRquest.StockSymbol = "MSFT";
            buyOrderRquest.Price = 2000;
            buyOrderRquest.Quantity = 2;
            buyOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

            BuyOrderRequest buyOrderRquest1 = new BuyOrderRequest();
            buyOrderRquest1.StockName = "Microsoft";
            buyOrderRquest1.StockSymbol = "MSFT";
            buyOrderRquest1.Price = 2000;
            buyOrderRquest1.Quantity = 2;
            buyOrderRquest1.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

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
                Assert.True(buyOrderGetRepsponse.BuyOrderID != Guid.Empty);
                Assert.Contains(buyOrderGetRepsponse, expectedBuyOrderList);

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
            List<SellOrderResponse>  sellOrderResponses = await _stockService.GetSellOrders();

            // 3. Assert
            Assert.Empty(sellOrderResponses);

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
            SellOrderRequest sellOrderRquest = new SellOrderRequest();
           sellOrderRquest.StockName = "Microsoft";
           sellOrderRquest.StockSymbol = "MSFT";
           sellOrderRquest.Price = 2000;
           sellOrderRquest.Quantity = 2;
            sellOrderRquest.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-20");

            SellOrderRequest sellOrderRquest1 = new SellOrderRequest();
            sellOrderRquest1.StockName = "Microsoft";
            sellOrderRquest1.StockSymbol = "MSFT";
            sellOrderRquest1.Price = 4000;
            sellOrderRquest1.Quantity = 2;
            sellOrderRquest1.DateAndTimeOfOrder = Convert.ToDateTime("2023-11-25");

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
                Assert.True(sellOrderGetRepsponse.SellOrderID != Guid.Empty);
                Assert.Contains(sellOrderGetRepsponse, expectedSellOrderList);
            }
        }
        #endregion
    }
}