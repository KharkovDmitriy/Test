using Company.Delivery.Api.Controllers.Waybills.Response;
using Company.Delivery.Domain.Dto;

namespace Company.Delivery.Api.Controllers.Waybills;

/// <summary>
/// Extensions
/// </summary>
public static class Extensions
{

    /// <summary>
    /// WaybillDtoToResponse
    /// </summary>
    /// <param name="waybillDto"></param>
    /// <returns>WaybillResponse</returns>
    public static WaybillResponse ToResponse(this WaybillDto waybillDto)
    {
        WaybillResponse response = new WaybillResponse()
        {
            Id = waybillDto.Id,
            Number = waybillDto.Number,
            Date = waybillDto.Date,
            Items = waybillDto.Items?.Select(ToResponse).ToList()
        };

        return response;
    }

    /// <summary>
    /// CargoItemDtoToResponse
    /// </summary>
    /// <param name="waybillDto"></param>
    /// <returns>WaybillResponse</returns>
    public static CargoItemResponse ToResponse(this CargoItemDto waybillDto)
    {
        CargoItemResponse response = new CargoItemResponse()
        {
            Id = waybillDto.Id,
            WaybillId = waybillDto.WaybillId,
            Number = waybillDto.Number,
            Name = waybillDto.Name
        };

        return response;
    }

}

