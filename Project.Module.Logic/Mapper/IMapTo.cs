using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Module.lookup.Mapper
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T));
    }
}
