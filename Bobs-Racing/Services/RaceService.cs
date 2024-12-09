// import RaceAnimal and RaceService interfaces
using Bobs_Racing.Interface;
// import Animal and RaceAnimal model classes
using Bobs_Racing.Models;

namespace Bobs_Racing.Services
{
    //implements RaceService interface into RaceService class
    public class RaceService : IRaceService
    {
    // read-only fild to hold refrence to RaceAnimal Repository-Interface
    // this handles all db operations for RaceAnimal
    private readonly IRaceAthleteRepository _raceAthleteRepository;
        
        // constructor for accepting RaceAnimalRepository interface object
        // this includes "SaveRaceResultsAsync" method
        public RaceService(IRaceAthleteRepository raceAthleteRepository)
        {
            _raceAthleteRepository = raceAthleteRepository;
        }

        /* asynchronous method for processing a race
             Accepts:
                RaceId
                List<animalIds>

             Returns:
                List<RaceAnimal>
                    - int: RaceAnimalId
                    - int: RaceId
                    - int: AnimalId
                    - CheckpointSpeeds []
                    - int: FinalPosition
         */
        public async Task<List<RaceAthlete>> ProcessRaceAsync(int raceId, List<Athlete> athletes)
        {
            // List<> for storing RaceAnimal objects
            var raceResults = new List<RaceAthlete>();
            // Initialize Random instance
            Random random = new Random();

            // iterates over each animal in list of animals recived
            foreach (var athlete in athletes)
            {
                // initialize a List<> for storing 3 different checkpoint speeds for the current animal
                // created as List<> for easier adding elements
                var checkpointSpeeds = new List<int>();
                for (int i = 0; i < 3; i++)
                {
                    // uses random to generate a random speed in a range based on min and max speed
                    // adding +1 to ensure the max value is included.
                    checkpointSpeeds.Add(random.Next((int)athlete.LowestTime, (int)(athlete.FastestTime + 1)));
                }
                // finds the sum of all checkpoint speeds to represent animals overall speed for a race
                int totalSpeed = checkpointSpeeds.Sum();

                // creates a RaceAnimal object for the current animal
                var raceAthlete = new RaceAthlete
                {
                    RaceId = raceId,
                    AthleteId = athlete.AthleteId,
                    // converts a the List<int> to an array
                    //CheckpointSpeeds = checkpointSpeeds.ToArray(),
                    // temporarily sets totalSpeed to FinalPosition
                    FinalPosition = totalSpeed
                };

                // add the current Animal to the raceResults List<>
                raceResults.Add(raceAthlete);
            }

            // sorts raceResults list in descending order based on FinalPosition(totalSpeed)
            // the Animal with the highest speed is number 1
            var rankedResults = raceResults
                .OrderByDescending(ra => ra.FinalPosition)
                .ToList();

            // iterates though rankedResults
            // assignes position based on ranking
            for  (int i = 0; i < rankedResults.Count;i++)
            {
                rankedResults[i].FinalPosition = i +1;
            }
            // uses the RaceAnimal repository to save the ranked results to the db
            // using await to ensure the operation is completed before coninuing
            await _raceAthleteRepository.SaveRaceResultsAsync(rankedResults);

            // returns a List<> of RaceAnimals with final position and heckpoint speed
            return rankedResults;
        }
    }
}