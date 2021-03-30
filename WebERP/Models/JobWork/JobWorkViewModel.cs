using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class JobWorkViewModel
    {
        public JobWorkIssueHdr JobWorkIssHeader { get; set; }
        public List<JobWorkIssueDet> JobWorkIssueDetails { get; set; }
    }
}
