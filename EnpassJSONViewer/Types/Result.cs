using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using System;

namespace EnpassJSONViewer.Types
{
    public readonly struct Result<T>
    {
        public bool Success { get; }
        public T Value { get; }
        public Exception Error { get; }

        public Result(T value)
        {
            Success = true;
            Value = value;
            Error = null;
        }

        public Result(Exception error)
        {
            Success = false;
            Value = default;
            Error = error;
        }

        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator Result<T>(Exception error) => new Result<T>(error);

        public override string ToString() => Success ? $"{Value}" : null;
    }
}
