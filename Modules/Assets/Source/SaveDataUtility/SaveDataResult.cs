namespace Modules
{
    public abstract class SaveDataResult
    {
        public readonly bool Success;

        public SaveDataResult(bool success)
        {
            Success = success;
        }
    }

    public class ReadDataResult : SaveDataResult
    {
        public readonly SaveData Data;

        public ReadDataResult(bool success, SaveData data) : base(success)
        {
            Data = data;
        }
    }

    public class WriteDataResult : SaveDataResult
    {
        public WriteDataResult(bool success) : base(success) { }
    }
}
