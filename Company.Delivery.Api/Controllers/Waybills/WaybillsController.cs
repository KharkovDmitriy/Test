using Company.Delivery.Api.Controllers.Waybills.Request;
using Company.Delivery.Api.Controllers.Waybills.Response;
using Company.Delivery.Domain;
using Company.Delivery.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Company.Delivery.Api.Controllers.Waybills;

/// <summary>
/// Waybills management
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WaybillsController : ControllerBase
{
    private readonly IWaybillService _waybillService;

    /// <summary>
    /// Waybills management
    /// </summary>
    public WaybillsController(IWaybillService waybillService) => _waybillService = waybillService;

    /// <summary>
    /// Получение Waybill
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WaybillResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // TODO: вернуть ответ с кодом 200 если найдено или кодом 404 если не найдено
        // TODO: WaybillsControllerTests должен выполняться без ошибок

        try
        {
            WaybillDto waybillDto = await _waybillService.GetByIdAsync(id, cancellationToken);

            WaybillResponse response = waybillDto.ToResponse();

            return new OkObjectResult(response);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }

    }

    /// <summary>
    /// Создание Waybill
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(WaybillResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([FromBody] WaybillCreateRequest request, CancellationToken cancellationToken)
    {
        // TODO: вернуть ответ с кодом 200 если успешно создано
        // TODO: WaybillsControllerTests должен выполняться без ошибок

        WaybillCreateDto waybillCreateDto = new WaybillCreateDto()
        {
            Number = request.Number,
            Date = request.Date,
            Items = request.Items?.Select(x => new CargoItemCreateDto() { Number = x.Number, Name = x.Name }).ToList()
        };

        WaybillDto waybillDto = await _waybillService.CreateAsync(waybillCreateDto, cancellationToken);

        WaybillResponse response = waybillDto.ToResponse();

        return new OkObjectResult(response);
    }

    /// <summary>
    /// Редактирование Waybill
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(WaybillResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateByIdAsync(Guid id, [FromBody] WaybillUpdateRequest request, CancellationToken cancellationToken)
    {
        // TODO: вернуть ответ с кодом 200 если найдено и изменено, или 404 если не найдено
        // TODO: WaybillsControllerTests должен выполняться без ошибок

        try
        {
            WaybillUpdateDto waybillUpdateDto = new WaybillUpdateDto()
            {
                Date = request.Date,
                Number = request.Number,
                Items = request.Items?.Select(x => new CargoItemUpdateDto() { Number = x.Number, Name = x.Name }).ToList()
            };

            WaybillDto waybillDto = await _waybillService.UpdateByIdAsync(id, waybillUpdateDto, cancellationToken);

            WaybillResponse response = waybillDto.ToResponse();

            return new OkObjectResult(response);

        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }

    }

    /// <summary>
    /// Удаление Waybill
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // TODO: вернуть ответ с кодом 200 если найдено и удалено, или 404 если не найдено
        // TODO: WaybillsControllerTests должен выполняться без ошибок

        try
        {
            await _waybillService.DeleteByIdAsync(id, cancellationToken);
            return new OkResult();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }

    }
}