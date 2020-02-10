using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace LogMe.Models
{
    public interface IEntry
    {
        [PrimaryKey, AutoIncrement]
        int ID { get; set; }
    }

    public class Entry : IEntry // Entry Base
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
    }


    public class LogEntry : Entry // Logs
    {

        public string Notes { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public string StartDateFormat => StartDate.ToString("g");
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int Severity { get; set; } = 0;

        [ManyToMany(typeof(LogEntryAffectedArea), CascadeOperations = CascadeOperation.All)]
        public List<AffectedArea> AffectedAreas { get; set; } = new List<AffectedArea>();

        [ManyToMany(typeof(LogEntryTrigger), CascadeOperations = CascadeOperation.All)]
        public List<FlareTrigger> FlareTriggers { get; set; } = new List<FlareTrigger>();

        //public bool Equals(LogEntry other)
        //{
        //    if (other == null) return false;
        //    return ID == other.ID;
        //    //return string.Equals(Notes, other.Notes) &&
        //    //    StartDate.Equals(other.StartDate) &&
        //    //    EndDate.Equals(other.EndDate) &&
        //    //    Severity == other.Severity &&
        //    //    AffectedAreas.Equals(other.AffectedAreas) &&
        //    //    FlareTriggers.Equals(other.FlareTriggers);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is null) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != GetType()) return false;
        //    return Equals(obj as LogEntry);
        //}
    }

    // Tags

    public class TagEntry : Entry // Tag base
    {
        [Unique]
        public string Name { get; set; }
    }


    public class FlareTrigger : TagEntry // Triggers
    {
        [ManyToMany(typeof(LogEntryTrigger), CascadeOperations = CascadeOperation.All, ReadOnly = true)]
        public List<LogEntry> LogEntries { get; set; } = new List<LogEntry>();

        //public bool Equals(FlareTrigger other)
        //{
        //    if (other == null) return false;
        //    return ID == other.ID;
        //    //return string.Equals(Name, other.Name) &&
        //    //LogEntries.Equals(other.LogEntries);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is null) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != GetType()) return false;
        //    return Equals(obj as FlareTrigger);
        //}
    }


    public class AffectedArea : TagEntry // Areas
    {

        [ManyToMany(typeof(LogEntryAffectedArea), CascadeOperations = CascadeOperation.All, ReadOnly = true)]
        public List<LogEntry> LogEntries { get; set; } = new List<LogEntry>();

        //public bool Equals(AffectedArea other)
        //{
        //    if (other == null) return false;
        //    return ID == other.ID;
        //    //return string.Equals(Name, other.Name) &&
        //    //    LogEntries.Equals(other.LogEntries);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj is null) return false;
        //    if (ReferenceEquals(this, obj)) return true;
        //    if (obj.GetType() != GetType()) return false;
        //    return Equals(obj as AffectedArea);
        //}
    }

    // Relationship Databases

    public class LogEntryTagEntry // Log <-> Tag base
    {
        [ForeignKey(typeof(LogEntry))]
        public int LogEntryId { get; set; }
    }

    public class LogEntryTrigger : LogEntryTagEntry // Log <-> Trigger
    {
        [ForeignKey(typeof(FlareTrigger))]
        public int TriggerId { get; set; }
    }

    public class LogEntryAffectedArea : LogEntryTagEntry // Log <-> Area
    {
        [ForeignKey(typeof(AffectedArea))]
        public int AffectedAreaId { get; set; }
    }
}
