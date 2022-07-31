namespace WebapiCorePractice.Models
{
    public class Issue
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? IsComplete { get; set; }
        public Priority Priority { get; set; }
        public IssueType IssueType { get; set; }
         
    }

    public enum Priority
    {
        Low, Medium, High 
    }

    public enum IssueType
    {
        Feature, Bug, Documentation
    }
}
