﻿using Qalsql.Models.Db;

namespace Qalsql.Models
{
    public class TaskDto
    {
        public HwExercise Exercise { get; set; }
        public string Answer { get; set; }
        public bool Passed { get; set; }
        public string Message { get; set; }
    }

    public class ResultBag
    {
        public SqlResult Custom { get; set; }
        public SqlResult Etalon { get; set; }
    }
}