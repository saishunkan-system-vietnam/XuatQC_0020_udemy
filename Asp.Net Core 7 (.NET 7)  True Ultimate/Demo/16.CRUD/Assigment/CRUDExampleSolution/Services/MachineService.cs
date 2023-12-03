using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class MachineService : IMachineService
    {
        private List<Machine> _machineList;
        public MachineService()
        {
            _machineList = new List<Machine>();
        }

        public MachineResponse AddMachine(MachineAddRequest machineAddRequest)
        {
            // if machine add request is null throw argument null exception
            if (machineAddRequest == null) throw new ArgumentNullException(nameof(MachineAddRequest));

            // if machine name is null throw argument exception
            if (machineAddRequest.MachineName == null) throw new ArgumentException("Machine name can not be blank");

            Machine machine = machineAddRequest.ToMachine();
            machine.MachineID = Guid.NewGuid();

            _machineList.Add(machine);

            return machine.ToMachineResponse();
        }

        public List<MachineResponse> GetAllMachines()
        {
            List<MachineResponse> machineResponses = new List<MachineResponse>();
            foreach (Machine machine in _machineList)
            {
                machineResponses.Add(machine.ToMachineResponse());
            }

            return machineResponses;
        }

        public MachineResponse? GetMachineByID(Guid? machineID)
        {
            if (machineID == null) return null;
            
            Machine? machine = _machineList.FirstOrDefault(item => item.MachineID == machineID);

            if (machine == null) return null;

            return machine.ToMachineResponse();
        }

        public List<MachineResponse> GetMachineByIDs(List<Guid> machineIDs)
        {
            List<MachineResponse> machineResponses = new List<MachineResponse>();

            if (machineIDs == null)
            {
                return machineResponses;
            }

            MachineResponse? machineResponse = null;
            foreach (Guid machineID in machineIDs) 
            {
                machineResponse = this.GetMachineByID(machineID);
                if (machineResponse != null) { machineResponses.Add(machineResponse); }
            }

            return machineResponses;
        }
    }
}
