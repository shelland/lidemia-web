namespace Lidemia.Core.Models.Service;

public class ResultInfo
{
    public ResultInfo()
    {
        this.IsSuccess = true;
    }

    public ResultInfo(bool isSuccess)
    {
        this.IsSuccess = isSuccess;
    }

    public static ResultInfo FromEntityResult<T>(CreateEntityResultModel<T> result)
    {
        if (!result.IsSuccess)
        {
            return new ErrorResultInfo(result.ErrorCodes);
        }

        return new ResultInfo<T>(true)
        {
            Data = result.Entity
        };
    }

    public bool IsSuccess { get; set; }

    public static ResultInfo Ok { get; } = new();
}

public class ErrorResultInfo<T> : ResultInfo
{
    public ErrorResultInfo(IEnumerable<T> errors)
    {
        this.IsSuccess = false;
        this.Data = new ErrorResponseBody<T>
        {
            Errors = errors
        };
    }

    public ErrorResponseBody<T> Data { get; set; }
}

public class ErrorResultInfo : ResultInfo
{
    public ErrorResultInfo()
    {
        IsSuccess = false;
    }

    public ErrorResultInfo(IEnumerable<string> errors)
    {
        IsSuccess = false;

        this.Data = new ErrorResponseBody<string>
        {
            Errors = errors
        };
    }

    public int Code { get; set; }

    public ErrorResponseBody<string>? Data { get; set; }
}

public class ResultInfo<T> : ResultInfo
{
    public ResultInfo() : base(true)
    {
    }

    public ResultInfo(bool isSuccess) : base(isSuccess)
    {
    }

    public T? Data { get; set; }
}

public class ExtraResultInfo<T> : ResultInfo<T>
{
    public int Code { get; set; }

    public int ExtraCode { get; set; }
}

public class ErrorResponseBody<T>
{
    public IEnumerable<T> Errors { get; set; } = null!;
}