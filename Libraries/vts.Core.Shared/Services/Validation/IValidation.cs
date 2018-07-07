namespace vts.Shared.Services
{
    public interface IValidation<T> where T : class
    {
        ValidationResultInfo Validate(T itemToValidate);
    }
}
