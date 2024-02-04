public enum ResultType
{
    SUCCESS, FAIL, ERROR, ETC
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