namespace DtuFitnessApi.Models
{
   public class Event
{
    public int EventID { get; set; }
    public string ClubID { get; set; } 
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }

    public virtual ClubModel Club { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
}

}