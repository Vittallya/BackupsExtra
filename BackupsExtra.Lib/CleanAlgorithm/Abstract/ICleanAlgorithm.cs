using Backups.BackupSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackupsExtra.Lib.CleanAlgorithm
{
    //гибрид под все - общие точки, которые подоходят для всех алгоритмов
    //гибрид хотя бы под один - каждый алгоритм чистит столько точек, сколько ему нужно

    public interface ICleanAlgorithm
    {
        //те точки, которые нужно удалить по мнению алгоритма
        IEnumerable<RestorePoint> Clean(IEnumerable<RestorePoint> points);
    }
}
