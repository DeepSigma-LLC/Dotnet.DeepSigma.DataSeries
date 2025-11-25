using DeepSigma.DataSeries.Interfaces;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DeepSigma.DataSeries.DataModels;

/// <summary>
/// Base class for data point features in the data series.
/// </summary>
/// <param name="Value">The value of the data point.</param>
/// <param name="IsRolled">Signifies if the data point has been rolled.</param>
/// <param name="IsSyntheticData">Signifies if the data point is sythetic (i.e., data imputation / interpolation)</param>
public record class Observation(decimal Value, bool IsRolled = false, bool IsSyntheticData = false) : IDataModel<Observation>
{
    
    /// <inheritdoc/>
    public Observation Scale(decimal scalar)
    {
        return new Observation(Value * scalar, IsRolled, IsSyntheticData);
    }

    /// <inheritdoc/>
    public (Observation? result, Exception? error) Add(Observation Item)
    {
        return ComputeWithError(Item, (a, b) => a + b);
    }

    /// <inheritdoc/>
    public (Observation? result, Exception? error) Subtract(Observation Item)
    {
        return ComputeWithError(Item, (a, b) => a - b);
    }

    /// <inheritdoc/>
    public (Observation? result, Exception? error) Multiply(Observation Item)
    {
        return ComputeWithError(Item, (a, b) => a * b);
    }

    /// <inheritdoc/>
    public (Observation? result, Exception? error) Divide(Observation Item)
    {
        if (Item.Value == 0) return (null, new Exception("Cannot divide by zero"));
        return ComputeWithError(Item, (a, b) => a / b);
    }

    private (Observation? result, Exception? error) ComputeWithError(Observation item, Func<decimal, decimal, decimal> operation)
    {
        try
        {
            Observation result = ComputeNew(item, operation);
            return (result, null);
        }
        catch (Exception ex)
        {
            return (null, ex);
        }
    }

    private Observation ComputeNew(Observation Item2, Func<decimal, decimal, decimal> operation)
    {
        decimal valueResult = operation(Value, Item2.Value);
        return new Observation(valueResult, IsRolled || Item2.IsRolled, IsSyntheticData || Item2.IsSyntheticData);
    }
}
