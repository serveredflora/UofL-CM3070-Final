public interface IProgressBar<TValue>
{
    TValue Value { get; set; }
    TValue Range { get; set; }
}
