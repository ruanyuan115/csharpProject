using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PersistentLayer.Mapper;

namespace dotnetProject.Entity
{
    public class ExerciseSetsDetails
    {
        private List<ExerciseSet> exerciseSets;
        private List<int?> scores;

        public ExerciseSetsDetails()
        {
        }

        public ExerciseSetsDetails(List<ExerciseSet> exerciseSets, List<int?> scores)
        {
            this.exerciseSets = exerciseSets;
            this.scores = scores;
        }
    }
}
