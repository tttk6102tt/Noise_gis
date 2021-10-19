using System;
using System.Collections.Generic;
using System.Text;
using FrameWork.Respository;
using FrameWork.Domain;

namespace FrameWork.Data {
    public class EngineRespositoryFactory : IRespositoryFactory{
        #region IRespositoryFactory Members

        public IValveRespository GetValveRespository() {
            return new ValveRespository();
        }

        public IPipeRespository GetPipeRespository() {
            return new PipeRespository();
        }

        #endregion
    }

    public class ValveRespository : AbsEngineRespository<Valve>, IValveRespository { };
    public class PipeRespository : AbsEngineRespository<Pipe>, IPipeRespository { };
}
