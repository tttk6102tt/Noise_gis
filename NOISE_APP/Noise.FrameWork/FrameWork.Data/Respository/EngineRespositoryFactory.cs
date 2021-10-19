using System;
using System.Collections.Generic;
using System.Text;
using FrameWork.Core.DataInterfaces;
using FrameWork.Core.Domain;

namespace FrameWork.Data.Respository
{
    public class EngineRespositoryFactory : IRespositoryFactory
    {
        #region IRespositoryFactory Members

        public IValveRespository GetValveRespository()
        {
            return new ValveRespository();
        }

        public IPipeRespository GetPipeRespository()
        {
            return new PipeRespository();
        }

        public INodeRespository GetNodeRespository()
        {
            return new NodeRespository();
        }

        public IScadaRespository GetScadaRespository()
        {
            return new ScadaRespository();
        }

        public IHydrantRespository GetHydrantRespository()
        {
            return new HydrantRespository();
        }

        public IJointRespository GetJointRespository()
        {
            return new JointRespository();
        }

        public ILeakRespository GetLeakRespository()
        {
            return new LeakRespository();
        }

        public IMeterRespository GetMeterRespository()
        {
            return new MeterRespository();
        }

        public IPumpStationRespository GetPumpStationRespository()
        {
            return new PumpStationRespository();
        }

        public IWCollectorRespository GetWCollectorRespository()
        {
            return new WCollectorRespository();
        }

        public IWPlantRespository GetWPlantRespository()
        {
            return new WPlantRespository();
        }

        public IONhiemDatRepository GetONhiemDatRepository()
        {
            return new ONhiemDatRepository();
        }
        #endregion
    }

    public class ValveRespository : AbsEngineRespository<Valve>, IValveRespository { };
    public class PipeRespository : AbsEngineRespository<Pipe>, IPipeRespository { };
    public class NodeRespository : AbsEngineRespository<Node>, INodeRespository { };
    public class ScadaRespository : AbsEngineRespository<Scada>, IScadaRespository { };
    public class HydrantRespository : AbsEngineRespository<Hydrant>, IHydrantRespository { };
    public class JointRespository : AbsEngineRespository<Joint>, IJointRespository { };
    public class LeakRespository : AbsEngineRespository<Leak>, ILeakRespository { };
    public class MeterRespository : AbsEngineRespository<Meter>, IMeterRespository { };
    public class PumpStationRespository : AbsEngineRespository<PumpStation>, IPumpStationRespository { };
    public class WCollectorRespository : AbsEngineRespository<WCollector>, IWCollectorRespository { };
    public class WPlantRespository : AbsEngineRespository<WPlant>, IWPlantRespository { };
    public class ONhiemDatRepository : AbsEngineRespository<ONhiemDat>, IONhiemDatRepository { };
}

