using EconomicApp.Models.DTOs;

namespace EconomicApp.Services;

public class ServicioConversorTasas : ITasaService
{
    private const int DECIMALES_PRECISION = 6;

    public ConversionTasaResponseDto CalcularTasaPeriodica(SimulacionRequestDto request)
    {
        var respuesta = new ConversionTasaResponseDto { EsExitoso = false };

        try
        {
            if (request.MontoPrestamo <= 0 || request.Plazo <= 0 || request.ValorTasa <= 0)
            {
                respuesta.Mensaje = "Los valores de monto, plazo y tasa deben ser mayores a cero.";
                return respuesta;
            }

            decimal tasaConvertida;

            if (request.TipoTasa == TipoTasa.Nominal)
            {
                tasaConvertida = ConvertirNominalAEfectiva(request.ValorTasa, request.FrecuenciaTasa);
            }
            else
            {
                tasaConvertida = request.ValorTasa / 100;
            }

            decimal tasaPeriodica = EquivalenciaTasas(tasaConvertida, request.FrecuenciaTasa, request.FrecuenciaPago);

            respuesta.TasaEfectivaPeriodica = Math.Round(tasaPeriodica, DECIMALES_PRECISION);
            respuesta.EsExitoso = true;
            respuesta.Mensaje = $"Tasa convertida exitosamente a tasa efectiva periódica ({request.FrecuenciaPago}).";
        }
        catch (Exception ex)
        {
            respuesta.Mensaje = $"Error en el cálculo: {ex.Message}";
        }

        return respuesta;
    }

    private decimal ConvertirNominalAEfectiva(decimal tasaNominal, Frecuencia frecuenciaTasa)
    {
        int m = (int)frecuenciaTasa;
        decimal tasaNominalDecimal = tasaNominal / 100;
        return tasaNominalDecimal / m;
    }

    private decimal EquivalenciaTasas(decimal tasaConocida, Frecuencia frecuenciaOrigen, Frecuencia frecuenciaDestino)
    {
        if (frecuenciaOrigen == frecuenciaDestino)
            return tasaConocida;

        decimal n1 = (int)frecuenciaOrigen;
        decimal n2 = (int)frecuenciaDestino;

        decimal factor = n2 / n1;
        decimal tasaEquivalente = (decimal)Math.Pow((double)(1 + tasaConocida), (double)factor) - 1;

        return Math.Round(tasaEquivalente, DECIMALES_PRECISION);
    }
}