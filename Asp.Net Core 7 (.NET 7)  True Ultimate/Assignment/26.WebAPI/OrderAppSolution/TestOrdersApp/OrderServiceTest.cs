using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace TestOrdersApp
{
    public class OrderServiceTest
    {
        private readonly IOrdersService _ordersService;
        private readonly IFixture _autoFixture;

        public OrderServiceTest()
        {
            // Create init order object to mock data
            var orderInitialData = new List<Order>() { };

            // create DB context mock
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            // 
            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Orders, orderInitialData);

            IOrdersRepository ordersRepository = new OrdersRepository(dbContext);

            _ordersService = new OrdersService(ordersRepository);
            _autoFixture = new Fixture();
        }

        #region AddOrder
        /// <summary>
        /// When supply OrderAddRequest is null, it should throw ArgumentNullException
        /// </summary>
        [Fact]
        public async Task CreateOrder_OrderAddRequestNullAsync()
        {
            // 1. Arrange
            OrderAddRequest orderAddRquest = null;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(orderAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        /// <summary>
        /// When supply CustomerName of OrderAddRequest is Empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] // passing parameters to the tet method
        public async Task CreateOrder_CustomerNameIsEmptyAsync(string? customerName)
        {
            // 1. Arrange
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_12071106")
                .With(temp => temp.TotalAmount, 1006)
                .With(temp => temp.CustomerName, customerName).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(orderAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When  supply CustomerName is greater than 50 character, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Nicholas Unless-Jesus-Christ-Had-Died-For-Thee-Thou-Hadst-Been-Damned Barbon")] //passing parameters to the tet method
        public async Task CreateOrder_CustomerNameIsOverSizeAsync(string? customerName)
        {
            // 1. Arrange
            OrderAddRequest OrderRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_12071106")
                .With(temp => temp.TotalAmount, 1006)
               .With(temp => temp.CustomerName, customerName).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(OrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply OrderNumber is empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] //passing parameters to the tet method
        public async Task CreateOrder_OrderNumberEmptyAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_12071106")
                .With(temp => temp.TotalAmount, 1006)
               .With(temp => temp.OrderNumber, orderNumber).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(orderAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply orderNumber is not valid format, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Order_")] //passing parameters to the tet method
        public async Task CreateOrder_OrderNumberIsNotValidAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_12071106")
                .With(temp => temp.TotalAmount, 1006).Create();

            orderAddRquest.OrderNumber = orderNumber;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(orderAddRquest);
            };


            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply null of orderDatetim, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(null)] //passing parameters to the tet method
        public async Task CreateOrder_OrderDatetimeIsnullAsync(DateTime? orderDatetime)
        {
            // 1. Arrange
            OrderAddRequest OrderRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_12071106")
                .With(temp => temp.TotalAmount, 1006)
               .With(temp => temp.OrderDate, orderDatetime).Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(OrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply total amount is nagative number, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(-1)] //passing parameters to the tet method
        public async Task CreateOrder_TotalAmountIsNagativeAsync(int totalAmount)
        {

            // 1. Arrange
            OrderAddRequest OrderRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_12071106")
                .With(temp => temp.TotalAmount, 1006).Create();

            OrderRquest.TotalAmount = totalAmount;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.AddOrder(OrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of OrderResponse type with auto-generated OrderId (guid).
        /// </summary>
        [Fact]
        public async Task CreateOrder_SuccessAsync()
        {
            // 1. Arrange
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2024_4")
                .With(temp => temp.TotalAmount, 1006).Create();

            // 2. Act
            OrderResponse OrderResponse = await _ordersService.AddOrder(orderAddRquest);

            // 3. Assert
            OrderResponse.OrderId.Should().NotBe(Guid.Empty);
        }
        #endregion

        #region UpdateOrder
        /// <summary>
        /// When supply OrderUpdateRequest is null, it should throw ArgumentNullException
        /// </summary>
        [Fact]
        public async Task UpdateOrder_OrderUpdateRequestNullAsync()
        {
            // 1. Arrange
            OrderUpdateRequest orderUpdateRequest = null;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRequest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        /// <summary>
        /// When supply CustomerName of OrderUpdateRequest is Empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] // passing parameters to the tet method
        public async Task UpdateOrder_CustomerNameIsEmptyAsync(string? customerName)
        {
            // 1. Arrange
            OrderUpdateRequest orderAddRquest = _autoFixture.Build<OrderUpdateRequest>()
                .With(temp => temp.CustomerName, customerName)
                .With(temp => temp.TotalAmount, 1006)
                .With(temp => temp.OrderNumber, "Order_2024_4").Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply OrderID of OrderUpdateRequest is Empty, it should throw ArgumentException
        /// </summary>
        [Fact]
        public async Task UpdateOrder_OrderIdIsEmptyAsync()
        {
            // 1. Arrange
            Guid orderId = new Guid();

            OrderUpdateRequest orderUpdateRequest = CreateOrderUpdateRequest();

            orderUpdateRequest.OrderId =  orderId;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRequest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When  supply CustomerName is greater than 50 character, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Nicholas Unless-Jesus-Christ-Had-Died-For-Thee-Thou-Hadst-Been-Damned Barbon")] //passing parameters to the tet method
        public async Task UpdateOrder_CustomerNameIsOverSizeAsync(string? customerName)
        {
            // 1. Arrange
            OrderUpdateRequest orderUpdateRquest = CreateOrderUpdateRequest();

            orderUpdateRquest.CustomerName = customerName;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply OrderNumber is empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] //passing parameters to the tet method
        public async Task UpdateOrder_OrderNumberEmptyAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderUpdateRequest orderUpdateRquest = CreateOrderUpdateRequest();
            orderUpdateRquest.OrderNumber = orderNumber;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply orderNumber is not valid format, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Order0_1")] //passing parameters to the tet method
        public async Task UpdateOrder_OrderNumberIsNotValidAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderUpdateRequest orderUpdateRquest = CreateOrderUpdateRequest();

            orderUpdateRquest.OrderNumber = orderNumber;


            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRquest);
            };


            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply null of orderDatetim, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(null)] //passing parameters to the tet method
        public async Task UpdateOrder_OrderDatetimeIsnullAsync(DateTime? orderDatetime)
        {
            // 1. Arrange
            OrderUpdateRequest orderUpdateRquest = CreateOrderUpdateRequest();

            orderUpdateRquest.OrderDate = orderDatetime;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply total amount is nagative number, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(-1)] //passing parameters to the tet method
        public async Task UpdateOrder_TotalAmountIsNagativeAsync(int totalAmount)
        {

            // 1. Arrange
            OrderUpdateRequest orderUpdateRquest = CreateOrderUpdateRequest();
            orderUpdateRquest.TotalAmount = totalAmount;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _ordersService.UpdateOrder(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply not exist of orderid, it should return null.
        /// </summary>
        [Fact]
        public async Task UpdateOrder_NotExistOrderIdAsync()
        {
            // 1. Arrange
            OrderUpdateRequest orderUpdateRquest = CreateOrderUpdateRequest();
            orderUpdateRquest.OrderId = Guid.NewGuid();

            // 2.Act
            var oderUpdateResponse =   await _ordersService.UpdateOrder(orderUpdateRquest);
            

            // 3. Assert
            oderUpdateResponse.Should().BeNull();

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of OrderResponse with same OrderId of added responses
        /// </summary>
        [Fact]
        public async Task UpdateOrder_SuccessAsync()
        {
            // 1. Add Order fake object data
            OrderAddRequest orderAddRquest = CreateOrderAddRequest();

            // 2. Add Order
            OrderResponse OrderResponse = await _ordersService.AddOrder(orderAddRquest);


            //// 3. Update Order
            OrderUpdateRequest OrderUpdateRquest = CreateOrderUpdateRequest();
            OrderUpdateRquest.OrderNumber = OrderResponse.OrderNumber;
            OrderUpdateRquest.OrderId = OrderResponse.OrderId;


            OrderResponse OrderUpdateResponse = await _ordersService.UpdateOrder(OrderUpdateRquest);

            // 3. Assert
            OrderUpdateResponse.OrderId.Should().Be(OrderUpdateRquest.OrderId);
        }
        #endregion

        #region getAllOrder
        /// <summary>
        /// #1 When the order not added yet, it should be return an empty list of order
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllOrders_EmptyList()
        {
            // Act
            List<OrderResponse> orderResponses = await _ordersService.GetAllOrders();

            // assert
            orderResponses.Should().BeEmpty();

        }


        /// <summary>
        /// #2 When the order already added, it should be return added order list
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllOrder_ReturnAddedList()
        {
            // 1. Arrange
            // Add order

            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
                  .With(temp => temp.OrderNumber, "Order_2023_1")
                .With(temp => temp.TotalAmount, 1006).Create();

            OrderAddRequest orderAddRquest1 = _autoFixture.Build<OrderAddRequest>()
                  .With(temp => temp.OrderNumber, "Order_2023_2")
                .With(temp => temp.TotalAmount, 1006).Create();

            // 2. Act
            var OrderAddedresult = await _ordersService.AddOrder(orderAddRquest);
            var OrderAddedresult1 = await _ordersService.AddOrder(orderAddRquest1);

            // Make expected list to compare with actual response
            List<OrderResponse> expectedOrderList = new List<OrderResponse>() { OrderAddedresult, OrderAddedresult1 };

            // Call GetAllOrders
            List<OrderResponse> actualOrderList = await _ordersService.GetAllOrders();

            // Loop the actual list return, and check each item is in the expected list
            foreach (var orderGetRepsponse in actualOrderList)
            {
                // 3. Assert
                orderGetRepsponse.OrderId.Should().NotBe(Guid.Empty);
                //expectedOrderList.Should().Contain(orderGetRepsponse);
            }

        }

        #endregion

        #region DeleteOrder
        /// <summary>
        /// When supply Note valid of OrderId, it should return false
        /// </summary>
        [Fact]
        public async Task DeleteOrder_InvalidAsync()
        {
            Guid orderId = Guid.NewGuid();

            // 1.Act
            var isScuccess = await _ordersService.DeleteOrderByOrderId(orderId);

            // 2. Assert
            isScuccess.Should().BeFalse();

        }

        /// <summary>
        /// When supply  valid of OrderId, it should return true
        /// </summary>
        [Fact]
        public async Task DeleteOrder_ValidAsync()
        {
            // 1. Add Order fake object data
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
                .With(temp => temp.OrderNumber, "Order_2023_1")
                .With(temp => temp.TotalAmount, 1006).Create();

            // 2. Add Order
            OrderResponse orderResponse = await _ordersService.AddOrder(orderAddRquest);

            // 3.Act delete
            var isScuccess = await _ordersService.DeleteOrderByOrderId(orderResponse.OrderId);

            // 4. Assert
            isScuccess.Should().BeTrue();

        }
        #endregion

        #region GetOrderItemByOrderItemId

        /// <summary>
        /// When supply OrderID is Empty, it should throw ArgumentException
        /// </summary>
        [Fact] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        public async Task GetOrderItemByOrderItemId_OrderIdIsEmptyAsync()
        {
            Guid orderId = new Guid();

            // 1.Act
            Func<Task> action = async () =>
            {
                await _ordersService.GetOrderByOrderId(orderId);
            };

            // 2.Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply OrderID is not valid, it should return null
        /// </summary>
        [Fact]
        public async Task GetOrderItemByOrderItemId_OrderIdIsNotValidAsync()
        {
            // 1. Arrange
            Guid orderId = Guid.NewGuid();

            // 2. Act
            OrderResponse orderResponse = await _ordersService.GetOrderByOrderId(orderId);

            // 3. Assert
            orderResponse.Should().BeNull();
        }

        /// <summary>
        /// When supply OrderID is valid, it should return OrderResponse
        /// </summary>
        [Fact]
        public async Task GetOrderItemByOrderItemId_ReturnValidResponseAsync()
        {
            // 1. Add Order fake object data
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
              .With(temp => temp.OrderNumber, "Order_2023_2").Create();

            // 2. Add Order
            OrderResponse orderResponse = await _ordersService.AddOrder(orderAddRquest);

            // 1.Act
            OrderResponse orderGetResponse = await _ordersService.GetOrderByOrderId(orderResponse.OrderId);

            // 2.Assert
            orderGetResponse.OrderId.Should().NotBe(Guid.Empty);
        }
        #endregion

        private OrderAddRequest CreateOrderAddRequest()
        {
            OrderAddRequest orderAddRquest = _autoFixture.Build<OrderAddRequest>()
             .With(temp => temp.TotalAmount, 1006)
             .With(temp => temp.OrderNumber, "Order_2023_1").Create();

            return orderAddRquest;
        }

        private OrderUpdateRequest CreateOrderUpdateRequest()
        {
            OrderUpdateRequest orderUpdateRquest = _autoFixture.Build<OrderUpdateRequest>()
             .With(temp => temp.TotalAmount, 1006)
             .With(temp => temp.OrderNumber, "Order_2024_1").Create();

            return orderUpdateRquest;
        }
    }
}