using System.ComponentModel.DataAnnotations;

namespace Pokedex.Infrastructure.Resilience;

public class RetryPolicyOptions
{
    [Required]
    [Range(1, int.MaxValue)]
    public int RetryCount { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double DelaySeconds { get; set; }

    [Required]
    public BackoffStrategy BackoffStrategy { get; set; }
}