using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDtstudy
{
    class TeacherModel
    {
        public string teacherId { get; set; }
        public string teaname { get; set; }
        public int teaage { get; set; }
        public int teasex { get; set; }
        public decimal teaheight { get; set; }
        public DateTime createtime { get; set; }
    }
}
