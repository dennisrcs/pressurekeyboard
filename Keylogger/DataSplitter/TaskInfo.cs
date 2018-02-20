using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAnalyzer;

namespace DataSplitter
{
    // stores information related to tasks performed, such as task order, and start and end timestamp
    public class TaskInfo
    {
        // members
        private string _session_id;
        public string SessionId {
            get { return _session_id; }
        }

        private string _participant_id;
        public string ParticipantId
        {
            get { return _participant_id; }
        }

        private List<int> _task_order;
        public List<int> TaskOrder {
            get { return _task_order; }
        }

        private List<DateTime> _task_start;
        public List<DateTime> TaskStart {
            get { return _task_start; }
        }

        private List<DateTime> _task_end;
        public List<DateTime> TaskEnd {
            get { return _task_end; }
        }

        // constructor
        public TaskInfo(int session_id, string task, string participant_id)
        {
            // number of elements in the task file should be equal to NUM_TASKS + NUM_TASKS * 2
            string[] elems = task.Split(',');
            if (elems.Length != Constants.NUM_TASKS + Constants.NUM_TASKS * 2)
                throw new ArgumentOutOfRangeException("The number of elements in the task file should be equal to NUM_TASKS * 3");

            // setting upvariables
            _task_order = new List<int>();
            _task_start = new List<DateTime>();
            _task_end = new List<DateTime>();
            _session_id = session_id + "";
            _participant_id = participant_id;

            // first NUM_TASKS elements store task order                
            for (int i = 0; i < Constants.NUM_TASKS; i++)
                _task_order.Add(Int32.Parse(elems[i]));

            // the remaining NUM_TASKS * 2 store the start and end timestamp for each task
            for (int i = Constants.NUM_TASKS; i < elems.Length; i = i + 2)
            {
                _task_start.Add(DateTime.Parse(elems[i]));
                _task_end.Add(DateTime.Parse(elems[i+1]));
            }
        }
    }
}
