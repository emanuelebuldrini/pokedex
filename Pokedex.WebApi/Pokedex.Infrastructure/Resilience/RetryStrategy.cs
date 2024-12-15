namespace Pokedex.Infrastructure.Resilience;

public enum BackoffStrategy
{
    Linear,
    Exponential,
    Constant
}
