using PolyClinic.Common.Models;
using PolyClinic.DAL.Models;

namespace PolyClinic.BL.Interface
{
    public interface IMapper<T, U>
    {
        T Map(U u);
        U Map(T t);

    }
}