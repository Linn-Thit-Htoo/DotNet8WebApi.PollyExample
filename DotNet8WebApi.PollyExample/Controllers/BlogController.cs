using DotNet8WebApi.PollyExample.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace DotNet8WebApi.PollyExample.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        var policy = Policy.Handle<Exception>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMicroseconds(1000 * attempt));

        var lst = await policy.ExecuteAsync(() => _context.Tbl_Blogs.AsNoTracking().OrderByDescending(x => x.BlogId).ToListAsync());
        return Ok(lst);
    }
}
