using System;
using System.Collections.Generic;
using System.Text;
using FrameWork.Domain;


namespace FrameWork.Respository {
    public interface IRespositoryFactory {
        IValveRespository GetValveRespository();
        IPipeRespository GetPipeRespository();
    }

    public interface IValveRespository : IRespository<Valve> {}
    public interface IPipeRespository : IRespository<Pipe> { }
}
