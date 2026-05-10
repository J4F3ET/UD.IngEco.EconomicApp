using System.Text;
using EconomicApp.Models.DTOs;

namespace EconomicApp.Services;

public class ServicioReportes : IReporteService
{
    public byte[] GenerarReporteExcel(AmortizacionResponseDto amortizacion, SimulacionRequestDto parametros)
    {
        var sb = new StringBuilder();

        sb.AppendLine("SIMULADOR DE CRÉDITOS - ECONOMICAPP");
        sb.AppendLine("==================================");
        sb.AppendLine();
        sb.AppendLine("PARÁMETROS DEL CRÉDITO");
        sb.AppendLine($"Monto del Préstamo:,{parametros.MontoPrestamo:C}");
        sb.AppendLine($"Plazo:,{parametros.Plazo} {parametros.FrecuenciaPago}");
        sb.AppendLine($"Valor Tasa:,{parametros.ValorTasa}%");
        sb.AppendLine($"Tipo Tasa:,{parametros.TipoTasa}");
        sb.AppendLine($"Frecuencia Tasa:,{parametros.FrecuenciaTasa}");
        sb.AppendLine();

        sb.AppendLine("RESUMEN FINANCIERO");
        sb.AppendLine($"Total Intereses:,{amortizacion.TotalInteresesPagados:C}");
        sb.AppendLine($"Total Pagado:,{amortizacion.TotalPagado:C}");
        sb.AppendLine();

        sb.AppendLine("TABLA DE AMORTIZACIÓN");
        sb.AppendLine("Periodo,Saldo Inicial,Interés,Abono Capital,Cuota,Saldo Final");
        foreach (var cuota in amortizacion.TablaAmortizacion)
        {
            sb.AppendLine($"{cuota.NumeroPeriodo},{cuota.SaldoInicial:F6},{cuota.Interes:F6},{cuota.AbonoCapital:F6},{cuota.ValorCuota:F6},{cuota.SaldoFinal:F6}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public byte[] GenerarReportePdf(AmortizacionResponseDto amortizacion, SimulacionRequestDto parametros)
    {
        var sb = new StringBuilder();

        sb.AppendLine("=== SIMULADOR DE CRÉDITOS ===");
        sb.AppendLine("EconomicApp - Ingeniería Económica");
        sb.AppendLine();
        sb.AppendLine("--- PARÁMETROS DEL CRÉDITO ---");
        sb.AppendLine($"Monto: {parametros.MontoPrestamo:C}");
        sb.AppendLine($"Plazo: {parametros.Plazo} {parametros.FrecuenciaPago}");
        sb.AppendLine($"Tasa: {parametros.ValorTasa}% {parametros.TipoTasa}");
        sb.AppendLine();
        sb.AppendLine("--- RESUMEN ---");
        sb.AppendLine($"Total Intereses: {amortizacion.TotalInteresesPagados:C}");
        sb.AppendLine($"Total Pagado: {amortizacion.TotalPagado:C}");
        sb.AppendLine();
        sb.AppendLine("--- TABLA DE AMORTIZACIÓN ---");
        sb.AppendLine("Per|Saldo Inicial|Interés|Abono|Cuota|Saldo Final");
        sb.AppendLine(new string('-', 50));

        foreach (var cuota in amortizacion.TablaAmortizacion)
        {
            sb.AppendLine($"{cuota.NumeroPeriodo,2}|{cuota.SaldoInicial,12:F2}|{cuota.Interes,10:F2}|{cuota.AbonoCapital,10:F2}|{cuota.ValorCuota,10:F2}|{cuota.SaldoFinal,12:F2}");
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}