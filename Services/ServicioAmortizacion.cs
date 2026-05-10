using EconomicApp.Models.DTOs;

namespace EconomicApp.Services;

public class ServicioAmortizacion : IAmortizacionService
{
    private const int DECIMALES_PRECISION = 6;

    public AmortizacionResponseDto GenerarTabla(AmortizacionRequestDto request)
    {
        var respuesta = new AmortizacionResponseDto();
        var tabla = new List<CuotaDto>();

        decimal saldoActual = request.Capital;

        if (request.PeriodosGracia > 0)
        {
            saldoActual = AplicarPeriodosGracia(tabla, saldoActual, request.TasaEfectivaPeriodica, request.PeriodosGracia);
        }

        if (request.SistemaAmortizacion == SistemaAmortizacion.Frances)
        {
            GenerarFrances(tabla, saldoActual, request.TasaEfectivaPeriodica, request.Plazo);
        }
        else
        {
            GenerarAleman(tabla, saldoActual, request.TasaEfectivaPeriodica, request.Plazo);
        }

        respuesta.TablaAmortizacion = tabla;
        respuesta.TotalInteresesPagados = Math.Round(tabla.Sum(c => c.Interes), DECIMALES_PRECISION);
        respuesta.TotalPagado = Math.Round(tabla.Sum(c => c.ValorCuota), DECIMALES_PRECISION);

        return respuesta;
    }

    private decimal AplicarPeriodosGracia(List<CuotaDto> tabla, decimal saldo, decimal tasa, int periodosGracia)
    {
        for (int i = 1; i <= periodosGracia; i++)
        {
            decimal interes = Math.Round(saldo * tasa, DECIMALES_PRECISION);
            decimal saldoFinal = Math.Round(saldo + interes, DECIMALES_PRECISION);

            tabla.Add(new CuotaDto
            {
                NumeroPeriodo = i,
                SaldoInicial = Math.Round(saldo, DECIMALES_PRECISION),
                Interes = interes,
                AbonoCapital = 0,
                ValorCuota = 0,
                SaldoFinal = saldoFinal
            });

            saldo = saldoFinal;
        }

        return saldo;
    }

    private void GenerarFrances(List<CuotaDto> tabla, decimal capital, decimal tasa, int plazo)
    {
        decimal factor = (decimal)Math.Pow(1 + (double)tasa, plazo);
        decimal cuota = Math.Round(capital * (tasa * factor) / (factor - 1), DECIMALES_PRECISION);

        decimal saldo = capital;

        for (int i = 1; i <= plazo; i++)
        {
            decimal saldoInicial = Math.Round(saldo, DECIMALES_PRECISION);
            decimal interes = Math.Round(saldo * tasa, DECIMALES_PRECISION);
            decimal abonoCapital = Math.Round(cuota - interes, DECIMALES_PRECISION);

            if (i == plazo)
            {
                abonoCapital = saldo;
                cuota = interes + abonoCapital;
            }

            saldo = Math.Round(saldo - abonoCapital, DECIMALES_PRECISION);

            tabla.Add(new CuotaDto
            {
                NumeroPeriodo = tabla.Count + 1,
                SaldoInicial = saldoInicial,
                Interes = interes,
                AbonoCapital = Math.Round(abonoCapital, DECIMALES_PRECISION),
                ValorCuota = Math.Round(cuota, DECIMALES_PRECISION),
                SaldoFinal = Math.Max(0, saldo)
            });
        }
    }

    private void GenerarAleman(List<CuotaDto> tabla, decimal capital, decimal tasa, int plazo)
    {
        decimal abonoFijo = Math.Round(capital / plazo, DECIMALES_PRECISION);
        decimal saldo = capital;

        for (int i = 1; i <= plazo; i++)
        {
            decimal saldoInicial = Math.Round(saldo, DECIMALES_PRECISION);
            decimal interes = Math.Round(saldo * tasa, DECIMALES_PRECISION);
            decimal cuota = Math.Round(abonoFijo + interes, DECIMALES_PRECISION);

            if (i == plazo)
            {
                abonoFijo = saldo;
                cuota = interes + abonoFijo;
            }

            saldo = Math.Round(saldo - abonoFijo, DECIMALES_PRECISION);

            tabla.Add(new CuotaDto
            {
                NumeroPeriodo = tabla.Count + 1,
                SaldoInicial = saldoInicial,
                Interes = interes,
                AbonoCapital = Math.Round(abonoFijo, DECIMALES_PRECISION),
                ValorCuota = Math.Round(cuota, DECIMALES_PRECISION),
                SaldoFinal = Math.Max(0, saldo)
            });
        }
    }
}