public enum ResultType
{
    SUCCESS, FAIL, ERROR, ETC
}
public enum AbilityResultType
{
    SUCCESS, FAIL, ERROR, QUEUE, ETC
}

public struct Result
{
    public ResultType result;
    public string message;
    public object obj;

    public Result(ResultType result, string message = "", object obj = null)
    {
        this.result = result;
        this.message = message;
        this.obj = obj;
    }

    public static Result Success(string message = "", object obj = null) => 
        new(ResultType.SUCCESS, message, obj);
    public static Result Fail(string message, object obj = null) =>
        new(ResultType.FAIL, message, obj);
    public static Result Error(string message, object obj = null) =>
        new(ResultType.ERROR, message, obj);
}
public struct AbilityCastTryResult
{
    public AbilityResultType result;
    public string message;
    public object obj;

    public AbilityCastTryResult(AbilityResultType result, string message = "", object obj = null)
    {
        this.result = result;
        this.message = message;
        this.obj = obj;
    }

    public static AbilityCastTryResult Success(string message = "", object obj = null) =>
        new(AbilityResultType.SUCCESS, message, obj);
    public static AbilityCastTryResult Fail(string message, object obj = null) =>
        new(AbilityResultType.FAIL, message, obj);
    public static AbilityCastTryResult Error(string message, object obj = null) =>
        new(AbilityResultType.ERROR, message, obj);
    public static AbilityCastTryResult Queue(string message, object obj = null) =>
        new(AbilityResultType.QUEUE, message, obj);
}