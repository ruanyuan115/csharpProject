using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetProject.Entity
{
    public class SemesterAndYear
    {
        public String semester;
        public int? year;
        public SemesterAndYear(String semester, int? year)
        {
            this.semester = semester;
            this.year = year;
        }
       
        public override int GetHashCode()
        {
            return (semester + year).GetHashCode();
        }
       
        public Boolean equals(Object obj)
        {
            if (obj.GetType()==typeof(SemesterAndYear)) {
                SemesterAndYear sem = (SemesterAndYear)obj;
                return this.semester==(sem.semester) && this.year==(sem.year);
            }
            return false;
        }
    }
}
