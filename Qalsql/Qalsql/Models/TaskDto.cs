﻿using Qalsql.Models.Db;

namespace Qalsql.Models
{
    public class TaskDto
    {
        public HwExercise Exercise { get; set; }
        public bool IsOk { get; set; }
        public string Answer { get; set; }
    }
}