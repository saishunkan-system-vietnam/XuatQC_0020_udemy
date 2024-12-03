using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class MachineServiceTest
    {
        private readonly IMachineService _machineService;
        public MachineServiceTest()
        {
            _machineService = new MachineService();
        }

        #region Add machine
        /// <summary>
        /// If we supply null machine it should be throw argument null exception
        /// </summary>
        [Fact]
        public void AddMachine_NullMachineParameter()
        {
            // Arrange
            MachineAddRequest machineAddRequest = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _machineService.AddMachine(machineAddRequest);
            });
        }

        /// <summary>
        /// If we supply null machine name it should be throw argument exception
        /// </summary>
        [Fact]
        public void AddMachine_NullMachineName()
        {
            // Arrange
            MachineAddRequest machineAddRequest = new MachineAddRequest() { MachineName = null };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _machineService.AddMachine(machineAddRequest);
            });
        }

        /// <summary>
        /// If we supply properly machine information it should be return valid response with machine ID
        /// </summary>
        [Fact]
        public void AddMachine_ProperMachineInfor()
        {
            // Arrange
            MachineAddRequest machineAddRequest = new MachineAddRequest() { MachineName = "LG Gram 17", MachineDescription = "a product of LG flagship 2021" };

            // Act
            MachineResponse machineResponse_FromAdd = _machineService.AddMachine(machineAddRequest);

            // Assert
            Assert.True(!string.IsNullOrEmpty(machineResponse_FromAdd.MachineID.ToString()));
        }
        #endregion

        #region GetMachine by id
        /// <summary>
        /// If we supply null machineID it should be return null Machine response
        /// </summary>
        [Fact]
        public void GetMachineBuyID_NullID()
        {
            // Arrange
            Guid? machineID = null;

            // Act
            MachineResponse machineResponse = _machineService.GetMachineByID(machineID);

            // Assert
            Assert.Null(machineResponse);

        }

        /// <summary>
        /// If we supply not valid machineID, it should be return null Machine response
        /// </summary>
        [Fact]
        public void GetMachineById_NotValidId()
        {
            // Arrange
            Guid machineID = Guid.NewGuid();

            // Act
            MachineResponse machineResponse = _machineService.GetMachineByID(machineID);

            // Assert
            Assert.Null(machineResponse);
        }

        [Fact]
        public void GetMachienByID_ValidID()
        {
            // Arrange
            // Add machine first
            MachineAddRequest machineAddRequest = new MachineAddRequest() { MachineName = "Apple M1", MachineDescription = "A product of Apple " };

            MachineResponse machineAddResponse = _machineService.AddMachine(machineAddRequest);

            // Act
            MachineResponse machineResponseFromGet = _machineService.GetMachineByID(machineAddResponse.MachineID);

            // Assert
            Assert.True(machineAddResponse.MachineID == machineResponseFromGet.MachineID);
        }

        /// <summary>
        /// If we supply empty of list MachineID it should be return empty machine response list 
        /// </summary>
        [Fact]
        public void GetMachineByMultiID_WhenListIdIsNull()
        {
            // Arrange
            List<Guid> machineIdList = null;

            // Act
            List<MachineResponse> machineResponses = _machineService.GetMachineByIDs(machineIdList);

            // Assert
            Assert.Empty(machineResponses);
        }

        /// <summary>
        /// When we supply valid ids list it should be return machine response with match Ids
        /// </summary>
        [Fact]
        public void GetMachineByMultiID_WhenValidIds()
        {
            // Arrange
            // Add to list
            MachineAddRequest machineAddRequest1 = new MachineAddRequest() { MachineName = "Dell xps", MachineDescription = "a best choice product for dell laptop" };
            MachineAddRequest machineAddRequest2 = new MachineAddRequest() { MachineName = "Lenovo thinkpad", MachineDescription = "a best choice product thin and strong latop" };

            MachineResponse machineResponse1 = _machineService.AddMachine(machineAddRequest1);
            MachineResponse machineResponse2 = _machineService.AddMachine(machineAddRequest2);

            List<Guid> machineIds = new List<Guid>();
            machineIds.Add(machineResponse1.MachineID);
            machineIds.Add(machineResponse2.MachineID);

            List<MachineResponse> expectMachineResponses = new List<MachineResponse> { machineResponse1, machineResponse2 };

            // Act
            List<MachineResponse> actualMachineResponses = _machineService.GetMachineByIDs(machineIds);

            // Assert
            foreach (var expectedmachineResponse in expectMachineResponses)
            {
                Assert.True(expectedmachineResponse.MachineID != Guid.Empty);
                Assert.Contains(expectedmachineResponse, actualMachineResponses);
            }
        }
        #endregion

        #region Get All machine
        /// <summary>
        /// When no machine has added to list it should be return empty list
        /// </summary>
        [Fact]
        public void GetAllMachine_WhenListEmpty()
        {
            // Arrange

            // Act
            List<MachineResponse> machineResponses = _machineService.GetAllMachines();

            // Assert
            Assert.Empty(machineResponses);
        }

        [Fact]
        public void GetAllMachine_ValidResponse()
        {
            // Arrange
            // Add to list
            MachineAddRequest machineAddRequest1 = new MachineAddRequest() { MachineName = "Dell xps", MachineDescription = "a best choice product for dell laptop" };
            MachineAddRequest machineAddRequest2 = new MachineAddRequest() { MachineName = "Lenovo thinkpad", MachineDescription = "a best choice product thin and strong latop" };

            MachineResponse machineResponse1 = _machineService.AddMachine(machineAddRequest1);
            MachineResponse machineResponse2 = _machineService.AddMachine(machineAddRequest2);


            List<MachineResponse> expectMachineResponses = new List<MachineResponse> { machineResponse1, machineResponse2 };

            // Act
            List<MachineResponse> actualMachineResponses = _machineService.GetAllMachines();

            // Assert
            foreach (var expectedmachineResponse in expectMachineResponses)
            {
                Assert.True(expectedmachineResponse.MachineID != Guid.Empty);
                Assert.Contains(expectedmachineResponse, actualMachineResponses);
            }
        }
        #endregion
    }
}
