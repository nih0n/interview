namespace Solution.Application.UseCases
{
    public interface IUseCase<Input, Output>
    {
        Output Execute(Input input);
    }
}
