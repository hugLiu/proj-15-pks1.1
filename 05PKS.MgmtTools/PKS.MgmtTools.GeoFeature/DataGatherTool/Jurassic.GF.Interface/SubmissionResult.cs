using System.Collections.Generic;

namespace Jurassic.GF.Interface
{
    public class SubmissionResult
    {
        public List<SubmissionError> Errors { get; set; }

        public int FailedBO { get; set; }

        public int InsertedBO { get; set; }

        public int TotalBO { get; set; }

        public int UpdatedBO { get; set; }
    }
}