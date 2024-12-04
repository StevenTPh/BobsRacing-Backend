﻿using Bobs_Racing.Interface;
using Bobs_Racing.Models;
using Bobs_Racing.Repositories;

namespace Bobs_Racing.Services
{
    public class RaceService : IRaceService
    {
        public Task<List<RaceAnimal>> ProcessRaceAsync(List<RaceAnimal> starlist)
        {
            throw new NotImplementedException();
        }

    private readonly IRaceAnimalRepository _raceAnimalRepository;
        
        public RaceService(IRaceAnimalRepository raceAnimalRepository)
        {
            _raceAnimalRepository = raceAnimalRepository;
        }
        public async Task<List<RaceAnimal>> ProcessRaceAsync(int raceId, List<Animal> animals)
        {
            var raceResults = new List<RaceAnimal>();
            Random random = new Random();

            foreach (var animal in animals)
            {
                var checkpointSpeeds = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    checkpointSpeeds.Add(random.Next(animal.MinSpeed, animal.MaxSpeed + 1));
                }

                int totalSpeed = checkpointSpeeds.Sum();

                var raceAnimal = new RaceAnimal
                {
                    RaceId = raceId,
                    AnimalId = animal.AnimalId,
                    CheckpointSpeeds = checkpointSpeeds,
                    FinalPosition = totalSpeed
                };

                raceResults.Add(raceAnimal);
            }

            var rankedResults = raceResults
                .OrderByDescending(ra => ra.FinalPosition)
                .ToList();

            for  (int i = 0; i < rankedResults.Count;i++)
            {
                rankedResults[i].FinalPosition = i +1;
            }
            await _raceAnimalRepository.SaveRaceResultsAsync(rankedResults);

            return rankedResults;
        }
    }
}