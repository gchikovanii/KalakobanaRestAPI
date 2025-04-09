using Kalakobana.Game.Application.Repos;
using Kalakobana.Game.Domain.Models;
using Kalakobana.SharedKernel.Constants;

namespace Kalakobana.Game.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IValidationRepository _validationRepo;

        public GameService(IValidationRepository validationRepo)
        {
            _validationRepo = validationRepo;
        }

        public async Task<AnswerResponseDTO> SubmitAnswersAsync(AnswerRequestDTO request)
        {
            var result = new AnswerResponseDTO
            {
                RoomId = request.RoomId,
                Answers = new List<PlayerScoresResponseDTO>()
            };

            var allAnswers = request.Answers;

            // List of all categories to check
            var categories = new List<string> {
                KalakobanaDataTables.FirstName.ToString(),KalakobanaDataTables.LastName.ToString(),
                KalakobanaDataTables.City.ToString(),KalakobanaDataTables.Country.ToString(),
                KalakobanaDataTables.Animal.ToString(),KalakobanaDataTables.Plant.ToString(),
                KalakobanaDataTables.Movie.ToString(),KalakobanaDataTables.River.ToString() };

            foreach (var player in allAnswers)
            {
                var score = new PlayerScoresResponseDTO
                {
                    UserId = player.UserId,
                };

                foreach (var category in categories)
                {
                    var answer = GetAnswerByCategory(player, category);

                    if (!string.IsNullOrWhiteSpace(answer) && await _validationRepo.ExistsInTable(category, answer))
                    {
                        int point = CalculatePoints(category, answer, allAnswers, _validationRepo).Result;

                        // Set the score dynamically based on category
                        SetScore(score, category, point);
                    }
                }

                result.Answers.Add(score);
            }

            return result;
        }
        private void SetScore(PlayerScoresResponseDTO score, string category, int point)
        {
            switch (category)
            {
                case "FirstName": score.FirstNamePoint = point; break;
                case "LastName": score.LastNamePoint = point; break;
                case "City": score.CityPoint = point; break;
                case "Country": score.CountryPoint = point; break;
                case "Animal": score.AnimalPoint = point; break;
                case "Movie": score.MoviePoint = point; break;
                case "Plant": score.PlantPoint = point; break;
                case "River": score.RiverPoint = point; break;
            }
        }
        private async Task<int> CalculatePoints(string category, string value, List<PlayerAnswerRequestDTO> allAnswers, IValidationRepository validationRepo)
        {
            // Get all correct values from all players for this category
            var correctAnswers = allAnswers
                .Select(p => new
                {
                    UserId = p.UserId,
                    Value = GetAnswerByCategory(p, category)
                })
                .Where(a => !string.IsNullOrWhiteSpace(a.Value))
                .ToList();

            var correctAnswersInDb = new List<string>();

            foreach (var answer in correctAnswers)
            {
                if (await validationRepo.ExistsInTable(category, answer.Value))
                {
                    correctAnswersInDb.Add(answer.Value);
                }
            }

            if (!correctAnswersInDb.Contains(value))
                return 0; // not valid

            var matchingCount = correctAnswersInDb.Count(v => v == value);
            var distinctCount = correctAnswersInDb.Distinct().Count();

            if (matchingCount == 1)
                return 15;
            if (matchingCount >= 2 && correctAnswersInDb.Distinct().Count() == 1)
                return 5;
            return 10;
        }

        private string GetAnswerByCategory(PlayerAnswerRequestDTO player, string category)
        {
            return category switch
            {
                "FirstName" => player.FirstName,
                "LastName" => player.LastName,
                "City" => player.City,
                "Country" => player.Country,
                "Animal" => player.Animal,
                "Movie" => player.Movie,
                "Plant" => player.Plant,
                "River" => player.River,
                _ => ""
            };
        }
    }
}
