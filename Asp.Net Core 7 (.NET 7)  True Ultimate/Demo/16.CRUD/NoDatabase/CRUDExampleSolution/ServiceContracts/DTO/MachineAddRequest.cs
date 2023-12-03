using Entities;

namespace ServiceContracts.DTO
{
    public class MachineAddRequest
    {
        /// <summary>
        /// 設備名
        /// </summary>
        public string? MachineName { get; set; } = null;

        /// <summary>
        /// 設備詳細
        /// </summary>
        public string? MachineDescription { get; set; } = null;

        public Machine ToMachine()
        {
            return new Machine { MachineName = MachineName, MachineDescription = MachineDescription };
        }
    }
}
