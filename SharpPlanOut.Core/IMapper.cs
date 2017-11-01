namespace SharpPlanOut.Core
{
    public interface IMapper
    {
        object Evaluate(object obj);

        object Get(string name, object defaultValue = null);
    }
}