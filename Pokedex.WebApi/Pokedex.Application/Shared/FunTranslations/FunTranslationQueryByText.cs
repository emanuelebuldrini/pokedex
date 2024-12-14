using JewelArchitecture.Core.Application.Queries;
using Pokedex.Domain.Shared;

namespace Pokedex.Application.Shared.FunTranslations;

public record FunTranslationQueryByText(string Text, FunTranslation TranslationType, string CacheKey): IQuery;
