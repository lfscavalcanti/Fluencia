public abstract class BaseRepositorio
{
    protected readonly FluenciaContexto _contexto;

    protected BaseRepositorio(FluenciaContexto contexto)
    {
        _contexto = contexto;
    }
}