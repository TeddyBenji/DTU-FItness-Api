namespace DTU_FItness_Api.Models.ClubModels
{
    
    public class ClubDto
    {
        public string ClubID { get; set; }
        public string ClubName { get; set; }
        public string Description { get; set; }
        public string OwnerUserId { get; set; }
        public DateTime CreationDate { get; set; }
    }

}
