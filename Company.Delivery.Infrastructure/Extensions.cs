using Company.Delivery.Core;
using Company.Delivery.Domain.Dto;

namespace Company.Delivery.Infrastructure;

public static class Extensions
{

    public static WaybillDto ToDto(this Waybill waybill)
    {
        WaybillDto dto = new WaybillDto
        {
            Id = waybill.Id,
            Number = waybill.Number,
            Date = waybill.Date,
            Items = waybill.Items?.Select(ToDto).ToList()
        };

        return dto;
    }

    public static CargoItemDto ToDto(this CargoItem cargoItem)
    {
        CargoItemDto dto = new CargoItemDto()
        {
            Id = cargoItem.Id,
            WaybillId = cargoItem.WaybillId,
            Number = cargoItem.Number,
            Name = cargoItem.Name
        };

        return dto;
    }

}