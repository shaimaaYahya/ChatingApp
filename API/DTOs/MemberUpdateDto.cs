using System;

namespace API.DTOs;

public class MemberUpdateDto
{
    public String? Introduction { get; set; }
    public String? LookingFor { get; set; }
    public String? Interests { get; set; }
    public String? City { get; set; }
    public String? Country { get; set; }
}
