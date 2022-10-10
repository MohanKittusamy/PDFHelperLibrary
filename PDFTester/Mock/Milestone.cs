using System;

namespace PDFTester.Mock
{
    internal class Milestone
    {       
        internal string WorkItem;
        internal string Description;
        internal int Hrs;
        internal int Sno;
        internal Milestone
                 (
                 int Sno,
                 string WorkItem,
                 string Description,
                 int Hrs
                 )
        {
            this.Sno = Sno;
            this.WorkItem = WorkItem;
            this.Description = Description;
            this.Hrs = Hrs;
            return;
        }

        internal static Milestone[] TaskList = new Milestone[]
             {
            new Milestone(1, "Requirement analysis and documentation", "", 1),
            new Milestone(2, "C#.NET 6 Class Library", " ", 4),
            new Milestone(3, "Console application", "", 1),
            new Milestone(4, "Testing ", "", 2)
             };
    }
}
