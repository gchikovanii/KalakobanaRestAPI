namespace Kalakobana.SharedKernel.Results
{
  public class Result<T>
  {
    public bool Succeeded { get; init; }
    public string Error { get; init; }
    public T ResultValue { get; init; }

    public bool Failed => !Succeeded;

    public static Result<T> Success(T value) =>
        new Result<T> { Succeeded = true, ResultValue = value };

    public static Result<T> Failure(string error) =>
        new Result<T> { Succeeded = false, Error = error };
  }
  public class Result
  {
    public bool Succeeded { get; init; }
    public string Error { get; init; }

    public bool Failed => !Succeeded;
    public static Result Success() =>
        new Result { Succeeded = true };

    public static Result Failure(string error) =>
        new Result { Succeeded = false, Error = error };
  }


}
