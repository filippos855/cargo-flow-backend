namespace cargo_flow_backend.Models.Requests
{
    public class UserUpdateRequest : UserCreateRequest
    {
        public int Id { get; set; }
    }
}