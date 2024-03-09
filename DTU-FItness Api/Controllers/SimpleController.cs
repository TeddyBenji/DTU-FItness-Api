using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SimpleController : ControllerBase
{
    private readonly SimpleService _simpleService;

    public SimpleController(SimpleService simpleService)
    {
        _simpleService = simpleService;
    }

    [HttpPost("Simple")]
    public async Task<IActionResult> Post([FromBody] SimpleModel model)
    {
        await _simpleService.AddIntegerAsync(model);
        return Ok();
    }

    [HttpGet("test")]
    public ActionResult Test()
    {
    return Ok("API is working!");
    }



}
