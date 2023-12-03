using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class return type for most of MachineService
    /// </summary>
    public class MachineResponse
    {
        /// <summary>
        /// Machine ID
        /// </summary>
        public Guid MachineID { get; set; }

        /// <summary>
        /// 設備名
        /// </summary>
        public string? MachineName { get; set; } = null;

        /// <summary>
        /// 設備詳細
        /// </summary>
        public string? MachineDescription { get; set; } = null;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(MachineResponse)) return false;

            MachineResponse machineResponse = (MachineResponse)obj;

            return MachineID == machineResponse.MachineID && 
                MachineName == machineResponse.MachineName && 
                MachineDescription == machineResponse.MachineDescription;
        }
    }

    public static class MachineExtensions
    {
        /// <summary>
        /// Function to convert from Machine to response
        /// </summary>
        /// <param name="machine">Current machine object</param>
        /// <returns>Machine response</returns>
        public static MachineResponse ToMachineResponse(this Machine machine)
        {
            return new MachineResponse
            {
                MachineID = machine.MachineID,
                MachineName = machine.MachineName,
                MachineDescription = machine.MachineDescription
            };
        }

    }
}
