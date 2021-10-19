using System;
using System.Collections.Generic;
using System.Text;
using FrameWork.Core.Domain;


namespace FrameWork.Core.DataInterfaces {
    public interface IRespositoryFactory {
        IValveRespository GetValveRespository();
        IPipeRespository GetPipeRespository();
        INodeRespository GetNodeRespository();
        IScadaRespository GetScadaRespository();
        IHydrantRespository GetHydrantRespository();
        IJointRespository GetJointRespository();
        ILeakRespository GetLeakRespository();
        IMeterRespository GetMeterRespository();
        IPumpStationRespository GetPumpStationRespository();
        IWCollectorRespository GetWCollectorRespository();
        IWPlantRespository GetWPlantRespository();
        IONhiemDatRepository GetONhiemDatRepository();
    }

    public interface IValveRespository : IRespository<Valve> {}
    public interface IPipeRespository : IRespository<Pipe> { }
    public interface INodeRespository : IRespository<Node> { }
    public interface IScadaRespository : IRespository<Scada> { }
    public interface IHydrantRespository : IRespository<Hydrant> { }
    public interface IJointRespository : IRespository<Joint> { }
    public interface ILeakRespository : IRespository<Leak> { }
    public interface IMeterRespository : IRespository<Meter> { }
    public interface IPumpStationRespository : IRespository<PumpStation> { }
    public interface IWCollectorRespository : IRespository<WCollector> { }
    public interface IWPlantRespository : IRespository<WPlant> { }
    public interface IONhiemDatRepository : IRespository<ONhiemDat> { }
    
}
