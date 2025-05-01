



namespace RestApiPractice.LogicLayer
{

    public interface ILoginLogic<TRequest, TResponse>
    {
        Task<TResponse> LoginAsync(TRequest req);
    }

}