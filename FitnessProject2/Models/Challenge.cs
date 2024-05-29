public class Challenge{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Category { get; set; }
    public string? Instructions { get; set; }
    public string? Difficulty { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public bool? IsDeleted { get; set; }
    public List<Participation>? ParticipationsList { get; set; }

     public ICollection<Participation>? Participations { get; set; }
     public ICollection<Leaderboard>? Leaderboards { get; set; }
     public ICollection<UserRate>? UserRates { get; set; }

}