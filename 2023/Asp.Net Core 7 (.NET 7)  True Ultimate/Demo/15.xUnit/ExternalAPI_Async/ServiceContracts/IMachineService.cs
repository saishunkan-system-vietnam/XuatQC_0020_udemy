using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    /// <summary>
    /// Represent for business logic for manulating machine Entity
    /// </summary>
    public interface IMachineService
    {
        /// <summary>
        /// Add machine to the machine list
        /// </summary>
        /// <param name="machineAddRequest">machine object from request to add</param>
        /// <returns>Machine response just added to the list</returns>
        MachineResponse AddMachine(MachineAddRequest machineAddRequest);

        /// <summary>
        /// Get machine by machineID
        /// </summary>
        /// <param name="machineID">machine id</param>
        /// <returns>Machine response match with ID parameter</returns>
        MachineResponse GetMachineByID(Guid? machineID);

        /// <summary>
        /// Get all machines from the manchine list
        /// </summary>
        /// <returns></returns>
        List<MachineResponse> GetAllMachines();

        /// <summary>
        /// Get machines from the machine list by the machine id list
        /// </summary>
        /// <param name="machineIDs">Machine ids list</param>
        /// <returns>Machine response list</returns>
        List<MachineResponse> GetMachineByIDs(List<Guid> machineIDs);
    }
}
