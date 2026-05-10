using EconomicApp.Models.DTOs;

namespace EconomicApp.Services;

public interface IAmortizacionService
{
    AmortizacionResponseDto GenerarTabla(AmortizacionRequestDto request);
}