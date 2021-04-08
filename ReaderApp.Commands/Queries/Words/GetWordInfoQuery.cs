using MediatR;
using ReaderApp.Data.DTOs.Word;
using ReaderApp.Data.Exceptions;
using ReaderApp.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderApp.Commands.Queries.Words
{
    public class GetWordInfoQuery : IRequest<WordInfoDto>
    {
        public string Word { get; set; }
    }

    public class GetWordInfoHandler : IRequestHandler<GetWordInfoQuery, WordInfoDto>
    {
        private readonly ITranslationProvider _translationProvider;
        private readonly IWordDefinitionProvider _definitionProvider;
        private readonly ILemmatizer _lemmatizer;

        public GetWordInfoHandler(
            ITranslationProvider translationProvider,
            IWordDefinitionProvider definitionProvider,
            ILemmatizer lemmatizer) => (_translationProvider, _definitionProvider, _lemmatizer) = (translationProvider, definitionProvider, lemmatizer);

        public async Task<WordInfoDto> Handle(GetWordInfoQuery request, CancellationToken cancellationToken)
        {
            ValidateWord(request.Word);

            var lemma = await _lemmatizer.Lemmatize(request.Word);
            var definitionsRequest = _definitionProvider.GetDefintitions(request.Word);
            var translattionsRequest = _translationProvider.GetTranslations(request.Word);

            var allDefinitions = new List<string>(await definitionsRequest);
            if (lemma != request.Word)
                allDefinitions.AddRange(await _definitionProvider.GetDefintitions(lemma));

            return new WordInfoDto
            {
                Definitions = allDefinitions,
                Translations = await translattionsRequest,
                Lemma = lemma
            };
        }

        private void ValidateWord(string word)
        {
            if (word.Split(' ').Length != 1)
                throw new BadRequestException("Provided input contains more than a single word");

            if (!word.All(char.IsLetter))
                throw new BadRequestException("Provided word contains non letter symbols");
        }
    }
}
