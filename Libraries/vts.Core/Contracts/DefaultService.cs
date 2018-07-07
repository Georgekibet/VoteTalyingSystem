using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace vts.Core.Contracts
{
    public interface IDefaultServicesList
    {
        DefaultServices Services { get; set; }
    }

    public class DefaultService
    {
        public static DefaultService New<TContract, TImpl>(int id) where TImpl : TContract
        {
            Type t1 = typeof(TContract);
            Type t2 = typeof(TImpl);
            return new DefaultService(t1, t2, id);
        }

        private DefaultService(Type contract, Type implementation, int id)
        {
            Contract = contract;
            Implementation = implementation;
            Id = id;
        }

        public Type Contract { get; set; }
        public Type Implementation { get; set; }
        public int Id { get; set; }
    }

    [Serializable]
    public class DefaultServiceDuplicateException : Exception
    {


        public DefaultServiceDuplicateException()
        {
        }

        public DefaultServiceDuplicateException(string message) : base(message)
        {
        }

        public DefaultServiceDuplicateException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DefaultServiceDuplicateException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    public class DefaultServices
    {
        public DefaultServices()
        {
            ServiceList = new List<DefaultService>();
        }
        public List<DefaultService> ServiceList { get; set; }
        public DefaultServices Add<TContract, TImpl>(int id = 0) where TImpl : TContract
        {
            var s = DefaultService.New<TContract, TImpl>(id);
            string contract = s.Contract.FullName;
            int count = ServiceList.Count(x => x.Contract.FullName == contract);
            if (count != 0)
            {
                string message = string.Format("Attempt to register duplicate service - {0}", contract);
                throw new DefaultServiceDuplicateException(message);
            }
            ServiceList.Add(s);
            return this;
        }

    }
}
