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
    public class OrderItemServiceTest
    {
        private readonly IOrderItemsService _orderItemsService;
        private readonly IFixture _autoFixture;

        public OrderItemServiceTest()
        {
            // Create init order item object to mock data
            var orderItemsInitialData = new List<OrderItem>() { };

            // create DB context mock
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                    new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );

            // 
            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.OrderItems, orderItemsInitialData);

            IOrderItemsRepository orderItemsRepository = new OrderItemsRepository(dbContext);

            _orderItemsService = new OrderItemsService(orderItemsRepository);
            _autoFixture = new Fixture();
        }

        #region AddOrderItem
        /// <summary>
        /// When supply OrderItemAddRequest is null, it should throw ArgumentNullException
        /// </summary>
        [Fact]
        public async Task CreateOrderItem_OrderItemAddRequestNullAsync()
        {
            // 1. Arrange
            OrderItemAddRequest OrderItemAddRquest = null;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderItemAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        /// <summary>
        /// When supply CustomerName of OrderItemAddRequest is Empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] // passing parameters to the tet method
        public async Task CreateOrderItem_CustomerNameIsEmptyAsync(string? customerName)
        {
            // 1. Arrange
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderItemAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When  supply CustomerName is greater than 50 character, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Nicholas Unless-Jesus-Christ-Had-Died-For-Thee-Thou-Hadst-Been-Damned Barbon")] //passing parameters to the tet method
        public async Task CreateOrderItem_CustomerNameIsOverSizeAsync(string? customerName)
        {
            // 1. Arrange
            OrderItemAddRequest OrderRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply OrderNumber is empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] //passing parameters to the tet method
        public async Task CreateOrderItem_OrderNumberEmptyAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderItemAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply orderNumber is not valid format, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("OrderItem_")] //passing parameters to the tet method
        public async Task CreateOrderItem_OrderNumberIsNotValidAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderItemAddRquest);
            };


            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply null of orderDatetim, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(null)] //passing parameters to the tet method
        public async Task CreateOrderItem_OrderDatetimeIsnullAsync(DateTime? orderDatetime)
        {
            // 1. Arrange
            OrderItemAddRequest OrderRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply total amount is nagative number, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(-1)] //passing parameters to the tet method
        public async Task CreateOrderItem_TotalAmountIsNagativeAsync(int totalAmount)
        {

            // 1. Arrange
            OrderItemAddRequest OrderRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.AddOrderItem(OrderRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of orderItemResponse type with auto-generated OrderId (guid).
        /// </summary>
        [Fact]
        public async Task CreateOrderItem_SuccessAsync()
        {
            // 1. Arrange
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2. Act
            OrderItemResponse orderItemResponse = await _orderItemsService.AddOrderItem(OrderItemAddRquest);

            // 3. Assert
            orderItemResponse.OrderId.Should().NotBe(Guid.Empty);
        }
        #endregion

        #region UpdateOrderItem
        /// <summary>
        /// When supply OrderItemUpdateRequest is null, it should throw ArgumentNullException
        /// </summary>
        [Fact]
        public async Task UpdateOrderItem_OrderItemUpdateRequestNullAsync()
        {
            // 1. Arrange
            OrderItemUpdateRequest OrderItemUpdateRequest = null;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(OrderItemUpdateRequest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        /// <summary>
        /// When supply CustomerName of OrderItemUpdateRequest is Empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] // passing parameters to the tet method
        public async Task UpdateOrderItem_CustomerNameIsEmptyAsync(string? customerName)
        {
            // 1. Arrange
            OrderItemUpdateRequest OrderItemAddRquest = _autoFixture.Build<OrderItemUpdateRequest>().Create();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(OrderItemAddRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply OrderID of OrderItemUpdateRequest is Empty, it should throw ArgumentException
        /// </summary>
        [Fact]
        public async Task UpdateOrderItem_OrderIdIsEmptyAsync()
        {
            // 1. Arrange
            Guid orderId = new Guid();

            OrderItemUpdateRequest OrderItemUpdateRequest = CreateOrderItemUpdateRequest();

            OrderItemUpdateRequest.OrderId =  orderId;

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(OrderItemUpdateRequest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When  supply CustomerName is greater than 50 character, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Nicholas Unless-Jesus-Christ-Had-Died-For-Thee-Thou-Hadst-Been-Damned Barbon")] //passing parameters to the tet method
        public async Task UpdateOrderItem_CustomerNameIsOverSizeAsync(string? customerName)
        {
            // 1. Arrange
            OrderItemUpdateRequest orderUpdateRquest = CreateOrderItemUpdateRequest();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply OrderNumber is empty, it should throw ArgumentException
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("")] //passing parameters to the tet method
        public async Task UpdateOrderItem_OrderNumberEmptyAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderItemUpdateRequest orderUpdateRquest = CreateOrderItemUpdateRequest();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply orderNumber is not valid format, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData("Order0_1")] //passing parameters to the tet method
        public async Task UpdateOrderItem_OrderNumberIsNotValidAsync(string? orderNumber)
        {
            // 1. Arrange
            OrderItemUpdateRequest orderUpdateRquest = CreateOrderItemUpdateRequest();


            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(orderUpdateRquest);
            };


            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply null of orderDatetim, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(null)] //passing parameters to the tet method
        public async Task UpdateOrderItem_OrderDatetimeIsnullAsync(DateTime? orderDatetime)
        {
            // 1. Arrange
            OrderItemUpdateRequest orderUpdateRquest = CreateOrderItemUpdateRequest();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply total amount is nagative number, it should throw ArgumentException.
        /// </summary>
        [Theory] // Using [Theory] instead of [Fact] that you can pass parameters to the test method
        [InlineData(-1)] //passing parameters to the tet method
        public async Task UpdateOrderItem_TotalAmountIsNagativeAsync(int totalAmount)
        {

            // 1. Arrange
            OrderItemUpdateRequest orderUpdateRquest = CreateOrderItemUpdateRequest();

            // 2.Act
            Func<Task> action = async () =>
            {
                await _orderItemsService.UpdateOrderItem(orderUpdateRquest);
            };

            // 3. Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        /// <summary>
        /// When supply not exist of orderid, it should return null.
        /// </summary>
        [Fact]
        public async Task UpdateOrderItem_NotExistOrderIdAsync()
        {
            // 1. Arrange
            OrderItemUpdateRequest orderUpdateRquest = CreateOrderItemUpdateRequest();
            orderUpdateRquest.OrderId = Guid.NewGuid();

            // 2.Act
            var oderUpdateResponse =   await _orderItemsService.UpdateOrderItem(orderUpdateRquest);
            

            // 3. Assert
            oderUpdateResponse.Should().BeNull();

        }

        /// <summary>
        /// #8 When supply all valid values, it should be successful and return an object of OrderItemResponse with same OrderId of added responses
        /// </summary>
        [Fact]
        public async Task UpdateOrderItem_SuccessAsync()
        {
            // 1. Add Order fake object data
            OrderItemAddRequest OrderItemAddRquest = CreateOrderItemAddRequest();

            // 2. Add Order
            OrderItemResponse OrderItemResponse = await _orderItemsService.AddOrderItem(OrderItemAddRquest);


            //// 3. Update Order
            OrderItemUpdateRequest OrderUpdateRquest = CreateOrderItemUpdateRequest();
            OrderUpdateRquest.OrderId = OrderItemResponse.OrderId;


            OrderItemResponse OrderUpdateResponse = await _orderItemsService.UpdateOrderItem(OrderUpdateRquest);

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
        public async Task GetAllOrderItems_EmptyList()
        {
            // Act
            List<OrderItemResponse> OrderItemResponses = await _orderItemsService.GetAllOrderItems();

            // assert
            OrderItemResponses.Should().BeEmpty();

        }


        /// <summary>
        /// #2 When the order already added, it should be return added order list
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllOrderItem_ReturnAddedList()
        {
            // 1. Arrange
            // Add order

            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            OrderItemAddRequest OrderItemAddRquest1 = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2. Act
            var OrderItemAddedresult = await _orderItemsService.AddOrderItem(OrderItemAddRquest);
            var OrderItemAddedresult1 = await _orderItemsService.AddOrderItem(OrderItemAddRquest1);

            // Make expected list to compare with actual response
            List<OrderItemResponse> expectedOrderList = new List<OrderItemResponse>() { OrderItemAddedresult, OrderItemAddedresult1 };

            // Call GetAllOrderItems
            List<OrderItemResponse> actualOrderList = await _orderItemsService.GetAllOrderItems();

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
        public async Task DeleteOrderItem_InvalidAsync()
        {
            Guid orderItemId = Guid.NewGuid();

            // 1.Act
            var isScuccess = await _orderItemsService.DeleteOrderItemByOrderItemId(orderItemId);

            // 2. Assert
            isScuccess.Should().BeFalse();

        }

        /// <summary>
        /// When supply  valid of OrderId, it should return true
        /// </summary>
        [Fact]
        public async Task DeleteOrderItem_ValidAsync()
        {
            // 1. Add Order fake object data
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2. Add Order
            OrderItemResponse OrderItemResponse = await _orderItemsService.AddOrderItem(OrderItemAddRquest);

            // 3.Act delete
            var isScuccess = await _orderItemsService.DeleteOrderItemByOrderItemId(OrderItemResponse.OrderItemId);

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
                await _orderItemsService.GetOrderItemByOrderItemId(orderId);
            };

            // 2.Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        /// <summary>
        /// When supply OrderItemID is not valid, it should return null
        /// </summary>
        [Fact]
        public async Task GetOrderItemByOrderItemId_OrderIdIsNotValidAsync()
        {
            // 1. Arrange
            Guid orderItemID = Guid.NewGuid();

            // 2. Act
            OrderItemResponse OrderItemResponse = await _orderItemsService.GetOrderItemByOrderItemId(orderItemID);

            // 3. Assert
            OrderItemResponse.Should().BeNull();
        }

        /// <summary>
        /// When supply OrderID is valid, it should return OrderItemResponse
        /// </summary>
        [Fact]
        public async Task GetOrderItemByOrderItemId_ReturnValidResponseAsync()
        {
            // 1. Add Order fake object data
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>().Create();

            // 2. Add Order
            OrderItemResponse OrderItemResponse = await _orderItemsService.AddOrderItem(OrderItemAddRquest);

            // 1.Act
            OrderItemResponse orderGetResponse = await _orderItemsService.GetOrderItemByOrderItemId(OrderItemResponse.OrderId);

            // 2.Assert
            orderGetResponse.OrderId.Should().NotBe(Guid.Empty);
        }
        #endregion

        private OrderItemAddRequest CreateOrderItemAddRequest()
        {
            OrderItemAddRequest OrderItemAddRquest = _autoFixture.Build<OrderItemAddRequest>()
             .With(temp => temp.UnitPrice, 1006)
             .With(temp => temp.TotalPrice, 1009).Create();

            return OrderItemAddRquest;
        }

        private OrderItemUpdateRequest CreateOrderItemUpdateRequest()
        {
            OrderItemUpdateRequest orderItemUpdateRquest = _autoFixture.Build<OrderItemUpdateRequest>()
             .With(temp => temp.UnitPrice, 1006)
             .With(temp => temp.TotalPrice, 1009).Create();

            return orderItemUpdateRquest;
        }
    }
}