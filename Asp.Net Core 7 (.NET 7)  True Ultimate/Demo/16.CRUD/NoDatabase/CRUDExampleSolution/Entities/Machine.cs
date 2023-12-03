namespace Entities
{
    /// <summary>
    /// Person model class
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// Machine ID
        /// </summary>
        public  Guid MachineID { get; set; }

        /// <summary>
        /// 設備名
        /// </summary>
        public string? MachineName { get; set; } = null;

        /// <summary>
        /// 設備詳細
        /// </summary>
        public string? MachineDescription { get; set;} = null;
    }
}
