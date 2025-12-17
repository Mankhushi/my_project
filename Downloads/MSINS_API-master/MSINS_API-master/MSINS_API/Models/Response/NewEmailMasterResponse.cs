namespace MSINS_API.Models.Response
{
    public class NewEmailMasterResponse
    {
        public int Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public int adminId { get; set; }
    }
}
