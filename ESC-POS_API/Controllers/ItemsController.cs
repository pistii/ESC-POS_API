using ESC_POS_API;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ItemsController : ControllerBase
    {
        /*
        [
            {
                "ItemName": "Kenyér",
                "Price": 1200,
                "Quantity": 2
            },
            {
                "ItemName": "Vaj",
                "Price": 300,
                "Quantity": 1
            }
        ]
        */
        [HttpPost]
        public async Task<IActionResult> Post(List<Items> items)
        {
            try
            {
                PrinterSetup printer = new PrinterSetup();
                await printer.CreateNew(items);
                return Ok();
            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}