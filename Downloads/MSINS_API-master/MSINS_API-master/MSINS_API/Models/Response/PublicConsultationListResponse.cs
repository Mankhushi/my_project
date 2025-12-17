namespace MSINS_API.Models.Response
{
    public class PublicConsultationListResponse
    {
        public string PublicConsultationId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string? CityDistrict { get; set; }

        public string? OrganizationName { get; set; }

        public string? ExpertiseSector { get; set; }

        public string? ExpertiseSectorOther { get; set; }

        public string? AspectPolicy { get; set; }

        public string? Suggestion { get; set; }

        public string? Rating { get; set; }

        public string? RecommendateProgram { get; set; }

        public string? File1 { get; set; }

        public string? File2 { get; set; }

        public string? File3 { get; set; }

        public string? File4 { get; set; }

        public string? File5 { get; set; }

        public DateTime? entryDateTime { get; set; }
    }
}
