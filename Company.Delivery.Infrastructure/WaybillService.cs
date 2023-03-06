using Company.Delivery.Core;
using Company.Delivery.Database;
using Company.Delivery.Domain;
using Company.Delivery.Domain.Dto;
using Microsoft.EntityFrameworkCore;

namespace Company.Delivery.Infrastructure;

public class WaybillService : IWaybillService
{

    private readonly DeliveryDbContext _dbContext;

    public WaybillService(DeliveryDbContext context)
    {
        _dbContext = context;
    }

    public async Task<WaybillDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // TODO: Если сущность не найдена по идентификатору, кинуть исключение типа EntityNotFoundException

        Waybill? waybill = await _dbContext.Waybills.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (waybill == null)
        {
            throw new EntityNotFoundException($"id: {id}");
        }

        return waybill.ToDto();
    }

    public async Task<WaybillDto> CreateAsync(WaybillCreateDto data, CancellationToken cancellationToken)
    {
        Waybill waybill = new()
        {
            Number = data.Number,
            Date = data.Date,
            Items = data.Items?.Select(x => new CargoItem() { Name = x.Name, Number = x.Number }).ToList()
        };

        await _dbContext.Waybills.AddAsync(waybill, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        WaybillDto dto = waybill.ToDto();

        return dto;
    }

    public async Task<WaybillDto> UpdateByIdAsync(Guid id, WaybillUpdateDto data, CancellationToken cancellationToken)
    {
        // TODO: Если сущность не найдена по идентификатору, кинуть исключение типа EntityNotFoundException

        Waybill? waybill = await _dbContext.Waybills.Include(x => x.Items).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (waybill == null)
        {
            throw new EntityNotFoundException($"id: {id}");
        }

        waybill.Number = data.Number;
        waybill.Date = data.Date;

        if (data.Items != null && data.Items.Any())
        {
            List<string> actualNumbers = data.Items.Select(x => x.Number).ToList();

            if (waybill.Items != null)
            {
                CargoItem[] toDelete = waybill.Items.Where(x => !actualNumbers.Contains(x.Number)).ToArray();
                _dbContext.CargoItems.RemoveRange(toDelete);
            }

            List<CargoItem> newItems = new List<CargoItem>();
            List<CargoItem> cargoItems = waybill.Items?.ToList() ?? new List<CargoItem>();

            foreach (CargoItemUpdateDto item in data.Items)
            {
                CargoItem? cargoItem = cargoItems.FirstOrDefault(x => x.Number == item.Number);

                if (cargoItem == null)
                {
                    cargoItem = new CargoItem();
                    cargoItem.Waybill = waybill;
                    cargoItem.Number = item.Number;
                    newItems.Add(cargoItem);
                }
                else
                {
                    cargoItems.Remove(cargoItem);
                }

                cargoItem.Name = item.Name;
            }

            await _dbContext.CargoItems.AddRangeAsync(newItems, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return waybill.ToDto();
    }

    public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        // TODO: Если сущность не найдена по идентификатору, кинуть исключение типа EntityNotFoundException

        Waybill? waybill = await _dbContext.Waybills.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (waybill == null)
        {
            throw new EntityNotFoundException($"id: {id}");
        }

        _dbContext.Waybills.Remove(waybill);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

}