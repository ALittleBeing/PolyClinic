namespace PolyClinic.BL.Interface
{
    public interface IMapper<T, U>
    {
        T Map(U u);
        U Map(T t);

    }
}